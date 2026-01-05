using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Emdep.Geos.Data.Common.File
{
    [MessageContract(IsWrapped = true)]
    public class FileUploadReturnMessage 
    {
        [MessageHeader(MustUnderstand = true)]
        public Boolean IsFileUpload;
    }
}
