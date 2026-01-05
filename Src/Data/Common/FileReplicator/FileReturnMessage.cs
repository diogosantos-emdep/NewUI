using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.FileReplicator
{
    [MessageContract(IsWrapped = true)]
   public class FileReturnMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public Boolean IsFileActionPerformed;

        [MessageHeader(MustUnderstand = true)]
        public string  returnFileUploadingPath;
    }
}
