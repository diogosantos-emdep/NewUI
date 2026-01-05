using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class ProductionTimetracking
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
        public ErrorMessage error { get; set; }
        //[pjadhav][APIGEOS-698][27/01/2023]
        [Display(Order = 5)]
        public List<TimetrackingAPI> TimeTracking { get; set; }

       
        #endregion
    }
}