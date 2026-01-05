using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Emdep.Geos.CurrencyConvertApi.Models
{

    public class ConversionModel :BaseModel
    {

        [JsonProperty("terms")]
        public string Terms { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("result")]
        public double Result { get; set; }
    }

    public class Info
    {

        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("quote")]
        public double Quote { get; set; }
    }

    public class Query
    {

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

}
