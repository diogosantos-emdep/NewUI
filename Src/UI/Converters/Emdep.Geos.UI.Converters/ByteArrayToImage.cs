using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Converters
{
    public class ByteArrayToImage : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {          

            ImageSource imgSrc = null;
           
            try
            {
                if (value != null)
                {
                    byte[] byteArrayIn = null;
                    BitmapImage biImg = new BitmapImage();
                    if (value.GetType().Name != "Byte[]")
                    {
                        DevExpress.XtraPivotGrid.PivotDrillDownDataSource source = ((DevExpress.Xpf.PivotGrid.Internal.FieldValueItem)value).Item.CreateDrillDownDataSource();
                        byteArrayIn = (byte[])source[0]["Image"];
                    }
                   else
                    {
                        byteArrayIn = (byte[])value;
                    }                 

                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    biImg.DecodePixelHeight = 10;
                    biImg.DecodePixelWidth = 10;
                    imgSrc = biImg as ImageSource;
                    return imgSrc;
                }
                
            }
            catch (Exception ex)
            {

            }

            return imgSrc;
            //return string.Format("{0} {1}", System.Convert.ToString(source[0]["Type"]), imgSrc);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
