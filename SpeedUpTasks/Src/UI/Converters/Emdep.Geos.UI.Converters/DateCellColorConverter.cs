using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI.Common;
using System.Globalization;
using System.Windows;

namespace Emdep.Geos.UI.Converters
{
   public class DateCellColorConverter : MarkupExtension, IMultiValueConverter
    {
        #region IValueConverter Members
        // Created for Leads offer close date display in red based on date and statusid condition.
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                {
                    return false;
                }

                Int32 StatusId = (Int32)values[1];
                if (values[0] != System.DBNull.Value)
                {
                    if (StatusId != 4 && StatusId != 17)
                    {
                        if ((values[0] != null && ((DateTime)(values[0]) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)))
                        {
                            return true;
                        }
                    }

                }
                return false;
            }
            catch
            {
                return false;
            }
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
