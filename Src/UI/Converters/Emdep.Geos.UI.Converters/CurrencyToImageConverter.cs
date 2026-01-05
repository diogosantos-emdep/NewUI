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
using DevExpress.Xpf.Grid;
namespace Emdep.Geos.UI.Converters
{
    //[nsatpute][20-02-2025][GEOS2-6722]
    public class CurrencyToImageConverter : IValueConverter
    {
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #region
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string currencyCode = string.Empty;
            if (value is EditGridCellData editGridCellData)
            {
                if (editGridCellData.Value != null)
                    currencyCode = editGridCellData.Value.ToString();
            }
            else
            {
                currencyCode = System.Convert.ToString(value);
            }
            string filePath = string.Empty;
            filePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Currencies/" + currencyCode.Trim() + ".png";
            byte[] ImageInBytes = null;
            BitmapImage OwnerImage;
            if (GeosApplication.isServiceDown == false)
            {
                try
                {
                    if (GeosApplication.ImageUrlBytePair == null)
                        GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        byte[] ImageBytes = null;
                        if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == currencyCode.ToLower()))
                        {
                            ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == currencyCode.ToLower()).Value;
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
                            GeosApplication.ImageUrlBytePair.Add(currencyCode, ImageBytes);
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
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            byte[] ImageBytes = null;
                            ImageBytes = GeosService.GetImagesByUrl(filePath);
                            GeosApplication.ImageUrlBytePair.Add(filePath, ImageBytes);
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
        #endregion
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}