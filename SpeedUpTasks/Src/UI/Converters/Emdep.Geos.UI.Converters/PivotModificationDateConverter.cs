
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using DevExpress.Xpf.PivotGrid.Internal;
using DevExpress.Xpf.PivotGrid;


namespace Emdep.Geos.UI.Converters
{
    public class PivotModificationDateConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FieldValueItem fieldValueContext = value as FieldValueItem;
            PivotGridField PivotGridField=value as PivotGridField;
            if (fieldValueContext == null)
                return string.Empty;

            DevExpress.XtraPivotGrid.PivotDrillDownDataSource ds = fieldValueContext.Item.CreateDrillDownDataSource();
            PivotGridField.GroupInterval = FieldGroupInterval.Date;
            return ds[0]["LastUpdate"].ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
