using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.TSM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.TSM.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.ComponentModel;

namespace Emdep.Geos.Modules.TSM.CommonClass
{
    public sealed class TSMCommon : Prism.Mvvm.BindableBase
    {
        //[GEOS2-5388][pallavi.kale][13.01.2025]
        private static readonly TSMCommon instance = new TSMCommon();

        #region Declaration
        private List<Company> plantOwnerList;//[GEOS2-8963][pallavi.kale][28.11.2025]
        private List<object> selectedPlantOwnerList;//[GEOS2-8963][pallavi.kale][28.11.2025]
        private List<Ots> pendingOrdersList;//[GEOS2-8963][pallavi.kale][28.11.2025]
        private string fromDate;//[GEOS2-8963][pallavi.kale][28.11.2025]
        private string toDate;//[GEOS2-8963][pallavi.kale][28.11.2025]
        #endregion

        #region Public Properties
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        public static TSMCommon Instance
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
        public List<Ots> PendingOrdersList
        {
            get { return pendingOrdersList; }
            set
            {
                pendingOrdersList = value;
                OnPropertyChanged("PendingOrdersList");
            }
        }
        public string FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public string ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }
        #endregion

        #region Constructor
        public TSMCommon()
        {
        }
        #endregion

        #region Common Methods
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
        public ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
        #endregion
    }
}
