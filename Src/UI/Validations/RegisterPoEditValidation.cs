using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class RegisterPoEditValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "SelectedIndexPoType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
            }

            else if (fieldName == "SelectedIndexCustomerGroup")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer Group");
                else if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer Group");
            }

            else if (fieldName == "SelectedIndexCompanyPlant")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer Plant");
                else if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer Plant");
            }

            else if (fieldName == "Code")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Number");
            }
            else if (fieldName == "Remarks")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Remark");
            }
            //[pramod.misal][GEOS2-9173][05-09-2025]https://helpdesk.emdep.com/browse/GEOS2-9173
            else if (fieldName == "Amount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()) || Convert.ToDecimal(fieldValue) == 0)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Value");
                }
                else
                {
                    decimal value = Convert.ToDecimal(fieldValue);
                    if (value < 0 || value > 999999999)
                    {
                        errorMessage = "Value must be between 0.0 and 999,999,999.00";
                    }

                }
                   

            }
            else if (fieldName == "ReceptionDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Reception Date");
            }
            else if (fieldName == "ReceptionDateNew")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Reception Date");
            }
            else if (fieldName == "SelectedIndexSender")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sender");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sender");
                //else if (fieldValue != null && fieldValue.Equals(0))
                //    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sender");
            }

            else if (fieldName == "SelectedIndexShipTo")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Ship To");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Ship To");
                //else if (fieldValue != null && fieldValue.Equals(0))
                //    errorMessage = string.Format("You cannot leave the {0} field empty.", "Ship To");
            }

            else if (fieldName == "Amount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Amount");
               
            }

            else if (fieldName == "SelectedIndexCurrency")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Currency");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Currency");
                //else if (fieldValue != null && fieldValue.Equals(0))
                //    errorMessage = string.Format("You cannot leave the {0} field empty.", "Currency");
            }
            else if (fieldName == "RegisterPOAttachmentSavedFileName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Attachment");
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
