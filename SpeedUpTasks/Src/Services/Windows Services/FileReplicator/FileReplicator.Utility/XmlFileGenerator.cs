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
                    FileReplicatorData xmlFileData = new FileReplicatorData();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(file);
                    var itemNodes = xmlDoc.SelectNodes("GEOSFileManager");

                    foreach (XmlNode node in itemNodes)
                    {
                        xmlFileData.fileOperation = node.SelectSingleNode("OperationType")?.InnerText;
                        xmlFileData.filePath = node.SelectSingleNode("FilePath")?.InnerText;
                        xmlFileData.oldFilePath = node.SelectSingleNode("FileOldPath")?.InnerText;
                        xmlFileData.folderWatcherRootPath = node.SelectSingleNode("FolderWatcherRootPath")?.InnerText;
                        xmlFileData.xmlFilePath = file.ToString();
                        XmlDetailslist.Add(xmlFileData);
                    }
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
                var file = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "setting.xml");
                XDocument document = XDocument.Load(file);
                FileReplicatorData FileWatcherPath = new FileReplicatorData();

                var FolderToWatch = from r in document.Descendants("FolderToWatch")
                                    select new
                                    {
                                        localfilepath = r.Element("LocalServerPath")?.Value,
                                        serverFilePath = r.Element("MainServerPath")?.Value,
                                    };

                foreach (var fileWatcherPath in FolderToWatch)
                {
                    DiFolderToWatch.Add(fileWatcherPath.localfilepath, fileWatcherPath.serverFilePath);
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
                                                downloadStartStopTime = r.Element("DownloadStartStopTime")?.Value
                                            };

                foreach (var setting in windowsServiceSetting)
                {
                    downloadingXmlFilePath = setting.downloadingXmlFilePath.ToString();
                    intervalTimeInMinutes = Convert.ToInt32(setting.intervalTime);
                    mainWebServerHostingIp = setting.mainWebServerHostingIp.ToString();
                    downloadStartStopTime = setting.downloadStartStopTime.ToString();
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

        public DateTime downloadStopTimes { get; set; }
    }
}
