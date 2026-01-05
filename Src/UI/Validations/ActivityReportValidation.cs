using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Validations
{
    public class ActivityReportValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "SelectedIndexBusinessUnit")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Unit");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Unit");
            }

            if (fieldName == "SelectedIndexAccount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Account");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Account");
            }

            else if (fieldName == "SelectedUserManagerDtl")
            {
                if (GeosApplication.Instance.IdUserPermission != 20)
                {
                    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Sales Person");

                    else if (fieldValue != null && ((List<object>)fieldValue).Count == 0)
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Sales Person");

                }
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
