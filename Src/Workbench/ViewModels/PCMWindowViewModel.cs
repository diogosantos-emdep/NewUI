using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Modules.PCM.ViewModels;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Workbench.Views;

namespace Workbench.ViewModels
{
    public class PCMWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Declaration

        private string showHideMenuButtonToolTip;

        private string moduleName;
        private TableView view;

        public static TableView ColnedView;
        public PriceListPermissionsViewModel objPriceListPermissionsViewModel;


        private INavigationService NavigationService { get { return this.GetService<INavigationService>(); } }

        public string ShowHideMenuButtonToolTip
        {
            get { return showHideMenuButtonToolTip; }
            set
            {
                showHideMenuButtonToolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        private ProductTypeArticleViewModel objProductTypeArticleViewModel;
        private DetectionsViewModel objProductTypeDetectionViewModel;
        public Workstation LoginWorkstation { get; set; }
        public System.Windows.Forms.DialogResult DialogResult { get; set; }
        private string visible;//[Sudhir.Jangra][GEOS2-2597][30/01/2023]
        private string moduleShortName;
        #endregion

        #region Properties
        public PriceListPermissionsViewModel ObjPriceListPermissionsViewModel
        {
            get { return objPriceListPermissionsViewModel; }
            set
            {
                objPriceListPermissionsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPriceListPermissionsViewModel"));
            }
        }

        public ProductTypeArticleViewModel ObjProductTypeArticleViewModel
        {
            get { return objProductTypeArticleViewModel; }
            set
            {
                objProductTypeArticleViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProductTypeArticleViewModel"));
            }
        }

        public DetectionsViewModel ObjProductTypeDetectionViewModel
        {
            get { return objProductTypeDetectionViewModel; }
            set
            {
                objProductTypeDetectionViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProductTypeDetectionViewModel"));
            }
        }
        public string Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        string announcement;
        public string Announcement
        {
            get { return announcement; }
            set
            {
                announcement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Announcement"));
            }
        }

        public string ModuleShortName
        {
            get
            {
                return moduleShortName;
            }

            set
            {
                moduleShortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleShortName"));
            }
        }
        #endregion


        #region Public Commands

        public ICommand HideTileBarButtonClickCommand { get; set; }
        public ICommand BackButtonClickCommand { get; set; }
        public ICommand ShortcutCommand { get; set; }//[Sudhir.jangra][GEOS2-2597][30/01/2023]
        public ICommand Announcement_ClickCommand { get; set; }//[Sudhir.jangra][GEOS2-2597][30/01/2023]

        #endregion  // Public Commands

        #region Constructor

        public PCMWindowViewModel()
        {
            try
            {
                HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu
                ShortcutCommand = new DelegateCommand<KeyEventArgs>(ShortcutAction);//[Sudhir.Jangra][GEOS2-2597][30/01/2023]
                /*Announcement_ClickCommand = new DelegateCommand<KeyEventArgs>(Announcement_Click);*///[Sudhir.Jangra][GEOS2-2597][30/01/2023]
                Announcement_ClickCommand = new DelegateCommand<object>(Announcement_Click);
                BackButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(BackButtonClickCommandAction);

                if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
                {
                    ModuleShortName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 8).Select(s => s.Acronym).FirstOrDefault();
                    ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 8).Select(s => s.Name).FirstOrDefault();
                }
                else
                {
                    ModuleShortName = "PCM";
                    ModuleName = "Product Catalogue Mangement";
                }
                //Shubham[skadam] GEOS2-2597 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 1  06 02 2023
                PCMCommon.Instance.GetShortcuts();
                Announcement = PCMCommon.Instance.Announcement;
            }
            catch (Exception ex)
            {

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

        public event EventHandler RequestClose;

        #endregion // Events

        #region Methods
        #region GEOS2-2597
        //Shubham[skadam] GEOS2-2597 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 1  06 02 2023
        private void Announcement_Click(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method Announcement_Click ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                string Selected = ((System.Windows.UIElement)((DevExpress.Xpf.Accordion.AccordionControl)obj).SelectedItem).Uid;
                // shortcuts
                // Get shortcut for Announcement
                if (Selected == "Announcement")
                {
                    Processing();
                    AnnouncementView pCMAnnouncementView = new AnnouncementView();
                    AnnouncementViewModel pCMAnnouncementViewModel = new AnnouncementViewModel();
                    EventHandler handle = delegate { pCMAnnouncementView.Close(); };
                    pCMAnnouncementViewModel.RequestClose += handle;
                    pCMAnnouncementView.DataContext = pCMAnnouncementViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    pCMAnnouncementView.ShowDialogWindow();
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Announcement_Click....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Opportunity_Click...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Processing()
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
                        Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new Emdep.Geos.Modules.PCM.Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)//[Sudhir.Jangra][GEOS2-2597][30/01/2023]
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                PCMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        private void HideTileBarButtonClickCommandAction(RoutedEventArgs obj)
        {
            if (GeosApplication.Instance.TileBarVisibility == Visibility.Collapsed)
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Visible;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString(); //Hide menu
            }
            else
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Collapsed;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("ShowMenuButtonToolTip").ToString(); // ShowMenu
            }
        }

        public void BackButtonClickCommandAction(RoutedEventArgs obj)
        {
            try
            {
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInArticleGrid();
                }
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInDetectionGrid();
                }
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                {
                    SavechangesInPriceListPermissionGrid();
                }
                GeosApplication.NavigationServiceOnGeosWorkbenchScreen.GoBack();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method BackButtonClickCommandAction in PCM WindowViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SavechangesInPriceListPermissionGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (PriceListPermissionMultipleCellEditHelper.Checkview == "PriceListPermissionsTableView")
                    {
                        PriceListPermissionsViewModel priceListPermissionsViewModel = new PriceListPermissionsViewModel();
                        priceListPermissionsViewModel.Init();
                        ObjPriceListPermissionsViewModel = priceListPermissionsViewModel;
                        ObjPriceListPermissionsViewModel = PCMCommon.Instance.GetPriceListPermissionsViewModel;
                        ObjPriceListPermissionsViewModel.InsertUpdateMultipleRowsPriceListPermissions(PriceListPermissionMultipleCellEditHelper.Viewtableview);
                    }
                }
                else
                {
                    view = PriceListPermissionMultipleCellEditHelper.Viewtableview;
                    PriceListPermissionMultipleCellEditHelper.SetIsValueChanged(view, false);
                    PriceListPermissionMultipleCellEditHelper.IsValueChanged = false;
                    return;
                }

                view = PriceListPermissionMultipleCellEditHelper.Viewtableview;
                PriceListPermissionMultipleCellEditHelper.SetIsValueChanged(view, false);
                PriceListPermissionMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SavechangesInPriceListPermissionGrid in PCMWindowViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                        ProductTypeArticleViewModel productTypeArticleViewModel = new ProductTypeArticleViewModel();
                        productTypeArticleViewModel.Init();
                        ObjProductTypeArticleViewModel = productTypeArticleViewModel;
                        ObjProductTypeArticleViewModel = PCMCommon.Instance.GetProductTypeArticleViewModelDetails;
                        ObjProductTypeArticleViewModel.UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);

                    }
                }
                else
                {
                    view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                    ProductTypeArticleViewMultipleCellEditHelper.IsLoad = true;
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                    return;
                }


                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SavechangesInArticleGrid in PCM WindowViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void SavechangesInDetectionGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (DetectionsViewMultipleCellEditHelper.Checkview == "ItemListTableView")
                    {
                        DetectionsViewModel productTypeArticleViewModel = new DetectionsViewModel();
                        productTypeArticleViewModel.Init();
                        ObjProductTypeDetectionViewModel = productTypeArticleViewModel;
                        ObjProductTypeDetectionViewModel = PCMCommon.Instance.GetProductTypeDetectionViewModelDetails;
                        ObjProductTypeDetectionViewModel.UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);

                    }
                }
                else
                {
                    view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                    DetectionsViewMultipleCellEditHelper.IsLoad = true;
                    DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                    return;
                }


                view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SavechangesInArticleGrid in PCM WindowViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
        }

        #endregion // Methods
    }
}
