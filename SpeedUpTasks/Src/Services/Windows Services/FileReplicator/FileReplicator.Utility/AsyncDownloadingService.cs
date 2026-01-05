using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Prism.Logging;
using System.Xml;
using System.Reflection;
using log4net.Config;
using Emdep.Geos.FileReplicator.Utility;
using System.Xml.Linq;

namespace Emdep.Geos.FileReplicator.Utility
{
    /// <summary>
    /// Use for Server to Local file synchronization
    /// </summary>
    public class AsyncDownloadingService
    {
        CancellationTokenSource cancelToken;
        XmlFileGenerator _generateXmlFileObj = new XmlFileGenerator();
        bool isBusyFileReplicator = false;

        /// <summary>
        /// Public default constructor 
        /// </summary>
        public AsyncDownloadingService()
        {

        }

        #region Sync file Server to Local 

        /// <summary>
        /// Dictionary of Source and Destination locations
        /// </summary>
        public Dictionary<string, string> SourceAndDestinationDirectories = new Dictionary<string, string>();

        /// <summary>
        /// Get all source and destination location in dictionary
        /// </summary>
        public void GetSourceAndDestinationDirectories()
        {
            try
            {
                SourceAndDestinationDirectories = new Dictionary<string, string>();
                var file = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "setting.xml");
                XDocument document = XDocument.Load(file);

                var SourceAndDestinationFolder = from r in document.Descendants("FolderToWatch")
                                                 select new
                                                 {
                                                     localfilepath = r.Element("LocalServerPath")?.Value,
                                                     serverFilePath = r.Element("MainServerPath")?.Value,
                                                 };

                foreach (var filePath in SourceAndDestinationFolder)
                {
                    SourceAndDestinationDirectories.Add(filePath.localfilepath, filePath.serverFilePath);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in GetSourceAndDestinationDirectories(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return;
        }

        /// <summary>
        /// Start async download tasks 
        /// </summary>
        async public void AsyncStartDownloading()
        {
            cancelToken = new CancellationTokenSource();
            CancellationToken ct = cancelToken.Token;

            try
            {
                Log4NetLogger.Logger.Log(string.Format("-------------------------------------------  Asynchronization Downloading Started -------------------------------------------", "AsyncStartDownloading"), category: Category.Info, priority: Priority.Low);

                if (!isBusyFileReplicator)
                {
                    GetSourceAndDestinationDirectories();

                    foreach (KeyValuePair<string, string> entry in SourceAndDestinationDirectories)
                    {
                        string mainServer = entry.Value;    //  Folder form Main server
                        string localServer = entry.Key;     // Folder form local server

                        if (_generateXmlFileObj.IsTimeBetween())
                        {
                            await SynchronizeDirectoryAsync(cancelToken.Token, mainServer, localServer);
                        }
                        else
                        {
                            if (cancelToken != null)
                            {
                                cancelToken.Cancel();
                            }
                        }
                    }
                    isBusyFileReplicator = false;
                }
            }
            catch (OperationCanceledException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in AsyncStartDownloading() - ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in AsyncStartDownloading() - ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                cancelToken.Cancel();
                cancelToken.Dispose();
            }

        }

        private void SynchronizeDirectoryAsync(string sourceDirectory, string targetDirectory)
        {
            SynchronizeDirectory(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
        }

        /// <summary>
        /// Task for each pair of source and destination Locations 
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        async private Task<string> SynchronizeDirectoryAsync(CancellationToken token, string sourceDirectory, string targetDirectory)
        {
            var task = Task.Run(() =>
            {
                if (_generateXmlFileObj.IsTimeBetween() == false)
                {
                    if (cancelToken != null)
                    {
                        cancelToken.Cancel();
                    }
                }
                else
                {
                    Log4NetLogger.Logger.Log(string.Format("Synchronization started directory - Main server {0} and Local server {1}", sourceDirectory, targetDirectory), category: Category.Info, priority: Priority.Low);

                    SynchronizeDirectory(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
                }
                return "";

            }, token);

            return await task;
        }

        /// <summary>
        /// Synchronize locations recursively  
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void SynchronizeDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName == target.FullName || !source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Sync files. We want to keep track of files in source we’ve processed,
            // and keep a list of target files we have not seen in the process.
            var targetFilesToDelete = target.GetFiles().Select(fi => fi.Name).ToHashSet();

            foreach (FileInfo sourceFile in source.GetFiles())
            {

                var targetFile = new FileInfo(Path.Combine(target.FullName, sourceFile.Name));

                if (!targetFile.Exists)
                {
                    try
                    {
                        if (_generateXmlFileObj.IsTimeBetween())
                        {
                            if (sourceFile.Name.ToLower() != "thumbs.db")
                            {
                                sourceFile.CopyTo(targetFile.FullName);  //  Create new file or folder
                                Log4NetLogger.Logger.Log(string.Format("New file created on local - {0}", targetFile.FullName), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error SynchronizeDirectory() - copy main to local - ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                    }
                }
                else if (sourceFile.LastWriteTime > targetFile.LastWriteTime)
                {
                    try
                    {
                        if (_generateXmlFileObj.IsTimeBetween())
                        {
                            if (sourceFile.Name.ToLower() != "thumbs.db")
                            {

                                if (FileIsBusy(targetFile.FullName))
                                {
                                    Log4NetLogger.Logger.Log(string.Format("The process cannot access the file - {0}  because it is being used by another process", targetFile.FullName), category: Category.Warn, priority: Priority.Low);
                                }
                                else
                                {
                                    sourceFile.CopyTo(targetFile.FullName, true);  //  Update existing file 
                                    Log4NetLogger.Logger.Log(string.Format("Existing directory/file modified on local - {0}", targetFile.FullName), category: Category.Info, priority: Priority.Low);
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error SynchronizeDirectory() - copy main to local - ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                    }
                }
            }

            // We have comment to delete file and folder because we dont want to provide delete file from local functionality to service
            // because we can lost file if new file created on local and does not uploaded so we remove this functionality  
            foreach (var file in targetFilesToDelete)
            {
                if (_generateXmlFileObj.IsTimeBetween())
                {
                    //File.Delete(Path.Combine(target.FullName, file)); //  Delete file for target folder which is not available in source 
                    //Log4NetLogger.Logger.Log(string.Format("File deleted on local - {0}", target.FullName), category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    return;
                }
            }

            // Sync folders.
            var targetSubDirsToDelete = target.GetDirectories().Select(fi => fi.Name).ToHashSet();
            foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
            {
                if (targetSubDirsToDelete.Contains(sourceSubDir.Name)) targetSubDirsToDelete.Remove(sourceSubDir.Name);
                DirectoryInfo targetSubDir = new DirectoryInfo(Path.Combine(target.FullName, sourceSubDir.Name));
                SynchronizeDirectory(sourceSubDir, targetSubDir);       //Recursive call for sub directory 
            }

            foreach (var subdir in targetSubDirsToDelete)
            {
                if (_generateXmlFileObj.IsTimeBetween())
                {

                    if (target.Name.ToLower() != "thumbs.db")
                    {
                        Directory.Delete(Path.Combine(target.FullName, subdir), true);
                        Log4NetLogger.Logger.Log(string.Format("File deleted on local - {0}", Path.Combine(target.FullName, subdir)), category: Category.Info, priority: Priority.Low);
                    }
                    //Directory.Delete(Path.Combine(target.FullName, subdir), true);
                    //Log4NetLogger.Logger.Log(string.Format("File deleted on local - {0}",Path.Combine(target.FullName, subdir)), category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// This method use for check file is use in another process 
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public bool FileIsBusy(string fileFullName)
        {
            bool isBusy = false;
            System.IO.FileStream fs;
            try
            {
                fs = System.IO.File.Open(fileFullName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch (System.IO.IOException ex)
            {
                isBusy = true;
            }
            return isBusy;
        }

        #endregion
    }

    /// <summary>
    /// Extension class for convert into Hashset and check file open or close
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method for convert into Hashset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }

    }
}
