using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.FileReplicator.Utility
{
    public class FileReplicatorData
    { 
        public string xmlFileName;
        public string xmlFilePath;
        public string filePath;
        public string fileOperation;
        public string filefullpath;
        public string fileName;
        public string oldFilePath;
        public string folderWatcherRootPath;
        public string fileExtensions;
        public string synchDirection;
        public string localServerPath;
        public string mainServerPath;
        //Shubham[skadam] GEOS2-8460 Upload missing documents of FQ certificates from local plant to EBDC 17 06 2025
        public string fQDocFromDate;
        public string fQDocToDate;
        public string fQDocFolder;
    }
}
