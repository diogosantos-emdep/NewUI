using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditCPLCustomerValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;


            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);


            if (fieldName == "SelectedRegion")
            {
                string value = string.Empty;
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)) && ((System.Collections.Generic.List<object>)fieldValue).Count > 0)
                {
                    errorMessage = string.Empty;
                }
                else if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)) && ((System.Collections.Generic.List<object>)fieldValue).Count == 0)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Region");
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Region");
                }
            }
            if (fieldName == "Reference")
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
