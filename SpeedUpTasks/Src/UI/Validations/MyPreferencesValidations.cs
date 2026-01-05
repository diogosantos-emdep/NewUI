using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Validations
{
    public class MyPreferencesValidations : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            //if (fieldName == "AutoRefreshOffOption")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Auto Refresh Option");
            //}
            //else 
            if (fieldName == "FromDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }

            }
            else if (fieldName == "ToDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
     
            }
            else if (fieldName == "Opportunity" || fieldName == "Contact" || fieldName == "Account"
                || fieldName == "Appointment" || fieldName == "Task" || fieldName == "Call"
                || fieldName == "Email" || fieldName == "Action" || fieldName == "SearchOpportunityOrOrder")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }

                //if(((DevExpress.Xpf.Editors.BaseEdit)((System.Windows.Data.BindingExpressionBase)fieldValue).Target).EditValue==null)
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
