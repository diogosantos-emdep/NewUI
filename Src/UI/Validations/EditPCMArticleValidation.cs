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
    public class EditPCMArticleValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "SelectedCategory")
            {
                    if (fieldValue is Emdep.Geos.Data.Common.PCM.PCMArticleCategory
                    && ((Emdep.Geos.Data.Common.PCM.PCMArticleCategory)fieldValue).IdPCMArticleCategory == 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Category");
            }

            if (fieldName == "SelectedStatus")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Status");


                else if (fieldValue is Emdep.Geos.Data.Common.Epc.LookupValue
                    && ((Emdep.Geos.Data.Common.Epc.LookupValue)fieldValue).IdLookupValue == 0)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Status");
            }

            if (fieldName == "CompatibilityError")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("Please check inputted data.");
            }

            if (fieldName == "InformationError")
            {
                if (fieldValue == null) 
                    errorMessage = string.Format("You cannot leave the information empty.");
            }

            if (fieldName == "PQuantityMin")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Min");
            }

            if (fieldName == "PQuantityMax")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Max");
            }

            if(fieldName == "Description")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Name");
            }

            if (fieldName == "Description_en")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Name");
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
