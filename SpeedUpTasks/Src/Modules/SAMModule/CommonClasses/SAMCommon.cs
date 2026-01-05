using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SAM
{
    /// <summary>
    /// [001][skhade][2019-09-25][GEOS2-1757] Create new module SAM (STRUCTURE ASSEMBLY MANAGER).
    /// [002][skale][13-12-2019][GEOS2-1953] At the time to scan OT & operator code by normal way that time beep sound not get
    /// </summary>
    public sealed class SAMCommon : Prism.Mvvm.BindableBase
    {
        private static readonly SAMCommon instance = new SAMCommon();

        #region Declaration

        private List<Company> plantOwnerList;
        private List<object> selectedPlantOwnerList;
        private string beepOkFilePath;
        private string beepNotOkFilePath;
        private List<Article> articles = new List<Article>();

        #endregion // Services

        #region Public Properties

        public static SAMCommon Instance
        {
            get { return instance; }
        }

        public List<Company> PlantOwnerList
        {
            get { return plantOwnerList; }
            set
            {
                plantOwnerList = value;
                OnPropertyChanged("PlantOwnerList");
            }
        }

        public List<object> SelectedPlantOwnerList
        {
            get { return selectedPlantOwnerList; }
            set
            {
                selectedPlantOwnerList = value;
                OnPropertyChanged("SelectedPlantOwnerList");
            }
        }

        //[002] added
        public string BeepOkFilePath
        {
            get { return beepOkFilePath; }
            set { beepOkFilePath = value; }
        }

        public string BeepNotOkFilePath
        {
            get { return beepNotOkFilePath; }
            set { beepNotOkFilePath = value; }
        }

        /// <summary>
        /// This is created to store and get images
        /// </summary>
        public List<Article> Articles
        {
            get { return articles; }
            set
            {
                articles = value;
                OnPropertyChanged("Articles");
            }
        }

        //end

        #endregion

        #region Constructor
        /// <summary>
        /// [002][skale][13-12-2019][GEOS2-1953] At the time to scan OT & operator code by normal way that time beep sound not get
        /// </summary>
        public SAMCommon()
        {
            //[002]added
            BeepOkFilePath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\Sounds\\BeepOK.wav";
            BeepNotOkFilePath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\Sounds\\BeepNOK.wav";
            //end
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
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
                GeosApplication.Instance.Logger.Log("Error in ByteArrayToBitmapImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
       public ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        #endregion
    }
}
