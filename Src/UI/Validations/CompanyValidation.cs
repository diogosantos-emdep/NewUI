using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class CompanyValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (fieldName == "PlantName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
            }
            if (fieldName == "RegisteredName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Registered Name");
            }
            if (fieldName == "Abbreviation")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            if (fieldName == "FiscalNumber")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Fiscal Number");
            }
            if (fieldName == "Size")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            if (fieldName == "DateOfEstablishment")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "date of establishment");
            }
            if (fieldName == "TelephoneNumber")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "telephone number");
            }
            if (fieldName == "Email")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            if (fieldName == "Address")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            if (fieldName == "ZipCode")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            if (fieldName == "SelectedCity")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "city");
            }
            if (fieldName == "State")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            if (fieldName == "SelectedCountryIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Country");
            }

            else if (fieldName == "InformationError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");

            }
            else if (fieldName == "SelectedComapanyTypes")
            {

                if (fieldValue is List<object>)
                {
                    var SelectedComapanyTypes = fieldValue as List<object>;
                    if (SelectedComapanyTypes == null || SelectedComapanyTypes?.Count == 0)
                    {
                        errorMessage = string.Format("At least one type has to be selected.");
                    }
                }
                else {
                    try
                    {
                        var VM = (((Window)(((System.Windows.Data.BindingExpression)fieldValue).DataItem)).DataContext);
                        var SelectedComapanyTypes = VM.GetType().GetProperty("SelectedComapanyTypes").GetValue(VM, null) as List<object>;
                        if (SelectedComapanyTypes == null || SelectedComapanyTypes?.Count == 0)
                        {
                            errorMessage = string.Format("At least one type has to be selected.");
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else if (fieldName == "SelectedBusinessUnitIndex") // [nsatpute][06-01-2025] [GEOS2-6775]
            {
                if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Unit");
            }


            if (fieldName == "IsCompany")
                errorMessage = "Error";
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
