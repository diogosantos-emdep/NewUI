using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class ShiftsValidations : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
       {
            string errorMessage = string.Empty;

            if (fieldName == "ShiftName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Shift Name");
                else if (fieldValue != null && (fieldValue.Equals("---")))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Employee");
            }


            if (fieldName == "SelectedWorkSchedule")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Work Schedule");
                else if (fieldValue != null && (fieldValue.Equals("---")))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Work Schedule");
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
