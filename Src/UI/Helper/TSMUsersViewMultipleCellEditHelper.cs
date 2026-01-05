using DevExpress.Xpf.Grid;

using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class TSMUsersViewMultipleCellEditHelper
    {
        #region//[pallavi.kale][GEOS2-5388]
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TSMUsersViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
        private static bool isValueChanged;
        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }
        public static bool GetIsEnabled(UIElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void IsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TableView view = source as TableView;
            
        }
        private static bool isLoad;
        public static bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; }
        }
        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set
            {
                if (!GeosApplication.Instance.IsAPMActionPlanPermission)
                {
                    isValueChanged = false;
                }
                else
                {
                    isValueChanged = value;
                }
            }
        }
        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
        public static string Checkview { get; set; }

        public static TableView viewtableview;
        public static TableView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
                Clonedviewtableview = viewtableview;
            }
        }
        private static TableView clonedviewtableview;
        public static TableView Clonedviewtableview
        {
            get { return clonedviewtableview; }
            set
            {
                clonedviewtableview = value;
            }
        }

        
      
        public static readonly DependencyProperty IsValueChangedProperty =
         DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(TSMUsersViewMultipleCellEditHelper), new PropertyMetadata(false));
        public static void SetIsValueChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }

      
#endregion
    }
}
