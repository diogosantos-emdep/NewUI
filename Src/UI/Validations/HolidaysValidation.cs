using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class HolidaysValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "HolidayName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Holiday Name");
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
            if (fieldName == "SelectedIndexHolidayType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Holiday Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Holiday Type");
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
