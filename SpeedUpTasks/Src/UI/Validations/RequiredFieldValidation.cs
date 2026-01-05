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
    public class RequiredValidationRule : ValidationRule
    {
        public string FieldName { get; set; }
        /// <summary>
        /// [001][skale][30-09-2019][GEOS2-1756] Add the possibility to Edit the Offer LOST Date
        /// [002][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);

            if (fieldName == "Description")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "SelectedIndexLeadSource")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Source");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Source");
            }

            else if (fieldName == "OfferAmount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                else if (Convert.ToDouble(fieldValue) == 0 && Convert.ToInt16(nullValue) != 2)
                    errorMessage = string.Format("{0} must be greater than zero.", fieldName);
            }

            else if (fieldName == "OfferCloseDate")
            {
                // If GeosStatus(nullValue) is "17-LOST" or "4-Cancelled" then do not apply validation on date else apply.
                if ((fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    && (Convert.ToInt16(nullValue) != 17 && Convert.ToInt16(nullValue) != 4))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
                else if ((Convert.ToInt16(nullValue) != 17 && Convert.ToInt16(nullValue) != 4)
                          && Convert.ToDateTime(fieldValue).Date < GeosApplication.Instance.ServerDateTime.Date)
                {
                    errorMessage = string.Format("{0} must be greater than current date.", fieldName);
                }
            }

            else if (fieldName == "QuoteSentDate")
            {
                // Offer 2 is for Status Only Quoted
                if (Convert.ToInt16(nullValue) == 1 && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString())))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "RFQReceptionDate")
            {
                // 2 is for Status Waiting for Quote
                if (Convert.ToInt16(nullValue) == 2 && (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString())))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            else if (fieldName == "SelectedIndexCompanyGroup")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Group");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Group");
            }
            else if (fieldName == "SelectedIndexSalesActivityType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Type");
            }
            else if (fieldName == "SelectedScopeListIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
            }
            else if (fieldName == "SelectedIndexCompanyPlant")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Plant");
            }

            else if (fieldName == "SelectedIndexBusinessUnit")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Unit");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Business Unit");
            }

            else if (fieldName == "SelectedIndexSalesOwner")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sales owner");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Sales owner");

            }
            else if (fieldName == "IsSiteResponsibleRemoved")
            {
                if (fieldValue.Equals(true))
                    errorMessage = string.Format("{0} does not exist.", "Sales owner");
            }
            else if (fieldName == "IsStatusDisabled")
            {
                if (fieldValue.Equals(true))
                    errorMessage = string.Format("{0} not allow to select.", "Status");
            }
            else if (fieldName == "ProductAndServicesCount")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Product and Services");
                else if (fieldValue != null && fieldValue.Equals(0))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Product and Services");
            }

            else if (fieldName == "SelectedItems") // Reason from Lost Opportunity.
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Reason");
            }

            else if (fieldName == "ProjectName")
            {
                if (GeosApplication.Instance.IsCarOEMExist)
                    errorMessage = string.Format("Please remove CarOEM name from project name.");

                else if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Project Name");
            }

            else if (fieldName == "SelectedIndexCarOEM")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Car OEM");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Car OEM");
            }
            else if (fieldName == "AttachedFileIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "file name");
                else if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "file name");
            }
            else if (fieldName == "InformationError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");
                //else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                //    errorMessage = string.Format("You cannot leave the information empty.");
            }
            else if (fieldName == "SheetNameSelectedIndex")
            {
                if (fieldValue.Equals(1))
                    errorMessage = string.Format("File is in wrong format");

            }
            else if (fieldName == "EmployeeAttendance")
            {
                if (fieldValue.Equals(0))
                    errorMessage = string.Format("File is in wrong format");

            }
            //[001] Added
            else if (fieldName == "OfferLostDate") // Lost Date from Lost Opportunity.
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Date");
            }
            //[002] added
            else if (fieldName == "SelectedIndexOfferOwner")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Offer owner");

                else if (fieldValue != null && (fieldValue.Equals(-1)|| fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Offer owner");


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
