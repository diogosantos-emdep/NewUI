using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddStatusUpdateCommentValidation : ValidationRule
    {

        public string FieldName { get; set; }
        //[Shweta.Thube][GEOS2-5976] 
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the field empty.");

            if (fieldName == "Comment")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the Comment field empty.");
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
