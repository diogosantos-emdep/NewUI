using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Modules.PCM.Views;
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

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class PCMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Properties
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

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

        #endregion

        #region Constructor

        /// <summary>
        /// [001][GEOS2-2528][avpawar][Move "Detections" section to "PRODUCTS" section]
        /// </summary>
        public PCMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PCMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

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

                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("PCMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("MyPreferences").ToString(),
                    BackColor = "#00BFFF",
                    GlyphUri = "MyPreference_Black.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(NavigateMyPreferences)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

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
                {
                    SavechangesInArticleGrid();
                }
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

        /// <summary>
        /// Method for Navigate Dashboard
        /// </summary>

        private void NavigateProductModuleView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateProductModuleView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInArticleGrid();
                }
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

                ProductTypeArticleViewModel productTypeArticleViewModel = new ProductTypeArticleViewModel();
                productTypeArticleViewModel.Init();
                ObjProductTypeArticleViewModel = productTypeArticleViewModel;
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeArticleView", productTypeArticleViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateProductArticleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateProductArticleView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NavigateMyPreferences(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInArticleGrid();
                }
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

        private void NavigateDetectionsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()...", category: Category.Info, priority: Priority.Low);

                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInArticleGrid();
                }
                DetectionsViewModel detectionsViewModel = new DetectionsViewModel();
                detectionsViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.DetectionsView", detectionsViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateDetectionsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
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
            catch(Exception ex)
            {

            }
        }

        #endregion

    }
}
