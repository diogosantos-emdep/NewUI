using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditWorkStageValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public bool FieldValueFlag { get; set; }
        public List<string> CodeLists { get; set; }
        public string OldCode { get; set; }

        public List<object> SelectedPlants { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, bool fieldValueFlag, List<string> codeList, string oldCode, List<object> selectedPlant, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Code")
            {
                if (fieldValueFlag == true)
                {
                    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    {
                        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                    }

                }

                if (oldCode == null && codeList != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    foreach (string ccode in codeList)
                    {
                        if (!string.IsNullOrEmpty(ccode))
                        {
                            if (Convert.ToString(fieldValue).ToUpper() == Convert.ToString(ccode).ToUpper())
                            {
                                errorMessage = string.Format("Code already exists.", fieldName);
                            }
                        }
                    }
                }
                //else if (oldCode != null || codeList != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                else if (oldCode != null || codeList != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    if (fieldValue != null)
                    {
                        if (Convert.ToString(oldCode).ToUpper() != Convert.ToString(fieldValue).ToUpper())
                        {
                            foreach (string ccode in codeList)
                            {
                                if (!string.IsNullOrEmpty(ccode))
                                {
                                    if (Convert.ToString(fieldValue).ToUpper() == Convert.ToString(ccode).ToUpper())
                                    {
                                        errorMessage = string.Format("Code already exists.", fieldName);
                                    }
                                }
                            }
                        }
                    }
                }


            }
            else if (fieldName == "Sequence")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "SelectedPlant")
            {
                int count = 0;
                if (fieldValue == null)
                {
                    errorMessage = string.Format("Please select plant.");
                    
                }
                else
                {
                    if (fieldValue != null || !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    {

                        if (selectedPlant != null)
                        {
                            foreach (Site item in selectedPlant)
                            {
                                string temp = Convert.ToString(item.Name);
                                if (temp != "---")
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                errorMessage = string.Format("Please select plant.");
                            }
                        }
                    }
                }
            }


            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, FieldValueFlag, CodeLists, OldCode, SelectedPlants);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
