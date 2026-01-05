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
        private List<object> selectedwarehouseList;
        private bool isPermissionReadOnly;
        private bool isPermissionEnabled;
        private bool isPermissionAuditor;
        private Warehouses selectedwarehouse;
        private string beepOkFilePath;
        private string beepNotOkFilePath;
        private bool isPickingTimer;
        private bool isWarehouseInInventory;
        private bool isPermissionAdmin;
        //[000] added
        private bool isRefillEmptyLocationsFirst;
        private int articleSleepDays;
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

        public Warehouses Selectedwarehouse
        {
            get { return selectedwarehouse; }
            set
            {
                selectedwarehouse = value;
                OnPropertyChanged("Selectedwarehouse");
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



        //public string FillStorageArticle { get; set; }
        //public bool IsPickingTimer { get; set; }
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

        #endregion

    }
}
