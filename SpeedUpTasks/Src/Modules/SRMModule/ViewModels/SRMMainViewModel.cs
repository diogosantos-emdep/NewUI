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
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


        //ISRMService HrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Properties

        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

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
                FillWarehouseList();

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

                //Suppliers
                TileBarItemsHelper tileBarItemsHelperSupplier = new TileBarItemsHelper();
                tileBarItemsHelperSupplier.Caption = System.Windows.Application.Current.FindResource("Suppliers").ToString();
                tileBarItemsHelperSupplier.BackColor = "#00879C";
                tileBarItemsHelperSupplier.GlyphUri = "Supplier.png";
                tileBarItemsHelperSupplier.Visibility = Visibility.Visible;
                tileBarItemsHelperSupplier.NavigateCommand = new DelegateCommand(NavigateSuppliers);
                tileBarItemsHelperSupplier.Children = new ObservableCollection<TileBarItemsHelper>();

                //TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                //{
                //    Caption = System.Windows.Application.Current.FindResource("CrmMainViewModelMyPreferences").ToString(),
                //    BackColor = "#00879C",
                //    GlyphUri = "MyPreference_Black.png",
                //    Visibility = Visibility.Visible,
                //    NavigateCommand = new DelegateCommand<object>(NavigateSuppliers)
                //};
                //tileBarItemsHelperSupplier.Children.Add(tileBarItemConfiguration);


                TileCollection.Add(tileBarItemsHelperSupplier);

                //TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                //tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("SRMConfiguration").ToString();
                //tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                //tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                //tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                //tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                //TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                //{
                //    Caption = System.Windows.Application.Current.FindResource("SRMMyPreferences").ToString(),
                //    BackColor = "#00BFFF",
                //    GlyphUri = "MyPreference_Black.png",
                //    Visibility = Visibility.Visible,
                //    NavigateCommand = new DelegateCommand<object>(NavigateMyPreferences)
                //};
                //tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

                //TileCollection.Add(tileBarItemsHelperConfiguration);

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

        /// <summary>
        /// Method for fill warehouse list.
        /// </summary>
        private void FillWarehouseList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseList...", category: Category.Info, priority: Priority.Low);

                SRMCommon.Instance.WarehouseList = new List<Warehouses>();
                SRMCommon.Instance.WarehouseList = WarehouseService.GetAllWarehousesByUserPermission_V2034(GeosApplication.Instance.ActiveUser.IdUser);

                if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedwarehouseId"))
                {
                    if (SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]) != null)
                        SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]);
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

                PendingArticleViewModel pendingArticleViewModel = new PendingArticleViewModel();
                Service.Navigate("Emdep.Geos.Modules.SRM.Views.PendingArticleView", pendingArticleViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductPendingArticlesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateProductPendingArticlesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateSuppliers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateSuppliers()...", category: Category.Info, priority: Priority.Low);

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
                //MyPreferencesView myPreferencesView = new MyPreferencesView();
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
        #endregion
    }
}
