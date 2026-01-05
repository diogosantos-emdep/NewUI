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
    public class DateCellColorCompareToCloseDateConverter : MarkupExtension, IMultiValueConverter
    {
        #region IValueConverter Members
        // [001][15-04-2022][cpatil][GEOS2-2976]
        // Created for CRM ->Actions (If ths action closed date the due date nveer can't be red) 
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values[0] == DependencyProperty.UnsetValue)
                {
                    return false;
                }
                int StatusId = (int)values[2];//[rdixit][GEOS2-4667][14.07.2023]
                DateTime? CloseDate = (DateTime?)values[1];
                if (values[0] != System.DBNull.Value && StatusId!=266)
                {
                    if (CloseDate == null )
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
