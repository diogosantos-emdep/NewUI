using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Contracts
{
    [DataContract]
    public class ServiceException
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string ErrorDetails { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string ErrorTest { get; set; }
        //[DataMember]
        //public Prism.Logging.ILoggerFacade Logger { get; set; }

        //public void AddLog(string message, Category category, Priority priority)
        //{
        //    Logger.Log(message, category, priority);
        //}
    }
}
