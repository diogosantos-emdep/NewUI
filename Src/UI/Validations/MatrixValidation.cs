using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class MatrixValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue,
            string displayFieldNameInErrorMessage, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", displayFieldNameInErrorMessage);

            List<string> mandatorytextFieldsList = new List<string>
            {
                "Name","Description","Url"
            };

            if (mandatorytextFieldsList.Contains(fieldName))
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", displayFieldNameInErrorMessage);
            }

            List<string> mandatorySelectionFieldsList = new List<string>
            {
                "SelectedIndexRegionLookupValue","SelectedIndexCustomer","SelectedIndexProductCategory"
            };

            if (mandatorySelectionFieldsList.Contains(fieldName))
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", displayFieldNameInErrorMessage);
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", displayFieldNameInErrorMessage);
            }
            
            return errorMessage;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, FieldName);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
