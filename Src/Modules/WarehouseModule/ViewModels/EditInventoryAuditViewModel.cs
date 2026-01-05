using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Views;
using DevExpress.Xpf.Grid;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.UI.Helper;
using System.Data;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Modules.Warehouse.Reports;
using System.Drawing;
using DevExpress.DataProcessing;
using DevExpress.Data;
using DevExpress.XtraCharts.Native;
using DevExpress.Map.Native;
using Emdep.Geos.Data.Common.WMS;
using DevExpress.CodeParser;
using DevExpress.Office.Utils;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class EditInventoryAuditViewModel : IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        // IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");
        #endregion

        #region  Task Log
        //[000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        #endregion

        #region Declaration
        #region [rdixit][02.12.2022][GEOS2-3962]
        ObservableCollection<PrintInventoryAuditReport> inventoryAuditReportList;
        List<InventoryAuditArticle> inventoryAuditArticleOriginalList;
        List<InventoryAuditLocation> inventoryAuditLocationOriginalList;
        List<InventoryAuditArticle> articleListByWarehouseLocation;
        ObservableCollection<InventoryAuditLocation> warehouseLocationListByArticle;
        List<InventoryAuditArticle> updatedInventoryAuditArticleList;
        double tabHeight;
        double tabwidth;
        public List<UInt64> addArticles;
        public List<UInt64> addWarehouseLocation;
        DataTable dataTableForArticleGridLayout;
        ObservableCollection<BandItem> bands;
        DataTable articleDataTable;
        DataTable dtArticle;
        ObservableCollection<InventoryAuditArticle> inventoryAuditArticleList;
        private ObservableCollection<ColumnItem> columns;
        private ObservableCollection<InventoryAuditLocation> inventoryAuditLocationList;
        ObservableCollection<BandItem> articleBand;
        ObservableCollection<ColumnItem> column;
        private ObservableCollection<WarehouseLocation> warehouseLocationList;
        private WarehouseLocation selectedWarehouseLocation;
        private DataTable dtLocation;
        private DataTable dataTableForLocationGridLayout;
        //ObservableCollection<PCMArticleCategory> articleMenuList;
        ObservableCollection<ArticleCategory> articleMenuList;
        #endregion
        //[000] added
        int flag;
        private double gridHeight;
        private string windowHeader;
        private string name;
        private string error = string.Empty;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? minEndDate;
        private bool isUpdate;
        private WarehouseInventoryAudit updateWarehouseInventoryAuditDetails;
        //end

        private string searchString;
        private WarehouseInventoryAudit selectedInventoryAudit;
        private WarehouseInventoryAuditItem selectedInventoryAuditItem;
        private ObservableCollection<WarehouseInventoryAuditItem> warehouseInventoryAuditItemsList;
        private List<Currency> currencies;
        private Currency selectedCurrency;
        private string selectedCurrencySymbol;

        private List<GeosAppSetting> geosAppSettingList;
        private SolidColorBrush oKColor;
        private SolidColorBrush notOKColor;
        public string EditInventoryAuditGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "EditInventoryAuditGridSettingFilePath.Xml";

        private int windowHeight;
        private int windowWidth;
        private SolidColorBrush successRateBackground;
        private SolidColorBrush successRateForeground;
        private string totalLocationSummaryItems;
        private string locationSearchedString;

        #endregion

        #region Properties
        #region [rdixit][02.12.2022][GEOS2-3962]
        public int Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Flag"));
            }
        }
        public List<InventoryAuditArticle> ArticleListByWarehouseLocation
        {
            get { return articleListByWarehouseLocation; }
            set
            {
                articleListByWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleListByWarehouseLocation"));
            }
        }
        public ObservableCollection<InventoryAuditLocation> WarehouseLocationListByArticle
        {
            get { return warehouseLocationListByArticle; }
            set
            {
                warehouseLocationListByArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationListByArticle"));
            }
        }
        public List<UInt64> AddArticles
        {
            get { return addArticles; }
            set
            {
                addArticles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddArticles"));
            }
        }
        public List<UInt64> AddWarehouseLocation
        {
            get { return addWarehouseLocation; }
            set
            {
                addWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddWarehouseLocation"));
            }
        }
        public ObservableCollection<InventoryAuditArticle> InventoryAuditArticleList
        {
            get { return inventoryAuditArticleList; }
            set
            {
                inventoryAuditArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryAuditArticleList"));
            }
        }
        public ObservableCollection<InventoryAuditLocation> InventoryAuditLocationList
        {
            get
            {
                return inventoryAuditLocationList;
            }

            set
            {
                inventoryAuditLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryAuditLocationList"));
            }
        }


        public List<InventoryAuditArticle> InventoryAuditArticleOriginalList
        {
            get { return inventoryAuditArticleOriginalList; }
            set
            {
                inventoryAuditArticleOriginalList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryAuditArticleOriginalList"));
            }
        }
        public List<InventoryAuditLocation> InventoryAuditLocationOriginalList
        {
            get
            {
                return inventoryAuditLocationOriginalList;
            }

            set
            {
                inventoryAuditLocationOriginalList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryAuditLocationOriginalList"));
            }
        }
        public ObservableCollection<WarehouseLocation> WarehouseLocationList
        {
            get { return warehouseLocationList; }
            set
            {
                warehouseLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationList"));
            }
        }

        ObservableCollection<WarehouseLocation> allWarehouseLocationList;
        public ObservableCollection<WarehouseLocation> AllWarehouseLocationList
        {
            get { return allWarehouseLocationList; }
            set
            {
                allWarehouseLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllWarehouseLocationList"));
            }
        }
        public WarehouseLocation SelectedWarehouseLocation
        {
            get { return selectedWarehouseLocation; }
            set
            {
                selectedWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseLocation"));
            }
        }
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }
        public ObservableCollection<BandItem> ArticleBand
        {
            get { return articleBand; }
            set
            {
                articleBand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleBand"));
            }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public DataTable DtLocation
        {
            get { return dtLocation; }
            set
            {
                dtLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtLocation"));
            }
        }
        public DataTable ArticleDataTable
        {
            get { return articleDataTable; }
            set
            {
                articleDataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleDataTable"));
            }
        }
        public DataTable DataTableForLocationGridLayout
        {
            get
            {
                return dataTableForLocationGridLayout;
            }
            set
            {
                dataTableForLocationGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForLocationGridLayout"));
            }
        }
        public DataTable DataTableForArticleGridLayout
        {
            get
            {
                return dataTableForArticleGridLayout;
            }
            set
            {
                dataTableForArticleGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForArticleGridLayout"));
            }
        }
        public DataTable DtArticle
        {
            get
            {
                return dtArticle;
            }
            set
            {
                dtArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtArticle"));
            }
        }
        public ObservableCollection<ArticleCategory> ArticleMenuList
        {
            get
            {
                return articleMenuList;
            }

            set
            {
                articleMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleMenuList"));
            }
        }
        public double TabHeight
        {
            get
            {
                return tabHeight;
            }

            set
            {
                tabHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TabHeight"));
            }
        }
        public double Tabwidth
        {
            get
            {
                return tabwidth;
            }

            set
            {
                tabwidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tabwidth"));
            }
        }
        #endregion
        public ObservableCollection<PrintInventoryAuditReport> InventoryAuditReportList
        {
            get
            {
                return inventoryAuditReportList;
            }

            set
            {
                inventoryAuditReportList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryAuditReportList"));
            }
        }
        public virtual string ResultFileName { get; set; }
        public SolidColorBrush SuccessRateBackground
        {
            get
            {
                return successRateBackground;
            }

            set
            {
                successRateBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuccessRateBackground"));
            }
        }
        public SolidColorBrush SuccessRateForeground
        {
            get
            {
                return successRateForeground;
            }

            set
            {
                successRateForeground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuccessRateForeground"));
            }
        }
        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }

            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
            }
        }
        public double GridHeight
        {
            get
            {
                return gridHeight;
            }

            set
            {
                gridHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridHeight"));
            }
        }
        public int WindowWidth
        {
            get
            {
                return windowWidth;
            }

            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
            }
        }
        public WarehouseInventoryAudit SelectedInventoryAudit
        {
            get
            {
                return selectedInventoryAudit;
            }

            set
            {
                selectedInventoryAudit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryAudit"));
            }
        }
        public WarehouseInventoryAuditItem SelectedInventoryAuditItem
        {
            get
            {
                return selectedInventoryAuditItem;
            }

            set
            {
                selectedInventoryAuditItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryAuditItem"));
            }
        }
        public ObservableCollection<WarehouseInventoryAuditItem> WarehouseInventoryAuditItemsList
        {
            get
            {
                return warehouseInventoryAuditItemsList;
            }

            set
            {
                warehouseInventoryAuditItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseInventoryAuditItemsList"));
            }
        }
		// [nsatpute][20-12-2024][GEOS2-6382]
        public string SearchString
        {
            get { return searchString; }
            set { searchString = value; OnPropertyChanged(new PropertyChangedEventArgs("SearchString")); CalculateSummaryCount(searchString); }
        }
        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
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
        public string SelectedCurrencySymbol
        {
            get
            {
                return selectedCurrencySymbol;
            }

            set
            {
                selectedCurrencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrencySymbol"));
            }
        }
        //[000] added
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));

            }
        }
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged(new PropertyChangedEventArgs("EndDate")); }

        }
        public DateTime? MinEndDate
        {
            get { return minEndDate; }
            set { minEndDate = value; OnPropertyChanged(new PropertyChangedEventArgs("MinEndDate")); }
        }
        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; OnPropertyChanged(new PropertyChangedEventArgs("IsUpdate")); }
        }
        private List<ArticleCategory> articlerecord;
        public List<ArticleCategory> Articlerecord
        {
            get { return articlerecord; }
            set
            {
                articlerecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Articlerecord"));

            }
        }


        private List<WarehouseLocation> warehouserecord;
        public List<WarehouseLocation> Warehouserecord
        {
            get { return warehouserecord; }
            set
            {
                warehouserecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Warehouserecord"));

            }
        }
        public WarehouseInventoryAudit UpdateWarehouseInventoryAuditDetails
        {
            get { return updateWarehouseInventoryAuditDetails; }
            set
            {
                updateWarehouseInventoryAuditDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateWarehouseInventoryAuditDetails"));

            }
        }

        private ObservableCollection<AuditedArticle> auditedArticleList;
        public ObservableCollection<AuditedArticle> AuditedArticleList
        {
            get
            {
                return auditedArticleList;
            }

            set
            {
                auditedArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AuditedArticleList"));
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        public string TotalLocationSummaryItems
        {
            get
            {
                return totalLocationSummaryItems;
            }

            set
            {
                totalLocationSummaryItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalLocationSummaryItems"));
            }
        }


        // [nsatpute][20-12-2024][GEOS2-6382]
        public string LocationSearchedString
        {
            get
            {
                return locationSearchedString;
            }

            set
            {
                locationSearchedString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationSearchedString"));
            }
        }

        //end
        #endregion

        #region Command
        //[000] added
        #region [rdixit][02.12.2022][GEOS2-3962]
        public ICommand CommandDropRecordArticleGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropArticleGrid { get; set; }
        public ICommand CommandOnDragRecordOverArticleGrid { get; set; }
        public ICommand CommandDropRecordLocationGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropLocationGrid { get; set; }
        public ICommand CommandOnDragRecordOverLocationGrid { get; set; }
        public ICommand ItemListTableViewLoadedCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        public ICommand AuditResulViewLoadedCommand { get; set; }
        #endregion
        public ICommand EditInventoryAuditViewCancelCommand { get; set; }
        public ICommand EditInventoryAuditViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        //end
        public ICommand WarehouseViewReferenceHyperlinkClickCommand { get; private set; }

        public ICommand ArticleDetailsViewDNHyperlinkClickCommand { get; set; }

        public ICommand TableViewLoadedCommand { get; set; }

        public ICommand TableViewUnLoadedCommand { get; set; }
        public ICommand ExportInventoryAuditViewCommand { get; set; }
        public ICommand PrintInventoryAuditViewCommand { get; set; }
        public ICommand DeleteSelectedRowsCommand { get; set; } //[nsatpute][05-12-2024][GEOS2-6381]
        #endregion

        #region Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region  Constructor
        public EditInventoryAuditViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditInventoryAuditViewModel()...", category: Category.Info, priority: Priority.Low);

                EditInventoryAuditViewCancelCommand = new RelayCommand(new Action<object>(CloseWindow));

                EditInventoryAuditViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditInventoryAudit));

                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);

                WarehouseViewReferenceHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseViewReferenceHyperlinkClickCommandAction);

                ArticleDetailsViewDNHyperlinkClickCommand = new DelegateCommand<object>(ArticleDetailsViewDNHyperlinkClickCommandAction); //[002] added for hyperlink for Delivery Note

                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);

                TableViewUnLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnLoadedCommandAction);

                ExportInventoryAuditViewCommand = new RelayCommand(new Action<object>(ExportInventoryAuditList));
                //PrintInventoryAuditViewCommand = new RelayCommand(new Action<object>(PrintInventoryAuditList));
                PrintInventoryAuditViewCommand = new RelayCommand(new Action<object>(PrintInventoryAuditListNew));
                #region [rdixit][02.12.2022][GEOS2-3962]

                CommandDropRecordArticleGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordArticleGrid);
                CommandOnDragRecordOverArticleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticleGrid);

                CommandCompleteRecordDragDropArticleGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropArticleGrid);

                CommandDropRecordLocationGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordLocationGrid);
                CommandOnDragRecordOverLocationGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverLocationGrid);

                CommandCompleteRecordDragDropLocationGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropLocationGrid);

                DeleteRowCommand = new DelegateCommand<object>(DeleteRowCommandAction);

                AuditResulViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(AuditResulViewLoadedCommandAction);
                DeleteSelectedRowsCommand = new DelegateCommand<object>(DeleteSelectedRowsCommandAction);
                #endregion
                var screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                var screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                WindowHeight = screenHeight - 500;
                TabHeight = WindowHeight - 50;
                WindowWidth = screenWidth - 100;
                GridHeight = TabHeight - 60;
                GeosApplication.Instance.Logger.Log("Constructor EditInventoryAuditViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EditInventoryAuditViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Command Action
        private void ExportInventoryAuditList(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportInventoryAuditList...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Inventory Audit Items";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                if (!(Boolean)saveFile.ShowDialog())
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    // IsBusy = true;
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
                    // IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportInventoryAuditList executed successfully.", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                //IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportInventoryAuditList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            if (e.Value != null && e.ColumnFieldName == "BalanceAmount" && e.Value.ToString() != "Balance Amount")
            {
                e.Value = String.Format("{0:n}", e.Value) + SelectedCurrencySymbol;
            }
            e.Handled = true;
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                Flag++;
                if (Flag > 1)
                {
                    //Set Real Height using the dummy 100 rows
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.Height = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.ActualHeight;
                    GridHeight = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.Height;
                    //Remove dummy 500 rows [rdixit][GEOS2-3962][13.12.2022]
                    //for (int removedItemsCount = 0; removedItemsCount < 100; removedItemsCount++)
                    //{
                    //    WarehouseInventoryAuditItemsList.RemoveAt(WarehouseInventoryAuditItemsList.Count - 1);
                    //}
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.View.Visibility = Visibility.Visible;
                    //GridControl.AllowInfiniteGridSize = true;
                    //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.Height = 
                    //    Math.Round(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 
                    //    (Window.GetWindow(obj.Source as FrameworkElement).Content as System.Windows.Controls.DockPanel).ActualHeight-400); 
                    // ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).;

                    if (File.Exists(EditInventoryAuditGridSettingFilePath))
                    {
                        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(EditInventoryAuditGridSettingFilePath);
                        SearchString = string.Empty;
                    }

                                //This code for save grid layout.
                                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(EditInventoryAuditGridSettingFilePath);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void TableViewUnLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {

                if (Flag > 1)
                {
                    GeosApplication.Instance.Logger.Log("Method TableViewUnLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                    //This code for save grid layout.
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(EditInventoryAuditGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method TableViewUnLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void ArticleDetailsViewDNHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleDetailsViewDNHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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

                TableView tblView = (TableView)obj;
                // ArticlesStock ac = (ArticlesStock)tblView.DataControl.CurrentItem;
                var idWarehouseDeliveryNote = ((WarehouseInventoryAuditItem)tblView.DataControl.CurrentItem).WarehouseDeliveryNoteItem.WarehouseDeliveryNote.IdWarehouseDeliveryNote;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
                // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(idWarehouseDeliveryNote);

                //[pramo.misal][GEOS2-4543][10-10-2023]
                //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2440(idWarehouseDeliveryNote);
                //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
                // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2480(idWarehouseDeliveryNote);
                //[Sudhir.Jangra][GEOS2-5457]
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2510(idWarehouseDeliveryNote);


                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (tblView as FrameworkElement);
                editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ArticleDetailsViewDNHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleDetailsViewDNHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                Article article = (Article)((WarehouseInventoryAuditItem)detailView.DataControl.CurrentItem).Article; //  detailView.FocusedRow;

                int index = detailView.FocusedRowHandle;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                // List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                // Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == article.MyWarehouse.IdWarehouse);
                //Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                //WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();

                //if (articleDetailsViewModel.IsResult)
                //{
                //    MyWarehouse mw = (MyWarehouse)article.MyWarehouse.Clone();
                //    mw.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                //    mw.MaximumStock = articleDetailsViewModel.MaximumQuantity;
                //    mw.LockedStock = articleDetailsViewModel.LockedStock;
                //    mw.Location = String.Join("\n", articleDetailsViewModel.ArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation).Select(x => x.WarehouseLocation.FullName).ToArray());
                //    article.MyWarehouse = mw;

                //    //ArticlesList.RemoveAt(index);
                //    //article.MyWarehouse.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                //    //article.MyWarehouse.MaximumStock = articleDetailsViewModel.MaximumQuantity;
                //    //ArticlesList.Insert(index, article);

                //    SelectedArticle = article;
                //}

                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            IsUpdate = false;
            RequestClose(null, null);
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void EditInventoryAudit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EditInventoryAudit()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name")); //[01] added
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));

                if (error != null)
                {
                    return;
                }
                else
                {
                    if (StartDate > EndDate)
                        return;
                }
                //[rdixit][02.12.2022][GEOS2-3962]
                List<InventoryAuditArticle> DeletedArticles = InventoryAuditArticleOriginalList.Except(InventoryAuditArticleList).ToList();
                List<InventoryAuditArticle> AddedArticles = InventoryAuditArticleList.Except(InventoryAuditArticleOriginalList).ToList();
                List<InventoryAuditArticle> UpdatedArticles = new List<InventoryAuditArticle>();
                List<InventoryAuditLocation> DeletedLocations = InventoryAuditLocationOriginalList.Except(InventoryAuditLocationList).ToList();
                List<InventoryAuditLocation> AddedLocation = InventoryAuditLocationList.Except(InventoryAuditLocationOriginalList).ToList();
                if (InventoryAuditArticleOriginalList != null)//To get Updated Articles
                {
                    foreach (var item in InventoryAuditArticleOriginalList)
                    {
                        InventoryAuditArticle InventoryAuditArticle = InventoryAuditArticleList.Where(j => j.IdArticle == item.IdArticle).ToList().Where(k => k.IsAudited != item.IsAudited).FirstOrDefault();
                        if (InventoryAuditArticle != null)
                        {
                            UpdatedArticles.Add(InventoryAuditArticle);
                        }
                    }
                }


                if (UpdateWarehouseInventoryAuditDetails.StartDate == StartDate &&
                    UpdateWarehouseInventoryAuditDetails.Name == Name &&
                    UpdateWarehouseInventoryAuditDetails.EndDate == EndDate &&
                    DeletedArticles == null &&
                    AddedArticles == null &&
                    DeletedLocations == null &&
                    AddedLocation == null)
                {
                    IsUpdate = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateInventoryAuditSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    return;
                }
                //[rdixit][02.12.2022][GEOS2-3962]
                #region Articles & Location Add/Delete
                // Delete Articles [08.12.2022][rdixit]

                UpdateWarehouseInventoryAuditDetails.DeletedArticles = InventoryAuditArticleOriginalList.Except(InventoryAuditArticleList).ToList();
                UpdateWarehouseInventoryAuditDetails.AddedArticles = InventoryAuditArticleList.Except(InventoryAuditArticleOriginalList).ToList();
                UpdateWarehouseInventoryAuditDetails.UpdatedArticles = UpdatedArticles;
                UpdateWarehouseInventoryAuditDetails.DeletedLocations = InventoryAuditLocationOriginalList.Except(InventoryAuditLocationList).ToList();
                UpdateWarehouseInventoryAuditDetails.AddedLocation = InventoryAuditLocationList.Except(InventoryAuditLocationOriginalList).ToList();
                #endregion



                UpdateWarehouseInventoryAuditDetails.StartDate = StartDate;
                UpdateWarehouseInventoryAuditDetails.Name = Name;
                UpdateWarehouseInventoryAuditDetails.EndDate = endDate;
                UpdateWarehouseInventoryAuditDetails.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;
                //Service Method changed from UpdateWarehouseInventoryAudit to UpdateWarehouseInventoryAudit_V2340
                //[nsatpute][12-12-2024][GEOS2-6382] 
                if (WarehouseService.UpdateWarehouseInventoryAudit_V2590(WarehouseCommon.Instance.Selectedwarehouse, UpdateWarehouseInventoryAuditDetails))
                {
                    IsUpdate = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateInventoryAuditSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("EditInventoryAudit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                IsUpdate = false;
                GeosApplication.Instance.Logger.Log("Get an error in EditInventoryAudit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="e"></param>
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                if (StartDate != null)
                    MinEndDate = StartDate;
                else
                    MinEndDate = null;

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }


            return null;
        }


        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => StartDate)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string InventoryAuditName = BindableBase.GetPropertyName(() => Name);
                string inventoryAuditStartDate = BindableBase.GetPropertyName(() => StartDate);

                if (columnName == InventoryAuditName)
                    return WarehouseInventoryAuditValidation.GetErrorMessage(InventoryAuditName, Name);
                if (columnName == inventoryAuditStartDate)
                    return WarehouseInventoryAuditValidation.GetErrorMessage(inventoryAuditStartDate, StartDate);

                return null;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="ExistwarehouseInventoryAuditDetails"></param>
        public void Init(WarehouseInventoryAudit ExistwarehouseInventoryAuditDetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("EditInventoryAudit").ToString();
                UpdateWarehouseInventoryAuditDetails = new WarehouseInventoryAudit();
                UpdateWarehouseInventoryAuditDetails = ExistwarehouseInventoryAuditDetails;

                StartDate = ExistwarehouseInventoryAuditDetails.StartDate;
                Name = ExistwarehouseInventoryAuditDetails.Name;
                EndDate = ExistwarehouseInventoryAuditDetails.EndDate;

                geosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("7,8");
                oKColor = new BrushConverter().ConvertFromString(geosAppSettingList[0].DefaultValue) as SolidColorBrush;
                notOKColor = new BrushConverter().ConvertFromString(geosAppSettingList[1].DefaultValue) as SolidColorBrush;
                #region [rdixit][02.12.2022][GEOS2-3962]
                AddColumnsToLocationDataTableWithoutBands();
                AddColumnsToArticleDataTableWithoutBands();
                FillWarehouseLocationsList();
                FillArticleMenuList();
                InventoryAuditArticleList = new ObservableCollection<InventoryAuditArticle>(WarehouseService.GetAllArticleForSelectedWarehouseInventoryAudit(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, ExistwarehouseInventoryAuditDetails.IdWarehouseInventoryAudit));
                InventoryAuditLocationList = new ObservableCollection<InventoryAuditLocation>(WarehouseService.GetAllLocationsForSelectedWarehouseInventoryAudit(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, ExistwarehouseInventoryAuditDetails.IdWarehouseInventoryAudit));
                InventoryAuditArticleOriginalList = InventoryAuditArticleList.Select(item => (InventoryAuditArticle)item.Clone()).ToList();
                //InventoryAuditArticleOriginalList = new List<InventoryAuditArticle>(InventoryAuditArticleList.ToList());
                InventoryAuditLocationOriginalList = new List<InventoryAuditLocation>(InventoryAuditLocationList.ToList());
                FillArticleGrid();
                FillLocationGrid();
                #endregion
                FillCurrencyDetails();
                FillInventoryAuditDetails(ExistwarehouseInventoryAuditDetails);
                //Add dummy 100 rows to set full screen real time Height
                //var item = new WarehouseInventoryAuditItem() { Article = new Article { } };
                //for (int i = 0; i < 100; i++)
                //{
                //    WarehouseInventoryAuditItemsList.Add(item);
                //}
                #region GEOS2-3965
                //Shubham[skadam] GEOS2-3965 Add a NEW option in the Inventory Audit Edit/View screen in order to print a report 24 03 2023 
                try
                {
                    AuditedArticleList = new ObservableCollection<AuditedArticle>(WarehouseService.GetAllAuditedArticleForInventoryAudit(ExistwarehouseInventoryAuditDetails.IdWarehouseInventoryAudit));
                }
                catch (Exception ex)
                {
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill Currency details from My Preference page and user setting file
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies.ToList();
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency_Warehouse"))
                {
                    SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                    SelectedCurrencySymbol = SelectedCurrency.Symbol;
                }

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for fill  Inventory Audit list
        /// </summary>
        /// <param name="ExistwarehouseInventoryAuditDetails">Existing warehouse Inventory Audit Details</param>
        private void FillInventoryAuditDetails(WarehouseInventoryAudit ExistwarehouseInventoryAuditDetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillInventoryAuditDetails()...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    try
                    {
                        //WarehouseInventoryAuditList = new ObservableCollection<WarehouseInventoryAudit>(WarehouseService.GetAllWarehouseInventoryAudits(WarehouseCommon.Instance.Selectedwarehouse));
                        WarehouseCommon.Instance.Selectedwarehouse.Currency = new Currency();
                        WarehouseCommon.Instance.Selectedwarehouse.Currency = SelectedCurrency;
                        #region Service commented 
                        //SelectedInventoryAudit = WarehouseService.GetAllItemsForSelectedWarehouseInventoryAudit_V2200(WarehouseCommon.Instance.Selectedwarehouse,ExistwarehouseInventoryAuditDetails.IdWarehouseInventoryAudit);

                        #endregion

                        //[pramod.misal][GEOS2-5069][29-12-2023]
                        SelectedInventoryAudit = WarehouseService.GetAllItemsForSelectedWarehouseInventoryAudit_V2470(WarehouseCommon.Instance.Selectedwarehouse, ExistwarehouseInventoryAuditDetails.IdWarehouseInventoryAudit);

                        if (SelectedInventoryAudit != null)
                            if (SelectedInventoryAudit != null)
                            {
                                if (SelectedInventoryAudit.Creator != null)
                                {
                                    SelectedInventoryAudit.Creator.LastName = $"{SelectedInventoryAudit.Creator.LastName} ( {SelectedInventoryAudit.CreationDate.ToString("dd/MM/yyyy")} ) ";
                                }
                                if (SelectedInventoryAudit.Modifier != null)
                                {
                                    var modificationDateString = string.Empty;

                                    if (SelectedInventoryAudit.ModificationDate.HasValue)
                                    {
                                        modificationDateString = $" ( {SelectedInventoryAudit.ModificationDate.Value.ToString("dd/MM/yyyy")} ) ";
                                    }
                                    SelectedInventoryAudit.Modifier.LastName = $"{SelectedInventoryAudit.Modifier.LastName}{modificationDateString}";
                                }
                                foreach (WarehouseInventoryAuditItem item in SelectedInventoryAudit.WarehouseInventoryAuditItemsList)
                                {
                                    if (SelectedCurrency != null)
                                    {
                                        item.BalanceAmountwithCurrentSymbol = item.BalanceAmount.ToString("#,##0.00") + SelectedCurrency.Symbol;
                                    }

                                    if ((item.CurrentQuantity - item.ExpectedQuantity) == 0)
                                    {
                                        item.DifferenceBackgoundColor = oKColor;
                                        item.Approver.FirstName = string.Empty;
                                        item.Approver.LastName = string.Empty;
                                        item.ModificationDate = null;
                                    }
                                    else
                                    {
                                        item.DifferenceBackgoundColor = notOKColor;
                                    }
                                }

                                WarehouseInventoryAuditItemsList = new ObservableCollection<WarehouseInventoryAuditItem>(
                                    SelectedInventoryAudit.WarehouseInventoryAuditItemsList);

                                SelectedInventoryAuditItem = SelectedInventoryAudit.WarehouseInventoryAuditItemsList.FirstOrDefault();

                                if (SelectedCurrency != null)
                                {
                                    SelectedInventoryAudit.BalanceAmountwithCurrentSymbol = SelectedInventoryAudit.BalanceAmount.ToString("#,##0.00") + SelectedCurrency.Symbol;
                                }

                                if (SelectedInventoryAudit.SuccessRate >= 90)
                                {
                                    SuccessRateBackground = new SolidColorBrush(Colors.DarkGreen);
                                    SuccessRateForeground = new SolidColorBrush(Colors.White);
                                }
                                else if (SelectedInventoryAudit.SuccessRate >= 80)
                                {
                                    SuccessRateBackground = new SolidColorBrush(Colors.LightGreen);
                                    SuccessRateForeground = new SolidColorBrush(Colors.White);
                                }
                                else if (SelectedInventoryAudit.SuccessRate >= 70)
                                {
                                    SuccessRateBackground = new SolidColorBrush(Colors.Yellow);
                                    SuccessRateForeground = new SolidColorBrush(Colors.Black);
                                }
                                else if (SelectedInventoryAudit.SuccessRate >= 50)
                                {
                                    SuccessRateBackground = new SolidColorBrush(Colors.Orange);
                                    SuccessRateForeground = new SolidColorBrush(Colors.White);
                                }
                                else
                                {
                                    SuccessRateBackground = new SolidColorBrush(Colors.Red);
                                    SuccessRateForeground = new SolidColorBrush(Colors.White);
                                }
                            }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditDetails() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                }
                //else
                //{
                //    WarehouseInventoryAuditList = new ObservableCollection<WarehouseInventoryAudit>();
                //}

                GeosApplication.Instance.Logger.Log("Method FillInventoryAuditDetails() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditDetails() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region [rdixit][02.12.2022][GEOS2-3962] Methods
        private void AuditResulViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                ((System.Windows.Controls.Grid)obj.OriginalSource).Height = ((System.Windows.Controls.Grid)obj.OriginalSource).ActualHeight;
                ((System.Windows.Controls.Grid)obj.OriginalSource).Width = WindowWidth;

                GeosApplication.Instance.Logger.Log("Method AuditResulViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AuditResulViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void FillArticleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleMenuList()...", category: Category.Info, priority: Priority.Low);
                ArticleMenuList = new ObservableCollection<ArticleCategory>(WarehouseService.GetWMSArticlesWithCategoryForReference());
                GeosApplication.Instance.Logger.Log("Method FillArticleMenuList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleMenuList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleMenuList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleMenuList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void AddColumnsToLocationDataTableWithoutBands()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToLocationDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                BandItem band1 = new BandItem() { BandName = "all", Header = "Inventory Audit Locations", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band1.Columns = new ObservableCollection<ColumnItem>();

                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CheckBox", HeaderText = " ", Width = 30, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.IsChecked, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "FullName", HeaderText = "Full Name", Width = 200, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Status", HeaderText = "Status", Width = 100, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Delete", HeaderText = "Delete", Width = 50, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Delete, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdWarehouseLocation", HeaderText = "IdWarehouseLocation", Width = 50, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = false });
                Bands.Add(band1);
                DataTableForLocationGridLayout = new DataTable();

                DataTableForLocationGridLayout.Columns.Add("CheckBox", typeof(bool));
                DataTableForLocationGridLayout.Columns.Add("FullName", typeof(string));
                DataTableForLocationGridLayout.Columns.Add("Status", typeof(string));
                DataTableForLocationGridLayout.Columns.Add("Delete", typeof(string));
                DataTableForLocationGridLayout.Columns.Add("IdWarehouseLocation", typeof(string));
                DataTableForLocationGridLayout.Columns.Add("IdParent", typeof(string));
                DataTableForLocationGridLayout.Columns.Add("Child", typeof(int));
                DtLocation = DataTableForLocationGridLayout;
                Bands = new ObservableCollection<BandItem>(Bands);
                TotalSummary = new ObservableCollection<Summary>()
                { new Summary() { Type = DevExpress.Data.SummaryItemType.Count, FieldName = "Name", DisplayFormat = "Total : {0}" } };

                GeosApplication.Instance.Logger.Log("Method AddColumnsToLocationDataTableWithoutBands executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToLocationDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToLocationDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToLocationDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void AddColumnsToArticleDataTableWithoutBands()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToArticleDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);

                ArticleBand = new ObservableCollection<BandItem>(); ArticleBand.Clear();
                BandItem band1 = new BandItem() { BandName = "all", Header = "Inventory Audit Articles", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band1.Columns = new ObservableCollection<ColumnItem>();

                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CheckBox", HeaderText = "", Width = 45, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Reference", HeaderText = "Reference", Width = 150, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Description", HeaderText = "Description", Width = 350, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Status", HeaderText = "Status", Width = 100, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Delete", HeaderText = "Delete", Width = 50, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Delete, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdArticle", HeaderText = "IdArticle", Width = 50, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = false });
                ArticleBand.Add(band1);
                DataTableForArticleGridLayout = new DataTable();

                DataTableForArticleGridLayout.Columns.Add("CheckBox", typeof(bool));
                DataTableForArticleGridLayout.Columns.Add("Reference", typeof(string));
                DataTableForArticleGridLayout.Columns.Add("Description", typeof(string));
                DataTableForArticleGridLayout.Columns.Add("Status", typeof(string));
                DataTableForArticleGridLayout.Columns.Add("Delete", typeof(string));
                DataTableForArticleGridLayout.Columns.Add("IdArticle", typeof(string));
                DataTableForArticleGridLayout.Columns.Add("IdParent", typeof(string));
                ArticleDataTable = DataTableForArticleGridLayout;
                ArticleBand = new ObservableCollection<BandItem>(ArticleBand);
                TotalSummary = new ObservableCollection<Summary>()
                { new Summary() { Type = DevExpress.Data.SummaryItemType.Count, FieldName = "Reference", DisplayFormat = "Total : {0}" } };

                GeosApplication.Instance.Logger.Log("Method AddColumnsToArticleDataTableWithoutBands executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToArticleDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToArticleDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillWarehouseLocationsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseLocationsList...", category: Category.Info, priority: Priority.Low);
                //[rdixit][GEOS2-5146][18.12.2023]
                //AllWarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetWarehouseLocationsByIdWarehouse_V2220(WarehouseCommon.Instance.Selectedwarehouse));
                //[pramod.misal][GEOS2-5488][16.05.2024]
                AllWarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetWarehouseLocationsByIdWarehouse_V2520(WarehouseCommon.Instance.Selectedwarehouse));
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(AllWarehouseLocationList.Where(i => i.InUse).Select(k => (WarehouseLocation)k.Clone()).ToList());
                SelectedWarehouseLocation = WarehouseLocationList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillWarehouseLocationsList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationsList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationsList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillArticleGrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if (InventoryAuditArticleList != null)
                {
                    foreach (InventoryAuditArticle ArticleMenu in InventoryAuditArticleList)
                    {
                        if (DataTableForArticleGridLayout == null)
                            DataTableForArticleGridLayout = new DataTable();
                        //ArticleMenu.Name = InventoryAuditArticleList.Where(i => i.IdArticle == ArticleMenu.IdArticle).FirstOrDefault().Name;
                        InventoryAuditArticle Article = InventoryAuditArticleList.Where(i => i.IdArticle == ArticleMenu.IdArticle).FirstOrDefault();
                        //List<long> IdWarehouseLocationList = new List<long>();
                        //if (Article.InventoryAuditLocation != null)
                        //{
                        //    IdWarehouseLocationList = Article.InventoryAuditLocation.Select(j => j.IdWarehouseLocation).Except(InventoryAuditLocationList.Where(k => k.IsAudited == 1).Select(i => i.IdWarehouseLocation)).ToList();
                        //}
                        //if (IdWarehouseLocationList.Count == 0 && Article.InventoryAuditLocation!=null)
                        //{                      
                        //    Article.IsAudited = 1;
                        //    Article.IsAuditedYesNo = "Audited";
                        //}
                        DataRow dr = DataTableForArticleGridLayout.NewRow();
                        dr["CheckBox"] = false;
                        dr["Reference"] = Article.Reference;
                        dr["Description"] = Article.Description;
                        dr["Status"] = Article.IsAuditedYesNo;
                        dr["IdArticle"] = ArticleMenu.IdArticle;
                        ArticleMenuList.Where(a => a.IdArticle == ArticleMenu.IdArticle).ToList().ForEach(b => b.IsChecked = true);
                        DataTableForArticleGridLayout.Rows.Add(dr);
                    }
                }
                DtArticle = DataTableForArticleGridLayout;
                DtArticle.TableName = "Article";
                DataTableForArticleGridLayout.TableName = "Article";
                GeosApplication.Instance.Logger.Log("Method FillArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleGrid() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void FillLocationGrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationGrid()...", category: Category.Info, priority: Priority.Low);
                if (AllWarehouseLocationList != null)//[rdixit][GEOS2-5146][18.12.2023]
                {
                    foreach (InventoryAuditLocation InventoryAuditLocation in InventoryAuditLocationList)
                    {
                        if (DataTableForLocationGridLayout == null)
                            DataTableForLocationGridLayout = new DataTable();
                        WarehouseLocation WarehouseLocation = new WarehouseLocation();
                        //WarehouseLocation.IdWarehouseLocation = WarehouseLocationList.Where(i => i.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation).FirstOrDefault().IdWarehouseLocation;
                        //InventoryAuditLocation Warehouse = InventoryAuditLocationList.Where(i => i.IdWarehouseLocation == WarehouseLocation.IdWarehouseLocation).FirstOrDefault();
                        if (AllWarehouseLocationList.Any(i => i.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation))
                        {
                            DataRow dr = DataTableForLocationGridLayout.NewRow();
                            dr["CheckBox"] = false;
                            dr["FullName"] = InventoryAuditLocation.FullName;
                            dr["Status"] = InventoryAuditLocation.IsAuditedYesNo;
                            dr["IdWarehouseLocation"] = InventoryAuditLocation.IdWarehouseLocation;
                            dr["IdParent"] = InventoryAuditLocation.Parent == 0 ? AllWarehouseLocationList.FirstOrDefault(i => i.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation)?.Parent : InventoryAuditLocation.Parent;
                            dr["Child"] = 1;
                            WarehouseLocationList.Where(a => a.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation).ToList().ForEach(b => b.IsChecked = true);
                            DataTableForLocationGridLayout.Rows.Add(dr);
                        }
                    }
                }
                RemoveParentLocation();
                AddParentAuditLocation();

                DtLocation = DataTableForLocationGridLayout;

                DtLocation.TableName = "Location";
                DataTableForLocationGridLayout.TableName = "Location";
                GeosApplication.Instance.Logger.Log("Method FillLocationGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationGrid() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillLocationGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CompleteRecordDragDropArticleGrid(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropArticleGrid()...", category: Category.Info, priority: Priority.Low);
                e.Handled = false;
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][26-11-2024][GEOS2-6379]
        private void OnDragRecordOverArticleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(ArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    int count = 0;
                    foreach (var rec in data.Records)
                    {
                        if (rec is ArticleCategory)
                        {
                            count++;
                            break;
                        }
                    }

                    Articlerecord = data.Records.OfType<ArticleCategory>().ToList();
                    foreach (ArticleCategory article in Articlerecord.Where(a => a.IdArticle > 0))
                    {
                        DataRow[] found = DataTableForArticleGridLayout.Select("Reference = '" + article.Name + "'");
                        if (found.Length == 0)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        e.Effects = DragDropEffects.Copy;
                        e.Handled = true;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = false;
                    }

                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][26-11-2024][GEOS2-6379]
        private void DropRecordArticleGrid(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(ArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCategory> records = data.Records.OfType<ArticleCategory>().ToList();
                    foreach (ArticleCategory record in records)
                    {
                        List<ArticleCategory> lstArticles = new List<ArticleCategory>();
                        if (record.IdArticle > 0)
                        {
                            lstArticles.Add(record);
                        }
                        else
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


                            if (ArticleMenuList.Any(x => x.IdArticleCategory == record.IdArticleCategory))
                            {
                                GetAllArticles(record, lstArticles);
                            }

                        }
                        foreach (ArticleCategory article in lstArticles)
                        {
                            DataRow[] found = DataTableForArticleGridLayout.Select("Reference = '" + article.Name + "'");
                            if (found.Length == 0 && article.IdArticle > 0)
                            {
                                if (AddArticles == null)
                                    AddArticles = new List<UInt64>();
                                AddArticles.Add(Convert.ToUInt64(article.IdArticle));
                                //IsCalculated = false;
                                if (DataTableForArticleGridLayout == null)
                                    DataTableForArticleGridLayout = new DataTable();

                                InventoryAuditArticle InvAuditArticle = new InventoryAuditArticle();
                                DataRow dr = DataTableForArticleGridLayout.NewRow();
                                dr["CheckBox"] = false;
                                dr["Reference"] = article.Reference;
                                dr["Status"] = null;
                                dr["Description"] = article.Description;
                                dr["IdArticle"] = article.IdArticle;

                                InvAuditArticle.IdArticle = Convert.ToUInt32(article.IdArticle);
                                InvAuditArticle.Description = article.Description;
                                InvAuditArticle.Reference = article.Reference;
                                InvAuditArticle.InventoryAuditLocation = new List<InventoryAuditLocation>();
                                #region Add WarehouseLocations of selected Article
                                WarehouseLocationListByArticle = new ObservableCollection<InventoryAuditLocation>(WarehouseService.GetAllWarehouseLocationsByArticle(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, article.IdArticle));
                                if (WarehouseLocationListByArticle != null || WarehouseLocationListByArticle.Count > 0)
                                {
                                    //List<long> Temp = WarehouseLocationListByArticle.Select(j => j.IdWarehouseLocation).Except(InventoryAuditLocationList.Select(i => i.IdWarehouseLocation)).ToList();

                                    //if (Temp.Count == 0)
                                    //{
                                    //    dr["Status"] = "Audited";
                                    //    InvAuditArticle.IsAuditedYesNo = "Audited";
                                    //    InvAuditArticle.IsAudited = 1;
                                    //}
                                    foreach (InventoryAuditLocation InventoryAuditLocation in WarehouseLocationListByArticle)
                                    {
                                        if (!InventoryAuditLocationList.Any(i => i.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation))
                                        {
                                            if (DataTableForLocationGridLayout == null)
                                                DataTableForLocationGridLayout = new DataTable();
                                            WarehouseLocation WarehouseLocation = new WarehouseLocation();
                                            WarehouseLocation loc = WarehouseLocationList.Where(i => i.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation).FirstOrDefault();
                                            if (loc != null)
                                                WarehouseLocation.IdWarehouseLocation = loc.IdWarehouseLocation;

                                            //InventoryAuditLocation Warehouse = InventoryAuditLocationList.Where(i => i.IdWarehouseLocation == WarehouseLocation.IdWarehouseLocation).FirstOrDefault();
                                            if (WarehouseLocation.IdWarehouseLocation != 0)
                                            {
                                                DataRow Locationdr = DataTableForLocationGridLayout.NewRow();
                                                Locationdr["CheckBox"] = false;
                                                Locationdr["FullName"] = InventoryAuditLocation.FullName;
                                                Locationdr["Status"] = InventoryAuditLocation.IsAuditedYesNo;
                                                Locationdr["IdWarehouseLocation"] = InventoryAuditLocation.IdWarehouseLocation;
                                                Locationdr["IdParent"] = InventoryAuditLocation.Parent;
                                                Locationdr["Child"] = 1;
                                                WarehouseLocationList.Where(a => a.IdWarehouseLocation == InventoryAuditLocation.IdWarehouseLocation).ToList().ForEach(b => b.IsChecked = true);
                                                DataTableForLocationGridLayout.Rows.Add(Locationdr);
                                                InventoryAuditLocationList.Add(InventoryAuditLocation);
                                                InvAuditArticle.InventoryAuditLocation.Add(InventoryAuditLocation);
                                            }
                                        }
                                    }
                                }
                                DtLocation = DataTableForLocationGridLayout;
                                #endregion
                                ArticleMenuList.Where(a => a.IdArticle == article.IdArticle).ToList().ForEach(b => b.IsChecked = true);
                                DataTableForArticleGridLayout.Rows.Add(dr);
                                InventoryAuditArticleList.Add(InvAuditArticle);
                            }
                        }
                    }
                    DtArticle = DataTableForArticleGridLayout;
                    RemoveParentLocation();
                    AddParentAuditLocation();
                    e.Handled = true;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DropRecordArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][26-11-2024][GEOS2-6379]
        private void GetAllArticles(ArticleCategory article, List<ArticleCategory> lstArticles)
        {
            if (ArticleMenuList.Any(x => x.IdArticleCategory == article.IdArticleCategory))
            {
                List<ArticleCategory> subArticles = ArticleMenuList.Where(x => x.IdArticleCategory == article.IdArticleCategory).ToList();
                subArticles.ForEach(x => { if (!lstArticles.Any(y => y.IdArticleCategory == x.IdArticleCategory && y.IdArticle == x.IdArticle)) { lstArticles.Add(x); } });
                foreach (ArticleCategory cat in subArticles)
                {
                    //if (ArticleMenuList.FirstOrDefault(x => x.IdArticleCategory == cat.IdArticleCategory)?.IdArticle == 0)
                    if (cat.IdArticle == 0)
                    {
                        List<ArticleCategory> lstChildArticles = ArticleMenuList.Where(x => x.Parent == cat.IdArticleCategory)?.ToList();
                        if (ArticleMenuList.Any(x => x.IdArticleCategory == cat.IdArticleCategory))
                        {
                            //lstArticles.AddRange(ArticleMenuList.Where(x => x.IdArticleCategory == cat.IdArticleCategory).ToList());
                            ArticleMenuList.Where(x => x.IdArticleCategory == cat.IdArticleCategory).ToList().ForEach(x => { if (!lstArticles.Any(y => y.IdArticleCategory == x.IdArticleCategory && y.IdArticle == x.IdArticle)) { lstArticles.Add(x); } });
                        }
                        if (lstChildArticles != null)
                        {
                            lstArticles.AddRange(lstChildArticles);
                            foreach (ArticleCategory child in lstChildArticles)
                            {
                                GetAllArticles(child, lstArticles);
                            }
                        }
                    }
                }
            }
        }
        private void CompleteRecordDragDropLocationGrid(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropLocationGrid()...", category: Category.Info, priority: Priority.Low);
                e.Handled = false;
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropLocationGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropLocationGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOverLocationGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverLocationGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(WarehouseLocation).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    Warehouserecord = data.Records.OfType<WarehouseLocation>().ToList();
                    int count = 0;
                    foreach (WarehouseLocation Warehouse in Warehouserecord.Where(a => a.IdWarehouse > 0))
                    {
                        DataRow[] found = DataTableForLocationGridLayout.Select("FullName = '" + Warehouse.FullName + "'");
                        if (found.Length == 0)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        e.Effects = DragDropEffects.Copy;
                        e.Handled = true;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = false;
                    }

                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverLocationGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverLocationGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][26-11-2024][GEOS2-6379]
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void DropRecordLocationGrid(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordLocationGrid()...", category: Category.Info, priority: Priority.Low);
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
                if ((e.IsFromOutside) && typeof(WarehouseLocation).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));

                    foreach (WarehouseLocation mainItem in data.Records.OfType<WarehouseLocation>().ToList())
                    {
                        List<WarehouseLocation> lstRecords = new List<WarehouseLocation>() { mainItem };
                        List<WarehouseLocation> records = new List<WarehouseLocation>();
                        foreach (WarehouseLocation item in lstRecords)
                        {
                            List<WarehouseLocation> TempRec = WarehouseLocationList.Where(i => i.Parent == item.IdWarehouseLocation).ToList();
                            foreach (var item1 in TempRec)
                            {
                                if (WarehouseLocationList.Any(j => j.Parent == item1.IdWarehouseLocation))
                                {
                                    records.AddRange(WarehouseLocationList.Where(i => i.Parent == item1.IdWarehouseLocation).ToList());
                                }
                                else
                                {
                                    records = TempRec;
                                    break;
                                }
                            }
                        }
                        if (records.Count == 0)
                            records = lstRecords.ToList();
                        foreach (WarehouseLocation record in records.Where(a => a.IdWarehouseLocation > 0))
                        {
                            DataRow[] found = DataTableForLocationGridLayout.Select("FullName = '" + record.FullName + "'");
                            if (found.Length == 0)
                            {
                                if (AddWarehouseLocation == null)
                                    AddWarehouseLocation = new List<UInt64>();
                                AddWarehouseLocation.Add(Convert.ToUInt64(record.IdWarehouseLocation));
                                if (DataTableForLocationGridLayout == null)
                                    DataTableForLocationGridLayout = new DataTable();

                                InventoryAuditLocation InvAuditLocation = new InventoryAuditLocation();
                                DataRow dr = DataTableForLocationGridLayout.NewRow();
                                dr["CheckBox"] = false;
                                dr["FullName"] = record.FullName;
                                dr["Status"] = null;
                                dr["IdWarehouseLocation"] = record.IdWarehouseLocation;
                                dr["IdParent"] = record.Parent;
                                dr["Child"] = 1;
                                InvAuditLocation.FullName = record.FullName;
                                InvAuditLocation.IdWarehouseLocation = record.IdWarehouseLocation;
                                InvAuditLocation.InventoryAuditArticle = new List<InventoryAuditArticle>();
                                #region Add Articles of selected WarehouseLocation
                                //ArticleListByWarehouseLocation = new List<InventoryAuditArticle>(WarehouseService.GetAllArticlesByWarehouseLocation(WarehouseCommon.Instance.Selectedwarehouse.IdSite, record.IdWarehouseLocation));
                                //[pramod.misal][16.05.2024][GEOS2-5488]
                                //Shubham[skadam] GEOS2-6647 EWRO inventory 22 11 2024.
                                ArticleListByWarehouseLocation = new List<InventoryAuditArticle>(WarehouseService.GetAllArticlesByWarehouseLocation_V2520(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, record.IdWarehouseLocation));
                                if (ArticleListByWarehouseLocation != null)
                                {
                                    foreach (InventoryAuditArticle ArticleMenu in ArticleListByWarehouseLocation)
                                    {
                                        if (!InventoryAuditArticleList.Any(i => i.IdArticle == ArticleMenu.IdArticle))
                                        {
                                            if (DataTableForArticleGridLayout == null)
                                                DataTableForArticleGridLayout = new DataTable();
                                            ArticleCategory Article = ArticleMenuList.Where(i => i.IdArticle == ArticleMenu.IdArticle).FirstOrDefault();
                                            if (Article != null)
                                            {
                                                DataRow Articledr = DataTableForArticleGridLayout.NewRow();
                                                Articledr["CheckBox"] = false;
                                                Articledr["Reference"] = ArticleMenu.Reference;
                                                Articledr["Description"] = ArticleMenu.Description;
                                                Articledr["Status"] = ArticleMenu.IsAuditedYesNo;
                                                Articledr["IdArticle"] = ArticleMenu.IdArticle;
                                                Articledr["IdParent"] = ArticleMenu.Parent;
                                                ArticleMenuList.Where(a => a.IdArticle == ArticleMenu.IdArticle).ToList().ForEach(b => b.IsChecked = true);
                                                DataTableForArticleGridLayout.Rows.Add(Articledr);
                                                InventoryAuditArticleList.Add(ArticleMenu);
                                                InvAuditLocation.InventoryAuditArticle.Add(ArticleMenu);
                                            }
                                        }
                                    }
                                    DtArticle = DataTableForArticleGridLayout;
                                }

                                #endregion

                                WarehouseLocationList.Where(a => a.IdWarehouseLocation == record.IdWarehouseLocation).ToList().ForEach(b => b.IsChecked = true);
                                DataTableForLocationGridLayout.Rows.Add(dr);
                                InventoryAuditLocationList.Add(InvAuditLocation);
                                e.Handled = true;
                            }
                        }
                    }
                    RemoveParentLocation();
                    AddParentAuditLocation();
                    DtLocation = DataTableForLocationGridLayout;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DropRecordLocationGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordLocationGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void AddParentAuditLocation()
        {
            List<string> distinctIdParents = DataTableForLocationGridLayout.AsEnumerable()
                    .Select(row => row.Field<string>("IdParent"))
                    .Distinct()
                    .ToList();

            distinctIdParents.ForEach(x =>
            {
                if (AllWarehouseLocationList.FirstOrDefault(y => y.IdWarehouseLocation.ToString() == x) != null)
                {
                    DataRow[] foundRows = DataTableForLocationGridLayout.Select($"FullName = '{AllWarehouseLocationList.FirstOrDefault(y => y.IdWarehouseLocation.ToString() == x)}'");
                    if (foundRows.Length == 0)
                    {
                        WarehouseLocation loc = AllWarehouseLocationList.FirstOrDefault(y => y.IdWarehouseLocation.ToString() == x);

                        if (loc != null) // Ensure loc is not null
                        {
                            // Check if the record already exists in the DataTable
                            bool recordExists = DataTableForLocationGridLayout.AsEnumerable()
                                .Any(row => row.Field<object>("IdWarehouseLocation")?.ToString() == loc.IdWarehouseLocation.ToString());

                            if (!recordExists) // Only add if it doesn't exist
                            {
                                DataRow dr = DataTableForLocationGridLayout.NewRow();
                                dr["CheckBox"] = false;
                                dr["FullName"] = loc.FullName;
                                dr["Status"] = "";
                                dr["IdWarehouseLocation"] = loc.IdWarehouseLocation;
                                dr["IdParent"] = loc.Parent;
                                dr["Child"] = 0;
                                //// Update WarehouseLocationList if necessary
                                //WarehouseLocationList
                                //    .Where(a => a.IdWarehouseLocation == loc.IdWarehouseLocation)
                                //    .ToList()
                                //    .ForEach(b => b.IsChecked = true);

                                DataTableForLocationGridLayout.Rows.Add(dr); // Add the row to the DataTable
                            }
                        }

                        if (loc.Parent != 0)
                        {
                            DataRow[] foundRootRows = DataTableForLocationGridLayout.Select($"FullName = '{AllWarehouseLocationList.FirstOrDefault(y => y.IdWarehouseLocation == loc.Parent)}'");
                            if (foundRootRows.Length == 0)
                            {
                                if (AllWarehouseLocationList.FirstOrDefault(z => z.IdWarehouseLocation == loc.Parent) != null)
                                {
                                    WarehouseLocation loc2 = AllWarehouseLocationList.FirstOrDefault(y => y.IdWarehouseLocation == loc.Parent);

                                    if (loc2 != null) // Ensure loc2 is not null
                                    {
                                        // Check if the record already exists in the DataTable
                                        bool recordExists = DataTableForLocationGridLayout.AsEnumerable().Any(row => row.Field<object>("IdWarehouseLocation")?.ToString() == loc2.IdWarehouseLocation.ToString());

                                        if (!recordExists) // Only add if it doesn't exist
                                        {
                                            DataRow dr2 = DataTableForLocationGridLayout.NewRow();
                                            dr2["CheckBox"] = false;
                                            dr2["FullName"] = loc2.FullName;
                                            dr2["Status"] = "";
                                            dr2["IdWarehouseLocation"] = loc2.IdWarehouseLocation;
                                            dr2["IdParent"] = loc2.Parent;
                                            dr2["Child"] = 0;
                                            //// Update WarehouseLocationList if necessary
                                            //WarehouseLocationList
                                            //    .Where(a => a.IdWarehouseLocation == loc2.IdWarehouseLocation)
                                            //    .ToList()
                                            //    .ForEach(b => b.IsChecked = true);

                                            DataTableForLocationGridLayout.Rows.Add(dr2); // Add the row to the DataTable
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foundRows[0]["Child"] = 0;
                    }
                }
            });
            DataView view = DataTableForLocationGridLayout.DefaultView;
            view.Sort = "FullName ASC"; // Sort in ascending order
            DataTableForLocationGridLayout = view.ToTable();
            CalculateSummaryCount();
        }
		// [nsatpute][20-12-2024][GEOS2-6382]
        private void CalculateSummaryCount()
        {
            int summaryCount = 0;
            foreach (DataRow dr in DataTableForLocationGridLayout.Rows)
            {
                if (Convert.ToInt16(dr["Child"]) == 1)
                    summaryCount++;
            }
            TotalLocationSummaryItems = $"Total Count : {summaryCount}";
        }
		// [nsatpute][20-12-2024][GEOS2-6382]
        private void CalculateSummaryCount(string location)
        {
            int summaryCount = 0;
            foreach (DataRow dr in DataTableForLocationGridLayout.Rows)
            {
                if (Convert.ToInt16(dr["Child"]) == 1 && Convert.ToString(dr["FullName"]).ToUpper().Contains(location.ToUpper()))
                {                    
                    summaryCount++;
                }   
            }
            TotalLocationSummaryItems = $"Total Count : {summaryCount}";
        }


        //[nsatpute][05-12-2024][GEOS2-6380]
        private void RemoveParentLocation()
        {
            for (int i = dataTableForLocationGridLayout.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dataTableForLocationGridLayout.Rows[i];
                if (row.Field<int>("Child") == 0)
                {
                    dataTableForLocationGridLayout.Rows.Remove(row);
                }
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void DeleteRowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteRowCommandAction()...", category: Category.Info, priority: Priority.Low);
                //if (((DevExpress.Xpf.Grid.GridColumnData)obj).Column is TreeListColumn)
                if (obj is DataRowView)
                {
                    #region Article location

                    if (DtArticle == null)
                        DtArticle = new DataTable();
                    else
                    {
                        DataRow MainRow = ((System.Data.DataRowView)obj).Row;
                        string article = Convert.ToString(MainRow[1]);

                        if(!Convert.ToBoolean(MainRow[0]))
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Editinventoryauditview_SelectionRequired").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }

                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["Editinventoryauditview_ArticlesRemoveSingle"].ToString(), article), "Yellow",
                                  CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            DeleteAuditArticles(MainRow);
                        }
                    }
                    DtArticle = DataTableForArticleGridLayout;
                    #endregion
                }
                else
                {
                    string location = Convert.ToString(((System.Data.DataRowView)((DevExpress.Xpf.Grid.GridCellData)obj).Row).Row.ItemArray[1]);

                    if(!Convert.ToBoolean(((System.Data.DataRowView)((DevExpress.Xpf.Grid.GridCellData)obj).Row).Row.ItemArray[0]))
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Editinventoryauditview_SelectionRequired").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["Editinventoryauditview_LocationRemoveSingle"].ToString(), location), "Yellow",
                              CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        Int64 idWarehouseLocation = Convert.ToInt64(((System.Data.DataRowView)((DevExpress.Xpf.Grid.GridCellData)obj).Row).Row.ItemArray[4]);                        
                        DeleteAuditLocations(idWarehouseLocation);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteArticleCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteArticleCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void DeleteAuditLocations(long idWarehouseLocation)
        {
            #region Location Delete
            if (DtLocation == null)
                DtLocation = new DataTable();
            else
            {
                foreach (InventoryAuditLocation loc in InventoryAuditLocationList)
                {
                    if (loc.Parent == 0)
                    {
                        loc.Parent = AllWarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == loc.IdWarehouseLocation).Parent;
                    }
                }

                //ArticleListByWarehouseLocation = new List<InventoryAuditArticle>(WarehouseService.GetAllArticlesByWarehouseLocation(WarehouseCommon.Instance.Selectedwarehouse.IdSite, IdWarehouseLocation));
                //[pramod.misal][16.05.2024][GEOS2-5488]
                //Shubham[skadam] GEOS2-6647 EWRO inventory 22 11 2024.
                List<long> lstIdwarehouseLocations = new List<long>();
                List<long> lstChileLocations = new List<long>();
                lstIdwarehouseLocations.Add(idWarehouseLocation);
                GetAllChildLocations(idWarehouseLocation, lstChileLocations);

                InventoryAuditLocationList.ToList().ForEach(x =>
                {
                   if(lstChileLocations.Contains(x.IdWarehouseLocation))
                        lstIdwarehouseLocations.Add(x.IdWarehouseLocation);
                });

                ArticleListByWarehouseLocation = new List<InventoryAuditArticle>(WarehouseService.GetAllArticlesByWarehouseLocations(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, string.Join(",", lstIdwarehouseLocations)));
                
                
                WarehouseLocationList.Where(a => a.IdWarehouseLocation == idWarehouseLocation).ToList().ForEach(b => b.IsChecked = false);
                WarehouseLocationList.Where(a => a.Parent == idWarehouseLocation).ToList().ForEach(b => b.IsChecked = false);
                foreach(WarehouseLocation wrMain in WarehouseLocationList.Where(a => a.Parent == idWarehouseLocation).ToList())
                {
                    if(wrMain.Parent == idWarehouseLocation)
                    {
                        WarehouseLocationList.ForEach(x=> { if (x.Parent == wrMain.IdWarehouseLocation) { x.IsChecked = false; } } );
                    }
                }

                //if(DataTableForLocationGridLayout.Select($"IdWarehouseLocation = {Convert.ToString(idWarehouseLocation)}").ToArray().Count() >0)
                List <AuditLocation> lstAuditLocation = new List<AuditLocation>();
                foreach (DataRow dr in DataTableForLocationGridLayout.Rows)
                {
                    AuditLocation loc = new AuditLocation();
                    object cellValue = dr["CheckBox"];
                    if (cellValue != null && cellValue != DBNull.Value)
                        loc.CheckBox = Convert.ToBoolean(dr["CheckBox"]);
                    else
                        loc.CheckBox = false;
                    loc.FullName = Convert.ToString(dr["FullName"]);
                    loc.Status = Convert.ToString(dr["Status"]);
                    loc.Delete = Convert.ToString(dr["Delete"]);
                    loc.IdWarehouseLocation = Convert.ToInt64(dr["IdWarehouseLocation"]);
                    loc.IdParent = Convert.ToInt64(dr["IdParent"]);
                    loc.Child = Convert.ToInt32(dr["Child"]);
                    lstAuditLocation.Add(loc);
                }
                List<long> lstLocationToDelete = new List<long>();
                GetAllChildLocations(idWarehouseLocation, lstAuditLocation, lstLocationToDelete);

                foreach (AuditLocation loc in lstAuditLocation.ToList())
                {
                    if (lstLocationToDelete.Contains(loc.IdWarehouseLocation))
                        lstAuditLocation.Remove(loc);
                }

                DataTableForLocationGridLayout.Clear();
                foreach (AuditLocation loc in lstAuditLocation)
                {
                    DataRow Locationdr = DataTableForLocationGridLayout.NewRow();
                    Locationdr["CheckBox"] = loc.CheckBox;
                    Locationdr["FullName"] = loc.FullName;
                    Locationdr["Status"] = loc.Status;
                    Locationdr["IdWarehouseLocation"] = loc.IdWarehouseLocation;
                    Locationdr["IdParent"] = loc.IdParent;
                    Locationdr["Child"] = loc.Child;
                    DataTableForLocationGridLayout.Rows.Add(Locationdr);
                }

                if (ArticleListByWarehouseLocation.Count > 0)
                {
                    foreach (InventoryAuditArticle article in ArticleListByWarehouseLocation)
                    {
                        InventoryAuditArticle tempArticle = InventoryAuditArticleList.Where(i => i.IdArticle == article.IdArticle).FirstOrDefault();
                        if (tempArticle != null)
                        {
                            //if (InventoryAuditLocationList.Any(i => i.InventoryAuditArticle.Any(j => j.IdArticle == tempArticle.IdArticle)))
                            if (InventoryAuditLocationList != null &&
                                    InventoryAuditLocationList.Any(i => i.InventoryAuditArticle != null &&
                                         i.InventoryAuditArticle.Any(j => j.IdArticle == tempArticle.IdArticle)))
                            {
                                InventoryAuditArticleList.Remove(InventoryAuditArticleList.Where(i => i.IdArticle == article.IdArticle).FirstOrDefault());
                                ArticleMenuList.Where(a => a.IdArticle == article.IdArticle).ToList().ForEach(b => b.IsChecked = false);
                            }
                        }
                    }
                    DataTableForArticleGridLayout.Clear();
                    dtArticle.Clear();
                    FillArticleGrid();
                }
                InventoryAuditLocationList.Remove(InventoryAuditLocationList.Where(i => i.IdWarehouseLocation == idWarehouseLocation).FirstOrDefault());
                InventoryAuditLocationList.Remove(InventoryAuditLocationList.Where(i => i.Parent == idWarehouseLocation).FirstOrDefault());

                InventoryAuditLocationList.ToList().ForEach(x => {
                    if (lstIdwarehouseLocations.Contains(x.IdWarehouseLocation))
                        InventoryAuditLocationList.Remove(x);
                });
            }
            DtLocation = DataTableForLocationGridLayout;
            RemoveParentLocation();
            AddParentAuditLocation();
            #endregion
        }
        private void GetAllChildLocations(long idWarehouseLocation, List<AuditLocation> lstAuditLocation, List<long> lstLocationToDelete)
        {
            foreach (AuditLocation loc in lstAuditLocation.ToList())
            {
                if (loc.IdWarehouseLocation == idWarehouseLocation)
                {
                    lstLocationToDelete.Add(idWarehouseLocation);
                }
                if (loc.IdParent == idWarehouseLocation && !lstLocationToDelete.Contains(loc.IdWarehouseLocation))
                {
                    lstLocationToDelete.Add(loc.IdWarehouseLocation);
                    GetAllChildLocations(loc.IdWarehouseLocation, lstAuditLocation, lstLocationToDelete);
                }
            }
        }
		//[nsatpute][12-12-2024][GEOS2-6382] 
        private void GetAllChildLocations(long idWarehouseLocation, List<long> LstLocations)
        {
            foreach (WarehouseLocation loc in AllWarehouseLocationList)
            {
                if (loc.IdWarehouseLocation == idWarehouseLocation)
                {
                    LstLocations.Add(idWarehouseLocation);
                }
                if (loc.Parent == idWarehouseLocation && !LstLocations.Contains(loc.IdWarehouseLocation))
                {
                    LstLocations.Add(loc.IdWarehouseLocation);
                    GetAllChildLocations(loc.IdWarehouseLocation, LstLocations);
                }
            }
        }
        //[nsatpute][05-12-2024][GEOS2-6380]
        private void DeleteAuditArticles(DataRow MainRow)
        {
            Int64 idArticle = Convert.ToInt64(MainRow["IdArticle"]);
            //WarehouseLocationListByArticle = new ObservableCollection<InventoryAuditLocation>(WarehouseService.GetAllWarehouseLocationsByArticle(WarehouseCommon.Instance.Selectedwarehouse.IdSite, idArticle));
            //if (WarehouseLocationListByArticle.Count > 0 && showWarningMessage)
            //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteLocationsofArticleWarning").ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
            //showWarningMessage = false;
            InventoryAuditArticleList.Remove(InventoryAuditArticleList.Where(i => i.IdArticle == Convert.ToUInt64(idArticle)).FirstOrDefault());
            ArticleMenuList.Where(a => a.IdArticle == idArticle).ToList().ForEach(b => b.IsChecked = false);
            //DataTableForArticleGridLayout.Rows.Remove(MainRow);
            foreach (DataRow row in DataTableForArticleGridLayout.Select($"IdArticle = '{idArticle}'"))
            {
                DataTableForArticleGridLayout.Rows.Remove(row);
            }
            //if (WarehouseLocationListByArticle.Count > 0)
            //{
            //    foreach (InventoryAuditLocation Location in WarehouseLocationListByArticle)
            //    {
            //        InventoryAuditLocation tempLocation = InventoryAuditLocationList.Where(i => i.IdWarehouseLocation == Location.IdWarehouseLocation).FirstOrDefault();
            //        if (tempLocation != null)
            //        {
            //            if (!InventoryAuditArticleList.Any(j => j.InventoryAuditLocation.Any(i => i.IdWarehouseLocation == tempLocation.IdWarehouseLocation)))
            //            {
            //                InventoryAuditLocationList.Remove(InventoryAuditLocationList.Where(i => i.IdWarehouseLocation == Location.IdWarehouseLocation).FirstOrDefault());
            //                WarehouseLocationList.Where(a => a.IdWarehouseLocation == Location.IdWarehouseLocation).ToList().ForEach(b => b.IsChecked = false);
            //            }
            //        }
            //    }
            //    DtLocation.Clear();
            //    DataTableForLocationGridLayout.Clear();
            //    FillLocationGrid();
            //}
        }

        //[nsatpute][05-12-2024][GEOS2-6381]
        private void DeleteSelectedRowsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteSelectedRowsCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (Convert.ToString(obj) == "ArticleAuditGrid")
                {
                    int checkedCount = DataTableForArticleGridLayout.AsEnumerable().Count(row => row.Field<bool>("CheckBox"));
                    if (checkedCount > 0)
                    {
                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["Editinventoryauditview_ArticlesRemoveMultiple"].ToString(), checkedCount), "Yellow",
                                  CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            DataTable clonedDataTable = DataTableForArticleGridLayout.Clone();
                            foreach (DataRow row in DataTableForArticleGridLayout.Rows)
                            {
                                DataRow newRow = clonedDataTable.NewRow();
                                newRow.ItemArray = row.ItemArray.Clone() as object[];
                                clonedDataTable.Rows.Add(newRow);
                            }
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
                            foreach (DataRow r in clonedDataTable.Rows)
                            {
                                if (Convert.ToBoolean(r["CheckBox"]))
                                    DeleteAuditArticles(r);
                            }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Editinventoryauditview_OneSelectionRequired").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    int checkedCount = DtLocation.AsEnumerable().Count(row =>
    (row.Field<bool?>("Checkbox") ?? false) &&
    (row.Field<int>("Child") == 1));


                    if (checkedCount > 0)
                    {
                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["Editinventoryauditview_LocationRemoveMultiple"].ToString(), checkedCount), "Yellow",
                                  CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            List<long> idWarehouseLocations = DtLocation.AsEnumerable()
                                .Where(row => row.Field<bool?>("Checkbox") ?? false)
                                .Select(row => Convert.ToInt64(row["IdWarehouseLocation"]))
                                .ToList();

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
                            foreach (long idWarehouseLocation in idWarehouseLocations)
                            {
                                DeleteAuditLocations(idWarehouseLocation);
                            }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Editinventoryauditview_OneSelectionRequired").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }

                }
                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed DeleteSelectedRowsCommandAction", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteSelectedRowsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        //[nsatpute][05-12-2024][GEOS2-6381]
        private void DeleteChildRecords(DataRow row)
        {
            if (Convert.ToInt16(row.ItemArray[6]) == 1)
            {
                DeleteChildRecords(row);
            }
            else
            {
                int IdWarhouseLocation = Convert.ToInt32(row.ItemArray[4]);
                foreach (DataRow row2 in DataTableForLocationGridLayout.Select($"IdParent = {IdWarhouseLocation}").ToArray())
                {
                    if (Convert.ToInt16(row2.ItemArray[6]) == 1)
                    {
                        DataTableForLocationGridLayout.Rows.Remove(row2);
                    }
                    else
                    {
                        DeleteChildRecords(row2);
                        DataTableForLocationGridLayout.Rows.Remove(row2);
                    }
                }
            }
        }
        #endregion

        #region [rdixit][19.12.2022][GEOS2-3965] Method
        private void PrintInventoryAuditList(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintInventoryAuditList...", category: Category.Info, priority: Priority.Low);
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
                List<string> YAxisLocations = new List<string>();
                InventoryAuditReportList = new ObservableCollection<PrintInventoryAuditReport>();
                InventoryAuditReportPrintView InventoryAuditReportPrint = new InventoryAuditReportPrintView();
                InventoryAuditReportPrint.xrChart1.BeginInit();
                InventoryAuditReportPrint.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                InventoryAuditReportPrint.SiteLbl.Text = WarehouseCommon.Instance.Selectedwarehouse.Company.Alias;
                InventoryAuditReportPrint.SiteLbl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InventoryAuditReportPrint.TitalLbl.Text = "Warehouse Inventory " + StartDate.Value.ToShortDateString() + " " + EndDate.Value.Date.ToShortDateString();
                InventoryAuditReportPrint.TitalLbl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InventoryAuditReportPrint.xrChart1.DataSource = WarehouseInventoryAuditItemsList;
                InventoryAuditReportPrint.AuditLbl.Text = SelectedInventoryAudit.SuccessRate.ToString() + "%";
                InventoryAuditReportPrint.ReviewedLbl.Text = SelectedInventoryAudit.SuccessRate.ToString() + "%";
                InventoryAuditReportPrint.BalAmtLbl.Text = SelectedInventoryAudit.BalanceAmountwithCurrentSymbol.ToString();
                InventoryAuditReportPrint.LossAmtLbl.Text = WarehouseInventoryAuditItemsList.Where(i => i.BalanceAmount < 0).ToList().Sum(j => j.BalanceAmount).ToString() + " " + SelectedCurrency.Symbol;
                InventoryAuditReportPrint.GainAmtLbl.Text = "+ " + WarehouseInventoryAuditItemsList.Where(i => i.BalanceAmount >= 0).ToList().Sum(j => j.BalanceAmount).ToString() + " " + SelectedCurrency.Symbol;
                if (SelectedInventoryAudit.BalanceAmount < 0)
                    InventoryAuditReportPrint.BalAmtLbl.ForeColor = System.Drawing.Color.Red;
                else
                    InventoryAuditReportPrint.BalAmtLbl.ForeColor = System.Drawing.Color.Green;
                InventoryAuditReportPrint.CurDate.Text = DateTime.Now.ToString();


                #region Fill Total Audited and Descrepencies Details
                foreach (WarehouseInventoryAuditItem item in WarehouseInventoryAuditItemsList)
                {
                    string temp = item.WarehouseLocation.FullName.Substring(0, 2);
                    if (!YAxisLocations.Any(i => i == temp))
                    {
                        YAxisLocations.Add(temp);
                        PrintInventoryAuditReport Report = new PrintInventoryAuditReport();
                        Report.MainWarehouseLocation = temp;
                        //Report.TotalAudited = 1;
                        InventoryAuditReportList.Add(Report);
                        #region Temp Commented
                        // if (item.DifferenceBackgoundColor == notOKColor)



                        /*
                        if (item.DifferenceBackgoundColor == oKColor)
                        {
                            if (temp == InventoryAuditReportPrint.th1.Text)
                                InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th2.Text)
                                InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th3.Text)
                                InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th4.Text)
                                InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th5.Text)
                                InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th6.Text)
                                InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th7.Text)
                                InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th8.Text)
                                InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                        }
                        else
                        {
                           // Report.Descrepancies = 1;
                            if (temp == InventoryAuditReportPrint.th1.Text)
                            {
                                InventoryAuditReportPrint.tr21.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr21.Text) + 1);
                                InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th2.Text)
                            {
                                InventoryAuditReportPrint.tr22.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr22.Text) + 1);
                                InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th3.Text)
                            {
                                InventoryAuditReportPrint.tr23.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr23.Text) + 1);
                                InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th4.Text)
                            {
                                InventoryAuditReportPrint.tr24.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr24.Text) + 1);
                                InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th5.Text)
                            {
                                InventoryAuditReportPrint.tr25.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr25.Text) + 1);
                                InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th6.Text)
                            {
                                InventoryAuditReportPrint.tr26.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr26.Text) + 1);
                                InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th7.Text)
                            {
                                InventoryAuditReportPrint.tr27.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr27.Text) + 1);
                                InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th8.Text)
                            {
                                InventoryAuditReportPrint.tr28.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr28.Text) + 1);
                                InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                            }
                        }
                    }
                    else
                    {
                        if (item.DifferenceBackgoundColor == oKColor)
                        {                           
                            if (temp == InventoryAuditReportPrint.th1.Text)
                                InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th2.Text)
                                InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th3.Text)
                                InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th4.Text)
                                InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th5.Text)
                                InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th6.Text)
                                InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th7.Text)
                                InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                            if (temp == InventoryAuditReportPrint.th8.Text)
                                InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                        }
                        else
                        {                          
                            if (temp == InventoryAuditReportPrint.th1.Text)
                            {
                                InventoryAuditReportPrint.tr21.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr21.Text) + 1);
                                InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th2.Text)
                            {
                                InventoryAuditReportPrint.tr22.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr22.Text) + 1);
                                InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th3.Text)
                            {
                                InventoryAuditReportPrint.tr23.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr23.Text) + 1);
                                InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th4.Text)
                            {
                                InventoryAuditReportPrint.tr24.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr24.Text) + 1);
                                InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th5.Text)
                            {
                                InventoryAuditReportPrint.tr25.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr25.Text) + 1);
                                InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th6.Text)
                            {
                                InventoryAuditReportPrint.tr26.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr26.Text) + 1);
                                InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th7.Text)
                            {
                                InventoryAuditReportPrint.tr27.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr27.Text) + 1);
                                InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                            }
                            if (temp == InventoryAuditReportPrint.th8.Text)
                            {
                                InventoryAuditReportPrint.tr28.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr28.Text) + 1);
                                InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                            }
                        }*/
                        #endregion
                    }
                    InventoryAuditReportList.Where(i => i.MainWarehouseLocation == temp).FirstOrDefault().TotalAudited += 1;
                    if (item.DifferenceBackgoundColor == oKColor)
                    {
                        if (temp == InventoryAuditReportPrint.th1.Text)
                            InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th2.Text)
                            InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th3.Text)
                            InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th4.Text)
                            InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th5.Text)
                            InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th6.Text)
                            InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th7.Text)
                            InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th8.Text)
                            InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th9.Text)
                            InventoryAuditReportPrint.tr19.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr19.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th10.Text)
                            InventoryAuditReportPrint.tr110.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr110.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th11.Text)
                            InventoryAuditReportPrint.tr111.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr111.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th12.Text)
                            InventoryAuditReportPrint.tr112.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr112.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th13.Text)
                            InventoryAuditReportPrint.tr113.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr113.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th14.Text)
                            InventoryAuditReportPrint.tr114.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr114.Text) + 1);
                    }
                    else
                    {
                        InventoryAuditReportList.Where(i => i.MainWarehouseLocation == temp).FirstOrDefault().Descrepancies += 1;
                        if (temp == InventoryAuditReportPrint.th1.Text)
                        {
                            InventoryAuditReportPrint.tr21.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr21.Text) + 1);
                            InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th2.Text)
                        {
                            InventoryAuditReportPrint.tr22.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr22.Text) + 1);
                            InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th3.Text)
                        {
                            InventoryAuditReportPrint.tr23.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr23.Text) + 1);
                            InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th4.Text)
                        {
                            InventoryAuditReportPrint.tr24.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr24.Text) + 1);
                            InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th5.Text)
                        {
                            InventoryAuditReportPrint.tr25.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr25.Text) + 1);
                            InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th6.Text)
                        {
                            InventoryAuditReportPrint.tr26.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr26.Text) + 1);
                            InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th7.Text)
                        {
                            InventoryAuditReportPrint.tr27.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr27.Text) + 1);
                            InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th8.Text)
                        {
                            InventoryAuditReportPrint.tr28.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr28.Text) + 1);
                            InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th9.Text)
                        {
                            InventoryAuditReportPrint.tr29.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr29.Text) + 1);
                            InventoryAuditReportPrint.tr19.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr19.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th10.Text)
                        {
                            InventoryAuditReportPrint.tr210.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr210.Text) + 1);
                            InventoryAuditReportPrint.tr110.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr110.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th11.Text)
                        {
                            InventoryAuditReportPrint.tr211.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr211.Text) + 1);
                            InventoryAuditReportPrint.tr111.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr111.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th12.Text)
                        {
                            InventoryAuditReportPrint.tr212.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr212.Text) + 1);
                            InventoryAuditReportPrint.tr112.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr112.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th13.Text)
                        {
                            InventoryAuditReportPrint.tr213.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr213.Text) + 1);
                            InventoryAuditReportPrint.tr113.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr113.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th14.Text)
                        {
                            InventoryAuditReportPrint.tr214.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr214.Text) + 1);
                            InventoryAuditReportPrint.tr114.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr114.Text) + 1);
                        }
                    }
                }
                #endregion

                #region Fill Percentage
                foreach (PrintInventoryAuditReport item in InventoryAuditReportList)
                {
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th1.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr21.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr11.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr31.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr31.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th2.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr22.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr12.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr32.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr32.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th3.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr23.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr13.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr33.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr33.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th4.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr24.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr14.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr34.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr34.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th5.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr25.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr15.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr35.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr35.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th6.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr26.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr16.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr36.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr36.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th7.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr27.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr17.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr37.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr37.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th8.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr28.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr18.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr38.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr38.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th9.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr29.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr19.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr39.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage * 100 == 0)
                            InventoryAuditReportPrint.tr39.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th10.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr210.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr110.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr310.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr310.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th11.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr211.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr111.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr311.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr311.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th12.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr212.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr112.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr312.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr312.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th13.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr213.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr113.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr313.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr313.BackColor = System.Drawing.Color.Green;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th14.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr214.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr114.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr314.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr314.BackColor = System.Drawing.Color.Green;
                    }
                }

                #endregion

                double AuditTotal = Math.Round((double)InventoryAuditReportList.Sum(i => i.TotalAudited));
                double descrTotal = Math.Round((double)InventoryAuditReportList.Sum(i => i.Descrepancies));
                InventoryAuditReportPrint.tr1Total.Text = Convert.ToString(AuditTotal);
                InventoryAuditReportPrint.tr2Total.Text = Convert.ToString(descrTotal);
                InventoryAuditReportPrint.tr3Total.Text = Convert.ToString(Math.Round((descrTotal / AuditTotal) * 100, 2, MidpointRounding.AwayFromZero)) + "%";

                DocumentPreviewWindow window = new DocumentPreviewWindow() { Owner = (EditInventoryAuditView)obj, WindowState = WindowState.Maximized, Top = 1 };
                window.PreviewControl.DocumentSource = InventoryAuditReportPrint;
                window.PreviewControl.Height = 950;
                InventoryAuditReportPrint.CreateDocument();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                window.Show();
            }
            catch (Exception ex)
            {
                //IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Print - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintInventoryAuditList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region GEOS2-3965
        private void PrintInventoryAuditListNew(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintInventoryAuditList...", category: Category.Info, priority: Priority.Low);
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
                List<string> YAxisLocations = new List<string>();
                InventoryAuditReportList = new ObservableCollection<PrintInventoryAuditReport>();
                InventoryAuditPrintReport InventoryAuditReportPrint = new InventoryAuditPrintReport();
                InventoryAuditReportPrint.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                InventoryAuditReportPrint.xrSiteLbl.Text = WarehouseCommon.Instance.Selectedwarehouse.Name;
                InventoryAuditReportPrint.xrSiteLbl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                InventoryAuditReportPrint.xrSiteLbl.BackColor = System.Drawing.Color.Black;
                InventoryAuditReportPrint.xrSiteLbl.ForeColor = System.Drawing.Color.White;
                string startd = StartDate == null ? "" : StartDate.Value.ToShortDateString();
                string endd = EndDate == null ? "" : EndDate.Value.Date.ToShortDateString();
                InventoryAuditReportPrint.xrHeaderLbl.Text = "Warehouse Inventory " + startd + " - " + endd;
                InventoryAuditReportPrint.xrHeaderLbl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                //InventoryAuditReportPrint.xrLblAudited.Text = InventoryAuditReportPrint.xrLblAudited.Text + " " + SelectedInventoryAudit.SuccessRate.ToString() + "%";
                //InventoryAuditReportPrint.xrLblReViewed.Text = InventoryAuditReportPrint.xrLblReViewed.Text +" "+ SelectedInventoryAudit.SuccessRate.ToString() + "%";
                Bitmap BitmapimgLogo = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Emdep_logo_mini);
                InventoryAuditReportPrint.imgLogo.Image = BitmapimgLogo;
                InventoryAuditReportPrint.imgLogo.Height = BitmapimgLogo.Height;
                InventoryAuditReportPrint.imgLogo.WidthF = BitmapimgLogo.Width;

                InventoryAuditReportPrint.BalAmtLbl.Text = SelectedInventoryAudit.BalanceAmountwithCurrentSymbol.ToString();
                InventoryAuditReportPrint.LossAmtLbl.Text = WarehouseInventoryAuditItemsList.Where(i => i.BalanceAmount < 0).ToList().Sum(j => j.BalanceAmount).ToString() + " " + SelectedCurrency.Symbol;
                InventoryAuditReportPrint.GainAmtLbl.Text = "+" + WarehouseInventoryAuditItemsList.Where(i => i.BalanceAmount >= 0).ToList().Sum(j => j.BalanceAmount).ToString() + " " + SelectedCurrency.Symbol;
                if (SelectedInventoryAudit.BalanceAmount < 0)
                    InventoryAuditReportPrint.BalAmtLbl.ForeColor = System.Drawing.Color.Red;
                else
                    InventoryAuditReportPrint.BalAmtLbl.ForeColor = System.Drawing.Color.Green;
                InventoryAuditReportPrint.CurDate.Text = DateTime.Now.ToString();


                int ReviewedCount = 0;
                #region Fill Total Audited and Descrepencies Details
                foreach (WarehouseInventoryAuditItem item in WarehouseInventoryAuditItemsList)
                {
                    try
                    {
                        #region GEOS2-3965
                        if ((item.CurrentQuantity - item.ExpectedQuantity) == 0)
                        {
                            ReviewedCount = ReviewedCount + 1;
                        }
                        else if (item.Approver != null)
                        {
                            if (!string.IsNullOrEmpty(item.Approver.FirstName))
                            {
                                ReviewedCount = ReviewedCount + 1;
                            }
                            else if (!string.IsNullOrEmpty(item.Approver.FullName))
                            {
                                // ReviewedCount = ReviewedCount + 1;
                            }
                            else
                            {

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                    string temp = item.WarehouseLocation.FullName.Substring(0, 2);
                    if (!YAxisLocations.Any(i => i == temp))
                    {
                        YAxisLocations.Add(temp);
                        PrintInventoryAuditReport Report = new PrintInventoryAuditReport();
                        Report.MainWarehouseLocation = temp;
                        //Report.TotalAudited = 1;
                        InventoryAuditReportList.Add(Report);
                    }
                    InventoryAuditReportList.Where(i => i.MainWarehouseLocation == temp).FirstOrDefault().TotalAudited += 1;
                    if (item.DifferenceBackgoundColor == oKColor)
                    {
                        if (temp == InventoryAuditReportPrint.th1.Text)
                            InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th2.Text)
                            InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th3.Text)
                            InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th4.Text)
                            InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th5.Text)
                            InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th6.Text)
                            InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th7.Text)
                            InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th8.Text)
                            InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th9.Text)
                            InventoryAuditReportPrint.tr19.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr19.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th10.Text)
                            InventoryAuditReportPrint.tr110.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr110.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th11.Text)
                            InventoryAuditReportPrint.tr111.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr111.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th12.Text)
                            InventoryAuditReportPrint.tr112.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr112.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th13.Text)
                            InventoryAuditReportPrint.tr113.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr113.Text) + 1);
                        if (temp == InventoryAuditReportPrint.th14.Text)
                            InventoryAuditReportPrint.tr114.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr114.Text) + 1);
                    }
                    else
                    {
                        InventoryAuditReportList.Where(i => i.MainWarehouseLocation == temp).FirstOrDefault().Descrepancies += 1;
                        if (temp == InventoryAuditReportPrint.th1.Text)
                        {
                            InventoryAuditReportPrint.tr21.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr21.Text) + 1);
                            InventoryAuditReportPrint.tr11.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr11.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th2.Text)
                        {
                            InventoryAuditReportPrint.tr22.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr22.Text) + 1);
                            InventoryAuditReportPrint.tr12.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr12.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th3.Text)
                        {
                            InventoryAuditReportPrint.tr23.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr23.Text) + 1);
                            InventoryAuditReportPrint.tr13.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr13.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th4.Text)
                        {
                            InventoryAuditReportPrint.tr24.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr24.Text) + 1);
                            InventoryAuditReportPrint.tr14.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr14.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th5.Text)
                        {
                            InventoryAuditReportPrint.tr25.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr25.Text) + 1);
                            InventoryAuditReportPrint.tr15.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr15.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th6.Text)
                        {
                            InventoryAuditReportPrint.tr26.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr26.Text) + 1);
                            InventoryAuditReportPrint.tr16.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr16.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th7.Text)
                        {
                            InventoryAuditReportPrint.tr27.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr27.Text) + 1);
                            InventoryAuditReportPrint.tr17.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr17.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th8.Text)
                        {
                            InventoryAuditReportPrint.tr28.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr28.Text) + 1);
                            InventoryAuditReportPrint.tr18.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr18.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th9.Text)
                        {
                            InventoryAuditReportPrint.tr29.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr29.Text) + 1);
                            InventoryAuditReportPrint.tr19.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr19.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th10.Text)
                        {
                            InventoryAuditReportPrint.tr210.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr210.Text) + 1);
                            InventoryAuditReportPrint.tr110.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr110.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th11.Text)
                        {
                            InventoryAuditReportPrint.tr211.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr211.Text) + 1);
                            InventoryAuditReportPrint.tr111.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr111.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th12.Text)
                        {
                            InventoryAuditReportPrint.tr212.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr212.Text) + 1);
                            InventoryAuditReportPrint.tr112.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr112.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th13.Text)
                        {
                            InventoryAuditReportPrint.tr213.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr213.Text) + 1);
                            InventoryAuditReportPrint.tr113.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr113.Text) + 1);
                        }
                        if (temp == InventoryAuditReportPrint.th14.Text)
                        {
                            InventoryAuditReportPrint.tr214.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr214.Text) + 1);
                            InventoryAuditReportPrint.tr114.Text = Convert.ToString(Convert.ToInt32(InventoryAuditReportPrint.tr114.Text) + 1);
                        }
                    }
                }
                #endregion

                #region Fill Percentage
                foreach (PrintInventoryAuditReport item in InventoryAuditReportList)
                {
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th1.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr21.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr11.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr31.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr31.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th2.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr22.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr12.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr32.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr32.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th3.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr23.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr13.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr33.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr33.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th4.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr24.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr14.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr34.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr34.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th5.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr25.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr15.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr35.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr35.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th6.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr26.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr16.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr36.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr36.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th7.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr27.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr17.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr37.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr37.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th8.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr28.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr18.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr38.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr38.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th9.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr29.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr19.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr39.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage * 100 == 0)
                            InventoryAuditReportPrint.tr39.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th10.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr210.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr110.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr310.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr310.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th11.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr211.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr111.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr311.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr311.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th12.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr212.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr112.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr312.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr312.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th13.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr213.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr113.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr313.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr313.BackColor = System.Drawing.Color.LightGreen;
                    }
                    if (item.MainWarehouseLocation == InventoryAuditReportPrint.th14.Text)
                    {
                        double desc = Convert.ToInt32(InventoryAuditReportPrint.tr214.Text);
                        double total = Convert.ToInt32(InventoryAuditReportPrint.tr114.Text);
                        item.Percentage = Math.Round((desc / total) * 100, 2, MidpointRounding.AwayFromZero);
                        InventoryAuditReportPrint.tr314.Text = Convert.ToString(item.Percentage) + "%";
                        if (item.Percentage == 0)
                            InventoryAuditReportPrint.tr314.BackColor = System.Drawing.Color.LightGreen;
                    }
                }

                #endregion

                double AuditTotal = Math.Round((double)InventoryAuditReportList.Sum(i => i.TotalAudited));
                double descrTotal = Math.Round((double)InventoryAuditReportList.Sum(i => i.Descrepancies));
                InventoryAuditReportPrint.tr1Total.Text = Convert.ToString(AuditTotal);
                InventoryAuditReportPrint.tr2Total.Text = Convert.ToString(descrTotal);
                InventoryAuditReportPrint.tr3Total.Text = Convert.ToString(Math.Round((descrTotal / AuditTotal) * 100, 2, MidpointRounding.AwayFromZero)) + "%";
                int CountAudited = 0;
                int ToatlAuditedCount = 0;
                try
                {
                    double ReViewed = (double)Math.Round((double)(100 * ReviewedCount) / AuditTotal);
                    if (Double.IsNaN(ReViewed))
                    {
                        ReViewed = 0;
                    }
                    InventoryAuditReportPrint.xrLblReViewed.Text = InventoryAuditReportPrint.xrLblReViewed.Text + " " + ReViewed + "%";
                    #region GEOS2-3965
                    try
                    {
                        foreach (InventoryAuditArticle InventoryAuditArticleitem in InventoryAuditArticleList)
                        {
                            if (InventoryAuditArticleitem.InventoryAuditLocation != null)
                            {
                                //ToatlAuditedCount = ToatlAuditedCount + InventoryAuditArticleitem.InventoryAuditLocation.Count();
                            }
                            //if (InventoryAuditArticleitem.InventoryAuditLocation.Count()>1)
                            //{

                            //}
                            //if (InventoryAuditArticleitem.Reference.Equals("3PEWC-WE-V7"))
                            //{

                            //}
                            if (InventoryAuditArticleitem.InventoryAuditLocation != null)
                            {
                                foreach (InventoryAuditLocation InventoryAuditLocationitem in InventoryAuditArticleitem.InventoryAuditLocation)
                                {
                                    if (InventoryAuditLocationList.Any(a => a.IdWarehouseLocation == InventoryAuditLocationitem.IdWarehouseLocation))
                                    {
                                        ToatlAuditedCount = ToatlAuditedCount + 1;
                                    }
                                }
                            }
                            if (WarehouseInventoryAuditItemsList.Any(a => a.IdArticle == InventoryAuditArticleitem.IdArticle))
                            {
                                List<WarehouseInventoryAuditItem> tempWarehouseInventoryAuditItem = WarehouseInventoryAuditItemsList.Where(a => a.IdArticle == InventoryAuditArticleitem.IdArticle).ToList();
                                foreach (InventoryAuditLocation InventoryAuditLocationitem in InventoryAuditArticleitem.InventoryAuditLocation)
                                {
                                    WarehouseInventoryAuditItem temp = tempWarehouseInventoryAuditItem.Where(w => w.IdWarehouseLocation == InventoryAuditLocationitem.IdWarehouseLocation).FirstOrDefault();
                                    if (temp != null)
                                    {
                                        CountAudited = CountAudited + 1;
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    double Audited = (double)Math.Round((double)(100 * CountAudited) / AuditedArticleList.Count());
                    if (Double.IsNaN(Audited))
                    {
                        Audited = 0;
                    }
                    InventoryAuditReportPrint.xrLblAudited.Text = InventoryAuditReportPrint.xrLblAudited.Text + " " + Audited + "%";
                    #endregion
                }
                catch (Exception ex)
                {
                }
                // WarehouseInventoryAudit WarehouseInventoryAudit = new WarehouseInventoryAudit();
                SelectedInventoryAudit.PrintInventoryAuditReport = new List<PrintInventoryAuditReport>();
                SelectedInventoryAudit.PrintInventoryAuditReport.AddRange(InventoryAuditReportList);
                // InventoryAuditReportPrint.objectDataSource4.DataSource = InventoryAuditReportList;

                List<PrintInventoryAuditReport> inventoryAuditReportList = InventoryAuditReportList.OrderBy(o => o.MainWarehouseLocation).ToList();
                InventoryAuditReportPrint.objectDataSource2.DataSource = inventoryAuditReportList;
                //InventoryAuditReportPrint.Detail3.Controls.Add(xrChart1);
                //InventoryAuditReportPrint.InventoryAuditReportList = new List<PrintInventoryAuditReport>();
                //InventoryAuditReportPrint.InventoryAuditReportList.AddRange(InventoryAuditReportList);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { Owner = (EditInventoryAuditView)obj, WindowState = WindowState.Maximized, Top = 1 };
                window.PreviewControl.DocumentSource = InventoryAuditReportPrint;
                window.PreviewControl.Height = 950;
                InventoryAuditReportPrint.CreateDocument();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                window.Show();
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Print - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintInventoryAuditList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public List<PrintInventoryAuditReport> PrintInventoryAuditReport1()
        {
            List<PrintInventoryAuditReport> data = new List<PrintInventoryAuditReport>();

            if (InventoryAuditReportList != null)
            {
                data.AddRange(InventoryAuditReportList);
            }
            return data;
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
