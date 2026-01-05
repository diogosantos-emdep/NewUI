using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.FileReplicator
{
    [MessageContract]
    public class FileDetail
    {
        [MessageBodyMember(Order = 1)]
        public string FileName;

        [MessageBodyMember(Order = 2)]
        public Byte[] FileByte;

        [MessageBodyMember(Order = 3)]
        public string FilePath;

        [MessageBodyMember(Order = 4)]
        public string fileOldName;

        [MessageBodyMember(Order = 5)]
        public string uploadinfFilePath;
    }
}
