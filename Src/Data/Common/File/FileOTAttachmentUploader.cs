using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.File
{
    [MessageContract]
    public class FileOTAttachmentUploader
    {
        [MessageBodyMember(Order = 1)]
        public string FileUploadName;

        [MessageBodyMember(Order = 2)]
        public Byte[] FileByte;

        [MessageBodyMember(Order = 3)]
        public string Year;

        [MessageBodyMember(Order = 4)]
        public string QuotationCode;

    }

}
