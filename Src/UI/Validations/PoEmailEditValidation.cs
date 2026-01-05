using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public  class PoEmailEditValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public List<People> fieldValue1 { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
            if (fieldName == "SelectedOffer")
            {
                 if (fieldValue ==null)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "TO");
            }
         
            return errorMessage;
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, fieldValue1);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
