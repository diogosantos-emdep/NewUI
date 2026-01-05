using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditTrainingValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
       {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "InformationError")
            {
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the information empty.");
            }


            else if (fieldName == "SelectedResponsible")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(Employee)))
                {
                    value = ((Employee)fieldValue).FullName.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Responsible");
                    
                }
            }
            else if (fieldName == "ExternalEntity")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Entity");
            }
            else if (fieldName == "ExternalTrainer")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "External Trainer");
            }
            else if(fieldName == "SelectedProfTrainingFile")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "File");
            }
            else if (fieldName == "FileName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
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
