using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class OpportunityCreateModel
    {
        bool _success = false;
        string _company = "EMDEP";
        string _terms = "This output has been generated automatically by GEOS. All the containing data is property of EMDEP. All rights reserved.";

        #region Properties
        [Display(Order = 1)]
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }
        [Display(Order = 2)]
        public string company
        {
            get { return _company; }
            set { _company = value; }
        }
        [Display(Order = 3)]
        public string terms
        {
            get { return _terms; }
            set { _terms = value; }
        }
        // Added By Rahul[RGadhave] for APIGEOS-872
        [Display(Order = 4)]
        public ErrorMessage warning { get; set; }
        [Display(Order = 5)]
        public OfferCreatedModel Offer { get; set; }

        #endregion
    }
}
