using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.PCM;
using System.Globalization;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditCategoryValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            if (fieldName == "SelectedOrderCategory")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);


                else if (fieldValue is Emdep.Geos.Data.Common.PCM.PCMArticleCategory
                    && ((Emdep.Geos.Data.Common.PCM.PCMArticleCategory)fieldValue).IdPCMArticleCategory == 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Order");
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
