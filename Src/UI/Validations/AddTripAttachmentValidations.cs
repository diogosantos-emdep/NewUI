using DevExpress.XtraEditors.Filtering.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[nsatpute][GEOS2-5931]
    public class AddTripAttachmentValidations : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, bool modelValidation = false, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (fieldName == "SelectedAttachmentType")
            {
                if (fieldValue == null || fieldValue.ToString().Trim().Equals("---"))
                    errorMessage = string.Format("You cannot leave the 'Type' empty.");


            } else if (fieldName == "TripAttachmentFile")
            {
                if (fieldValue == null || fieldValue.ToString().Trim() == string.Empty)
                    errorMessage = string.Format("You cannot leave the 'Attachment' empty.");
                
                if(modelValidation)
                    errorMessage = string.Format("The selected file already exists.");
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
