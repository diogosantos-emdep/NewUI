using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[Sudhir.Jangra][GEOS2-4915]
    public class AddEditHardLockPluginNameValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName,object fieldValue,object nullValue=null)
        {
            string errorMessage = string.Empty;

            if (fieldName=="Name")
            {
                if (fieldValue==null||fieldValue.ToString().Equals(""))
                {
                    errorMessage = string.Format("You cannot leave the name empty.");
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
