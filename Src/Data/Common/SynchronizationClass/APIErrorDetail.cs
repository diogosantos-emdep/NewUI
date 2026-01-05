using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SynchronizationClass
{
    public class APIErrorDetailForErrorFalse
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "code")]
        public String Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

    }

    public class APIErrorDetail
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "code")]
        public String Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public Message Message { get; set; }

    }

    public class Message
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public String _Message { get; set; }

        [JsonProperty(PropertyName = "errorObject")]
        public ErrorObject ErrorObject { get; set; }

    }

    public class SuccessMessage
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public String _Message { get; set; }

        [JsonProperty(PropertyName = "errorObject")]
        public ErrorObject ErrorObject { get; set; }

    }
    public class ErrorObject
    {


    }
}
