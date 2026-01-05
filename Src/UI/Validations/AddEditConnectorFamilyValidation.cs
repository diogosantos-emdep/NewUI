using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditConnectorFamilyValidation : ValidationRule
    {

        public string FieldName { get; set; }
        public List<string> ConnectorName { get; set; }
        public static string GetErrorMessage(string fieldName, List<string> ConnectorName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "ImageName") //image Name validation
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "File");
            }
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (ConnectorName != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    if (ConnectorName.Any(name => name.ToUpper() == fieldValue.ToString().ToUpper()))
                    {
                        errorMessage = string.Format("Name is already exist.", fieldName);
                    }                   
                }                
            }
            else if (fieldName == "SelectedConnectorType")//[rdixit][GEOS2-5148,5149,5150][29.01.2024]
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the Type field empty.");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the Type field empty.");
            }
            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, ConnectorName,value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
