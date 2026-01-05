using DevExpress.Data;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo.DB;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Converters;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class ProductionTimelineViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services


        IGeosRepositoryService geosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      //  IERMService ERMService = new ERMServiceController("localhost:6699");
        #endregion

        #region Declaration
        ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
        private string windowHeader;
        DateTime startDate;
        DateTime endDate;
        Visibility isCalendarVisible;
        private bool isPeriod;
        private bool isBusy;
        string fromDate;
        string toDate;
        int isButtonStatus;
        private Duration _currentDuration;
        ObservableCollection<PlanningDateReviewStages> productionTimeStagesList;
        private Int32 iD;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        // private List<ERM_ProductionTimeline> productionTimeList;
        List<ProductionTimeReportLegend> productionTimeReportLegendList;


        private ObservableCollection<BandItem> bands = new ObservableCollection<BandItem>();
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        private Visibility isGridViewVisible;
        private Visibility isTimelineViewVisible;
        private string myFilterString;
        private string idstages;
        private string jobDescriptioID;
        private List<GeosAppSetting> workStages = new List<GeosAppSetting>();
        private List<ERMWorkStageWiseJobDescription> workStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
        public string ERM_ProductionTimelineReport_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_ProductionTimelineReport_Setting.Xml";
        TableView tableViewInstance;
        #endregion
        #region Properties
        bool allowPaging;
        int resultPages;
        public bool AllowPaging
        {
            get
            {
                return allowPaging;
            }

            set
            {
                allowPaging = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllowPaging"));
            }
        }
        public int ResultPages
        {
            get
            {
                return resultPages;
            }

            set
            {
                resultPages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultPages"));
            }
        }
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
        public bool IsPeriod
        {
            get { return isPeriod; }
            set { isPeriod = value; }
        }
        private bool isVisibleChanged;
        public bool IsVisibleChanged
        {
            get { return isVisibleChanged; }
            set
            {
                isVisibleChanged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleChanged"));
            }
        }
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
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public string FromDate
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
        public string ToDate
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
        public ObservableCollection<PlanningDateReviewStages> ProductionTimeStagesList
        {

            get
            {
                return productionTimeStagesList;
            }

            set
            {
                productionTimeStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeStagesList"));
            }

        }
        public Int32 ID
        {
            get { return iD; }
            set
            {
                iD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }
        List<string> stageCodes = new List<string>();

        //public List<ERM_ProductionTimeline> ProductionTimeList
        //{

        //    get
        //    {
        //        return productionTimeList;
        //    }

        //    set
        //    {
        //        productionTimeList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeList;"));
        //    }

        //}

        public List<ProductionTimeReportLegend> ProductionTimeReportLegendList
        {

            get
            {
                return productionTimeReportLegendList;
            }

            set
            {
                productionTimeReportLegendList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeReportLegendList"));
            }

        }

        //private List<ProductionTimeReportLegend> productionTimeReportLegendloggedColorList;
        //public List<ProductionTimeReportLegend> ProductionTimeReportLegendloggedColorList
        //{

        //    get
        //    {
        //        return productionTimeReportLegendloggedColorList;
        //    }

        //    set
        //    {
        //        productionTimeReportLegendloggedColorList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeReportLegendloggedColorList"));
        //    }

        //}
        //private List<ProductionTimeReportLegend> productionTimeReportLegendAttendanceColorList;
        //public List<ProductionTimeReportLegend> ProductionTimeReportLegendAttendanceColorList
        //{

        //    get
        //    {
        //        return productionTimeReportLegendAttendanceColorList;
        //    }

        //    set
        //    {
        //        productionTimeReportLegendAttendanceColorList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeReportLegendAttendanceColorList"));
        //    }

        //}

        private List<ProductionTimeReportLegend> productionTimeReportLegendloggedColorList_Cloned;
        public List<ProductionTimeReportLegend> ProductionTimeReportLegendloggedColorList_Cloned
        {

            get
            {
                return productionTimeReportLegendloggedColorList_Cloned;
            }

            set
            {
                productionTimeReportLegendloggedColorList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeReportLegendloggedColorList_Cloned"));
            }

        }
        private List<ProductionTimeReportLegend> productionTimeReportLegendAttendanceColorList_Cloned;
        public List<ProductionTimeReportLegend> ProductionTimeReportLegendAttendanceColorList_Cloned
        {

            get
            {
                return productionTimeReportLegendAttendanceColorList_Cloned;
            }

            set
            {
                productionTimeReportLegendAttendanceColorList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeReportLegendAttendanceColorList_Cloned"));
            }

        }
        //Visibility isLegendVisible;
        //public Visibility IsLegendVisible
        //{
        //    get
        //    {
        //        return isLegendVisible;
        //    }

        //    set
        //    {
        //        isLegendVisible = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsLegendVisible"));
        //    }
        //}



        private string maxHeight = "0";
        public string MaxHeight
        {
            get { return maxHeight; }
            set
            {
                maxHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxHeight"));
            }
        }
        #region [GEOS2-5238][gulab lakade][29 01 2024]
        private Visibility isAccordionControlVisible;
        public Visibility IsAccordionControlVisible
        {
            get { return isAccordionControlVisible; }
            set
            {
                isAccordionControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisible"));
            }
        }
         private int currentpageindex;
        public int CurrentPageIndex
        {
            get { return currentpageindex; }
            set
            {
                currentpageindex = value;

                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPageIndex"));
            }
            }
        private bool isPageindex;
        public bool IsPageindex
        {
            get { return isPageindex; }
            set
            {
                isPageindex = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsPageindex"));
            }
        }
        //private ProductionTimelineAccordian selectedproductionTimeline;
        private  object  selectedproductionTimeline;
        //public ProductionTimelineAccordian SelectedproductionTimeline
        public virtual object SelectedproductionTimeline
        {
            get { return selectedproductionTimeline; }
            set
            {
                selectedproductionTimeline = value;
                if (selectedproductionTimeline == null)
                {
                    if (selectedproductionTimelineforpaging != null)
                    {
                        SelectedproductionTimeline = SelectedproductionTimelineforpaging;
                        
                    }
                }
                Getlogweek();
                //int index = ERM_CalenderWeek
                //         .ToList()
                //         .FindIndex(x => x.ToString().Trim() == SelectedproductionTimeline.ToString().Trim());

                //if (index >= 0)
                //    CurrentPageIndex = index;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedproductionTimeline"));
            }
        }
        private ProductionTimelineAccordian selectedproductionTimelineforpaging;
        public ProductionTimelineAccordian SelectedproductionTimelineforpaging
        {
            get { return selectedproductionTimelineforpaging; }
            set
            {
                selectedproductionTimelineforpaging = value;

                if (selectedproductionTimelineforpaging!=null)
                {
                    SelectedproductionTimeline = SelectedproductionTimelineforpaging;
                }
                
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedproductionTimelineforpaging"));
            }
        }
        private string _pageNumberToGo;
        public string PageNumberToGo
        {
            get { return _pageNumberToGo; }
            set
            {
                _pageNumberToGo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageNumberToGo"));
            }
        }

        //Aishwarya Ingale[Geos2-5853]
        private List<ProductionTimeReportLegend> productionTimeReporManagementtLegendloggedColorList_Cloned;
        public List<ProductionTimeReportLegend> ProductionTimeReporManagementtLegendloggedColorList_Cloned
        {

            get
            {
                return productionTimeReporManagementtLegendloggedColorList_Cloned;
            }

            set
            {
                productionTimeReporManagementtLegendloggedColorList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeReporManagementtLegendloggedColorList_Cloned"));
            }

        }


        #endregion
        public Visibility IsTimelineViewVisible
        {
            get
            {
                return isTimelineViewVisible;
            }
            set
            {
                if (value == Visibility.Visible && IsGridViewVisible != Visibility.Hidden)
                {
                    IsTimelineViewVisible = Visibility.Hidden;
                }

                isTimelineViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTimelineViewVisible"));
            }
        }
        public Visibility IsGridViewVisible
        {
            get
            {
                return isGridViewVisible;
            }
            set
            {
                if (value == Visibility.Visible && IsTimelineViewVisible != Visibility.Hidden)
                {
                    IsTimelineViewVisible = Visibility.Hidden;
                }

                isGridViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridViewVisible"));
            }
        }

        private string backgroundColor;
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BackgroundColor"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummary { get; private set; }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }


        private bool isColumnChooserVisibleForGrid;
        public bool IsColumnChooserVisibleForGrid
        {
            get
            {
                return isColumnChooserVisibleForGrid;
            }

            set
            {
                isColumnChooserVisibleForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColumnChooserVisibleForGrid"));
            }
        }

        #endregion
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public string Idstages
        {
            get { return idstages; }
            set
            {
                idstages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idstages"));
            }
        }
        public string JobDescriptioID
        {
            get
            {
                return jobDescriptioID;
            }

            set
            {
                jobDescriptioID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptioID"));
            }
        }

        public List<GeosAppSetting> WorkStages
        {
            get { return workStages; }
            set
            {
                workStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStages"));
            }
        }

        public List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription
        {
            get { return workStageWiseJobDescription; }
            set
            {
                workStageWiseJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStageWiseJobDescription"));
            }
        }

        private ObservableCollection<ProductionTimelineAccordian> eRM_CalenderWeek;
        public ObservableCollection<ProductionTimelineAccordian> ERM_CalenderWeek
        {
            get { return eRM_CalenderWeek; }
            set
            {
                eRM_CalenderWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERM_CalenderWeek"));
            }
        }
        private int pageSize = 1;
        public Int32 PageSize
        {
            get { return PageSize; }
            set
            {
                PageSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageSize"));
            }
        }
       
        

        #region ICommands
        public ICommand RefreshProductionTimelineCommand { get; set; }
        public ICommand PrintProductionTimelineCommand { get; set; }
        public ICommand ExportProductionTimelineCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand FilterOptionLoadedCommand { get; set; }
        public ICommand FilterOptionEditValueChangedCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        //  public ICommand ChangePlantCommand { get; set; }
        public ICommand ChangeEmployeeCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }//[GEOS2-5238][gulab lakade][29 01 2024]
        public ICommand HideRightPanelCommand { get; set; }//[GEOS2-5238][gulab lakade][29 01 2024]

        public ICommand ShowTimeLineViewCommand { get; private set; }
        public ICommand ShowGridViewCommand { get; private set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand PaginationofIndexchangedCommand { get; set; }

        public ICommand GoToPageCommand { get; set; }
        #endregion

        #region Constructor
        public ProductionTimelineViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ProductionTimelineViewModel()...", category: Category.Info, priority: Priority.Low);
              
                RefreshProductionTimelineCommand = new RelayCommand(new Action<object>(RefreshProductionTimelineCommandAction));
                //PrintProductionTimelineCommand = new RelayCommand(new Action<object>(PrintPlantOperationalPlanningCommandAction));
                ExportProductionTimelineCommand = new RelayCommand(new Action<object>(ExportProductionTimelineCommandAction));
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                FilterOptionLoadedCommand = new RelayCommand(new Action<object>(FilterOptionLoadedCommandAction));
                FilterOptionEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(FilterOptionEditValueChangedCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                ChangePlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangePlantCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                PrintProductionTimelineCommand = new RelayCommand(new Action<object>(PrintProductionTimelineCommandAction));
                ChangeEmployeeCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeEmployeeCommandAction);
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));//[GEOS2-5238][gulab lakade][29 01 2024]
                HideRightPanelCommand = new RelayCommand(new Action<object>(HideRightPanel));//[GEOS2-5238][gulab lakade][29 01 2024]
                ShowTimeLineViewCommand = new RelayCommand(new Action<object>(ShowTimelineViewCommandAction));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowGridViewCommandAction));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                PaginationofIndexchangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PaginationofIndexchangedCommandAction);
                GoToPageCommand = new RelayCommand(new Action<object>(GoToPageCommandAction));
                GeosApplication.Instance.Logger.Log("Constructor Constructor ProductionTimelineViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ProductionTimelineViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GoToPageCommandAction(object obj)
        {
            
         if (int.TryParse(PageNumberToGo, out int pageNumber))
        {
            int newIndex = pageNumber - 1;
            if (newIndex >= 0 && newIndex<ERM_CalenderWeek.Count)
            {
                CurrentPageIndex = newIndex;
            }
            //else
            //{

            //        CustomMessageBox.Show(string.Format(Application.Current.Resources["Pagenumber"].ToString(),$"{ ERM_CalenderWeek.Count}").ToString());, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

            //        //MessageBox.Show($"Invalid page number. Please enter between 1 and {ERM_CalenderWeek.Count}");
            //}
        
            }
        }
        #endregion

        #region Method
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        public void Init_OLD()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

                if (GeosApplication.Instance.UserSettings.ContainsKey("ERMProductionTimeLine_IsFileDeleted_V2670"))  //[GEOS2-9197][pallavi jadhav][29-08-2025]
                {
                    if (GeosApplication.Instance.UserSettings["ERMProductionTimeLine_IsFileDeleted_V2670"].ToString() == "0") //[GEOS2-9197][pallavi jadhav][29-08-2025]
                    {
                        if (File.Exists(@ERM_ProductionTimelineReport_SettingFilePath))
                        {
                            File.Delete(@ERM_ProductionTimelineReport_SettingFilePath);
                            GeosApplication.Instance.UserSettings["ERMProductionTimeLine_IsFileDeleted_V2670"] = "1"; //[GEOS2-9197][pallavi jadhav][29-08-2025]
                            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                            {
                                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }
                            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        }
                    }
                }
                ERMCommon.Instance.SelectedPlant = new Site();
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList.Count() > 0)
                {
                    ERMCommon.Instance.SelectedPlant = (Site)ERMCommon.Instance.SelectedAuthorizedPlantsList.FirstOrDefault();
                }

                #region[pallavi jadhav][13 01 2025][GEOS2-6716]
                ERMCommon.Instance.DetailsList_IDLookup = new List<int>();
                ERMCommon.Instance.DetailsList_IDLookup.Add(1910);
                ERMCommon.Instance.DetailsList_IDLookup.Add(1725);
                ERMCommon.Instance.DetailsList_IDLookup.Add(1813);
                ERMCommon.Instance.DetailsList_IDLookup.Add(1724);

                #endregion
                IsPeriod = false;
                IsVisibleChanged = true;
                IsCalendarVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                MaxHeight = "0";
                ERMCommon.Instance.IsShowFailedPlantWarning = false;//[GEOS2-4839][gulab lakade][20 09 2023]
                ERMCommon.Instance.WarningFailedPlants = string.Empty;//[GEOS2-4839][gulab lakade][20 09 2023]

                //setDefaultPeriod();
                FromDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                ToDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                FillStages();
                GetIdStageAndJobDescriptionByAppSetting();
                //[GEOS2-5558][gulab lakade]
                ERMCommon.Instance.ProductionStagesList = new StageByOTItemAndIDDrawing();
                ERMCommon.Instance.ProductionStagesList = ERMService.GetAllStagesPerIDOTItemAndIDDrawing_V2500();

                //end [GEOS2-5558][gulab lakade]
                #region [GEOS2-5883][gulab lakade][27 06 2024]
                ERMCommon.Instance.WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                ERMCommon.Instance.WorkStageWiseJobDescription = WorkStageWiseJobDescription;
                #endregion
                ERMCommon.Instance.StartDate = DateTime.Today.Date;
                ERMCommon.Instance.EndDate = DateTime.Today.Date;
                // FillHtmlColor();
                //StartDate = DateTime.Today.Date;
                //EndDate = DateTime.Today.Date;
                //StartDate = new DateTime(2023, 1, 1);
                //EndDate = new DateTime(2023, 05, 31);

                #region RND STAGE

                ERMCommon.Instance.SelectedProductionTimeStagesList = new List<object>();
                ERMCommon.Instance.SelectedProductionTimeStagesList.AddRange(ProductionTimeStagesList);

                ID = 1;

                #endregion
                FillData();
                FillSelectedEmployee();//[gulab lakade][25 01 2024]
                Filllogweek();
                Getlogweek();
                //  ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                FillLegend();
                PrintAndExcelBinding();
                //                                                                                                                                                                                                                                                                                                                                                               GridBinding();
                //var DateList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date : x.AttendanceStartDate.Value.Date).OrderByDescending(a => a.Key).ToList();//[GEOS2-5418] [gulab lakade] [23 02 2024]

                //if (DateList.Count > 0)
                //{
                //    foreach (var item in DateList)
                //    {
                //        DateTime StartDate = Convert.ToDateTime(item.Key);
                //        string DayName = StartDate.ToString("dddd", new CultureInfo("en-US"));
                //        var TempHolidayRecord = ERMCommon.Instance.ProductionTimeList.Where(x => x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == StartDate.Date : x.AttendanceStartDate.Value.Date == StartDate.Date).FirstOrDefault();//[GEOS2-5418] [gulab lakade] [23 02 2024]
                //        string HolidayType = string.Empty;
                //        string Holidayname = string.Empty;
                //        Image TooltipImage = new Image();
                //        TooltipImage.Visibility = Visibility.Hidden;


                //        if (TempHolidayRecord != null)
                //        {
                //            if (!string.IsNullOrWhiteSpace(TempHolidayRecord.HolidayType))
                //            {
                //                HolidayType = Convert.ToString(TempHolidayRecord.HolidayType);
                //            }
                //            if (!string.IsNullOrWhiteSpace(TempHolidayRecord.HolidayName))
                //            {
                //                Holidayname = Convert.ToString(TempHolidayRecord.HolidayName);
                //            }

                //        }
                //        if ((DayName == "Saturday" || DayName == "Sunday") && (!string.IsNullOrWhiteSpace(Holidayname)))
                //        {
                //            BackgroundColor = "#808080";
                //        }
                //    }
                //}
                IsGridViewVisible = Visibility.Collapsed;
                IsTimelineViewVisible = Visibility.Visible;
                if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                {
                    ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                    //IsLegendVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                    IsAccordionControlVisible = Visibility.Visible;

                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }
        //public static void TempData()
        //{
        //    try
        //    {
        //        FillData();
        //    }
        //    catch(Exception ex)
        //    {

        //    }

        //}

        public void FillData_OLD()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillData ...", category: Category.Info, priority: Priority.Low);
                ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                ERMCommon.Instance.AllProductionTimeList = new List<ERM_ProductionTimeline>();

                //if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)    //[GEOS2-4839][gulab lakade][20 09 2023]
                if (ERMCommon.Instance.SelectedPlant != null)    //[GEOS2-4839][gulab lakade][20 09 2023]
                {
                    //List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    //ERMCommon.Instance.FailedPlants = new List<string>();
                    //ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    //ERMCommon.Instance.WarningFailedPlants = string.Empty;

                    Site plantOwners = ERMCommon.Instance.SelectedPlant;
                    //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    var plantOwnersIds = plantOwners.Name;
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    //foreach (var itemPlantOwnerUsers in plantOwners)
                    if (plantOwners != null)
                    {

                        //DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                        //string year = Convert.ToString(tempFromyear.Year);
                        string PlantName = Convert.ToString(plantOwners.Name);
                        int IDCompany = Convert.ToInt32(plantOwners.IdSite);
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plantOwners.Name;
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == plantOwners.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            //StartDate = new DateTime(2023, 01, 04);
                            //EndDate = new DateTime(2023, 01, 20);
                            //StartDate = ERMCommon.Instance.StartDate;
                            //EndDate = ERMCommon.Instance.EndDate;
                            string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
                            FromDate = ERMCommon.Instance.StartDate.ToString("dd/MM/yyyy");
                            ToDate = ERMCommon.Instance.EndDate.ToString("dd/MM/yyyy");
                            //StartDate = ERMCommon.Instance.StartDate;
                            //EndDate = ERMCommon.Instance.EndDate;
                            string tempFromDate = ERMCommon.Instance.StartDate.ToString(sysUIFormat);
                            string tempToDate = ERMCommon.Instance.EndDate.ToString(sysUIFormat);
                            StartDate = DateTime.ParseExact(tempFromDate, sysUIFormat, CultureInfo.CurrentCulture);
                            EndDate = DateTime.ParseExact(tempToDate, sysUIFormat, CultureInfo.CurrentCulture);
                            ERMService = new ERMServiceController(serviceurl);
                           // ERMService = new ERMServiceController("localhost:6699");

                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2450(IDCompany, StartDate, EndDate));
                            //  ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2460(IDCompany, StartDate, EndDate));

                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2470(IDCompany, StartDate, EndDate)); //[pallavi jadhav] [GEOS2-5197] [02 01 2024]

                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2480(IDCompany, StartDate, EndDate)); //[pallavi jadhav] [GEOS2-5220] [12 01 2024]
                            // ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2490(IDCompany, StartDate, EndDate)); //[pallavi jadhav] [GEOS2-5220] [12 01 2024]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2500(IDCompany, StartDate, EndDate, jobDescriptioID));  //[Aishwarya Ingale][18 03 2024][GEOS2-5424]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2510(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-5558][gulab lakade]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2520(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-5742][pallavi jadhav]
                            // ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2530(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-5742][pallavi jadhav]
                            // ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2540(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-5853][Aishwarya Ingale] 
                            // ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2550(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-6069][pallavi jadhav][21 08 2024]                         
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2580(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-6579][Aishwarya Ingale][07 11 2024]                         
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2590(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-6554][gulab lakade][28 11 2024]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2600(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-6868][gulab lakade][27 01 2025]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2610(IDCompany, StartDate, EndDate, jobDescriptioID));  //[GEOS2-6771][gulab lakade][05 04 2025]


                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2620(IDCompany, StartDate, EndDate, jobDescriptioID)); //[GEOS2-6965][rani dhamankar] [10-03-2025]
                            // ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2630(IDCompany, StartDate, EndDate, jobDescriptioID)); //[GEOS2-7642][gulab lakade][27 03 2025]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2640(IDCompany, StartDate, EndDate, jobDescriptioID)); // [GEOS2-6573][rani dhamankar] [07 - 05 - 2025]
                            //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2650(IDCompany, StartDate, EndDate, jobDescriptioID)); // [GEOS2-8189][rani dhamankar] [29 - 05 - 2025]
                            ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2660(IDCompany, StartDate, EndDate, jobDescriptioID)); // [GEOS2-8189][rani dhamankar] [29 - 05 - 2025]
                           //ERMCommon.Instance.AllProductionTimeList.AddRange(ERMService.GetProductionTimeline_V2670(IDCompany, StartDate, EndDate, jobDescriptioID)); 

                            #region [Aishwarya Ingale][18 03 2024][GEOS2-5424]

                            foreach (var productionTimeObject in ERMCommon.Instance.AllProductionTimeList.Distinct().ToList())
                            {
                                bool foundMatch = false;

                                foreach (var workStageObject in WorkStageWiseJobDescription)
                                {
                                    if (productionTimeObject.IdStage == workStageObject.IdWorkStage && workStageObject.IdJobDescription.Contains(productionTimeObject.IdJobDescription.ToString()))
                                    {


                                        foundMatch = true;
                                        break;
                                    }
                                    else
                                            if (workStageObject.IdJobDescription.Contains(productionTimeObject.IdJobDescription.ToString()))
                                    {
                                        //if (productionTimeObject.IdStage == null)
                                        //{
                                        var tempStageData = ProductionTimeStagesList.Where(x => x.IdStage == workStageObject.IdWorkStage).FirstOrDefault();
                                        if (tempStageData == null)
                                        {
                                            foundMatch = false;
                                            break;
                                        }
                                        else
                                        {
                                            if (tempStageData != null)
                                            {
                                                if (!string.IsNullOrEmpty(productionTimeObject.StageCode))
                                                {
                                                    if (productionTimeObject.StageCode == tempStageData.StageCode)
                                                    {

                                                        productionTimeObject.IdStage = tempStageData.IdStage;
                                                        if (tempStageData.StageCode != null)
                                                        {
                                                            productionTimeObject.StageCode = tempStageData.StageCode;
                                                        }
                                                        productionTimeObject.StageName = tempStageData.StageName;
                                                        productionTimeObject.Sequence = tempStageData.Sequence;
                                                    }
                                                }
                                                else if (string.IsNullOrEmpty(productionTimeObject.StageCode))
                                                {
                                                    productionTimeObject.IdStage = tempStageData.IdStage;
                                                    if (tempStageData.StageCode != null)
                                                    {
                                                        productionTimeObject.StageCode = tempStageData.StageCode;
                                                    }
                                                    productionTimeObject.StageName = tempStageData.StageName;
                                                    productionTimeObject.Sequence = tempStageData.Sequence;
                                                }
                                            }
                                            foundMatch = true;
                                            break;
                                            //}

                                        }
                                    }
                                }
                                if (!foundMatch)
                                {
                                    ERMCommon.Instance.AllProductionTimeList.Remove(productionTimeObject);
                                }
                            }

                            #endregion
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    if (stageCodes.Count > 0)
                    {
                        ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.Where(x => stageCodes.Contains(x.StageCode)).ToList();
                    }
                    else
                    {
                        ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.ToList();
                    }

                    //[GEOS2-4649][rupali sarode][10-07-2023]
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }

                    ERMCommon.Instance.ProductionTimeList_Clone = new List<ERM_ProductionTimeline>();
                    ERMCommon.Instance.ProductionTimeList_Clone.AddRange(ERMCommon.Instance.ProductionTimeList);

                    ERMCommon.Instance.EmployeeProductionTimeList = new List<ERM_ProductionTimeline>();//[gulab lakade][25 01 2024]


                    //ERMCommon.Instance.EmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b=>b.EmployeeName).ToList();//[GEOS2-5677][gulab lakade][26 04 2024]

                    #region [GEOS2-6965][rani dhamankar][24-03-2025]
                    var EmpList = (from dw in ERMCommon.Instance.ProductionTimeList
                                   group dw by dw.EmployeeCode into grouped
                                   select new
                                   {
                                       AttendanceDates = grouped.Select(x =>
                                           (x.AttendanceStartDate ?? (x.CounterpartStartDate ?? x.LeaveStartDate)))
                                   })
                                   .SelectMany(x => x.AttendanceDates)
                                  .Distinct()
                                   .Where(date => date.Value.Date >= ERMCommon.Instance.StartDate.Date && date.Value.Date <= ERMCommon.Instance.EndDate.Date) // Filter by date range
                                   .OrderBy(a => a)
                                   .ToList();


                    ERMCommon.Instance.EmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList
                        .Where(a => EmpList.Contains(a.AttendanceStartDate ?? (a.CounterpartStartDate ?? a.LeaveStartDate)))
                        .GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b => b.EmployeeName).ToList();
                    #endregion

                    if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                    {
                        ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                        //IsLegendVisible = Visibility.Visible;
                        ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                        IsAccordionControlVisible = Visibility.Visible;
                        if (IsGridViewVisible == Visibility.Visible)
                        {
                            ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                            //ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;
                        }
                    }
                    var OrderBySequenceEmployeeProductionTimeList = ERMCommon.Instance.AllProductionTimeList.OrderBy(a => a.Sequence).ToList();
                    ERMCommon.Instance.AllProductionTimeList = new List<ERM_ProductionTimeline>(OrderBySequenceEmployeeProductionTimeList);
                    //FillSelectedEmployee();//[gulab lakade][25 01 2024]
                    //Filllogweek();
                }
                GeosApplication.Instance.Logger.Log("Method FillData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillData()", category: Category.Exception, priority: Priority.Low);
            }


        }
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

                if (GeosApplication.Instance.UserSettings.ContainsKey("ERMProductionTimeLine_IsFileDeleted_V2670"))  //[GEOS2-9197][pallavi jadhav][29-08-2025]
                {
                    if (GeosApplication.Instance.UserSettings["ERMProductionTimeLine_IsFileDeleted_V2670"].ToString() == "0") //[GEOS2-9197][pallavi jadhav][29-08-2025]
                    {
                        if (File.Exists(@ERM_ProductionTimelineReport_SettingFilePath))
                        {
                            File.Delete(@ERM_ProductionTimelineReport_SettingFilePath);
                            GeosApplication.Instance.UserSettings["ERMProductionTimeLine_IsFileDeleted_V2670"] = "1"; //[GEOS2-9197][pallavi jadhav][29-08-2025]
                            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                            {
                                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }
                            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        }
                    }
                }
                ERMCommon.Instance.SelectedPlant = new Site();
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList.Count() > 0)
                {
                    ERMCommon.Instance.SelectedPlant = (Site)ERMCommon.Instance.SelectedAuthorizedPlantsList.FirstOrDefault();
                }

                #region[pallavi jadhav][13 01 2025][GEOS2-6716]
                ERMCommon.Instance.DetailsList_IDLookup = new List<int>();
                ERMCommon.Instance.DetailsList_IDLookup.Add(1910);
                ERMCommon.Instance.DetailsList_IDLookup.Add(1725);
                ERMCommon.Instance.DetailsList_IDLookup.Add(1813);
                ERMCommon.Instance.DetailsList_IDLookup.Add(1724);

                #endregion
                IsPeriod = false;
                IsVisibleChanged = true;
                IsCalendarVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                MaxHeight = "0";
                ERMCommon.Instance.IsShowFailedPlantWarning = false;//[GEOS2-4839][gulab lakade][20 09 2023]
                ERMCommon.Instance.WarningFailedPlants = string.Empty;//[GEOS2-4839][gulab lakade][20 09 2023]

                //setDefaultPeriod();
                FromDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                ToDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                FillStages();
                GetIdStageAndJobDescriptionByAppSetting();
                //[GEOS2-5558][gulab lakade]
                ERMCommon.Instance.ProductionStagesList = new StageByOTItemAndIDDrawing();
                ERMCommon.Instance.ProductionStagesList = ERMService.GetAllStagesPerIDOTItemAndIDDrawing_V2500();

                //end [GEOS2-5558][gulab lakade]
                #region [GEOS2-5883][gulab lakade][27 06 2024]
                ERMCommon.Instance.WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                ERMCommon.Instance.WorkStageWiseJobDescription = WorkStageWiseJobDescription;
                #endregion
                ERMCommon.Instance.StartDate = DateTime.Today.Date;
                ERMCommon.Instance.EndDate = DateTime.Today.Date;
                //ERMCommon.Instance.StartDate = new DateTime(2025, 05, 14);
                //ERMCommon.Instance.EndDate = new DateTime(2025, 05, 14);
                // FillHtmlColor();
                //StartDate = DateTime.Today.Date;
                //EndDate = DateTime.Today.Date;
                //StartDate = new DateTime(2023, 1, 1);
                //EndDate = new DateTime(2023, 05, 31);

                #region RND STAGE

                ERMCommon.Instance.SelectedProductionTimeStagesList = new List<object>();
                ERMCommon.Instance.SelectedProductionTimeStagesList.AddRange(ProductionTimeStagesList);

                ID = 1;

                #endregion
                FillData();
                FillLegend();
                PrintAndExcelBinding();
                if (ERMCommon.Instance.ERM_Employee_Attendance.Count() > 0 || ERMCommon.Instance.ERM_Counterpartstracking.Count() > 0 || ERMCommon.Instance.ERM_NO_OT_Time.Count() > 0 || ERMCommon.Instance.ERMCompanyHoliday.Count() > 0|| ERMCommon.Instance.ERM_OT_Working_Times.Count() > 0) //[GEOS2-9393][pallavi jadhav][12 11 2025]
                {
                    FillSelectedEmployee();//[gulab lakade][25 01 2024]
                    Filllogweek();
                    //if (ERMCommon.Instance.ProductionTimelineWeek.Count()>0)
                    //{
                    //    var ProductionTimelineWeektemp = ERMCommon.Instance.ProductionTimelineWeek.Where(a => a.logWeek != "All").ToList();
                    //    if (ProductionTimelineWeektemp.Count()>0)
                    //    {
                    //        SelectedproductionTimeline = ProductionTimelineWeektemp.FirstOrDefault().logWeek;
                    //        Getlogweek();
                    //    }
                    
                    //}

                }
                else
                {
                    ProductionTimelineAccordian ProductionTimelineWeekAll = new ProductionTimelineAccordian();
                    if (ProductionTimelineWeekAll.LogDate == null)
                        ProductionTimelineWeekAll.LogDate = new List<string>();
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;//[GEOS2-9406][gulab lakade][09 10 2025]
                                                                               // ProductionTimelineWeekAll.logWeek = "All";
                                                                               // productionTimelineWeeklist.Insert(0, ProductionTimelineWeekAll);
                    ERMCommon.Instance.ProductionTimelineWeek = new ObservableCollection<ProductionTimelineAccordian>();
                    ERMCommon.Instance.ProductionTimelineWeek.Add(ProductionTimelineWeekAll);
                   ERM_CalenderWeek = new ObservableCollection<ProductionTimelineAccordian>();
                    ERM_CalenderWeek.AddRange(ERMCommon.Instance.ProductionTimelineWeek.Where(a=>a.logWeek != "All").ToList());
                    ERMCommon.Instance.ERM_EmployeeDetailsList = new List<ERM_EmployeeDetails>();
                    ERMCommon.Instance.SelectedEmployee = new List<object>();

                }

                //  ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);


                //                                                                                                                                                                                                                                                                                                                                                               GridBinding();
                //var DateList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date : x.AttendanceStartDate.Value.Date).OrderByDescending(a => a.Key).ToList();//[GEOS2-5418] [gulab lakade] [23 02 2024]

                //if (DateList.Count > 0)
                //{
                //    foreach (var item in DateList)
                //    {
                //        DateTime StartDate = Convert.ToDateTime(item.Key);
                //        string DayName = StartDate.ToString("dddd", new CultureInfo("en-US"));
                //        var TempHolidayRecord = ERMCommon.Instance.ProductionTimeList.Where(x => x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == StartDate.Date : x.AttendanceStartDate.Value.Date == StartDate.Date).FirstOrDefault();//[GEOS2-5418] [gulab lakade] [23 02 2024]
                //        string HolidayType = string.Empty;
                //        string Holidayname = string.Empty;
                //        Image TooltipImage = new Image();
                //        TooltipImage.Visibility = Visibility.Hidden;


                //        if (TempHolidayRecord != null)
                //        {
                //            if (!string.IsNullOrWhiteSpace(TempHolidayRecord.HolidayType))
                //            {
                //                HolidayType = Convert.ToString(TempHolidayRecord.HolidayType);
                //            }
                //            if (!string.IsNullOrWhiteSpace(TempHolidayRecord.HolidayName))
                //            {
                //                Holidayname = Convert.ToString(TempHolidayRecord.HolidayName);
                //            }

                //        }
                //        if ((DayName == "Saturday" || DayName == "Sunday") && (!string.IsNullOrWhiteSpace(Holidayname)))
                //        {
                //            BackgroundColor = "#808080";
                //        }
                //    }
                //}
                IsGridViewVisible = Visibility.Collapsed;
                IsTimelineViewVisible = Visibility.Visible;

                if (ERMCommon.Instance.ERM_Employee_Attendance.Count() > 0 || ERMCommon.Instance.ERM_Counterpartstracking.Count() > 0 || ERMCommon.Instance.ERM_NO_OT_Time.Count() > 0 || ERMCommon.Instance.ERMCompanyHoliday.Count() > 0|| ERMCommon.Instance.ERM_OT_Working_Times.Count() > 0)// [GEOS2-9393][pallavi jadhav][12 11 2025]
                {
                    ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                    //IsLegendVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                    IsAccordionControlVisible = Visibility.Visible;

                }
                // if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }

        public void FillData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillData ...", category: Category.Info, priority: Priority.Low);
                ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                ERMCommon.Instance.AllProductionTimeList = new List<ERM_ProductionTimeline>();
                ERMCommon.Instance.Main_Productiontimeline = new ERM_Main_productiontimeline();
                if (ERMCommon.Instance.SelectedPlant != null)    //[GEOS2-4839][gulab lakade][20 09 2023]
                {
                    Site plantOwners = ERMCommon.Instance.SelectedPlant;
                    //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    var plantOwnersIds = plantOwners.Name;
                    ERMCommon.Instance.FailedPlants = new List<string>();
                    ERMCommon.Instance.IsShowFailedPlantWarning = false;
                    ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    //foreach (var itemPlantOwnerUsers in plantOwners)
                    if (plantOwners != null)
                    {

                        //DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                        //string year = Convert.ToString(tempFromyear.Year);
                        string PlantName = Convert.ToString(plantOwners.Name);
                        int IDCompany = Convert.ToInt32(plantOwners.IdSite);
                        int ConsumerIdSite = IDCompany;
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plantOwners.Name;
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == plantOwners.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
                            FromDate = ERMCommon.Instance.StartDate.ToString("dd/MM/yyyy");
                            ToDate = ERMCommon.Instance.EndDate.ToString("dd/MM/yyyy");
                            //StartDate = ERMCommon.Instance.StartDate;
                            //EndDate = ERMCommon.Instance.EndDate;
                            string tempFromDate = ERMCommon.Instance.StartDate.ToString(sysUIFormat);
                            string tempToDate = ERMCommon.Instance.EndDate.ToString(sysUIFormat);
                            StartDate = DateTime.ParseExact(tempFromDate, sysUIFormat, CultureInfo.CurrentCulture);
                            EndDate = DateTime.ParseExact(tempToDate, sysUIFormat, CultureInfo.CurrentCulture);
                            ERMService = new ERMServiceController(serviceurl);
                           // ERMService = new ERMServiceController("localhost:6699");

                            //ERMCommon.Instance.Main_Productiontimeline = ERMService.GetProductionTimeline_V2660_V1(IDCompany, StartDate, EndDate, jobDescriptioID);
                          //  ERMCommon.Instance.Main_Productiontimeline = ERMService.GetProductionTimeline_V2670(IDCompany, StartDate, EndDate, jobDescriptioID);
                          //  ERMCommon.Instance.Main_Productiontimeline = ERMService.GetProductionTimeline_V2680(IDCompany, StartDate, EndDate, jobDescriptioID);// [pallavi jadhav][GEOS2-9419][30 10 2025]
                            ERMCommon.Instance.Main_Productiontimeline = ERMService.GetProductionTimeline_V2690(IDCompany, StartDate, EndDate, jobDescriptioID);// [GEOS2-9393][pallavi jadhav][12 11 2025]
                            ERMCommon.Instance.ERM_EmployeeDetails = new List<ERM_EmployeeDetails>();
                            ERMCommon.Instance.ERM_EmployeeDetails = ERMCommon.Instance.Main_Productiontimeline.ERM_EmployeeDetails;
                            ERMCommon.Instance.ERM_Employee_Attendance = new List<ERM_Employee_Attendance>();
                            ERMCommon.Instance.ERM_Employee_Attendance = ERMCommon.Instance.Main_Productiontimeline.ERM_Employee_Attendance;
                            ERMCommon.Instance.ERM_Employee_MAX_Attendance = new List<ERM_Employee_Attendance>();
                            ERMCommon.Instance.ERM_Employee_MAX_Attendance = ERMCommon.Instance.Main_Productiontimeline.ERM_Employee_MAX_Attendance;
                            ERMCommon.Instance.ERMEmployeeLeave = new List<ERMEmployeeLeave>();
                            ERMCommon.Instance.ERMEmployeeLeave = ERMCommon.Instance.Main_Productiontimeline.ERMEmployeeLeave;
                            ERMCommon.Instance.ERM_Counterpartstracking = new List<Counterpartstracking>();
                            ERMCommon.Instance.ERM_Counterpartstracking = ERMCommon.Instance.Main_Productiontimeline.Counterpartstracking;
                            ERMCommon.Instance.ERM_NO_OT_Time = new List<ERM_NO_OT_Time>();
                            ERMCommon.Instance.ERM_NO_OT_Time = ERMCommon.Instance.Main_Productiontimeline.ERM_NO_OT_Time;
                            ERMCommon.Instance.ERMCompanyHoliday = new List<ERMCompanyHoliday>();
                            ERMCommon.Instance.ERMCompanyHoliday = ERMCommon.Instance.Main_Productiontimeline.ERMCompanyHoliday;
                            ERMCommon.Instance.ERM_OT_Working_Times = new List<ERM_OT_Working_Times>();
                            ERMCommon.Instance.ERM_OT_Working_Times = ERMCommon.Instance.Main_Productiontimeline.ERM_OT_Working_Times;

                            #region [GEOS2-7091][rani dhamankar][11 09 2025]
                            ERMCommon.Instance.ERM_SharedOtCounterpartstracking = new List<Counterpartstracking>();
                            ERMCommon.Instance.DesignSharedItemsEmployeeDetails = new List<DesignSharedItemsEmployeeDetails>();//[GEOS2-7091][rani dhamankar] [11-09-2025]
                            ERMCommon.Instance.DesignSharedItemsEmployeeDetails = ERMCommon.Instance.Main_Productiontimeline.DesignSharedItemsEmployeeDetails;
                            try
                            {
                                if (ERMCommon.Instance.DesignSharedItemsEmployeeDetails != null && ERMCommon.Instance.DesignSharedItemsEmployeeDetails.Count>0)
                                {
                                    var employeeDetails = ERMCommon.Instance.DesignSharedItemsEmployeeDetails;

                                    string nameOfPlantString = string.Join(",", employeeDetails
                                        .Select(x => x.IdSiteOwnersPlant.ToString())
                                        .Distinct());

                                    string[] plantArray = nameOfPlantString.Split(',');

                                    if (plantArray.Length > 0)
                                    {
                                        IList<Company> allPlantswithURL = PLMService.GetEmdepSitesCompaniesWithServiceURL_V2490();

                                        foreach (var plantStr in plantArray)
                                        {
                                            try
                                            {
                                                if (plantStr != null)
                                                {
                                                    var plantCompany = allPlantswithURL
                                                        .FirstOrDefault(a => a.IdCompany == Convert.ToInt32(plantStr));

                                                    if (plantCompany != null && !string.IsNullOrWhiteSpace(plantCompany.ServiceProviderUrl))
                                                    {
                                                        var ERMService = new ERMServiceController(plantCompany.ServiceProviderUrl);
                                                 //     ERMService = new ERMServiceController("localhost:6699");
                                                        //List<Counterpartstracking> trackingList =
                                                        //    ERMService.GetOperatorDesignSahredItemTimeTrackingDetails_V2670(
                                                        //        Convert.ToUInt32(plantStr), StartDate, EndDate);
                                                        List<Counterpartstracking> trackingList =
                                                            ERMService.GetOperatorDesignSahredItemTimeTrackingDetails_V2670(
                                                                Convert.ToUInt32(ConsumerIdSite), StartDate, EndDate, Convert.ToUInt32(plantStr));// [gulab lakade][2025 22 09][GEOS2-7091]

                                                        if (trackingList != null)
                                                        {
                                                            ERMCommon.Instance.ERM_SharedOtCounterpartstracking.AddRange(trackingList);
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception Ex)
                                            {
                                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", plantStr, " Failed ", Ex.Message), category: Category.Exception, priority: Priority.Low);
                                            }
                                        }
                                    }
                                }
                                if(ERMCommon.Instance.Main_Productiontimeline.Counterpartstracking!=null  && ERMCommon.Instance.ERM_SharedOtCounterpartstracking!=null) // [gulab lakade][2025 22 09][GEOS2-7091]
                                {
                                    ERMCommon.Instance.Main_Productiontimeline.Counterpartstracking.AddRange(ERMCommon.Instance.ERM_SharedOtCounterpartstracking);
                                }
                                
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in DesignSharedItemsEmployeeDetails List", category: Category.Exception, priority: Priority.Low);
                            }

                            #endregion

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(PlantName);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    if (stageCodes.Count > 0)
                    {
                        ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.Where(x => stageCodes.Contains(x.StageCode)).ToList();
                    }
                    else
                    {
                        ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.ToList();
                    }

                    //[GEOS2-4649][rupali sarode][10-07-2023]
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                    ERMCommon.Instance.Main_ProductionTimeList_Clone = new ERM_Main_productiontimeline();
                    ERMCommon.Instance.Main_ProductionTimeList_Clone = ERMCommon.Instance.Main_Productiontimeline;
                    ERMCommon.Instance.ProductionTimeList_Clone = new List<ERM_ProductionTimeline>();
                    ERMCommon.Instance.ERM_EmployeeDetailsList = new List<ERM_EmployeeDetails>();
                    ERMCommon.Instance.ERM_EmployeeDetailsList = ERMCommon.Instance.ERM_EmployeeDetails.GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b => b.EmployeeName).ToList();//[GEOS2-9220][gulab lakade][12 08 2025]
                    if (ERMCommon.Instance.ERM_EmployeeDetails.Count > 0)
                    {
                        ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                        //IsLegendVisible = Visibility.Visible;
                        ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                        IsAccordionControlVisible = Visibility.Visible;
                        if (IsGridViewVisible == Visibility.Visible)
                        {
                            ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                            //ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillData() executed successfully", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                    AllowPaging = Convert.ToBoolean(GeosApplication.Instance.UserSettings["AllowPaging"]);
                //[nsatpute][21-05-2025][GEOS2-7996]
                if (GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                    ResultPages = Convert.ToInt32(GeosApplication.Instance.UserSettings["ResultPages"]);
                else
                    ResultPages = 10;

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillData()", category: Category.Exception, priority: Priority.Low);
            }


        }
        private void GetIdStageAndJobDescriptionByAppSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting ...", category: Category.Info, priority: Priority.Low);

                Idstages = string.Empty;
                jobDescriptioID = string.Empty;
                WorkStages = new List<GeosAppSetting>();
                //   WorkStages = WorkbenchStartUp.GetSelectedGeosAppSettings("98");
                WorkStages = WorkbenchStartUp.GetSelectedGeosAppSettings("133");//Aishwarya ingale[Geos2-6529]
                if (workStages.Count > 0)
                {
                    List<string> tempWorkStageList = new List<string>();
                    foreach (var item in workStages)
                    {
                        string tempstring = Convert.ToString(item.DefaultValue.Replace('(', ' '));
                        tempWorkStageList = Convert.ToString(tempstring).Split(')').ToList();
                    }
                    if (tempWorkStageList.Count > 0)
                    {
                        if (WorkStageWiseJobDescription != null)
                        {
                            WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                        }
                        List<string> TempJobDescriptionID = new List<string>();
                        foreach (var item in tempWorkStageList)
                        {
                            //string tempstring = Convert.ToString(item.Remove('0',','));
                            List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
                            if (tempIDStageList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(tempIDStageList[0]) && !string.IsNullOrEmpty(tempIDStageList[1]))
                                {
                                    ERMWorkStageWiseJobDescription IDStage = new ERMWorkStageWiseJobDescription();
                                    string tempstring = Convert.ToString(tempIDStageList[0].Replace(',', ' '));
                                    IDStage.IdWorkStage = Convert.ToInt32(tempstring.Trim());
                                    //IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim());
                                    IDStage.IdJobDescription = new List<string>();
                                    //TempJobDescriptionID = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
                                    TempJobDescriptionID.AddRange(Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList());
                                    //IDStage.IdJobDescription = TempJobDescriptionID;
                                    IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
                                    WorkStageWiseJobDescription.Add(IDStage);
                                    if (string.IsNullOrEmpty(Idstages))
                                    {
                                        Idstages = Convert.ToString(tempstring.Trim());
                                    }
                                    else
                                    {
                                        Idstages = Idstages + "," + Convert.ToString(tempstring.Trim());
                                    }
                                }
                            }

                        }
                        if (TempJobDescriptionID.Count > 0)
                        {
                            var Temp = TempJobDescriptionID.Distinct().ToList();
                            if (Temp.Count > 0)
                            {

                                foreach (var Tempitem in Temp)
                                {
                                    if (string.IsNullOrEmpty(jobDescriptioID))
                                    {
                                        jobDescriptioID = Convert.ToString(Tempitem);
                                    }
                                    else
                                    {
                                        jobDescriptioID = jobDescriptioID + "," + Convert.ToString(Tempitem);
                                    }
                                }

                            }
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetIdStageAndJobDescriptionByAppSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetIdStageAndJobDescriptionByAppSetting_old()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting ...", category: Category.Info, priority: Priority.Low);

                Idstages = string.Empty;
                jobDescriptioID = string.Empty;
                WorkStages = new List<GeosAppSetting>();
                //   WorkStages = WorkbenchStartUp.GetSelectedGeosAppSettings("98");
                WorkStages = WorkbenchStartUp.GetSelectedGeosAppSettings("133");//Aishwarya ingale[Geos2-6529]
                if (workStages.Count > 0)
                {
                    List<string> tempWorkStageList = new List<string>();
                    foreach (var item in workStages)
                    {
                        string tempstring = Convert.ToString(item.DefaultValue.Replace('(', ' '));
                        tempWorkStageList = Convert.ToString(tempstring).Split(')').ToList();
                    }
                    if (tempWorkStageList.Count > 0)
                    {
                        if (WorkStageWiseJobDescription != null)
                        {
                            WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
                        }
                        List<string> TempJobDescriptionID = new List<string>();
                        foreach (var item in tempWorkStageList)
                        {
                            //string tempstring = Convert.ToString(item.Remove('0',','));
                            List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
                            if (tempIDStageList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(tempIDStageList[0]) && !string.IsNullOrEmpty(tempIDStageList[1]))
                                {
                                    ERMWorkStageWiseJobDescription IDStage = new ERMWorkStageWiseJobDescription();
                                    string tempstring = Convert.ToString(tempIDStageList[0].Replace(',', ' '));
                                    IDStage.IdWorkStage = Convert.ToInt32(tempstring.Trim());
                                    //IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim());
                                    IDStage.IdJobDescription = new List<string>();
                                    //TempJobDescriptionID = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
                                    TempJobDescriptionID.AddRange(Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList());
                                    //IDStage.IdJobDescription = TempJobDescriptionID;
                                    IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
                                    WorkStageWiseJobDescription.Add(IDStage);
                                    if (string.IsNullOrEmpty(Idstages))
                                    {
                                        Idstages = Convert.ToString(tempstring.Trim());
                                    }
                                    else
                                    {
                                        Idstages = Idstages + "," + Convert.ToString(tempstring.Trim());
                                    }
                                }
                            }

                        }
                        if (TempJobDescriptionID.Count > 0)
                        {
                            var Temp = TempJobDescriptionID.Distinct().ToList();
                            if (Temp.Count > 0)
                            {

                                foreach (var Tempitem in Temp)
                                {
                                    if (string.IsNullOrEmpty(jobDescriptioID))
                                    {
                                        jobDescriptioID = Convert.ToString(Tempitem);
                                    }
                                    else
                                    {
                                        jobDescriptioID = jobDescriptioID + "," + Convert.ToString(Tempitem);
                                    }
                                }

                            }
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetIdStageAndJobDescriptionByAppSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion




        #region Period Chanegs
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        private void setDefaultPeriod_old()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod()...", category: Category.Info, priority: Priority.Low);

                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                FromDate = StartFromDate.ToString("dd/MM/yyyy");
                ToDate = EndToDate.ToString("dd/MM/yyyy");
                //FromDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                //ToDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method setDefaultPeriod() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void PeriodCommandAction_old(object obj)
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
            IsCalendarVisible = Visibility.Collapsed;
        }
        private void PeriodCustomRangeCommandAction_old(object obj)
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

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }


        private void FlyoutControl_Closed_old(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();
                string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
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
                    FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    ToDate = thisMonthEnd.ToString("dd/MM/yyyy");

                    // DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(FromDate).Year), Convert.ToDateTime(FromDate).Month, 1);
                    StartDate = thisMonthStart;
                    EndDate = thisMonthEnd;
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                    StartDate = lastOneMonthStart;
                    EndDate = lastOneMonthEnd;
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                    StartDate = lastMonthStart;
                    EndDate = lastMonthEnd;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                    StartDate = thisWeekStart;
                    EndDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                    StartDate = lastOneWeekStart;
                    EndDate = lastOneWeekEnd;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                    StartDate = lastWeekStart;
                    EndDate = lastWeekEnd;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = ERMCommon.Instance.StartDate.ToString("dd/MM/yyyy");
                    ToDate = ERMCommon.Instance.EndDate.ToString("dd/MM/yyyy");
                    StartDate = ERMCommon.Instance.StartDate;
                    EndDate = ERMCommon.Instance.EndDate;
                    //FromDate = StartDate.ToString("dd/MM/yyyy");
                    //ToDate = EndDate.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    ToDate = EndToDate.ToString("dd/MM/yyyy");
                    StartDate = StartFromDate;
                    EndDate = EndToDate;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToString("dd/MM/yyyy");
                    ToDate = Date_T.ToString("dd/MM/yyyy");
                    StartDate = Date_F;
                    EndDate = Date_T;
                }

                IsBusy = false;
                IsPeriod = true;

                //StartDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //EndDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string[] sysUIFormat = {"dd/MM/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd",
                //   "dd-MM-yyyy", "M/d/yyyy", "dd MMM yyyy"};


                //string tempFromDate = FromDate.ToString(sysUIFormat);
                //string tempToDate = ERMCommon.Instance.EndDate.ToString(sysUIFormat);
                //StartDate = DateTime.ParseExact(FromDate, sysUIFormat, CultureInfo.CurrentCulture);
                //EndDate = DateTime.ParseExact(ToDate, sysUIFormat, CultureInfo.CurrentCulture);
                ERMCommon.Instance.StartDate = StartDate;// DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ERMCommon.Instance.EndDate = EndDate;// DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                FillData();
                FillSelectedEmployee();//[gulab lakade][25 01 2024]
                Filllogweek();
                FillLegend();
                var TempProductionTimeReportList = ERMCommon.Instance.AllProductionTimeList.Where(a => a.AttendanceTypeInUse == false || a.LeaveTypeInUse == false || a.PauseTypeInUse == false).ToList();
                if (TempProductionTimeReportList.Count > 0)
                {
                    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>(ProductionTimeReportLegendloggedColorList_Cloned);
                    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>(ProductionTimeReportLegendAttendanceColorList_Cloned);
                    ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = new List<ProductionTimeReportLegend>(ProductionTimeReporManagementtLegendloggedColorList_Cloned);//Aishwarya Ingale[Geos2-5853]

                }
                else
                {
                    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>();
                    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = ProductionTimeReportLegendloggedColorList_Cloned.Where(i => i.InUse == true).ToList();
                    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>();
                    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = ProductionTimeReportLegendAttendanceColorList_Cloned.Where(z => z.InUse == true).ToList();
                    ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = new List<ProductionTimeReportLegend>();//Aishwarya Ingale[Geos2-5853]
                    ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = ProductionTimeReporManagementtLegendloggedColorList_Cloned.Where(i => i.InUse == true).ToList();//[GEOS2-8124][rani dhamankar][16-05-2025]  //Aishwarya Ingale[Geos2-5853]
                }
                if (ERMCommon.Instance.AllProductionTimeList.Count > 0)
                {
                    //IsLegendVisible = Visibility.Visible;
                    ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                    if (IsGridViewVisible == Visibility.Visible)
                    {
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                        //ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;
                    }
                }

                MaxHeight = "500";
                //ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();


                #region [GEOS2-5857][gulab lakde] [21 06 2024]
                List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                List<Int32> CNDB_PlantId = new List<Int32>();
                if (CompaniesNotDeductBreak != null)
                {
                    string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                    if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                    {
                        CNDB_PlantId = new List<Int32>();
                        CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                    }

                }
                Site plantOwners = ERMCommon.Instance.SelectedPlant;
                var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                #endregion

                ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);

                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.ProductionTimeList);
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
        private void setDefaultPeriod()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod()...", category: Category.Info, priority: Priority.Low);

                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                FromDate = StartFromDate.ToString("dd/MM/yyyy");
                ToDate = EndToDate.ToString("dd/MM/yyyy");
                //FromDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                //ToDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                GeosApplication.Instance.Logger.Log("Method setDefaultPeriod()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method setDefaultPeriod() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

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
            IsCalendarVisible = Visibility.Collapsed;
        }
        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }
        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();
                string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
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
                    FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    ToDate = thisMonthEnd.ToString("dd/MM/yyyy");

                    // DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(FromDate).Year), Convert.ToDateTime(FromDate).Month, 1);
                    StartDate = thisMonthStart;
                    EndDate = thisMonthEnd;
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                    StartDate = lastOneMonthStart;
                    EndDate = lastOneMonthEnd;
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                    StartDate = lastMonthStart;
                    EndDate = lastMonthEnd;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                    StartDate = thisWeekStart;
                    EndDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                    StartDate = lastOneWeekStart;
                    EndDate = lastOneWeekEnd;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                    StartDate = lastWeekStart;
                    EndDate = lastWeekEnd;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = ERMCommon.Instance.StartDate.ToString("dd/MM/yyyy");
                    ToDate = ERMCommon.Instance.EndDate.ToString("dd/MM/yyyy");
                    StartDate = ERMCommon.Instance.StartDate;
                    EndDate = ERMCommon.Instance.EndDate;
                    //FromDate = StartDate.ToString("dd/MM/yyyy");
                    //ToDate = EndDate.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    ToDate = EndToDate.ToString("dd/MM/yyyy");
                    StartDate = StartFromDate;
                    EndDate = EndToDate;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToString("dd/MM/yyyy");
                    ToDate = Date_T.ToString("dd/MM/yyyy");
                    StartDate = Date_F;
                    EndDate = Date_T;
                }

                IsBusy = false;
                IsPeriod = true;

                //StartDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //EndDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string[] sysUIFormat = {"dd/MM/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd",
                //   "dd-MM-yyyy", "M/d/yyyy", "dd MMM yyyy"};


                //string tempFromDate = FromDate.ToString(sysUIFormat);
                //string tempToDate = ERMCommon.Instance.EndDate.ToString(sysUIFormat);
                //StartDate = DateTime.ParseExact(FromDate, sysUIFormat, CultureInfo.CurrentCulture);
                //EndDate = DateTime.ParseExact(ToDate, sysUIFormat, CultureInfo.CurrentCulture);
                ERMCommon.Instance.StartDate = StartDate;// DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ERMCommon.Instance.EndDate = EndDate;// DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                FillData();
                FillSelectedEmployee();//[gulab lakade][25 01 2024]
                Filllogweek();
                FillLegend();
                #region [GEOS2-9804][gulab lakade][09 10 2025] code commented
                //var TempProductionTimeReportList = ERMCommon.Instance.AllProductionTimeList.Where(a => a.AttendanceTypeInUse == false || a.LeaveTypeInUse == false || a.PauseTypeInUse == false).ToList();
                //if (TempProductionTimeReportList.Count > 0)
                //{
                //    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>(ProductionTimeReportLegendloggedColorList_Cloned);
                //    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>(ProductionTimeReportLegendAttendanceColorList_Cloned);
                //    ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = new List<ProductionTimeReportLegend>(ProductionTimeReporManagementtLegendloggedColorList_Cloned);//Aishwarya Ingale[Geos2-5853]

                //}
                //else
                //{
                //    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>();
                //    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = ProductionTimeReportLegendloggedColorList_Cloned.Where(i => i.InUse == true).ToList();
                //    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>();
                //    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = ProductionTimeReportLegendAttendanceColorList_Cloned.Where(z => z.InUse == true).ToList();
                //    ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = new List<ProductionTimeReportLegend>();//Aishwarya Ingale[Geos2-5853]
                //    ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = ProductionTimeReporManagementtLegendloggedColorList_Cloned.Where(i => i.InUse == true).ToList();//[GEOS2-8124][rani dhamankar][16-05-2025]  //Aishwarya Ingale[Geos2-5853]
                //}
                #endregion 
                //if (ERMCommon.Instance.AllProductionTimeList.Count > 0)
                if (ERMCommon.Instance.ProductionTimelineWeek.Count > 0)
                {
                    //IsLegendVisible = Visibility.Visible;
                    ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                    if (IsGridViewVisible == Visibility.Visible)
                    {
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                        //ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;
                    }
                }

                MaxHeight = "500";
                //ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();


                #region [GEOS2-5857][gulab lakde] [21 06 2024]
                List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                List<Int32> CNDB_PlantId = new List<Int32>();
                if (CompaniesNotDeductBreak != null)
                {
                    string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                    if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                    {
                        CNDB_PlantId = new List<Int32>();
                        CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                    }

                }
                Site plantOwners = ERMCommon.Instance.SelectedPlant;
                var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                #endregion
                ProductionTimelineView.AllCustomDataNew_GEOS2_9220(ERMCommon.Instance.Grid, TempPlantId, StartDate, EndDate);
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.Main_Productiontimeline, TempPlantId, StartDate, EndDate);

                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.ProductionTimeList);
                // if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion
        #endregion
        #region Stages
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        private void FillStages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStages ...", category: Category.Info, priority: Priority.Low);

                ProductionTimeStagesList = new ObservableCollection<PlanningDateReviewStages>();
                //ERMService = new ERMServiceController("localhost:6699");
                PlanningDateReviewStages ProductionStages = new PlanningDateReviewStages();
                ProductionStages.IdStage = 0;
                ProductionStages.StageCode = "Blanks";
                ProductionTimeStagesList.Add(ProductionStages);
                ProductionTimeStagesList.AddRange(ERMService.GetProductionPlanningReviewStage_V2400());
                stageCodes = new List<string>();
                stageCodes = ProductionTimeStagesList.Select(a => a.StageCode).ToList();
                #region [GEOS2-5883][gulab lakade][27 06 2024]
                ERMCommon.Instance.StageList = new ObservableCollection<PlanningDateReviewStages>();
                ERMCommon.Instance.StageList = ProductionTimeStagesList;
                #endregion
                GeosApplication.Instance.Logger.Log("Method FillStages() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStages()", category: Category.Exception, priority: Priority.Low);
            }


        }
        private void FilterOptionLoadedCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionLoadedCommandAction()...", category: Category.Info, priority: Priority.Low);

                //IsFilterLoaded = true;
                ComboBoxEdit combo = obj as ComboBoxEdit;
                //combo.SelectAllItems();
                var items = new List<object>();
                combo.SelectAllItems();

                //IsFilterLoaded = false;
                //IsFilterNotSelectedVisiblity = Visibility.Hidden;
                //IsFilterSomeVisiblity = Visibility.Hidden;
                //IsFilterAllVisiblity = Visibility.Visible;
                //   ProductionPlanningReviewList = ProductionPlanningReviewListCopy;
                GeosApplication.Instance.Logger.Log("Method FilterOptionLoadedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FilterOptionLoadedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        private void FilterOptionEditValueChangedCommandAction_old(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                //// DevExpress.Xpf.Editors.PopupCloseMode closetemp=(DevExpress.Xpf.Editors.PopupCloseMode)obj;
                ////if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                //if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                //{
                //    //return;
                //}
                //else
                //{
                if (ERMCommon.Instance.SelectedProductionTimeStagesList != null)
                {
                    ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                                                                                  //DevExpress.Xpf.Editors.ComboBoxEdit temp = (DevExpress.Xpf.Editors.ComboBoxEdit)obj;
                                                                                  //List<ProductionPlanningReview> TempProductionTimeList = new List<ProductionPlanningReview>();


                    //TempProductionTimeList = new List<ProductionPlanningReview>();
                    //ComboBoxEdit combo = obj as ComboBoxEdit;
                    //var options = combo.SelectedItems as ObservableCollection<object>;
                    int SelecteCount = ERMCommon.Instance.SelectedProductionTimeStagesList.Count();

                    if (SelecteCount == ProductionTimeStagesList.Count)
                    {
                        ID = 1;
                    }
                    else if (SelecteCount == 0)
                    {
                        ID = 2;
                    }
                    else if (SelecteCount != 0 && SelecteCount < ProductionTimeStagesList.Count)
                    {
                        ID = 3;
                    }
                    //CustomObservableCollection<UI.Helper.PlanningAppointment> TempAppointmentItems = new CustomObservableCollection<PlanningAppointment>();
                    //  List<ProductionPlanningReview> test = ProductionPlanningReviewListCopy.Where(i => i.CurrentWorkStation != null).ToList();

                    //var propertyValues = options.Select(item => item.GetType().GetProperty("StageCode")?.GetValue(item));
                    //List<string> stageCodes = propertyValues.Cast<string>().ToList();

                    //start[GEOS2-4708][gulab lakade][25 07 2023]
                    stageCodes = new List<string>();
                    //stageCodes = propertyValues.Cast<string>().ToList();
                    stageCodes = ERMCommon.Instance.SelectedProductionTimeStagesList.Cast<PlanningDateReviewStages>().Select(x => x.StageCode).ToList();//[GEOS2-5883][gulab lakade][274 06 2024]
                    if (stageCodes.Count > 0)
                    {
                        List<Int32> IdStageList = ERMCommon.Instance.SelectedProductionTimeStagesList.Cast<PlanningDateReviewStages>().Select(x => x.IdStage).ToList();//[GEOS2-5883][gulab lakade][274 06 2024]
                        var TempIDJD = ERMCommon.Instance.WorkStageWiseJobDescription.Where(a => IdStageList.Contains(a.IdWorkStage)).ToList();//[GEOS2-5883][gulab lakade][274 06 2024]
                        List<string> IdJobdescriptionINstring = new List<string>();
                        foreach (var i in TempIDJD)
                        {
                            IdJobdescriptionINstring.AddRange(i.IdJobDescription);
                        }
                        List<Int32> IdJobdescription = new List<int>();
                        IdJobdescription = IdJobdescriptionINstring.Select(int.Parse).ToList();
                        //ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.Where(x => stageCodes.Contains(x.StageCode)).ToList();
                        ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.Where(x => IdJobdescription.Contains(x.IdJobDescription)).ToList();

                    }
                    else
                        if (stageCodes.Count == 0)
                    {
                        ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                    }
                    else
                    {
                        ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.AllProductionTimeList.ToList();
                    }


                    ERMCommon.Instance.ProductionTimeList_Clone = new List<ERM_ProductionTimeline>();
                    ERMCommon.Instance.ProductionTimeList_Clone.AddRange(ERMCommon.Instance.ProductionTimeList);
                    ERMCommon.Instance.EmployeeProductionTimeList = new List<ERM_ProductionTimeline>();
                    // ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                    if (ERMCommon.Instance.ProductionTimeList.Count > 0)
                    {
                        //IsLegendVisible = Visibility.Visible;
                        ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                        ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                        ERMCommon.Instance.EmployeeProductionTimeList = new List<ERM_ProductionTimeline>();//[gulab lakade][25 01 2024]
                        //ERMCommon.Instance.EmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.EmployeeCode).Select(a => a.First()).ToList();//[GEOS2-5677][gulab lakade][26 04 2024]
                        ERMCommon.Instance.EmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b => b.EmployeeName).ToList();//[GEOS2-5677][gulab lakade][26 04 2024]
                        FillSelectedEmployee();//[gulab lakade][25 01 2024]
                                               //Filllogweek();
                    }
                    List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                    CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                    List<Int32> CNDB_PlantId = new List<Int32>();
                    if (CompaniesNotDeductBreak != null)
                    {
                        string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                        if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                        {
                            CNDB_PlantId = new List<Int32>();
                            CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                        }

                    }
                    Site plantOwners = ERMCommon.Instance.SelectedPlant;
                    var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                    ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);
                    //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]    
                    // ProductionTimelineView.AllCustomData(ERMCommon.Instance.ProductionTimeList);

                }
                else
                {

                }
                // }



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterOptionEditValueChangedCommandAction()", category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterOptionEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (ERMCommon.Instance.SelectedProductionTimeStagesList != null)
                {
                    ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                    int SelecteCount = ERMCommon.Instance.SelectedProductionTimeStagesList.Count();

                    if (SelecteCount == ProductionTimeStagesList.Count)
                    {
                        ID = 1;
                    }
                    else if (SelecteCount == 0)
                    {
                        ID = 2;
                    }
                    else if (SelecteCount != 0 && SelecteCount < ProductionTimeStagesList.Count)
                    {
                        ID = 3;
                    }

                    if (ERMCommon.Instance.ERM_Employee_Attendance.Count() > 0 || ERMCommon.Instance.ERM_Counterpartstracking.Count() > 0 || ERMCommon.Instance.ERM_NO_OT_Time.Count() > 0 || ERMCommon.Instance.ERMCompanyHoliday.Count() > 0 || ERMCommon.Instance.ERM_OT_Working_Times.Count() > 0) //[GEOS2-9393][pallavi jadhav][12 11 2025]
                    {
                        //IsLegendVisible = Visibility.Visible;
                        ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                        ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                        ERMCommon.Instance.EmployeeProductionTimeList = new List<ERM_ProductionTimeline>();//[gulab lakade][25 01 2024]
                                                                                                           //ERMCommon.Instance.EmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.EmployeeCode).Select(a => a.First()).ToList();//[GEOS2-5677][gulab lakade][26 04 2024]
                                                                                                           // ERMCommon.Instance.EmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList.GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b => b.EmployeeName).ToList();//[GEOS2-5677][gulab lakade][26 04 2024]
                        FillSelectedEmployee();//[gulab lakade][25 01 2024]
                                               //Filllogweek();
                    }
                    List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                    CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                    List<Int32> CNDB_PlantId = new List<Int32>();
                    if (CompaniesNotDeductBreak != null)
                    {
                        string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                        if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                        {
                            CNDB_PlantId = new List<Int32>();
                            CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                        }

                    }
                    Site plantOwners = ERMCommon.Instance.SelectedPlant;
                    var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                    ProductionTimelineView.AllCustomDataNew_GEOS2_9220(ERMCommon.Instance.Grid, TempPlantId, StartDate, EndDate);
                }
                else
                {

                }
                // }



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterOptionEditValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterOptionEditValueChangedCommandAction()", category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion
        #region Plant
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        private void ChangePlantCommandAction_old(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                if (ERMCommon.Instance.SelectedPlant != null)
                {
                    try
                    {
                        FillData();
                        FillSelectedEmployee();//[gulab lakade][25 01 2024]
                        Filllogweek();
                        //SelectedproductionTimeline = "All";
                        FillLegend();
                        GetIdStageAndJobDescriptionByAppSetting();
                        //  ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                        List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                        CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                        List<Int32> CNDB_PlantId = new List<Int32>();
                        if (CompaniesNotDeductBreak != null)
                        {
                            string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                            if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                            {
                                CNDB_PlantId = new List<Int32>();
                                CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                            }

                        }
                        Site plantOwners = ERMCommon.Instance.SelectedPlant;
                        var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                        var OrderBySequenceEmployeeProductionTimeList = ERMCommon.Instance.ProductionTimeList.OrderBy(a => a.Sequence).ToList();
                        ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(OrderBySequenceEmployeeProductionTimeList);

                        ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);
                        //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]

                        // ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                        //ProductionTimelineView.AllCustomData(ERMCommon.Instance.ProductionTimeList);
                        //setDefaultPeriod();
                        //FillStages();

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                    }
                }
                //if (obj == null)
                //{


                //}
                //else
                //{

                //    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                //    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                //    {

                //    }
                //    else
                //    {
                //        GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                //        var TempSelectedPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                //        List<Object> TmpSelectedPlant = new List<object>();

                //        foreach (var tmpPlant in (dynamic)TempSelectedPlant)
                //        {
                //            TmpSelectedPlant.Add(tmpPlant);
                //        }


                //        //ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                //        //ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                //        //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ProductionTimeList);
                //        //ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                //        //ERMCommon.Instance.SelectedAuthorizedPlantsList = TmpSelectedPlant;




                //        //if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                //        if (ERMCommon.Instance.SelectedPlant != null)
                //        {
                //            try
                //            {
                //                FillData();

                //                ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                //                ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                //                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.ProductionTimeList);
                //                //setDefaultPeriod();
                //                //FillStages();

                //            }
                //            catch (FaultException<ServiceException> ex)
                //            {
                //            }
                //        }
                //        else
                //        {

                //        }
                //    }
                //}


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangePlantCommandAction(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                if (ERMCommon.Instance.SelectedPlant != null)
                {
                    try
                    {
                        FillData();
                        FillSelectedEmployee();//[gulab lakade][25 01 2024]
                        Filllogweek();
                        //SelectedproductionTimeline = "All";
                        FillLegend();
                        GetIdStageAndJobDescriptionByAppSetting();
                        List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                        CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                        List<Int32> CNDB_PlantId = new List<Int32>();
                        if (CompaniesNotDeductBreak != null)
                        {
                            string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                            if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                            {
                                CNDB_PlantId = new List<Int32>();
                                CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                            }

                        }
                        Site plantOwners = ERMCommon.Instance.SelectedPlant;
                        var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                        ProductionTimelineView.AllCustomDataNew_GEOS2_9220(ERMCommon.Instance.Grid, TempPlantId, StartDate, EndDate);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                    }
                }


                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion

        #region Refresh
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        private void RefreshProductionTimelineCommandAction_old(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //FromDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                //ToDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                //ERMCommon.Instance.StartDate = DateTime.Today.Date;
                //ERMCommon.Instance.EndDate = DateTime.Today.Date;
                FillData();
                FillSelectedEmployee();//[gulab lakade][25 01 2024]
                Filllogweek();
                //SelectedproductionTimeline = "All";
                FillLegend();
                GetIdStageAndJobDescriptionByAppSetting();
                // FillLegend();
                //var TempProductionTimeReportList = ERMCommon.Instance.AllProductionTimeList.Where(a => a.AttendanceTypeInUse == false || a.LeaveTypeInUse == false || a.PauseTypeInUse == false).ToList();
                //if (TempProductionTimeReportList.Count > 0)
                //{
                //    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>(ProductionTimeReportLegendloggedColorList_Cloned);
                //    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>(ProductionTimeReportLegendAttendanceColorList_Cloned);
                //}
                //else
                //{
                //    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>();
                //    ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = ProductionTimeReportLegendloggedColorList_Cloned.Where(i => i.InUse == true).ToList();
                //    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>();
                //    ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = ProductionTimeReportLegendAttendanceColorList_Cloned.Where(z => z.InUse == true).ToList();

                //}
                //ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.ProductionTimeList);
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshProductionTimelineCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RefreshProductionTimelineCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RefreshProductionTimelineCommandAction(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillData();
                FillSelectedEmployee();//[gulab lakade][25 01 2024]
                Filllogweek();
               // SelectedproductionTimeline = "All";
                FillLegend();
                GetIdStageAndJobDescriptionByAppSetting();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshProductionTimelineCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RefreshProductionTimelineCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion

        #region Print and Export to excel

        private void PrintProductionTimelineCommandAction(object obj)
        {
            try
            {

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ProductionTimeLinePrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ProductionTimeLinePrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintProductionTimelineCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ExportProductionTimelineCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportProductionTimelineCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Production TimeLine";
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
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    activityTableView.ShowTotalSummary = true;
                    GeosApplication.Instance.Logger.Log("Method ExportProductionTimelineCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportProductionTimelineCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        #endregion


        private void FillLegend()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLegend ...", category: Category.Info, priority: Priority.Low);
                #region
                ERMCommon.Instance.WOO_ProductionTimeline = new ERM_WorkOrder_Other_ProductionTimeline();
                ERMCommon.Instance.WOO_ProductionTimeline = ERMService.GetWorkorder_Other_MonthlyProduction_V2680();
                #endregion
                // ERMService = new ERMServiceController("localhost:6699");  
                ProductionTimeReportLegendList = new List<ProductionTimeReportLegend>();
                //ProductionTimeReportLegendList.AddRange(ERMService.GetAllProductionTimeReportLegend_V2480());
                //ProductionTimeReportLegendList.AddRange(ERMService.GetAllProductionTimeReportLegend_V2490());
                ProductionTimeReportLegendList.AddRange(ERMService.GetAllProductionTimeReportLegend_V2540());//Aishwarya Ingale[Geos2-5853]
                //ProductionTimeReportLegendList.AddRange(ERMService.GetAllProductionTimeReportLegend_V2680());//rajashri GEOS2-9350 25/9/2025
                ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>();
                ProductionTimeReportLegend LegendloggedColor = new ProductionTimeReportLegend();
                LegendloggedColor.HtmlColor = "#00b54d";
                LegendloggedColor.IdLookupKey = 0;
                LegendloggedColor.IdLookupValue = 0;
                LegendloggedColor.InUse = true;
                LegendloggedColor.Name = "Working";
                ERMCommon.Instance.ProductionTimeReportWorkingColor = new List<ProductionTimeReportLegend>();
                ERMCommon.Instance.ProductionTimeReportWorkingColor.Add(LegendloggedColor);

                #region Aishwarya[Geos2-5853]

                ProductionTimeReportLegend ManagementLegendloggedColor = new ProductionTimeReportLegend();
                ManagementLegendloggedColor.HtmlColor = "#FFFFFF";
                ManagementLegendloggedColor.IdLookupKey = 0;
                ManagementLegendloggedColor.IdLookupValue = 0;
                ManagementLegendloggedColor.InUse = true;
                ManagementLegendloggedColor.Name = "Management";
                ERMCommon.Instance.ProductionTimeReportManagementColor = new List<ProductionTimeReportLegend>();
                ERMCommon.Instance.ProductionTimeReportManagementColor.Add(ManagementLegendloggedColor);

                #endregion

                var tempPuaseName = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => x.PauseTypeInUse == null || x.PauseTypeInUse == false).GroupBy(a => a.PauseType).ToList();
                if (tempPuaseName != null && tempPuaseName.Count() > 0)
                {
                    List<string> Puaselist = new List<string>();
                    foreach (var item in tempPuaseName)
                    {
                        if (item.Key != null)
                        {
                            Puaselist.Add(item.Key);
                        }

                    }
                    var legendlist = ProductionTimeReportLegendList.Where(x => Puaselist.Contains(x.Name) || x.InUse == true || x.IdLookupValue == 1863 ).ToList();
                    var legendListorderBy = legendlist.OrderBy(x => x.Position).ThenBy(p => p.Name).ToList();
                    ProductionTimeReportLegendList = new List<ProductionTimeReportLegend>();
                    ProductionTimeReportLegendList.AddRange(legendListorderBy);
                }
                else
                {
                    #region [GEOS2-6573][rani dhamankar][07-05-2025]
                    var legendListorderBy = ProductionTimeReportLegendList.Where(a => a.InUse == true || a.IdLookupValue == 1863).OrderBy(x => x.Position).ThenBy(p => p.Name).ToList();
                    #endregion
                    ProductionTimeReportLegendList = new List<ProductionTimeReportLegend>();
                    ProductionTimeReportLegendList.AddRange(legendListorderBy);
                }
                // ERMCommon.Instance.ProductionTimeReportLegendloggedColorList.Add(LegendloggedColor);
                //ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = ProductionTimeReportLegendList.Where(a => a.IdLookupKey == 109).ToList();
                ERMCommon.Instance.ProductionTimeReportLegendloggedColorList.AddRange(ProductionTimeReportLegendList.Where(a => a.IdLookupKey == 109).ToList());
                #region [pallavi jadhav] [GEOS2-5743][05-06-2024]
                var NotInUseProductionTimeReportLegendloggedColorList = ERMCommon.Instance.ProductionTimeReportLegendloggedColorList.Where(a => a.InUse == true || a.IdLookupValue == 1863 ).ToList();
                //   var NotInUseProductionTimeReportLegendloggedColorList = ERMCommon.Instance.ProductionTimeReportLegendloggedColorList.Where(a => a.InUse == true).ToList();
                ERMCommon.Instance.ProductionTimeReportLegendloggedColorList = new List<ProductionTimeReportLegend>(NotInUseProductionTimeReportLegendloggedColorList);
                #endregion

                ProductionTimeReportLegendloggedColorList_Cloned = new List<ProductionTimeReportLegend>(ERMCommon.Instance.ProductionTimeReportLegendloggedColorList);
                ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>();
                ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = ProductionTimeReportLegendList.Where(b => b.IdLookupKey == 33).ToList();
                #region [pallavi jadhav] [GEOS2-5743][05-06-2024]
                var NotInUseProductionTimeReportLegendAttendanceColorList = ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList.Where(a => a.InUse == true).ToList();
                ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList = new List<ProductionTimeReportLegend>(NotInUseProductionTimeReportLegendAttendanceColorList);
                #endregion
                ProductionTimeReportLegendAttendanceColorList_Cloned = new List<ProductionTimeReportLegend>(ERMCommon.Instance.ProductionTimeReportLegendAttendanceColorList);

                #region Aishwarya[Geos2-5853]
                ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = new List<ProductionTimeReportLegend>();

                ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = ProductionTimeReportLegendList.Where(b => b.IdLookupKey == 148).ToList();
                //rajashri GEOS2[9530] 25/9/2025
                //ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = ProductionTimeReportLegendList.Where(b => b.IdLookupKey == 148 || b.IdLookupKey == 184)
                //.OrderBy(b => b.IdLookupKey).ToList();
            
                #region [GEOS2-6573][rani dhamankar][07-05-2025]
                var NotInUseProductionTimeReportManagementLegendColorList = ERMCommon.Instance.ProductionTimeReportManagementLegendColorList.Where(a => a.InUse == true).ToList();
                #endregion
                ERMCommon.Instance.ProductionTimeReportManagementLegendColorList = new List<ProductionTimeReportLegend>(NotInUseProductionTimeReportManagementLegendColorList);
                #endregion
                ProductionTimeReporManagementtLegendloggedColorList_Cloned = new List<ProductionTimeReportLegend>(ERMCommon.Instance.ProductionTimeReportManagementLegendColorList);

                GeosApplication.Instance.Logger.Log("Method FillLegend() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLegend()", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SelectedItems(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method SelectedItems ...", category: Category.Info, priority: Priority.Low);
                ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                ERMCommon.Instance.SelectedEmployee = new List<object>();
                DevExpress.Xpf.Editors.ComboBoxEdit comboBox = (DevExpress.Xpf.Editors.ComboBoxEdit)obj;
                var TempSelectedEmployee = (((DevExpress.Xpf.Editors.ComboBoxEdit)comboBox).EditValue);
                List<Object> TmpSelectedEmps = new List<object>();

                if (TempSelectedEmployee != null)
                {
                    foreach (var tmpEmployee in (dynamic)TempSelectedEmployee)
                    {
                        TmpSelectedEmps.Add(tmpEmployee);
                    }

                    ERMCommon.Instance.SelectedEmployee = new List<object>();
                    ERMCommon.Instance.SelectedEmployee = TmpSelectedEmps;
                }

                if (ERMCommon.Instance.SelectedEmployee == null) ERMCommon.Instance.SelectedEmployee = new List<object>();

                if (ERMCommon.Instance.SelectedEmployee.Count > 0)
                {

                    List<string> EmployeeIds = ERMCommon.Instance.SelectedEmployee.Select(i => Convert.ToString((i as ERM_ProductionTimeline).EmployeeName)).Distinct().ToList();

                    List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.Where(i => EmployeeIds.Contains(Convert.ToString(i.EmployeeName))).ToList());
                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(filteredData);
                }
                else
                {

                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.ToList());
                }

                //ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                List<Int32> CNDB_PlantId = new List<Int32>();
                if (CompaniesNotDeductBreak != null)
                {
                    string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                    if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                    {
                        CNDB_PlantId = new List<Int32>();
                        CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                    }

                }
                Site plantOwners = ERMCommon.Instance.SelectedPlant;
                var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]
                // ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                GeosApplication.Instance.Logger.Log("Method SelectedItems() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception)
            {

                GeosApplication.Instance.Logger.Log("Get an error in SelectedItems()", category: Category.Exception, priority: Priority.Low);
            }
        }
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        //private void ChangeEmployeeCommandAction(object sender, EditValueChangedEventArgs e)
        private void ChangeEmployeeCommandAction_old(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ChangeEmployeeCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (obj == null)
                {


                }
                else
                {
                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                    // DevExpress.Xpf.Editors.PopupCloseMode closetemp=(DevExpress.Xpf.Editors.PopupCloseMode)obj;
                    //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                        //return;
                    }
                    else
                    {
                        ERMCommon.Instance.IsEmployeeVisible = Visibility.Collapsed;
                        ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;//[GEOS2-5238][gulab lakade][31 01 2024]
                        ERMCommon.Instance.IsRightToggleVisible = Visibility.Collapsed;
                        //IsLegendVisible = Visibility.Collapsed;


                        if (ERMCommon.Instance.SelectedEmployee == null) ERMCommon.Instance.SelectedEmployee = new List<object>();

                        if (ERMCommon.Instance.SelectedEmployee.Count > 0)
                        {

                            List<string> EmployeeIds = ERMCommon.Instance.SelectedEmployee.Select(i => Convert.ToString((i as ERM_ProductionTimeline).EmployeeName)).Distinct().ToList();
                            #region GEOS2-4045 Gulab lakade
                            if (SelectedproductionTimeline != null)
                            {
                                if (SelectedproductionTimeline.ToString().Contains("CW"))
                                {
                                    string tempselectItem = Convert.ToString(SelectedproductionTimeline);
                                    int index = tempselectItem.LastIndexOf("(");
                                    if (index > 0)
                                        tempselectItem = tempselectItem.Substring(0, index);
                                    List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.Where(i => EmployeeIds.Contains(Convert.ToString(i.EmployeeName)) && i.AttendanceWeek.ToString().Contains(tempselectItem.Trim())).ToList());
                                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(filteredData);
                                }
                                else if (SelectedproductionTimeline.ToString().Contains("All"))
                                {
                                    List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.Where(i => EmployeeIds.Contains(Convert.ToString(i.EmployeeName))).ToList());
                                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(filteredData);
                                }
                                else
                                if (!string.IsNullOrEmpty(SelectedproductionTimeline.ToString()))
                                {
                                    //int tempTimeTrackingcount = 0;
                                    DateTime TempDate = Convert.ToDateTime(SelectedproductionTimeline);
                                    //var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == TempDate.Date : x.AttendanceStartDate.Value.Date == TempDate.Date)
                                    // || x.AttendanceStartDate == null && x.CounterpartStartDate == null ? x.LeaveStartDate.HasValue && x.LeaveStartDate.Value.Date == TempDate.Date : false //[rani dhamankar] [21 -03 - 2025][GEOS2 - 6965]
                                    //).ToList();//[GEOS2-5418] [gulab lakade] [23 02 2024]
                                    #region [GEOS2-7755][gulab lakade][17 04 2025]
                                    DateTime TempEDDate = TempDate.AddDays(1);//[GEOS2-7755][gulab lakade][08 04 2025]
                                    TimeSpan tmpEDTimeCheck = new TimeSpan(11, 00, 0);
                                    TempEDDate = TempEDDate.Add(tmpEDTimeCheck);
                                    TimeSpan tmpStartTimeCheck = new TimeSpan(14, 00, 0);//[GEOS2-7755][gulab lakade][08 04 2025]

                                    //var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (x.AttendanceStartDate == null ? (x.CounterpartStartDate == null ? x.LeaveStartDate.Value.Date == TempDate.Date : x.CounterpartStartDate == TempDate.Date) : x.AttendanceStartDate.Value.Date == TempDate.Date)

                                    //).ToList();//[GEOS2-5418] [gulab lakade] [23 02 2024]
                                    var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (
                          (x.AttendanceStartDate == null && x.CounterpartStartDate.HasValue &&
                          (
                          x.IsNightShift == 1 ? (
                          x.ShiftStartTime <= tmpStartTimeCheck ? x.CounterpartStartDate.Value.Date == TempDate.Date :
                          (x.CounterpartStartDate.Value.Date <= TempEDDate && x.CounterpartStartDate.Value.Date >= TempDate.Date)

                          ) : x.CounterpartStartDate.Value.Date == TempDate.Date
                          )
                          )
                                                              || (x.AttendanceStartDate.HasValue &&
                                                              (
                                                              x.IsNightShift == 1 ? (
                                                              x.ShiftStartTime <= tmpStartTimeCheck ? x.AttendanceStartDate.Value.Date == TempDate.Date :
                                                              (x.AttendanceStartDate.Value.Date >= TempDate.Date && x.AttendanceStartDate <= TempEDDate)
                                                              )
                                                              : x.AttendanceStartDate.Value.Date == TempDate.Date
                                                              )
                                                              )
                                                              || (x.AttendanceStartDate == null && x.CounterpartStartDate == null && x.LeaveStartDate.HasValue && x.LeaveStartDate.Value.Date == TempDate.Date)
                                                               )
                                                               ).ToList();
                                    //List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.Where(i => EmployeeIds.Contains(Convert.ToString(i.EmployeeName)) && i.AttendanceStartDate.Value.Date == TempDate.Date).ToList());
                                    //List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.Where(i => EmployeeIds.Contains(Convert.ToString(i.EmployeeName)) &&
                                    //(i.AttendanceStartDate == null ? (i.CounterpartStartDate == null ? i.LeaveStartDate.Value.Date == TempDate.Date : i.CounterpartStartDate.Value.Date == TempDate.Date) : i.AttendanceStartDate.Value.Date == TempDate.Date)

                                    //).ToList());

                                    List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(

                                        ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (
                          (x.AttendanceStartDate == null && x.CounterpartStartDate.HasValue &&
                          (
                          x.IsNightShift == 1 ? (
                          x.ShiftStartTime <= tmpStartTimeCheck ? x.CounterpartStartDate.Value.Date == TempDate.Date :
                          (x.CounterpartStartDate.Value.Date <= TempEDDate && x.CounterpartStartDate.Value.Date >= TempDate.Date)

                          ) : x.CounterpartStartDate.Value.Date == TempDate.Date
                          )
                          )
                                                              || (x.AttendanceStartDate.HasValue &&
                                                              (
                                                              x.IsNightShift == 1 ? (
                                                              x.ShiftStartTime <= tmpStartTimeCheck ? x.AttendanceStartDate.Value.Date == TempDate.Date :
                                                              (x.AttendanceStartDate.Value.Date >= TempDate.Date && x.AttendanceStartDate <= TempEDDate)
                                                              )
                                                              : x.AttendanceStartDate.Value.Date == TempDate.Date
                                                              )
                                                              )
                                                              || (x.AttendanceStartDate == null && x.CounterpartStartDate == null && x.LeaveStartDate.HasValue && x.LeaveStartDate.Value.Date == TempDate.Date)
                                                               )
                                                               ).ToList()

                                     );
                                    #endregion
                                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(filteredData);

                                }
                            }
                            else
                            {
                                List<ERM_ProductionTimeline> filteredData = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.Where(i => EmployeeIds.Contains(Convert.ToString(i.EmployeeName))).ToList());
                                ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(filteredData);
                            }
                            #endregion


                        }
                        else if (ERMCommon.Instance.SelectedEmployee.Count == 0)
                        {

                            ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                        }
                        else
                        {
                            ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>(ERMCommon.Instance.ProductionTimeList_Clone.ToList());
                        }

                        // ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                        if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                        {
                            //IsLegendVisible = Visibility.Visible;
                            ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                            ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                            ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                            ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                        }
                        List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                        CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                        List<Int32> CNDB_PlantId = new List<Int32>();
                        if (CompaniesNotDeductBreak != null)
                        {
                            string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                            if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                            {
                                CNDB_PlantId = new List<Int32>();
                                CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                            }

                        }
                        Site plantOwners = ERMCommon.Instance.SelectedPlant;
                        var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                        ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);
                        //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]
                        //  ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                        GeosApplication.Instance.Logger.Log("Method ChangeEmployeeCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in ChangeEmployeeCommandAction()", category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeEmployeeCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ChangeEmployeeCommandAction ...", category: Category.Info, priority: Priority.Low);
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
                        List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                        CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                        List<Int32> CNDB_PlantId = new List<Int32>();
                        if (CompaniesNotDeductBreak != null)
                        {
                            string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                            if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                            {
                                CNDB_PlantId = new List<Int32>();
                                CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                            }
                        }
                        Site plantOwners = ERMCommon.Instance.SelectedPlant;
                        var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                        ProductionTimelineView.AllCustomDataNew_GEOS2_9220(ERMCommon.Instance.Grid, TempPlantId, StartDate, EndDate);
                        GeosApplication.Instance.Logger.Log("Method ChangeEmployeeCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in ChangeEmployeeCommandAction()", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillSelectedEmployee_old()
        {
            try
            {
                if (ERMCommon.Instance.EmployeeProductionTimeList != null)
                {
                    if (ERMCommon.Instance.EmployeeProductionTimeList.Count > 0)
                    {
                        //var TempEmployee = ERMCommon.Instance.EmployeeProductionTimeList.GroupBy(x => x.EmployeeName).ToList();
                        //if(TempEmployee.Count>0)
                        //{
                        ERMCommon.Instance.SelectedEmployee = new List<object>(ERMCommon.Instance.EmployeeProductionTimeList.OrderBy(x => x.EmployeeName).ToList());
                        //    foreach (var item in TempEmployee)
                        //    {
                        //        ERMCommon.Instance.SelectedEmployee.Add(item.Key);
                        //    }

                        //    //ERMCommon.Instance.SelectedEmployee.AddRange(TempEmployee.ToList());
                        //}

                    }
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSelectedEmployee()", category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillSelectedEmployee()
        {
            try
            {
                if (ERMCommon.Instance.SelectedProductionTimeStagesList != null && ProductionTimeStagesList.Count != ERMCommon.Instance.SelectedProductionTimeStagesList.Count() && ERMCommon.Instance.SelectedProductionTimeStagesList.Count() != 0)
                {
                    stageCodes = new List<string>();
                    stageCodes = ERMCommon.Instance.SelectedProductionTimeStagesList.Cast<PlanningDateReviewStages>().Select(x => x.StageCode).ToList();//[GEOS2-5883][gulab lakade][274 06 2024]
                    if (stageCodes.Count > 0)
                    {
                        List<Int32> IdStageList = ERMCommon.Instance.SelectedProductionTimeStagesList.Cast<PlanningDateReviewStages>().Select(x => x.IdStage).ToList();//[GEOS2-5883][gulab lakade][274 06 2024]
                        var TempIDJD = ERMCommon.Instance.WorkStageWiseJobDescription.Where(a => IdStageList.Contains(a.IdWorkStage)).ToList();//[GEOS2-5883][gulab lakade][274 06 2024]
                        List<string> IdJobdescriptionINstring = new List<string>();
                        foreach (var i in TempIDJD)
                        {
                            IdJobdescriptionINstring.AddRange(i.IdJobDescription);
                        }
                        List<Int32> IdJobdescription = new List<int>();
                        IdJobdescription = IdJobdescriptionINstring.Select(int.Parse).ToList();
                        if (ERMCommon.Instance.ERM_EmployeeDetails.Count() > 0)
                        {
                            ERMCommon.Instance.ERM_EmployeeDetailsList = new List<ERM_EmployeeDetails>();
                            ERMCommon.Instance.ERM_EmployeeDetailsList = ERMCommon.Instance.ERM_EmployeeDetails.Where(a => IdJobdescription.Contains(a.IdJobDescription)).GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b => b.EmployeeName).ToList();
                            if (ERMCommon.Instance.ERM_EmployeeDetailsList != null)
                            {
                                if (ERMCommon.Instance.ERM_EmployeeDetailsList.Count > 0)
                                {
                                    ERMCommon.Instance.SelectedEmployee = new List<object>(ERMCommon.Instance.ERM_EmployeeDetailsList.OrderBy(x => x.EmployeeName).ToList());
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (ERMCommon.Instance.ERM_EmployeeDetails.Count() > 0)
                    {
                        ERMCommon.Instance.ERM_EmployeeDetailsList = new List<ERM_EmployeeDetails>();
                        ERMCommon.Instance.ERM_EmployeeDetailsList = ERMCommon.Instance.ERM_EmployeeDetails.GroupBy(x => x.EmployeeCode).Select(a => a.FirstOrDefault()).OrderBy(b => b.EmployeeName).ToList();//[GEOS2-9220][gulab lakade][12 08 2025]
                        if (ERMCommon.Instance.ERM_EmployeeDetailsList != null)
                        {
                            if (ERMCommon.Instance.ERM_EmployeeDetailsList.Count > 0)
                            {

                                ERMCommon.Instance.SelectedEmployee = new List<object>(ERMCommon.Instance.ERM_EmployeeDetailsList.OrderBy(x => x.EmployeeName).ToList());

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSelectedEmployee()", category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-5238] [gulab lakade][29 01 2024]
        private void Filllogweek_old()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Filllogweek()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                List<ERM_ProductionTimeline> AttendanceWeekGroup = new List<ERM_ProductionTimeline>();
                var temp = ERMCommon.Instance.ProductionTimeList_Clone.Where(p => stageCodes.Contains(p.StageCode)).GroupBy(x => x.AttendanceWeek)
                    .Select(group => new
                    {
                        AttendanceWeek = ERMCommon.Instance.ProductionTimeList_Clone.Where(p => stageCodes.Contains(p.StageCode)).FirstOrDefault(a => a.AttendanceWeek == group.Key).AttendanceWeek,

                        Count = ERMCommon.Instance.ProductionTimeList_Clone.Where(b => b.AttendanceWeek == null && stageCodes.Contains(b.StageCode)).Count(),
                    }).ToList().OrderBy(i => i.AttendanceWeek);     ////GEOS2-4045 Gulab lakade Order by CW ASC

                ERMCommon.Instance.ProductionTimelineWeek = new ObservableCollection<ProductionTimelineAccordian>();

                foreach (var item in temp)
                {

                    //AttendanceWeekGroup = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => x.AttendanceWeek == item.AttendanceWeek && stageCodes.Contains(x.StageCode)).ToList();
                    AttendanceWeekGroup = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => x.AttendanceWeek == item.AttendanceWeek && stageCodes.Contains(x.StageCode)).ToList();//[GEOS2-7642][gulab lakade][27 03 2025]
                    ProductionTimelineAccordian productionTimelineWeek = new ProductionTimelineAccordian();
                    var currentculter = CultureInfo.CurrentCulture;
                    //List<ERM_ProductionTimeline> ActiveAndInActiveEmplyeeData = new List<ERM_ProductionTimeline>();
                    //ActiveAndInActiveEmplyeeData = AttendanceWeekGroup.Where(a =>a.JobDescriptionEndDate != null && a.JobDescriptionEndDate.Value.Date >= StartDate && a.JobDescriptionEndDate.Value.Date <= EndDate).ToList();
                    #region [GEOS2-7641][gulab lakade][27 03 2025]
                    //var tempproductionAttendanceDate = (from dw in AttendanceWeekGroup
                    //                                    select new
                    //                                    {
                    //                                        //CopyAttendanceDate = dw.AttendanceStartDate,
                    //                                        //CopyAttendanceDate = (dw.AttendanceStartDate == null ? dw.CounterpartStartDate : dw.AttendanceStartDate),//[GEOS2-5418] [gulab lakade] [23 02 2024]
                    //                                        CopyAttendanceDate = (dw.AttendanceStartDate == null ? (dw.CounterpartStartDate ?? dw.LeaveStartDate) : dw.AttendanceStartDate),//[GEOS2-6965][rani dhamankar][12-03-2025]
                    //                                        //AttendanceDate = dw.AttendanceStartDate
                    //                                        //AttendanceDate = (dw.AttendanceStartDate == null ? dw.CounterpartStartDate : dw.AttendanceStartDate)//[GEOS2-5418] [gulab lakade] [23 02 2024]
                    //                                        AttendanceDate = (dw.AttendanceStartDate == null? (dw.CounterpartStartDate ?? dw.LeaveStartDate) : dw.AttendanceStartDate)//[GEOS2-6965][rani dhamankar][12-03-2025]

                    //                                    }
                    //                          ).Distinct().OrderBy(a => a.AttendanceDate).ToList();
                    TimeSpan NightTimeMin = new TimeSpan(15, 00, 0);
                    TimeSpan NightTimeMax = new TimeSpan(23, 59, 0);
                    TimeSpan tmpStartTimeCheck = new TimeSpan(14, 00, 0);//[GEOS2-7755][gulab lakade][08 04 2025]
                    var tempproductionAttendanceDate = (from dw in AttendanceWeekGroup
                                                        select new
                                                        {
                                                            //CopyAttendanceDate = dw.AttendanceStartDate,
                                                            //CopyAttendanceDate = (dw.AttendanceStartDate == null ? dw.CounterpartStartDate : dw.AttendanceStartDate),//[GEOS2-5418] [gulab lakade] [23 02 2024]
                                                            //CopyAttendanceDate = (dw.AttendanceStartDate == null ? (dw.CounterpartStartDate ?? dw.LeaveStartDate) :
                                                            //(dw.IsNightShift == 1 ? (
                                                            //(dw.AttendanceStartDate <= dw.AttendanceStartDate.Value.Date.Add(NightTimeMax) && dw.AttendanceStartDate >= dw.AttendanceStartDate.Value.Date.Add(NightTimeMin))

                                                            //?
                                                            //dw.AttendanceStartDate : dw.AttendanceStartDate.Value.Date.AddDays(-1)

                                                            //) : dw.AttendanceStartDate)
                                                            //),//[GEOS2-6965][rani dhamankar][12-03-2025]
                                                            // CopyAttendanceDate = (dw.AttendanceStartDate == null ? (
                                                            //dw.CounterpartStartDate != null ?
                                                            // ((dw.CounterpartStartDate <= dw.CounterpartStartDate.Value.Date.Add(NightTimeMax) && dw.CounterpartStartDate >= dw.CounterpartStartDate.Value.Date.Add(NightTimeMin))
                                                            //     ?
                                                            //      dw.CounterpartStartDate
                                                            //     : (dw.ShiftStartTime <= tmpStartTimeCheck ? dw.CounterpartStartDate : dw.CounterpartStartDate.Value.Date.AddDays(-1))
                                                            //     )
                                                            // : dw.LeaveStartDate) :
                                                            // (dw.IsNightShift == 1 ? (
                                                            // (dw.AttendanceStartDate <= dw.AttendanceStartDate.Value.Date.Add(NightTimeMax) && dw.AttendanceStartDate >= dw.AttendanceStartDate.Value.Date.Add(NightTimeMin))

                                                            // ?
                                                            // dw.AttendanceStartDate : (dw.ShiftStartTime <= tmpStartTimeCheck ? dw.AttendanceStartDate : dw.AttendanceStartDate.Value.Date.AddDays(-1))

                                                            // ) : dw.AttendanceStartDate)
                                                            // ),//[GEOS2-6965][rani dhamankar][12-03-2025]

                                                            CopyAttendanceDate = (dw.AttendanceStartDate == null ? (
                                                           dw.CounterpartStartDate != null ?
                                                            (
                            dw.IsNightShift == 1 ? (
                            dw.ShiftStartTime <= tmpStartTimeCheck ? dw.CounterpartStartDate :
                            dw.CounterpartStartDate.Value.Date.AddDays(-1)
                            ) : (dw.CounterpartStartDate >= dw.CounterpartStartDate.Value.Date.Add(dw.ShiftStartTime) && dw.CounterpartStartDate <= dw.CounterpartStartDate.Value.Date.Add(dw.ShiftEndTime)? dw.CounterpartStartDate:null)
                            
                                                                )
                                                            : dw.LeaveStartDate) :
                                                            (dw.IsNightShift == 1 ? (
                                                            (dw.AttendanceStartDate <= dw.AttendanceStartDate.Value.Date.Add(NightTimeMax) && dw.AttendanceStartDate >= dw.AttendanceStartDate.Value.Date.Add(NightTimeMin))

                                                            ?
                                                            dw.AttendanceStartDate : (dw.ShiftStartTime <= tmpStartTimeCheck ? dw.AttendanceStartDate : dw.AttendanceStartDate.Value.Date.AddDays(-1))

                                                            ) : dw.AttendanceStartDate)
                                                            ),
                                                            //AttendanceDate = dw.AttendanceStartDate
                                                            //AttendanceDate = (dw.AttendanceStartDate == null ? dw.CounterpartStartDate : dw.AttendanceStartDate)//[GEOS2-5418] [gulab lakade] [23 02 2024]
                                                            //AttendanceDate = (dw.AttendanceStartDate == null ? (dw.CounterpartStartDate ?? dw.LeaveStartDate) :
                                                            //(dw.IsNightShift == 1 ? (
                                                            //(dw.AttendanceStartDate <= dw.AttendanceStartDate.Value.Date.Add(NightTimeMax) && dw.AttendanceStartDate >= dw.AttendanceStartDate.Value.Date.Add(NightTimeMin))

                                                            //?
                                                            //dw.AttendanceStartDate : dw.AttendanceStartDate.Value.Date.AddDays(-1)

                                                            //) : dw.AttendanceStartDate)

                                                            //)//[GEOS2-6965][rani dhamankar][12-03-2025]

                                                            AttendanceDate = (dw.AttendanceStartDate == null ? (
                                                           dw.CounterpartStartDate != null ?
                                                            (
                            dw.IsNightShift == 1 ? (
                            dw.ShiftStartTime <= tmpStartTimeCheck ? dw.CounterpartStartDate :
                            dw.CounterpartStartDate.Value.Date.AddDays(-1)
                            ) : (dw.CounterpartStartDate >= dw.CounterpartStartDate.Value.Date.Add(dw.ShiftStartTime) && dw.CounterpartStartDate <= dw.CounterpartStartDate.Value.Date.Add(dw.ShiftEndTime) ? dw.CounterpartStartDate : null)

                                                                )
                                                            : dw.LeaveStartDate) :
                                                            (dw.IsNightShift == 1 ? (
                                                            (dw.AttendanceStartDate <= dw.AttendanceStartDate.Value.Date.Add(NightTimeMax) && dw.AttendanceStartDate >= dw.AttendanceStartDate.Value.Date.Add(NightTimeMin))

                                                            ?
                                                            dw.AttendanceStartDate : (dw.ShiftStartTime <= tmpStartTimeCheck ? dw.AttendanceStartDate : dw.AttendanceStartDate.Value.Date.AddDays(-1))

                                                            ) : dw.AttendanceStartDate)
                                                            )//[GEOS2-7755][gulab lakade][08 04 2025]
                                                        }
                                              ).Distinct().OrderBy(a => a.AttendanceDate).ToList();

                    #endregion 
                    //List<DateTime?> tempDate = tempproductionAttendanceDate.Select(a => a.AttendanceDate).Distinct().ToList();
                    List<DateTime?> tempDate = tempproductionAttendanceDate.Where(x=>x.AttendanceDate!=null).Select(a => a.AttendanceDate).Distinct().ToList();



                    if (productionTimelineWeek.LogDate == null)
                        productionTimelineWeek.LogDate = new List<string>();
                    List<DateTime?> tempDateorderBy = tempDate.Where(a => a.Value.Date >= ERMCommon.Instance.StartDate && a.Value.Date <= ERMCommon.Instance.EndDate).OrderBy(a => a.Value).ToList(); //[rani dhamankar] [18-03 -2025][GEOS2 - 7229]  //// Gulab lakade Order by CW ASC 04-05-2023
                    if (tempDateorderBy.Count > 0)//[rani dhamankar] [18-03 -2025][GEOS2 - 7229]
                    {
                        foreach (DateTime item1 in tempDateorderBy)
                        {
                            string TempDate = item1.ToString("d", currentculter);
                            if (!productionTimelineWeek.LogDate.Contains(TempDate))
                            {

                                productionTimelineWeek.LogDate.Add(TempDate);

                            }
                        }

                        productionTimelineWeek.logWeek = item.AttendanceWeek + " (" + productionTimelineWeek.LogDate.Count + ")"; ;
                        productionTimelineWeek.copyLogWeek = item.AttendanceWeek;
                        ERMCommon.Instance.ProductionTimelineWeek.Add(productionTimelineWeek);

                    }


                }
                if (ERMCommon.Instance.ProductionTimelineWeek.Count() == 0)
                {
                    ERMCommon.Instance.EmployeeProductionTimeList = new List<ERM_ProductionTimeline>();
                    ERMCommon.Instance.SelectedEmployee = new List<object>();
                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();
                }
                ProductionTimelineAccordian ProductionTimelineWeekAll = new ProductionTimelineAccordian();
                if (ProductionTimelineWeekAll.LogDate == null)
                    ProductionTimelineWeekAll.LogDate = new List<string>();
               // ProductionTimelineWeekAll.logWeek = "All";
               // ERMCommon.Instance.ProductionTimelineWeek.Insert(0, ProductionTimelineWeekAll);
                ProductionTimelineAccordian productionTimelineWeek1 = new ProductionTimelineAccordian();
                //productionTimelineWeek1.LogDate = null;
                //productionTimelineWeek1.logWeek = "All";
                //productionTimelineWeek1.copyLogWeek = null;
                //SelectedproductionTimeline = productionTimelineWeek1;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Filllogweek()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Filllogweek() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Filllogweek() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method Filllogweek() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        private void Filllogweek()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Filllogweek()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.SplashScreenMessage = "Week and Date is loading ..";
                List<ERM_ProductionTimeline> AttendanceWeekGroup = new List<ERM_ProductionTimeline>();
                ERMCommon.Instance.ProductionTimelineWeek = new ObservableCollection<ProductionTimelineAccordian>();
                List<ProductionTimelineAccordian> productionTimelineWeeklist = new List<ProductionTimelineAccordian>();
                #region Attendance
                if (ERMCommon.Instance.ERM_Employee_Attendance.Count > 0)
                {
                    var AttWeek = ERMCommon.Instance.ERM_Employee_Attendance.GroupBy(x => x.CalenderWeek).ToList().OrderBy(i => i.Key);     ////GEOS2-4045 Gulab lakade Order by CW ASC
                    productionTimelineWeeklist.AddRange(AttWeek.Select(item => new ProductionTimelineAccordian
                    {
                        logWeek = item.Key,
                        copyLogWeek = item.Key
                    }).ToList());
                }
                #endregion
                #region Leave
                if (ERMCommon.Instance.ERMEmployeeLeave.Count > 0)
                {
                    foreach (var item in ERMCommon.Instance.ERMEmployeeLeave.GroupBy(x => x.CalenderWeek).ToList().OrderBy(i => i.Key))
                    {
                        if (productionTimelineWeeklist.Count > 0)
                        {
                            if (productionTimelineWeeklist.Where(a => a.logWeek == item.Key).ToList().Count() == 0)
                            {
                                productionTimelineWeeklist.Add(
                                                                new ProductionTimelineAccordian
                                                                {
                                                                    logWeek = item.Key,
                                                                    copyLogWeek = item.Key
                                                                });
                            }
                        }
                        else
                        {
                            productionTimelineWeeklist.Add(
                                new ProductionTimelineAccordian
                                {
                                    logWeek = item.Key,
                                    copyLogWeek = item.Key
                                });
                        }
                    }
                }
                #endregion
                #region ERM_Counterpartstracking
                if (ERMCommon.Instance.ERM_Counterpartstracking.Count > 0)
                {
                    foreach (var item in ERMCommon.Instance.ERM_Counterpartstracking.GroupBy(x => x.CalenderWeek).ToList().OrderBy(i => i.Key))
                    {
                        if (productionTimelineWeeklist.Count > 0)
                        {
                            if (productionTimelineWeeklist.Where(a => a.logWeek == item.Key).ToList().Count() == 0)
                            {
                                productionTimelineWeeklist.Add(
                                                                new ProductionTimelineAccordian
                                                                {
                                                                    logWeek = item.Key,
                                                                    copyLogWeek = item.Key
                                                                });
                            }
                        }
                        else
                        {
                            productionTimelineWeeklist.Add(
                                new ProductionTimelineAccordian
                                {
                                    logWeek = item.Key,
                                    copyLogWeek = item.Key
                                });
                        }
                    }
                }
                #endregion
                #region Leave
                if (ERMCommon.Instance.ERM_NO_OT_Time.Count > 0)
                {
                    foreach (var item in ERMCommon.Instance.ERM_NO_OT_Time.GroupBy(x => x.CalenderWeek).ToList().OrderBy(i => i.Key))
                    {
                        if (productionTimelineWeeklist.Count > 0)
                        {
                            if (productionTimelineWeeklist.Where(a => a.logWeek == item.Key).ToList().Count() == 0)
                            {
                                productionTimelineWeeklist.Add(
                                                                new ProductionTimelineAccordian
                                                                {
                                                                    logWeek = item.Key,
                                                                    copyLogWeek = item.Key
                                                                });
                            }
                        }
                        else
                        {
                            productionTimelineWeeklist.Add(
                                new ProductionTimelineAccordian
                                {
                                    logWeek = item.Key,
                                    copyLogWeek = item.Key
                                });
                        }
                    }
                }
                #endregion
                #region ERMCompanyHoliday
                if (ERMCommon.Instance.ERMCompanyHoliday.Count > 0)
                {
                    foreach (var item in ERMCommon.Instance.ERMCompanyHoliday.GroupBy(x => x.CalenderWeek).ToList().OrderBy(i => i.Key))
                    {
                        if (productionTimelineWeeklist.Count > 0)
                        {
                            if (productionTimelineWeeklist.Where(a => a.logWeek == item.Key).ToList().Count() == 0)
                            {
                                productionTimelineWeeklist.Add(
                                                                new ProductionTimelineAccordian
                                                                {
                                                                    logWeek = item.Key,
                                                                    copyLogWeek = item.Key
                                                                });
                            }
                        }
                        else
                        {
                            productionTimelineWeeklist.Add(
                                new ProductionTimelineAccordian
                                {
                                    logWeek = item.Key,
                                    copyLogWeek = item.Key
                                });
                        }
                    }
                }
                #endregion

                #region
                #region OT_Working_time [GEOS2-9393][pallavi jadhav][12 11 2025]
                if (ERMCommon.Instance.ERM_OT_Working_Times.Count > 0)
                {
                    foreach (var item in ERMCommon.Instance.ERM_OT_Working_Times.GroupBy(x => x.CalenderWeek).ToList().OrderBy(i => i.Key))
                    {
                        if (productionTimelineWeeklist.Count > 0)
                        {
                            if (productionTimelineWeeklist.Where(a => a.logWeek == item.Key).ToList().Count() == 0)
                            {
                                productionTimelineWeeklist.Add(
                                                                new ProductionTimelineAccordian
                                                                {
                                                                    logWeek = item.Key,
                                                                    copyLogWeek = item.Key
                                                                });
                            }
                        }
                        else
                        {
                            productionTimelineWeeklist.Add(
                                new ProductionTimelineAccordian
                                {
                                    logWeek = item.Key,
                                    copyLogWeek = item.Key
                                });
                        }
                    }
                }
                #endregion
                #endregion
                var currentculter = CultureInfo.CurrentCulture;
                if (productionTimelineWeeklist.Count() > 0)
                {
                    foreach (var item in productionTimelineWeeklist.OrderBy(a => a.logWeek).ToList())
                    {
                        item.LogDate = new List<string>();

                        item.LogDate.AddRange(ERMCommon.Instance.ERM_Employee_Attendance.Where(a => a.CalenderWeek == item.logWeek && a.AttendanceStartDate.Value.Date >= ERMCommon.Instance.StartDate.Date && a.AttendanceStartDate.Value.Date <= ERMCommon.Instance.EndDate.Date).GroupBy(c => c.AttendanceStartDate.Value.Date).Select(b => b.Key.ToString("d", currentculter)).ToList());

                        #region Leave
                        if (ERMCommon.Instance.ERMEmployeeLeave.Count > 0)
                        {
                            foreach (var leave in ERMCommon.Instance.ERMEmployeeLeave.Where(a => a.CalenderWeek == item.logWeek && a.StartDate.Value.Date >= ERMCommon.Instance.StartDate.Date && a.StartDate.Value.Date <= ERMCommon.Instance.EndDate.Date).GroupBy(x => x.StartDate.Value.Date).ToList().OrderBy(i => i.Key))
                            {
                                if (!item.LogDate.Contains(leave.Key.Date.ToString("d", currentculter)))
                                {
                                    item.LogDate.Add(leave.Key.Date.ToString("d", currentculter));
                                }

                            }
                        }
                        #endregion
                        #region ERM_Counterpartstracking
                        if (ERMCommon.Instance.ERM_Counterpartstracking.Count > 0)
                        {
                            foreach (var track in ERMCommon.Instance.ERM_Counterpartstracking.Where(a => a.CalenderWeek == item.logWeek && a.StartDate.Value.Date >= ERMCommon.Instance.StartDate.Date && a.StartDate.Value.Date <= ERMCommon.Instance.EndDate.Date).GroupBy(x => x.StartDate.Value.Date).ToList().OrderBy(i => i.Key))
                            {
                                if (!item.LogDate.Contains(track.Key.Date.ToString("d", currentculter)))
                                {
                                    item.LogDate.Add(track.Key.Date.ToString("d", currentculter));
                                }

                            }
                        }
                        #endregion
                        #region Instance.ERM_NO_OT_Time
                        if (ERMCommon.Instance.ERM_NO_OT_Time.Count > 0)
                        {
                            foreach (var no_ot in ERMCommon.Instance.ERM_NO_OT_Time.Where(a => a.CalenderWeek == item.logWeek && a.BreakStartDate.Value.Date >= ERMCommon.Instance.StartDate.Date && a.BreakStartDate.Value.Date <= ERMCommon.Instance.EndDate.Date).GroupBy(x => x.BreakStartDate.Value.Date).ToList().OrderBy(i => i.Key))
                            {
                                if (!item.LogDate.Contains(no_ot.Key.Date.ToString("d", currentculter)))
                                {
                                    item.LogDate.Add(no_ot.Key.Date.ToString("d", currentculter));
                                }

                            }
                        }
                        #endregion
                        #region ERMCompanyHoliday
                        if (ERMCommon.Instance.ERMCompanyHoliday.Count > 0)
                        {
                            foreach (var no_ot in ERMCommon.Instance.ERMCompanyHoliday.Where(a => a.CalenderWeek == item.logWeek && a.StartDate.Value.Date >= ERMCommon.Instance.StartDate.Date && a.StartDate.Value.Date <= ERMCommon.Instance.EndDate.Date).GroupBy(x => x.StartDate.Value.Date).ToList().OrderBy(i => i.Key))
                            {
                                if (!item.LogDate.Contains(no_ot.Key.Date.ToString("d", currentculter)))
                                {
                                    item.LogDate.Add(no_ot.Key.Date.ToString("d", currentculter));
                                }

                            }
                        }
                        #endregion
                        #region OT_Working_Time [GEOS2-9393][pallavi jadhav][12 11 2025]
                        if (ERMCommon.Instance.ERM_OT_Working_Times.Count > 0)
                        {
                            foreach (var track in ERMCommon.Instance.ERM_OT_Working_Times.Where(a => a.CalenderWeek == item.logWeek && a.StartDate.Value.Date >= ERMCommon.Instance.StartDate.Date && a.StartDate.Value.Date <= ERMCommon.Instance.EndDate.Date).GroupBy(x => x.StartDate.Value.Date).ToList().OrderBy(i => i.Key))
                            {
                                if (!item.LogDate.Contains(track.Key.Date.ToString("d", currentculter)))
                                {
                                    item.LogDate.Add(track.Key.Date.ToString("d", currentculter));
                                }

                            }
                        }
                        #endregion
                        if (item.LogDate.Count > 0)
                        {

                            List<DateTime> tmpOderbydate = item.LogDate.Select(a => Convert.ToDateTime(a)).OrderBy(a => a).ToList();
                            item.LogDate = new List<string>();
                            foreach (var logdate in tmpOderbydate)
                            {
                                string TempDate = logdate.ToString("d", currentculter);
                                if (!item.LogDate.Contains(TempDate))
                                {

                                    item.LogDate.Add(TempDate);

                                }
                            }
                            item.logWeek = item.logWeek + " (" + item.LogDate.Count + ")";
                        }
                        else
                        {
                            item.logWeek = item.logWeek + " (0)";
                        }
                    }
                }
                ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-9406][gulab lakade][09 10 2025]
                if (productionTimelineWeeklist.Count() == 0)
                {
                    ERMCommon.Instance.ERM_EmployeeDetailsList = new List<ERM_EmployeeDetails>();
                    ERMCommon.Instance.SelectedEmployee = new List<object>();
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;//[GEOS2-9406][gulab lakade][09 10 2025]
                }
                

                ProductionTimelineAccordian ProductionTimelineWeekAll = new ProductionTimelineAccordian();
                if (ProductionTimelineWeekAll.LogDate == null)
                    ProductionTimelineWeekAll.LogDate = new List<string>();
               // ProductionTimelineWeekAll.logWeek = "All";
                //productionTimelineWeeklist.Insert(0, ProductionTimelineWeekAll);
                ERMCommon.Instance.ProductionTimelineWeek.AddRange(productionTimelineWeeklist);
                ERM_CalenderWeek = new ObservableCollection<ProductionTimelineAccordian>();
                ERM_CalenderWeek.AddRange(productionTimelineWeeklist.ToList());
                SelectedproductionTimeline = ERMCommon.Instance.ProductionTimelineWeek.FirstOrDefault();
                //SelectedproductionTimelineforpaging = ERMCommon.Instance.ProductionTimelineWeek.FirstOrDefault().logWeek;
                //if (GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                //    AllowPaging = Convert.ToBoolean(GeosApplication.Instance.UserSettings["AllowPaging"]);
                ////[nsatpute][21-05-2025][GEOS2-7996]
                //if (GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                //    ResultPages = Convert.ToInt32(GeosApplication.Instance.UserSettings["ResultPages"]);
                //else
                //    ResultPages = ERM_CalenderWeek.Count();
                //if (ERMCommon.Instance.ProductionTimelineWeek.Count() > 0)
                //{
                //    var ProductionTimelineWeektemp = ERMCommon.Instance.ProductionTimelineWeek.Where(a => a.logWeek != "All").ToList();
                //    if (ProductionTimelineWeektemp.Count() > 0)
                //    {
                //        SelectedproductionTimeline = ProductionTimelineWeektemp.FirstOrDefault().logWeek;
                //        Getlogweek();
                //    }

                //}
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Filllogweek()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Filllogweek() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Filllogweek() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method Filllogweek() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        private void Getlogweek_old()
        {
            GeosApplication.Instance.Logger.Log("Method Getlogweek ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                {
                    ERMCommon.Instance.ProductionTimeList = new List<ERM_ProductionTimeline>();

                    #region GEOS2-4045 Gulab lakade
                    List<string> EmployeeIds = ERMCommon.Instance.SelectedEmployee.Select(i => Convert.ToString((i as ERM_ProductionTimeline).EmployeeName)).Distinct().ToList();
                    if (SelectedproductionTimeline != null)
                    {

                        if (SelectedproductionTimeline.ToString().Contains("CW"))
                        {
                            string tempselectItem = Convert.ToString(SelectedproductionTimeline);
                            int index = tempselectItem.LastIndexOf("(");
                            if (index > 0)
                                tempselectItem = tempselectItem.Substring(0, index);

                            ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && x.AttendanceWeek.ToString().Contains(tempselectItem.Trim())).ToList();
                        }
                        else if (SelectedproductionTimeline.ToString().Contains("All"))
                        {
                            ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName)).ToList();
                        }
                        else
                        if (!string.IsNullOrEmpty(SelectedproductionTimeline.ToString()))
                        {
                            //int tempTimeTrackingcount = 0;
                            DateTime TempDate = Convert.ToDateTime(SelectedproductionTimeline);
                            DateTime TempEDDate = TempDate.AddDays(1);//[GEOS2-7755][gulab lakade][08 04 2025]
                            TimeSpan tmpEDTimeCheck = new TimeSpan(11, 00, 0);
                            TempEDDate = TempEDDate.Add(tmpEDTimeCheck);
                            TimeSpan tmpStartTimeCheck = new TimeSpan(14, 00, 0);//[GEOS2-7755][gulab lakade][08 04 2025]
                            TempDate = TempDate.Add(tmpStartTimeCheck);
                            //var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && x.AttendanceStartDate.Value.Date == TempDate.Date).ToList();
                            //var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && x.CounterpartStartDate.Value.Date == TempDate.Date).ToList();
                            //var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (
                            //x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == TempDate.Date : x.AttendanceStartDate.Value.Date == TempDate.Date)).ToList();//[GEOS2-5418] [gulab lakade] [23 02 2024]
                            #region  [GEOS2-7641][gulab lakade][27 03 2025] 
                            // var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (
                            // (x.AttendanceStartDate == null && x.CounterpartStartDate.HasValue && x.CounterpartStartDate.Value.Date == TempDate.Date)
                            //                                     || (x.AttendanceStartDate.HasValue && x.AttendanceStartDate.Value.Date == TempDate.Date)
                            //                                     || (x.AttendanceStartDate == null && x.CounterpartStartDate == null && x.LeaveStartDate.HasValue && x.LeaveStartDate.Value.Date == TempDate.Date)
                            //                                      )//[rani dhamankar][18-03-2025][GEOS2-7229]

                            //// x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == TempDate.Date : x.AttendanceStartDate.Value.Date == TempDate.Date)


                            //).ToList();
                            // var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (
                            // (x.AttendanceStartDate == null && x.CounterpartStartDate.HasValue && (x.IsNightShift == 1 ? (x.CounterpartStartDate.Value.Date == TempDate.Date.AddDays(1) || x.CounterpartStartDate.Value.Date == TempDate.Date) : x.CounterpartStartDate.Value.Date == TempDate.Date))
                            //                                     || (x.AttendanceStartDate.HasValue && (x.IsNightShift == 1 ? (x.AttendanceStartDate.Value.Date == TempDate.Date || x.AttendanceStartDate.Value.Date == TempDate.Date.AddDays(1)) : x.AttendanceStartDate.Value.Date == TempDate.Date))
                            //                                     || (x.AttendanceStartDate == null && x.CounterpartStartDate == null && x.LeaveStartDate.HasValue && x.LeaveStartDate.Value.Date == TempDate.Date)
                            //                                      )//[rani dhamankar][18-03-2025][GEOS2-7229]

                            //// x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == TempDate.Date : x.AttendanceStartDate.Value.Date == TempDate.Date)


                            //).ToList();
                            #region [GEOS2-7755][gulab lakade][08 04 2025]
                            var TempproductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName) && (
                            (x.AttendanceStartDate == null && x.CounterpartStartDate.HasValue &&
                            (
                            x.IsNightShift == 1 ? (
                            x.ShiftStartTime <= tmpStartTimeCheck ? x.CounterpartStartDate.Value.Date == TempDate.Date :
                            (x.CounterpartStartDate <= TempEDDate && x.CounterpartStartDate >= TempDate)

                            ) : (x.CounterpartStartDate >= TempDate.Date.Add(x.ShiftStartTime) && x.CounterpartStartDate <= TempDate.Date.Add(x.ShiftEndTime))
                            )
                            )
                                                                || (x.AttendanceStartDate.HasValue &&
                                                                (
                                                                x.IsNightShift == 1 ? (
                                                                x.ShiftStartTime <= tmpStartTimeCheck ? x.AttendanceStartDate.Value.Date == TempDate.Date :
                                                                (x.AttendanceStartDate >= TempDate && x.AttendanceStartDate <= TempEDDate)
                                                                )
                                                                : x.AttendanceStartDate.Value.Date == TempDate.Date
                                                                )
                                                                )
                                                                || (x.AttendanceStartDate == null && x.CounterpartStartDate == null && x.LeaveStartDate.HasValue && x.LeaveStartDate.Value.Date == TempDate.Date)
                                                                 )//[rani dhamankar][18-03-2025][GEOS2-7229]

                           // x.AttendanceStartDate == null ? x.CounterpartStartDate.Value.Date == TempDate.Date : x.AttendanceStartDate.Value.Date == TempDate.Date)


                           ).ToList();
                            #endregion
                            #endregion


                            ERMCommon.Instance.ProductionTimeList.AddRange(TempproductionTimeList);

                        }
                        #endregion

                    }
                    else
                    {
                        //if (ERMCommon.Instance.ProductionTimelineWeek.Count() > 0)
                        //{
                        if (EmployeeIds.Count > 0)
                        {
                            ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.Where(x => EmployeeIds.Contains(x.EmployeeName)).ToList();
                        }
                        else
                        {
                            ERMCommon.Instance.ProductionTimeList = ERMCommon.Instance.ProductionTimeList_Clone.ToList();
                        }

                        //}

                    }
                    //ProductionTimelineView ProductionTimelineView = new ProductionTimelineView();
                    if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                    {
                        //IsLegendVisible = Visibility.Visible;
                        if (IsGridViewVisible == Visibility.Visible)
                        {
                            ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                        }
                        else
                        {
                            ERMCommon.Instance.IsEmployeeVisible = Visibility.Visible;
                            ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;//[GEOS2-5238][gulab lakade][31 01 2024]
                            ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                        }
                    }

                    List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                    CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                    List<Int32> CNDB_PlantId = new List<Int32>();
                    if (CompaniesNotDeductBreak != null)
                    {
                        string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                        if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                        {
                            CNDB_PlantId = new List<Int32>();
                            CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                        }

                    }
                    Site plantOwners = ERMCommon.Instance.SelectedPlant;
                    var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();
                    ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);
                    //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]
                    //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method Getlogweek() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetRecordbyDeliveryDate()", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        private void Getlogweek()
        {
            GeosApplication.Instance.Logger.Log("Method Getlogweek ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (SelectedproductionTimeline != null)
                {

                    if (SelectedproductionTimeline.ToString().Contains("CW"))
                    {
                        ERMCommon.Instance.SelectedproductionTimeline = SelectedproductionTimeline.ToString();
                    }
                    //else if (SelectedproductionTimeline.ToString().Contains("All"))
                    //{
                    //    ERMCommon.Instance.SelectedproductionTimeline = "All";
                    //}
                    else
                    if (!string.IsNullOrEmpty(SelectedproductionTimeline.ToString()))
                    {
                        ERMCommon.Instance.SelectedproductionTimeline = SelectedproductionTimeline.ToString();
                    }


                }
                else
                {
                    if (ERMCommon.Instance.ProductionTimelineWeek.Count > 0)
                    {
                        ERMCommon.Instance.SelectedproductionTimeline = ERMCommon.Instance.ProductionTimelineWeek.FirstOrDefault().logWeek;
                        SelectedproductionTimeline = ERMCommon.Instance.SelectedproductionTimeline;//[GEOS2-9406][gulab lakade][09 10 2025]
                    }
                   // SelectedproductionTimeline = ERMCommon.Instance.SelectedproductionTimeline;
                }

                List<GeosAppSetting> CompaniesNotDeductBreak = new List<GeosAppSetting>();
                CompaniesNotDeductBreak = WorkbenchStartUp.GetSelectedGeosAppSettings("120");
                List<Int32> CNDB_PlantId = new List<Int32>();
                if (CompaniesNotDeductBreak != null)
                {
                    string TempCNDB_PlantId = CompaniesNotDeductBreak.Select(s => s.DefaultValue).FirstOrDefault();
                    if (!string.IsNullOrEmpty(TempCNDB_PlantId))
                    {
                        CNDB_PlantId = new List<Int32>();
                        CNDB_PlantId = TempCNDB_PlantId.Split(';').Select(int.Parse).ToList();
                    }

                }

                Site plantOwners = ERMCommon.Instance.SelectedPlant;
                var TempPlantId = CNDB_PlantId.Where(a => a == plantOwners.IdSite).ToList();

                ProductionTimelineView.AllCustomDataNew_GEOS2_9220(ERMCommon.Instance.Grid, TempPlantId, StartDate, EndDate);
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, StartDate, EndDate);
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList, TempPlantId, WorkStageWiseJobDescription, ProductionTimeStagesList);//[GEOS2-5883][gulab lakade][27 06 2024]
                //ProductionTimelineView.AllCustomData(ERMCommon.Instance.Grid, ERMCommon.Instance.ProductionTimeList);
                #region [GEOS2-9406][gulab lakade][09 10 2025]
                try
                {

                    int index = 0;
                    if (SelectedproductionTimeline != null)
                    {
                        IsPageindex = true;
                        
                        if (!SelectedproductionTimeline.ToString().Contains("CW"))
                        {
                            index = ERM_CalenderWeek
                 .ToList()
                 .FindIndex(week => week.LogDate != null &&
                                    week.LogDate.Contains(ERMCommon.Instance.SelectedproductionTimeline));
                        }
                        else
                        {
                            index = ERM_CalenderWeek
                     .ToList()
                     .FindIndex(x => x.ToString().Trim() == ERMCommon.Instance.SelectedproductionTimeline.ToString().Trim());
                        }

                        if (index >= 0)
                            CurrentPageIndex = index;
                        PageNumberToGo = (CurrentPageIndex + 1).ToString();

                        IsPageindex = false;
                    }
                    else
                    {
                        CurrentPageIndex = 0;
                        PageNumberToGo = (CurrentPageIndex + 1).ToString();

                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in GetRecordbyDeliveryDate() Paggination ", category: Category.Exception, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                #endregion 
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Getlogweek() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetRecordbyDeliveryDate()", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        #endregion
        #region [GEOS2-5238][gulab lakade][29 01 2024]
        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (IsAccordionControlVisible == Visibility.Collapsed)
                    IsAccordionControlVisible = Visibility.Visible;
                else
                    IsAccordionControlVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void HideRightPanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (ERMCommon.Instance.IsLegendVisible == Visibility.Collapsed)
                    ERMCommon.Instance.IsLegendVisible = Visibility.Visible;
                else
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        private void ShowTimelineViewCommandAction_old(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowTimelineViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                {
                    IsTimelineViewVisible = Visibility.Visible;

                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    IsGridViewVisible = Visibility.Hidden;
                    IsAccordionControlVisible = Visibility.Visible;
                }
                else
                {
                    IsTimelineViewVisible = Visibility.Visible;

                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    IsGridViewVisible = Visibility.Hidden;
                    IsAccordionControlVisible = Visibility.Hidden;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowTimelineViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowTimelineViewCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowTimelineViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowTimelineViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (ERMCommon.Instance.ERM_Employee_Attendance.Count() > 0 || ERMCommon.Instance.ERM_Counterpartstracking.Count() > 0 || ERMCommon.Instance.ERM_NO_OT_Time.Count() > 0 || ERMCommon.Instance.ERMCompanyHoliday.Count() > 0 || ERMCommon.Instance.ERM_OT_Working_Times.Count() > 0)//[GEOS2-9393][pallavi jadhav][12 11 2025]
                {
                    IsTimelineViewVisible = Visibility.Visible;

                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    IsGridViewVisible = Visibility.Hidden;
                    IsAccordionControlVisible = Visibility.Visible;
                    
                }
                else
                {
                    IsTimelineViewVisible = Visibility.Visible;

                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                    //ERMCommon.Instance.IsRightToggleVisible = Visibility.Visible;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    IsGridViewVisible = Visibility.Hidden;
                    IsAccordionControlVisible = Visibility.Hidden;
                    //start[GEOS2-9406][gulab lakade][09 10 2025]
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;//[GEOS2-9406][gulab lakade][09 10 2025]
                    //end [GEOS2-9406][gulab lakade][09 10 2025]
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowTimelineViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowTimelineViewCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowGridViewCommandAction_old(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowGridViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (ERMCommon.Instance.ProductionTimeList_Clone.Count > 0)
                {
                    IsTimelineViewVisible = Visibility.Hidden;

                    IsGridViewVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                    IsAccordionControlVisible = Visibility.Visible;
                }
                else
                {
                    IsTimelineViewVisible = Visibility.Hidden;

                    IsGridViewVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;
                    IsAccordionControlVisible = Visibility.Collapsed;

                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowGridViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowGridViewCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowGridViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowGridViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (ERMCommon.Instance.ERM_Employee_Attendance.Count() > 0 || ERMCommon.Instance.ERM_Counterpartstracking.Count() > 0 || ERMCommon.Instance.ERM_NO_OT_Time.Count() > 0 || ERMCommon.Instance.ERMCompanyHoliday.Count() > 0 || ERMCommon.Instance.ERM_OT_Working_Times.Count() > 0)// [GEOS2-9393][pallavi jadhav] [12 11 2025]
                {
                    IsTimelineViewVisible = Visibility.Hidden;

                    IsGridViewVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Visible;
                    IsAccordionControlVisible = Visibility.Visible;
                }
                else
                {
                    IsTimelineViewVisible = Visibility.Hidden;

                    IsGridViewVisible = Visibility.Visible;
                    ERMCommon.Instance.IsRightToggleVisible = Visibility.Hidden;
                    ERMCommon.Instance.IsLegendVisible = Visibility.Collapsed;
                    ERMCommon.Instance.IsLeftToggleVisible = Visibility.Collapsed;
                    IsAccordionControlVisible = Visibility.Collapsed;
                    //start[GEOS2-9406][gulab lakade][09 10 2025]
                      ERMCommon.Instance.IsLeftToggleVisible = Visibility.Hidden;//[GEOS2-9406][gulab lakade][09 10 2025]
                    //end [GEOS2-9406][gulab lakade][09 10 2025]

                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowGridViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowGridViewCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region column chooser
        #region [GEOS2-9220][gulab lakade][12 08 2025][performance task]
        private void TableViewLoadedCommandAction_old(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_ProductionTimelineReport_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_ProductionTimelineReport_SettingFilePath);
                    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)gridControlInstance.View;
                    this.tableViewInstance = tableView;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_ProductionTimelineReport_SettingFilePath);

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
                        visibleFalseColumnsCount++;
                    }
                }

                if (visibleFalseColumnsCount > 0)
                {
                    IsColumnChooserVisibleForGrid = true;
                }
                else
                {
                    IsColumnChooserVisibleForGrid = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;

                //TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                // datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                // if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_ProductionTimelineReport_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_ProductionTimelineReport_SettingFilePath);
                    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)gridControlInstance.View;
                    this.tableViewInstance = tableView;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_ProductionTimelineReport_SettingFilePath);

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
                        visibleFalseColumnsCount++;
                    }
                }

                if (visibleFalseColumnsCount > 0)
                {
                    IsColumnChooserVisibleForGrid = true;
                }
                else
                {
                    IsColumnChooserVisibleForGrid = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;

                //TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                // datailView.BestFitColumns();
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        //    private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        //    {
        //        try
        //        {
        //            GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
        //            {
        //                //if (IsBandOpen)
        //                //{
        //                int visibleFalseColumn = 0;
        //                //GridControl gridControl = obj as GridControl;
        //                //TableView tableView = (TableView)gridControl.View;


        //                if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
        //                {
        //                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
        //                    GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
        //                    TableView tableView = (TableView)gridControl.View;
        //                    this.tableViewInstance = tableView;

        //                    if (tableView.SearchString != null)
        //                    {
        //                        tableView.SearchString = null;
        //                    }

        //                    //gridControl.BeginInit();



        //                    //if (iSModuleVisible)
        //                    //{
        //                    if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
        //                    {
        //                        gridControl.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
        //                    }
        //                    //}
        //                    //if (iSStructureVisible)
        //                    //{
        //                    //    if (File.Exists(StructureBandSettingFilePath))
        //                    //    {
        //                    //        gridControl.RestoreLayoutFromXml(StructureBandSettingFilePath);
        //                    //    }
        //                    //}

        //                    //This code for save grid layout.
        //                    //if (iSModuleVisible)
        //                    gridControl.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);
        //                    //else if (iSStructureVisible)
        //                    //    gridControl.SaveLayoutToXml(StructureBandSettingFilePath);

        //                    foreach (GridColumn column in gridControl.Columns)
        //                    {
        //                        DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
        //                        if (descriptor != null)
        //                        {
        //                            descriptor.AddValueChanged(column, VisibleChanged);
        //                        }

        //                        DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
        //                        if (descriptorColumnPosition != null)
        //                        {
        //                            descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
        //                        }

        //                        if (!column.Visible)
        //                        {
        //                            //if ((column.FieldName != "IdTemplate" && column.FieldName != "IdCPType" && column.FieldName != "HtmlColor"))
        //                            //{
        //                            visibleFalseColumn++;
        //                            ((Emdep.Geos.UI.Helper.ColumnItem)column.DataContext).IsVertical = false;
        //                            //}
        //                        }
        //                    }

        //                    if (visibleFalseColumn > 0)
        //                    {
        //                        IsColumnChooserVisibleForGrid = true;
        //                    }
        //                    else
        //                    {
        //                        IsColumnChooserVisibleForGrid = false;
        //                    }
        //                    TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
        //                    datailView.ShowTotalSummary = true;
        ////                    gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
        ////new GridSummaryItem() {
        ////    SummaryType = SummaryItemType.Sum,
        ////    FieldName = "Real",
        ////    DisplayFormat= "{0}",
        ////    Visible=true,
        ////   //FixedTotalSummaryElementStyle=TextAlignment.Right
        ////    //Alignment=GridSummaryItemAlignment.Right,

        ////},
        ////new GridSummaryItem() {
        ////    SummaryType = SummaryItemType.Sum,
        ////    FieldName = "Remaining",
        ////    DisplayFormat="{0}",
        ////    Visible=true


        ////}});
        //                    //gridControl.EndInit();
        //                    //tableView.SearchString = null;
        //                    //tableView.ShowGroupPanel = false;
        //                }
        //                //}

        //                //IsBusy = false;
        //            }
        //            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //            GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
        //        }
        //        catch (Exception ex)
        //        {
        //            GeosApplication.Instance.Logger.Log("Error on GridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        }
        //    }
        //private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //        int visibleFalseColumnsCount = 0;

        //        if (File.Exists(ERM_TimeTrackinggrid_SettingFilePath))
        //        {
        //            ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_TimeTrackinggrid_SettingFilePath);
        //            GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

        //            TableView tableView = (TableView)gridControlInstance.View;
        //            this.tableViewInstance = tableView;

        //            if (tableView.SearchString != null)
        //            {
        //                tableView.SearchString = null;
        //            }
        //        }

        //        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

        //        //This code for save grid layout.
        //        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

        //        GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
        //        foreach (GridColumn column in gridControl.Columns)
        //        {
        //            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
        //            if (descriptor != null)
        //            {
        //                descriptor.AddValueChanged(column, VisibleChanged);
        //            }

        //            DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
        //            if (descriptorColumnPosition != null)
        //            {
        //                descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
        //            }

        //            if (column.Visible == false)
        //            {
        //                visibleFalseColumnsCount++;
        //            }
        //        }

        //        if (visibleFalseColumnsCount > 0)
        //        {
        //            IsColumnChooserVisibleForGrid = true;
        //        }
        //        else
        //        {
        //            IsColumnChooserVisibleForGrid = false;
        //        }
        //        TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
        //        datailView.ShowTotalSummary = true;


        //        //            gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
        //        //new GridSummaryItem() {
        //        //    SummaryType = SummaryItemType.Sum,
        //        //    FieldName = "UITempobservedTime",
        //        //    DisplayFormat= "{0}",
        //        //    Visible=true,
        //        //   //FixedTotalSummaryElementStyle=TextAlignment.Right
        //        //    //Alignment=GridSummaryItemAlignment.Right,

        //        //},
        //        //new GridSummaryItem() {
        //        //    SummaryType = SummaryItemType.Sum,
        //        //    FieldName = "UITempNormalTime",
        //        //    DisplayFormat="{0}",
        //        //    Visible=true


        //        //}});
        //        //TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
        //        // datailView.BestFitColumns();

        //        GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ERM_ProductionTimelineReport_SettingFilePath);
                    //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                }

                if (column.Visible == false)
                {
                    IsColumnChooserVisibleForGrid = true;
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
                ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ERM_ProductionTimelineReport_SettingFilePath);
                //((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_TimeTrackinggrid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
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
            catch (Exception ex)
            {

            }
        }
        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);


                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(ERM_ProductionTimelineReport_SettingFilePath);


                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion
        #endregion
        private void PrintAndExcelBinding()
        {
            try
            {
                // Dictionary<Int32, string> DictonarySites = new Dictionary<Int32, string>();


                //  ERMCommon.Instance.MeetingsList.AddRange(ERMCommon.Instance.WOO_ProductionTimeline.ERM_MeetingType.ToList().Select(a => a.LookupValue));
                #region [pallavi jadhav][GEOS2-9419][2025 10 30]
                if (ERMCommon.Instance.WOO_ProductionTimeline.ERM_MeetingType!=null&& ERMCommon.Instance.WOO_ProductionTimeline.ERM_MeetingType.Count()>0)
                {
                    ERMCommon.Instance.MeetingsList = ERMCommon.Instance.WOO_ProductionTimeline.ERM_MeetingType.ToDictionary(a => a.IdLookupValue, a => a.LookupValue);
                }
                if (ERMCommon.Instance.WOO_ProductionTimeline.ERM_MaintenanceType!=null&& ERMCommon.Instance.WOO_ProductionTimeline.ERM_MaintenanceType.Count()>0)
                {
                    ERMCommon.Instance.MaintenanceList = ERMCommon.Instance.WOO_ProductionTimeline.ERM_MaintenanceType.ToDictionary(a => a.IdLookupValue, a => a.LookupValue);

                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method PrintAndExcelBinding ...", category: Category.Info, priority: Priority.Low);
                #region RND gulab 16 02 2024

                //ERMCommon.Instance.ProductionBands = new ObservableCollection<BandItem>(); ERMCommon.Instance.ProductionBands.Clear();
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                BandItem band0 = new BandItem() { BandName = "all", Visible = false, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band0.Columns = new ObservableCollection<ColumnItem>();
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant", HeaderText = "Plant", Width = 70, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Week", HeaderText = "Week", Width = 100, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Date", HeaderText = "Date", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.DateTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Employee", HeaderText = "Employee", Width = 250, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Stage", HeaderText = "Stage", Width = 60, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_Shift", HeaderText = "Shift Time", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_Logged", HeaderText = "Logged Time", Width = 100, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });

                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_Attendance", HeaderText = "Attendance Time", Width = 120, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "WOTime", HeaderText = "OT Registered Time", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "ManagementValue_", HeaderText = "", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.ManagementBreakTemplate, Visible = false });


                ERMCommon.Instance.DataTableForProductionTimeLine = new DataTable();

                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Plant", typeof(string));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Week", typeof(string));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Date", typeof(DateTime));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Employee", typeof(string));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Stage", typeof(string));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Total_Shift", typeof(TimeSpan));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Total_Logged", typeof(TimeSpan));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Total_Attendance", typeof(TimeSpan));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("WOTime", typeof(TimeSpan));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("DateHTMLColor", typeof(TimeSpan));
                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("ManagementValue_", typeof(TimeSpan));
                TotalSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                //ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Maintance11", typeof(string));
                //ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Meeting11", typeof(string));
                foreach (var item in ERMCommon.Instance.ProductionTimeReportLegendloggedColorList)
                {
                    if (item.Name != "Working" && !ERMCommon.Instance.MeetingsList.Values.Contains(item.Name) && !ERMCommon.Instance.MaintenanceList.Values.Contains(item.Name)) //[GEOS2-9197][pallavi jadhav] [26 - 08 - 2025]
                    {
                        //ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add(item.Name, typeof(TimeSpan));
                        //band0.Columns.Add(new ColumnItem() { ColumnFieldName = item.Name, Width = 100, HeaderText = item.Name, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
                        //TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName =  item.Name, DisplayFormat = @" {0:hh\:mm\:ss}" });

                        #region[pallavi jadhav][13 01 2025][GEOS2-6716]
                        if (ERMCommon.Instance.DetailsList_IDLookup != null)
                        {
                            if (ERMCommon.Instance.DetailsList_IDLookup.Contains(item.IdLookupValue))
                            {
                                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add(item.Name, typeof(TimeSpan));
                                band0.Columns.Add(new ColumnItem() { ColumnFieldName = item.Name, Width = 100, HeaderText = item.Name, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
                                #region [GEOS2-6573][rani dhamankar][08-05-2025]
                                if (item.IdLookupValue == 1813)
                                {
                                    ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Details_" + item.IdLookupValue, typeof(string));
                                    band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Details_" + item.IdLookupValue, Width = 100, HeaderText = "Details", IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.Details, Visible = true });
                                    ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("SwitchToStage", typeof(string));
                                    band0.Columns.Add(new ColumnItem() { ColumnFieldName = "SwitchToStage", Width = 100, HeaderText = "SwitchToStage", IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.Details, Visible = true });
                                    ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Download", typeof(TimeSpan));
                                    band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Download", Width = 100, HeaderText = "Download", IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.Details, Visible = true });
                                    ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Transferred", typeof(TimeSpan));
                                    band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Transferred", Width = 100, HeaderText = "Transferred", IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.Details, Visible = true });
                                }
                                #endregion
                                else
                                {
                                    ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Details_" + item.IdLookupValue, typeof(string));
                                    band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Details_" + item.IdLookupValue, Width = 100, HeaderText = "Details", IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.Details, Visible = true });
                                }
                            }
                            else
                            {
                                ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add(item.Name, typeof(TimeSpan));
                                band0.Columns.Add(new ColumnItem() { ColumnFieldName = item.Name, Width = 100, HeaderText = item.Name, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });

                            }

                        }
                        else
                        {
                            ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add(item.Name, typeof(TimeSpan));
                            band0.Columns.Add(new ColumnItem() { ColumnFieldName = item.Name, Width = 100, HeaderText = item.Name, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });

                        }

                        TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = item.Name, DisplayFormat = @" {0:hh\:mm\:ss}" });
                        #endregion

                    }
                }

                //Aishwarya Ingale[Geos2-5853]
                foreach (var item1 in ERMCommon.Instance.ProductionTimeReportManagementLegendColorList)
                {
                    string uniqueColumnName = item1.Name + "_" + item1.IdLookupValue;
                    if (!ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Contains(uniqueColumnName))
                    {
                        ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add(uniqueColumnName, typeof(TimeSpan));
                        //ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add(item1.Name, typeof(TimeSpan));
                        band0.Columns.Add(new ColumnItem() { ColumnFieldName = uniqueColumnName, Width = 100, HeaderText = item1.Name, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.ManagementBreakTemplate, Visible = true });
                        TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = item1.Name, DisplayFormat = @" {0:hh\:mm\:ss}" });
                    }
                }

                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "Plant", DisplayFormat = "Total: {0}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_Shift", DisplayFormat = @" {0:hh\:mm\:ss}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_Logged", DisplayFormat = @" {0:hh\:mm\:ss}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_Attendance", DisplayFormat = @" {0:hh\:mm\:ss}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "WOTime", DisplayFormat = @" {0:hh\:mm\:ss}" });
                Bands.Add(band0);
                //this.ProdControl.ItemsSource = ERMCommon.Instance.DataTableForProductionTimeLine;
                #endregion

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintAndExcelBinding()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PaginationofIndexchangedCommandAction(object obj)
        {
            try
            {
                if (IsPageindex)
                {
                    return;
                }
                var args = obj as DataPagerPageIndexChangingEventArgs;
                int newvalue  = args.NewValue;
                //SelectedproductionTimelineforpaging = ERM_CalenderWeek[newvalue].logWeek;
                SelectedproductionTimeline= ERM_CalenderWeek[newvalue];
                args.IsCancel = false;
               
                //Getlogweek();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //private void GridBinding()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method GridBinding ...", category: Category.Info, priority: Priority.Low);
        //        #region RND 

        //        //ERMCommon.Instance.ProductionBands = new ObservableCollection<BandItem>(); ERMCommon.Instance.ProductionBands.Clear();
        //        Bands = new ObservableCollection<BandItem>(); Bands.Clear();
        //        BandItem band0 = new BandItem() { BandName = "all", Visible = true };
        //        band0.Columns = new ObservableCollection<ColumnItem>();
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant", HeaderText = "Plant", Width = 70, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Week", HeaderText = "Week", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });

        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "GridDate", HeaderText = "Date", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.GridDateTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Employee", HeaderText = "Employee", Width = 250, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Stage", HeaderText = "Stage", Width = 60, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_Shift", HeaderText = "Total Shift", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_Logged", HeaderText = "Total Logged", Width = 100, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Total_Attendance", HeaderText = "Total Attendance", Width = 120, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "WOTime", HeaderText = "WOTime", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "DateHTMLColor", HeaderText = "DateHTMLColor", Width = 90, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.StringTemplate, Visible = true });


        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid = new DataTable();

        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Plant", typeof(string));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Week", typeof(string));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("GridDate", typeof(DateTime));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Employee", typeof(string));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Stage", typeof(string));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Total_Shift", typeof(TimeSpan));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Total_Logged", typeof(TimeSpan));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("Total_Attendance", typeof(TimeSpan));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("WOTime", typeof(TimeSpan));
        //        ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add("DateHTMLColor", typeof(TimeSpan));
        //        //ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Maintance11", typeof(string));
        //        //ERMCommon.Instance.DataTableForProductionTimeLine.Columns.Add("Meeting11", typeof(string));
        //        foreach (var item in ERMCommon.Instance.ProductionTimeReportLegendloggedColorList)
        //        {
        //            if (item.Name != "Working")
        //            {
        //                ERMCommon.Instance.DataTableForProductionTimeLineGrid.Columns.Add(item.Name, typeof(TimeSpan));
        //                band0.Columns.Add(new ColumnItem() { ColumnFieldName = item.Name, Width = 100, HeaderText = item.Name, IsVertical = false, ProductionTimelineSetting = ProductionTimeLineHelper.ProductionTimeLineSettingType.BreakTimeTemplate, Visible = true });
        //            }
        //        }
        //        Bands.Add(band0);
        //        //this.ProdControl.ItemsSource = ERMCommon.Instance.DataTableForProductionTimeLine;
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method GridBinding()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
    }
}
