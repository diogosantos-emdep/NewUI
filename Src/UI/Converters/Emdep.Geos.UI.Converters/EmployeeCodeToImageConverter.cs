using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using System.Windows.Data;

namespace Emdep.Geos.UI.Converters
{
    //[nsatpute][GEOS2-5388][30-01-2025]
    public class EmployeeCodeToImageConverter: IValueConverter
    {
            
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #region
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string EmployeeCodeWithIdGender = value as string;
            string[] splitted = EmployeeCodeWithIdGender.Split('_');
            string EmployeeCode = splitted[0];
            int IdGender = int.Parse(splitted[1]);
            string filePath = string.Empty;
            filePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/" + EmployeeCode + ".png";
            byte[] ImageInBytes = null;
            BitmapImage OwnerImage;
            try
            {
                if (GeosApplication.ImageUrlBytePair == null)
                    GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();

                if (!string.IsNullOrEmpty(filePath))
                {
                    byte[] ImageBytes = null;
                    if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == EmployeeCodeWithIdGender.ToLower()))
                    {
                        ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == EmployeeCodeWithIdGender.ToLower()).Value;
                    }
                    else
                    {
                        if (GeosApplication.IsImageURLException == false)
                        {
                            //using (WebClient webClient = new WebClient())
                            //{
                            //    ImageBytes = webClient.DownloadData(filePath);
                            //}
                            ImageBytes = Utility.ImageUtil.GetImageByWebClient(filePath);
                        }
                        else
                        {
                            ImageBytes = GeosService.GetImagesByUrl(filePath);
                        }
                        if (ImageBytes == null || ImageBytes.Length <= 0)
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (IdGender == 1)
                                {
                                    OwnerImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                                }
                                else
                                {
                                    OwnerImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                                }

                            }
                            else
                            {
                                if (IdGender == 1)
                                {
                                    OwnerImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserBlue.png", UriKind.RelativeOrAbsolute));
                                }
                                else
                                {
                                    OwnerImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserBlue.png", UriKind.RelativeOrAbsolute));
                                }
                            }
                            ImageBytes = BitmapImageToByteArray(OwnerImage);
                        }
                        GeosApplication.ImageUrlBytePair.Add(EmployeeCodeWithIdGender, ImageBytes);
                    }
                    return ImageBytes;
                }
            }
            catch (Exception ex)
            {
                // Added code to get https data for the user which is out side of the VPN  [rdixit][24.05.2023]
                if (GeosApplication.IsImageURLException == false)
                    GeosApplication.IsImageURLException = true;

                if (!string.IsNullOrEmpty(filePath))
                {
                    byte[] ImageBytes = null;
                    ImageBytes = GeosService.GetImagesByUrl(filePath);
                    GeosApplication.ImageUrlBytePair.Add(filePath, ImageBytes);
                    return ImageBytes;
                }
            }
            return null;
        }
        public byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] byteArray = null;
            try
            {
                // Create a memory stream to hold the image data
                using (MemoryStream stream = new MemoryStream())
                {
                    // Use a PNG encoder to encode the BitmapImage to the stream
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(stream);

                    // Convert the stream to a byte array
                    byteArray = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
            return byteArray;
        }
        #endregion

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
