using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Emdep.Geos.Utility;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.XtraScheduler;
using System.Threading;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class HrmMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// [M049-07][20180810][Add option to add and edit shifts][adadibathina]
        /// </summary>

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        private int idUser;
        private long selectedPeriod;
        LeavesViewModel objLeavesViewModel;
        #endregion // Services
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #region Properties    
        public static bool IsAttendanceLoaded=false;
        public LeavesViewModel ObjLeavesViewModel
        {
            get { return objLeavesViewModel; }
            set
            {
                objLeavesViewModel = value;
            }
        }
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

        TileBarItemsHelper selectedTileCollection;
        public TileBarItemsHelper SelectedTileCollection
        {
            get { return selectedTileCollection; }
            set
            {
                selectedTileCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileCollection"));
            }
        }
        public int IdHrmUserViewPermission { get; private set; }
        public ICommand LoadedViewInstanceCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// [001][skale][2019-21-5][GEOS2-273][SP63]Add new fields Company, Organization,Location in employees grid
		/// [002][nsatpute][18-09-2024][GEOS2-5929]
        /// </summary>
        public HrmMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor HrmMainViewModel()...", category: Category.Info, priority: Priority.Low);
                IsAttendanceLoaded = false;
                LoadedViewInstanceCommand = new RelayCommand(new Action<object>(LoadedViewInstanceAction));
                List<GeosAppSetting> settings = WorkbenchService.GetSelectedGeosAppSettings("54");
                if (settings != null && settings.Count > 0)
                {
                    decimal percentage;
                    if (Decimal.TryParse(settings[0].DefaultValue, out percentage))
                    {
                        HrmCommon.Instance.PercentageForHighRemainingLeaves = percentage;
                    }
                }

                GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2490(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();
                HrmCommon.Instance.SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                //Updated service GetAuthorizedPlantsByIdUser_V2031 with GetAuthorizedPlantsByIdUser_V2430 by [rdixit][26.08.2023][GEOS2-3483]
                // HrmCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Company>(HrmService.GetAuthorizedPlantsByIdUser_V2430(GeosApplication.Instance.ActiveUser.IdUser));
                //[Sudhir.jangra][GEOS2-4816]
                //[nsatpute][18-09-2024][GEOS2-5929]
                HrmCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Company>(HrmService.GetAuthorizedPlantsByIdUser_V2600(GeosApplication.Instance.ActiveUser.IdUser));  // [nsatpute][13-01-2025][GEOS2-6776]
                HrmCommon.Instance.IsCompanyList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList.Where(x => x.IsCompany == 1).OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));
                HrmCommon.Instance.IsOrganizationList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList.Where(x => x.IsOrganization == 1).OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));
                HrmCommon.Instance.IsLocationList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList.Where(x => x.IsLocation == 1).OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));

                //[001] added
                ObservableCollection<Company> tempCommonList = new ObservableCollection<Company>();

                if (HrmCommon.Instance.IsCompanyList != null)
                    tempCommonList.AddRange(HrmCommon.Instance.IsCompanyList);

                if (HrmCommon.Instance.IsOrganizationList != null)
                    tempCommonList.AddRange(HrmCommon.Instance.IsOrganizationList);

                if (HrmCommon.Instance.IsLocationList != null)
                    tempCommonList.AddRange(HrmCommon.Instance.IsLocationList);

                tempCommonList = new ObservableCollection<Company>(tempCommonList.OrderBy(Company => Company.IdCountry).ToList());

                ObservableCollection<Company> tempCollection = new ObservableCollection<Company>(tempCommonList.GroupBy(x => x.IdCompany).Select(group => group.First()));
                HrmCommon.Instance.CombineIslocationIsorganizationIscompanyList = new ObservableCollection<Company>(tempCollection.OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));
                //end

                HrmCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                HrmCommon.Instance.IsPermissionEnabled = true;
                HrmCommon.Instance.ActiveEmployee = HrmService.GetEmployeeCurrentDetail(GeosApplication.Instance.ActiveUser.IdUser, HrmCommon.Instance.SelectedPeriod);
                HrmCommon.Instance.IdUserPermission = SelectIdUserPermission();
                HrmCommon.Instance.UserPermission = SelectUserPermission();

                SetUserPermission();

                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                Company selectedPlant = HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.ShortName == serviceurl);

                if (usrDefault != null)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                }
                else
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                }

                if (selectedPlant != null)
                {
                    HrmCommon.Instance.SelectedAuthorizedPlantsList.Add(selectedPlant);
                }
                else
                {
                    HrmCommon.Instance.SelectedAuthorizedPlantsList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                }
                HrmCommon.Instance.FinancialYearList = GeosApplication.Instance.FillFinancialYear();
                //[GEOS2-6880][rdixit][30.01.2025] Removed if condition of showing only employee section if ManageEmployeeContacts permission is exists
                TileCollection = new ObservableCollection<TileBarItemsHelper>();
                //[rdixit][GEOS2-6979][02.04.2025]
                if (GeosApplication.Instance.IsHRMTravelManagerPermission || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                {
                    //Employee
                    TileBarItemsHelper tileBarItemsHelperEmployees = new TileBarItemsHelper();
                    tileBarItemsHelperEmployees.Caption = System.Windows.Application.Current.FindResource("HRMEmployees").ToString();
                    tileBarItemsHelperEmployees.BackColor = "#CC6D00";
                    tileBarItemsHelperEmployees.GlyphUri = "hrm_employee.png";
                    tileBarItemsHelperEmployees.Visibility = Visibility.Visible;
                    tileBarItemsHelperEmployees.NavigateCommand = new DelegateCommand(NavigateEmployeesView);
                    TileCollection.Add(tileBarItemsHelperEmployees);

                    //Travel
                    TileBarItemsHelper tileBarItemsHelperTravel = new TileBarItemsHelper();
                    tileBarItemsHelperTravel.Caption = System.Windows.Application.Current.FindResource("HRMTravel").ToString();
                    tileBarItemsHelperTravel.BackColor = "#5900b3";
                    tileBarItemsHelperTravel.GlyphUri = "Travel.png";
                    tileBarItemsHelperTravel.Visibility = Visibility.Visible;
                    tileBarItemsHelperTravel.Children = new ObservableCollection<TileBarItemsHelper>();


                    //[pramod.misal][GEOS2-3363][20.09.2023]
                    //Trip
                    TileBarItemsHelper tileBarItemTripw = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("TitleForTrips").ToString(),
                        BackColor = "#5900b3",
                        GlyphUri = "Trips.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateToTrips)
                    };
                    tileBarItemsHelperTravel.Children.Add(tileBarItemTripw);

                    //ExpensesReport
                    TileBarItemsHelper tileBarItemExpenseReport = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("TravelExpenseReport").ToString(),
                        BackColor = "#5900b3",
                        GlyphUri = "ExpensesReport.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateEmployeeExpenseReport)
                    };
                    tileBarItemsHelperTravel.Children.Add(tileBarItemExpenseReport);

                    //Meal Allowance
                    TileBarItemsHelper tileBarMealAllowance = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("MealAllowanceTile").ToString(),
                        BackColor = "#5900b3",
                        GlyphUri = "Meal Allowance.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateMealAllowance)
                    };
                    tileBarItemsHelperTravel.Children.Add(tileBarMealAllowance);

                    TileCollection.Add(tileBarItemsHelperTravel);
                }
                else
                {                 
                    TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                    tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("HRMDashboard").ToString();
                    tileBarItemsHelperDashboard.BackColor = "#00879C";
                    tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                    tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                    tileBarItemsHelperDashboard.NavigateCommand = new DelegateCommand(NavigateDashboardView);
                    TileCollection.Add(tileBarItemsHelperDashboard);

                    TileBarItemsHelper tileBarItemsHelperEmployees = new TileBarItemsHelper();
                    tileBarItemsHelperEmployees.Caption = System.Windows.Application.Current.FindResource("HRMEmployees").ToString();
                    tileBarItemsHelperEmployees.BackColor = "#CC6D00";
                    tileBarItemsHelperEmployees.GlyphUri = "hrm_employee.png";
                    tileBarItemsHelperEmployees.Visibility = Visibility.Visible;
                    tileBarItemsHelperEmployees.NavigateCommand = new DelegateCommand(NavigateEmployeesView);
                    TileCollection.Add(tileBarItemsHelperEmployees);

                    TileBarItemsHelper tileBarItemsHelperOrganization = new TileBarItemsHelper();
                    tileBarItemsHelperOrganization.Caption = System.Windows.Application.Current.FindResource("HRMOrganization").ToString();
                    tileBarItemsHelperOrganization.BackColor = "#5C5C5C";
                    tileBarItemsHelperOrganization.GlyphUri = "hrm_organization.png";
                    tileBarItemsHelperOrganization.Visibility = Visibility.Visible;
                    tileBarItemsHelperOrganization.NavigateCommand = new DelegateCommand(NavigateEmployeesOrganizationView);
                    TileCollection.Add(tileBarItemsHelperOrganization);

                    //Leaves
                    TileBarItemsHelper tileBarItemsHelperLeaves = new TileBarItemsHelper();
                    tileBarItemsHelperLeaves.Caption = System.Windows.Application.Current.FindResource("HRMLeaves").ToString();
                    tileBarItemsHelperLeaves.BackColor = "#3E7038";
                    tileBarItemsHelperLeaves.GlyphUri = "hrm_Leaves.png";
                    tileBarItemsHelperLeaves.Visibility = Visibility.Visible;
                    tileBarItemsHelperLeaves.Children = new ObservableCollection<TileBarItemsHelper>();

                    TileBarItemsHelper tileBarItemLeavesSchedule = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("LeavesSchedule").ToString(),
                        BackColor = "#3E7038",
                        GlyphUri = "bLeavesSchedule.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateEmployeesLeavesView)
                    };
                    tileBarItemsHelperLeaves.Children.Add(tileBarItemLeavesSchedule);

                    //[Sprint_61] [17-04-2019] (#70119) New section Leave Summary under Leaves section---[sdesai]
                    TileBarItemsHelper tileBarItemLeavesSummary = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("LeavesSummary").ToString(),
                        BackColor = "#3E7038",
                        GlyphUri = "bLeavesSummary.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateEmployeesLeavesSummaryView)

                    };
                    tileBarItemsHelperLeaves.Children.Add(tileBarItemLeavesSummary);

                    TileCollection.Add(tileBarItemsHelperLeaves);

                    //Attendance
                    TileBarItemsHelper tileBarItemsHelperAttendance = new TileBarItemsHelper();
                    tileBarItemsHelperAttendance.Caption = System.Windows.Application.Current.FindResource("HRMAttendance").ToString();
                    tileBarItemsHelperAttendance.BackColor = "#840a6a";
                    tileBarItemsHelperAttendance.GlyphUri = "hrm_attendance.png";
                    tileBarItemsHelperAttendance.Visibility = Visibility.Visible;
                    tileBarItemsHelperAttendance.NavigateCommand = new DelegateCommand(NavigateEmployeesAttendanceView);
                    TileCollection.Add(tileBarItemsHelperAttendance);

                    //Training
                    TileBarItemsHelper tileBarItemsHelperTraining = new TileBarItemsHelper();
                    tileBarItemsHelperTraining.Caption = System.Windows.Application.Current.FindResource("HRMTraining").ToString();
                    tileBarItemsHelperTraining.BackColor = "#1BB0BA";
                    tileBarItemsHelperTraining.GlyphUri = "Training.png";  //"hrm_training.png";
                    tileBarItemsHelperTraining.Visibility = Visibility.Visible;
                    tileBarItemsHelperTraining.Children = new ObservableCollection<TileBarItemsHelper>();

                    if (GeosApplication.Instance.IsManageTrainingPermission == true)
                    {
                        // Trainings
                        TileBarItemsHelper tileBarItemTrainging_Trainings = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Trainings").ToString(),
                            BackColor = "#1BB0BA",
                            GlyphUri = "b_trainings.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateTraining_TrainingsView)
                        };
                        tileBarItemsHelperTraining.Children.Add(tileBarItemTrainging_Trainings);

                        // Training Plans
                        TileBarItemsHelper tileBarItemTraingingPlans = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("TrainingPlans").ToString(),
                            BackColor = "#1BB0BA",
                            GlyphUri = "b_trainingPlans.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateTrainingPlansView)
                        };
                        tileBarItemsHelperTraining.Children.Add(tileBarItemTraingingPlans);

                        TileCollection.Add(tileBarItemsHelperTraining);
                    }

                    //Travel //[GEOS2-3943][rdixit][04.01.2022] Added User Permission Validation [rdixit][09.01.2024][GEOS2-5112]
                    if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 39 || up.IdPermission == 40 || up.IdPermission == 96 || up.IdPermission == 97 || up.IdPermission == 98 || up.IdPermission == 41) && up.Permission.IdGeosModule == 7))
                    {
                        TileBarItemsHelper tileBarItemsHelperTravel = new TileBarItemsHelper();
                        tileBarItemsHelperTravel.Caption = System.Windows.Application.Current.FindResource("HRMTravel").ToString();
                        tileBarItemsHelperTravel.BackColor = "#5900b3";
                        tileBarItemsHelperTravel.GlyphUri = "Travel.png";
                        tileBarItemsHelperTravel.Visibility = Visibility.Visible;
                        tileBarItemsHelperTravel.Children = new ObservableCollection<TileBarItemsHelper>();


                        //[pramod.misal][GEOS2-3363][20.09.2023]
                        //Trip
                        TileBarItemsHelper tileBarItemTripw = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("TitleForTrips").ToString(),
                            BackColor = "#5900b3",
                            GlyphUri = "Trips.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateToTrips)
                        };
                        tileBarItemsHelperTravel.Children.Add(tileBarItemTripw);

                        //ExpensesReport
                        TileBarItemsHelper tileBarItemExpenseReport = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("TravelExpenseReport").ToString(),
                            BackColor = "#5900b3",
                            GlyphUri = "ExpensesReport.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateEmployeeExpenseReport)
                        };
                        tileBarItemsHelperTravel.Children.Add(tileBarItemExpenseReport);

                        //Meal Allowance
                        TileBarItemsHelper tileBarMealAllowance = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("MealAllowanceTile").ToString(),
                            BackColor = "#5900b3",
                            GlyphUri = "Meal Allowance.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateMealAllowance)
                        };
                        tileBarItemsHelperTravel.Children.Add(tileBarMealAllowance);

                        TileCollection.Add(tileBarItemsHelperTravel);
                    }

                    //[rdixit][22.04.2025][GEOS2-5647]

                    TileBarItemsHelper tileBarItemsHelperSurvey = new TileBarItemsHelper();
                    tileBarItemsHelperSurvey.Caption = Application.Current.FindResource("HRMSurvey").ToString();
                    tileBarItemsHelperSurvey.BackColor = "#89ad1a";
                    tileBarItemsHelperSurvey.GlyphUri = "Survey.png";
                    tileBarItemsHelperSurvey.Visibility = Visibility.Visible;
                    TileCollection.Add(tileBarItemsHelperSurvey);

                    //Career //[GEOS2-3479][rajashri][17/10/2023]
                    if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 92)))
                    {
                        TileBarItemsHelper tileBarItemsHelperCareer = new TileBarItemsHelper();
                        tileBarItemsHelperCareer.Caption = System.Windows.Application.Current.FindResource("HRMCareer").ToString();
                        tileBarItemsHelperCareer.BackColor = "#3C7EC9";
                        tileBarItemsHelperCareer.GlyphUri = "Career.png";
                        tileBarItemsHelperCareer.Visibility = Visibility.Visible;
                        tileBarItemsHelperCareer.NavigateCommand = new DelegateCommand(NavigateToCareerView);
                        TileCollection.Add(tileBarItemsHelperCareer);
                    }

                    //Configuration
                    TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                    tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("HRMConfiguration").ToString();
                    tileBarItemsHelperConfiguration.BackColor = "#8b99e8";
                    tileBarItemsHelperConfiguration.GlyphUri = "hrm_configuration.png";
                    tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                    tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                    if (GeosApplication.Instance.IsWatchMySelfOnlyPermissionForHRM) //[rdixit][16.01.2024][GEOS2-5074]
                    {
                        TileBarItemsHelper tileBarConfigutionMyPreferences = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("MyPreferences").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "MyPreference_Black.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateMyPreferenceView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionMyPreferences);
                    }
                    else
                    {

                        TileBarItemsHelper tileBarItemConfigutionCompanies = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Entities").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bCompanies.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateEmployeesConfigurationView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionCompanies);

                        TileBarItemsHelper tileBarItemConfigutionDepartments = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Departments").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bDepartment.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateDepartmentView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionDepartments);

                        //Sprint 41-Add Holidays Section ---sdesai
                        TileBarItemsHelper tileBarItemConfigutionHolidays = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Holidays").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bHolidays.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateHolidaysView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionHolidays);

                        //Sprint 41-Add Job Descriptions Section ---sdesai
                        TileBarItemsHelper tileBarItemConfigutionJobDescription = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("JobDescriptions").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bJobDescriptions.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateJobDescriptionsView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionJobDescription);
                        //Sprint 49 
                        //TileCollection.Add(tileBarItemsHelperConfiguration);

                        //Sprint 49 - Add option to add and edit shifts -- adadibathina

                        TileBarItemsHelper tileBarConfigutionShifts = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Shifts").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bShifts.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateShiftsView)

                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionShifts);

                        TileBarItemsHelper tileBarConfigutionLeaves = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Leaves").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bLeaves.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateLeavesView)

                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionLeaves);

                        // [nsatpute][14-11-2024][GEOS2-5747]
                        TileBarItemsHelper tileBarConfigutionBacklogHolidays = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Automaticbacklog_Backlogholidays").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bCalenderBackArrow.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateBackHolidaysView)

                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionBacklogHolidays);


                        TileBarItemsHelper tileBarConfigutionWorkTypes = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("WorkTypes").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bWorkTypes.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateWorkTypesView)

                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionWorkTypes);

                        //sjadhav
                        TileBarItemsHelper tileBarConfigutionMyPreferences = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("MyPreferences").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "MyPreference_Black.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateMyPreferenceView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionMyPreferences);

                        // Skills
                        TileBarItemsHelper tileBarConfigutionSkills = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Skills").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "bSkill.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateSkillsView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionSkills);

                        // Objectives
                        TileBarItemsHelper tileBarConfigutionObjectives = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Objectives").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "Objective.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateObjectiveView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionObjectives);

                        // Tasks
                        TileBarItemsHelper tileBarConfigutionTasks = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Tasks").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "Tasks.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateTasksView)
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarConfigutionTasks);
                    }

                    TileCollection.Add(tileBarItemsHelperConfiguration);
                }
                GetEmailExitEventJDList();

                GeosApplication.Instance.Logger.Log("Constructor Constructor HrmMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor HrmMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor HrmMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor HrmMainViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void LoadedViewInstanceAction(object obj)
        {
            HrmCommon.Instance.MainWindowINavigationService = this.Service;
        }

        private void AddSettings()
        {
            GeosApplication.Instance.Logger.Log("HrmMainViewModel Method AddSettings()...", Category.Exception, priority: Priority.Low);
            bool addsettings = false;
            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
            if (!GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceFilePath"))
            {
                // lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceFilePath", ""));
                addsettings = true;
            }
            if (!GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceDataSourceSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceDataSourceSelectedIndex", ""));
                addsettings = true;
            }
            if (addsettings)
            {
                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));

                }
                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            }


            GeosApplication.Instance.Logger.Log("HrmMainViewModel Method AddSettings()...executed successfully", Category.Exception, priority: Priority.Low);

        }

        /// <summary>
        /// Method for Navigate Dashboard
        /// </summary>
        private void NavigateDashboardView()
        {
            try
            {
                IsAttendanceLoaded = false;
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()...", category: Category.Info, priority: Priority.Low);
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.DashboardView", new DashboardViewModel(), null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDashboardView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees View
        /// </summary>
        private void NavigateEmployeesView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeProfileViewModel employeeProfileViewModel = new EmployeeProfileViewModel();
                employeeProfileViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeProfileView", employeeProfileViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Expense Report View
        /// </summary>
        private void NavigateEmployeeExpenseReport()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeeExpenseReport()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMTravel").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeExpenseReportViewModel employeeExpenseReportViewModel = new EmployeeExpenseReportViewModel();
                employeeExpenseReportViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeExpenseReportView", employeeExpenseReportViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeeExpenseReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeeExpenseReport()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateToTrips()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateToTrips()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMTravel").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeTripsViewModel employeetripsViewModel = new EmployeeTripsViewModel();
                employeetripsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeTripsView", employeetripsViewModel, null, this, true);


                GeosApplication.Instance.Logger.Log("Method NavigateToTrips()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateToTrips()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateMealAllowance()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateMealAllowance()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMTravel").ToString());//[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                MealAllowanceViewModel MealAllowanceViewModel = new MealAllowanceViewModel();
                MealAllowanceViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.MealAllowanceView", MealAllowanceViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateMealAllowance()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateMealAllowance()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees Organization View
        /// </summary>
        private void NavigateEmployeesOrganizationView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesOrganizationView()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeOrganizationViewModel employeeOrganizationViewModel = new EmployeeOrganizationViewModel();

                employeeOrganizationViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeOrganizationView", employeeOrganizationViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesOrganizationView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees Leaves View
        /// </summary>
        private void NavigateEmployeesLeavesView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesLeavesView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMLeaves").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();           
                EmployeeLeavesViewModel employeeLeavesScheduleViewModel = new EmployeeLeavesViewModel();
                employeeLeavesScheduleViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeLeavesView", employeeLeavesScheduleViewModel, null, this, true);

                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesLeavesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }

        /// <summary> 
        /// Method for Navigate Employees Configuration View
        /// </summary>

        private void NavigateEmployeesConfigurationView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesConfigurationViewModel()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
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

                EmployeeConfigurationViewModel employeeConfigurationViewModel = new EmployeeConfigurationViewModel();
                employeeConfigurationViewModel.Init();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeConfigurationView", employeeConfigurationViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeeConfigurationView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees Attendance View
        /// </summary>
        private void NavigateEmployeesAttendanceView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesAttendanceView()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                //[rdixit][GEOS2-8236][09.07.2025]
                if (IsAttendanceLoaded == false)
                {
                    FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();

                    EmployeeAttendanceViewModel employeeAttendanceViewModel = new EmployeeAttendanceViewModel();
                    employeeAttendanceViewModel.IsFilterLoaded = true;//[rdixit][29.08.2022][GEOS2-3751]
                    employeeAttendanceViewModel.Init();

                    Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeAttendanceView", employeeAttendanceViewModel, null, this, true);
                }
              
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesAttendanceView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesAttendanceView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Department View
        /// </summary>
        private void NavigateDepartmentView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateDepartmentView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeDepartmentsViewModel employeeDepartmentsViewModel = new EmployeeDepartmentsViewModel();
                employeeDepartmentsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeDepartmentsView", employeeDepartmentsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDepartmentView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDepartmentView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Sprint 41-[HRM-M041-07] New configuration section Holidays---sdesai
        /// Method to Navigate Employees Holidays View
        /// </summary>
        private void NavigateHolidaysView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesHolidaysView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeesHolidaysViewModel employeesHolidaysViewModel = new EmployeesHolidaysViewModel();
                employeesHolidaysViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeesHolidaysView", employeesHolidaysViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesHolidaysView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesHolidaysView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateJobDescriptionsView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateJobDescriptionsView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeJobDescriptionsViewModel employeeJobDescriptionsViewModel = new EmployeeJobDescriptionsViewModel();
                employeeJobDescriptionsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeJobDescriptionsView", employeeJobDescriptionsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateJobDescriptionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateJobDescriptionsView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Sprint 49-[M049-07][20180810][Add option to add and edit shifts][adadibathina]
        /// Method to Navigate Configuration Shifts
        /// </summary>
        public void NavigateShiftsView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateShiftsView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeShiftsViewModel employeeShiftsViewModel = new EmployeeShiftsViewModel();
                employeeShiftsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeShiftsView", employeeShiftsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateShiftsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateShiftsView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Navigate Configuration Leaves
        /// </summary>
        private void NavigateLeavesView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateLeavesView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null && ObjLeavesViewModel.IsPlantChange)
                    SavechangesInLeaveGrid();
                LeavesViewModel leavesViewModel = new LeavesViewModel();            
                leavesViewModel.Init();
                ObjLeavesViewModel = leavesViewModel;
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.LeavesView", leavesViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateLeavesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateLeavesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][14-11-2024][GEOS2-5747]
        private void NavigateBackHolidaysView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateBackHolidaysView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                EmployeeBacklogHolidaysViewModel employeeBacklogHolidaysViewModel = new EmployeeBacklogHolidaysViewModel();
                //employeeShiftsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeBacklogHolidaysView", employeeBacklogHolidaysViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateBackHolidaysView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateBackHolidaysView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Navigate Configuration My Preference
        /// </summary>
        private void NavigateMyPreferenceView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferenceView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }

                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                //IsBusy = false;
                myPreferencesView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferenceView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateMyPreferenceView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Navigate Configuration Work Types
        /// </summary>
        private void NavigateWorkTypesView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateWorkTypesView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMConfiguration").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                WorkTypesViewModel workTypesViewModel = new WorkTypesViewModel();
                workTypesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.WorkTypesView", workTypesViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateWorkTypesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateWorkTypesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001] [vsana] [27-01-2021] [New Skill menu in the configuration section [#CS15]]
        /// Method to Navigate Configuration Skills
        /// </summary>
        private void NavigateSkillsView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateSkillsView()....", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                SkillsViewModel skillsViewModel = new SkillsViewModel();
                skillsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.SkillsView", skillsViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateSkillsView().... executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateSkillsView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001] [vsana] [23-02-2021] [New objectives menu in the configuration section [#CS16] [1 of 2 - Not Add+ Edit]]
        /// Method to Navigate Configurtaion Objectives
        /// </summary>
        private void NavigateObjectiveView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateObjectiveView()....", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                ObjectivesViewModel objectivesViewModel = new ObjectivesViewModel();
                objectivesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.ObjectivesView", objectivesViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateObjectiveView().... executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateObjectiveView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateTasksView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateTasksView()....", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                TasksViewModel tasksViewModel = new TasksViewModel();
                tasksViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.TasksView", tasksViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateTasksView().... executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateTasksView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [Sprint 61]-[17-04-2019][(#70119) New section Leave Summary under Leaves section][sdesai]
        /// Method to navigate Leaves summary
        /// </summary>
        private void NavigateEmployeesLeavesSummaryView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesLeavesSummaryView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMLeaves").ToString()); //[rdixit][GEOS2-8595][23.06.2025]
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                var employeeLeavesSummaryViewModel = new EmployeeLeavesSummaryViewModel();
                employeeLeavesSummaryViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeLeavesSummaryView", employeeLeavesSummaryViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesLeavesSummaryView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesLeavesSummaryView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }
        /// <summary>
        /// [001] [vsana] [23-03-2021] [HRM - Trainings 1 of 8 [#TRN01]]
        /// Method to Navigate TRAINING -> Trainings
        /// </summary>
        private void NavigateTraining_TrainingsView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateTraining_TrainingsView()...", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMTraining").ToString());                
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                HRMTrainingsViewModel hrmTrainingsViewModel = new HRMTrainingsViewModel();
                hrmTrainingsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.HRMTrainingsView", hrmTrainingsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateTraining_TrainingsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateTraining_TrainingsView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] [vsana] [23-03-2021] [HRM - Trainings 2 of 8 [#TRN02]]
        /// Method to Navigate TRAINING -> TrainingPlans
        /// </summary>
        private void NavigateTrainingPlansView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateTrainingPlansView()....", category: Category.Info, priority: Priority.Low);
                SelectedTileCollection = TileCollection.FirstOrDefault(i => i.Caption == Application.Current.FindResource("HRMTraining").ToString());
                //[GEOS2-5680][rdixit][22.07.2024]
                if (ObjLeavesViewModel != null)
                {
                    if (ObjLeavesViewModel.IsPlantChange)
                        SavechangesInLeaveGrid();
                }
                //TrainingPlansViewModel trainingPlansViewModel = new TrainingPlansViewModel();
                //trainingPlansViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.TrainingPlansView", trainingPlansViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateTrainingPlansView().... executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateTrainingPlansView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 26 View
        /// 30 Change
        /// 38 Admin
        /// 39 Watch All
        /// 40 Watch Organization
        /// 41 Watch Department
        /// </summary>
        /// <returns></returns>
        public PermissionManagement SelectUserPermission()
        {
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
             && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
             && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 38)
             && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return PermissionManagement.SuperAdmin;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
                 && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
                 && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return PermissionManagement.Admin;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
                  && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
                  && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 40))
            {
                return PermissionManagement.PlantViewer;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 26)
               && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 30)
               && GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 41))
            {
                return PermissionManagement.GlobalViewer;
            }

            return PermissionManagement.None;
        }
        private void SetUserPermission()  //[rdixit][16.01.2024][GEOS2-5074]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserPermission()...", category: Category.Info, priority: Priority.Low);
                switch (HrmCommon.Instance.UserPermission)
                {
                    case PermissionManagement.SuperAdmin:
                        GeosApplication.Instance.IsHRMPermissionEnabled = true;                     
                        break;

                    case PermissionManagement.Admin:
                        GeosApplication.Instance.IsHRMPermissionEnabled = true;                     
                        break;

                    case PermissionManagement.PlantViewer:
                        GeosApplication.Instance.IsHRMPermissionEnabled = false;              
                        break;

                    case PermissionManagement.GlobalViewer:
                        GeosApplication.Instance.IsHRMPermissionEnabled = false;                 
                        break;

                    default:
                        GeosApplication.Instance.IsHRMPermissionEnabled = false;                        
                        break;
                }
                GeosApplication.Instance.Logger.Log("Method SetUserPermission()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetUserPermission()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;
        }
        public int SelectIdUserPermission()
        {
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
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


        private void GetEmailExitEventJDList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetEmailExitEventJDList()...", category: Category.Info, priority: Priority.Low);

                List<GeosAppSetting> GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("55");

                if (GeosAppSettingList != null && GeosAppSettingList.Count > 0)
                {
                    HrmCommon.Instance.EmailExitEventJDList = Convert.ToString(GeosAppSettingList[0].DefaultValue);
                }
                GeosApplication.Instance.Logger.Log("Method GetEmailExitEventJDList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetEmailExitEventJDList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetEmailExitEventJDList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetEmailExitEventJDList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       //[GEOS2-5680][rdixit][22.07.2024]
        public void SavechangesInLeaveGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeaveGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {                 
                    if (ObjLeavesViewModel != null)
                    {
                        ObjLeavesViewModel.AcceptButtonAction(null);
                    }
                }      
                else
                {
                    ObjLeavesViewModel.IsPlantChange = false;
                }  
            }
            catch (Exception ex)
            {

            }
        }

		// [nsatpute][28-04-2025][GEOS2-6502]
        private void NavigateToCareerView()
        {
            try
            {
                IsAttendanceLoaded = false;
                GeosApplication.Instance.Logger.Log("Method NavigateToCareerView()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-5706][nsatpute][25.04.2025]              
                EmployeeRecruitmentViewModel employeeRecruitmentViewModel = new EmployeeRecruitmentViewModel();
                employeeRecruitmentViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeRecruitmentView", employeeRecruitmentViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateToCareerView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateToCareerView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
