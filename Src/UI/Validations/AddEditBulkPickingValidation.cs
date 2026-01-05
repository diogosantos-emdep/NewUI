using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[Sudhir.jangra][GEOS2-4414]
    public class AddEditBulkPickingValidation : ValidationRule
    {
        public string FieldName { get; set; }

      

        public static string GetErrorMessage(string fieldName,object fieldValue,object nullValue=null)
        {
            string errorMessage = string.Empty;

            if (fieldName== "SelectedArticleList")
            { 
                if (fieldValue==null||fieldValue.ToString().Equals("---"))
                {
                    errorMessage = string.Format("You cannot leave the Selected Article empty.");
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
