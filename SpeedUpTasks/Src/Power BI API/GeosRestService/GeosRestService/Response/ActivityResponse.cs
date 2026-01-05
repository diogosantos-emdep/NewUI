using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Entities;
namespace GeosRestService.Response
{
    
    [DataContract]
    public class ActivityResponse:BaseResponse
    {
        

        #region Properties
        
        [DataMember(Order = 4)]
        public List<Activity> Activities { get; set; }
        #endregion
    }
}