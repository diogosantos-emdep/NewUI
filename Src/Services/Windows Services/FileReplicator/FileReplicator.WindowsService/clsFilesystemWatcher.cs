using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReplicator.WindowsService
{
    class clsFilesystemWatcher
    {
        FileSystemWatcher filewatcher = new FileSystemWatcher();
        //public void MonitorFolder()
        //{
        //    filewatcher.Path = Properties.Settings.Default.filepath;
        //    filewatcher.Filter = "*.*";
        //    filewatcher.EnableRaisingEvents = true;
        //    filewatcher.IncludeSubdirectories = true;
        //    filewatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
        //    | NotifyFilters.FileName;
        //}
    }
}
