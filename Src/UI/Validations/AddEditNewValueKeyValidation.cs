using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[Sudhir.Jangra][GEOS2-4502][11/07/2023]
    public class AddEditNewValueKeyValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public List<string> LookUpKeyNameList { get; set; }
        public static string GetErrorMessage(string fieldName, List<string> LookUpKeyNameList, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (fieldName == "Name")
            {
                if (fieldValue==null||string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.",fieldName);
                }
                  
                    if (LookUpKeyNameList != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    {
                        foreach (string name in LookUpKeyNameList)
                        {
                            if (fieldValue.ToString().ToUpper() == name.ToUpper())
                            {
                                errorMessage = string.Format("Name is already exist.", fieldName);
                            }
                        }
                    }

                
            }
            return errorMessage;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, LookUpKeyNameList, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
