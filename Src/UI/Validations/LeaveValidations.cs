using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
   public class LeaveValidations : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "SelectedIndexForEmployee")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Employee");
                else if (fieldValue != null && fieldValue.Equals(-1) )
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Employee");
            }

            if ( fieldName == "SelectedLeaveType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
            }
	        /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
            if (fieldName == "SelectedIndexForCompanyShift")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()) || fieldValue.ToString() =="---")
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Shift");
                else if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Shift");
            }

            if (fieldName == "StartTime")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Start Time");
            }
            if (fieldName == "EndTime")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "End Time");
            }
            if (fieldName == "StartDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Start Date");
            }
            if (fieldName == "EndDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "End Date");
            }
         

            return errorMessage;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}

