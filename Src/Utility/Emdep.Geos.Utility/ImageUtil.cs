using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// ImageUtil class use for getting information of Image in application
    /// </summary>
    public class ImageUtil
    {
        public byte[] ImageBytes { get; set; }
        /// <summary>
        /// This method is to change color of image
        /// </summary>
        /// <param name="scrBitmap">Get image in bitmap</param>
        /// <param name="color">Get color to change</param>
        /// <returns>Changed Image in bitmap</returns>
        public static Bitmap ChangeColor(Bitmap scrBitmap, Color color)
        {
            //You can change your new color here. Red,Green,LawnGreen any..
            Color newColor = color;
            Color actualColor;
            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150)
                        newBitmap.SetPixel(i, j, newColor);
                    else
                        newBitmap.SetPixel(i, j, actualColor);
                }
            }
            return newBitmap;
        }


        /// <summary>
        /// Downloads image data from the specified URL using WebClient.
        /// </summary>
        /// <param name="URL">The URL of the image to download.</param>
        /// <returns>A byte array containing the image data, or null if an error occurs.</returns>
        /// Shubham[skadam] GEOS2-8118 Due to protocol in service get error where images get from api in workbench 15 05 2025
        public static byte[] GetImageByWebClient(string URL)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                using (WebClient webClient = new WebClient())
                {
                    return webClient.DownloadData(URL);
                }
            }
            catch (Exception ex) { return null; }
        }
    }
}

