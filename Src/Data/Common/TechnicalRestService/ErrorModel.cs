using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class ErrorModel
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

        [Display(Order = 4)]
        public Error error { get; set; }



        #endregion

    }

    public class Error
    {
        [Display(Order = 1)]
        public string code { get; set; }
        [Display(Order = 2)]
        public string info { get; set; }
    }


    public class ErrorModel_Validate
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

        [Display(Order = 4)]
        public Error error { get; set; }

        [Display(Order = 5)]
        public string JsonData { get; set; }

        #endregion

    }

    public class ErrorModel_Close
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

        [Display(Order = 4)]
        public Error error { get; set; }

        [Display(Order = 5)]
        public string JsonData { get; set; }

        #endregion

    }

}
