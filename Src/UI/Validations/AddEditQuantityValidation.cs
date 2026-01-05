using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditQuantityValidation : ValidationRule
    {

        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
           
            //Quantity
            if (fieldName == "Quantity")
            {
                if (fieldValue.ToString().Equals("0") || fieldValue == null)
                {
                    errorMessage = string.Format("Please enter a {0} greater than zero.","Quantity");
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
