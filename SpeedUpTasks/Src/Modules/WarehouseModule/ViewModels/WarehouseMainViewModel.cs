using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WarehouseMainViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        bool isBusy;

        #endregion

        #region Properties

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
        /// <summary>
        /// [001][skale][2019-19-04][GEOS2-256] New section "Pending Articles" in work orders
        /// [002][Sprint_72]__[GEOS2-1656]__[Add article Sleeping days column in warehouse section]__[sdesai]
        /// [003][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        public WarehouseMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WarehouseMainViewModel....", category: Category.Info, priority: Priority.Low);
                FillWarehouseList();

                WarehouseCommon.Instance.IsPermissionAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 31);

                WarehouseCommon.Instance.IsPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 28);

                WarehouseCommon.Instance.IsPermissionAdmin = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 32);

                //[003]added
                WarehouseCommon.Instance.IsStockRegularizationApproval = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 37);

                if (WarehouseCommon.Instance.IsPermissionReadOnly)
                    WarehouseCommon.Instance.IsPermissionEnabled = false;
                else
                    WarehouseCommon.Instance.IsPermissionEnabled = true;

                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("PickingTimer"))
                        WarehouseCommon.Instance.IsPickingTimer = Convert.ToBoolean(GeosApplication.Instance.UserSettings["PickingTimer"]);
                }

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                //** For Dashboard
                TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelDashboard").ToString();
                tileBarItemsHelperDashboard.BackColor = "#00879C";
                tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                tileBarItemsHelperDashboard.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemsHelperDashboard1 = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Dashboard1").ToString(),
                    BackColor = "#00879C",
                    GlyphUri = "bDashboard1.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(Dashboard1ViewAction(), null, this); })
                };

                tileBarItemsHelperDashboard.Children.Add(tileBarItemsHelperDashboard1);
                TileBarItemsHelper tileBarItemsHelperDashboard2 = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("DashBoard2ViewHeader").ToString(),
                    BackColor = "#00879C",
                    GlyphUri = "bDashboard2.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(Dashboard2ViewAction(), null, this); })
                };
                tileBarItemsHelperDashboard.Children.Add(tileBarItemsHelperDashboard2);

                TileCollection.Add(tileBarItemsHelperDashboard);

                //** For PURCHASE ORDERS
                TileBarItemsHelper tileBarItemsHelperPurchaseOrder = new TileBarItemsHelper();
                tileBarItemsHelperPurchaseOrder.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPurchaseOrders").ToString();
                tileBarItemsHelperPurchaseOrder.BackColor = "#FFF78A09";
                tileBarItemsHelperPurchaseOrder.GlyphUri = "PurchaseOrder.png";
                tileBarItemsHelperPurchaseOrder.Visibility = Visibility.Visible;
                tileBarItemsHelperPurchaseOrder.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemsHelperPendingReception = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPendingReception").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "PendingReception.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(PurchaseOrderViewAction(), null, this); })
                };
                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemsHelperPendingReception);

                TileBarItemsHelper tileBarItemPOSchedule = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelSchedule").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "Schedule.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(PurchaseOrdersScheduleViewAction(), null, this); })
                };
                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemPOSchedule);

                TileBarItemsHelper tileBarItemPendingArticles = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPendingArticles").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "PendingArticles.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(PendingArticlesViewViewAction(), null, this); })
                };
                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemPendingArticles);

               

                TileCollection.Add(tileBarItemsHelperPurchaseOrder);

                //** For WORK ORDERS
                TileBarItemsHelper tileBarItemsHelperWorkOrders = new TileBarItemsHelper();
                tileBarItemsHelperWorkOrders.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPicking").ToString();
                tileBarItemsHelperWorkOrders.BackColor = "#FF427940";
                tileBarItemsHelperWorkOrders.GlyphUri = "WorkOrder.png";
                tileBarItemsHelperWorkOrders.Visibility = Visibility.Visible;
                tileBarItemsHelperWorkOrders.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemWOMaterial = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelWorkOrders").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "Material.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WorkOrderViewAction(), null, this); })
                };
                tileBarItemsHelperWorkOrders.Children.Add(tileBarItemWOMaterial);

                TileBarItemsHelper tileBarItemtileBarItemWOSchedule = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelSchedule").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "Schedule.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WorkOrderScheduleViewction(), null, this); })
                };
                tileBarItemsHelperWorkOrders.Children.Add(tileBarItemtileBarItemWOSchedule);

                //[001] Added
                TileBarItemsHelper tileBarItemtileBarPickingPendingArticle = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPendingArticles").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "PendingArticles.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WOPendingArticleViewAction(), null, this); })
                };
                tileBarItemsHelperWorkOrders.Children.Add(tileBarItemtileBarPickingPendingArticle);
                //END

                TileBarItemsHelper tileBarItemtileBarOdn = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelOdn").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "ODN.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(OdnViewAction(), null, this); })
                };
                tileBarItemsHelperWorkOrders.Children.Add(tileBarItemtileBarOdn);

            //    TileBarItemsHelper tileBarItemInternalUse = new TileBarItemsHelper()
            //    {
            //        Caption = System.Windows.Application.Current.FindResource("InternalUse").ToString(),
            //        BackColor = "#FF427940",
            //        // GlyphUri = "PendingArticles.png",
            //        Visibility = Visibility.Visible,
            //        // NavigateCommand = new DelegateCommand(() => { Service.Navigate(InternalUseAction(), null, this); })
            //        NavigateCommand = new DelegateCommand<object>(InternalUseAction)
            //};
            //    tileBarItemsHelperWorkOrders.Children.Add(tileBarItemInternalUse);

                TileCollection.Add(tileBarItemsHelperWorkOrders);

                //** For Warehouse
                TileBarItemsHelper tileBarItemsHelperWarehouse = new TileBarItemsHelper();
                tileBarItemsHelperWarehouse.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelWAREHOUSE").ToString();
                tileBarItemsHelperWarehouse.BackColor = "#FF083493";
                tileBarItemsHelperWarehouse.GlyphUri = "Warehouse.png";
                tileBarItemsHelperWarehouse.Visibility = Visibility.Visible;
                tileBarItemsHelperWarehouse.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemWarehouse = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelWarehouse").ToString(),
                    BackColor = "#FF083493",
                    GlyphUri = "WarehouseSection.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WarehouseviewAction(), null, this); })
                };
                tileBarItemsHelperWarehouse.Children.Add(tileBarItemWarehouse);

                TileBarItemsHelper tileBarItemPendingStorage = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPendingStorage").ToString(),
                    BackColor = "#FF083493",
                    GlyphUri = "bPendingStorage.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(PendingStorageViewViewAction(), null, this); })
                };
                tileBarItemsHelperWarehouse.Children.Add(tileBarItemPendingStorage);

                //For Location
                TileBarItemsHelper tileBarItemLocation = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelLocation").ToString(),
                    BackColor = "#FF083493",
                    GlyphUri = "bLocations.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(LocationViewAction(), null, this); })
                };
                tileBarItemsHelperWarehouse.Children.Add(tileBarItemLocation);

                //[003]
                //For Inventory Audits
                TileBarItemsHelper tileBarItemInventoryAudits = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("InventoryAudits").ToString(),
                    BackColor = "#FF083493",
                    GlyphUri = "InventoryAudit.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(InventoryAuditViewAction(), null, this); })
                };
                tileBarItemsHelperWarehouse.Children.Add(tileBarItemInventoryAudits);

                TileCollection.Add(tileBarItemsHelperWarehouse);

                // FOR PACKING
                TileBarItemsHelper tileBarItemsHelperPacking = new TileBarItemsHelper();
                tileBarItemsHelperPacking.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelPacking").ToString();
                tileBarItemsHelperPacking.BackColor = "#6666FF";
                tileBarItemsHelperPacking.GlyphUri = "Packing.png";
                tileBarItemsHelperPacking.Visibility = Visibility.Visible;
                tileBarItemsHelperPacking.Children = new ObservableCollection<TileBarItemsHelper>();
                tileBarItemsHelperPacking.NavigateCommand = new DelegateCommand(() => { Service.Navigate(PackingViewAction(), null, this); });
                TileCollection.Add(tileBarItemsHelperPacking);

                // FOR DISPATCH
                TileBarItemsHelper tileBarItemsHelperDispatch = new TileBarItemsHelper();
                tileBarItemsHelperDispatch.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelDispatch").ToString();
                tileBarItemsHelperDispatch.BackColor = "#859C27";
                tileBarItemsHelperDispatch.GlyphUri = "Dispatch.png";
                tileBarItemsHelperDispatch.Visibility = Visibility.Visible;
                tileBarItemsHelperDispatch.Children = new ObservableCollection<TileBarItemsHelper>();
                TileCollection.Add(tileBarItemsHelperDispatch);

                //For Management
                if (WarehouseCommon.Instance.IsPermissionAdmin)
                {
                    TileBarItemsHelper tileBarItemsHelperManagement = new TileBarItemsHelper();
                    tileBarItemsHelperManagement.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelManagement").ToString();
                    tileBarItemsHelperManagement.BackColor = "#34495E";
                    tileBarItemsHelperManagement.GlyphUri = "Management.png";
                    tileBarItemsHelperManagement.Visibility = Visibility.Visible;
                    tileBarItemsHelperManagement.Children = new ObservableCollection<TileBarItemsHelper>();

                    TileBarItemsHelper tileBarItemsHelperPlanningSimulator = new TileBarItemsHelper();
                    tileBarItemsHelperPlanningSimulator.Caption = System.Windows.Application.Current.FindResource("PlanningSimulator").ToString();
                    tileBarItemsHelperPlanningSimulator.BackColor = "#34495E";
                    tileBarItemsHelperPlanningSimulator.Visibility = Visibility.Visible;
                    tileBarItemsHelperPlanningSimulator.GlyphUri = "bPlanningSimulator.png";
                    tileBarItemsHelperPlanningSimulator.NavigateCommand = new DelegateCommand(() => { Service.Navigate(PlanningSimulatorViewAction(), null, this); });

                    tileBarItemsHelperManagement.Children.Add(tileBarItemsHelperPlanningSimulator);
                    // TileCollection.Add(tileBarItemsHelperManagement);

                    //Orders
                    TileBarItemsHelper tileBarItemsHelperOrders = new TileBarItemsHelper();
                    tileBarItemsHelperOrders.Caption = System.Windows.Application.Current.FindResource("ManagementOrders").ToString();
                    tileBarItemsHelperOrders.BackColor = "#34495E";
                    tileBarItemsHelperOrders.Visibility = Visibility.Visible;
                    tileBarItemsHelperOrders.GlyphUri = "ManagementOrders.png";
                    //tileBarItemsHelperOrders.GlyphUri = "ManagementOrders__new.png";
                    tileBarItemsHelperOrders.NavigateCommand = new DelegateCommand(() => { Service.Navigate(ManagementOrdersViewAction(), null, this); });
                    tileBarItemsHelperManagement.Children.Add(tileBarItemsHelperOrders);

                    TileCollection.Add(tileBarItemsHelperManagement);
                }

                // FOR CONFIGURATION
                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelConfigurations").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelMyPreferences").ToString(),
                    BackColor = "#00BFFF",
                    GlyphUri = "MyPreference_Black.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(OpenMyPreference)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

                TileBarItemsHelper tileBarItemConfigurationLocations = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelConfigurationLocations").ToString(),
                    BackColor = "#00BFFF",
                    GlyphUri = "bWarehouseLocations.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WarehouseLocationsViewAction(), null, this); })
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationLocations);

                TileBarItemsHelper tileBarItemConfigurationSystemSettings = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelConfigurationSystemSettings").ToString(),
                    BackColor = "#00BFFF",
                    GlyphUri = "bSystemSettings.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(SystemSettingsViewAction)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationSystemSettings);

                TileCollection.Add(tileBarItemsHelperConfiguration);

                //System Settings Visibility
                if (!WarehouseCommon.Instance.IsPermissionAdmin)
                {
                    tileBarItemConfigurationSystemSettings.Visibility = Visibility.Collapsed;
                }

                CheckIsRefillEmptyLocationsFirstEnable();
                GetArticleSleepingDays();
                FillPackingSettings();
                GeosApplication.Instance.Logger.Log("Constructor WarehouseMainViewModel() ....executed successfully ", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WarehouseMainViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WarehouseMainViewModel() Constructor - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WarehouseMainViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for fill warehouse list.
        /// </summary>
        private void FillWarehouseList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseList...", category: Category.Info, priority: Priority.Low);

                WarehouseCommon.Instance.WarehouseList = new List<Warehouses>();
                WarehouseCommon.Instance.WarehouseList = WarehouseService.GetAllWarehousesByUserPermission_V2034(GeosApplication.Instance.ActiveUser.IdUser);

                if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedwarehouseId"))
                {
                    if (WarehouseCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]) != null)
                        WarehouseCommon.Instance.Selectedwarehouse = WarehouseCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]);
                    else
                        WarehouseCommon.Instance.Selectedwarehouse = WarehouseCommon.Instance.WarehouseList[0];
                }
                else
                {
                    WarehouseCommon.Instance.Selectedwarehouse = WarehouseCommon.Instance.WarehouseList[0];
                }

                GeosApplication.Instance.Logger.Log("Method FillWarehouseList() executed successfully", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for show Pending Reception section (PurchaseOrder).
        /// </summary>
        /// <returns></returns>
        private PurchaseOrderView PurchaseOrderViewAction()
        {
            PurchaseOrderView purchaseOrderView = new PurchaseOrderView();
            PurchaseOrderViewModel purchaseOrderViewModel = new PurchaseOrderViewModel();
            purchaseOrderView.DataContext = purchaseOrderViewModel;
            return purchaseOrderView;
        }

        /// <summary>
        /// Method for show Purchase Order Schedule section.
        /// </summary>
        /// <returns></returns>
        private PurchaseOrderScheduleView PurchaseOrdersScheduleViewAction()
        {
            PurchaseOrderScheduleView purchaseOrdersScheduleView = new PurchaseOrderScheduleView();
            PurchaseOrderScheduleViewModel purchaseOrdersScheduleViewModel = new PurchaseOrderScheduleViewModel();
            purchaseOrdersScheduleView.DataContext = purchaseOrdersScheduleViewModel;
            return purchaseOrdersScheduleView;
        }

        /// <summary>
        /// Method for show Work Order Schedule section.
        /// </summary>
        /// <returns></returns>
        private WorkOrderScheduleView WorkOrderScheduleViewction()
        {
            WorkOrderScheduleView workOrdersScheduleView = new WorkOrderScheduleView();
            WorkOrderScheduleViewModel workOrderScheduleViewModel = new WorkOrderScheduleViewModel();
            workOrdersScheduleView.DataContext = workOrderScheduleViewModel;
            return workOrdersScheduleView;
        }

        /// <summary>
        /// [00][skale][2019-19-04][GEOS2-256] New section "Pending Articles" in work orders
        /// Method for show Work Order Pending Articles section .
        /// </summary>
        /// <returns></returns>
        private WOPendingArticlesView WOPendingArticleViewAction()
        {
            WOPendingArticlesView WOPendingArticlesView = new WOPendingArticlesView();
            WOPendingArticlesViewModel WOPendingArticlesViewModel = new WOPendingArticlesViewModel();
            WOPendingArticlesView.DataContext = WOPendingArticlesViewModel;
            return WOPendingArticlesView;
        }

        private ODNView OdnViewAction()
        {
            ODNView OdnView = new ODNView();
            ODNViewModel OdnViewModel = new ODNViewModel();
            OdnView.DataContext = OdnViewModel;
            return OdnView;
        }

        /// <summary>
        /// Method for show Work Order section .
        /// </summary>
        /// <returns></returns>
        private WorkOrderView WorkOrderViewAction()
        {
            WorkOrderView workOrderView = new WorkOrderView();
            WorkOrderViewModel workOrderViewModel = new WorkOrderViewModel();
            workOrderView.DataContext = workOrderViewModel;
            return workOrderView;
        }

        /// <summary>
        /// Method for show Warehouse view section .
        /// </summary>
        /// <returns></returns>
        private WarehouseView WarehouseviewAction()
        {
            WarehouseView warehouseview = new WarehouseView();
            WarehouseViewModel warehouseviewModel = new WarehouseViewModel();
            warehouseview.DataContext = warehouseviewModel;
            return warehouseview;
        }

        /// <summary>
        /// Method for show Pending Storage View section .
        /// </summary>
        /// <returns></returns>
        private PendingStorageView PendingStorageViewViewAction()
        {
            PendingStorageView pendingStorageView = new PendingStorageView();
            PendingStorageViewModel PendingStorageViewModel = new PendingStorageViewModel();
            pendingStorageView.DataContext = PendingStorageViewModel;
            return pendingStorageView;
        }

        /// <summary>
        /// Method for show Location View section .
        /// </summary>
        /// <returns></returns>
        private LocationView LocationViewAction()
        {
            LocationView locationView = new LocationView();
            LocationViewModel locationViewModel = new LocationViewModel();
            locationView.DataContext = locationViewModel;
            return locationView;
        }

        /// <summary>
        /// Method for show Pending Articles View section .
        /// </summary>
        /// <returns></returns>
        private PendingArticlesView PendingArticlesViewViewAction()
        {
            PendingArticlesView pendingArticlesView = new PendingArticlesView();
            PendingArticlesViewModel PendingArticlesViewModel = new PendingArticlesViewModel();
            pendingArticlesView.DataContext = PendingArticlesViewModel;
            return pendingArticlesView;
        }

       // private void InternalUseAction(object obj)
       // {
            //IsBusy = true;
            //InternalUseView internalUseView = new InternalUseView();
            //InternalUseViewModel internalUseViewModel = new InternalUseViewModel();
            //internalUseView.DataContext = internalUseViewModel;
            //EventHandler handle = delegate { internalUseView.Close(); };
            //internalUseViewModel.RequestClose += handle;
            //internalUseView.DataContext = internalUseViewModel;
            //IsBusy = false;
            //internalUseView.ShowDialogWindow();
      //  }

        /// <summary>
        /// Method for WarehouseLocations.
        /// </summary>
        /// <returns></returns>
        private WarehouseLocationsView WarehouseLocationsViewAction()
        {
            WarehouseLocationsView warehouseLocationsView = new WarehouseLocationsView();
            WarehouseLocationsViewModel warehouseLocationsViewModel = new WarehouseLocationsViewModel();
            warehouseLocationsView.DataContext = warehouseLocationsViewModel;
            return warehouseLocationsView;
        }

        /// <summary>
        /// Method for show Packing View section .
        /// </summary>
        /// <returns></returns>
        private PackingView PackingViewAction()
        {

            PackingView PackingView = new PackingView();
            PackingViewModel PackingViewModel = new PackingViewModel();
            PackingView.DataContext = PackingViewModel;
            return PackingView;
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

                IsBusy = true;
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                IsBusy = false;
                myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method OpenMyPreference() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in OpenMyPreference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SystemSettingsViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                SystemSettingsViewModel systemSettingsViewModel = new SystemSettingsViewModel();
                SystemSettingsView systemSettingsView = new SystemSettingsView();
                EventHandler handle = delegate { systemSettingsView.Close(); };
                systemSettingsViewModel.RequestClose += handle;
                systemSettingsView.DataContext = systemSettingsViewModel;
                IsBusy = false;
                systemSettingsView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private PlanningSimulatorView PlanningSimulatorViewAction()
        {
            PlanningSimulatorView planningSimulatorView = new PlanningSimulatorView();
            PlanningSimulatorViewModel planningSimulatorViewModel = new PlanningSimulatorViewModel();
            planningSimulatorView.DataContext = planningSimulatorViewModel;
            return planningSimulatorView;
        }

        private ManagementOrderView ManagementOrdersViewAction()
        {
            ManagementOrderView managementOrderView = new ManagementOrderView();
            ManagementOrderViewModel managementOrderViewModel = new ManagementOrderViewModel();
            managementOrderView.DataContext = managementOrderViewModel;
            return managementOrderView;
        }

        public void Dispose()
        {
        }

        private Dashboard1View Dashboard1ViewAction()
        {
            Dashboard1View dashboard1View = new Dashboard1View();
            Dashboard1ViewModel dashboard1ViewModel = new Dashboard1ViewModel();
            dashboard1View.DataContext = dashboard1ViewModel;
            return dashboard1View;
        }

        private Dashboard2View Dashboard2ViewAction()
        {
            Dashboard2View dashboard2View = new Dashboard2View();
            Dashboard2ViewModel dashboard2ViewModel = new Dashboard2ViewModel();
            dashboard2View.DataContext = dashboard2ViewModel;
            return dashboard2View;
        }

        /// <summary>
        /// [000][skale][24-07-2019][GEOS2-1667]MEJORA EN REFILL
        /// </summary>
        private void CheckIsRefillEmptyLocationsFirstEnable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckIsRefillEmptyLocationsFirstEnable()...", category: Category.Info, priority: Priority.Low);

                List<GeosAppSetting> GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("25");

                if (GeosAppSettingList.Count > 0)
                {
                    string[] GeosAppSettingDefaultValues = GeosAppSettingList[0].DefaultValue.Split(',');

                    if (GeosAppSettingDefaultValues.Any(x => x.Contains(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString())))
                        WarehouseCommon.Instance.IsRefillEmptyLocationsFirst = true;
                    else
                        WarehouseCommon.Instance.IsRefillEmptyLocationsFirst = false;
                }
                GeosApplication.Instance.Logger.Log("Method CheckIsRefillEmptyLocationsFirstEnable()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CheckIsRefillEmptyLocationsFirstEnable() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CheckIsRefillEmptyLocationsFirstEnable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckIsRefillEmptyLocationsFirstEnable()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get Article Sleeping Days from Geos App Setting.
        ///  //[002][Sprint_72]__[GEOS2-1656]__[Add article Sleeping days column in warehouse section]__[sdesai]
        /// </summary>
        private void GetArticleSleepingDays()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetArticleSleepingDays()...", category: Category.Info, priority: Priority.Low);

                List<GeosAppSetting> GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("30");

                if (GeosAppSettingList.Count > 0)
                {
                    WarehouseCommon.Instance.ArticleSleepDays = Convert.ToInt32(GeosAppSettingList[0].DefaultValue);
                }
                GeosApplication.Instance.Logger.Log("Method GetArticleSleepingDays()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetArticleSleepingDays() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetArticleSleepingDays() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetArticleSleepingDays()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillPackingSettings()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPackingSettings()...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedScaleModel"))
                        WarehouseCommon.Instance.SelectedScaleModel = GeosApplication.Instance.UserSettings["SelectedScaleModel"];

                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedPort"))
                        WarehouseCommon.Instance.SelectedPort = GeosApplication.Instance.UserSettings["SelectedPort"];

                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedParity"))
                        WarehouseCommon.Instance.SelectedParity = GeosApplication.Instance.UserSettings["SelectedParity"];

                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedStopBit"))
                        WarehouseCommon.Instance.SelectedStopBit = GeosApplication.Instance.UserSettings["SelectedStopBit"];

                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedBaudRate"))
                        WarehouseCommon.Instance.SelectedBaudRate = GeosApplication.Instance.UserSettings["SelectedBaudRate"];

                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedDataBit"))
                        WarehouseCommon.Instance.SelectedDataBit = GeosApplication.Instance.UserSettings["SelectedDataBit"];
                }
                GeosApplication.Instance.Logger.Log("Method FillPackingSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillPackingSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for show Inventory Audits View section .
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <returns></returns>
        private InventoryAuditView InventoryAuditViewAction()
        {
            InventoryAuditView inventoryAuditsView = new InventoryAuditView();
            InventoryAuditViewModel inventoryAuditsViewModel = new InventoryAuditViewModel();
            inventoryAuditsViewModel.Init();
            inventoryAuditsView.DataContext = inventoryAuditsViewModel;
            return inventoryAuditsView;
        }

        #endregion // Methods

    }
}
