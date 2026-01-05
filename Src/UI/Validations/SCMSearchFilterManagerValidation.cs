using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class SCMSearchFilterManagerValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public Int32 NumberOfPages { get; set; }
        public static string GetErrorMessage(string fieldName, Int32 NoOfPages, object fieldValue, object nullValue = null)
        {
            //[rdixit][GEOS2-4973][14.12.2023]
            string errorMessage = string.Empty;
            Regex regex = new Regex(@"^[0-9]+(?:,[0-9]+)*$");

            if (nullValue != null && nullValue.Equals(fieldValue) && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            else if (fieldName == "SelectedColor")
            {
                if (fieldValue == null && NoOfPages > 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                else
                    errorMessage = string.Empty;
            }

            else if (fieldName == "ComponentsPagesToApply")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()) && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                {
                    string value = Convert.ToString(fieldValue).Trim();

                    string[] splittedValue = value.Split(',');

                    foreach (var item in splittedValue)
                    {
                        if (!string.IsNullOrEmpty(item) && item != "" && regex.IsMatch(item))
                        {
                            Int32 itemValue = Convert.ToInt32(item);
                            if (NoOfPages < itemValue)
                            {
                                errorMessage = string.Format("You cannot Enter pages to apply more than no. of pages.", fieldName);
                                break;
                            }
                        }
                        if (!regex.IsMatch(item))
                        {
                            errorMessage = string.Format("You can only enter comma and numbers. Special charachter, alphabates and space not allowed", fieldName);
                            break;
                        }
                    }
                }
            }

            else if (fieldName == "ReferencePagesToApply")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()) && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                {
                    string value = Convert.ToString(fieldValue).Trim();

                    string[] splittedValue = value.Split(',');

                    foreach (var item in splittedValue)
                    {
                        if (!string.IsNullOrEmpty(item) && item != "" && regex.IsMatch(item))
                        {
                            Int32 itemValue = Convert.ToInt32(item);
                            if (NoOfPages < itemValue)
                            {
                                errorMessage = string.Format("You cannot Enter pages to apply more than no. of pages.", fieldName);
                                break;
                            }
                        }
                        if (!regex.IsMatch(item))
                        {
                            errorMessage = string.Format("You can only enter comma and numbers. Special charachter, alphabates and space not allowed", fieldName);
                            break;
                        }
                    }
                }
            }

            else if (fieldName == "AppearancePagesToApply")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()) && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                {
                    string value = Convert.ToString(fieldValue).Trim();

                    string[] splittedValue = value.Split(',');

                    foreach (var item in splittedValue)
                    {
                        if (!string.IsNullOrEmpty(item) && item != "" && regex.IsMatch(item))
                        {
                            Int32 itemValue = Convert.ToInt32(item);
                            if (NoOfPages < itemValue)
                            {
                                errorMessage = string.Format("You cannot Enter pages to apply more than no. of pages.", fieldName);
                                break;
                            }
                        }
                        if (!regex.IsMatch(item))
                        {
                            errorMessage = string.Format("You can only enter comma and numbers. Special charachter, alphabates and space not allowed", fieldName);
                            break;
                        }
                    }
                }
            }

            else if (fieldName == "DiameterAndSizePagesToApply")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()) && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                {
                    string value = Convert.ToString(fieldValue).Trim();

                    string[] splittedValue = value.Split(',');

                    foreach (var item in splittedValue)
                    {
                        if (!string.IsNullOrEmpty(item) && item != "" && regex.IsMatch(item))
                        {
                            Int32 itemValue = Convert.ToInt32(item);
                            if (NoOfPages < itemValue)
                            {
                                errorMessage = string.Format("You cannot Enter pages to apply more than no. of pages.", fieldName);
                                break;
                            }
                        }
                        if (!regex.IsMatch(item))
                        {
                            errorMessage = string.Format("You can only enter comma and numbers. Special charachter, alphabates and space not allowed", fieldName);
                            break;
                        }
                    }
                }
            }

            else if (fieldName == "WaysMarginPagesApply")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()) && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                {
                    string value = Convert.ToString(fieldValue).Trim();

                    string[] splittedValue = value.Split(',');

                    foreach (var item in splittedValue)
                    {
                        if (!string.IsNullOrEmpty(item) && item != "" && regex.IsMatch(item))
                        {
                            Int32 itemValue = Convert.ToInt32(item);
                            if (NoOfPages < itemValue)
                            {
                                errorMessage = string.Format("You cannot Enter pages to apply more than no. of pages.", fieldName);
                                break;
                            }
                        }
                        if (!regex.IsMatch(item))
                        {
                            errorMessage = string.Format("You can only enter comma and numbers. Special charachter, alphabates and space not allowed", fieldName);
                            break;
                        }
                    }
                }
            }
            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, NumberOfPages, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
