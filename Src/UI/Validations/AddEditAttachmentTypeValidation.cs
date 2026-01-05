using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditAttachmentTypeValidation : ValidationRule
    {
        public string FieldName { get; set; }

        //[rdixit][GEOS2-4074][12.12.2022]
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "SelectedAttachmentType")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(LookupValue)))
                {
                    value = ((LookupValue)fieldValue).Value.ToString().Trim();

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

            else if (fieldName == "SelectedOptionWayDetectionSparePartFile")   //[rdixit][GEOS2-4074][12.12.2022]
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(DetectionAttachedDoc)))
                {
                    if (((DetectionAttachedDoc)fieldValue).SavedFileName != null)
                    {
                        value = ((DetectionAttachedDoc)fieldValue).SavedFileName.ToString().Trim();

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
            if (fieldName == "ModuleSelectedType")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(LookupValue)))
                {
                    value = ((LookupValue)fieldValue).Value.ToString().Trim();

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
