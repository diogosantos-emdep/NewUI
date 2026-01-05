using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.Hrm;

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
                else if (fieldValue != null)
                {
                    if (fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    {                        
                    }
                    else if(fieldValue.Equals("---"))
                    {
                        errorMessage = string.Format("You cannot select '---'.", "Work Schedule");
                    }
                }
            }
            if (fieldName== "SelectedIndexType")
            {
                //if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                //    errorMessage = string.Format("You cannot leave the {0} field empty.", "SelectedIndexType");
                //else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                //    errorMessage = string.Format("You cannot leave the {0} field empty.", "SelectedIndexType");
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "SelectedIndexType");
                else if (fieldValue!=null)
                {
                    if (fieldValue.Equals(0))
                    {
                        errorMessage = string.Format("You cannot select '---'.", "SelectedIndexType");
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
