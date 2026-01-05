using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse
{
    public sealed class WarehouseCommon : Prism.Mvvm.BindableBase
    {
        #region Task Log
        // [000][skale][24-07-2019][GEOS2-1667]Priorize first empty locations in Refill
        // [001][GEOS2-1656][Add article Sleeping days][23-09-2019][sdesai]
        // [002][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        // [003][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        #endregion

        #region

        //Singleton object
        private static readonly WarehouseCommon instance = new WarehouseCommon();
        private List<Warehouses> warehouseList;
        private List<Warehouses> isRegionalWarehouseList;
        private List<object> selectedwarehouseList;
        private bool isPermissionReadOnly;
        private bool isPermissionEnabled;
        private bool isPermissionAuditor;
        private bool isPermissionEditLockedStock;
        private Warehouses selectedwarehouse;
        private Warehouses selectedStockBySupplierwarehouse;
        private string beepOkFilePath;
        private string beepNotOkFilePath;
        private bool isPickingTimer;
        private bool isWarehouseInInventory;
        private bool isPermissionAdmin;
        //[000] added
        private bool isRefillEmptyLocationsFirst;
        private int articleSleepDays;
        private int articleSleepMonths;
        private string selectedScaleModel;
        private string selectedParity;
        private string selectedPort;
        private string selectedStopBit;
        private string selectedBaudRate;
        private string selectedDataBit;
        //[002] added
        private string appearance;
        //[003]added
        private bool isStockRegularizationApproval;
        private bool isViewFinancialDataPermission;

        private double scaleWeight = 00;
        private bool isPermissionUpdateArticleSetting; // [nsatpute][28-04-2025][GEOS2-6502]
        private bool isPermissionAdminTransportFrequencySetting; //[nsatpute][GEOS2-9362][17.11.2025]
        #endregion

        #region Constructor

        public WarehouseCommon()
        {
            BeepOkFilePath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\Sounds\\BeepOK.wav";
            BeepNotOkFilePath = AppDomain.CurrentDomain.BaseDirectory + "Assets\\Sounds\\BeepNOK.wav";
            //SelectedItemChangedEvent wont Occur where default warehouse is setted
            IsWarehouseChangedEventCanOccur = true;
        }

        #endregion

        #region Public Properties

        public static WarehouseCommon Instance
        {
            get { return instance; }
        }

        public bool IsWarehouseChangedEventCanOccur { get; set; }
        public List<Warehouses> WarehouseList
        {
            get { return warehouseList; }
            set
            {
                warehouseList = value;
                this.OnPropertyChanged("WarehouseList");
            }
        }
        public List<Warehouses> IsRegionalWarehouseList
        {
            get { return isRegionalWarehouseList; }
            set
            {
                isRegionalWarehouseList = value;
                this.OnPropertyChanged("IsRegionalWarehouseList");
            }
        }

        public bool IsPermissionReadOnly
        {
            get { return isPermissionReadOnly; }
            set
            {
                isPermissionReadOnly = value;
                OnPropertyChanged("IsPermissionReadOnly");
            }
        }

        public bool IsPermissionEnabled
        {
            get { return isPermissionEnabled; }
            set
            {
                isPermissionEnabled = value;
                OnPropertyChanged("IsPermissionEnabled");
            }
        }

        public bool IsPermissionAuditor
        {
            get { return isPermissionAuditor; }
            set
            {
                isPermissionAuditor = value;
                OnPropertyChanged("IsPermissionAuditor");
            }
        }

        public bool IsPermissionEditLockedStock
        {
            get { return isPermissionEditLockedStock; }
            set { isPermissionEditLockedStock = value;
                OnPropertyChanged("IsPermissionEditLockedStock");
            }
        }
		
		// [nsatpute][28-04-2025][GEOS2-6502]
        public bool IsPermissionUpdateArticleSetting
        {
            get { return isPermissionUpdateArticleSetting; }
            set
            {
                isPermissionUpdateArticleSetting = value;
                OnPropertyChanged("IsPermissionUpdateArticleSetting");
            }
        }
		//[nsatpute][GEOS2-9362][17.11.2025]
        public bool IsPermissionAdminTransportFrequencySetting
        {
            get { return isPermissionAdminTransportFrequencySetting; }
            set
            {
                isPermissionAdminTransportFrequencySetting = value;
                OnPropertyChanged("IsPermissionAdminTransportFrequencySetting");
            }
        }

        public Warehouses Selectedwarehouse
        {
            get { return selectedwarehouse; }
            set
            {
                selectedwarehouse = value;
                OnPropertyChanged("Selectedwarehouse");
            }
        }
        

        public Warehouses SelectedStockBySupplierwarehouse
        {
            get { return selectedStockBySupplierwarehouse; }
            set
            {
                selectedStockBySupplierwarehouse = value;
                OnPropertyChanged("SelectedStockBySupplierwarehouse");
            }
        }
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

        public bool IsPickingTimer
        {
            get
            {
                return isPickingTimer;
            }

            set
            {
                isPickingTimer = value;
                OnPropertyChanged("IsPickingTimer");
            }
        }

        public bool IsWarehouseInInventory
        {
            get
            {
                return isWarehouseInInventory;
            }

            set
            {
                isWarehouseInInventory = value;
                OnPropertyChanged("IsWarehouseInInventory");
            }
        }


        public bool IsPermissionAdmin
        {
            get
            {
                return isPermissionAdmin;
            }

            set
            {
                isPermissionAdmin = value;
                OnPropertyChanged("IsPermissionAdmin");
            }
        }

        //[000] added
        public bool IsRefillEmptyLocationsFirst
        {
            get
            {
                return isRefillEmptyLocationsFirst;
            }
            set
            {
                isRefillEmptyLocationsFirst = value;
                OnPropertyChanged("IsRefillEmptyLocationsFirst");
            }
        }

        //[001] added
        public int ArticleSleepDays
        {
            get
            {
                return articleSleepDays;
            }

            set
            {
                articleSleepDays = value;
                OnPropertyChanged("ArticleSleepDays");
            }
        }
        public int ArticleSleepMonths
        {
            get
            {
                return articleSleepMonths;
            }

            set
            {
                articleSleepMonths = value;
                OnPropertyChanged("ArticleSleepMonths");
            }
        }
        public string SelectedScaleModel
        {
            get
            {
                return selectedScaleModel;
            }

            set
            {
                selectedScaleModel = value;
                OnPropertyChanged("SelectedScaleModelIndex");
            }
        }
        public string SelectedParity
        {
            get
            {
                return selectedParity;
            }

            set
            {
                selectedParity = value;
                OnPropertyChanged("SelectedParity");
            }
        }

        public string SelectedPort
        {
            get
            {
                return selectedPort;
            }

            set
            {
                selectedPort = value;
                OnPropertyChanged("SelectedPort");
            }
        }

        public string SelectedStopBit
        {
            get
            {
                return selectedStopBit;
            }

            set
            {
                selectedStopBit = value;
                OnPropertyChanged("SelectedStopBit");
            }
        }
        public string SelectedBaudRate
        {
            get
            {
                return selectedBaudRate;
            }

            set
            {
                selectedBaudRate = value;
                OnPropertyChanged("SelectedBaudRate");
            }
        }

        public string SelectedDataBit
        {
            get
            {
                return selectedDataBit;
            }

            set
            {
                selectedDataBit = value;
                OnPropertyChanged("SelectedDataBit");
            }
        }
        //[002] added
        public string Appearance
        {
            get { return appearance; }
            set
            {
                appearance = value;
                OnPropertyChanged("Appearance");
            }
        }
        //[003] added
        public bool IsStockRegularizationApproval
        {
            get { return isStockRegularizationApproval; }
            set { isStockRegularizationApproval = value; OnPropertyChanged("IsStockRegularizationApproval"); }
        }

        public bool IsViewFinancialDataPermission
        {
            get { return isViewFinancialDataPermission; }
            set { isViewFinancialDataPermission = value; OnPropertyChanged("IsViewFinancialDataPermission"); }
        }

        //[csaid][08-12-2021][GEOS2-3366]
        public bool IsWMSEdit_Article_Properties { get; set; }

        //public string FillStorageArticle { get; set; }
        //public bool IsPickingTimer { get; set; }
        
        public double ScaleWeight
        {
            get { return scaleWeight; }
            set
            {
                scaleWeight = value;
                OnPropertyChanged("ScaleWeight");
            }
        }

		//Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        int inactivityMinutes;
        public int InactivityMinutes
        {
            get
            {
                return inactivityMinutes;
            }

            set
            {
                inactivityMinutes = value;
                OnPropertyChanged("InactivityMinutes");
            }
        }
        //[nsatpute][12.09.2025][GEOS2-8791]
        long selectedYear;
        public long SelectedYear
        {
            get
            {
                return selectedYear;
            }

            set
            {
                selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }
        #endregion //Properties

        #region Methods

        public ImageSource ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    biImg.DecodePixelHeight = 10;
                    biImg.DecodePixelWidth = 10;

                    ImageSource imgSrc = biImg as ImageSource;

                    GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....executed successfully", category: Category.Info, priority: Priority.Low);
                    return imgSrc;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ByteArrayToImage...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }


        //[Sudhir.Jangra][GEOS2-4271][25/05/2023]
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


        //[Sudhir.Jangra][GEOS2-4271][25/05/2023]
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
