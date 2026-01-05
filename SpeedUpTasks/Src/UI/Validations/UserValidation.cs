using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class UserValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "FirstName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            if (fieldName == "SelectedPermission")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Permission");

                else if (fieldValue != null && ((List<object>)fieldValue).Count == 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Permission");
            }

            else if (fieldName == "SelectedIndexCompanyPlant")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
            }

            else if (fieldName == "SelectedIndexSalesUnit")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sales Unit");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sales Unit");

            }

            else if (fieldName == "SelectedAuthorizedPlant")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Authorized Plant");

                else if (fieldValue != null && ((List<object>)fieldValue).Count == 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Authorized Plant");
            }

            else if (fieldName == "SelectedUser")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "User");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "User");
            }

            else if (fieldName == "SalesQuotaAmountWithExchangeRate")
            {
                double val=(double)fieldValue;
                if (fieldValue == null || val==0)
                    errorMessage = string.Format("Currency Exchange Rate should not be zero ");              
            }
            else if (fieldName == "TargetAmountWithExchangeRate")
            {
                decimal val = (decimal)fieldValue;
                if (fieldValue == null || val < 0)
                     errorMessage = string.Format("Currency Exchange Rate should not be less than zero ");
                   
            }

            //else if(fieldName== "EditActivityError")
            //{
            //    errorMessage = string.Empty;
            //}


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
