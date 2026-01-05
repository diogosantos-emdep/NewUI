using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace Emdep.Geos.FileReplicator.Utility
{
    public class XmlFileGenerator
    {
        public Dictionary<string, string> DiFolderToWatch = new Dictionary<string, string>();
        public string mainWebServerHostingIp = "";
        public string downloadingXmlFilePath = "";
        public int intervalTimeInMinutes = 2;
        public string downloadStartStopTime = "";
        public string takeNumberOfFiles = "";
        public string  uploadXmlFilePath = "";
        public FileReplicatorData UploadWorkingOrdersFileReplicatorData = new FileReplicatorData();
        public Dictionary<string, string> UploadWorkingOrdersFileDiFolderToWatch = new Dictionary<string, string>();
        public FileReplicatorData UploadWorkingOrder = new FileReplicatorData();
        public string uploadFQDocFolderStartStopTime = "";
        //public int downloadStartTime;
        //public int downloadStopTime;
        //public string downloadingXmlFileServerPath = "";
        //public int autoDownloadTimeInterval = 1;
        //public int hour = 12;
        //public string mdnight = "";

        public XmlFileGenerator()
        {
            if (Log4NetLogger.Logger == null)
            {
                string ApplicationLogFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                FileInfo file = new FileInfo(ApplicationLogFilePath);
                Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
            }

            GetSetting();
            DirectoryToWatch();
        }

        /// <summary>
        /// This method Use For Create Xml file on Local Server
        /// </summary>
        /// <param name="fileData"></param>
        public void CreateXMLFile(FileReplicatorData fileData)
        {
            try
            {
                XmlWriter xmlWriter;
                string xmlFilePath = fileData.filePath.ToString() + @"\" + fileData.fileName.ToString() + ".xml";
                string fileType = string.Empty;
                bool checkFiletype = IsFileOrFolder(fileData.filefullpath.ToString());

                if (checkFiletype)
                    fileType = "floder";
                else
                    fileType = "file";

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;

                using (xmlWriter = XmlWriter.Create(xmlFilePath, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("GEOSFileManager");
                    xmlWriter.WriteElementString("OperationType", fileData.fileOperation.ToString());
                    xmlWriter.WriteElementString("FilePath", fileData.filefullpath.ToString());

                    if (!String.IsNullOrEmpty(fileData.oldFilePath))
                        xmlWriter.WriteElementString("FileOldPath", fileData.oldFilePath.ToString());

                    xmlWriter.WriteElementString("FolderWatcherRootPath", fileData.folderWatcherRootPath.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }

                XmlDocument xDocument = new XmlDocument();
                xDocument.Load(xmlFilePath);
                xDocument.DocumentElement.SetAttribute("FileType", fileType);
                xDocument.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in CreateXMLFile() method. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This Method use for the  check file type  is folder or file 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsFileOrFolder(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            path = path.Trim();

            if (Directory.Exists(path))
                return true;

            if (File.Exists(path))
                return false;

            if (new[] { "\\", "/" }.Any(x => path.EndsWith(x)))
                return true;

            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }

        /// <summary>
        /// This Method Use For Get Xml File Name 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetTimestampXMlFileName(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// This method use for Read XmlFile And Get data into List 
        /// </summary>
        /// <returns></returns>
        public List<FileReplicatorData> ReadXmlFileDetails()
        {
            List<FileReplicatorData> XmlDetailslist = new List<FileReplicatorData>();
            try
            {
                //To get file local to server

                foreach (var file in System.IO.Directory.GetFiles(downloadingXmlFilePath, "*.xml"))
                {
                    if (new FileInfo(file).Length > 0)
                    {
                        Log4NetLogger.Logger.Log(string.Format(" {0} Xml file Read for Operation", file), category: Category.Info, priority: Priority.Low);
                        FileReplicatorData xmlFileData = new FileReplicatorData();
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(file);
                        var itemNodes = xmlDoc.SelectNodes("GEOSFileManager");
                         
                        foreach (XmlNode node in itemNodes)
                        {
                            xmlFileData.xmlFileName = Path.GetFileName(file);//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                            xmlFileData.fileOperation = node.SelectSingleNode("OperationType")?.InnerText;
                            xmlFileData.filePath = node.SelectSingleNode("FilePath")?.InnerText;
                            xmlFileData.oldFilePath = node.SelectSingleNode("FileOldPath")?.InnerText;
                            xmlFileData.folderWatcherRootPath = node.SelectSingleNode("FolderWatcherRootPath")?.InnerText;
                            xmlFileData.xmlFilePath = file.ToString();
                            XmlDetailslist.Add(xmlFileData);
                        }
                    }
                    else
                    {
                        File.Delete(file);
                    }
                }

                try
                {
					//Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                    foreach (var file in System.IO.Directory.GetFiles(uploadXmlFilePath, "*.xml"))
                    {
                        if (new FileInfo(file).Length > 0)
                        {
                            Log4NetLogger.Logger.Log(string.Format(" {0} Xml file Read for Operation", file), category: Category.Info, priority: Priority.Low);
                            FileReplicatorData xmlFileData = new FileReplicatorData();
                            var xmlDoc = new XmlDocument();
                            xmlDoc.Load(file);
                            var itemNodes = xmlDoc.SelectNodes("GEOSFileManager");

                            foreach (XmlNode node in itemNodes)
                            {
                                xmlFileData.xmlFileName = Path.GetFileName(file);//[https://helpdesk.emdep.com/browse/IESD-118245][rdixit][08.10.2024]
                                xmlFileData.fileOperation = node.SelectSingleNode("OperationType")?.InnerText;
                                xmlFileData.filePath = node.SelectSingleNode("FilePath")?.InnerText;
                                xmlFileData.oldFilePath = node.SelectSingleNode("FileOldPath")?.InnerText;
                                xmlFileData.folderWatcherRootPath = node.SelectSingleNode("FolderWatcherRootPath")?.InnerText;
                                xmlFileData.xmlFilePath = file.ToString();
                                if (!XmlDetailslist.Any(a=>a.xmlFileName.Equals(xmlFileData.xmlFileName)))
                                {
                                    XmlDetailslist.Add(xmlFileData);
                                }
                            }
                        }
                        else
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in ReadXmlFileDetails() method. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);

                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in ReadXmlFileDetails() method. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return XmlDetailslist;
        }

        public void DirectoryToWatch()
        {
            try
            {
                DiFolderToWatch = new Dictionary<string, string>();
                UploadWorkingOrdersFileDiFolderToWatch = new Dictionary<string, string>();
                var file = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "setting.xml");
                XDocument document = XDocument.Load(file);
                FileReplicatorData FileWatcherPath = new FileReplicatorData();

                var FolderToWatch = from r in document.Descendants("FolderToWatch")
                                    select new
                                    {
                                        localfilepath = r.Element("LocalServerPath")?.Value,
                                        serverFilePath = r.Element("MainServerPath")?.Value,
                                        fileExtensions = r.Element("FileExtensions")?.Value ?? null,
                                        synchDirection = r.Element("SynchDirection")?.Value ?? null,
                                        fQDocFromDate = r.Element("FQDocFromDate")?.Value ?? null,
                                        fQDocToDate = r.Element("FQDocToDate")?.Value ?? null,
                                        fQDocFolder = r.Element("FQDocFolder")?.Value ?? null,
                                        uploadFQDocFolderStartStopTime = r.Element("UploadFQDocFolderStartStopTime")?.Value ?? null,
                                    };

                foreach (var fileWatcherPath in FolderToWatch)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(fileWatcherPath.fileExtensions) && string.IsNullOrEmpty(fileWatcherPath.synchDirection)&&
                            string.IsNullOrEmpty(fileWatcherPath.fQDocFromDate) && string.IsNullOrEmpty(fileWatcherPath.fQDocToDate))
                        {
                            DiFolderToWatch.Add(fileWatcherPath.localfilepath, fileWatcherPath.serverFilePath);
                        }
                        else
                        {
                            if (fileWatcherPath.synchDirection!=null)
                            {
                                if (fileWatcherPath.synchDirection.ToLower().Equals("Upload".ToLower()))
                                {
                                    //Shubham[skadam] GEOS2-6555 Update File Replicated service to upload QA documents on main server.  11 12 2024
                                    UploadWorkingOrdersFileReplicatorData.localServerPath = fileWatcherPath.localfilepath;
                                    UploadWorkingOrdersFileReplicatorData.mainServerPath = fileWatcherPath.serverFilePath;
                                    UploadWorkingOrdersFileReplicatorData.fileExtensions = fileWatcherPath.fileExtensions;
                                    UploadWorkingOrdersFileReplicatorData.synchDirection = fileWatcherPath.synchDirection;
                                    UploadWorkingOrdersFileDiFolderToWatch.Add(fileWatcherPath.localfilepath, fileWatcherPath.serverFilePath);
                                }
                            }
                            else if (!string.IsNullOrEmpty(fileWatcherPath.fQDocFromDate) && !string.IsNullOrEmpty(fileWatcherPath.fQDocToDate))//Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
                            {
                                UploadWorkingOrder.localServerPath = fileWatcherPath.localfilepath;
                                UploadWorkingOrder.mainServerPath = fileWatcherPath.serverFilePath;
                                UploadWorkingOrder.fQDocFromDate = fileWatcherPath.fQDocFromDate;
                                UploadWorkingOrder.fQDocToDate = fileWatcherPath.fQDocToDate;
                                UploadWorkingOrder.fQDocFolder = fileWatcherPath.fQDocFolder;
                                uploadFQDocFolderStartStopTime = fileWatcherPath.uploadFQDocFolderStartStopTime;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error in DirectoryToWatch() method. ErrorMessage - {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in DirectoryToWatch() method. ErrorMessage - {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return;
        }

        public void GetSetting()
        {
            try
            {
                var file = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "setting.xml");
                XDocument document = XDocument.Load(file);
                FileReplicatorData FileWatcherPath = new FileReplicatorData();

                var windowsServiceSetting = from r in document.Descendants("Setting")
                                            select new
                                            {
                                                intervalTime = r.Element("IntervalTimeInMinutes")?.Value,
                                                mainWebServerHostingIp = r.Element("MainWebServerHostingIp")?.Value,
                                                downloadingXmlFilePath = r.Element("DownloadingXmlFilePath")?.Value,
                                                downloadStartStopTime = r.Element("DownloadStartStopTime")?.Value,
                                                uploadXmlFilePath = r.Element("UploadXmlFilePath")?.Value
                                            };

                foreach (var setting in windowsServiceSetting)
                {
                    downloadingXmlFilePath = setting.downloadingXmlFilePath.ToString();
                    intervalTimeInMinutes = Convert.ToInt32(setting.intervalTime);
                    mainWebServerHostingIp = setting.mainWebServerHostingIp.ToString();
                    downloadStartStopTime = setting.downloadStartStopTime.ToString();
                    uploadXmlFilePath = setting.uploadXmlFilePath.ToString();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in GetSetting() method. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return;
        }

        public bool IsTimeBetween()
        {
            string[] startStopTime = downloadStartStopTime.Split('-');

            DateTime downloadStartTime = Convert.ToDateTime(startStopTime[0]);
            DateTime downloadStopTime = Convert.ToDateTime(startStopTime[1]);
            downloadStopTimes = Convert.ToDateTime(startStopTime[1]);
            DateTime currentTime = DateTime.Now;
            bool result = false;

            if (downloadStartTime <= downloadStopTime)
            {
                // start and stop times are in the same day
                if (currentTime >= downloadStartTime && currentTime <= downloadStopTime)
                {
                    result = true;
                }
            }
            else
            {
                // start and stop times are in different days
                if (currentTime >= downloadStartTime || currentTime <= downloadStopTime)
                {
                    result = true;
                }
            }
            return result;
        }

		//Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
        public bool IsUploadFQTimeBetween()
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(uploadFQDocFolderStartStopTime))
                {
                    string[] startStopTime = uploadFQDocFolderStartStopTime.Split('-');
                    DateTime downloadStartTime = Convert.ToDateTime(startStopTime[0]);
                    DateTime downloadStopTime = Convert.ToDateTime(startStopTime[1]);
                    downloadStopTimes = Convert.ToDateTime(startStopTime[1]);
                    DateTime currentTime = DateTime.Now;
                    if (downloadStartTime <= downloadStopTime)
                    {
                        // start and stop times are in the same day
                        if (currentTime >= downloadStartTime && currentTime <= downloadStopTime)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        // start and stop times are in different days
                        if (currentTime >= downloadStartTime || currentTime <= downloadStopTime)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public DateTime downloadStopTimes { get; set; }
    }
}
