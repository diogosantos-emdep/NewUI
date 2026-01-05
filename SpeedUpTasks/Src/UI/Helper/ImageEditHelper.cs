using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Internal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.UI.Helper
{
    public class ImageEditHelper : ImageEdit
    {
        static string base64String = null;
        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register("ImagePath", typeof(string), typeof(ImageEditHelper), null);
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }
        public static string Base64String
        {
            get { return base64String; }
            set { base64String = value; }
        }
        protected override void LoadCore()
        {
            if (Image == null)
                return;

            ImageSource image = LoadImage();

            if (image != null)
                EditStrategy.SetImage(image);
        }

        public override void Clear()
        {
           Source = null;
           Base64String = null;
        }
        ImageSource LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = EditorLocalizer.GetString(EditorStringId.ImageEdit_OpenFileFilter);

            if (dlg.ShowDialog() == true)
            {
                using (Stream stream = dlg.OpenFile())
                {
                    if (stream is FileStream)
                        ImagePath = ((FileStream)stream).Name;
                    string extention = Path.GetExtension(ImagePath);
                    Image ResizedImage;
                    MemoryStream ms = new MemoryStream(stream.GetDataFromStream());

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(ImagePath))
                    {
                        //int iWidth = (int)Math.Round((decimal)image.Width);
                        //int iHeight = (int)Math.Round((decimal)image.Height);

                        Bitmap b = new Bitmap(96, 96);
                        Graphics g = Graphics.FromImage((Image)b);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(image,0,0,96,96);
                        g.Dispose();
                        ResizedImage = (Image)b;
                    }

                    using (System.Drawing.Image image = ResizedImage)
                    {
                        using (MemoryStream m = new MemoryStream())
                        {                   
                            if(extention==".png")
                            {
                                image.Save(m, ImageFormat.Png);
                                //image.Save(@"C:\Users\lsharma\Desktop\Outlook\abc1.Png", ImageFormat.Png);
                            }
                            else
                            {
                                image.Save(m, ImageFormat.Jpeg);
                                //image.Save(@"C:\Users\lsharma\Desktop\Outlook\abc1.jpeg", ImageFormat.Jpeg);
                            }

                            byte[] imageBytes = m.ToArray();
                            Base64String = Convert.ToBase64String(imageBytes);
                                
                            MemoryStream mss = new MemoryStream(m.GetDataFromStream());
                            ms = mss;
                        }
                    }
                    return ImageHelper.CreateImageFromStream(ms);
                }
            }
            return null;
        }

    }
}
