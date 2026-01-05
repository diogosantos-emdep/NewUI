using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Entities;
namespace GeosRestService.Response
{
    [DataContract]
    public class CurrencyResponse:BaseResponse
    {
        [DataMember(Order = 4)]
        public List<Currency> Currencies { get; set; }
    }
}