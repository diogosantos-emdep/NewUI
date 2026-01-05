using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.FileReplicator.Utility
{
    public class FileSystemWatcher
    {
        #region Declaration
        XmlFileGenerator generateXmlFile = new XmlFileGenerator();
        private List<System.IO.FileSystemWatcher> fileSystemWatchers;

        //[001][skale][2019-28-05][GEOS2-1490] File replicator - Employee image is not copied
        public List<FileReplicatorData> fileDetailsList;
        public List<FileReplicatorData> FileDetailsList
        {
            get { return fileDetailsList; }
            set { fileDetailsList = value; }
        }
        //end
        #endregion

        #region Constructor
        /// <summary>
        ///  [001][skale][2019-28-05][GEOS2-1490] File replicator - Employee image is not copied
        /// </summary>
        public FileSystemWatcher()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                }

                fileSystemWatchers = new List<System.IO.FileSystemWatcher>();

                FileDetailsList = new List<FileReplicatorData>();

                generateXmlFile.DirectoryToWatch();
                generateXmlFile.GetSetting();

                FileSystemWatcherMethod();
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in FileSystemWatcher(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// [001][skale][2019-28-05][GEOS2-1490] File replicator - Employee image is not copied
        /// </summary>
        private void FileSystemWatcherMethod()
        {
            foreach (KeyValuePair<string, string> filePath in generateXmlFile.DiFolderToWatch)
            {
                if (Directory.Exists(filePath.Key))
                {
                    System.IO.FileSystemWatcher fileSystemWatcher = new System.IO.FileSystemWatcher();
                    fileSystemWatcher = new System.IO.FileSystemWatcher();
                    fileSystemWatcher.Path = filePath.Key.ToString();
                    fileSystemWatcher.Filter = "*.*";
                    fileSystemWatcher.IncludeSubdirectories = true;
                    fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
                    fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_created);
                    fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemWatcher_deleted);
                    //[001] added
                    fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
                    //End
                    fileSystemWatcher.Renamed += new RenamedEventHandler(fileSystemWatcher_renamed);
                    fileSystemWatcher.EnableRaisingEvents = true;
                    fileSystemWatchers.Add(fileSystemWatcher);

                    Log4NetLogger.Logger.Log(string.Format("Folder watcher event created - {0}", filePath.Key), category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    Log4NetLogger.Logger.Log(string.Format("Folder does not exists - {0}", filePath.Key), category: Category.Warn, priority: Priority.Low);
                    Log4NetLogger.Logger.Log(string.Format("Folder watcher event not created - {0}", filePath.Key), category: Category.Warn, priority: Priority.Low);
                }
            }
        }
        /// <summary>
        /// this method use for start file watcher 
        /// </summary>
        public void StartFileWatcher()
        {
            try
            {
                foreach (System.IO.FileSystemWatcher tmpFSW in fileSystemWatchers)
                {
                    if (tmpFSW.EnableRaisingEvents != true)
                    {
                        tmpFSW.EnableRaisingEvents = true;
                        string path = tmpFSW.Path;
                        string location = Path.GetFileName(path);

                        Log4NetLogger.Logger.Log(string.Format("-------------------------------------------  File watcher started for {0} -------------------------------------------", location), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in StartFileWatcher(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.High);
            }
        }
        /// <summary>
        /// this method use for stop file watcher 
        /// </summary>
        public void StopFileWatcher()
        {
            try
            {
                foreach (System.IO.FileSystemWatcher fileSystemWatcher in fileSystemWatchers)
                {
                    if (fileSystemWatcher.EnableRaisingEvents != false)
                    {
                        fileSystemWatcher.EnableRaisingEvents = false;

                        string path = fileSystemWatcher.Path;
                        string location = Path.GetFileName(path);

                        Log4NetLogger.Logger.Log(string.Format("-------------------------------------------  File watcher stopped for {0} -------------------------------------------", location), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in StopFileWatcher(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.High);
            }
        }

        #endregion

        #region FileSystemWatcher Class event

        /// <summary>
        /// Invoke when a Create event would be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileSystemWatcher_created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.Name.ToLower() != "thumbs.db")
                {
                    FileReplicatorData FileReplicatorData = new FileReplicatorData();
                    FileReplicatorData.folderWatcherRootPath = (sender as System.IO.FileSystemWatcher).Path;
                    // Log4NetLogger.Logger.Log(string.Format("Created watcher root folder - {0}", FileReplicatorData.folderWatcherRootPath), category: Category.Info, priority: Priority.Low);
                    FileReplicatorData.filefullpath = e.FullPath.ToString();
                    FileReplicatorData.filePath = generateXmlFile.downloadingXmlFilePath.ToString();
                    FileReplicatorData.fileOperation = e.ChangeType.ToString();
                    FileReplicatorData.fileName = generateXmlFile.GetTimestampXMlFileName(DateTime.Now);
                    generateXmlFile.CreateXMLFile(FileReplicatorData);
                    FileDetailsList.Add(FileReplicatorData);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in file System Watcher class create event. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        void fileSystemWatcher_deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!generateXmlFile.DiFolderToWatch.ContainsKey(e.FullPath) && e.Name.ToLower() != "thumbs.db")
                {
                    FileReplicatorData FileReplicatorData = new FileReplicatorData();
                    FileReplicatorData.folderWatcherRootPath = (sender as System.IO.FileSystemWatcher).Path;
                    //  Log4NetLogger.Logger.Log(string.Format("Deleted watcher root folder  - {0}", FileReplicatorData.folderWatcherRootPath), category: Category.Info, priority: Priority.Low);
                    FileReplicatorData.filefullpath = e.FullPath.ToString();
                    FileReplicatorData.filePath = generateXmlFile.downloadingXmlFilePath;
                    FileReplicatorData.fileOperation = e.ChangeType.ToString();
                    FileReplicatorData.fileName = generateXmlFile.GetTimestampXMlFileName(DateTime.Now);
                    generateXmlFile.CreateXMLFile(FileReplicatorData);

                    var findObj = FileDetailsList.Where(x => x.filefullpath.Contains(e.FullPath.ToString())).ToList();
                    foreach (var removeObj in findObj)
                    {
                        FileDetailsList.Remove(removeObj);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in file System Watcher delete event. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        void fileSystemWatcher_renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                if (!generateXmlFile.DiFolderToWatch.ContainsKey(e.FullPath) && e.Name.ToLower() != "thumbs.db")
                {
                    FileReplicatorData FileReplicatorData = new FileReplicatorData();
                    FileReplicatorData.folderWatcherRootPath = (sender as System.IO.FileSystemWatcher).Path;
                    //  Log4NetLogger.Logger.Log(string.Format("Renamed watcher root folder  - {0}", FileReplicatorData.folderWatcherRootPath), category: Category.Info, priority: Priority.Low);
                    FileReplicatorData.filefullpath = e.FullPath.ToString();
                    FileReplicatorData.filePath = generateXmlFile.downloadingXmlFilePath;
                    FileReplicatorData.fileOperation = e.ChangeType.ToString();
                    FileReplicatorData.oldFilePath = e.OldFullPath.ToString();
                    FileReplicatorData.fileName = generateXmlFile.GetTimestampXMlFileName(DateTime.Now);
                    generateXmlFile.CreateXMLFile(FileReplicatorData);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in file System Watcher rename event. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this event use for changed 
        /// [000][skale][2019-28-05][GEOS2-1490] File replicator - Employee image is not copied
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                if (e.Name.ToLower() != "thumbs.db")
                {
                    var oldFileDetails = FileDetailsList.Where(x => x.filefullpath == e.FullPath.ToString()).ToList();

                    if (oldFileDetails.Count > 0)
                        return;
                    else
                    {
                        FileReplicatorData FileReplicatorData = new FileReplicatorData();
                        FileReplicatorData.folderWatcherRootPath = (source as System.IO.FileSystemWatcher).Path;
                        FileReplicatorData.filefullpath = e.FullPath.ToString();
                        FileReplicatorData.filePath = generateXmlFile.downloadingXmlFilePath.ToString();
                        FileReplicatorData.fileOperation = e.ChangeType.ToString();
                        FileReplicatorData.fileName = generateXmlFile.GetTimestampXMlFileName(DateTime.Now);
                        generateXmlFile.CreateXMLFile(FileReplicatorData);
                        FileDetailsList.Add(FileReplicatorData);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in file System Watcher OnChanged  event. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

    }
}
