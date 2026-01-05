using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class AddEditEquivalencyWeightValidation: ValidationRule
    {
        public string FieldName { get; set; }
        public object fieldValue1 { get; set; }
        public object fieldValue2 { get; set; }
        public object fieldValue3 { get; set; }
        public object fieldValue4 { get; set; }
        public object fieldValue5 { get; set; }
        public object fieldValue6 { get; set; }
        public object fieldValue7 { get; set; }
        public static string GetErrorMessage(string fieldName, object fieldValue, object fieldValue1, object fieldValue2, object fieldValue3, object fieldValue4, object fieldValue5, object fieldValue6, object fieldValue7, object nullValue = null)
        {
            string errorMessage = string.Empty;

            if (fieldName == "EquivalentWeight")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
            }


            if (fieldName == "StartDate")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
                }
                else
                {
                    if (fieldValue3 != null)
                    {
                        DateTime SelectedStartDate = (DateTime)fieldValue;
                        DateTime SelectedStartDate1 = (DateTime)fieldValue3;
                        DateTime endDate;
                        if (fieldValue1 != null)
                        {
                            endDate = (DateTime)fieldValue1;
                            TimeSpan test = SelectedStartDate.Subtract(endDate);

                            if (Convert.ToInt32(fieldValue6) != 1 && test.TotalHours != 24)
                            {
                                if (SelectedStartDate != SelectedStartDate1)
                                {
                                    errorMessage = string.Format("Can not gap between the End Date to Start Date.");
                                }
                            }
                        }

                       
                    }
                    else
                    {
                        if (fieldValue1 != null)
                        {
                            DateTime LastEndDate = (DateTime)fieldValue4;
                            DateTime SelectedStartDate = (DateTime)fieldValue;
                            TimeSpan test = SelectedStartDate.Subtract(LastEndDate);
                            if (test.TotalHours != 24)
                            {
                                errorMessage = string.Format("Can not gap between the End Date to Start Date.");
                            }
                           // else if()
                        }
                        else
                        if (fieldValue5 != null)
                        {
                            DateTime LastEndDate = (DateTime)fieldValue5;
                            DateTime SelectedStartDate = (DateTime)fieldValue;
                            TimeSpan test = SelectedStartDate.Subtract(LastEndDate);
                           // DateTime SelectedEndDate1 = (DateTime)fieldValue2;
                            if (test.TotalHours != 24)
                            {
                                errorMessage = string.Format("Can not gap between the End Date to Start Date.");
                            }
                        }


                    }
                }
            }

            if (fieldName == "EndDate")
            {

                if (fieldValue2 != null)
                {
                    DateTime SelectedStartDate = (DateTime)fieldValue;
                    DateTime SelectedEndDate1 = (DateTime)fieldValue2;
                    if (SelectedStartDate > SelectedEndDate1)
                    {
                        errorMessage = string.Format("End Date must be greater than or equal Start Date.");
                    }
               }
                //else
                //{

                //}
                //else
                //{
                //    if (fieldValue5 != null)
                //    {
                //        DateTime LastEndDate = (DateTime)fieldValue5;
                //        DateTime SelectedStartDate = (DateTime)fieldValue;
                //        TimeSpan test = SelectedStartDate.Subtract(LastEndDate);
                //        DateTime SelectedEndDate1 = (DateTime)fieldValue2;
                //        if (test.TotalHours != 24)
                //        {
                //            errorMessage = string.Format("Can not gap between the End Date to Start Date.");
                //        }
                //        else if (SelectedStartDate > SelectedEndDate1)
                //        {
                //            errorMessage = string.Format("End Date must be greater than or equal Start Date.");
                //        }
                //    }
                //}
                //else
                //{
                   
                //   // DateTime SelectedEndDate1 = (DateTime)fieldValue2;
                //     if (fieldValue2 == null || string.IsNullOrEmpty(fieldValue.ToString()))
                //    {
                //        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldValue2);
                //    }
                //}
              
            }
            return errorMessage;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value, fieldValue1, fieldValue2, fieldValue3, fieldValue4, fieldValue5, fieldValue6, fieldValue7);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
