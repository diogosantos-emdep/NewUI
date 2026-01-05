using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Entities;
namespace GeosRestService.Response
{
    [DataContract]
    public class CustomerResponse:BaseResponse
    {
        [DataMember(Order = 4)]
        public List<Customer> Customers { get; set; }
    }
}