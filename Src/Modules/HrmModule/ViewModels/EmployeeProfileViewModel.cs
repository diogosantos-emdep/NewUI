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
using NodaTime;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeProfileViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region TaskLog
        //[M051-08][Year selection is not saved after change section][adadibathina]
        //[000][SP63][skale][21-05-2019][GEOS2-273]Add new fields Company, Organization,Location in employees grid
        //[000][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        //[002][avpawar][22-04-2020][GEOS2-2172][Length of service wrong sort]
        #endregion

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return GetService<INavigationService>(); } }

        //IHrmService HrmService = new HrmServiceController("localhost:6699");
       //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");

        #endregion // End Services

        #region Public ICommand
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand ButtonAddNewEmployee { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand DocumentViewCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand SelectedYearChangedCommand { get; private set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand { get; set; }
        public ICommand CustomSortColumnDataCommand { get; set; }

        #endregion

        #region Declaration

        private bool isInIt = false;
        private bool isBusy;
        private bool isGridRowVisible;
        private List<Employee> employeeList;
        private Employee selectedGridRow;
        private ObservableCollection<Employee> finalEmployeeList;
        private bool isButtonRowVisible;
        private long selectedPeriod;
        private bool isEmployeeProfileViewColumnChooserVisible;
        public string EmployeeProfileGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "EmployeeProfileGridSetting.Xml";
        private ObservableCollection<EmployeeJobDescription> tempEmployeeJobDescriptionList;
        private ObservableCollection<EmployeePolyvalence> tempEmployeePolyvalenceList;
        private ObservableCollection<Company> combineIslocationIsorganizationIscompanyList; //[SP63-001] added
        string myFilterString;
        bool addShiftEnabled;
        private ObservableCollection<EmployeeExitEvent> exitEventList;
        #endregion

        #region Properties
        public ObservableCollection<EmployeeJobDescription> TempEmployeeJobDescriptionList
        {
            get
            {
                return tempEmployeeJobDescriptionList;
            }

            set
            {
                tempEmployeeJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempEmployeeJobDescriptionList"));
            }
        }

        public ObservableCollection<EmployeePolyvalence> TempEmployeePolyvalenceList
        {
            get
            {
                return tempEmployeePolyvalenceList;
            }

            set
            {
                tempEmployeePolyvalenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempEmployeePolyvalenceList"));
            }
        }
        public bool IsEmployeeProfileViewColumnChooserVisible
        {
            get { return isEmployeeProfileViewColumnChooserVisible; }
            set
            {
                isEmployeeProfileViewColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeProfileViewColumnChooserVisible"));
            }
        }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public ObservableCollection<Employee> FinalEmployeeList
        {
            get { return finalEmployeeList; }
            set
            {
                finalEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalEmployeeList"));
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
        public Employee SelectedGridRow
        {
            get { return selectedGridRow; }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

            }
        }
        public bool IsGridRowVisible
        {
            get
            {
                return isGridRowVisible;
            }

            set
            {
                isGridRowVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridRowVisible"));
            }
        }
        //[SP63-000] added
        public ObservableCollection<Company> CombineIslocationIsorganizationIscompanyList
        {
            get
            {
                return combineIslocationIsorganizationIscompanyList;
            }

            set
            {
                combineIslocationIsorganizationIscompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CombineIslocationIsorganizationIscompanyList"));
            }
        }
        //[SP-65-000]added
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public bool AddShiftEnabled
        {
            get { return addShiftEnabled; }
            set
            {
                addShiftEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddShiftEnabled"));
            }
        }
        public ObservableCollection<EmployeeExitEvent> ExitEventList
        {
            get { return exitEventList; }
            set
            {
                exitEventList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExitEventList"));
            }
        }

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

        #region IService
        //IServiceContainer serviceContainer = null;
        //protected IServiceContainer ServiceContainer
        //{
        //    get
        //    {
        //        if (serviceContainer == null)
        //            serviceContainer = new ServiceContainer(this);
        //        return serviceContainer;
        //    }
        //}     
        #endregion

        #region Constructor
        /// <summary>
        /// [001][skale][2019-04-11][GEOS2-46] Wrong Age in employees
        /// [002][2020-04-21][GEOS2-2172] [Length of service wrong sort]
        /// </summary>
        public EmployeeProfileViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeProfileViewModel()...", category: Category.Info, priority: Priority.Low);

                isInIt = true;
                CriteriaOperator.RegisterCustomFunction(new CustomFilter());

                GeosApplication.Instance.IsButtonRowVisible = true;
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshEmployeeList));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintEmployeeList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportEmployeeList));
                ButtonAddNewEmployee = new RelayCommand(new Action<object>(OpenAddNewEmployee));

                CommandGridDoubleClick = new DelegateCommand<object>(OpenEmployeeProfileDetailView);

                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                DocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeDocument));
                // SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                GeosApplication.Instance.FillFinancialYear();
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                SelectedYearChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SelectedYearChangedCommandAction);

                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                CustomCellAppearanceCommand = new DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);

                CustomUnboundColumnDataCommand = new DelegateCommand<object>(CustomUnboundColumnDataAction);//[001] added

                CustomSortColumnDataCommand = new DelegateCommand<object>(CustomSortColumnDataAction); //[002] added

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

                FinalEmployeeList = new ObservableCollection<Employee>();


                isInIt = false;
                IsEmployeeProfileViewColumnChooserVisible = true;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor EmployeeProfileViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeProfileViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// [001][skale][2019-21-5][GEOS2-273][SP63]Add new fields Company, Organization,Location in employees grid
        /// [002][SP-66][skale][27-06-2019][GEOS2-1589]THRM - Bug: Employees List filter doesn't work
        /// [003][smazhar][13-08-2020][GEOS2-2538]Employee display name not appear in Edit Employee and grid
        /// [004][cpatil][27-07-2021][GEOS2-2333]New columns in HRM
        /// [005][cpatil][21-03-2022][GEOS2-3634]HRM - Allow add future Job descriptions [#ERF97] - 3
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

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)//[001] added
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();//[001] added
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    #region Service Comments
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2036(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2041(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //[003] [004] [005]
                    //  IHrmService HrmService = new HrmServiceController("localhost:6699");
                    // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2250(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //service method changed GetAllEmployeesByIdCompany_V2330 to GetAllEmployeesByIdCompany_V2360 [GEOS2-4003][sshegaonkar][11.01.2023]
                    // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2360(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[Sudhir.Jangra][GEOS2-4536][01/06/2023] Changed SP And Service Version Wise
                    //   FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2400(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[Sudhir.Jangra][GEOS2-4686][19/07/2023]
                    //service method changed GetAllEmployeesByIdCompany_V2410 to GetAllEmployeesByIdCompany_V2420 [GEOS2-2466][rdixit][07.08.2023]
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 19 12 2023
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2470(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                    //HrmService = new HrmServiceController("localhost:6699");
                    //[rdixit][GEOS2-6661][27.11.2024]
                    #endregion
                    // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2590(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //Sudhir.Jangra][GEOS2-5656]
                    //HrmService = new HrmServiceController("localhost:6699");
                    FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    CalculateLengthOfService();
                    //SetHireDateAndLenghtOfService(FinalEmployeeList);
                }
                else
                {
                    FinalEmployeeList = new ObservableCollection<Employee>();
                }

                if (FinalEmployeeList.Count > 0)
                    SelectedGridRow = FinalEmployeeList.FirstOrDefault();

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][SP63][skale][2019-21-5][GEOS2-273]Add new fields Company, Organization,Location in employees grid
        /// [002][SP-66][skale][27-06-2019][GEOS2-1589]THRM - Bug: Employees List filter doesn't work
        /// [003][smazhar][13-08-2020][GEOS2-2538]Employee display name not appear in Edit Employee and grid
        /// [004][cpatil][27-07-2021][GEOS2-2333]New columns in HRM
        /// [005][cpatil][21-03-2022][GEOS2-3634]HRM - Allow add future Job descriptions [#ERF97] - 3
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshEmployeeList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshEmployeeList()...", category: Category.Info, priority: Priority.Low);
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
                IsBusy = true;

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)//[001] added
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();//[001] added
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    #region Service comments
                    // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2033(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));//[002]added
                    //[003] [004] [005]
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2250(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //service method changed GetAllEmployeesByIdCompany_V2330 to GetAllEmployeesByIdCompany_V2360 [GEOS2-4003][sshegaonkar][11.01.2023]                    
                    //[Sudhir.Jangra][GEOS2-4686]
                    //service method changed GetAllEmployeesByIdCompany_V2410 to GetAllEmployeesByIdCompany_V2420 [GEOS2-2466][rdixit][07.08.2023]

                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 19 12 2023
                    //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                    //[rdixit][GEOS2-6661][27.11.2024]
                    #endregion
                   // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2590(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

                    //[Sudhir.Jangra][GEOS2-5656]
                    FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));


                    CalculateLengthOfService();
                    //SetHireDateAndLenghtOfService(FinalEmployeeList);
                }
                else
                {
                    FinalEmployeeList = new ObservableCollection<Employee>();
                }

                IsBusy = false;

                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                        visibleFalseCoulumn++;
                }

                if (visibleFalseCoulumn > 0)
                    IsEmployeeProfileViewColumnChooserVisible = true;
                else
                    IsEmployeeProfileViewColumnChooserVisible = false;

                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshEmployeeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshEmployeeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][27.11.2024][GEOS2-6661]
        private void SetHireDateAndLenghtOfService(ObservableCollection<Employee> FinalEmployeeList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetHireDateAndLenghtOfService()...", category: Category.Info, priority: Priority.Low);
                if (FinalEmployeeList != null)
                {
                    foreach (var employee in FinalEmployeeList)
                    {
                        var activeSituations = employee.EmployeeContractSituations.Where(s => s.IdEmployeeExitEvent == null).ToList();

                        employee.HireDate = activeSituations.Any()? activeSituations.Min(s => s.ContractSituationStartDate): employee.EmployeeContractSituations
                                .OrderByDescending(s => s.ContractSituationStartDate).FirstOrDefault()?.ContractSituationStartDate;

                        if (employee.EmployeeJobDescription != null)
                        {
                            employee.EmployeeJobDescription.JobDescriptionStartDate = employee.HireDate;
                        }

                        // employee.LengthOfServiceString = CalculateLengthOfService(employee.EmployeeContractSituations);
                         employee.LengthOfServiceString = CalculateLengthOfService(employee.LengthOfService);

                    }
                }
                GeosApplication.Instance.Logger.Log("Method SetHireDateAndLenghtOfService()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method SetHireDateAndLenghtOfService()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOOS2-5656]
        public string CalculateLengthOfService(int totalDays)
        {
            int years = totalDays / 365; // Get years
            int remainingDays = totalDays % 365; // Get remaining days after extracting years
            int months = remainingDays / 30; // Approximate months (assuming 30 days per month)

            return $"{years}y {months}m";
        }
        //[Shweta.Thube][GEOS2-5656]
        private void CalculateLengthOfService()
        {
            try
            {
                //[rdixit][GEOS2-5657][11.03.2025]
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()...", category: Category.Info, priority: Priority.Low);
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.Today;
               
                // Calculate year and month difference
                if (FinalEmployeeList != null)
                {
                    foreach (var item in FinalEmployeeList)
                    {
                        if(item.EmployeeContractSituations != null)
                        {
                            List<EmployeeContractSituation> ContractList = item.EmployeeContractSituations.Select(i => (EmployeeContractSituation)i.Clone()).OrderBy(j => j.ContractSituationStartDate).ToList();

                            if (ContractList.Count > 0)
                            {
                                var lastExitEvent = item.EmployeeExitEvents?.OrderByDescending(i => i.ExitDate).FirstOrDefault();

                                if (lastExitEvent != null)
                                {
                                    var Newcontract = ContractList.Where(i => i.ContractSituationStartDate.Value.Date > lastExitEvent.ExitDate.Value.Date).FirstOrDefault();

                                    if (Newcontract == null)
                                    {
                                        startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                                        var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                        endDate = contract?.ContractSituationEndDate ?? DateTime.MinValue; // or handle null properly
                                    }
                                    else
                                    {
                                        startDate = Convert.ToDateTime(Newcontract.ContractSituationStartDate);
                                        var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                        //[rdixit][GEOS2-7877][16.04.2025]
                                        if (startDate > DateTime.Today)
                                            endDate = startDate;
                                        else
                                            endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                                    }                                        
                                }
                                else
                                {
                                    startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                                    var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                    //[rdixit][GEOS2-7877][16.04.2025]
                                    if (startDate > DateTime.Today)
                                        endDate = startDate;
                                    else
                                        endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                                }

                                int year = endDate.Year - startDate.Year;
                                int month = endDate.Month - startDate.Month;
                                int day = endDate.Day - startDate.Day;
                                if (day < 0)
                                {
                                    month -= 1;
                                    DateTime previousMonth = endDate.AddMonths(-1);
                                    day += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                                }
                                if (month < 0)
                                {
                                    year -= 1;
                                    month += 12;
                                }
                                item.EmployeeJobDescription.JobDescriptionStartDate = startDate;
                                item.LengthOfServiceString = Convert.ToString(year) + "Y  " + Convert.ToString(month) + "M";
                            }
                        }
                    }
                    
                    
                }
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateLengthOfService()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintEmployeeList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeList()...", category: Category.Info, priority: Priority.Low);

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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintEmployeeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportEmployeeList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEmployeeList()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Employee List";
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
                    TableView employeeTableView = ((TableView)obj);
                    employeeTableView.ShowTotalSummary = false;
                    employeeTableView.ShowFixedTotalSummary = false;
                    employeeTableView.ExportToXlsx(ResultFileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });

                    IsBusy = false;
                    employeeTableView.ShowTotalSummary = true;
                    employeeTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportEmployeeList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportEmployeeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        /// <summary>
        /// [001][skale][2019-17-04][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][skale][2019-21-5][GEOS2-273]Add new fields Company, Organization,Location in employees grid
        /// [003][skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
        ///[004][smazhar][11-08-2020][GEOS2-2498] Employee display name [#ERF67]
        /// </summary>
        /// <param name="obj"></param>
        private void OpenEmployeeProfileDetailView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfileDetailView()...", category: Category.Info, priority: Priority.Low);
                 //[rdixit][GEOS2-6979][02.04.2025]
                if (GeosApplication.Instance.IsHRMTravelManagerPermission 
                    || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                {
                    CustomMessageBox.Show(
                        string.Format(Application.Current.FindResource("UnAuthorizedForEmployeeDetail").ToString()), 
                        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                Employee employee = null;
                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    employee = (Employee)detailView.DataControl.CurrentItem;
                    SelectedGridRow = employee;
                }
                else if (obj is Employee)
                {
                    employee = (Employee)obj;
                    SelectedGridRow = employee;
                }

                if (employee != null)
                {
                    EmployeeProfileDetailView employeeProfileDetailView = new EmployeeProfileDetailView();
                    EmployeeProfileDetailViewModel employeeProfileDetailViewModel = new EmployeeProfileDetailViewModel();
                    EventHandler handle = delegate { employeeProfileDetailView.Close(); };
                    employeeProfileDetailViewModel.RequestClose += handle;
                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;

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
                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            employeeProfileDetailViewModel.InitReadOnly(SelectedGridRow, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                        else
                            employeeProfileDetailViewModel.Init(SelectedGridRow, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());

                    }

                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;
                    employeeProfileDetailViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;

                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (obj as FrameworkElement);
                    employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);

                    employeeProfileDetailView.ShowDialog();

                    if (employeeProfileDetailViewModel.IsSaveChanges == true)
                    {
                        employee.EmployeeCode = employeeProfileDetailViewModel.EmployeeUpdatedDetail.EmployeeCode;
                        employee.FirstName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.FirstName;
                        employee.LastName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.LastName;
                        employee.DisplayName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.DisplayName;
                        employee.NativeName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.NativeName;
                        employee.DateOfBirth = employeeProfileDetailViewModel.EmployeeUpdatedDetail.DateOfBirth;

                        if (employeeProfileDetailViewModel.EmployeeUpdatedDetail.DateOfBirth != null)
                            employee.BirthDate = (DateTime)employeeProfileDetailViewModel.EmployeeUpdatedDetail.DateOfBirth;
                        else
                            employee.BirthDate = DateTime.MinValue;

                        employee.Gender = employeeProfileDetailViewModel.EmployeeUpdatedDetail.Gender;
                        employee.IdGender = employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdGender;
                        employee.MaritalStatus = employeeProfileDetailViewModel.EmployeeUpdatedDetail.MaritalStatus;
                        employee.IdMaritalStatus = employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdMaritalStatus;
                        employee.AddressRegion = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressRegion;
                        employee.Nationality = employeeProfileDetailViewModel.EmployeeUpdatedDetail.Nationality;
                        employee.IdNationality = employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdNationality;
                        employee.AddressCountry = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressCountry;
                        employee.AddressIdCountry = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressIdCountry;
                        employee.AddressCity = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressCity;

                        employee.AddressStreet = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressStreet;
                        employee.AddressZipCode = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressZipCode;
                        employee.Remarks = employeeProfileDetailViewModel.EmployeeUpdatedDetail.Remarks;
                        employee.Disability = employeeProfileDetailViewModel.EmployeeUpdatedDetail.Disability;
                        employee.Company = employeeProfileDetailViewModel.EmployeeUpdatedDetail.Company;

                        employee.ProfileImageInBytes = employeeProfileDetailViewModel.EmployeeUpdatedDetail.ProfileImageInBytes;

                        employee.EmployeeStatus = employeeProfileDetailViewModel.EmployeeUpdatedDetail.EmployeeStatus;
                        employee.IdEmployeeStatus = employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployeeStatus;

                        employee.FullAddress = employee.AddressStreet;
                        if (!string.IsNullOrEmpty(employee.AddressZipCode))
                            employee.FullAddress = employee.FullAddress + " - " + employee.AddressZipCode;
                        if (!string.IsNullOrEmpty(employee.AddressCity))
                            employee.FullAddress = employee.FullAddress + " - " + employee.AddressCity;
                        if (!string.IsNullOrEmpty(employee.AddressRegion))
                            employee.FullAddress = employee.FullAddress + " - " + employee.AddressRegion;
                        if (!string.IsNullOrEmpty(employee.AddressCountry.Name))
                            employee.FullAddress = employee.FullAddress + " - " + employee.AddressCountry.Name;

                        employee.EmployeePersonalContacts = new List<EmployeeContact>(employeeProfileDetailViewModel.EmployeeContactList);
                        employee.EmployeeProfessionalContacts = new List<EmployeeContact>(employeeProfileDetailViewModel.EmployeeProfessionalContactList);
                        employee.EmployeeDocuments = new List<EmployeeDocument>(employeeProfileDetailViewModel.EmployeeDocumentList);
                        employee.EmployeeLanguages = new List<EmployeeLanguage>(employeeProfileDetailViewModel.EmployeeLanguageList);
                        employee.EmployeeEducationQualifications = new List<EmployeeEducationQualification>(employeeProfileDetailViewModel.EmployeeEducationQualificationList);
                        //[001] added
                        employee.EmployeePolyvalences = new List<EmployeePolyvalence>(employeeProfileDetailViewModel.EmployeePolyvalenceList);
                        employee.EmployeeContractSituations = new List<EmployeeContractSituation>(employeeProfileDetailViewModel.EmployeeContractSituationList);
                        employee.EmployeeFamilyMembers = new List<EmployeeFamilyMember>(employeeProfileDetailViewModel.EmployeeFamilyMemberList);
                        employee.EmployeeJobDescriptions = new List<EmployeeJobDescription>(employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(ejd => ejd.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date && (ejd.JobDescriptionEndDate == null || ejd.JobDescriptionEndDate >= DateTime.Now)).ToList());

                        if (employee.EmployeeJobDescriptions == null || employee.EmployeeJobDescriptions.Count == 0)
                        {
                            employee.EmployeeJobDescriptions = new List<EmployeeJobDescription>(employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(i => i.JobDescriptionStartDate.Value.Date > DateTime.Now.Date));
                        }
                        //employee.EmployeeJobDescriptions = new List<EmployeeJobDescription>(employeeProfileDetailViewModel.UpdatedEmployeeJobDescriptionList);
                        //[003] added
                        employee.EmployeeShifts = new List<EmployeeShift>(employeeProfileDetailViewModel.EmployeeShiftList);

                        if (employee.EmployeeShifts.Count > 0 || employee.EmployeeShifts != null)
                        {
                            foreach (EmployeeShift shiftinuse in employee.EmployeeShifts)//[sudhir.jangra][GEOS2-2716][21/10/2022][added field EmployeeshiftInUse]
                            {
                                if (shiftinuse.CompanyShift.IsInUse == 0)
                                    shiftinuse.IsEmployeeShiftNOTInUse = true;
                            }
                            employee.EmployeeShiftNames = String.Join("\n", employee.EmployeeShifts.Select(x => x.CompanyShift.Name).ToArray());
                            employee.EmployeeShiftScheduleNames = String.Join("\n", employee.EmployeeShifts.Select(x => x.CompanyShift.CompanySchedule.Name).Distinct().ToArray());

                        }
                        else
                        {
                            employee.EmployeeShiftNames = null;
                            employee.EmployeeShiftScheduleNames = null;
                        }

                        employee.EmployeeProfessionalEducations = new List<EmployeeProfessionalEducation>(employeeProfileDetailViewModel.EmployeeProfessionalEducationList);
                        employee.EmployeeChangelogs = new List<EmployeeChangelog>(employeeProfileDetailViewModel.EmployeeAllChangeLogList);
                        employee.EmpJobCodes = string.Empty;



                        if (employee.EmployeeJobDescriptions.Count > 0)
                        {
                            string JobTitleAndCode = "";
                            string jobDescriptionCode = string.Empty;
                            string abbreviation = string.Empty;
                            employee.EmployeeJobDescription = new EmployeeJobDescription();
                            EmployeeJobDescription JD = employee.EmployeeJobDescriptions.FirstOrDefault(x => x.JobDescriptionUsage == 100);

                            if (JD != null)
                            {
                                JobTitleAndCode = JD.JobDescription.JobDescriptionCode + " - " + JD.JobDescription.JobDescriptionTitle;
                                jobDescriptionCode = JD.JobDescription.JobDescriptionCode;
                                abbreviation = JD.JobDescription.Abbreviation;
                            }
                            else
                            {
                                foreach (EmployeeJobDescription jobDesc in employee.EmployeeJobDescriptions)
                                {
                                    if (JobTitleAndCode == string.Empty)
                                    {
                                        JobTitleAndCode = JobTitleAndCode + jobDesc.JobDescription.JobDescriptionCode + " - " + jobDesc.JobDescription.JobDescriptionTitle + " (" + jobDesc.JobDescriptionUsage + "%" + ")";
                                        jobDescriptionCode = jobDesc.JobDescription.JobDescriptionCode;
                                        abbreviation = jobDesc.JobDescription.Abbreviation;
                                    }
                                    else
                                    {
                                        JobTitleAndCode = JobTitleAndCode + "\n" + jobDesc.JobDescription.JobDescriptionCode + " - " + jobDesc.JobDescription.JobDescriptionTitle + " (" + jobDesc.JobDescriptionUsage + "%" + ")";
                                        jobDescriptionCode = jobDescriptionCode + "\n" + jobDesc.JobDescription.JobDescriptionCode;
                                        abbreviation = abbreviation + "\n" + jobDesc.JobDescription.Abbreviation;
                                    }
                                }
                            }

                            employee.EmployeeJobDescriptions[0].JobDescription.JobDescriptionTitleAndCode = JobTitleAndCode;
                            employee.EmpJobCodeAbbreviations = abbreviation;
                            employee.EmpJobCodes = jobDescriptionCode;
                            employee.EmployeeJobDescription = employee.EmployeeJobDescriptions[0];
                        }


                        if (employee.EmployeeProfessionalContacts.Count > 0)
                        {
                            EmployeeContact ProfContact = employee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88);
                            if (ProfContact != null)
                            {
                                employee.EmployeeContactEmail = ProfContact.EmployeeContactValue;
                            }
                            else
                            {
                                employee.EmployeeContactEmail = string.Empty;
                            }

                            ProfContact = employee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90);
                            if (ProfContact != null)
                            {
                                employee.EmployeeContactMobile = ProfContact.EmployeeContactValue;
                            }
                            else
                            {
                                employee.EmployeeContactMobile = string.Empty;
                            }
                        }

                        employee.EmployeeContactPrivateEmailList = employeeProfileDetailViewModel.EmployeeContactList.Where(x => x.EmployeeContactIdType == 88).Select(x => x.EmployeeContactValue).ToList();
                        employee.EmployeeContactPrivateMobileList = employeeProfileDetailViewModel.EmployeeContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToList();
                        employee.EmployeeContactCompanyEmailList = employeeProfileDetailViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 88).Select(x => x.EmployeeContactValue).ToList();
                        employee.EmployeeContactCompanyMobileList = employeeProfileDetailViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToList();

                        TempEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(employeeProfileDetailViewModel.EmployeeJobDescriptionsList.Where(a => (a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).ToList());


                        if (TempEmployeeJobDescriptionList.Count == 0)
                        {
                            List<EmployeeJobDescription> JDListOrderedByDescending =
                                employeeProfileDetailViewModel.EmployeeJobDescriptionList.OrderByDescending(x => x.JobDescriptionEndDate).ToList();

                            int TotalUsage = 0;
                            List<EmployeeJobDescription> JDListLatest100Percentage = new List<EmployeeJobDescription>();
                            for (int i = 0; i < JDListOrderedByDescending.Count; i++)
                            {
                                TotalUsage += JDListOrderedByDescending[i].JobDescriptionUsage;
                                if (TotalUsage <= 100)
                                {
                                    JDListLatest100Percentage.Add(JDListOrderedByDescending[i]);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (JDListLatest100Percentage.Count == 0)
                            {
                                employee.EmployeeJobTitles = string.Empty;
                            }
                            else if (JDListLatest100Percentage.Count == 1)
                            {
                                employee.EmployeeJobTitles = String.Join("\n", JDListLatest100Percentage.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                                employee.EmpJobCodes = String.Join("\n", JDListLatest100Percentage.Select(x => x.JobDescription.JobDescriptionCode).ToArray());
                                employee.EmpJobCodeAbbreviations = String.Join("\n", JDListLatest100Percentage.Select(x => x.JobDescription.Abbreviation).ToArray());
                            }
                            else
                            {
                                employee.EmployeeJobTitles = String.Join("\n", JDListLatest100Percentage.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                employee.EmployeeJobTitles = AddPlantAliasToJobTitle(employee.EmployeeJobTitles, JDListLatest100Percentage.ToList());

                            }
                            //employee.EmployeeJobCodes = JDListLatest100Percentage.Select(x => x.JobDescription.JobDescriptionCode).ToList();
                            //TempEmployeeJobDescriptionList.Clear();
                            //TempEmployeeJobDescriptionList.AddRange(JDListLatest100Percentage);
                        }

                        else
                        {
                            if (TempEmployeeJobDescriptionList.Count == 1)
                            {
                                employee.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                            }
                            else
                            {
                                employee.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                employee.EmployeeJobTitles = AddPlantAliasToJobTitle(employee.EmployeeJobTitles, TempEmployeeJobDescriptionList.ToList());
                            }
                            employee.EmployeeJobCodes = TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode).ToList();
                        }


                        // [pjadhav][GEOS2-2618][10/19/2022]

                        //TempEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(employeeProfileDetailViewModel.EmployeePolyvalenceList.Where(a => (a.JobDescriptionEndDate != null || a.JobDescriptionEndDate >= DateTime.Now) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).ToList());

                        // [pramod.misal][GEOS2-4556][13/07/2022]
                        TempEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(employeeProfileDetailViewModel.EmployeePolyvalenceList.ToList());

                        if (TempEmployeePolyvalenceList.Count == 0)
                        {
                            List<EmployeePolyvalence> EPListOrderedByDescending =
                                employeeProfileDetailViewModel.EmployeePolyvalenceList.OrderByDescending(x => x.JobDescriptionEndDate).ToList();

                            int TotalUsage = 0;
                            List<EmployeePolyvalence> EPListLatest100Percentage = new List<EmployeePolyvalence>();
                            for (int i = 0; i < EPListOrderedByDescending.Count; i++)
                            {
                                TotalUsage += EPListOrderedByDescending[i].PolyvalenceUsage;
                                if (TotalUsage <= 100)
                                {
                                    EPListOrderedByDescending.Add(EPListOrderedByDescending[i]);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (EPListLatest100Percentage.Count == 0)
                            {
                                employee.EmployeePolyvalence = string.Empty;
                            }
                            else if (EPListLatest100Percentage.Count == 1)
                            {
                                employee.EmployeePolyvalence = String.Join("\n", EPListLatest100Percentage.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                                employee.EmpJobCodeAbbreviations = String.Join("\n", EPListLatest100Percentage.Select(x => x.JobDescription.Abbreviation).ToArray());
                            }
                            else
                            {
                                employee.EmployeePolyvalence = String.Join("\n", EPListLatest100Percentage.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.PolyvalenceUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                employee.EmployeePolyvalence = AddEmployeePolyvalence(employee.EmployeePolyvalence, EPListLatest100Percentage.ToList());

                            }

                        }
                        else
                        {
                            if (TempEmployeePolyvalenceList.Count == 1)
                            {
                                employee.EmployeePolyvalence = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                            }
                            else
                            {
                                employee.EmployeePolyvalence = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.PolyvalenceUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                employee.EmployeePolyvalence = AddEmployeePolyvalence(employee.EmployeePolyvalence, TempEmployeePolyvalenceList.ToList());
                            }
                            //  employee.EmployeeJobCodes = TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionCode).ToList();
                        }






                        //if (employee.EmployeeJobDescription.JobDescriptionUsage == 100)
                        //{
                        //    employee.EmployeeJobTitles = String.Join("\n", employeeProfileDetailViewModel.EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                        //}
                        //else
                        //{
                        //    employee.EmployeeJobTitles = String.Join("\n", employeeProfileDetailViewModel.EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + "(" + x.JobDescriptionUsage + "%)").ToArray());
                        //}

                        employee.EmployeeContactCompanyMobiles = String.Join("\n", employeeProfileDetailViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToArray());
                        employee.EmployeeContactPrivateMobiles = String.Join("\n", employeeProfileDetailViewModel.EmployeeContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToArray());
                        employee.EmployeeContactCompanySkypes = String.Join("\n", employeeProfileDetailViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 87).Select(x => x.EmployeeContactValue).ToArray());
                        employee.EmployeeContactCompanyLandlines = String.Join("\n", employeeProfileDetailViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 89).Select(x => x.EmployeeContactValue).ToArray());
                        employee.EmployeeContactTranings = String.Join("\n", employeeProfileDetailViewModel.EmployeeProfessionalEducationList.Where(x => x.IdType == 120).Select(x => x.Name).ToArray());
                        employee.ExitDate = employeeProfileDetailViewModel.EmployeeUpdatedDetail.ExitDate;
                        employee.ExitIdReason = employeeProfileDetailViewModel.EmployeeUpdatedDetail.ExitIdReason;
                        employee.ExitReason = employeeProfileDetailViewModel.EmployeeUpdatedDetail.ExitReason;
                        employee.ExitRemarks = employeeProfileDetailViewModel.EmployeeUpdatedDetail.ExitRemarks;

                        employee.Languages = String.Join("\n", employeeProfileDetailViewModel.EmployeeLanguageList.Select(x => x.Language.Value).ToArray());
                        //employee.EmployeeJobCodes = TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode).ToList();

                        if (employee.EmployeeDocuments.Count > 0)
                        {
                            employee.EmployeeDocument = new EmployeeDocument();
                            employee.EmployeeDocument = employee.EmployeeDocuments.FirstOrDefault(i => i.EmployeeDocumentIdType == 80);
                        }
                        //employee.CompanyShift = employeeProfileDetailViewModel.EmployeeUpdatedDetail.CompanyShift;
                        //employee.CompanyShift.CompanySchedule.Name = employeeProfileDetailViewModel.EmployeeUpdatedDetail.CompanyShift.CompanySchedule.Name;

                        employee.LengthOfServiceString = employeeProfileDetailViewModel.LengthOfService;

                        // [adhatkar] [GEOS2-3238][we have to get latest contract details as per luis]

                        //DateTime date = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), DateTime.Now.Month, DateTime.Now.Day);
                        if (employee.EmployeeContractSituations != null && employee.EmployeeContractSituations.Count > 0)
                        {
                            //List<EmployeeContractSituation> tempList = employee.EmployeeContractSituations.Where(x => x.ContractSituationEndDate >= date || x.ContractSituationEndDate == null).ToList();
                            //tempList = tempList.OrderByDescending(row => row.ContractSituationEndDate ?? DateTime.MinValue).ToList();

                            //foreach (EmployeeContractSituation item in tempList)
                            //{
                            //    if (date >= item.ContractSituationStartDate && date <= item.ContractSituationEndDate)
                            //    {
                            //        employee.ContractSituation.Name = item.ContractSituation.Name;
                            //        break;
                            //    }
                            //    else if (date >= item.ContractSituationStartDate && item.ContractSituationEndDate == null)
                            //    {
                            //        employee.ContractSituation.Name = item.ContractSituation.Name;
                            //        break;
                            //    }
                            //    else
                            //    {
                            //        employee.ContractSituation.Name = string.Empty;
                            //    }

                            //}

                            //if (employee.EmployeeContractSituations != null)
                            //{
                            //    employee.ContractSituation.Name = employee.EmployeeContractSituations.OrderByDescending(i => i.ContractSituationStartDate).FirstOrDefault().ContractSituation.Name;
                            //}
                            //else
                            //{
                            //    employee.ContractSituation.Name = string.Empty;
                            //}
                            employee.ContractSituation.Name = employee.EmployeeContractSituations.OrderByDescending(i => i.ContractSituationStartDate).FirstOrDefault().ContractSituation.Name;
                        }
                        else
                            employee.ContractSituation.Name = string.Empty;

                        employee.EmployeeDocPassport = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 81).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocClockId = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 193).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocDrLicense = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 82).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocOther = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 153).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocPriIns = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 119).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocPubIns = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 118).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocVisa = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 152).Select(y => y.EmployeeDocumentNumber).ToArray());
                        employee.EmployeeDocNationalId = String.Join("\n", employeeProfileDetailViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 80).Select(y => y.EmployeeDocumentNumber).ToArray());

                        //[Sprint-62][09/05/2019][sdesai]----Add new fields Company, Organization, Location in employees grid
                        employee.CompanyLocation = employeeProfileDetailViewModel.LocationList[employeeProfileDetailViewModel.SelectedLocationIndex];




                        //[GEOS2-3238][adhatkar]
                        //List<EmployeeContractSituation> tempEmployeeContractSituation = employeeProfileDetailViewModel.EmployeeContractSituationList.Where(x => x.Company != null && (x.ContractSituationEndDate == null || x.ContractSituationEndDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)).ToList();

                        //if (tempEmployeeContractSituation != null && tempEmployeeContractSituation.Count > 0)
                        //{
                        //    if (tempEmployeeContractSituation.Count > 1)
                        //    {
                        //        EmployeeContractSituation tempContract = tempEmployeeContractSituation.FirstOrDefault(x => x.ContractSituationEndDate != null);
                        //        if (tempContract != null)
                        //            employee.Company = tempContract.Company.Alias;
                        //    }
                        //    else if (tempEmployeeContractSituation.Count == 1)
                        //    {
                        //        employee.Company = tempEmployeeContractSituation[0].Company.Alias;
                        //    }
                        //}
                        //else
                        //{
                        //    List<EmployeeContractSituation> tempList = employee.EmployeeContractSituations;
                        //    tempList = tempList.OrderByDescending(row => row.ContractSituationEndDate ?? DateTime.MinValue).ToList();
                        //    employee.Company = tempList[0].Company.Alias;
                        //}

                        if (employee.EmployeeContractSituations != null && employee.EmployeeContractSituations.Count > 0)
                        {
                            EmployeeContractSituation tempEmployeeContractSituation = employee.EmployeeContractSituations.OrderByDescending(i => i.ContractSituationStartDate).FirstOrDefault();
                            employee.Company = tempEmployeeContractSituation.Company.Alias;
                        }

                        employee.Organization = String.Join("\n", employeeProfileDetailViewModel.EmployeeTopFourJobDescriptionList.Select(x => x.Company.Alias).Distinct().ToList().ToArray());
                        employee.EmployeeDepartments = String.Join("\n", employeeProfileDetailViewModel.EmployeeTopFourJobDescriptionList.Select(x => x.JobDescription.Department.DepartmentName).ToArray());

                        List<Company> SelectedPlant = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        if (SelectedPlant != null)
                        {
                            List<Company> isExistPlantList = SelectedPlant.Where(x => x.Alias.Contains(employee.CompanyLocation.Alias) || x.Alias.Contains(employee.Company) || x.Alias.Contains(employee.Organization)).ToList();
                            if (isExistPlantList.Count <= 0)
                            {
                                finalEmployeeList.Remove(employee);
                                if (finalEmployeeList.Count > 0)
                                    SelectedGridRow = finalEmployeeList[0];
                            }
                            else
                                SelectedGridRow = employee;
                        }
                    }
                    //End
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }


                }
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfileDetailView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeProfileDetailView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Add plant alias when JD belongs to different plant
        /// [cpatil][2018-12-25][HRM-M053-04] Add plant when JD belongs to different plants
        /// </summary>
        private string AddPlantAliasToJobTitle(string employeeJobTitles, List<EmployeeJobDescription> empJobDescription)
        {
            if (!string.IsNullOrEmpty(employeeJobTitles))
            {
                List<String> result = new List<String>(employeeJobTitles.Split(new string[] { "\n" }, StringSplitOptions.None));
                if (result.Count() > 1)
                {
                    string matchString = null;
                    bool isDiffPlant = false;
                    matchString = result[0].Substring(result[0].IndexOf("[") + 1, ((result[0].IndexOf("]")) - (result[0].IndexOf("[") + 1)));
                    if (string.IsNullOrEmpty(matchString))
                    {
                        isDiffPlant = false;
                    }
                    else
                    {
                        matchString = " [" + matchString + "]";
                        foreach (string item in result)
                        {
                            if (!item.Contains(matchString))
                            {
                                isDiffPlant = true;
                                break;
                            }
                        }
                    }

                    if (isDiffPlant == false)
                    {
                        employeeJobTitles = String.Join("\n", empJobDescription.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)").ToArray());
                    }

                }

            }
            return employeeJobTitles;
        }
        /// <summary>
        /// Custom filter 
        /// [001][skhade][2018-08-29][HRM-M046-13] Add JD Code as prefix in Employees grid
        /// [002][SP-66][skale][27-06-2019][GEOS2-1589]THRM - Bug: Employees List filter doesn't work
        /// [003][skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
        /// </summary>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "EmployeeContactEmailUnbound" && e.Column.FieldName != "EmployeeContactMobileUnbound" && e.Column.Name != "EmployeeJobTitles" && e.Column.FieldName != "EmployeeContactPrivateMobileUnbound" && e.Column.FieldName != "EmployeeContactPrivateEmailUnbound" && e.Column.FieldName != "EmployeeContactCompanyMobiles" && e.Column.FieldName != "EmployeeContactCompanySkypes" && e.Column.FieldName != "EmployeeContactCompanyLandlines" && e.Column.FieldName != "EmployeeContactTranings" && e.Column.FieldName != "EmployeeContactPrivateMobiles" && e.Column.FieldName != "Languages" && e.Column.FieldName != "EmpJobCodes" && e.Column.FieldName != "EmployeeDocDrLicense" && e.Column.FieldName != "EmployeeDocPassport" && e.Column.FieldName != "EmployeeDocClockId" && e.Column.FieldName != "EmployeeDocPubIns" && e.Column.FieldName != "EmployeeDocPriIns" && e.Column.FieldName != "EmployeeDocVisa" && e.Column.FieldName != "EmployeeDocOther" && e.Column.FieldName != "EmployeeDocNationalId" && e.Column.FieldName != "EmpJobCodeAbbreviations" && e.Column.FieldName != "Company" && e.Column.FieldName != "Organization" && e.Column.FieldName != "EmployeeDepartments" && e.Column.FieldName != "EmployeeShiftScheduleNames" && e.Column.FieldName != "EmployeeShiftNames" && e.Column.FieldName != "EmployeePolyvalence" && e.Column.FieldName != "Country")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "EmployeeContactEmailUnbound")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([EmployeeContactEmailUnbound])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([EmployeeContactEmailUnbound])")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactCompanyEmailList == null)
                        {
                            continue;
                        }

                        foreach (var employeeContactEmail in dataObject.EmployeeContactCompanyEmailList)
                        {
                            if (!string.IsNullOrEmpty(employeeContactEmail))
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactEmail.ToString()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = employeeContactEmail;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse("CustomFilter([EmployeeContactEmailUnbound], ?)", employeeContactEmail);//[002] added
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeContactCompanyMobiles")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactCompanyMobiles = ''")  // SalesOwner is equal to ' '
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactCompanyMobiles <> ''")  // SalesOwner does not equal to ' '
                    });
                    //[002]added
                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactCompanyMobiles == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeContactCompanyMobiles != null)
                        {
                            if (dataObject.EmployeeContactCompanyMobiles.Contains("\n"))
                            {
                                string tempEmployeeContactCompanyMobiles = dataObject.EmployeeContactCompanyMobiles;

                                for (int index = 0; index < tempEmployeeContactCompanyMobiles.Length; index++)
                                {
                                    string employeeContactCompanyMobiles = tempEmployeeContactCompanyMobiles.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactCompanyMobiles))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeContactCompanyMobiles;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactCompanyMobiles Like '%{0}%'", employeeContactCompanyMobiles));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeContactCompanyMobiles.Contains("\n"))
                                        tempEmployeeContactCompanyMobiles = tempEmployeeContactCompanyMobiles.Remove(0, employeeContactCompanyMobiles.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeContactCompanyMobiles == dataObject.EmployeeContactCompanyMobiles).Select(slt => slt.EmployeeContactCompanyMobiles).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeContactCompanyMobiles == dataObject.EmployeeContactCompanyMobiles).Select(slt => slt.EmployeeContactCompanyMobiles).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactCompanyMobiles Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeContactCompanyMobiles == dataObject.EmployeeContactCompanyMobiles).Select(slt => slt.EmployeeContactCompanyMobiles).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }
                else if (e.Column.FieldName == "EmployeeContactCompanySkypes")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactCompanySkypes = ''")  // SalesOwner is equal to ' '
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactCompanySkypes <> ''")  // SalesOwner does not equal to ' '
                    });
                    //[002]added
                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactCompanySkypes == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeContactCompanySkypes != null)
                        {
                            if (dataObject.EmployeeContactCompanySkypes.Contains("\n"))
                            {
                                string tempEmployeeContactCompanySkypes = dataObject.EmployeeContactCompanySkypes;

                                for (int index = 0; index < tempEmployeeContactCompanySkypes.Length; index++)
                                {
                                    string employeeContactCompanySkypes = tempEmployeeContactCompanySkypes.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactCompanySkypes))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeContactCompanySkypes;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactCompanySkypes Like '%{0}%'", employeeContactCompanySkypes));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeContactCompanySkypes.Contains("\n"))
                                        tempEmployeeContactCompanySkypes = tempEmployeeContactCompanySkypes.Remove(0, employeeContactCompanySkypes.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeContactCompanySkypes == dataObject.EmployeeContactCompanySkypes).Select(slt => slt.EmployeeContactCompanySkypes).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeContactCompanySkypes == dataObject.EmployeeContactCompanySkypes).Select(slt => slt.EmployeeContactCompanySkypes).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactCompanySkypes Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeContactCompanySkypes == dataObject.EmployeeContactCompanySkypes).Select(slt => slt.EmployeeContactCompanySkypes).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }
                else if (e.Column.FieldName == "EmployeeContactCompanyLandlines")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactCompanyLandlines = ''")  // SalesOwner is equal to ' '
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactCompanyLandlines <> ''")  // SalesOwner does not equal to ' '
                    });
                    //[002]added
                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactCompanyLandlines == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeContactCompanyLandlines != null)
                        {
                            if (dataObject.EmployeeContactCompanyLandlines.Contains("\n"))
                            {
                                string tempEmployeeContactCompanyLandlines = dataObject.EmployeeContactCompanyLandlines;

                                for (int index = 0; index < tempEmployeeContactCompanyLandlines.Length; index++)
                                {
                                    string employeeContactCompanyLandlines = tempEmployeeContactCompanyLandlines.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactCompanyLandlines))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeContactCompanyLandlines;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactCompanyLandlines Like '%{0}%'", employeeContactCompanyLandlines));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeContactCompanyLandlines.Contains("\n"))
                                        tempEmployeeContactCompanyLandlines = tempEmployeeContactCompanyLandlines.Remove(0, employeeContactCompanyLandlines.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeContactCompanyLandlines == dataObject.EmployeeContactCompanyLandlines).Select(slt => slt.EmployeeContactCompanyLandlines).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeContactCompanyLandlines == dataObject.EmployeeContactCompanyLandlines).Select(slt => slt.EmployeeContactCompanyLandlines).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactCompanyLandlines Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeContactCompanyLandlines == dataObject.EmployeeContactCompanyLandlines).Select(slt => slt.EmployeeContactCompanyLandlines).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }
                else if (e.Column.FieldName == "EmployeeContactTranings")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactTranings = ''")  // SalesOwner is equal to ' '
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactTranings <> ''")  // SalesOwner does not equal to ' '
                    });
                    //[002]added
                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactTranings == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeContactTranings != null)
                        {
                            if (dataObject.EmployeeContactTranings.Contains("\n"))
                            {
                                string tempEmployeeContactTranings = dataObject.EmployeeContactTranings;

                                for (int index = 0; index < tempEmployeeContactTranings.Length; index++)
                                {
                                    string employeeContactTranings = tempEmployeeContactTranings.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactTranings))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeContactTranings;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactTranings Like '%{0}%'", employeeContactTranings));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeContactTranings.Contains("\n"))
                                        tempEmployeeContactTranings = tempEmployeeContactTranings.Remove(0, employeeContactTranings.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeContactTranings == dataObject.EmployeeContactTranings).Select(slt => slt.EmployeeContactTranings).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeContactTranings == dataObject.EmployeeContactTranings).Select(slt => slt.EmployeeContactTranings).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactTranings Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeContactTranings == dataObject.EmployeeContactTranings).Select(slt => slt.EmployeeContactTranings).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }
                else if (e.Column.FieldName == "EmployeeContactPrivateMobiles")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactPrivateMobiles = ''")   // SalesOwner is equal to ' '
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeContactPrivateMobiles <> ''")  // SalesOwner does not equal to ' '
                    });
                    //[002] added
                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactPrivateMobiles == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeContactPrivateMobiles != null)
                        {
                            if (dataObject.EmployeeContactPrivateMobiles.Contains("\n"))
                            {
                                string tempEmployeeContactPrivateMobiles = dataObject.EmployeeContactPrivateMobiles;
                                for (int index = 0; index < tempEmployeeContactPrivateMobiles.Length; index++)
                                {
                                    string employeeContactPrivateMobiles = tempEmployeeContactPrivateMobiles.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactPrivateMobiles))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeContactPrivateMobiles;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactPrivateMobiles Like '%{0}%'", employeeContactPrivateMobiles));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeContactPrivateMobiles.Contains("\n"))
                                        tempEmployeeContactPrivateMobiles = tempEmployeeContactPrivateMobiles.Remove(0, employeeContactPrivateMobiles.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeContactPrivateMobiles == dataObject.EmployeeContactPrivateMobiles).Select(slt => slt.EmployeeContactPrivateMobiles).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeContactPrivateMobiles == dataObject.EmployeeContactPrivateMobiles).Select(slt => slt.EmployeeContactPrivateMobiles).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeContactPrivateMobiles Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeContactPrivateMobiles == dataObject.EmployeeContactPrivateMobiles).Select(slt => slt.EmployeeContactPrivateMobiles).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }
                //END
                else if (e.Column.FieldName == "EmployeeContactPrivateEmailUnbound")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([EmployeeContactPrivateEmailUnbound])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([EmployeeContactPrivateEmailUnbound])")//[002] added
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeContactPrivateEmailList == null)
                        {
                            continue;
                        }

                        foreach (var employeeContactPrivateEmail in dataObject.EmployeeContactPrivateEmailList)
                        {
                            if (!string.IsNullOrEmpty(employeeContactPrivateEmail))
                            {

                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeContactPrivateEmail.ToString()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = employeeContactPrivateEmail;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse("CustomFilter([EmployeeContactPrivateEmailUnbound], ?)", employeeContactPrivateEmail);//[002] added
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Languages")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Languages = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Languages <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.Languages == null)
                        {
                            continue;
                        }
                        else if (dataObject.Languages != null)
                        {
                            if (dataObject.Languages.Contains("\n"))
                            {
                                string tempLanguages = dataObject.Languages;
                                for (int index = 0; index < tempLanguages.Length; index++)
                                {
                                    string empLanguages = tempLanguages.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empLanguages))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empLanguages;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Languages Like '%{0}%'", empLanguages));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempLanguages.Contains("\n"))
                                        tempLanguages = tempLanguages.Remove(0, empLanguages.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.Languages == dataObject.Languages).Select(slt => slt.Languages).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.Languages == dataObject.Languages).Select(slt => slt.Languages).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Languages Like '%{0}%'", FinalEmployeeList.Where(y => y.Languages == dataObject.Languages).Select(slt => slt.Languages).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmpJobCodes")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodes = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodes <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmpJobCodes == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmpJobCodes != null)
                        {
                            if (dataObject.EmpJobCodes.Contains("\n"))
                            {
                                string tempEmpJobCodes = dataObject.EmpJobCodes;
                                for (int index = 0; index < tempEmpJobCodes.Length; index++)
                                {
                                    string empJobCodes = tempEmpJobCodes.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empJobCodes))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empJobCodes;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmpJobCodes Like '%{0}%'", empJobCodes));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmpJobCodes.Contains("\n"))
                                        tempEmpJobCodes = tempEmpJobCodes.Remove(0, empJobCodes.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmpJobCodes == dataObject.EmpJobCodes).Select(slt => slt.EmpJobCodes).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmpJobCodes == dataObject.EmpJobCodes).Select(slt => slt.EmpJobCodes).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmpJobCodes Like '%{0}%'", FinalEmployeeList.Where(y => y.EmpJobCodes == dataObject.EmpJobCodes).Select(slt => slt.EmpJobCodes).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }

                else if (e.Column.FieldName == "EmployeeJobTitles")
                {
                    //[rdixit][GEOS2-4001][06.06.2023]
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty(EmployeeJobTitles)")//CriteriaOperator.Parse("EmployeeJobTitles = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Not IsNullOrEmpty(EmployeeJobTitles)") //CriteriaOperator.Parse("EmployeeJobTitles <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeJobTitles == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeJobTitles != null)
                        {
                            if (dataObject.EmployeeJobTitles.Contains("\n"))
                            {
                                string tempEmployeeJobTitles = dataObject.EmployeeJobTitles;
                                for (int index = 0; index < tempEmployeeJobTitles.Length; index++)
                                {
                                    string employeeJobTitles = tempEmployeeJobTitles.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeJobTitles))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeJobTitles;
                                        //customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeJobTitles Like '%{0}%'", employeeJobTitles));
                                        customComboBoxItem.EditValue = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("EmployeeJobTitles"), employeeJobTitles);
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeJobTitles.Contains("\n"))
                                        tempEmployeeJobTitles = tempEmployeeJobTitles.Remove(0, employeeJobTitles.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim();
                                    //customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeJobTitles Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim()));
                                    customComboBoxItem.EditValue = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("EmployeeJobTitles"), FinalEmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim());
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocDrLicense")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocDrLicense = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocDrLicense != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocDrLicense == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocDrLicense != null)
                        {
                            if (dataObject.EmployeeDocDrLicense.Contains("\n"))
                            {
                                string tempEmployeeDocDrLicense = dataObject.EmployeeDocDrLicense;
                                for (int index = 0; index < tempEmployeeDocDrLicense.Length; index++)
                                {
                                    string employeeDocDrLicense = tempEmployeeDocDrLicense.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocDrLicense))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocDrLicense;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocDrLicense Like '%{0}%'", employeeDocDrLicense));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocDrLicense.Contains("\n"))
                                        tempEmployeeDocDrLicense = tempEmployeeDocDrLicense.Remove(0, employeeDocDrLicense.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocDrLicense == dataObject.EmployeeDocDrLicense).Select(slt => slt.EmployeeDocDrLicense).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocDrLicense == dataObject.EmployeeDocDrLicense).Select(slt => slt.EmployeeDocDrLicense).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocDrLicense Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocDrLicense == dataObject.EmployeeDocDrLicense).Select(slt => slt.EmployeeDocDrLicense).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocPassport")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocPassport = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocPassport != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocPassport == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocPassport != null)
                        {
                            if (dataObject.EmployeeDocPassport.Contains("\n"))
                            {
                                string tempEmployeeDocPassport = dataObject.EmployeeDocPassport;
                                for (int index = 0; index < tempEmployeeDocPassport.Length; index++)
                                {
                                    string employeeDocPassport = tempEmployeeDocPassport.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocPassport))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocPassport;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocPassport Like '%{0}%'", employeeDocPassport));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocPassport.Contains("\n"))
                                        tempEmployeeDocPassport = tempEmployeeDocPassport.Remove(0, employeeDocPassport.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocPassport == dataObject.EmployeeDocPassport).Select(slt => slt.EmployeeDocPassport).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocPassport == dataObject.EmployeeDocPassport).Select(slt => slt.EmployeeDocPassport).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocPassport Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocPassport == dataObject.EmployeeDocPassport).Select(slt => slt.EmployeeDocPassport).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocClockId")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocClockId = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocClockId != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocClockId == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocClockId != null)
                        {
                            if (dataObject.EmployeeDocClockId.Contains("\n"))
                            {
                                string tempEmployeeDocClockId = dataObject.EmployeeDocClockId;
                                for (int index = 0; index < tempEmployeeDocClockId.Length; index++)
                                {
                                    string employeeDocClockId = tempEmployeeDocClockId.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocClockId))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocClockId;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocClockId Like '%{0}%'", employeeDocClockId));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocClockId.Contains("\n"))
                                        tempEmployeeDocClockId = tempEmployeeDocClockId.Remove(0, employeeDocClockId.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocClockId == dataObject.EmployeeDocClockId).Select(slt => slt.EmployeeDocClockId).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocClockId == dataObject.EmployeeDocClockId).Select(slt => slt.EmployeeDocClockId).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocClockId Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocClockId == dataObject.EmployeeDocClockId).Select(slt => slt.EmployeeDocClockId).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocPubIns")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocPubIns = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocPubIns != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocPubIns == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocPubIns != null)
                        {
                            if (dataObject.EmployeeDocPubIns.Contains("\n"))
                            {
                                string tempEmployeeDocDrLicense = dataObject.EmployeeDocPubIns;
                                for (int index = 0; index < tempEmployeeDocDrLicense.Length; index++)
                                {
                                    string employeeDocDrLicense = tempEmployeeDocDrLicense.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocDrLicense))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocDrLicense;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocPubIns Like '%{0}%'", employeeDocDrLicense));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocDrLicense.Contains("\n"))
                                        tempEmployeeDocDrLicense = tempEmployeeDocDrLicense.Remove(0, employeeDocDrLicense.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocPubIns == dataObject.EmployeeDocPubIns).Select(slt => slt.EmployeeDocPubIns).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocPubIns == dataObject.EmployeeDocPubIns).Select(slt => slt.EmployeeDocPubIns).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocPubIns Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocPubIns == dataObject.EmployeeDocPubIns).Select(slt => slt.EmployeeDocPubIns).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocPriIns")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocPriIns = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocPriIns != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocPriIns == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocPriIns != null)
                        {
                            if (dataObject.EmployeeDocPriIns.Contains("\n"))
                            {
                                string tempEmployeeDocPriIns = dataObject.EmployeeDocPriIns;
                                for (int index = 0; index < tempEmployeeDocPriIns.Length; index++)
                                {
                                    string employeeDocPriIns = tempEmployeeDocPriIns.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocPriIns))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocPriIns;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocPriIns Like '%{0}%'", employeeDocPriIns));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocPriIns.Contains("\n"))
                                        tempEmployeeDocPriIns = tempEmployeeDocPriIns.Remove(0, employeeDocPriIns.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocPriIns == dataObject.EmployeeDocPriIns).Select(slt => slt.EmployeeDocPriIns).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocPriIns == dataObject.EmployeeDocPriIns).Select(slt => slt.EmployeeDocPriIns).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocPriIns Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocPriIns == dataObject.EmployeeDocPriIns).Select(slt => slt.EmployeeDocPriIns).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocVisa")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocVisa = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocVisa != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocVisa == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocVisa != null)
                        {
                            if (dataObject.EmployeeDocVisa.Contains("\n"))
                            {
                                string tempEmployeeDocVisa = dataObject.EmployeeDocVisa;
                                for (int index = 0; index < tempEmployeeDocVisa.Length; index++)
                                {
                                    string employeeDocVisa = tempEmployeeDocVisa.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocVisa))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocVisa;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocVisa Like '%{0}%'", employeeDocVisa));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocVisa.Contains("\n"))
                                        tempEmployeeDocVisa = tempEmployeeDocVisa.Remove(0, employeeDocVisa.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocVisa == dataObject.EmployeeDocVisa).Select(slt => slt.EmployeeDocVisa).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocVisa == dataObject.EmployeeDocVisa).Select(slt => slt.EmployeeDocVisa).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocVisa Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocVisa == dataObject.EmployeeDocVisa).Select(slt => slt.EmployeeDocVisa).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocOther")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocOther = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocOther != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocOther == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocOther != null)
                        {
                            if (dataObject.EmployeeDocOther.Contains("\n"))
                            {
                                string tempEmployeeDocOther = dataObject.EmployeeDocOther;
                                for (int index = 0; index < tempEmployeeDocOther.Length; index++)
                                {
                                    string employeeDocOther = tempEmployeeDocOther.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocOther))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocOther;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocOther Like '%{0}%'", employeeDocOther));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocOther.Contains("\n"))
                                        tempEmployeeDocOther = tempEmployeeDocOther.Remove(0, employeeDocOther.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocOther == dataObject.EmployeeDocOther).Select(slt => slt.EmployeeDocOther).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocOther == dataObject.EmployeeDocOther).Select(slt => slt.EmployeeDocOther).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocOther Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocOther == dataObject.EmployeeDocOther).Select(slt => slt.EmployeeDocOther).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeDocNationalId")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocNationalId = null")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDocNationalId != null")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDocNationalId == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDocNationalId != null)
                        {
                            if (dataObject.EmployeeDocNationalId.Contains("\n"))
                            {
                                string tempEmployeeDocNationalId = dataObject.EmployeeDocNationalId;
                                for (int index = 0; index < tempEmployeeDocNationalId.Length; index++)
                                {
                                    string employeeDocNationalId = tempEmployeeDocNationalId.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeDocNationalId))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeDocNationalId;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocNationalId Like '%{0}%'", employeeDocNationalId));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDocNationalId.Contains("\n"))
                                        tempEmployeeDocNationalId = tempEmployeeDocNationalId.Remove(0, employeeDocNationalId.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDocNationalId == dataObject.EmployeeDocNationalId).Select(slt => slt.EmployeeDocNationalId).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDocNationalId == dataObject.EmployeeDocNationalId).Select(slt => slt.EmployeeDocNationalId).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDocNationalId Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDocNationalId == dataObject.EmployeeDocNationalId).Select(slt => slt.EmployeeDocNationalId).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmpJobCodeAbbreviations")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodeAbbreviations = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodeAbbreviations <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmpJobCodeAbbreviations == null || string.IsNullOrEmpty(dataObject.EmpJobCodeAbbreviations))
                        {
                            continue;
                        }
                        else if (dataObject.EmpJobCodeAbbreviations != null)
                        {
                            if (dataObject.EmpJobCodeAbbreviations.Contains("\n"))
                            {
                                string tempEmpJobCodeAbbreviations = dataObject.EmpJobCodeAbbreviations;
                                for (int index = 0; index < tempEmpJobCodeAbbreviations.Length; index++)
                                {
                                    string empJobCodeAbbreviations = tempEmpJobCodeAbbreviations.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empJobCodeAbbreviations))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empJobCodeAbbreviations;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmpJobCodeAbbreviations Like '%{0}%'", empJobCodeAbbreviations));


                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmpJobCodeAbbreviations.Contains("\n"))
                                        tempEmpJobCodeAbbreviations = tempEmpJobCodeAbbreviations.Remove(0, empJobCodeAbbreviations.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmpJobCodeAbbreviations == dataObject.EmpJobCodeAbbreviations).Select(slt => slt.EmpJobCodeAbbreviations).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmpJobCodeAbbreviations == dataObject.EmpJobCodeAbbreviations).Select(slt => slt.EmpJobCodeAbbreviations).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmpJobCodeAbbreviations Like '%{0}%'", FinalEmployeeList.Where(y => y.EmpJobCodeAbbreviations == dataObject.EmpJobCodeAbbreviations).Select(slt => slt.EmpJobCodeAbbreviations).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Company")
                {

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Company = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Company <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.Company == null)
                        {
                            continue;
                        }
                        else if (dataObject.Company != null)
                        {
                            if (dataObject.Company.Contains("\n"))
                            {
                                string tempCompany = dataObject.Company;
                                for (int index = 0; index < tempCompany.Length; index++)
                                {
                                    string company = tempCompany.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == company))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = company;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Company Like '%{0}%'", company));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempCompany.Contains("\n"))
                                        tempCompany = tempCompany.Remove(0, company.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.Company == dataObject.Company).Select(slt => slt.Company).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.Company == dataObject.Company).Select(slt => slt.Company).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Company Like '%{0}%'", FinalEmployeeList.Where(y => y.Company == dataObject.Company).Select(slt => slt.Company).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Organization")
                {

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Organization = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Organization <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {

                        if (dataObject.Organization == null)
                        {
                            continue;
                        }
                        else if (dataObject.Organization != null)
                        {
                            if (dataObject.Organization.Contains("\n"))
                            {
                                string tempOrganization = dataObject.Organization;
                                for (int index = 0; index < tempOrganization.Length; index++)
                                {
                                    string organization = tempOrganization.Split('\n').First();


                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == organization))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = organization;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Organization Like '%{0}%'", organization));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempOrganization.Contains("\n"))
                                        tempOrganization = tempOrganization.Remove(0, organization.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Organization Like '%{0}%'", FinalEmployeeList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }

                else if (e.Column.FieldName == "EmployeeDepartments")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDepartments = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeDepartments <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeDepartments == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeDepartments != null)
                        {
                            if (dataObject.EmployeeDepartments.Contains("\n"))
                            {
                                string tempEmployeeDepartments = dataObject.EmployeeDepartments;
                                for (int index = 0; index < tempEmployeeDepartments.Length; index++)
                                {
                                    string empEmployeeDepartments = tempEmployeeDepartments.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empEmployeeDepartments))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empEmployeeDepartments;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDepartments Like '%{0}%'", empEmployeeDepartments));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeDepartments.Contains("\n"))
                                        tempEmployeeDepartments = tempEmployeeDepartments.Remove(0, empEmployeeDepartments.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDepartments Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                //[003] added
                else if (e.Column.FieldName == "EmployeeShiftScheduleNames")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeShiftScheduleNames = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeShiftScheduleNames <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeShiftScheduleNames == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeShiftScheduleNames != null)
                        {
                            if (dataObject.EmployeeShiftScheduleNames.Contains("\n"))
                            {
                                string tempEmployeeShiftScheduleNames = dataObject.EmployeeShiftScheduleNames;
                                for (int index = 0; index < tempEmployeeShiftScheduleNames.Length; index++)
                                {
                                    string employeeShiftScheduleNames = tempEmployeeShiftScheduleNames.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeShiftScheduleNames))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeShiftScheduleNames;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeShiftScheduleNames Like '%{0}%'", employeeShiftScheduleNames));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeShiftScheduleNames.Contains("\n"))
                                        tempEmployeeShiftScheduleNames = tempEmployeeShiftScheduleNames.Remove(0, employeeShiftScheduleNames.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeShiftScheduleNames == dataObject.EmployeeShiftScheduleNames).Select(slt => slt.EmployeeShiftScheduleNames).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeShiftScheduleNames == dataObject.EmployeeShiftScheduleNames).Select(slt => slt.EmployeeShiftScheduleNames).FirstOrDefault().Trim();

                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeShiftScheduleNames Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeShiftScheduleNames == dataObject.EmployeeShiftScheduleNames).Select(slt => slt.EmployeeShiftScheduleNames).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }

                else if (e.Column.FieldName == "EmployeeShiftNames")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeShiftNames = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeShiftNames <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeeShiftNames == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeShiftNames != null)
                        {
                            if (dataObject.EmployeeShiftNames.Contains("\n"))
                            {
                                string employeeShiftNames = dataObject.EmployeeShiftNames;

                                for (int index = 0; index < employeeShiftNames.Length; index++)
                                {
                                    string EmployeeShiftNames = employeeShiftNames.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeShiftNames))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = EmployeeShiftNames;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeShiftNames Like '%{0}%'", EmployeeShiftNames));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (employeeShiftNames.Contains("\n"))
                                        employeeShiftNames = employeeShiftNames.Remove(0, EmployeeShiftNames.Length + 1);
                                    else
                                        break;
                                }
                            }


                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeeShiftNames == dataObject.EmployeeShiftNames).Select(slt => slt.EmployeeShiftNames).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeeShiftNames == dataObject.EmployeeShiftNames).Select(slt => slt.EmployeeShiftNames).FirstOrDefault().Trim();

                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeShiftNames Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeeShiftNames == dataObject.EmployeeShiftNames).Select(slt => slt.EmployeeShiftNames).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }

                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeePolyvalence")
                {
                    //[rdixit][GEOS2-4001][06.06.2023]
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse(string.Format("IsNullOrEmpty(EmployeePolyvalence)"))//CriteriaOperator.Parse("EmployeePolyvalence = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse(string.Format("Not IsNullOrEmpty(EmployeePolyvalence)"))//CriteriaOperator.Parse("EmployeePolyvalence <> ''")
                    });

                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (dataObject.EmployeePolyvalence == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeePolyvalence != null)
                        {
                            if (dataObject.EmployeePolyvalence.Contains("\n"))
                            {
                                string tempEmployeePolyvalence = dataObject.EmployeePolyvalence;
                                for (int index = 0; index < tempEmployeePolyvalence.Length; index++)
                                {
                                    string employeePolyvalence = tempEmployeePolyvalence.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeePolyvalence))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeePolyvalence;
                                        //customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeePolyvalence Like '%{0}%'", employeePolyvalence));
                                        customComboBoxItem.EditValue = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("EmployeePolyvalence"), employeePolyvalence);
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeePolyvalence.Contains("\n"))
                                        tempEmployeePolyvalence = tempEmployeePolyvalence.Remove(0, employeePolyvalence.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.EmployeePolyvalence == dataObject.EmployeePolyvalence).Select(slt => slt.EmployeePolyvalence).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.EmployeePolyvalence == dataObject.EmployeePolyvalence).Select(slt => slt.EmployeePolyvalence).FirstOrDefault().Trim();
                                    //customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeePolyvalence Like '%{0}%'", FinalEmployeeList.Where(y => y.EmployeePolyvalence == dataObject.EmployeePolyvalence).Select(slt => slt.EmployeePolyvalence).FirstOrDefault().Trim()));
                                    customComboBoxItem.EditValue = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("EmployeePolyvalence"), FinalEmployeeList.Where(y => y.EmployeePolyvalence == dataObject.EmployeePolyvalence).Select(slt => slt.EmployeePolyvalence).FirstOrDefault().Trim());
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 21 12 2023
                else if (e.Column.FieldName == "Country")
                {
                    foreach (var dataObject in FinalEmployeeList)
                    {
                        if (string.IsNullOrEmpty(dataObject.AddressCountry.Name))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.AddressCountry.Name))
                        {

                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == dataObject.AddressCountry.Name))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = dataObject.AddressCountry.Name;
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("AddressCountry.Name Like '%{0}%'", dataObject.AddressCountry.Name));
                                filterItems.Add(customComboBoxItem);
                            }
                            else
                                continue;
                        }
                        else
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == FinalEmployeeList.Where(y => y.AddressCountry.Name == dataObject.AddressCountry.Name).Select(slt => slt.AddressCountry.Name).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = FinalEmployeeList.Where(y => y.AddressCountry.Name == dataObject.AddressCountry.Name).Select(slt => slt.AddressCountry.Name).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("AddressCountry.Name Like '%{0}%'", FinalEmployeeList.Where(y => y.AddressCountry.Name == dataObject.AddressCountry.Name).Select(slt => slt.AddressCountry.Name).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }



                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][2019-04-04][GEOS2-220] Filter never disappear in employees grid 
        /// </summary>
        /// <param name="obj"></param>
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(EmployeeProfileGridSettingFilePath))
                {
                    //  ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(EmployeeProfileGridSettingFilePath);
                    //if (Properties.Settings.Default.IsDelete == false)
                    //{
                    //    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(EmployeeProfileGridSettingFilePath);
                    //    Properties.Settings.Default.IsDelete = true;
                    //    Properties.Settings.Default.Save();
                    //}
                    //else
                    //{
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(EmployeeProfileGridSettingFilePath);
                    //[001] Added
                    GridControl GridControlEmpolyeeDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView EmployeeProfileTableView = (TableView)GridControlEmpolyeeDetails.View;

                    if (EmployeeProfileTableView.SearchString != null)
                    {
                        EmployeeProfileTableView.SearchString = null;
                    }
                    //END
                }


                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(EmployeeProfileGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //[GEOS2-6499][rdixit][04.11.2024]
                foreach (GridColumn column in gridControl.Columns)
                {
                    bool isRestrictedColumn =
                        column.FieldName == "EmployeeCode" || column.FieldName == "FirstName" || column.FieldName == "LastName" ||
                        column.FieldName == "Gender.Value" || column.FieldName == "NativeName" || column.FieldName == "DisplayName" ||
                        column.FieldName == "EmployeeDocNationalId" || column.FieldName == "EmployeeContactCompanyMobiles" ||
                        column.FieldName == "EmployeeContactCompanySkypes" || column.FieldName == "EmployeeContactPrivateEmailUnbound" ||
                        column.FieldName == "AddressZipCode" || column.FieldName == "EmployeeContactPrivateMobiles" ||
                        column.FieldName == "EmployeeContactCompanyLandlines" || column.FieldName == "EmployeeContactEmailUnbound" ||
                        column.FieldName == "Nationality.Name" || column.FieldName == "AddressRegion" || column.FieldName == "MaritalStatus.Value" ||
                        column.FieldName == "DateOfBirth" || column.FieldName == "EmployeeStatus.Value" || column.FieldName == "AddressCity" ||
                        column.FieldName == "Company" || column.FieldName == "EmployeeDocPassport" || column.FieldName == "BirthDateUnbound" ||
                        column.FieldName == "Organization" || column.FieldName == "CompanyLocation.Alias";

                    if (GeosApplication.Instance.IsHRMManageEmployeeContactsPermission)
                    {
                        if (!isRestrictedColumn)
                        {
                            column.Visible = false;
                            column.Width = 0;
                            column.ShowInColumnChooser = isRestrictedColumn;
                        }
                        else
                        {
                            if (column.Visible)
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
                            }
                            if (!column.Visible)
                            {
                                visibleFalseCoulumn++;
                            }
                        }
                    }
                    else
                    {
                        // Only attach event handlers to columns that are visible
                        if (column.Visible)
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
                        }

                        // Increment count if the column is not visible
                        if (!column.Visible)
                        {
                            visibleFalseCoulumn++;
                        }
                    }
                }

                //if (visibleFalseCoulumn > 0)
                //{
                //    IsAccountColumnChooserVisible = true;
                //}
                //else
                //{
                //    IsAccountColumnChooserVisible = false;
                //}

                //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(AccountGridSettingFilePath);

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableView.SearchStringProperty)
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

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(EmployeeProfileGridSettingFilePath);
                }

                //if (column.Visible == false)
                //{
                //    IsAccountColumnChooserVisible = true;
                //}

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(EmployeeProfileGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][skale][2019-04-11][GEOS2-46] Wrong Age in employees
        /// Add for calculate  Employee Age in gridview using unboundExpression
        /// </summary>
        /// <param name="e"></param>
        private void CustomUnboundColumnDataAction(object e)
        {
            if (e == null) return;

            GridColumnDataEventArgs obj = e as GridColumnDataEventArgs;

            if (obj.Column.Name == "EmployeeAge")
            {

                DateTime birthdate = (DateTime)obj.GetListSourceFieldValue("BirthDate");

                if (birthdate.Date == DateTime.MinValue.Date)
                {
                    obj.Value = null;
                    return;
                }

                DateTime today = DateTime.Today;
                int employeeAge = today.Year - birthdate.Year;

                if (birthdate > today.AddYears(-employeeAge))
                {
                    employeeAge--;
                }

                obj.Value = string.Format("{0}", employeeAge.ToString());
            }
        }


        /// <summary>
        /// [002][avpawar][22-04-2020][GEOS2-2172][Length of service wrong sort]
        /// method for to Custom Sort the column's data
        /// </summary>
        /// <param name="e"></param>
        private void CustomSortColumnDataAction(object obj)
        {
            CustomColumnSortEventArgs e = obj as CustomColumnSortEventArgs;

            if (e.Column.FieldName == "LengthOfServiceString")
            {
                string FirstValue = Convert.ToString(e.Value1);
                string SecondValue = Convert.ToString(e.Value2);

                int FirstValueYear = Convert.ToInt32(Regex.Match(FirstValue, @"^.*?(?=y)").Value); // Convert.ToInt32(FirstValue.Substring(0, FirstValue.IndexOf("y")));
                int SecondValueYear = Convert.ToInt32(Regex.Match(SecondValue, @"^.*?(?=y)").Value); // Convert.ToInt32(SecondValue.Substring(0, SecondValue.IndexOf("y")));

                if (FirstValueYear > SecondValueYear)
                {
                    e.Result = 1;
                }
                else if (FirstValueYear < SecondValueYear)
                {
                    e.Result = -1;
                }
                else
                {
                    int FirstValueMonth = Convert.ToInt32(Regex.Match(FirstValue, @"y (.+?)M").Groups[1].Value);
                    int SecondValueMonth = Convert.ToInt32(Regex.Match(SecondValue, @"y (.+?)M").Groups[1].Value);

                    if (FirstValueMonth > SecondValueMonth)
                    {
                        e.Result = 1;
                    }
                    else if (FirstValueMonth < SecondValueMonth)
                    {
                        e.Result = -1;
                    }
                }

                e.Handled = true;
            }
        }

        #endregion
        /// <summary>
        /// [001][skale][2019-04-12][GEOS2-46] Wrong Age in employees
        /// [002][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [003][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// </summary>
        /// <param name="obj"></param>
        public void OpenAddNewEmployee(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method OpenAddNewEmployee()...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            AddNewEmployeeView addEmployee = new AddNewEmployeeView();
            AddNewEmployeeViewModel addNewEmployeeViewModel = new AddNewEmployeeViewModel();
            try
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

                EventHandler handle = delegate { addEmployee.Close(); };
                addNewEmployeeViewModel.RequestClose += handle;
                addNewEmployeeViewModel.Init();
                addEmployee.DataContext = addNewEmployeeViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEmployee.Owner = Window.GetWindow(ownerInfo);
                addEmployee.ShowDialogWindow();
                if (addNewEmployeeViewModel.IsSaved)
                {
                    addNewEmployeeViewModel.EmployeeDetail.FullAddress = addNewEmployeeViewModel.EmployeeDetail.AddressStreet;
                    if (!string.IsNullOrEmpty(addNewEmployeeViewModel.EmployeeDetail.AddressZipCode))
                        addNewEmployeeViewModel.EmployeeDetail.FullAddress = addNewEmployeeViewModel.EmployeeDetail.FullAddress + " - " + addNewEmployeeViewModel.EmployeeDetail.AddressZipCode;
                    if (!string.IsNullOrEmpty(addNewEmployeeViewModel.EmployeeDetail.AddressCity))
                        addNewEmployeeViewModel.EmployeeDetail.FullAddress = addNewEmployeeViewModel.EmployeeDetail.FullAddress + " - " + addNewEmployeeViewModel.EmployeeDetail.AddressCity;
                    if (!string.IsNullOrEmpty(addNewEmployeeViewModel.EmployeeDetail.AddressRegion))
                        addNewEmployeeViewModel.EmployeeDetail.FullAddress = addNewEmployeeViewModel.EmployeeDetail.FullAddress + " - " + addNewEmployeeViewModel.EmployeeDetail.AddressRegion;
                    if (!string.IsNullOrEmpty(addNewEmployeeViewModel.EmployeeDetail.AddressCountry.Name))
                        addNewEmployeeViewModel.EmployeeDetail.FullAddress = addNewEmployeeViewModel.EmployeeDetail.FullAddress + " - " + addNewEmployeeViewModel.EmployeeDetail.AddressCountry.Name;

                    addNewEmployeeViewModel.EmployeeDetail.EmployeeProfessionalContacts = new List<EmployeeContact>(addNewEmployeeViewModel.EmployeeProfessionalContactList);

                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContractSituations = new List<EmployeeContractSituation>(addNewEmployeeViewModel.EmployeeContractSituationList);
                    //[002] added 
                    addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalences = new List<EmployeePolyvalence>(addNewEmployeeViewModel.EmployeePolyvalenceList);
                    //END
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescriptions = new List<EmployeeJobDescription>(addNewEmployeeViewModel.EmployeeJobDescriptionList.Where(i => i.JobDescriptionStartDate.Value.Date < DateTime.Now.Date));

                    if (addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescriptions.Count > 0)
                    {
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescription = new EmployeeJobDescription();
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescription = addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescriptions[0];
                    }
                    else
                    {
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescriptions = new List<EmployeeJobDescription>(addNewEmployeeViewModel.EmployeeJobDescriptionList.Where(i => i.JobDescriptionStartDate.Value.Date > DateTime.Now.Date));
                    }

                    if (addNewEmployeeViewModel.EmployeeDetail.EmployeeProfessionalContacts.Count > 0)
                    {
                        EmployeeContact ProfContact = addNewEmployeeViewModel.EmployeeDetail.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88);
                        if (ProfContact != null)
                        {
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeContactEmail = ProfContact.EmployeeContactValue;
                        }
                        else
                        {
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeContactEmail = string.Empty;
                        }
                        ProfContact = addNewEmployeeViewModel.EmployeeDetail.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90);
                        if (ProfContact != null)
                        {
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeContactMobile = ProfContact.EmployeeContactValue;
                        }
                        else
                        {
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeContactMobile = string.Empty;
                        }
                    }
                    List<string> EmpJobDescriptions = addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescriptions.Where(x => (x.JobDescriptionEndDate == null || x.JobDescriptionEndDate > DateTime.Now) && x.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).Select(x => x.JobDescription.JobDescriptionCode).ToList();
                    addNewEmployeeViewModel.EmployeeDetail.EmpJobCodes = string.Join("\n", EmpJobDescriptions);
                    List<string> EmpAbbreviation = addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescriptions.Where(x => (x.JobDescriptionEndDate == null || x.JobDescriptionEndDate > DateTime.Now) && x.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).Select(x => x.JobDescription.Abbreviation).ToList();
                    addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = string.Join("\n", EmpAbbreviation);
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactPrivateEmailList = addNewEmployeeViewModel.EmployeeContactList.Where(x => x.EmployeeContactIdType == 88).Select(x => x.EmployeeContactValue).ToList();
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactPrivateMobileList = addNewEmployeeViewModel.EmployeeContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToList();
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactCompanyEmailList = addNewEmployeeViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 88).Select(x => x.EmployeeContactValue).ToList();
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactCompanyMobileList = addNewEmployeeViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToList();
                    TempEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(addNewEmployeeViewModel.EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).ToList());

                    //[003] added
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeShifts = new List<EmployeeShift>(addNewEmployeeViewModel.EmployeeShiftList);

                    if (addNewEmployeeViewModel.EmployeeDetail.EmployeeShifts.Count > 0 || addNewEmployeeViewModel.EmployeeDetail.EmployeeShifts != null)
                    {
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeShiftNames = String.Join("\n", addNewEmployeeViewModel.EmployeeDetail.EmployeeShifts.Select(x => x.CompanyShift.Name).ToArray());
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeShiftScheduleNames = String.Join("\n", addNewEmployeeViewModel.EmployeeDetail.EmployeeShifts.Select(x => x.CompanyShift.CompanySchedule.Name).Distinct().ToArray());
                    }
                    else
                    {
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeShiftNames = null;
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeShiftScheduleNames = null;
                    }

                    if (TempEmployeeJobDescriptionList.Count == 0)
                    {
                        if (addNewEmployeeViewModel.EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                        {
                            TempEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(addNewEmployeeViewModel.EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).ToList());

                            if (TempEmployeeJobDescriptionList.Count == 1)
                            {

                                addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                                addNewEmployeeViewModel.EmployeeDetail.EmpJobCodes = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode));
                                addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Abbreviation));
                                addNewEmployeeViewModel.EmployeeDetail.EmployeeDepartments = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Department.DepartmentName));
                            }
                            else
                            {
                                addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = AddPlantAliasToJobTitle(addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles, TempEmployeeJobDescriptionList.ToList());
                                addNewEmployeeViewModel.EmployeeDetail.EmpJobCodes = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode));
                                addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Abbreviation));
                                addNewEmployeeViewModel.EmployeeDetail.EmployeeDepartments = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Department.DepartmentName));
                            }

                        }
                        else
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = string.Empty;
                    }
                    else
                    {
                        if (TempEmployeeJobDescriptionList.Count == 1)
                        {

                            addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                            addNewEmployeeViewModel.EmployeeDetail.EmpJobCodes = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode));
                            addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Abbreviation));
                        }
                        else
                        {
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                            addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = AddPlantAliasToJobTitle(addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles, TempEmployeeJobDescriptionList.ToList());
                            addNewEmployeeViewModel.EmployeeDetail.EmpJobCodes = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode));
                            addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Abbreviation));
                        }
                    }

                    //[pjadhav][Geos-2618][10/19/2022] Add
                    if (TempEmployeePolyvalenceList != null)    //chitra[cgirigosavi][Geos2-4773][26/09/2023
                    {
                        if (TempEmployeePolyvalenceList.Count == 0)
                        {
                            if (addNewEmployeeViewModel.EmployeePolyvalenceList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                            {
                                TempEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(addNewEmployeeViewModel.EmployeePolyvalenceList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).ToList());

                                if (TempEmployeePolyvalenceList.Count == 1)
                                {

                                    addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalenceDescription = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                                    addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.Abbreviation));
                                }
                                else
                                {


                                    addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalenceDescription = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.PolyvalenceUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                    addNewEmployeeViewModel.EmployeeDetail.EmplyeePolyvalence.JobDescription.JobDescriptionTitle = AddEmployeePolyvalence(addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalenceDescription, TempEmployeePolyvalenceList.ToList());
                                    addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.Abbreviation));
                                }

                            }
                            else
                                addNewEmployeeViewModel.EmployeeDetail.EmplyeePolyvalence.JobDescription.JobDescriptionTitle = string.Empty;
                        }
                        else
                        {
                            if (TempEmployeePolyvalenceList.Count == 1)
                            {

                                addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalenceDescription = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                                addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.Abbreviation));

                            }
                            else
                            {
                                addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalenceDescription = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.PolyvalenceUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                                addNewEmployeeViewModel.EmployeeDetail.EmplyeePolyvalence.JobDescription.JobDescriptionTitle = AddEmployeePolyvalence(addNewEmployeeViewModel.EmployeeDetail.EmployeePolyvalenceDescription, TempEmployeePolyvalenceList.ToList());
                                addNewEmployeeViewModel.EmployeeDetail.EmpJobCodeAbbreviations = String.Join("\n", TempEmployeePolyvalenceList.Select(x => x.JobDescription.Abbreviation));
                            }
                        }
                    }

                    //  [001] Added            
                    if (addNewEmployeeViewModel.EmployeeDetail.DateOfBirth != null)
                        addNewEmployeeViewModel.EmployeeDetail.BirthDate = (DateTime)addNewEmployeeViewModel.EmployeeDetail.DateOfBirth;
                    else
                        addNewEmployeeViewModel.EmployeeDetail.BirthDate = DateTime.MinValue;
                    //End

                    //if (addNewEmployeeViewModel.EmployeeDetail.EmployeeJobDescription.JobDescriptionUsage == 100)
                    //{
                    //    addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", addNewEmployeeViewModel.EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                    //}
                    //else
                    //{
                    //    addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", addNewEmployeeViewModel.EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + "(" + x.JobDescriptionUsage + "%)").ToArray());
                    //}

                    // addNewEmployeeViewModel.EmployeeDetail.EmployeeJobTitles = String.Join("\n", addNewEmployeeViewModel.EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + "(" + x.JobDescriptionUsage + "%)").ToArray());

                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactCompanyMobiles = String.Join("\n", addNewEmployeeViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactCompanySkypes = String.Join("\n", addNewEmployeeViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 87).Select(x => x.EmployeeContactValue).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactCompanyLandlines = String.Join("\n", addNewEmployeeViewModel.EmployeeProfessionalContactList.Where(x => x.EmployeeContactIdType == 89).Select(x => x.EmployeeContactValue).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactTranings = String.Join("\n", addNewEmployeeViewModel.EmployeeProfessionalEducationList.Where(x => x.IdType == 120).Select(x => x.Name).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeContactPrivateMobiles = String.Join("\n", addNewEmployeeViewModel.EmployeeContactList.Where(x => x.EmployeeContactIdType == 90).Select(x => x.EmployeeContactValue).ToArray());

                    addNewEmployeeViewModel.EmployeeDetail.Languages = String.Join("\n", addNewEmployeeViewModel.EmployeeLanguageList.Select(x => x.Language.Value).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeJobCodes = addNewEmployeeViewModel.EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode).ToList();

                    if (addNewEmployeeViewModel.EmployeeDetail.EmployeeDocuments.Count > 0)
                    {
                        addNewEmployeeViewModel.EmployeeDetail.EmployeeDocument = new EmployeeDocument();
                        // int Index = addNewEmployeeViewModel.EmployeeDetail.EmployeeDocuments.IndexOf(addNewEmployeeViewModel.EmployeeDetail.EmployeeDocuments.Single(i => i.EmployeeDocumentIdType == 80));

                        addNewEmployeeViewModel.EmployeeDetail.EmployeeDocument = addNewEmployeeViewModel.EmployeeDetail.EmployeeDocuments.FirstOrDefault(i => i.EmployeeDocumentIdType == 80);

                    }

                    // Code Comment for [GEOS2 - 1714] Adjust shift developments(improvements)
                    //addNewEmployeeViewModel.EmployeeDetail.CompanyShift = addNewEmployeeViewModel.EmployeeDetail.CompanyShift;
                    //addNewEmployeeViewModel.EmployeeDetail.CompanyShift.CompanySchedule.Name = addNewEmployeeViewModel.EmployeeDetail.CompanyShift.CompanySchedule.Name;


                    addNewEmployeeViewModel.EmployeeDetail.LengthOfServiceString = addNewEmployeeViewModel.LengthOfService;

                    DateTime date = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), DateTime.Now.Month, DateTime.Now.Day);

                    if (addNewEmployeeViewModel.EmployeeDetail.EmployeeContractSituations != null && addNewEmployeeViewModel.EmployeeDetail.EmployeeContractSituations.Count > 0)
                    {
                        List<EmployeeContractSituation> tempList = addNewEmployeeViewModel.EmployeeDetail.EmployeeContractSituations.Where(x => x.ContractSituationEndDate >= date || x.ContractSituationEndDate == null).ToList();
                        tempList = tempList.OrderByDescending(row => row.ContractSituationEndDate ?? DateTime.MinValue).ToList();

                        foreach (EmployeeContractSituation item in tempList)
                        {
                            if (date >= item.ContractSituationStartDate && date <= item.ContractSituationEndDate)
                            {
                                addNewEmployeeViewModel.EmployeeDetail.ContractSituation = item.ContractSituation;
                                break;
                            }
                            else if (date >= item.ContractSituationStartDate && item.ContractSituationEndDate == null)
                            {
                                addNewEmployeeViewModel.EmployeeDetail.ContractSituation = item.ContractSituation;
                                break;
                            }
                            else
                            {
                                addNewEmployeeViewModel.EmployeeDetail.ContractSituation = null;
                            }
                        }

                    }

                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocPassport = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 81).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocClockId = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 193).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocDrLicense = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 82).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocOther = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 153).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocPriIns = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 119).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocPubIns = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 118).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocVisa = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 152).Select(y => y.EmployeeDocumentNumber).ToArray());
                    addNewEmployeeViewModel.EmployeeDetail.EmployeeDocNationalId = String.Join("\n", addNewEmployeeViewModel.EmployeeDocumentList.Where(x => x.EmployeeDocumentIdType == 80).Select(y => y.EmployeeDocumentNumber).ToArray());

                    //[Sprint-62][09/05/2019][sdesai]----Add new fields Company, Organization, Location in employees grid
                    addNewEmployeeViewModel.EmployeeDetail.CompanyLocation = addNewEmployeeViewModel.LocationList[addNewEmployeeViewModel.SelectedLocationIndex];

                    List<EmployeeContractSituation> tempEmployeeContractSituation = addNewEmployeeViewModel.EmployeeContractSituationList.Where(x => x.Company != null && (x.ContractSituationEndDate == null || x.ContractSituationEndDate >= GeosApplication.Instance.ServerDateTime)).ToList();
                    if (tempEmployeeContractSituation != null && tempEmployeeContractSituation.Count > 0)
                    {
                        if (tempEmployeeContractSituation.Count > 1)
                        {
                            EmployeeContractSituation tempContract = tempEmployeeContractSituation.FirstOrDefault(x => x.ContractSituationEndDate != null);
                            if (tempContract != null)
                                addNewEmployeeViewModel.EmployeeDetail.Company = tempContract.Company.Alias;
                        }
                        else if (tempEmployeeContractSituation.Count == 1)
                        {
                            addNewEmployeeViewModel.EmployeeDetail.Company = tempEmployeeContractSituation[0].Company.Alias;
                        }
                    }
                    //else
                    //    addNewEmployeeViewModel.EmployeeDetail.Company = addNewEmployeeViewModel.LocationList[addNewEmployeeViewModel.SelectedLocationIndex].Alias;
                    else
                    {
                        List<EmployeeContractSituation> tempEmployeeContract = addNewEmployeeViewModel.EmployeeContractSituationList.ToList(); ;
                        tempEmployeeContract = tempEmployeeContract.OrderByDescending(row => row.ContractSituationEndDate ?? DateTime.MinValue).ToList();
                        addNewEmployeeViewModel.EmployeeDetail.Company = tempEmployeeContract[0].Company.Alias;
                    }

                    addNewEmployeeViewModel.EmployeeDetail.Organization = String.Join("\n", addNewEmployeeViewModel.EmployeeJobDescriptionList.Select(x => x.Company.Alias).Distinct().ToList().ToArray());

                    FinalEmployeeList.Add(addNewEmployeeViewModel.EmployeeDetail);
                    SelectedGridRow = addNewEmployeeViewModel.EmployeeDetail;
                }

                IsBusy = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenAddNewEmployee()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAddNewEmployee()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for open MailTo in Outlook for send Email. 
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                isBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                isBusy = false;
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][SP-63][skale][21-5-2019][GEOS2-273]Add new fields Company, Organization,Location in employees grid
        /// [002][SP-65][skale][11-06-2019][GEOS2-1556]Grid data reflection problems
        /// [003][SP-66][skale][27-06-2019][GEOS2-1589]THRM - Bug: Employees List filter doesn't work
        /// [004][smazhar][13-08-2020][GEOS2-2538]Employee display name not appear in Edit Employee and grid
        /// [005][cpatil][27-07-2021][GEOS2-2333]New columns in HRM
        /// [006][cpatil][21-03-2022][GEOS2-3634]HRM - Allow add future Job descriptions [#ERF97] - 3
        /// </summary>
        /// <param name="obj"></param>
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

            MyFilterString = string.Empty;

            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)//[001] added
            {
                try
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList(); //[001] added
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    #region Service Comments
                    /*FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2033(plantOwnersIds, HrmCommon.Instance.SelectedPeriod)); */    //[003] added
                    //[004] [005] [006]
                    //service method changed GetAllEmployeesByIdCompany_V2330 to GetAllEmployeesByIdCompany_V2360 [GEOS2-4003][sshegaonkar][11.01.2023]  
                    //  FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2250(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[Sudhir.Jangra][GEOS2-4686]
                    //service method changed GetAllEmployeesByIdCompany_V2410 to GetAllEmployeesByIdCompany_V2420 [GEOS2-2466][rdixit][07.08.2023]
                    
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 19 12 2023
                    //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                    //[rdixit][27.11.2024][GEOS2-6661]
                    #endregion
                   // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2590(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[Sudhir.Jangra][GEOS2-5656]
                    FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    CalculateLengthOfService();
                    // SetHireDateAndLenghtOfService(FinalEmployeeList);
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
            else
            {
                FinalEmployeeList = new ObservableCollection<Employee>();
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// [001][SP-63][skale][21-5-2019][GEOS2-273]Add new fields Company, Organization,Location in employees grid
        /// [002][SP-65][skale][11-06-2019][GEOS2-1656]Grid data reflection problems
        /// [003][SP-66][skale][27-06-2019][GEOS2-1589]THRM - Bug: Employees List filter doesn't work
        /// [004][smazhar][13-08-2020][GEOS2-2538]Employee display name not appear in Edit Employee and grid
        /// [005][cpatil][27-07-2021][GEOS2-2333]New columns in HRM
        /// [006][cpatil][21-03-2022][GEOS2-3634]HRM - Allow add future Job descriptions [#ERF97] - 3
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
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

            MyFilterString = string.Empty;//[002] added

            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)//[001] added
            {
                try
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList(); //[001] added
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    #region Service Comments
                    /*FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2033(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));*/ //[003]added
                    //[004] [005] [006]
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2250(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //service method changed GetAllEmployeesByIdCompany_V2330 to GetAllEmployeesByIdCompany_V2360 [GEOS2-4003][sshegaonkar][11.01.2023]
                    //[Sudhir.Jangra][GEOS2-4686]
                    //service method changed GetAllEmployeesByIdCompany_V2410 to GetAllEmployeesByIdCompany_V2420 [GEOS2-2466][rdixit][07.08.2023]
                    
                    //FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 19 12 2023
                    //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                    //[rdixit][GEOS2-6661][27.11.2024]
                    #endregion
                   // FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2590(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[Sudhir.Jangra][GEOS2-5656]
                    FinalEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    CalculateLengthOfService();
                    //SetHireDateAndLenghtOfService(FinalEmployeeList);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in SelectedYearChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in SelectedYearChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
            }
            else
            {
                FinalEmployeeList = new ObservableCollection<Employee>();
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //private void FillContactListByPlant()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillContactListByPlant ...", category: Category.Info, priority: Priority.Low);
        //        if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
        //        {
        //            List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
        //            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
        //            FinalEmployeeList = new ObservableCollection<Employee>(HrmService.i(plantOwnersIds));
        //        }
        //        else
        //        {
        //            FinalEmployeeList = new ObservableCollection<Employee>();
        //        }
        //        GeosApplication.Instance.Logger.Log("Method FillContactListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillContactListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //}
        private void OpenEmployeeDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                //TableView tableView = (TableView)obj;
                //Employee empDocument = (Employee)tableView.FocusedRow;
                string EmpCode = string.Empty;
                EmployeeDocumentView employeeEducationDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel employeeEducationDocumentViewModel = new EmployeeDocumentViewModel();
                employeeEducationDocumentViewModel.OpenPdfByEmployeeCode(EmpCode, obj, true);
                employeeEducationDocumentView.DataContext = employeeEducationDocumentViewModel;
                employeeEducationDocumentView.Show();
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //employeeEducationQualification.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //CustomMessageBox.Show(string.Format("Could not find file '{0}'.", employeeEducationQualification.QualificationFileName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetUserPermission()
        {
            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    AddShiftEnabled = true;
                    break;

                case PermissionManagement.Admin:
                    AddShiftEnabled = true;
                    break;

                case PermissionManagement.PlantViewer:
                    AddShiftEnabled = false;
                    break;

                case PermissionManagement.GlobalViewer:
                    AddShiftEnabled = false;
                    break;

                default:
                    AddShiftEnabled = false;
                    break;
            }
            //[rdixit][GEOS2-6979][02.04.2025]
            if (GeosApplication.Instance.IsHRMTravelManagerPermission || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
            {
                AddShiftEnabled = false;
            }
        }

        // [pjadhav][GEOS2-2618][10/19/2022]
        private string AddEmployeePolyvalence(string empPolyvalence, List<EmployeePolyvalence> emplyeePolyvalence)
        {
            if (!string.IsNullOrEmpty(empPolyvalence))
            {
                List<String> result = new List<String>(empPolyvalence.Split(new string[] { "\n" }, StringSplitOptions.None));
                if (result.Count() > 1)
                {
                    string matchString = null;
                    bool isPolyvalence = false;
                    matchString = result[0].Substring(result[0].IndexOf("[") + 1, ((result[0].IndexOf("]")) - (result[0].IndexOf("[") + 1)));
                    if (string.IsNullOrEmpty(matchString))
                    {
                        isPolyvalence = false;
                    }
                    else
                    {
                        matchString = " [" + matchString + "]";
                        foreach (string item in result)
                        {
                            if (!item.Contains(matchString))
                            {
                                isPolyvalence = true;
                                break;
                            }
                        }
                    }

                    if (isPolyvalence == false)
                    {
                        empPolyvalence = String.Join("\n", emplyeePolyvalence.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.PolyvalenceUsage + "%)").ToArray());
                    }

                }

            }
            return empPolyvalence;
        }


        //[rdixit][27.11.2024][GEOS2-6661]
        private string CalculateLengthOfService(List<EmployeeContractSituation> employeeContractSituations)
        {
            if (employeeContractSituations == null || employeeContractSituations.Count == 0)
                return "0y 0M";

            int totalMonths = 0;

            try
            {
                foreach (var situation in employeeContractSituations)
                {
                    if (situation.ContractSituationStartDate.HasValue)
                    {
                        LocalDate start = new LocalDate(
                            situation.ContractSituationStartDate.Value.Year,
                            situation.ContractSituationStartDate.Value.Month,
                            situation.ContractSituationStartDate.Value.Day
                        );

                        LocalDate end = situation.ContractSituationEndDate.HasValue && situation.ContractSituationEndDate <= DateTime.Now
                            ? new LocalDate(
                                situation.ContractSituationEndDate.Value.Year,
                                situation.ContractSituationEndDate.Value.Month,
                                situation.ContractSituationEndDate.Value.Day
                            ).PlusDays(1)
                            : new LocalDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).PlusDays(1);

                        if (start < end) // Only consider periods in the past
                        {
                            Period period = Period.Between(start, end, PeriodUnits.Months);
                            totalMonths += period.Months;
                        }
                    }
                }

                int years = totalMonths / 12;
                int months = totalMonths % 12;

                return $"{years}y {months}M";
            }
            catch
            {
                // Handle exceptions properly, e.g., logging
                throw;
            }
        }

        //[rdixit][GEOS2-4686][27.07.2023]
        //private string CalculateLengthOfService(List<EmployeeContractSituation> EmployeeContractSituations)
        //{
        //    string lengthOfService = "0";
        //    try
        //    {

        //        if (EmployeeContractSituations.Count > 0)
        //        {
        //            int month = 0;
        //            int year = 0;
        //            foreach (EmployeeContractSituation employeeContractSituation in EmployeeContractSituations)
        //            {
        //                if (employeeContractSituation.ContractSituationStartDate.Value.Date >= DateTime.Now.Date)
        //                {
        //                    lengthOfService += 0;
        //                }
        //                else if (employeeContractSituation.ContractSituationEndDate == null)
        //                {

        //                    LocalDate start = new LocalDate(employeeContractSituation.ContractSituationStartDate.Value.Year, employeeContractSituation.ContractSituationStartDate.Value.Month, employeeContractSituation.ContractSituationStartDate.Value.Day);
        //                    LocalDate end = new LocalDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).PlusDays(1);
        //                    Period period = Period.Between(start, end, PeriodUnits.Months);
        //                    month = month + period.Months;
        //                }
        //                else
        //                {
        //                    if (employeeContractSituation.ContractSituationEndDate > DateTime.Now.Date)
        //                    {
        //                        LocalDate start = new LocalDate(employeeContractSituation.ContractSituationStartDate.Value.Year, employeeContractSituation.ContractSituationStartDate.Value.Month, employeeContractSituation.ContractSituationStartDate.Value.Day);
        //                        LocalDate end = new LocalDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).PlusDays(1);
        //                        Period period = Period.Between(start, end, PeriodUnits.Months);
        //                        month = month + period.Months;
        //                    }
        //                    else
        //                    {
        //                        LocalDate start = new LocalDate(employeeContractSituation.ContractSituationStartDate.Value.Year, employeeContractSituation.ContractSituationStartDate.Value.Month, employeeContractSituation.ContractSituationStartDate.Value.Day);
        //                        LocalDate end = new LocalDate(employeeContractSituation.ContractSituationEndDate.Value.Year, employeeContractSituation.ContractSituationEndDate.Value.Month, employeeContractSituation.ContractSituationEndDate.Value.Day).PlusDays(1);
        //                        Period period = Period.Between(start, end, PeriodUnits.Months);
        //                        month = month + period.Months;
        //                    }
        //                }

        //            }

        //            if (month >= 12)
        //            {
        //                year += month / 12;
        //                month = month % 12;
        //                year = year + month / 12;
        //            }

        //            lengthOfService = Convert.ToString(year) + "y  " + Convert.ToString(month) + "M";
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return lengthOfService;
        //}

        
    }

}
