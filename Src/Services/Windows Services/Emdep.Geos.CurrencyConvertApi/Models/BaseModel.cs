using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.CurrencyConvertApi.Models
{
    public class BaseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }
    public class Error : BaseModel
    {

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }
    }
}
