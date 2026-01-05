using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class PORequestsValidation : ValidationRule
    {
        // [nsatpute][01-12-2024][GEOS2-6462]
        public string FieldName { get; set; }

        public string SecondFieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (fieldName == "ReceptionDateTo" || fieldName == "CreationDateTo" || fieldName == "UpdateDateTo" || fieldName == "UpdateDateTo" || fieldName == "CancellationDateTo")
            {
                errorMessage = string.Format("To date must be greater than From Date");
            }
            if (fieldName == "PoValueRangeTo")
            {
                errorMessage = string.Format("To value must be greater than From value");
            }
            return errorMessage;
        }

        public static string GetEmptyErrorMessage(string fieldName)
        {
            return "You cannot leave the field empty.";
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidationResult.ValidResult;
        }
    }
}
