using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Hrm;
using System.ComponentModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.Export.Xl;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;
using DevExpress.Data.Filtering;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using System.Text.RegularExpressions;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Globalization;
using System.Net;
using System.Text;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Utility;
using DevExpress.Data;
using DevExpress.Xpf.Editors.RangeControl;


namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeExpenseReportViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return GetService<INavigationService>(); } }
        #endregion // End Services

        #region Public ICommand
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditTravelExpenseCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand AddButtonCommand { get; private set; }
        public ICommand OpenPDFDocumentCommand { get; set; }

        public ICommand SelectedYearChangedCommand { get; private set; }
        //[Rahul.Gadhave][GEOS2-5540][Date:23-05-2024]
        public ICommand CommandFilterStatusTileClick { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand PageLoadedCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }

        public ICommand ViewHideRangeControlCommand { get; set; }//[Sudhir.Jangra][GEOS2-5540]
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

        #endregion // Events

        #region Declaration
        public ObservableCollection<TravelExpenses> finalExpenseList;
        public TravelExpenses selectedGridRow;
        bool isBusy;
        string myFilterString;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        //[Rahul.Gadhave][GEOS2-5539][Date:22-05-2024]
        private bool isFromFilterString = false;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;
        //[Rahul.Gadhave][GEOS2-5540][Date:23-05-2024]
        private TileBarFilters selectedTileBarItem;
        private ObservableCollection<TileBarFilters> filterStatusListOfTile;
        private ObservableCollection<WorkflowStatus> StatusList;
        private string userSettingsKey = "HRM_ExpenseReport_";
        public string FilterStringCriteria { get; set; }
        public string FilterStringName { get; set; }
        ObservableCollection<WorkflowStatus> geosStatusList;
        private List<GridColumn> GridColumnList;
        private bool isEdit;
        private int visibleRowCount;
        private TileBarFilters selectedFilter;
        public string HRM_ExpenseReportGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ExpenseReportGridSetting.Xml";
        private bool isViewRangeControl;//[Sudhir.jangra][GEOS2-5540]
        private string gridRowHeight;//[Sudhir.jangra][GEOS2-5540]
        #endregion

        #region Properties

        public string MyFilterString
        {


            get { return myFilterString; }
            set
            {
                myFilterString = value;
                if (myFilterString == "")
                {
                    SelectedFilter = FilterStatusListOfTile.FirstOrDefault();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public ObservableCollection<TravelExpenses> FinalExpenseList
        {
            get { return finalExpenseList; }
            set
            {
                finalExpenseList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalExpenseList"));
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public TravelExpenses SelectedGridRow
        {
            get { return selectedGridRow; }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

            }
        }

        public object GlobalProperties { get; private set; }

        public List<Country> CountryList { get; set; }

        //[Rahul.Gadhave][GEOS2-5539][Date:22-05-2024]
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
        //[Rahul.Gadhave][GEOS2-5539][Date:22-05-2024]
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
        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
            }
        }
        public ObservableCollection<TileBarFilters> FilterStatusListOfTile
        {
            get { return filterStatusListOfTile; }
            set
            {
                filterStatusListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStatusListOfTile"));
            }
        }
        public ObservableCollection<WorkflowStatus> GeosStatusList
        {
            get { return geosStatusList; }
            set { geosStatusList = value; }
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
        public string CustomFilterStringName { get; set; }
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
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

        //[Sudhir.Jangra][GEOS2-5540]
        public bool IsViewRangeControl
        {
            get { return isViewRangeControl; }
            set
            {
                isViewRangeControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewRangeControl"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5540]
        public string GridRowHeight
        {
            get { return gridRowHeight; }
            set
            {
                gridRowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeight"));
            }
        }
        bool isAddButtonVisible = false;
        public bool IsAddButtonVisible 
        {
            get
            {
                return isAddButtonVisible;
            }
            set
            {
                isAddButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddButtonVisible"));
            }
        }
        #endregion

        #region Constructor
        public EmployeeExpenseReportViewModel()
        {
            PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
            RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
            CustomShowFilterPopupCommand = new DelegateCommand<DevExpress.Xpf.Grid.FilterPopupEventArgs>(CustomShowFilterPopupAction);
            EditTravelExpenseCommand = new RelayCommand(new Action<object>(EditTravelExpenseCommandAction));
            AddButtonCommand = new RelayCommand(new Action<object>(AddButtonCommandAction));//[rdixit][GEOS2-4025][24.11.2022]
            OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));
            SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
            //PageLoadedCommand = new RelayCommand(new Action<object>(OnViewLoaded));
            //[Rahul.Gadhave][GEOS2-5540][Date:24-05-2024]
            CommandFilterStatusTileClick = new DelegateCommand<object>(CommandFilterStatusTileClickAction);
            FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
            CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
            StatusList = new ObservableCollection<WorkflowStatus>(HrmService.GetWorkFlowStatus_V2520().AsEnumerable());//[Sudhir.Jangra][GEOs2-5540]
            TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);

            ViewHideRangeControlCommand = new DelegateCommand<object>(ViewHideRangeControl);//[Sudhir.Jangra][GEOS2-5540]

            //[003]added
            //FillCountryList();
            FillFilterTileBar();
           
            AddCustomSetting();
            IsViewRangeControl = true;
            GridRowHeight = "100";

        }
        #endregion

        #region Method

        /// <summary>
        ///  Method for Intialize....
        ///  [001][rdixit][GEOS2-3827][10.08.2022]
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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
                MyFilterString = string.Empty;
                if(GeosApplication.Instance.IsHRMManageEmployeeContactsPermission ||
                   GeosApplication.Instance.IsTravel_AssistantPermissionForHRM ||
                   GeosApplication.Instance.IsHRMTravelManagerPermission)
                {
                    IsAddButtonVisible =true;
                }
                else
                {
                    IsAddButtonVisible = false;
                }
                FillExpenseGrid();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
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
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Fill Grid with Travel Expense Reports
        /// [001][rdixit][GEOS2-3827][10.08.2022]
        /// [002][cpatil][GEOS2-5538][21-05-2024]
        /// </summary>
        private void FillExpenseGrid()
        {
            try
            {
                //[GEOS2-3943][rdixit][04.01.2022] Added User Permission Validation               
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 40 || up.IdPermission == 41) && up.Permission.IdGeosModule == 7))
                {
                    List<UserPermission> Permissions = GeosApplication.Instance.ActiveUser.UserPermissions.Where(i => i.Permission.IdGeosModule == 7).ToList();
                    UserPermission userwithPermission = Permissions.Where(up => (up.IdPermission == 40 || up.IdPermission == 41)).FirstOrDefault();
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                    #region Commented
                    //GetAllTravelExpensewithPermission_V2360 Method updated with GetAllTravelExpensewithPermission_V2420 [rdixit][GEOS2-4301][04.08.2023]
                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(
                    //    HrmService.GetAllTravelExpensewithPermission_V2420(
                    //        plantOwnersIds, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.ActiveEmployee.Organization,
                    //        userwithPermission.IdPermission));
                    //Shubham[skadam] GEOS2-5139 Add country column with flag in expense reports 18 12 2023
                    // [002] [cpatil][GEOS2-5538][21-05-2024]
                    #endregion
                    FinalExpenseList = new ObservableCollection<TravelExpenses>(
                       HrmService.GetAllTravelExpensewithPermission_V2520(
                           plantOwnersIds, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.ActiveEmployee.Organization,
                           userwithPermission.IdPermission, HrmCommon.Instance.SelectedPeriod));
                }
                else if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 39) && up.Permission.IdGeosModule == 7)))
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                    #region Commented
                    //GetTravelExpenses Method Updated with GetTravelExpenses_V2320 [rdixit][GEOS2-3829][26.09.2022]
                    //GetTravelExpenses_V2320 Method Updated with GetTravelExpenses_V2340 [rdixit][GEOS2-4022][24.11.2022]
                    //GetTravelExpenses_V2340 Method Updated with GetTravelExpenses_V2360 [rdixit][GEOS2-4066][03.01.2023]
                    //GetTravelExpenses_V2360 Method Updated with GetTravelExpenses_V2410 [rdixit][GEOS2-4472][05.07.2023]
                    //GetTravelExpenses_V2410 Method updated with GetTravelExpenses_V2420 [rdixit][GEOS2-4301][04.08.2023]
                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2420(plantOwnersIds));
                    #region start of  GEOS2-4848
                    //[pramod.misal][GEOS2-4848][28-11-2023]
                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2460(plantOwnersIds));
                    //Shubham[skadam] GEOS2-5139 Add country column with flag in expense reports 18 12 2023
                    #endregion

                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2470(plantOwnersIds));
                    #region GEOS2-5286
                    //[pramod.misal][GEOS2-5286][05-02-2024]
                    //[cpatil][GEOS2-5538][21-05-2024]
                    #endregion GEOS2-5286
                    #endregion GEOS2-4848
                    FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2520(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));

                    if (FinalExpenseList?.Count > 0)
                    {
                        FinalExpenseList.ToList().ForEach(i =>
                        {
                            i.GivenAmount = Convert.ToDouble(i.GivenAmount.ToString("n2", CultureInfo.CurrentCulture));
                            i.TotalAmount = Convert.ToDouble(i.TotalAmount.ToString("n2", CultureInfo.CurrentCulture));
                        });
                    }
                    if (FinalExpenseList.Count > 0)
                    {

                        foreach (var item in FinalExpenseList)
                        {
                            if (item.IdLinkedTrip != null && item.LinkedFromDate.HasValue)
                            {
                                //string formattedStartDate = item.LinkedFromDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);
                                //string formattedEndDate = item.LinkedToDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);

                                string formattedEndDate = item.LinkedFromDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);
                                string formattedStartDate = item.LinkedToDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);

                                if (formattedStartDate != null && formattedEndDate != null && item.LinkedName != null)
                                {
                                    item.LinkedTripTitle = $"[{formattedStartDate} - {formattedEndDate}] – {item.LinkedName}";
                                }

                            }


                        }
                    }
                }
                else
                {
                    FinalExpenseList = new ObservableCollection<TravelExpenses>();
                }
                if (FinalExpenseList.Count > 0)
                    SelectedGridRow = FinalExpenseList.FirstOrDefault();

                FillFilterTileBar();
                AddCustomSetting();
                IsBusy = false;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExpenseGrid() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExpenseGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseGrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        //[GEOS2-4472][rdixit][05.07.2023]
        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);
                //HrmService = new HrmServiceController("localhost:6699");
                //Dictionary<string, byte[]> ExpenseFile = HrmService.GetEmployeeExpenseReport(SelectedGridRow.Company, SelectedGridRow.ReporterCode, SelectedGridRow.ExpenseCode);
                //Shubham[skadam] GEOS2-7731 Need to improvement in Issue with Report Display in GEOS  18 04 2025
                Dictionary<string, byte[]> ExpenseFile = HrmService.GetEmployeeExpenseReport_V2630(SelectedGridRow.Company, SelectedGridRow.ReporterCode, SelectedGridRow.ExpenseCode);
                if (ExpenseFile?.Count > 0)
                {
                    if (ExpenseFile.FirstOrDefault().Value != null)
                    {
                        // Open PDF in another window
                        EmployeeDocumentView documentView = new EmployeeDocumentView();
                        EmployeeDocumentViewModel documentViewModel = new EmployeeDocumentViewModel();

                        documentViewModel.OpenPdfFromBytes(ExpenseFile.FirstOrDefault().Value, ExpenseFile.FirstOrDefault().Key);
                        documentView.DataContext = documentViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        documentView.Show();
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ExpenseReportFileNotFound").ToString(), ExpenseFile.FirstOrDefault().Key), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to Fill Country List
        ///  </summary>
        private void FillCountryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountryList()...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempCountryList = HrmService.GetAllCountries();
                CountryList = new List<Country>();
                CountryList.AddRange(tempCountryList);
                foreach (var item in CountryList)
                {
                    try
                    {
                        //itememp.ProfileImageInBytes = GetEmployeeImage(itememp.EmployeeCode, employeesProfileImagePath);
                        //using (System.Net.WebClient webClient = new System.Net.WebClient())
                        //{
                        //    item.CountryIconBytes = webClient.DownloadData("https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + item.Iso + ".png");
                        //}
                        item.CountryIconBytes = Utility.ImageUtil.GetImageByWebClient("https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + item.Iso + ".png");
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillCountryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillFilterTileBar()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFilterTileBar()...", category: Category.Info, priority: Priority.Low);
                FilterStatusListOfTile = new ObservableCollection<TileBarFilters>();
                ObservableCollection<WorkflowStatus> tempStatusList = new ObservableCollection<WorkflowStatus>();
                int _id = 1;

                if (FinalExpenseList != null)
                {
                    var DttableList = FinalExpenseList.AsEnumerable().ToList();
                    List<int> idOfferStatusTypeList = DttableList.Select(x => (int)x.IdWorkflowStatus).Distinct().ToList();
                    foreach (int item in idOfferStatusTypeList)
                    {
                        WorkflowStatus status = StatusList.FirstOrDefault(x => x.IdWorkflowStatus == item);
                        if (status != null)
                            tempStatusList.Add(status);
                    }

                    FilterStatusListOfTile.Add(new TileBarFilters()
                    {
                        Caption = (System.Windows.Application.Current.FindResource("EmployeeExpenseReportTileBarCaption").ToString()),
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCount = FinalExpenseList.Count,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });

                    foreach (var item in tempStatusList.OrderBy(x=>x.SortOrder))
                    {
                        FilterStatusListOfTile.Add(new TileBarFilters()
                        {
                            Caption = item.Name,
                            Id = _id++,
                            IdOfferStatusType = item.IdWorkflowStatus,
                            BackColor = item.HtmlColor,
                            FilterCriteria = "[Status] In ('" + item.Name + "')",
                            ForeColor = item.HtmlColor,
                            EntitiesCount = (DttableList.Where(x => (int)x.IdWorkflowStatus == item.IdWorkflowStatus).ToList()).Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }

                FilterStatusListOfTile.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("EmployeeExpenseReportCustomFilter").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    FilterCriteria = (System.Windows.Application.Current.FindResource("EmployeeExpenseReportCustomFilter").ToString()),
                    Height = 30,
                    width = 200,
                });


                if (FilterStatusListOfTile.Count > 0)
                    SelectedTileBarItem = FilterStatusListOfTile[0];

                GeosApplication.Instance.Logger.Log("Method FillFilterTileBar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillFilterTileBar() " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                            //count = FinalExpenseList.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).ToList().Count;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }


                        FilterStatusListOfTile.Add(
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
        #endregion

        #region Command Action
        /// <summary>
        /// [cpatil][GEOS2-5538][21-05-2024]
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
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

                //[GEOS2-3943][rdixit][04.01.2022] Added User Permission Validation
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 39) && up.Permission.IdGeosModule == 7)))
                {
                    try
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                        #region Comments
                        //GetTravelExpenses Method Updated with GetTravelExpenses_V2320  [rdixit][GEOS2-3829][26.09.2022]
                        //GetTravelExpenses_V2320 Method Updated with GetTravelExpenses_V2340 [rdixit][GEOS2-4022][24.11.2022]
                        //GetTravelExpenses_V2340 Method Updated with GetTravelExpenses_V2360 [rdixit][GEOS2-4066][03.01.2023]  
                        //GetTravelExpenses_V2360 Method Updated with GetTravelExpenses_V2410 [rdixit][GEOS2-4472][05.07.2023] 
                        //GetTravelExpenses_V2410 Method updated with GetTravelExpenses_V2420 [rdixit][GEOS2-4301][04.08.2023]      
                        #endregion

                        //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2420(plantOwnersIds));
                        #region start of  GEOS2-4848
                        //[pramod.misal][GEOS2-4848][28-11-2023]
                        //[cpatil][GEOS2-5538][21-05-2024]
                        FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2520(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));

                        if (FinalExpenseList.Count > 0)
                        {

                            foreach (var item in FinalExpenseList)
                            {
                                if (item.IdLinkedTrip != null)
                                {
                                    string formattedStartDate = item.LinkedFromDate?.ToString(CultureInfo.CurrentCulture);
                                    string formattedEndDate = item.LinkedToDate?.ToString(CultureInfo.CurrentCulture);

                                    item.LinkedTripTitle = $"[{formattedStartDate} - {formattedEndDate}] – {item.LinkedName}";
                                }


                            }
                        }

                        #endregion GEOS2-4848   
                        FillFilterTileBar();
                        AddCustomSetting();
                        AddCustomSettingCount(gridControl);

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in PlantOwnerPopupClosedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in PlantOwnerPopupClosedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    }
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 40 || up.IdPermission == 41) && up.Permission.IdGeosModule == 7))
                {
                    List<UserPermission> Permissions = GeosApplication.Instance.ActiveUser.UserPermissions.Where(i => i.Permission.IdGeosModule == 7).ToList();
                    UserPermission userwithPermission = Permissions.Where(up => (up.IdPermission == 40 || up.IdPermission == 41)).FirstOrDefault();
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    //GetAllTravelExpensewithPermission_V2360 Method updated with GetAllTravelExpensewithPermission_V2420 [rdixit][GEOS2-4301][04.08.2023]
                    //[cpatil][GEOS2-5538][21-05-2024]
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                    FinalExpenseList = new ObservableCollection<TravelExpenses>(
                        HrmService.GetAllTravelExpensewithPermission_V2520(
                        plantOwnersIds,
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                        HrmCommon.Instance.ActiveEmployee.Organization,
                        userwithPermission.IdPermission, Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)));
                }
                else
                {
                    FinalExpenseList = new ObservableCollection<TravelExpenses>();
                }

                RangeControl rang = (RangeControl)((object[])obj)[1];
                rang.VisibleRangeStart = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), 01, 01);
                rang.RangeStart = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), 01, 01);
                rang.RangeEnd = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), 12, 31);
                rang.VisibleRangeEnd = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), 12, 31);
                rang.UpdateLayout();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method To show Travel Expense Reports for selected Plants
        ///[001][rdixit][GEOS2-3827][10.08.2022]
        ///[002][cpatil][GEOS2-5538][21-05-2024]
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;
            //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            //{
            //    return;
            //}
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

            //[GEOS2-3943][rdixit][04.01.2022] Added User Permission Validation
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 39) && up.Permission.IdGeosModule == 7)))
            {
                try
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                    #region Comments
                    //GetTravelExpenses Method Updated with GetTravelExpenses_V2320  [rdixit][GEOS2-3829][26.09.2022]
                    //GetTravelExpenses_V2320 Method Updated with GetTravelExpenses_V2340 [rdixit][GEOS2-4022][24.11.2022]
                    //GetTravelExpenses_V2340 Method Updated with GetTravelExpenses_V2360 [rdixit][GEOS2-4066][03.01.2023]  
                    //GetTravelExpenses_V2360 Method Updated with GetTravelExpenses_V2410 [rdixit][GEOS2-4472][05.07.2023] 
                    //GetTravelExpenses_V2410 Method updated with GetTravelExpenses_V2420 [rdixit][GEOS2-4301][04.08.2023]      
                    #endregion

                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2420(plantOwnersIds));
                    #region start of  GEOS2-4848
                    //[pramod.misal][GEOS2-4848][28-11-2023]
                    //[cpatil][GEOS2-5538][21-05-2024]
                    FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2520(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));

                    if (FinalExpenseList.Count > 0)
                    {

                        foreach (var item in FinalExpenseList)
                        {
                            if (item.IdLinkedTrip != null)
                            {
                                string formattedStartDate = item.LinkedFromDate?.ToString(CultureInfo.CurrentCulture);
                                string formattedEndDate = item.LinkedToDate?.ToString(CultureInfo.CurrentCulture);

                                item.LinkedTripTitle = $"[{formattedStartDate} - {formattedEndDate}] – {item.LinkedName}";
                            }


                        }
                    }

                    #endregion GEOS2-4848   

                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in PlantOwnerPopupClosedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in PlantOwnerPopupClosedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 40 || up.IdPermission == 41) && up.Permission.IdGeosModule == 7))
            {
                List<UserPermission> Permissions = GeosApplication.Instance.ActiveUser.UserPermissions.Where(i => i.Permission.IdGeosModule == 7).ToList();
                UserPermission userwithPermission = Permissions.Where(up => (up.IdPermission == 40 || up.IdPermission == 41)).FirstOrDefault();
                List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //GetAllTravelExpensewithPermission_V2360 Method updated with GetAllTravelExpensewithPermission_V2420 [rdixit][GEOS2-4301][04.08.2023]
                //[cpatil][GEOS2-5538][21-05-2024]
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                FinalExpenseList = new ObservableCollection<TravelExpenses>(
                    HrmService.GetAllTravelExpensewithPermission_V2520(
                    plantOwnersIds,
                    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                    HrmCommon.Instance.ActiveEmployee.Organization,
                    userwithPermission.IdPermission, HrmCommon.Instance.SelectedPeriod));
            }
            else
            {
                FinalExpenseList = new ObservableCollection<TravelExpenses>();
            }
            FillFilterTileBar();
            AddCustomSetting();
            AddCustomSettingCount(gridControl);

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method To Refresh Travel Expense Reports
        /// [001][rdixit][GEOS2-3827][10.08.2022]
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("EmployeeExpenseReportViewModel Method RefreshButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
            try
            {
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                StartSelectionDate = DateTime.MinValue;
                FinishSelectionDate = DateTime.MinValue;
                MyFilterString = string.Empty;
                FillExpenseGrid();

                //[Rahul.Gadhave][GEOS2-5540][Date:24-05-2024]
                FillFilterTileBar();
                AddCustomSetting();
                AddCustomSettingCount(gridControl);

                if (FilterStatusListOfTile.Count > 0)
                    SelectedTileBarItem = FilterStatusListOfTile[0];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeExpenseReportViewModel RefreshButtonCommandAction method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("EmployeeExpenseReportViewModel RefreshButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method To print Travel Expense Reports
        /// [001][rdixit][GEOS2-3827][10.08.2022]
        /// </summary>
        /// <param name="obj"></param>
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TravelReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TravelReportPrintFooterTemplate"];
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method To Export Travel Expense Reports
        /// [001][rdixit][GEOS2-3827][10.08.2022]
        /// </summary>
        /// <param name="obj"></param>
        public void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EmployeeExpenseReportViewModel Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Expenses Report";
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
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    activityTableView.ExportToXlsx(ResultFileName, options);


                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("EmployeeExpenseReportViewModel Method ExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeExpenseReportViewModel Method ExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methods to apply filter to the currency and balance Manually
        /// [001][rdixit][GEOS2-3827][10.08.2022]
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopupAction(DevExpress.Xpf.Grid.FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                MyFilterString = string.Empty;
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName != "Currency" && e.Column.FieldName != "TotalBalance" && e.Column.FieldName != "Country")
                {
                    return;
                }
                #region Currency
                if (e.Column.FieldName == "Currency")
                {
                    foreach (var dataObject in FinalExpenseList)
                    {
                        if (dataObject.ExpenseCurrency == null)
                        {
                            continue;
                        }
                        else if (dataObject.ExpenseCurrency != null)
                        {

                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == dataObject.ExpenseCurrency))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = dataObject.ExpenseCurrency;
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("ExpenseCurrency Like '%{0}%'", dataObject.ExpenseCurrency));
                                filterItems.Add(customComboBoxItem);
                            }
                            else
                                continue;
                        }
                        else
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalExpenseList.Where(y => y.ExpenseCurrency == dataObject.ExpenseCurrency).Select(slt => slt.ExpenseCurrency).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = FinalExpenseList.Where(y => y.ExpenseCurrency == dataObject.ExpenseCurrency).Select(slt => slt.ExpenseCurrency).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("ExpenseCurrency Like '%{0}%'", FinalExpenseList.Where(y => y.ExpenseCurrency == dataObject.ExpenseCurrency).Select(slt => slt.ExpenseCurrency).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }
                #endregion

                #region Balance
                if (e.Column.FieldName == "TotalBalance")
                {
                    foreach (var dataObject in FinalExpenseList)
                    {
                        if (dataObject.Balance == 0)
                        {
                            continue;
                        }
                        else if (dataObject.Balance != 0)
                        {

                            if (!filterItems.Any(x => Convert.ToDouble(((CustomComboBoxItem)x).DisplayValue) == dataObject.Balance))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = dataObject.Balance;
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Balance Like '%{0}%'", dataObject.Balance));
                                filterItems.Add(customComboBoxItem);
                            }
                            else
                                continue;
                        }
                        else
                        {
                            if (!filterItems.Any(x => Convert.ToDouble(((CustomComboBoxItem)x).DisplayValue) == FinalExpenseList.Where(y => y.Balance == dataObject.Balance).Select(slt => slt.Balance).FirstOrDefault())) ;
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = FinalExpenseList.Where(y => y.Balance == dataObject.Balance).Select(slt => slt.Balance).FirstOrDefault();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Balance Like '%{0}%'", FinalExpenseList.Where(y => y.Balance == dataObject.Balance).Select(slt => slt.Balance)));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }
                #endregion
                #region Country
                //Shubham[skadam] GEOS2-5139 Add country column with flag in expense reports 21 12 2023
                if (e.Column.FieldName == "Country")
                {
                    foreach (var dataObject in FinalExpenseList)
                    {
                        if (dataObject.CountryName == null)
                        {
                            continue;
                        }
                        else if (dataObject.CountryName != null)
                        {

                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == dataObject.CountryName))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = dataObject.CountryName;
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CountryName Like '%{0}%'", dataObject.CountryName));
                                filterItems.Add(customComboBoxItem);
                            }
                            else
                                continue;
                        }
                        else
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalExpenseList.Where(y => y.CountryName == dataObject.CountryName).Select(slt => slt.CountryName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = FinalExpenseList.Where(y => y.CountryName == dataObject.CountryName).Select(slt => slt.CountryName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CountryName Like '%{0}%'", FinalExpenseList.Where(y => y.CountryName == dataObject.CountryName).Select(slt => slt.CountryName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }
                #endregion
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methods to Edit Selected Travel Expense Report
        /// [001][rdixit][GEOS2-3827][10.08.2022]
        /// </summary>
        /// <param name="obj"></param>
        private void EditTravelExpenseCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTravelExpenseCommandAction....", category: Category.Info, priority: Priority.Low);

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

                TableView detailView = (TableView)obj;
                var travelexpense = (TravelExpenses)detailView.DataControl.CurrentItem;
                long id = travelexpense.IdEmployeeExpenseReport;
                TravelExpenses FoundRow = FinalExpenseList.Where(mol => mol.IdEmployeeExpenseReport == id).FirstOrDefault();
                EditTravelExpenseViewModel EditTravelExpenseViewModel = new EditTravelExpenseViewModel();
                EditTravelExpenseView EditTravelExpenseView = new EditTravelExpenseView();
                EventHandler handle1 = delegate { EditTravelExpenseView.Close(); };
                EditTravelExpenseViewModel.RequestClose += handle1;
                EditTravelExpenseViewModel.IsNew = false;
                EditTravelExpenseViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditTravelExpenseTitle").ToString();
                EditTravelExpenseViewModel.Init(FoundRow);//[pramod.misal][GEOS2-4848][22.11.2023]
                EditTravelExpenseView.DataContext = EditTravelExpenseViewModel;
                var ownerInfo = (obj as FrameworkElement);
                EditTravelExpenseView.Owner = Window.GetWindow(ownerInfo);
                EditTravelExpenseView.ShowDialogWindow();
                if (!EditTravelExpenseViewModel.IsUpdated)
                {
                    detailView.DataControl.CurrentItem = FoundRow; 
                }
                else
                {
                    FillExpenseGrid();
                    RefreshButtonCommandAction(obj);
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EditTravelExpenseCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methods to Add new Expense Report
        /// [001][rdixit][GEOS2-4025][24.11.2022]
        /// </summary>
        /// <param name="obj"></param>
        private void AddButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddButtonCommandAction....", category: Category.Info, priority: Priority.Low);

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

                TableView detailView = (TableView)obj;
                EditTravelExpenseViewModel EditTravelExpenseViewModel = new EditTravelExpenseViewModel();
                EditTravelExpenseView EditTravelExpenseView = new EditTravelExpenseView();
                EventHandler handle1 = delegate { EditTravelExpenseView.Close(); };
                EditTravelExpenseViewModel.RequestClose += handle1;
                EditTravelExpenseViewModel.IsNew = true;
                EditTravelExpenseViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddTravelExpenseTitle").ToString();
                EditTravelExpenseViewModel.AddInit();
                EditTravelExpenseView.DataContext = EditTravelExpenseViewModel;
                EditTravelExpenseView.ShowDialogWindow();

                if (EditTravelExpenseViewModel.IsSaved)
                    FillExpenseGrid();
                RefreshButtonCommandAction(obj);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommandFilterStatusTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterEquivalencyWeightGridAction....", category: Category.Info, priority: Priority.Low);

                if (FilterStatusListOfTile.Count > 0)
                {
                    var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                    if (temp.AddedItems.Count > 0)
                    {
                        string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                        string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                        string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;


                        if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeExpenseReportCustomFilter").ToString()))
                            return;


                        if (str == null)
                        {
                            if (!string.IsNullOrEmpty(_FilterString))
                            {

                                if (!string.IsNullOrEmpty(_FilterString))
                                {
                                    if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                                    {
                                        string st = string.Format("[StartDate] >= #{0}# And [EndDate] <= #{1}# And", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
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
                                FinalExpenseList = FinalExpenseList;
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
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method CommandFilterStatusTileClickAction()...", category: Category.Info, priority: Priority.Low);
            //    object[] test = ((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems);
            //    if (test?.Count() > 0)
            //    {
            //        long statusIdType = ((TileBarFilters)test[0]).IdOfferStatusType;
            //        MyFilterString = ((TileBarFilters)test[0]).FilterCriteria;
            //        FilterStringName = ((TileBarFilters)test[0]).Caption;
            //        if (statusIdType == 0)
            //        {
            //            MyFilterString = string.Empty;
            //        }


            //            //if (statusIdType != 0)
            //            //{
            //            //    StringBuilder builder = new StringBuilder();
            //            //    foreach (WorkflowStatus item in StatusList)
            //            //    {
            //            //        if (item.IdWorkflowStatus == statusIdType)
            //            //            builder.Append("'").Append(item.Name).Append("'" + ",");
            //            //    }

            //            //    string result = builder.ToString();
            //            //    result = result.TrimEnd(',');

            //            //    if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
            //            //    {
            //            //        CultureInfo culture = CultureInfo.CurrentCulture;
            //            //        string st = string.Format("[StartDate] >= '{0}' And [EndDate] <= '{1}'", StartSelectionDate.ToString(culture.DateTimeFormat.ShortDatePattern, culture), FinishSelectionDate.ToString(culture.DateTimeFormat.ShortDatePattern, culture));
            //            //        st += string.Format(" And [Status] In ( " + result + ")");
            //            //        MyFilterString = st;
            //            //    }
            //            //    //else
            //            //    //{
            //            //    //    MyFilterString = string.Format("[Status] In ( " + result + ")");
            //            //    //}
            //            //}
            //            //else if (FilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeExpenseReportCustomFilter").ToString()))
            //            //{
            //            //    return;
            //            //}
            //            //else if (FilterString != null && !string.IsNullOrEmpty(FilterString))
            //            //{
            //            //    MyFilterString = FilterString;
            //            //    FilterStringCriteria = FilterString;
            //            //}
            //            //else
            //            //{
            //            //    StringBuilder builder = new StringBuilder();
            //            //    foreach (WorkflowStatus item in StatusList)
            //            //    {
            //            //        builder.Append("'").Append(item.Name).Append("'" + ",");
            //            //    }

            //            //    string result = builder.ToString();
            //            //    result = result.TrimEnd(',');

            //            //    if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
            //            //    {
            //            //        CultureInfo culture = CultureInfo.CurrentCulture;
            //            //        string st = string.Format("[StartDate] >= '{0}' And [EndDate] <= '{1}'", StartSelectionDate.ToString(culture.DateTimeFormat.ShortDatePattern, culture), FinishSelectionDate.ToString(culture.DateTimeFormat.ShortDatePattern, culture));
            //            //        st += string.Format(" And [Status] In ( " + result + ")");
            //            //        MyFilterString = st;
            //            //    }
            //            //    //else
            //            //    //{
            //            //    //    MyFilterString = string.Format("[Status] In ( " + result + ")");
            //            //    //}
            //            //}
            //            GeosApplication.Instance.Logger.Log("Method CommandFilterStatusTileClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in Method CommandFilterStatusTileClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in StatusList)
                {
                    if (CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                    }
                }

                if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeExpenseReportCustomFilter").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("EmployeeExpenseReportTileBarCaption").ToString()))
                {
                    return;
                }

                DevExpress.Xpf.Grid.TableView table = (DevExpress.Xpf.Grid.TableView)obj;
                //TableViewEx table = (TableViewEx)obj;

                GridControl gridControl = (table).Grid;
                gridControl.FilterString = FilterStatusListOfTile?.FirstOrDefault(i => i.Caption == CustomFilterStringName)?.FilterCriteria;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
                IsEdit = true;

                table.ShowFilterEditor(column);

                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        //[Rahul.Gadhave][GEOS2-5539][Date:22-05-2024]
        public void UpdateFilterString()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateFilterString ...", category: Category.Info, priority: Priority.Low);

                StringBuilder builder = new StringBuilder();
                foreach (WorkflowStatus item in StatusList)
                {
                    builder.Append("'").Append(item.Name).Append("'" + ",");
                }
                string result = builder.ToString();
                result = result.TrimEnd(',');

                if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                {
                    //CultureInfo culture = CultureInfo.CurrentCulture;
                    //string st = string.Format("[StartDate] >= '{0}' And [EndDate] <= '{1}'", StartSelectionDate.ToString(culture.DateTimeFormat.ShortDatePattern, culture), FinishSelectionDate.ToString(culture.DateTimeFormat.ShortDatePattern, culture));
                    //MyFilterString = st;
                    string st = string.Format("[StartDate] >= #{0}# And [EndDate] <= #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
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
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            //obj.Handled = true;
            //UI.Helper.TableViewEx table = (UI.Helper.TableViewEx)obj.OriginalSource;
            //GridControl gridControl = (table).Grid;
            //GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
            //GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
            //  ShowFilterEditor(obj);
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

                customFilterEditorViewModel.Init(e.FilterControl, FilterStatusListOfTile);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        FilterStatusListOfTile.Remove(tileBarItem);
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
                    TileBarFilters tileBarItem = FilterStatusListOfTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        VisibleRowCount = gridControl.VisibleRowCount;
                        //List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                        //VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
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
                    //List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.tot);
                    //GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    //List<DevExpress.Xpf.Grid.GridSummaryItem> summuries = new List<DevExpress.Xpf.Grid.GridSummaryItem>(gridControl.TotalSummary);

                    VisibleRowCount = gridControl.VisibleRowCount;

                    FilterStatusListOfTile.Add(new TileBarFilters()
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
                    SelectedFilter = FilterStatusListOfTile.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
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
                        MyFilterString = FilterStatusListOfTile.FirstOrDefault(j => j.FilterCriteria == item.Value).FilterCriteria;
                        FilterStatusListOfTile.FirstOrDefault(j => j.FilterCriteria == item.Value).EntitiesCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
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

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(HRM_ExpenseReportGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(HRM_ExpenseReportGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(HRM_ExpenseReportGridSettingFilePath);

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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_ExpenseReportGridSettingFilePath);
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_ExpenseReportGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-5540]
        private void ViewHideRangeControl(object obj)
        {
            if (IsViewRangeControl)
            {
                GridRowHeight = "0";
                IsViewRangeControl = false;
            }
            else
            {
                GridRowHeight = "100";
                IsViewRangeControl = true;
            }
        }

       

    }
}
