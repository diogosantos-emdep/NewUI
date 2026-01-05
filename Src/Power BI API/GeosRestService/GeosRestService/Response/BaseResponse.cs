using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using Entities;

namespace GeosRestService.Response
{
    [KnownType(typeof(ActivityResponse))]
    [DataContract]
    public class BaseResponse
    {
        bool _success = false;
        string _company = "EMDEP";
        string _terms = "This output has been generated automatically by GEOS. All the containing data is property of EMDEP. All rights reserved.";
        //private Error _error;
        
        #region Properties
        [DataMember(Order = 1)]
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }
        [DataMember(Order = 2)]
        public string company
        {
            get { return _company; }
            set { _company = value; }
        }
        [DataMember(Order = 3)]
        public string terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        //[DataMember(Order = 4,EmitDefaultValue =false,IsRequired =true)]
        [DataMember(Order = 4)]
        //[ScriptIgnore]
        public Error error
        {get;set;}
       // [DataMember(Order = 5)]
       //// [ScriptIgnore]
       // public bool IsComplete
       // {
       //     get { return error != null; }
       // }

        #endregion
    }



    public class Error
    {
       
        public string code { get; set; }
        public string info { get; set; }
    }
}