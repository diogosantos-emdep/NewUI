using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common;
using System.Windows.Input;
using System.Windows;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Views;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Xpf.Accordion;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Hardware;
using Emdep.Geos.Hardware.Balances;
using System.IO.Ports;
using Emdep.Geos.Hardware.Balances.Sartorius;
using System.Timers;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpo.DB;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class PackingScanViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region TaskLog
        // [001][GEOS2-1631] WMS packaging
        // [002][Sprint_78][GEOS2-1889][Add a new setting to specify the customers requiring one order one box]
        // [003][Sprint_78][GEOS2-1991][Show connection indicator for Weighing Machine]
        #endregion

        #region Services
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private double windowWidth;
        private double windowHeight;
        private Visibility isBoxesVisible = Visibility.Visible;
        private ObservableCollection<PackingBoxType> packingBoxTypeList;
        private ObservableCollection<WOItem> packedItemsList;
        private ObservableCollection<PackingCompany> packingCompanyList;
        private ObservableCollection<WOItem> unPackedItemsList;
        private object selectedItem;
        private bool focusUserControl;
        private bool isScanBox;
        private bool isAutoWeighing;
        private bool isScanFromPackedItems;
        private Visibility packingBoxDetailsVisibility;
        private bool isScanDirectionArrow;
        private WOItem selectedUnPackedItem;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;
        private string wrongItem;
        private string barcodeStr;
        private WOItem selectedPackedItem;
        private ImageSource articleImage;
        private double estimatedWeight = 00;
        private PackingBox selectedPackingBox;
        private List<int> customersIdList;
        private int totalPackingBoxCount;
        private List<Company> companyList;
        private bool isBoxWeightTolerance;
        private double boxWeightTolerance;
        private double scaleWeight = 00;
        private int totalItems;
        IBalance balance;
        Timer timer;
        private string scaleWeightValue = "0KG";
        private GeosAppSetting geosAppSetting;
        private Ots oT;
        private int stateIndex;
        private string toolTip;
        private string strWeighingIsOff = "Auto Weighing is OFF";
        private string MachineConnected = "Machine is connected";
        private string MachineNOTConnected = "Machine is NOT connected";
        private Int64 idOTUnPackedItem;
        private Int64 idOTPackedItem;
        private string infoTooltipBackColor;
        #endregion

        #region Properties

        public double WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
            }
        }

        public double WindowHeight
        {
            get { return windowHeight; }
            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
            }
        }

        public Visibility IsBoxesVisible
        {
            get { return isBoxesVisible; }
            set
            {
                isBoxesVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBoxesVisible"));
            }
        }

        public ObservableCollection<PackingBoxType> PackingBoxTypeList
        {
            get { return packingBoxTypeList; }
            set
            {
                packingBoxTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingBoxTypeList"));
            }
        }

        public ObservableCollection<WOItem> PackedItemsList
        {
            get { return packedItemsList; }
            set
            {
                packedItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackedItemsList"));
            }
        }

        public ObservableCollection<PackingCompany> PackingCompanyList
        {
            get { return packingCompanyList; }
            set
            {
                packingCompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingCompanyList"));
            }
        }

        public ObservableCollection<WOItem> UnPackedItemsList
        {
            get { return unPackedItemsList; }
            set
            {
                unPackedItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UnPackedItemsList"));
            }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public bool FocusUserControl
        {
            get { return focusUserControl; }
            set
            {
                focusUserControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusUserControl"));
            }
        }

        public bool IsScanBox
        {
            get { return isScanBox; }
            set
            {
                isScanBox = value;
                FocusUserControl = true;
                SetBoxVisibilityTemplate();
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanBox"));
            }
        }

        public bool IsAutoWeighing
        {
            get { return isAutoWeighing; }
            set
            {
                isAutoWeighing = value;

                if (isAutoWeighing)
                    GetScaleWeight();
                else
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Enabled = false;
                        timer.Elapsed -= new ElapsedEventHandler(timer_Tick);

                    }
                    if (balance != null)
                    {
                        balance.Dispose();
                        balance = null;
                    }

                    StateIndex = 0;
                    ToolTip = strWeighingIsOff;
                }

                OnPropertyChanged(new PropertyChangedEventArgs("IsAutoWeighing"));
            }
        }

        public bool IsScanFromPackedItems
        {
            get { return isScanFromPackedItems; }
            set
            {
                isScanFromPackedItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanFromPackedItems"));
            }
        }

        public Visibility PackingBoxDetailsVisibility
        {
            get { return packingBoxDetailsVisibility; }
            set
            {
                packingBoxDetailsVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingBoxDetailsVisibility"));
            }
        }

        /// <summary>
        /// True -> when unpacked to packed
        /// False -> when packed to unpacked
        /// </summary>
        public bool IsScanDirectionArrow
        {
            get { return isScanDirectionArrow; }
            set
            {
                isScanDirectionArrow = value;
                FocusUserControl = true;
                SetErrorVisibilityTemplate();
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanDirectionArrow"));

            }
        }

        public WOItem SelectedUnPackedItem
        {
            get { return selectedUnPackedItem; }
            set
            {
                selectedUnPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUnPackedItem"));
            }
        }

        public Visibility IsWrongItemErrorVisible
        {
            get { return isWrongItemErrorVisible; }
            set
            {
                isWrongItemErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongItemErrorVisible"));
            }
        }

        public string WrongItem
        {
            get { return wrongItem; }
            set
            {
                wrongItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongItem"));
            }
        }

        public string BarcodeStr
        {
            get { return barcodeStr; }
            set
            {
                barcodeStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeStr"));
            }
        }

        public WOItem SelectedPackedItem
        {
            get { return selectedPackedItem; }
            set
            {
                selectedPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPackedItem"));
            }
        }

        public ImageSource ArticleImage
        {
            get { return articleImage; }
            set
            {
                articleImage = value;
                if (articleImage == null)
                {
                    ArticleImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleImage"));
            }
        }

        public double EstimatedWeight
        {
            get { return estimatedWeight; }
            set
            {
                estimatedWeight = value;
                CheckBoxWeightTolerance();
                OnPropertyChanged(new PropertyChangedEventArgs("EstimatedWeight"));
            }
        }

        public PackingBox SelectedPackingBox
        {
            get { return selectedPackingBox; }
            set
            {
                selectedPackingBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPackingBox"));
            }
        }

        public List<int> CustomersIdList
        {
            get { return customersIdList; }
            set
            {
                customersIdList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomersIdList"));
            }
        }

        //public List<int> GeosAppSettingCustomerIdList
        //{
        //    get
        //    {
        //        return geosAppSettingCustomerIdList;
        //    }
        //    set
        //    {
        //        geosAppSettingCustomerIdList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingCustomerIdList"));
        //    }
        //}

        public int TotalPackingBoxCount
        {
            get { return totalPackingBoxCount; }
            set
            {
                totalPackingBoxCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPackingBoxCount"));
            }
        }

        public List<Company> CompanyList
        {
            get { return companyList; }
            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPackingBoxCount"));
            }
        }

        public bool IsBoxWeightTolerance
        {
            get { return isBoxWeightTolerance; }
            set
            {
                isBoxWeightTolerance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBoxWeightTolerance"));
            }
        }

        public double ScaleWeight
        {
            get { return scaleWeight; }
            set
            {
                scaleWeight = value;
                CheckBoxWeightTolerance();
                OnPropertyChanged(new PropertyChangedEventArgs("ScaleWeight"));
            }
        }

        public int TotalItems
        {
            get { return totalItems; }
            set
            {
                totalItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalItems"));
            }
        }

        Dictionary<string, string> PrintValues { get; set; }

        PrintLabel PrintLabel { get; set; }

        public string ScaleWeightValue
        {
            get { return scaleWeightValue; }
            set
            {
                scaleWeightValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScaleWeightValue"));
            }
        }

        public GeosAppSetting GeosAppSetting
        {
            get { return geosAppSetting; }
            set
            {
                geosAppSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSetting"));
            }
        }

        public Ots OT
        {
            get { return oT; }
            set
            {
                oT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }

        public int StateIndex
        {
            get { return stateIndex; }
            set
            {
                stateIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StateIndex"));
            }
        }

        public string ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTip"));
            }
        }
        public long IdOTUnPackedItem
        {
            get
            {
                return idOTUnPackedItem;
            }

            set
            {
                idOTUnPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTUnPackedItem"));
            }
        }
        public long IdOTPackedItem
        {
            get
            {
                return idOTPackedItem;
            }

            set
            {
                idOTPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTPackedItem"));
            }
        }
        public string InfoTooltipBackColor
        {
            get
            {
                return infoTooltipBackColor;
            }

            set
            {
                infoTooltipBackColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InfoTooltipBackColor"));
            }
        }
        #endregion

        #region Events

        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events

        #region ICommands
        public ICommand CommandOnLoaded { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand WorkOrderHyperlinkClickCommand { get; set; }
        public ICommand ArticleReferenceHyperlinkClickCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand EditBoxCommand { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand ItemExpandedCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand OpenCloseBoxCommand { get; set; }
        public ICommand UnPackedItemImageClickCommand { get; set; }
        public ICommand PackedItemImageClickCommand { get; set; }
        public ICommand CommandKeyDown { get; set; }

       
        #endregion

        #region Constructor
        public PackingScanViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PackingScanViewModel...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GeosApplication.Instance.Logger.Log("Constructor PackingScanViewModel....", category: Category.Info, priority: Priority.Low);
                WindowHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                WindowWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                CommandOnLoaded = new DelegateCommand(LoadedAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                SelectedItemChangedCommand = new DelegateCommand(SelectedItemChangedCommandAction);
                WorkOrderHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderHyperlinkClickCommandAction);
                ArticleReferenceHyperlinkClickCommand = new DelegateCommand<object>(ArticleReferenceHyperlinkClickCommandAction);
                AddButtonCommand = new RelayCommand(new Action<object>(AddNewBox));
                EditBoxCommand = new RelayCommand(new Action<object>(EditBoxCommandAction));
                CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
                ItemExpandedCommand = new DelegateCommand<AccordionItemExpandedEventArgs>(ItemExpandedCommandAction);
                DeleteButtonCommand = new DelegateCommand<object>(DeleteButtonCommandAction);
                OpenCloseBoxCommand = new DelegateCommand<object>(OpenCloseBoxCommandAction);
                PackedItemImageClickCommand = new DelegateCommand<object>(PackedItemImageClickCommandAction);       //ImageClick
                UnPackedItemImageClickCommand = new DelegateCommand<object>(UnPackedItemImageClickCommandAction);       //ImageClick
                CommandKeyDown = new DelegateCommand<object>(CommandKeyDownAction);
                PackingBoxDetailsVisibility = Visibility.Collapsed;
                IsScanDirectionArrow = true;
                GetCustomersRequiremrntSettings();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor PackingScanViewModel....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PackingScanViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init(List<Company> list)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                CompanyList = list;
                List<Company> tempCompanyList = list.Where(x => x.ShortName != null).ToList();
                tempCompanyList = tempCompanyList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                string idSites = string.Join(",", tempCompanyList.Select(i => i.IdCompany));
                PackingCompanyList = new ObservableCollection<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2035(WarehouseCommon.Instance.Selectedwarehouse, idSites));

                if (PackingCompanyList.Count > 0)
                    SelectedItem = PackingCompanyList.FirstOrDefault();

                IsScanBox = true;
                TotalPackingBoxCount = PackingCompanyList.Sum(x => x.PackingBoxes.Count);

                var GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("23");

                if (GeosAppSettingList.Count > 0)
                    boxWeightTolerance = Convert.ToDouble(GeosAppSettingList.Select(x => x.DefaultValue).FirstOrDefault());

                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;

                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void LoadedAction()
        {
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.PackingScanUserControlView", null, this);
        }

        private void CancelButtonCommandAction(object obj)
        {
            if (timer != null)
            {
                timer.Elapsed -= new ElapsedEventHandler(timer_Tick);
                timer.Enabled = false;
            }

            if (balance != null)
                balance.Dispose();

            RequestClose(null, null);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (IsBoxesVisible == Visibility.Collapsed)
                    IsBoxesVisible = Visibility.Visible;
                else
                    IsBoxesVisible = Visibility.Collapsed;

                FocusUserControl = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedItemChangedCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction...", category: Category.Info, priority: Priority.Low);

                //if (SelectedItem is PackingCompany)
                //{
                //    PackingCompany tempPackingCompany = (PackingCompany)SelectedItem;
                //    UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));
                //    PackingBoxDetailsVisibility = Visibility.Collapsed;
                //    if (SelectedPackedItem != null)
                //        SelectedPackedItem = null;
                //}

                if (SelectedItem is PackingBox)
                {
                    if (((PackingBox)SelectedItem).IsClosed == 0)
                        PackingBoxDetailsVisibility = Visibility.Visible;
                    else if (((PackingBox)SelectedItem).IsClosed == 1)
                        PackingBoxDetailsVisibility = Visibility.Collapsed;

                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2051(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    //PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));
                    PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2051(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));

                    SelectedPackingBox = (PackingBox)SelectedItem;
                    SetBoxVisibilityTemplate();

                    if (IsAutoWeighing)
                    {
                        if (balance != null)
                        {
                            balance.Dispose();
                            balance = null;
                            GetScaleWeight();
                        }                        
                    }

                    EstimatedWeight = ((PackingBox)SelectedItem).NetWeight + PackedItemsList.Sum(x => x.ArticleWeight * x.Qty);
                    ToolTip = strWeighingIsOff;
                }

                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to open Work Order details view
        /// </summary>
        /// <param name="obj"></param>
        private void WorkOrderHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                WOItem unPackedItem = (WOItem)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;
                workOrderItemDetailsViewModel.Init(unPackedItem.IdOT, WarehouseCommon.Instance.Selectedwarehouse);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workOrderItemDetailsView.ShowDialogWindow();
                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method HyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to open Article Details.
        /// </summary>
        /// <param name="obj"></param>
        private void ArticleReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                WOItem article = (WOItem)obj;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articleDetailsView.ShowDialog();

                if (articleDetailsViewModel.IsResult)
                {
                    if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
                    {
                        foreach (var item in UnPackedItemsList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        foreach (var item in PackedItemsList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        if (SelectedPackedItem != null)
                        {
                            if (article.IdArticle == SelectedPackedItem.IdArticle)
                                ArticleImage = ByteArrayToImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
                        }
                    }
                }

                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Method ArticleReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewBox(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewBox....", category: Category.Info, priority: Priority.Low);
                NewBoxView newBoxView = new NewBoxView();
                NewBoxViewModel newBoxViewModel = new NewBoxViewModel();
                EventHandler handler = delegate { newBoxView.Close(); };
                newBoxViewModel.RequestClose += handler;
                newBoxView.DataContext = newBoxViewModel;

                newBoxViewModel.WindowHeader = System.Windows.Application.Current.FindResource("NewBox").ToString();
                newBoxViewModel.IsOpenCloseButtonVisibile = Visibility.Collapsed;
                newBoxViewModel.Init(CompanyList);
                newBoxView.ShowDialogWindow();
                if (newBoxViewModel.IsSave)
                {
                    if (PackingCompanyList.Any(x => x.IdCompany == newBoxViewModel.NewPackingBox.IdSite))
                    {
                        if (SelectedItem is PackingCompany)
                        {
                            PackingCompany tempPackingCompany = (PackingCompany)SelectedItem;
                            tempPackingCompany.PackingBoxes.Add(newBoxViewModel.NewPackingBox);

                            SelectedItem = newBoxViewModel.NewPackingBox;
                        }
                        else if (SelectedItem is PackingBox)
                        {
                            PackingCompany tempPackingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == newBoxViewModel.NewPackingBox.IdSite);
                            tempPackingCompany.PackingBoxes.Add(newBoxViewModel.NewPackingBox);
                            SelectedItem = newBoxViewModel.NewPackingBox;
                        }
                    }
                    else
                    {
                        PackingCompany packingCompany = new PackingCompany();
                        packingCompany.IdCompany = newBoxViewModel.CustomersList[newBoxViewModel.SelectedCustomerIndex].IdCompany;
                        packingCompany.ShortName = newBoxViewModel.CustomersList[newBoxViewModel.SelectedCustomerIndex].ShortName;
                        packingCompany.Name = newBoxViewModel.CustomersList[newBoxViewModel.SelectedCustomerIndex].Name;
                        packingCompany.PackingBoxes = new ObservableCollection<PackingBox>();
                        packingCompany.PackingBoxes.Add(newBoxViewModel.NewPackingBox);
                        PackingCompanyList.Add(packingCompany);
                        SelectedItem = newBoxViewModel.NewPackingBox;
                    }

                    TotalPackingBoxCount = PackingCompanyList.Sum(x => x.PackingBoxes.Count);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewBox....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewBox...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditBoxCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBoxCommandAction....", category: Category.Info, priority: Priority.Low);
                if (SelectedItem is PackingBox)
                {
                    NewBoxView newBoxView = new NewBoxView();
                    EditBoxViewModel editBoxViewModel = new EditBoxViewModel();
                    EventHandler handler = delegate { newBoxView.Close(); };
                    editBoxViewModel.RequestClose += handler;
                    newBoxView.DataContext = editBoxViewModel;
                    editBoxViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditBox").ToString();
                    editBoxViewModel.IsOpenCloseButtonVisibile = Visibility.Visible;
                    editBoxViewModel.Init((PackingBox)SelectedItem, CompanyList);
                    newBoxView.ShowDialogWindow();

                    if (editBoxViewModel.IsResult)
                    {
                        PackingBox tempPackingBox = (PackingBox)SelectedItem;
                        tempPackingBox.BoxNumber = editBoxViewModel.UpdatePackingBox.BoxNumber;
                        tempPackingBox.IdPackingBoxType = editBoxViewModel.UpdatePackingBox.IdPackingBoxType;
                        tempPackingBox.Length = editBoxViewModel.UpdatePackingBox.Length;
                        tempPackingBox.Width = editBoxViewModel.UpdatePackingBox.Width;
                        tempPackingBox.Height = editBoxViewModel.UpdatePackingBox.Height;
                        tempPackingBox.NetWeight = editBoxViewModel.UpdatePackingBox.NetWeight;
                        tempPackingBox.GrossWeight = editBoxViewModel.UpdatePackingBox.GrossWeight;
                        tempPackingBox.IsClosed = editBoxViewModel.UpdatePackingBox.IsClosed;

                        if (tempPackingBox.IdSite == editBoxViewModel.UpdatePackingBox.IdSite)
                        {
                            SelectedItem = editBoxViewModel.UpdatePackingBox;
                        }
                        else
                        {
                            PackingCompany company = PackingCompanyList.FirstOrDefault(x => x.IdCompany == tempPackingBox.IdSite);
                            company.PackingBoxes.Remove(tempPackingBox);
                            if (PackingCompanyList.Any(x => x.IdCompany == editBoxViewModel.UpdatePackingBox.IdSite))
                            {
                                PackingCompany packingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == editBoxViewModel.UpdatePackingBox.IdSite);
                                packingCompany.PackingBoxes.Add(editBoxViewModel.UpdatePackingBox);
                                SelectedItem = editBoxViewModel.UpdatePackingBox;
                                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                            }
                            else
                            {
                                PackingCompany packingCompany = new PackingCompany();
                                packingCompany.IdCompany = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].IdCompany;
                                packingCompany.ShortName = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].ShortName;
                                packingCompany.Name = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].Name;
                                packingCompany.PackingBoxes = new ObservableCollection<PackingBox>();
                                packingCompany.PackingBoxes.Add(editBoxViewModel.UpdatePackingBox);
                                PackingCompanyList.Add(packingCompany);
                                SelectedItem = editBoxViewModel.UpdatePackingBox;
                                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                            }
                        }
                    }

                    // FocusUserControl = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditBoxCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditBoxCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);

                if (obj.Text == "\r")
                {
                    var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr);
                    if (IsScanBox)
                    {
                        if (isProperBarcode)
                        {
                            var tempBarcode = BarcodeStr.Substring(1).TrimStart(new char[] { '0' });
                            PackingBox tempPackingBox = PackingCompanyList.SelectMany(x => x.PackingBoxes.Where(y => y.IdPackingBox == (long)(Convert.ToDouble(tempBarcode)))).ToList().FirstOrDefault();
                            if (tempPackingBox != null)
                            {
                                string barcode = "B" + tempPackingBox.IdPackingBox.ToString().PadLeft(9, '0');
                                if (barcode.Equals(BarcodeStr))
                                {
                                    SelectedItem = tempPackingBox;
                                    SetBoxVisibilityTemplate();
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                    if (IsAutoWeighing)
                                    {
                                        if (balance != null)
                                        {
                                            balance.Dispose();
                                            balance = null;
                                            GetScaleWeight();
                                        }
                                    }
                                    return;
                                }
                                else
                                {
                                    SetWrongBoxVisibilityTemplate();
                                    return;
                                }
                            }
                            else
                            {
                                SetWrongBoxVisibilityTemplate();
                                return;
                            }
                        }
                        else
                        {
                            SetWrongBoxVisibilityTemplate();
                            return;
                        }
                    }

                    if (SelectedItem is PackingBox && !IsScanBox)
                    {
                        string _partNumberCode;
                        Int64 _qty;
                        int _barcodeLength = BarcodeStr.Length;

                        if (IsScanDirectionArrow)
                        {
                            if (isProperBarcode && BarcodeStr.Length >= 17)
                            {
                                _qty = Convert.ToInt64(BarcodeStr.Substring(BarcodeStr.Length - 6));
                                _partNumberCode = BarcodeStr.Substring(0, (_barcodeLength - 6));
                                WOItem unPackedItem = UnPackedItemsList.FirstOrDefault(x => x.PartNumberCode.Substring(0, (x.PartNumberCode.Length - 3)) == _partNumberCode);

                                GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(34);

                                List<int> OneOrderOneBoxSiteIds = GeosAppSetting.DefaultValue.Split(',').Select(Int32.Parse).ToList();

                                if (PackedItemsList != null && PackedItemsList.Count >= 1)      //[002] start
                                {
                                    if (OneOrderOneBoxSiteIds.Any(x => x == ((PackingBox)SelectedItem).IdSite))     //checking idSite with GeosAppSettingFile
                                    {
                                        WOItem packedItem = (WOItem)unPackedItem.Clone();

                                        if (PackedItemsList[0].IdOffer == unPackedItem.IdOffer)
                                        {
                                            PackedItemsList.Add(packedItem);
                                            UnPackedItemsList.Remove(UnPackedItemsList.FirstOrDefault(x => x.IdArticle == packedItem.IdArticle));

                                            bool result = WarehouseService.UpdatePackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, packedItem.IdOTItem);

                                            if (result)
                                            {
                                                bool isOTItemStatus = WarehouseService.UpdateOTItemStatus_V2035(WarehouseCommon.Instance.Selectedwarehouse, packedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                            }
                                             ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                            //show comment
                                            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                            if (SelectedPackedItem.ArticleComment != null)
                                            {
                                                if(SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                                                {
                                                    SelectedPackedItem.ShowComment = true;
                                                }
                                                else if (SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                                {
                                                    SelectedPackedItem.ShowComment = true;
                                                }
                                                
                                            }
                                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                        }
                                        else
                                        {
                                            //SetErrorTemplate();
                                            OneOrderOneBoxErrorTemplate();       //[002] Added
                                        }
                                    }
                                    else
                                    {
                                        if (unPackedItem != null)
                                        {
                                            WOItem packedItem = (WOItem)unPackedItem.Clone();

                                            if (_qty <= packedItem.Qty)
                                            {
                                                if (_qty == unPackedItem.Qty)
                                                {
                                                    UnPackedItemsList.Remove(unPackedItem);
                                                    SelectedUnPackedItem = null;
                                                    unPackedItem.Qty = unPackedItem.Qty - _qty;
                                                }
                                                else if (_qty < unPackedItem.Qty)
                                                {
                                                    unPackedItem.Qty = unPackedItem.Qty - _qty;
                                                    SelectedUnPackedItem = unPackedItem;
                                                }

                                                if (CustomersIdList.Contains(((PackingBox)SelectedItem).IdSite))
                                                {
                                                    if (PackedItemsList.Count == 0)
                                                    {
                                                        if (unPackedItem.Qty == 0)
                                                        {
                                                            PackedItemsList.Add(packedItem);
                                                            SelectedPackedItem = packedItem;
                                                            SelectedPackedItem.Qty = packedItem.OriginalQty;
                                                            EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomersRequiringOneBoxOneWorkOrderMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    if (unPackedItem.Qty == 0)
                                                    {
                                                        PackedItemsList.Add(packedItem);
                                                        SelectedPackedItem = packedItem;
                                                        SelectedPackedItem.Qty = packedItem.OriginalQty;
                                                        EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                                    }
                                                }

                                                if (SelectedPackedItem != null)
                                                    ArticleImage = ByteArrayToImage(SelectedPackedItem.ArticleImageInBytes);

                                                if (unPackedItem.Qty == 0)
                                                {
                                                    bool result = WarehouseService.UpdatePackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, packedItem.IdOTItem);
                                                    if (result)
                                                    {
                                                        bool isOTItemStatus = WarehouseService.UpdateOTItemStatus_V2035(WarehouseCommon.Instance.Selectedwarehouse, packedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                                    }
                                                }

                                                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                                //show comment
                                                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                                if (SelectedPackedItem.ArticleComment != null)
                                                {
                                                    if (SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                                                    {
                                                        SelectedPackedItem.ShowComment = true;
                                                    }
                                                    else if(SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                                    {
                                                        SelectedPackedItem.ShowComment = true;
                                                    }
                                                    
                                                }
                                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                                WrongItem = "";
                                                IsWrongItemErrorVisible = Visibility.Collapsed;
                                                BarcodeStr = "";
                                            }
                                            else
                                            {
                                                WrongItem = String.Format("Max available quantity : {0}", packedItem.Qty);
                                                IsWrongItemErrorVisible = Visibility.Visible;
                                                BarcodeStr = "";
                                                SelectedUnPackedItem = unPackedItem;
                                                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (unPackedItem != null)
                                    {
                                        WOItem packedItem = (WOItem)unPackedItem.Clone();

                                        if (_qty <= packedItem.Qty)
                                        {
                                            if (_qty == unPackedItem.Qty)
                                            {
                                                UnPackedItemsList.Remove(unPackedItem);
                                                SelectedUnPackedItem = null;
                                                unPackedItem.Qty = unPackedItem.Qty - _qty;
                                            }
                                            else if (_qty < unPackedItem.Qty)
                                            {
                                                unPackedItem.Qty = unPackedItem.Qty - _qty;
                                                SelectedUnPackedItem = unPackedItem;
                                            }

                                            if (CustomersIdList.Contains(((PackingBox)SelectedItem).IdSite))
                                            {
                                                if (PackedItemsList.Count == 0)
                                                {
                                                    if (unPackedItem.Qty == 0)
                                                    {
                                                        PackedItemsList.Add(packedItem);
                                                        SelectedPackedItem = packedItem;
                                                        SelectedPackedItem.Qty = packedItem.OriginalQty;
                                                        EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                                    }
                                                }
                                                else
                                                {
                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomersRequiringOneBoxOneWorkOrderMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                if (unPackedItem.Qty == 0)
                                                {
                                                    PackedItemsList.Add(packedItem);
                                                    SelectedPackedItem = packedItem;
                                                    SelectedPackedItem.Qty = packedItem.OriginalQty;
                                                    EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                                }
                                            }

                                            if (SelectedPackedItem != null)
                                                ArticleImage = ByteArrayToImage(SelectedPackedItem.ArticleImageInBytes);

                                            if (unPackedItem.Qty == 0)
                                            {
                                                bool result = WarehouseService.UpdatePackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, packedItem.IdOTItem);
                                                if (result)
                                                {
                                                    bool isOTItemStatus = WarehouseService.UpdateOTItemStatus_V2035(WarehouseCommon.Instance.Selectedwarehouse, packedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                                }
                                            }

                                            ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                            //show comment
                                            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                            if (SelectedPackedItem.ArticleComment != null)
                                            {
                                                if (SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                                                {
                                                    SelectedPackedItem.ShowComment = true;
                                                }
                                                else if (SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                                {
                                                    SelectedPackedItem.ShowComment = true;
                                                }
                                               
                                            }
                                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                            WrongItem = "";
                                            IsWrongItemErrorVisible = Visibility.Collapsed;
                                            BarcodeStr = "";
                                        }
                                        else
                                        {
                                            WrongItem = String.Format("Max available quantity : {0}", packedItem.Qty);
                                            IsWrongItemErrorVisible = Visibility.Visible;
                                            BarcodeStr = "";
                                            SelectedUnPackedItem = unPackedItem;
                                            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                        }
                                    }
                                    else
                                        SetErrorTemplate();
                                } // [002] End
                            }
                            else
                                SetErrorTemplate();
                        }
                        else if (!IsScanDirectionArrow)
                        {
                            if (isProperBarcode && BarcodeStr.Length >= 17)
                            {
                                _qty = Convert.ToInt64(BarcodeStr.Substring(BarcodeStr.Length - 6));
                                _partNumberCode = BarcodeStr.Substring(0, (_barcodeLength - 6));

                                WOItem packedItemToUnPackedItem = PackedItemsList.FirstOrDefault(x => x.PartNumberCode.Substring(0, (x.PartNumberCode.Length - 3)) == _partNumberCode);

                                if (packedItemToUnPackedItem != null)
                                {
                                    WOItem unPackedItem = (WOItem)packedItemToUnPackedItem.Clone();
                                    if (_qty <= unPackedItem.Qty)
                                    {

                                        if (_qty == packedItemToUnPackedItem.Qty)
                                        {
                                            PackedItemsList.Remove(packedItemToUnPackedItem);
                                            UnPackedItemsList.Add(unPackedItem);
                                            ArticleImage = null;
                                            SelectedPackedItem = null;
                                            EstimatedWeight = EstimatedWeight - packedItemToUnPackedItem.ArticleWeight * unPackedItem.OriginalQty;
                                            SelectedUnPackedItem = unPackedItem;
                                            SelectedUnPackedItem.Qty = unPackedItem.OriginalQty;
                                            packedItemToUnPackedItem.Qty = packedItemToUnPackedItem.Qty - _qty;

                                        }
                                        else if (_qty < unPackedItem.Qty)
                                        {
                                            packedItemToUnPackedItem.Qty = packedItemToUnPackedItem.Qty - _qty;
                                        }

                                        if (SelectedPackedItem != null)
                                            ArticleImage = ByteArrayToImage(SelectedPackedItem.ArticleImageInBytes);

                                        if (packedItemToUnPackedItem.Qty == 0)
                                        {
                                            bool result = WarehouseService.UpdateUnPackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, unPackedItem.IdOTItem);
                                            if (result)
                                            {
                                                bool isOTItemStatusToFinished = WarehouseService.UpdateOTItemStatusToFinished_V2035(WarehouseCommon.Instance.Selectedwarehouse, unPackedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                            }
                                        }

                                        ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                        //show comment
                                        UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                        PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                        if (SelectedUnPackedItem.ArticleComment != null)
                                        {
                                            if (SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                                            {
                                                SelectedPackedItem.ShowComment = true;
                                            }
                                            else if (SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                            {
                                                SelectedPackedItem.ShowComment = true;
                                            }
                                          
                                        }
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                        WrongItem = "";
                                        IsWrongItemErrorVisible = Visibility.Collapsed;
                                        BarcodeStr = "";
                                    }
                                    else
                                    {
                                        WrongItem = String.Format("Max available quantity : {0}", unPackedItem.Qty);
                                        IsWrongItemErrorVisible = Visibility.Visible;
                                        BarcodeStr = "";
                                        UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                        PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                    }
                                }
                                else
                                    SetErrorTemplate();
                            }
                            else
                                SetErrorTemplate();
                        }
                    }
                    else if (SelectedItem is PackingCompany && !IsScanBox)
                    {
                        WrongItem = "Scan box first";
                        IsWrongItemErrorVisible = Visibility.Visible;
                        BarcodeStr = "";
                    }
                }
                else
                {
                    BarcodeStr = BarcodeStr + obj.Text;
                }

                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                BarcodeStr = "";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                BarcodeStr = "";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                BarcodeStr = "";
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetErrorTemplate()
        {
            WrongItem = "Wrong Item " + BarcodeStr;
            IsWrongItemErrorVisible = Visibility.Visible;
            BarcodeStr = "";
            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        /// <summary>
        /// For OneOrderOneBox Error
        /// </summary>
        private void OneOrderOneBoxErrorTemplate()
        {
            PackingCompany packingCompany = PackingCompanyList.FirstOrDefault(c => c.IdCompany == SelectedPackingBox.IdSite);
            WrongItem = string.Format(Application.Current.FindResource("OneOrderOneBoxErrorMsg").ToString(), packingCompany.ShortName);
            IsWrongItemErrorVisible = Visibility.Visible;
            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void SetErrorVisibilityTemplate()
        {
            IsWrongItemErrorVisible = Visibility.Collapsed;
            WrongItem = "";
            BarcodeStr = "";
        }

        private void SetWrongBoxVisibilityTemplate()
        {
            WrongItem = "Wrong Box " + BarcodeStr;
            IsWrongItemErrorVisible = Visibility.Visible;
            BarcodeStr = "";
            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void SetBoxVisibilityTemplate()
        {
            WrongItem = "";
            IsWrongItemErrorVisible = Visibility.Collapsed;
            BarcodeStr = "";
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to Get Customers Requiremrnt Settings [ Customers_Requiring_One_Box_One_WorkOrder]
        /// </summary>
        private void GetCustomersRequiremrntSettings()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCustomersRequiremrntSettings()...", category: Category.Info, priority: Priority.Low);

                List<GeosAppSetting> GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("22");
                if (GeosAppSettingList.Count > 0)
                {
                    string[] GeosAppSettingDefaultValues = GeosAppSettingList[0].DefaultValue.Split(',');
                    CustomersIdList = GeosAppSettingDefaultValues.ToList().ConvertAll(int.Parse);
                }

                GeosApplication.Instance.Logger.Log("Method GetCustomersRequiremrntSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetCustomersRequiremrntSettings() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetCustomersRequiremrntSettings() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GetCustomersRequiremrntSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  method to Check Box Weight Tolerance
        /// </summary>
        private void CheckBoxWeightTolerance()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckBoxWeightTolerance...", category: Category.Info, priority: Priority.Low);
                double boxWeightToleranceDifferance = ScaleWeight - boxWeightTolerance;
                double boxWeightToleranceDifferanceValue = ScaleWeight + boxWeightTolerance;

                if (boxWeightToleranceDifferance <= EstimatedWeight && EstimatedWeight <= boxWeightToleranceDifferanceValue)
                    IsBoxWeightTolerance = true;
                else
                    IsBoxWeightTolerance = false;

                GeosApplication.Instance.Logger.Log("Method CheckBoxWeightTolerance executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckBoxWeightTolerance() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][cpatil][17-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
        private void ItemExpandedCommandAction(AccordionItemExpandedEventArgs e)
        {
            if (e.Item is PackingCompany)
            {
                PackingCompany tempPackingCompany = (PackingCompany)e.Item;

                if (tempPackingCompany.PackingBoxes != null && tempPackingCompany.PackingBoxes.Count == 1)
                {
                    SelectedItem = tempPackingCompany.PackingBoxes.FirstOrDefault();
                    if (((PackingBox)SelectedItem).IsClosed == 0)
                        PackingBoxDetailsVisibility = Visibility.Visible;
                    else if (((PackingBox)SelectedItem).IsClosed == 1)
                        PackingBoxDetailsVisibility = Visibility.Collapsed;
                    //[001]
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2051(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    //PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));
                    PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2051(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));
                    SelectedPackingBox = (PackingBox)SelectedItem;
                    SetBoxVisibilityTemplate();
                    EstimatedWeight = ((PackingBox)SelectedItem).NetWeight + PackedItemsList.Sum(x => x.ArticleWeight * x.Qty);
                }
                else
                {
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));
                    UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2051(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));
                    PackingBoxDetailsVisibility = Visibility.Collapsed;
                }
            }
        }

        private void DeleteButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction...", category: Category.Info, priority: Priority.Low);

                if (SelectedItem is PackingBox)
                {
                    PackingBox tempPackingBox = (PackingBox)SelectedItem;
                    if (tempPackingBox.ItemsInBox == 0)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePackingBoxMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            bool isDelete = WarehouseService.RemovePackingBox(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox);
                            if (isDelete)
                            {
                                PackingCompany tempPackingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == tempPackingBox.IdSite);
                                if (tempPackingCompany != null)
                                {
                                    tempPackingCompany.PackingBoxes.Remove(tempPackingBox);
                                }
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeletePackingBoxSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            }
                        }
                    }
                    else
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotAllowToDeletePackingBox").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteButtonCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteButtonCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenCloseBoxCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenCloseButtonCommandAction...", category: Category.Info, priority: Priority.Low);
                if (SelectedItem is PackingBox)
                {
                    PackingBox editPackingBox = (PackingBox)SelectedItem;
                    if (editPackingBox.IsClosed == 1)
                    {
                        editPackingBox.IsClosed = 0;
                        PackingBoxDetailsVisibility = Visibility.Visible;
                    }
                    else if (editPackingBox.IsClosed == 0)
                    {
                        editPackingBox.IsClosed = 1;
                        PackingBoxDetailsVisibility = Visibility.Collapsed;
                    }
                    bool isPackingBoxClosed = WarehouseService.UpdateIsClosedInPackingBox(WarehouseCommon.Instance.Selectedwarehouse, editPackingBox.IdPackingBox, editPackingBox.IsClosed);

                    if (isPackingBoxClosed && editPackingBox.IsClosed == 1)
                        PrintBoxLabel(editPackingBox);
                }
                GeosApplication.Instance.Logger.Log("Method OpenCloseBoxCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintBoxLabel(PackingBox editPackingBox)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintBoxLabel...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                List<BoxPrint> tempPackingBoxList = WarehouseService.GetWorkorderByIdPackingBox_V2036(WarehouseCommon.Instance.Selectedwarehouse, editPackingBox.IdPackingBox);
                PrintValues = new Dictionary<string, string>();
                byte[] printFile = GeosRepositoryService.GetBoxLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
                PrintLabel = new PrintLabel(PrintValues, printFile);
                CreatePrintValues(tempPackingBoxList);
                PrintLabel.Print();

                if (printFile != null)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BoxLabelPrintSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintBoxLabel executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CreatePrintValues(List<BoxPrint> tempPackingBoxList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreatePrintValues...", category: Category.Info, priority: Priority.Low);
                PrintValues.Add("@USER", GeosApplication.Instance.ActiveUser.IdUser.ToString());

                #region SiteName(As per GPM)
                string customerName = String.Format("{0} - {1}", tempPackingBoxList.FirstOrDefault().CustomerName, tempPackingBoxList.FirstOrDefault().SiteNameWithCountry);

                if (customerName.Length > 26)
                {
                    int index = 0;
                    string firstLine = GetFirstLine(customerName, ref index);
                    PrintValues.Add("@CUSTOMER00", firstLine);
                    PrintValues.Add("@CUSTOMER01", customerName.Substring(index));
                }
                else
                {
                    PrintValues.Add("@CUSTOMER00", customerName);
                    PrintValues.Add("@CUSTOMER01", "");
                }
                #endregion

                PrintValues.Add("@OT00", "");
                PrintValues.Add("@OT01", "");
                PrintValues.Add("@OT02", "");
                PrintValues.Add("@OT03", "");
                PrintValues.Add("@OT04", "");
                PrintValues.Add("@OT05", "");
                PrintValues.Add("@OT06", "");
                PrintValues.Add("@OT07", "");
                PrintValues.Add("@OT08", "");
                PrintValues.Add("@OT09", "");
                int id = 0;
                foreach (BoxPrint item in tempPackingBoxList)
                {
                    PrintValues["@OT0" + id] = item.OtCode;
                    id++;
                }

                PrintValues.Add("@BOX_NUMBER", tempPackingBoxList.FirstOrDefault().BoxNumber);
                PrintValues.Add("@BOX_ID", GetPackingBoxBarCode(tempPackingBoxList.FirstOrDefault().IdPackingBox.ToString()));
                PrintValues.Add("@WEIGHT", tempPackingBoxList.FirstOrDefault().GrossWeight.ToString() + "Kg");
                PrintValues.Add("@CARRIAGE_METHOD_CODE", tempPackingBoxList.FirstOrDefault().CarriageMethodAbbreviation);
                PrintValues.Add("@CARRIAGE_METHOD_NAME", tempPackingBoxList.FirstOrDefault().CarriageMethodValue);
                PrintValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);
                GeosApplication.Instance.Logger.Log("Method CreatePrintValues executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreatePrintValues() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetFirstLine(string customer, ref int i)
        {
            string temp = "";

            for (int j = 0; j < customer.Length; j++)
            {
                if (customer[j] == ' ')
                {
                    if (j > 26)
                    {
                        break;
                    }
                    else
                    {
                        i = j + 1;
                        temp = customer.Substring(0, j);
                    }
                }
            }
            return temp;
        }

        private string GetPackingBoxBarCode(string idPackingBox)
        {
            string barcode = "";
            barcode = "B" + idPackingBox.PadLeft(9, '0');
            return barcode;
        }

        /// <summary>
        /// Method to get Scale Weight
        /// [003][Sprint_78][GEOS2-1991][Show connection indicator for Weighing Machine]
        /// </summary>
        private void GetScaleWeight()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetScaleWeight...", category: Category.Info, priority: Priority.Low);

                if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedScaleModel))
                {
                    if (balance == null)
                    {
                        if (WarehouseCommon.Instance.SelectedScaleModel == Balance.Model.SARTORIUS_MIRAS_IW2.ToString())
                        {
                            if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedPort) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedBaudRate) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedParity) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedDataBit) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedStopBit))
                            {
                                balance = new Miras_IW2();
                                balance.Communication = new SerialCommunication(WarehouseCommon.Instance.SelectedPort, Convert.ToInt32(WarehouseCommon.Instance.SelectedBaudRate), (Parity)Enum.Parse(typeof(Parity), WarehouseCommon.Instance.SelectedParity, true), Convert.ToInt32(WarehouseCommon.Instance.SelectedDataBit), (StopBits)Enum.Parse(typeof(StopBits), WarehouseCommon.Instance.SelectedStopBit));

                                if (timer == null)
                                {
                                    timer = new Timer();
                                    timer.Interval = 1000;
                                }
                                timer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                timer.Enabled = true;
                            }
                            else
                            {
                                StateIndex = 3;
                                ToolTip = MachineNOTConnected;
                                ScaleWeightValue = "0KG";
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectCOMSettingsMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        else if (WarehouseCommon.Instance.SelectedScaleModel == Balance.Model.SARTORIUS_MIDRICS_MW1.ToString())
                        {
                            if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedPort) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedBaudRate) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedParity) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedDataBit) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedStopBit))
                            {
                                balance = new Midrics_MW1();
                                balance.Communication = new SerialCommunication(WarehouseCommon.Instance.SelectedPort, Convert.ToInt32(WarehouseCommon.Instance.SelectedBaudRate), (Parity)Enum.Parse(typeof(Parity), WarehouseCommon.Instance.SelectedParity, true), Convert.ToInt32(WarehouseCommon.Instance.SelectedDataBit), (StopBits)Enum.Parse(typeof(StopBits), WarehouseCommon.Instance.SelectedStopBit));

                                if (timer == null)
                                {
                                    timer = new Timer();
                                    timer.Interval = 1000;
                                }

                                timer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                timer.Enabled = true;
                            }
                            else
                            {
                                StateIndex = 3;
                                ToolTip = MachineNOTConnected;
                                ScaleWeightValue = "0KG";
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectCOMSettingsMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                    }
                }
                else
                {
                    StateIndex = 3;
                    ToolTip = MachineNOTConnected;
                    ScaleWeightValue = "0KG";
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectScaleModelMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method GetScaleWeight executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (BalanceException ex)
            {
                StateIndex = 3;
                ToolTip = MachineNOTConnected;
                GeosApplication.Instance.Logger.Log("Get an error in GetScaleWeight() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                StateIndex = 3;
                ToolTip = MachineNOTConnected;
                ScaleWeightValue = "Error";
                GeosApplication.Instance.Logger.Log("Get an error in GetScaleWeight() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Timmer
        /// [003][Sprint_78][GEOS2-1991][Show connection indicator for Weighing Machine]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                timer.Enabled = false;
                if (!IsAutoWeighing) return;

                WeightMeasurement measure = null;

                try
                {
                    GeosApplication.Instance.Logger.Log("Method timer_Tick...", category: Category.Info, priority: Priority.Low);
                    measure = (balance).GetWeight();
                }
                catch (BalanceException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method timer_Tick BalanceException - {0}.", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                if (isAutoWeighing)
                {
                    if (measure != null)
                    {
                        ScaleWeightValue = measure.Value + measure.Unit.ToString();
                        ScaleWeight = Convert.ToDouble(measure.Value);
                        StateIndex = 1;
                        ToolTip = MachineConnected;
                    }
                    else
                    {
                        StateIndex = 3;
                        ToolTip = MachineNOTConnected;
                        GeosApplication.Instance.Logger.Log(string.Format("Method timer_Tick Weight Measurement not received"), category: Category.Info, priority: Priority.Low);
                    }
                }
                timer.Enabled = true;
                GeosApplication.Instance.Logger.Log("Method timer_Tick executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in timer_Tick() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PackedItemImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PackedItemImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                WOItem woItem = (WOItem)obj;
                IdOTPackedItem = woItem.IdOTItem;
                PackedItemsList.Where(a => a.IdOTItem != IdOTPackedItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                PackedItemsList.Where(a => a.IdOTItem == IdOTPackedItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PackedItemImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method PackedItemImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }

        private void UnPackedItemImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method UnPackedItemImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                WOItem picking = (WOItem)obj;
                IdOTUnPackedItem = picking.IdOTItem;
                UnPackedItemsList.Where(a => a.IdOTItem != IdOTUnPackedItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in UnPackedItemImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method UnPackedItemImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }

        private void CommandKeyDownAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommandKeyDownAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                var Type = ((System.Windows.RoutedEventArgs)obj).OriginalSource.GetType();
                if (Type.Name != "Image")
                {
                    if (IdOTUnPackedItem > 0)
                    {
                        UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                    if(IdOTPackedItem>0)
                    { 
                        PackedItemsList.Where(a => a.IdOTItem == IdOTPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                }
                else
                {
                    if (((System.Windows.UIElement)((System.Windows.RoutedEventArgs)obj).OriginalSource).Uid == "IdUnPackedImage")
                    {
                        PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                    else if (((System.Windows.UIElement)((System.Windows.RoutedEventArgs)obj).OriginalSource).Uid== "IdPackedItem")
                    {
                        UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                    else
                    {
                        if (IdOTUnPackedItem > 0)
                        {
                            UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                        }
                        if (IdOTPackedItem > 0)
                        {
                            PackedItemsList.Where(a => a.IdOTItem == IdOTPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandKeyDownAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method CommandKeyDownAction....executed ", category: Category.Info, priority: Priority.Low);
        }
        #endregion
    }
}
