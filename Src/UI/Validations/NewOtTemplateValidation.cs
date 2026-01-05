using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class NewOtTemplateValidation : ValidationRule
    {
        public string FieldName { get; set; }

        #region 
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
           // object obj = 0;
            string result = string.Empty;
            if (fieldName == "TemplateName" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Template Name");
            }

            if (fieldName == "SelectedIndexGroup")
            {
                if ((fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
                {
                    result = string.Format("You cannot leave the {0} field empty.", "Group");
                }
                else
                {
                    if(fieldValue.ToString()=="0")
                    {
                        result = string.Format("You cannot leave the {0} field empty.", "Group");
                    }
                }
            }

            if (fieldName == "SelectedIndexRegion")
            {
                if ((fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
                {
                    result = string.Format("You cannot leave the {0} field empty.", "Region");
                }
                else
                {
                    if (fieldValue.ToString() == "-1")
                    {
                        result = string.Format("You cannot leave the {0} field empty.", "Region");
                    }
                }
                
            }
            if (fieldName == "SelectedIndexCountry")
            {
                if ((fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
                {
                    result = string.Format("You cannot leave the {0} field empty.", "Country");
                }
                else
                {
                    if (fieldValue.ToString() == "-1")
                    {
                        result = string.Format("You cannot leave the {0} field empty.", "Country");
                    }
                }
                
            }

            if (fieldName == "SelectedIndexPlant" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()) || fieldValue.ToString() == "-1"))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Plant");
            }

            if (fieldName == "SelectedTemplateFile" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "File");
            }
            //[Rahul.Gadhave][GEOS2-6734][Date:16-01-2024]
            // Mapping Fields for Excel
            if (fieldName == "PONumberRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "PO Number");
            }
            if (fieldName == "PODateRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "PO Date");
            }
            if (fieldName == "CustomerRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Customer");
            }
            if (fieldName == "ContactRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Contact");
            }
            if (fieldName == "AmountRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Amount");
            }
            if (fieldName == "CurrencyRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Currency");
            }
            if (fieldName == "ShipToRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Ship To");
            }
            if (fieldName == "IncotermsRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Incoterms");
            }
            if (fieldName == "PaymentTermsRangeExcel" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Payment Terms");
            }
            //[Rahul.Gadhave][GEOS2-6734][Date:16-01-2024]
            // Mapping Fields for Pdf -PO Number
            if (fieldName == "PONumberKeyword" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "PONumberDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "PONumberCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Date
            if (fieldName == "DateKeyword" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "DateDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "DateCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Customer
            if (fieldName == "CustomerKeyWord" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "CustomerDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "CustomerCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Contact
            if (fieldName == "ContactKeyWord" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "ContactDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "ContactCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Amount
            if (fieldName == "AmountKeyWord" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "AmountDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "AmountCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Currency
            if (fieldName == "CurrencyKeyword" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "CurrencyDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "CurrencyCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Ship To
            if (fieldName == "ShipTOKeyword" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "ShipTODelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "ShipTOCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Incoterms
            if (fieldName == "IncotermsKeyWord" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "IncotermsDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "IncotermsCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            // Mapping Fields for Pdf -PO Payment terms
            if (fieldName == "PaymentTermsKeyWord" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "KeyWord");
            }
            if (fieldName == "PaymentTermsDelimiter" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Delimiter");
            }
            if (fieldName == "PaymentTermsCoordinates" && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim())))
            {
                result = string.Format("You cannot leave the {0} field empty.", "Coordinates");
            }
            return result;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string errorMessage = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new ValidationResult(isValid: false, errorMessage);
            }

            return ValidationResult.ValidResult;
        }

        #endregion

    }
}
