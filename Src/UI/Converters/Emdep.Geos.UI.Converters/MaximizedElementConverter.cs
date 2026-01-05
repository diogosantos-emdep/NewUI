using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.UI.Converters
{
    public class MaximizedElementConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var control = values[1] as FlowLayoutControl;
            foreach (FrameworkElement elem in control.Children)
            {
                var data = values[0] as Emdep.Geos.Data.Common.PCM.ProductTypeImage;
                var data1 = values[0] as Emdep.Geos.Data.Common.PCM.DetectionImage;
                var data2 = values[0] as Emdep.Geos.Data.Common.PCM.PCMArticleImage;

                if (elem.DataContext == data)
                {
                    return elem;
                }
                if (elem.DataContext == data1)
                {
                    return elem;
                }
                if (elem.DataContext == data2)
                {
                    return elem;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                //var control = value as FrameworkElement;
                //var dataItem = control.DataContext as Emdep.Geos.Data.Common.PCM.ProductTypeImage;

                //var parent = control.Parent;
                //return new object[] { dataItem, parent };

                var control = value as FrameworkElement;
                var dataItem = control.DataContext as Emdep.Geos.Data.Common.PCM.ProductTypeImage;
                var dataItem1 = control.DataContext as Emdep.Geos.Data.Common.PCM.DetectionImage;
                var dataItem2 = control.DataContext as Emdep.Geos.Data.Common.PCM.PCMArticleImage;

                var parent = control.Parent;

                if (dataItem != null)
                {
                    if (dataItem == control.DataContext as Emdep.Geos.Data.Common.PCM.ProductTypeImage)
                    {
                        return new object[] { dataItem, parent };
                    }
                }

                if (dataItem1 != null)
                {
                    if (dataItem1 == control.DataContext as Emdep.Geos.Data.Common.PCM.DetectionImage)
                    {
                        return new object[] { dataItem1, parent };
                    }
                }

                if (dataItem2 != null)
                {
                    if (dataItem2 == control.DataContext as Emdep.Geos.Data.Common.PCM.PCMArticleImage)
                    {
                        return new object[] { dataItem2, parent };
                    }
                }
                return new object[] { };
            }
            else return null;
        }
    }
}
