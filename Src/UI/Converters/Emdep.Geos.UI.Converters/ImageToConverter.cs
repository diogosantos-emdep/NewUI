using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Converters
{
        /// <summary>
        /// One-way converter from System.Drawing.Image to System.Windows.Media.ImageSource
        /// </summary> 
        /// 
        [ValueConversion(typeof(System.Drawing.Image), typeof(System.Windows.Media.ImageSource))]
        public class ImageToConverter : IValueConverter
        {
            
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string path = value as string;

                if (path == null || !File.Exists(path))
                    return null;

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                return bmp;
            }
            public object ConvertBack(object value, Type targetType,
                      object parameter, CultureInfo culture)
            {
                return null;
            }
        }

    

}
