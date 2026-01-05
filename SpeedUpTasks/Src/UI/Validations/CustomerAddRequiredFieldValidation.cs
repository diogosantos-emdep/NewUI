using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Validations
{
    public class CustomerAddRequiredFieldValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "CustomerPlantName")
            {
                if (GeosApplication.Instance.IsGroupNameExist)
                    errorMessage = string.Format("Please remove group name from Plant name.");
                else if(!GeosApplication.Instance.IsCityNameExist)
                    errorMessage = string.Format("Please add city name in Plant name.");

                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant Name");
            }

            else if (fieldName == "SelectedIndexCompanyGroup")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Company Group");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Company Group");
            }

            else if (fieldName == "SelectedIndexBusinessCenter")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Center");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Center");
            }

            else if (fieldName == "SelectedIndexBusinessField")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Field");
                //else if (fieldValue != null && fieldValue.Equals(0))
                //    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Field");
            }


            else if (fieldName == "SelectedBusinessProductItems")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Product");

                else if (fieldValue != null && ((List<object>)fieldValue).Count == 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Product");
            }

            else if (fieldName == "CustomerAddress")
            {
                if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    int addressLegnth = fieldValue.ToString().Length;

                    if (addressLegnth > 500)
                        errorMessage = string.Format("{0} must be less than 500 characters.", "Address");
                }

            }

            else if (fieldName == "CustomerName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Group Name");
            }

            else if (fieldName == "SelectedIndexCountry")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Country");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Country");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Country");
            }

            else if (fieldName == "CustomerCity")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "City");
            }

            else if (fieldName == "InformationError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");
                
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
