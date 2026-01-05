using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    /// <summary>
    /// [nsatpute][12-06-2024] GEOS2-5463
    /// </summary>
    public class CatalogueDashboardViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        #endregion
        #region Constructor
        public CatalogueDashboardViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor CatalogueDashboardViewModel...", category: Category.Info, priority: Priority.Low);
                ShowSplashScreen();
                FillWarehouseCategories();
                FillConditionOperators();
                FillSupplierList();
                IsStockChangesEnabled = false;
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                ClearCommand = new DelegateCommand<object>(ClearCommandAction);
                SearchCommand = new DelegateCommand<object>(SearchCommandAction);
                ViewArticleHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(ViewArticleHyperlinkClickCommandAction);
                ChildNodeSelectUnselectCommand = new DelegateCommand<object>(ChildNodeSelectUnselectCommandAction);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(CommandWarehouseEditValueChangedCommandAction);
                ClearConditionalOperator = Visibility.Hidden;
                ClearSuppliers = Visibility.Hidden;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor CatalogueDashboardViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CatalogueDashboardViewModel() Constructor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
        #region ICommands
        public ICommand ClearCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ViewArticleHyperlinkClickCommand { get; set; }
        public ICommand ChildNodeSelectUnselectCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; set; }
        #endregion
        #region Declaration
        private ObservableCollection<WarehouseCategory> warehouseCategories;
        private ObservableCollection<string> conditionalOperators;
        private string selectedConditionalOperator;
        private ObservableCollection<ArticleSupplier> supplierList;
        private List<object> selectedSupplierList;
        private int selectedSearchIndex;
        public ObservableCollection<WarehouseCategory> WarehouseCategories
        {
            get { return warehouseCategories; }
            set
            {
                warehouseCategories = value;
                OnPropertyChanged("WarehouseCategories");
            }
        }

        public ObservableCollection<string> ConditionalOperators
        {
            get { return conditionalOperators; }
            set
            {
                conditionalOperators = value;
                OnPropertyChanged(nameof(ConditionalOperators));
            }
        }
        public string SelectedConditionalOperator
        {
            get { return selectedConditionalOperator; }
            set
            {
                selectedConditionalOperator = value;
                OnPropertyChanged(nameof(SelectedConditionalOperator));

                if (value == null || SelectedConditionalOperator.Trim().Length == 0)
                    ClearConditionalOperator = Visibility.Hidden;
                else
                    ClearConditionalOperator = Visibility.Visible;
                IsStockChangesEnabled = !string.IsNullOrEmpty(selectedConditionalOperator);
            }
        }
        private ObservableCollection<Catalogue> catelogueList;
        private string reference;

        public string Reference
        {
            get { return reference; }
            set { reference = value; OnPropertyChanged("Reference"); }
        }
        private string stockValue;

        public string StockValue
        {
            get { return stockValue; }
            set { stockValue = value; OnPropertyChanged("StockValue"); }
        }


        public ObservableCollection<Catalogue> CatalogueList
        {
            get { return catelogueList; }
            set { catelogueList = value; OnPropertyChanged("CatelogueList"); }
        }

        public ObservableCollection<ArticleSupplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged("SupplierList");
            }
        }
        public List<object> SelectedSupplierList
        {
            get { return selectedSupplierList; }
            set
            {
                selectedSupplierList = value;
                if (value == null || SelectedSupplierList?.Count == 0)
                    ClearSuppliers = Visibility.Hidden;
                else
                    ClearSuppliers = Visibility.Visible;
                OnPropertyChanged("SelectedSupplierList");
            }
        }

        private bool isStockChangesEnabled;
        private Currency selectedCurrency;


        public bool IsStockChangesEnabled
        {
            get { return isStockChangesEnabled; }
            set
            {
                isStockChangesEnabled = value;
                OnPropertyChanged("IsStockChangesEnabled");
            }
        }
        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }
            set
            {
                selectedCurrency = value;
                OnPropertyChanged("SelectedCurrency");
            }
        }
        private ObservableCollection<CatalogueSearched> catalogueSearchedList;

        public ObservableCollection<CatalogueSearched> CatalogueSearchedList
        {
            get { return catalogueSearchedList; }
            set
            {
                catalogueSearchedList = value;
                OnPropertyChanged("CatalogueSearchedList");
            }
        }

        public int SelectedSearchIndex
        {
            get { return selectedSearchIndex; }
            set
            {
                selectedSearchIndex = value;
                OnPropertyChanged("SelectedSearchIndex");
            }
        }

        private Visibility clearSuppliers;

        public Visibility ClearSuppliers
        {
            get { return clearSuppliers; }
            set { clearSuppliers = value; OnPropertyChanged("ClearSuppliers"); }
        }

        private Visibility clearConditionalOperator;

        public Visibility ClearConditionalOperator
        {
            get { return clearConditionalOperator; }
            set { clearConditionalOperator = value; OnPropertyChanged("ClearConditionalOperator"); }
        }
        #endregion
        #region Properties
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        #region Methods
        private void FillWarehouseCategories()
        {
            WarehouseCategories = new ObservableCollection<WarehouseCategory>(SRMService.GetWarehouseCategories().OrderBy(c => c.BaseName).ToList());
        }
        private void FillConditionOperators()
        {
            ConditionalOperators = new ObservableCollection<string>();
            ConditionalOperators.Add("     <");
            ConditionalOperators.Add("     =");
            ConditionalOperators.Add("     >");
        }
        private void FillSupplierList()
        {
            SupplierList = new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSuppliersByWarehouseForCatalogue(SRMCommon.Instance.Selectedwarehouse.IdWarehouse, SRMCommon.Instance.Selectedwarehouse.Company.ConnectPlantConstr));
        }
        private void ShowSplashScreen()
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
        }
        private void CloseSplashScreen()
        {
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        public void Dispose()
        {
        }
        #region Components
        private void ClearCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ClearCommandAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Reference = null;
                SelectedSupplierList = null;
                WarehouseCategories.ToList().ForEach(x => { x.IsCategoryChecked = false; });
                OnPropertyChanged("WarehouseCategories");
                SelectedConditionalOperator = null;
                StockValue = null;
                GeosApplication.Instance.Logger.Log("Method ClearCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ClearCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method SearchCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (SRMCommon.Instance.Selectedwarehouse != null)
                {
                    ShowSplashScreen();
                    if (CatalogueSearchedList == null)
                        CatalogueSearchedList = new ObservableCollection<CatalogueSearched>();

                    string selectedWarehouseCategories = string.Join(",", WarehouseCategories.ToList().Where(x => x.IsCategoryChecked).Select(y => y.IdArticleCategory).ToList());
                    string supplierList = SelectedSupplierList == null ? "" : string.Join(",", SelectedSupplierList.OfType<ArticleSupplier>().ToList().Select(x => x.IdArticleSupplier).ToList());
                    string reference = Reference == null ? "" : Reference;
                    string conditionalOperator = SelectedConditionalOperator == null ? "" : SelectedConditionalOperator;
                    string stockValue = StockValue == null ? "" : StockValue;

                    CatalogueList = new ObservableCollection<Catalogue>(SRMService.GetCatalogue(SRMCommon.Instance.Selectedwarehouse.Company.ConnectPlantConstr, reference, supplierList, selectedWarehouseCategories, conditionalOperator, stockValue, SRMCommon.Instance.Selectedwarehouse.IdWarehouse, SelectedCurrency));
                    //CatalogueList.ToList().ForEach(x => { x.ReferenceImage = ByteArrayToBitmapImage(x.ArticleImageInBytes); });
                    catelogueList.ToList().ForEach(x => { x.PriceWithSelectedCurrencySymbol = string.Format($"{x.Value.ToString("N2")} {SelectedCurrency.Symbol}"); });


                    CatalogueSearchedFilter filter = new CatalogueSearchedFilter() { CategoryIds = selectedWarehouseCategories, ConditionalOperator = conditionalOperator, Reference = reference, StockQuanitty = stockValue, SupplierIds = supplierList };
                    CatalogueSearchedList.Add(new CatalogueSearched() { Header = $"Result Search [{CatalogueList?.Count}]", CatalogueList = CatalogueList, SavedFilter = filter });
                    //if (catalogueSearchedList.Any(x =>
                    //    x.SavedFilter.Reference == reference &&
                    //    x.SavedFilter.CategoryIds == selectedWarehouseCategories &&
                    //    x.SavedFilter.ConditionalOperator == conditionalOperator &&
                    //    x.SavedFilter.StockQuanitty == stockValue &&
                    //    x.SavedFilter.SupplierIds == supplierList
                    //    ))
                    //{
                    //    catalogueSearchedList.FirstOrDefault(x =>
                    //    x.SavedFilter.Reference == reference &&
                    //    x.SavedFilter.CategoryIds == selectedWarehouseCategories &&
                    //    x.SavedFilter.ConditionalOperator == conditionalOperator &&
                    //    x.SavedFilter.StockQuanitty == stockValue &&
                    //    x.SavedFilter.SupplierIds == supplierList).CatalogueList = CatalogueList;
                    //}
                    //else
                    //{
                    //    CatalogueSearchedList.Add(new CatalogueSearched() { Header = $"Result Search [{CatalogueList?.Count}]", CatalogueList = CatalogueList, SavedFilter = filter });
                    //}

                    SelectedSearchIndex = CatalogueSearchedList.Count - 1;
                    CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SearchCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                CloseSplashScreen();
            }
        }
        public void ViewArticleHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                Catalogue article = (Catalogue)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                Warehouse.Views.ArticleDetailsView articleDetailsView = new Warehouse.Views.ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                articleDetailsViewModel.Init_SRM(article.Reference, warehouseId, warehouse, ArticleSleepDays);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChildNodeSelectUnselectCommandAction(object obj)
        {
            var selection = obj as TreeListNodeEventArgs;
            if (selection == null) return;

            var selectedCategory = selection.Node.Content as WarehouseCategory;
            if (selectedCategory == null) return;

            // Set child node checkboxes recursively
            SetChildNodeCheckboxes(selectedCategory.IdKey, selectedCategory.IsCategoryChecked);
        }
        private void CommandWarehouseEditValueChangedCommandAction(object obj)
        {
            try
            {
                ShowSplashScreen();
                FillSupplierList();
                CloseSplashScreen();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CommandWarehouseEditValueChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                CloseSplashScreen();
            }
           
        }
        private void SetChildNodeCheckboxes(long parentId, bool isChecked)
        {
            foreach (var category in WarehouseCategories)
            {
                if (category.IdParent == parentId)
                {
                    category.IsCategoryChecked = isChecked;
                    // Call recursively for hierarchical checking
                    SetChildNodeCheckboxes(category.IdKey, isChecked);
                }
            }
        }
        #endregion
        #endregion
    }
}
