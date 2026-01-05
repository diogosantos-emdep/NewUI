using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Emdep.Geos.Data.Common.File
{

    [DataContract]
    public class FileMetaData
    {
        public FileMetaData(
        string localFileName,
        string remoteFileName,long fileSize)
        {
            this.LocalFileName = localFileName;
            this.RemoteFileName = remoteFileName;
            this.FileSize = fileSize;
        }

        [DataMember]
        public string LocalFileName;
        [DataMember]
        public string RemoteFileName;
        [DataMember]
        public long FileSize;
    }
}