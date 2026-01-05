using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class ReportValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);


            if (fieldName == "FromDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }

            }
            else if (fieldName == "ToDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
            }
            //else if (fieldName == "SelectedIndexGroup")
            //{
            //    if (fieldValue.Equals((short)-1) || fieldValue.Equals((short)0))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Group");
            //}
            //else  if (fieldName == "SelectedIndexPlant")
            //{
            //    if (fieldValue.Equals((short)-1) || fieldValue.Equals((short)0))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
            //}
           
            //else if (fieldName == "SelectedIndexTemplate")
            //{
            //    if (fieldValue.Equals((short)-1) || fieldValue.Equals((short)0))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Template");
            //}
            //else if (fieldName == "SelectedIndexCpType")
            //{
            //    if (fieldValue.Equals((short)-1) || fieldValue.Equals((short)0))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
            //}
            //else if (fieldName == "SelectedIndexOption")
            //{
            //    if (fieldValue.Equals((short)-1))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Option");
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
