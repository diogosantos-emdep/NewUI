using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Net;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;

namespace Emdep.Geos.UI.Converters
{
    public class ImageUrlToByteArrayConverter : IValueConverter
    {
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IGeosRepositoryService GeosService = new GeosRepositoryServiceController("localhost:6699");
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the input value (path to the image file) to a byte array of the image [rdixit][GEOS2-4419][27.04.2023]
            string ImageUrl = value as string;
            if (GeosApplication.isServiceDown == false)
            {
                try
                {
                    if (GeosApplication.ImageUrlBytePair == null)
                        GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();

                    if (!string.IsNullOrEmpty(ImageUrl))
                    {
                        byte[] ImageBytes = null;
                        if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == ImageUrl.ToLower()))
                        {
                            ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == ImageUrl.ToLower()).Value;
                        }
                        else
                        {
                            if (GeosApplication.IsImageURLException == false)
                            {
                                //using (WebClient webClient = new WebClient())
                                //{
                                //    ImageBytes = webClient.DownloadData(ImageUrl);
                                //}
                                ImageBytes = Utility.ImageUtil.GetImageByWebClient(ImageUrl);
                            }
                            else
                            {
                                ImageBytes = GeosService.GetImagesByUrl(ImageUrl);
                            }
                            GeosApplication.ImageUrlBytePair.Add(ImageUrl, ImageBytes);
                        }
                        return ImageBytes;
                    }
                }
                catch (Exception ex)
                {
                    
                    // Added code to get https data for the user which is out side of the VPN  [rdixit][24.05.2023]
                    if (GeosApplication.IsImageURLException == false)
                        GeosApplication.IsImageURLException = true;
                    try
                    {
                        if (!string.IsNullOrEmpty(ImageUrl))
                        {
                            byte[] ImageBytes = null;
                            ImageBytes = GeosService.GetImagesByUrl(ImageUrl);
                            GeosApplication.ImageUrlBytePair.Add(ImageUrl, ImageBytes);
                            return ImageBytes;
                        }
                    }
                    catch (Exception exforVPN)
                    {
                        GeosApplication.isServiceDown = true;
                    }
                }
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
