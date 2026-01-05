using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SRM.Views;
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
using Emdep.Geos.Modules.Warehouse;
using Emdep.Geos.Modules.Crm.ViewModels;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class SRMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// [001][skhade][2020-02-24][GEOS2-1799] New module SRM - 1.
        /// </summary>

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //  IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        //  ISRMService SRMService = new SRMServiceController("localhost:6699");
        //ISRMService HrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        private PendingPurchaseOrderViewModel objPurchaseOrderViewModel;
        private bool isBusy;    
        public PendingPurchaseOrderViewModel ObjPurchaseOrderViewModel
        {
            get { return objPurchaseOrderViewModel; }
            set
            {
                objPurchaseOrderViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPurchaseOrderViewModel"));
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
        #endregion

        #region Constructor

        /// <summary>
        /// [001][skhade][2020-02-24][GEOS2-1799] New module SRM - 1.
        /// </summary>
        public SRMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SRMMainViewModel()...", category: Category.Info, priority: Priority.Low);
                if (SRMPurchaseOrderCellEditHelper.IsValueChanged)
                    SavechangesPurchaseOrderGrid();

                FillWarehouseList();
                #region GEOS2-4401,GEOS2-4402.GEOS2-4403 Sudhir.Jangra 02/05/2023
                SRMCommon.Instance.SelectedAuthorizedWarehouseList = new List<object>();
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Warehouses selectedPlant = SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.Name == serviceurl);
                if (selectedPlant != null)
                {
                    SRMCommon.Instance.SelectedAuthorizedWarehouseList.Add(selectedPlant);
                }
                else
                {
                    SRMCommon.Instance.SelectedAuthorizedWarehouseList.AddRange(SRMCommon.Instance.WarehouseList);
                }
                #endregion

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                //Purchase order
                TileBarItemsHelper tileBarItemsHelperPurchaseOrder = new TileBarItemsHelper();
                tileBarItemsHelperPurchaseOrder.Caption = System.Windows.Application.Current.FindResource("PurchaseOrdersInSRM").ToString();
                tileBarItemsHelperPurchaseOrder.BackColor = "#FFF78A09";
                tileBarItemsHelperPurchaseOrder.GlyphUri = "PurchaseOrder.png";
                tileBarItemsHelperPurchaseOrder.Visibility = Visibility.Visible;

                TileBarItemsHelper tileBarItemPendingOrder = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("PendingOrders").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "PendingOrders.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigatePendingOrdersModuleView)

                };
                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemPendingOrder);

                TileBarItemsHelper tileBarItemClosedOrders = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("ClosedOrders").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "completedOrders.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigateClosedOrdersView)
                };
                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemClosedOrders);



                TileBarItemsHelper tileBarItemPendingArticle = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("PendingArticles").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "PendingArticles.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigateProductPendingArticlesView)
                };
                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemPendingArticle);

             

                TileCollection.Add(tileBarItemsHelperPurchaseOrder);


                TileBarItemsHelper tileBarItemsHelperWorkOrders = new TileBarItemsHelper();
                tileBarItemsHelperWorkOrders.Caption = System.Windows.Application.Current.FindResource("SRMWorkOrders").ToString();
                tileBarItemsHelperWorkOrders.BackColor = "#FF427940";
                tileBarItemsHelperWorkOrders.GlyphUri = "Material.png";
                tileBarItemsHelperWorkOrders.Visibility = Visibility.Visible;
                tileBarItemsHelperWorkOrders.Children = new ObservableCollection<TileBarItemsHelper>();


                TileBarItemsHelper tileBarItemPendingReview = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("PendingReview").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "PendingReview.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigateProductPendingReviewView)
                };
                tileBarItemsHelperWorkOrders.Children.Add(tileBarItemPendingReview);


                TileCollection.Add(tileBarItemsHelperWorkOrders);

                //Suppliers
                TileCollection.Add(new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Suppliers").ToString(),
                    BackColor = "#00879C",
                    GlyphUri = "Supplier.png",
                    Visibility = Visibility.Visible,

                    Children = new ObservableCollection<TileBarItemsHelper>()
                    {
                        new TileBarItemsHelper()
                        {
                            Caption=System.Windows.Application.Current.FindResource("Companies").ToString(),
                            BackColor="#00879C",
                            GlyphUri="Account.png",
                            Visibility=Visibility.Visible,
                            NavigateCommand=new DelegateCommand(NavigateSuppliers)
                        },
                        new TileBarItemsHelper()
                        {
                            Caption=System.Windows.Application.Current.FindResource("Contacts").ToString(),
                            BackColor="#00879C",
                            GlyphUri="Contacts.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand=new DelegateCommand(NavigateContacts)
                        }
                    }



                });

                //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
                if (GeosApplication.Instance.IsWMManagerPermission)
                {

                    TileBarItemsHelper tileBarItemsHelperWMM = new TileBarItemsHelper();
                    tileBarItemsHelperWMM.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModeWMM").ToString();
                    tileBarItemsHelperWMM.BackColor = "#FCC419";
                    tileBarItemsHelperWMM.GlyphUri = "WMM.png";
                    tileBarItemsHelperWMM.Visibility = Visibility.Visible;
                    tileBarItemsHelperWMM.Children = new ObservableCollection<TileBarItemsHelper>();

                    TileBarItemsHelper tileBarItemReOrder = new TileBarItemsHelper()
                    {
                        Caption = Application.Current.FindResource("WarehouseViewModeReOrderItem").ToString(),
                        BackColor = "#FCC419",
                        GlyphUri = "Preorder.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(PreOrderViewAction)
                    };
                    tileBarItemsHelperWMM.Children.Add(tileBarItemReOrder);

                    TileBarItemsHelper tileBarItemAnalysis = new TileBarItemsHelper()
                    {
                        Caption = Application.Current.FindResource("WarehouseViewModeAnalysisItem").ToString(),
                        BackColor = "#FCC419",
                        GlyphUri = "Analysis.png",
                        Visibility = Visibility.Visible,
                        //NavigateCommand = new DelegateCommand(() => { Service.Navigate(ProductInspectionviewAction(), null, this); })
                    };
                    tileBarItemsHelperWMM.Children.Add(tileBarItemAnalysis);
                    TileCollection.Add(tileBarItemsHelperWMM);
                }




                // Reports
                // [nsatpute][21-01-2025][GEOS2-5725]
                #region Reports

                {
                    TileCollection.Add(new TileBarItemsHelper
                    {
                        Caption = System.Windows.Application.Current.FindResource("Reports").ToString(),
                        BackColor = "#A84FA9",
                        GlyphUri = "Reports.png",
                        Visibility = Visibility.Visible,
                        
                        Children = new ObservableCollection<TileBarItemsHelper>
                        {
                            new TileBarItemsHelper{
                            Caption = System.Windows.Application.Current.FindResource("Srmreports_Artpurchase_Articlepurchasereport").ToString(),
                            BackColor = "#A84FA9",
                            GlyphUri = "SalesReport.png",
                            Visibility = Visibility.Visible
                            ,NavigateCommand = new DelegateCommand<object>(OpenPurchasingReport)

                        },


                        //     //this is commented section .View and View model already commited. and task is completed.//[Rahul.Gadhave][GEOS2-5726][Date:23-05-2025]
                        //     new TileBarItemsHelper{
                        //    Caption = System.Windows.Application.Current.FindResource("Srmreports_ArtSales_Articlepurchasereport").ToString(),
                        //    BackColor = "#A84FA9",
                        //    GlyphUri = "bArticlesReport.png",
                        //    Visibility = Visibility.Visible
                        //    ,NavigateCommand = new DelegateCommand<object>(OpenSalesReport)

                        //},
                        //      //this is commented section .View and View model already commited. and task is completed.//[pramod.misal][GEOS-5727][Date:23-05-2025]
                        //      new TileBarItemsHelper{
                        //    Caption = System.Windows.Application.Current.FindResource("Srmreports_Artreception_Articlepurchasereport").ToString(),
                        //    BackColor = "#A84FA9",
                        //    GlyphUri = "bModulesReport.png",
                        //    Visibility = Visibility.Visible
                        //    ,NavigateCommand = new DelegateCommand<object>(OpenreceptionReport)

                        //},
                        //      //this is commented section .View and View model already commited. and task is completed.//[pramod.misal][GEOS-5728][Date:23-05-2025]
                        //       new TileBarItemsHelper{
                        //    Caption = System.Windows.Application.Current.FindResource("Srmreports_ArtManufacturers_Articlepurchasereport").ToString(),
                        //    BackColor = "#A84FA9",
                        //    GlyphUri = "Activityreport.png",
                        //    Visibility = Visibility.Visible
                        //    ,NavigateCommand = new DelegateCommand<object>(OpenManufacturersReport)

                        //},
                        //   //this is commented section .View and View model already commited. and task is completed.//[Rahul.Gadhave][GEOS2-5729][Date:23-05-2025]
                        // new TileBarItemsHelper{
                        //    Caption = System.Windows.Application.Current.FindResource("ArticleDeliveryTimeReportFilterViewTitle").ToString(),
                        //    BackColor = "#A84FA9",
                        //    GlyphUri = "SalesReport.png",
                        //    Visibility = Visibility.Visible
                        //    ,NavigateCommand = new DelegateCommand<object>(OpenArticleDeliveryTimeReport)

                        //}
                        //   //this is commented section .View and View model already commited. and task is completed.//[pramod.misal][GEOS-5731][Date:23-05-2025]
                        //       ,
                        //     new TileBarItemsHelper{
                        //    Caption = System.Windows.Application.Current.FindResource("Srmreports_StockRotation").ToString(),
                        //    BackColor = "#A84FA9",
                        //    GlyphUri = "Activityreport.png",
                        //    Visibility = Visibility.Visible
                        //    ,NavigateCommand = new DelegateCommand<object>(OpenStockRoatationReport)

                        //}



                        }
                    });
                }
                #endregion






                //  [nsatpute][12-06-2024] GEOS2-5463
                TileBarItemsHelper tileBarCatalogueDashboard = new TileBarItemsHelper();
                tileBarCatalogueDashboard.Caption = System.Windows.Application.Current.FindResource("CATALOGUE").ToString();
                tileBarCatalogueDashboard.BackColor = "#840a6a";
                tileBarCatalogueDashboard.GlyphUri = "wProductCatalogue.png";
                tileBarCatalogueDashboard.Visibility = Visibility.Visible;
                tileBarCatalogueDashboard.Children = new ObservableCollection<TileBarItemsHelper>();
                tileBarCatalogueDashboard.NavigateCommand = new DelegateCommand(() => NavigateCatalogueDashboardView());
                TileCollection.Add(tileBarCatalogueDashboard);

                //TileBarItemsHelper tileBarItemsHelperSupplier = new TileBarItemsHelper();
                //tileBarItemsHelperSupplier.Caption = System.Windows.Application.Current.FindResource("Suppliers").ToString();
                //tileBarItemsHelperSupplier.BackColor = "#00879C";
                //tileBarItemsHelperSupplier.GlyphUri = "Supplier.png";
                //tileBarItemsHelperSupplier.Visibility = Visibility.Visible;
                //tileBarItemsHelperSupplier.NavigateCommand = new DelegateCommand(NavigateSuppliers);
                //tileBarItemsHelperSupplier.Children = new ObservableCollection<TileBarItemsHelper>();

                //TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                //{
                //    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelMyPreferences").ToString(),
                //    BackColor = "#00879C",
                //    GlyphUri = "MyPreference_Black.png",
                //    Visibility = Visibility.Visible,
                //    NavigateCommand = new DelegateCommand<object>(NavigateSuppliers)
                //};
                //tileBarItemsHelperSupplier.Children.Add(tileBarItemConfiguration);


                // TileCollection.Add(tileBarItemsHelperSupplier);
                #region GEOS2-4407 Sudhir.Jangra 04/05/2023
                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = Application.Current.FindResource("SRMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration_SRM.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SRMSystemSettings").ToString(),
                    BackColor = "#C7BFE6",
                    GlyphUri = "bSystemSettings.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(NavigateSystemSettings)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

                TileCollection.Add(tileBarItemsHelperConfiguration);
                #endregion


                GetArticleSleepingDays();

                GeosApplication.Instance.Logger.Log("Constructor Constructor SRMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor SRMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor SRMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor SRMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion

        #region Methods

        public void SavechangesPurchaseOrderGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ObjPurchaseOrderViewModel == null)
                    {
                        ObjPurchaseOrderViewModel = new PendingPurchaseOrderViewModel();

                    }
                    if (SRMPurchaseOrderCellEditHelper.Checkview == "PendingPurchaseOrderTableView")
                    {
                        ObjPurchaseOrderViewModel.UpdateMultipleRowsPendingPurchaseGridCommandAction(SRMPurchaseOrderCellEditHelper.Viewtableview);
                    }
                }
                SRMPurchaseOrderCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Method for fill warehouse list.
        /// </summary>
        private void FillWarehouseList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseList...", category: Category.Info, priority: Priority.Low);

                SRMCommon.Instance.WarehouseList = new List<Warehouses>();
                //[Sudhir.jangra][GEOS2-4489][30/05/2023]
                //[cpatil][GEOS2-5299][27/02/2024]
                //[rdixit][GEOS2-8252][16.10.2025]
                SRMCommon.Instance.WarehouseList = SRMService.GetAllWarehousesByUserPermissionInSRM_V2680(GeosApplication.Instance.ActiveUser.IdUser);
                //[rdixit][13.07.2023][GEOS2-4634]
                if (SRMCommon.Instance.WarehouseList != null)
                {
                    foreach (var item in SRMCommon.Instance.WarehouseList)
                    {
                        if (item.Company != null)
                        {
                            item.Company.ServiceProviderUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Company.Alias).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        }
                    }
                }
                #region Old code
                //if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedwarehouseId"))
                //{
                //    if (SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]) != null)
                //        SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]);
                //    else
                //        SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList[0];
                //}
                //else
                //{
                //    SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList[0];
                //}

                #endregion
                //pramod.misal 25.06.2024 GEOS2-5463
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Warehouses selectedPlant = SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.Name == serviceurl);
                if (selectedPlant != null)
                {
                    if (SRMCommon.Instance.WarehouseList.Any(x => x.IdWarehouse == selectedPlant.IdWarehouse))
                        SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse == selectedPlant.IdWarehouse);
                    else
                        SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList[0];
                }
                else
                {
                    SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList[0];
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
        private void NavigatePendingOrdersModuleView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePendingOrdersModuleView()...", category: Category.Info, priority: Priority.Low);
                if (SRMPurchaseOrderCellEditHelper.IsValueChanged)
                    SavechangesPurchaseOrderGrid();
                PendingPurchaseOrderViewModel pendingPurchaseOrderViewModel = new PendingPurchaseOrderViewModel();
                pendingPurchaseOrderViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.PendingPurchaseOrderView", pendingPurchaseOrderViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigatePendingOrdersModuleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigatePendingOrdersModuleView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees View
        /// </summary>
        private void NavigateProductPendingArticlesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductPendingArticlesView()...", category: Category.Info, priority: Priority.Low);
                if (SRMPurchaseOrderCellEditHelper.IsValueChanged)
                    SavechangesPurchaseOrderGrid();
                PendingArticleViewModel pendingArticleViewModel = new PendingArticleViewModel();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.PendingArticleView", pendingArticleViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductPendingArticlesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateProductPendingArticlesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5634]
        private void NavigateProductPendingReviewView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductPendingReviewView()...", category: Category.Info, priority: Priority.Low);
                PendingReviewViewModel pendingReviewViewModel = new PendingReviewViewModel();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.PendingReviewView", pendingReviewViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateProductPendingReviewView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateProductPendingReviewView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //completed Orders Rajashri [GEOS2-5460]
        private void NavigateClosedOrdersView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateClosedOrdersView()...", category: Category.Info, priority: Priority.Low);
                if (SRMPurchaseOrderCellEditHelper.IsValueChanged)
                    SavechangesPurchaseOrderGrid();
                CompletedOrdersViewModel closedOrdersViewModel = new CompletedOrdersViewModel();
                closedOrdersViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.CompletedOrdersView", closedOrdersViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateClosedOrdersView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateClosedOrdersView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateSuppliers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateSuppliers()...", category: Category.Info, priority: Priority.Low);
                if (SRMPurchaseOrderCellEditHelper.IsValueChanged)
                    SavechangesPurchaseOrderGrid();
                ArticleSupplierViewModel articleSupplierViewModel = new ArticleSupplierViewModel();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.ArticleSupplierView", articleSupplierViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateSuppliers()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateSuppliers()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to get Article Sleeping Days from Geos App Setting.
        ///  //[002][adhatkar]__[GEOS2-2064]
        /// </summary>
        private void GetArticleSleepingDays()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetArticleSleepingDays()...", category: Category.Info, priority: Priority.Low);

                List<GeosAppSetting> GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("30");

                if (GeosAppSettingList.Count > 0)
                {
                    SRMCommon.Instance.ArticleSleepDays = Convert.ToInt32(GeosAppSettingList[0].DefaultValue);
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

        private void NavigateMyPreferences(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);

                //MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                // MyPreferencesView myPreferencesView = new MyPreferencesView();
                //EventHandler handle = delegate { myPreferencesView.Close(); };
                //myPreferencesViewModel.RequestClose += handle;
                //myPreferencesView.DataContext = myPreferencesViewModel;
                //myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateMyPreferences()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4407][04/05/2023]
        private void NavigateSystemSettings(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction ...", category: Category.Info, priority: Priority.Low);


                SystemSettingsViewModel systemSettingsViewModel = new SystemSettingsViewModel();
                SystemSettingsView systemSettingsView = new SystemSettingsView();
                EventHandler handle = delegate { systemSettingsView.Close(); };
                systemSettingsViewModel.RequestClose += handle;
                systemSettingsView.DataContext = systemSettingsViewModel;
                systemSettingsView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        private void NavigateContacts()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateContacts()...", category: Category.Info, priority: Priority.Low);

                ContactsViewModel contactsViewModel = new ContactsViewModel();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.ContactsView", contactsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateContacts()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateContacts()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

		//  [nsatpute][12-06-2024] GEOS2-5463
        private void NavigateCatalogueDashboardView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateCatalogueDashboardView()...", category: Category.Info, priority: Priority.Low);
                CatalogueDashboardView catalogueDashboardView = new CatalogueDashboardView();

                CatalogueDashboardViewModel catalogueDashboardViewModel = new CatalogueDashboardViewModel();
                catalogueDashboardView.DataContext = catalogueDashboardViewModel;
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.CatalogueDashboardView", catalogueDashboardViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateCatalogueDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateCatalogueDashboardView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        // [nsatpute][21-01-2025][GEOS2-5725]
        private void OpenPurchasingReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPurchasingReport ...", category: Category.Info, priority: Priority.Low);

                PurchasingReportFilterViewModel purchasingReportViewModel = new PurchasingReportFilterViewModel();
                PurchasingReportFilterView purchasingReportView = new PurchasingReportFilterView();
                EventHandler handle = delegate { purchasingReportView.Close(); };
                purchasingReportViewModel.RequestClose += handle;
                purchasingReportView.DataContext = purchasingReportViewModel;
                purchasingReportView.ShowDialogWindow();
                if (purchasingReportViewModel.GenerateReport)
                {
                    DateTime fromDate = purchasingReportViewModel.FromDate;
                    DateTime toDate = purchasingReportViewModel.ToDate;
                    long idArticleSupplier = purchasingReportViewModel.SelectedSupplier == null ? 0 : purchasingReportViewModel.SelectedSupplier.IdArticleSupplier;
                    int idArticle = purchasingReportViewModel.SelectedArticle == null ? 0 : purchasingReportViewModel.SelectedArticle.IdArticle;
                    List<object> SelectedItems = purchasingReportViewModel.SelectedItems;
                    ArticlePurchaseReportView articlePurchaseReportView = new ArticlePurchaseReportView();
                    ArticlePurchaseReportViewModel articlePurchaseReportViewModel = new ArticlePurchaseReportViewModel(fromDate, toDate, idArticleSupplier, idArticle, SelectedItems);
                    articlePurchaseReportView.DataContext = articlePurchaseReportViewModel;
                    Service.Navigate("Emdep.Geos.Modules.SRM.Views.ArticlePurchaseReportView", articlePurchaseReportViewModel, null, this, true); ;
                }
                GeosApplication.Instance.Logger.Log("Method OpenPurchasingReport() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenPurchasingReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[pramod.misal][20-05-2025][GEOS2-5727]
        //private void OpenSalesReport(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenPurchasingReport ...", category: Category.Info, priority: Priority.Low);

        //        SalesReportFilterViewModel purchasingReportViewModel = new SalesReportFilterViewModel();
        //        SalesReportFilterView purchasingReportView = new SalesReportFilterView();
        //        EventHandler handle = delegate { purchasingReportView.Close(); };
        //        purchasingReportViewModel.RequestClose += handle;
        //        purchasingReportView.DataContext = purchasingReportViewModel;
        //        purchasingReportView.ShowDialogWindow();
        //        if (purchasingReportViewModel.GenerateReport)
        //        {
        //            DateTime fromDate = purchasingReportViewModel.FromDate;
        //            DateTime toDate = purchasingReportViewModel.ToDate;
        //            long idArticleSupplier = purchasingReportViewModel.SelectedSupplier == null ? 0 : purchasingReportViewModel.SelectedSupplier.IdArticleSupplier;
        //            int idArticle = purchasingReportViewModel.SelectedArticle == null ? 0 : purchasingReportViewModel.SelectedArticle.IdArticle;
        //            List<object> SelectedItems = purchasingReportViewModel.SelectedItems;
        //            ArticleSalesReportView articlePurchaseReportView = new ArticleSalesReportView();
        //            ArticlePurchaseReportViewModel articlePurchaseReportViewModel = new ArticlePurchaseReportViewModel(fromDate, toDate, idArticleSupplier, idArticle, SelectedItems);
        //            articlePurchaseReportView.DataContext = articlePurchaseReportViewModel;
        //            Service.Navigate("Emdep.Geos.Modules.SRM.Views.ArticlePurchaseReportView", articlePurchaseReportViewModel, null, this, true); ;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method OpenPurchasingReport() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenPurchasingReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[pramod.misal][20-05-2025][GEOS2-5727]
        //private void OpenreceptionReport(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenPurchasingReport ...", category: Category.Info, priority: Priority.Low);

        //        ReceptionReportFilterViewModel receptionReportFilterViewModel = new ReceptionReportFilterViewModel();
        //        ReceptionReportFilterView receptionReportFilterView = new ReceptionReportFilterView();
        //        EventHandler handle = delegate { receptionReportFilterView.Close(); };
        //        receptionReportFilterViewModel.RequestClose += handle;
        //        receptionReportFilterView.DataContext = receptionReportFilterViewModel;
        //        receptionReportFilterView.ShowDialogWindow();
        //        if (receptionReportFilterViewModel.GenerateReport)
        //        {
        //            DateTime fromDate = receptionReportFilterViewModel.FromDate;
        //            DateTime toDate = receptionReportFilterViewModel.ToDate;

        //            int idArticle = receptionReportFilterViewModel.SelectedArticle == null ? 0 : receptionReportFilterViewModel.SelectedArticle.IdArticle;
        //            List<object> SelectedItems = receptionReportFilterViewModel.SelectedItems;
        //            ArticleReceptionReportView articleReceptionReportView = new ArticleReceptionReportView();
        //            ArticleReceptionReportViewModel articleReceptionReportViewModel = new ArticleReceptionReportViewModel(fromDate, toDate, idArticle, SelectedItems);
        //            articleReceptionReportView.DataContext = articleReceptionReportViewModel;
        //            Service.Navigate("Emdep.Geos.Modules.SRM.Views.ArticleReceptionReportView", articleReceptionReportViewModel, null, this, true); ;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method OpenreceptionReport() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenreceptionReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[pramod.misal][20-05-2025][GEOS2-5727]
        //private void OpenManufacturersReport(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenManufacturersReport ...", category: Category.Info, priority: Priority.Low);

        //        ManufacturersReportFilterViewModel manufacturersReportFilterViewModel = new ManufacturersReportFilterViewModel();
        //        ManufacturersReportFilterView manufacturersReportFilterView = new ManufacturersReportFilterView();
        //        EventHandler handle = delegate { manufacturersReportFilterView.Close(); };
        //        manufacturersReportFilterViewModel.RequestClose += handle;
        //        manufacturersReportFilterView.DataContext = manufacturersReportFilterViewModel;
        //        manufacturersReportFilterView.ShowDialogWindow();
        //        if (manufacturersReportFilterViewModel.GenerateReport)
        //        {
        //            DateTime fromDate = manufacturersReportFilterViewModel.FromDate;
        //            DateTime toDate = manufacturersReportFilterViewModel.ToDate;

        //            int idArticle = manufacturersReportFilterViewModel.SelectedArticle == null ? 0 : manufacturersReportFilterViewModel.SelectedArticle.IdArticle;
        //            List<object> SelectedItems = manufacturersReportFilterViewModel.SelectedItems;
        //            ManufacturersReportView manufacturersReportView = new ManufacturersReportView();
        //            ManufacturersReportViewModel manufacturersReportViewModel = new ManufacturersReportViewModel(fromDate, toDate, idArticle, SelectedItems);
        //            manufacturersReportView.DataContext = manufacturersReportViewModel;
        //            Service.Navigate("Emdep.Geos.Modules.SRM.Views.ManufacturersReportView", manufacturersReportViewModel, null, this, true); ;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method OpenManufacturersReport() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenManufacturersReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        //private void OpenStockRoatationReport(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenStockRoatationReport ...", category: Category.Info, priority: Priority.Low);

        //        StockRotationReportFilterViewModel stockRotationReportFilterViewModel = new StockRotationReportFilterViewModel();
        //        StockRotationReportFilterView stockRotationReportFilterView = new StockRotationReportFilterView();
        //        EventHandler handle = delegate { stockRotationReportFilterView.Close(); };
        //        stockRotationReportFilterViewModel.RequestClose += handle;
        //        stockRotationReportFilterView.DataContext = stockRotationReportFilterViewModel;
        //        stockRotationReportFilterView.ShowDialogWindow();
        //        if (stockRotationReportFilterViewModel.GenerateReport)
        //        {
        //            DateTime fromDate = stockRotationReportFilterViewModel.FromDate;
        //            DateTime toDate = stockRotationReportFilterViewModel.ToDate;

        //            int idArticle = stockRotationReportFilterViewModel.SelectedArticle == null ? 0 : stockRotationReportFilterViewModel.SelectedArticle.IdArticle;
        //            List<object> SelectedItems = stockRotationReportFilterViewModel.SelectedItems;
        //            int idMaterialType = stockRotationReportFilterViewModel.SelectedMaterialType == null ? 0 : stockRotationReportFilterViewModel.SelectedMaterialType.IdMaterialType;

        //            StockRotationReportView manufacturersReportView = new StockRotationReportView();
        //            StockRotationReportViewModel manufacturersReportViewModel = new StockRotationReportViewModel(fromDate, toDate, idArticle, idMaterialType, SelectedItems);
        //            manufacturersReportView.DataContext = manufacturersReportViewModel;
        //            Service.Navigate("Emdep.Geos.Modules.SRM.Views.StockRotationReportView", manufacturersReportViewModel, null, this, true); ;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method OpenStockRoatationReport() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenStockRoatationReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[Rahul.Gadhave][GEOS2-5729][Date:23-05-2025]
        //private void OpenArticleDeliveryTimeReport(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenArticleDeliveryTimeReport ...", category: Category.Info, priority: Priority.Low);

        //        ArticleDeliveryTimeReportFilterViewModel receptionReportFilterViewModel = new ArticleDeliveryTimeReportFilterViewModel();
        //        ArticleDeliveryTimeReportFilterView receptionReportFilterView = new ArticleDeliveryTimeReportFilterView();
        //        EventHandler handle = delegate { receptionReportFilterView.Close(); };
        //        receptionReportFilterViewModel.RequestClose += handle;
        //        receptionReportFilterView.DataContext = receptionReportFilterViewModel;
        //        receptionReportFilterView.ShowDialogWindow();
        //        if (receptionReportFilterViewModel.GenerateReport)
        //        {
        //            int idArticle = receptionReportFilterViewModel.SelectedArticle == null ? 0 : receptionReportFilterViewModel.SelectedArticle.IdArticle;
        //            List<object> SelectedItems = receptionReportFilterViewModel.SelectedItems;
        //            long idArticleSupplier = receptionReportFilterViewModel.SelectedSupplier == null ? 0 : receptionReportFilterViewModel.SelectedSupplier.IdArticleSupplier;
        //            ArticleDeliveryTimeReportView articleReceptionReportView = new ArticleDeliveryTimeReportView();
        //            ArticleDeliveryTimeReportViewModel articleReceptionReportViewModel = new ArticleDeliveryTimeReportViewModel(idArticle, idArticleSupplier, SelectedItems);
        //            articleReceptionReportView.DataContext = articleReceptionReportViewModel;
        //            Service.Navigate("Emdep.Geos.Modules.SRM.Views.ArticleDeliveryTimeReportView", articleReceptionReportViewModel, null, this, true); ;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method OpenArticleDeliveryTimeReport() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenArticleDeliveryTimeReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
        private void PreOrderViewAction()
        {
            PreOrderView reOrderView = new PreOrderView();
            PreOrderViewModel reOrderViewModel = new PreOrderViewModel();
            reOrderView.DataContext = reOrderViewModel;
            reOrderViewModel.Init();
            Service.Navigate("Emdep.Geos.Modules.SRM.Views.PreOrderView", reOrderViewModel, null, this, true);
        }

        #endregion
    }
}
