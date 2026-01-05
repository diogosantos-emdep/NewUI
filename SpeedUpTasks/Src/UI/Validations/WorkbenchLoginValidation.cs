using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class WorkbenchLoginValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "ProductionUserLoginCode")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("Please enter the Operator Id.");

                //else if (Convert.ToString(fieldValue).Length < 14)
                //    errorMessage = string.Format("Operator Id must contain at least fourteen characters !");
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
