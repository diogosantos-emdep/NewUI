using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditSubTaskValidation : ValidationRule
    { //[pallavi.kale][GEOS2-7002]
        public string FieldName { get; set; }
     
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the field empty.");

            if (fieldName == "Title")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the Title field empty.");
            }
            if (fieldName == "SelectedLocation")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(Company)))
                {
                    value = ((Company)fieldValue).Alias.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the Location field empty.");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the Location field empty.");
                }
            }
            if (fieldName == "SelectedResponsible")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(Responsible)))
                {
                    value = ((Responsible)fieldValue).FullName.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the Responsible field empty.");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the Responsible field empty.");
                }
            }
            if (fieldName == "SelectedStatus")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(LookupValue)))
                {
                    value = ((LookupValue)fieldValue).Value.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the Status field empty.");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the Status field empty.");
                }
            }
            if (fieldName == "SelectedPriority")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(LookupValue)))
                {
                    value = ((LookupValue)fieldValue).Value.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the Priority field empty.");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the Priority field empty.");
                }
            }
            
            if (fieldName == "SelectedTheme")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(LookupValue)))
                {
                    value = ((LookupValue)fieldValue).Value.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the Theme field empty.");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the Theme field empty.");
                }
            }
            if (fieldName == "Progress")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("Only the Action Plan Responsible can set it up to 100.");
                else if (fieldValue.Equals(100))
                    errorMessage = string.Format("Only the Action Plan Responsible can set it up to 100.");

            }
            if (fieldName == "TaskStatusComment")//[Sudhir.Jangra][GEOS2-7006]
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the Comment field empty.");
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
