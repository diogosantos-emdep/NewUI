using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class PackingBoxValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "BoxNumber")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Box Number");
                }
                if (fieldValue != null && fieldValue.ToString().Trim() != "System.Windows.Data.BindingExpression")
                {
                    if (fieldValue.GetType() == typeof(bool))
                    {
                        if (Convert.ToBoolean(fieldValue) == true)
                            errorMessage = string.Format("The {0} already exist.", "Box Number");
                    }
                }
            }

            if (fieldName == "SelectedPackingBoxTypeIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Packing Box Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Packing Box Type");
            }
            if (fieldName == "SelectedCustomerIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Customer");
            }
            if (fieldName== "Height")
            {
                if (fieldValue.ToString() == "0" || fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Height");
                }
                
            }
            if (fieldName == "Width")
            {
                if (fieldValue.ToString()=="0" || fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Width");
                }
            }
            if (fieldName == "Length")
            {
                if (fieldValue.ToString() == "0" || fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Length");
                }
            }
            if (fieldName == "NetWeight")
            {
                if (fieldValue.ToString() == "0" || fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "TareWeight");
                }
            }
            if (fieldName == "GrossWeight")
            {
                if (fieldValue.ToString() == "0" || fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "GrossWeight");
                }
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
