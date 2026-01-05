using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class SRMArticleSupplierValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            //[rdixit][GEOS2-4738][18.10.2023]
            if (fieldName == "Name")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the Name field empty.");
            }
            //if (fieldName == "DepartmentName" || fieldName == "HTMLColor")
            //{
            //    if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", fieldName);
            //}
            if (fieldName == "SelectedArticleSupplierCategoryIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Category");
            }
            if (fieldName == "SelectedArticleSupplierPaymentIndex")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Payment Terms");
            }
            if (fieldName == "SelectedIndexOrderReception")
            {
                if (fieldValue != null && (fieldValue.Equals(-1) || fieldValue.Equals(0)))
                    errorMessage = string.Format("You cannot leave the {0} field empty.", "Order Reception");
            }
            //[rdixit][GEOS2-4643][14.07.2023]
            else if (fieldName == "InformationError")
            {              
                if (fieldValue != null)
                    errorMessage = string.Format("You cannot leave the information empty.");
            }

            if (fieldName== "SelectedArticleSupplier")
            {
                if (fieldValue!=null&&fieldValue.Equals("---"))
                {
                    errorMessage = string.Format("You cannot leave the country empty.");
                }
            }
            //if (fieldName == "SelectedArticleSupplierCategory")
            //{
            //    if (fieldValue == "---" )
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Category");
            //}
            //if (fieldName == "SelectedArticleSupplierPayment")
            //{
            //    if (fieldValue == "---")
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Payment Terms");
            //}
            //if (fieldName == "SelectedOrderReception")
            //{
            //    if (fieldValue == "---")
            //        errorMessage = string.Format("You cannot leave the {0} field empty.", "Order Reception");
            //}
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
