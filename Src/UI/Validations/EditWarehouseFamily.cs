using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class EditWarehouseFamily : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, params int[] invalidValues)
        {
            string errorMessage = string.Empty;
            if (fieldName == "SelectedFamilyIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Family");
                else if (fieldValue != null && (fieldValue.Equals(-1) /*|| fieldValue.Equals(0)*/ ))  //[Rahul.Gadhave][GEOS2-9103][Date:16-09-2025]
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Family");
            }
            else if (fieldName == "SelectedArticleType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
            }

            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
            {
                return new ValidationResult(false, error);
            }
            return ValidationResult.ValidResult;
        }
    }
}
