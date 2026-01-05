using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    //[pramod.misal][GEOS2-][15.05.2024]
    public class AddEditConnectorAttachmentTypeValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            //file
            if (fieldName == "SelectedConnectorFile")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(ConnectorAttachements)))
                {
                    if (((ConnectorAttachements)fieldValue).SavedFileName != null)
                    {
                        value = ((ConnectorAttachements)fieldValue).SavedFileName.ToString().Trim();

                        if (fieldValue == null || string.IsNullOrEmpty(value))
                            errorMessage = string.Format("You cannot leave the File field empty.");
                        else
                            errorMessage = string.Empty;
                    }
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the File field empty.");
                }
            }

            // Company
            if (fieldName == "SelectedCompany")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(Company)))
                {
                    value = ((Company)fieldValue).Name.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer Type");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer Type");
                }
            }
            // Document Type 
            if (fieldName == "SelectedAttachmentType")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(ConnectorAttachements)))
                {
                    value = ((ConnectorAttachements)fieldValue).DocumentType.Name.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Document Type");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Document Type");
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
