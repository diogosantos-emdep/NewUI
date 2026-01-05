using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Emdep.Geos.UI.Validations
{
    public class AddEditModuleValidation : ValidationRule
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

            if (fieldName == "ImageName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "File");
            }

            if (fieldName == "SelectedProductTypeFile")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "File");
            }

            if (fieldName == "SelectedArticleFile")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "File");
            }

            if (fieldName == "LinkAddress")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Url");
            }

            if (fieldName == "InformationError")
            {
                if (fieldValue == null) // || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the information empty.");
            }

            if (fieldName == "SelectedTemplate")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("---")) // || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the Selected Template empty.");
            }

            if (fieldName == "CompatibilityError")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("Please input valid data.");
            }
            //#region [GEOS2-3759][gulab lakade][02 01 2023]
            //if (fieldName == "Abbrivation")
            //{
            //    if(fieldValue != null)
            //    {
            //        bool isValid = Regex.IsMatch(fieldValue.ToString(), "^[a-z0-9 ]+$", RegexOptions.IgnoreCase);
            //        if (isValid==false)
            //        {
            //            errorMessage = string.Format("Please input valid data.");
            //        }
            //    }
               
                    
                    
            //}
            //#endregion

            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
        //public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
