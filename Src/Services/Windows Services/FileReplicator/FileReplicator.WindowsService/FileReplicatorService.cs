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

        private System.Timers.Timer _timer;
        static readonly object _object = new object();
        XmlFileGenerator GenerateXmlFile;
        bool isBusyFileReplicator = false;
        AsyncDownloadingService _dwonloadService;
        Emdep.Geos.FileReplicator.Utility.FileSystemWatcher fileWatcher;
        List<string> directoryList = new List<string>();
        List<string> getAllWorkingOrdersFiles = new List<string>();
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
/*
                GenerateXmlFile = new XmlFileGenerator();
                fileWatcher = new Emdep.Geos.FileReplicator.Utility.FileSystemWatcher();
                //UploadWorkingOrders();
                UploadWorkingOrdersDataInBackground(); // Call this instead of UploadWorkingOrders();
                Thread.Sleep(20000);
                Thread.Sleep(90000);
*/
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  FileReplicatorService() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
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
                GetIniSetting();
                XmldownloadindFolderExistOrNot();
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        protected override void OnStop()
        {
            Log4NetLogger.Logger.Log(string.Format("Service Status Message- {0}", "Service Stop" + "  " + DateTime.Now), category: Category.Info, priority: Priority.Low);
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
                GetIniSetting();
                XmldownloadindFolderExistOrNot();

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region METHOD
        private void timerScheduler_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                #region IsUpload
                //if (GenerateXmlFile.IsUploadFQTimeBetween())
                //{
                //    try
                //    {
                //        fileWatcher.StopFileWatcher();
                //        //Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
                //        UploadWorkingOrdersDataInBackground();
                //    }
                //    catch (Exception ex)
                //    {
                //        Log4NetLogger.Logger.Log(string.Format("ERROR in ReadXmlFileLocalServer(). UploadWorkingOrdersDataInBackground....  ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                //    }
                //}
                #endregion

                if (GenerateXmlFile.IsUploadFQTimeBetween())
                {
                    try
                    {
                        //Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
                        UploadWorkingOrdersDataInBackground();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("ERROR in ReadXmlFileLocalServer(). UploadWorkingOrdersDataInBackground....  ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                    }
                }

                if (GenerateXmlFile.IsTimeBetween())
                {
                    //Load Files from Main server to Local server
                    _dwonloadService = new AsyncDownloadingService();
                    Log4NetLogger.Logger.Log(string.Format("-------------------------------------------  Dwonloading Service Start  -------------------------------------------", "ReadXmlFileLocalServer Start"), category: Category.Info, priority: Priority.Low);
                    fileWatcher.StopFileWatcher();
                    _dwonloadService.AsyncStartDownloading();
                }
                else
                {
                    //Take Files from Local server to Main server
                    fileWatcher.StartFileWatcher();
                    ReadXmlFileLocalServer();
                    //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                    try
                    {
                        //if (directoryList == null && directoryList.Count() == 0)
                        //{
                        //    // Assuming you want to populate directoryList with directories from the local path
                        //    directoryList = Directory.GetDirectories(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath).ToList();
                        //}
                        //UploadWorkingOrdersToMainServer();
                        //UploadWorkingOrdersToMainServer_V2590();
                        //asyncUploadWorkingOrdersToMainServer_V2590();

                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("ERROR in ReadXmlFileLocalServer(). UploadWorkingOrdersToMainServer....  ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in timerScheduler_Tick . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.High);
            }
            finally
            {
                _timer.AutoReset = true;
                _timer.Start();
            }
        }

        // This Method User For get file data into byte format
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
                if (!File.Exists(localServerfilePath))
                {
                    return false;
                }
                fileDetail.FileName = Path.Combine(FileInfo.Name);
                fileDetail.FilePath = mainServerPath;
                byte[] bytes = GetBytesFromFile(localServerfilePath);
                fileDetail.FileByte = bytes;
                //Updated service from FileUpload to FileUpload_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                //Updated service from FileUpload_V2450 to FileUpload_V2460 by [24.11.2023][rdixit]
                //[rdixit][29.01.2024]
                FileReturnMessage returnMsgFileUploading = ServiceController.FileUpload_V2480(fileDetail);
                return returnMsgFileUploading.IsFileActionPerformed;
            }
        }

        /// <summary>
        /// Read Xml File from LocalServer
        /// Perform opration acording to xml file
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
                    if (xmlDetailslist.Count > 0)
                    {
                        Log4NetLogger.Logger.Log(string.Format(" ReadXmlFileLocalServer().Message- {0} Started file uploading count is - {1}", "ReadXmlFileLocalServer Start", xmlDetailslist.Count), category: Category.Info, priority: Priority.Low);
                    }

                    int successCount = 0;
                    foreach (var xmlFileDetails in xmlDetailslist)
                    {
                        try
                        {
                            Log4NetLogger.Logger.Log(string.Format(" ReadXmlFileLocalServer(). Working on xaml file - {0}. ", xmlFileDetails.xmlFileName), category: Category.Info, priority: Priority.Low);

                            string fileName = Path.GetFileName(xmlFileDetails.xmlFilePath);
                            if (fileName.ToLower() != "thumbs.db")
                            {
                                #region Created
                                if (xmlFileDetails.fileOperation.ToString() == "Created")
                                {
                                    checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                    if (checkFiletype)
                                    {
                                        fileDetail = new FileDetail();
                                        //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                        var folderUploadingPath =string.Empty;
                                        if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                        {
                                             folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                        }
                                        else
                                        {
                                            folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                        }
                                        //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                        fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                        //Updated service from CheckDirectoryExistOrNot to CheckDirectoryExistOrNot_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                        FileReturnMessage ReturnMessageDirectory = ServiceController.CheckDirectoryExistOrNot_V2450(fileDetail);

                                        Log4NetLogger.Logger.Log(string.Format("New Folder created on local - {0}. Create new folder on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                        if (!ReturnMessageDirectory.IsFileActionPerformed)
                                        {
                                            FileReturnMessage ReturnMessageCreateDirectory = ServiceController.CreateDirectory(fileDetail);
                                            if (ReturnMessageCreateDirectory.IsFileActionPerformed)
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Folder- {0}", "New Folder Created " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);
                                            }
                                            else
                                                Log4NetLogger.Logger.Log(string.Format("Folder- {0}", "New Folder Created unsuccessfully" + " " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);
                                        }
                                        else
                                            Log4NetLogger.Logger.Log(string.Format("Folder- {0}", "New Folder Created unsuccessfully" + " (Folder does Not exist in the local server) " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);
                                        DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                    }
                                    else
                                    {
                                        fileDetail = new FileDetail();
                                        //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                        var folderUploadingPath = string.Empty;
                                        if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                        {
                                            folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                        }
                                        else
                                        {
                                            folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                        }
                                        //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                        fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                        //Updated service from CheckFileExistOrNot to CheckFileExistOrNot_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                        FileDetail xmlFileDetail = new FileDetail();
                                        xmlFileDetail.FilePath = xmlFileDetails.filePath;
                                        FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot_V2450(xmlFileDetail);
                                        Log4NetLogger.Logger.Log(string.Format("New File created on local - {0}. Create new file on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                        if (ReturnMessage.IsFileActionPerformed)
                                        {
                                            returnmsg = FileUploadOnMainServer(xmlFileDetails.filePath.ToString(), fileDetail.FilePath);
                                            if (returnmsg)                                            
                                                Log4NetLogger.Logger.Log(string.Format("File Details- {0}", "File Upload successfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);                                            
                                            else
                                                Log4NetLogger.Logger.Log(string.Format("File Details- {0}", "File Uploading unsuccessfully " + " (Check if File path is available on main server) " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                        }
                                        else
                                        {
                                            //[rdixit][29.01.2024]
                                            FileReturnMessage returnMsgFileUploading = ServiceController.FileUpload_V2480(fileDetail);
                                            if (returnMsgFileUploading.IsFileActionPerformed)                                            
                                                Log4NetLogger.Logger.Log(string.Format("File Details- {0}", "File Upload successfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);                                              
                                            else
                                                Log4NetLogger.Logger.Log(string.Format("File Details- {0}", "File Uploading unsuccessfully " + " (Check if File path is available on local server) " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);

                                        }
                                        DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                        Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Create XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                    }
                                }
                                #endregion

                                #region Renamed
                                else if (xmlFileDetails.fileOperation.ToString() == "Renamed")
                                {
                                    try
                                    {
                                        checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                        if (checkFiletype)
                                        {
                                            fileDetail = new FileDetail();
                                            //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                            var folderUploadingPath = string.Empty;
                                            if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                            {
                                                folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            else
                                            {
                                                folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            fileDetail.fileOldName = xmlFileDetails.oldFilePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            //Updated service from CheckDirectoryExistOrNot to CheckDirectoryExistOrNot_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                            FileReturnMessage ReturnMessage = ServiceController.CheckDirectoryExistOrNot_V2450(fileDetail);

                                            Log4NetLogger.Logger.Log(string.Format("Rename Folder created on local - {0}. Rename new folder on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                            if (!ReturnMessage.IsFileActionPerformed)
                                            {
                                                FileReturnMessage ReturnMessageRenamefolder = ServiceController.RenameFolder(fileDetail);
                                                if (ReturnMessageRenamefolder.IsFileActionPerformed)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Folder ReName Successfully " + "  " + xmlFileDetails.filePath), category: Category.Info, priority: Priority.Low);
                                                }
                                                else
                                                    Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Folder ReName unsuccessfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                            }
                                            else
                                                Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Folder ReName unsuccessfully [Already exist folder name exist]" + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);

                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Rename XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);

                                        }
                                        else
                                        {
                                            fileDetail = new FileDetail();
                                            //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                            var folderUploadingPath = string.Empty;
                                            if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                            {
                                                folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            else
                                            {
                                                folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            fileDetail.fileOldName = xmlFileDetails.oldFilePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            //Updated service from CheckFileExistOrNot to CheckFileExistOrNot_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                            FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot_V2450(fileDetail);

                                            Log4NetLogger.Logger.Log(string.Format("Rename file created on local - {0}. Rename new file on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                            if (!ReturnMessage.IsFileActionPerformed)
                                            {
                                                FileReturnMessage ReturnMessageRenamefile = ServiceController.RenameFile(fileDetail);
                                                if (ReturnMessageRenamefile.IsFileActionPerformed)
                                                    Log4NetLogger.Logger.Log(string.Format("File Details- {0}", "File ReName Successfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);
                                                else
                                                    Log4NetLogger.Logger.Log(string.Format("File Details- {0}", "File ReName unsuccessfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                            }
                                            else
                                                Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "File ReName unsuccessfully [Already exist file name exist] " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);

                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Rename XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);

                                        }
                                    }
                                    catch (FileNotFoundException ex)//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                                    {
                                        DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                        Log4NetLogger.Logger.Log($"File not found: {xmlFileDetails.filePath}. Exception Message: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            FileDetail xmlFileDetail = new FileDetail();
                                            xmlFileDetail.FilePath = xmlFileDetails.filePath;
                                            FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot_V2450(xmlFileDetail);
                                            if (ReturnMessage.IsFileActionPerformed)
                                            {
                                                if (GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                                {
                                                    string getServerPath = GetRelativePath(xmlFileDetails.filePath, GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath);
                                                    string mainServerPath = Path.Combine(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath, getServerPath);
                                                    try
                                                    {
                                                        // Get the directory path (excluding the file name)
                                                        string mainServerDirectoryPath = Path.GetDirectoryName(mainServerPath);
                                                        xmlFileDetail.FilePath = mainServerPath;
                                                        ReturnMessage = ServiceController.CheckFileExistOrNot_V2450(xmlFileDetail);
                                                        if (!ReturnMessage.IsFileActionPerformed)
                                                        {
                                                            Log4NetLogger.Logger.Log($"Info in GetRelativePath(). getServerPath {xmlFileDetails.filePath.ToString()}", category: Category.Info, priority: Priority.Low);
                                                            Log4NetLogger.Logger.Log($"Info in mainServerPath {xmlFileDetail.FilePath}", category: Category.Info, priority: Priority.Low);
                                                            // Copy the file
                                                            //File.Copy(xmlFileDetails.filePath, mainServerPath, overwrite: true);
                                                            returnmsg = FileUploadOnMainServer(xmlFileDetails.filePath.ToString(), xmlFileDetail.FilePath);
                                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                                            successCount++;
                                                        }
                                                    }
                                                    catch (Exception Exc)
                                                    {
                                                        Log4NetLogger.Logger.Log($"ERROR in UploadWorkingOrdersToMainServer_V2590(). ErrorMessage - {Exc}", category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of File ReadXmlFileLocalServer() Renamed File Action . ErrorMessage- {0}", e.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                                        }
                                        Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of File ReadXmlFileLocalServer() Renamed File Action . ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                                    }
                                }
                                #endregion

                                #region Deleted
                                else if (xmlFileDetails.fileOperation.ToString() == "Deleted")
                                {
                                    try
                                    {
                                        checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                        if (checkFiletype)
                                        {
                                            fileDetail = new FileDetail();
                                            //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                            var folderUploadingPath = string.Empty;
                                            if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                            {
                                                folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            else
                                            {
                                                folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            //Updated service from CheckDirectoryExistOrNot to CheckDirectoryExistOrNot_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                            FileReturnMessage ReturnMessage = ServiceController.CheckDirectoryExistOrNot_V2450(fileDetail);

                                            Log4NetLogger.Logger.Log(string.Format("Delete Folder created on local - {0}. Delete new folder on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                            if (ReturnMessage.IsFileActionPerformed)
                                            {
                                                FileReturnMessage ReturnMessageForDelete = ServiceController.DeleteFolder(fileDetail);

                                                if (ReturnMessageForDelete.IsFileActionPerformed)
                                                    Log4NetLogger.Logger.Log(string.Format("Folder  Details- {0}", "Folder Delete successfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);

                                                else
                                                    Log4NetLogger.Logger.Log(string.Format("Folder  Details- {0}", "Folder Delete unsuccessfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                            }
                                            else  //  Added by Gopal Mahale   to delete extra file genrated
                                                Log4NetLogger.Logger.Log(string.Format("Folder  Details- {0}", "Folder Delete unsuccessfully " + "(does not exist in the local path)  " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);

                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Rename XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                        }
                                        else
                                        {
                                            fileDetail = new FileDetail();
                                            //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                            var folderUploadingPath = string.Empty;
                                            if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                            {
                                                folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            else
                                            {
                                                folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            //Updated service from CheckFileExistOrNot to CheckFileExistOrNot_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                            FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot_V2450(fileDetail);

                                            Log4NetLogger.Logger.Log(string.Format("Delete file created on local - {0}. Delete file on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                            if (ReturnMessage.IsFileActionPerformed)
                                            {
                                                //Updated service from DeleteFile to DeleteFile_V2450 by [GEOS2-5021][10.11.2023][rdixit]
                                                FileReturnMessage ReturnMessageDelete = ServiceController.DeleteFile_V2450(fileDetail);
                                                if (ReturnMessageDelete.IsFileActionPerformed)
                                                    Log4NetLogger.Logger.Log(string.Format("File  Details- {0}", "File Delete successfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);
                                                else
                                                    Log4NetLogger.Logger.Log(string.Format("File  Details- {0}", "File Delete unsuccessfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                            }
                                            else  //  Added by Gopal Mahale   to delete extra file genrated
                                                Log4NetLogger.Logger.Log(string.Format("File  Details- {0}", "File Delete unsuccessfully " + " (Does not exist in the local path) " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);

                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log(string.Format("Folder Details- {0}", "Delete XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);

                                        }
                                    }
                                    catch (FileNotFoundException ex)//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                                    {
                                        DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                        successCount++;
                                        Log4NetLogger.Logger.Log($"File not found: {xmlFileDetails.filePath}. Exception Message: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of File ReadXmlFileLocalServer() Deleted File Action. ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                                    }
                                }
                                #endregion

                                //[rdixit][28.11.2023][GEOS2-5088]
                                #region Changed
                                else if (xmlFileDetails.fileOperation.ToString() == "Changed")
                                {
                                    checkFiletype = GenerateXmlFile.IsFileOrFolder(xmlFileDetails.filePath.ToString());
                                    if (checkFiletype)
                                    {
                                        try
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] Is Folder {0}", checkFiletype), category: Category.Info, priority: Priority.Low);
                                            fileDetail = new FileDetail();
                                            FileDetail OldfileDetail = new FileDetail();
                                            var oldFile = xmlFileDetails;
                                            OldfileDetail.FilePath = xmlFileDetails.filePath;
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] Old folder path {0}", OldfileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                            //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                            var folderUploadingPath = string.Empty;
                                            if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                            {
                                                folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            else
                                            {
                                                folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] folder Uploading Path {0}", folderUploadingPath), category: Category.Info, priority: Priority.Low);
                                            fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            FileReturnMessage ReturnMessage = ServiceController.CheckDirectoryExistOrNot_V2450(fileDetail);
                                            if (ReturnMessage.IsFileActionPerformed == false)
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Rename Folder created on local - {0}. Rename new folder on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                                //[rdixit][29.01.2024][GEOS2-5300] Service Updated
                                                bool result = ServiceController.CopyDirectory_V2480(xmlFileDetails.filePath.ToString(), fileDetail.FilePath);
                                                if (result)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("[Changed Action] Folder Details- {0}", "Folder Created Successfully " + "  " + xmlFileDetails.filePath), category: Category.Info, priority: Priority.Low);
                                                }
                                                else
                                                    Log4NetLogger.Logger.Log(string.Format("[Changed Action] Folder Details- {0}", "Folder Created unsuccessfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                            }
                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] Folder Details- {0}", "Changes XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                        }
                                        catch (FileNotFoundException ex)//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                                        {
                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log($"File not found: {xmlFileDetails.filePath}. Exception Message: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] ERROR in while operation of folder ReadXmlFileLocalServer() Changed File Action. ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] Is File {0}", checkFiletype), category: Category.Info, priority: Priority.Low);

                                            fileDetail = new FileDetail();
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] File Watcher Root Path {0}", xmlFileDetails.folderWatcherRootPath), category: Category.Info, priority: Priority.Low);
                                            //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                            var folderUploadingPath = string.Empty;
                                            if (GenerateXmlFile.DiFolderToWatch.Any(a => a.Key == xmlFileDetails.folderWatcherRootPath))
                                            {
                                                folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            else
                                            {
                                                folderUploadingPath = GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            }
                                            //var folderUploadingPath = GenerateXmlFile.DiFolderToWatch[xmlFileDetails.folderWatcherRootPath];
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] File Uploading Path {0}", xmlFileDetails.folderWatcherRootPath), category: Category.Info, priority: Priority.Low);
                                            fileDetail.FilePath = xmlFileDetails.filePath.Replace(xmlFileDetails.folderWatcherRootPath, folderUploadingPath);
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] File Uploading Path {0}", fileDetail.FilePath), category: Category.Info, priority: Priority.Low);
                                            FileReturnMessage ReturnMessage = ServiceController.CheckFileExistOrNot(fileDetail);
                                            //Shubham[skadam] GEOS2-6922 HRM Photo.  05 02 2024
                                            //if (ReturnMessage.IsFileActionPerformed == false)
                                            //{
                                                Log4NetLogger.Logger.Log(string.Format("[Changed Action] New File created on local - {0}. Create new file on main {1}.", xmlFileDetails.filePath, fileDetail.FilePath), category: Category.Info, priority: Priority.Low);

                                                returnmsg = FileUploadOnMainServer(xmlFileDetails.filePath.ToString(), fileDetail.FilePath);
                                                if (returnmsg)
                                                    Log4NetLogger.Logger.Log(string.Format("[Changed Action] File Details- {0}", "File Changed successfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Info, priority: Priority.Low);

                                                else
                                                    Log4NetLogger.Logger.Log(string.Format("[Changed Action] File Details- {0}", "File Changed unsuccessfully " + "  " + xmlFileDetails.filePath.ToString()), category: Category.Warn, priority: Priority.Low);
                                            //}
                                            RemoveFileFromFileDetailsList(xmlFileDetails.xmlFilePath.ToString());
                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log(string.Format("[Changed Action] File Details- {0}", "Changes XAML file Deleted successfully " + "  " + xmlFileDetails.filePath), category: Category.Warn, priority: Priority.Low);
                                        }
                                        catch (FileNotFoundException ex)//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                                        {
                                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                                            successCount++;
                                            Log4NetLogger.Logger.Log($"File not found: {xmlFileDetails.filePath}. Exception Message: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of File ReadXmlFileLocalServer() Changed File Action. ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        catch (FileNotFoundException ex)//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                        {
                            DeletelocalServerXmlFile(xmlFileDetails.xmlFilePath.ToString());
                            successCount++;
                            Log4NetLogger.Logger.Log($"File not found: {xmlFileDetails.filePath}. Exception Message: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of file ReadXmlFileLocalServer(). ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of file ReadXmlFileLocalServer(). ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in while operation of file ReadXmlFileLocalServer(). ErrorMessage- {0}", ex.ToString() + " " + xmlFileDetails.xmlFilePath), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    if (xmlDetailslist.Count > 0 && successCount > 0)
                    {
                        Log4NetLogger.Logger.Log(string.Format(" ReadXmlFileLocalServer(). Read Xml file SuccessFully operation with file count - {0}", successCount), category: Category.Info, priority: Priority.Low);
                    }
                    isBusyFileReplicator = false;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in ReadXmlFileLocalServer(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
        public void UploadWorkingOrdersDataInBackground()
        {
            // Fire-and-forget with error logging
            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    await System.Threading.Tasks.Task.WhenAll(
                       UploadWorkingOrdersAsync()
                    );
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("ERROR in LoadAllDataInBackground() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                }
            });
        }
        //Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
        public async Task UploadWorkingOrdersAsync()
        {
            FileReplicatorData UploadWorkingOrder = new FileReplicatorData();
            List<string> filteredFiles = new List<string>();
            try
            {
                if (GenerateXmlFile.UploadWorkingOrder != null)
                {
                    if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrder.localServerPath) &&
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrder.mainServerPath))
                    {
                        UploadWorkingOrder = GenerateXmlFile.UploadWorkingOrder;
                        DateTime fQDocFromDate = DateTime.Parse(UploadWorkingOrder.fQDocFromDate);
                        DateTime fQDocToDate = DateTime.Parse(UploadWorkingOrder.fQDocToDate);
                        try
                        {
                            if (!string.IsNullOrEmpty(UploadWorkingOrder.fQDocFolder))
                            {
                                // Parse comma-separated folder names into a list (trimmed, case-insensitive)
                                string[] targetFoldersRaw = UploadWorkingOrder.fQDocFolder.Split(',');
                                List<string> targetFolders = targetFoldersRaw.Select(f => f.Trim()).ToList();
                                filteredFiles = Directory.EnumerateFiles(GenerateXmlFile.UploadWorkingOrder.localServerPath, "*.*", SearchOption.AllDirectories)
                                    .Where(f =>
                                    {
                                        try
                                        {
                                            // Normalize path for comparison (case-insensitive, full path)
                                            string filePath = f.ToLowerInvariant();
                                            // Check if the file is in any of the target subfolders
                                            bool inTargetFolder = targetFolders.Any(folder => filePath.Contains(Path.DirectorySeparatorChar + folder.ToLowerInvariant() + Path.DirectorySeparatorChar));
                                            if (!inTargetFolder) return false;
                                            DateTime creationTime = File.GetCreationTime(f);
                                            DateTime lastWriteTime = File.GetLastWriteTime(f);
                                            return (creationTime >= fQDocFromDate && creationTime <= fQDocToDate) || (lastWriteTime >= fQDocFromDate && lastWriteTime <= fQDocToDate);
                                        }
                                        catch
                                        {
                                            return false; // Skip unreadable files
                                        }
                                    })
                                    .ToList();
                            }
                            else
                            {
                               // Enumerate files instead of loading all at once
                                filteredFiles = Directory.EnumerateFiles(GenerateXmlFile.UploadWorkingOrder.localServerPath, "*.*", SearchOption.AllDirectories)
                                    .Where(f =>
                                    {
                                        try
                                        {
                                            DateTime creationTime = File.GetCreationTime(f);
                                            DateTime lastWriteTime = File.GetLastWriteTime(f);
                                            return (creationTime >= fQDocFromDate && creationTime <= fQDocToDate) || (lastWriteTime >= fQDocFromDate && lastWriteTime <= fQDocToDate);
                                        }
                                        catch
                                        {
                                            return false; // Skip unreadable files
                                        }
                                    }).ToList();
                            }

                            Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrders().Operation file count - {0}", filteredFiles.Count()), category: Category.Info, priority: Priority.Low);
                            if (filteredFiles.Count() > 0)
                            {
                                foreach (var file in filteredFiles)
                                {
                                    if (!GenerateXmlFile.IsUploadFQTimeBetween())
                                    {
                                        return;
                                    }
                                    if (File.Exists(file))
                                    {
                                        string getServerPath = GetRelativePath(file, GenerateXmlFile.UploadWorkingOrder.localServerPath);
                                        string mainServerPath = Path.Combine(GenerateXmlFile.UploadWorkingOrder.mainServerPath, getServerPath);
                                        try
                                        {
                                            if (!File.Exists(mainServerPath))
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Info in GetRelativePath() . getServerPath {0}", getServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                Log4NetLogger.Logger.Log(string.Format("Info in localServerPath {0}", file.ToString()), category: Category.Info, priority: Priority.Low);
                                                Log4NetLogger.Logger.Log(string.Format("Info in mainServerPath {0}", mainServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                try
                                                {
                                                    // Get the file name from the source file path
                                                    string fileName = Path.GetFileName(file);
                                                    // Get the directory path (excluding the file name)
                                                    string mainServerdirectoryPath = Path.GetDirectoryName(mainServerPath);
                                                    if (!Directory.Exists(mainServerdirectoryPath))
                                                    {
                                                        System.IO.Directory.CreateDirectory(mainServerdirectoryPath);
                                                        Log4NetLogger.Logger.Log(string.Format("Info in CreateDirectory {0}", mainServerdirectoryPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                    }
                                                    //Copy the file
                                                    File.Copy(file, mainServerPath, overwrite: true);
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (!ex.ToString().Contains("because it is being used by another process"))
                                                    {
                                                        Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                            }
                                            //else
                                            //{
                                            //    Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrders().File.Exists on main server - {0}", mainServerPath), category: Category.Info, priority: Priority.Low);
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrders().Operation file count - {0}", filteredFiles.Count()), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrders() .Info localServerPath - {0}", GenerateXmlFile.UploadWorkingOrder.localServerPath), category: Category.Info, priority: Priority.Low);
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrders() .Info mainServerPath - {0}", GenerateXmlFile.UploadWorkingOrder.mainServerPath), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UploadWorkingOrders()
        {
            FileReplicatorData UploadWorkingOrder = new FileReplicatorData();
            try
            {
                if (GenerateXmlFile.UploadWorkingOrder != null)
                {
                    if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrder.localServerPath) &&
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrder.mainServerPath))
                    {
                        UploadWorkingOrder = GenerateXmlFile.UploadWorkingOrder;
                        DateTime fQDocFromDate = DateTime.Parse(UploadWorkingOrder.fQDocFromDate);
                        DateTime fQDocToDate = DateTime.Parse(UploadWorkingOrder.fQDocToDate);
                        try
                        {
                            #region UnnecessaryCode
                            /*
                            List<string> allFiles = new List<string>();
                            // Enumerate files instead of loading all at once
                            string[] getWorkingOrdersFileNew = Directory.GetFiles(GenerateXmlFile.UploadWorkingOrder.localServerPath, "*.*", SearchOption.AllDirectories);
                            // Filter by date range
                            List<string> filteredFiles = getWorkingOrdersFile
                                        .Where(file =>
                                        {
                                            DateTime creationDate = File.GetCreationTime(file);
                                            DateTime modifiedDate = File.GetLastWriteTime(file);

                                            return (creationDate >= fQDocFromDate && creationDate <= fQDocToDate) ||  (modifiedDate >= fQDocFromDate && modifiedDate <= fQDocToDate);
                                        })
                                        .ToList();
                            */
                            #endregion
                            //Enumerate files instead of loading all at once
                            List<string> filteredFiles = Directory.EnumerateFiles(GenerateXmlFile.UploadWorkingOrder.localServerPath, "*.*", SearchOption.AllDirectories)
                                .Where(f =>
                                {
                                    try
                                    {
                                        DateTime creationTime = File.GetCreationTime(f);
                                        DateTime lastWriteTime = File.GetLastWriteTime(f);
                                        return (creationTime >= fQDocFromDate && creationTime <= fQDocToDate) || (lastWriteTime >= fQDocFromDate && lastWriteTime <= fQDocToDate);
                                    }
                                    catch
                                    {
                                        return false; // Skip unreadable files
                                    }
                                }).ToList();
                            if (filteredFiles.Count() > 0)
                            {
                                foreach (var file in filteredFiles)
                                {
                                    if (File.Exists(file))
                                    {
                                        string getServerPath = GetRelativePath(file, GenerateXmlFile.UploadWorkingOrder.localServerPath);
                                        string mainServerPath = Path.Combine(GenerateXmlFile.UploadWorkingOrder.mainServerPath, getServerPath);
                                        try
                                        {
                                            if (!File.Exists(mainServerPath))
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Info in GetRelativePath() . getServerPath {0}", getServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                Log4NetLogger.Logger.Log(string.Format("Info in mainServerPath {0}", mainServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                try
                                                {
                                                    // Get the file name from the source file path
                                                    string fileName = Path.GetFileName(file);
                                                    // Get the directory path (excluding the file name)
                                                    string mainServerdirectoryPath = Path.GetDirectoryName(mainServerPath);
                                                    if (!Directory.Exists(mainServerdirectoryPath))
                                                    {
                                                        System.IO.Directory.CreateDirectory(mainServerdirectoryPath);
                                                        Log4NetLogger.Logger.Log(string.Format("Info in CreateDirectory {0}", mainServerdirectoryPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                    }
                                                    //Copy the file
                                                    File.Copy(file, mainServerPath, overwrite: true);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                            else
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrders().File.Exists on main server - {0}", mainServerPath), category: Category.Info, priority: Priority.Low);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrders().Operation file count - {0}", filteredFiles.Count()), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrders() .Info localServerPath - {0}", GenerateXmlFile.UploadWorkingOrder.localServerPath), category: Category.Info, priority: Priority.Low);
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrders() .Info mainServerPath - {0}", GenerateXmlFile.UploadWorkingOrder.mainServerPath), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrders() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
        private void UploadWorkingOrdersToMainServer()
        {
            try
            {
                if (GenerateXmlFile.UploadWorkingOrdersFileReplicatorData!=null)
                {
                    if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions) && 
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection))
                    {
                        if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath) &&
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath)
                        && GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection.ToLower().Equals("Upload".ToLower()))
                        {
                            // Get all PDF and PNG files
                            // string[] pdfFiles = Directory.GetFiles(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath, "*.pdf");
                            //string[] pngFiles = Directory.GetFiles(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath, "*.png");
                            // Split the string into an array and convert to a List
                            // Get all PDF and PNG files
                            List<string> allFiles = new List<string>();
                            if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions))
                            {
                                Log4NetLogger.Logger.Log(string.Format("Info in fileExtensions {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions.ToString()), category: Category.Info, priority: Priority.Low);
                                List<string> commaSeparatedItems = GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions.Split(',').ToList();
                                foreach (var item in commaSeparatedItems)
                                {
                                    string[] files = Directory.GetFiles(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath, item.Trim(), SearchOption.AllDirectories);
                                    allFiles.AddRange(files); // Add to the aggregated list
                                }
                            }
                            if (allFiles.Count()>0)
                            {
                                foreach (string file in allFiles)
                                {
                                    if (File.Exists(file))
                                    {
                                        string getServerPath = GetRelativePath(file, GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath);
                                        string mainServerPath = Path.Combine(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath, getServerPath);
                                        try
                                        {
                                            if (!File.Exists(mainServerPath))
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Info in GetRelativePath() . getServerPath {0}", getServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                Log4NetLogger.Logger.Log(string.Format("Info in mainServerPath {0}", mainServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                try
                                                {
                                                    // Get the file name from the source file path
                                                    string fileName = Path.GetFileName(file);
                                                    // Get the directory path (excluding the file name)
                                                    string mainServerdirectoryPath = Path.GetDirectoryName(mainServerPath);
                                                    if (!Directory.Exists(mainServerdirectoryPath))
                                                    {
                                                        System.IO.Directory.CreateDirectory(mainServerdirectoryPath);
                                                        Log4NetLogger.Logger.Log(string.Format("Info in CreateDirectory {0}", mainServerdirectoryPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                    }
                                                    // Copy the file
                                                    File.Copy(file, mainServerPath, overwrite: true);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrdersToMainServer() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrdersToMainServer() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrdersToMainServer().Operation file count - {0}", allFiles.Count()), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        else
                        {
                            Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath), category: Category.Info, priority: Priority.Low);
                            Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath), category: Category.Info, priority: Priority.Low);
                            Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection), category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer() .Info fileExtensions - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions), category: Category.Info, priority: Priority.Low);
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrdersToMainServer() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
        private void UploadWorkingOrdersToMainServer_V2590()
        {
            try
            {
                if (GenerateXmlFile.UploadWorkingOrdersFileReplicatorData != null)
                {
                    if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions) &&
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection))
                    {
                        if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath) &&
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath)
                        && GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection.ToLower().Equals("Upload".ToLower()))
                        {
                            // Get all PDF and PNG files
                            
                            if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions))
                            {

                                List<string> commaSeparatedItems = GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions.Split(',').ToList();
                                if (getAllWorkingOrdersFiles.Count()==0)
                                {
                                    getdirectory:
                                    if (directoryList != null && directoryList.Count() > 0)
                                    {
                                        string directory = directoryList.FirstOrDefault();
                                        foreach (var item in commaSeparatedItems)
                                        {
                                            GetWorkingOrdersFiles(directory, item);
                                        }
                                        directoryList.Remove(directory);
                                    }
                                    else
                                    {
                                        if (directoryList == null || directoryList.Count() == 0)
                                        {
                                            // Assuming you want to populate directoryList with directories from the local path
                                            directoryList = Directory.GetDirectories(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath).ToList();
                                            goto getdirectory;
                                        }
                                    }
                                }
                                else
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrdersToMainServer_V2590().Remaining Operation file count - {0}", getAllWorkingOrdersFiles.Count()), category: Category.Info, priority: Priority.Low);
                                }
                            }
                            if (getAllWorkingOrdersFiles.Count() > 0)
                            {
                                // Assuming updatefileCount is already defined
                                Int32 updatefileCount = Convert.ToInt32(GenerateXmlFile.takeNumberOfFiles);
                                // Convert the existing List (getAllWorkingOrdersFiles) to a new List (WorkingOrdersFiles)
                                List<string> workingOrdersFiles = getAllWorkingOrdersFiles.Take(updatefileCount).ToList();
                                foreach (var file in workingOrdersFiles)
                                {
                                    if (File.Exists(file))
                                    {
                                        string getServerPath = GetRelativePath(file, GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath);
                                        string mainServerPath = Path.Combine(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath, getServerPath);
                                        try
                                        {
                                            if (!File.Exists(mainServerPath))
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("Info in GetRelativePath() . getServerPath {0}", getServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                Log4NetLogger.Logger.Log(string.Format("Info in mainServerPath {0}", mainServerPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                // Get the file name from the source file path
                                                string fileName = Path.GetFileName(file);
                                                // Get the directory path (excluding the file name)
                                                string mainServerdirectoryPath = Path.GetDirectoryName(mainServerPath);
                                                if (!Directory.Exists(mainServerdirectoryPath))
                                                {
                                                    System.IO.Directory.CreateDirectory(mainServerdirectoryPath);
                                                    Log4NetLogger.Logger.Log(string.Format("Info in CreateDirectory {0}", mainServerdirectoryPath.ToString()), category: Category.Info, priority: Priority.Low);
                                                }
                                                // Copy the file
                                                File.Copy(file, mainServerPath, overwrite: true);
                                            }
                                            //getAllWorkingOrdersFiles.Remove(file);
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrdersToMainServer() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                        }
                                        getAllWorkingOrdersFiles.Remove(file);
                                    }
                                }
                            }
                            else
                            {
                                Log4NetLogger.Logger.Log(string.Format("Info UploadWorkingOrdersToMainServer_V2590().Operation file count - {0}", getAllWorkingOrdersFiles.Count()), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        else
                        {
                            Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer_V2590() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath), category: Category.Info, priority: Priority.Low);
                            Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer_V2590() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath), category: Category.Info, priority: Priority.Low);
                            Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer_V2590() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection), category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer_V2590() .Info fileExtensions - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions), category: Category.Info, priority: Priority.Low);
                        Log4NetLogger.Logger.Log(string.Format("Info in UploadWorkingOrdersToMainServer_V2590() .Info synchDirection - {0}", GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in UploadWorkingOrdersToMainServer_V2590() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
        private async Task asyncUploadWorkingOrdersToMainServer_V2590()
        {
            try
            {
                if (GenerateXmlFile.UploadWorkingOrdersFileReplicatorData != null)
                {
                    if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions) &&
                        !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection))
                    {
                        if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath) &&
                            !string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath) &&
                            GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection.Equals("Upload", StringComparison.OrdinalIgnoreCase))
                        {
                            // Get all PDF and PNG files
                            if (!string.IsNullOrEmpty(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions))
                            {
                                List<string> commaSeparatedItems = GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions.Split(',').ToList();

                                if (getAllWorkingOrdersFiles.Count() == 0)
                                {
                                    getdirectory:
                                    if (directoryList != null && directoryList.Any())
                                    {
                                        string directory = directoryList.FirstOrDefault();
                                        foreach (var item in commaSeparatedItems)
                                        {
                                            await Task.Run(() => GetWorkingOrdersFiles(directory, item));
                                        }
                                        directoryList.Remove(directory);
                                    }
                                    else
                                    {
                                        if (directoryList == null || !directoryList.Any())
                                        {
                                            // Populate directoryList with directories from the local path
                                            directoryList = (await Task.Run(() => Directory.GetDirectories(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath))).ToList();
                                            goto getdirectory;
                                        }
                                    }
                                }
                                else
                                {
                                    Log4NetLogger.Logger.Log($"Info UploadWorkingOrdersToMainServer_V2590().Remaining Operation file count - {getAllWorkingOrdersFiles.Count()}", category: Category.Info, priority: Priority.Low);
                                }
                            }

                            if (getAllWorkingOrdersFiles.Count() > 0)
                            {
                                // Assuming updatefileCount is already defined
                                int updateFileCount = Convert.ToInt32(GenerateXmlFile.takeNumberOfFiles);
                                // Convert the existing List (getAllWorkingOrdersFiles) to a new List (WorkingOrdersFiles)
                                List<string> workingOrdersFiles = getAllWorkingOrdersFiles.Take(updateFileCount).ToList();

                                foreach (var file in workingOrdersFiles)
                                {
                                    if (File.Exists(file))
                                    {
                                        string getServerPath = GetRelativePath(file, GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.localServerPath);
                                        string mainServerPath = Path.Combine(GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.mainServerPath, getServerPath);

                                        try
                                        {
                                            if (!File.Exists(mainServerPath))
                                            {
                                                Log4NetLogger.Logger.Log($"Info in GetRelativePath(). getServerPath {getServerPath}", category: Category.Info, priority: Priority.Low);
                                                Log4NetLogger.Logger.Log($"Info in mainServerPath {mainServerPath}", category: Category.Info, priority: Priority.Low);

                                                // Get the directory path (excluding the file name)
                                                string mainServerDirectoryPath = Path.GetDirectoryName(mainServerPath);
                                                if (!Directory.Exists(mainServerDirectoryPath))
                                                {
                                                    await Task.Run(() => Directory.CreateDirectory(mainServerDirectoryPath));
                                                    Log4NetLogger.Logger.Log($"Info in CreateDirectory {mainServerDirectoryPath}", category: Category.Info, priority: Priority.Low);
                                                }
                                                // Copy the file
                                                await Task.Run(() => File.Copy(file, mainServerPath, overwrite: true));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4NetLogger.Logger.Log($"ERROR in UploadWorkingOrdersToMainServer_V2590(). ErrorMessage - {ex}", category: Category.Exception, priority: Priority.Low);
                                        }
                                        getAllWorkingOrdersFiles.Remove(file);
                                    }
                                }
                            }
                            else
                            {
                                Log4NetLogger.Logger.Log($"Info UploadWorkingOrdersToMainServer_V2590().Operation file count - {getAllWorkingOrdersFiles.Count()}", category: Category.Info, priority: Priority.Low);
                            }
                        }
                        else
                        {
                            Log4NetLogger.Logger.Log($"Info in UploadWorkingOrdersToMainServer_V2590(). synchDirection - {GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.synchDirection}", category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        Log4NetLogger.Logger.Log($"Info in UploadWorkingOrdersToMainServer_V2590(). fileExtensions - {GenerateXmlFile.UploadWorkingOrdersFileReplicatorData.fileExtensions}", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"ERROR in UploadWorkingOrdersToMainServer_V2590(). ErrorMessage - {ex}", category: Category.Exception, priority: Priority.Low);
            }
        }

        public async Task GetWorkingOrdersFiles(string localServerPath, string searchPattern)
        {
            try
            {
                //List<string> allFiles = new List<string>();
                Parallel.ForEach(Directory.EnumerateDirectories(localServerPath, "*", SearchOption.AllDirectories), folder =>
                {
                    try
                    {
                        foreach (var file in Directory.EnumerateFiles(folder, searchPattern))
                        {
                            getAllWorkingOrdersFiles.Add(file);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Ignore folders we cannot access
                        Log4NetLogger.Logger.Log(string.Format("ERROR in GetWorkingOrdersFiles() Ignore folders we cannot access "), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing folder {folder}: {ex.Message}");
                    }
                });

                Log4NetLogger.Logger.Log(string.Format("Info in GetWorkingOrdersFiles()... Info - {0}","Path :- "+ localServerPath + " Pattern:- " + searchPattern + " File Count:- " + getAllWorkingOrdersFiles.Count), category: Category.Info, priority: Priority.Low);
                //getAllWorkingOrdersFiles.AddRange(allFiles);
           }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in GetWorkingOrdersFiles() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        static string GetRelativePath(string fullPath, string basePath)
        {
            string relativePath = string.Empty;
            try
            {
                // Ensure paths are treated correctly with Uri
                Uri fileUri = new Uri(fullPath);
                Uri baseUri = new Uri(basePath.EndsWith("\\") ? basePath : basePath + "\\");

                // Get relative path
                 relativePath = Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString())
                                        .Replace('/', '\\'); // Convert to Windows-style backslashes
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in GetRelativePath() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);

            }
            return relativePath;
        }

        /// <summary>
        /// Delete local Server XmlFile
        /// </summary>
        /// <param name="filePath"></param>
        public void DeletelocalServerXmlFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                Log4NetLogger.Logger.Log(string.Format("{0} Xml file SuccessFully deleted", filePath), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in DeletelocalServerXmlFile(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void GetIniSetting()
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("Service Status - {0}", "Service Start"), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Timer Interval Time - {0}", +GenerateXmlFile.intervalTimeInMinutes + " " + "Min"), category: Category.Info, priority: Priority.Low);
                foreach (KeyValuePair<string, string> filePath in GenerateXmlFile.DiFolderToWatch)
                {
                    Log4NetLogger.Logger.Log(string.Format(" Monitoring Local folder Path - {0} AND Uploading to Main server path - {1}", filePath.Key, filePath.Value), category: Category.Info, priority: Priority.Low);
                }
                Log4NetLogger.Logger.Log(string.Format("Xml Downloading Path local Server  - {0}", GenerateXmlFile.downloadingXmlFilePath), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("mainWebServerHostingIp  - {0}", GenerateXmlFile.mainWebServerHostingIp), category: Category.Info, priority: Priority.Low);

                //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                foreach (KeyValuePair<string, string> filePath in GenerateXmlFile.UploadWorkingOrdersFileDiFolderToWatch)
                {
                    Log4NetLogger.Logger.Log(string.Format(" Monitoring Local folder Path - {0} AND Uploading to Main server path - {1}", filePath.Key, filePath.Value), category: Category.Info, priority: Priority.Low);
                }
                Log4NetLogger.Logger.Log(string.Format("Xml uploadXmlFile Path local Server  - {0}", GenerateXmlFile.uploadXmlFilePath), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("mainWebServerHostingIp  - {0}", GenerateXmlFile.mainWebServerHostingIp), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in GetIniSetting(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        ///Check Xml downloading  Folder Eixts or Not (Check Local server)
        /// </summary>
        public void XmldownloadindFolderExistOrNot()
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
                //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                ///Check Xml uploadXmlFilePath Folder Eixts or Not (Check Local server)
                if (!string.IsNullOrEmpty(GenerateXmlFile.uploadXmlFilePath))
                {
                    if (!Directory.Exists(GenerateXmlFile.uploadXmlFilePath))
                    {
                        Directory.CreateDirectory(GenerateXmlFile.uploadXmlFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in XmldownloadindFolderExistOrNot() . ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-6922 HRM Photo.  05 02 2024
        public void RemoveFileFromFileDetailsList(string file)
        {
            try
            {
                string fileName = Path.GetFileName(file);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                fileWatcher.FileDetailsList.RemoveAll(r=>r.fileName.Equals(fileNameWithoutExtension));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in RemoveFileFromFileDetailsList(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}



