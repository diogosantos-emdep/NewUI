using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Emdep.Geos.UI.Common;
using System;

namespace Emdep.Geos.UI.Validations
{
    public class SplitOtValidationRule : ValidationRule
   
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                    

            else if (fieldName == "OfferAmountSplit")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                else if (Convert.ToDouble(fieldValue) == 0 && Convert.ToInt16(nullValue) != 2)
                    errorMessage = string.Format("{0} must be greater than zero.", fieldName);
            }

            else if (fieldName == "OfferCloseDateSplit")
            {
                // If GeosStatus(nullValue) is "17-LOST" or "4-Cancelled" then do not apply validation on date else apply.
                if ((fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    && (Convert.ToInt16(nullValue) != 17 && Convert.ToInt16(nullValue) != 4))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
                else if ((Convert.ToInt16(nullValue) != 17 && Convert.ToInt16(nullValue) != 4)
                          && Convert.ToDateTime(fieldValue).Date < GeosApplication.Instance.ServerDateTime.Date)
                {
                    errorMessage = string.Format("{0} must be greater than current date.", fieldName);
                }
            }

            else if (fieldName == "QuoteSentDateSplit")
            {
                // Offer 2 is for Status Only Quoted
                if (Convert.ToInt16(nullValue) == 1 && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString())))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "RFQReceptionDateSplit")
            {
                // 2 is for Status Waiting for Quote
                if (Convert.ToInt16(nullValue) == 2 && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString())))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "ProductAndServicesSplitCount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Product and Services");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Product and Services");
            }
            else if (fieldName == "IsSplitOfferStatusDisabled")
            {
                if (fieldValue.Equals(true))
                    errorMessage = string.Format("{0} not allow to select.", "Status");
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
