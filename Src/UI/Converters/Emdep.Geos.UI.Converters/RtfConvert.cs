using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class RtfConvert : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string rtfFromRtb = string.Empty;
            var doc = new FlowDocument();
            if (value != null)
            {
                if (value.ToString() != string.Empty)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {

                        TextRange range2 = new TextRange(doc.ContentStart, doc.ContentEnd);
                        range2.Save(ms, DataFormats.Rtf);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            rtfFromRtb = sr.ReadToEnd();
                        }
                    }
                }

            }
            doc.Blocks.Clear();
            return rtfFromRtb;
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
