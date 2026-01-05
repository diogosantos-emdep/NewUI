using System;
using System.Globalization;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class DeliveryDateOfOTsValidation : ValidationRule
    {
        #region Task Log
        //[pramod.misal][GEOS2-5094][19-12-2023] 
        //|Display a new popup informing the user to change the delivery dates of the OTS with “Todo” Status in the list|
        #endregion

        public string FieldName { get; set; }


        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

           

            if (fieldName == "DeliveryDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Delivery Date");
                }                  
                else
                {
                    DateTime deliveryDate;
                    if (DateTime.TryParse(fieldValue.ToString(), out deliveryDate))
                    {
                       
                        if (deliveryDate >= DateTime.Now)
                        {
                           
                        }
                        else
                        {
                            errorMessage = string.Format("Please specify a Delivery date greater than the current date for the {0} field.", "Delivery Date");
                        }
                    }
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
