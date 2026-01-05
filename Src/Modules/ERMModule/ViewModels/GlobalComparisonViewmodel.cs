using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
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

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class GlobalComparisonViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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
        #endregion
        #region Properties
        private string _selectedTop;
        public string SelectedTop
        {
            get { return _selectedTop; }
            set
            {
                _selectedTop = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTop"));

            }
        }
        private ObservableCollection<string> topList;

        public ObservableCollection<string> TopList
        {
            get { return topList; }
            set
            {
                topList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TopList"));
               
            }
        }


        private List<object> selectedRegion;
        public List<object> SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedRegionNotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
       
        private Duration _currentDuration;
        private bool isPeriod;
        public bool IsPeriod
        {
            get { return isPeriod; }
            set { isPeriod = value; }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        Visibility isCalendarVisible;
        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }

            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
            }
        }
        int isButtonStatus;


        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }
        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }
        private List<object> selectedcptype;
        public List<object> Selectedcptype
        {
            get { return selectedcptype; }
            set
            {
                selectedcptype = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Selectedcptype"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCptypeNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selecteddesignsystem;
        public List<object> Selecteddesignsystem
        {
            get { return selecteddesignsystem; }
            set
            {
                selecteddesignsystem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Selecteddesignsystem"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedDesignSystemNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selecteddesigntype;
        public List<object> SelecteddesignType
        {
            get { return selecteddesigntype; }
            set
            {
                selecteddesigntype = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelecteddesignType"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedDesignTypeNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selectedDSAStatus;
        public List<object> SelectedDSAStatus
        {
            get { return selectedDSAStatus; }
            set
            {
                selectedDSAStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDSAStatus"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedDSAStatusNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selectedTypeOfways;
        public List<object> SelectedTypeOfways
        {
            get { return selectedTypeOfways; }
            set
            {
                selectedTypeOfways = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTypeOfways"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedTypeOfwaysNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selectedNumOfways;
        public List<object> SelectedNumOfways
        {
            get { return selectedNumOfways; }
            set
            {
                selectedNumOfways = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNumOfways"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedNumOfwaysNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selectedDetection;
        public List<object> SelectedDetection
        {
            get { return selectedDetection; }
            set
            {
                selectedDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetection"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedDetectionNotEmpty"));
                UpdateColor();
            }
        }

        private List<object> selectedStage;
        public List<object> SelectedStage
        {
            get { return selectedStage; }
            set
            {
                selectedStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStage"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedStageNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selectedTemplate;
        public List<object> SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedTemplate"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedTemplateNotEmpty"));
                UpdateColor();
            }
        }
        private List<object> selectedOption;
        public List<object> SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOption"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOptionNotEmpty"));
                UpdateColor();
            }
        }
        private string lastYearStartDate;
        public string LastYearStartDate
        {
            get
            {
                return lastYearStartDate;
            }

            set
            {
                lastYearStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastYearStartDate"));
            }
        }

        private string currentYearStartDate;
        public string CurrentYearStartDate
        {
            get
            {
                return currentYearStartDate;
            }

            set
            {
                currentYearStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentYearStartDate"));
            }
        }
        private List<ERM_GlobalComparisonTimes> cpTypeList;
        public List<ERM_GlobalComparisonTimes> CpTypeList
        {
            get
            {
                return cpTypeList;
            }

            set
            {
                cpTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CpTypeList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> regionList;
        public List<ERM_GlobalComparisonTimes> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> stageList;
        public List<ERM_GlobalComparisonTimes> StageList
        {
            get
            {
                return stageList;
            }

            set
            {
                stageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StageList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> templateList;
        public List<ERM_GlobalComparisonTimes> TemplateList
        {
            get
            {
                return templateList;
            }

            set
            {
                templateList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> optionList;
        public List<ERM_GlobalComparisonTimes> OptionList
        {
            get
            {
                return optionList;
            }

            set
            {
                optionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> detectionList;
        public List<ERM_GlobalComparisonTimes> DetectionList
        {
            get
            {
                return detectionList;
            }

            set
            {
                detectionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> dsaStatusList;
        public List<ERM_GlobalComparisonTimes> DSAStatusList
        {
            get
            {
                return dsaStatusList;
            }

            set
            {
                dsaStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DSAStatusList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> designSystemList;
        public List<ERM_GlobalComparisonTimes> DesignSystemList
        {
            get
            {
                return designSystemList;
            }

            set
            {
                designSystemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DesignSystemList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> designtypeList;
        public List<ERM_GlobalComparisonTimes> DesignTypeList
        {
            get
            {
                return designtypeList;
            }

            set
            {
                designtypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DesigntypeList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> typeOfWaysList;
        public List<ERM_GlobalComparisonTimes> TypeOfWaysList
        {
            get
            {
                return typeOfWaysList;
            }

            set
            {
                typeOfWaysList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeOfWaysList"));
            }
        }
        private List<ERM_GlobalComparisonTimes> numOfWaysList;
        public List<ERM_GlobalComparisonTimes> NumOfWaysList
        {
            get
            {
                return numOfWaysList;
            }

            set
            {
                numOfWaysList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NumOfWaysList"));
            }
        }
        DateTime fromDate;
        DateTime toDate;
        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }
        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }
        List<string> failedPlants;
        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        DateTime startDate;
        DateTime endDate;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        private ObservableCollection<ERM_GlobalComparisonTimes> globalComparisonList_cloned;

        public ObservableCollection<ERM_GlobalComparisonTimes> GlobalComparisonList_cloned
        {
            get
            {
                return globalComparisonList_cloned;
            }

            set
            {
                globalComparisonList_cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GlobalComparisonList_Cloned"));
            }
        }
        private ObservableCollection<ERM_GlobalComparisonTimes> globalComparisonList;

        public ObservableCollection<ERM_GlobalComparisonTimes> GlobalComparisonList
        {
            get
            {
                return globalComparisonList;
            }

            set
            {
                globalComparisonList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GlobalComparisonList"));
            }
        }

        public virtual bool DialogResult { get; protected set; }//Aishwarya Ingale[Geos2-6715]
        public virtual string ResultFileName { get; protected set; }//Aishwarya Ingale[Geos2-6715]
        #endregion
        #region ICommand
        public ICommand RefreshGlobalComparisonCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCpTypeCommand { get; set; }
        public ICommand ChangeDesignSystemCommand { get; set; }
        public ICommand ChangeDesignTypeCommand { get; set; }
        public ICommand ChangeTopCommand { get; set; }

        public ICommand ChangeStageCommand { get; set; }
        public ICommand ChangeTemplateCommand { get; set; }
        public ICommand ChangeDSAStatusCommand { get; set; }
        public ICommand ChangeTypeOfWaysCommand { get; set; }
        public ICommand ChangeNumOFWaysCommand { get; set; }

        public ICommand ExportGolbalComparisonCommand { get; set; }
        #endregion
        #region Constructor
        public GlobalComparisonViewModel()
        {
            ChangeRegionCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeRegionCommandAction);
            ChangeCpTypeCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCpTypeCommandAction);
            ChangeDesignSystemCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeDesignSystemCommandAction);
            ChangeDesignTypeCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeDesignTypeCommandAction);
            ChangeDSAStatusCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeDSAStatusCommandAction);
            ChangeTypeOfWaysCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeTypeOfWaysCommandActions);
            ChangeNumOFWaysCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeNumOfWaysCommandActions);
            ChangeTopCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeTopCommandActions);
            ChangeStageCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeStageCommandAction);
            ChangeTemplateCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeTemplateCommandAction);
            ExportGolbalComparisonCommand = new DevExpress.Mvvm.DelegateCommand<object>(ExportGolbalComparisonCommandAction);
            RefreshGlobalComparisonCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshGlobalComparisonCommandAction);
            PeriodCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCommandAction);
            ApplyCommand = new DevExpress.Mvvm.DelegateCommand<object>(ApplyCommandAction);
            CancelCommand = new DevExpress.Mvvm.DelegateCommand<object>(CancelCommandAction);
            PeriodCustomRangeCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCustomRangeCommandAction);
            ChangePlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangePlantCommandAction);
            SelectedTop = "5";
            TopList = new ObservableCollection<string> { "1", "2", "3" ,"4","5","6","7","8","9","10"};
        }

        private void ChangeTopCommandActions(object obj)
        {
            GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>();

            if (obj == null)
            {


            }
            else
            {

                DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                {
                    //return;
                }
                else
                {
                    GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                    var TempSelectedTop= (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);

                    // Explicitly cast EditValue to string
                    SelectedTop = TempSelectedTop as string;

                    // Alternatively, check for null values
                    if (TempSelectedTop != null)
                    {
                        SelectedTop = TempSelectedTop.ToString();
                    }
                    else
                    {
                        SelectedTop = null; // Handle null case as per your logic
                    }

                    if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        try
                        {

                            //GlobalComparisonList.AddRange(ERMService.GetGlobalComparisonTimesResults_V2590(startDate, endDate, TmpSelectedPlant, IdSite));
                            //GlobalComparisonList_cloned = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList);


                            GetGlobalComparison(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate,SelectedTop);
                           
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                        }
                    }
                    else
                    {

                    }


                }
            }

        }
        #endregion
        #region Methods
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                int CurrentYear = DateTime.Now.Year;
                int LastYear = DateTime.Now.Year - 1;
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 12, 31);
                LastYearStartDate = TempLastYearStartDate.ToString("dd/MM/yyyy");
                CurrentYearStartDate = TempCurrentYearStartDate.ToString("dd/MM/yyyy");
                setDefaultPeriod();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                FillGlobalCmparison();
                Fillfilter();

                FillAllselectedFilter();
                //FillTop();
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RefreshGlobalComparisonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails ...", category: Category.Info, priority: Priority.Low);


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

                FillGlobalCmparison();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDashboardDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void SelectAllData()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SelectAllData ...", category: Category.Info, priority: Priority.Low);

                SelectedRegion = new List<object>(RegionList.ToList());
            }
            catch (Exception ex)
            {
            }
        }
        private void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;

            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            // IsCalendarVisible = Visibility.Collapsed;
        }
        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }
        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;
                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    //FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = thisMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = thisMonthStart;
                    ToDate = thisMonthEnd;
                    PlanningSchedulerControl scheduler = new PlanningSchedulerControl();
                    DateTime start = Convert.ToDateTime(FromDate);
                    DateTime end = Convert.ToDateTime(FromDate).AddDays(1);
                    scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);

                    DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(FromDate).Year), Convert.ToDateTime(FromDate).Month, 1);
                    scheduler.Month = startDate;
                    scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(Convert.ToDateTime(FromDate), Convert.ToDateTime(FromDate).AddDays(1));
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    //FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = lastOneMonthStart;
                    ToDate = lastOneMonthEnd;
                }
                else if (IsButtonStatus == 3) //last month
                {
                    //FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = lastMonthStart;
                    ToDate = lastMonthEnd;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    //FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    //ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = thisWeekStart;
                    ToDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    //    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    //    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = thisWeekStart;
                    ToDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    //FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    //ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = lastWeekStart;
                    ToDate = lastWeekEnd;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    //FromDate = StartDate.ToString("dd/MM/yyyy");
                    //ToDate = EndDate.ToString("dd/MM/yyyy");
                    FromDate = StartDate;
                    ToDate = EndDate;
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    //ToDate = EndToDate.ToString("dd/MM/yyyy");
                    //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    //ToDate = EndToDate.ToString("dd/MM/yyyy");
                    FromDate = StartFromDate;
                    ToDate = EndToDate;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    //FromDate = Date_F.ToString("dd/MM/yyyy");
                    //ToDate = Date_T.ToString("dd/MM/yyyy");
                    FromDate = Date_F;
                    ToDate = Date_T;

                }


                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    GetGlobalComparison(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate,SelectedTop);
                    Fillfilter();
                    FillAllselectedFilter();
                }
                //   FillDVManagementProduction();
                IsBusy = false;
                IsPeriod = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private Action Processing()
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
            return null;
        }
        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }
        private void setDefaultPeriod()
        {

            int year = DateTime.Now.Year;
            DateTime StartFromDate = new DateTime(year, 1, 1);
            DateTime EndToDate = new DateTime(year, 12, 31);
            //FromDate = StartFromDate.ToString("dd/MM/yyyy");
            //ToDate = EndToDate.ToString("dd/MM/yyyy");
            FromDate = StartFromDate;
            ToDate = EndToDate;
        }
        private void FillOption()
        {
            try
            {
                OptionList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedOption = new ERM_GlobalComparisonTimes();
                    selectedOption.Option = item.Option;
                    OptionList.Add(selectedOption);

                }


                var TempOptionList1 = (from a in OptionList
                                                      select new
                                                      {
                                                          a.Option


                                                      }).Distinct().ToList();
                OptionList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempOptionList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.Option = item1.Option;


                    OptionList.Add(selectedvalue);

                }
                OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);
               

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOption()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDetection()
        {
            try
            {
                DetectionList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selecteddetections = new ERM_GlobalComparisonTimes();
                    selecteddetections.Detection = item.Detection;
                    DetectionList.Add(selecteddetections);

                }


                var TempDetectionList1 = (from a in DetectionList
                                                      select new
                                                      {
                                                          a.Detection


                                                      }).Distinct().ToList();
                DetectionList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempDetectionList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.Detection = item1.Detection;
                    DetectionList.Add(selectedvalue);

                }
                DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDetection()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillNumOfWays()
        {
            try
            {
                NumOfWaysList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedNumOfWays = new ERM_GlobalComparisonTimes();
                    selectedNumOfWays.NoOfWays = item.NoOfWays;
                    NumOfWaysList.Add(selectedNumOfWays);

                }


                var TempNumOfWaysList1 = (from a in NumOfWaysList
                                                      select new
                                                      {
                                                          a.NoOfWays


                                                      }).Distinct().ToList();
                NumOfWaysList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempNumOfWaysList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.NoOfWays = item1.NoOfWays;
                    NumOfWaysList.Add(selectedvalue);
                }
                NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillNumOfWays()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTypeOFways()
        {
            try
            {
                TypeOfWaysList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedtypeofWays = new ERM_GlobalComparisonTimes();
                    selectedtypeofWays.TypeOfways = item.TypeOfways;
                    TypeOfWaysList.Add(selectedtypeofWays);

                }


                var TempTypeOfWaysList1 = (from a in TypeOfWaysList
                                                      select new
                                                      {
                                                          a.TypeOfways


                                                      }).Distinct().ToList();
                TypeOfWaysList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempTypeOfWaysList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.TypeOfways = item1.TypeOfways;


                    TypeOfWaysList.Add(selectedvalue);

                }
                TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
              
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeOfWays()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDSASystem()
        {
            try
            {
                DSAStatusList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedcptype = new ERM_GlobalComparisonTimes();
                    selectedcptype.DsaStatus = item.DsaStatus;
                    DSAStatusList.Add(selectedcptype);

                }


                var TempDSAStatusList1 = (from a in DSAStatusList
                                                      select new
                                                      {
                                                          a.DsaStatus


                                                      }).Distinct().ToList();
                DSAStatusList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempDSAStatusList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.DsaStatus = item1.DsaStatus;
                    DSAStatusList.Add(selectedvalue);
                }
                DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
               

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDSASystem()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDesigntype()
        {
            try
            {
                DesignTypeList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selecteddesignType = new ERM_GlobalComparisonTimes();
                    selecteddesignType.DesignType = item.DesignType;
                    DesignTypeList.Add(selecteddesignType);

                }


                var TempDesignTypeList1 = (from a in DesignTypeList
                                                      select new
                                                      {
                                                          a.DesignType


                                                      }).Distinct().ToList();
                DesignTypeList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempDesignTypeList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.DesignType = item1.DesignType;


                    DesignTypeList.Add(selectedvalue);

                }
                DesignTypeList = new List<ERM_GlobalComparisonTimes>(DesignTypeList);
                
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDesignType()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDesignSystem()
        {
            try
            {
                DesignSystemList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selecteddesignsystem = new ERM_GlobalComparisonTimes();
                    selecteddesignsystem.DesignSystem = item.DesignSystem;
                    DesignSystemList.Add(selecteddesignsystem);

                }


                var TempDesignSystemList1 = (from a in DesignSystemList
                                             select new
                                                      {
                                                          a.DesignSystem


                                                      }).Distinct().ToList();
                DesignSystemList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempDesignSystemList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.DesignSystem = item1.DesignSystem;
                    DesignSystemList.Add(selectedvalue);
                }
                DesignSystemList= new List<ERM_GlobalComparisonTimes>(DesignSystemList);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDesignSystem()", category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangePlantCommandAction(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>();

                if (obj == null)
                {


                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                        //return;
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                        var TempSelectedPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedPlant = new List<object>();

                        foreach (var tmpPlant in (dynamic)TempSelectedPlant)
                        {
                            TmpSelectedPlant.Add(tmpPlant);
                        }

                        ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                        ERMCommon.Instance.SelectedAuthorizedPlantsList = TmpSelectedPlant;

                        if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            try
                            {
                                GetGlobalComparison(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate,SelectedTop);
                                Fillfilter();
                                FillAllselectedFilter();
                                //SelectAllData();
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                            }
                        }
                        else
                        {

                        }


                    }
                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetGlobalComparison(List<object> SelectedPlant, DateTime FromDate, DateTime ToDate,string  limit)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method GetGlobalComparison ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                         //   ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);
                            GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>();
                            
                            //GlobalComparisonList.AddRange(ERMService.GetGlobalComparisonTimesResults_V2590(FromDate, ToDate, Convert.ToInt16(limit), IdSite));
                            GlobalComparisonList_cloned = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList);

                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillERM_GlobalComparisonTimes() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in GetGlobalComparison()", category: Category.Exception, priority: Priority.Low);
            }
            //GetGlobalComparisonTimesResults_V2590
        }
        private void FillGlobalCmparison()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillGlobalCmparison ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                         //   ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);
                            GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>();
                            DateTime startDate = FromDate;
                            DateTime endDate = ToDate;

                            //GlobalComparisonList.AddRange(ERMService.GetGlobalComparisonTimesResults_V2590(startDate, endDate, Convert.ToInt16(SelectedTop), IdSite));
                            GlobalComparisonList_cloned = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList);


                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillERM_GlobalComparisonTimes() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillGlobalCmparison()", category: Category.Exception, priority: Priority.Low);
            }
            //GetGlobalComparisonTimesResults_V2590
        }
        private void Fillfilter()
        {
            try
            {
                FillRegion();
                FillStage();
                FillTemplate();
                FillCptype();
                FillDesignSystem();
                FillDesigntype();
                FillDSASystem();
                FillTypeOFways();
                FillNumOfWays();
                FillDetection();
                FillOption();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void   FillAllselectedFilter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()...", category: Category.Info, priority: Priority.Low);
                if (SelectedRegion == null) SelectedRegion = new List<object>();
                SelectedRegion = new List<object>(RegionList.ToList());

                if (SelectedStage == null) SelectedStage = new List<object>();
                SelectedStage = new List<object>(StageList.ToList());

                if (SelectedTemplate == null) SelectedTemplate = new List<object>();
                SelectedTemplate = new List<object>(TemplateList.ToList());

                if (Selectedcptype == null) Selectedcptype = new List<object>();
                Selectedcptype = new List<object>(CpTypeList.ToList());

                if (SelecteddesignType == null) SelecteddesignType = new List<object>();
                SelecteddesignType = new List<object>(DesignTypeList.ToList());

                if (Selecteddesignsystem == null) Selecteddesignsystem = new List<object>();
                Selecteddesignsystem = new List<object>(DesignSystemList.ToList());

                if (SelectedNumOfways == null) SelectedNumOfways = new List<object>();
                SelectedNumOfways = new List<object>(NumOfWaysList.ToList());

                if (SelectedDSAStatus == null) SelectedDSAStatus = new List<object>();
                SelectedDSAStatus = new List<object>(DSAStatusList.ToList());

                if (SelectedDetection == null) SelectedDetection = new List<object>();
                SelectedDetection = new List<object>(DetectionList.ToList());

                if (SelectedTypeOfways == null) SelectedTypeOfways = new List<object>();
                SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());

                if (SelectedOption == null) SelectedOption = new List<object>();
                SelectedOption = new List<object>(OptionList.ToList());

                ApplyFilterConditions();

            }
            catch (Exception Ex)
            {

            }
            }
        private void FillRegion()
        {
            try
            {
                RegionList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedRegion = new ERM_GlobalComparisonTimes();
                    selectedRegion.Region = item.Region;
                    RegionList.Add(selectedRegion);

                }
                var TempRegionList1 = (from a in RegionList
                                       select new
                                       {
                                           a.Region
                                       }).Distinct().ToList();
                RegionList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempRegionList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.Region = item1.Region;
                    RegionList.Add(selectedvalue);
                }
                RegionList = new List<ERM_GlobalComparisonTimes>(RegionList);
               
            }
            catch (Exception ex)
            {

            }
        }
        private void FillStage()
        {
            try
            {
                StageList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedStage = new ERM_GlobalComparisonTimes();
                    selectedStage.StageName = item.StageName;
                    StageList.Add(selectedStage);

                }
                var TempStageList1 = (from a in StageList
                                       select new
                                       {
                                           a.StageName
                                       }).Distinct().ToList();
                StageList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempStageList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.StageName = item1.StageName;
                    StageList.Add(selectedvalue);
                }
                StageList = new List<ERM_GlobalComparisonTimes>(StageList);

            }
            catch (Exception ex)
            {

            }
        }
        private void FillTemplate()
        {
            try
            {
                TemplateList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedTemplate = new ERM_GlobalComparisonTimes();
                    selectedTemplate.TemplateName = item.TemplateName;
                    TemplateList.Add(selectedTemplate);

                }
                var TempTemplateList1 = (from a in TemplateList
                                       select new
                                       {
                                           a.TemplateName
                                       }).Distinct().ToList();
                TemplateList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempTemplateList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.TemplateName = item1.TemplateName;
                    TemplateList.Add(selectedvalue);
                }
                TemplateList = new List<ERM_GlobalComparisonTimes>(TemplateList);

            }
            catch (Exception ex)
            {

            }
        }
        private void FillCptype()
        {
            try
            {
                CpTypeList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item in GlobalComparisonList)
                {
                    ERM_GlobalComparisonTimes selectedcptype = new ERM_GlobalComparisonTimes();
                    selectedcptype.CpType = item.CpType;
                    CpTypeList.Add(selectedcptype);

                }


                var TempCptypeList1 = (from a in CpTypeList
                                                      select new
                                                      {
                                                          a.CpType


                                                      }).Distinct().ToList();
                CpTypeList = new List<ERM_GlobalComparisonTimes>();

                foreach (var item1 in TempCptypeList1)
                {
                    ERM_GlobalComparisonTimes selectedvalue = new ERM_GlobalComparisonTimes();
                    selectedvalue.CpType = item1.CpType;


                    CpTypeList.Add(selectedvalue);

                }
                CpTypeList = new List<ERM_GlobalComparisonTimes>(CpTypeList);
                //Region_Cloned = new List<ERM_GlobalComparisonTimes>();
                //Region_Cloned = RegionList.ToList();
                // List<ERM_GlobalComparisonTimes> TEmpRegion = new List<ERM_GlobalComparisonTimes>(RegionList.Select(a => a.Region).Distinct().ToList());


                //List<ERM_GlobalComparisonTimes> tempRegion = RegionList.Select(a => a.Region).Distinct();
                // RegionList = new List<ERM_GlobalComparisonTimes>(tempRegion);

            }
            catch (Exception ex)
            {

            }
        }
        public bool IsSelectedCptypeNotEmpty
        {
            get
            {
                return Selectedcptype != null && Selectedcptype.Count > 0 && Selectedcptype.Count != CpTypeList.Count;
            }

        }
        public bool IsSelectedNumOfWaysNotEmpty
        {
            get
            {
                return SelectedNumOfways != null && SelectedNumOfways.Count > 0 && SelectedNumOfways.Count != NumOfWaysList.Count;
            }

        }
        public bool IsSelectedDesignSystemNotEmpty
        {
            get
            {
                return Selecteddesignsystem != null && Selecteddesignsystem.Count > 0 && Selecteddesignsystem.Count != DesignSystemList.Count;
            }
        }
        public bool IsSelectedRegionNotEmpty
        {
            get
            {
                return SelectedRegion != null && SelectedRegion.Count > 0 && SelectedRegion.Count != RegionList.Count;
            }

        }
        
        public bool IsSelectedDetectionNotEmpty
        {
            get
            {
                return SelectedDetection != null && SelectedDetection.Count > 0 && SelectedDetection.Count != DetectionList.Count;
            }

        }
        public bool IsSelectedStageNotEmpty
        {
            get
            {
                return SelectedStage != null && SelectedStage.Count > 0 && SelectedStage.Count != StageList.Count;
            }

        }
        public bool IsSelectedTemplateNotEmpty
        {
            get
            {
                return SelectedTemplate != null && SelectedTemplate.Count > 0 && SelectedTemplate.Count != TemplateList.Count;
            }

        }
        public bool IsSelectedDesignTypeNotEmpty
        {
            get
            {
                return SelecteddesignType != null && SelecteddesignType.Count > 0 && SelecteddesignType.Count != DesignTypeList.Count;
            }

        }
        public bool IsSelectedOptionNotEmpty
        {
            get
            {
                return SelectedOption != null && SelectedOption.Count > 0 && SelectedOption.Count != OptionList.Count;
            }

        }
        
        public bool IsSelectedDSAStatusNotEmpty
        {
            get
            {
                return SelectedDSAStatus != null && SelectedDSAStatus.Count > 0 && SelectedDSAStatus.Count != DSAStatusList.Count;
            }

        }
        public bool IsSelectedTypeOfwaysNotEmpty
        {
            get
            {
                return SelectedTypeOfways != null && SelectedTypeOfways.Count > 0 && SelectedTypeOfways.Count != TypeOfWaysList.Count;
            }

        }
        

        private void UpdateColor()
        {
            if (IsSelectedRegionNotEmpty || IsSelectedCptypeNotEmpty|| IsSelectedDesignSystemNotEmpty||IsSelectedDetectionNotEmpty ||
IsSelectedOptionNotEmpty ||
IsSelectedTypeOfwaysNotEmpty ||
IsSelectedDesignTypeNotEmpty ||
IsSelectedDSAStatusNotEmpty || IsSelectedNumOfWaysNotEmpty|| IsSelectedStageNotEmpty|| IsSelectedTemplateNotEmpty)
            {
                Color = "Black";
            }
            else
            {
                Color = "White"; // or any other default color
            }
        }
        private void ChangeRegionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedRegion = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedRegion = new List<object>();

                        if (TempSelectedRegion != null)
                        {
                            foreach (var tmpRegion in (dynamic)TempSelectedRegion)
                            {
                                TmpSelectedRegion.Add(tmpRegion);
                            }

                            SelectedRegion = new List<object>();
                            SelectedRegion = TmpSelectedRegion;
                        }

                        if (SelectedRegion == null) SelectedRegion = new List<object>();

                        List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).Region)).Distinct().ToList();
                        // List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());
                        //[Aishwarya Ingale][Geos2-5918]
                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => RegionIds.Contains(Convert.ToString(i.Region))).Distinct().ToList());
                        CpTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);

                        List<ERM_GlobalComparisonTimes> TempCptypeList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempdesignsystemList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempdesigntypeList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDSAList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();


                        TempCptypeList.AddRange(CpTypeList.GroupBy(i => i.CpType).Select(grp => grp.First()).ToList().Distinct());
                        TempdesignsystemList.AddRange(DesignSystemList.GroupBy(i => i.DesignSystem).Select(grp => grp.First()).ToList().Distinct());
                        TempdesigntypeList.AddRange(DesignTypeList.GroupBy(i => i.DesignType).Select(grp => grp.First()).Distinct().ToList()); 

                        TempDSAList.AddRange(DSAStatusList.GroupBy(i => i.DsaStatus).Select(grp => grp.First()).ToList().Distinct());
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());

                        CpTypeList = new List<ERM_GlobalComparisonTimes>(TempCptypeList);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempdesignsystemList);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(TempdesigntypeList);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);
                        
                        Selectedcptype = new List<object>(CpTypeList.ToList());
                        Selecteddesignsystem = new List<object>(DesignSystemList.ToList());
                        SelectedDSAStatus = new List<object>(DSAStatusList.ToList());
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        SelecteddesignType = new List<object>(DesignTypeList.ToList());

                        ApplyFilterConditions();


                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerGroupCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeStageCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeStageCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedStage = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedStages = new List<object>();

                        if (TempSelectedStage != null)
                        {
                            foreach (var tmpRegion in (dynamic)TempSelectedStage)
                            {
                                TempSelectedStages.Add(tmpRegion);
                            }

                            SelectedStage = new List<object>();
                            SelectedStage = TempSelectedStages;
                        }

                        if (SelectedStage == null) SelectedStage = new List<object>();

                        List<string> stagess = SelectedStage.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).StageName)).Distinct().ToList();
                        
                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => stagess.Contains(Convert.ToString(i.StageName)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());
                        TemplateList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        CpTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);

                        List<ERM_GlobalComparisonTimes> TempcptypList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptemplateList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDesignSystemList = new List<ERM_GlobalComparisonTimes>();
                        TempDesignSystemList.AddRange(DesignSystemList.GroupBy(i => i.DesignSystem)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempDesignSystemList.Where(a => a.DesignSystem != null).ToList());

                       
                        List<ERM_GlobalComparisonTimes> TempdesigntypeList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDSAList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();


                        TempcptypList.AddRange(CpTypeList.GroupBy(i => i.CpType).Select(grp => grp.First()).Distinct().ToList());
                        TemptemplateList.AddRange(TemplateList.GroupBy(i => i.TemplateName).Select(grp => grp.First()).Distinct().ToList());

                        TempdesigntypeList.AddRange(DesignTypeList.GroupBy(i => i.DesignType).Select(grp => grp.First()).Distinct().ToList());

                        TempDSAList.AddRange(DSAStatusList.GroupBy(i => i.DsaStatus).Select(grp => grp.First()).ToList().Distinct());
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());

                        TemplateList = new List<ERM_GlobalComparisonTimes>(TemptemplateList);
                        CpTypeList = new List<ERM_GlobalComparisonTimes>(TempcptypList);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempDesignSystemList);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(TempdesigntypeList);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);

                        SelectedTemplate = new List<object>(TemplateList.ToList());
                        Selectedcptype = new List<object>(CpTypeList.ToList());
                        Selecteddesignsystem = new List<object>(DesignSystemList.ToList());
                        SelectedDSAStatus = new List<object>(DSAStatusList.ToList());
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        SelecteddesignType = new List<object>(DesignTypeList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCpTypeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCpTypeCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeTemplateCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeTemplateCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedTemplate = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedTemplates = new List<object>();

                        if (TempSelectedTemplate != null)
                        {
                            foreach (var tmpRegion in (dynamic)TempSelectedTemplate)
                            {
                                TempSelectedTemplates.Add(tmpRegion);
                            }

                            SelectedTemplate = new List<object>();
                            SelectedTemplate = TempSelectedTemplates;
                        }

                        if (SelectedTemplate == null) SelectedTemplate = new List<object>();

                        List<string> templt = SelectedTemplate.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).TemplateName)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => templt.Contains(Convert.ToString(i.TemplateName)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());
                       
                        CpTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);

                        List<ERM_GlobalComparisonTimes> TempcptypList = new List<ERM_GlobalComparisonTimes>();
                        
                        List<ERM_GlobalComparisonTimes> TempDesignSystemList = new List<ERM_GlobalComparisonTimes>();
                        TempDesignSystemList.AddRange(DesignSystemList.GroupBy(i => i.DesignSystem)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempDesignSystemList.Where(a => a.DesignSystem != null).ToList());


                        List<ERM_GlobalComparisonTimes> TempdesigntypeList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDSAList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();


                        TempcptypList.AddRange(CpTypeList.GroupBy(i => i.CpType).Select(grp => grp.First()).Distinct().ToList());
                       
                        TempdesigntypeList.AddRange(DesignTypeList.GroupBy(i => i.DesignType).Select(grp => grp.First()).Distinct().ToList());

                        TempDSAList.AddRange(DSAStatusList.GroupBy(i => i.DsaStatus).Select(grp => grp.First()).ToList().Distinct());
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());
                        
                        CpTypeList = new List<ERM_GlobalComparisonTimes>(TempcptypList);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempDesignSystemList);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(TempdesigntypeList);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);
                        
                        Selectedcptype = new List<object>(CpTypeList.ToList());
                        Selecteddesignsystem = new List<object>(DesignSystemList.ToList());
                        SelectedDSAStatus = new List<object>(DSAStatusList.ToList());
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        SelecteddesignType = new List<object>(DesignTypeList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeTemplateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeTemplateCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeCpTypeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCpTypeCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCpType = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedCpTypes = new List<object>();

                        if (TempSelectedCpType != null)
                        {
                            foreach (var tmpRegion in (dynamic)TempSelectedCpType)
                            {
                                TempSelectedCpTypes.Add(tmpRegion);
                            }

                            Selectedcptype = new List<object>();
                            Selectedcptype = TempSelectedCpTypes;
                        }

                        if (Selectedcptype == null) Selectedcptype = new List<object>();

                        List<string> CpTypess = Selectedcptype.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).CpType)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => CpTypess.Contains(Convert.ToString(i.CpType)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        List<ERM_GlobalComparisonTimes> TempDesignSystemList = new List<ERM_GlobalComparisonTimes>();
                        TempDesignSystemList.AddRange(DesignSystemList.GroupBy(i => i.DesignSystem)
                                                                                  .Select(grp => grp.First())
                                                                                  .ToList().Distinct());

                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempDesignSystemList.Where(a => a.DesignSystem != null).ToList());


                        List<ERM_GlobalComparisonTimes> TempdesigntypeList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDSAList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();



                        TempdesigntypeList.AddRange(DesignTypeList.GroupBy(i => i.DesignType).Select(grp => grp.First()).Distinct().ToList());

                        TempDSAList.AddRange(DSAStatusList.GroupBy(i => i.DsaStatus).Select(grp => grp.First()).ToList().Distinct());
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());

                        //CpTypeList = new List<ERM_GlobalComparisonTimes>(TempCptypeList);
                        DesignSystemList = new List<ERM_GlobalComparisonTimes>(TempDesignSystemList);
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(TempdesigntypeList);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);

                        Selecteddesignsystem = new List<object>(DesignSystemList.ToList());
                        SelectedDSAStatus = new List<object>(DSAStatusList.ToList());
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        SelecteddesignType = new List<object>(DesignTypeList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCpTypeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCpTypeCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeDesignSystemCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeDesignSystemCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedDesignSystem = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedDesignSystems = new List<object>();

                        if (TempSelectedDesignSystem != null)
                        {
                            foreach (var tmpdesignSystem in (dynamic)TempSelectedDesignSystem)
                            {
                                TempSelectedDesignSystems.Add(tmpdesignSystem);
                            }

                            Selecteddesignsystem = new List<object>();
                            Selecteddesignsystem = TempSelectedDesignSystems;
                        }

                        if (Selecteddesignsystem == null) Selecteddesignsystem = new List<object>();

                        List<string> DesignSystemm = Selecteddesignsystem.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DesignSystem)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => DesignSystemm.Contains(Convert.ToString(i.DesignSystem)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());

                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        

                        List<ERM_GlobalComparisonTimes> TempdesigntypeList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDSAList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();



                        TempdesigntypeList.AddRange(DesignTypeList.GroupBy(i => i.DesignType).Select(grp => grp.First()).Distinct().ToList());

                        TempDSAList.AddRange(DSAStatusList.GroupBy(i => i.DsaStatus).Select(grp => grp.First()).ToList().Distinct());
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());
                        
                        DesignTypeList = new List<ERM_GlobalComparisonTimes>(TempdesigntypeList);
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);
                        
                        SelectedDSAStatus = new List<object>(DSAStatusList.ToList());
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        SelecteddesignType = new List<object>(DesignTypeList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeDesignSystemCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeDesignSystemCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeDesignTypeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeDesignTypeCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedDesignType = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedDesignTypes = new List<object>();

                        if (TempSelectedDesignType != null)
                        {
                            foreach (var tmpdesignType in (dynamic)TempSelectedDesignType)
                            {
                                TempSelectedDesignTypes.Add(tmpdesignType);
                            }

                            SelecteddesignType = new List<object>();
                            SelecteddesignType = TempSelectedDesignTypes;
                        }

                        if (SelecteddesignType == null) SelecteddesignType = new List<object>();

                        List<string> DesignTypee = SelecteddesignType.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DesignType)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => DesignTypee.Contains(Convert.ToString(i.DesignType)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());
                        
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);

                        
                        List<ERM_GlobalComparisonTimes> TempDSAList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();


                        
                        TempDSAList.AddRange(DSAStatusList.GroupBy(i => i.DsaStatus).Select(grp => grp.First()).ToList().Distinct());
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());
                        
                        DSAStatusList = new List<ERM_GlobalComparisonTimes>(DSAStatusList);
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);

                        SelectedDSAStatus = new List<object>(DSAStatusList.ToList());
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        SelecteddesignType = new List<object>(DesignTypeList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeDesignTypeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeDesignTypeCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeDSAStatusCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeDSAStatusCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedDSAStatus = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedDSAStatuss= new List<object>();

                        if (TempSelectedDSAStatus != null)
                        {
                            foreach (var tmpdSAStatus in (dynamic)TempSelectedDSAStatus)
                            {
                                TempSelectedDSAStatuss.Add(tmpdSAStatus);
                            }

                            SelectedDSAStatus = new List<object>();
                            SelectedDSAStatus = TempSelectedDSAStatuss;
                        }

                        if (SelectedDSAStatus == null) SelectedDSAStatus = new List<object>();

                        List<string> DSAStatuss = SelectedDSAStatus.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DsaStatus)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => DSAStatuss.Contains(Convert.ToString(i.DsaStatus)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());
                        
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);


                        
                        List<ERM_GlobalComparisonTimes> TemptypeofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();


                        
                        TemptypeofwaysList.AddRange(TypeOfWaysList.GroupBy(i => i.TypeOfways).Select(grp => grp.First()).ToList().Distinct());
                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempnumofwaysList.AddRange(NumOfWaysList.GroupBy(i => i.NoOfWays).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());

                        
                        TypeOfWaysList = new List<ERM_GlobalComparisonTimes>(TypeOfWaysList);
                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);
                        
                        SelectedTypeOfways = new List<object>(TypeOfWaysList.ToList());
                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeDSAStatusCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeDSAStatusCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeTypeOfWaysCommandActions(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeTypeOfWaysCommandActions()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedtypeOfWays = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectedtypeOfWay = new List<object>();

                        if (TempSelectedtypeOfWays != null)
                        {
                            foreach (var tmpTypeOfWays in (dynamic)TempSelectedtypeOfWays)
                            {
                                TempSelectedtypeOfWay.Add(tmpTypeOfWays);
                            }

                            SelectedTypeOfways = new List<object>();
                            SelectedTypeOfways = TempSelectedtypeOfWay;
                        }

                        if (SelectedTypeOfways == null) SelectedTypeOfways = new List<object>();

                        List<string> TypeOfWayss = SelectedTypeOfways.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).TypeOfways)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => TypeOfWayss.Contains(Convert.ToString(i.DsaStatus)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());

                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);




                        List<ERM_GlobalComparisonTimes> TempnumofwaysList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();




                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                        TempnumofwaysList.AddRange(NumOfWaysList.GroupBy(i => i.NoOfWays).Select(grp => grp.First()).ToList().Distinct());
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());

                        NumOfWaysList = new List<ERM_GlobalComparisonTimes>(NumOfWaysList);
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);

                        SelectedNumOfways = new List<object>(NumOfWaysList.ToList());
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeTypeOFwaysCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeDSAStatusCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeNumOfWaysCommandActions(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeTypeOfWaysCommandActions()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedNumOfWays = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TempSelectednumOfWay = new List<object>();

                        if (TempSelectedNumOfWays != null)
                        {
                            foreach (var tmpnumOfWays in (dynamic)TempSelectedNumOfWays)
                            {
                                TempSelectednumOfWay.Add(tmpnumOfWays);
                            }

                            SelectedNumOfways = new List<object>();
                            SelectedNumOfways = TempSelectednumOfWay;
                        }

                        if (SelectedNumOfways == null) SelectedNumOfways = new List<object>();

                        List<string> NumOfWayss = SelectedNumOfways.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).NoOfWays)).Distinct().ToList();

                        List<ERM_GlobalComparisonTimes> filteredData = new List<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => NumOfWayss.Contains(Convert.ToString(i.DsaStatus)) && SelectedRegion.Select(j => Convert.ToString((j as ERM_GlobalComparisonTimes).Region)).Contains(Convert.ToString(i.Region))).ToList());
                        
                        DetectionList = new List<ERM_GlobalComparisonTimes>(filteredData);
                        OptionList = new List<ERM_GlobalComparisonTimes>(filteredData);

                        
                        
                        List<ERM_GlobalComparisonTimes> TempDetectionList = new List<ERM_GlobalComparisonTimes>();
                        List<ERM_GlobalComparisonTimes> TempOptionList = new List<ERM_GlobalComparisonTimes>();




                        TempDetectionList.AddRange(DetectionList.GroupBy(i => i.Detection).Select(grp => grp.First()).ToList().Distinct());
                       
                        TempOptionList.AddRange(OptionList.GroupBy(i => i.NoOfOption).Select(grp => grp.First()).ToList().Distinct());
                        
                        DetectionList = new List<ERM_GlobalComparisonTimes>(DetectionList);
                        OptionList = new List<ERM_GlobalComparisonTimes>(OptionList);
                        
                        SelectedDetection = new List<object>(DetectionList.ToList());
                        SelectedOption = new List<object>(OptionList.ToList());
                        ApplyFilterConditions();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeNumOFwaysCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeNumOfWaysCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ApplyFilterConditions()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()...", category: Category.Info, priority: Priority.Low);

                if (SelectedRegion == null) SelectedRegion = new List<object>();
                if (SelectedStage == null) SelectedStage = new List<object>();
                if (SelectedTemplate == null) SelectedTemplate = new List<object>();
                if (Selectedcptype == null) Selectedcptype = new List<object>();

                if (SelecteddesignType == null) SelecteddesignType = new List<object>();

                if (Selecteddesignsystem == null) Selecteddesignsystem = new List<object>();

                if (SelectedNumOfways == null) SelectedNumOfways = new List<object>();

                if (SelectedDSAStatus == null) SelectedDSAStatus = new List<object>();

                if (SelectedDetection == null) SelectedDetection = new List<object>();

                if (SelectedTypeOfways == null) SelectedTypeOfways = new List<object>();

                if (SelectedOption == null) SelectedOption = new List<object>();

                if (GlobalComparisonList_cloned != null)
                    GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.ToList());
                
                if (SelectedRegion.Count == RegionList.Count && SelectedStage.Count == StageList.Count &&
                     SelectedTemplate.Count == TemplateList.Count && Selectedcptype.Count == CpTypeList.Count 
                    && Selecteddesignsystem.Count == DesignSystemList.Count && 
                    SelecteddesignType.Count == DesignTypeList.Count && 
                    SelectedDSAStatus.Count == DSAStatusList.Count && 
                    SelectedTypeOfways.Count == SelectedTypeOfways.Count && 
                    SelectedNumOfways.Count == SelectedNumOfways.Count &&
                    SelectedDetection.Count == DetectionList.Count&& SelectedOption.Count==OptionList.Count)
                {
                    GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.ToList());
                   
                } // If Selected ALL filters

                else if (SelectedRegion != null && SelectedRegion.Count > 0)
                {
                    List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).Region)).Distinct().ToList();

                    GlobalComparisonList_cloned = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region)).ToList());
                    if (SelectedStage != null && SelectedStage.Count > 0)
                    {
                        List<string> stg = Selectedcptype.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).StageName)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                            (GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) &&
                            stg.Contains(i.StageName)).ToList());
                        if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                        {
                            List<string> tmplte = Selectedcptype.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).TemplateName)).Distinct().ToList();

                            GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) &&
                               stg.Contains(i.StageName) && tmplte.Contains(i.TemplateName)).ToList());
                        }
                            if (Selectedcptype != null && Selectedcptype.Count > 0)
                        {
                            List<string> cptypee = Selectedcptype.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).CpType)).Distinct().ToList();

                            GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                                (GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) &&
                                cptypee.Contains(i.CpType)).ToList());

                            if (Selecteddesignsystem != null && Selecteddesignsystem.Count > 0)
                            {
                                List<string> dsignsystem = Selecteddesignsystem.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DesignSystem))
                                    .Distinct().ToList();
                                // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as ERM_GlobalComparisonTimes).IdProductCategory)).Distinct().ToList();

                                GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) &&
                                cptypee.Contains(i.CpType) && dsignsystem.Contains(i.DesignSystem)).ToList());

                                if (SelecteddesignType != null && SelecteddesignType.Count > 0)
                                {

                                    List<string> dsignType = SelecteddesignType.Select(i =>
                                    Convert.ToString((i as ERM_GlobalComparisonTimes).DesignType)).Distinct().
                                    ToList();

                                    GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                                        (GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) && cptypee.Contains(i.CpType) &&
                                    dsignsystem.Contains(i.DesignSystem)
                                    && dsignType.Contains(i.DesignType)).ToList());

                                }
                                if (SelectedDSAStatus != null && SelectedDSAStatus.Count > 0)
                                {

                                    List<string> dsastatuss = SelectedDSAStatus.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).TemplateName)).Distinct().ToList();

                                    GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                                        (GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) &&
                                        cptypee.Contains(i.CpType) &&
                                    dsignsystem.Contains(i.DesignSystem)
                                    && dsastatuss.Contains(Convert.ToString(i.TemplateName))).ToList());

                                    if (SelectedTypeOfways != null && SelectedTypeOfways.Count > 0)
                                    {
                                        List<string> typeofwayss = SelectedTypeOfways.Select(i => Convert.ToString(
                                            (i as ERM_GlobalComparisonTimes).TypeOfways)).Distinct().ToList();

                                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                                            (GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region) &&
                                            cptypee.Contains(i.CpType) &&
                                            dsignsystem.Contains(i.DesignSystem) &&
                                            dsastatuss.Contains(i.TemplateName) &&
                                            typeofwayss.Contains(i.TypeOfways)).ToList());

                                        if (SelectedNumOfways != null && SelectedNumOfways.Count > 0)
                                        {
                                            List<string> numofwayss = SelectedNumOfways.Select(i =>
                                            Convert.ToString((i as ERM_GlobalComparisonTimes).NoOfWays)).Distinct().ToList();

                                            GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                                                (GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region)
                                                && cptypee.Contains(i.CpType) && dsignsystem.Contains(i.DesignSystem)
                                                && dsastatuss.Contains(i.TemplateName) &&
                                                typeofwayss.Contains(i.TypeOfways) &&
                                                numofwayss.Contains(Convert.ToString(i.NoOfWays))).ToList());

                                            if (SelectedDetection != null && SelectedDetection.Count > 0)
                                            {

                                                List<string> detectionss = SelectedDetection.Select(i =>
                                                Convert.ToString((i as ERM_GlobalComparisonTimes).Detection)).Distinct().ToList();

                                                GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(
                                                    GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region)
                                                    && cptypee.Contains(i.CpType) && dsignsystem.Contains(i.DesignSystem)
                                                    && dsastatuss.Contains(i.TemplateName) && typeofwayss.Contains(i.TypeOfways)
                                                    && numofwayss.Contains(Convert.ToString(i.NoOfWays)) &&
                                                    detectionss.Contains(Convert.ToString(i.Detection))).ToList());
                                                if (SelectedOption != null && SelectedOption.Count > 0)
                                                {

                                                    List<string> optionss = SelectedOption.Select(i =>
                                                    Convert.ToString((i as ERM_GlobalComparisonTimes).Option)).Distinct().ToList();

                                                    GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(
                                                        GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region)
                                                        && cptypee.Contains(i.CpType) && dsignsystem.Contains(i.DesignSystem)
                                                        && dsastatuss.Contains(i.TemplateName) && typeofwayss.Contains(i.TypeOfways)
                                                        && numofwayss.Contains(Convert.ToString(i.NoOfWays)) &&
                                                        detectionss.Contains(Convert.ToString(i.Detection)) && optionss.Contains(Convert.ToString(i.Option))).ToList());

                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    }

                }
                else
                {
                    if (SelectedRegion != null && SelectedRegion.Count > 0)
                    {
                        List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).Region)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => RegionIds.Contains(i.Region)).ToList());
                        
                    }
                    else if (SelectedStage != null && SelectedStage.Count > 0)
                    {

                        List<string> stg = SelectedStage.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).StageName)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => stg.Contains(i.StageName)).ToList());

                    }
                    else if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                    {

                        List<string> tmplt = SelectedTemplate.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).TemplateName)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => tmplt.Contains(i.TemplateName)).ToList());

                    }
                    else if (Selectedcptype != null && Selectedcptype.Count > 0)
                    {

                        List<string> cptypee = Selectedcptype.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).CpType)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => cptypee.Contains(i.CpType)).ToList());
                        
                    }
                    else if (Selecteddesignsystem != null && Selecteddesignsystem.Count > 0)
                    {

                        List<string> dsgnsystem = Selecteddesignsystem.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DesignSystem)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => dsgnsystem.Contains(i.DesignSystem)).ToList());
                        
                    }
                    else if (SelecteddesignType != null && SelecteddesignType.Count > 0)
                    {

                        List<string> dsgntypee = SelecteddesignType.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DesignType)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => dsgntypee.Contains(i.DesignType)).ToList());
                        
                    }
                    else if (SelectedDSAStatus != null && SelectedDSAStatus.Count > 0)
                    {
                        List<string> dsasysm = SelectedDSAStatus.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).DsaStatus)).Distinct().ToList();
                        
                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => dsasysm.Contains(i.DsaStatus)).ToList());
                        
                    }
                    else if (SelectedTypeOfways != null && SelectedTypeOfways.Count > 0)
                    {
                        List<string> typeofwys = SelectedTypeOfways.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).TypeOfways)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>
                            (GlobalComparisonList_cloned.Where(i => typeofwys.Contains
                            (Convert.ToString(i.TypeOfways))).ToList());
                        

                    }
                    else if (SelectedNumOfways != null && SelectedNumOfways.Count > 0)
                    {
                        List<string> numofwayss = SelectedNumOfways.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).NoOfWays)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => numofwayss.Contains(Convert.ToString(i.NoOfWays))).ToList());
                        
                    }
                    else if (SelectedDetection != null && SelectedDetection.Count > 0)
                    {
                        List<string> detectionss = SelectedDetection.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).Detection)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => detectionss.Contains(Convert.ToString(i.Detection))).ToList());
                        
                    }
                    else if (SelectedOption != null && SelectedOption.Count > 0)
                    {
                        List<string> optionss = SelectedOption.Select(i => Convert.ToString((i as ERM_GlobalComparisonTimes).Option)).Distinct().ToList();

                        GlobalComparisonList = new ObservableCollection<ERM_GlobalComparisonTimes>(GlobalComparisonList_cloned.Where(i => optionss.Contains(Convert.ToString(i.Option))).ToList());

                    }
                }


                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in ApplyFilterConditions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportGolbalComparisonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportplantdeliveryCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Global Comparision";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

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
                    activityTableView.ShowTotalSummary = true;
                    GeosApplication.Instance.Logger.Log("Method ExportplantdeliveryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportplantdeliveryCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
    }
}
