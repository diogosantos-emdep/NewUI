using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Modules.ERM.Views;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Core;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    class ERMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMServie = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Properties
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        private TableView view;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private WorkOperationsViewModel objWorkOperationsViewModel;
        public WorkOperationsViewModel ObjWorkOperationsViewModell
        {
            get { return objWorkOperationsViewModel; }
            set
            {
                objWorkOperationsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjWorkOperationsViewModel"));
            }
        }

        private PlanningDateReviewViewModel objPlanningDateReviewViewModel;
        public PlanningDateReviewViewModel ObjPlanningDateReviewViewModel
        {
            get { return objPlanningDateReviewViewModel; }
            set
            {
                objPlanningDateReviewViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPlanningDateReviewViewModel"));
            }
        }
        #region [GEOS2-4260][gulab lakade][17 04 2023]
        private PlantOperationalPlanningViewModel objPlantOperationalPlanningViewModel;
        public PlantOperationalPlanningViewModel ObjPlantOperationalPlanningViewModel
        {
            get { return objPlantOperationalPlanningViewModel; }
            set
            {
                objPlantOperationalPlanningViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPlantOperationalPlanningViewModel"));
            }
        }
        #endregion

        private DashboardViewModel objDashboardViewModel;
        public DashboardViewModel ObjDashboardViewModel
        {
            get { return objDashboardViewModel; }
            set
            {
                objDashboardViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDashboardViewModel"));
            }
        }
        //private ReportsDeliveryVisualManagementAndTimeTrackingViewModel objReportsDeliveryVisualManagementAndTimeTrackingViewModel;
        //public ReportsDeliveryVisualManagementAndTimeTrackingViewModel ObjReportsDeliveryVisualManagementAndTimeTrackingViewModel
        //{
        //    get { return objReportsDeliveryVisualManagementAndTimeTrackingViewModel; }
        //    set
        //    {
        //        objReportsDeliveryVisualManagementAndTimeTrackingViewModel = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ObjReportsDeliveryVisualManagementAndTimeTrackingViewModel"));
        //    }
        //}
        #endregion

        private PlantDeliveryAnalysisViewModel objPlantDeliveryAnalysisViewModel;
        public PlantDeliveryAnalysisViewModel ObjPlantDeliveryAnalysisViewModel
        {
            get { return objPlantDeliveryAnalysisViewModel; }
            set
            {
                objPlantDeliveryAnalysisViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPlantDeliveryAnalysisViewModel"));
            }
        }

        private DeliveryTimeDistributionViewModel objDTDWorkstationCpViewModel;
        public DeliveryTimeDistributionViewModel ObjDTDWorkstationCpViewModel
        {
            get { return objDTDWorkstationCpViewModel; }
            set
            {
                objDTDWorkstationCpViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDTDWorkstationCpViewModel"));
            }
        }

        #region [GEOS2-4867][gulab lakade][17 10 2023]
        private ProductionTimelineViewModel objProductionTimelineViewModel;
        public ProductionTimelineViewModel ObjProductionTimelineViewModel
        {
            get { return objProductionTimelineViewModel; }
            set
            {
                objProductionTimelineViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProductionTimelineViewModel"));
            }
        }
        #endregion

        #region  [Daivshala Vighne][GEOS2-6611][28-11-2024]

        private GlobalComparisonViewModel objGlobalComparisonViewModel;
        public GlobalComparisonViewModel ObjGlobalComparisonViewmodel
        {
            get { return objGlobalComparisonViewModel; }
            set
            {
                objGlobalComparisonViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjGlobalComparisonViewmodel"));
            }
        }
        #endregion

        #region  [Daivshala Vighne][GEOS2-6884][28 01 2025]

        private MaterialStockPlanningViewModel objMaterialStockPlanningViewModel;
        public MaterialStockPlanningViewModel ObjMaterialStockPlanningViewModel
        {
            get { return objMaterialStockPlanningViewModel; }
            set
            {
                objMaterialStockPlanningViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjMaterialStockPlanningViewModel"));
            }
        }
        #endregion

        #region [pallavi jadhav][27 03 2025][GEOS2-7095]

        private TimeTrackingViewModel objTimeTrackingViewModel;
        public TimeTrackingViewModel ObjTimeTrackingViewModel
        {
            get { return objTimeTrackingViewModel; }
            set
            {
                objTimeTrackingViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjTimeTrackingViewModel"));
            }
        }

        private CAMCADTimeTrackingViewModel objCAMCADTimeTrackingViewModel;
        public CAMCADTimeTrackingViewModel ObjCAMCADTimeTrackingViewModel
        {
            get { return objCAMCADTimeTrackingViewModel; }
            set
            {
                objCAMCADTimeTrackingViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjCAMCADTimeTrackingViewModel"));
            }
        }
        #endregion
        #region Constructor

        public ERMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ERMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                #region [GEOS2 4275] Shiv Kumar 02/05/2023
                if (GeosApplication.Instance.IsViewPermissionERM == true)
                {


                    TileBarItemsHelper tileBarItemsHelperDashBoard = new TileBarItemsHelper();
                    tileBarItemsHelperDashBoard.Caption = System.Windows.Application.Current.FindResource("DashBoard").ToString();
                    tileBarItemsHelperDashBoard.BackColor = "#00879C";
                    tileBarItemsHelperDashBoard.GlyphUri = "DashBoard.png";
                    tileBarItemsHelperDashBoard.Visibility = Visibility.Visible;
                    tileBarItemsHelperDashBoard.Width = 180;

                    TileBarItemsHelper tileBarItemRealTimeMonitor = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("RealTimeMonitor").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "Real-Time Monitor.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { NavigateRealTimeMonitorView(); })
                    };
                    tileBarItemsHelperDashBoard.Children.Add(tileBarItemRealTimeMonitor);

                    TileBarItemsHelper tileBarItemPlantDeliveryAnalysis = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("PlantDeliveryAnalysis").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "ProductionDelivery.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { NavigatePlantDeliveryAnalysisView(); })
                    };
                    tileBarItemsHelperDashBoard.Children.Add(tileBarItemPlantDeliveryAnalysis);
                    //if (GeosApplication.Instance.IsAdminPermissionERM)
                    //{
                        TileBarItemsHelper tileBarItemPlantLoadAnalysis = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("PlantLoadAnalysis").ToString(),
                            BackColor = "#00879C",
                            GlyphUri = "PlantLoadAnalysis.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { NavigatePlantLoadAnalysisView(); })
                        };

                        tileBarItemsHelperDashBoard.Children.Add(tileBarItemPlantLoadAnalysis);
                   // }
                    TileCollection.Add(tileBarItemsHelperDashBoard);
                }
                #endregion



               
                    if (GeosApplication.Instance.IsViewPermissionERM || GeosApplication.Instance.IsReadSODPermissionERM || GeosApplication.Instance.IsEditSODPermissionERM)
                    {
                        TileBarItemsHelper tileBarItemsHelperStandardTime = new TileBarItemsHelper();
                        tileBarItemsHelperStandardTime.Caption = System.Windows.Application.Current.FindResource("ERMStandardOperationsDictionary").ToString(); // "STD OPS DICTIONARY"
                                                                                                                                                                //tileBarItemsHelperStandardTime.BackColor = "#00879C";
                        tileBarItemsHelperStandardTime.BackColor = "#FF427940";
                        tileBarItemsHelperStandardTime.GlyphUri = "standardTime54X54.png";
                        tileBarItemsHelperStandardTime.Visibility = Visibility.Visible;
                        tileBarItemsHelperStandardTime.NavigateCommand = new DelegateCommand(NavigateStandardOperationsDictionaryView);
                        TileCollection.Add(tileBarItemsHelperStandardTime);
                    }
                
                if (GeosApplication.Instance.IsViewPermissionERM || GeosApplication.Instance.IsTimeTrackingReadPermissionERM)
                {
                    #region [pallavi jadhav][27 03 2025][GEOS2-7095]
                    //if (GeosApplication.Instance.IsTimeTrackingReadPermissionERM)
                    //{
                    TileBarItemsHelper tileBarItemsHelperTrackingTime = new TileBarItemsHelper();
                        tileBarItemsHelperTrackingTime.Caption = System.Windows.Application.Current.FindResource("ERMTimeTracking").ToString(); // "Time Tracking"
                        tileBarItemsHelperTrackingTime.BackColor = "#D17800";
                        tileBarItemsHelperTrackingTime.GlyphUri = "Timetracking.png";
                        tileBarItemsHelperTrackingTime.Visibility = Visibility.Visible;
                        tileBarItemsHelperTrackingTime.Children = new ObservableCollection<TileBarItemsHelper>();
                       // tileBarItemsHelperTrackingTime.NavigateCommand = new DelegateCommand(NavigateTimeTracking);
                       // TileCollection.Add(tileBarItemsHelperTrackingTime);


                    TileBarItemsHelper tileBarItemConfigutionGlobalTrackingTime = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("ERMGlobalTimeTracking").ToString(),
                        BackColor = "#D17800",
                        Width = 180,
                        GlyphUri = "BGlobalTimeTracking.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(NavigateTimeTracking(), null, this); })
                    };
                    tileBarItemsHelperTrackingTime.Children.Add(tileBarItemConfigutionGlobalTrackingTime);

                    TileBarItemsHelper tileBarItemConfigutionCAMCADTrackingTime = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("ERMCAMCADTimeTracking").ToString(),
                        BackColor = "#D17800",
                        Width = 180,
                        GlyphUri = "BTimeTrakingCadCam.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(NavigateCAMCADTimeTracking(), null, this); })
                    };
                    tileBarItemsHelperTrackingTime.Children.Add(tileBarItemConfigutionCAMCADTrackingTime);
                    TileCollection.Add(tileBarItemsHelperTrackingTime);
                    #endregion
                    //}
                }
                #region  [GEOS2-4151] [Gulaba lakade][31 01 2023]
                if (GeosApplication.Instance.IsViewPermissionERM || GeosApplication.Instance.IsReadProductionPlanPermissionERM || GeosApplication.Instance.IsEditProductionPlanPermissionERM)
                {
                    //if (GeosApplication.Instance.IsReadProductionPlanPermissionERM || GeosApplication.Instance.IsEditProductionPlanPermissionERM)       // [Rupali Sarode][GEOS2-4155][03-02-2023]
                    //{
                        TileBarItemsHelper tileBarItemsHelperProductionPlan = new TileBarItemsHelper();
                        tileBarItemsHelperProductionPlan.Caption = System.Windows.Application.Current.FindResource("ProductionPlan").ToString(); // "STD OPS DICTIONARY"
                        tileBarItemsHelperProductionPlan.BackColor = "#FCC419";
                        tileBarItemsHelperProductionPlan.GlyphUri = "ProductionPlan.png";
                        tileBarItemsHelperProductionPlan.Visibility = Visibility.Visible;
                        tileBarItemsHelperProductionPlan.Width = 180;
                        // tileBarItemsHelperProductionPlan.NavigateCommand = new DelegateCommand(NavigateTimeTracking);

                        #region[GEOS2 4260] Shiv Kumar 18/04/2023

                        TileBarItemsHelper tileBarItemProductionPlan = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("PlanningDateReview").ToString(),
                            BackColor = "#FCC419",
                            GlyphUri = "Production_Review_Date.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { NavigatePlanningDateReviewView(); })
                        };
                        tileBarItemsHelperProductionPlan.Children.Add(tileBarItemProductionPlan);
                        #endregion

                        #region [GEOS2-4260 Shiv Kumar 18/04/2023]
                        TileBarItemsHelper tileBarPlantOperationalPlanning = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("PlantOperationalPlanning").ToString(),

                            BackColor = "#FCC419",
                            GlyphUri = "Plant Operational Planning.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { NavigatePlantOperationalPlanningView(); })
                        };
                        tileBarItemsHelperProductionPlan.Children.Add(tileBarPlantOperationalPlanning);

                        #endregion
                        TileCollection.Add(tileBarItemsHelperProductionPlan);
                    //}
                }

                #endregion

                #region REPORTS [GEOS2 4615] [pallavi jadhav][28-06-2023]
                if (GeosApplication.Instance.IsViewPermissionERM)
                {
                    TileBarItemsHelper tileBarItemsHelperReports = new TileBarItemsHelper();
                    tileBarItemsHelperReports.Caption = System.Windows.Application.Current.FindResource("ERMReports").ToString();
                    tileBarItemsHelperReports.BackColor = "#A84FA9";
                    tileBarItemsHelperReports.GlyphUri = "Reports.png";
                    tileBarItemsHelperReports.Visibility = Visibility.Visible;
                    tileBarItemsHelperReports.Children = new ObservableCollection<TileBarItemsHelper>();

                    if (GeosApplication.Instance.IsViewSupervisorERM)     //[Pallavi jadhav][GEOS2-5910][17-07-2024]
                    {

                    }
                    else
                    {
                        TileBarItemsHelper tileBarItemReportsOperations = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("DeliveryVisualManagementReports").ToString(),
                            BackColor = "#B637FB",
                            GlyphUri = "DeliveryVisualManagement.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand<object>(NavigateDeliveryVisualManagementReport)  ////[GEOS2-4624][rupali sarode][29-06-2023]
                        };
                        tileBarItemsHelperReports.Children.Add(tileBarItemReportsOperations);

                        TileBarItemsHelper tileBarReport = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("TimeTracking").ToString(),

                            BackColor = "#B637FB",
                            GlyphUri = "Timetracking.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand<object>(NavigateTimeTrackingReport)
                            // NavigateCommand = new DelegateCommand(NavigateTimeTrackingReport)
                            //  NavigateCommand = new DelegateCommand(() => { Service.Navigate(NavigateTimeTrackingReport(), null, this); })
                        };
                        tileBarItemsHelperReports.Children.Add(tileBarReport);

                        //[Aishwarya Ingale][GEOS2-4820][13-09-2023]
                        TileBarItemsHelper tileBarReworksOperations = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("Reworks").ToString(),

                            BackColor = "#B637FB",
                            GlyphUri = "ReworkReport.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand<object>(NavigateReworkReport)
                        };
                        tileBarItemsHelperReports.Children.Add(tileBarReworksOperations);
                    }
                    //[Aishwarya Ingale][GEOS2-4820][20-10-2023]
                    //if (GeosApplication.Instance.IsAdminPermissionERM)
                    //{
                    TileBarItemsHelper tileBarProductionTimeline = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("ProductionTimeline").ToString(),

                        BackColor = "#B637FB",
                        GlyphUri = "ProductionTimeline.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(NavigateProductionTimelineReports)
                    };
                    tileBarItemsHelperReports.Children.Add(tileBarProductionTimeline);


                    //[Daivshala Vighne][GEOS2-6611][28-11-2024]

                    TileBarItemsHelper tileBarGlobalCompTimeResults = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("GlobalCompTimeResults").ToString(),

                        BackColor = "#B637FB",
                        GlyphUri = "GlobalCompTRes.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(NavigateGlobalComparisonTimesResultsReports)
                    };
                    tileBarItemsHelperReports.Children.Add(tileBarGlobalCompTimeResults);

                    #region [GEOS2-6884][Daivshala Vighne][28 01 2025]
                    TileBarItemsHelper tileBarMaterialStockPlanning = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("MaterialStockPlanning").ToString(),

                        BackColor = "#B637FB",
                        GlyphUri = "MaterialStockPlanning.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(NavigateMaterialStockPlanningReports)
                    };
                    tileBarItemsHelperReports.Children.Add(tileBarMaterialStockPlanning);
                    #endregion



                    TileCollection.Add(tileBarItemsHelperReports);
                    // }
                }

                #endregion

                #region OPS Management [GEOS2-4617][gulab lakade][04-07-2023]
                if (GeosApplication.Instance.IsViewPermissionERM)
                {
                    TileBarItemsHelper tileBarItemsHelperOPManagement = new TileBarItemsHelper();
                    tileBarItemsHelperOPManagement.Caption = System.Windows.Application.Current.FindResource("OPSManagement").ToString();
                    tileBarItemsHelperOPManagement.BackColor = "#5C5C5C";
                    tileBarItemsHelperOPManagement.GlyphUri = "OPSManagement.png";
                    tileBarItemsHelperOPManagement.Visibility = Visibility.Visible;
                    //tileBarItemsHelperOPManagement.Children = new ObservableCollection<TileBarItemsHelper>();


                    // tileBarItemsHelperOPManagement.Children.Add(tileBarReport);

                    TileCollection.Add(tileBarItemsHelperOPManagement);
                }
                #endregion
                //if (GeosApplication.Instance.IsViewPermissionERM)
                //{
                    //if (GeosApplication.Instance.IsReadWOPermissionERM)
                    //{
                        TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                        tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("ERMConfiguration").ToString();
                        tileBarItemsHelperConfiguration.BackColor = "#8b99e8";
                        tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                        tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                        tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                    if (GeosApplication.Instance.IsViewPermissionERM || GeosApplication.Instance.IsReadWOPermissionERM || GeosApplication.Instance.IsEditWOPermissionERM)
                    {
                        TileBarItemsHelper tileBarItemConfigutionOperations = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("ERMOperations").ToString(),
                            BackColor = "#8b99e8",
                            GlyphUri = "WorkOperation.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(NavigateWorkOperationsView(), null, this); })
                        };
                        tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigutionOperations);
                    }

                    #region GEOS2-3600 Gulab lakade 27/12/2022
                    //   if (GeosApplication.Instance.IsReadWorkStagesPermissionERM || GeosApplication.Instance.IsEditWorkStagesPermissionERM )       ////[sdeshpande][GEOS2-3841][11/1/2023]

                    if (GeosApplication.Instance.IsViewPermissionERM || GeosApplication.Instance.IsReadWorkStagesPermissionERM || GeosApplication.Instance.IsEditWorkStagesPermissionERM)       ////[sdeshpande][GEOS2-3841][11/1/2023]

                    {
                            TileBarItemsHelper tileBarItemWorkStage = new TileBarItemsHelper()
                            {
                                Caption = System.Windows.Application.Current.FindResource("Workstages").ToString(),

                                BackColor = "#8b99e8",
                                GlyphUri = "WorkStage.png",
                                Visibility = Visibility.Visible,
                                NavigateCommand = new DelegateCommand<object>(NavigateWorkstages)
                            };
                            tileBarItemsHelperConfiguration.Children.Add(tileBarItemWorkStage);
                        }
                        #endregion

                        #region GEOS2-4242 Pallavi Jadhav 03/04/2023
                        //if (GeosApplication.Instance.IsViewPermissionERM)
                        //{
                            if (GeosApplication.Instance.IsAdminPermissionERM || GeosApplication.Instance.IsViewPermissionERM)
                            {
                                TileBarItemsHelper tileBarModulesEquivalencyWeight = new TileBarItemsHelper()
                                {
                                    Caption = System.Windows.Application.Current.FindResource("ModulesEquivalencyWeight").ToString(),

                                    BackColor = "#8b99e8",
                                    GlyphUri = "ModulesEquivalencyWeight.png",
                                    Visibility = Visibility.Visible,
                                    Height = 500,
                                    NavigateCommand = new DelegateCommand<object>(NavigateModulesEquivalencyWeight)
                                };
                                tileBarItemsHelperConfiguration.Children.Add(tileBarModulesEquivalencyWeight);
                            }
                      //  }
                        //if (GeosApplication.Instance.IsViewPermissionERM)
                        //{
                            if (GeosApplication.Instance.IsAdminPermissionERM || GeosApplication.Instance.IsViewPermissionERM)
                            {
                                TileBarItemsHelper tileBarStructuresEquivalencyWeight = new TileBarItemsHelper()
                                {
                                    Caption = System.Windows.Application.Current.FindResource("StructuresEquivalencyWeight").ToString(),

                                    BackColor = "#8b99e8",
                                    GlyphUri = "StructuresEquivalencyWeight.png",
                                    Visibility = Visibility.Visible,
                                    NavigateCommand = new DelegateCommand<object>(NavigateStructuresEquivalencyWeight)
                                };
                                tileBarItemsHelperConfiguration.Children.Add(tileBarStructuresEquivalencyWeight);
                            }
                // }
                #endregion

                #region Aishwarya
                //if (GeosApplication.Instance.IsViewPermissionERM)
                //{


                if (GeosApplication.Instance.IsViewPermissionERM || GeosApplication.Instance.IsEditDeliveryTimeDistributionERM) //[GEOS2-5269][Rupali Sarode][05-02-2024]
                {
                    TileBarItemsHelper tileBarDTDWorkstationCP = new TileBarItemsHelper()
                                {
                                    Caption = System.Windows.Application.Current.FindResource("DTDWorkstationCp").ToString(),

                                    BackColor = "#8b99e8",
                                    GlyphUri = "DTDWorkstation.png",
                                    Visibility = Visibility.Visible,
                                    NavigateCommand = new DelegateCommand(() => { NavigateDTDWorkstationCp(); })
                                };
                                tileBarItemsHelperConfiguration.Children.Add(tileBarDTDWorkstationCP);
                }
                //}
                #endregion


                if (GeosApplication.Instance.IsViewPermissionERM)
                {
                    TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("MyPreferences").ToString(),
                        BackColor = "#8b99e8",
                        GlyphUri = "MyPreference_Black.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(NavigateMyPreferences)
                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

                    
                }

                TileCollection.Add(tileBarItemsHelperConfiguration);

                //}
                GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();
                ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                ERMCommon.Instance.SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                //ERMService = new ERMServiceController("localhost:6699");
                #region [GEOS2-8698][rani dhamankar][15-07-2025]
                //GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
                //GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();

                //ERMCommon.Instance.PlantOwnerUsersList = ERMService.GetAllCompaniesDetails_V2660(GeosApplication.Instance.ActiveUser.IdUser);
                ERMCommon.Instance.PlantOwnerUsersList = ERMService.GetAllCompaniesDetails_V2680(GeosApplication.Instance.ActiveUser.IdUser);//[pallavi jadhav][11 11 2025][GEOS2-10146]
                ERMCommon.Instance.PlantOwnerUsersList = ERMCommon.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();

                //ERMService = new ERMServiceController("localhost:6699");
                //PLMService = new PLMServiceController("localhost:6699");

                //[GEOS2-4619][rupali sarode][28-06-2023]
                // ERMCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                // ERMCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Site>(ERMService.GetPlants_V2410(GeosApplication.Instance.ActiveUser.IdUser));

                //[GEOS2-8698][rani dhamankar] [15 -07 - 2025]
                //ERMCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Site>(ERMService.GetPlants_V2660(GeosApplication.Instance.ActiveUser.IdUser));
                ERMCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Site>(ERMService.GetPlants_V2680(GeosApplication.Instance.ActiveUser.IdUser)); //[pallavi jadhav][11 11 2025][GEOS2-10146]
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company usrDefault = ERMCommon.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                #endregion

                Site selectedPlant = ERMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Name == serviceurl);

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
                    ERMCommon.Instance.SelectedAuthorizedPlantsList.Add(selectedPlant);
                }
                GeosApplication.Instance.Logger.Log("Constructor Constructor ERMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor ERMMainViewModel() Method - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in  Constructor ERMMainViewModel() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ERMMainViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateStandardOperationsDictionaryView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateStandardOperationsDictionaryView()...", category: Category.Info, priority: Priority.Low);
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();
                StandardOperationsDictionaryViewModel standardOperationsDictionaryViewModel = new StandardOperationsDictionaryViewModel();
                standardOperationsDictionaryViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.StandardOperationsDictionaryView", standardOperationsDictionaryViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateStandardOperationsDictionaryView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateStandardOperationsDictionaryView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private TimeTrackingView NavigateTimeTracking()
        {
            #region [pallavi jadhav][27 03 2025][GEOS2-7095]
            TimeTrackingView timeTrackingView = new TimeTrackingView();
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateTimeTracking()...", category: Category.Info, priority: Priority.Low);
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();
            
                TimeTrackingViewModel TimeTrackingViewModel = new TimeTrackingViewModel();
                TimeTrackingViewModel.Init();
                ObjTimeTrackingViewModel = TimeTrackingViewModel;
                timeTrackingView.DataContext = TimeTrackingViewModel;
            //    Service.Navigate("Emdep.Geos.Modules.ERM.Views.TimeTrackingView", TimeTrackingViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateTimeTracking()....executed successfully", category: Category.Info, priority: Priority.Low);
               
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateStandardOperationsDictionaryView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return timeTrackingView;
            #endregion
        }

        #region [GEOS2-3997][Rupali Sarode][01Dec2022]
        private void NavigateMyPreferences(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferences()...", category: Category.Info, priority: Priority.Low);
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferences()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateMyPreferences()..", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        #endregion

        #region GEOS2-3600 Gulab lakade 27/12/2022
        private void NavigateWorkstages(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateWorkstages()...", category: Category.Info, priority: Priority.Low);
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();

                WorkStagesViewModel WorkStagesViewModel = new WorkStagesViewModel();
                WorkStagesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.WorkStagesView", WorkStagesViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateWorkStages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateWorkStages()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region GEOS2-4242 Pallavi Jadhav 03/04/2023
        private void NavigateModulesEquivalencyWeight(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateModulesEquivalencyWeight()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();

                ModuleEquivalencyWeightViewModel ModuleEquivalencyWeightViewModel = new ModuleEquivalencyWeightViewModel();
                ModuleEquivalencyWeightViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.ModuleEquivalencyWeightView", ModuleEquivalencyWeightViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateModulesEquivalencyWeight()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateModulesEquivalencyWeight()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateStructuresEquivalencyWeight(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateStructuresEquivalencyWeight()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();

                StructuresEquivalencyWeightViewModel ModuleEquivalencyWeightViewModel = new StructuresEquivalencyWeightViewModel();
                ModuleEquivalencyWeightViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.StructuresEquivalencyWeightView", ModuleEquivalencyWeightViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateStructuresEquivalencyWeight()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateStructuresEquivalencyWeight()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

       // Aishwarya Ingale[Geos2 - 5269][29/01/2024]
        //private void NavigateDTDWorkstationCp(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method NavigateDTDWorkstationCp()...", category: Category.Info, priority: Priority.Low);

        //        if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
        //            SavechangesInWorkOperationGrid();


        //        if (PlanningAppointment.IsSaveButtonEnabled)
        //            SaveChangesInPlanningDateReview();

        //        if (PlanningDateReviewCellEditHelper.IsValueChanged)
        //            SaveGridChangesInPlanningDateReview();

        //        DTDWorkstationCpViewModel DTDWorkstationCpViewModel = new DTDWorkstationCpViewModel();
        //        DTDWorkstationCpViewModel.Init();
        //        Service.Navigate("Emdep.Geos.Modules.ERM.Views.DTDWorkstationCpView", DTDWorkstationCpViewModel, null, this, true);

        //        GeosApplication.Instance.Logger.Log("Method NavigateDTDWorkstationCp()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateDTDWorkstationCp()..", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        private void NavigateDTDWorkstationCp()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDTDWorkstationCp()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                DeliveryTimeDistributionViewModel DTDWorkstationCpViewModel = new DeliveryTimeDistributionViewModel();
                ObjDTDWorkstationCpViewModel = DTDWorkstationCpViewModel;
                DTDWorkstationCpViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.DTDWorkstationCpView", DTDWorkstationCpViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDTDWorkstationCp()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateDTDWorkstationCp()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }



        //#region [GEOS2 4922] Aishwarya Ingale 13/10/2023
        //private void NavigateDTDWorkstationCp(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method NavigateDTDWorkstationCp()...", category: Category.Info, priority: Priority.Low);



        //        DTDWorkstationCpViewModel DTDWorkstationCpViewModel = new DTDWorkstationCpViewModel();
        //        ObjDTDWorkstationCpViewModel = DTDWorkstationCpViewModel;
        //        DTDWorkstationCpViewModel.Init();
        //        Service.Navigate("Emdep.Geos.Modules.ERM.Views.DTDWorkstationCpView", DTDWorkstationCpViewModel, null, this, true);

        //        GeosApplication.Instance.Logger.Log("Method NavigateProductionTimelineView()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductionTimelineView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //#endregion



        #endregion
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private WorkOperationsView NavigateWorkOperationsView()
        {

            if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                SavechangesInWorkOperationGrid();

            //[Rupali Sarode][GEOS2-4161][02-03-2023]
            if (PlanningAppointment.IsSaveButtonEnabled)
                SaveChangesInPlanningDateReview();
            //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
            if (PlanningDateReviewCellEditHelper.IsValueChanged)
                SaveGridChangesInPlanningDateReview();

            WorkOperationsView workOperationsView = new WorkOperationsView();
            WorkOperationsViewModel WorkOperationsViewModel = new WorkOperationsViewModel();
            WorkOperationsViewModel.Init();
            ObjWorkOperationsViewModell = WorkOperationsViewModel;
            workOperationsView.DataContext = WorkOperationsViewModel;
            return workOperationsView;
        }

        public void SavechangesInWorkOperationGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["WorkOperationGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ERMWorkOperationViewMultipleCellEditHelper.Checkview == "WorkOperationsListTableView")
                    {
                        ObjWorkOperationsViewModell.UpdateMultipleRowsWorkOperationGridCommandAction(ERMWorkOperationViewMultipleCellEditHelper.Viewtableview);
                    }
                }
                ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {

            }
        }
        #region [GEOS2-4151] [Gulaba lakade][31 01 2023]
        private void NavigatePlanningDateReviewView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePlanningDateReviewView()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                PlanningDateReviewViewModel planningDateReviewViewModel = new PlanningDateReviewViewModel();
                ObjPlanningDateReviewViewModel = planningDateReviewViewModel;
                planningDateReviewViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.PlanningDateReviewView", planningDateReviewViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigatePlanningDateReviewView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePlanningDateReviewView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        //[GEOS2-4161][Rupali Sarode][02-03-2023]
        private void SaveChangesInPlanningDateReview()
        {
            MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                ObjPlanningDateReviewViewModel.SavePlanningDateCommandAction();
            }
            else
            {
                ObjPlanningDateReviewViewModel.IsSaveEnabled = false;
                PlanningAppointment.IsSaveButtonEnabled = false;
            }

        }

        //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
        private void SaveGridChangesInPlanningDateReview()
        {
            view = PlanningDateReviewCellEditHelper.Viewtableview;
            if (PlanningDateReviewCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ObjPlanningDateReviewViewModel.SaveGridPlanningDateCommandAction(PlanningDateReviewCellEditHelper.Viewtableview);
                }
                PlanningDateReviewCellEditHelper.IsValueChanged = false;
            }

            PlanningDateReviewCellEditHelper.IsValueChanged = false;

            if (view != null)
            {
                PlanningDateReviewCellEditHelper.SetIsValueChanged(view, false);
            }

        }

        #endregion

        #region [GEOS2-4260] [Shiv Kumar][18 04 2023]
        private void NavigatePlantOperationalPlanningView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePlantOperationalPlanningView()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();

                PlantOperationalPlanningViewModel plantOperationalPlanningViewModel = new PlantOperationalPlanningViewModel();
                ObjPlantOperationalPlanningViewModel = plantOperationalPlanningViewModel;
                plantOperationalPlanningViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.PlantOperationalPlanningView", plantOperationalPlanningViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigatePlantOperationalPlanningView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePlantOperationalPlanningView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region [GEOS2 4275] Shiv Kumar 02/05/2023
        private void NavigateRealTimeMonitorView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateRealTimeMonitorView()...", category: Category.Info, priority: Priority.Low);

                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();

                DashboardViewModel DashboardViewModel = new DashboardViewModel();
                ObjDashboardViewModel = DashboardViewModel;
                DashboardViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.DashboardView", DashboardViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateRealTimeMonitorView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateRealTimeMonitorView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion



        #region [GEOS2-4624][rupali sarode][29-06-2023]
        private void NavigateDeliveryVisualManagementReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDeliveryVisualManagementReport()...", category: Category.Info, priority: Priority.Low);

                ReportsDeliveryVisualManagementAndTimeTrackingViewModel reportsDeliveryVisualManagementAndTimeTrackingViewModel = new ReportsDeliveryVisualManagementAndTimeTrackingViewModel("DeliveryVisualManagement"); //[nsatpute][25-06-2025][GEOS2-8641]
                ReportsDeliveryVisualManagementAndTimeTrackingView reportsDeliveryVisualManagementAndTimeTrackingView = new ReportsDeliveryVisualManagementAndTimeTrackingView();
                EventHandler handle = delegate { reportsDeliveryVisualManagementAndTimeTrackingView.Close(); };
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.RequestClose += handle;
                //reportsDeliveryVisualManagementAndTimeTrackingViewModel.Source = "DeliveryVisualManagement"; //[GEOS2-4624][rupali sarode][29-06-2023]
                reportsDeliveryVisualManagementAndTimeTrackingView.DataContext = reportsDeliveryVisualManagementAndTimeTrackingViewModel;
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("DVMReport").ToString();
                reportsDeliveryVisualManagementAndTimeTrackingView.DataContext = reportsDeliveryVisualManagementAndTimeTrackingViewModel;

                reportsDeliveryVisualManagementAndTimeTrackingViewModel.Init();
                reportsDeliveryVisualManagementAndTimeTrackingView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method NavigateDeliveryVisualManagementReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateTimeTrackingReport()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region [GEOS2-4626][pallavi Jadhav][03 07 2023]
        private void NavigateTimeTrackingReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateTimeTrackingReport()...", category: Category.Info, priority: Priority.Low);

                ReportsDeliveryVisualManagementAndTimeTrackingViewModel reportsDeliveryVisualManagementAndTimeTrackingViewModel = new ReportsDeliveryVisualManagementAndTimeTrackingViewModel("TimeTracking"); //[nsatpute][25-06-2025][GEOS2-8641]
                ReportsDeliveryVisualManagementAndTimeTrackingView reportsDeliveryVisualManagementAndTimeTrackingView = new ReportsDeliveryVisualManagementAndTimeTrackingView();
                EventHandler handle = delegate { reportsDeliveryVisualManagementAndTimeTrackingView.Close(); };
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.RequestClose += handle;
                //reportsDeliveryVisualManagementAndTimeTrackingViewModel.Source = "TimeTracking";
                reportsDeliveryVisualManagementAndTimeTrackingView.DataContext = reportsDeliveryVisualManagementAndTimeTrackingViewModel;
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("TimeTrackingReport").ToString();
                reportsDeliveryVisualManagementAndTimeTrackingView.DataContext = reportsDeliveryVisualManagementAndTimeTrackingViewModel;
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.Init();
                reportsDeliveryVisualManagementAndTimeTrackingView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method NavigateTimeTrackingReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateTimeTrackingReport()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
        #region [GEOS2-4820][pallavi Jadhav][13 09 2023]
        private void NavigateReworkReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateReworkReport()...", category: Category.Info, priority: Priority.Low);

                ReportsDeliveryVisualManagementAndTimeTrackingViewModel reportsDeliveryVisualManagementAndTimeTrackingViewModel = new ReportsDeliveryVisualManagementAndTimeTrackingViewModel("Rework"); //[nsatpute][25-06-2025][GEOS2-8641]
                ReportsDeliveryVisualManagementAndTimeTrackingView reportsDeliveryVisualManagementAndTimeTrackingView = new ReportsDeliveryVisualManagementAndTimeTrackingView();
                EventHandler handle = delegate { reportsDeliveryVisualManagementAndTimeTrackingView.Close(); };
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.RequestClose += handle;
                //reportsDeliveryVisualManagementAndTimeTrackingViewModel.Source = "Rework";
                reportsDeliveryVisualManagementAndTimeTrackingView.DataContext = reportsDeliveryVisualManagementAndTimeTrackingViewModel;
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("ReworkReport").ToString();
                reportsDeliveryVisualManagementAndTimeTrackingView.DataContext = reportsDeliveryVisualManagementAndTimeTrackingViewModel;
                reportsDeliveryVisualManagementAndTimeTrackingViewModel.Init();
                reportsDeliveryVisualManagementAndTimeTrackingView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method NavigateReworkReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateReworkReport()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region [GEOS2 4922] Aishwarya Ingale 13/10/2023
        private void NavigatePlantDeliveryAnalysisView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductionTimelineView()...", category: Category.Info, priority: Priority.Low);

                

                PlantDeliveryAnalysisViewModel PlantDeliveryAnalysisViewModel = new PlantDeliveryAnalysisViewModel();
                ObjPlantDeliveryAnalysisViewModel = PlantDeliveryAnalysisViewModel;
                PlantDeliveryAnalysisViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.PlantDeliveryAnalysisView", PlantDeliveryAnalysisViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductionTimelineView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductionTimelineView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region [GEOS2-4867][gulab lakade][16 10 2023]
        private void NavigateProductionTimelineReports(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductionTimelineReports()...", category: Category.Info, priority: Priority.Low);

                ProductionTimelineViewModel productionTimelineViewModel = new ProductionTimelineViewModel();

                ObjProductionTimelineViewModel = productionTimelineViewModel;
                productionTimelineViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.ProductionTimelineView", productionTimelineViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateReworkReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductionTimelineReports()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion



        #region  [Daivshala Vighne][GEOS2-6611][28-11-2024]
        private void NavigateGlobalComparisonTimesResultsReports(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateGlobalComparisonTimesResultsReports()...", category: Category.Info, priority: Priority.Low);

                GlobalComparisonViewModel globalComparisonViewmodel = new GlobalComparisonViewModel();

                ObjGlobalComparisonViewmodel = globalComparisonViewmodel;
                globalComparisonViewmodel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.GlobalComparisonView", globalComparisonViewmodel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateGlobalComparisonTimesResultsReports()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateGlobalComparisonTimesResultsReports()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }


        }

        #endregion


        #region [GEOS2 5031] Pallavi Jadhav 27/11/2023
        private void NavigatePlantLoadAnalysisView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePlantLoadAnalysisView()...", category: Category.Info, priority: Priority.Low);

               

                PlantLoadAnalysisViewModel PlantLoadAnalysisViewModel = new PlantLoadAnalysisViewModel();
                PlantLoadAnalysisViewModel = PlantLoadAnalysisViewModel;
                PlantLoadAnalysisViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.PlantLoadAnalysisView", PlantLoadAnalysisViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigatePlantLoadAnalysisView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePlantLoadAnalysisView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region [GEOS2-6884][Daivshala Vighne][28 01 2025]
        private void NavigateMaterialStockPlanningReports(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateMaterialStockPlanningReports()...", category: Category.Info, priority: Priority.Low);

                MaterialStockPlanningViewModel materialStockPlanningViewModel = new MaterialStockPlanningViewModel();

                ObjMaterialStockPlanningViewModel = materialStockPlanningViewModel;
                materialStockPlanningViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.MaterialStockPlanningView", materialStockPlanningViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateMaterialStockPlanningReports()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateMaterialStockPlanningReports()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }


        }

        #endregion

        #region [pallavi jadhav][27 03 2025][GEOS2-7095]
        private CAMCADTimeTrackingView NavigateCAMCADTimeTracking()
        {
            CAMCADTimeTrackingView cAMCADTimeTrackingView = new CAMCADTimeTrackingView();
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateCAMCADTimeTracking()...", category: Category.Info, priority: Priority.Low);
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                //[Rupali Sarode][GEOS2-4161][02-03-2023]
                if (PlanningAppointment.IsSaveButtonEnabled)
                    SaveChangesInPlanningDateReview();
                //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
                if (PlanningDateReviewCellEditHelper.IsValueChanged)
                    SaveGridChangesInPlanningDateReview();

                CAMCADTimeTrackingViewModel CAMCADTimeTrackingViewModel = new CAMCADTimeTrackingViewModel();
                CAMCADTimeTrackingViewModel.Init();
                ObjCAMCADTimeTrackingViewModel = CAMCADTimeTrackingViewModel;
                cAMCADTimeTrackingView.DataContext = CAMCADTimeTrackingViewModel;
                Service.Navigate("Emdep.Geos.Modules.ERM.Views.CAMCADTimeTrackingView", CAMCADTimeTrackingViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateCAMCADTimeTracking()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateCAMCADTimeTracking()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return cAMCADTimeTrackingView;
        }
        #endregion

    }
}
