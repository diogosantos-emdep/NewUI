using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.Modules.PLM.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    //[PRAMOD.MISAL][GEOS2-4442][29-08-2023]
    public class PCMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Properties
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private ProductTypeArticleViewModel objProductTypeArticleViewModel;
        public ProductTypeArticleViewModel ObjProductTypeArticleViewModel
        {
            get { return objProductTypeArticleViewModel; }
            set
            {
                objProductTypeArticleViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProductTypeArticleViewModel"));
            }
        }

        private DetectionsViewModel objProductTypeDetectionViewModel;
        public DetectionsViewModel ObjProductTypeDetectionViewModel
        {
            get { return objProductTypeDetectionViewModel; }
            set
            {
                objProductTypeDetectionViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProductTypeDetectionViewModel"));
            }
        }
        //[plahange][3956]
        private StructuresDetectionsViewModel objProductTypeStructuresDetectionViewModel;
        public StructuresDetectionsViewModel ObjProductTypeStructuresDetectionViewModel
        {
            get { return objProductTypeStructuresDetectionViewModel; }
            set
            {
                objProductTypeStructuresDetectionViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProductTypeStructuresDetectionViewModel"));
            }
        }

        private BasePriceListGridViewModel objBasePriceListViewModel;
        public BasePriceListGridViewModel ObjBasePriceListViewModel
        {
            get { return objBasePriceListViewModel; }
            set
            {
                objBasePriceListViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjBasePriceListViewModel"));
            }
        }
        private CustomerPriceListGridViewModel objCustomerPriceListViewModel;
        public CustomerPriceListGridViewModel ObjCustomerPriceListViewModel
        {
            get { return objCustomerPriceListViewModel; }
            set
            {
                objCustomerPriceListViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("objCustomerPriceListViewModel"));
            }
        }
        private PriceListPermissionsViewModel objPriceListPermissionsViewModel;
        public PriceListPermissionsViewModel ObjPriceListPermissionsViewModel
        {
            get { return objPriceListPermissionsViewModel; }
            set
            {
                objPriceListPermissionsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPriceListPermissionsViewModel"));
            }
        }

        //[001][kshinde][27/07/2022][GEOS2-3099]
        private DiscountsListGridViewModel objDiscountsListViewModel;
        public DiscountsListGridViewModel ObjDiscountsListViewModel
        {
            get { return objDiscountsListViewModel; }
            set
            {
                objDiscountsListViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjDiscountsListViewModel"));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// [001][GEOS2-2528][avpawar][Move "Detections" section to "PRODUCTS" section]
        /// [002][GEOS2-3341][cpatil][2021-09-21][Sr N 6- Synchronization between PCM and ECOS. [#PLM69]]
        /// </summary>
        public PCMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PCMMainViewModel()...", category: Category.Info, priority: Priority.Low);
                
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();
                
                TileCollection = new ObservableCollection<TileBarItemsHelper>();
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 33))
                {
                    TileBarItemsHelper tileBarItemsHelperProduct = new TileBarItemsHelper();
                    tileBarItemsHelperProduct.Caption = System.Windows.Application.Current.FindResource("Products").ToString();
                    tileBarItemsHelperProduct.BackColor = "#CC6D00";
                    tileBarItemsHelperProduct.GlyphUri = "wProductCatalogue.png";
                    tileBarItemsHelperProduct.Visibility = Visibility.Visible;

                    TileBarItemsHelper tileBarItemModule = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("Module").ToString(),
                        BackColor = "#CC6D00",
                        GlyphUri = "bModules.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateProductModuleView)

                    };
                    tileBarItemsHelperProduct.Children.Add(tileBarItemModule);
                    
                    //[001] Start
                    TileBarItemsHelper tileBarItemDetections = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("CaptionDetections").ToString(),
                        BackColor = "#CC6D00",
                        GlyphUri = "Detections.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateDetectionsView)
                    };
                    tileBarItemsHelperProduct.Children.Add(tileBarItemDetections);
                    //[001] End

                    TileBarItemsHelper tileBarItemStructure = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("Structures").ToString(),
                        BackColor = "#CC6D00",
                        GlyphUri = "Structures.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateProductModuleStructureView)

                    };
                    tileBarItemsHelperProduct.Children.Add(tileBarItemStructure);
                    //003Start [Plahange][GEOS2-3956]
                    TileBarItemsHelper tileBarItemModuleDetectionStructures = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("StructuresDetections").ToString(),
                        BackColor = "#CC6D00",
                        GlyphUri = "Detections.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateStructuresDetectionsView)

                    };
                    tileBarItemsHelperProduct.Children.Add(tileBarItemModuleDetectionStructures);
                    //003End

                    TileBarItemsHelper tileBarItemArticle = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("Article").ToString(),
                        BackColor = "#CC6D00",
                        GlyphUri = "bProductCatalogue.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateProductArticleView)
                    };
                    tileBarItemsHelperProduct.Children.Add(tileBarItemArticle);

                    TileCollection.Add(tileBarItemsHelperProduct);
                }

                //[pramod.misal][GEOS2-4440][25-08-2023]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 33))
                {
                    TileBarItemsHelper tileBarItemsSoftware = new TileBarItemsHelper();
                    tileBarItemsSoftware.Caption = System.Windows.Application.Current.FindResource("Software").ToString();
                    tileBarItemsSoftware.BackColor = "#6666FF";
                    tileBarItemsSoftware.GlyphUri = "Software.png";
                    tileBarItemsSoftware.Visibility = Visibility.Visible;

                    //[1]HardlockLicenses
                    TileBarItemsHelper tileBarItemHardlockLicenses = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("HardlockLicenses").ToString(),
                        BackColor = "#6666FF",
                        GlyphUri = "Hardlocklicens.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateHardLockLicensesView)

                    };
                    tileBarItemsSoftware.Children.Add(tileBarItemHardlockLicenses);

                    //[2]Freeplugins
                    TileBarItemsHelper tileBarItemFreeplugins = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("Freeplugins").ToString(),
                        BackColor = "#6666FF",
                        GlyphUri = "FreePlugins.png",
                        Visibility = Visibility.Visible,
                        //[pramod.misal][GEOS2-4440][28-08-2023]
                        NavigateCommand = new DelegateCommand(() => { Service.Navigate(FreePluginsViewAction(), null, this); })
                    };
                    tileBarItemsSoftware.Children.Add(tileBarItemFreeplugins);

                    TileCollection.Add(tileBarItemsSoftware);
                }
            

                if (GeosApplication.Instance.IsPLMPermissionView || GeosApplication.Instance.IsPLMPermissionChange)
                {
                    //Price List Start
                    TileBarItemsHelper tileBarItemsHelperPriceList = new TileBarItemsHelper();
                    tileBarItemsHelperPriceList.Caption = System.Windows.Application.Current.FindResource("PriceList").ToString();
                    tileBarItemsHelperPriceList.BackColor = "#aaaaaa";
                    tileBarItemsHelperPriceList.GlyphUri = "plm54X54.png";
                    tileBarItemsHelperPriceList.Visibility = Visibility.Visible;

                    TileBarItemsHelper tileBarBasePrice = new TileBarItemsHelper()
                    {
                        Caption = "Base Prices",
                        BackColor = "#aaaaaa",
                        GlyphUri = "bBasePrice.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateBasePriceListView)

                    };
                    tileBarItemsHelperPriceList.Children.Add(tileBarBasePrice);

                    TileBarItemsHelper tileBarCustomerPrice = new TileBarItemsHelper()
                    {
                        Caption = "Customer Prices",
                        BackColor = "#aaaaaa",
                        GlyphUri = "bCustomerPrice.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateCustomerPriceListView)

                    };
                    tileBarItemsHelperPriceList.Children.Add(tileBarCustomerPrice);

                   /* TileBarItemsHelper tileBarCompetitorPrice = new TileBarItemsHelper()
                    {
                        Caption = "Competitor Prices",
                        BackColor = "#aaaaaa",
                        GlyphUri = "bCompetitorPrice.png",
                        Visibility = Visibility.Visible,
                        //NavigateCommand = new DelegateCommand(NavigateCompetitorPriceListView)

                    };
                    tileBarItemsHelperPriceList.Children.Add(tileBarCompetitorPrice);*/

                    //[001][kshinde][27/07/2022][GEOS2-3099]
                    TileBarItemsHelper tileBarDiscounts = new TileBarItemsHelper()
                    {
                        Caption = "Discounts",
                        BackColor = "#aaaaaa",
                        GlyphUri = "Discount.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateDiscountsListView)

                    };
                    tileBarItemsHelperPriceList.Children.Add(tileBarDiscounts);

                    TileCollection.Add(tileBarItemsHelperPriceList);
                }


                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("PCMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                if (GeosApplication.Instance.IsPLMPermissionView || GeosApplication.Instance.IsPLMPermissionChange)
                {
                    TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("AdditionalArticleCosts").ToString(),
                        BackColor = "#C7BFE6",
                        GlyphUri = "bArticleCost.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigateAdditionalArticleCost)
                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);
                }

                // Categories menu [plahange][17-02-23]
                TileBarItemsHelper tileBarItemConfigurationCategories = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("Categories").ToString(),
                    BackColor = "#C7BFE6",
                    GlyphUri = "confcategories.png",
                    Visibility = Visibility.Visible,
                     NavigateCommand = new DelegateCommand(NavigateConfigurationCategoriesView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationCategories);


                // Articles Categories Mapping menu [chitra.girigosavi][GEOS2-4808][04/10/2023]
                TileBarItemsHelper tileBarItemConfigurationArticlesCategoriesMapping = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("ArticlesCategoriesMapping").ToString(),
                    BackColor = "#C7BFE6",
                    GlyphUri = "ArticlesCategoriesMapping.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigateArticlesCategoriesMapping)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationArticlesCategoriesMapping);

                //MyPreference_Black As per User permission
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 33))
                {
                    TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("MyPreferences").ToString(),
                        BackColor = "#C7BFE6",
                        GlyphUri = "MyPreference_Black.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(NavigateMyPreferences)
                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);
                }
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 59) || GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 60))
                {
                    //[002]
                    TileBarItemsHelper tileBarItemConfigurationSystemSettings = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("PCMSystemSetting").ToString(),
                        BackColor = "#C7BFE6",
                        GlyphUri = "bSystemSettings.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(SystemSettingsViewAction)
                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationSystemSettings);

                    //[002]
                    //System Settings Visibility
                    //if (!GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization)
                    //{
                    //    tileBarItemConfigurationSystemSettings.Visibility = Visibility.Collapsed;
                    //}
                }
                // Price List Permission menu shown as per PLM User permission
                if (GeosApplication.Instance.IsPLMPermissionAdmin)
                {
                    //[002]
                    TileBarItemsHelper tileBarItemConfigurationSystemSettings = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("PLMPermissions").ToString(),
                        BackColor = "#C7BFE6",
                        GlyphUri = "bprice_list_permissions.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(NavigatePriceListPermissions)
                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationSystemSettings);
                }
                TileCollection.Add(tileBarItemsHelperConfiguration);


                GeosApplication.Instance.Logger.Log("Constructor Constructor PCMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor PCMMainViewModel() Method - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in  Constructor PCMMainViewModel() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PCMMainViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        #endregion

        #region Methods

        private void NavigateProductModuleStructureView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductModuleStructureView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                ProductTypeViewModel productTypeViewModel = new ProductTypeViewModel();
                productTypeViewModel.iSStructureVisible = true;
                productTypeViewModel.Title = System.Windows.Application.Current.FindResource("Structures").ToString();
                productTypeViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeGridView", productTypeViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductModuleStructureView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductModuleView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }        
        private void NavigateProductModuleView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductModuleView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                ProductTypeViewModel productTypeViewModel = new ProductTypeViewModel();
                productTypeViewModel.iSModuleVisible = true;
                productTypeViewModel.Title = System.Windows.Application.Current.FindResource("Module").ToString();
                productTypeViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeGridView", productTypeViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductModuleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductModuleView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateProductArticleView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductArticleView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                ProductTypeArticleViewModel productTypeArticleViewModel = new ProductTypeArticleViewModel();
                productTypeArticleViewModel.Init();
                ObjProductTypeArticleViewModel = productTypeArticleViewModel;
                PCMCommon.Instance.GetProductTypeArticleViewModelDetails = ObjProductTypeArticleViewModel;
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeArticleView", productTypeArticleViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductArticleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductArticleView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateDetectionsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()...", category: Category.Info, priority: Priority.Low);
                
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                DetectionsViewModel detectionsViewModel = new DetectionsViewModel();
                detectionsViewModel.Init();
                ObjProductTypeDetectionViewModel = detectionsViewModel;
                PCMCommon.Instance.GetProductTypeDetectionViewModelDetails = ObjProductTypeDetectionViewModel;
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.DetectionsView", detectionsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateDetectionsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[plahange][GEOS2-3956]
        private void NavigateStructuresDetectionsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateStructuresDetectionsView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                StructuresDetectionsViewModel structureviewModel=new StructuresDetectionsViewModel();
                // DetectionsViewModel detectionsViewModel = new DetectionsViewModel();
                structureviewModel.Init();
                ObjProductTypeStructuresDetectionViewModel = structureviewModel;
                PCMCommon.Instance.GetProductTypeStructureDetectionViewModelDetails = ObjProductTypeStructuresDetectionViewModel;
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.StructuresDetectionsView", structureviewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateStructuresDetectionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateStructuresDetectionsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-4440][25-08-2023]
        private FreePluginsView FreePluginsViewAction()
        {
            FreePluginsView freePluginsView = new FreePluginsView();
            FreePluginsViewModel freepluginsviewmodel = new FreePluginsViewModel();
            freePluginsView.DataContext = freepluginsviewmodel;

            freepluginsviewmodel.Init();
            return freePluginsView;
        }

        //
        private void NavigateBasePriceListView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateBasePriceListView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();           
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                BasePriceListGridViewModel basePriceListGridViewModel = new BasePriceListGridViewModel();
                basePriceListGridViewModel.Init();

                ObjBasePriceListViewModel = basePriceListGridViewModel;
                PCMCommon.Instance.GetBasePriceListViewModel = ObjBasePriceListViewModel;
                Service.Navigate("Emdep.Geos.Modules.PLM.Views.BasePriceListGridView", basePriceListGridViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateBasePriceListView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateBasePriceListView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateCustomerPriceListView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateCustomerPriceListView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                CustomerPriceListGridViewModel customerPriceListGridViewModel = new CustomerPriceListGridViewModel();
                customerPriceListGridViewModel.Init();
                
                ObjCustomerPriceListViewModel = customerPriceListGridViewModel;
                PCMCommon.Instance.GetCustomerPriceListViewModel = ObjCustomerPriceListViewModel;
                Service.Navigate("Emdep.Geos.Modules.PLM.Views.CustomerPriceListGridView", customerPriceListGridViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateCustomerPriceListView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateCustomerPriceListView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][kshinde][27/07/2022][GEOS2-3099]
        private void NavigateDiscountsListView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDiscountsListView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                DiscountsListGridViewModel discountsListGridViewModel = new DiscountsListGridViewModel();
                discountsListGridViewModel.Init();

                ObjDiscountsListViewModel = discountsListGridViewModel;
                PCMCommon.Instance.GetDiscountsListViewModel = ObjDiscountsListViewModel;
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.DiscountsListGridView", discountsListGridViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDiscountsListView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateCustomerPriceListView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateMyPreferences(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateMyPreferences()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateAdditionalArticleCost()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateAdditionalArticleCost..."), category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                AdditionalArticleCostViewModel additionalArticleCostViewModel = new AdditionalArticleCostViewModel();
                additionalArticleCostViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.AdditionalArticleCostView", additionalArticleCostViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateAdditionalArticleCost..."), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateAdditionalArticleCost()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SystemSettingsViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

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

                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigatePriceListPermissions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigatePriceListPermissions..."), category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInArticleGrid();
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInDetectionGrid();
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                    SavechangesInPriceListPermissionsGrid();

                PriceListPermissionsViewModel priceListPermissionsViewModel = new PriceListPermissionsViewModel();
                priceListPermissionsViewModel.Init();
                ObjPriceListPermissionsViewModel = priceListPermissionsViewModel;
                PCMCommon.Instance.GetPriceListPermissionsViewModel = ObjPriceListPermissionsViewModel;
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.PriceListPermissionsView", priceListPermissionsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log(string.Format("Method NavigatePriceListPermissions..."), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePriceListPermissions()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SavechangesInArticleGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ProductTypeArticleViewMultipleCellEditHelper.Checkview == "ItemListTableView")
                    {
                        ObjProductTypeArticleViewModel.UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                }
                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {

            }
        }
        public void SavechangesInDetectionGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    //[rdixit][GEOS2-3970][01.12.2022] 
                    if (ObjProductTypeDetectionViewModel != null)
                    {
                        if (DetectionsViewMultipleCellEditHelper.Checkview == "ItemListTableView")
                        {
                            ObjProductTypeDetectionViewModel.UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                        }
                    }
                    else if (ObjProductTypeStructuresDetectionViewModel != null)
                    {
                        if (DetectionsViewMultipleCellEditHelper.Checkview == "ItemListTableView")
                        {
                            ObjProductTypeStructuresDetectionViewModel.UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                        }
                    }
                }
                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {

            }
        }
        public void SavechangesInPriceListPermissionsGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (PriceListPermissionMultipleCellEditHelper.Checkview == "PriceListPermissionsTableView")
                    {
                        ObjPriceListPermissionsViewModel.InsertUpdateMultipleRowsPriceListPermissions(PriceListPermissionMultipleCellEditHelper.Viewtableview);
                    }
                }
                PriceListPermissionMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {

            }
        }
        //[plahange][GEOS2-2544][21-2-23]
        private void NavigateConfigurationCategoriesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductArticleView()...", category: Category.Info, priority: Priority.Low);

                //if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                //    SavechangesInArticleGrid();
                //if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                //    SavechangesInDetectionGrid();
                //if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                //    SavechangesInPriceListPermissionsGrid();

                ConfigurationCategoriesViewModel configurationCategoriesViewModel = new ConfigurationCategoriesViewModel();
                configurationCategoriesViewModel.Init();
               
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ConfigurationCategoriesView", configurationCategoriesViewModel, null, this, true);
               // ObjProductTypeArticleViewModel = configurationCategoriesViewModel;
               // PCMCommon.Instance.GetProductTypeArticleViewModelDetails = ObjProductTypeArticleViewModel;
              
                GeosApplication.Instance.Logger.Log("Method NavigateProductArticleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductArticleView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4441][21/09/2023]
        private void NavigateHardLockLicensesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateHardLockLicensesView()...", category: Category.Info, priority: Priority.Low);
                HardLockLicensesViewModel hardLockLicensesViewModel = new HardLockLicensesViewModel();
                hardLockLicensesViewModel.Title = System.Windows.Application.Current.FindResource("HardLockLicenses").ToString();
                hardLockLicensesViewModel.Init();

                Service.Navigate("Emdep.Geos.Modules.PCM.Views.HardLockLicensesView", hardLockLicensesViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateHardLockLicensesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateHardLockLicensesView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4874]
        private void NavigateArticlesCategoriesMapping()
         {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateArticlesCategoriesMapping()...", category: Category.Info, priority: Priority.Low);
                ArticlesCategoriesMappingViewModel articlesCategoriesMappingViewModel = new ArticlesCategoriesMappingViewModel();                
                ArticlesCategoriesMappingView ArticlesCategoriesMappingView = new ArticlesCategoriesMappingView();
                EventHandler handle = delegate { ArticlesCategoriesMappingView.Close(); };
                articlesCategoriesMappingViewModel.RequestClose += handle;
                ArticlesCategoriesMappingView.DataContext = articlesCategoriesMappingViewModel;
                articlesCategoriesMappingViewModel.Init();
                ArticlesCategoriesMappingView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method NavigateArticlesCategoriesMapping()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateArticlesCategoriesMapping()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
