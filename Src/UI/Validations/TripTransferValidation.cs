using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    /// <summary>
    /// //[pramod.misal][GEOS2-7989][17-09-2025]https://helpdesk.emdep.com/browse/GEOS2-7989
    /// </summary>
    public class TripTransferValidation : ValidationRule
    {   
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "SelectedOrigin")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Origin");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Origin");
            }

            else if (fieldName == "Selecteddestination")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Destination");
                else if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Destination");
            }

            else if (fieldName == "TransferDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Date");
            }

            else if (fieldName == "TransferTime")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Time");
            }
            else if (fieldName == "SelectedTransport")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Transport");
            }
            else if (fieldName == "SelectedProvider")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Provider");
            }
            else if (fieldName == "Contactperson")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contact Person");
            }
            else if (fieldName == "ContactNumber")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contact Number");
            }
            else if (fieldName == "EstimatedDuration")
            {

                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Estimated Duration");

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
