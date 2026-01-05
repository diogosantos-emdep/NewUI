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
                else if (!GeosApplication.Instance.IsCityNameExist)
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

                if (fieldValue != null && fieldValue.Equals(true))
                    errorMessage = string.Format("Address must not contain City/State/Country/Registered Name/Zip code");

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
                //if (fieldValue != null)
                //    errorMessage = string.Format("You cannot leave the information empty.");
                //[rdixit][07.04.2023][GEOS2-4295]
                if (fieldValue != null)
                {
                    if (fieldValue.ToString() == "AddressError")
                    {
                        errorMessage = string.Format("Address must not contain City/State/Country/Registered Name/Zip code");
                    }
                    else
                    {
                        errorMessage = string.Format("You cannot leave the information empty.");
                    }
                }

            }
            else if (fieldName == "AssignedSalesOwnerError")//[rdixit][19.06.2023][GEOS2-4559]
            {
                if (fieldValue != null)
                {
                    errorMessage = string.Format("Please select atleast one Sales Owner.");
                }
            }
            //[RGadhave][12.11.2024][GEOS2-6462]
            else if (fieldName == "SelectedIndexIncoterms")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Incoterms");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Incoterms");
            }
            else if (fieldName == "SelectedIndexPaymentTerms")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "PaymentTerms");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "PaymentTerms");
            }
            //chitra.girigosavi[GEOS2-7242][14/04/2025]
            else if (fieldName == "SelectedStatusIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Status");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Status");
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
