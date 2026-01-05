using Emdep.Geos.Data.Common.Hrm;
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
   public class EmployeeProfileValidation: ValidationRule
    {
        public string FieldName { get; set; }
        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile // Add new Validation for location and Organization
        /// [002][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// [003][skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "ContactValue" || fieldName == "DocumentName" || fieldName == "DocumentNumber" || fieldName== "Entity" || fieldName == "Description"||fieldName== "DurationValue")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }

            if (fieldName == "FirstName" || fieldName == "LastName" || fieldName == "Address"||fieldName== "Percent" || fieldName == "BirthDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            }
         
            if (fieldName == "SelectedNationalityIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Nationality");
            }
            //[001] added
            if (fieldName == "SelectedLocationIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Location");
            }
            if (fieldName == "SelectedOrganizationIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Organization");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Organization");
            }
            //End
            if (fieldName == "SelectedCountryIndex")
            {
              if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Country");
            }

            if (fieldName == "SelectedMaritalStatusIndex")
            {
              if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Marital Status");
            }

            if (fieldName == "SelectedIndexGender")
            {    
                 if (fieldValue != null && fieldValue.Equals(-1))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Gender");
            }

            if (fieldName == "SelectedIndexContactType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contact Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contact Type");
            }

            if (fieldName == "SelectedIndexDocumentType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Document Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Document Type");
            }

            if (fieldName == "SelectedIndexLanguage")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language");
            }

            if (fieldName == "SelectedIndexUnderstandingLevel")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language Understanding Level");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language Understanding Level");
            }

            if (fieldName == "SelectedIndexSpeakingLevel")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language Speaking Level");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language Speaking Level");
            }

            if (fieldName == "SelectedIndexWritingLevel")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language Writing Level");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Language Writing Level");
            }

            if (fieldName == "SelectedIndexQualification")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Qualification Name");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Qualification Name");
            }

            if (fieldName == "SelectedIndexContractSituation")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contract Situation");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contract Situation");
            }

            if (fieldName == "SelectedIndexProfessionalCategory")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Professional Category");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Professional Category");
            }

            if (fieldName == "SelectedRelationshipIndex")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Relationship Type");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Relationship Type");
            }

            if (fieldName == "SelectedIndexCompany")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Company");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Company");
            }

            if (fieldName == "SelectedJobDescription")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Job Title");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Job Title");
            }

            if (fieldName == "SelectedScope")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
            }

            if (fieldName == "SelectedIndexCountry")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
            }

            if (fieldName == "SelectedIndexCountry")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
            }

            if (fieldName == "SelectedScope")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Scope");
            }

            if (fieldName == "Percentage")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Percentage");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Percentage");
            }

            if (fieldName == "SelectedIndexEducationType")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Education Title");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Education Title");
            }

            if (fieldName == "SelectedIndexTimeDuration")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Duration Unit");
                else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Duration Unit");
            }

            if (fieldName == "StartDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Start Date");
            }
            if (fieldName == "EndDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "End Date");
            }
            if (fieldName == "IssueDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Issue Date");
            }
            if (fieldName == "ExpiryDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Expiry Date");
            }
            if (fieldName == "ExpiryDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Expiry Date");
            }
            if (fieldName == "JobDescriptionError")
            {
                if (fieldValue!=null)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Job Description");
            }

            if (fieldName == "ContractSituationError")
            {
                if (fieldValue!= null)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Contract Situation");
            }
            //if (fieldName == "SelectedEmpolyeeStatusIndex")
            //{
            //    if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Marital Status");
            //}

            // [003] Comment Add
            //if (fieldName == "SelectedCompanyShiftIndex")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Company Shift");
            //    else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Company Shift");
            //}
            //if (fieldName == "SelectedCompanyScheduleIndex")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Company Schedule");
            //    else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Company Schedule");
            //}

            if (fieldName == "SelectedCountryRegion")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Region");
            }
            if (fieldName == "SelectedCity")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "City");
            }
            if (fieldName == "ZipCode")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Zip Code");
            }
            else if (fieldName == "InformationError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");
                //else if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                //    errorMessage = string.Format("You cannot leave the information empty.");
            }
            //[002] added
            if (fieldName == "EmployeeShiftError")
            {
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Shift");
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
