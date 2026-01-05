using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.PLM;
using System.Globalization;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditCustompropertyValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public List<string> ConnectorNameList { get; set; }

        public static string GetErrorMessage(string fieldName, List<string> ConnectorNameList, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;


            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (ConnectorNameList != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    foreach (string name in ConnectorNameList)
                    {
                        if (fieldValue.ToString().ToUpper() == name.ToUpper())
                        {
                            errorMessage = string.Format("Name is already exist.", fieldName);
                        }
                    }
                }
            }
            else if (fieldName == "InformationError")
            {
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the information empty.");
            }
            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, ConnectorNameList, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }

    }
}