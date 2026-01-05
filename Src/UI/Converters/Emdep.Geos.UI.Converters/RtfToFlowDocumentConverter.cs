using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{

    public class RtfToFlowDocumentConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            var doc = new FlowDocument();
            TextRange range = null;
            if (value != null)
            {
                if (value.ToString() != string.Empty)
                {
                    doc = new FlowDocument();
                    doc.FontFamily = GeosApplication.Instance.FontFamilyAsPerTheme;
                    MemoryStream stream = new MemoryStream(encoding.GetBytes(System.Convert.ToString(value)));
                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
            }
            return doc;       
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

    }
}
