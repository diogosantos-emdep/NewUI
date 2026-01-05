using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Validations
{
    public class GridValidationRuleQuantity : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = Convert.ToDecimal(value);
            return val == 0 ? new ValidationResult(false, "Quantity can not be 0") : new ValidationResult(true, null);
          
        }
    }

    public class GridValidationRuleSerialNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                string val = value.ToString();
                return val == null ? new ValidationResult(false, "Serial Number can not be empty") : new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Serial Number can not be empty");
            }
           // return ValidationResult.ValidResult;
        }
    }
}
