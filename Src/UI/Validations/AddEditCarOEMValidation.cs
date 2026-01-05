using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.Crm;


namespace Emdep.Geos.UI.Validations
{
    
   public class AddEditCarOEMValidation : ValidationRule
    {
        //[001][rdixit][GEOS2-244][22/06/2022]
        //[002][rdixit][GEOS2-245][25/06/2022]
        public string FieldName { get; set; }
        public string PrevCarOEMName { get; set; }
        public List<string> CarName { get; set; }
        public static string GetErrorMessage(string fieldName,string PrevCarOEMName, List<string> CarNames, object fieldValue, object nullValue = null)//[001]
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            else if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                if (PrevCarOEMName == null && CarNames != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    foreach (string name in CarNames)
                    {
                        if (fieldValue.ToString().ToUpper() == name.ToUpper())
                        {
                            errorMessage = string.Format("Car OEM name is already exist.", fieldName);
                        }
                    }
                }
                else if (PrevCarOEMName != null || CarNames != null && fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                {
                    if (PrevCarOEMName.ToUpper() != fieldValue.ToString().ToUpper())
                    {
                        foreach (string name in CarNames)
                        {
                            if (fieldValue.ToString().ToUpper() == name.ToUpper())
                            {
                                errorMessage = string.Format("Car OEM name is already exist.", fieldName);
                            }
                        }
                    }
                }
            }
            return errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName,PrevCarOEMName, CarName, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
