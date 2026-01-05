using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[Sudhir.jangra][GEOS2-5579]
    public class AddEditEmployeeEquipmentValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, bool modelValidation = false, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (fieldName == "SelectedEquipment")
            {
                if (fieldValue == null || fieldValue.ToString().Trim() == string.Empty)
                    errorMessage = string.Format("You cannot leave the 'Equipment' empty.");
            }
            else if (fieldName == "EndDate")
            {
                if (fieldValue == null || fieldValue.ToString().Trim() == string.Empty)
                    errorMessage = string.Format("You cannot leave the 'Expiry Date' empty.");
            }
            else if (fieldName == "AttachmentObjectList")
            {
                if (fieldValue == null || fieldValue.ToString().Trim() == string.Empty)
                    errorMessage = string.Format("You cannot leave the 'Attachment' empty.");
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
