using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class WarehouseValidation: ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "SupplierReference" || fieldName == "DeliveryDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "InformationError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");
              
            }

            //else if (fieldName == "Quantity")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Attendees");
            //    else
            //    {
            //        if (fieldValue != null)
            //        {
            //            int i = Convert.ToInt32(fieldValue);
            //            if (i == 0)
            //                errorMessage = string.Format("You cannot  quote the {0} field with 0 or remove reference", "Quantity");
            //        }
            //    }
            //}
            //else if (fieldName == "Producer")
            //{
            //    if (fieldValue != null)
            //    {
            //        ManufacturersByArticle obj = (ManufacturersByArticle)fieldValue;
            //        //if (obj.IdManufacturerByArticle == null || obj.IdManufacturerByArticle == 0)
            //        //    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            //    }
            //    else
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            //}
            //else if (fieldName == "Code")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            //    //else
            //    //{
            //    //    if (fieldValue != null)
            //    //    {
            //    //        string serialNumber = fieldValue.ToString();
            //    //        //if (serialNumber == )
            //    //        //    errorMessage = string.Format("You cannot  quote the {0} field with 0 or remove reference", "Quantity");
            //    //    }
            //    //}
            //}
            else if (fieldName == "ManufacturerName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Manufacturer");
                }
            }
      
            else if (fieldName == "SelectedIndexManufacturer")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Manufacturer");
                else if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Manufacturer");
            }
            else if (fieldName == "WrongLocation")
            {
                if (fieldValue == null || fieldValue.ToString()=="")
                     errorMessage = null;

            }
            else if (fieldName == "WrongItem")
            {
                if (fieldValue == null || fieldValue.ToString() == "")
                    errorMessage = null;
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
