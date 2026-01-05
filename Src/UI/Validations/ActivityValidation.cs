using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class ActivityValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "SelectedIndexOwner")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Owner");
            }

            else if (fieldName == "Description" || fieldName == "ActivityAddress" || fieldName == "Subject")
            {
                if (fieldValue == null || string.IsNullOrEmpty(Convert.ToString(fieldValue).Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "SelectedIndexType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
            }

            else if (fieldName == "FromDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
                //else if (Convert.ToDateTime(fieldValue).Date < GeosApplication.Instance.ServerDateTime.Date)
                //{
                //    errorMessage = string.Format("{0} must be greater than current date.", fieldName);
                //}
            }

            else if (fieldName == "ToDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
                //else if (Convert.ToDateTime(fieldValue).Date < GeosApplication.Instance.ServerDateTime.Date)
                //{
                //    errorMessage = string.Format("{0} must be greater than current date.", fieldName);
                //}
            }
            else if (fieldName == "StartTime")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
            else if (fieldName == "EndTime")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "AttendeesCount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Attendees");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Attendees");
            }
            else if (fieldName == "DueDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Due Date");
            }

            else if (fieldName == "SelectedIndexTaskStatus")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Status");
                else if (fieldValue != null && (fieldValue.Equals(-1)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Status");
            }

            else if (fieldName == "SelectedAccountActivityLinkedItemsCount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("Please add an Account.");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("Please add an Account.");
            }

            else if (fieldName == "SelectedActivityOwnerList")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Activity Owners");
            }

            else if (fieldName == "TagName")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Tag Name");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Tag Name");
            }
            else if (fieldName == "SelectedIndexCompanyGroup")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Group");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Group");
            }
            else if (fieldName == "SelectedIndexCompanyPlant")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
            }
            else if (fieldName == "EditActivityError")
            {
                if (fieldValue != null )
                    errorMessage = string.Format("You cannot leave the information empty.");
                //else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                //    errorMessage = string.Format("You cannot leave the information empty.");
            }

            else if (fieldName == "AddActivityError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");
                
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
