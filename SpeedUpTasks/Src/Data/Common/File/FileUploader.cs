using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.File
{
   [MessageContract]
   public class FileUploader
    {
        [MessageBodyMember(Order = 1)]
        public string FileUploadName;

        [MessageBodyMember(Order = 2)]
        public Byte[] FileByte;

    }
     
}
