using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class ValidationRulesCollection : List<ValidationRule> { }
    public static class GridColumnValidationHelper
    {
        public static bool GetAllowValidation(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowValidationProperty);
        }

        public static void SetAllowValidation(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowValidationProperty, value);
        }

        // Using a DependencyProperty as the backing store for AllowValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowValidationProperty =
            DependencyProperty.RegisterAttached("AllowValidation", typeof(bool), typeof(GridColumnValidationHelper), new PropertyMetadata(true));

        public static ValidationRulesCollection GetValidationRules(GridColumn obj)
        {
            return (ValidationRulesCollection)obj.GetValue(ValidationRulesProperty);
        }

        public static void SetValidationRules(GridColumn obj, ValidationRulesCollection value)
        {
            obj.SetValue(ValidationRulesProperty, value);
        }

        public static readonly DependencyProperty ValidationRulesProperty =
            DependencyProperty.RegisterAttached("ValidationRules", typeof(ValidationRulesCollection), typeof(GridColumnValidationHelper), new PropertyMetadata(null, OnValidationRulesChanged));

        static void OnValidationRulesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridColumn column = d as GridColumn;
            if (column == null)
                return;
            column.Validate -= new GridCellValidationEventHandler(OnColumnValidate);
            if (e.NewValue != null)
                column.Validate += new GridCellValidationEventHandler(OnColumnValidate);
        }

        static void OnColumnValidate(object sender, GridCellValidationEventArgs e)
        {
            GridColumn column = (GridColumn)sender;
            if (!GetAllowValidation(column.View))
            {
                e.IsValid = true;
                return;
            }
            foreach (ValidationRule rule in GetValidationRules(column))
            {
                ValidationResult result = rule.Validate(e.Value, CultureInfo.CurrentCulture);
                if (!result.IsValid)
                {
                    e.IsValid = false;
                    e.ErrorContent = result.ErrorContent;
                    break;
                }
            }

        }

    }
   
}
