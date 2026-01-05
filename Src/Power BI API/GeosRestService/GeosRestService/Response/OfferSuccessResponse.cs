using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Entities;

namespace GeosRestService.Response
{
    [DataContract]
    public class OfferSuccessResponse
    {
        bool _success = false;
        string _company = "EMDEP";
        string _terms = "This output has been generated automatically by GEOS. All the containing data is property of EMDEP. All rights reserved.";

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
        [DataMember(Order = 4)]
        public List<Opportunities> Offers { get; set; }
        #endregion
    }
}