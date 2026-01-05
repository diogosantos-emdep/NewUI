using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
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
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data;


namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class CrmMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region TaskLog
        //GEOS2-222 Sprint-60 Recurrent Activities section must be visible only for "Audit" permission [adadibathina]
        #endregion

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        bool isBusy;
        LeadsViewModel objLeadsViewModel;
        LeadsPipelineViewModel objLeadsPipelineViewModel;
        OrderViewModel objOrderViewModel;
        AccountViewModel objAccountViewModel;
        ContactViewModel objContactViewModel;
        AnnualSalesPerformanceViewModel objAnnualSalesPerformanceViewModel;
        DashboardSaleViewModel objDashboardSaleViewModel;
        DahsBoardPerformanceViewModel objDahsBoardPerformanceViewModel;
        DashBoardOperationsViewModel objDashBoardOperationsViewModel;
        DashboardEngineeringAnalysisViewModel objDashboardEngineeringAnalysisViewModel;
        ReportDashboardViewModel objReportDashboardViewModel;
        ItemForecastReportViewModel objItemForecastReportViewModel;
        TargetAndForecastViewModel objTargetAndForecastViewModel;
        UsersViewModel objUsersViewModel;
        PlantQuotaViewModel objPlantQuotaViewModel;
        ActivityViewModel objActivityViewModel;
        ActivityReportViewModel objActivityReportViewModel;
        ArticlesReportViewModel objArticlesReportViewModel;
        ModulesReportViewModel objModulesReportViewModel;
        BackgroundWorker MyWorker = new BackgroundWorker();
        ActionsViewModel objActionsViewModel;

        private ObservableCollection<Customer> listGroup;
        private ObservableCollection<Company> entireCompanyPlantList;

        #endregion

        #region Properties
        public ObservableCollection<Customer> ListGroup
        {
            get { return listGroup; }
            set
            {
                listGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListGroup"));
            }
        }
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public ActivityViewModel ObjActivityViewModel
        {
            get { return objActivityViewModel; }
            set
            {
                objActivityViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjActivityViewModel"));
            }
        }

        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public LeadsViewModel ObjLeadsViewModel
        {
            get { return objLeadsViewModel; }
            set
            {
                objLeadsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjLeadsViewModel"));
            }
        }

        public LeadsPipelineViewModel ObjLeadsPipelineViewModel
        {
            get { return objLeadsPipelineViewModel; }
            set
            {
                objLeadsPipelineViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjLeadsPipelineViewModel"));
            }
        }

        public OrderViewModel ObjOrderViewModel
        {
            get { return objOrderViewModel; }
            set
            {
                objOrderViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjOrderViewModel"));
            }
        }

        public AccountViewModel ObjAccountViewModel
        {
            get { return objAccountViewModel; }
            set
            {
                objAccountViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjAccountViewModel"));
            }
        }

        public ContactViewModel ObjContactViewModel
        {
            get { return objContactViewModel; }
            set
            {
                objContactViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjContactViewModel"));
            }
        }

        public AnnualSalesPerformanceViewModel ObjAnnualSalesPerformanceViewModel
        {
            get { return objAnnualSalesPerformanceViewModel; }
            set
            {
                objAnnualSalesPerformanceViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjAnnualSalesPerformanceViewModel"));
            }
        }

        public DashboardSaleViewModel ObjDashboardSaleViewModel
        {
            get { return objDashboardSaleViewModel; }
            set
            {
                objDashboardSaleViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDashboardSaleViewModel"));
            }
        }

        public DahsBoardPerformanceViewModel ObjDahsBoardPerformanceViewModel
        {
            get { return objDahsBoardPerformanceViewModel; }
            set
            {
                objDahsBoardPerformanceViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDahsBoardPerformanceViewModel"));
            }
        }


        public DashBoardOperationsViewModel ObjDashBoardOperationsViewModel
        {
            get { return objDashBoardOperationsViewModel; }
            set
            {
                objDashBoardOperationsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDashBoardOperationsViewModel"));
            }
        }

        public DashboardEngineeringAnalysisViewModel ObjDashboardEngineeringAnalysisViewModel
        {
            get { return objDashboardEngineeringAnalysisViewModel; }
            set
            {
                objDashboardEngineeringAnalysisViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDashboardEngineeringAnalysisViewModel"));
            }
        }

        public ReportDashboardViewModel ObjReportDashboardViewModel
        {
            get { return objReportDashboardViewModel; }
            set
            {
                objReportDashboardViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjReportDashboardViewModel"));
            }
        }

        public ItemForecastReportViewModel ObjItemForecastReportViewModel
        {
            get { return objItemForecastReportViewModel; }
            set
            {
                objItemForecastReportViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjItemForecastReportViewModel"));
            }
        }

        public TargetAndForecastViewModel ObjTargetAndForecastViewModel
        {
            get { return objTargetAndForecastViewModel; }
            set
            {
                objTargetAndForecastViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjTargetAndForecastViewModel"));
            }
        }

        public UsersViewModel ObjUsersViewModel
        {
            get
            {
                return objUsersViewModel;
            }

            set
            {
                objUsersViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjUsersViewModel"));
            }
        }

        public PlantQuotaViewModel ObjPlantQuotaViewModel
        {
            get { return objPlantQuotaViewModel; }
            set
            {
                objPlantQuotaViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPlantQuotaViewModel"));
            }
        }

        public ActivityReportViewModel ObjActivityReportViewModel
        {
            get { return objActivityReportViewModel; }
            set
            {
                objActivityReportViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjActivityReportViewModel"));
            }
        }

        public ArticlesReportViewModel ObjArticlesReportViewModel
        {
            get { return objArticlesReportViewModel; }
            set
            {
                objArticlesReportViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjArticlesReportViewModel"));
            }
        }

        public ModulesReportViewModel ObjModulesReportViewModel
        {
            get { return objModulesReportViewModel; }
            set
            {
                objModulesReportViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjModulesReportViewModel"));
            }
        }

        public ActionsViewModel ObjActionsViewModel
        {
            get { return objActionsViewModel; }
            set
            {
                objActionsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjActionsViewModel"));
            }
        }

        #endregion // Properties

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

        public CrmMainViewModel()
        {
            //Task task = new Task(() => FillCommonDetails());

            GetLoadtimeTasks();
        }

        #endregion // Constructor

        #region Methods

        public async void GetLoadtimeTasks()
        {
            try
            {


                if (!GeosApplication.Instance.ObjectPool.ContainsKey("CrmMainViewModel") || GeosApplication.Instance.ObjectPool.Count == 0)
                    GeosApplication.Instance.ObjectPool.Add("CrmMainViewModel", this);
                GeosApplication.Instance.Logger.Log("Constructor CrmMainViewModel ...", category: Category.Info, priority: Priority.Low);
              
                FillCommonDetails();
                await FillPeopleList();
                TileCollection = new ObservableCollection<TileBarItemsHelper>();
                if (GeosApplication.Instance.IsCommercialUser != true)
                {

                    TileBarItemsHelper tileBarItemsHelper = new TileBarItemsHelper();
                    tileBarItemsHelper.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard").ToString();
                    tileBarItemsHelper.BackColor = "#00879C";
                    tileBarItemsHelper.GlyphUri = "Dashboard.png";
                    tileBarItemsHelper.Visibility = Visibility.Visible;
                    tileBarItemsHelper.Children = new ObservableCollection<TileBarItemsHelper>();

                    //Dashboard 1 - Annual Sales.
                    TileBarItemsHelper tileBarItemDashboard1 = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard1").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "b_graph_dashboard1.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenAnnualSalesPerformanceView(), null, this); })
                    };
                    tileBarItemsHelper.Children.Add(tileBarItemDashboard1);

                    //Dashboard 2 - Sales.
                    TileBarItemsHelper tileBarItemDashboard2 = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard2").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "bDashboard2bar-chart.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenDashboardSaleView(), null, this); })
                    };
                    tileBarItemsHelper.Children.Add(tileBarItemDashboard2);

                    //Dashboard 3 - Performance
                    TileBarItemsHelper tileBarItemDashboard3 = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard3").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "bDashboard_monitor.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenDashboardPerformanceView(), null, this); })
                    };
                    tileBarItemsHelper.Children.Add(tileBarItemDashboard3);

                    // Dashboard Operations
                    TileBarItemsHelper tileBarItemDashboardOperations = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardOperations").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "bDashboardOperations.png",
                        Visibility = Visibility.Collapsed,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenDashBoardOperationsView(), null, this); })
                    };

                    //Dashboard Engineering Analysis
                    TileBarItemsHelper tileBarItemDashboardEngineeringAnalysis = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardEngineeringAnalysis").ToString(),
                        BackColor = "#00879C",
                        GlyphUri = "bDashboardEnggAnalysis.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenDashboardEngineeringAnalysisView(), null, this); })
                    };

                    //Dashboard - Opearations & Engineering Analysis.
                    if (GeosApplication.Instance.IdUserPermission == 22)
                    {
                        tileBarItemDashboardOperations.Visibility = Visibility.Visible;
                        // tileBarItemDashboardEngineeringAnalysis.Visibility = Visibility.Visible;
                    }

                    tileBarItemsHelper.Children.Add(tileBarItemDashboardOperations);
                    tileBarItemsHelper.Children.Add(tileBarItemDashboardEngineeringAnalysis);

                    TileCollection.Add(tileBarItemsHelper);
                }


                TileCollection.Add(new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelActivities").ToString(),
                    BackColor = "#5C5C5C",
                    GlyphUri = "Activities.png",
                    Visibility = Visibility.Visible,
                    //NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenActivityView(), null, this); })

                    Children = new ObservableCollection<TileBarItemsHelper>()
                    {
                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelActivitiesPage").ToString(),
                            BackColor = "#5C5C5C",
                            GlyphUri = "bActivities.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenActivityView(), null, this); })
                        },
                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelActions").ToString(),
                            BackColor = "#5C5C5C",
                            GlyphUri = "bActionsPlan.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenActionsView(), null, this); })
                        },
                    }
                });

                TileCollection.Add(new TileBarItemsHelper()
                {
                    Caption = "CLIENTS",
                    BackColor = "#D17800",
                    GlyphUri = "Clients.png",
                    Visibility = Visibility.Visible,

                    Children = new ObservableCollection<TileBarItemsHelper>()
                    {
                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelAccounts").ToString(),
                            BackColor = "#D17800",
                            GlyphUri = "Account.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenAccountView(), null, this); })
                        },
                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelContacts").ToString(),
                            BackColor = "#D17800",
                            GlyphUri = "Contacts.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenContactView(), null, this); })
                        },
                    }
                });

                TileCollection.Add(new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelLeadOffers").ToString(),
                    BackColor = "#427940",
                    GlyphUri = "Leads.png",
                    Visibility = Visibility.Visible,

                    Children = new ObservableCollection<TileBarItemsHelper>()
                    {
                        new TileBarItemsHelper(){
                            Caption =  System.Windows.Application.Current.FindResource("CrmMainViewModelTimeline").ToString(),
                            BackColor = "#427940",
                            GlyphUri = "bTimeline24x24.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenLeadView(), null, this);})
                        },
                        new TileBarItemsHelper(){
                            Caption =  System.Windows.Application.Current.FindResource("CrmMainViewModelPipeline").ToString(),
                            BackColor = "#427940",
                            GlyphUri = "bPipeline24x24.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenLeadsPipelineView(), null, this); })
                        },
                    }
                });

                TileCollection.Add(new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelOrders").ToString(),
                    BackColor = "#27B568",
                    GlyphUri = "Order.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenOrderView(), null, this); })
                });

                TileCollection.Add(new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelReports").ToString(),
                    BackColor = "#A84FA9",
                    GlyphUri = "Reports.png",
                    Visibility = Visibility.Visible,

                    Children = new ObservableCollection<TileBarItemsHelper>()
                    {
                        new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("ItemsForecastHeader").ToString(),
                            BackColor = "#A84FA9",
                            GlyphUri = "bItemForcast.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenItemForecastReport(), null, this);})
                        },

                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("ActivityReportHeader").ToString(),
                            BackColor = "#A84FA9",
                            GlyphUri = "Activityreport.png",
                            Visibility = Visibility.Visible,
                             NavigateCommand = new DelegateCommand<object>(OpenActivityReport)

                        },

                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("ArticlesReportHeader").ToString(),
                            BackColor = "#A84FA9",
                            GlyphUri = "bArticlesReport.png",
                            Visibility = Visibility.Visible,
                             NavigateCommand = new DelegateCommand<object>(OpenArticlesReport)
                        },

                        new TileBarItemsHelper(){
                            Caption = System.Windows.Application.Current.FindResource("ModulesReportHeader").ToString(),
                            BackColor = "#A84FA9",
                            GlyphUri = "bModulesReport.png",
                            Visibility = Visibility.Visible,
                             NavigateCommand = new DelegateCommand<object>(OpenModulesReport)
                        },
                    }
                });


                TileBarItemsHelper ConfigurationTileBarItemsHelper = new TileBarItemsHelper();
                ConfigurationTileBarItemsHelper.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelConfigurations").ToString();
                ConfigurationTileBarItemsHelper.BackColor = "#C7BFE6";
                ConfigurationTileBarItemsHelper.GlyphUri = "Configuration.png";
                ConfigurationTileBarItemsHelper.Visibility = Visibility.Visible;
                ConfigurationTileBarItemsHelper.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper ExchangeTileBarItemsHelper = new TileBarItemsHelper();
                ExchangeTileBarItemsHelper.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelExchangeRateView").ToString();
                ExchangeTileBarItemsHelper.BackColor = "#00BFFF";
                ExchangeTileBarItemsHelper.GlyphUri = "Exchange_Black.png";
                ExchangeTileBarItemsHelper.Visibility = Visibility.Visible;
                ExchangeTileBarItemsHelper.NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenExchangeRateView(), null, this); });
                ConfigurationTileBarItemsHelper.Children.Add(ExchangeTileBarItemsHelper);

                TileBarItemsHelper TargetForecastTileBarItemsHelper = new TileBarItemsHelper();
                TargetForecastTileBarItemsHelper.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelTargetForecast").ToString();
                TargetForecastTileBarItemsHelper.BackColor = "#00BFFF";
                TargetForecastTileBarItemsHelper.GlyphUri = "TargerAndForecast_Black.png";
                TargetForecastTileBarItemsHelper.Visibility = Visibility.Visible;
                TargetForecastTileBarItemsHelper.NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenTargetAndForecastView(), null, this); });
                ConfigurationTileBarItemsHelper.Children.Add(TargetForecastTileBarItemsHelper);

                TileBarItemsHelper PlantQuotaTileBarItemsHelper = new TileBarItemsHelper();
                PlantQuotaTileBarItemsHelper.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelPlantQuota").ToString();
                PlantQuotaTileBarItemsHelper.BackColor = "#00BFFF";
                PlantQuotaTileBarItemsHelper.GlyphUri = "bPlantNew.png";
                PlantQuotaTileBarItemsHelper.Visibility = Visibility.Visible;
                PlantQuotaTileBarItemsHelper.NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenPlantQuotaView(), null, this); });
                ConfigurationTileBarItemsHelper.Children.Add(PlantQuotaTileBarItemsHelper);

                TileBarItemsHelper TileBarItemsHelperUsers = new TileBarItemsHelper();
                TileBarItemsHelperUsers.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelUsers").ToString();
                TileBarItemsHelperUsers.BackColor = "#00BFFF";
                TileBarItemsHelperUsers.GlyphUri = "users.png";
                TileBarItemsHelperUsers.Visibility = Visibility.Visible;
                TileBarItemsHelperUsers.NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenUsersView(), null, this); });
                ConfigurationTileBarItemsHelper.Children.Add(TileBarItemsHelperUsers);


                //GEOS2 - 222
                //if (GeosApplication.Instance.IsPermissionAuditor)
                //{
                TileBarItemsHelper TileBarItemsHelperRecurrentActivities = new TileBarItemsHelper();
                TileBarItemsHelperRecurrentActivities.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelRecurrentActivities").ToString();
                TileBarItemsHelperRecurrentActivities.BackColor = "#00BFFF";
                TileBarItemsHelperRecurrentActivities.GlyphUri = "bRecurrentActivities.png";
                TileBarItemsHelperRecurrentActivities.NavigateCommand = new DelegateCommand(OpenRecurrentActivitiesView);
                ConfigurationTileBarItemsHelper.Children.Add(TileBarItemsHelperRecurrentActivities);
                //}

                TileBarItemsHelper TileBarItemsHelperMyPreferences = new TileBarItemsHelper();
                TileBarItemsHelperMyPreferences.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelMyPreferences").ToString();
                TileBarItemsHelperMyPreferences.BackColor = "#00BFFF";
                TileBarItemsHelperMyPreferences.GlyphUri = "MyPreference_Black.png";
                TileBarItemsHelperMyPreferences.Visibility = Visibility.Visible;
                TileBarItemsHelperMyPreferences.NavigateCommand = new DelegateCommand<object>(OpenMyPreference);
                ConfigurationTileBarItemsHelper.Children.Add(TileBarItemsHelperMyPreferences);

                TileCollection.Add(ConfigurationTileBarItemsHelper);

                if (TileCollection.Count >= 6)
                    if (GeosApplication.Instance.IsPermissionAdminOnly)
                    {
                        TileCollection[6].Children[3].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        TileCollection[6].Children[3].Visibility = Visibility.Collapsed;
                    }
                GeosApplication.Instance.Logger.Log("Constructor CrmMainViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CrmMainViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.IsLoadOneTime = false;

            FillSectionsDeatils();
            if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
            {
                int value;
                if (int.TryParse(GeosApplication.Instance.UserSettings["LoadDataOn"].ToString(), out value))
                    if (Convert.ToInt32(GeosApplication.Instance.UserSettings["LoadDataOn"].ToString()) == 1)
                        FillAllObjectsOneTime();

            }
        }


        /// <summary>
        ///  This method is for to convert Image to ImageSource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource ConvertImageToImageSource(string path)
        {
            BitmapImage biImg = new BitmapImage(new Uri(path, UriKind.Relative));
            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }


        /// <summary>
        /// Method for fill list of all sections of CRM module.
        /// </summary>
        private void FillSectionsDeatils()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils ...", category: Category.Info, priority: Priority.Low);

                List<CRMSections> crmList = new List<CRMSections>();

                crmList.Add(new CRMSections() { IdSection = 1, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard1").ToString() });
                crmList.Add(new CRMSections() { IdSection = 2, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard2").ToString() });
                crmList.Add(new CRMSections() { IdSection = 3, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboard3").ToString() });

                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    crmList.Add(new CRMSections() { IdSection = 4, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardOperations").ToString() });

                }
                crmList.Add(new CRMSections() { IdSection = 5, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelDashboardEngineeringAnalysis").ToString() });            
                crmList.Add(new CRMSections() { IdSection = 6, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelActivities").ToString() });
                //crmList.Add(new CRMSections() { IdSection = 6, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelActivitiesPage").ToString() });

                crmList.Add(new CRMSections() { IdSection = 7, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelAccounts").ToString() });
                crmList.Add(new CRMSections() { IdSection = 8, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelContacts").ToString() });
                crmList.Add(new CRMSections() { IdSection = 9, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelTimeline").ToString() });

                crmList.Add(new CRMSections() { IdSection = 10, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelPipeline").ToString() });
                crmList.Add(new CRMSections() { IdSection = 11, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelOrders").ToString() });
                crmList.Add(new CRMSections() { IdSection = 12, SectionName = System.Windows.Application.Current.FindResource("ItemsForecastHeader").ToString() });
                // crmList.Add(new CRMSections() { IdSection = 13, SectionName = System.Windows.Application.Current.FindResource("ReportDashboardHeader").ToString() });

                crmList.Add(new CRMSections() { IdSection = 15, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelTargetForecast").ToString() });
                crmList.Add(new CRMSections() { IdSection = 16, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelPlantQuota").ToString() });
                crmList.Add(new CRMSections() { IdSection = 17, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelUsers").ToString() });
              
                //  crmList.Add(new CRMSections() { IdSection = 18, SectionName = System.Windows.Application.Current.FindResource("CrmMainViewModelActions").ToString() });


                GeosApplication.Instance.CrmsectionsList = crmList;

                string[] CrmSelectedStr = GeosApplication.Instance.UserSettings["SelectedCRMSectionLoadData"].Split(',');
                GeosApplication.Instance.SelectedCrmsectionsList = new List<CRMSections>();
                if (CrmSelectedStr != null && CrmSelectedStr[0] != string.Empty)
                {
                    foreach (var item in CrmSelectedStr)
                    {

                        GeosApplication.Instance.SelectedCrmsectionsList.Add(GeosApplication.Instance.CrmsectionsList.FirstOrDefault(crm => crm.IdSection == Convert.ToInt16(item.ToString())));
                    }

                }
                else
                {
                    GeosApplication.Instance.SelectedCrmsectionsList = GeosApplication.Instance.CrmsectionsList.ToList();
                }

                GeosApplication.Instance.Logger.Log("Method FillAllObjectsOneTime() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSectionsDeatils() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for load all crm mdule data one time.
        /// </summary>
        private void FillAllObjectsOneTime()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllObjectsOneTime ...", category: Category.Info, priority: Priority.Low);

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

                GeosApplication.Instance.IsLoadOneTime = true;

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    GeosApplication.Instance.SelectedSalesOwnerUsersList.Clear();
                    GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                }

                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList.Clear();
                    GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                }

                var cts = new CancellationTokenSource();
                List<Task> allTasks = new List<Task>();
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 1))
                {
                    var task0 = Task.Factory.StartNew(() => { ObjAnnualSalesPerformanceViewModel = new AnnualSalesPerformanceViewModel(); }, cts.Token, TaskCreationOptions.None, scheduler);
                    allTasks.Add(task0);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 2))
                {
                    var task1 = Task.Factory.StartNew(() => { ObjDashboardSaleViewModel = new DashboardSaleViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task1);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 3))
                {
                    var task2 = Task.Factory.StartNew(() => { ObjDahsBoardPerformanceViewModel = new DahsBoardPerformanceViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task2);
                }

                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 4))
                    {
                        var task3 = Task.Factory.StartNew(() => { ObjDashBoardOperationsViewModel = new DashBoardOperationsViewModel(); }, TaskCreationOptions.AttachedToParent);
                        allTasks.Add(task3);
                    }


                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 5))
                {
                    var task4 = Task.Factory.StartNew(() => { ObjDashboardEngineeringAnalysisViewModel = new DashboardEngineeringAnalysisViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task4);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 6))
                {
                    var task5 = Task.Factory.StartNew(() => { ObjActivityViewModel = new ActivityViewModel(); }, cts.Token, TaskCreationOptions.None, scheduler);
                    allTasks.Add(task5);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 7))
                {
                    var task6 = Task.Factory.StartNew(() => { ObjAccountViewModel = new AccountViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task6);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 8))
                {
                    var task7 = Task.Factory.StartNew(() => { ObjContactViewModel = new ContactViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task7);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 9))
                {
                    var task8 = Task.Factory.StartNew(() => { ObjLeadsViewModel = new LeadsViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task8);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 10))
                {
                    var task9 = Task.Factory.StartNew(() => { ObjLeadsPipelineViewModel = new LeadsPipelineViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task9);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 11))
                {
                    var task10 = Task.Factory.StartNew(() => { ObjOrderViewModel = new OrderViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task10);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 12))
                {
                    var task11 = Task.Factory.StartNew(() => { ObjItemForecastReportViewModel = new ItemForecastReportViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task11);
                }


                //if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 13))
                //{
                //    var task12 = Task.Factory.StartNew(() => { ObjReportDashboardViewModel = new ReportDashboardViewModel(); }, TaskCreationOptions.AttachedToParent);
                //    allTasks.Add(task12);
                //}


                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 14))
                {
                    var task13 = Task.Factory.StartNew(() => { ObjTargetAndForecastViewModel = new TargetAndForecastViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task13);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 15))
                {
                    var task14 = Task.Factory.StartNew(() => { ObjPlantQuotaViewModel = new PlantQuotaViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task14);
                }

                if (GeosApplication.Instance.SelectedCrmsectionsList.Exists(cm => cm.IdSection == 16))
                {
                    var task15 = Task.Factory.StartNew(() => { ObjUsersViewModel = new UsersViewModel(); }, TaskCreationOptions.AttachedToParent);
                    allTasks.Add(task15);
                }


                var AllTasks = allTasks.ToArray();
                Task.WaitAll(AllTasks);

                // var allTasks = new Task[] { task0, task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14 };

                GeosApplication.Instance.IsLoadOneTime = false;

                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Method FillAllObjectsOneTime() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllObjectsOneTime() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        //private void FillDetialInBackgound(object Sender, DoWorkEventArgs e)
        //{
        //    FillCommonDetails();
        //}
        private async Task FillCommonDetailsAsync()
        {

            await Task.Run(() =>
            {
                FillCommonDetails();
            });
        }
        private void SetDate()
        {
            TimeSpan timeSpanStart = new TimeSpan(0, 0, 0);
            TimeSpan timeSpanEnd = new TimeSpan(23, 59, 59);
            if (GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption") && GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("0"))
            {
                //GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToDateTime((Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1).ToString("yyyy/MM/dd")).Add(timeSpanStart);
                //  GeosApplication.Instance.SelectedyearStarDate = new DateTime(Convert.ToDateTime((Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1).ToString("yyyy/MM/dd"))).Add(timeSpanStart);
                DateTime Start = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1);
                Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                DateTime End = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 12, 31);
                End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));
                GeosApplication.Instance.SelectedyearStarDate = Start.Add(timeSpanStart);
                GeosApplication.Instance.SelectedyearEndDate = End.Add(timeSpanEnd);
            }
            else if (GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption") && GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("1"))
            {
                if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["CrmOfferFromInterval"]) || string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["CrmOfferToInterval"]))
                {
                    DateTime Start = new DateTime(Convert.ToInt32(DateTime.Now.Year), 1, 1);
                    Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                    DateTime End = new DateTime(Convert.ToInt32(DateTime.Now.Year), 12, 31);
                    End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));
                    GeosApplication.Instance.SelectedyearStarDate = Start.Add(timeSpanStart);
                    GeosApplication.Instance.SelectedyearEndDate = End.Add(timeSpanEnd);
                }
                else
                {
                    try
                    {
                        //DateTime Start = new DateTime(Convert.ToInt32(GeosApplication.Instance.UserSettings["CrmOfferFromInterval"]), 1, 1);
                        //Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                        //DateTime End = new DateTime(Convert.ToInt32(GeosApplication.Instance.UserSettings["CrmOfferToInterval"]), 12, 31);
                        //End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));

                        DateTime Start = Convert.ToDateTime(GeosApplication.Instance.UserSettings["CrmOfferFromInterval"]);
                        Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                        DateTime End = Convert.ToDateTime(GeosApplication.Instance.UserSettings["CrmOfferToInterval"]);
                        End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));
                        GeosApplication.Instance.SelectedyearStarDate = Start.Add(timeSpanStart);
                        GeosApplication.Instance.SelectedyearEndDate = End.Add(timeSpanEnd);
                    }
                    catch (Exception ex)
                    {

                        // throw;
                    }

                }

            }
        }
        public async Task FillPeopleList()
        {
            //Thread.Sleep(7000);
            if (GeosApplication.Instance.PeopleList == null)
            {
                GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
            }

        }
        /// <summary>
        /// Method for fill common company,People and financial dates.
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        private async void FillCommonDetails()
        {

            GeosApplication.Instance.Logger.Log("Method FillCommonDetails ...", category: Category.Info, priority: Priority.Low);
            //[001]Changed service method
             GeosApplication.Instance.CompanyList = CrmStartUp.GetAllCompaniesDetails_V2040(GeosApplication.Instance.ActiveUser.IdUser);
            //[001] Added
            if (GeosApplication.Instance.CompanyList.Any(i => i.ShortName == GeosApplication.Instance.SiteName))
                GeosApplication.Instance.ActiveIdSite = GeosApplication.Instance.CompanyList.Where(i => i.ShortName == GeosApplication.Instance.SiteName).FirstOrDefault().IdCompany;
          
           SetDate();
            //FillGroupList();
            //FillCompanyPlantList();


            //GeosApplication.Instance.RemainingDays = (Math.Round((GeosApplication.Instance.SelectedyearEndDate.Date.AddDays(+1) - GeosApplication.Instance.SelectedyearStarDate.Date).TotalDays, 0)).ToString();

            //[CRM-M044-10] Wrong remaining days Task By Amit
            int RemainDays = (int)(GeosApplication.Instance.SelectedyearEndDate.Date - DateTime.Now.Date).TotalDays;
            if (RemainDays <= 0)
            {
                RemainDays = 0;
            }
            GeosApplication.Instance.RemainingDays = RemainDays.ToString();

            GeosApplication.Instance.Logger.Log("Method FillCommonDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for open my preference form.
        /// </summary>
        /// <param name="obj"></param>
        private void OpenMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenMyPreference ...", category: Category.Info, priority: Priority.Low);
                //if (MultipleCellEditHelper.IsValueChanged)
                //{
                //    SavechangesLeadOrOrder();
                //}
                IsBusy = true;
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                IsBusy = false;
                myPreferencesView.ShowDialogWindow();

                //Dashboard - Opearations & Engineering Analysis. Visibility after changed 
                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    TileCollection[0].Children[3].Visibility = Visibility.Visible;
                    //TileCollection[0].Children[4].Visibility = Visibility.Visible;
                }
                else
                {
                    TileCollection[0].Children[3].Visibility = Visibility.Collapsed;
                    //TileCollection[0].Children[4].Visibility = Visibility.Collapsed;
                }

                GeosApplication.Instance.Logger.Log("Method OpenMyPreference() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenMyPreference() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for open my preference form.
        /// </summary>
        /// <param name="obj"></param>
        private void OpenActivityReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenActivityReport ...", category: Category.Info, priority: Priority.Low);

                if (ActionPlanMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInActionGrid();
                }

                if (MultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesLeadOrOrder();
                }
                IsBusy = true;
                ActivityReportViewModel activityReportViewModel = new ActivityReportViewModel();
                ActivityReportView activityReportView = new ActivityReportView();
                EventHandler handle = delegate { activityReportView.Close(); };
                activityReportViewModel.RequestClose += handle;
                activityReportView.DataContext = activityReportViewModel;
                IsBusy = false;
                activityReportView.ShowDialogWindow();

                //Dashboard - Opearations. Visibility after changed 
                //if (GeosApplication.Instance.IdUserPermission == 22)
                //{
                //    TileCollection[0].Children[3].Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    TileCollection[0].Children[3].Visibility = Visibility.Collapsed;
                //}

                GeosApplication.Instance.Logger.Log("Method OpenActivityReport() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenActivityReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenArticlesReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenArticlesReport ...", category: Category.Info, priority: Priority.Low);

                if (ActionPlanMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInActionGrid();
                }

                if (MultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesLeadOrOrder();
                }

                IsBusy = true;
                ArticlesReportViewModel articlesReportViewModel = new ArticlesReportViewModel();
                ArticlesReportView articlesReportView = new ArticlesReportView();
                EventHandler handle = delegate { articlesReportView.Close(); };
                articlesReportViewModel.RequestClose += handle;
                articlesReportView.DataContext = articlesReportViewModel;
                IsBusy = false;
                articlesReportView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method OpenArticlesReport() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenArticlesReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenModulesReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenModulesReport ...", category: Category.Info, priority: Priority.Low);

                if (ActionPlanMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInActionGrid();
                }

                if (MultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesLeadOrOrder();
                }

                IsBusy = true;
                ModulesReportViewModel modulesReportViewModel = new ModulesReportViewModel();
                ModulesReportView modulesReportView = new ModulesReportView();
                EventHandler handle = delegate { modulesReportView.Close(); };
                modulesReportViewModel.RequestClose += handle;
                modulesReportView.DataContext = modulesReportViewModel;
                IsBusy = false;
                modulesReportView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method OpenModulesReport() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenModulesReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        ///Method for open ActivityView. 
        /// </summary>
        /// <param name="obj"></param>
        private ActivityView OpenActivityView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenActivityView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            ActivityView activityView = new ActivityView();
            try
            {
                if (!DXSplashScreen.IsActive)
                {
                    //  DXSplashScreen.Show<SplashScreenView>(); 
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

                if (ObjActivityViewModel == null)
                {
                    ObjActivityViewModel = new ActivityViewModel();
                    //GeosApplication.Instance.ObjectPool.Add("ActivityViewModel", objContactViewModel);
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjActivityViewModel = new ActivityViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            if (IsChangedCombobox(ObjActivityViewModel.PreviouslySelectedSalesOwners, ObjActivityViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjActivityViewModel = new ActivityViewModel();
                            }
                            else
                            {
                                ObjActivityViewModel.ActivityFilterCriteria = null;
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                activityView.DataContext = ObjActivityViewModel;
                GeosApplication.Instance.Logger.Log("Method OpenActivityView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenActivityView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return activityView;
        }


        /// <summary>
        ///Method for open LeadView. 
        /// </summary>
        /// <param name="obj"></param>
        private LeadsView OpenLeadView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenLeadView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            LeadsView leadsView = new LeadsView();
            try
            {
                if (!DXSplashScreen.IsActive)
                {
                    //DXSplashScreen.Show<SplashScreenView>(); 
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

                if (ObjLeadsViewModel == null)
                {
                    ObjLeadsViewModel = new LeadsViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjLeadsViewModel = new LeadsViewModel();

                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjLeadsViewModel.IsShowFailedPlantWarning = false;
                            if (IsChangedCombobox(ObjLeadsViewModel.PreviouslySelectedSalesOwners, ObjLeadsViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjLeadsViewModel = new LeadsViewModel();

                            }
                            ObjLeadsViewModel.StartSelectionDate = DateTime.MinValue;
                            ObjLeadsViewModel.FinishSelectionDate = DateTime.MinValue;
                        }

                    }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                leadsView.DataContext = ObjLeadsViewModel;
                GeosApplication.Instance.Logger.Log("Method OpenLeadView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenLeadView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return leadsView;
        }

        /// <summary>
        ///Method for open Pipeline. 
        /// </summary>
        /// <param name="obj"></param>
        private LeadsPipelineView OpenLeadsPipelineView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenLeadsPipelineView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            LeadsPipelineView leadsPipelineView = new LeadsPipelineView();
            try
            {
                if (!DXSplashScreen.IsActive)
                {
                    //DXSplashScreen.Show<SplashScreenView>(); 
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

                if (ObjLeadsPipelineViewModel == null)
                {
                    ObjLeadsPipelineViewModel = new LeadsPipelineViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjLeadsPipelineViewModel = new LeadsPipelineViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjLeadsPipelineViewModel.IsShowFailedPlantWarning = false;
                            if (IsChangedCombobox(ObjLeadsPipelineViewModel.PreviouslySelectedSalesOwners, ObjLeadsPipelineViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjLeadsPipelineViewModel = new LeadsPipelineViewModel();
                            }
                            else
                            {
                                ObjLeadsPipelineViewModel.PipelineFilterCriteria = null;
                                ObjLeadsPipelineViewModel.CreateSeperateLists();
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                leadsPipelineView.DataContext = ObjLeadsPipelineViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenLeadsPipelineView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenLeadsPipelineView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return leadsPipelineView;
        }


        /// <summary>
        ///Method for open OrderView. 
        /// </summary>
        /// <param name="obj"></param>
        private OrderView OpenOrderView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenOrderView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            OrderView orderView = new OrderView();
            try
            {
                if (!DXSplashScreen.IsActive)
                {
                    //  DXSplashScreen.Show<SplashScreenView>(); 
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

                if (ObjOrderViewModel == null)
                {
                    ObjOrderViewModel = new OrderViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjOrderViewModel = new OrderViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjOrderViewModel.IsShowFailedPlantWarning = false;
                            if (IsChangedCombobox(ObjOrderViewModel.PreviouslySelectedSalesOwners, ObjOrderViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjOrderViewModel = new OrderViewModel();
                                ObjOrderViewModel.StartSelectionDate = DateTime.MinValue;
                                ObjOrderViewModel.FinishSelectionDate = DateTime.MinValue;
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                orderView.DataContext = ObjOrderViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenOrderView() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenOrderView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return orderView;
        }

        /// <summary>
        ///Method for open AccountView. 
        /// </summary>
        /// <param name="obj"></param>
        private AccountView OpenAccountView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenAccountView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            AccountView accountView = new AccountView();
            try
            {
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

                if (ObjAccountViewModel == null)
                {
                    ObjAccountViewModel = new AccountViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjAccountViewModel = new AccountViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            if (IsChangedCombobox(ObjAccountViewModel.PreviouslySelectedSalesOwners, ObjAccountViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjAccountViewModel = new AccountViewModel();
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                accountView.DataContext = ObjAccountViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenAccountView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenAccountView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return accountView;
        }

        /// <summary>
        ///Method for open OpenActionView. 
        /// </summary>
        /// <param name="obj"></param>
        private ActionsView OpenActionsView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenActionsView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            ActionsView actionsView = new ActionsView();
            try
            {
                if (!DXSplashScreen.IsActive)
                {
                    //  DXSplashScreen.Show<SplashScreenView>(); 
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

                if (ObjActionsViewModel == null)
                {
                    ObjActionsViewModel = new ActionsViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjActionsViewModel = new ActionsViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            if (IsChangedCombobox(ObjActionsViewModel.PreviouslySelectedSalesOwners, ObjActionsViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjActionsViewModel = new ActionsViewModel();
                            }
                        }
                    }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                actionsView.DataContext = ObjActionsViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenActionsView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenActionsView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return actionsView;
        }

        /// <summary>
        ///Method for open ContactView. 
        /// </summary>
        /// <param name="obj"></param>
        private ContactView OpenContactView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenContactView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            ContactView contactView = new ContactView();
            try
            {
                if (!DXSplashScreen.IsActive)
                {
                    //  DXSplashScreen.Show<SplashScreenView>(); 
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

                if (ObjContactViewModel == null)
                {
                    ObjContactViewModel = new ContactViewModel();
                    // GeosApplication.Instance.ObjectPool.Add("ContactViewModel", objContactViewModel);
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjContactViewModel = new ContactViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            if (IsChangedCombobox(ObjContactViewModel.PreviouslySelectedSalesOwners, ObjContactViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjContactViewModel = new ContactViewModel();
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                contactView.DataContext = ObjContactViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenContactView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenContactView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return contactView;
        }

        /// <summary>
        ///Method for open AnnualSalesPerformanceView. 
        /// </summary>
        /// <param name="obj"></param>
        /// 

        private AnnualSalesPerformanceView OpenAnnualSalesPerformanceView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenAnnualSalesPerformanceView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }

            AnnualSalesPerformanceView annualSalesPerformanceView = new AnnualSalesPerformanceView();

            try
            {
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

                if (ObjAnnualSalesPerformanceViewModel == null)
                {
                    ObjAnnualSalesPerformanceViewModel = new AnnualSalesPerformanceViewModel();
                    // GeosApplication.Instance.ObjectPool.Add("ContactViewModel", objContactViewModel);
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjAnnualSalesPerformanceViewModel = new AnnualSalesPerformanceViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjAnnualSalesPerformanceViewModel.IsShowFailedPlantWarning = false;

                            if (IsChangedCombobox(ObjAnnualSalesPerformanceViewModel.PreviouslySelectedSalesOwners, ObjAnnualSalesPerformanceViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjAnnualSalesPerformanceViewModel = new AnnualSalesPerformanceViewModel();
                            }
                        }
                    }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                annualSalesPerformanceView.DataContext = ObjAnnualSalesPerformanceViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenAnnualSalesPerformanceView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenAnnualSalesPerformanceView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return annualSalesPerformanceView;
        }

        /// <summary>
        ///Method for open DashboardSaleView. 
        /// </summary>
        /// <param name="obj"></param>
        private DashboardSale OpenDashboardSaleView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenDashboardSaleView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            DashboardSale dashboardSaleView = new DashboardSale();
            try
            {
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

                if (ObjDashboardSaleViewModel == null)
                {
                    ObjDashboardSaleViewModel = new DashboardSaleViewModel();
                    // GeosApplication.Instance.ObjectPool.Add("ContactViewModel", objContactViewModel);
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjDashboardSaleViewModel = new DashboardSaleViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjDashboardSaleViewModel.IsShowFailedPlantWarning = false;
                            if (IsChangedCombobox(ObjDashboardSaleViewModel.PreviouslySelectedSalesOwners, ObjDashboardSaleViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjDashboardSaleViewModel = new DashboardSaleViewModel();
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                dashboardSaleView.DataContext = ObjDashboardSaleViewModel;
                GeosApplication.Instance.Logger.Log("Method OpenDashboardSaleView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenDashboardSaleView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return dashboardSaleView;
        }


        /// <summary>
        ///Method for open DashboardPerformanceView. 
        /// </summary>
        /// <param name="obj"></param>
        private DashboardPerformanceView OpenDashboardPerformanceView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenDashboardPerformanceView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            DashboardPerformanceView dashboardPerformanceView = new DashboardPerformanceView();
            try
            {
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

                if (ObjDahsBoardPerformanceViewModel == null)
                {
                    ObjDahsBoardPerformanceViewModel = new DahsBoardPerformanceViewModel();
                    // GeosApplication.Instance.ObjectPool.Add("ContactViewModel", objContactViewModel);
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjDahsBoardPerformanceViewModel = new DahsBoardPerformanceViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjDahsBoardPerformanceViewModel.IsShowFailedPlantWarning = false;
                            if (IsChangedCombobox(ObjDahsBoardPerformanceViewModel.PreviouslySelectedSalesOwners, ObjDahsBoardPerformanceViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjDahsBoardPerformanceViewModel = new DahsBoardPerformanceViewModel();
                            }
                        }
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                dashboardPerformanceView.DataContext = ObjDahsBoardPerformanceViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenDashboardPerformanceView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenDashboardPerformanceView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return dashboardPerformanceView;
        }

        /// <summary>
        ///Method for open DashboardPerformanceView. 
        /// </summary>
        /// <param name="obj"></param>
        private DashBoardOperationsView OpenDashBoardOperationsView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenDashBoardOperationsView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            DashBoardOperationsView dashBoardOperationsView = new DashBoardOperationsView();
            try
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

                if (ObjDashBoardOperationsViewModel == null)
                {
                    ObjDashBoardOperationsViewModel = new DashBoardOperationsViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjDashBoardOperationsViewModel = new DashBoardOperationsViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjDashBoardOperationsViewModel.IsShowFailedPlantWarning = false;

                            if (IsChangedCombobox(null, ObjDashBoardOperationsViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjDashBoardOperationsViewModel = new DashBoardOperationsViewModel();
                            }
                        }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                dashBoardOperationsView.DataContext = ObjDashBoardOperationsViewModel;
                GeosApplication.Instance.Logger.Log("Method OpenDashBoardOperationsView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenDashBoardOperationsView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return dashBoardOperationsView;
        }

        /// <summary>
        ///Method for open DashboardPerformanceView. 
        /// </summary>
        /// <param name="obj"></param>
        private DashboardEngineeringAnalysisView OpenDashboardEngineeringAnalysisView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenDashboardEngineeringAnalysisView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }

            DashboardEngineeringAnalysisView dashboardEngineeringAnalysisView = new DashboardEngineeringAnalysisView();
            try
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

                if (ObjDashboardEngineeringAnalysisViewModel == null)
                {
                    ObjDashboardEngineeringAnalysisViewModel = new DashboardEngineeringAnalysisViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjDashboardEngineeringAnalysisViewModel = new DashboardEngineeringAnalysisViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjDashboardEngineeringAnalysisViewModel.IsShowFailedPlantWarning = false;

                            if (IsChangedCombobox(null, ObjDashboardEngineeringAnalysisViewModel.PreviouslySelectedPlantOwners))
                            {
                                objDashboardEngineeringAnalysisViewModel = new DashboardEngineeringAnalysisViewModel();
                            }
                        }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                dashboardEngineeringAnalysisView.DataContext = ObjDashboardEngineeringAnalysisViewModel;
                GeosApplication.Instance.Logger.Log("Method OpenDashboardEngineeringAnalysisView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenDashboardEngineeringAnalysisView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return dashboardEngineeringAnalysisView;
        }

        /// <summary>
        ///Method for open ReportDashboard2. 
        /// </summary>
        /// <param name="obj"></param>
        private ReportDashboard2 OpenReportDashboard2()
        {
            //    GeosApplication.Instance.Logger.Log("Method OpenReportDashboard2 ...", category: Category.Info, priority: Priority.Low);
            //    if (MultipleCellEditHelper.IsValueChanged)
            //    {
            //        SavechangesLeadOrOrder();
            //    }
            //    ReportDashboard2 reportDashboard2 = new ReportDashboard2();
            //    try
            //    {
            //        if (!DXSplashScreen.IsActive)
            //        {
            //            //  DXSplashScreen.Show<SplashScreenView>(); 
            //            DXSplashScreen.Show(x =>
            //            {
            //                Window win = new Window()
            //                {
            //                    ShowActivated = false,
            //                    WindowStyle = WindowStyle.None,
            //                    ResizeMode = ResizeMode.NoResize,
            //                    AllowsTransparency = true,
            //                    Background = new SolidColorBrush(Colors.Transparent),
            //                    ShowInTaskbar = false,
            //                    Topmost = true,
            //                    SizeToContent = SizeToContent.WidthAndHeight,
            //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //                };
            //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
            //                win.Topmost = false;
            //                return win;
            //            }, x =>
            //            {
            //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
            //            }, null, null);
            //        }

            //        if (ObjReportDashboardViewModel == null)
            //        {
            //            ObjReportDashboardViewModel = new ReportDashboardViewModel();
            //        }
            //        else
            //        {
            //            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
            //            {
            //                if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
            //                {
            //                    ObjReportDashboardViewModel = new ReportDashboardViewModel();
            //                }
            //                else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
            //                {
            //                    ObjReportDashboardViewModel.IsShowFailedPlantWarning = false;
            //                    if (IsChangedCombobox(ObjReportDashboardViewModel.PreviouslySelectedSalesOwners, ObjReportDashboardViewModel.PreviouslySelectedPlantOwners))
            //                    {
            //                        ObjReportDashboardViewModel = new ReportDashboardViewModel();
            //                    }
            //                }
            //            }
            //        }
            //        GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            //        GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            //        reportDashboard2.DataContext = ObjReportDashboardViewModel;
            //        GeosApplication.Instance.Logger.Log("Method OpenReportDashboard2() executed successfully", category: Category.Info, priority: Priority.Low);
            //    }
            //    catch (Exception ex)
            //    {
            //        IsBusy = false;
            //        GeosApplication.Instance.Logger.Log("Get an error in OpenReportDashboard2() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            //    }

            //    if (DXSplashScreen.IsActive ) { DXSplashScreen.Close(); }

            //    return reportDashboard2;
            return null;
        }


        /// <summary>
        ///Method for open ItemForecastReport. 
        /// </summary>
        /// <param name="obj"></param>
        private ItemForecastReport OpenItemForecastReport()
        {
            GeosApplication.Instance.Logger.Log("Method OpenItemForecastReport ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            ItemForecastReport itemForecastReport = new ItemForecastReport();
            try
            {
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

                if (ObjItemForecastReportViewModel == null)
                {
                    ObjItemForecastReportViewModel = new ItemForecastReportViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjItemForecastReportViewModel = new ItemForecastReportViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            ObjItemForecastReportViewModel.IsShowFailedPlantWarning = false;
                            if (IsChangedCombobox(ObjItemForecastReportViewModel.PreviouslySelectedSalesOwners, ObjItemForecastReportViewModel.PreviouslySelectedPlantOwners))
                            {
                                ObjItemForecastReportViewModel = new ItemForecastReportViewModel();
                            }
                        }
                    }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                itemForecastReport.DataContext = ObjItemForecastReportViewModel;
                GeosApplication.Instance.Logger.Log("Method OpenItemForecastReport() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenItemForecastReport() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return itemForecastReport;
        }

        /// <summary>
        ///Method for open TargetAndForecastView. 
        /// </summary>
        /// <param name="obj"></param>
        private TargetAndForecastView OpenTargetAndForecastView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenTargetAndForecastView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            TargetAndForecastView targetAndForecastView = new TargetAndForecastView();
            try
            {
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

                if (ObjTargetAndForecastViewModel == null)
                {
                    ObjTargetAndForecastViewModel = new TargetAndForecastViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
                    {
                        if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                        {
                            ObjTargetAndForecastViewModel = new TargetAndForecastViewModel();
                        }
                        else if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
                        {
                            if (IsChangedCombobox(ObjTargetAndForecastViewModel.PreviouslySelectedSalesOwners, null))
                            {
                                ObjTargetAndForecastViewModel = new TargetAndForecastViewModel();
                            }
                        }
                    }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                targetAndForecastView.DataContext = ObjTargetAndForecastViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenTargetAndForecastView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenTargetAndForecastView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return targetAndForecastView;
        }

        /// <summary>
        ///Method for open UsersView. 
        /// </summary>
        /// <param name="obj"></param>
        private UsersView OpenUsersView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenUsersView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            UsersView usersView = new UsersView();
            try
            {
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

                if (ObjUsersViewModel == null)
                {
                    ObjUsersViewModel = new UsersViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh")
                        && GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                    {
                        ObjUsersViewModel = new UsersViewModel();
                    }
                }

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                usersView.DataContext = ObjUsersViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenUsersView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenUsersView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return usersView;
        }

        /// <summary>
        ///Method for open PlantQuotaView. 
        /// </summary>
        /// <param name="obj"></param>
        private PlantQuotaView OpenPlantQuotaView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenPlantQuotaView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            PlantQuotaView plantQuotaView = new PlantQuotaView();
            try
            {
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

                if (ObjPlantQuotaViewModel == null)
                {
                    ObjPlantQuotaViewModel = new PlantQuotaViewModel();
                }
                else
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh")
                        && GeosApplication.Instance.UserSettings["AutoRefresh"] == "Yes")
                    {
                        ObjPlantQuotaViewModel = new PlantQuotaViewModel();
                    }
                }
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                plantQuotaView.DataContext = ObjPlantQuotaViewModel;

                GeosApplication.Instance.Logger.Log("Method OpenPlantQuotaView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenPlantQuotaView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            return plantQuotaView;
        }

        private ExchangeRateView OpenExchangeRateView()
        {
            GeosApplication.Instance.Logger.Log("Method OpenTargetAndForecastView ...", category: Category.Info, priority: Priority.Low);

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                SavechangesInActionGrid();
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                SavechangesLeadOrOrder();
            }
            ExchangeRateView exchangeRateView = new ExchangeRateView();
            ExchangeRateViewModel exchangeRateViewModel = new ExchangeRateViewModel();
            exchangeRateView.DataContext = exchangeRateViewModel;
            return exchangeRateView;
        }

        /// <summary>
        /// This method is used to compare PreviouslySelectedSalesOwners/PreviouslySelectedPlantOwners and
        /// Currently PreviouslySelectedSalesOwners/PreviouslySelectedPlantOwners, depend on this load data.
        /// </summary>
        /// <param name="previouslySelectedSalesOwners"></param>
        /// <param name="previouslySelectedPlantOwners"></param>
        /// <returns></returns>
        private bool IsChangedCombobox(string previouslySelectedSalesOwners, string previouslySelectedPlantOwners)
        {
            GeosApplication.Instance.Logger.Log("Method IsChangedCombobox ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().OrderBy(x => x.IdUser).ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                    //string[] newSelected = salesOwnersIds.Split(',');
                    int[] newSelected = Array.ConvertAll(salesOwnersIds.Split(','), s => int.Parse(s));
                    Array.Sort(newSelected, StringComparer.InvariantCulture);

                    int[] previous = previouslySelectedSalesOwners.Split(',').Select(int.Parse).ToArray();
                    Array.Sort(previous, StringComparer.InvariantCulture);

                    var result1 = string.Join(",", newSelected);
                    var result2 = string.Join(",", previous);

                    if (!result1.Equals(result2))
                    {
                        return true;
                    }
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().OrderBy(i => i.ConnectPlantId).ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    //string[] newSelected = plantOwnersIds.Split(',');

                    int[] newSelected = Array.ConvertAll(plantOwnersIds.Split(','), s => int.Parse(s));
                    Array.Sort(newSelected, StringComparer.InvariantCulture);

                    int[] previous = previouslySelectedPlantOwners.Split(',').Select(int.Parse).ToArray();
                    Array.Sort(previous, StringComparer.InvariantCulture);

                    var result1 = string.Join(",", newSelected);
                    var result2 = string.Join(",", previous);

                    if (!result1.Equals(result2))
                    {
                        return true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method IsChangedCombobox() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in IsChangedCombobox() CrmMainViewModel." + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return false;
        }

        public void SavechangesInActionGrid()
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["OteditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                //if (ObjActionsViewModel == null)
                //{
                //    ObjActionsViewModel = new ActionsViewModel();
                //    ObjActionsViewModel.ActionPlanItemList = GeosApplication.Instance.ActionPlanItemList;
                //}
                if (ActionPlanMultipleCellEditHelper.Checkview == "ActionsTableView")
                {
                    ObjActionsViewModel.UpdateMultipleRowsActionsGridCommandAction(ActionPlanMultipleCellEditHelper.Viewtableview);
                }
            }
            ActionPlanMultipleCellEditHelper.IsValueChanged = false;
        }

        public void SavechangesLeadOrOrder()
        {

            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (MultipleCellEditHelper.Checkview == "LeadsTableView")
                {
                    ObjLeadsViewModel.UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);


                }
                else if (MultipleCellEditHelper.Checkview == "OrderTableView")
                {
                    ObjOrderViewModel.UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);
                }
            }

            MultipleCellEditHelper.IsValueChanged = false;
        }


        /// <summary>
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);
                //ObservableCollection<Customer> TempCompanyGroupList = null;
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().OrderBy(x => x.IdUser).ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                    else
                    {

                        ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", ListGroup);
                    }
                }
                else
                {

                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    else
                    {

                        ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", ListGroup);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Company list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().OrderBy(x => x.IdUser).ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                    }

                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);

                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        private void OpenRecurrentActivitiesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenRecurrentActivitiesView()...", category: Category.Info, priority: Priority.Low);


                RecurrentActivitiesViewModel recurrentActivitiesViewModel = new RecurrentActivitiesViewModel();
                recurrentActivitiesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.Crm.Views.RecurrentActivitiesView", recurrentActivitiesViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method OpenRecurrentActivitiesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenRecurrentActivitiesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        #endregion // Methods
    }
}
