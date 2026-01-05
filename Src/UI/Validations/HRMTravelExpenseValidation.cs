using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace Emdep.Geos.UI.Validations
{
    //[rdixit][GEOS2-4022,4021,4025][24.11.2022]
    public class HRMTravelExpenseValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public object fieldValue1 { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object fieldValue1, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "SelectedExpenseReasonIndex")
            {
                if (fieldValue == null || (fieldValue.Equals(-1) || fieldValue.Equals(0)) || fieldValue == "---")
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Travel Expense Reason");
            }
            if (fieldName == "SelectedCurrencyIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Currency");
            }
            if (fieldName == "SelectedCompanyIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Company");
            }
            if (fieldName == "SelectedEmployeeIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Employee");
            }
            if (fieldName == "Title")
            {
                if (fieldValue== null)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Title");
            }
            if (fieldName == "EndDate")
            {
                if (fieldValue1 == null || string.IsNullOrEmpty(fieldValue1.ToString()))
                {
                    errorMessage = string.Format("End Date can not be empty.");
                }
                else
                {
                    DateTime StartDate = (DateTime)fieldValue; DateTime EndDate = (DateTime)fieldValue1;
                    if (EndDate < StartDate)
                    {
                        errorMessage = string.Format("End Date must be greater than or equal than Start Date.");
                    }
                }

            }
            if (fieldName == "StartDate")
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
            if (fieldName == "Comment")
            {
                if (fieldValue == null)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "comment");
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
