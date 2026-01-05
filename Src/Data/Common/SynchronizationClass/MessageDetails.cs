using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SynchronizationClass
{
    public class MessageDetails
    {
        public List<APIErrorDetailForErrorFalse> APIErrorDetailForErrorFalse { get; set; }
        public APIErrorDetail APIErrorDetail { get; set; }
        public Message Message { get; set; }
        public List<SuccessMessage> SuccessMessage { get; set; }
        public List<ErrorDetails> ErrorDetails { get; set; }
        public List<ErrorDetails> SuccessDetails { get; set; }
    }
}
