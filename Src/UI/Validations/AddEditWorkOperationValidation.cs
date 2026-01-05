using Emdep.Geos.Data.Common.ERM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditWorkOperationValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public bool FlagIsFather { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, bool flagIsFather, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "Code")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "SelectedStages")
            {
                string value = string.Empty;
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)) && ((System.Collections.Generic.List<object>)fieldValue).Count > 0)
                {
                    if (((System.Collections.Generic.List<object>)fieldValue).Count == 1)
                    {
                        var td = ((System.Collections.Generic.List<object>)fieldValue);
                        foreach (var item in td)
                        {
                            value = ((Stages)item).Code.ToString().Trim();

                            if (value == "---")
                            {
                                errorMessage = string.Format("You cannot leave the {0} field empty.", "Stages");
                                return errorMessage;
                            }
                        }
                    }
                    errorMessage = string.Empty;
                }
                else if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)) && ((System.Collections.Generic.List<object>)fieldValue).Count == 0)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Stages");
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Stages");
                }
            }
            else if (fieldName == "Activity")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()) )
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                else if(!fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)) && !string.IsNullOrEmpty(fieldValue.ToString().Trim()) &&  Convert.ToInt32(fieldValue)<0)
                    errorMessage = string.Format("{0} should be greater than zero.", fieldName);
            }
            else if (fieldName == "ObservedTime")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "UITempobservedTime")  //[Rupali Sarode][21-11-2022]
            {
                if (flagIsFather == false)
                {
                    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                        errorMessage = string.Format("You cannot leave the Observed Time field empty.");

                    // [GEOS2-4933][Rupali Sarode][21-11-2023]
                    //else if (fieldValue.ToString() == "00:00:00")
                    //    errorMessage = string.Format("Observed Time should be greater than zero.");
                    else if (fieldValue.ToString() == "00:00:00")
                        errorMessage = string.Format("Observed Time should be greater than zero.");
                }
            }


            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, FlagIsFather);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
