using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class DVMAndTimeTrackingValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public string fieldValue1 { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object fieldValue1, object nullValue = null)
        {
            string errorMessage = string.Empty;
            try
            {

                if (fieldName == "FromDate")
                {
                    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

                    if (fieldValue != null && fieldValue1 != null)
                    {
                        DateTime SelectedFromDate = (DateTime)fieldValue;
                        DateTime SelectedToDate = (DateTime)fieldValue1;

                        if (SelectedFromDate > SelectedToDate)
                        {
                            errorMessage = string.Format("From date should be less than To date.", fieldName);
                        }
                    }

                }

                if (fieldName == "ToDate")
                {
                   // if (fieldValue1 == null || string.IsNullOrEmpty(fieldValue1.ToString().Trim()))
                    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

                    if (fieldValue != null && fieldValue1 != null)
                    {
                        DateTime SelectedToDate = (DateTime)fieldValue;
                        DateTime SelectedFromDate = (DateTime)fieldValue1;

                        if (SelectedFromDate > SelectedToDate)
                        {
                            errorMessage = string.Format("To date should be greater than From date.", fieldName);
                        }
                    }

                }
                return errorMessage;
            }
            catch (Exception ex)
            {
                return errorMessage;
            }
            
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, fieldValue1);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
