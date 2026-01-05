using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    //[nsatpute][29.08.2025][GEOS2-6505]
    public class MinMaxArticleStockViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("10.13.3.33:99");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Task log
        // [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section (unused code)
        #endregion

        #region Declaration
        private ObservableCollection<Article> articlesList;
        private ObservableCollection<Article> articlesListOriginal;
        private bool isBusy;
        private string myFilterString;
        private long warehouseId;
        private ObservableCollection<object> selectedArticles = new ObservableCollection<object>();
        private bool isInventoryEnable;
        private bool isValueColumnVisible;
        private Currency selectedCurrency;
        public string WMS_Grid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WMS_MinMaxArticleStock_Setting.Xml";
        private bool isCategoriesColumnChooserVisible;
        private ObservableCollection<LookupValue> logisticList;
        private bool? isAllSelected;
        private bool isUpdatingFromSelection = false;
        #endregion // Declaration

        #region Properties
        public bool IsCategoriesColumnChooserVisible
        {
            get { return isCategoriesColumnChooserVisible; }
            set
            {
                isCategoriesColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCategoriesColumnChooserVisible"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }
        public long WarehouseId { get; set; }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public ObservableCollection<Article> ArticlesList
        {
            get
            {
                return articlesList;
            }

            set
            {
                articlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesList"));
            }
        }

        public ObservableCollection<Article> ArticlesListOriginal
        {
            get
            {
                return articlesListOriginal;
            }

            set
            {
                articlesListOriginal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesListOriginal"));
            }
        }


        public ObservableCollection<object> SelectedArticles
        {
            get => selectedArticles;
            set
            {
                if (selectedArticles != value)
                {
                    selectedArticles = value ?? new ObservableCollection<object>();
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticles"));
                }
            }
        }

        //[000]add comment
        public bool IsInventoryEnable
        {
            get { return isInventoryEnable; }
            set
            {
                isInventoryEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInventoryEnable"));
            }
        }
        public bool IsValueColumnVisible
        {
            get { return isValueColumnVisible; }
            set
            {
                isValueColumnVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsValueColumnVisible"));
            }
        }

        public ObservableCollection<Data.Common.Epc.LookupValue> LogisticList
        {
            get { return logisticList; }
            set
            {
                logisticList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LogisticList"));
            }
        }
		//[nsatpute][25.11.2025][GEOS2-9364]
        public bool? IsAllSelected
        {
            get { return isAllSelected; }
            set
            {
                if (isAllSelected != value)
                {
                    if (value == null)
                        value = false;
                    isAllSelected = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));

                    // Only execute SelectAll if the change came from user interaction
                    // AND it's a definite true/false value (not indeterminate)
                    if (!isUpdatingFromSelection && value.HasValue)
                    {
                        SelectAll(value.Value);
                    }
                }
            }
        }        

        #endregion // Properties

        #region Commands

        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand CommandTransferMaterial { get; private set; }
        public ICommand RefreshWarehouseArticleViewCommand { get; set; }
        public ICommand PrintWarehouseArticleViewCommand { get; set; }
        public ICommand ExportWarehouseArticleViewCommand { get; set; }
        public ICommand SaveArticleStockCommand { get; set; }
        public ICommand WarehouseViewReferenceHyperlinkClickCommand { get; private set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }

        #endregion // Commands

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

        public MinMaxArticleStockViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MinMaxArticleStockViewModel()...", category: Category.Info, priority: Priority.Low);                
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
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

                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehousePopupClosedCommandAction);
                CommandTransferMaterial = new DevExpress.Mvvm.DelegateCommand<object>(TransferMaterialAction);
                RefreshWarehouseArticleViewCommand = new RelayCommand(new Action<object>(RefreshWarehouseArticleList));
                PrintWarehouseArticleViewCommand = new RelayCommand(new Action<object>(PrintWarehouseArticleList));
                ExportWarehouseArticleViewCommand = new RelayCommand(new Action<object>(ExportWarehouseArticleList));
                SaveArticleStockCommand = new RelayCommand(new Action<object>(SaveArticleStockCommandAction));

                WarehouseViewReferenceHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseViewReferenceHyperlinkClickCommandAction);
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor MinMaxArticleStockViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor MinMaxArticleStockViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(WMS_Grid_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(WMS_Grid_SettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(WMS_Grid_SettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsCategoriesColumnChooserVisible = true;
                }
                else
                {
                    IsCategoriesColumnChooserVisible = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WMS_Grid_SettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsCategoriesColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WMS_Grid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "MyWarehouse.Location")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName == "MyWarehouse.Location")
                {

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("MyWarehouse.Location = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("MyWarehouse.Location <> ''")
                    });

                    foreach (var dataObject in ArticlesList)
                    {
                        if (dataObject.MyWarehouse == null)
                        {
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dataObject.MyWarehouse.Location))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.MyWarehouse.Location))
                        {
                            string[] locationsList = dataObject.MyWarehouse.Location.Split(Environment.NewLine.ToCharArray());

                            foreach (string location in locationsList)
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == location))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = location;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("MyWarehouse.Location Like '%{0}%'", location));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();


                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void WarehouseViewReferenceHyperlinkClickCommandAction(object obj)
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                Article article = (Article)detailView.DataControl.CurrentItem; //  detailView.FocusedRow;

                int index = detailView.FocusedRowHandle;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                // List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                // Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == article.MyWarehouse.IdWarehouse);
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(article.Reference, WarehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();

                if (articleDetailsViewModel.IsResult)
                {
                    MyWarehouse mw = (MyWarehouse)article.MyWarehouse.Clone();
                    mw.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                    mw.MaximumStock = articleDetailsViewModel.MaximumQuantity;
                    mw.LockedStock = articleDetailsViewModel.LockedStock;
                    mw.Location = String.Join("\n", articleDetailsViewModel.ArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation).Select(x => x.WarehouseLocation.FullName).ToArray());
                    article.MyWarehouse = mw;

                    //ArticlesList.RemoveAt(index);
                    //article.MyWarehouse.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                    //article.MyWarehouse.MaximumStock = articleDetailsViewModel.MaximumQuantity;
                    //ArticlesList.Insert(index, article);

                    // SelectedArticle = article; //todo
                }

                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for open scan Locate material window.
        /// </summary>
        /// <param name="obj"></param>
        private void TransferMaterialAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method TransferMaterialAction....", category: Category.Info, priority: Priority.Low);

                //if (PendingStorageArticlesList.Count > 0)
                //{
                TransferMaterialView transferMaterialView = new TransferMaterialView();
                TransferMaterialViewModel transferMaterialViewModel = new TransferMaterialViewModel();

                TableView detailView = (TableView)obj;
                EventHandler handle = delegate { transferMaterialView.Close(); };
                transferMaterialViewModel.RequestClose += handle;
                //locateMaterialsViewModel.InIt(PendingStorageArticlesList);
                transferMaterialView.DataContext = transferMaterialViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                transferMaterialView.Owner = Window.GetWindow(ownerInfo);
                transferMaterialView.ShowDialog();


                //For refresh the grid after scan.
                //PendingStorageArticlesList = new List<PendingStorageArticles>(PendingStorageArticlesList);
                // }

                GeosApplication.Instance.Logger.Log("Method TransferMaterialAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehousePopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);

            List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
            SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

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

            FillWarehouseDetails();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        public void SaveArticleStockCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveArticleStockCommandAction()...", category: Category.Info, priority: Priority.Low);                
                IsBusy = true;
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
                
                List<Article> ModifiedArticles = GetModifiedSelectedArticles();
                if (ModifiedArticles.Count > 0)
                    Save(ModifiedArticles);
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SaveArticleStockCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveArticleStockCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void RefreshWarehouseArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseArticleList()...", category: Category.Info, priority: Priority.Low);
                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                IsBusy = true;

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

                //fill data as per selected warehouse
                FillWarehouseDetails();
                detailView.SearchString = null;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWarehouseArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintWarehouseArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWarehouseArticleList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["MinMaxArticleListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["MinMaxArticleListReportPrintFooterTemplate"];
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                // === Configure page settings ===
                pcl.PaperKind = System.Drawing.Printing.PaperKind.Custom;

                // Custom paper size is in HUNDREDTHS of an inch
                pcl.CustomPaperSize = new System.Drawing.Size(800, 2000);

                pcl.Landscape = true;
                pcl.CreateDocument();



                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintWarehouseArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWarehouseArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportWarehouseArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWarehouseArticleList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Warehouse Article Stock List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
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

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportWarehouseArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWarehouseArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment()
            {
                WrapText = true
            };
            //if (e.Value != null && e.Value.ToString() != "Value" && e.ColumnFieldName == "Price")
            //{
            //    e.Formatting.NumberFormat = "#,##0.00" + " " + SelectedCurrency.Symbol;


            //}

            e.Handled = true;
        }

        /// <summary>
        /// Method for fill warehouse details
        /// [001][cpatil][02-12-2019][GEOS2-1862]Wrong calculation of the Sleeping days
        /// [002][cpatil][15-01-2020][GEOS2-2020]Display articles in Warehouse section information depending of the type of warehouse
        /// </summary>
        private void FillWarehouseDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseDetails...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    ObservableCollection<Article> TempArticlesList = new ObservableCollection<Article>();
                    ArticlesList = new ObservableCollection<Article>();

                    IsBusy = true;
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

                    try
                    {
                        IsValueColumnVisible = true;
                        IWarehouseService SelectedWarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        TempArticlesList = new ObservableCollection<Article>(SelectedWarehouseService.GetArticlesStockByWarehouse_V2670(WarehouseCommon.Instance.Selectedwarehouse, SelectedCurrency));

                        for (int i = 0; i < TempArticlesList.Count; i++)
                        {
                            TempArticlesList[i].PriceWithSelectedCurrencySymbol = string.Format($"{TempArticlesList[i].Price.ToString("N2")} {SelectedCurrency.Symbol}");
                        }
                        ArticlesList.AddRange(TempArticlesList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseDetails() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }

                    ArticlesList = new ObservableCollection<Article>(ArticlesList);
					//[nsatpute][25.11.2025][GEOS2-9364]
                    if (ArticlesList is INotifyCollectionChanged observableCollection)
                    {
                        observableCollection.CollectionChanged += (s, e) => UpdateSelectAllState();
                    }
                    if (ArticlesList != null)
                    {
                        foreach (var item in ArticlesList)
                        {
                            if (item is INotifyPropertyChanged notifyItem)
                            {
                                notifyItem.PropertyChanged += (s, e) =>
                                {
                                    if (e.PropertyName == nameof(item.IsSelected))
                                    {
                                        UpdateSelectAllState();
                                    }
                                };
                            }
                        }
                    }
                }
                else
                {
                    ArticlesList = new ObservableCollection<Article>();
                }
                ArticlesListOriginal = CreateDeepClone();
                IsAllSelected = false;
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method FillWarehouseDetails executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseDetails() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateSelectAllState()
        {
            if (isUpdatingFromSelection) return;

            isUpdatingFromSelection = true;

            try
            {
                if (ArticlesList == null || ArticlesList.Count == 0)
                {
                    isAllSelected = false;
                }
                else
                {
                    int selectedCount = ArticlesList.Count(item => item.IsSelected);
                    int totalCount = ArticlesList.Count;

                    // Three visual states, but only two click actions
                    if (selectedCount == 0)
                    {
                        isAllSelected = false;        // Unchecked - no items selected
                    }
                    else if (selectedCount == totalCount)
                    {
                        isAllSelected = true;         // Checked - all items selected
                    }
                    else
                    {
                        isAllSelected = null;         // Indeterminate - some items selected
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
            }
            finally
            {
                isUpdatingFromSelection = false;
            }
        }
        private void SelectAll(bool select)
        {
            isUpdatingFromSelection = true;

            try
            {
                // Update both collections
                if (ArticlesList != null)
                {
                    foreach (var item in ArticlesList)
                    {
                        item.IsSelected = select;
                    }
                }

                if (ArticlesListOriginal != null)
                {
                    foreach (var item in ArticlesListOriginal)
                    {
                        item.IsSelected = select;
                    }
                }
            }
            finally
            {
                isUpdatingFromSelection = false;
            }
        }
        #endregion

        /// <summary>
        /// [000][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// </summary>
        /// <param name="obj"></param>

        public void Init()
        {
            FillWarehouseDetails();
        }

        private ObservableCollection<Article> CreateDeepClone()
        {
            var clonedCollection = new ObservableCollection<Article>();
            foreach (var article in articlesList)
            {
                clonedCollection.Add((Article)article.Clone());
            }
            return clonedCollection;
        }
        public List<Article> GetModifiedSelectedArticles()
        {
            var modifiedArticles = new List<Article>();

            // Ensure both collections exist and have the same count
            if (ArticlesList == null || ArticlesListOriginal == null ||
                ArticlesList.Count != ArticlesListOriginal.Count)
            {
                return modifiedArticles;
            }
            if (ArticlesList.Any(x => x.IsSelected))
            {
                foreach(Article currentArticle in ArticlesList.Where(x=> x.IsSelected).ToList())
                {
                    var originalArticle = ArticlesListOriginal.FirstOrDefault(x => x.Reference == currentArticle.Reference);
                    // Check for changes in MinimumStock or MaximumStock
                    bool hasMinimumStockChanged = currentArticle.MyWarehouse?.MinimumStock != originalArticle.MyWarehouse?.MinimumStock;
                    bool hasMaximumStockChanged = currentArticle.MyWarehouse?.MaximumStock != originalArticle.MyWarehouse?.MaximumStock;

                    if (hasMinimumStockChanged || hasMaximumStockChanged)
                    {
                        modifiedArticles.Add(currentArticle);
                    }

                }
            }
            else
            {
                foreach (Article currentArticle in ArticlesList.ToList())
                {
                    var originalArticle = ArticlesListOriginal.FirstOrDefault(x => x.Reference == currentArticle.Reference);
                    // Check for changes in MinimumStock or MaximumStock
                    bool hasMinimumStockChanged = currentArticle.MyWarehouse?.MinimumStock != originalArticle.MyWarehouse?.MinimumStock;
                    bool hasMaximumStockChanged = currentArticle.MyWarehouse?.MaximumStock != originalArticle.MyWarehouse?.MaximumStock;

                    if (hasMinimumStockChanged || hasMaximumStockChanged)
                    {
                        modifiedArticles.Add(currentArticle);
                    }

                }
            }
            return modifiedArticles;
        }

        private void Save(List<Article> ModifiedArticles)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Save...", category: Category.Info, priority: Priority.Low);
                try
                {                    
                    IWarehouseService LocalWarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());                  
                    LocalWarehouseService.UpdateArticleMinMaxStock_V2670(WarehouseCommon.Instance.Selectedwarehouse, ModifiedArticles);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Save() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Save() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }

                ArticlesList = new ObservableCollection<Article>(ArticlesList);

                ArticlesListOriginal = CreateDeepClone();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Save executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Save() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][GEOS2-9362][17.11.2025]
        private void FillLogisticList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogisticList()...", category: Category.Info, priority: Priority.Low);
                LogisticList = new ObservableCollection<LookupValue>();
                IList<LookupValue> temptypeList = CrmService.GetLookupValues(101);
                LogisticList = new ObservableCollection<LookupValue>(temptypeList);
                GeosApplication.Instance.Logger.Log("Method FillLogisticList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }
}
