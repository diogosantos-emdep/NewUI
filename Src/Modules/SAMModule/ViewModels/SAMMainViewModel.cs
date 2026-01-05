using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
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

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class SAMMainViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// [001][skhade][2019-09-25][GEOS2-1757] Create new module SAM (STRUCTURE ASSEMBLY MANAGER).
        /// </summary>

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ICrmService SAMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMServices = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //ISAMService HrmService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        PendingWorkOrdersViewModel objPendingWorkOrdersViewModel;
        public static WorkPlanningView ObjWorkPlanningView { get; set; }
        public static OrderItemsView ObjOrderItemsView { get; set; }
      
        #endregion // Declaration

        #region Properties

        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

        public PendingWorkOrdersViewModel ObjPendingWorkOrdersViewModel
        {
            get { return objPendingWorkOrdersViewModel; }
            set
            {
                objPendingWorkOrdersViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPendingWorkOrdersViewModel"));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// [001][skhade][2019-09-25][GEOS2-1757] Create new module SAM (STRUCTURE ASSEMBLY MANAGER).
        /// </summary>
        public SAMMainViewModel()
        {
            try
            {
                if (!GeosApplication.Instance.ObjectPool.ContainsKey("SAMMainViewModel") || GeosApplication.Instance.ObjectPool.Count == 0)
                    GeosApplication.Instance.ObjectPool.Add("SAMMainViewModel", this);
                GeosApplication.Instance.Logger.Log("Constructor SAMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                FillUserAuthorizedPlants();
                if(GeosApplication.Instance.ActiveUser.UserPermissions.Any(i=>i.IdPermission== 65))
                {
                   SAMCommon.Instance.IsPermissionQualityInspection = true;
                }
                else
                {
                    SAMCommon.Instance.IsPermissionQualityInspection = false;
                }
                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelDashboard").ToString();
                tileBarItemsHelperDashboard.BackColor = "#00879C";
                tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                tileBarItemsHelperDashboard.Children = new ObservableCollection<TileBarItemsHelper>();
                tileBarItemsHelperDashboard.NavigateCommand = new DelegateCommand(() => { Service.Navigate(NavigateDashboardView(), null, this); });

                TileCollection.Add(tileBarItemsHelperDashboard);

                TileBarItemsHelper tileBarItemsHelperOrders = new TileBarItemsHelper();
                tileBarItemsHelperOrders.Caption = System.Windows.Application.Current.FindResource("SAMOrders").ToString();
                tileBarItemsHelperOrders.BackColor = "#FF427940";
                tileBarItemsHelperOrders.GlyphUri = "Orders.png";
                tileBarItemsHelperOrders.Visibility = Visibility.Visible;
                tileBarItemsHelperOrders.Children = new ObservableCollection<TileBarItemsHelper>();
                // [001][plahange][06-12-2022][GEOS2-3680] Pending Work Orders title has been change to Structure
                TileBarItemsHelper tbiPendingWorkOrders = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMPendingWorkOrders").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "Structures.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(PendingWorkOrdersViewAction(), null, this); })
                };

                tileBarItemsHelperOrders.Children.Add(tbiPendingWorkOrders);

                TileBarItemsHelper tbiWorkPlanning = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMWorkPlanning").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "CalendarWithWatch.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(ViewWorkPlanningScreen(), null, this); })
                };

                tileBarItemsHelperOrders.Children.Add(tbiWorkPlanning);

                TileBarItemsHelper tblItems = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMItems").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "SAMItems.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(ViewOrderItemsScreen(), null, this); })
                };

                tileBarItemsHelperOrders.Children.Add(tblItems);


                
                TileCollection.Add(tileBarItemsHelperOrders);


                TileBarItemsHelper tileBarItemsHelperReports = new TileBarItemsHelper();
                tileBarItemsHelperReports.Caption = System.Windows.Application.Current.FindResource("SAMREPORTS").ToString();
                tileBarItemsHelperReports.BackColor = "#a8389f";
                tileBarItemsHelperReports.GlyphUri = "Reports.png";
                tileBarItemsHelperReports.Visibility = Visibility.Visible;
                tileBarItemsHelperReports.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tbiWorklogReport = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMWorklogReport").ToString(),
                    BackColor = "#a8389f",
                    GlyphUri = "bModulesReport.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WorklogReportViewAction(), null, this); })
                };

                TileBarItemsHelper tbiOrdersReport = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMOrderReports").ToString(),
                    BackColor = "#a8389f",
                    GlyphUri = "SAMOrdersReport.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(OrdersReportViewAction(), null, this); })
                };

                tileBarItemsHelperReports.Children.Add(tbiWorklogReport);
                tileBarItemsHelperReports.Children.Add(tbiOrdersReport);
                TileCollection.Add(tileBarItemsHelperReports);

                //Configuration
                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("SAMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMMyPreferences").ToString(),
                    BackColor = "#C7BFE6",
                    GlyphUri = "MyPreference_Black.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(NavigateMyPreferences)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

                TileCollection.Add(tileBarItemsHelperConfiguration);


                GeosApplication.Instance.Logger.Log("Constructor Constructor SAMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor SAMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor SAMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor SAMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// [001][cpatil][GEOS2-5299][26-02-2024]
        private void FillUserAuthorizedPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUserAuthorizedPlants() started executing...", category: Category.Info, priority: Priority.Low);
                // [001] Changed Service method
                //SAMCommon.Instance.PlantOwnerList = SAMService.GetAllCompaniesDetails_V2490(GeosApplication.Instance.ActiveUser.IdUser).OrderBy(o => o.Country.IdCountry).ToList();
                // [Rahul.Gadhave][GEOS2-8713][Date:03/11/2025]
                //  SAMServices = new SAMServiceController("localhost:6699");
                SAMCommon.Instance.PlantOwnerList = SAMServices.GetAllCompaniesDetails_V2680(GeosApplication.Instance.ActiveUser.IdUser).OrderBy(o => o.Country.IdCountry).ToList();

                SAMCommon.Instance.SelectedPlantOwnerList = new List<object>();

                // EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(GeosApplication.Instance.ActiveUser...Site.ConnectPlantId));
                //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();

                Company companyDefault = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(x => x.ShortName == GeosApplication.Instance.SiteName);
                if (companyDefault != null)
                {
                    SAMCommon.Instance.SelectedPlantOwnerList.Add(companyDefault);
                }
                else
                {
                    SAMCommon.Instance.SelectedPlantOwnerList.AddRange(SAMCommon.Instance.SelectedPlantOwnerList);
                }

                //pramod.misal GEOS2-5327
                if (SAMCommon.Instance.PlantOwnerList != null)
                {
                    foreach (var item in SAMCommon.Instance.PlantOwnerList)
                    {
                        if (item.Country != null)
                        {
                            item.ServiceProviderUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Alias).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillUserAuthorizedPlants() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillUserAuthorizedPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillUserAuthorizedPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillUserAuthorizedPlants SAMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        #region Methods

        /// <summary>
        /// Method for Navigate Dashboard
        /// </summary>
        private DashboardView NavigateDashboardView()
        {
            DashboardView dashboardView = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()...", category: Category.Info, priority: Priority.Low);

                if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
                {
                    SavechangesOrder();
                }

                if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
                {
                    SavechangesInSAMWorkPlanning();
                }

                dashboardView = new DashboardView();
                var dashboardViewModel = new DashboardViewModel();
                dashboardViewModel.InitAsync();//Shubham[skadam] [V.2.6.9.0] GEOS2-8853 SAM module very slow when trying to load informations - Dashboard (6/6) 25 11 2025
                dashboardView.DataContext = dashboardViewModel;

                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDashboardView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return dashboardView;
        }

        private PendingWorkOrdersView PendingWorkOrdersViewAction()
        {
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                SavechangesOrder();
            }

            if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
            {
                SavechangesInSAMWorkPlanning();
            }

            PendingWorkOrdersView workOrderView = new PendingWorkOrdersView();
            PendingWorkOrdersViewModel pendingWorkOrdersViewModel = new PendingWorkOrdersViewModel();
            pendingWorkOrdersViewModel.InitAsync();//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
            workOrderView.DataContext = pendingWorkOrdersViewModel;
            return workOrderView;
        }
        
        private WorkPlanningView ViewWorkPlanningScreen()
        {
            GC.Collect();
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                SavechangesOrder();
            }

            if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
            {
                SavechangesInSAMWorkPlanning();
            }

            WorkPlanningView workPlanningView = new WorkPlanningView();
            WorkPlanningViewModel workPlanningViewModel = new WorkPlanningViewModel();
            workPlanningViewModel.Init();
            workPlanningViewModel.InitAsync();//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
            workPlanningView.DataContext = workPlanningViewModel;

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GC.Collect();
            ObjWorkPlanningView = workPlanningView;
            return workPlanningView;

        }
        
        public void SavechangesInSAMWorkPlanning()
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["OteditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ObjWorkPlanningView != null)
                {
                    WorkPlanningViewModel workPlanningViewModel = (WorkPlanningViewModel)ObjWorkPlanningView.DataContext;
                    workPlanningViewModel.UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkPlanning.Viewtableview);
                }               
            }
            MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
        }
        private OrderItemsView ViewOrderItemsScreen()
        {
            GC.Collect();
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                SavechangesOrder();
            }

            if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
            {
                SavechangesInSAMWorkPlanning();
            }

            OrderItemsView orderItemsView = new OrderItemsView();
            OrderItemsViewModel orderItemsViewModel = new OrderItemsViewModel();
            orderItemsViewModel.InitAsync();//Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
            orderItemsView.DataContext = orderItemsViewModel;

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GC.Collect();
            ObjOrderItemsView = orderItemsView;
            return orderItemsView;

        }

        


        ///----------[Sprint-83] [GEOS2-2372]  [19-06-2020] [sjadhav]---------
        public void SavechangesOrder()
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["OteditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ObjPendingWorkOrdersViewModel == null)
                {
                    ObjPendingWorkOrdersViewModel = new PendingWorkOrdersViewModel();
                    ObjPendingWorkOrdersViewModel.InitAsync();//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
                    ObjPendingWorkOrdersViewModel.MainOtsList = GeosApplication.Instance.MainOtsList;
                }
                if (MultipleCellEditHelperSAMWorkOrder.Checkview == "PendingWorkOrderTableView")
                {
                    ObjPendingWorkOrdersViewModel.UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkOrder.Viewtableview);
                }                
            }
            MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
        }

        private WorklogReportView WorklogReportViewAction()
        {
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                SavechangesOrder();
            }

            if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
            {
                SavechangesInSAMWorkPlanning();
            }

            WorklogReportView worklogReportView = new WorklogReportView();
            WorklogReportViewModel worklogReportViewModel = new WorklogReportViewModel();
            worklogReportViewModel.InitAsync();//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6) 15 10 2025
            worklogReportView.DataContext = worklogReportViewModel;
            return worklogReportView;
        }

        private OrdersReportView OrdersReportViewAction()
        {
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                SavechangesOrder();
            }

            if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
            {
                SavechangesInSAMWorkPlanning();
            }

            OrdersReportView ordersReportView = new OrdersReportView();
            OrdersReportViewModel ordersReportViewModel = new OrdersReportViewModel();
            ordersReportViewModel.InitAsync();//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
            ordersReportView.DataContext = ordersReportViewModel;
            return ordersReportView;
        }

        private void NavigateMyPreferences(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);

                MyPreferencesView myPreferencesView = new MyPreferencesView();
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateMyPreferences()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            //if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            //{
            //    SavechangesOrder();
            //}
            //PendingWorkOrdersView workOrderView = new PendingWorkOrdersView();
            //PendingWorkOrdersViewModel pendingWorkOrdersViewModel = new PendingWorkOrdersViewModel();
            //workOrderView.DataContext = pendingWorkOrdersViewModel;
            //return workOrderView;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
