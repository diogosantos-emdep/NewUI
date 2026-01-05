using DevExpress.Data;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class PendingWorkOrdersViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region
        #region Declaration
        private ObservableCollection<TileBarFilters> quickFilterList = new ObservableCollection<TileBarFilters>();
        private ObservableCollection<Template> listOfTemplate = new ObservableCollection<Template>();
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        private List<Ots> mainOtsList = new List<Ots>();
        private List<Ots> filterWiseListOfWorkOrder = new List<Ots>();
        private ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM> columns;
        private IList<Template> templates;
        List<OfferOption> offerOptions;
        private DataTable dttable;
        private DataTable dttableCopy;
        TreeListView view;
        private string filterString;
        private bool isBusy;
        private TileBarFilters selectedFilter;
        private int visibleRowCount;
        private string userSettingsKey = "SAM_PendingWorkOrder_";
        private bool isEdit;
        private bool isSave;
        private bool isWorkOrderColumnChooserVisible;
        public string PendingWorkOrdersGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "SAM_PendingWorkOrdersGridSetting.Xml";
        private object geosAppSettingList;
        private List<TileBarFilters> filterList;
        private List<Ots> mainOtsList_New = new List<Ots>();
        private int totalCount;
        private int visibleChildRowCount;
        private bool isRemarkReadOnly;
        int emptyCellsTotalCount = 0;
        int oldid = 0;
        int emptyCellsTotalCountOld = 0;
        private bool showClockIcon;
        #endregion // End Of Declaration
        #region Properties
        public bool ShowClockIcon
        {
            get { return showClockIcon; }
            set
            {
                showClockIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowClockIcon"));
            }
        }
        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
        }
        public IList<Template> Templates
        {
            get { return templates; }
            set { templates = value; }
        }
        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public List<int> ProgressList { get; private set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }
        public List<Ots> FilterWiseListOfWorkOrder
        {
            get { return filterWiseListOfWorkOrder; }
            set
            {
                filterWiseListOfWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfWorkOrder"));
            }
        }
        public List<Ots> MainOtsList
        {
            get { return mainOtsList; }
            set
            {
                mainOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList"));
            }
        }
        public ObservableCollection<Template> ListOfTemplate
        {
            get { return listOfTemplate; }
            set
            {
                listOfTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("listOfTemplate"));
            }
        }
        public ObservableCollection<TileBarFilters> QuickFilterList
        {
            get { return quickFilterList; }
            set
            {
                quickFilterList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuickFilterList"));
            }
        }
        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
            }
        }
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public string CustomFilterStringName { get; set; }
        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }
        public object GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        public List<Ots> MainOtsList_New
        {
            get { return mainOtsList_New; }
            set
            {
                mainOtsList_New = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList_New"));
            }
        }
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                totalCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }
        public int VisibleChildRowCount
        {
            get { return visibleChildRowCount; }
            set
            {
                visibleChildRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleChildRowCount"));
            }
        }
        public bool IsRemarkReadOnly
        {
            get { return isRemarkReadOnly; }
            set
            {
                isRemarkReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemarkReadOnly"));
            }
        }

		//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        List<string> failedPlants;
        List<string> successPlantList; //[nsatpute][08.07.2025][GEOS2-7205]
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        //[nsatpute][08.07.2025][GEOS2-7205]
        public List<string> SuccessPlantList
        {
            get { return successPlantList; }
            set { successPlantList = value; }
        }

        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        #endregion //End Of Properties
        #region Icommands
        public ICommand CommandFilterTileClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand PlantOwnerPopupClosed { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }
        public ICommand PrintWorkOrderViewCommand { get; set; }
        public ICommand ExportWorkOrderViewCommand { get; set; }
        public ICommand ScanWorkOderCommand { get; set; }
        public ICommand RefundWorkOrderCommand { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand UpdateMultipleRowsWorkOrderCommand { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand1 { get; set; }
        public ICommand ShowingEditorCommand { get; set; }
        public ICommand ScanValidationCommand { get; set; }
        public ICommand CustomSummaryCommand { get; set; }
        public ICommand PendingWorkOrderViewHyperlinkClickCommand { get; set; }
        #endregion //End Of Icommand
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // End Of Events 
        #region Constructor
        public PendingWorkOrdersViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                UpdateMultipleRowsWorkOrderCommand = new DelegateCommand<object>(UpdateMultipleRowsCommandAction);
                CommandFilterTileClick = new DelegateCommand<object>(ShowSelectedFilterGridAction);
                //CommandGridDoubleClick = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);
                PlantOwnerPopupClosed = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedAction);
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshPendingWorkOrderList));
                PrintWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintPendingWorkOrderList));
                ExportWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportPendingWorkOrderList));
                ScanWorkOderCommand = new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrder);
                RefundWorkOrderCommand = new DelegateCommand<object>(RefundWorkOrderCommandAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CustomUnboundColumnDataCommand = new DelegateCommand<object>(CustomUnboundColumnDataAction);
                CustomUnboundColumnDataCommand1 = new DelegateCommand<object>(CustomUnboundColumnDataAction);
                ShowingEditorCommand = new DelegateCommand<object>(ShowingEditorCommandAction);
                ScanValidationCommand =new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrderValidation);
                CustomSummaryCommand= new DelegateCommand<object>(CustomSummaryCommandAction);
                PendingWorkOrderViewHyperlinkClickCommand = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                    IsRemarkReadOnly = false;
                else
                    IsRemarkReadOnly = true;
                /*
                FillProgressList();
                FillListOfColor();  // Called only once for colors
                LoadData();
                FillLeadGridDetails();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                */
                // Instead of calling LoadData() here, call async loader
                //InitAsync(); // fire async loader
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion //End Of Constructor
        #region Methods
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        public async Task InitAsync()
        {
            GeosApplication.Instance.Logger.Log("Constructor Init....", category: Category.Info, priority: Priority.Low);
            try
            {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                    IsRemarkReadOnly = false;
                else
                    IsRemarkReadOnly = true;
                FillProgressList();
                FillListOfColor();  // Called only once for colors
                //await FillMainOtListAsync();
                //await FillMainOtListAsync();
                //FillMainOtListAsync();
                await FillMainOtListAsync();
                FillLeadGridDetails();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is called from constructor & refresh & plant combobox
        /// </summary>
        private void LoadData()
        {
            FillMainOtList();
        }
		//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task LoadDataAsync()
        {
            try
            {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
               // await FillMainOtListAsync();
               //FillMainOtListAsync();
                await FillMainOtListAsync();
                FillLeadGridDetails();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LoadDataAsync() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task UpdateMultipleRowsLoadDataAsync()
        {
            try
            {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                await FillMainOtListAsync();
                FillDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsLoadDataAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private void FillMainOtListParallel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                MainOtsList = new List<Ots>();
                var otsConcurrentBag = new ConcurrentBag<Ots>();
                var failedPlantsConcurrentBag = new ConcurrentBag<string>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {

                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    if (SAMCommon.Instance.SelectedPlantOwnerList.Count == 1)
                    {
                        //FillMainOtListForSinglePlant();
                        FillMainOtList();
                    }
                    else
                    {
                        Parallel.ForEach(SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>(), plant =>
                        {
                            try
                            {
                                SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                                SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ?
                                    SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                                // latest service call
                                var tempOtsList = SAMService.GetPendingWorkorders_V2660(plant);

                                foreach (var ots in tempOtsList)
                                {
                                    otsConcurrentBag.Add(ots);
                                }
                                lock (SuccessPlantList)
                                {
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                string errorMessage = ex is FaultException<ServiceException> faultEx ? faultEx.Detail.ErrorMessage : ex.Message;

                                lock (FailedPlants)
                                {
                                    FailedPlants.Add(plant.Alias);
                                    if (FailedPlants.Count > 0)
                                    {
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), string.Join(",", FailedPlants));
                                    }

                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 2;
                                }
                                Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}", Category.Exception, Priority.Low);
                            }
                        });
                    }
                    MainOtsList = otsConcurrentBag.ToList();
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Unexpected error in FillMainOtList() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private void FillMainOtListForSinglePlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();
                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                                SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                //SAMService = new SAMServiceController("localhost:6699");
                                TempMainOtsList = new List<Ots>(SAMService.GetPendingWorkorders_V2660(plant)); // [nsatpute][13-11-2024][GEOS2-5889] //[nsatpute][12.08.2025][GEOS2-9163]
                                MainOtsList.AddRange(TempMainOtsList);
                                lock (SuccessPlantList)
                                {
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", plant.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(plant.Alias);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", plant.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", plant.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(plant.Alias);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", plant.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", plant.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(plant.Alias);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", plant.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                        SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private async Task FillMainOtListAsync_old001()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                MainOtsList = new List<Ots>();
                var otsConcurrentBag = new ConcurrentBag<Ots>();
                var failedPlantsConcurrentBag = new ConcurrentBag<string>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    MainOtsList = new List<Ots>();
                    var errors = new List<(int IdCompany, string PlantName, string ErrorMessage)>();
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    try
                    {

                        // Run parallel tasks for each plant
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(plant =>
                            Task.Run(() =>
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                    var selectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                                    var serviceUrl = (selectedPlantOwner != null && selectedPlantOwner.ServiceProviderUrl != null) ? selectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
                                    var samService = new SAMServiceController(serviceUrl);
                                    //return new List<Ots>(samService.GetPendingWorkorders_V2660(plant));
                                    List<Ots> tempOtsList = SAMService.GetPendingWorkorders_V2660(plant);
                                    lock (SuccessPlantList)
                                    {
                                        SuccessPlantList.Add(plant.Alias);
                                        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                        if (statusMsg != null) statusMsg.IsSuccess = 1;

                                    }
                                    return tempOtsList;
                                }
                                catch (Exception ex)
                                {
                                    string errorMessage = ex is FaultException<ServiceException> faultEx ? faultEx.Detail.ErrorMessage : ex.Message;

                                    lock (FailedPlants)
                                    {
                                        FailedPlants.Add(plant.Alias);
                                        if (FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), string.Join(",", FailedPlants));
                                        }

                                        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                        if (statusMsg != null) statusMsg.IsSuccess = 2;
                                    }
                                    Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}", Category.Exception, Priority.Low);
                                    return new List<Ots>();
                                }
                                //catch (FaultException<ServiceException> ex)
                                //{
                                //    lock (errors)
                                //        errors.Add((plant.IdCompany, plant.Alias, ex.Detail.ErrorMessage));
                                //    GeosApplication.Instance.Logger.Log($"Plant {plant.IdCompany} failed: {ex.Detail.ErrorMessage}", category: Category.Warn, priority: Priority.Medium);
                                //    return new List<Ots>();
                                //}
                                //catch (Exception ex)
                                //{
                                //    lock (errors)
                                //        errors.Add((plant.IdCompany, plant.Alias, ex.Message));
                                //    GeosApplication.Instance.Logger.Log($"Plant {plant.IdCompany} failed: {ex.Message}", category: Category.Warn, priority: Priority.Medium);
                                //    return new List<Ots>();
                                //}
                            })
                        ).ToArray();

                        // Await all
                        var results = await Task.WhenAll(tasks);

                        // Merge results
                        foreach (var result in results)
                        {
                            MainOtsList.AddRange(result);
                        }
                        // Reset service at the end (same as old code)
                        SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtListAsync() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private async Task FillMainOtList_Async()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtList started...", category: Category.Info, priority: Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }

                    // Throttle parallelism (avoid connection storms)
                    var semaphore = new SemaphoreSlim(5); // adjust as needed
                    var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                            // Resolve service URL
                            var selectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                            var serviceUrl = (selectedPlantOwner != null && selectedPlantOwner.ServiceProviderUrl != null)? selectedPlantOwner.ServiceProviderUrl
                            : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();

                            //// Create fresh service instance per task
                            //using (var samService = new SAMServiceController(serviceUrl))
                            //{
                            //    List<Ots> tempOtsList = SAMService.GetPendingWorkorders_V2660(plant);

                            //    lock (SuccessPlantList)
                            //    {
                            //        SuccessPlantList.Add(plant.Alias);
                            //        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                            //        if (statusMsg != null) statusMsg.IsSuccess = 1;
                            //    }
                            //    return tempOtsList;
                            //}
                            // Create fresh service instance per task
                            var samService = new SAMServiceController(serviceUrl);
                            try
                            {
                                List<Ots> tempOtsList = SAMService.GetPendingWorkorders_V2660(plant);
                                lock (SuccessPlantList)
                                {
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                                return tempOtsList;
                            }
                            finally
                            {
                                // Properly cleanup WCF channel
                                if (samService is ICommunicationObject commObj)
                                {
                                    try { commObj.Close(); }
                                    catch { commObj.Abort(); }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx ? faultEx.Detail.ErrorMessage: ex.Message;

                            lock (FailedPlants)
                            {
                                FailedPlants.Add(plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"),string.Join(",", FailedPlants));
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                            }
                            GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}",Category.Exception, Priority.Low);

                            return new List<Ots>();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToArray();

                    // Await all tasks
                    var results = await Task.WhenAll(tasks);

                    // Merge results into main list
                    MainOtsList = results.SelectMany(r => r).ToList();

                    // Reset service at the end
                    SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync(): " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtListAsync_notworking()
        {
            try
            {
                if (DXSplashScreen.IsActive)  DXSplashScreen.Close();
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
                },x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() }, null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtList started...", Category.Info, Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    var semaphore = new SemaphoreSlim(10); // throttle connections
                    var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                            // Resolve service URL
                            var selectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                            var serviceUrl = !string.IsNullOrEmpty(selectedPlantOwner?.ServiceProviderUrl)? selectedPlantOwner.ServiceProviderUrl: GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
                            var samService = new SAMServiceController(serviceUrl);
                            try
                            {
                                List<Ots> tempOtsList = SAMService.GetPendingWorkorders_V2660(plant);
                                lock (SuccessPlantList)
                                {
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                                return tempOtsList;
                            }
                            finally
                            {
                                if (samService is ICommunicationObject commObj)
                                {
                                    try { commObj.Close(); }
                                    catch { commObj.Abort(); }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx? faultEx.Detail.ErrorMessage: ex.Message;
                            lock (FailedPlants)
                            {
                                FailedPlants.Add(plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                            }
                            GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}", Category.Exception, Priority.Low);
                            return new List<Ots>();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToArray();

                    var results = await Task.WhenAll(tasks);
                    // Merge results into main list
                    MainOtsList = results.SelectMany(r => r).ToList();
                    // Reset service at the end
                    SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Critical error in FillMainOtListAsync: " + ex.ToString(), Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtListAsync()
        {
            try
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
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
                }, x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() }, null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;

                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...", Category.Info, Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    var semaphore = new SemaphoreSlim(10); // throttle max 10 at once
                    var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                            // Resolve service URL per plant
                            var selectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                            var serviceUrl = !string.IsNullOrEmpty(selectedPlantOwner?.ServiceProviderUrl)? selectedPlantOwner.ServiceProviderUrl: GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
                            var samService = new SAMServiceController(serviceUrl);
                            try
                            {
                                // Run blocking SAMService call in background thread
                                var tempOtsList = await Task.Run(() =>SAMService.GetPendingWorkorders_V2660(plant));
                                lock (SuccessPlantList)
                                {
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                                return tempOtsList;
                            }
                            finally
                            {
                                if (samService is ICommunicationObject commObj)
                                {
                                    try { commObj.Close(); }
                                    catch { commObj.Abort(); }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx? faultEx.Detail.ErrorMessage: ex.Message;

                            lock (FailedPlants)
                            {
                                FailedPlants.Add(plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"),string.Join(",", FailedPlants));

                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                            }
                            GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}", Category.Exception,Priority.Low);
                            return new List<Ots>();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToArray();

                    var results = await Task.WhenAll(tasks);
                    // Merge results into main list
                    MainOtsList = results.SelectMany(r => r).ToList();
                    // Reset service at the end
                    SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Critical error in FillMainOtListAsync: " + ex.ToString(),Category.Exception,Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }


        /// <summary>
        /// [001]-sjadhav
        /// This method is called from constructor & fills Progress List
        /// </summary>
        private void FillProgressList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProgressList ...", category: Category.Info, priority: Priority.Low);
                ProgressList = new List<int>();
                ProgressList.Add(0);
                ProgressList.Add(10);
                ProgressList.Add(20);
                ProgressList.Add(30);
                ProgressList.Add(40);
                ProgressList.Add(50);
                ProgressList.Add(60);
                ProgressList.Add(70);
                ProgressList.Add(80);
                ProgressList.Add(90);
                ProgressList.Add(100);
                GeosApplication.Instance.Logger.Log("Method FillProgressList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProgressList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        ///----------[Sprint-83] [GEOS2-2372]  [19-06-2020] [sjadhav]---------
        /// This method is called from constructor & save data in grid
        /// </summary>
        public void UpdateMultipleRowsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction in SAM Work Order...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                view = obj as TreeListView;                
                bool isOtSave = false;
                bool isAllSave = false;
                Ots[] foundRow = MainOtsList_New.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() foundRow"+ foundRow.Length, category: Category.Info, priority: Priority.Low);
                if (foundRow.Length==0)
                {
                    foundRow = GeosApplication.Instance.MainOtsList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                }
                foreach (Ots Ot  in foundRow)
                {
                    GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() update OT " + Ot.IdOT+ " Site "+ Ot.Site.Name + "Planned Start Date" + Ot.ExpectedStartDate + "Planned End Date" + Ot.ExpectedEndDate, category: Category.Info, priority: Priority.Low);
                    isOtSave = SAMService.UpdateOTFromGrid_V2180(Ot.Site, Ot);
                    GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() update OT " + Ot.IdOT +" IsOTSave "+ isOtSave, category: Category.Info, priority: Priority.Low);
                    if (isOtSave)
                        isAllSave = true;
                    else
                        isAllSave = false;
                }
                if (isAllSave)
                {                   
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderMsgUpdateFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                MultipleCellEditHelperSAMWorkOrder.SetIsValueChanged(view, false);
                MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;

                //FillMainOtList();
                //FillDataTable();
                UpdateMultipleRowsLoadDataAsync();
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() in SAM Work Order executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsCommandAction() Method in SAM Work Order" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction...", category: Category.Info, priority: Priority.Low);
            view = MultipleCellEditHelperSAMWorkOrder.Viewtableview;
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkOrder.Viewtableview);
                }
                MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
            }
            MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
            if (view != null)
            {
                MultipleCellEditHelperSAMWorkOrder.SetIsValueChanged(view, false);
            }
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
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
            /*
            LoadData();
            FillLeadGridDetails();
            FillPendingWorkOrderFilterList();
            AddCustomSetting();
            */
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            LoadDataAsync();
            FilterString = string.Empty;
            if (QuickFilterList.Count > 0)
                SelectedFilter = QuickFilterList.FirstOrDefault();
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }
        public void RefreshPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                view = MultipleCellEditHelperSAMWorkOrder.Viewtableview;
                if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkOrder.Viewtableview);
                    }
                    MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
                }
                MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
                if (view != null)
                {
                    MultipleCellEditHelperSAMWorkOrder.SetIsValueChanged(view, false);
                }
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
                TreeListView detailView = (TreeListView)obj;
                TreeListControl gridControl = (TreeListControl)(detailView).DataControl;
                IsBusy = true;
                // code for hide column chooser if empty
                TreeListView tableView = (TreeListView)gridControl.View;
                int visibleFalseCoulumn = 0;
                foreach (TreeListColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }
                /*
                LoadData();
                FillLeadGridDetails();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                */
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                LoadDataAsync();
                FilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][20-05-2020][cpatil][GEOS2-1936] Ignore STRUCTURE template items in the #Modules count
        //[002][17-06-2020][smazhar][GEOS2-2376]  Filter the users by the selected plant
        //[003][cpatil][02-08-2021][GEOS2-2906] Include the “STRUCTURE” orders in the Orders grid
        //[004][cpatil][09-05-2022][GEOS2-3417] Display X and V OT's - Display VISION and ELECTRIFICATION work orders in “Work Orders” grid
        //[005][cpatil][03-06-2022][GEOS2-3585] [TESTBOARD_CERTIFICATION] Add the Template “QUALITY CERTIFICATION” work orders in the Orders grid
        //[006][cpatil][15-09-2022][GEOS2-3915]
        private void FillMainOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();
                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            #region Service comments
                            // [001]
                            // [002] changed service function
                            // [003] changed service function
                            // [004] changed service function
                            //[005] Changed service function
                            //[006] Changed service function
                            //Service GetPendingWorkorders_V2301 updated with GetPendingWorkorders_V2430 by [rdixit][8.08.2023][GEOS2-4754]
                            //[pramod.misal][GEOS2-5327][05.03.2024]
                            //Service GetPendingWorkorders_V2430 updated with GetPendingWorkorders_V2500 by [rdixit][12.03.2024][GEOS2-5361]
                            #endregion
                            SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == plant.IdCompany);
                            SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            //SAMService = new SAMServiceController("localhost:6699");
                            TempMainOtsList = new List<Ots>(SAMService.GetPendingWorkorders_V2660(plant)); // [nsatpute][13-11-2024][GEOS2-5889] //[nsatpute][12.08.2025][GEOS2-9163]
                            MainOtsList.AddRange(TempMainOtsList);
                            // Assuming TempMainOtsList is of type List<Ots>
                            //List<Byte> filteredIdDetectionsTemplateList = TempMainOtsList.Where(ots => ots.Quotation != null && ots.Quotation.IdDetectionsTemplate == 11)
                            //                                             .Select(ots => ots.Quotation.IdDetectionsTemplate).ToList();
                            //foreach (var ots in TempMainOtsList)
                            //{
                            //    if (ots.Quotation != null && (!ots.ExpectedEndDate.HasValue))
                            //    {
                            //        //bool isExpectedEndDateNull = !ots.ExpectedEndDate.HasValue;
                            //        bool containsTestboardTemplate = filteredIdDetectionsTemplateList.Contains(11);
                            //        ots.ShowClockIcon = containsTestboardTemplate;
                            //    }
                            //}
                        }
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        //SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());                       
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for refresh Lead Grid details.
        /// </summary>
        private void FillLeadGridDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                AddDataTableColumns();
                FillDataTable();
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadGridDetails() " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        /// [001][cpatil][09/05/2022][GEOS2-3750]- Display X and V OT's - Add the country flag in the column “Country”
        /// [002][cpatil][09/05/2022][GEOS2-3748]- Display X and V OT's - Edit VISION WorkOrder
        private void FillDataTable()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                }, x => { return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() }; }, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading data...";
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
                Dttable.Rows.Clear();
                DttableCopy = Dttable.Copy();
                MainOtsList_New = new List<Ots>();
                var results = MainOtsList.GroupBy(
                                p => p.IdOffer,
                                (key, g) => new { ParentId = key, OtList = g.ToList() });
                int i = 1;
                foreach (var res in results.ToList())
                {
                    i++;
                    Ots parent = MainOtsList.FirstOrDefault(b => b.IdOffer == res.ParentId);
                    DataRow dr1 = DttableCopy.NewRow();
                    dr1["IsParent"] = true;
                    dr1["ChildId"] = parent.IdQuotation;
                    dr1["ParentId"] = i;
                    dr1["OfferType"] = parent.Quotation.Offer.OfferType.IdOfferType;
                    dr1["Code"] = parent.OfferCode.ToString();
                    if (res.OtList.Where(a => a.OperatorNames == null && a.OperatorNames == "").ToList().Count == 0)
                    {
                        dr1["OperatorNames"] = "IsParent";
                        dr1["ParentOperatorNamesNull"] = string.Empty;
                        //dr1["ParentOperatorNamesNull"] = string.Join(",", res.OtList.Where(a => a.OperatorNames != null && a.OperatorNames != "").ToList().Select(a => a.OperatorNames));
                    }
                    //if (res.OtList.Any(a => a.OperatorNames == null || a.OperatorNames == ""))
                    //{
                    //    dr1["ParentOperatorNamesNull"] = string.Empty;
                    //}
                    //dr1["Type"] = parent.Quotation.Template.Name.ToString();
                    var MinExpectedDeliveryDate = res.OtList.Min(a => a.DeliveryDate);
                    if (MinExpectedDeliveryDate != null)
                    {
                        CultureInfo cul = CultureInfo.CurrentCulture;
                        dr1["DeliveryDate"] = MinExpectedDeliveryDate;
                        int Delay = res.OtList.FirstOrDefault(a => a.DeliveryDate == MinExpectedDeliveryDate).Delay;
                        dr1["Delay"] = Delay;
                    }
                    int SumProgress = res.OtList.Sum(a => a.Progress);
                    dr1["Progress"] = SumProgress/ res.OtList.Count();
                    var MinPlannedStartDate = res.OtList.Where(x => x.ExpectedStartDate != null && x.ExpectedStartDate != DateTime.MinValue).Min(a => a.ExpectedStartDate);
                    var MaxPlannedStartDate = res.OtList.Where(x => x.ExpectedEndDate != null && x.ExpectedEndDate != DateTime.MaxValue).Max(a => a.ExpectedEndDate);
                    if (MinPlannedStartDate != null && MaxPlannedStartDate != null)
                    {
                        if (MinPlannedStartDate != DateTime.MinValue)
                        {
                            dr1["PlannedStartDate"] = MinPlannedStartDate;
                        }
                        else
                            dr1["PlannedStartDate"] = DBNull.Value;
                        if (MaxPlannedStartDate != DateTime.MinValue)
                        {
                            dr1["PlannedEndDate"] = MaxPlannedStartDate;
                        }
                        else
                            dr1["PlannedEndDate"] = DBNull.Value;
                        //dr1["PlannedStartDate"] = MinPlannedStartDate;
                        //dr1["PlannedEndDate"] = MaxPlannedStartDate;
                        if (MinPlannedStartDate != DateTime.MinValue && MaxPlannedStartDate != DateTime.MinValue)
                        {
                            DateTime startdate = (DateTime)MinPlannedStartDate;
                            DateTime enddate = (DateTime)MaxPlannedStartDate;
                            dr1["PlannedDuration"] = (enddate - startdate).TotalDays;
                        }
                    }
                    dr1["Description"] = res.OtList.FirstOrDefault().Quotation.Offer.Description.ToString();
                    dr1["Modules"] = res.OtList.FirstOrDefault().Modules.ToString();
                    dr1["Group"] = res.OtList.FirstOrDefault().Quotation.Site.Customer.CustomerName.ToString();
                    dr1["Plant"] = res.OtList.FirstOrDefault().Quotation.Site.Name.ToString();
                    dr1["Country"] = res.OtList.FirstOrDefault().Quotation.Site.Country.Name.ToString();
                    dr1["CountryImageBytes"] = res.OtList.FirstOrDefault().Quotation.Site.Country.CountryIconBytes;//[001]
                    if (res.OtList.FirstOrDefault().Quotation.Offer.CarOEM != null)
                        dr1["OEM"] = res.OtList.FirstOrDefault().Quotation.Offer.CarOEM.Name;
                    if (res.OtList.FirstOrDefault().Quotation.Offer.CarProject != null)
                        dr1["Project"] = res.OtList.FirstOrDefault().Quotation.Offer.CarProject.Name.ToString();
                    if (res.OtList.FirstOrDefault().Quotation.Offer.BusinessUnit != null)
                        dr1["BusinessUnit"] = res.OtList.FirstOrDefault().Quotation.Offer.BusinessUnit.Value.ToString();
                    if (res.OtList.FirstOrDefault().PoDate != null)
                        dr1["PODate"] = res.OtList.FirstOrDefault().PoDate;
                    else
                        dr1["PODate"] = DBNull.Value;
                    if (res.OtList.Any(ol => ol.Quotation.IdDetectionsTemplate != 24) || res.OtList.Any(ol => ol.Quotation.IdDetectionsTemplate != 8))
                    {
                        dr1["Status"] = res.OtList.Where(ol => ol.Quotation.IdDetectionsTemplate != 24 || ol.Quotation.IdDetectionsTemplate != 8).ToList().FirstOrDefault().WorkflowStatus.Name.ToString();
                        dr1["HtmlColor"] = res.OtList.Where(ol => ol.Quotation.IdDetectionsTemplate != 24 || ol.Quotation.IdDetectionsTemplate != 8).ToList().FirstOrDefault().WorkflowStatus.HtmlColor.ToString();
                    }
                    if (res.OtList.FirstOrDefault().RealStartDate != null)
                        dr1["RealStartDate"] = res.OtList.FirstOrDefault().RealStartDate;
                    else
                        dr1["RealStartDate"] = DBNull.Value;
                    if (res.OtList.FirstOrDefault().RealEndDate != null)
                        dr1["RealEndDate"] = res.OtList.FirstOrDefault().RealEndDate;
                    else
                        dr1["RealEndDate"] = DBNull.Value;
                    if (res.OtList.FirstOrDefault().DeliveryWeek != null)
                    {
                        //CultureInfo cul = CultureInfo.CurrentCulture;
                        //dr1["ExpectedDeliveryWeek"] = res.OtList.FirstOrDefault().DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)res.OtList.FirstOrDefault().DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                        dr1["ExpectedDeliveryWeek"] = res.OtList.FirstOrDefault().DeliveryWeek;
                    }
                    dr1["RealDuration"] = res.OtList.FirstOrDefault().RealDuration;
                    if(res.OtList.Any(a => a.Quotation.Template.Name == "TESTBOARD" ||  a.Quotation.Template.Name == "ELECTRIFICATION" || a.Quotation.Template.Name == "QUALITY CERTIFICATION"))
                    dr1["Remarks"] =string.Join(",", res.OtList.Where(a => (a.Quotation.Template.Name == "TESTBOARD" || a.Quotation.Template.Name == "ELECTRIFICATION" || a.Quotation.Template.Name == "QUALITY CERTIFICATION") && !string.IsNullOrEmpty(a.Observations)).Select(a=>a.Observations));
                    try
                    {
                        //foreach (OptionsByOfferGrid item in ot.Quotation.Offer.OptionsByOffersGrid)
                        foreach (OptionsByOfferGrid item in res.OtList.FirstOrDefault().Quotation.Offer.OptionsByOffersGrid)
                        {
                            if (item.OfferOption != null)
                            {
                                if (item.IdOption.ToString() == "6" ||
                                     item.IdOption.ToString() == "19" ||
                                     item.IdOption.ToString() == "21" ||
                                     item.IdOption.ToString() == "23" ||
                                     item.IdOption.ToString() == "25" ||
                                     item.IdOption.ToString() == "27")
                                {
                                    var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                    int indexc = Columns.IndexOf(column);
                                    Columns[indexc].Visible = false;
                                }
                                else if (DttableCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                                {
                                    var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                    int indexc = Columns.IndexOf(column);
                                    Columns[indexc].Visible = true;
                                    dr1[item.OfferOption] = item.Quantity;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    try
                    {
                        if (res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid?.IdProductCategory > 0)
                        {
                            if (res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Category != null)
                                dr1["Category1"] = res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Category.Name;
                            if (res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.IdParent == 0)
                            {
                                dr1["Category1"] = res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Name;
                            }
                            else
                            {
                                dr1["Category2"] = res.OtList.FirstOrDefault().Quotation.Offer.ProductCategoryGrid.Name;
                            }
                        }
                        dr1["OfferStartDateMinValue"] = GeosApplication.Instance.ServerDateTime.Date;
                        dr1["ProducedModules"] = res.OtList.FirstOrDefault().ProducedModules;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    DttableCopy.Rows.Add(dr1);
                    MainOtsList_New.Add(parent);
                    //List<Ots> ChildOTList = MainOtsList.Where(b => b.IdOffer == res.ParentId).ToList();
                    foreach (Ots ot in res.OtList)
                    {
                        i++;
                        DataRow dr = DttableCopy.NewRow();
                        dr["IsParent"] = false;
                        dr["ChildId"] = i;
                        dr["ParentId"] = parent.IdQuotation;
                        dr["IdOt"] = ot.IdOT.ToString();
                        dr["OfferType"] = ot.Quotation.Offer.OfferType.IdOfferType;
                        dr["Code"] = ot.MergeCode.ToString();
                        dr["Type"] = ot.Quotation.Template.Name.ToString();
                        dr["Description"] = ot.Quotation.Offer.Description.ToString();
                        dr["Modules"] = ot.Modules.ToString();
                        dr["Group"] = ot.Quotation.Site.Customer.CustomerName.ToString();
                        dr["Plant"] = ot.Quotation.Site.Name.ToString();
                        dr["Country"] = ot.Quotation.Site.Country.Name.ToString();
                        dr["CountryImageBytes"] = ot.Quotation.Site.Country.CountryIconBytes;//[001]
                        if (ot.Quotation.Template.Name == "TESTBOARD" || ot.Quotation.Template.Name == "ELECTRIFICATION" || ot.Quotation.Template.Name == "QUALITY CERTIFICATION")
                            dr["Remarks"] = ot.Observations;
                        else
                            dr["Remarks"] = string.Empty;
                        if (ot.Quotation.Offer.CarOEM != null)
                            dr["OEM"] = ot.Quotation.Offer.CarOEM.Name;
                        if (ot.Quotation.Offer.CarProject != null)
                            dr["Project"] = ot.Quotation.Offer.CarProject.Name.ToString();
                        if (ot.Quotation.Offer.BusinessUnit != null)
                            dr["BusinessUnit"] = ot.Quotation.Offer.BusinessUnit.Value.ToString();
                        if (ot.PoDate != null)
                            dr["PODate"] = ot.PoDate;
                        else
                            dr["PODate"] = DBNull.Value;
                        if (ot.Quotation.IdDetectionsTemplate != 24 || ot.Quotation.IdDetectionsTemplate != 8)
                        {
                            dr["Status"] = ot.WorkflowStatus.Name.ToString();
                            dr["HtmlColor"] = ot.WorkflowStatus.HtmlColor.ToString();
                        }
                        if (ot.DeliveryDate != null)
                            dr["DeliveryDate"] = ot.DeliveryDate;
                        else
                            dr["DeliveryDate"] = DBNull.Value;
                        if (ot.ExpectedStartDate != DateTime.MinValue && ot.ExpectedStartDate != null)
                        {
                            dr["PlannedStartDate"] = ot.ExpectedStartDate;
                        }
                        else
                            dr["PlannedStartDate"] = DBNull.Value;
                        if (ot.ExpectedEndDate != DateTime.MinValue && ot.ExpectedEndDate != null)
                        {
                            dr["PlannedEndDate"] = ot.ExpectedEndDate;
                        }
                        else
                            dr["PlannedEndDate"] = DBNull.Value;
                        //if (ot.ExpectedStartDate != null)
                        //    dr["PlannedStartDate"] = ot.ExpectedStartDate;
                        //else
                        //    dr["PlannedStartDate"] = DBNull.Value;
                        //if (ot.ExpectedEndDate != null)
                        //    dr["PlannedEndDate"] = ot.ExpectedEndDate;
                        //else
                        //    dr["PlannedEndDate"] = DBNull.Value;
                        if (ot.ExpectedStartDate != null && ot.ExpectedEndDate != null)
                        {
                            if (ot.ExpectedStartDate != DateTime.MinValue && ot.ExpectedEndDate != DateTime.MinValue)
                            {
                                DateTime startdate = (DateTime)ot.ExpectedStartDate;
                                DateTime enddate = (DateTime)ot.ExpectedEndDate;
                                dr["PlannedDuration"] = (enddate - startdate).TotalDays;
                            }
                        }
                        if (ot.RealStartDate != null)
                            dr["RealStartDate"] = ot.RealStartDate;
                        else
                            dr["RealStartDate"] = DBNull.Value;
                        if (ot.RealEndDate != null)
                            dr["RealEndDate"] = ot.RealEndDate;
                        else
                            dr["RealEndDate"] = DBNull.Value;
                        //if (ot.DeliveryDate != null)
                        //{
                        //    CultureInfo cul = CultureInfo.CurrentCulture;
                        //    dr["ExpectedDeliveryWeek"] = ot.DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)ot.DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                        //}
                        if (ot.DeliveryWeek != null)
                        {                           
                            dr["ExpectedDeliveryWeek"] = ot.DeliveryWeek;
                        }                    
                        dr["Delay"] = ot.Delay;
                        dr["Progress"] = ot.Progress;
                        //dr["PlannedDuration"] = ot.RealDuration;
                        dr["RealDuration"] = ot.RealDuration;
                        //[pramod.misal][GEOS2-5888][22.08.2024] 
                        //ot.Quotation.Template.Name == "TESTBOARD"
                        //ot.Quotation.IdDetectionsTemplate == 11
                        //if (ot.Quotation != null && string.IsNullOrEmpty(ot.RealEndDate?.ToString()) && !string.IsNullOrEmpty(ot.IdOT.ToString()))
                        //{   
                        //    if (ot.Quotation.Template.Name == "TESTBOARD")
                        //    {
                        //        dr["Clock"] = 1;
                        //    }
                        //    else
                        //    {
                        //        dr["Clock"] = 0;
                        //    }
                        //}
                        //else
                        //{
                        //    dr["Clock"] = DBNull.Value;
                        //}
                        if (ot.EndTimeFlag != 0)
                        {
                            if (ot.Quotation != null && string.IsNullOrEmpty(ot.EndDateTime?.ToString()) && !string.IsNullOrEmpty(ot.IdOT.ToString()))
                            {
                                if (ot.Quotation.Template.Name == "TESTBOARD")
                                {
                                    dr["Clock"] = 1;
                                }
                                else
                                {
                                    dr["Clock"] = 0;
                                }
                            }
                            else
                            {
                                dr["Clock"] = DBNull.Value;
                            }
                        }
                        else
                        {
                            dr["Clock"] = DBNull.Value;
                        }
                        //End [pramod.misal][GEOS2-5888][21.08.2024]
                        try
                        {
                            //foreach (OptionsByOfferGrid item in ot.Quotation.Offer.OptionsByOffersGrid)
                            foreach (OptionsByOfferGrid item in ot.Quotation.Offer.OptionsByOffersGrid)
                            {
                                if (item.OfferOption != null)
                                {
                                    if (item.IdOption.ToString() == "6" ||
                                         item.IdOption.ToString() == "19" ||
                                         item.IdOption.ToString() == "21" ||
                                         item.IdOption.ToString() == "23" ||
                                         item.IdOption.ToString() == "25" ||
                                         item.IdOption.ToString() == "27")
                                    {
                                        var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                        int indexc = Columns.IndexOf(column);
                                        Columns[indexc].Visible = false;
                                    }
                                    else if (DttableCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                                    {
                                        var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                        int indexc = Columns.IndexOf(column);
                                        Columns[indexc].Visible = true;
                                        dr[item.OfferOption] = item.Quantity;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        try
                        {
                            if (ot.Quotation.Offer.ProductCategoryGrid?.IdProductCategory > 0)
                            {
                                if (ot.Quotation.Offer.ProductCategoryGrid.Category != null)
                                    dr["Category1"] = ot.Quotation.Offer.ProductCategoryGrid.Category.Name;
                                if (ot.Quotation.Offer.ProductCategoryGrid.IdParent == 0)
                                {
                                    dr["Category1"] = ot.Quotation.Offer.ProductCategoryGrid.Name;
                                }
                                else
                                {
                                    dr["Category2"] = ot.Quotation.Offer.ProductCategoryGrid.Name;
                                }
                            }
                            dr["OfferStartDateMinValue"] = GeosApplication.Instance.ServerDateTime.Date;
                            dr["ProducedModules"] = ot.ProducedModules;
                            dr["OperatorNames"] = ot.OperatorNames;
                            dr["ParentOperatorNamesNull"] = ot.OperatorNames;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        DttableCopy.Rows.Add(dr);
                        MainOtsList_New.Add(ot);
                    }
                }
                try
                {
                    //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
                    //var parentIds = DttableCopy.AsEnumerable().Select(row => row["ParentId"]).ToList();
                    //// check duplicate ParentId
                    //var duplicateParentIds = parentIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => new { Value = g.Key, Count = g.Count() }).ToList();
                    //DttableCopy.PrimaryKey = null;
                    //DttableCopy.Columns.Cast<DataColumn>().ToList().ForEach(c => c.Unique = false);

                    var childId = DttableCopy.AsEnumerable().Select(row => row["ChildId"]).ToList();
                    // check duplicate ChildId
                    var duplicateChildIds = childId.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => new { Value = g.Key, Count = g.Count() }).ToList();
                    // get distinct rows by ChildId (keep the first occurrence)
                    var distinctRows = DttableCopy.AsEnumerable().GroupBy(r => r["ChildId"]).Select(g => g.First()).CopyToDataTable();
                    // replace the old table
                    DttableCopy = distinctRows;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                Dttable = DttableCopy;
                //----------[Sprint - 89][GEOS2 - 2377][05 - 12 - 2020][adhatkar]-------- -
                if (GeosApplication.Instance.MainOtsList.Count == 0)
                {
                    GeosApplication.Instance.MainOtsList = MainOtsList_New;
                }
                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            }
        /// <summary>
        /// [001][sjadhav][26/06/2020][GEOS2-2381]- Add new columns in Work Orders grid
        /// [002][cpatil][09/05/2022][GEOS2-3750]- Display X and V OT's - Add the country flag in the column “Country”
        /// </summary>
        private void AddDataTableColumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns ...", category: Category.Info, priority: Priority.Low);
                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM>() {
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="",HeaderText=" ", Settings = SettingsTypeSAM.Empty, AllowCellMerge=false, Width=40,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        //[pramod.misal][GEOS2-5888][20.08.2024]
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Clock",HeaderText="Clock", Settings = SettingsTypeSAM.Clock, AllowCellMerge=false,Width=40,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Type",HeaderText="Type", Settings = SettingsTypeSAM.Type, AllowCellMerge=false, Width=130,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="OfferType",HeaderText="OfferType", Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false, Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="IdOt",HeaderText="IdOt", Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false, Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Code",HeaderText="Code", Settings = SettingsTypeSAM.OfferCode, AllowCellMerge=false, Width=125,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Description",HeaderText="Description", Settings = SettingsTypeSAM.Description, AllowCellMerge=false, Width=250,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Modules", HeaderText="#Modules", Settings = SettingsTypeSAM.Modules, AllowCellMerge=false, Width=70,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Group", HeaderText="Group", Settings = SettingsTypeSAM.Group, AllowCellMerge=false, Width=160,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Plant", HeaderText="Plant", Settings = SettingsTypeSAM.Plant, AllowCellMerge=false, Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Country", HeaderText="Country", Settings = SettingsTypeSAM.Default, AllowCellMerge=false, Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Project", HeaderText="Project", Settings = SettingsTypeSAM.Project, AllowCellMerge=false, Width=60,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="OEM", HeaderText="OEM", Settings = SettingsTypeSAM.OEM, AllowCellMerge=false, Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="BusinessUnit", HeaderText="Business Unit", Settings = SettingsTypeSAM.BusinessUnit, AllowCellMerge=false, Width=140,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PODate", HeaderText="PO Date",Settings = SettingsTypeSAM.PODate, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Status", HeaderText="Status",Settings = SettingsTypeSAM.Status, AllowCellMerge=false,Width=130,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="HtmlColor", HeaderText="HtmlColor",Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="DeliveryDate", HeaderText="Expected Delivery Date",Settings = SettingsTypeSAM.DeliveryDate, AllowCellMerge=false,Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="ExpectedDeliveryWeek", HeaderText="Expected Delivery Week", Settings = SettingsTypeSAM.DeliveryWeek, AllowCellMerge=false, Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Delay", HeaderText="Delay", Settings = SettingsTypeSAM.Delay, AllowCellMerge=false, Width=40,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Progress", HeaderText="Progress",Settings = SettingsTypeSAM.PercentText, AllowCellMerge=false,Width=60,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PlannedStartDate", HeaderText="Planned Start Date",Settings = SettingsTypeSAM.PlannedStartDate, AllowCellMerge=false,Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PlannedEndDate", HeaderText="Planned End Date",Settings = SettingsTypeSAM.PlannedEndDate, AllowCellMerge=false,Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },                        
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PlannedDuration", HeaderText="Planned Duration(d)",Settings = SettingsTypeSAM.PlannedDuration, UnboundType="Integer",AllowCellMerge=false,Width=120,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="RealStartDate", HeaderText="Real Start Date",Settings = SettingsTypeSAM.RealStartDate, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="RealEndDate", HeaderText="Real End Date",Settings = SettingsTypeSAM.RealEndDate, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="RealDuration", HeaderText="Real Duration(h)",Settings = SettingsTypeSAM.RealDuration, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Category1", HeaderText="Category1",Settings = SettingsTypeSAM.Category1, AllowCellMerge=false,Width=200,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Category2", HeaderText="Category2",Settings = SettingsTypeSAM.Category2, AllowCellMerge=false,Width=200,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },                       
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="OperatorNames", HeaderText="OperatorNames",Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Remarks", HeaderText="Remarks",Settings = SettingsTypeSAM.Remark, AllowCellMerge=false,Width=200,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=false },
                          new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="CountryImageBytes", HeaderText="CountryImageBytes",Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                 }; 
                 Dttable = new DataTable();
                Dttable.Columns.Add("IdOt", typeof(long));
                Dttable.Columns.Add("OfferType", typeof(byte));
                //[pramod.misal][GEOS2-5888][20.08.2024]
                Dttable.Columns.Add("Clock", typeof(bool));
                Dttable.Columns.Add("Type", typeof(string));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("Description", typeof(string));
                Dttable.Columns.Add("Modules", typeof(string));
                Dttable.Columns.Add("Group", typeof(string));
                Dttable.Columns.Add("Plant", typeof(string));
                Dttable.Columns.Add("Country", typeof(string));
                Dttable.Columns.Add("Project", typeof(string));
                Dttable.Columns.Add("OEM", typeof(string));
                Dttable.Columns.Add("BusinessUnit", typeof(string));
                Dttable.Columns.Add("PODate", typeof(DateTime));
                Dttable.Columns.Add("Status", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("DeliveryDate", typeof(DateTime));
                Dttable.Columns.Add("ExpectedDeliveryWeek", typeof(string));
                Dttable.Columns.Add("Delay", typeof(Int32));
                Dttable.Columns.Add("Progress", typeof(string));
                Dttable.Columns.Add("PlannedStartDate", typeof(DateTime));
                Dttable.Columns.Add("PlannedEndDate", typeof(DateTime));
                Dttable.Columns.Add("OfferStartDateMinValue", typeof(DateTime));
                Dttable.Columns.Add("PlannedDuration", typeof(string));
                Dttable.Columns.Add("RealStartDate", typeof(DateTime));
                Dttable.Columns.Add("RealEndDate", typeof(DateTime));                
                Dttable.Columns.Add("RealDuration", typeof(string));
                Dttable.Columns.Add("Category1", typeof(string));
                Dttable.Columns.Add("Category2", typeof(string));
                Dttable.Columns.Add("OperatorNames", typeof(string));
                Dttable.Columns.Add("ChildId", typeof(string)).DefaultValue = string.Empty;
                Dttable.Columns.Add("ParentId", typeof(string)).DefaultValue = string.Empty;
                Dttable.Columns.Add("IsParent", typeof(bool));
                Dttable.Columns.Add("ParentOperatorNamesNull", typeof(string)).DefaultValue = string.Empty;
                Dttable.Columns.Add("Remarks", typeof(string));
                Dttable.Columns.Add("CountryImageBytes", typeof(byte[]));//[002]
                //added site column in last on the grid.
                OfferOptions = CrmStartUp.GetAllOfferOptions();
                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!Dttable.Columns.Contains(OfferOptions[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsTypeSAM.Array, AllowCellMerge = false, Width = 40, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = false });
                        Dttable.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));
                    }
                }
                Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName = "ProducedModules", HeaderText = "Produced Modules", Settings = SettingsTypeSAM.ProducedModules, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = false });
                Dttable.Columns.Add("ProducedModules", typeof(Int32));
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method To Fill Pending Work Order Filter List
        /// </summary>
        public void FillPendingWorkOrderFilterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList....", category: Category.Info, priority: Priority.Low);
                ListOfTemplate = new ObservableCollection<Template>();
                FillPendingWorkOrderFilterTiles(ListOfTemplate, MainOtsList);
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method To Fill Pending Work Order Filter Tiles 
        /// [001][skale][07/11/2019][GEOS2-1758]- Create Orders section with the grid of working orders
        /// </summary>
        public void FillPendingWorkOrderFilterTiles(ObservableCollection<Template> FilterList, List<Ots> MainListWorkOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);
                QuickFilterList = new ObservableCollection<TileBarFilters>();
                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllWO").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(),
                    //ImageUri = "Template.png",
                    //BackColor = "Wheat",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });
                foreach (Ots ot in MainListWorkOrder)
                {
                    if (ot.UserShortDetails != null && ot.UserShortDetails.Count > 0)
                    {
                        foreach (UserShortDetail user in ot.UserShortDetails)
                        {
                            ImageSource userImage = null;
                            //[001] added
                            if (user.UserImageInBytes == null)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    if (user != null && user.IdUserGender == 1)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png"));
                                    else if (user != null && user.IdUserGender == 2)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png"));
                                }
                                else
                                {
                                    if (user != null && user.IdUserGender == 1)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png"));
                                        else if (user != null && user.IdUserGender == 2)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png"));
                                }
                            }
                            else
                            {
                                userImage = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes);
                            }
                            if (!QuickFilterList.Any(x => x.Id == user.IdUser))
                            {
                                QuickFilterList.Add(new TileBarFilters()
                                {
                                    Caption = user.UserName,
                                    Id = user.IdUser,
                                    Type = user.UserName,
                                    EntitiesCount = MainListWorkOrder.Count(x => !string.IsNullOrEmpty(x.OperatorNames) && x.OperatorNames.Split(',').ToList().Contains(user.UserName)),
                                    // Image = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes),   
                                    Image = userImage,
                                    //BackColor = item.HtmlColor,
                                    EntitiesCountVisibility = Visibility.Visible,
                                    Height = 80,
                                    width = 200
                                });
                            }
                        }
                    }
                }
                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("UnassignedWO").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(x => string.IsNullOrEmpty(x.OperatorNames)),
                    //ImageUri = "Template.png",
                    //BackColor = "Wheat",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });
                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = System.Windows.Application.Current.FindResource("CustomFilters").ToString(),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });
                filterList = new List<TileBarFilters>();
                filterList = QuickFilterList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for showing Grid as per Filter Tile Selection
        /// </summary>
        private void ShowSelectedFilterGridAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);
                if (QuickFilterList.Count > 0)
                {
                    System.Windows.Controls.SelectionChangedEventArgs obj = (System.Windows.Controls.SelectionChangedEventArgs)e;
                    string Template = null;
                    string _FilterString = null;
                    if (obj.AddedItems.Count > 0)
                    {
                        //int IdTemplate = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Id;
                        Template = ((TileBarFilters)(obj.AddedItems)[0]).Type;
                        _FilterString = ((TileBarFilters)(obj.AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)(obj.AddedItems)[0]).Caption;
                    }
                    if (CustomFilterStringName != null && CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;
                    if (CustomFilterStringName == "Unassigned")
                    {
                        FilterString = "IsNullOrEmpty([OperatorNames])";
                        return;
                    }
                    if (Template == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            FilterString = _FilterString;
                        else
                            FilterString = string.Empty;
                    }
                    else
                    {
                        //FilterString = "[OperatorNames] In ('" + Template + "')";
                        FilterString = "Contains([OperatorNames], '" + Template + "')";
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for showing Grid's selected row Item detailed Window
        ///[001][cpatil][02-08-2021][GEOS2-2906] Include the “STRUCTURE” orders in the Orders grid
        /// [002][cpatil][09/05/2022][GEOS2-3748]- Display X and V OT's - Edit VISION WorkOrder
        /// [003][cpatil][09/05/2022][GEOS2-3749]- Display X and V OT's - Edit ELECTRIFICATION WorkOrder
        /// </summary>
        private void ShowSelectedGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                TreeListView detailView = (TreeListView)obj;
                long OtId= Convert.ToInt64(((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[0].ToString());
                Ots FoundRow = MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault();
                string serviceProviderUrl = FoundRow.Site.ServiceProviderUrl;
                //[001]
                //[002]
                if (FoundRow.Quotation.IdDetectionsTemplate == 24 || FoundRow.Quotation.IdDetectionsTemplate == 8)
                {
                    WorkOrderItemDetailsForStructureViewModel workOrderItemDetailsForStructureViewModel = new WorkOrderItemDetailsForStructureViewModel();
                    WorkOrderItemDetailsForStructureView workOrderItemDetailsForStructureView = new WorkOrderItemDetailsForStructureView();
                    EventHandler handle1 = delegate { workOrderItemDetailsForStructureView.Close(); };
                    workOrderItemDetailsForStructureViewModel.RequestClose += handle1;
                    workOrderItemDetailsForStructureViewModel.InitRemoteService(serviceProviderUrl); //[nsatpute][18-02-2025][GEOS2-6997]
                    workOrderItemDetailsForStructureViewModel.Init(FoundRow);
                    workOrderItemDetailsForStructureView.DataContext = workOrderItemDetailsForStructureViewModel;
                    workOrderItemDetailsForStructureView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    FillDataTable();
                }
                else if (FoundRow.Quotation.IdDetectionsTemplate == 19)
                {
                    WorkOrderItemDetailsForElectrificationViewModel workOrderItemDetailsForElectrificationViewModel = new WorkOrderItemDetailsForElectrificationViewModel();
                    WorkOrderItemDetailsForElectrificationView workOrderItemDetailsForElectrificationView = new WorkOrderItemDetailsForElectrificationView();
                    EventHandler handle = delegate { workOrderItemDetailsForElectrificationView.Close(); };
                    workOrderItemDetailsForElectrificationViewModel.RequestClose += handle;
                    workOrderItemDetailsForElectrificationViewModel.InitRemoteService(serviceProviderUrl); //[nsatpute][18-02-2025][GEOS2-6997]
                    workOrderItemDetailsForElectrificationViewModel.Init(FoundRow);
                    workOrderItemDetailsForElectrificationView.DataContext = workOrderItemDetailsForElectrificationViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsForElectrificationView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    //if (workOrderItemDetailsForElectrificationViewModel.WorkflowStatus != null)
                    //{
                    //    MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                    //    MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                    //    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                    //    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                    //}
                    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsForElectrificationViewModel.Remark;
                    FillDataTable();
                }
                else if(FoundRow.Quotation.IdDetectionsTemplate == 18)
                {
                    WorkOrderItemDetailsForQualityCertificationViewModel workOrderItemDetailsForQualityCertificationViewModel = new WorkOrderItemDetailsForQualityCertificationViewModel();
                    WorkOrderItemDetailsForQualityCertificationView workOrderItemDetailsForQualityCertificationView = new WorkOrderItemDetailsForQualityCertificationView();
                    EventHandler handle = delegate { workOrderItemDetailsForQualityCertificationView.Close(); };
                    workOrderItemDetailsForQualityCertificationViewModel.RequestClose += handle;
                    workOrderItemDetailsForQualityCertificationViewModel.InitRemoteService(serviceProviderUrl); //[nsatpute][18-02-2025][GEOS2-6997]
                    workOrderItemDetailsForQualityCertificationViewModel.Init(FoundRow);
                    workOrderItemDetailsForQualityCertificationView.DataContext = workOrderItemDetailsForQualityCertificationViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsForQualityCertificationView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    if (workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus != null)
                    {
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus;
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus.IdWorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus.IdWorkflowStatus;
                    }
                    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsForQualityCertificationViewModel.Remark;
                    FillDataTable();
                }
                else
                {
                    WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                    WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                    EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                    workOrderItemDetailsViewModel.RequestClose += handle;
                    workOrderItemDetailsViewModel.InitRemoteService(serviceProviderUrl); //[nsatpute][18-02-2025][GEOS2-6997]
                    workOrderItemDetailsViewModel.Init(FoundRow);
                    workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    if (workOrderItemDetailsViewModel.WorkflowStatus != null)
                    {
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                    }
                    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsViewModel.Remark;
                    FillDataTable();
                }
                //workOrderItemDetailsViewModel.Init(FoundRow);
                //workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //workOrderItemDetailsView.ShowDialogWindow();
                //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //if (workOrderItemDetailsViewModel.WorkflowStatus != null)
                //{
                //    MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                //    MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                //    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                //    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                //}
                //FillDataTable();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
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
                PrintableControlLink pcl = new PrintableControlLink((TreeListView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TreeListView)obj).Resources["PendingWorkOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TreeListView)obj).Resources["PendingWorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending Work Order List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();
                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
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
                    ResultFileName = (saveFile.FileName);
                    TreeListView activityTableView = ((TreeListView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    options.CustomizeDocumentColumn += Options_CustomizeDocumentColumn;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Open Scan ot view
        /// [000][skale][11-12-2019][GEOS2-1881] Add new option to Log the worked time in an OT
        /// </summary>
        /// <param name="obj"></param>
        private void ScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);
                WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("LogWork").ToString();
                workOrderScanViewModel.Init(MainOtsList_New);
                EventHandler handler = delegate { workOrderScanView.Close(); };
                workOrderScanViewModel.RequestClose += handler;
                workOrderScanView.DataContext = workOrderScanViewModel;
                workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private List<GeosAppSetting> copyGeosAppSettingList = null;
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            string htmlColor = string.Empty;
            DateTime todaysDate = DateTime.Now.Date;
            if (copyGeosAppSettingList == null)
            {
                copyGeosAppSettingList = ((List<GeosAppSetting>)GeosAppSettingList);
            }
            if (e.ColumnFieldName == "RowData.Row")
            {
                if(e.Value!=" ")
                {
                    var Type = ((System.Data.DataRowView)e.Value);
                    DataRowView dr = (DataRowView)Type;
                    byte idoffertype = (byte)dr.Row["OfferType"];
                    DateTime DeliveryDate = (DateTime)dr.Row["DeliveryDate"];
                    if (idoffertype == 2 || idoffertype == 3)
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 17)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //red
                    }
                    else if (DeliveryDate.Date <= todaysDate)
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 14)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //yellow
                    }
                    else if (DeliveryDate.Date > todaysDate && DeliveryDate.Date <= todaysDate.AddDays(6))
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 15)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //Green
                    }
                    else if (DeliveryDate.Date >= todaysDate.AddDays(7))
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 16)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //Blue
                    }
                    e.Value = null;
                }
            }
            e.Formatting.Alignment = new XlCellAlignment()
            {
                WrapText = true,
            };
            if (e.Value != null && OfferOptions.Any(a=>a.Name==e.Value.ToString()) && OfferOptions.Any(a=>a.Name== e.ColumnFieldName))
            {
                e.Formatting.Alignment = new XlCellAlignment()
                {
                    WrapText = true,
                    TextRotation = 90,
                };
            }
            if (e.Value!=null && e.Value.ToString() != "Planned Duration(d)" && e.ColumnFieldName == "PlannedDuration")
            {
                e.Formatting.Alignment = new XlCellAlignment()
                {
                    WrapText = true,
                    HorizontalAlignment = XlHorizontalAlignment.Right
                };
            }
            if (e.Value != null && e.Value.ToString() != "#Modules" && e.ColumnFieldName == "Modules")
            {
                e.Formatting.Alignment = new XlCellAlignment()
                {
                    WrapText = true,
                    HorizontalAlignment = XlHorizontalAlignment.Right
                };
            }
            e.Handled = true;
        }
        private void Options_CustomizeDocumentColumn(CustomizeDocumentColumnEventArgs e)
        {
            if (e.ColumnFieldName == "Quotation.Offer.IsCritical")
                e.DocumentColumn.WidthInPixels = 90;
        }
        public void Dispose()
        {           
        }
        private void RefundWorkOrderCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()...", category: Category.Info, priority: Priority.Low);
                //WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                //WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                //workOrderScanViewModel.WindowHeader = Application.Current.FindResource("RefundWorkOrderHeader").ToString();
                //workOrderScanViewModel.Init(MainOtsList);
                //workOrderScanViewModel.IsRefund = true;
                //EventHandler handler = delegate { workOrderScanView.Close(); };
                //workOrderScanViewModel.RequestClose += handler;
                //workOrderScanView.DataContext = workOrderScanViewModel;
                //workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TreeListView table = (TreeListView)obj.OriginalSource;
            TreeListControl gridControl = (TreeListControl)(table).DataControl;
            ShowFilterEditor(obj);
        }
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;
                customFilterEditorViewModel.Init(e.FilterControl, QuickFilterList);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();
                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        QuickFilterList.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");
                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleChildRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");
                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    QuickFilterList.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleChildRowCount
                    });
                    string filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = QuickFilterList.LastOrDefault();
                }
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddCustomSetting()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                }, x => { return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() }; }, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading filter...";
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    foreach (var item in tempUserSettings)
                    {
                        ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Ots)), item.Value, false);
                        List<Ots> tempList = new List<Ots>();
                        foreach (var ot in MainOtsList_New)
                        {
                            if (evaluator.Fit(ot))
                                tempList.Add(ot);
                        }
                        FilterString = item.Value;
                        QuickFilterList.Add(new TileBarFilters()
                        {
                            Caption = item.Key.Replace(userSettingsKey, ""),
                            Id = 0,
                            BackColor = null,
                            ForeColor = null,
                            FilterCriteria = item.Value,
                            EntitiesCount = tempList.Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                //if (ListOfTemplate.Any(x => x.Name == CustomFilterStringName) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()))
                //    return;
                foreach(var item in filterList)
                {
                    if(CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Caption))
                            return;
                    }
                }
                TreeListView table = (TreeListView)obj;
                TreeListControl gridControl = (TreeListControl)(table).DataControl;
                List<TreeListColumn> GridColumnList = gridControl.Columns.Where(x => x.FieldName != null).ToList();
                string columnName = FilterString.Substring(FilterString.IndexOf("[") + 1, FilterString.IndexOf("]") - 2 - FilterString.IndexOf("[") + 1);
                TreeListColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals(columnName));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowingEditorCommandAction(object e)
        {
            if(((TreeListShowingEditorEventArgs)e).Node.Level==0)
            {
                ((TreeListShowingEditorEventArgs)e).Cancel = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                if (File.Exists(PendingWorkOrdersGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl.RestoreLayoutFromXml(PendingWorkOrdersGridSettingFilePath);
                    TreeListControl gridControlView = (TreeListControl)((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl;
                    TreeListView tableView = (TreeListView)gridControlView.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }
                ((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl.SaveLayoutToXml(PendingWorkOrdersGridSettingFilePath);
                TreeListControl gridControl = (TreeListControl)((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl;
                foreach (TreeListColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(TreeListColumn.VisibleProperty, typeof(TreeListColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }
                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(TreeListColumn.VisibleIndexProperty, typeof(TreeListColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }
                    if (column.Visible == false)
                    {
                        visibleFalseColumn++;
                    }
                }
                if (visibleFalseColumn > 0)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }
                TreeListView datailView = ((TreeListView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                if (datailView.FormatConditions == null || datailView.FormatConditions.Count == 0)
                {
                    var profitFormatCondition = new FormatCondition()
                    {
                        Expression = "[DeliveryDate] <= LocalDateTimeToday()",
                        FieldName = "ExpectedDeliveryWeek",
                        Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                        {
                            Foreground = Brushes.Red
                        }
                    };
                    datailView.FormatConditions.Add(profitFormatCondition);
                }
                //datailView.BestFitColumns();
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == TreeListControl.FilterStringProperty)
                e.Allow = false;
            if (e.Property.Name == "GroupCount")
                e.Allow = false;
            if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                e.Allow = false;
            if (e.Property.Name == "FormatConditions")
                e.Allow = false;
        }
        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);
                TreeListColumn column = sender as TreeListColumn;
                if (column.ShowInColumnChooser)
                {
                    if((TreeListControl)((System.Windows.FrameworkContentElement)sender).Parent!=null)
                    ((TreeListControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingWorkOrdersGridSettingFilePath);
                }
                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                TreeListColumn column = sender as TreeListColumn;
                ((TreeListControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingWorkOrdersGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillListOfColor() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillListOfColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillListOfColor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomUnboundColumnDataAction(object e)
        {
            TreeListCellValueChangedEventArgs obj = (TreeListCellValueChangedEventArgs)e;
            //e = Dttable;
            if (obj.Column.FieldName == "PlannedStartDate" || obj.Column.FieldName== "PlannedEndDate")
            {
                DateTime? PlannedStartDate = null;
                DateTime? PlannedEndDate = null;
                DataRowView dr = (DataRowView)obj.Row;
                if (dr.Row["PlannedStartDate"].ToString() != "")
                {
                    PlannedStartDate = Convert.ToDateTime(dr.Row["PlannedStartDate"].ToString());
                }
                if (dr.Row["PlannedEndDate"].ToString() != "")
                {
                    PlannedEndDate = Convert.ToDateTime(dr.Row["PlannedEndDate"].ToString());
                }
                if(PlannedStartDate!=null && PlannedEndDate!=null)
                {
                    dr.Row["PlannedDuration"] = ((DateTime)PlannedEndDate - (DateTime)PlannedStartDate).TotalDays;
                }
                //Ots item = MainOtsList[obj.ListSourceRowIndex];
                //if (item.ExpectedStartDate != null && item.ExpectedEndDate != null)
                //{
                //    DateTime startdate = (DateTime)item.ExpectedStartDate;
                //    DateTime enddate = (DateTime)item.ExpectedEndDate;
                //    obj.Value = (enddate - startdate).TotalDays;
                //}
            }
            //if (e.Column.FieldName == "ExpectedDeliveryWeek")
            //{
            //    Ots item = MainOtsList[e.ListSourceRowIndex];
            //    if(item.DeliveryDate != null)
            //    {
            //        CultureInfo cul = CultureInfo.CurrentCulture;
            //        e.Value = item.DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)item.DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
            //    }
            //}
        }
        /// <summary>
        /// Open Scan ot view
        /// [000][psutar][04-05-2020][GEOS2-1881] scan OT validation
        /// </summary>
        /// <param name="obj"></param>
        private void ScanWorkOrderValidation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanWorkOrderValidation...", category: Category.Info, priority: Priority.Low);
                WorkOrderScanValidationView workOrderScanView = new WorkOrderScanValidationView();
                WorkOrderScanValidationViewModel workOrderScanViewModel = new WorkOrderScanValidationViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("Validation").ToString();
                workOrderScanViewModel.Init(MainOtsList_New);
                EventHandler handler = delegate { workOrderScanView.Close(); };
                workOrderScanViewModel.RequestClose += handler;
                workOrderScanView.DataContext = workOrderScanViewModel;
                workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method ScanWorkOrderValidation executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanWorkOrderValidation() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomSummaryCommandAction(object obj)
        {
            TreeListCustomSummaryEventArgs e = (TreeListCustomSummaryEventArgs)obj;
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                emptyCellsTotalCount = 0;
            // Calculation.
            if(e.Node==null && e.SummaryProcess != DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                emptyCellsTotalCountOld = 0;
                oldid = 0;
            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                if (e.Node.ActualLevel == 1)
                {
                    if(oldid != e.Node.Id)
                    {
                        emptyCellsTotalCount=1;
                        if (e.TotalValue != null)
                        {
                            e.TotalValue = emptyCellsTotalCountOld + emptyCellsTotalCount;
                            emptyCellsTotalCountOld = (int)e.TotalValue;
                        }
                        else
                        {
                            e.TotalValue = emptyCellsTotalCountOld+emptyCellsTotalCount;
                            emptyCellsTotalCountOld = (int)e.TotalValue;
                        }
                        oldid = e.Node.Id;
                    }
                }
            // Finalization.
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                e.TotalValue = emptyCellsTotalCountOld;
                VisibleChildRowCount = emptyCellsTotalCountOld;
            }
        }
        #endregion //End Of Methods
    }
}