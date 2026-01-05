using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using Prism.Logging;
using DevExpress.Xpf.LayoutControl;
using System.ComponentModel;

namespace Emdep.Geos.Modules.PCM.Common_Classes
{

    public sealed class PCMCommon : Prism.Mvvm.BindableBase
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        #endregion

        #region Task Logs

        /// <summary>
        /// [000][skhade][09-08-2019]Added Singleton class to define common properties and methods for PCM.
        /// </summary>

        #endregion //Task Logs

        #region Declarations

        private string pCM_Appearance;
        MaximizedElementPosition maximizedElementPosition;


        #endregion //Declarations

        #region Properties

        public string PCM_Appearance
        {
            get { return pCM_Appearance; }
            set
            {
                pCM_Appearance = value;
                OnPropertyChanged("PCM_Appearance");
            }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged("MaximizedElementPosition");
            }
        }

        #endregion //Properties

        #region Singleton object

        //Singleton object
        private static readonly PCMCommon instance = new PCMCommon();

        #endregion //Singleton object

        #region Constructor

        private PCMCommon()
        {
        }

        #endregion //Constructor

        #region Public Properties

        public static PCMCommon Instance
        {
            get { return instance; }
        }

        #endregion //Properties

        #region Common Methods

        public Boolean Rotate(Bitmap bmp)
        {
            PropertyItem pi = bmp.PropertyItems.Select(x => x)
                                               .FirstOrDefault(x => x.Id == 0x0112);
            if (pi == null) return false;

            byte o = pi.Value[0];

            if (o == 2) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if (o == 3) bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            if (o == 4) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            if (o == 5) bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            if (o == 6) bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (o == 7) bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
            if (o == 8) bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);

            return true;
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public ImageSource GetByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn != null)
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    Bitmap bmp = new Bitmap(ms);

                    if (PCMCommon.Instance.Rotate(bmp))
                        return Convert(bmp);
                    else
                        return ByteArrayToImage(byteArrayIn);
                }
            }
            return null;
        }

        public ImageSource ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ByteArrayToImage." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        public MaximizedElementPosition SetMaximizedElementPosition()
        {

            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                        return MaximizedElementPosition;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString(), true);
                        return MaximizedElementPosition;
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                    return MaximizedElementPosition;
                }
            }
            return MaximizedElementPosition;
        }


        #endregion //Common Methods

    }
}
