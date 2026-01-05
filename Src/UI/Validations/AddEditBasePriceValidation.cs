using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.PLM;
using System.Globalization;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.UI.Validations
{
   public  class AddEditBasePriceValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;


            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "InformationError")
            {
                if (fieldValue != null && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
                    errorMessage = string.Format("You cannot leave the information empty.");
            }
            if (fieldName == "SelectedBasicPriceFile")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "File");
            }
            if (fieldName == "BPLSelectedType")
            {
                string value = string.Empty;
                if (fieldValue != null && fieldValue.GetType().Equals(typeof(LookupValue)))
                {
                    value = ((LookupValue)fieldValue).Value.ToString().Trim();

                    if (fieldValue == null || string.IsNullOrEmpty(value) || value == "---")
                        errorMessage = string.Format("You cannot leave the {0} field empty.", "Base Price Document Type");
                    else
                        errorMessage = string.Empty;
                }
                else if (fieldValue == null)
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Base Price Document Type");
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
