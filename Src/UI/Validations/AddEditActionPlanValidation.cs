using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{//[Sudhir.Jangra][GEOS2-5978]
    public class AddEditActionPlanValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            //[GEOS2-6496][27.09.2024][rdixit]
            if (fieldName == "SelectedLocationIndex")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("-1"))
                    errorMessage = string.Format("You cannot leave the Location empty.");
            }
            if (fieldName == "SelectedResponsibleIndex")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("-1"))
                    errorMessage = string.Format("You cannot leave the Responsible empty.");
            }
            if (fieldName == "SelectedOriginIndex")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("-1"))
                    errorMessage = string.Format("You cannot leave the Origin empty.");
            }
            //[Shweta.Thube][GEOS2-6586]
            if (fieldName == "SelectedBusinessIndex")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("-1"))
                    errorMessage = string.Format("You cannot leave the Business Unit empty.");
            }
            //[nsatpute][05-03-2025][GEOS2-7059]
            if (fieldName == "SelectedDepartmentIndex")
            {
                if (fieldValue == null || fieldValue.ToString().Equals("-1"))
                    errorMessage = string.Format("You cannot leave the Department empty.");
            }
            //[pallavi.kale][GEOS2-8218]
            if (fieldName == "Description")
            {
                if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString().Trim()))
                    errorMessage = string.Format("You cannot leave the Name empty.");
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
