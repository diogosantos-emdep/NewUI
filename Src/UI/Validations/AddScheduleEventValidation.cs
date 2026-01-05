using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddScheduleEventValidation : ValidationRule
    {
        //[nsatpute][30.09.2025][GEOS2-9210]
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "SelectedIndexForEventType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
            }
            if (fieldName == "SelectedIndexForLogistic")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
            }
            if (fieldName == "SelectedIndexOwner")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
            }
            else if (fieldName == "Description" || fieldName == "ActivityAddress" || fieldName == "Subject")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "SelectedIndexType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
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
