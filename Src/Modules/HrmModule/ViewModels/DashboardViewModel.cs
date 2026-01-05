using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.Scheduling.Visual;
using DevExpress.XtraScheduler;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //HRM M050-22	Display company name in daily event holidays [adadibathina]
        //[M051-08][Year selection is not saved after change section][adadibathina]

        #endregion

        #region Services  
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion // End Services

        #region public ICommand

        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand SelectedYearChangedCommand { get; private set; }
        public ICommand GroupBoxExpandedCommand { get; set; }
        public ICommand DisableAppointmentCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand DeleteAppointmentCommand { get; set; }
        public ICommand scheduler_VisibleIntervalsChangedCommand { get; set; }
        #endregion

        #region Declaration
        //[M051-08]
        //  private long selectedPeriod;
        private int todaysTotalAppointment;
        private ObservableCollection<Department> employeesHeadCountByDepartment;
        private ObservableCollection<Employee> upcomingBirthdayOfEmployee;
        private ObservableCollection<EmployeeLeave> upcomingEmployeeLeaves;
        private ObservableCollection<LookupValue> employeeCountByGender;
        private ObservableCollection<ContractSituation> employeeCountByContract;
        private ObservableCollection<LookupValue> employeeCountbyDepartmentArea;
        private ObservableCollection<JobDescription> employeeCountByJobPosition;
        private ChartControl chartControl;
        private ObservableCollection<CompanyHoliday> companyHolidays;
        private ObservableCollection<EmployeeLeave> employeeLeaves;
        private ObservableCollection<EmployeeDocument> employeeDocuments;
        private ObservableCollection<Employee> employeesWithExpiryDate;
        private ObservableCollection<CompanyLeave> companyLeaves;
        private ObservableCollection<EmployeeContractSituation> employeeContractSituations;
        private ObservableCollection<Employee> employeesAnniversary;
        private ObservableCollection<Company> totalEmployeeList;
        private ObservableCollection<Department> employeesLengthOfServiceByDepartment;

        private ObservableCollection<LabelHelper> labelItems;
        private ObservableCollection<UI.Helper.Appointment> appointmentItems;
        private ObservableCollection<UI.Helper.Appointment> filterAppointments;
        private ObservableCollection<LookupValue> holidayList;

        private Visibility todaysTotalAppointmentVisibility = Visibility.Collapsed;
        private List<string> appointmentType;
        private List<object> selectedAppointmentType;
        private Employee employeeDetails;
        private uint totalEmployeeCount;
        private ObservableCollection<EmployeeTrips> travelList;
        #endregion

        #region Properties

        public Employee EmployeeDetails
        {
            get
            {
                return employeeDetails;
            }

            set
            {
                employeeDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDetails"));
            }
        }
        public List<object> SelectedAppointmentType
        {
            get
            {
                return selectedAppointmentType;
            }

            set
            {
                selectedAppointmentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppointmentType"));

                ObservableCollection<UI.Helper.Appointment> TempFilterAppointments = new ObservableCollection<UI.Helper.Appointment>();

                if (selectedAppointmentType != null && selectedAppointmentType.Count > 0)
                {
                    foreach (var item in selectedAppointmentType)
                    {
                        if (AppointmentItems != null)
                        {
                            List<UI.Helper.Appointment> tm = AppointmentItems.Where(app => app.Location.Contains(item.ToString())).Select(app => app).ToList();
                            if (tm != null && tm.Count > 0)
                                TempFilterAppointments.AddRange(tm);
                        }
                        // FilterAppointments
                    }
                }
                else
                {
                    TempFilterAppointments = new ObservableCollection<UI.Helper.Appointment>();
                }

                FilterAppointments = new ObservableCollection<UI.Helper.Appointment>(TempFilterAppointments);
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

        public ObservableCollection<Department> EmployeesHeadCountByDepartment
        {
            get { return employeesHeadCountByDepartment; }
            set
            {
                employeesHeadCountByDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeesHeadCountByDepartment"));
            }
        }

        public ObservableCollection<Employee> UpcomingBirthdayOfEmployee
        {
            get { return upcomingBirthdayOfEmployee; }
            set
            {
                upcomingBirthdayOfEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpcomingBirthdayOfEmployee"));
            }
        }

        public ObservableCollection<EmployeeLeave> UpcomingEmployeeLeaves
        {
            get { return upcomingEmployeeLeaves; }
            set
            {
                upcomingEmployeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpcomingEmployeeLeaves"));
            }
        }

        public ObservableCollection<LookupValue> EmployeeCountByGender
        {
            get { return employeeCountByGender; }
            set
            {
                employeeCountByGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCountByGender"));
            }
        }

        public ObservableCollection<ContractSituation> EmployeeCountByContract
        {
            get { return employeeCountByContract; }
            set
            {
                employeeCountByContract = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCountByContract"));
            }
        }

        public ObservableCollection<LookupValue> EmployeeCountbyDepartmentArea
        {
            get { return employeeCountbyDepartmentArea; }
            set
            {
                employeeCountbyDepartmentArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCountbyDepartmentArea"));
            }
        }

        public ObservableCollection<JobDescription> EmployeeCountByJobPosition
        {
            get { return employeeCountByJobPosition; }
            set
            {
                employeeCountByJobPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCountByJobPosition"));
            }
        }

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

        public ObservableCollection<LabelHelper> LabelItems
        {
            get
            {
                return labelItems;
            }

            set
            {
                labelItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelItems"));
            }
        }

        public ObservableCollection<UI.Helper.Appointment> AppointmentItems
        {
            get
            {
                return appointmentItems;
            }

            set
            {
                appointmentItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentItems"));
            }
        }

        public ObservableCollection<LookupValue> HolidayList
        {
            get
            {
                return holidayList;
            }

            set
            {
                holidayList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HolidayList"));
            }
        }
        public ObservableCollection<EmployeeTrips> TravelList
        {
            get
            {
                return travelList;
            }

            set
            {
                travelList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravelList"));
            }
        }
        public ObservableCollection<EmployeeLeave> EmployeeLeaves
        {
            get
            {
                return employeeLeaves;
            }

            set
            {
                employeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
        }

        public ObservableCollection<EmployeeDocument> EmployeeDocuments
        {
            get
            {
                return employeeDocuments;
            }

            set
            {
                employeeDocuments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDocuments"));
            }
        }

        public ObservableCollection<Employee> EmployeesWithExpiryDate
        {
            get
            {
                return employeesWithExpiryDate;
            }

            set
            {
                employeesWithExpiryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeesWithExpiryDate"));
            }
        }

        public ObservableCollection<CompanyLeave> CompanyLeaves
        {
            get
            {
                return companyLeaves;
            }

            set
            {
                companyLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyLeaves"));
            }
        }

        public int TodaysTotalAppointment
        {
            get
            {
                return todaysTotalAppointment;
            }

            set
            {
                todaysTotalAppointment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TodaysTotalAppointment"));
            }
        }

        public Visibility TodaysTotalAppointmentVisibility
        {
            get { return todaysTotalAppointmentVisibility; }
            set
            {
                todaysTotalAppointmentVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TodaysTotalAppointmentVisibility"));
            }
        }

        public ObservableCollection<EmployeeContractSituation> EmployeeContractSituations
        {
            get
            {
                return employeeContractSituations;
            }

            set
            {
                employeeContractSituations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContractSituations"));
            }
        }

        public ObservableCollection<Employee> EmployeesAnniversary
        {
            get
            {
                return employeesAnniversary;
            }

            set
            {
                employeesAnniversary = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeesAnniversary"));
            }
        }

        public ObservableCollection<Company> TotalEmployeeList
        {
            get
            {
                return totalEmployeeList;
            }

            set
            {
                totalEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalEmployeeList"));
            }
        }

        public ObservableCollection<Department> EmployeesLengthOfServiceByDepartment
        {
            get
            {
                return employeesLengthOfServiceByDepartment;
            }

            set
            {
                employeesLengthOfServiceByDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeesLengthOfServiceByDepartment"));
            }
        }

        public List<string> AppointmentType
        {
            get
            {
                return appointmentType;
            }

            set
            {
                appointmentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentType"));
            }
        }

        public ObservableCollection<UI.Helper.Appointment> FilterAppointments
        {
            get
            {
                return filterAppointments;
            }

            set
            {
                filterAppointments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterAppointments"));
            }
        }

        public uint TotalEmployeeCount
        {
            get
            {
                return totalEmployeeCount;
            }

            set
            {
                totalEmployeeCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalEmployeeCount"));
            }
        }
        public int Font { get; set; }
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


        //static DashboardViewModel()
        //{
        //    //ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(SchedulerScrollViewer), new FrameworkPropertyMetadata(null, (d, e) => ScrollBarVisibility.Visible));
        //    ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(SchedulerScrollViewer), new FrameworkPropertyMetadata(null, (d, value) =>
        //    {
        //        var scheduler = SchedulerControl.GetScheduler((DependencyObject)d);
        //        if (scheduler != null && scheduler.ActiveView is TimelineView)
        //            return ScrollBarVisibility.Visible;
        //        return value;
        //    }));
        //}
        //public class Datetimes
        //{
        //    public DateTime startDate;
        //    public DateTime? endDate;
        //}
        /// <summary>     
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri      
        /// </summary>
        public DashboardViewModel()
        {
            //List<Datetimes> dts = new List<Datetimes>();
            //Datetimes dt = new Datetimes();
            //dt.startDate = new DateTime(2019, 06, 15);
            //dt.endDate = new DateTime(2019, 06, 18);
            //dts.Add(dt);
            //Datetimes dt1 = new Datetimes();
            //dt.startDate = new DateTime(2019, 06, 22);
            //dt.endDate = null;
            //dts.Add(dt1);
            //DateTime checkdate = new DateTime(2019, 06, 16);
            //bool IsContract = dts.Any(x => x.startDate.Date <= checkdate.Date && (x.endDate==null?DateTime.Now:x.endDate) >= checkdate.Date);
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel()...", category: Category.Info, priority: Priority.Low);

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

                PlantOwnerPopupClosedCommand = new DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshDashboardView));
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                GroupBoxExpandedCommand = new DelegateCommand<object>(GroupBoxExpandAction);
                DisableAppointmentCommand = new DelegateCommand<AppointmentWindowShowingEventArgs>(AppointmentWindowShowing);
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowing);
                DeleteAppointmentCommand = new DelegateCommand<KeyEventArgs>(DeleteAppointment);
                scheduler_VisibleIntervalsChangedCommand = new RelayCommand(new Action<object>(VisibleIntervalsChanged));

                SelectedAppointmentType = new List<object>();
                GeosApplication.Instance.FillFinancialYear();
                //
                //SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;

                // [001] Fill Employee Leaves 
                FillEmployeeLeaveType();
                FillAppointmentType();
                FillDashbordDataByPlant();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor DashboardViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DashboardViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [001][cpatil][GEOS2-3636][23-03-2022]
        private void FillDashbordDataByPlant()
        {
            //List<Employee> emp = HrmService.GetTodayBirthdayOfEmployees(0, DateTime.Now);
            try
            {
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {

                    string plantOwnersIds = string.Empty;

                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    }
                    #region Service Comments
                    //Service updated from GetLengthOfServiceByDepartment_V2041 to GetLengthOfServiceByDepartment_V2420 by [rdixit][GEOS2-4516][01.08.2023]
                    //EmployeesHeadCountByDepartment = new ObservableCollection<Department>(HrmService.GetEmployeesHeadCountByDepartmentByIdCompany(plantOwnersIds, SelectedPeriod));
                    //Service Updated from version V2250 to V2400 by [rdixit][09.06.2023][GEOS2-4515]
                    //UpcomingBirthdayOfEmployee = new ObservableCollection<Employee>(HrmService.GetUpcomingBirthdaysOfEmployees(plantOwnersIds, SelectedPeriod));
                    //UpcomingEmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetUpcomingLeavesOfEmployees(plantOwnersIds, SelectedPeriod));
                    //[001] code comment
                    //CompanyLeaves = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(plantOwnersIds));
                    //Service updated from GetEmployeesCountByIdCompany_V2250 to GetEmployeesCountByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetEmployeesCountByJobPositionByIdCompany_V2400 to GetEmployeesCountByJobPositionByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetBirthdaysOfEmployeesByYear_V2041 to GetBirthdaysOfEmployeesByYear_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetEmployeesCountByDepartmentAreaByIdCompany_V2250 to GetEmployeesCountByDepartmentAreaByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetEmployeeLeavesForDashboard_V2041 to GetEmployeeLeavesForDashboard_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetEmployeeDocumentsExpirationForDashboard_V2041 to GetEmployeeDocumentsExpirationForDashboard_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetLatestContractExpirationForDashboard_V2041 to GetLatestContractExpirationForDashboard_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetEmployeesWithAnniversaryDate_V2041 to GetEmployeesWithAnniversaryDate_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    #endregion
                    //EmployeesLengthOfServiceByDepartment = new ObservableCollection<Department>(HrmService.GetLengthOfServiceByDepartment_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[Shweta.Thube][GEOS2-5660][17.03.2025]
                    EmployeesLengthOfServiceByDepartment = new ObservableCollection<Department>(HrmService.GetLengthOfServiceByDepartment_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    TotalEmployeeList = new ObservableCollection<Company>(HrmService.GetEmployeesCountByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    TotalEmployeeCount = (uint)(TotalEmployeeList.Sum(x => x.EmployeesCount));
                    //[rdixit][GEOS2-5659][17.03.2025]
                    EmployeeCountByJobPosition = new ObservableCollection<JobDescription>(HrmService.GetEmployeesCountByJobPositionByIdCompany_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    UpcomingBirthdayOfEmployee = new ObservableCollection<Employee>(HrmService.GetBirthdaysOfEmployeesByYear_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeeCountbyDepartmentArea = new ObservableCollection<LookupValue>(HrmService.GetEmployeesCountByDepartmentAreaByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeeCountByGender = new ObservableCollection<LookupValue>(HrmService.GetEmployeesCountByGenderByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeeCountByContract = new ObservableCollection<ContractSituation>(HrmService.GetEmployeesCountByContractByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                    CompanyHolidays = new ObservableCollection<CompanyHoliday>(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>(HrmService.GetEmployeeLeavesForDashboard_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeeDocuments = new ObservableCollection<EmployeeDocument>(HrmService.GetEmployeeDocumentsExpirationForDashboard_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeesWithExpiryDate = new ObservableCollection<Employee>(HrmService.GetEmployeesWithExitDateForDashboard_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    EmployeeContractSituations = new ObservableCollection<EmployeeContractSituation>(HrmService.GetLatestContractExpirationForDashboard_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    //[rdixit][21.03.2025][GEOS2-5661]
                    EmployeesAnniversary = new ObservableCollection<Employee>(HrmService.GetEmployeesWithAnniversaryDate_V2620(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    HolidayList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    //rajashri
                    //IHrmService HrmService1 = new HrmServiceController("localhost:6699");
                   
                    AppointmentItems = new ObservableCollection<UI.Helper.Appointment>();
                    LabelItems = new ObservableCollection<LabelHelper>();


                    foreach (CompanyHoliday CompanyHoliday in CompanyHolidays)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = CompanyHoliday.Name + " [" + CompanyHoliday.Company.Alias + "]";
                        modelAppointment.StartDate = CompanyHoliday.StartDate;
                        modelAppointment.EndDate = CompanyHoliday.EndDate;
                        modelAppointment.Label = CompanyHoliday.IdHoliday;
                        if (CompanyHoliday.IsRecursive == 1)
                        {
                            modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                            modelAppointment.RecurrenceInfo = new RecurrenceInfo
                            {
                                Type = RecurrenceType.Yearly,
                                Periodicity = 1,
                                Month = CompanyHoliday.StartDate.Value.Month,
                                WeekOfMonth = WeekOfMonth.None,
                                DayNumber = CompanyHoliday.StartDate.Value.Day,
                                Range = RecurrenceRange.NoEndDate
                            }.ToXml();
                        }
                        //modelAppointment.AllDay = true;
                        modelAppointment.Location = "Holidays";
                        modelAppointment.Description = string.Format("{0} {1}", CompanyHoliday.Holiday.Value, "Holiday");
                        AppointmentItems.Add(modelAppointment);

                    }

                    foreach (var EmployeeLeave in EmployeeLeaves)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName);
                        modelAppointment.StartDate = Convert.ToDateTime(EmployeeLeave.StartDate.ToString());
                        if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                        }
                        else
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }

                        modelAppointment.Label = EmployeeLeave.IdLeave;
                        modelAppointment.EmployeeCode = EmployeeLeave.Employee.EmployeeCode;
                        modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                        modelAppointment.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                        modelAppointment.Location = "Leave";
                        modelAppointment.Description = string.Format("{0} {1}", EmployeeLeave.CompanyLeave.Name, "Leave");
                        modelAppointment.IdEmployee = EmployeeLeave.IdEmployee;
                        AppointmentItems.Add(modelAppointment);
                    }
                    foreach (var EmployeeDocument in EmployeeDocuments)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}", EmployeeDocument.Employee.FirstName, EmployeeDocument.Employee.LastName);
                        modelAppointment.StartDate = EmployeeDocument.EmployeeDocumentExpiryDate;
                        modelAppointment.EndDate = EmployeeDocument.EmployeeDocumentExpiryDate;
                        modelAppointment.EmployeeCode = EmployeeDocument.Employee.EmployeeCode;
                        modelAppointment.Label = 0;
                        modelAppointment.Location = "Document Expiry";
                        modelAppointment.Description = string.Format("{0} {1}", EmployeeDocument.EmployeeDocumentType.Value, "Expiry");
                        modelAppointment.IdEmployee = EmployeeDocument.IdEmployee;
                        AppointmentItems.Add(modelAppointment);
                    }


                    foreach (var EmployeeWithExpiryDate in EmployeesWithExpiryDate)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}", EmployeeWithExpiryDate.FirstName, EmployeeWithExpiryDate.LastName);
                        modelAppointment.StartDate = EmployeeWithExpiryDate.ExitDate;
                        modelAppointment.EndDate = EmployeeWithExpiryDate.ExitDate;
                        modelAppointment.EmployeeCode = EmployeeWithExpiryDate.EmployeeCode;
                        modelAppointment.Label = 0;
                        modelAppointment.Location = "Employee Exit";
                        modelAppointment.Description = System.Windows.Application.Current.FindResource("EmployeesExit").ToString();
                        modelAppointment.IdEmployee = EmployeeWithExpiryDate.IdEmployee;
                        AppointmentItems.Add(modelAppointment);
                    }

                    foreach (var EmployeeContractSituation in EmployeeContractSituations)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}", EmployeeContractSituation.Employee.FirstName, EmployeeContractSituation.Employee.LastName);
                        modelAppointment.StartDate = EmployeeContractSituation.ContractSituationEndDate;
                        modelAppointment.EndDate = EmployeeContractSituation.ContractSituationEndDate;
                        modelAppointment.EmployeeCode = EmployeeContractSituation.Employee.EmployeeCode;
                        modelAppointment.Label = 0;
                        modelAppointment.Location = "Contract Expiry";
                        modelAppointment.Description = System.Windows.Application.Current.FindResource("EmployeesContractExpiry").ToString();
                        modelAppointment.IdEmployee = EmployeeContractSituation.IdEmployee;
                        AppointmentItems.Add(modelAppointment);
                    }

                    foreach (var EmployeeAnniversary in EmployeesAnniversary)    //[rdixit][21.03.2025][GEOS2-5661]
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}", EmployeeAnniversary.FirstName, EmployeeAnniversary.LastName);
                        DateTime startdate = Convert.ToDateTime(EmployeeAnniversary.HireDate);
                        int year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                        modelAppointment.StartDate = new DateTime(year, startdate.Month, startdate.Day);
                        modelAppointment.EndDate = new DateTime(year, startdate.Month, startdate.Day);
                        modelAppointment.EmployeeCode = EmployeeAnniversary.EmployeeCode;
                        modelAppointment.Label = 0;
                        modelAppointment.Location = "Company Anniversary";
                        string years = EmployeeAnniversary.LengthOfService.ToString();
                        string suffix = "th";

                        if (int.TryParse(years, out int numYears))
                        {                    
                            if (numYears % 100 >= 11 && numYears % 100 <= 13)
                            {
                                suffix = "th";
                            }
                            else
                            {                              
                                switch (numYears % 10)
                                {
                                    case 1:
                                        suffix = "st";
                                        break;
                                    case 2:
                                        suffix = "nd";
                                        break;
                                    case 3:
                                        suffix = "rd";
                                        break;
                                    default:
                                        suffix = "th";
                                        break;
                                }
                            }
                        }
                        modelAppointment.Description = string.Format("{0}{1} {2}", EmployeeAnniversary.LengthOfService, suffix, "Company Anniversary");
                        modelAppointment.IdEmployee = EmployeeAnniversary.IdEmployee;
                        AppointmentItems.Add(modelAppointment);
                    }

                    foreach (var EmployeeBirthday in UpcomingBirthdayOfEmployee)
                    {
                        UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                        modelAppointment.Subject = string.Format("{0} {1}", EmployeeBirthday.FirstName, EmployeeBirthday.LastName);
                        modelAppointment.StartDate = EmployeeBirthday.BirthDate;
                        modelAppointment.EndDate = EmployeeBirthday.BirthDate;
                        modelAppointment.EmployeeCode = EmployeeBirthday.EmployeeCode;
                        modelAppointment.Label = 0;
                        modelAppointment.Location = "Birthday";
                        modelAppointment.Description = System.Windows.Application.Current.FindResource("EmployeesBirthday").ToString();
                        modelAppointment.IdEmployee = EmployeeBirthday.IdEmployee;
                        AppointmentItems.Add(modelAppointment);
                    }

                    foreach (var Holiday in HolidayList)
                    {
                        LabelHelper labelForCompanyHoliday = new LabelHelper();
                        labelForCompanyHoliday.Id = Holiday.IdLookupValue;
                        labelForCompanyHoliday.Color = new BrushConverter().ConvertFromString(Holiday.HtmlColor.ToString()) as SolidColorBrush;
                        labelForCompanyHoliday.Caption = Holiday.Value;
                        LabelItems.Add(labelForCompanyHoliday);
                    }
                    //foreach (var CompanyLeave in CompanyLeaves)
                    //{
                    //    LabelHelper label = new LabelHelper();
                    //    label.Id = Convert.ToInt32(CompanyLeave.IdCompanyLeave);
                    //    label.Color = new BrushConverter().ConvertFromString(CompanyLeave.HtmlColor.ToString()) as SolidColorBrush;
                    //    label.Caption = CompanyLeave.Name;
                    //    LabelItems.Add(label);
                    //}

                    foreach (var EmployeeLeaveType in GeosApplication.Instance.EmployeeLeaveList)
                    {
                        if (EmployeeLeaveType.IdLookupValue > 0 && EmployeeLeaveType.HtmlColor != null && EmployeeLeaveType.HtmlColor != string.Empty)
                        {
                            LabelHelper label = new LabelHelper();
                            label.Id = Convert.ToInt32(EmployeeLeaveType.IdLookupValue);
                            label.Color = new BrushConverter().ConvertFromString(EmployeeLeaveType.HtmlColor.ToString()) as SolidColorBrush;
                            label.Caption = EmployeeLeaveType.Value;
                            LabelItems.Add(label);
                        }
                    }
  						SelectedAppointmentType = new List<object>(AppointmentType);

                    if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 105)))
                    {
                        TravelList = new ObservableCollection<EmployeeTrips>(HrmService.GetTripDetails(plantOwnersIds, (UInt32)GeosApplication.Instance.ActiveUser.IdUser, HrmCommon.Instance.SelectedPeriod));

                        foreach (var travelinfo in TravelList)
                        {
                            if (travelinfo.FirstName != null)
                            {
                                DateTime arrivalDate = (DateTime)travelinfo.ArrivalDate;
                                string ATime = arrivalDate.ToString("HH:mm");
                                DateTime deptDate = (DateTime)travelinfo.DepartureDate;
                                string DTime = deptDate.ToString("HH:mm");
                                UI.Helper.Appointment modelAppointment = new UI.Helper.Appointment();
                                modelAppointment.Subject = string.Format("{0} {1}\n{2} - {3}| {4} - {5}", travelinfo.FirstName, travelinfo.LastName, travelinfo.OriginalPlant, ATime, travelinfo.Destination, DTime);
                                modelAppointment.StartDate = travelinfo.ArrivalDate;
                                modelAppointment.EndDate = travelinfo.DepartureDate;
                                modelAppointment.EmployeeCode = travelinfo.Code;
                                modelAppointment.Label = 0;
                                if (travelinfo.MainTransport == "Air")
                                {
                                    modelAppointment.Location = "TravelsDataAir";
                                }
                                if (travelinfo.MainTransport == "Car")
                                {
                                    modelAppointment.Location = "TravelsDataCar";
                                }
                                if (travelinfo.MainTransport == "Motorcycle")
                                {
                                    modelAppointment.Location = "TravelsDataMotor";
                                }
                                if (travelinfo.MainTransport == "Train")
                                {
                                    modelAppointment.Location = "TravelsDataTrain";
                                }
                                //modelAppointment.Description = string.Format("{0} {1}", travelinfo.MainTransport, "Travel");
                                modelAppointment.IdEmployee = (int)travelinfo.IdEmployee;
                                AppointmentItems.Add(modelAppointment);
                            }
                          
                        }
                    }
                }
                else
                {

                    EmployeesLengthOfServiceByDepartment = new ObservableCollection<Department>();
                    TotalEmployeeList = new ObservableCollection<Company>();

                    EmployeesHeadCountByDepartment = new ObservableCollection<Department>();
                    EmployeeCountByJobPosition = new ObservableCollection<JobDescription>();

                    UpcomingBirthdayOfEmployee = new ObservableCollection<Employee>();
                    UpcomingEmployeeLeaves = new ObservableCollection<EmployeeLeave>();

                    EmployeeCountbyDepartmentArea = new ObservableCollection<LookupValue>();
                    EmployeeCountByGender = new ObservableCollection<LookupValue>();
                    EmployeeCountByContract = new ObservableCollection<ContractSituation>();



                    CompanyHolidays = new ObservableCollection<CompanyHoliday>();
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>();
                    EmployeeDocuments = new ObservableCollection<EmployeeDocument>();
                    EmployeesWithExpiryDate = new ObservableCollection<Employee>();
                    EmployeeContractSituations = new ObservableCollection<EmployeeContractSituation>();
                    EmployeesAnniversary = new ObservableCollection<Employee>();

                    AppointmentItems.Clear();

                    SelectedAppointmentType = new List<object>();
                    SelectedAppointmentType.AddRange(AppointmentType);

                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DashboardViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DashboardViewModel() - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        private void RefreshDashboardView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDashboardView()...", category: Category.Info, priority: Priority.Low);

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

                FillDashbordDataByPlant();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDashboardView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

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

            FillDashbordDataByPlant();

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

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

            FillDashbordDataByPlant();

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method for scroll daily event down when expanded it.
        /// </summary>
        /// <param name="obj"></param>
        private void GroupBoxExpandAction(object obj)
        {

            try
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(

                new Action(() =>
                {
                    (obj as LayoutGroup).BringIntoView();
                })
            , DispatcherPriority.Loaded);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GroupBoxExpandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillAppointmentType()
        {
            AppointmentType = new List<string>();
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 105)))
            {
                AppointmentType.Add("Travel");
            }
            AppointmentType.Add("Leave");
            AppointmentType.Add("Holidays");
            AppointmentType.Add("Document Expiry");
            AppointmentType.Add("Employee Exit");
            AppointmentType.Add("Company Anniversary");
            AppointmentType.Add("Birthday");
            AppointmentType.Add("Contract Expiry");

            //SelectedAppointmentType.AddRange(AppointmentType);
        }


        private void AppointmentWindowShowing(AppointmentWindowShowingEventArgs obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()...", category: Category.Info, priority: Priority.Low);
                obj.Cancel = true;
                if (obj.Appointment.SourceObject != null)
                {
                    if (((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployee != 0)
                    {
                        EmployeeDetails = HrmService.GetEmployeeByIdEmployee(((UI.Helper.Appointment)obj.Appointment.SourceObject).IdEmployee, HrmCommon.Instance.SelectedPeriod);
                        EmployeeProfileDetailView employeeProfileDetailView = new EmployeeProfileDetailView();
                        EmployeeProfileDetailViewModel employeeProfileDetailViewModel = new EmployeeProfileDetailViewModel();
                        EventHandler handle = delegate { employeeProfileDetailView.Close(); };
                        employeeProfileDetailViewModel.RequestClose += handle;
                        employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;

                        if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                            if (HrmCommon.Instance.IsPermissionReadOnly)
                                employeeProfileDetailViewModel.InitReadOnly(EmployeeDetails, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                            else
                                employeeProfileDetailViewModel.Init(EmployeeDetails, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                        }



                        employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;
                        var ownerInfo = (obj.OriginalSource as FrameworkElement);
                        employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);
                        employeeProfileDetailView.ShowDialog();

                        if (employeeProfileDetailViewModel.IsSaveChanges == true)
                        {
                            EmployeeDetails.EmployeeCode = employeeProfileDetailViewModel.EmployeeUpdatedDetail.EmployeeCode;
                            EmployeeDetails.FirstName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.FirstName;
                            EmployeeDetails.LastName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.LastName;
                            EmployeeDetails.EmployeeDocuments = new List<EmployeeDocument>(employeeProfileDetailViewModel.EmployeeDocumentList);
                            EmployeeDetails.EmployeeAnnualLeaves = new List<EmployeeAnnualLeave>(employeeProfileDetailViewModel.EmployeeAnnualLeaveList);

                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AppointmentWindowShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PopupMenuShowing(PopupMenuShowingEventArgs obj)
        {


            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()...", category: Category.Info, priority: Priority.Low);

                if (obj.MenuType == ContextMenuType.CellContextMenu)
                {



                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();



                }
                else if (obj.MenuType == ContextMenuType.AppointmentContextMenu)
                {
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                }


                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PopupMenuShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        private void DeleteAppointment(KeyEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAppointment()...", category: Category.Info, priority: Priority.Low);

                if (obj.Source is SchedulerControlEx)
                {
                    if (obj.Key == Key.Delete)
                    {
                        SchedulerControlEx schedule = (SchedulerControlEx)obj.Source;
                        if (schedule.SelectedAppointments != null)
                        {
                            if (schedule.SelectedAppointments.Count > 0)
                            {
                                obj.Handled = true;

                            }
                        }

                    }
                }


                GeosApplication.Instance.Logger.Log("Method DeleteAppointment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAppointment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void VisibleIntervalsChanged(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()...", category: Category.Info, priority: Priority.Low);

                SchedulerControlEx scheduler = (SchedulerControlEx)obj;

                TodaysTotalAppointment = scheduler.GetAppointments(new DateTimeRange(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))).ToList().Count;
                //= AppointmentItems.Where(n => Convert.ToDateTime(n.StartDate).Date <= GeosApplication.Instance.ServerDateTime.Date && Convert.ToDateTime(n.EndDate).Date >= GeosApplication.Instance.ServerDateTime.Date).ToList().Count;

                if (TodaysTotalAppointment > 0)
                {
                    TodaysTotalAppointmentVisibility = Visibility.Visible;
                }
                else
                {
                    TodaysTotalAppointmentVisibility = Visibility.Collapsed;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleIntervalsChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method VisibleIntervalsChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to fill EmployeeLeaveType from Lookup values
        /// </summary>
        public void FillEmployeeLeaveType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.EmployeeLeaveList == null)
                {
                    GeosApplication.Instance.EmployeeLeaveList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32));
                    GeosApplication.Instance.EmployeeLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

    }
}
