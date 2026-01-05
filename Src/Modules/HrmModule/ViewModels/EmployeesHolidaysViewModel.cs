using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Sprint 41-[HRM-M041-07] New configuration section Holidays---sdesai
/// [002][20180723][skhade][HRM-M043-16] Holidays must be filtered by year
/// </summary>
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeesHolidaysViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //[M051-08][Year selection is not saved after change section][adadibathina]
        #endregion


        #region Service

        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion

        #region Declaration
        private ObservableCollection<CompanyHoliday> companyHolidays;
        private CompanyHoliday selectedHoliday;
        //[M051-08]
        //  private long selectedPeriod;
        IWorkbenchStartUp objWorkbenchStartUp;
        private string timeEditMask;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private bool isDeleted;
        private List<CompanyHoliday> companyHolidayslist;
        private DataTable dataTableForGridLayout;
        #endregion

        #region Properties
        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public ObservableCollection<CompanyHoliday> CompanyHolidays
        {
            get
            {
                return companyHolidays;
            }

            set
            {
                companyHolidays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyHolidays"));
            }
        }
        public CompanyHoliday SelectedHoliday
        {
            get
            {
                return selectedHoliday;
            }

            set
            {
                selectedHoliday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedHoliday"));
            }
        }
        //[M051-08]
        //public long SelectedPeriod
        //{
        //    get { return selectedPeriod; }
        //    set
        //    {
        //        selectedPeriod = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
        //    }
        //}
        public string TimeEditMask
        {
            get
            {
                return timeEditMask;
            }

            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
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
        public List<CompanyHoliday> CompanyHolidaysList
        {
            get
            {
                return companyHolidayslist;
            }

            set
            {
                companyHolidayslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyHolidaysList"));
            }
        }


        public DataTable DataTableForGridLayout
        {
            get
            {
                return dataTableForGridLayout;
            }
            set
            {
                dataTableForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayout"));
            }
        }

        #endregion

        #region Public ICommand

        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand AddNewHolidayCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand EditHolidaysDoubleClickCommand { get; set; }
        //002
        public ICommand SelectedYearChangedCommand { get; set; }

        public ICommand DeleteEmployeeHolidaysCommand { get; set; }
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

        #region Constructor

        public EmployeesHolidaysViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeesHolidaysViewModel()...", category: Category.Info, priority: Priority.Low);
                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintHolidaysList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportHolidaysList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshHolidaysList));
                AddNewHolidayCommand = new RelayCommand(new Action<object>(AddNewHoliday));
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                EditHolidaysDoubleClickCommand = new DelegateCommand<object>(EditHolidaysInformation);
                DeleteEmployeeHolidaysCommand = new RelayCommand(new Action<object>(DeleteEmployeeHolidaysCommandAction));

                //002
                SelectedYearChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SelectedYearChangedCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor EmployeesHolidaysViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeesHolidaysViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to initialize Employee Holidays
        /// </summary>
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

                GeosApplication.Instance.FillFinancialYear();
                //[M051-08]
                //  SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    //CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //HrmService = new HrmServiceController("localhost:6699");
                    //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    if (CompanyHolidays.Count > 0)
                        SelectedHoliday = CompanyHolidays[0];
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

        /// <summary>
        /// Method to Print Holidays List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintHolidaysList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintHolidaysList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["HolidaysReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["HolidaysReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintHolidaysList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintHolidaysList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method to Export Holidays List
        /// </summary>
        /// <param name="obj"></param>
        private void ExportHolidaysList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportHolidaysList ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Holidays";
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
                    TableView holidaysTableView = ((TableView)obj);
                    holidaysTableView.ShowTotalSummary = false;
                    holidaysTableView.ShowFixedTotalSummary = false;
                    holidaysTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    holidaysTableView.ShowTotalSummary = false;

                    holidaysTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportHolidaysList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Get an error in ExportHolidaysList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
        }


        /// <summary>
        /// Method to Refresh Holidays List
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshHolidaysList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshHolidaysList()...", category: Category.Info, priority: Priority.Low);
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

                detailView.SearchString = null;
                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    //CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //HrmService = new HrmServiceController("localhost:6699");
                    //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));

                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshHolidaysList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshHolidaysList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshHolidaysList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshHolidaysList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Add New Holiday
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewHoliday(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewHoliday()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                //objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList == null)
                {
                    CustomMessageBox.Show("Please select atleast one company to add holiday!", Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                else if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && HrmCommon.Instance.SelectedAuthorizedPlantsList.Count > 1)
                {
                    CustomMessageBox.Show("Please select only one company to add holiday!", Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                //GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                //CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                AddHolidaysView addHolidaysView = new AddHolidaysView();
                AddHolidaysViewModel addHolidaysViewModel = new AddHolidaysViewModel();
                EventHandler handle = delegate { addHolidaysView.Close(); };
                addHolidaysViewModel.RequestClose += handle;
                addHolidaysView.DataContext = addHolidaysViewModel;
                addHolidaysViewModel.ExistHolidays(CompanyHolidays);

                //addHolidaysViewModel.WorkingPlantId = CurrentGeosProvider.IdCompany;
                //addHolidaysViewModel.Company = CurrentGeosProvider.Company;

                addHolidaysViewModel.Init();
                addHolidaysViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddHolidaysInformation").ToString();
                addHolidaysViewModel.IsNew = true;
                var ownerInfo = (detailView as FrameworkElement);
                addHolidaysView.Owner = Window.GetWindow(ownerInfo);
                addHolidaysView.ShowDialog();

                if (addHolidaysViewModel.IsSave)
                {
                    addHolidaysViewModel.NewHoliday.StartTime = addHolidaysViewModel.STime;
                    addHolidaysViewModel.NewHoliday.EndTime = addHolidaysViewModel.ETime;
                    CompanyHolidays.Add(addHolidaysViewModel.NewHoliday);
                    SelectedHoliday = addHolidaysViewModel.NewHoliday;
                }

                GeosApplication.Instance.Logger.Log("Method AddNewHoliday()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewHoliday()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillHolidaysListByPlant();
            }
            else
            {
                CompanyHolidays = new ObservableCollection<CompanyHoliday>();
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// [002][20180723][skhade][HRM-M043-16] Holidays must be filtered by year
        /// </summary>
        /// <param name="obj">Null</param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

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

            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillHolidaysListByPlant();
            }
            else
            {
                CompanyHolidays = new ObservableCollection<CompanyHoliday>();
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to Fill Holidays List By Plant
        /// </summary>
        private void FillHolidaysListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactListByPlant ...", category: Category.Info, priority: Priority.Low);

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    //CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //HrmService = new HrmServiceController("localhost:6699");
					//Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                }

                GeosApplication.Instance.Logger.Log("Method FillContactListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        /// <summary>
        /// /Method to Edit Holidays Information
        /// </summary>
        /// <param name="obj"></param>
        private void EditHolidaysInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditHolidaysInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                CompanyHoliday holiday = (CompanyHoliday)detailView.FocusedRow;
                SelectedHoliday = holiday;

                if (holiday != null)
                {
                    AddHolidaysView addHolidaysView = new AddHolidaysView();
                    AddHolidaysViewModel addHolidaysViewModel = new AddHolidaysViewModel();
                    EventHandler handle = delegate { addHolidaysView.Close(); };
                    addHolidaysViewModel.RequestClose += handle;
                    addHolidaysView.DataContext = addHolidaysViewModel;
                    addHolidaysViewModel.WorkingPlantId = holiday.IdCompany;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addHolidaysViewModel.InitReadOnly(CompanyHolidays, holiday);
                    else
                        addHolidaysViewModel.EditHoliday(CompanyHolidays, holiday);
                    addHolidaysViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditHolidaysInformation").ToString();
                    addHolidaysViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addHolidaysView.Owner = Window.GetWindow(ownerInfo);
                    addHolidaysView.ShowDialog();

                    if (addHolidaysViewModel.IsSave)
                    {
                        holiday.Name = addHolidaysViewModel.UpdateHoliday.Name;
                        holiday.IdHoliday = addHolidaysViewModel.UpdateHoliday.IdHoliday;
                        holiday.Holiday = addHolidaysViewModel.UpdateHoliday.Holiday;
                        holiday.StartDate = addHolidaysViewModel.UpdateHoliday.StartDate;
                        holiday.StartTime = addHolidaysViewModel.STime;
                        holiday.EndTime = addHolidaysViewModel.ETime;
                        holiday.EndDate = addHolidaysViewModel.UpdateHoliday.EndDate;
                        holiday.IsAllDayEvent = addHolidaysViewModel.UpdateHoliday.IsAllDayEvent;
                        holiday.IsRecursive = addHolidaysViewModel.UpdateHoliday.IsRecursive;
                        SelectedHoliday = holiday;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditHolidaysInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditHolidaysInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pjadhav][GEOS2-235][17.11.2022]
        private void DeleteEmployeeHolidaysCommandAction(object obj)
        {
            
            //  DataRow dr1 =  (DataRow)((System.Data.DataRowView)obj).Row;
            //  System.Data.DataRowView temp = (System.Data.DataRowView).dr1;
              SelectedHoliday = new CompanyHoliday();
              SelectedHoliday = (CompanyHoliday)obj;
                try
                {
                    GeosApplication.Instance.Logger.Log("Method DeleteEmployeeHolidaysCommandAction()...", category: Category.Info, priority: Priority.Low);

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteEmployeeHolidaysMessageWithoutCode"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = HrmService.IsDeleteEmployeeHolidays(SelectedHoliday.IdCompanyHoliday);


                        if (IsDeleted)
                        {

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeHolidaysDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                        //CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                        //HrmService = new HrmServiceController("localhost:6699");
                        //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                        CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));


                        //    companyHolidays.Remove(companyHolidays.FirstOrDefault(a => a.IdCompanyHoliday == (ulong)((System.Data.DataRowView)obj).Row.ItemArray[7]));
                        //    DataTableForGridLayout.Rows.Remove(dr1);

                        //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        //  //  TileBarArrange(TemplatesMenuList);
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteProductTypeItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteProductTypeItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteProductTypeItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteProductTypeItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

            
        #endregion

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }
    }
}
