using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class EngineeringAnalysisValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "Comments")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "DueDate")
            {
                if ((fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString())))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "SelectedEngineeringAnalysisTypes")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the Type field empty.", fieldName);
            }

            //else if (fieldName == "SelectedIndexAttachment")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Attachment");
            //    else if (fieldValue != null && fieldValue.Equals(-1))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Attachment");
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
