using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Prism.Logging;
using System.Xml;
using System.Reflection;
using log4net.Config;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.FileReplicator.Utility;
using Emdep.Geos.Data.Common.FileReplicator;
using System.Xml.Linq;
using Emdep.Geos.Services.Contracts;
using System.ServiceModel;

namespace FileReplicator.WindowsService
{
    public partial class FileReplicatorService : ServiceBase, IDisposable
    {
        #region Service
        // WorkbenchServiceController ServiceController = new WorkbenchServiceController(Properties.Settings.Default.mainWebServerHostingIp.ToString());
        WorkbenchServiceController ServiceController;
        #endregion

        #region Declaration

        private System.Timers.Timer _timer;
        static readonly object _object = new object();
        XmlFileGenerator GenerateXmlFile;
        bool isBusyFileReplicator = false;
        bool isBusyDownloadFileReplicator = false;
        AsyncDownloadingService _downloadService;
        Emdep.Geos.FileReplicator.Utility.FileSystemWatcher fileWatcher;
        bool isTimerStop = false;

        #endregion

        public FileReplicatorService()
        {
            InitializeComponent();
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in  file replicator service() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }

        }
        protected override void OnStart(string[] args)
        {
            try
            {
                GenerateXmlFile = new XmlFileGenerator();

                fileWatcher = new Emdep.Geos.FileReplicator.Utility.FileSystemWatcher();

                _timer = new System.Timers.Timer(GenerateXmlFile.intervalTimeInMinutes * 60 * 1000);    //  minutes expressed as milliseconds
                _timer.Elapsed += new ElapsedEventHandler(timerScheduler_Tick);
                _timer.AutoReset = true;
                _timer.Start();
                GetInitialSetting();
                XmlDownloadingFolderExistOrNot();
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        protected override void OnStop()
        {
            Log4NetLogger.Logger.Log(string.Format("Service status - {0}", "Service stop" + "  " + DateTime.Now), category: Category.Info, priority: Priority.Low);
            _timer.Stop();
            _timer.Dispose();
        }
        public void StartService()
        {
            try
            {
                GenerateXmlFile = new XmlFileGenerator();
                fileWatcher = new Emdep.Geos.FileReplicator.Utility.FileSystemWatcher();
                _timer = new System.Timers.Timer(GenerateXmlFile.intervalTimeInMinutes * 60 * 1000);    //  minutes expressed as milliseconds
                _timer.Elapsed += new ElapsedEventHandler(timerScheduler_Tick);
                _timer.AutoReset = true;
                _timer.Start();
                GetInitialSetting();
                XmlDownloadingFolderExistOrNot();

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region METHOD
        private void timerScheduler_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                _timer.AutoReset = false;
                if (GenerateXmlFile.IsTimeBetween())
                {
                    if (!isBusyDownloadFileReplicator)
                    {
                        isBusyDownloadFileReplicator = true;
                        _downloadService = new AsyncDownloadingService();
                        Log4NetLogger.Logger.Log(string.Format("-------------------------------------------  Downloading service start -------------------------------------------", "ReadXmlFileLocalServer start"), category: Category.Info, priority: Priority.Low);
                        fileWatcher.StopFileWatcher();
                        _downloadService.AsyncStartDownloading();
                        isBusyDownloadFileReplicator = false;
                        DateTime currentTime = DateTime.Now;
                        if (GenerateXmlFile.downloadStopTimes >= currentTime)
                        {
                            TimeSpan sleepingTime = GenerateXmlFile.downloadStopTimes - currentTime;
                            Thread.Sleep(1000 * 60 * Convert.ToInt32(sleepingTime.Minutes));
                        }
                    }
                }
                else
                {
                    fileWatcher.StartFileWatcher();

                    ReadXmlFileLocalServer();
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in timerScheduler_Tick . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.High);
            }
            finally
            {
                _timer.AutoReset = true;
                _timer.Start();
            }
        }
        /// <summary>
        /// This Method User For get file data into byte format
        /// </summary>
        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }

        }

        /// <summary>
        /// Upload file to main server 
        /// This method User for file Upload  to main server using wcf services
        /// </summary>
        /// <param name="localServerfilePath"></param>
        /// <param name="mainServerPath"></param>
        /// <returns></returns>
        private bool FileUploadOnMainServer(string localServerfilePath, string mainServerPath)
        {
            lock (_object)
            {
                ServiceController = new WorkbenchServiceController(GenerateXmlFile.mainWebServerHostingIp);
                FileDetail fileDetail = new FileDetail();
                FileInfo FileInfo = new FileInfo(localServerfilePath);
                fileDetail.FileName = Path.Combine(FileInfo.Name);
                fileDetail.FilePath = mainServerPath;
                byte[] bytes = GetBytesFromFile(localServerfilePath);
                fileDetail.FileByte = bytes;
                FileReturnMessage returnMsgFileUploading = ServiceController.FileUpload(fileDetail);
                return returnMsgFileUploading.IsFileActionPerformed;
            }
        }

        /// <summary>
        /// Read Xml File from LocalServer
        /// Perform opration according to xml file
        /// [001][skale][2019-28-05][GEOS2-1490] File replicator - Employee image is not copied
        /// </summary>
        private void ReadXmlFileLocalServer()
        {
            try
            {
                if (!isBusyFileReplicator)
                {
                    ServiceController = new WorkbenchServiceController(GenerateXmlFile.mainWebServerHostingIp);
                    isBusyFileReplicator = true;

                    List<FileReplicatorData> xmlDetailslist = new List<FileReplicatorData>();
                    bool checkFiletype, returnmsg;
                    FileDetail fileDetail = null;
                    xmlDetailslist = GenerateXmlFile.ReadXmlFileDetails();

                    fileWatcher.FileDetailsList.Clear();//[01] added

                    if (xmlDetailslist.Count > 0)
                    {
                        Log4NetLogger.Logger.Log(string.Format("ReadXmlFileLocalServer().Message- ReadXmlFileLocalServer Started file uploading count is - {0}", xmlDetailslist.Count), category: Category.Info, priority: Priority.Low);
                    }
                    int successCount = 0;

                    foreach (var xmlFileDetails in xmlDetailslist)
                    {
                        try
                        {
                            if (xmlFileDetails.fileOperation.ToString() == "Created")
                            {
                                checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                if (checkFiletype)
                                {
                                    fileDetail = new FileDetail();
                                    var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                    fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    FileReturnMessage ReturnMessageDirectory = ServiceController.CheckDirectoryExistOrNot(fileDetail);

                                    if (!ReturnMessageDirectory.IsFileActionPerformed)
                                    {
                                        FileReturnMessage ReturnMessageCreateDirectory = ServiceController.CreateDirectory(fileDetail);
                                        if (ReturnMessageCreateDirectory.IsFileActionPerformed)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("New folder created on local - {0}. New folder created on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                        }
                                    }
                                    DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                    successCount++;
                                }
                                else
                                {
                                    fileDetail = new FileDetail();
                                    var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                    fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot(fileDetail);

                                    if (!ReturnMessage.IsFileActionPerformed)
                                    {
                                        //[001] added if condition
                                        if (File.Exists(xmlFileDetails.filePath.ToString()))
                                        {
                                            returnmsg = FileUploadOnMainServer(xmlFileDetails.filePath.ToString(), fileDetail.FilePath);
                                            if (returnmsg)
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("New file created on local - {0}. New file uploaded on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                                //DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                                //successCount++;
                                            }
                                            else
                                                Log4NetLogger.Logger.Log(string.Format("Unsuccessful file uploads {0}", xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                        }
                                        else
                                            Log4NetLogger.Logger.Log(string.Format("Could not find file - {0}.", xmlFileDetails.filePath), category: Category.Info, priority: Priority.Low);

                                        DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                    }
                                    else
                                    {
                                        if (ReplaceExistsFile(xmlFileDetails.filePath.ToString(), fileDetail.FilePath, xmlFileDetails.xmlFilePath.ToString()))
                                            successCount++;
                                        // Log4NetLogger.Logger.Log(string.Format("File already exist {0}.", xmlFileDetails.filePath), category: Category.Info, priority: Priority.Low);
                                    }
                                }
                            }
                            else if (xmlFileDetails.fileOperation.ToString() == "Renamed")
                            {
                                checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                if (checkFiletype)
                                {
                                    fileDetail = new FileDetail();
                                    var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                    fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    fileDetail.fileOldName = xmlFileDetails.oldFilePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    FileReturnMessage ReturnMessage = ServiceController.CheckDirectoryExistOrNot(fileDetail);

                                    if (!ReturnMessage.IsFileActionPerformed)
                                    {
                                        FileReturnMessage ReturnMessageRenamefolder = ServiceController.RenameFolder(fileDetail);
                                        if (ReturnMessageRenamefolder.IsFileActionPerformed)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("Folder rename on local - {0}. Folder rename on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                            DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                        }
                                        else
                                            Log4NetLogger.Logger.Log(string.Format("Folder rename unsuccessful- {0}", xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                    }
                                    else
                                    {
                                        DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                    }
                                }
                                else
                                {
                                    fileDetail = new FileDetail();
                                    var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                    fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    fileDetail.fileOldName = xmlFileDetails.oldFilePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot(fileDetail);

                                    if (!ReturnMessage.IsFileActionPerformed)
                                    {
                                        FileReturnMessage ReturnMessageRenamefile = ServiceController.RenameFile(fileDetail);
                                        if (ReturnMessageRenamefile.IsFileActionPerformed)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("File rename on local - {0}. File rename on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                            DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                        }
                                        else
                                            Log4NetLogger.Logger.Log(string.Format("File rename unsuccessful- {0}", xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                    }
                                    else
                                    {
                                        DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                    }
                                }
                            }
                            else if (xmlFileDetails.fileOperation.ToString() == "Deleted")
                            {
                                checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                if (checkFiletype)
                                {
                                    fileDetail = new FileDetail();
                                    var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                    fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    FileReturnMessage ReturnMessage = ServiceController.CheckDirectoryExistOrNot(fileDetail);

                                    if (ReturnMessage.IsFileActionPerformed)
                                    {
                                        FileReturnMessage ReturnMessageForDelete = ServiceController.DeleteFolder(fileDetail);

                                        if (ReturnMessageForDelete.IsFileActionPerformed)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("Folder deleted successfully from local server - {0}. Folder deleted successfully from main server - {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                            DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                        }
                                        else
                                            Log4NetLogger.Logger.Log(string.Format("Folder delete unsuccessful- {0}", xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                    }
                                    else  //  Added by Gopal Mahale   to delete extra file generated
                                    {
                                        DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                    }
                                }
                                else
                                {
                                    fileDetail = new FileDetail();
                                    var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                    fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                    FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot(fileDetail);

                                    if (ReturnMessage.IsFileActionPerformed)
                                    {
                                        FileReturnMessage ReturnMessageDelete = ServiceController.DeleteFile(fileDetail);
                                        if (ReturnMessageDelete.IsFileActionPerformed)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("File deleted successfully from local server - {0}. File deleted successfully from main server - {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                            DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                        }
                                        else
                                            Log4NetLogger.Logger.Log(string.Format("File delete unsuccessful- {0}", xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                    }
                                    else  //Added by Gopal Mahale to delete extra file generated
                                    {
                                        DeleteLocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                    }
                                }
                            }
                            //[001] Added
                            else if (xmlFileDetails.fileOperation.ToString() == "Changed")
                            {
                                fileDetail = new FileDetail();
                                var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);

                                if (ReplaceExistsFile(xmlFileDetails.filePath.ToString(), fileDetail.FilePath, xmlFileDetails.xmlFilePath.ToString()))
                                    successCount++;
                            }
                            //end
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error in ReadXmlFileLocalServer(). ErrorMessage- {0} - {1}", ex.ToString(), xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error in ReadXmlFileLocalServer(). ErrorMessage- {0} - {1}", ex.ToString(), xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error in ReadXmlFileLocalServer(). ErrorMessage- {0} - {1}", ex.ToString(), xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    if (xmlDetailslist.Count > 0 && successCount > 0)
                    {
                        Log4NetLogger.Logger.Log(string.Format("ReadXmlFileLocalServer() - Read xml file successfully with file count - {0}", successCount), category: Category.Info, priority: Priority.Low);
                    }

                    isBusyFileReplicator = false;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in ReadXmlFileLocalServer(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Delete local Server XmlFile
        /// </summary>
        /// <param name="filePath"></param>
        public void DeleteLocalServerXmlFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in DeletelocalServerXmlFile(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for get initial setting
        /// </summary>
        public void GetInitialSetting()
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("Service status - {0}", "Service start" + "  " + DateTime.Now), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Timer Interval Time - {0} Minute", GenerateXmlFile.intervalTimeInMinutes), category: Category.Info, priority: Priority.Low);

                foreach (KeyValuePair<string, string> filePath in GenerateXmlFile.DiFolderToWatch)
                {
                    Log4NetLogger.Logger.Log(string.Format("Monitoring local folder - {0} and uploading to main server - {1}", filePath.Key, filePath.Value), category: Category.Info, priority: Priority.Low);
                }

                Log4NetLogger.Logger.Log(string.Format("Xml file downloading on local server - {0}", GenerateXmlFile.downloadingXmlFilePath), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Main web server hosting ip - {0}", GenerateXmlFile.mainWebServerHostingIp), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Download start and stop time  - {0}", GenerateXmlFile.downloadStartStopTime), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in GetInitialSetting(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///Check Xml downloading  Folder Eixts or Not (Check Local server)
        /// </summary>
        public void XmlDownloadingFolderExistOrNot()
        {
            try
            {
                if (GenerateXmlFile.downloadingXmlFilePath != string.Empty)
                {
                    if (!Directory.Exists(GenerateXmlFile.downloadingXmlFilePath))
                    {
                        Directory.CreateDirectory(GenerateXmlFile.downloadingXmlFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in XmlDownloadingFolderExistOrNot() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// this method  use  for Replace Already Exist File from  local server to main server
        /// </summary>
        /// <param name="localServerPath"></param>
        /// <param name="mainServerPath"></param>
        ///  <param name="xmlfilepath"></param>
        public bool ReplaceExistsFile(string localServerPath, string mainServerPath, string xmlfilepath)
        {
            var sourceFile = new FileInfo(localServerPath);
            var mainServerfile = new FileInfo(mainServerPath);
            bool isReplace = false;

            if (sourceFile.LastWriteTime > mainServerfile.LastWriteTime)
            {
                try
                {
                    if (sourceFile.Name.ToLower() != "thumbs.db")
                    {
                        AsyncDownloadingService objAsyncDownloadingService = new AsyncDownloadingService();
                        //check file is busy in another process or not
                        if (objAsyncDownloadingService.FileIsBusy(sourceFile.FullName))
                        {
                            Log4NetLogger.Logger.Log(string.Format("The process cannot access the file - {0}  because it is being used by another process", sourceFile.FullName), category: Category.Warn, priority: Priority.Low);
                            DeleteLocalServerXmlFile(xmlfilepath);
                            isReplace = true;
                        }
                        else
                        {
                            sourceFile.CopyTo(mainServerPath, true); // Replace exists file
                            Log4NetLogger.Logger.Log(string.Format("File replace successfully from  - {0} to  {1} ", localServerPath, mainServerPath), category: Category.Info, priority: Priority.Low);
                            DeleteLocalServerXmlFile(xmlfilepath);
                            isReplace = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in ReplaceAlreadyExistFile(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                }
            }
            else
            {
                DeleteLocalServerXmlFile(xmlfilepath);
                isReplace = true;
            }
            return isReplace;
        }
        #endregion
    }
}



