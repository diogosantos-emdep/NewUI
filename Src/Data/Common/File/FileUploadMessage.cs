using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Emdep.Geos.Data.Common.File
{

    [MessageContract]
    public class FileUploadMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public FileMetaData Metadata;
        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;
       
    }
}