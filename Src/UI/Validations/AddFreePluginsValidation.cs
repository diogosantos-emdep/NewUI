using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;


namespace Emdep.Geos.UI.Validations
{

    //[PRAMOD.MISAL][GEOS2-4443][29-08-2023]
    public class AddFreePluginsValidation : ValidationRule
    {
        public string FieldName { get; set; }


        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "SelectedFreePluginName")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("---"))
                {
                    errorMessage = string.Format("You cannot leave the Selected Name empty.");
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
