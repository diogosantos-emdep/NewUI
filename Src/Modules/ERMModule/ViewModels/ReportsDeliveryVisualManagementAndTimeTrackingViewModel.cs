using DevExpress.Spreadsheet;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility.Text;
using Prism.Commands;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Emdep.Geos.UI.Validations;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.File;




namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class ReportsDeliveryVisualManagementAndTimeTrackingViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Services
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion

        #region Declaration 

        private string windowHeader;
        DateTime? fromDate;
        DateTime? toDate;
        //[GEOS2-4624][rupali sarode][29-06-2023]
        private string source;
        private ObservableCollection<ERMDeliveryVisualManagement> deliveryVisualManagementList;
        private List<DeliveryVisualManagementStages> stagesList;
        // private List<TimeTrackingProductionStage> stagesList;  // [Rupali Sarode][GEOS2-5523][26-03-2024]
        string[] ArrActiveInPlants;
        private bool FlagPresentInActivePlants = false;
        private List<ERMDeliveryVisualManagement> DVMQualityList = new List<ERMDeliveryVisualManagement>();  //Red
        private List<ERMDeliveryVisualManagement> DVMGreaterThanSevenDayList = new List<ERMDeliveryVisualManagement>(); //Blue
        private List<ERMDeliveryVisualManagement> DVMLDelayList = new List<ERMDeliveryVisualManagement>(); //Yellow
        private List<ERMDeliveryVisualManagement> DVMLessThanEqualSevenList = new List<ERMDeliveryVisualManagement>(); //Green
        private List<ERMDeliveryVisualManagement> DVMGoAheadList = new List<ERMDeliveryVisualManagement>(); //Orange
        private string error = string.Empty;
        private List<GeosAppSetting> geosAppSettingList;
        private List<TimeTracking> allPlantTimeTrackinReport;
        List<string> failedPlants;
        private List<TimeTracking> allPlantReworkReport;
        private List<ERM_StagebyExcelColumnIndex> stagebyExcelColumnIndexlist;//[gulab lakade][11 03 2024][GEOS2-5466]
        private List<int> otItemStagesList; // [Rupali Sarode][GEOS2-5523][26-03-2024]
        private List<int> drawingIdStagesList; // [Rupali Sarode][GEOS2-5523][26-03-2024]
        private StageByOTItemAndIDDrawing timetrackingStagesList; // [Rupali Sarode][GEOS2-5523][26-03-2024]
        private List<ERM_CADCAMTimePerDesignType> cADCAMDesignTypeList;//[GEOS2-5854][gulab lakade][18 07 2024]
        public List<TimeTracking> timeTrackingmismatch { get; set; }//rajashri GEOS2-5988[22-08-2024]
        public List<ERM_Timetracking_rework_ows> reworkOWSList { get; set; } //[GEOS2-6891][pallavi jadhav][05 02 2025]
        #endregion

        #region Properties

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public DateTime? FromDate
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
        public DateTime? ToDate
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

        //[GEOS2-4624][rupali sarode][29-06-2023]
        public string Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Source"));
            }
        }

        #region [GEOS2-4624][rupali sarode][29-06-2023]
        public ObservableCollection<ERMDeliveryVisualManagement> DeliveryVisualManagementList
        {
            get
            {
                return deliveryVisualManagementList;
            }
            set
            {
                deliveryVisualManagementList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryVisualManagementList"));
            }
        }

        public List<DeliveryVisualManagementStages> StagesList
        {
            get
            {
                return stagesList;
            }
            set
            {
                stagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StagesList"));
            }
        }
        //public List<TimeTrackingProductionStage> StagesList
        //{
        //    get
        //    {
        //        return stagesList;
        //    }
        //    set
        //    {
        //        stagesList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("StagesList"));
        //    }
        //}


        #endregion

        #region    //[GEOS2-4626][pallavi jadhav][03 07 2023]
        public List<GeosAppSetting> GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        public List<TimeTracking> AllPlantTimeTrackinReport
        {
            get { return allPlantTimeTrackinReport; }
            set
            {
                allPlantTimeTrackinReport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPlantTimeTrackinReport"));


            }
        }

        // public List<TimeTrackingProductionStage> TimeTrackingProductionStage { get; set; }

        #endregion


        public virtual bool DialogResult { get; protected set; }
        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;
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

        public List<TimeTracking> TimeTrackingList { get; set; }

        public string PreviouslySelectedPlantOwners { get; set; }

        private ObservableCollection<Site> plantListForTrackingData;
        public ObservableCollection<Site> PlantListForTrackingData
        {
            get
            {
                return plantListForTrackingData;
            }

            set
            {
                plantListForTrackingData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantListForTrackingData"));
            }
        }

        private List<int> appSettingData;
        public List<int> AppSettingData
        {
            get
            {
                return appSettingData;
            }
            set
            {
                appSettingData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppSettingData"));
            }
        }

        private ObservableCollection<Site> plantList;
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }

        private List<object> selectedPlant;
        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        private List<object> selectedPlantold;

        public List<object> SelectedPlantold
        {
            get
            {
                return selectedPlantold;
            }

            set
            {
                selectedPlantold = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantold"));
            }
        }

        public List<Site> AllPlantList { get; set; }

        public List<TimeTracking> AllPlantReworkReport
        {
            get { return allPlantReworkReport; }
            set
            {
                allPlantReworkReport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPlantReworkReport"));


            }
        }
        private ObservableCollection<Site> plantListForReworkData;
        public ObservableCollection<Site> PlantListForReworkData
        {
            get
            {
                return plantListForReworkData;
            }

            set
            {
                plantListForReworkData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantListForReworkData"));
            }
        }

        //[GEOS2-5099][gulab lakade][30 11 2023]
        private List<ERMTimetrackingSite> timetrackingProductionList;
        public List<ERMTimetrackingSite> TimetrackingProductionList
        {
            get
            {
                return timetrackingProductionList;
            }

            set
            {
                timetrackingProductionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimetrackingProductionList"));
            }
        }
        //[GEOS2-5099][gulab lakade][30 11 2023]
        //start [gulab lakade][11 03 2024][GEOS2-5466]
        public List<ERM_StagebyExcelColumnIndex> StagebyExcelColumnIndexlist
        {
            get
            {
                return stagebyExcelColumnIndexlist;
            }

            set
            {
                stagebyExcelColumnIndexlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StagebyExcelColumnIndexlist"));
            }
        }
        //end [gulab lakade][11 03 2024][GEOS2-5466]

        #region  [Rupali Sarode][GEOS2-5523][26-03-2024]
        public List<int> OtItemStagesList
        {
            get
            {
                return otItemStagesList;
            }

            set
            {
                otItemStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemStagesList"));
            }
        }

        public List<int> DrawingIdStagesList
        {
            get
            {
                return drawingIdStagesList;
            }

            set
            {
                drawingIdStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DrawingIdStagesList"));
            }
        }

        public StageByOTItemAndIDDrawing TimetrackingStagesList
        {
            get
            {
                return timetrackingStagesList;
            }

            set
            {
                timetrackingStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimetrackingStagesList"));
            }
        }

        #endregion  [Rupali Sarode][GEOS2-5523][26-03-2024]
        //[GEOS2-5854][gulab lakade][18 07 2024]
        public List<ERM_CADCAMTimePerDesignType> CADCAMDesignTypeList
        {
            get
            {
                return cADCAMDesignTypeList;
            }

            set
            {
                cADCAMDesignTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CADCAMDesignTypeList"));
            }
        }
        //[GEOS2-5854][gulab lakade][18 07 2024]
        public List<GeosAppSetting> ActivePlantList = new List<GeosAppSetting>();//Aishwarya Ingale[Geos2-6786]

        //[GEOS2-6891][pallavi jadhav][05 02 2025]
        public List<ERM_Timetracking_rework_ows> ReworkOWSList
        {
            get
            {
                return reworkOWSList;
            }

            set
            {
                reworkOWSList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReworkOWSList"));
            }
        }

        private Visibility isFQUDate;
        public Visibility IsFQUDate
        {
            get
            {
                return isFQUDate;
            }

            set
            {
                isFQUDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFQUDate"));
            }
        }
        private Visibility isDeliveryDate;
        public Visibility IsDeliveryDate
        {
            get
            {
                return isDeliveryDate;
            }

            set
            {
                isDeliveryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeliveryDate"));
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

        #endregion // Events

        #region Command
        public ICommand CancelButtonCommand { get; set; }

        public ICommand EscapeButtonCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }  //[GEOS2-4624][rupali sarode][29-06-2023]
        #endregion

        #region Constructor
		//[nsatpute][25-06-2025][GEOS2-8641]
        public ReportsDeliveryVisualManagementAndTimeTrackingViewModel(string source)
        {
            CancelButtonCommand = new DevExpress.Mvvm.DelegateCommand<object>(CloseWindow);
            EscapeButtonCommand = new DevExpress.Mvvm.DelegateCommand<object>(CloseWindow);
            Source = source;
            if (Source == "TimeTracking")
                AcceptButtonCommand = new DevExpress.Mvvm.AsyncCommand<object>(TimeTrackingAcceptButtonCommandAction);
            else
                AcceptButtonCommand = new DevExpress.Mvvm.DelegateCommand<object>(AcceptButtonCommandAction);
            FillListOfColor();
        }
        #endregion

        #region Methods
		//[nsatpute][26-06-2025][GEOS2-8641]
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.TimeTrackingActivePlantList == null)
                    GeosApplication.Instance.TimeTrackingActivePlantList = ActivePlantList = WorkbenchStartUp.GetSelectedGeosAppSettings("134");
                else
                    ActivePlantList = GeosApplication.Instance.TimeTrackingActivePlantList.ToList();

                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GetPlants();
                setDefaultPeriod();
                FillStages();
                FailedPlants = new List<string>();
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }

        private void setDefaultPeriod()
        {
            if (Source == "DeliveryVisualManagement")
            {
                IsDeliveryDate = Visibility.Visible;
                IsFQUDate = Visibility.Collapsed;
                FromDate = Convert.ToDateTime(DateTime.Now.Date);
                // Convert.ToDateTime(modulesEquivalencyWeight.StartDate.Value.Date)
                ToDate = Convert.ToDateTime(DateTime.Now.Date);
            }
            else if (Source == "TimeTracking")
            {
                IsDeliveryDate = Visibility.Collapsed;
                IsFQUDate = Visibility.Visible;
                FillDataByPlant();
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);

                FromDate = StartFromDate;
                if (TimeTrackingList.Count != 0)
                {
                    var TimeTrackingListDate = TimeTrackingList.Select(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date)).Max();

                    // ToDate = EndToDate;
                    //  fromDate = StartFromDate;
                    if (TimeTrackingListDate != null)
                    {
                        DateTime DeliveryEndToDate = TimeTrackingListDate.Date;
                        ToDate = DeliveryEndToDate;
                    }

                }
                else
                {
                    DateTime EndToDate = new DateTime(year, 12, 31);
                    ToDate = EndToDate;
                }
            }
            else if (Source == "Rework")
            {
                IsDeliveryDate = Visibility.Visible;
                IsFQUDate = Visibility.Collapsed;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                int daysInMonth = DateTime.DaysInMonth(year, month);
                FromDate = new DateTime(year, month, 01);
                // Convert.ToDateTime(modulesEquivalencyWeight.StartDate.Value.Date)
                ToDate = new DateTime(year, month, daysInMonth);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()..."), category: Category.Info, priority: Priority.Low);
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void AcceptButtonCommandAction(object obj)
        {
            string plants = string.Empty;
            try
            {

                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                //#region rajashri GEOS2-9526 [9-10-2025]
                SelectedPlant = new List<object>();
                SelectedPlant.AddRange(ERMCommon.Instance.SelectedAuthorizedPlantsList);
                //#endregion
                allowValidation = true;
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                if (error != null)
                {
                    return;
                }

                FailedPlants = new List<string>();

                List<ReworkData> OTsWithIDOTItemList = new List<ReworkData>(); //  [Rupali Sarode][GEOS2-5523][26-03-2024] 
                List<ReworkData> OTsWithIDDrawingList = new List<ReworkData>(); // [Rupali Sarode][GEOS2-5523][26-03-2024] 

                #region DeliveryVisualManagementReport
                if (Source == "DeliveryVisualManagement")
                {
                    // if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    // string DvsPlantName = string.Empty;
                    // List<Site> DvsPlantName1 = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    //// var plantOwnersIds1 = string.Join(",", DvsPlantName1.Select(i => i.Name));
                    // foreach (var itemPlantOwnerUsers1 in DvsPlantName1) //[Aishwarya ingale [01-09-2023][4792]]
                    // {

                    //     if (string.IsNullOrEmpty(DvsPlantName))
                    //         DvsPlantName = itemPlantOwnerUsers1.Name;
                    //     else
                    //         DvsPlantName = DvsPlantName + "_" + itemPlantOwnerUsers1.Name;

                    // }

                    string ResultFileName;
                    string DateInFileName = DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year;
                    //   string FileName = PlantName+"_" +"ERM_Data_List_"+FromDate.Value.ToString("ddMMyyyy") + "_"+ToDate.Value.ToString("ddMMyyyy") + ".xlsx";
                    string FileName = "ERM_DVM_List_" + Convert.ToDateTime(FromDate).Date.ToString("ddMMyyyy") + "_" + Convert.ToDateTime(ToDate).Date.ToString("ddMMyyyy");//[Aishwarya ingale [01-09-2023][4792]]
                    Microsoft.Win32.SaveFileDialog SaveFileDialogService1 = new Microsoft.Win32.SaveFileDialog();
                    SaveFileDialogService1.DefaultExt = "xlsx";
                    SaveFileDialogService1.FileName = FileName;
                    SaveFileDialogService1.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    SaveFileDialogService1.FilterIndex = 1;
                    bool DialogResult = (Boolean)SaveFileDialogService1.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                        return;
                    }
                    else
                    {
                        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        {
                            DXSplashScreen.Show(x =>
                            {
                                System.Windows.Window win = new System.Windows.Window()
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
                        ResultFileName = (SaveFileDialogService1.FileName);
                        // ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                        Workbook workbook = new Workbook();
                        DeliveryVisualManagementList = new ObservableCollection<ERMDeliveryVisualManagement>();
                        DVMQualityList = new List<ERMDeliveryVisualManagement>();
                        DVMGreaterThanSevenDayList = new List<ERMDeliveryVisualManagement>();
                        DVMLDelayList = new List<ERMDeliveryVisualManagement>();
                        DVMLessThanEqualSevenList = new List<ERMDeliveryVisualManagement>();
                        DVMGoAheadList = new List<ERMDeliveryVisualManagement>();

                        string FilePath = ResultFileName;


                        //if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                        if (SelectedPlant != null)
                        {
                            //List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                            //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                            ERMCommon.Instance.FailedPlants = new List<string>();
                            IsShowFailedPlantWarning = false;
                            WarningFailedPlants = string.Empty;
                            if (FailedPlants == null)
                            {
                                FailedPlants = new List<string>();
                            }
                            FailedPlants = new List<string>();
                            foreach (Site itemPlant in SelectedPlant)
                            {
                                var TempRemainingPlant = PlantList.Where(x => x.IdSite == itemPlant.IdSite).ToList();
                                foreach (var itemPlantOwnerUsers in TempRemainingPlant)
                                {
                                    try
                                    {
                                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                                        ERMService = new ERMServiceController(serviceurl);

                                        string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);

                                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                                        //  ERMService = new ERMServiceController("localhost:6699");
                                        //     DeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2410(IdSite, Convert.ToDateTime(FromDate), Convert.ToDateTime(toDate)));

                                        DeliveryVisualManagementList.AddRange(ERMService.GetDVManagementProduction_V2540(IdSite, Convert.ToDateTime(FromDate), Convert.ToDateTime(toDate))); //[pallavi jadhav][GEOS2-5907][18 07 2024]


                                    }
                                    catch (FaultException<ServiceException> ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                        if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                        {
                                            ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                            if (FailedPlants != null && FailedPlants.Count > 0)
                                            {
                                                IsShowFailedPlantWarning = true;
                                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                            }
                                        }
                                        //  System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (ServiceUnexceptedException ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");

                                        if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                        {
                                            FailedPlants.Add(itemPlantOwnerUsers.Name);
                                            if (FailedPlants != null && FailedPlants.Count > 0)
                                            {
                                                IsShowFailedPlantWarning = true;
                                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                            }
                                        }
                                        //System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                        if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                        {
                                            FailedPlants.Add(itemPlantOwnerUsers.Name);
                                            if (FailedPlants != null && FailedPlants.Count > 0)
                                            {
                                                IsShowFailedPlantWarning = true;
                                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                            }
                                        }
                                        //  System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }
                                }
                            }
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;

                            if (FailedPlants == null || FailedPlants.Count == 0)
                            {
                                IsShowFailedPlantWarning = false;
                                WarningFailedPlants = string.Empty;
                            }
                        }

                        foreach (var stage in StagesList)
                        {

                            if (stage.ActiveInPlants != null && stage.ActiveInPlants != "")
                            {
                                ArrActiveInPlants = stage.ActiveInPlants.Split(',');
                                List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                                FlagPresentInActivePlants = false;
                                foreach (Site itemSelectedPlant in SelectedPlant)
                                {
                                    if (FlagPresentInActivePlants == true)
                                    {
                                        break;
                                    }

                                    foreach (var iPlant in ArrActiveInPlants)
                                    {
                                        if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                        {
                                            FlagPresentInActivePlants = true;
                                            break;
                                        }
                                    }

                                }
                            }

                            //Fill all lists first
                            if (FlagPresentInActivePlants == true || stage.ActiveInPlants == null || stage.ActiveInPlants == "")
                            {

                                DVMQualityList.AddRange(DeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.IdOfferType == 2 || i.IdOfferType == 3) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList()); //Red [pallavi jadhav][09 04 2023][GEOS2-4792] - Added idtemplate!=24 and idtemplate!=9 
                                DateTime? tempDate = DateTime.Now.AddDays(7);
                                // DVMGreaterThanSevenDayList.AddRange(DeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).ToList()); //Blue
                                DVMGreaterThanSevenDayList.AddRange(DeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList()); //Blue [pallavi jadhav][09 04 2023][GEOS2-4792] - Added idtemplate!=24 and idtemplate!=9 

                                tempDate = DateTime.Now;
                                //  DVMLDelayList.AddRange(DeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).ToList()); //Yellow
                                DVMLDelayList.AddRange(DeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList()); //Yellow [pallavi jadhav][09 04 2023][GEOS2-4792] - Added idtemplate!=24 and idtemplate!=9 

                                tempDate = DateTime.Now.AddDays(7);
                                DateTime TodaysDate = DateTime.Now.Date;
                                //  DVMLessThanEqualSevenList.AddRange(DeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.DeliveryDate.Value.Date > TodaysDate && i.DeliveryDate.Value.Date <= tempDate.Value.Date) && i.CurrentWorkStation == stage.StageCode).ToList()); //Green
                                DVMLessThanEqualSevenList.AddRange(DeliveryVisualManagementList.Where(i => (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) > TodaysDate && (i.PlannedDeliveryDate != null ? i.PlannedDeliveryDate.Value.Date : i.DeliveryDate.Value.Date) <= tempDate.Value.Date && i.IdStage == stage.IdStage && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList());  //Green [pallavi jadhav][09 04 2023][GEOS2-4792] - Added idtemplate!=24 and idtemplate!=9 
                                //DVMGoAheadList.AddRange(DeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null && i.PODate == null) && i.CurrentWorkStation == stage.StageCode).ToList()); //Orange
                                DVMGoAheadList.AddRange(DeliveryVisualManagementList.Where(i => i.IdStage == stage.IdStage && (i.GoAheadDate != null) && i.CurrentWorkStation == stage.StageCode && i.IdTemplate != 24 && i.IdTemplate != 9).ToList()); //Orange //[pallavi jadhav][09 04 2023][GEOS2-4792] - Added idtemplate!=24 and idtemplate!=9 and remove PODate!=null
                            }

                        }

                        //export data to excel


                        workbook = ExportDataToExcel(DVMGreaterThanSevenDayList, 0, workbook, "BLUE List");
                        workbook = ExportDataToExcel(DVMLessThanEqualSevenList, 1, workbook, "GREEN List");
                        workbook = ExportDataToExcel(DVMLDelayList, 2, workbook, "YELLOW List");
                        workbook = ExportDataToExcel(DVMQualityList, 3, workbook, "RED List");
                        workbook = ExportDataToExcel(DVMGoAheadList, 4, workbook, "ORANGE List");

                        // }
                        try
                        {
                            workbook.Worksheets.ActiveWorksheet = workbook.Worksheets[0];
                            workbook.SaveDocument(FilePath, DocumentFormat.Xlsx);
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DVMReportExportMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, null);
                            System.Diagnostics.Process.Start(FilePath);
                        }
                        catch (Exception ex)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }

                }
                #endregion

                #region ReworksReport
                if (Source == "Rework")
                {
                    //ERMService = new ERMServiceController("localhost:6699");
                    List<PlanningDateReviewStages> AllPlantWeeklyReworksMailStage = ERMService.GetProductionPlanningReviewStage_V2400();
                    string ResultFileName;
                    string DateInFileName = DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year;
                    //   string FileName = PlantName+"_" +"ERM_Data_List_"+FromDate.Value.ToString("ddMMyyyy") + "_"+ToDate.Value.ToString("ddMMyyyy") + ".xlsx";
                    string FileName = "ERM_Reworks_List_" + Convert.ToDateTime(FromDate).Date.ToString("ddMMyyyy") + "_" + Convert.ToDateTime(ToDate).Date.ToString("ddMMyyyy");
                    Microsoft.Win32.SaveFileDialog SaveFileDialogService1 = new Microsoft.Win32.SaveFileDialog();
                    SaveFileDialogService1.DefaultExt = "xlsx";
                    SaveFileDialogService1.FileName = FileName;
                    SaveFileDialogService1.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    SaveFileDialogService1.FilterIndex = 1;
                    bool DialogResult = (Boolean)SaveFileDialogService1.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                        return;
                    }
                    else
                    {
                        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        {
                            DXSplashScreen.Show(x =>
                            {
                                System.Windows.Window win = new System.Windows.Window()
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
                        ResultFileName = (SaveFileDialogService1.FileName);
                        // ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                    }
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    string filePath = "";
                    Workbook workbook = new Workbook();

                    Worksheet worksheet = workbook.Worksheets[0];
                    AllPlantReworkReport = new List<TimeTracking>();
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }
                    //    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    if (SelectedPlant != null)
                    {

                        ERMCommon.Instance.FailedPlants = new List<string>();
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;

                        FailedPlants = new List<string>();

                        var CurrencyNameFromSetting = String.Empty;
                        string PlantName = string.Empty;
                        //  int rows = 4;
                        TimeTrackingWithSites reworkReportSite = new TimeTrackingWithSites();
                        PlantListForTrackingData = new ObservableCollection<Site>();
                        foreach (Site itemPlant in SelectedPlant)
                        {
                            var TempRemainingPlant = PlantList.Where(x => x.IdSite == itemPlant.IdSite).ToList();

                            foreach (var itemPlantOwnerUsers in TempRemainingPlant)
                            {

                                DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                                string year = Convert.ToString(tempFromyear.Year);
                                UInt32 IdSite = Convert.ToUInt32(itemPlantOwnerUsers.IdSite);
                                PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                                try
                                {

                                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                                    ERMService = new ERMServiceController(serviceurl);
                                    //  ERMService = new ERMServiceController("localhost:6699");
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + PlantName;
                                    //   reworkReportSite = ERMService.GetAllReworkReport_V2440(Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date), IdSite);
                                    //  reworkReportSite = ERMService.GetAllReworkReport_V2460(Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date), IdSite);// [Pallavi Jadhav][24-11-2023][GEOS2-5053]
                                    // [Rupali Sarode][GEOS2-5523][26-03-2024]
                                    //reworkReportSite = ERMService.GetAllReworkReport_V2500(Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date), IdSite);
                                    //    reworkReportSite = ERMService.GetAllReworkReport_V2540(Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date), IdSite); //[pallavi jadhav][GEOS2-5907][18 07 2024]
                                    reworkReportSite = ERMService.GetAllReworkReport_V2550(Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date), IdSite); //[Aishwarya Ingale][GEOS2-6034][07 08 2024]

                                    if (PlantListForReworkData == null)
                                    {
                                        PlantListForReworkData = new ObservableCollection<Site>();
                                    }

                                    PlantListForReworkData.AddRange(reworkReportSite.siteList);
                                    AllPlantReworkReport.AddRange(reworkReportSite.TimeTrackingList);

                                    if (plants == string.Empty)
                                    {
                                        plants = itemPlantOwnerUsers.Name;
                                    }
                                    else
                                    {
                                        plants = plants + "," + itemPlantOwnerUsers.Name;
                                    }
                                }

                                catch (FaultException<ServiceException> ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                    if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.Name);
                                        if (FailedPlants != null && FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        }
                                    }
                                    // System.Threading.Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                    //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    //    FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.Name);

                                        if (FailedPlants != null && FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        }
                                    }
                                    //  System.Threading.Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                    if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.Name);
                                        if (FailedPlants != null && FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        }
                                    }
                                    // System.Threading.Thread.Sleep(1000);
                                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }

                                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                                if (FailedPlants == null || FailedPlants.Count == 0)
                                {
                                    IsShowFailedPlantWarning = false;
                                    WarningFailedPlants = string.Empty;
                                }


                            }
                        }

                        try
                        {
                            worksheet.Name = "ERM_Reworks";
                            worksheet.Cells[0, 0].Value = "Delivery Week";
                            worksheet.Cells[0, 1].Value = "Delivery Date";
                            worksheet.Cells[0, 2].Value = "PO Date";
                            worksheet.Cells[0, 3].Value = "PO Type";
                            worksheet.Cells[0, 4].Value = "Customer";
                            worksheet.Cells[0, 5].Value = "Project";
                            worksheet.Cells[0, 6].Value = "Offer";
                            worksheet.Cells[0, 7].Value = "OT Code";
                            worksheet.Cells[0, 8].Value = "Origin Plant";
                            worksheet.Cells[0, 9].Value = "Production Plant";
                            worksheet.Cells[0, 10].Value = "Customer Ref";
                            worksheet.Cells[0, 11].Value = "Template";
                            worksheet.Cells[0, 12].Value = "Type";
                            worksheet.Cells[0, 13].Value = "QTY";
                            worksheet.Cells[0, 14].Value = "Serial Number";
                            worksheet.Cells[0, 15].Value = "Item Number";
                            worksheet.Cells[0, 16].Value = "Item Status";
                            worksheet.Cells[0, 17].Value = "Current WorkStation";
                            worksheet.Cells[0, 18].Value = "T. Reworks";
                            worksheet.Cells[0, 19].Value = "Id Drawing";
                            worksheet.Cells[0, 20].Value = "Workbook Drawing";

                            int startColumnIndex = 0;
                            int numberOfColumns = 21;
                            for (int coll = startColumnIndex; coll < startColumnIndex + numberOfColumns; coll++)
                            {
                                worksheet.Columns[coll].WidthInCharacters = 20;
                            }


                            int col = 21;
                            foreach (var item in AllPlantWeeklyReworksMailStage)
                            {
                                worksheet.Cells[0, col].Value = item.StageCode;
                                col++;
                            }
                            CellRange range = worksheet[0, col];
                            worksheet.AutoFilter.Apply(range);
                            int rows = 1;
                            if (AllPlantReworkReport != null)
                            {
                                var AllPlantReworkReportList = AllPlantReworkReport.OrderBy(x => x.DeliveryWeek).ToList();

                                AllPlantReworkReport = new List<TimeTracking>();
                                AllPlantReworkReport = AllPlantReworkReportList;
                                //AllPlantReworkReport = AllPlantReworkReport.Where(a => a.Rework != 0).ToList();
                            }

                            OTsWithIDOTItemList = new List<ReworkData>(); //  [Rupali Sarode][GEOS2-5523][26-03-2024] 
                            OTsWithIDDrawingList = new List<ReworkData>(); // [Rupali Sarode][GEOS2-5523][26-03-2024] 

                            bool FlagNewAlgorithmCOM = false;
                            bool FlagNewAlgorithmCADCAM = false;

                            foreach (var reworkMail in AllPlantReworkReport)
                            {
                                worksheet.Cells[rows, 0].Value = reworkMail.DeliveryWeek;
                                worksheet.Cells[rows, 1].Value = reworkMail.DeliveryDate;
                                worksheet.Cells[rows, 2].Value = reworkMail.PODate;
                                worksheet.Cells[rows, 3].Value = reworkMail.POType;
                                worksheet.Cells[rows, 4].Value = reworkMail.Customer;
                                worksheet.Cells[rows, 5].Value = reworkMail.Project;
                                worksheet.Cells[rows, 6].Value = reworkMail.Offer;
                                worksheet.Cells[rows, 7].Value = reworkMail.OTCode;
                                worksheet.Cells[rows, 8].Value = reworkMail.OriginalPlant;
                                worksheet.Cells[rows, 9].Value = reworkMail.ProductionPlant;
                                worksheet.Cells[rows, 10].Value = reworkMail.CustomerReference;
                                worksheet.Cells[rows, 11].Value = reworkMail.Template;
                                worksheet.Cells[rows, 12].Value = reworkMail.Type;
                                worksheet.Cells[rows, 13].Value = reworkMail.QTY;
                                worksheet.Cells[rows, 14].Value = reworkMail.SerialNumber;
                                worksheet.Cells[rows, 15].Value = reworkMail.ItemNumber;
                                worksheet.Cells[rows, 16].Value = reworkMail.ItemStatus;
                                worksheet.Cells[rows, 17].Value = reworkMail.CurrentWorkStation;
                                // worksheet.Cells[rows, 17].Value = reworkMail.Rework;
                                worksheet.Cells[rows, 19].Value = reworkMail.IdDrawing;
                                worksheet.Cells[rows, 20].Value = reworkMail.Workbookdrawing;


                                rows++;
                            }
                            worksheet.FreezeColumns(1);

                            List<string> ListReference = new List<string>();
                            // List<string> ListOfComment = new List<string>();
                            int i = 1;
                            int row = 1;
                            List<Counterpartstracking> CounterpartList = new List<Counterpartstracking>();

                            Comment comment = null;
                            foreach (var reworkMail in AllPlantReworkReport)
                            {
                                int totalReworks = 0;
                                int stageCol = 21;

                                #region [Rupali Sarode][GEOS2-5523][26-03-2024] -- As per New algorithm

                                List<Counterpartstracking> StageReworksList = new List<Counterpartstracking>();

                                FlagNewAlgorithmCOM = false;
                                FlagNewAlgorithmCADCAM = false;

                                if (reworkMail.IsBatch == false)
                                {
                                    if (reworkMail.IdOTItem != 0)
                                    {
                                        var TempIdOTItem = OTsWithIDOTItemList.Where(j => j.IdOT == reworkMail.IdOt && j.IdOTItem == reworkMail.IdOTItem).FirstOrDefault();

                                        if (TempIdOTItem == null)
                                        {
                                            ReworkData TempReworkOTItem = new ReworkData();

                                            TempReworkOTItem.IdOT = reworkMail.IdOt;
                                            TempReworkOTItem.IdOTItem = reworkMail.IdOTItem;
                                            TempReworkOTItem.IdCounterpart = reworkMail.IdCounterpart;
                                            OTsWithIDOTItemList.Add(TempReworkOTItem);
                                            FlagNewAlgorithmCOM = false;
                                        }
                                        else
                                        {
                                            FlagNewAlgorithmCOM = true;
                                        }
                                    }

                                    if (reworkMail.IdDrawing != 0 && reworkMail.IdWorkbookOfCpProducts != 0)//Aishwarya Ingale[Geos2-6034]
                                    {
                                        var TempIdDrawingAndWorkbook = OTsWithIDDrawingList
                                            .Where(j => j.IdOT == reworkMail.IdOt && j.IdDrawing == reworkMail.IdDrawing && j.IdWorkbookOfCpProducts == reworkMail.IdWorkbookOfCpProducts)
                                            .FirstOrDefault();

                                        if (TempIdDrawingAndWorkbook == null)
                                        {
                                            ReworkData TempReworkDrawing = new ReworkData();

                                            TempReworkDrawing.IdOT = reworkMail.IdOt;
                                            TempReworkDrawing.IdOTItem = reworkMail.IdOTItem;
                                            TempReworkDrawing.IdCounterpart = reworkMail.IdCounterpart;
                                            TempReworkDrawing.IdDrawing = reworkMail.IdDrawing;
                                            TempReworkDrawing.IdWorkbookOfCpProducts = reworkMail.IdWorkbookOfCpProducts; //Aishwarya Ingale[Geos2-6034]

                                            OTsWithIDDrawingList.Add(TempReworkDrawing);
                                            FlagNewAlgorithmCADCAM = false;
                                        }
                                        else
                                        {
                                            FlagNewAlgorithmCADCAM = true;
                                        }
                                    }

                                }

                                #endregion [Rupali Sarode][GEOS2-5523][26-03-2024]


                                foreach (var stage in AllPlantWeeklyReworksMailStage)
                                {

                                    if (((FlagNewAlgorithmCOM == true && OtItemStagesList.Contains(stage.IdStage)) || (FlagNewAlgorithmCADCAM == true && (DrawingIdStagesList.Contains(stage.IdStage)))) && reworkMail.IsBatch == false)
                                    {

                                    }
                                    else
                                    {
                                        var tempReworklist = reworkMail.CounterpartstrackingList.Where(x => x.IdStage == stage.IdStage && x.Rework == true).ToList();
                                        CounterpartList = tempReworklist;
                                        List<string> tempStageCode = new List<string>();
                                        if (tempReworklist.Count > 0)
                                        {


                                            foreach (var reworkMail1 in tempReworklist)
                                            {
                                                var testing = reworkMail.CounterpartstrackingList.IndexOf(reworkMail1);
                                                if (testing != 0)
                                                {
                                                    var Reworkrecord = reworkMail.CounterpartstrackingList[testing - 1];

                                                    if (Reworkrecord != null)
                                                    {
                                                        tempStageCode.Add(Reworkrecord.StageCode);
                                                    }
                                                }
                                                Cell cell = worksheet.Cells[row, stageCol];
                                                var cellValue = (string)(worksheet.Cells[row, stageCol]).GetRangeWithAbsoluteReference().ToString();
                                                string[] parts = cellValue.Split(',');
                                                string rangePart = parts[0].Trim(':');
                                                string worksheetPart = parts[1].Trim();
                                                string rangeAddress = rangePart.Substring(rangePart.IndexOf('"') + 1, rangePart.LastIndexOf('"') - rangePart.IndexOf('"') - 1);// rangePart.Split(':')[1].Trim('\"'); 
                                                string worksheetName = worksheetPart.Split(':')[1].Trim('\"');
                                                var Reference = worksheet.Comments.FirstOrDefault(a => a.Reference == rangeAddress);

                                                if (Reference == null)
                                                {
                                                    string selectedComment = string.Empty;
                                                    //List<Comment> comments = worksheet.Comments.GetComments(Reference);
                                                    comment = worksheet.Comments.Add(cell, reworkMail1.FullName, reworkMail1.Remarks);
                                                    CommentRunCollection runs = comment.Runs;
                                                    runs.Insert(0, reworkMail1.FullName + " \r\n");
                                                    runs[0].Font.Bold = true;

                                                }
                                                if (Reference != null)
                                                {

                                                    string selectedComment = string.Empty;
                                                    CellRange rangeE9E10 = worksheet[Reference.Reference];
                                                    List<Comment> comments1 = worksheet.Comments.GetComments(rangeE9E10);
                                                    var commentTexts = new StringBuilder();
                                                    foreach (var commentt in comments1)
                                                    {
                                                        commentTexts.AppendLine(commentt.Text);
                                                    }

                                                    commentTexts.AppendLine(reworkMail1.FullName);
                                                    commentTexts.AppendLine("\r\n");
                                                    commentTexts.AppendLine(reworkMail1.Remarks);
                                                    string allCommentText = commentTexts.ToString();

                                                    List<Comment> TempComment = worksheet.Comments.Where(item => item.Reference == Reference.Reference).ToList();
                                                    foreach (var Items in TempComment)
                                                    {
                                                        worksheet.Comments.Remove(Items);


                                                    }
                                                    if (allCommentText != null)
                                                    {
                                                        comment = worksheet.Comments.Add(cell, "", "");

                                                        string[] parts1 = allCommentText.Split('\r');
                                                        string rangePart1 = parts1[0].Trim();
                                                        string rangePart2 = parts1[1].Trim();
                                                        string rangePart3 = parts1[2].Trim();
                                                        string rangePart4 = parts1[3].Trim();
                                                        CommentRunCollection runs = comment.Runs;
                                                        runs.Insert(0, rangePart1 + "\n");
                                                        runs.Insert(1, rangePart2 + "\n");
                                                        runs.Insert(2, rangePart3 + "\n");
                                                        runs.Insert(3, rangePart4 + "\n");
                                                        runs[0].Font.Bold = true;
                                                        runs[2].Font.Bold = true;


                                                    }
                                                }
                                            }
                                            if (tempStageCode.Count > 0)
                                            {
                                                try
                                                {
                                                    string StageNamewithCount = string.Empty;
                                                    int StageCount = 0;
                                                    foreach (var item in tempStageCode.GroupBy(a => a).Select(g => new { Symbol = g.Key, Count = g.Count() }))
                                                    {
                                                        if (string.IsNullOrEmpty(StageNamewithCount))
                                                        {
                                                            StageNamewithCount = item.Symbol + "-" + item.Count;
                                                        }
                                                        else
                                                        {
                                                            StageNamewithCount = StageNamewithCount + "\r\n" + item.Symbol + "-" + item.Count;
                                                        }

                                                        StageCount = StageCount + item.Count;

                                                    }

                                                    worksheet.Cells[row, stageCol].Value = StageNamewithCount;
                                                    worksheet.Cells[row, stageCol].Alignment.WrapText = true;
                                                    totalReworks += StageCount;

                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                        }
                                    }
                                    stageCol++;
                                }
                                try
                                {
                                    worksheet.Cells[row, 18].Value = totalReworks;

                                    row++;
                                }
                                catch (Exception ex)
                                {

                                }
                            }


                            try
                            {
                                //using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                                //{

                                workbook.SaveDocument(ResultFileName, DocumentFormat.Xlsx);
                                // }

                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReworkReportExportedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                System.Diagnostics.Process.Start(ResultFileName);
                            }
                            catch (Exception ex)
                            {
                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion


                        GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        //[nsatpute][25-06-2025][GEOS2-8641]
        private async Task TimeTrackingAcceptButtonCommandAction(object obj)
        {
            string plants = string.Empty;
            try
            {

                GeosApplication.Instance.Logger.Log(string.Format("Method TimeTrackingAcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);

                #region rajashri GEOS2-9526 [9-10-2025]
                SelectedPlant = new List<object>();
                SelectedPlant.AddRange(ERMCommon.Instance.SelectedAuthorizedPlantsList);
                #endregion

                allowValidation = true;
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                if (error != null)
                {
                    return;
                }
                FailedPlants = new List<string>();

                List<ReworkData> OTsWithIDOTItemList = new List<ReworkData>(); //  [Rupali Sarode][GEOS2-5523][26-03-2024] 
                List<ReworkData> OTsWithIDDrawingList = new List<ReworkData>(); // [Rupali Sarode][GEOS2-5523][26-03-2024] 
                #region TimeTrackingReport
                if (Source == "TimeTracking")
                {


                    FillCADCAMDesignTypeList(); //[GEOS2-5854][gulab lakade][19 07 2024]
                    string fileName = "ERM_TimeTracking_List" + "_" + Convert.ToDateTime(FromDate).Date.ToString("ddMMyyyy") + "_" + Convert.ToDateTime(ToDate).Date.ToString("ddMMyyyy");

                    string ResultFileName;
                    Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                    saveFile.DefaultExt = "xlsx";
                    saveFile.FileName = fileName;
                    saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    saveFile.FilterIndex = 1;
                    DialogResult = (Boolean)saveFile.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                        return;
                    }
                    else
                    {
                        if (File.Exists(saveFile.FileName))
                            File.Delete(saveFile.FileName);
                        ResultFileName = (saveFile.FileName);
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (SelectedPlant != null)
                        {
                            string connectingSites = string.Empty;
                            if (SelectedPlant.Count == 1)
                            {
                                connectingSites = $"{System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Connectingto").ToString()} {string.Join(",", SelectedPlant.Cast<Site>().Select(p => p.Name))}";
                            }
                            else
                            {
                                connectingSites = $"{System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Parallellyconnectingto").ToString()} {string.Join(",", SelectedPlant.Cast<Site>().Select(p => p.Name))}";
                            }

                            if (GeosApplication.Instance.DownloadedReportFiles == null)
                                GeosApplication.Instance.DownloadedReportFiles = new ObservableCollection<FileDetail>();

                            if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                            {
                                GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Downloading").ToString();
                                GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).FilePath = ResultFileName;
                                GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = connectingSites;                                
                            }
                            else
                            {
                                GeosApplication.Instance.DownloadedReportFiles.Add(new FileDetail() { FileName = Path.GetFileName(ResultFileName), Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Downloading").ToString(), FilePath = ResultFileName, ConnectingSites = connectingSites });
                            }
                        }
                    });
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Reportisgeneratinginbackgr").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);

                    await Task.Run(() =>
                    {
                        Workbook workbook = new Workbook();
                        GeosApplication.Instance.Logger.Log("Method TimeTrackingAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                        Worksheet worksheet = workbook.Worksheets[0];
                        AllPlantTimeTrackinReport = new List<TimeTracking>();
                        if (ERMCommon.Instance.FailedPlants == null)
                        {
                            ERMCommon.Instance.FailedPlants = new List<string>();
                        }

                        ActivePlantList = WorkbenchStartUp.GetSelectedGeosAppSettings("134");//Aishwarya Ingale[Geos2-6786]
                        if (SelectedPlant != null)
                        {
                            ERMCommon.Instance.FailedPlants = new List<string>();
                            ERMCommon.Instance.IsShowFailedPlantWarning = false;
                            ERMCommon.Instance.WarningFailedPlants = string.Empty;
                            FailedPlants = new List<string>();
                            var CurrencyNameFromSetting = String.Empty;
                            string PlantName = string.Empty;
                            int rows = 4;
                            TimeTrackingWithSites timeTrackingSite = new TimeTrackingWithSites();
                            PlantListForTrackingData = new ObservableCollection<Site>();
                            List<SitesByShippingAddress> sitesByShippingAddressList = ERMService.GetAllSitesByShippingAddress();
                            GeosAppSetting timeTrackingAppSetting = CRMService.GetGeosAppSettings(96);
                            //foreach (Site itemPlant in SelectedPlant)
                            Parallel.ForEach(SelectedPlant.Cast<Site>(), itemPlant =>
                            {
                                var TempRemainingPlant = PlantList.Where(x => x.IdSite == itemPlant.IdSite).ToList();

                                foreach (var itemPlantOwnerUsers in TempRemainingPlant)
                                {

                                    DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                                    string year = Convert.ToString(tempFromyear.Year);
                                    UInt32 IdSite = Convert.ToUInt32(itemPlantOwnerUsers.IdSite);
                                    PlantName = Convert.ToString(itemPlantOwnerUsers.Name);
                                    try
                                    {
                                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                                        ERMService = new ERMServiceController(serviceurl);
                                     //    ERMService = new ERMServiceController("localhost:6699");
                                        // GeosApplication.Instance.SplashScreenMessage = "Connecting to " + PlantName;
                                        //[nsatpute][26-06-2025][GEOS2-8641]
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                                            {
                                                GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = "Connecting to " + PlantName;
                                            }
                                        });

                                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                                        {
                                            CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                                        }

                                        //[nsatpute][24-06-2025][GEOS2-8641]

                                        //timeTrackingSite = ERMService.GetAllTimeTrackingReport_V2660(CurrencyNameFromSetting, PlantName, GeosAppSettingList, Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date));//[Pallavi.jadhav][16 05 2025][GEOS2-8124]

                                        timeTrackingSite = ERMService.GetAllTimeTrackingReport_V2660V1(
                                               CurrencyNameFromSetting,
                                               PlantName, sitesByShippingAddressList,
                                               GeosAppSettingList, timeTrackingAppSetting,
                                               Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date));

                                        if (PlantListForTrackingData == null)
                                        {
                                            PlantListForTrackingData = new ObservableCollection<Site>();
                                        }
                                        PlantListForTrackingData.AddRange(timeTrackingSite.siteList);
                                        AllPlantTimeTrackinReport.AddRange(timeTrackingSite.TimeTrackingList);

                                        if (plants == string.Empty)
                                        {
                                            plants = itemPlantOwnerUsers.Name;
                                        }
                                        else
                                        {
                                            plants = plants + "," + itemPlantOwnerUsers.Name;
                                        }
                                    }

                                    catch (FaultException<ServiceException> ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                        if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                        {
                                            FailedPlants.Add(itemPlantOwnerUsers.Name);
                                            if (FailedPlants != null && FailedPlants.Count > 0)
                                            {
                                                IsShowFailedPlantWarning = true;
                                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                            }
                                        }
                                        // System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (ServiceUnexceptedException ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                        if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                        {
                                            FailedPlants.Add(itemPlantOwnerUsers.Name);

                                            if (FailedPlants != null && FailedPlants.Count > 0)
                                            {
                                                IsShowFailedPlantWarning = true;
                                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                            }
                                        }
                                        //  System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method TimeTrackingAcceptButtonCommandAction() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                        //GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                        //[nsatpute][26-06-2025][GEOS2-8641]
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                                            {
                                                GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Failed").ToString();
                                                GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                            }
                                        });

                                        if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                        {
                                            FailedPlants.Add(itemPlantOwnerUsers.Name);
                                            if (FailedPlants != null && FailedPlants.Count > 0)
                                            {
                                                IsShowFailedPlantWarning = true;
                                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                            }
                                        }
                                        // System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }

                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                                    if (FailedPlants == null || FailedPlants.Count == 0)
                                    {
                                        IsShowFailedPlantWarning = false;
                                        WarningFailedPlants = string.Empty;
                                    }
                                }
                            });
                            try
                            {
                                if (timeTrackingSite.AppSettingData != null)  // [pallavi.jadhav][24 06 2025][GEOS2-8678]
                                {


                                    AppSettingData = new List<int>();
                                    AppSettingData = timeTrackingSite.AppSettingData;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                            #region [GEOS2-5099][gulab lakade][1 12 2023]

                            var tempSiteID = timeTrackingSite.TimeTrackingList.GroupBy(a => a.ProductionIdSite).ToList();
                            //PlantListForTrackingData = new ObservableCollection<Site>();
                            TimetrackingProductionList = new List<ERMTimetrackingSite>();
                            foreach (var item in tempSiteID)
                            {
                                ERMTimetrackingSite tempSiteList = new ERMTimetrackingSite();

                                var TempSite = timeTrackingSite.TimeTrackingList.Where(x => x.ProductionIdSite == item.Key).FirstOrDefault();
                                if (TempSite != null)
                                {
                                    tempSiteList.ProductionIdSite = Convert.ToInt32(item.Key);
                                    tempSiteList.ProductionSite = TempSite.ProductionPlant;
                                    tempSiteList.OriginalIdSite = TempSite.IdProductionPlant;
                                    tempSiteList.OriginalSite = TempSite.OriginalPlant;
                                    TimetrackingProductionList.Add(tempSiteList);
                                }

                            }
                            if (TimetrackingProductionList.Count != 0)
                            {
                                foreach (string itemSelectedPlant in plants.Split(','))
                                {
                                    foreach (var item in TimetrackingProductionList.Where(i => i.ProductionSite != itemSelectedPlant.Trim()))
                                    {
                                        try
                                        {

                                            UInt32 ProductionIdSite = Convert.ToUInt32(item.ProductionIdSite);
                                            UInt32 OriginalPlantIdSite = Convert.ToUInt32(item.OriginalIdSite);
                                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.ProductionSite).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                                            ERMService = new ERMServiceController(serviceurl);
                                          //  ERMService = new ERMServiceController("localhost:6699");
                                            // GeosApplication.Instance.SplashScreenMessage = "Please wait while getting data from the plant " + item.ProductionSite;
                                            //[nsatpute][26-06-2025][GEOS2-8641]
                                            Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                                                {
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = "Please wait while getting data from the plant " + item.ProductionSite;
                                                }
                                            });

                                           // AllPlantTimeTrackinReport.AddRange(ERMService.GetTimeTrackingReportBYPlant_V2640(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList, Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date))); //[Pallavi.jadhav][16 05 2025][GEOS2-8124]
                                           AllPlantTimeTrackinReport.AddRange(ERMService.GetTimeTrackingReportBYPlant_V2660V1(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList, Convert.ToDateTime(FromDate.Value.Date), Convert.ToDateTime(ToDate.Value.Date))); //[pallavi.jadhav][07-08-2025][GEOS2-8814]

                                        }

                                        catch (FaultException<ServiceException> ex)
                                        {
                                            //GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                            //GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ProductionSite, " Failed");
											//[nsatpute][26-06-2025][GEOS2-8641]
                                            Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                                                {
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Failed").ToString();
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = String.Concat("Connecting to ", item.ProductionSite, " Failed");
                                                }
                                            });

                                            if (!FailedPlants.Contains(item.ProductionSite))
                                            {
                                                FailedPlants.Add(item.ProductionSite);
                                                if (FailedPlants != null && FailedPlants.Count > 0)
                                                {
                                                    IsShowFailedPlantWarning = true;
                                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                                }
                                            }
                                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ProductionSite, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                        }
                                        catch (ServiceUnexceptedException ex)
                                        {
                                            // GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                            //GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ProductionSite, " Failed");
											//[nsatpute][26-06-2025][GEOS2-8641]
                                            Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                                                {
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Failed").ToString();
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = String.Concat("Connecting to ", item.ProductionSite, " Failed");
                                                }
                                            });
                                            if (!FailedPlants.Contains(item.ProductionSite))
                                            {
                                                FailedPlants.Add(item.ProductionSite);

                                                if (FailedPlants != null && FailedPlants.Count > 0)
                                                {
                                                    IsShowFailedPlantWarning = true;
                                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                                }
                                            }
                                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ProductionSite, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                            GeosApplication.Instance.Logger.Log(string.Format("Error in method TimeTrackingAcceptButtonCommandAction() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                            //GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ProductionSite, " Failed");
											//[nsatpute][26-06-2025][GEOS2-8641]
                                            Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                if (GeosApplication.Instance.DownloadedReportFiles.Any(x => x.FileName == Path.GetFileName(ResultFileName)))
                                                {
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Failed").ToString();
                                                    GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = String.Concat("Connecting to ", item.ProductionSite, " Failed");
                                                }
                                            });
                                            if (!FailedPlants.Contains(item.ProductionSite))
                                            {
                                                FailedPlants.Add(item.ProductionSite);
                                                if (FailedPlants != null && FailedPlants.Count > 0)
                                                {
                                                    IsShowFailedPlantWarning = true;
                                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                                }
                                            }
                                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ProductionSite, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                        }
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        if (FailedPlants == null || FailedPlants.Count == 0)
                                        {
                                            IsShowFailedPlantWarning = false;
                                            WarningFailedPlants = string.Empty;
                                        }
                                    }
                                }
                            }
                            #endregion
                            StagebyExcelColumnIndexlist = new List<ERM_StagebyExcelColumnIndex>();
                            try
                            {

                                worksheet.Cells[0, 0].Value = "From:" + Convert.ToDateTime(FromDate).Date.ToShortDateString();
                                worksheet.Cells[0, 0].Font.Bold = true;
                                worksheet.Cells[0, 2].Value = "Plant: " + plants;
                                worksheet.Cells[0, 2].Font.Bold = true;
                                worksheet.Cells[1, 0].Value = "To:" + Convert.ToDateTime(ToDate).Date.ToShortDateString();
                                worksheet.Cells[1, 0].Font.Bold = true;
                                worksheet.Cells[2, 21].Value = "Sale Price";
                                worksheet.MergeCells(worksheet.Range["V3:W3"]);
                                worksheet.Cells[2, 21].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                                worksheet.Cells[2, 23].Value = "Item Information";
                                worksheet.MergeCells(worksheet.Range["X3:AD3"]);
                                worksheet.Cells[2, 23].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                                int dataRow = 3;
                                worksheet.Cells[3, 0].Value = "Delivery Week";
                                worksheet.Cells[3, 1].Value = "Delivery Date";
                                worksheet.Cells[3, 2].Value = "Planned Delivery Date";
                                worksheet.Cells[3, 3].Value = "First Delivery Date";
                                worksheet.Cells[3, 4].Value = "Quote Send Date";
                                worksheet.Cells[3, 5].Value = "Go Ahead Date";
                                worksheet.Cells[3, 6].Value = "PO Date";
                                worksheet.Cells[3, 7].Value = "Samples";
                                worksheet.Cells[3, 8].Value = "Samples Date";
                                worksheet.Cells[3, 9].Value = "Available For Design Date";
                                worksheet.Cells[3, 10].Value = "PO Type";
                                worksheet.Cells[3, 11].Value = "Customer";
                                worksheet.Cells[3, 12].Value = "Project";
                                worksheet.Cells[3, 13].Value = "Offer";
                                worksheet.Cells[3, 14].Value = "OT Code";
                                worksheet.Cells[3, 15].Value = "Origin Plant";
                                worksheet.Cells[3, 16].Value = "Production Plant";
                                worksheet.Cells[3, 17].Value = "Customer Ref.";
                                worksheet.Cells[3, 18].Value = "Template";
                                worksheet.Cells[3, 19].Value = "Type";
                                worksheet.Cells[3, 20].Value = "QTY";

                                worksheet.Cells[3, 21].Value = "Unit Price";
                                worksheet.Cells[3, 22].Value = "Total Price";

                                #region [GEOS2-5582][Rupali Sarode][05-04-2024]
                                worksheet.Cells[3, 23].Value = "Id Drawing"; //[GEOS2-5582][Rupali Sarode][05-04-2024]

                                //worksheet.Cells[3, 23].Value = "Serial Number";
                                //worksheet.Cells[3, 24].Value = "Item Number";
                                //worksheet.Cells[3, 25].Value = "Item Status";
                                //worksheet.Cells[3, 26].Value = "Design Type";
                                //worksheet.Cells[3, 27].Value = "Tray";
                                //worksheet.Cells[3, 28].Value = "Current WorkStation";
                                //worksheet.Cells[3, 29].Value = "T. Reworks";

                                worksheet.Cells[3, 24].Value = "Serial Number";
                                worksheet.Cells[3, 25].Value = "Item Number";
                                worksheet.Cells[3, 26].Value = "Item Status";
                                worksheet.Cells[3, 27].Value = "Design Type";
                                worksheet.Cells[3, 28].Value = "Tray";
                                worksheet.Cells[3, 29].Value = "Current WorkStation";
                                #region [rani dhamankar][01-04-2025][GEOS2-7097]
                                worksheet.Cells[3, 30].Value = "Design System";
                                worksheet.Cells[3, 31].Value = "Designer";
                                worksheet.Cells[3, 32].Value = "Start Revision";
                                worksheet.Cells[3, 33].Value = "Last Revision";
                                #endregion
                                worksheet.Cells[3, 34].Value = "T. Reworks";
                                worksheet.Cells[3, 35].Value = "Workbook Drawing";


                                #region // [pallavi.jadhav][21 05 2025][GEOS2-8135]
                                worksheet.Cells[3, 36].Value = "Shipment Date"; // [pallavi.jadhav][20 05 2025]
                                worksheet.Cells[3, 37].Value = "No_Of_Ways";
                                worksheet.Cells[3, 38].Value = "Way_Name";
                                worksheet.Cells[3, 39].Value = "No_Of_Detections";
                                worksheet.Cells[3, 40].Value = "Detection_Name";
                                worksheet.Cells[3, 41].Value = "No_Of_Options";
                                worksheet.Cells[3, 42].Value = "Option_Name";
                                #endregion

                                #endregion [GEOS2-5582][Rupali Sarode][05-04-2024]


                                int col1 = 43;//[rani dhamankar][08-04-2025][GEOS2-7097]
                                int col = 43;//[rani dhamankar][08-04-2025][GEOS2-7097] //[GEOS2-5582][Rupali Sarode][05-04-2024]
                                int startColumnIndex = 0;
                                int numberOfColumns = col;
                                for (int coll = startColumnIndex; coll < startColumnIndex + numberOfColumns; coll++)
                                {
                                    worksheet.Columns[coll].WidthInCharacters = 17;
                                }
                                foreach (var item in StagesList.OrderBy(a => a.Sequence).ToList())
                                {
                                    #region Aishwarya[Geos2-6786]
                                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                                    string ActivePlantString = ActivePlantList.Select(a => a.DefaultValue).FirstOrDefault();
                                    var activePlantIds = ActivePlantString?.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToUInt32(id)).ToList();
                                    var SelectedPlants = string.Join(", ", SelectedPlant
                                                                    .Where(a => a.GetType().GetProperty("IdSite") != null)
                                                                    .Select(a => a.GetType().GetProperty("IdSite")?.GetValue(a)?.ToString()));
                                    var filteredPlantOwners = PlantList.Where(plantOwner => SelectedPlants.Contains(Convert.ToString(plantOwner.IdSite))).ToList();
                                    bool isMatch = activePlantIds != null && activePlantIds.Any(id => filteredPlantOwners.Any(owner => owner.IdSite == id));
                                    #endregion


                                    #region [pallavi jadhav][GEOS2-5465][06 11 2024]
                                    if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                    {
                                        if (item.IdStage == 2)//[rani dhamankar] [28 - 03 - 2025][GEOS2 - 7097]  
                                        {

                                            int n = col + 3;
                                            worksheet.Cells[2, col].Value = item.StageCode;
                                            worksheet.Cells[2, col].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                                            int colSpan = 14;

                                            string rangeStart = ExcelCellHelper.ConvertToLetter(col + 1) + "3";
                                            string rangeEnd = ExcelCellHelper.ConvertToLetter(col + 1 + colSpan - 1) + "3";

                                            string rangeAddress = rangeStart + ":" + rangeEnd;

                                            worksheet.MergeCells(worksheet.Range[rangeAddress]);


                                            ERM_StagebyExcelColumnIndex selectedStagebyPDDExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyPDDExcelColumnIndex.IdStage = item.IdStage;
                                            selectedStagebyPDDExcelColumnIndex.ColumnIndex = col;
                                            selectedStagebyPDDExcelColumnIndex.ColumName = "Planned Delivery Date";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyPDDExcelColumnIndex);

                                            ERM_StagebyExcelColumnIndex selectedStagebyDaysExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyDaysExcelColumnIndex.IdStage = item.IdStage;
                                            selectedStagebyDaysExcelColumnIndex.ColumnIndex = col + 1;
                                            selectedStagebyDaysExcelColumnIndex.ColumName = "Days";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyDaysExcelColumnIndex);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex.ColumnIndex = col + 2;
                                            selectedStagebyExcelColumnIndex.ColumName = "Expected";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexDownload = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexDownload.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexDownload.ColumnIndex = col + 3;
                                            selectedStagebyExcelColumnIndexDownload.ColumName = "Download";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexDownload);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexTransferred = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexTransferred.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexTransferred.ColumnIndex = col + 4;
                                            selectedStagebyExcelColumnIndexTransferred.ColumName = "Transferred";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexTransferred);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexEDS = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexEDS.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexEDS.ColumnIndex = col + 5;
                                            selectedStagebyExcelColumnIndexEDS.ColumName = "EDS";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexEDS);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexADDIN = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexADDIN.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexADDIN.ColumnIndex = col + 6;
                                            selectedStagebyExcelColumnIndexADDIN.ColumName = "Addin";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexADDIN);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexPostServer = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexPostServer.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexPostServer.ColumnIndex = col + 7;
                                            selectedStagebyExcelColumnIndexPostServer.ColumName = "PostServer";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexPostServer);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex1 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex1.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex1.ColumnIndex = col + 8;
                                            selectedStagebyExcelColumnIndex1.ColumName = "Production";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex1);

                                            //Aishwarya Ingale[Geos2-6055]
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex2 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex2.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex2.ColumnIndex = col + 9;
                                            selectedStagebyExcelColumnIndex2.ColumName = "Production-Ows";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex2);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex3 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex3.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex3.ColumnIndex = col + 10;
                                            selectedStagebyExcelColumnIndex3.ColumName = "Rework";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex3);

                                            //Aishwarya Ingale[Geos2-6055]
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex4 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex4.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex4.ColumnIndex = col + 11;
                                            selectedStagebyExcelColumnIndex4.ColumName = "Rework-Ows";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex4);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex5 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex5.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex5.ColumnIndex = col + 12;
                                            selectedStagebyExcelColumnIndex5.ColumName = "Real";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex5);
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex6 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex6.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex6.ColumnIndex = col + 13;
                                            selectedStagebyExcelColumnIndex6.ColumName = "Remainning";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex6);
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex7 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex7.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex7.ColumnIndex = col + 14;
                                            selectedStagebyExcelColumnIndex7.ColumName = "FirstValidatedDate";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex7);

                                            if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                            {
                                                worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                worksheet.Cells[3, col + 1].Value = "Days";
                                            }
                                            worksheet.Cells[3, col + 2].Value = "Expected";
                                            worksheet.Cells[3, col + 3].Value = "Download";
                                            worksheet.Cells[3, col + 4].Value = "Transferred";
                                            worksheet.Cells[3, col + 5].Value = "EDS";
                                            worksheet.Cells[3, col + 6].Value = "Addin";
                                            worksheet.Cells[3, col + 7].Value = "PostServer";
                                            worksheet.Cells[3, col + 8].Value = "Production";
                                            worksheet.Cells[3, col + 9].Value = "Production-Ows";//Aishwarya Ingale[Geos2-6055]
                                            worksheet.Cells[3, col + 10].Value = "Rework";
                                            worksheet.Cells[3, col + 11].Value = "Rework-Ows";//Aishwarya Ingale[Geos2-6055]
                                            worksheet.Cells[3, col + 12].Value = "Real";
                                            worksheet.Cells[3, col + 13].Value = "Remainning";
                                            worksheet.Cells[3, col + 14].Value = "FirstValidatedDate";
                                            col = col + 15;

                                        }
                                        else
                                        {
                                            #region// [pallavi.jadhav][21 05 2025][GEOS2-8135]
                                            if (item.IdStage == 8 || item.IdStage == 9 || item.IdStage == 11 || item.IdStage == 10 || item.IdStage == 3 || item.IdStage == 4 || item.IdStage == 5 || item.IdStage == 21 || item.IdStage == 27 || item.IdStage == 28 || item.IdStage == 34 || item.IdStage == 35 || item.IdStage == 37 || item.IdStage == 38 || item.IdStage == 33)
                                            {
                                                int n = col + 3;
                                                worksheet.Cells[2, col].Value = item.StageCode;
                                                worksheet.Cells[2, col].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;


                                                //int colSpan = 3;   //start [gulab lakade][11 03 2024][GEOS2-5466]
                                                int colSpan = 11;   //start [gulab lakade][11 03 2024][GEOS2-5466]

                                                string rangeStart = ExcelCellHelper.ConvertToLetter(col + 1) + "3";
                                                string rangeEnd = ExcelCellHelper.ConvertToLetter(col + 1 + colSpan - 1) + "3";

                                                string rangeAddress = rangeStart + ":" + rangeEnd;

                                                worksheet.MergeCells(worksheet.Range[rangeAddress]);

                                                ERM_StagebyExcelColumnIndex selectedStagebyPDDExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyPDDExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyPDDExcelColumnIndex.ColumnIndex = col;
                                                selectedStagebyPDDExcelColumnIndex.ColumName = "Planned Delivery Date";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyPDDExcelColumnIndex);

                                                ERM_StagebyExcelColumnIndex selectedStagebyDaysExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyDaysExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyDaysExcelColumnIndex.ColumnIndex = col + 1;
                                                selectedStagebyDaysExcelColumnIndex.ColumName = "Days";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyDaysExcelColumnIndex);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex.ColumnIndex = col + 2;
                                                selectedStagebyExcelColumnIndex.ColumName = "Expected";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex1 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex1.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex1.ColumnIndex = col + 3;
                                                selectedStagebyExcelColumnIndex1.ColumName = "Production";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex1);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex2 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex2.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex2.ColumnIndex = col + 4;
                                                selectedStagebyExcelColumnIndex2.ColumName = "Production-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex2);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex3 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex3.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex3.ColumnIndex = col + 5;
                                                selectedStagebyExcelColumnIndex3.ColumName = "Rework";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex3);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex4 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex4.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex4.ColumnIndex = col + 6;
                                                selectedStagebyExcelColumnIndex4.ColumName = "Rework-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex4);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex5 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex5.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex5.ColumnIndex = col + 7;
                                                selectedStagebyExcelColumnIndex5.ColumName = "Real";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex5);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex6 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex6.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex6.ColumnIndex = col + 8;
                                                selectedStagebyExcelColumnIndex6.ColumName = "Remainning";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex6);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex7 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex7.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex7.ColumnIndex = col + 9;
                                                selectedStagebyExcelColumnIndex7.ColumName = "FirstScanDateTime";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex7);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex8 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex8.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex8.ColumnIndex = col + 10;
                                                selectedStagebyExcelColumnIndex8.ColumName = "LastScanDateTime";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex8);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex9 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex9.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex9.ColumnIndex = col + 11;
                                                selectedStagebyExcelColumnIndex9.ColumName = "FirstValidatedDate";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex9);
                                                if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                                {
                                                    worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                    worksheet.Cells[3, col + 1].Value = "Days";
                                                }
                                                worksheet.Cells[3, col + 2].Value = "Expected";
                                                worksheet.Cells[3, col + 3].Value = "Production";
                                                worksheet.Cells[3, col + 4].Value = "Production-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col + 5].Value = "Rework";
                                                worksheet.Cells[3, col + 6].Value = "Rework-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col + 7].Value = "Real";
                                                worksheet.Cells[3, col + 8].Value = "Remainning";
                                                worksheet.Cells[3, col + 9].Value = "FirstScanDateTime";
                                                worksheet.Cells[3, col + 10].Value = "LastScanDateTime";
                                                worksheet.Cells[3, col + 11].Value = "FirstValidatedDate";
                                                col = col + 12;
                                            }
                                            #endregion // [pallavi.jadhav][21 05 2025][GEOS2-8135]
                                            else
                                            {
                                                int n = col + 3;
                                                worksheet.Cells[2, col].Value = item.StageCode;
                                                worksheet.Cells[2, col].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;


                                                //int colSpan = 3;   //start [gulab lakade][11 03 2024][GEOS2-5466]
                                                int colSpan = 9;   //start [gulab lakade][11 03 2024][GEOS2-5466]

                                                string rangeStart = ExcelCellHelper.ConvertToLetter(col + 1) + "3";
                                                string rangeEnd = ExcelCellHelper.ConvertToLetter(col + 1 + colSpan - 1) + "3";

                                                string rangeAddress = rangeStart + ":" + rangeEnd;

                                                worksheet.MergeCells(worksheet.Range[rangeAddress]);

                                                ERM_StagebyExcelColumnIndex selectedStagebyPDDExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyPDDExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyPDDExcelColumnIndex.ColumnIndex = col;
                                                selectedStagebyPDDExcelColumnIndex.ColumName = "Planned Delivery Date";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyPDDExcelColumnIndex);

                                                ERM_StagebyExcelColumnIndex selectedStagebyDaysExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyDaysExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyDaysExcelColumnIndex.ColumnIndex = col + 1;
                                                selectedStagebyDaysExcelColumnIndex.ColumName = "Days";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyDaysExcelColumnIndex);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex.ColumnIndex = col + 2;
                                                selectedStagebyExcelColumnIndex.ColumName = "Expected";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex1 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex1.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex1.ColumnIndex = col + 3;
                                                selectedStagebyExcelColumnIndex1.ColumName = "Production";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex1);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex2 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex2.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex2.ColumnIndex = col + 4;
                                                selectedStagebyExcelColumnIndex2.ColumName = "Production-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex2);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex3 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex3.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex3.ColumnIndex = col + 5;
                                                selectedStagebyExcelColumnIndex3.ColumName = "Rework";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex3);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex4 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex4.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex4.ColumnIndex = col + 6;
                                                selectedStagebyExcelColumnIndex4.ColumName = "Rework-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex4);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex5 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex5.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex5.ColumnIndex = col + 7;
                                                selectedStagebyExcelColumnIndex5.ColumName = "Real";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex5);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex6 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex6.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex6.ColumnIndex = col + 8;
                                                selectedStagebyExcelColumnIndex6.ColumName = "Remainning";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex6);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex7 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex7.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex7.ColumnIndex = col + 9;
                                                selectedStagebyExcelColumnIndex7.ColumName = "FirstValidatedDate";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex7);
                                                if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                                {
                                                    worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                    worksheet.Cells[3, col + 1].Value = "Days";
                                                }
                                                worksheet.Cells[3, col + 2].Value = "Expected";
                                                worksheet.Cells[3, col + 3].Value = "Production";
                                                worksheet.Cells[3, col + 4].Value = "Production-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col + 5].Value = "Rework";
                                                worksheet.Cells[3, col + 6].Value = "Rework-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col + 7].Value = "Real";
                                                worksheet.Cells[3, col + 8].Value = "Remainning";
                                                worksheet.Cells[3, col + 9].Value = "FirstValidatedDate";
                                                col = col + 10;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (item.IdStage == 2)//[rani dhamankar] [28 - 03 - 2025][GEOS2 - 7097]  
                                        {
                                            int n = col1 + 3;
                                            worksheet.Cells[2, col1].Value = item.StageCode;
                                            worksheet.Cells[2, col1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

                                            int colSpan = 12;

                                            string rangeStart = ExcelCellHelper.ConvertToLetter(col1 + 1) + "3";
                                            string rangeEnd = ExcelCellHelper.ConvertToLetter(col1 + 1 + colSpan - 1) + "3";

                                            string rangeAddress = rangeStart + ":" + rangeEnd;

                                            worksheet.MergeCells(worksheet.Range[rangeAddress]);



                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex.ColumnIndex = col1;
                                            selectedStagebyExcelColumnIndex.ColumName = "Expected";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexDownload = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexDownload.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexDownload.ColumnIndex = col1 + 1;
                                            selectedStagebyExcelColumnIndexDownload.ColumName = "Download";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexDownload);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexTransferred = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexTransferred.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexTransferred.ColumnIndex = col1 + 2;
                                            selectedStagebyExcelColumnIndexTransferred.ColumName = "Transferred";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexTransferred);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexEDS = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexEDS.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexEDS.ColumnIndex = col1 + 3;
                                            selectedStagebyExcelColumnIndexEDS.ColumName = "EDS";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexEDS);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexADDIN = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexADDIN.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexADDIN.ColumnIndex = col1 + 4;
                                            selectedStagebyExcelColumnIndexADDIN.ColumName = "Addin";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexADDIN);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndexPostServer = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndexPostServer.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndexPostServer.ColumnIndex = col1 + 5;
                                            selectedStagebyExcelColumnIndexPostServer.ColumName = "PostServer";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndexPostServer);


                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex1 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex1.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex1.ColumnIndex = col1 + 6;
                                            selectedStagebyExcelColumnIndex1.ColumName = "Production";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex1);

                                            //Aishwarya Ingale[Geos2-6055]
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex2 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex2.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex2.ColumnIndex = col1 + 7;
                                            selectedStagebyExcelColumnIndex2.ColumName = "Production-Ows";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex2);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex3 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex3.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex3.ColumnIndex = col1 + 8;
                                            selectedStagebyExcelColumnIndex3.ColumName = "Rework";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex3);

                                            //Aishwarya Ingale[Geos2-6055]
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex4 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex4.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex4.ColumnIndex = col1 + 9;
                                            selectedStagebyExcelColumnIndex4.ColumName = "Rework-Ows";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex4);

                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex5 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex5.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex5.ColumnIndex = col1 + 10;
                                            selectedStagebyExcelColumnIndex5.ColumName = "Real";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex5);
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex6 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex6.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex6.ColumnIndex = col1 + 11;
                                            selectedStagebyExcelColumnIndex6.ColumName = "Remainning";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex6);
                                            ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex7 = new ERM_StagebyExcelColumnIndex();
                                            selectedStagebyExcelColumnIndex7.IdStage = item.IdStage;
                                            selectedStagebyExcelColumnIndex7.ColumnIndex = col1 + 12;
                                            selectedStagebyExcelColumnIndex7.ColumName = "FirstValidatedDate";
                                            StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex7);

                                            if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                            {
                                                //  worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                //   worksheet.Cells[3, col + 1].Value = "Days";
                                            }
                                            worksheet.Cells[3, col1].Value = "Expected";
                                            worksheet.Cells[3, col1 + 1].Value = "Download";
                                            worksheet.Cells[3, col1 + 2].Value = "Transferred";
                                            worksheet.Cells[3, col1 + 3].Value = "EDS";
                                            worksheet.Cells[3, col1 + 4].Value = "Addin";
                                            worksheet.Cells[3, col1 + 5].Value = "PostServer";
                                            worksheet.Cells[3, col1 + 6].Value = "Production";
                                            worksheet.Cells[3, col1 + 7].Value = "Production-Ows";//Aishwarya Ingale[Geos2-6055]
                                            worksheet.Cells[3, col1 + 8].Value = "Rework";
                                            worksheet.Cells[3, col1 + 9].Value = "Rework-Ows";//Aishwarya Ingale[Geos2-6055]
                                            worksheet.Cells[3, col1 + 10].Value = "Real";
                                            worksheet.Cells[3, col1 + 11].Value = "Remainning";
                                            worksheet.Cells[3, col1 + 12].Value = "FirstValidatedDate";
                                            col1 = col1 + 13;

                                        }
                                        #region // [pallavi.jadhav][21 05 2025][GEOS2-8135]
                                        else
                                        {
                                            if (item.IdStage == 8 || item.IdStage == 9 || item.IdStage == 11 || item.IdStage == 10 || item.IdStage == 3 || item.IdStage == 4 || item.IdStage == 5 || item.IdStage == 21 || item.IdStage == 27 || item.IdStage == 28 || item.IdStage == 34 || item.IdStage == 35 || item.IdStage == 37 || item.IdStage == 38 || item.IdStage == 33)
                                            {
                                                int n = col1 + 3;
                                                worksheet.Cells[2, col1].Value = item.StageCode;
                                                worksheet.Cells[2, col1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;


                                                //int colSpan = 3;   //start [gulab lakade][11 03 2024][GEOS2-5466]
                                                int colSpan = 9;   //start [gulab lakade][11 03 2024][GEOS2-5466]

                                                string rangeStart = ExcelCellHelper.ConvertToLetter(col1 + 1) + "3";
                                                string rangeEnd = ExcelCellHelper.ConvertToLetter(col1 + 1 + colSpan - 1) + "3";

                                                string rangeAddress = rangeStart + ":" + rangeEnd;

                                                worksheet.MergeCells(worksheet.Range[rangeAddress]);


                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex.ColumnIndex = col1;
                                                selectedStagebyExcelColumnIndex.ColumName = "Expected";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex);


                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex1 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex1.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex1.ColumnIndex = col1 + 1;
                                                selectedStagebyExcelColumnIndex1.ColumName = "Production";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex1);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex2 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex2.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex2.ColumnIndex = col1 + 2;
                                                selectedStagebyExcelColumnIndex2.ColumName = "Production-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex2);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex3 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex3.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex3.ColumnIndex = col1 + 3;
                                                selectedStagebyExcelColumnIndex3.ColumName = "Rework";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex3);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex4 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex4.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex4.ColumnIndex = col1 + 4;
                                                selectedStagebyExcelColumnIndex4.ColumName = "Rework-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex4);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex5 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex5.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex5.ColumnIndex = col1 + 5;
                                                selectedStagebyExcelColumnIndex5.ColumName = "Real";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex5);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex6 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex6.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex6.ColumnIndex = col1 + 6;
                                                selectedStagebyExcelColumnIndex6.ColumName = "Remainning";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex6);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex7 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex7.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex7.ColumnIndex = col1 + 7;
                                                selectedStagebyExcelColumnIndex7.ColumName = "FirstScanDateTime";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex7);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex8 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex8.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex8.ColumnIndex = col1 + 8;
                                                selectedStagebyExcelColumnIndex8.ColumName = "LastScanDateTime";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex8);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex9 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex9.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex9.ColumnIndex = col1 + 9;
                                                selectedStagebyExcelColumnIndex9.ColumName = "FirstValidatedDate";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex9);
                                                if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                                {
                                                    //  worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                    //   worksheet.Cells[3, col + 1].Value = "Days";
                                                }
                                                worksheet.Cells[3, col1].Value = "Expected";
                                                worksheet.Cells[3, col1 + 1].Value = "Production";
                                                worksheet.Cells[3, col1 + 2].Value = "Production-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col1 + 3].Value = "Rework";
                                                worksheet.Cells[3, col1 + 4].Value = "Rework-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col1 + 5].Value = "Real";
                                                worksheet.Cells[3, col1 + 6].Value = "Remainning";
                                                worksheet.Cells[3, col1 + 7].Value = "FirstScanDateTime";
                                                worksheet.Cells[3, col1 + 8].Value = "LastScanDateTime";
                                                worksheet.Cells[3, col1 + 9].Value = "FirstValidatedDate";
                                                col1 = col1 + 10;

                                                //if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                                //{
                                                //    worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                //    worksheet.Cells[3, col + 1].Value = "Days";
                                                //}


                                            }

                                            else
                                            {
                                                int n = col1 + 3;
                                                worksheet.Cells[2, col1].Value = item.StageCode;
                                                worksheet.Cells[2, col1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;


                                                //int colSpan = 3;   //start [gulab lakade][11 03 2024][GEOS2-5466]
                                                int colSpan = 7;   //start [gulab lakade][11 03 2024][GEOS2-5466]

                                                string rangeStart = ExcelCellHelper.ConvertToLetter(col1 + 1) + "3";
                                                string rangeEnd = ExcelCellHelper.ConvertToLetter(col1 + 1 + colSpan - 1) + "3";

                                                string rangeAddress = rangeStart + ":" + rangeEnd;

                                                worksheet.MergeCells(worksheet.Range[rangeAddress]);


                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex.ColumnIndex = col1;
                                                selectedStagebyExcelColumnIndex.ColumName = "Expected";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex);


                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex1 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex1.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex1.ColumnIndex = col1 + 1;
                                                selectedStagebyExcelColumnIndex1.ColumName = "Production";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex1);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex2 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex2.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex2.ColumnIndex = col1 + 2;
                                                selectedStagebyExcelColumnIndex2.ColumName = "Production-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex2);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex3 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex3.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex3.ColumnIndex = col1 + 3;
                                                selectedStagebyExcelColumnIndex3.ColumName = "Rework";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex3);

                                                //Aishwarya Ingale[Geos2-6055]
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex4 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex4.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex4.ColumnIndex = col1 + 4;
                                                selectedStagebyExcelColumnIndex4.ColumName = "Rework-Ows";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex4);

                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex5 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex5.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex5.ColumnIndex = col1 + 5;
                                                selectedStagebyExcelColumnIndex5.ColumName = "Real";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex5);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex6 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex6.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex6.ColumnIndex = col1 + 6;
                                                selectedStagebyExcelColumnIndex6.ColumName = "Remainning";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex6);
                                                ERM_StagebyExcelColumnIndex selectedStagebyExcelColumnIndex7 = new ERM_StagebyExcelColumnIndex();
                                                selectedStagebyExcelColumnIndex7.IdStage = item.IdStage;
                                                selectedStagebyExcelColumnIndex7.ColumnIndex = col1 + 7;
                                                selectedStagebyExcelColumnIndex7.ColumName = "FirstValidatedDate";
                                                StagebyExcelColumnIndexlist.Add(selectedStagebyExcelColumnIndex7);

                                                if (isMatch) //Aishwarya Ingale[Geos2-6786]
                                                {
                                                    //  worksheet.Cells[3, col].Value = "Planned Delivery Date";
                                                    //   worksheet.Cells[3, col + 1].Value = "Days";
                                                }
                                                worksheet.Cells[3, col1].Value = "Expected";
                                                worksheet.Cells[3, col1 + 1].Value = "Production";
                                                worksheet.Cells[3, col1 + 2].Value = "Production-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col1 + 3].Value = "Rework";
                                                worksheet.Cells[3, col1 + 4].Value = "Rework-Ows";//Aishwarya Ingale[Geos2-6055]
                                                worksheet.Cells[3, col1 + 5].Value = "Real";
                                                worksheet.Cells[3, col1 + 6].Value = "Remainning";
                                                worksheet.Cells[3, col1 + 7].Value = "FirstValidatedDate";
                                                col1 = col1 + 8;
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion


                                    //End [gulab lakade][11 03 2024][GEOS2-5466]
                                }
                                dataRow++;

                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in method TimeTrackingAcceptButtonCommandAction() in filling Timetracking Generate column list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            if (AllPlantTimeTrackinReport != null)
                            {
                                var AllPlantTimeTrackinReportList = AllPlantTimeTrackinReport.OrderBy(x => x.DeliveryWeek).ToList();

                                AllPlantTimeTrackinReport = new List<TimeTracking>();
                                AllPlantTimeTrackinReport = AllPlantTimeTrackinReportList;
                            }
                            if (AllPlantTimeTrackinReport.Count != 0)
                            {
                                #region [GEOS2-6891][pallavi jadhav][10 02 2025]
                                OTsWithIDOTItemList = new List<ReworkData>(); //  [Rupali Sarode][GEOS2-5523][26-03-2024] 
                                OTsWithIDDrawingList = new List<ReworkData>(); // [Rupali Sarode][GEOS2-5523][26-03-2024] 
                                DataTable DataTableForGridLayout = new DataTable();
                                DataTableForGridLayout.Columns.Add("DeliveryWeek", typeof(string));
                                DataTableForGridLayout.Columns.Add("DeliveryDate", typeof(DateTime));
                                DataTableForGridLayout.Columns.Add("OTCode", typeof(string));
                                DataTableForGridLayout.Columns.Add("SerialNumber", typeof(string));
                                foreach (var reworkMail in AllPlantTimeTrackinReport)
                                {


                                    worksheet.Cells[rows, 0].Value = reworkMail.DeliveryWeek;
                                    worksheet.Cells[rows, 1].Value = reworkMail.DeliveryDate;
                                    worksheet.Cells[rows, 2].Value = reworkMail.PlannedDeliveryDate;
                                    worksheet.Cells[rows, 3].Value = reworkMail.FirstDeliveryDate;
                                    worksheet.Cells[rows, 4].Value = reworkMail.QuoteSendDate;
                                    worksheet.Cells[rows, 5].Value = reworkMail.GoAheadDate;
                                    worksheet.Cells[rows, 6].Value = reworkMail.PODate;
                                    worksheet.Cells[rows, 7].Value = reworkMail.Samples;
                                    worksheet.Cells[rows, 8].Value = reworkMail.SamplesDate;
                                    worksheet.Cells[rows, 9].Value = reworkMail.AvailbleForDesignDate;
                                    worksheet.Cells[rows, 10].Value = reworkMail.POType;
                                    worksheet.Cells[rows, 11].Value = reworkMail.Customer;
                                    worksheet.Cells[rows, 12].Value = reworkMail.Project;
                                    worksheet.Cells[rows, 13].Value = reworkMail.Offer;
                                    worksheet.Cells[rows, 14].Value = reworkMail.OTCode;
                                    worksheet.Cells[rows, 15].Value = reworkMail.OriginalPlant;
                                    worksheet.Cells[rows, 16].Value = reworkMail.ProductionPlant;
                                    worksheet.Cells[rows, 17].Value = reworkMail.Reference;
                                    worksheet.Cells[rows, 18].Value = reworkMail.Template;
                                    worksheet.Cells[rows, 19].Value = reworkMail.Type;
                                    worksheet.Cells[rows, 20].Value = reworkMail.QTY;
                                    worksheet.Cells[rows, 21].Value = reworkMail.Unit;
                                    worksheet.Cells[rows, 22].Value = reworkMail.TotalSalePrice;

                                    #region [GEOS2-5582][Rupali Sarode][05-04-2024]
                                    //worksheet.Cells[rows, 23].Value = reworkMail.SerialNumber;
                                    //worksheet.Cells[rows, 24].Value = reworkMail.NumItem;
                                    //worksheet.Cells[rows, 25].Value = reworkMail.ItemStatus;
                                    //worksheet.Cells[rows, 26].Value = reworkMail.DrawingType;
                                    //worksheet.Cells[rows, 27].Value = reworkMail.TrayName;
                                    //worksheet.Cells[rows, 28].Value = reworkMail.CurrentWorkStation;
                                    if (reworkMail.IdDrawing != 0)
                                    {
                                        worksheet.Cells[rows, 23].Value = reworkMail.IdDrawing;
                                    }
                                    worksheet.Cells[rows, 24].Value = reworkMail.SerialNumber;
                                    worksheet.Cells[rows, 25].Value = reworkMail.NumItem;
                                    worksheet.Cells[rows, 26].Value = reworkMail.ItemStatus;
                                    worksheet.Cells[rows, 27].Value = reworkMail.DrawingType;
                                    worksheet.Cells[rows, 28].Value = reworkMail.TrayName;
                                    worksheet.Cells[rows, 29].Value = reworkMail.CurrentWorkStation;
                                    #endregion [GEOS2-5582][Rupali Sarode][05-04-2024]

                                    //  worksheet.Cells[rows, 29].Value = Convert.ToInt32(reworkMail.TRework);

                                    //worksheet.Cells[rows, 0].Value = reworkMail.DeliveryWeek;

                                    #region [rani dhamankar][01-04-2025][GEOS2-7097]
                                    worksheet.Cells[rows, 30].Value = reworkMail.DesignSystem;
                                    worksheet.Cells[rows, 32].Value = reworkMail.StartRevision;
                                    worksheet.Cells[rows, 33].Value = reworkMail.LastRevision;

                                    var OperatorNames = string.Join(",", reworkMail.TimeTrackingAddingPostServer.Where(a => a.TimeTrackIdCounterpart == reworkMail.IdCounterpart).Select(x => x.OperatorName.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList());
                                    string[] names = OperatorNames.Split(',').Select(name => name.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                                    worksheet.Cells[rows, 31].Value = string.Join(", ", names);

                                    #endregion

                                    #region [Rupali Sarode][GEOS2-5523][26-03-2024] calculate rework as per new algorithm

                                    List<TimeTrackingCurrentStage> StageReworksList = new List<TimeTrackingCurrentStage>();
                                    // worksheet.Cells[rows, 29].Value = Convert.ToInt32(reworkMail.TRework);
                                    worksheet.Cells[rows, 34].Value = Convert.ToInt32(reworkMail.TRework); //[rani dhamankar][08-04-2025][GEOS2-7097]// [GEOS2-5582][Rupali Sarode][05-04-2024]
                                    worksheet.Cells[rows, 35].Value = reworkMail.Workbookdrawing;//[rani dhamankar][08-04-2025][GEOS2-7097]//Aishwarya Ingale[Geos2-6034]

                                    #region // [pallavi.jadhav][21 05 2025][GEOS2-8135]
                                    worksheet.Cells[rows, 36].Value = reworkMail.SampleDate; //[pallavi.jadhav][20 05 2025]
                                    worksheet.Cells[rows, 37].Value = reworkMail.No_Of_Ways;
                                    worksheet.Cells[rows, 38].Value = reworkMail.Way_Name;
                                    worksheet.Cells[rows, 39].Value = reworkMail.No_Of_Detections;
                                    worksheet.Cells[rows, 40].Value = reworkMail.Detection_Name;
                                    worksheet.Cells[rows, 41].Value = reworkMail.No_Of_Options;
                                    worksheet.Cells[rows, 42].Value = reworkMail.Option_Name;
                                    #endregion

                                    worksheet.Columns[31].WidthInCharacters = 20;
                                    if (reworkMail.IsBatch == false && reworkMail.TRework > 0)
                                    {
                                        if (reworkMail.IdOTItem != 0)
                                        {
                                            var TempIdOTItem = OTsWithIDOTItemList.Where(j => j.IdOT == reworkMail.IdOt && j.IdOTItem == reworkMail.IdOTItem).FirstOrDefault();

                                            if (TempIdOTItem == null)
                                            {
                                                ReworkData TempReworkOTItem = new ReworkData();

                                                TempReworkOTItem.IdOT = reworkMail.IdOt;
                                                TempReworkOTItem.IdOTItem = reworkMail.IdOTItem;
                                                TempReworkOTItem.IdCounterpart = reworkMail.IdCounterpart;
                                                OTsWithIDOTItemList.Add(TempReworkOTItem);
                                                //FlagNewAlgorithmCOM = false;
                                            }
                                            else
                                            {
                                                //FlagNewAlgorithmCOM = true;

                                                // Update rework for COM stage in Time tracking Lists to display in Time tracking grid 
                                                if (reworkMail.TimeTrackingStage != null)
                                                {
                                                    StageReworksList = reworkMail.TimeTrackingStage.Where(j => OtItemStagesList.Contains(j.NewIdStage) && j.Rework == 1).ToList();

                                                    if (StageReworksList.Count > 0)  // Update rework only if rework is related to specific stage.
                                                    {
                                                        if (reworkMail.TRework > 0)
                                                        {
                                                            reworkMail.TRework = reworkMail.TRework - (ulong?)StageReworksList.Count;
                                                            worksheet.Cells[rows, 34].Value = Convert.ToInt32(reworkMail.TRework);//[rani dhamankar][08-04-2025][GEOS2-7097]
                                                        }

                                                    }
                                                }
                                            }
                                        }

                                        if (reworkMail.IdDrawing != 0 && reworkMail.IdWorkbookOfCpProducts != 0)//Aishwarya Ingale[Geos2-6034]
                                        {
                                            var TempIdDrawing = OTsWithIDDrawingList.Where(j => j.IdOT == reworkMail.IdOt && j.IdDrawing == reworkMail.IdDrawing && j.IdWorkbookOfCpProducts == reworkMail.IdWorkbookOfCpProducts).FirstOrDefault();
                                            if (TempIdDrawing == null)
                                            {
                                                ReworkData TempReworkDrawing = new ReworkData();

                                                TempReworkDrawing.IdOT = reworkMail.IdOt;
                                                TempReworkDrawing.IdOTItem = reworkMail.IdOTItem;
                                                TempReworkDrawing.IdCounterpart = reworkMail.IdCounterpart;
                                                TempReworkDrawing.IdDrawing = reworkMail.IdDrawing;
                                                TempReworkDrawing.IdWorkbookOfCpProducts = reworkMail.IdWorkbookOfCpProducts;//Aishwarya Ingale[Geos2-6034]
                                                OTsWithIDDrawingList.Add(TempReworkDrawing);
                                            }
                                            else
                                            {
                                                // Update rework for CAD & CAM stage in ProductionOutPutReport & AllPlantWeeklyReworksMail Lists to display in mail 

                                                StageReworksList = new List<TimeTrackingCurrentStage>();
                                                if (reworkMail.TimeTrackingStage != null)
                                                {
                                                    StageReworksList = reworkMail.TimeTrackingStage.Where(j => DrawingIdStagesList.Contains(j.NewIdStage) && j.Rework == 1).ToList();

                                                    if (StageReworksList.Count > 0)  // Update rework only if rework is related to specific stage.
                                                    {
                                                        reworkMail.TRework = reworkMail.TRework - (ulong?)StageReworksList.Count;
                                                        //worksheet.Cells[rows, 29].Value = Convert.ToInt32(reworkMail.TRework);
                                                        worksheet.Cells[rows, 34].Value = Convert.ToInt32(reworkMail.TRework); //[rani dhamankar][08-04-2025][GEOS2-7097]// [GEOS2-5582][Rupali Sarode][05-04-2024]

                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion [Rupali Sarode][GEOS2-5523][26-03-2024]

                                    int col1 = 30;


                                    #region 

                                    List<TimeTrackingCurrentStage> TempTimeTrackingByStageList = new List<TimeTrackingCurrentStage>();
                                    var temprecord = reworkMail.TimeTrackingStage.OrderBy(a => a.IdCounterparttracking).ToList();

                                    ReworkOWSList = new List<ERM_Timetracking_rework_ows>();////[GEOS2-8378][gulab lakade][10 06 2025]
                                    var reorderedResult1 = temprecord
                                  .OrderBy(r => r.Startdate)
                                  .ThenBy(r => r.Enddate)
                                  .ToList();
                                    // var reorderedResult1 = temprecord.ToList();

                                    #region [GEOS2-6620][gulab lakade][05 12 2024]
                                    int? checkIdstage_first = 0;
                                    string Production = string.Empty;
                                    //string Rework = string.Empty;

                                    string Rework_First = string.Empty;
                                    string Rework_Second = string.Empty;
                                    string Rework_Third = string.Empty;
                                    string Rework_Fourth = string.Empty;

                                    string POWS_First = string.Empty;
                                    string POWS_Second = string.Empty;
                                    string POWS_Third = string.Empty;
                                    string POWS_Fourth = string.Empty;


                                    string ROWS_First = string.Empty;
                                    string ROWS_Second = string.Empty;
                                    string ROWS_Third = string.Empty;
                                    string ROWS_Fourth = string.Empty;


                                    TimeSpan ProductionTime = new TimeSpan();
                                    TimeSpan ReworkTime = new TimeSpan();
                                    double ReworkTimeIndouble = 0;
                                    TimeSpan TotalProductiontime = new TimeSpan();
                                    TimeSpan TotalReworktime = new TimeSpan();


                                    int? DetectedIdStage_first = 0;

                                    string Rework = string.Empty;
                                    ////rajashri start GEOS2-6054
                                    //double ProductionOWStime = 0;
                                    string POWS = string.Empty;
                                    string ROWS = string.Empty;
                                    TimeSpan TotalProductionOwstime = new TimeSpan();
                                    TimeSpan TotalReworkOwstime = new TimeSpan();
                                    TimeSpan OWSprodandReworkTime = new TimeSpan();
                                    //TimeSpan tempstoreOWSvalues = new TimeSpan();



                                    #endregion

                                    try
                                    {

                                        List<TimeTrackingCurrentStage> ReworkTimeStageList = new List<TimeTrackingCurrentStage>();
                                        foreach (var item in reorderedResult1)
                                        {
                                            DataRow dr = DataTableForGridLayout.NewRow();
                                            dr["DeliveryWeek"] = Convert.ToString(reworkMail.DeliveryWeek);
                                            dr["DeliveryDate"] = Convert.ToString(reworkMail.DeliveryDate);
                                            dr["OTCode"] = Convert.ToString(reworkMail.OTCode);
                                            dr["SerialNumber"] = Convert.ToString(reworkMail.SerialNumber);

                                            #region [GEOS2-6620][gulab lakade][12 12 2024]
                                            if (ReworkOWSList == null)
                                            {
                                                ReworkOWSList = new List<ERM_Timetracking_rework_ows>();
                                            }
                                            #endregion

                                            bool FlagPresentInActivePlants = false;
                                            var Stagerecord = StagesList.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                                            if (Stagerecord != null)
                                            {
                                                //worksheet.Cells[2, col1].Value = Stagerecord.StageCode;
                                                int n = col1 + 2;
                                                //TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                                string[] ArrActiveInPlants;
                                                if (Stagerecord.ActiveInPlants != null && Stagerecord.ActiveInPlants != "")
                                                {
                                                    ArrActiveInPlants = Stagerecord.ActiveInPlants.Split(',');
                                                    List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                                                    FlagPresentInActivePlants = false;
                                                    foreach (Site itemSelectedPlant in tmpSelectedPlant)
                                                    {
                                                        if (FlagPresentInActivePlants == true)
                                                        {
                                                            break;
                                                        }

                                                        foreach (var iPlant in ArrActiveInPlants)
                                                        {
                                                            if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                                            {
                                                                FlagPresentInActivePlants = true;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                }
                                                if (FlagPresentInActivePlants == true || Stagerecord.ActiveInPlants == null || Stagerecord.ActiveInPlants == "")
                                                {
                                                    if (TempTimeTrackingByStageList.Count() > 0)
                                                    {
                                                        //decimal ? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault().Real;
                                                        //decimal? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).FirstOrDefault().Real;

                                                        var Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                                                        decimal? RealTime = 0;
                                                        decimal? ExpectedTime = 0;
                                                        if (Real != null)
                                                        {
                                                            if (Real.Real != null)
                                                            {
                                                                RealTime = Real.Real;
                                                            }
                                                            if (Real.Expected == null)
                                                            {
                                                                if (item.Expected == null || item.Expected == 0)
                                                                {
                                                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                                                                }
                                                                else
                                                                {
                                                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = item.Expected);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = Real.Expected);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                                            //var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage && X.Expected!=null).FirstOrDefault();

                                                            if (item.Expected != null)
                                                            {
                                                                TempTimeTrackingByStage.Expected = item.Expected;
                                                            }
                                                            else
                                                            {
                                                                TempTimeTrackingByStage.Expected = 0;
                                                            }
                                                            TempTimeTrackingByStage.IdStage = item.IdStage;
                                                            TempTimeTrackingByStage.Rework = item.Rework;
                                                            TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(item.TimeDifference);
                                                            TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                                                            TempTimeTrackingByStage.Days = item.Days;
                                                            TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);

                                                        }
                                                        //decimal? Realtime = RealTime + Convert.ToDecimal(ReworkTimeIndouble);
                                                        //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                                                        //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                                                    }
                                                    else
                                                    {
                                                        TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                                        // var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage).FirstOrDefault();

                                                        if (item.Expected != null)
                                                        {
                                                            TempTimeTrackingByStage.Expected = item.Expected;
                                                        }
                                                        else
                                                        {
                                                            TempTimeTrackingByStage.Expected = 0;
                                                        }
                                                        TempTimeTrackingByStage.IdStage = item.IdStage;
                                                        TempTimeTrackingByStage.Rework = item.Rework;
                                                        TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(item.TimeDifference);
                                                        TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                                                        TempTimeTrackingByStage.Days = item.Days;
                                                        TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                                                    }
                                                    #region First Rework is true
                                                    if (item.IdStage == 2)//[rani dhamankar] [28 - 03 - 2025][GEOS2 - 7097]  
                                                    {
                                                        if (!DataTableForGridLayout.Columns.Contains("Download_" + Convert.ToString(item.IdStage)))
                                                            DataTableForGridLayout.Columns.Add("Download_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                        if (!DataTableForGridLayout.Columns.Contains("Transferred_" + Convert.ToString(item.IdStage)))
                                                            DataTableForGridLayout.Columns.Add("Transferred_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                        if (!DataTableForGridLayout.Columns.Contains("EDS_" + Convert.ToString(item.IdStage)))
                                                            DataTableForGridLayout.Columns.Add("EDS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                        if (!DataTableForGridLayout.Columns.Contains("Addin_" + Convert.ToString(item.IdStage)))
                                                            DataTableForGridLayout.Columns.Add("Addin_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                        if (!DataTableForGridLayout.Columns.Contains("PostServer_" + Convert.ToString(item.IdStage)))
                                                            DataTableForGridLayout.Columns.Add("PostServer_" + Convert.ToString(item.IdStage), typeof(TimeSpan));

                                                    }


                                                    if (!DataTableForGridLayout.Columns.Contains("Production_" + Convert.ToString(item.IdStage)))
                                                        DataTableForGridLayout.Columns.Add("Production_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                    if (!DataTableForGridLayout.Columns.Contains("POWS_" + Convert.ToString(item.IdStage)))
                                                        DataTableForGridLayout.Columns.Add("POWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                    if (!DataTableForGridLayout.Columns.Contains("Rework_" + Convert.ToString(item.IdStage)))
                                                        DataTableForGridLayout.Columns.Add("Rework_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                    if (!DataTableForGridLayout.Columns.Contains("ROWS_" + Convert.ToString(item.IdStage)))
                                                        DataTableForGridLayout.Columns.Add("ROWS_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                    if (!DataTableForGridLayout.Columns.Contains("Real_" + Convert.ToString(item.IdStage)))
                                                        DataTableForGridLayout.Columns.Add("Real_" + Convert.ToString(item.IdStage), typeof(TimeSpan));
                                                    //if (reworkflag_First == true)
                                                    if (ReworkOWSList.Count() > 0)
                                                    {
                                                        #region check reworkflag_Second 
                                                        //if (reworkflag_Second == false)
                                                        if (ReworkOWSList.Count() == 1)
                                                        {
                                                            var tempR_P_OWS = ReworkOWSList.Where(a => a.OWS_ID == 1).FirstOrDefault();
                                                            if (tempR_P_OWS != null)
                                                            {
                                                                checkIdstage_first = tempR_P_OWS.CheckStageId;
                                                                DetectedIdStage_first = tempR_P_OWS.DetectedStageId;
                                                            }
                                                            #region reworkflag_First data check and fill only
                                                            #region checkIdstage_first == item.IdStage
                                                            if (checkIdstage_first == item.IdStage)
                                                            {
                                                                try
                                                                {
                                                                    #region reworkflag_First completed
                                                                    string production_first = "Production_" + checkIdstage_first;
                                                                    if (!DataTableForGridLayout.Columns.Contains(Production))
                                                                    {
                                                                        dr[production_first] = TimeSpan.Zero;
                                                                    }
                                                                    if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                    {
                                                                        if (Convert.ToString(dr[production_first]) != null)
                                                                        {
                                                                            string reworkowsInstring = Convert.ToString(dr["Production_" + checkIdstage_first]);
                                                                            if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                            {
                                                                                reworkowsInstring = "0.00:00:00";
                                                                            }
                                                                            string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                            TimeSpan TempproductionTime = TimeSpan.ParseExact(timeString, format, null);

                                                                            if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                            {
                                                                                dr[production_first] = (TempproductionTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                            {
                                                                                dr[production_first] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }

                                                                        }

                                                                    }


                                                                    #region [rani dhamankar][15-04-2025][GEOS2-7097]
                                                                    TimeSpan EDStotalTimeDifference = TimeSpan.Zero;
                                                                    TimeSpan AddintotalTimeDifference = TimeSpan.Zero;
                                                                    TimeSpan PostservertotalTimeDifference = TimeSpan.Zero;
                                                                    TimeSpan ProductionTimeDifference = TimeSpan.Zero;
                                                                    if (item.IdStage == 2)
                                                                    {
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };
                                                                        if (reworkMail.DesignSystem == "EDS")
                                                                        {
                                                                            EDStotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2230 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            if (DataTableForGridLayout.Columns.Contains("EDS_" + Convert.ToString(item.IdStage)))
                                                                            {
                                                                                if (EDStotalTimeDifference != null)
                                                                                {
                                                                                    dr["EDS_" + Convert.ToString(item.IdStage)] = EDStotalTimeDifference;
                                                                                }

                                                                            }
                                                                            AddintotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2228 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            if (DataTableForGridLayout.Columns.Contains("Addin_" + Convert.ToString(item.IdStage)))
                                                                            {
                                                                                if (AddintotalTimeDifference != null)
                                                                                {
                                                                                    dr["Addin_" + Convert.ToString(item.IdStage)] = AddintotalTimeDifference;
                                                                                }
                                                                            }
                                                                            PostservertotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2229 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            if (DataTableForGridLayout.Columns.Contains("PostServer_" + Convert.ToString(item.IdStage)))
                                                                            {
                                                                                if (PostservertotalTimeDifference != null)
                                                                                {
                                                                                    dr["PostServer_" + Convert.ToString(item.IdStage)] = PostservertotalTimeDifference;
                                                                                }
                                                                            }
                                                                            ProductionTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 0 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();

                                                                            if (DataTableForGridLayout.Columns.Contains(Production))
                                                                            {
                                                                                //only testing sathi  dr[production_first] = AddintotalTimeDifference + PostservertotalTimeDifference + ProductionTimeDifference;
                                                                            }
                                                                        }
                                                                        TimeSpan DownloadTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 1920 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (DownloadTimeDifference != null)
                                                                        {
                                                                            dr["Download_" + Convert.ToString(item.IdStage)] = TimeSpan.ParseExact(Convert.ToString(DownloadTimeDifference), format, null);
                                                                        }
                                                                        else
                                                                        {
                                                                            dr["Download_" + Convert.ToString(item.IdStage)] = DBNull.Value;
                                                                        }
                                                                        TimeSpan TransferredTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 1921 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (TransferredTimeDifference != null)
                                                                        {
                                                                            dr["Transferred_" + Convert.ToString(item.IdStage)] = TimeSpan.ParseExact(Convert.ToString(TransferredTimeDifference), format, null);
                                                                        }
                                                                        else
                                                                        {
                                                                            dr["Transferred_" + Convert.ToString(item.IdStage)] = DBNull.Value;
                                                                        }
                                                                    }

                                                                    #endregion

                                                                    // reworkflag_First = false;
                                                                    checkIdstage_first = 0;
                                                                    ReworkTimeIndouble = 0;
                                                                    DetectedIdStage_first = 0;
                                                                    //ProductionOWStime_First = 0;//GEOS2-6054
                                                                    // reworkflag_First = false;
                                                                    // checkIdstage_first = 0;
                                                                    ReworkOWSList.RemoveAll(a => a.OWS_ID == 1);
                                                                    #endregion


                                                                    #region reworkflag_First again true
                                                                    // gulab lakade 19 04 2024
                                                                    if (item.Rework == 1)
                                                                    {
                                                                        ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                        selected_rework_ows.OWS_ID = 1;
                                                                        selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                                        //reworkflag_First = true;
                                                                        checkIdstage_first = item.IdStage;
                                                                        var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                        int lastindex = reorderedResult1.Count();
                                                                        if (lastindex - 1 > DetectedStageIndex)
                                                                        {
                                                                            var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                            if (Detectedrecord != null)
                                                                            {
                                                                                DetectedIdStage_first = Detectedrecord.IdStage;// detected rework stage
                                                                                selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                            }
                                                                            TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                                                                            TempReworkTime.IdStage = Detectedrecord.IdStage;
                                                                            TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                                                                            TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                                                                            ReworkTimeStageList.Add(TempReworkTime);

                                                                        }

                                                                        if (checkIdstage_first == DetectedIdStage_first)
                                                                        {
                                                                            // reworkflag_First = false;
                                                                            checkIdstage_first = 0;
                                                                            DetectedIdStage_first = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            ReworkOWSList.Add(selected_rework_ows);
                                                                        }

                                                                    }
                                                                    #endregion
                                                                    //end gulab lakade 19 04 2024
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    GeosApplication.Instance.Logger.Log(string.Format("Error in method TimeTrackingAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                try
                                                                {

                                                                    #region only Add reworkflag_First data not other  
                                                                    #region  if (item.Rework == 1) DetectedIdStage_Second
                                                                    #region if(checkIdstage_Second==DetectedIdStage_Second)
                                                                    Int32 Max_owsID = ReworkOWSList.Max(a => a.OWS_ID);
                                                                    ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                    selected_rework_ows.OWS_ID = Max_owsID + 1;
                                                                    bool sameCheck_Detect = false;
                                                                    if (item.Rework == 1)
                                                                    {
                                                                        var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                        if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                                        {
                                                                            var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                            if (Detectedrecord != null)
                                                                            {
                                                                                //DetectedIdStage_Second = Detectedrecord.IdStage;
                                                                                selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                            }
                                                                        }
                                                                        selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);

                                                                        // checkIdstage_Second = item.IdStage;
                                                                        if (selected_rework_ows.CheckStageId == selected_rework_ows.DetectedStageId)
                                                                        {
                                                                            sameCheck_Detect = true;
                                                                            //checkIdstage_Second = 0;
                                                                            //DetectedIdStage_Second = 0;
                                                                            //reworkflag_Second = false;
                                                                        }
                                                                        else
                                                                        {
                                                                            ReworkOWSList.Add(selected_rework_ows);

                                                                        }

                                                                    }

                                                                    #endregion

                                                                    if (item.Rework == 1 && sameCheck_Detect == false)
                                                                    {
                                                                        //var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                        //if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                                        //{
                                                                        //    var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                        //    if (Detectedrecord != null)
                                                                        //    {
                                                                        //        DetectedIdStage_Second = Detectedrecord.IdStage;
                                                                        //    }
                                                                        //   // reworkflag_Second = true;
                                                                        //    checkIdstage_Second = item.IdStage;
                                                                        Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                                        Int32 Max_owsID_Add_minus = Max_owsID_Add - 1;
                                                                        var R_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                                        var R_P_OWSMax_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add_minus).FirstOrDefault();

                                                                        #region Rework==1 and DetectedIdStage_first == item.IdStage
                                                                        //if (DetectedIdStage_first == item.IdStage)
                                                                        if (R_P_OWSMax_minus.DetectedStageId == item.IdStage)
                                                                        {

                                                                            string Rework_second = "Rework_" + item.IdStage;
                                                                            if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                            {
                                                                                dr[Rework_second] = TimeSpan.Zero;
                                                                            }
                                                                            if (Convert.ToString(dr[Rework_second]) != null)
                                                                            {
                                                                                string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                {
                                                                                    reworkowsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                                {
                                                                                    dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }

                                                                        }
                                                                        #endregion
                                                                        else
                                                                        {
                                                                            #region Rework OWS
                                                                            string ROWS_second = "ROWS_" + R_P_OWSMax_minus.DetectedStageId;
                                                                            if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                            {
                                                                                dr[ROWS_second] = TimeSpan.Zero;
                                                                            }

                                                                            if (Convert.ToString(dr[ROWS_second]) != null)
                                                                            {
                                                                                string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                {
                                                                                    reworkowsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                                {
                                                                                    dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }
                                                                            #endregion
                                                                            #region Production OWS
                                                                            string POWS_second = "POWS_" + item.IdStage;
                                                                            if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                            {
                                                                                dr[POWS_second] = TimeSpan.Zero;
                                                                            }

                                                                            if (Convert.ToString(dr[POWS_second]) != null)
                                                                            {
                                                                                string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                                {
                                                                                    P_owsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                                {
                                                                                    dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }

                                                                            #endregion
                                                                        }


                                                                        // }

                                                                    }
                                                                    #endregion
                                                                    else
                                                                    {


                                                                        #region if (DetectedIdStage_first != item.IdStage)
                                                                        Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                                        //Int32 Max_owsID_Add_minus = Max_owsID_Add - 1;
                                                                        var R_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                                        //var R_P_OWSMax_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add_minus).FirstOrDefault();

                                                                        //if (DetectedIdStage_first == item.IdStage)
                                                                        if (R_P_OWSMax.DetectedStageId == item.IdStage)
                                                                        {

                                                                            string Rework_second = "Rework_" + item.IdStage;
                                                                            if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                            {
                                                                                dr[Rework_second] = TimeSpan.Zero;
                                                                            }
                                                                            if (Convert.ToString(dr[Rework_second]) != null)
                                                                            {
                                                                                string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                {
                                                                                    reworkowsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                                {
                                                                                    dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            #region  rework ows and production ows


                                                                            POWS_First = "POWS_" + item.IdStage;
                                                                            ROWS_First = "ROWS_" + R_P_OWSMax.DetectedStageId;
                                                                            if (!DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                            {
                                                                                dr[POWS_First] = TimeSpan.Zero;
                                                                            }
                                                                            if (!DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                            {
                                                                                dr[ROWS_First] = TimeSpan.Zero;
                                                                            }
                                                                            if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                            {
                                                                                if (Convert.ToString(dr[ROWS_First]) != null)
                                                                                {
                                                                                    string ROWSInstring = Convert.ToString(dr[ROWS_First]);
                                                                                    if (string.IsNullOrEmpty(Convert.ToString(ROWSInstring)) || ROWSInstring == "0" || ROWSInstring == "00:00:00")
                                                                                    {
                                                                                        ROWSInstring = "00:00:00";
                                                                                    }
                                                                                    string timeString = Convert.ToString(ROWSInstring); // Example time format string
                                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                    TimeSpan tempROWSInstring = TimeSpan.ParseExact(timeString, format, null);
                                                                                    TimeSpan timespan = tempROWSInstring + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                    //tempstoreOWSvalues = timespan;
                                                                                    // TotalProductionOwstime += timespan;
                                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                                    {
                                                                                        dr[ROWS_First] = timespan;

                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                                    {
                                                                                        dr[ROWS_First] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                    }

                                                                                }
                                                                            }
                                                                            TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                            TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                            if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                            {
                                                                                if (Convert.ToString(dr[POWS_First]) != null)
                                                                                {
                                                                                    string POWInstring = Convert.ToString(dr[POWS_First]);
                                                                                    if (string.IsNullOrEmpty(Convert.ToString(POWInstring)) || POWInstring == "0" || POWInstring == "00:00:00")
                                                                                    {
                                                                                        POWInstring = "00:00:00";
                                                                                    }
                                                                                    string timeString = Convert.ToString(POWInstring); // Example time format string
                                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                                    TimeSpan POWInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                                    TimeSpan timespan = POWInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                                    {
                                                                                        dr[POWS_First] = timespan;
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                                    {
                                                                                        dr[POWS_First] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                    }

                                                                                }
                                                                            }

                                                                            #endregion
                                                                        }

                                                                        #endregion
                                                                    }
                                                                    #endregion
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                                }
                                                            }
                                                            #endregion
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {



                                                                #region check reworkflag_Third 
                                                                //if (reworkflag_Third == false)
                                                                if (ReworkOWSList.Count() > 1)
                                                                {
                                                                    Int32 Max_owsID = ReworkOWSList.Max(a => a.OWS_ID);
                                                                    Int32 Max_owsID_minus = Max_owsID - 1;
                                                                    var tempR_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID).FirstOrDefault();
                                                                    var tempR_P_OWSMax_minusone = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_minus).FirstOrDefault();
                                                                    #region reworkflag_Second data check and fill only
                                                                    #region checkIdstage_Second  == item.IdStage
                                                                    //if (checkIdstage_Second == item.IdStage)
                                                                    if (tempR_P_OWSMax.CheckStageId == item.IdStage)
                                                                    {
                                                                        #region   if(checkIdstage_Second==DetectedIdStage_first)
                                                                        //if (checkIdstage_Second == DetectedIdStage_first)
                                                                        if (tempR_P_OWSMax.CheckStageId == tempR_P_OWSMax_minusone.DetectedStageId)
                                                                        {
                                                                            string Rework_second = "Rework_" + item.IdStage;
                                                                            if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                            {
                                                                                dr[Rework_second] = TimeSpan.Zero;
                                                                            }


                                                                            if (Convert.ToString(dr[Rework_second]) != null)
                                                                            {
                                                                                string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                {
                                                                                    reworkowsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                                {
                                                                                    dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }
                                                                        }
                                                                        #endregion
                                                                        else
                                                                        {
                                                                            #region Rework OWS

                                                                            //string ROWS_second = "ROWS_" + DetectedIdStage_first;
                                                                            string ROWS_second = "ROWS_" + tempR_P_OWSMax_minusone.DetectedStageId;
                                                                            if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                            {
                                                                                dr[ROWS_second] = TimeSpan.Zero;
                                                                            }

                                                                            if (Convert.ToString(dr[ROWS_second]) != null)
                                                                            {
                                                                                string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                {
                                                                                    reworkowsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                                {
                                                                                    dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }
                                                                            #endregion
                                                                            #region Production OWS
                                                                            string POWS_second = "POWS_" + item.IdStage;
                                                                            if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                            {
                                                                                dr[POWS_second] = TimeSpan.Zero;
                                                                            }

                                                                            if (Convert.ToString(dr[POWS_second]) != null)
                                                                            {
                                                                                string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                                if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                                {
                                                                                    P_owsInstring = "00:00:00";
                                                                                }
                                                                                string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                                {
                                                                                    dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }

                                                                            #endregion

                                                                        }
                                                                        #region reworkflag_Second completed
                                                                        ReworkOWSList.RemoveAll(a => a.OWS_ID == Max_owsID);

                                                                        //ReworkTimeIndouble = 0;
                                                                        //DetectedIdStage_Second = 0;
                                                                        //ProductionOWStime_Second = 0;//GEOS2-6054
                                                                        //reworkflag_Second = false;
                                                                        //checkIdstage_Second = 0;
                                                                        #endregion


                                                                        #region reworkflag_Second again true
                                                                        // gulab lakade 19 04 2024
                                                                        if (item.Rework == 1)
                                                                        {
                                                                            Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                                            var tempR_P_OWSMax_add = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                                            ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                            selected_rework_ows.OWS_ID = Max_owsID_Add + 1;
                                                                            selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                                            //reworkflag_Second = true;
                                                                            //checkIdstage_Second = item.IdStage;
                                                                            var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                            int lastindex = reorderedResult1.Count();
                                                                            if (lastindex - 1 > DetectedStageIndex)
                                                                            {
                                                                                var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                                if (Detectedrecord != null)
                                                                                {
                                                                                    //DetectedIdStage_Second = Detectedrecord.IdStage;// detected rework stage
                                                                                    selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                                }
                                                                                TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                                                                                TempReworkTime.IdStage = Detectedrecord.IdStage;
                                                                                TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                                                                                TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                                                                                ReworkTimeStageList.Add(TempReworkTime);
                                                                            }
                                                                            ReworkOWSList.Add(selected_rework_ows);


                                                                        }
                                                                        #endregion
                                                                        //end gulab lakade 19 04 2024
                                                                    }
                                                                    else
                                                                    {
                                                                        #region only Add reworkflag_Second data not other  
                                                                        #region  if (item.Rework == 1) DetectedIdStage_Third
                                                                        #region if(checkIdstage_Second==DetectedIdStage_Second)
                                                                        Int32 Max_owsID_1 = ReworkOWSList.Max(a => a.OWS_ID);
                                                                        var tempR_P_OWSMax_1 = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_1).FirstOrDefault();
                                                                        Int32 Max_owsID_1_minus = Max_owsID_1 - 1;
                                                                        var tempR_P_OWSMax_1_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_1_minus).FirstOrDefault();
                                                                        bool sameCheck_Detect = false;
                                                                        if (item.Rework == 1)
                                                                        {
                                                                            ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                            selected_rework_ows.OWS_ID = Max_owsID_1 + 1;
                                                                            var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                            if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                                            {
                                                                                var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                                if (Detectedrecord != null)
                                                                                {
                                                                                    //DetectedIdStage_Third = Detectedrecord.IdStage;
                                                                                    selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                                }
                                                                            }
                                                                            //checkIdstage_Third = item.IdStage;
                                                                            selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                                            //if (checkIdstage_Third == DetectedIdStage_Third )
                                                                            if (selected_rework_ows.CheckStageId == selected_rework_ows.DetectedStageId)
                                                                            {
                                                                                sameCheck_Detect = true;
                                                                                //checkIdstage_Third = 0;
                                                                                //DetectedIdStage_Third = 0;
                                                                                //reworkflag_Third = false;
                                                                            }
                                                                            else
                                                                            {
                                                                                ReworkOWSList.Add(selected_rework_ows);
                                                                            }

                                                                        }

                                                                        #endregion

                                                                        if (item.Rework == 1 && sameCheck_Detect == false)
                                                                        {
                                                                            #region item.Rework == 1 and  if (DetectedIdStage_Second != item.IdStage)
                                                                            //if (DetectedIdStage_Second == item.IdStage)
                                                                            if (tempR_P_OWSMax_1_minus.DetectedStageId == item.IdStage)
                                                                            {

                                                                                string Rework_third = "Rework_" + item.IdStage;
                                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                                {
                                                                                    dr[Rework_third] = TimeSpan.Zero;
                                                                                }
                                                                                if (Convert.ToString(dr[Rework_third]) != null)
                                                                                {
                                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_third]);
                                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                    {
                                                                                        reworkowsInstring = "00:00:00";
                                                                                    }
                                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                                    {
                                                                                        dr[Rework_third] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    dr[Rework_third] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                }

                                                                            }
                                                                            #endregion
                                                                            else
                                                                            {
                                                                                #region Rework OWS
                                                                                if (tempR_P_OWSMax_1_minus.DetectedStageId != 0)
                                                                                {
                                                                                    //string ROWS_second = "ROWS_" + DetectedIdStage_Second;
                                                                                    string ROWS_second = "ROWS_" + tempR_P_OWSMax_1_minus.DetectedStageId;
                                                                                    if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                                    {
                                                                                        dr[ROWS_second] = TimeSpan.Zero;
                                                                                    }

                                                                                    if (Convert.ToString(dr[ROWS_second]) != null)
                                                                                    {
                                                                                        string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                        {
                                                                                            reworkowsInstring = "00:00:00";
                                                                                        }
                                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                                        {
                                                                                            dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                            TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                    }
                                                                                }
                                                                                #endregion
                                                                                #region Production OWS
                                                                                string POWS_second = "POWS_" + item.IdStage;
                                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                                {
                                                                                    dr[POWS_second] = TimeSpan.Zero;
                                                                                }

                                                                                if (Convert.ToString(dr[POWS_second]) != null)
                                                                                {
                                                                                    string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                                    if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                                    {
                                                                                        P_owsInstring = "00:00:00";
                                                                                    }
                                                                                    string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                                    {
                                                                                        dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                }

                                                                                #endregion
                                                                            }



                                                                            //}

                                                                        }
                                                                        #endregion
                                                                        else
                                                                        {

                                                                            #region if (DetectedIdStage_Second != item.IdStage)
                                                                            //if (DetectedIdStage_Second == item.IdStage)
                                                                            if (tempR_P_OWSMax_1.DetectedStageId == item.IdStage)
                                                                            {

                                                                                string Rework_third = "Rework_" + item.IdStage;
                                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                                {
                                                                                    dr[Rework_third] = TimeSpan.Zero;
                                                                                }
                                                                                if (Convert.ToString(dr[Rework_third]) != null)
                                                                                {
                                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_third]);
                                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                                    {
                                                                                        reworkowsInstring = "00:00:00";
                                                                                    }
                                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                                    {
                                                                                        dr[Rework_third] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    dr[Rework_third] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                #region  rework ows and production ows


                                                                                POWS_Second = "POWS_" + item.IdStage;
                                                                                //ROWS_Second = "ROWS_" + DetectedIdStage_Second;
                                                                                ROWS_Second = "ROWS_" + tempR_P_OWSMax_1.DetectedStageId;
                                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                                {
                                                                                    dr[POWS_Second] = TimeSpan.Zero;
                                                                                }
                                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                                {
                                                                                    dr[ROWS_Second] = TimeSpan.Zero;
                                                                                }
                                                                                if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                                {
                                                                                    if (Convert.ToString(dr[ROWS_Second]) != null)
                                                                                    {
                                                                                        string ROWSInstring = Convert.ToString(dr[ROWS_Second]);
                                                                                        if (string.IsNullOrEmpty(Convert.ToString(ROWSInstring)) || ROWSInstring == "0" || ROWSInstring == "00:00:00")
                                                                                        {
                                                                                            ROWSInstring = "00:00:00";
                                                                                        }
                                                                                        string timeString = Convert.ToString(ROWSInstring); // Example time format string
                                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                                        TimeSpan tempROWSInstring = TimeSpan.ParseExact(timeString, format, null);
                                                                                        TimeSpan timespan = tempROWSInstring + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                        //tempstoreOWSvalues = timespan;
                                                                                        // TotalProductionOwstime += timespan;
                                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                                        {
                                                                                            dr[ROWS_Second] = timespan;

                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                                        {
                                                                                            dr[ROWS_Second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                        }

                                                                                    }
                                                                                }
                                                                                TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                                {
                                                                                    if (Convert.ToString(dr[POWS_Second]) != null)
                                                                                    {
                                                                                        string POWInstring = Convert.ToString(dr[POWS_Second]);
                                                                                        if (string.IsNullOrEmpty(Convert.ToString(POWInstring)) || POWInstring == "0" || POWInstring == "00:00:00")
                                                                                        {
                                                                                            POWInstring = "00:00:00";
                                                                                        }
                                                                                        string timeString = Convert.ToString(POWInstring); // Example time format string
                                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                                        TimeSpan POWInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                                        TimeSpan timespan = POWInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                                        {
                                                                                            dr[POWS_Second] = timespan;
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                                        {
                                                                                            dr[POWS_Second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                                        }

                                                                                    }
                                                                                }

                                                                                #endregion
                                                                            }

                                                                            #endregion
                                                                        }
                                                                        #endregion
                                                                    }

                                                                    #endregion
                                                                    #endregion
                                                                }

                                                                #endregion
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion
                                                    else
                                                    {
                                                        try
                                                        {


                                                            #region if (item.Rework == 1) Firstrework==true
                                                            if (item.Rework == 1)
                                                            {
                                                                ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                selected_rework_ows.OWS_ID = 1;
                                                                var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                int lastindex = reorderedResult1.Count();
                                                                if (lastindex - 1 > DetectedStageIndex)
                                                                {
                                                                    try
                                                                    {
                                                                        var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                        if (Detectedrecord != null)
                                                                        {
                                                                            // DetectedIdStage_first = Detectedrecord.IdStage;// detected rework stage
                                                                            selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);// detected rework stage
                                                                        }

                                                                    }
                                                                    catch (Exception ex)
                                                                    {

                                                                    }
                                                                    //end

                                                                    // reworkflag_First = true;
                                                                    // checkIdstage_first = item.IdStage;
                                                                    selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                                    ReworkOWSList.Add(selected_rework_ows);
                                                                    Production = "Production_" + item.IdStage;

                                                                    Rework_First = "Rework_" + selected_rework_ows.DetectedStageId;
                                                                    POWS_First = "POWS_" + selected_rework_ows.DetectedStageId;
                                                                    ROWS_First = "ROWS_" + selected_rework_ows.DetectedStageId;
                                                                    ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    TotalProductiontime += ProductionTime;
                                                                    //dr[Production] = ProductionTime;
                                                                    if (DataTableForGridLayout.Columns.Contains(Production))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                                    {
                                                                        if (!string.IsNullOrEmpty(Convert.ToString(item.TimeDifference)) && Convert.ToString(item.TimeDifference) != "0")
                                                                        {

                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework_First] == DBNull.Value)
                                                                                    dr[Rework_First] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime;
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework_First] == DBNull.Value)
                                                                                    dr[Rework_First] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime;
                                                                            }
                                                                        }

                                                                    }
                                                                    #region [rani dhamankar][15-04-2025][GEOS2-7097]
                                                                    TimeSpan EDStotalTimeDifference = TimeSpan.Zero;
                                                                    TimeSpan AddintotalTimeDifference = TimeSpan.Zero;
                                                                    TimeSpan PostservertotalTimeDifference = TimeSpan.Zero;
                                                                    TimeSpan ProductionTimeDifference = TimeSpan.Zero;
                                                                    if (item.IdStage == 2)
                                                                    {
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };
                                                                        if (reworkMail.DesignSystem == "EDS")
                                                                        {
                                                                            EDStotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2230 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            if (DataTableForGridLayout.Columns.Contains("EDS_" + Convert.ToString(item.IdStage)))
                                                                            {
                                                                                if (EDStotalTimeDifference != null)
                                                                                {
                                                                                    dr["EDS_" + Convert.ToString(item.IdStage)] = EDStotalTimeDifference;
                                                                                }

                                                                            }
                                                                            AddintotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2228 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            if (DataTableForGridLayout.Columns.Contains("Addin_" + Convert.ToString(item.IdStage)))
                                                                            {
                                                                                if (AddintotalTimeDifference != null)
                                                                                {
                                                                                    dr["Addin_" + Convert.ToString(item.IdStage)] = AddintotalTimeDifference;
                                                                                }
                                                                            }
                                                                            PostservertotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2229 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            if (DataTableForGridLayout.Columns.Contains("PostServer_" + Convert.ToString(item.IdStage)))
                                                                            {
                                                                                if (PostservertotalTimeDifference != null)
                                                                                {
                                                                                    dr["PostServer_" + Convert.ToString(item.IdStage)] = PostservertotalTimeDifference;
                                                                                }
                                                                            }

                                                                            ProductionTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 0 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                            ProductionTime = AddintotalTimeDifference + PostservertotalTimeDifference + ProductionTimeDifference;
                                                                            if (DataTableForGridLayout.Columns.Contains(Production))
                                                                            {
                                                                                // dr[Production] = ProductionTime;//[GEOS2-8378][gulab lakade][10 06 2025][remove logic for adding and posteserver in production 
                                                                            }


                                                                        }
                                                                        TimeSpan DownloadTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 1920 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (DownloadTimeDifference != null)
                                                                        {
                                                                            dr["Download_" + Convert.ToString(item.IdStage)] = TimeSpan.ParseExact(Convert.ToString(DownloadTimeDifference), format, null);
                                                                        }
                                                                        else
                                                                        {
                                                                            dr["Download_" + Convert.ToString(item.IdStage)] = DBNull.Value;
                                                                        }
                                                                        TimeSpan TransferredTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 1921 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (TransferredTimeDifference != null)
                                                                        {
                                                                            dr["Transferred_" + Convert.ToString(item.IdStage)] = TimeSpan.ParseExact(Convert.ToString(TransferredTimeDifference), format, null);
                                                                        }
                                                                        else
                                                                        {
                                                                            dr["Transferred_" + Convert.ToString(item.IdStage)] = DBNull.Value;
                                                                        }
                                                                    }
                                                                    #endregion

                                                                }
                                                            }
                                                            #endregion
                                                            else
                                                            {
                                                                Production = "Production_" + item.IdStage;
                                                                Rework = "Rework_" + item.IdStage;
                                                                ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                ReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                                                                POWS = "POWS_" + item.IdStage;
                                                                ROWS = "ROWS_" + item.IdStage;
                                                                OWSprodandReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                                                                TotalProductiontime += ProductionTime;

                                                                #region old production time get
                                                                string oldproductiontime = Convert.ToString(dr[Production]);

                                                                if (string.IsNullOrEmpty(Convert.ToString(oldproductiontime)) || oldproductiontime == "0" || oldproductiontime == "00:00:00")
                                                                {
                                                                    oldproductiontime = "0.00:00:00";
                                                                }
                                                                string oldprod_string = Convert.ToString(oldproductiontime); // Example time format string
                                                                string[] format_Prod = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                TimeSpan OldproductionTime = TimeSpan.ParseExact(oldprod_string, format_Prod, null);
                                                                #endregion
                                                                if (!string.IsNullOrEmpty(Convert.ToString(item.TimeDifference)) && Convert.ToString(item.TimeDifference) != "0")
                                                                {

                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                    {
                                                                        if (dr[Rework] == DBNull.Value)
                                                                            dr[Rework] = TimeSpan.Zero;
                                                                        dr[Production] = ProductionTime + OldproductionTime;
                                                                        dr[POWS] = OWSprodandReworkTime;
                                                                        dr[ROWS] = OWSprodandReworkTime;
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                    {
                                                                        if (dr[Rework] == DBNull.Value)
                                                                            dr[Rework] = TimeSpan.Zero;
                                                                        dr[Production] = ProductionTime + OldproductionTime;
                                                                        dr[POWS] = OWSprodandReworkTime;
                                                                        dr[ROWS] = OWSprodandReworkTime;
                                                                    }

                                                                }
                                                                #region [rani dhamankar][15-04-2025][GEOS2-7097]
                                                                TimeSpan EDStotalTimeDifference = TimeSpan.Zero;
                                                                TimeSpan AddintotalTimeDifference = TimeSpan.Zero;
                                                                TimeSpan PostservertotalTimeDifference = TimeSpan.Zero;
                                                                TimeSpan ProductionTimeDifference = TimeSpan.Zero;
                                                                if (item.IdStage == 2)
                                                                {
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };
                                                                    if (reworkMail.DesignSystem == "EDS")
                                                                    {
                                                                        EDStotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2230 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (DataTableForGridLayout.Columns.Contains("EDS_" + Convert.ToString(item.IdStage)))
                                                                        {
                                                                            if (EDStotalTimeDifference != null)
                                                                            {
                                                                                dr["EDS_" + Convert.ToString(item.IdStage)] = EDStotalTimeDifference;
                                                                            }

                                                                        }
                                                                        AddintotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2228 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (DataTableForGridLayout.Columns.Contains("Addin_" + Convert.ToString(item.IdStage)))
                                                                        {
                                                                            if (AddintotalTimeDifference != null)
                                                                            {
                                                                                dr["Addin_" + Convert.ToString(item.IdStage)] = AddintotalTimeDifference;
                                                                            }
                                                                        }
                                                                        PostservertotalTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 2229 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                        if (DataTableForGridLayout.Columns.Contains("PostServer_" + Convert.ToString(item.IdStage)))
                                                                        {
                                                                            if (PostservertotalTimeDifference != null)
                                                                            {
                                                                                dr["PostServer_" + Convert.ToString(item.IdStage)] = PostservertotalTimeDifference;
                                                                            }
                                                                        }

                                                                        ProductionTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 0 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();

                                                                        if (DataTableForGridLayout.Columns.Contains(Production))
                                                                        {
                                                                            // dr[Production] = AddintotalTimeDifference + PostservertotalTimeDifference + ProductionTimeDifference; //[GEOS2-8378][gulab lakade][10 06 2025][remove logic for adding and posteserver in production 
                                                                        }


                                                                    }
                                                                    TimeSpan DownloadTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 1920 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                    if (DownloadTimeDifference != null)
                                                                    {
                                                                        dr["Download_" + Convert.ToString(item.IdStage)] = TimeSpan.ParseExact(Convert.ToString(DownloadTimeDifference), format, null);
                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Download_" + Convert.ToString(item.IdStage)] = DBNull.Value;
                                                                    }
                                                                    TimeSpan TransferredTimeDifference = reworkMail.TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && x.ProductionActivityTimeType == 1921 && x.TimeTrackDifference > TimeSpan.Zero).Select(a => a.TimeTrackDifference).FirstOrDefault();
                                                                    if (TransferredTimeDifference != null)
                                                                    {
                                                                        dr["Transferred_" + Convert.ToString(item.IdStage)] = TimeSpan.ParseExact(Convert.ToString(TransferredTimeDifference), format, null);
                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Transferred_" + Convert.ToString(item.IdStage)] = DBNull.Value;
                                                                    }
                                                                }
                                                                #endregion
                                                                decimal? tempProduction = Convert.ToDecimal(item.TimeDifference);
                                                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                                                                TotalReworktime += ReworkTime;
                                                                TotalProductionOwstime += OWSprodandReworkTime;
                                                                if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                                {
                                                                    #region Aishwarya Ingale[Geos2-6069]
                                                                    if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                    {
                                                                        dr[Rework] = ReworkTime;
                                                                    }
                                                                    else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                                                                    {
                                                                        dr[Rework] = DBNull.Value;
                                                                        //   TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                    }
                                                                    else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                    {
                                                                        dr[Rework] = ReworkTime;
                                                                        //  TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                    }
                                                                    else if (ProductionTime == TimeSpan.Zero)
                                                                    {
                                                                        dr[Rework] = DBNull.Value;
                                                                        //  TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                    }

                                                                    #endregion


                                                                    if (Convert.ToString(dr[Rework]) != null)
                                                                    {
                                                                        string ReworkInstring = Convert.ToString(dr[Rework]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(ReworkInstring)))
                                                                        {
                                                                            ReworkInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(ReworkInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                        TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                        TimeSpan timespan = TempReworkInTimespan + ReworkTime;

                                                                        if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                                        {
                                                                            #region Aishwarya Ingale[Geos2-6069]
                                                                            if (ProductionTime != TimeSpan.Zero && timespan != TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = timespan;
                                                                            }
                                                                            else if (ProductionTime == TimeSpan.Zero && timespan == TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = DBNull.Value;
                                                                                //  TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                            }
                                                                            else if (ProductionTime == TimeSpan.Zero && timespan != TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = timespan;
                                                                                // TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                            }
                                                                            else if (ProductionTime == TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = DBNull.Value;
                                                                                //TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                            }

                                                                            #endregion
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                                        {
                                                                            #region Aishwarya Ingale[Geos2-6069]
                                                                            if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = ReworkTime;
                                                                            }
                                                                            else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = DBNull.Value;
                                                                                //  TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                            }
                                                                            else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = ReworkTime;
                                                                                // TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                            }
                                                                            else if (ProductionTime == TimeSpan.Zero)
                                                                            {
                                                                                dr[Rework] = DBNull.Value;
                                                                                //TimeTrackingList[i].ProductionHtmlColorFlag = false;
                                                                            }

                                                                            #endregion
                                                                        }

                                                                    }
                                                                }
                                                                //rajashri
                                                                if (DataTableForGridLayout.Columns.Contains(POWS))
                                                                {
                                                                    string productionowsInstring = Convert.ToString(dr[POWS]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(productionowsInstring)) || productionowsInstring == "0" || productionowsInstring == "00:00:00")
                                                                    {
                                                                        productionowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(productionowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string

                                                                    TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                    TimeSpan timespan = OWSprodandReworkTime;
                                                                    if (DataTableForGridLayout.Columns.Contains(POWS))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                                    {
                                                                        dr[POWS] = timespan;
                                                                        dr[ROWS] = timespan;

                                                                    }
                                                                }
                                                                //end


                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                        }
                                                    }



                                                    #region Real value
                                                    string real = "Real_" + item.IdStage;
                                                    TimeSpan Tempreal = TimeSpan.Parse("0");
                                                    string[] dtformat = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };  // Specify the format of the time string
                                                    string tempproduction = Convert.ToString(dr["Production_" + item.IdStage]);
                                                    if (string.IsNullOrEmpty(Convert.ToString(tempproduction)) || tempproduction == "0" || tempproduction == "00:00:00")
                                                    {
                                                        tempproduction = "00:00:00";
                                                    }
                                                    string PRO_timeString = Convert.ToString(tempproduction); // Example time format string

                                                    TimeSpan Pro_InTimespan = TimeSpan.ParseExact(PRO_timeString, dtformat, null);

                                                    string tempRework = Convert.ToString(dr["Rework_" + item.IdStage]);
                                                    if (string.IsNullOrEmpty(Convert.ToString(tempRework)) || tempRework == "0" || tempRework == "00:00:00")
                                                    {
                                                        tempRework = "00:00:00";
                                                    }
                                                    string R_timeString = Convert.ToString(tempRework); // Example time format string

                                                    TimeSpan R_InTimespan1 = TimeSpan.ParseExact(R_timeString, dtformat, null);
                                                    string tempP_OWS = Convert.ToString(dr["POWS_" + item.IdStage]);
                                                    if (string.IsNullOrEmpty(Convert.ToString(tempP_OWS)) || tempP_OWS == "0" || tempP_OWS == "00:00:00")
                                                    {
                                                        tempP_OWS = "00:00:00";
                                                    }
                                                    string POWS_timeString = Convert.ToString(tempP_OWS); // Example time format string

                                                    TimeSpan POWS_InTimespan1 = TimeSpan.ParseExact(POWS_timeString, dtformat, null);
                                                    Tempreal = Pro_InTimespan + R_InTimespan1 + POWS_InTimespan1;
                                                    if (DataTableForGridLayout.Columns.Contains(real))
                                                    {
                                                        if (Tempreal == TimeSpan.Zero)
                                                        {
                                                            dr[real] = DBNull.Value;
                                                        }
                                                        else
                                                        {
                                                            dr[real] = Tempreal;
                                                        }
                                                    }
                                                    #endregion


                                                }
                                                else
                                                {
                                                    //if (item.Rework == 1)
                                                    //{
                                                    //    reworkflag = true;
                                                    //    checkIdstage = item.IdStage;
                                                    //}
                                                }

                                            }
                                            else
                                            {
                                                //if (item.Rework == 1)
                                                //{
                                                //    reworkflag = true;
                                                //    checkIdstage = item.IdStage;
                                                //}
                                            }



                                            DataTableForGridLayout.Rows.Add(dr);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() in filling Timetracking list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }
                                    //TimeSpan TempReworkInTimespans = TimeSpan.Zero;
                                    //TimeSpan R_InTimespan = TimeSpan.Zero;
                                    //TimeSpan POWS_InTimespan = TimeSpan.Zero;
                                    foreach (var stageItem in TempTimeTrackingByStageList)
                                    {
                                        try
                                        {

                                            string real = "Real_" + Convert.ToString(stageItem.IdStage);
                                            double TempExpectedTime = 0;//gulab lakade  mismatch total
                                            TimeSpan Tempreal = TimeSpan.Parse("0");


                                            TimeSpan drProductiontime = TimeSpan.Parse("0");
                                            if (DataTableForGridLayout.Columns.Contains("Production_" + stageItem.IdStage))
                                            {
                                                if (stageItem.IdStage == 1)
                                                {

                                                }
                                                //drrealtime =
                                                // var tmpReal=DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Real_2").HasValue).Select(x => x.Field<TimeSpan?>("Real_2").Value).Sum();
                                                var test = DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Production_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Production_" + stageItem.IdStage).Value).ToList();

                                                if (test.Count == 0)
                                                {
                                                    var TempStagedata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Production").FirstOrDefault();
                                                    if (TempStagedata != null)
                                                    {
                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = drProductiontime.ToString(@"dd\.hh\:mm\:ss");
                                                        string temp = Convert.ToString(worksheet.Cells[rows, TempStagedata.ColumnIndex].Value);
                                                        TimeSpan tempp = TimeSpan.Zero;
                                                        string owsTempValue = string.Empty;
                                                        if (TimeSpan.TryParse(temp, out tempp))
                                                        {
                                                            owsTempValue = Convert.ToString(tempp);
                                                        }
                                                        else if (DateTime.TryParse(temp, out DateTime tempDateTime))
                                                        {
                                                            tempp = tempDateTime.TimeOfDay;
                                                            owsTempValue = Convert.ToString(tempp);
                                                        }
                                                        else
                                                        {
                                                            // Handle invalid format
                                                            tempp = TimeSpan.Zero; // Default value or throw an exception
                                                            owsTempValue = Convert.ToString(tempp);
                                                        }
                                                        if (string.IsNullOrEmpty(Convert.ToString(owsTempValue)))
                                                        {
                                                            owsTempValue = "00.00:00:00";
                                                        }
                                                        string owsTimeString = Convert.ToString(owsTempValue);
                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };

                                                        TimeSpan TempproductionTime = TimeSpan.ParseExact(owsTimeString, format, null);
                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = TempproductionTime.ToString(@"hh\:mm\:ss");
                                                    }
                                                }
                                                foreach (var tmpProduction in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Production_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Production_" + stageItem.IdStage).Value).ToList())
                                                {
                                                    drProductiontime = drProductiontime + tmpProduction;
                                                    var TempStagedata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Production").FirstOrDefault();
                                                    if (TempStagedata != null)
                                                    {
                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = drProductiontime.ToString(@"hh\:mm\:ss");
                                                    }
                                                }


                                            }

                                            TimeSpan drPOWStime = TimeSpan.Parse("0");
                                            if (DataTableForGridLayout.Columns.Contains("POWS_" + stageItem.IdStage))
                                            {
                                                //drrealtime =
                                                // var tmpReal=DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Real_2").HasValue).Select(x => x.Field<TimeSpan?>("Real_2").Value).Sum();

                                                foreach (var tmpPows in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("POWS_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("POWS_" + stageItem.IdStage).Value).ToList())
                                                {
                                                    drPOWStime = drPOWStime + tmpPows;
                                                    var TempStagedata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Production-Ows").FirstOrDefault();
                                                    if (TempStagedata != null)
                                                    {

                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = drPOWStime.ToString(@"dd\.hh\:mm\:ss");

                                                    }
                                                }
                                            }
                                            TimeSpan drReworktime = TimeSpan.Parse("0");
                                            if (DataTableForGridLayout.Columns.Contains("Rework_" + stageItem.IdStage))
                                            {
                                                //drrealtime =
                                                // var tmpReal=DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Real_2").HasValue).Select(x => x.Field<TimeSpan?>("Real_2").Value).Sum();

                                                foreach (var tmpRework in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Rework_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Rework_" + stageItem.IdStage).Value).ToList())
                                                {
                                                    drReworktime = drReworktime + tmpRework;
                                                    var TempStagedata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Rework").FirstOrDefault();
                                                    if (TempStagedata != null)
                                                    {

                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = drReworktime.ToString(@"dd\.hh\:mm\:ss");

                                                    }
                                                }
                                            }
                                            TimeSpan drROWStime = TimeSpan.Parse("0");
                                            if (DataTableForGridLayout.Columns.Contains("ROWS_" + stageItem.IdStage))
                                            {
                                                //drrealtime =
                                                // var tmpReal=DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Real_2").HasValue).Select(x => x.Field<TimeSpan?>("Real_2").Value).Sum();

                                                foreach (var tmpRows in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("ROWS_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("ROWS_" + stageItem.IdStage).Value).ToList())
                                                {
                                                    drROWStime = drROWStime + tmpRows;
                                                    var TempStagedata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Rework-Ows").FirstOrDefault();
                                                    if (TempStagedata != null)
                                                    {

                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = drROWStime.ToString(@"dd\.hh\:mm\:ss");

                                                    }
                                                }
                                            }
                                            TimeSpan drrealtime = TimeSpan.Parse("0");
                                            if (DataTableForGridLayout.Columns.Contains("Real_" + stageItem.IdStage))
                                            {

                                                foreach (var tmpReal in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Real_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Real_" + stageItem.IdStage).Value).ToList())
                                                {
                                                    drrealtime = drrealtime + tmpReal;
                                                    var TempStagedata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Real").FirstOrDefault();
                                                    if (TempStagedata != null)
                                                    {
                                                        // Tempreal = TempReworkInTimespans + R_InTimespan + POWS_InTimespan;

                                                        worksheet.Cells[rows, TempStagedata.ColumnIndex].Value = drrealtime.ToString(@"dd\.hh\:mm\:ss");

                                                    }
                                                }
                                            }


                                            //  TimeSpan Tempreal = TimeSpan.Parse("0");

                                            TimeSpan Tempexpected = TimeSpan.Parse("0");

                                            var Expecteddata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Expected").FirstOrDefault();
                                            if (Expecteddata != null)
                                            {

                                                //Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(stageItem.Expected));

                                                #region [GEOS2-5854][gulab lakade][18 07 2024]
                                                if (CADCAMDesignTypeList.Count() > 0)
                                                {
                                                    var TempCADCAM = CADCAMDesignTypeList.Where(x => x.IdStage == stageItem.IdStage && x.DesignType == Convert.ToString(reworkMail.DrawingType)).FirstOrDefault();
                                                    if (TempCADCAM != null)
                                                    {
                                                        if (TempCADCAM.RoleValue == "C")
                                                        {
                                                            Tempexpected = TimeSpan.FromSeconds(TempCADCAM.DesignValue);
                                                        }
                                                        else
                                                        {

                                                            double doubleExpected = Convert.ToDouble(stageItem.Expected) * Convert.ToDouble(Convert.ToDouble(TempCADCAM.DesignValue) / Convert.ToDouble(100));
                                                            Tempexpected = TimeSpan.FromMinutes(doubleExpected);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        double doubleExpected = Convert.ToDouble(stageItem.Expected);
                                                        Tempexpected = TimeSpan.FromMinutes(doubleExpected);
                                                    }
                                                }
                                                else
                                                {
                                                    double doubleExpected = Convert.ToDouble(stageItem.Expected);
                                                    Tempexpected = TimeSpan.FromMinutes(doubleExpected);
                                                }

                                                #endregion

                                                //[GEOS2-5519][Rupali Sarode][15-04-2024]
                                                //worksheet.Cells[rows, Expecteddata.ColumnIndex].Value = Tempexpected.ToString(@"hh\:mm\:ss");
                                                worksheet.Cells[rows, Expecteddata.ColumnIndex].Value = Tempexpected.ToString(@"dd\.hh\:mm\:ss");

                                                if (reworkMail.ExpectedHtmlColorFlag == true && AppSettingData.Contains(Expecteddata.IdStage))  // [pallavi.jadhav][24 06 2025][GEOS2-8678]
                                                {
                                                    TimeSpan TempexpectedData = TimeSpan.FromMinutes(Convert.ToDouble(0));
                                                    //[GEOS2-5519][Rupali Sarode][15-04-2024]
                                                    worksheet.Cells[rows, Expecteddata.ColumnIndex].Value = TempexpectedData.ToString(@"dd\.hh\:mm\:ss");

                                                    //worksheet.Cells[rows, Expecteddata.ColumnIndex].Value = TempexpectedData.ToString(@"hh\:mm\:ss");

                                                    worksheet.Cells[rows, Expecteddata.ColumnIndex].FillColor = System.Drawing.Color.Gray;
                                                }
                                            }
                                            TimeSpan TempRemaining = TimeSpan.Parse("0");
                                            var Remainingdata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Remainning").FirstOrDefault();
                                            if (Remainingdata != null)
                                            {

                                                var TempProduction = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Production").FirstOrDefault();
                                                if (TempProduction != null)
                                                {
                                                    var tempvalue = worksheet.Cells[rows, TempProduction.ColumnIndex].Value;
                                                    if (string.IsNullOrEmpty(Convert.ToString(tempvalue)))
                                                    {
                                                        //tempvalue = "00:00:00";
                                                        tempvalue = "00.00:00:00"; //[GEOS2-5519][Rupali Sarode][15-04-2024]

                                                    }
                                                    string timeString = Convert.ToString(tempvalue); // Example time format string
                                                                                                     //string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                                                    string[] formats = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };

                                                    TimeSpan TempreworkInTimespan = TimeSpan.ParseExact(timeString, formats, null);
                                                    TimeSpan timespan = TempreworkInTimespan;


                                                    TempRemaining = (Tempexpected - drProductiontime);

                                                    if (TempRemaining.ToString().Contains("-"))
                                                    {
                                                        //[GEOS2-5519][Rupali Sarode][15-04-2024]
                                                        // worksheet.Cells[rows, Remainingdata.ColumnIndex].Value = "-" + TempRemaining.ToString(@"hh\:mm\:ss");
                                                        worksheet.Cells[rows, Remainingdata.ColumnIndex].Value = "-" + TempRemaining.ToString(@"dd\.hh\:mm\:ss");

                                                    }
                                                    else
                                                    {
                                                        //worksheet.Cells[rows, Remainingdata.ColumnIndex].Value = TempRemaining.ToString(@"hh\:mm\:ss");
                                                        worksheet.Cells[rows, Remainingdata.ColumnIndex].Value = TempRemaining.ToString(@"dd\.hh\:mm\:ss");
                                                    }
                                                }

                                            }

                                            #region [pallavi jadhav][GEOS2-5465][07 11 2024]

                                            var PDDdata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Planned Delivery Date").FirstOrDefault();
                                            if (PDDdata != null)
                                            {
                                                worksheet.Cells[rows, PDDdata.ColumnIndex].Value = stageItem.PlannedDeliveryDateByStage;

                                            }
                                            var Daysdata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Days").FirstOrDefault();

                                            if (Daysdata != null)
                                            {
                                                if (stageItem.Days != 0)
                                                {
                                                    worksheet.Cells[rows, Daysdata.ColumnIndex].Value = stageItem.Days;
                                                }

                                            }
                                            #endregion
                                            #region [rani dhamankar][31-03-2025][GEOS2-7097]

                                            if (stageItem.IdStage == 2)
                                            {

                                                TimeSpan drEDStime = TimeSpan.Parse("00.00:00:00");
                                                if (DataTableForGridLayout.Columns.Contains("EDS_" + stageItem.IdStage))
                                                {
                                                    // [rani dhamankar] [23 - 05 - 2025][GEOS2 - 8131]
                                                    foreach (var tmpTime in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("EDS_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("EDS_" + stageItem.IdStage).Value).ToList())
                                                    {
                                                        drEDStime = drEDStime + tmpTime;
                                                        var EDSdata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "EDS").FirstOrDefault();
                                                        if (EDSdata != null)
                                                        {
                                                            worksheet.Cells[rows, EDSdata.ColumnIndex].Value = drEDStime.ToString(@"dd\.hh\:mm\:ss");

                                                        }
                                                    }

                                                }
                                                TimeSpan drAddintime = TimeSpan.Parse("00.00:00:00");
                                                if (DataTableForGridLayout.Columns.Contains("Addin_" + stageItem.IdStage))
                                                {
                                                    // [rani dhamankar] [23 - 05 - 2025][GEOS2 - 8131]
                                                    foreach (var tmpTime in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Addin_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Addin_" + stageItem.IdStage).Value).ToList())
                                                    {
                                                        drAddintime = drAddintime + tmpTime;
                                                        var Addindata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Addin").FirstOrDefault();
                                                        if (Addindata != null)
                                                        {
                                                            worksheet.Cells[rows, Addindata.ColumnIndex].Value = drAddintime.ToString(@"dd\.hh\:mm\:ss");

                                                        }
                                                    }
                                                }
                                                TimeSpan drPostServertime = TimeSpan.Parse("00.00:00:00");
                                                if (DataTableForGridLayout.Columns.Contains("PostServer_" + stageItem.IdStage))
                                                {
                                                    // [rani dhamankar] [23 - 05 - 2025][GEOS2 - 8131]
                                                    foreach (var tmpTime in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("PostServer_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("PostServer_" + stageItem.IdStage).Value).ToList())
                                                    {
                                                        drPostServertime = drPostServertime + tmpTime;
                                                        var PostServerdata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "PostServer").FirstOrDefault();
                                                        if (PostServerdata != null)
                                                        {
                                                            worksheet.Cells[rows, PostServerdata.ColumnIndex].Value = drPostServertime.ToString(@"dd\.hh\:mm\:ss");

                                                        }
                                                    }


                                                }
                                                TimeSpan drDownloadtime = TimeSpan.Parse("00.00:00:00");
                                                if (DataTableForGridLayout.Columns.Contains("Download_" + stageItem.IdStage))
                                                {
                                                    // [rani dhamankar] [23 - 05 - 2025][GEOS2 - 8131]
                                                    foreach (var tmpTime in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Download_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Download_" + stageItem.IdStage).Value).ToList())
                                                    {
                                                        drDownloadtime = drDownloadtime + tmpTime;
                                                        var Downloaddata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Download").FirstOrDefault();
                                                        if (Downloaddata != null)
                                                        {
                                                            worksheet.Cells[rows, Downloaddata.ColumnIndex].Value = drDownloadtime.ToString(@"dd\.hh\:mm\:ss");

                                                        }
                                                    }



                                                }
                                                TimeSpan drTransferredtime = TimeSpan.Parse("00.00:00:00");
                                                if (DataTableForGridLayout.Columns.Contains("Transferred_" + stageItem.IdStage))
                                                {
                                                    // [rani dhamankar] [23 - 05 - 2025][GEOS2 - 8131]
                                                    foreach (var tmpTime in DataTableForGridLayout.AsEnumerable().Where(a => a.Field<string>("SerialNumber") == reworkMail.SerialNumber && a.Field<TimeSpan?>("Transferred_" + stageItem.IdStage).HasValue).Select(x => x.Field<TimeSpan?>("Transferred_" + stageItem.IdStage).Value).ToList())
                                                    {
                                                        drTransferredtime = drTransferredtime + tmpTime;
                                                        var Transferreddata = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "Transferred").FirstOrDefault();
                                                        if (Transferreddata != null)
                                                        {
                                                            worksheet.Cells[rows, Transferreddata.ColumnIndex].Value = drTransferredtime.ToString(@"dd\.hh\:mm\:ss");

                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region // [pallavi.jadhav][21 05 2025][GEOS2-8135]

                                            #region [pallavi.jadhav][24 07 2025][GEOS2-8814]
                                            if (reworkMail.TimeTrackingStage != null)
                                            {

                                                if (stageItem.IdStage == 3)
                                                {
                                    
                                                    var FirstValidatedDatecolumn = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "FirstValidatedDate").FirstOrDefault();
                                                    DateTime? FirstValidatedDate = reworkMail.TimeTrackingStage.Where(b => b.IdStage == stageItem.IdStage && b.Rework == 0).OrderBy(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                    if (FirstValidatedDatecolumn != null)
                                                    {
                                                        worksheet.Cells[rows, FirstValidatedDatecolumn.ColumnIndex].Value = FirstValidatedDate;
                                                    }

                                                }
                                                else
                                                {
                                                    var FirstValidatedDatecolumn = StagebyExcelColumnIndexlist.Where(x => x.IdStage == stageItem.IdStage && x.ColumName == "FirstValidatedDate").FirstOrDefault();
                                                    DateTime? FirstValidatedDate = reworkMail.TimeTrackingStage.Where(b => b.IdStage == stageItem.IdStage && b.Rework == 0).OrderBy(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                    if (FirstValidatedDatecolumn != null)
                                                    {
                                                        worksheet.Cells[rows, FirstValidatedDatecolumn.ColumnIndex].Value = FirstValidatedDate;
                                                    }

                                                    //  double firstscanDateTime =reworkMail.TimeTrackingStage.Where(b => b.IdStage == 8).OrderBy(b => b.IdCounterparttracking).Select(b => b.TimeDifference).FirstOrDefault();
                                                  
                                                }
                                            }
                                            #endregion
                                            var FirstScanDateTime8 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 8 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime8 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 8 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                //  double firstscanDateTime =reworkMail.TimeTrackingStage.Where(b => b.IdStage == 8).OrderBy(b => b.IdCounterparttracking).Select(b => b.TimeDifference).FirstOrDefault();
                                                // double lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 8).OrderByDescending(b => b.IdCounterparttracking).Select(b => b.TimeDifference).FirstOrDefault();
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 8).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 8).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime8 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    //string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime8.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime8 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime8.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime9 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 9 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime9 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 9 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 9).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 9).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime9 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime9.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime9 != null)
                                                {
                                                    //TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime9.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }

                                            var FirstScanDateTime11 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 11 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime11 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 11 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 11).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 11).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime11 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime11.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime11 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime11.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }

                                            var FirstScanDateTime10 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 10 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime10 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 10 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 10).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 10).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime10 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime10.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime10 != null)
                                                {
                                                    //  TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime10.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime3 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 3 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime3 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 3 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 3).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 3).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime3 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime3.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime3 != null)
                                                {
                                                    //TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime3.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime4 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 4 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime4 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 4 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 4).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 4).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime4 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime4.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime4 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime4.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime5 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 5 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime5 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 5 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 5).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 5).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime5 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime5.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime5 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime5.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime21 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 21 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime21 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 21 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 21).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 21).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime21 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime21.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime21 != null)
                                                {
                                                    //   TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime21.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime27 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 27 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime27 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 27 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 27).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 27).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime27 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime27.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime27 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime27.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime28 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 28 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime28 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 28 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 28).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 28).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime28 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime28.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime28 != null)
                                                {
                                                    //TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime28.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime34 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 34 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime34 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 34 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 34).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 34).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime34 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime34.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime34 != null)
                                                {
                                                    //TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime34.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime35 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 35 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime35 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 35 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 35).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 35).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime35 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime35.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime35 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime35.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime37 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 37 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime37 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 37 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 37).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 37).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime37 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime37.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime37 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime37.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime38 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 38 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime38 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 38 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 38).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 38).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime38 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime38.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime38 != null)
                                                {
                                                    //  TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime38.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            var FirstScanDateTime33 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 33 && x.ColumName == "FirstScanDateTime").FirstOrDefault();
                                            var LastScanDateTime33 = StagebyExcelColumnIndexlist.Where(x => x.IdStage == 33 && x.ColumName == "LastScanDateTime").FirstOrDefault();
                                            if (reworkMail.TimeTrackingStage != null)
                                            {
                                                DateTime? firstscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 33).OrderBy(b => b.Startdate).Select(b => b.Startdate).FirstOrDefault();
                                                DateTime? lasttscanDateTime = reworkMail.TimeTrackingStage.Where(b => b.IdStage == 33).OrderByDescending(b => b.Enddate).Select(b => b.Enddate).FirstOrDefault();
                                                if (FirstScanDateTime33 != null)
                                                {
                                                    // TimeSpan timeSpan = TimeSpan.FromHours(firstscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, FirstScanDateTime33.ColumnIndex].Value = firstscanDateTime;

                                                }
                                                if (LastScanDateTime33 != null)
                                                {
                                                    //TimeSpan timeSpan = TimeSpan.FromHours(lasttscanDateTime);
                                                    // string formatted = timeSpan.ToString(@"hh\:mm\:ss");
                                                    worksheet.Cells[rows, LastScanDateTime33.ColumnIndex].Value = lasttscanDateTime;
                                                }
                                            }
                                            #endregion

                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log(string.Format("Error in method TimeTrackingAcceptButtonCommandAction() in filling Timetracking list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }

                                    rows++;
                                }
                                #endregion
                                #endregion


                            }
                        }
                        try
                        {
                            workbook.SaveDocument(ResultFileName, DocumentFormat.Xlsx);                            
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                        catch (Exception ex)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    });
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).ConnectingSites = GeosApplication.Instance.DownloadedReportFiles.FirstOrDefault(x => x.FileName == Path.GetFileName(ResultFileName)).Status = System.Windows.Application.Current.FindResource("Erm_Windowviewmodel_Completed").ToString();                        
                    });
                }
                #endregion

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method TimeTrackingAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }





        private void FillStages()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillStages ...", category: Category.Info, priority: Priority.Low);

                StagesList = new List<DeliveryVisualManagementStages>();
                //  StagesList = new List<TimeTrackingProductionStage>();
                OtItemStagesList = new List<int>();
                DrawingIdStagesList = new List<int>();

              //  ERMService = new ERMServiceController("localhost:6699");
                StagesList.AddRange(ERMService.GetDVManagementProductionStage_V2400());
                //  TimetrackingStagesList = ERMService.GetAllTimeTrackingProductionStage_V2500(); // This is changed because we need stages lists for new algorithm for COM, CAD and CAM stages
                TimetrackingStagesList = ERMService.GetAllStagesPerIDOTItemAndIDDrawing_V2500();
                // StagesList.AddRange(TimetrackingStagesList.AllStagesList);
                OtItemStagesList = TimetrackingStagesList.OTITemStagesList.Select(i => i.IdStage).ToList();
                DrawingIdStagesList = TimetrackingStagesList.DrawingIdStagesList.Select(i => i.IdStage).ToList();

                GeosApplication.Instance.Logger.Log("Method FillStages() executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStages() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private Workbook ExportDataToExcel(List<ERMDeliveryVisualManagement> ListToBeExported, int WorkSheetNumber, Workbook workbook, string WorkSheetName)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDataToExcel ...", category: Category.Info, priority: Priority.Low);

                if (WorkSheetNumber > 0)
                {
                    workbook.Worksheets.Add();
                    //workbook.Worksheets


                }

                int TotalCP = 0;
                Worksheet worksheet = workbook.Worksheets[WorkSheetNumber];
                worksheet.Name = WorkSheetName;

                // apply colors to worksheet tab
                switch (WorkSheetNumber)
                {
                    case 0:
                        worksheet.ActiveView.TabColor = System.Drawing.Color.Blue;
                        break;
                    case 1:
                        worksheet.ActiveView.TabColor = System.Drawing.Color.Green;
                        break;
                    case 2:
                        worksheet.ActiveView.TabColor = System.Drawing.Color.Yellow;
                        break;
                    case 3:
                        worksheet.ActiveView.TabColor = System.Drawing.Color.Red;
                        break;
                    case 4:
                        worksheet.ActiveView.TabColor = System.Drawing.Color.Orange;
                        break;
                }

                worksheet.Cells[0, 0].Value = "RTM Data - " + WorkSheetName + " - at " + DateTime.Now.Date.ToShortDateString() + " at " + DateTime.Now.ToString("H:mm");
                worksheet.MergeCells(worksheet.Range["A1:C1"]);
                if (ListToBeExported == null)
                {
                    worksheet.Cells[1, 4].Value = "TOTAL";
                    worksheet.Cells[1, 5].Value = 0;
                }
                else if (ListToBeExported.Count == 0)
                {
                    worksheet.Cells[1, 4].Value = "TOTAL";
                    worksheet.Cells[1, 5].Value = 0;
                }
                for (int i = 0; i < 6; i++)
                {
                    worksheet.Columns[i].WidthInCharacters = 14;
                }
                worksheet.Cells[0, 0].Font.Bold = true;
                worksheet.Cells[1, 4].Font.Bold = true;
                worksheet.Cells[1, 4].Value = "TOTAL";

                worksheet.Cells[2, 0].Font.Bold = true;
                worksheet.Cells[2, 1].Font.Bold = true;
                worksheet.Cells[2, 2].Font.Bold = true;
                worksheet.Cells[2, 3].Font.Bold = true;
                worksheet.Cells[2, 4].Font.Bold = true;
                worksheet.Cells[2, 5].Font.Bold = true;
                worksheet.Cells[2, 6].Font.Bold = true;

                worksheet.Cells[2, 0].Value = "Plant";
                worksheet.Cells[2, 1].Value = "OT Code";
                worksheet.Cells[2, 2].Value = "Item Number";
                worksheet.Cells[2, 3].Value = "Station";
                worksheet.Cells[2, 4].Value = "Delivery Date";
                worksheet.Cells[2, 5].Value = "Template";
                worksheet.Cells[2, 6].Value = "Item Status";

                if (ListToBeExported != null)
                {
                    if (ListToBeExported.Count > 0)
                    {
                        //  TotalCP = ListToBeExported.Select(i => i.IdCounterpart).Distinct().Count();
                        TotalCP = ListToBeExported.Select(i => i.IdCounterpart).Count();
                        worksheet.Cells[1, 5].Value = TotalCP;

                        int rows = 3;
                        foreach (ERMDeliveryVisualManagement Item in ListToBeExported)
                        {
                            worksheet.Cells[rows, 0].Value = Item.OriginalPlant;
                            worksheet.Cells[rows, 1].Value = Item.OTCode;
                            worksheet.Cells[rows, 2].Value = Item.NumItem;
                            worksheet.Cells[rows, 3].Value = Item.Station;
                            worksheet.Cells[rows, 4].Value = Item.DeliveryDate;
                            worksheet.Cells[rows, 5].Value = Item.TemplateName;
                            worksheet.Cells[rows, 6].Value = Item.ItemStatus;
                            //  worksheet.Cells[rows, 6].Value = Item.IdCounterpart;
                            rows++;

                        }

                        // 
                    }

                }
                GeosApplication.Instance.Logger.Log("Method ExportDataToExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExportDataToExcel() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            return workbook;
        }


        //[nsatpute][26-06-2025][GEOS2-8641]
        private void FillListOfColor()
        {
            try
            {
                if (GeosApplication.Instance.TrackingTimeGeosAppSettingList == null)
                    GeosApplication.Instance.TrackingTimeGeosAppSettingList = GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("14,15,16,17");
                else
                    GeosAppSettingList = GeosApplication.Instance.TrackingTimeGeosAppSettingList.ToList();
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

        private void FillDataByPlant()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDataByPlant ...", category: Category.Info, priority: Priority.Low);
                TimeTrackingList = new List<TimeTracking>();
                OffersOptionsList offersOptionsLst = new OffersOptionsList();
                if (SelectedPlant != null)
                {
                    //List<Site> plantOwners = SelectedPlant.Cast<Site>().ToList();
                    //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    //PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.IdSite));
                    if (FailedPlants == null)
                    {
                        FailedPlants = new List<string>();
                    }

                    foreach (Site itemPlant in SelectedPlant)
                    {
                        var TempRemainingPlant = PlantList.Where(x => x.IdSite == itemPlant.IdSite).ToList();
                        foreach (var itemPlantOwnerUsers in TempRemainingPlant)
                        {
                            try
                            {

                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;
                                //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                                //==========================================================================================
                                Int32 IdSite = Convert.ToInt32(itemPlantOwnerUsers.IdSite);
                                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                                //if(serviceurl!=null)
                                //{
                                ERMService = new ERMServiceController(serviceurl);
                                //}

                                //  ERMService = new ERMServiceController("localhost:6699");
                                //TimeTrackingList.AddRange(ERMService.GetAllTimeTracking_V2330());

                                TimeTrackingWithSites timeTrackingSite = new TimeTrackingWithSites();

                                TimeTrackingList.AddRange(ERMService.GetDeliveryDateForTimeTrackingReport_V2410(IdSite));
                                //PlantListForTrackingData = new ObservableCollection<Site>();
                                //PlantListForTrackingData.AddRange(timeTrackingSite.siteList);

                                //TimeTrackingList.AddRange(timeTrackingSite.TimeTrackingList);




                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.Name);
                                        if (FailedPlants != null && FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        }
                                    }

                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.Name);
                                        if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        }
                                    }

                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))

                                    if (!FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                    {
                                        FailedPlants.Add(itemPlantOwnerUsers.Name);

                                        if (FailedPlants != null && FailedPlants.Count > 0)
                                        {
                                            IsShowFailedPlantWarning = true;
                                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                        }
                                    }
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillDataByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByPlant() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDataByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillDataByPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        private void GetPlants()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);
                if (PlantList == null || PlantList.Count == 0)
                {
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                    AllPlantList = PlantList.ToList();
                }

                //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                //if (SelectedPlant == null)
                //{
                //    SelectedPlant = new List<object>();
                //}
                //SelectedPlant.Add(PlantList.FirstOrDefault(x => x.Name == serviceurl));
                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdSite).FirstOrDefault());
                List<Site> PlantList1 = new List<Site>();
                foreach (Site item in plantOwners)
                {

                    UInt32 plantid = Convert.ToUInt32(item.IdSite);
                    PlantList1 = PlantList.Where(x => x.IdSite == plantid).ToList();
                    if (SelectedPlant == null)
                        SelectedPlant = new List<object>();

                    foreach (Site plant in PlantList1)
                    {
                        SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plant.IdSite));
                        if (SelectedPlantold == null)
                            SelectedPlantold = new List<object>();
                        SelectedPlantold = SelectedPlant;
                    }

                }

                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Validation

        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => FromDate)] +
                    me[BindableBase.GetPropertyName(() => ToDate)];



                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string selectedFromDate = BindableBase.GetPropertyName(() => FromDate);
                string selectedToDate = BindableBase.GetPropertyName(() => ToDate);


                if (columnName == selectedFromDate)
                {
                    return DVMAndTimeTrackingValidation.GetErrorMessage(selectedFromDate, FromDate, ToDate);
                }

                if (columnName == selectedToDate)
                {
                    return DVMAndTimeTrackingValidation.GetErrorMessage(selectedToDate, ToDate, FromDate);
                }
                return null;
            }
        }


        #endregion
        private class ReworkData
        {
            public Int64 IdOT;
            public Int64 IdOTItem;
            public Int64 IdDrawing;
            public Int64 IdCounterpart;
            public Int64 IdWorkbookOfCpProducts;//Aishwarya Ingale[Geos2-6034]
        }
        #region [GEOS2-5854][gulab lakade][18 07 2024]
        private void FillCADCAMDesignTypeList()
        {
            try
            {
                List<GeosAppSetting> AppSetting = new List<GeosAppSetting>();
                AppSetting = WorkbenchStartUp.GetSelectedGeosAppSettings("124");
                if (AppSetting.Count > 0)
                {
                    CADCAMDesignTypeList = new List<ERM_CADCAMTimePerDesignType>();
                    List<string> tempWorkStageList = new List<string>();
                    foreach (var item in AppSetting)
                    {
                        string tempstring = Convert.ToString(item.DefaultValue.Replace('(', ' '));
                        tempstring = tempstring.Replace(')', ' ');
                        tempWorkStageList = Convert.ToString(tempstring).Split(',').ToList();
                    }
                    if (tempWorkStageList.Count > 0)
                    {
                        foreach (var item in tempWorkStageList)
                        {
                            List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
                            if (tempIDStageList.Count() > 0)
                            {
                                ERM_CADCAMTimePerDesignType selectCADCAMDesignTypeList = new ERM_CADCAMTimePerDesignType();
                                selectCADCAMDesignTypeList.IdStage = Convert.ToInt32(tempIDStageList[0]);
                                selectCADCAMDesignTypeList.DesignType = Convert.ToString(tempIDStageList[1]);
                                selectCADCAMDesignTypeList.DesignValue = Convert.ToInt32(tempIDStageList[2]);
                                selectCADCAMDesignTypeList.RoleValue = Convert.ToString(tempIDStageList[3]);
                                CADCAMDesignTypeList.Add(selectCADCAMDesignTypeList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillCADCAMDesignTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion
        #endregion
    }
}
