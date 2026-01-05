using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
   public class ItemWorkLogValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public object fieldValue1 { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object fieldValue1,object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "EndDate" && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("End Date can not be empty.");
                }
                else
                {
                    DateTime EndDate = (DateTime)fieldValue1; DateTime StartDate = (DateTime)fieldValue;
                    if (EndDate < StartDate)
                    {
                        errorMessage = string.Format("End Date must be greater than or equal than Start Date.");
                    }
                }

            }

            if (fieldName == "StartDate" && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
            {
                
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("Start Date can not be empty.");
                }
                else 
                {
                      DateTime StartDate = (DateTime)fieldValue; DateTime EndDate = (DateTime)fieldValue1;
                    if (EndDate < StartDate)
                    {
                        errorMessage = string.Format("Start Date must be Less than or equal than End Date.");
                    }
                }
            }
            if (fieldName == "EndTime" && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("End Date can not be empty.");
                }
                else
                {
                    DateTime EndTime = (DateTime)fieldValue1; DateTime StartTime = (DateTime)fieldValue;
                    if (EndTime < StartTime)
                    {
                        errorMessage = string.Format("End Time must be greater than or equal than Start Time.");
                    }
                }

            }

            if (fieldName == "StartTime" && !fieldValue.GetType().Equals(typeof(System.Windows.Data.BindingExpression)))
            {

                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("Start Date can not be empty.");
                }
                else
                {
                    DateTime StartTime = (DateTime)fieldValue; DateTime EndTime = (DateTime)fieldValue1;
                    if (EndTime < StartTime)
                    {
                        errorMessage = string.Format("Start Time must be Less than or equal than End Time.");
                    }
                }
            }


            return errorMessage;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, fieldValue1);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}

