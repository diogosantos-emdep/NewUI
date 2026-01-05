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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Emdep.Geos.UI.Converters
{
    public class EmployeeCodeInitialToImageConverter : IValueConverter
    {
		//[nsatpute][11-02-2025][GEOS2-6726]
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #region
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string EmployeeCodeWithInitials = value as string;
            string EmployeeCode = string.Empty;
            string initial = string.Empty;
            string[] splitted = null;
            if (!EmployeeCodeWithInitials.Contains("_"))
                return null;
            if (!EmployeeCodeWithInitials.StartsWith("_"))
            {
                splitted = EmployeeCodeWithInitials.Split('_');
                EmployeeCode = splitted[0];
                initial = splitted[1];
            }
            else
            {
                initial = EmployeeCodeWithInitials.Replace("_",string.Empty);
            }
            string filePath = string.Empty;
            filePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/" + EmployeeCode.Trim() + ".png";
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
                        if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == EmployeeCodeWithInitials.ToLower()))
                        {
                            ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == EmployeeCodeWithInitials.ToLower()).Value;
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
                                ImageBytes = CreateImageWithInitials(initial);
                            }
                            GeosApplication.ImageUrlBytePair.Add(EmployeeCodeWithInitials, ImageBytes);
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
        public byte[] CreateImageWithInitials(string initials, int diameter = 100)
        {
            using (Bitmap bitmap = new Bitmap(diameter, diameter))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Set background color to white
                    graphics.Clear(Color.White);
                    // Define the circular clipping path
                    GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(0, 0, diameter, diameter);
                    Region region = new Region(path);
                    graphics.SetClip(region, CombineMode.Replace);
                    // Define the font
                    Font font = new Font("Arial", diameter / 3, FontStyle.Bold, GraphicsUnit.Point);
                    Brush brush = new SolidBrush(Color.Black);
                    // Calculate the position to center the text
                    SizeF textSize = graphics.MeasureString(initials, font);
                    PointF position = new PointF(
                        (diameter - textSize.Width) / 2,
                        (diameter - textSize.Height) / 2);
                    // Draw the text
                    graphics.DrawString(initials, font, brush, position);
                    // Save the image to a memory stream
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        return stream.ToArray();
                    }
                }
            }
        }
        #endregion
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}