using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{

    #region Task_Comments
    //[pramod,misal][GEOS2-4815][25.09.2023]
    #endregion

    public class EmployeeTripsViewModel : INotifyPropertyChanged
    {

        #region Service

          IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private ObservableCollection<EmployeeTrips> employeeTripsList;
        private EmployeeTrips selectedEmployeeTrips;
        bool isEnableField = false;
        int idUserPermission;
        //[rushikesh.gaikwad][GEOS2-5927][28.08.2024]
        private ObservableCollection<TileBarFilters> _filterStatusListOfTile;
        private ObservableCollection<WorkflowStatus> tripStatusList;
        private TileBarFilters _selectedTileBarItem;
        public bool isDelVisible;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;
        private bool isFromFilterString = false;
        string myFilterString;
        private TileBarFilters selectedFilter;
        private string userSettingsKey = "HRM_TripsReport_";
        public string HRM_TripsReportGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "TripsReportGridSetting.Xml";
        private bool isEdit;
        private int visibleRowCount;
        private List<GridColumn> GridColumnList;
        private bool isDeleted;
        #endregion

        #region Properties

        public ObservableCollection<WorkflowStatus> TripStatusList
        {
            get
            {
                return tripStatusList;
            }

            set
            {
                tripStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TripStatusList"));
            }
        }
        public bool IsEnableField
        {
            get
            {
                return isEnableField;
            }

            set
            {
                isEnableField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableField"));
            }
        }
        public int IdUserPermission
        {
            get
            {
                return idUserPermission;
            }

            set
            {
                idUserPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeTripsList"));
            }
        }
        public ObservableCollection<EmployeeTrips> EmployeeTripsList
        {
            get
            {
                return employeeTripsList;
            }

            set
            {
                employeeTripsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeTripsList"));
            }
        }

        public EmployeeTrips SelectedEmployeeTrips
        {
            get
            {
                return selectedEmployeeTrips;
            }
            set
            {
                selectedEmployeeTrips = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeTrips"));
            }
        }

        public virtual bool DialogResult { get; set; }

        public virtual string ResultFileName { get; set; }

        public ObservableCollection<TileBarFilters> FilterStatusListOfTiles
        {
            get { return _filterStatusListOfTile; }
            set
            {
                _filterStatusListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStatusListOfTiles"));
            }
        }
        public TileBarFilters SelectedTileBarItems
        {
            get { return _selectedTileBarItem; }
            set
            {
                _selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItems"));
            }
        }

        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set
            {
                isDelVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDelVisible"));
                /*OnPropertyChanged("IsDelVisible");*/
            }
        }

        public string CustomFilterStringName { get; set; }
        public DateTime StartSelectionDate
        {
            get { return startSelectionDate; }
            set
            {
                startSelectionDate = value;

                if (!isFromFilterString)
                    UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("StartSelectionDate"));
            }
        }
        public DateTime FinishSelectionDate
        {
            get { return finishSelectionDate; }
            set
            {
                finishSelectionDate = value;

                if (!isFromFilterString)
                    UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("FinishSelectionDate"));
            }
        }

        public string MyFilterString
        {


            get { return myFilterString; }
            set
            {
                myFilterString = value;
                //if (myFilterString == "")
                //{
                //    SelectedFilter = FilterStatusListOfTiles.FirstOrDefault();
                //}
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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
        public bool IsEdit
        {
            get
            {
                return isEdit;
            }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
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
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        #endregion

        #region Public ICommand

        public ICommand RefreshButtonCommand { get; set; }

        public ICommand PrintButtonCommand { get; set; }

        public ICommand PlantOwnerPopupClosedCommand { get; private set; }

        public ICommand SelectedYearChangedCommand { get; set; }

        public ICommand ExportButtonCommand { get; set; }

        public ICommand AddNewTripCommand { get; set; }//[sudhir.jangra][GEOS2-4816]

        public ICommand EditTripDoubleClickCommand { get; set; }//[Sudhir.jangra][GEOS2-4816]
        public ICommand DeleteTripsRecordCommand { get; set; }  //[rushikesh.gaikwad][GEOS2-5927][03.09.2024]  
        public ICommand CommandFilterTripStatusTileClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Constructor

        public EmployeeTripsViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EmployeeTripsViewModel()...", category: Category.Info, priority: Priority.Low);

                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshEmployeeTripsList));
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                SelectedYearChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SelectedYearChangedCommandAction);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintEmployeeTripsList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportEmployeeTripsList));
                AddNewTripCommand = new RelayCommand(new Action<object>(AddNewTripCommandAction)); //[Sudhir.Jangra][GEOS2-4816]
                EditTripDoubleClickCommand = new RelayCommand(new Action<object>(EditTripDoubleClickCommandAction));//[Sudhir.jangra][GEOS2-4816]
                DeleteTripsRecordCommand = new RelayCommand(new Action<object>(DeleteTripsRecordCommandAction));  //[rushikesh.gaikwad][GEOS2-5927][03.09.2024]
                CommandFilterTripStatusTileClick = new DelegateCommand<object>(CommandFilterTripStatusTileClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandActions);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTripsStatusTileBarClickDoubleClickAction);
                IdUserPermission = SelectIdUserPermission();
                EmployeeTripsList = new ObservableCollection<EmployeeTrips>();
                TripStatusList = new ObservableCollection<WorkflowStatus>(HrmService.HRM_GetAllTripWorkflowStatus()); //[rdixit][30.09.2024][30.09.2024]   //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
                //[rdixit][GEOS2-6979][02.04.2025]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 143))                
                    IsEnableField = true;                
                else
                    isEnableField = false;
                //[rdixit][GEOS2-6979][02.04.2025]
                if (GeosApplication.Instance.IsChangeAndAdminPermissionForHRM 
                    || GeosApplication.Instance.ActiveUser.UserPermissions.Any(i=>i.IdPermission == 105 || i.IdPermission == 143))//[GEOS2-5927][rdixit][21.09.2024]
                    IsDelVisible = true;
                else
                    IsDelVisible = false;

                FillTripStausTilebar();
                AddCustomSetting();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeTripsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeTripsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
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
               
                GeosApplication.Instance.FillFinancialYear();
                MyFilterString = string.Empty;
                FillEmployeeTripsListByPlant();
                if (EmployeeTripsList?.Count > 0)
                {
                    SelectedEmployeeTrips = EmployeeTripsList[0];
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private void RefreshEmployeeTripsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshEmployeeTripsList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

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

                detailView.SearchString = null;
                StartSelectionDate = DateTime.MinValue;
                FinishSelectionDate = DateTime.MinValue;
                MyFilterString = string.Empty;
                FillEmployeeTripsListByPlant();
                FillTripStausTilebar();
                AddCustomSetting();
                AddCustomSettingCount(gridControl);
                if (FilterStatusListOfTiles.Count > 0)
                    SelectedTileBarItems = FilterStatusListOfTiles[0];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshEmployeeTripsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshEmployeeTripsList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshEmployeeTripsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshEmployeeTripsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;
            if (!DXSplashScreen.IsActive)
            {
                // DXSplashScreen.Show<SplashScreenView>(); 
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

            MyFilterString = string.Empty;
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillEmployeeTripsListByPlant();
            }
            else
            {
                EmployeeTripsList = new ObservableCollection<EmployeeTrips>();
            }
            FillTripStausTilebar();
            AddCustomSetting();
            AddCustomSettingCount(gridControl);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillEmployeeTripsListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeTripsListByPlant ...", category: Category.Info, priority: Priority.Low);

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    #region comments
                    //EmployeeTripsList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdCompany_V2440(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //Service GetEmployeeTripsBySelectedIdCompany_V2440 updated with GetEmployeeTripsBySelectedIdCompany_V2480 by [rdixit][09.01.2024][GEOS2-5112]
                    //Service GetEmployeeTripsBySelectedIdCompany_V2480 updated with GetEmployeeTripsBySelectedIdCompany_V2510 by [rdixit][09.01.2024][GEOS2-5112]
                    /*   EmployeeTripsList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdCompany_V2510
                           (plantOwnersIds,
                           HrmCommon.Instance.SelectedPeriod,
                           HrmCommon.Instance.ActiveEmployee.Organization,
                           HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                           IdUserPermission,(UInt32)GeosApplication.Instance.ActiveUser.IdUser));  */
                    #endregion
                    EmployeeTripsList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdCompany_V2560
                                (plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, IdUserPermission, (UInt32)GeosApplication.Instance.ActiveUser.IdUser));
                }
                FillTripStausTilebar();
                AddCustomSetting();

                GeosApplication.Instance.Logger.Log("Method FillEmployeeTripsListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeTripsListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeTripsListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        private void SelectedYearChangedCommandAction(object obj)
        {
            TableView detailView = (TableView)((object[])obj)[0];
            GridControl gridControl = (detailView).Grid;
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

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

            MyFilterString = string.Empty;
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillEmployeeTripsListByPlant();
            }
            else
            {
                EmployeeTripsList = new ObservableCollection<EmployeeTrips>();
            }
            FillTripStausTilebar();
            AddCustomSetting();
            AddCustomSettingCount(gridControl);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }



        private void PrintEmployeeTripsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeTripsList()...", category: Category.Info, priority: Priority.Low);

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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeTripsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeTripsReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintEmployeeTripsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeTripsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportEmployeeTripsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEmployeeTripsList ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Trips";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
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
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = (saveFile.FileName);
                    TableView TripsTableView = ((TableView)obj);
                    TripsTableView.ShowTotalSummary = false;
                    TripsTableView.ShowFixedTotalSummary = false;
                    TripsTableView.ExportToXlsx(ResultFileName);
                   

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    TripsTableView.ShowTotalSummary = false;

                    TripsTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportEmployeeTripsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Get an error in ExportEmployeeTripsList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
        }


        //[Sudhir.jangra][GEOS2-4816]
        private void AddNewTripCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTripCommandAction....", category: Category.Info, priority: Priority.Low);

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

                AddEditTripsView addEditTripsView = new AddEditTripsView();
                AddEditTripsViewModel addEditTripsViewModel = new AddEditTripsViewModel();
                EventHandler handle = delegate { addEditTripsView.Close(); };
                addEditTripsViewModel.RequestClose += handle;
                addEditTripsViewModel.IsNew = true;
                addEditTripsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddTripInformation").ToString();
                addEditTripsViewModel.Init();
                addEditTripsView.DataContext = addEditTripsViewModel;
                addEditTripsView.ShowDialogWindow();

                if (addEditTripsViewModel.IsSaveChanges)
                {
                    FillEmployeeTripsListByPlant();
                    #region [rdixit][25.09.2024][GEOS2-6476] To show records as per origin
                    //EmployeeTrips employeeTrip = new EmployeeTrips();
                    //employeeTrip.IdEmployeeTrip = addEditTripsViewModel.NewEmployeeTrip.IdEmployeeTrip;
                    //employeeTrip.Code = addEditTripsViewModel.NewEmployeeTrip.Code;
                    //employeeTrip.IdEmployee = addEditTripsViewModel.SelectedTraveller.IdEmployee;
                    //employeeTrip.Traveller = addEditTripsViewModel.SelectedTraveller.FullName;
                    //employeeTrip.Title = addEditTripsViewModel.NewEmployeeTrip.Title;
                    //employeeTrip.IdTripType = (UInt32)addEditTripsViewModel.SelectedType.IdLookupValue;
                    //employeeTrip.Type = addEditTripsViewModel.SelectedType.Value;
                    //employeeTrip.IdTripPropose = (UInt32)addEditTripsViewModel.SelectedPropose.IdLookupValue;
                    //employeeTrip.Propose = addEditTripsViewModel.SelectedPropose.Value;
                    //employeeTrip.IdOriginPlant = (UInt32)addEditTripsViewModel.SelectedOrigin.IdCompany;
                    //employeeTrip.Origin = addEditTripsViewModel.SelectedOrigin.ShortName;                  
                    //employeeTrip.Destination = addEditTripsViewModel.SelectedDestination.Name;
                    //employeeTrip.ArrivalDate = addEditTripsViewModel.NewEmployeeTrip.ArrivalDate;
                    //employeeTrip.DepartureDate = addEditTripsViewModel.NewEmployeeTrip.DepartureDate;
                    //employeeTrip.IdStatus = (UInt32)addEditTripsViewModel.SelectedStatus.IdLookupValue;
                    //employeeTrip.Status = addEditTripsViewModel.SelectedStatus.Value;
                    ////[GEOS2-5932][rdixit][21.09.2024]
                    //employeeTrip.Remarks = addEditTripsViewModel.Remarks;
                    //employeeTrip.ArrTransportationType = addEditTripsViewModel.SelectedArrivalTransportationType;
                    //employeeTrip.DeptTransportationType = addEditTripsViewModel.SelectedDepartureTransportationType;
                    //employeeTrip.Requestor = addEditTripsViewModel.SelectedResponsible.FullName;                    
                    //EmployeeTripsList.Add(employeeTrip);
                    //SelectedEmployeeTrips = employeeTrip;
                    #endregion
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewTripCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][09.01.2024][GEOS2-5112]
        public int SelectIdUserPermission()
        {
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 38))
            {
                return 38;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30) && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 40))
            {
                return 40;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30) && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 41))
            {
                return 41;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30) && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26))
            {
                return 26;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30))
            {
                return 30;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return 39;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 40))
            {
                return 40;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 41))
            {
                return 41;
            }
            return 0;
        }
   
     
        //[Sudhir.Jangra][GEOS2-4816]
        public void EditTripDoubleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTripDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                EmployeeTrips EmployeeTrips = (EmployeeTrips)gridControl.SelectedItem;
                AddEditTripsView addEditTripsView = new AddEditTripsView();
                AddEditTripsViewModel addEditTripsViewModel = new AddEditTripsViewModel();
                EventHandler handle = delegate { addEditTripsView.Close(); };
                addEditTripsViewModel.RequestClose += handle;
                addEditTripsView.DataContext = addEditTripsViewModel;
                addEditTripsViewModel.IsNew = false;
                addEditTripsViewModel.SelectedEmployeeTrips = EmployeeTrips;
                addEditTripsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditTripViewHeader").ToString();
                addEditTripsViewModel.EditInit((UInt32)SelectedEmployeeTrips.IdEmployeeTrip);

                var ownerInfo = (obj as FrameworkElement);
                addEditTripsView.Owner = Window.GetWindow(ownerInfo);

                addEditTripsView.ShowDialog();

                if (addEditTripsViewModel.IsSaveChanges)
                {
                    FillEmployeeTripsListByPlant();
                }



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditTripDoubleClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditTripDoubleClickCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rushikesh.gaikwad][GEOS2-5927][28.08.2024]
        public void FillTripStausTilebar()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTripStausTilebar()...", category: Category.Info, priority: Priority.Low);
                FilterStatusListOfTiles = new ObservableCollection<TileBarFilters>();
                ObservableCollection<WorkflowStatus> tempStatusList = new ObservableCollection<WorkflowStatus>();

                int _id = 1;

                if (EmployeeTripsList != null)
                {
                    var DttableList = EmployeeTripsList.AsEnumerable().ToList();
                    List<int> idOfferStatusTypeList = DttableList.Select(x => (int)x.IdStatus).Distinct().ToList();
                    foreach (int item in idOfferStatusTypeList)
                    {
                        WorkflowStatus status = TripStatusList.FirstOrDefault(x => x.IdWorkflowStatus == item);
                        if (status != null)
                            tempStatusList.Add(status);
                    }

                    FilterStatusListOfTiles.Add(new TileBarFilters()
                    {
                        Caption = (System.Windows.Application.Current.FindResource("EmployeeTripsReportTileBarCaption").ToString()),
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCount = EmployeeTripsList.Count,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });

                    foreach (var item in tempStatusList)
                    {
                        FilterStatusListOfTiles.Add(new TileBarFilters()
                        {
                            Caption = item.Name,
                            Id = _id++,
                            IdOfferStatusType = item.IdWorkflowStatus,
                            BackColor = item.HtmlColor,
                            FilterCriteria = "[Status] In ('" + item.Name + "')",
                            ForeColor = item.HtmlColor,
                            EntitiesCount = (DttableList.Where(x => (int)x.IdStatus == item.IdWorkflowStatus).ToList()).Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }

                FilterStatusListOfTiles.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("EmployeeTripsReportCustomFilter").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    FilterCriteria = (System.Windows.Application.Current.FindResource("EmployeeTripsReportCustomFilter").ToString()),
                    Height = 30,
                    width = 200,
                });


                if (FilterStatusListOfTiles.Count > 0)
                    SelectedTileBarItems = FilterStatusListOfTiles[0];

                GeosApplication.Instance.Logger.Log("Method FillTripStausTilebar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTripStausTilebar() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rushikesh.gaikwad][GEOS2-5927][16.09.2024]
        private void DeleteTripsRecordCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteTripsRecordCommandAction....", category: Category.Info, priority: Priority.Low);

                MessageBoxResult messageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteTrip"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = HrmService.IsDeleteTrips(SelectedEmployeeTrips.IdEmployeeTrip);

                    if (IsDeleted)
                    {
                        EmployeeTripsList.Remove(SelectedEmployeeTrips);
                        EmployeeTripsList = new ObservableCollection<EmployeeTrips>(EmployeeTripsList);
                        SelectedEmployeeTrips = EmployeeTripsList.FirstOrDefault();
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TripsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    }
                    
                }
                GeosApplication.Instance.Logger.Log("Method DeleteTripsRecordCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTripsRecordCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTripsRecordCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTripsRecordCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [rushikesh.gaikwad] [GEOS2-5927][05.09.2024]
        private void CommandFilterTripStatusTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandFilterTripStatusTileClickAction....", category: Category.Info, priority: Priority.Low);

                if (FilterStatusListOfTiles.Count > 0)
                {
                    var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                    if (temp.AddedItems.Count > 0)
                    {
                        var selectedFilter = temp.AddedItems[0] as TileBarFilters;
                        if (selectedFilter == null) return;

                        string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                        string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                        string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;


                        if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeTripsReportCustomFilter").ToString()))
                            return;


                        if (str == null)
                        {
                            if (!string.IsNullOrEmpty(_FilterString))
                            {

                                if (!string.IsNullOrEmpty(_FilterString))
                                {
                                    if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                                    {
                                        string st = string.Format("[ArrivalDate] >= #{0}# And [DepartutreDate] <= #{1}# And", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                                        st += _FilterString;
                                        MyFilterString = st;
                                    }
                                    else
                                        MyFilterString = _FilterString;
                                }

                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                                MyFilterString = string.Empty;
                        }
                        else
                        {
                            if (str.Equals("All"))
                            {
                                MyFilterString = string.Empty;
                                EmployeeTripsList = EmployeeTripsList;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                {

                                    if (!string.IsNullOrEmpty(_FilterString))
                                        MyFilterString = _FilterString;
                                    else
                                        MyFilterString = string.Empty;
                                }
                                else
                                    MyFilterString = string.Empty;
                            }
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterEquivalencyWeightGridAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedFilterEquivalencyWeightGridAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        // [rushikesh.gaikwad] [GEOS2-5927][05.09.2024]
        private void CommandTripsStatusTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTripsStatusTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in TripStatusList)
                {
                    if (CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                    }
                }

                if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeTripsReportCustomFilter").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeTripsReportTileBarCaption").ToString()))
                {
                    return;
                }

                DevExpress.Xpf.Grid.TableView table = (DevExpress.Xpf.Grid.TableView)obj;
                GridControl gridControl = (table).Grid;
                gridControl.FilterString = FilterStatusListOfTiles?.FirstOrDefault(i => i.Caption == CustomFilterStringName)?.FilterCriteria;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
                IsEdit = true;

                table.ShowFilterEditor(column);

                GeosApplication.Instance.Logger.Log("Method CommandTripsStatusTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTripsStatusTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[rushikesh.gaikwad][GEOS2-5927][05.09.2024]
        public void UpdateFilterString()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateFilterString ...", category: Category.Info, priority: Priority.Low);

                StringBuilder builder = new StringBuilder();
                foreach (WorkflowStatus item in TripStatusList)
                {
                    builder.Append("'").Append(item.Name).Append("'" + ",");
                }
                string result = builder.ToString();
                result = result.TrimEnd(',');

                if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                {
                    
                    string st = string.Format("[ArrivalDate] >= #{0}# And [DepartutreDate] <= #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                    st += string.Format(" And [Status] In ( " + result + ")");
                    MyFilterString = st;

                }
                else
                {
                    MyFilterString = string.Format("[Status] In ( " + result + ")");
                }


                GeosApplication.Instance.Logger.Log("UpdateFilterString executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateFilterString() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rushikesh.gaikwad][GEOS2-5927][10.09.2024]
        private void FilterEditorCreatedCommandActions(FilterEditorEventArgs obj)
        {
            
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorCreatedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                obj.Handled = true;
                TableView table = (TableView)obj.OriginalSource;
                GridControl gridControl = (table).Grid;
                ShowFilterEditor(obj);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterEditorCreatedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FilterEditorCreatedCommandAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

      
       //[rushikesh.gaikwad][GEOS2-5927][10.09.2024]
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

                customFilterEditorViewModel.Init(e.FilterControl, FilterStatusListOfTiles);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTiles.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        FilterStatusListOfTiles.Remove(tileBarItem);
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
                    TileBarFilters tileBarItem = FilterStatusListOfTiles.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        VisibleRowCount = gridControl.VisibleRowCount;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
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

                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    VisibleRowCount = gridControl.VisibleRowCount;
                    FilterStatusListOfTiles.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKey + customFilterEditorViewModel.FilterName;

                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = FilterStatusListOfTiles.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddCustomSettingCount(GridControl gridControl)
        {
            try
            {
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                foreach (var item in tempUserSettings)
                {
                    try
                    {
                        MyFilterString = FilterStatusListOfTiles.FirstOrDefault(j => j.FilterCriteria == item.Value).FilterCriteria;
                        FilterStatusListOfTiles.FirstOrDefault(j => j.FilterCriteria == item.Value).EntitiesCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSettingCount() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                MyFilterString = string.Empty;
                GeosApplication.Instance.Logger.Log("Method AddCustomSettingCount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSettingCount() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();

                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();

                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        int count = 0;
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                            
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }


                        FilterStatusListOfTiles.Add(
                            new TileBarFilters()
                            {
                                Caption = item.Key.Replace(userSettingsKey, ""),
                                Id = 0,
                                BackColor = null,
                                ForeColor = null,
                                FilterCriteria = item.Value,
                                //  EntitiesCount = count,
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 80,
                                width = 200
                            });

                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(HRM_TripsReportGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(HRM_TripsReportGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(HRM_TripsReportGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                });

                AddCustomSettingCount(gridControl);
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == UI.Helper.TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_TripsReportGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
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

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_TripsReportGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion


    }
}
