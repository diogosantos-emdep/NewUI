using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Entities;
namespace GeosRestService.Response
{
    [DataContract]
    public class OrderResponse:BaseResponse
    {
        
        [DataMember(Order = 4)]
        public List<Order> Orders { get; set; }
    }
}