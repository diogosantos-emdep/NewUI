using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using System.IO;
using Emdep.Geos.Data.Common.SynchronizationClass;
using Emdep.Geos.Modules.PCM.Common_Classes;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Data.Filtering;
using System.Data;
using DevExpress.Data;
using static Emdep.Geos.UI.Helper.PcmArticleColumnTemplateSelector;
using Emdep.Geos.Data.Common.PLM;
using DevExpress.Utils.Filtering;
using DevExpress.Xpf.Editors.Helpers;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class ProductTypeArticleViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service

        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPCMService PCMService = new PCMServiceController("localhost:6699");        
        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }



        #endregion // End Of Events 

        #region Declarations
        private DataTable dataTable;
        private DataTable dataTableForGridLayout;
        private ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> columns;
        DataTable dataTableForGridLayoutCopy;
        List<BasePriceListByItem> basePriceListByItem;
        List<ArticleCostPrice> articleCostPriceList;
        List<CurrencyConversion> currencyConversionList;
        bool isExpand;
        string category;
        private ObservableCollection<Articles> articleList;
        private ObservableCollection<Articles> articleList_All;
        private Articles selectedArticle;

        private ObservableCollection<PCMArticleCategory> categoryMenulist;
        private PCMArticleCategory selectedCategory;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private List<PCMArticleCategory> pcmArticleCategory;
        private ObservableCollection<PCMArticleCategory> clonedPCMArticleCategory;
        private PCMArticleCategory updatedItem;
        private bool isSave;
        private List<LookupValue> tempStatusList;
        private LookupValue selectedStatus;
        private List<LookupValue> statusList;
        private TableView view;
        private bool isArticleSave;
        private bool? isAllSave;
        private List<PCMArticleLogEntry> changeLogsEntry;
        private List<LogEntriesByArticle> wMSArticleChangeLogEntry;
        private bool confirmationYesNo { get; set; }
        private ObservableCollection<PCMArticleCategory> parentCategoryList;
        private string minMaxError;
        private string error = string.Empty;
        private IList<LookupValue> eCOSVisibilityList;
        private LookupValue selectedECOSVisibility;
        private bool isReadOnlyMinMax;
        public bool isNameChangedFromNegativeTestingScenario = false;
        private string pCMDescription_Richtext;
        private Visibility textboxnormal;
        private Visibility richtextboxrtf;
        private RichEditControl foundTextBox1;
        private string pCMDescription;
        private bool isReadOnlyName;
        public bool isSaveFromReference = false;
        private List<Articles> clonedArticleList;
        private bool isArticleColumnChooserVisible;
        private string visible;
        private bool isReadOnlyField;
        private bool allowDragDrop;
        private bool isEnabled;

        private ITokenService tokenService;
        List<GeosAppSetting> geosAppSettingList;
        ObservableCollection<PCMArticleImage> imageList;//[Sudhir.Jangra][GEOS2-2922][09/03/2023]
        string eAESPrice;
        string eAMXPrice;
        string eAROPrice;
        string eCNIPrice;
        string eBROPrice;
        string eBTRPrice;
        string eCEGPrice;
        string eCPTPrice;
        string eEINPrice;
        string eEPYPrice;
        string eIBRPrice;
        string eJMX1Price;
        string eJMX2Price;
        string eNRUPrice;
        string ePINPrice;
        string ePUSPrice;
        string ePZAPrice;
        string eSCNPrice;
        string eSHNPrice;
        string eSMA1Price;
        string eSMA2Price;
        string eSMXPrice;
        string eTCNPrice;
        string eTHQPrice;
        string eTMAPrice;
        string eTTNPrice;
        string eZTNPrice;
        #endregion

        #region Properties

        List<Company> companyList;
        public List<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        public string ArticleGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ArticleGridSetting.Xml";
        public IList<LookupValue> tempECOSVisibilityList { get; set; }
        public ObservableCollection<Articles> ArticleList
        {
            get
            {
                return articleList;
            }

            set
            {
                articleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleList"));
            }
        }
        public ObservableCollection<Articles> ArticleList_All
        {
            get
            {
                return articleList_All;
            }

            set
            {
                articleList_All = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleList_All"));
            }
        }
        public Articles SelectedArticle
        {
            get
            {
                return selectedArticle;
            }

            set
            {
                selectedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticle"));
            }
        }
        public ObservableCollection<PCMArticleCategory> CategoryMenulist
        {
            get
            {
                return categoryMenulist;
            }

            set
            {
                categoryMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryMenulist"));
            }
        }
        public PCMArticleCategory SelectedCategory
        {
            get
            {
                return selectedCategory;
            }

            set
            {
                selectedCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCategory"));
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
        public List<PCMArticleCategory> PCMArticleCategory
        {
            get { return pcmArticleCategory; }
            set
            {
                pcmArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArticleCategory"));
            }
        }
        public bool IsSave
        {
            get
            {
                return isSave;
            }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public ObservableCollection<PCMArticleCategory> ClonedPCMArticleCategory
        {
            get
            {
                return clonedPCMArticleCategory;
            }
            set
            {
                clonedPCMArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedProductType"));
            }
        }
        public PCMArticleCategory UpdatedItem
        {
            get
            {
                return updatedItem;
            }

            set
            {
                updatedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedItem"));
            }
        }
        public bool ConfirmationYesNo
        {
            get
            {
                return confirmationYesNo;
            }
            set
            {
                confirmationYesNo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConfirmationYesNo"));
            }
        }
        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        public bool IsArticleSave
        {
            get { return isArticleSave; }
            set
            {
                isArticleSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleSave"));
            }
        }
        public bool? IsAllSave
        {
            get { return isAllSave; }
            set
            {
                isAllSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllSave"));
            }
        }
        public List<PCMArticleLogEntry> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }
        public string MinMaxError
        {
            get
            {
                return minMaxError;
            }

            set
            {
                minMaxError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinMaxError"));
            }
        }
        public IList<LookupValue> ECOSVisibilityList
        {
            get
            {
                return eCOSVisibilityList;
            }
            set
            {
                eCOSVisibilityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ECOSVisibilityList"));
            }
        }
        public bool IsReadOnlyMinMax
        {
            get
            {
                return isReadOnlyMinMax;
            }

            set
            {
                isReadOnlyMinMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyMinMax"));
            }
        }
        public List<LogEntriesByArticle> WMSArticleChangeLogEntry
        {
            get
            {
                return wMSArticleChangeLogEntry;
            }

            set
            {
                wMSArticleChangeLogEntry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WMSArticleChangeLogEntry"));
            }
        }
        public bool IsReadOnlyName
        {
            get
            {
                return isReadOnlyName;
            }

            set
            {
                isReadOnlyName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyName"));
            }
        }
        public List<Articles> ClonedArticleList
        {
            get
            {
                return clonedArticleList;
            }

            set
            {
                clonedArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedArticleList"));
            }
        }
        public bool IsArticleColumnChooserVisible
        {
            get
            {
                return isArticleColumnChooserVisible;
            }

            set
            {
                isArticleColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleColumnChooserVisible"));
            }
        }
        public bool IsExpand
        {
            get
            {
                return isExpand;
            }

            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpand"));
            }
        }

        #region Plants Column Header properties
        public string EAESPrice
        {
            get
            {
                return eAESPrice;
            }

            set
            {
                eAESPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EAESPrice"));
            }
        }                  
        public string EAMXPrice
        {
            get
            {
                return eAMXPrice;
            }

            set
            {
                eAMXPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EAMXPrice"));
            }
        }     
        public string EAROPrice
        {
            get
            {
                return eAROPrice;
            }

            set
            {
                eAROPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EAROPrice"));
            }
        }            
        public string EBROPrice
        {
            get
            {
                return eBROPrice;
            }

            set
            {
                eBROPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EBROPrice"));
            }
        }           
        public string EEPYPrice
        {
            get
            {
                return eEPYPrice;
            }

            set
            {
                eEPYPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EEPYPrice"));
            }
        }
        public string EIBRPrice
        {
            get
            {
                return eIBRPrice;
            }

            set
            {
                eIBRPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EIBRPrice"));
            }
        }        
        public string EJMX1Price
        {
            get
            {
                return eJMX1Price;
            }

            set
            {
                eJMX1Price = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EJMX1Price"));
            }
        }     
        public string EJMX2Price
        {
            get
            {
                return eJMX2Price;
            }

            set
            {
                eJMX2Price = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EJMX2Price"));
            }
        }        
        public string ENRUPrice
        {
            get
            {
                return eNRUPrice;
            }

            set
            {
                eNRUPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ENRUPrice"));
            }
        }      
        public string EPINPrice
        {
            get
            {
                return ePINPrice;
            }

            set
            {
                ePINPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EPINPrice"));
            }
        }            
        public string ESCNPrice
        {
            get
            {
                return eSCNPrice;
            }

            set
            {
                eSCNPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ESCNPrice"));
            }
        }       
        public string ESHNPrice
        {
            get
            {
                return eSHNPrice;
            }

            set
            {
                eSHNPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ESHNPrice"));
            }
        }      
        public string ESMA1Price
        {
            get
            {
                return eSMA1Price;
            }

            set
            {
                eSMA1Price = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ESMA1Price"));
            }
        }        
        public string ESMA2Price
        {
            get
            {
                return eSMA2Price;
            }

            set
            {
                eSMA2Price = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ESMA2Price"));
            }
        }      
        public string ESMXPrice
        {
            get
            {
                return eSMXPrice;
            }

            set
            {
                eSMXPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ESMXPrice"));
            }
        }    
        public string ETCNPrice
        {
            get
            {
                return eTCNPrice;
            }

            set
            {
                eTCNPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETCNPrice"));
            }
        }      
        public string ETHQPrice
        {
            get
            {
                return eTHQPrice;
            }

            set
            {
                eTHQPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETHQPrice"));
            }
        }
        public string ETMAPrice
        {
            get
            {
                return eTMAPrice;
            }

            set
            {
                eTMAPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETMAPrice"));
            }
        }
        public string ETTNPrice
        {
            get
            {
                return eTTNPrice;
            }

            set
            {
                eTTNPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETTNPrice"));
            }
        }
        string c1;
        string c2;
        string c3;
        string c4;
        string c5;
        bool c1Show=false;
        bool c2Show = false;
        bool c3Show = false;
        bool c4Show = false;
        bool c5Show = false;
      
        public string C1
        {
            get
            {
                return c1;
            }

            set
            {
                c1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C1"));
            }
        }
        public string C2
        {
            get
            {
                return c2;
            }

            set
            {
                c2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C2"));
            }
        }
        public string C3
        {
            get
            {
                return c3;
            }

            set
            {
                c3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C3"));
            }
        }
        public string C4
        {
            get
            {
                return c4;
            }

            set
            {
                c4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C4"));
            }
        }
        public string C5
        {
            get
            {
                return c5;
            }

            set
            {
                c5 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C5"));
            }
        }

     
        public bool C1Show
        {
            get
            {
                return c1Show;
            }

            set
            {
                c1Show = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C1Show"));
            }
        }
        public bool C2Show
        {
            get
            {
                return c2Show;
            }

            set
            {
                c2Show = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C2Show"));
            }
        }
        public bool C3Show
        {
            get
            {
                return c3Show;
            }

            set
            {
                c3Show = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C3Show"));
            }
        }
        public bool C4Show
        {
            get
            {
                return c4Show;
            }

            set
            {
                c4Show = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C4Show"));
            }
        }
        public bool C5Show
        {
            get
            {
                return c5Show;
            }

            set
            {
                c5Show = value;
                OnPropertyChanged(new PropertyChangedEventArgs("C5Show"));
            }
        }
        #endregion

        #region [rdixit][15.07.2024][rdixit] 

        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public DataTable DataTable
        {
            get { return dataTable; }
            set
            {
                dataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTable"));
            }
        }

        public DataTable DataTableForGridLayout
        {
            get
            {
                return dataTableForGridLayout;
            }
            set
            {
                dataTableForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayout"));
            }
        }

        public DataTable DataTableForGridLayoutCopy
        {
            get
            {
                return dataTableForGridLayoutCopy;
            }
            set
            {
                dataTableForGridLayoutCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutCopy"));
            }
        }

        public List<BasePriceListByItem> BasePriceListByItem
        {
            get
            {
                return basePriceListByItem;
            }
            set
            {
                basePriceListByItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BasePriceListByItem"));
            }
        } 
        public List<CurrencyConversion> CurrencyConversionList
        {
            get
            {
                return currencyConversionList;
            }

            set
            {
                currencyConversionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyConversionList"));
            }
        }
        public List<ArticleCostPrice> ArticleCostPriceList
        {
            get
            {
                return articleCostPriceList;
            }

            set
            {
                articleCostPriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleCostPriceList"));
            }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        #endregion

        #endregion

        #region ICommands
        public ICommand SelectCategoryCommand { get; set; }
        public ICommand ImportWarehouseItemsToPCMCommand { get; set; }
        public ICommand AddNewCategoryCommand { get; set; }
        public ICommand EditCategoryCommand { get; set; }
        public ICommand RefreshArticleViewCommand { get; set; }
        public ICommand EditArticleCommand { get; set; }
        public ICommand PrintArticleCommand { get; set; }
        public ICommand ExportArticleCommand { get; set; }
        public ICommand CommandOnDragRecordOverArticleCatagoriesGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropArticleCatagories { get; set; }
        public ICommand CommandTreeListViewDropRecordArticleCatagories { get; set; }
        public ICommand UpdateMultipleRowsArticleGridCommand { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand PageLoadedCommand { get; set; }
        public ICommand ArticleGridControlLoadedCommand { get; set; }
        public ICommand ArticleItemListTableViewLoadedCommand { get; set; }
        public ICommand ArticleGridControlUnloadedCommand { get; set; }
        public ICommand PasteInSearchControlCommand { get; set; }
        public ICommand ExpandAndCollapsCategoriesCommand { get; set; }
        //public ICommand CollapseCategoriesCommand { get; set; }
        public ICommand OpenArticleImageCommand { get; set; } //[sshegaonkar][GEOS2-2922][14.02.2023]
        #endregion

        #region Constructor
        public ProductTypeArticleViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ProductTypeArticleViewModel ...", category: Category.Info, priority: Priority.Low);

                AddNewCategoryCommand = new DelegateCommand<object>(AddNewCategory);
                ExpandAndCollapsCategoriesCommand = new DelegateCommand<object>(ExpandAndCollapsCategoriesCommandAction);//[rdixit][24.11.2022][GEOS2-2718]
                //CollapseCategoriesCommand = new DelegateCommand<object>(CollapseCategoriesCommandAction);//[rdixit][24.11.2022][GEOS2-2718]
                EditCategoryCommand = new DelegateCommand<object>(EditCategory);
                RefreshArticleViewCommand = new RelayCommand(new Action<object>(RefreshArticleView));
                PasteInSearchControlCommand = new DelegateCommand<object>(PasteInSearchControlCommandAction);//[rdixit][29.07.2022][GEOS2-3373]
                EditArticleCommand = new RelayCommand(new Action<object>(EditArticleAction));
                PrintArticleCommand = new RelayCommand(new Action<object>(PrintArticle));
                ExportArticleCommand = new RelayCommand(new Action<object>(ExportArticle));
                CommandOnDragRecordOverArticleCatagoriesGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticleCatagoriesGrid);
                CommandCompleteRecordDragDropArticleCatagories = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropArticleCatagories);
                CommandTreeListViewDropRecordArticleCatagories = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordArticleCategories);
                UpdateMultipleRowsArticleGridCommand = new DelegateCommand<object>(UpdateMultipleRowsArticleGridCommandAction);
                CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                DeleteCategoryCommand = new DelegateCommand<object>(DeleteCategoryAction);
                PageLoadedCommand = new RelayCommand(new Action<object>(OnViewLoaded));

                ArticleGridControlLoadedCommand = new DelegateCommand<object>(ArticleGridControlLoadedAction);
                ArticleItemListTableViewLoadedCommand = new DelegateCommand<object>(ArticleItemListTableViewLoadedAction);
                ArticleGridControlUnloadedCommand = new DelegateCommand<object>(ArticleGridControlUnloadedCommandAction);
                OpenArticleImageCommand = new DelegateCommand<object>(OpenArticleImageCommandAction);//[sshegaonkar][GEOS2-2922][14.02.2023]
                if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                {
                    IsReadOnlyName = false;
                }
                else
                {
                    IsReadOnlyName = true;
                }
                IsExpand = true;
                FillArticleCostPriceList();
                FillBasePriceList();
                FillCurrencyConversionList_All();
                //[GEOS2-6522][rdixit][29.11.2024]

                GeosApplication.Instance.ArticleCostPriceList = new ObservableCollection<ArticleCostPrice>();
                GeosApplication.Instance.BasePriceListByItem = new List<BasePriceListByItem>();

                GeosApplication.Instance.Logger.Log("Constructor ProductTypeArticleViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor ProductTypeArticleViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method

        private void AddColumnsToDataTable()//[rdixit][15.07.2024][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);

                var initialColumns = new[]
             {
                    new ColumnItem { ColumnFieldName = "Reference", HeaderText = GetResource("ArticleGridColReference"), Width = 180, ArticleTypeSetting = ArticleTypeSettingsType.Reference, Visible = true },
                    new ColumnItem { ColumnFieldName = "Description", HeaderText = GetResource("WarehousesName"), Width = 300, ArticleTypeSetting = ArticleTypeSettingsType.Description, Visible = true },
                    new ColumnItem { ColumnFieldName = "IsRichText", HeaderText = GetResource("GridColDescription"), Width = 120, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = true },
                    new ColumnItem { ColumnFieldName = "PcmArticleCategory", HeaderText = GetResource("ArticleGridColCategory"), Width = 280, ArticleTypeSetting = ArticleTypeSettingsType.ArticleCategory, Visible = true },
                    new ColumnItem { ColumnFieldName = "SupplierName", HeaderText = GetResource("ArticleGridColSupplier"), Width = 320, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = true },
                    new ColumnItem { ColumnFieldName = "ArticleImageCount", HeaderText = GetResource("ArticleGridColImage"), Width = 100, ArticleTypeSetting = ArticleTypeSettingsType.Image, Visible = true },
                    new ColumnItem { ColumnFieldName = "Weight", HeaderText = GetResource("ArticleGridColWeight"), Width = 110, ArticleTypeSetting = ArticleTypeSettingsType.Weight, Visible = true },
                    new ColumnItem { ColumnFieldName = "Length", HeaderText = GetResource("ArticleGridColLength"), Width = 110, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = true },
                    new ColumnItem { ColumnFieldName = "Width", HeaderText = GetResource("ArticleGridColWidth"), Width = 110, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = true },
                    new ColumnItem { ColumnFieldName = "Height", HeaderText = GetResource("ArticleGridColHeight"), Width = 110, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = true },
                    new ColumnItem { ColumnFieldName = "ECOSVisibilityValue", HeaderText = GetResource("ArticleGridECOSVisibility"), Width = 130, ArticleTypeSetting = ArticleTypeSettingsType.ECOSVisibility, Visible = true },
                    new ColumnItem { ColumnFieldName = "PurchaseQtyMin", HeaderText = GetResource("ArticleGridColPQtyMin"), Width = 110, ArticleTypeSetting = ArticleTypeSettingsType.PQty, Visible = true },
                    new ColumnItem { ColumnFieldName = "PurchaseQtyMax", HeaderText = GetResource("ArticleGridColPQtyMax"), Width = 110, ArticleTypeSetting = ArticleTypeSettingsType.PQty, Visible = true },
                    new ColumnItem { ColumnFieldName = "PCMStatus", HeaderText = GetResource("ArticleGridColStatus"), Width = 130, ArticleTypeSetting = ArticleTypeSettingsType.Status, Visible = true },
                    new ColumnItem { ColumnFieldName = "WarehouseStatus", HeaderText = GetResource("ArticleGridColWarehouseStatus"), Width = 180, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = true },
                    new ColumnItem { ColumnFieldName = "WarehouseCreationDate", HeaderText = GetResource("ArticleGridColWarehouseCreationDate"), Width = 180, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = false },
                    new ColumnItem { ColumnFieldName = "PCMCreationDate", HeaderText = GetResource("ArticleGridColPCMCreationDate"), Width = 180, ArticleTypeSetting = ArticleTypeSettingsType.Default, Visible = false },
                };

                // Add initial and hidden columns
                Columns = new ObservableCollection<ColumnItem>(initialColumns.Concat(GetHiddenColumns()));
                DataTableForGridLayout = CreateDataTable();
                foreach (var item in GeosApplication.Instance.ArticleCostPlantPriceList.GroupBy(i => i.Item2))
                {
                    try
                    {
                        ColumnItem col = new ColumnItem()
                        {
                            ColumnFieldName = item.Key.ToString(),
                            HeaderText = "Cost " + item.Key + " (" + GeosApplication.Instance.PCMCurrentCurrency.Symbol + ") ",
                            Width = 180,
                            IsVertical = false,
                            ArticleTypeSetting = ArticleTypeSettingsType.CostPrice,
                            Visible = false
                        };
                        Columns.Add(col);

                        DataTableForGridLayout.Columns.Add(item.Key.ToString(), typeof(double));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }

                foreach (var item in GeosApplication.Instance.BasePriceNameList.GroupBy(i => i.Item1))
                {
                    foreach (var item1 in GeosApplication.Instance.Currencies)
                    {
                        try
                        {
                            ColumnItem col = new ColumnItem()
                            {
                                ColumnFieldName = "BPL_" + item.Key +"_"+ item1.IdCurrency,
                                HeaderText = "Sale " + item.FirstOrDefault().Item2 + " (" + item1.Name + ") ",
                                Width = 180,
                                IsVertical = false,
                                ArticleTypeSetting = ArticleTypeSettingsType.BasePrice,
                                Visible = false
                            };
                            Columns.Add(col);
                            DataTableForGridLayout.Columns.Add("BPL_" + item.Key + "_" + item1.IdCurrency, typeof(double));
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                Columns = new ObservableCollection<ColumnItem>(Columns);
                TotalSummary = new ObservableCollection<Summary>()
                { new Summary() { Type = SummaryItemType.Count, FieldName = "Reference", DisplayFormat = "Total : {0}" } };

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        private static string GetResource(string resourceKey)
        {
            return Convert.ToString(Application.Current.FindResource(resourceKey));
        }

        private static ObservableCollection<ColumnItem> GetHiddenColumns()
        {
            return new ObservableCollection<ColumnItem>   
           {     
                new ColumnItem() { ColumnFieldName = "IsImageVisible", HeaderText = "IsImageVisible", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                    
                new ColumnItem() { ColumnFieldName = "IsImageButtonEnabled", HeaderText = "IsImageButtonEnabled", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                    
                new ColumnItem() { ColumnFieldName = "ECOSVisibilityHTMLColor", HeaderText = "ECOSVisibilityHTMLColor", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                    
                new ColumnItem() { ColumnFieldName = "IdECOSVisibility", HeaderText = "IdECOSVisibility", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                    
                new ColumnItem() { ColumnFieldName = "StatusHTMLColor", HeaderText = "StatusHTMLColor", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                    
                new ColumnItem() { ColumnFieldName = "CategoryMenulist", HeaderText = "CategoryMenulist", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                                   
                new ColumnItem() { ColumnFieldName = "ECOSVisibilityList", HeaderText = "ECOSVisibilityList", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                   
                new ColumnItem() { ColumnFieldName = "StatusList", HeaderText = "StatusList", Width = 0, IsVertical = false, ArticleTypeSetting = ArticleTypeSettingsType.Hidden, Visible = false },                   
                new ColumnItem() { ColumnFieldName = "IsChecked", HeaderText="IsValueChanged", ArticleTypeSetting = ArticleTypeSettingsType.Hidden,Width=0,Visible=false,IsVertical= false },                   
                new ColumnItem() { ColumnFieldName = "IdArticle", HeaderText="IdArticle", ArticleTypeSetting = ArticleTypeSettingsType.Hidden,Width=0,Visible=false,IsVertical= false }    
            };
        }

        private static DataTable CreateDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("IdArticle", typeof(ulong));
            dataTable.Columns.Add("Reference", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("IsRichText", typeof(string));
            dataTable.Columns.Add("PcmArticleCategory", typeof(PCMArticleCategory));
            dataTable.Columns.Add("SupplierName", typeof(string));
            dataTable.Columns.Add("ArticleImageCount", typeof(string));
            dataTable.Columns.Add("Weight", typeof(decimal));
            dataTable.Columns.Add("Length", typeof(float));
            dataTable.Columns.Add("Width", typeof(float));
            dataTable.Columns.Add("Height", typeof(float));
            dataTable.Columns.Add("ECOSVisibilityValue", typeof(string));
            dataTable.Columns.Add("PurchaseQtyMin", typeof(float));
            dataTable.Columns.Add("PurchaseQtyMax", typeof(float));
            dataTable.Columns.Add("PCMStatus", typeof(string));
            dataTable.Columns.Add("WarehouseStatus", typeof(string));
            dataTable.Columns.Add("WarehouseCreationDate", typeof(DateTime));
            dataTable.Columns.Add("PCMCreationDate", typeof(DateTime));
            dataTable.Columns.Add("IsImageVisible", typeof(string));
            dataTable.Columns.Add("IsImageButtonEnabled", typeof(string));
            dataTable.Columns.Add("ECOSVisibilityHTMLColor", typeof(string));
            dataTable.Columns.Add("IdECOSVisibility", typeof(int));
            dataTable.Columns.Add("StatusHTMLColor", typeof(string));
            dataTable.Columns.Add("CategoryMenulist", typeof(ObservableCollection<PCMArticleCategory>));
            dataTable.Columns.Add("StatusList", typeof(List<LookupValue>));
            dataTable.Columns.Add("ECOSVisibilityList", typeof(List<LookupValue>));
            dataTable.Columns.Add("IsChecked", typeof(bool));           
            return dataTable;
        }

        private void FillArticleCostPriceList()//[rdixit][15.07.2024][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillArticleCostPriceList..."), category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.ArticleCostPlantPriceList == null || GeosApplication.Instance.ArticleCostPlantPriceList?.Count == 0)
                    GeosApplication.Instance.ArticleCostPlantPriceList = new List<Tuple<ulong, string>>(PCMService.GetArticleCostPricesPlantByCurrency_V2590());

                GeosApplication.Instance.Logger.Log(string.Format("Method FillArticleCostPriceList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillArticleCostPriceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillArticleCostPriceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleCostPriceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCurrencyConversionList_All()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyConversionList_All ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.CurrencyConversionList == null || GeosApplication.Instance.CurrencyConversionList?.Count == 0)
                {
                    GeosApplication.Instance.CurrencyConversionList = new List<CurrencyConversion>(PCMService.GetCurrencyConversionsByLatestDate_V2590(
                        string.Join(",", GeosApplication.Instance.BasePriceNameList.Select(i=>i.Item3)?.ToList()),
                        GeosApplication.Instance.PCMCurrentCurrency.IdCurrency));
                }
                GeosApplication.Instance.Logger.Log("Method FillCurrencyConversionList_All() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyConversionList_All() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyConversionList_All() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyConversionList_All() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillBasePriceList()//[rdixit][15.07.2024][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillBasePriceList..."), category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.BasePriceNameList == null || GeosApplication.Instance.BasePriceNameList?.Count == 0)
                    GeosApplication.Instance.BasePriceNameList = PCMService.GetSalesPriceNameList_V2590();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillBasePriceList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillBasePriceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillBasePriceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillBasePriceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDataTable()//[rdixit][15.07.2024][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);

                DataTableForGridLayout.Rows.Clear();
                DataTableForGridLayoutCopy = DataTableForGridLayout.Copy();              
                foreach (Articles item in ArticleList)
                {
                    DataRow dr = DataTableForGridLayoutCopy.NewRow();
                    dr["IdArticle"] = item.IdArticle;
                    dr["Reference"] = item.Reference;
                    dr["Description"] = item.Description;
                    dr["IsRichText"] = item.IsRichText;
                    dr["StatusList"] = item.StatusList;
                    dr["ECOSVisibilityList"] = item.ECOSVisibilityList;
                    dr["PcmArticleCategory"] = item.PcmArticleCategory;
                    dr["PurchaseQtyMax"] = item.PurchaseQtyMax;
                    dr["PurchaseQtyMin"] = item.PurchaseQtyMin;
                    dr["IsChecked"] = false;
                    if (item.ArticleImageCount == null)
                    {
                        dr["IsImageVisible"] = "Hidden";
                        dr["IsImageButtonEnabled"] = "false";
                        dr["ArticleImageCount"] = null;
                    }
                    else
                    {
                        dr["IsImageVisible"] = "Visible";
                        dr["IsImageButtonEnabled"] = "true";
                        dr["ArticleImageCount"] = item.ArticleImageCount;
                    }
                    dr["SupplierName"] = item.SupplierName;
                    dr["Weight"] = item.Weight;
                    dr["Length"] = item.Length;
                    dr["Width"] = item.Width;
                    dr["Height"] = item.Height;
                    dr["ECOSVisibilityValue"] = item.ECOSVisibilityValue;
                    dr["ECOSVisibilityHTMLColor"] = item.ECOSVisibilityHTMLColor;
                    dr["PurchaseQtyMin"] = item.PurchaseQtyMin;
                    dr["PurchaseQtyMax"] = item.PurchaseQtyMax;
                    dr["PCMStatus"] = item.PCMStatus;
                    dr["WarehouseStatus"] = item.WarehouseStatus.ToString();
                    dr["WarehouseCreationDate"] = item.WarehouseCreationDate;
                    dr["PCMCreationDate"] = item.PCMCreationDate;
                    dr["IdECOSVisibility"] = item.IdECOSVisibility;
                    dr["StatusHTMLColor"] = item.StatusHTMLColor;             
                    dr["CategoryMenulist"] = new ObservableCollection<PCMArticleCategory>(item.CategoryMenulist);                  
                    DataTableForGridLayoutCopy.Rows.Add(dr);
                }

                DataTableForGridLayout = DataTableForGridLayoutCopy;

                GeosApplication.Instance.Logger.Log("Method FillDataTableWithoutBands() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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
                //FillCurrencyConversionList_All();
                CompanyList = new List<Company>(PCMService.GetEmdepSites());
                SelectCategoryCommand = new DelegateCommand<object>(RetrieveArticlesByCategory);
                ImportWarehouseItemsToPCMCommand = new DelegateCommand<object>(ImportWarehouseItemsToPCM);
                //FillArticleCostPriceList(); 
                FillECOSVisibilityList();
                FillStatusList();
                FillArticleList();
                FillArticleCatagoryList();
             
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteCategoryAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCategoryAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteArticleCategoryMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (SelectedCategory.Article_count == 0)
                    {
                        List<PCMArticleCategory> tempList = new List<Data.Common.PCM.PCMArticleCategory>();
                        List<string> temp = getIdsForRetriveArticlesByParentClick();
                        tempList.Add(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));
                        CategoryMenulist.Remove(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));

                        foreach (string id in temp)
                        {
                            tempList.Add(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == Convert.ToUInt32(id)));
                            CategoryMenulist.Remove(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == Convert.ToUInt32(id)));
                        }
                        bool isDelete = PCMService.IsDeletePCMArticleCategory(tempList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CategoryDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("Can not delete selected category."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }


                GeosApplication.Instance.Logger.Log("Method DeleteCategoryAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCategoryAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCategoryAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DeleteCategoryAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddNewCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()..."), category: Category.Info, priority: Priority.Low);

                AddEditCategoryView addEditCategoryView = new AddEditCategoryView();
                AddEditCategoryViewModel addEditCategoryViewModel = new AddEditCategoryViewModel();
                EventHandler handle = delegate { addEditCategoryView.Close(); };
                addEditCategoryViewModel.RequestClose += handle;
                addEditCategoryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCategoryHeader").ToString();
                addEditCategoryViewModel.IsNew = true;
                addEditCategoryViewModel.ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/ImageEditLogo.png"));
                addEditCategoryViewModel.IsReferenceImageExist = false;
                bool status = IsArticleColumnChooserVisible;
                IsArticleColumnChooserVisible = false;
                addEditCategoryViewModel.Init();
                addEditCategoryView.DataContext = addEditCategoryViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditCategoryView.Owner = Window.GetWindow(ownerInfo);
                addEditCategoryView.ShowDialog();

                if (addEditCategoryViewModel.IsSave)
                {

                    addEditCategoryViewModel.OrderCategoryList.Where(a => a.IdPCMArticleCategory == 0).ToList().ForEach(a => { a.IdPCMArticleCategory = addEditCategoryViewModel.NewArticleCategory.IdPCMArticleCategory; });

                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(addEditCategoryViewModel.OrderCategoryList.Where(a => a.Name != "---"));
                    foreach (PCMArticleCategory category in CategoryMenulist)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
                    {
                        if (category.InUse == "NO")
                            category.IspcmCategoryNOTInUse = true;
                    }
                    PCMArticleCategory articleCategories = new PCMArticleCategory();
                    articleCategories.Name = "All";
                    articleCategories.KeyName = "Group_All";
                    articleCategories.Article_count = ArticleList_All.Count();
                    articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";

                    CategoryMenulist.Insert(0, articleCategories);

                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));

                    SelectedCategory = CategoryMenulist.Where(a => a.IdPCMArticleCategory == addEditCategoryViewModel.NewArticleCategory.IdPCMArticleCategory).FirstOrDefault();

                    if (SelectedCategory != null)
                    {
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(a => a.PcmArticleCategory.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));
                    }
                    else
                    {
                        SelectedCategory = CategoryMenulist.FirstOrDefault();
                    }

                    FillArticleCatagoryList();
                }
                IsArticleColumnChooserVisible = status;

                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()..."), category: Category.Info, priority: Priority.Low);
                if (SelectedCategory.Name == "All" && SelectedCategory.KeyName == "Group_All")
                {
                    return;
                }
                AddEditCategoryView addEditCategoryView = new AddEditCategoryView();
                AddEditCategoryViewModel addEditCategoryViewModel = new AddEditCategoryViewModel();
                EventHandler handle = delegate { addEditCategoryView.Close(); };
                addEditCategoryViewModel.RequestClose += handle;
                addEditCategoryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCategoryHeader").ToString();
                addEditCategoryViewModel.IsNew = false;
                bool status = IsArticleColumnChooserVisible;
                IsArticleColumnChooserVisible = false;
                addEditCategoryViewModel.EditInitCategory(SelectedCategory);
                addEditCategoryView.DataContext = addEditCategoryViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditCategoryView.Owner = Window.GetWindow(ownerInfo);
                addEditCategoryView.ShowDialog();
                PCMArticleCategory selected_Cat = SelectedCategory;
                if (addEditCategoryViewModel.IsSave)
                {
                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2290());
                    foreach (PCMArticleCategory category in CategoryMenulist)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
                    {
                        if (category.InUse == "NO")
                            category.IspcmCategoryNOTInUse = true;
                    }
                    UpdatePCMCategoryCount();
                    PCMArticleCategory articleCategories = new PCMArticleCategory();
                    articleCategories.Name = "All";
                    articleCategories.KeyName = "Group_All";
                    articleCategories.Article_count = ArticleList_All.Count();
                    articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                    CategoryMenulist.Insert(0, articleCategories);
                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                    SelectedCategory = selected_Cat;

                    ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;


                    if (SelectedCategory == null || SelectedCategory.Name == "All")
                    {
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                    }
                    else
                    {
                        string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                        string[] ids = (Concat_ChildArticles + "," + SelectedCategory.IdPCMArticleCategory).Split(',');
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                    }
                }
                IsArticleColumnChooserVisible = status;

                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RefreshArticleView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshArticleView()...", category: Category.Info, priority: Priority.Low);

                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
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

                GridControl gridControl = (GridControl)obj;
                ArticleGridControlUnloadedCommandAction(gridControl);
                TableView detailView = (TableView)gridControl.View;
                FillArticleList();
                detailView.DataControl.RefreshData();
                detailView.DataControl.UpdateLayout();
                ArticleGridControlLoadedAction(gridControl);
                ArticleItemListTableViewLoadedAction(detailView);
                
                detailView.SearchString = null;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshArticleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshArticleView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshCatelogueView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshCatelogueView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditArticleAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()..."), category: Category.Info, priority: Priority.Low);
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
                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        isSaveFromReference = true;
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }


                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;           
                System.Data.DataRowView row = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                SelectedArticle = ArticleList.FirstOrDefault(i => i.Reference == row.Row["Reference"].ToString());
                Articles SelectedRow = SelectedArticle;
                EditPCMArticleView editPCMArticleView = new EditPCMArticleView();
                EditPCMArticleViewModel editPCMArticleViewModel = new EditPCMArticleViewModel();
                EventHandler handle = delegate { editPCMArticleView.Close(); };
                editPCMArticleViewModel.RequestClose += handle;
                editPCMArticleViewModel.IsNew = false;
                bool status = IsArticleColumnChooserVisible;
                IsArticleColumnChooserVisible = false;           
                editPCMArticleViewModel.EditInit(SelectedArticle);
                editPCMArticleView.DataContext = editPCMArticleViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);

                editPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                editPCMArticleViewModel.IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                editPCMArticleView.ShowDialog();
                PCMArticleCategory selected_Cat = SelectedCategory;

                if (editPCMArticleViewModel.IsSave)
                {
                    //if (SelectedRow.IdECOSVisibility == editPCMArticleViewModel.UpdatedArticle.IdECOSVisibility)
                    {
                        ArticleGridImageViewModel.StaticImagesList = editPCMArticleViewModel.ImagesList.ToList();
                        SelectedRow.IdPCMArticleCategory = editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory;
                        SelectedRow.IdPCMStatus = editPCMArticleViewModel.UpdatedArticle.IdPCMStatus;
                        SelectedRow.Reference = editPCMArticleViewModel.UpdatedArticle.Reference;
                        SelectedRow.PCMStatus = editPCMArticleViewModel.SelectedStatus.Value;
                        SelectedRow.PurchaseQtyMin = editPCMArticleViewModel.UpdatedArticle.PurchaseQtyMin;
                        SelectedRow.PurchaseQtyMax = editPCMArticleViewModel.UpdatedArticle.PurchaseQtyMax;
                        SelectedRow.IdECOSVisibility = editPCMArticleViewModel.UpdatedArticle.IdECOSVisibility;
                        SelectedRow.ECOSVisibilityValue = editPCMArticleViewModel.UpdatedArticle.ECOSVisibilityValue;
                        SelectedRow.ECOSVisibilityHTMLColor = editPCMArticleViewModel.UpdatedArticle.ECOSVisibilityHTMLColor;
                        SelectedRow.Description = editPCMArticleViewModel.UpdatedArticle.WarehouseArticle?.Description;
                        SelectedRow.Description = SelectedRow.Description.Trim();
                        SelectedRow.PCMDescription = editPCMArticleViewModel.UpdatedArticle.PCMDescription;                       
                        if (string.IsNullOrEmpty(editPCMArticleViewModel.UpdatedArticle.PCMDescription))
                        {
                            SelectedRow.IsRichText = "";
                            SelectedRow.PCMDescription = null;
                        }
                        else
                            SelectedRow.IsRichText = "Yes";

                        RichEditDocumentServer reds = new RichEditDocumentServer();
                        if (SelectedRow.PCMDescription != null)
                        {
                            reds.HtmlText = SelectedRow.PCMDescription.ToString();
                            SelectedRow.PCMDescription = reds.RtfText;
                        }

                        foreach (Articles article in ArticleList_All.Where(a => a.IdArticle == SelectedRow.IdArticle).ToList())
                        {
                            article.IdPCMArticleCategory = editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory;
                            article.PcmArticleCategory = new PCMArticleCategory();
                            article.PcmArticleCategory.IdPCMArticleCategory = editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory;
                            article.PcmArticleCategory.Name = editPCMArticleViewModel.SelectedCategory.Name;
                            article.ArticleCategory = new ArticleCategories();
                            article.ArticleCategory.Name = editPCMArticleViewModel.SelectedCategory.Name;

                            article.PcmArticleCategory = article.CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedRow.IdPCMArticleCategory);
                        }
                    }


                    if (editPCMArticleViewModel.ClonedArticle.IdPCMArticleCategory != editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory)
                    {
                        CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2290());
                        //foreach (PCMArticleCategory category in CategoryMenulist)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
                        //{
                        //    if (category.InUse == "NO")
                        //        category.IspcmCategoryNOTInUse = true;
                        //}
                        UpdatePCMCategoryCount();
                        PCMArticleCategory articleCategories = new PCMArticleCategory();
                        articleCategories.Name = "All";
                        articleCategories.KeyName = "Group_All";
                        articleCategories.Article_count = ArticleList_All.Count();
                        articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                        CategoryMenulist.Insert(0, articleCategories);
                        CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                        SelectedCategory = selected_Cat;

                        ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;

                        if (SelectedCategory == null || SelectedCategory.Name == "All")
                        {
                            ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                        }
                        else
                        {
                            string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                            string[] ids = (Concat_ChildArticles + "," + SelectedCategory.IdPCMArticleCategory).Split(',');
                            ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                        }
                    }

                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(detailView, false);
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                    gridControl.RefreshData();
                    gridControl.UpdateLayout();

                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsArticleColumnChooserVisible = status;

                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RetrieveArticlesByCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveArticlesByCategory()...", category: Category.Info, priority: Priority.Low);
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
                if (SelectedCategory == null || SelectedCategory.Name == "All")
                {
                    ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                }
                else
                {
                    string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                    string[] ids = (Concat_ChildArticles + "," + SelectedCategory.IdPCMArticleCategory).Split(',');
                    ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                }
                DataTableForGridLayout = DataTableForGridLayoutCopy.AsEnumerable().Where(row => ArticleList
                .Any(article => article.Reference == row["Reference"].ToString())).CopyToDataTable();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveArticlesByCategory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveArticlesByCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList()...", category: Category.Info, priority: Priority.Low);
                #region
                //ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2110());
                //ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2290());//[rdixit][GEOS2-GEOS2-2571][06.07.2022]
                //service GetAllPCMArticles_V2380 updated with GetAllPCMArticles_V2380 by  //[rdixit][GEOS2-2922][24.04.2023]
                //ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2380());[Sudhir.Jangra][GEOS2-4809]           
                //service updated from GetAllPCMArticles_V2440 to GetAllPCMArticles_V2460 //[rdixit][GEOS2-4897][01.12.2023]
                //service updated from GetAllPCMArticles_V2460 to GetAllPCMArticles_V2540 [rdixit][15.07.2024][rdixit]
                #endregion

                ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2540());
                //ColumnName();
                SelectedArticle = new Articles();
                SelectedArticle = ArticleList.FirstOrDefault();
                foreach (Articles item in ArticleList)
                {
                    foreach (PCMArticleCategory category in item.CategoryMenulist)
                    {
                        if (category.InUse == "NO")
                            category.IspcmCategoryNOTInUse = true;
                    }
                    item.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).Value;
                    item.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).HtmlColor;
                    item.ECOSVisibilityList = ECOSVisibilityList.ToList();
                    item.PCMStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdPCMStatus).Value;
                    item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdPCMStatus).HtmlColor;
                    item.StatusList = StatusList.ToList();

                    if (item.IsRichText == "No")
                    {
                        item.IsRichText = "";
                        item.PCMDescription = null;
                    }

                    RichEditDocumentServer reds = new RichEditDocumentServer();
                    if (item.PCMDescription != null && item.IsRtfText == true)
                    {
                        reds.HtmlText = item.PCMDescription.ToString();
                        item.PCMDescription = reds.RtfText;
                    }
                }
                ArticleList_All = new ObservableCollection<Articles>(ArticleList);
                ClonedArticleList = ArticleList.Select(x => (Articles)x.Clone()).ToList();
                AddColumnsToDataTable();
                FillDataTable();
                GeosApplication.Instance.Logger.Log("Method FillArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillArticleCatagoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()...", category: Category.Info, priority: Priority.Low);

                CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2290());
                foreach (PCMArticleCategory category in CategoryMenulist)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
                {
                    if (category.InUse == "NO")
                        category.IspcmCategoryNOTInUse = true;
                }
                UpdatePCMCategoryCount();
                PCMCommon.Instance.CategoryList = CategoryMenulist;
                PCMArticleCategory articleCategories = new PCMArticleCategory();
                articleCategories.Name = "All";
                articleCategories.KeyName = "Group_All";
                articleCategories.Article_count = ArticleList_All.Count();
                articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                CategoryMenulist.Insert(0, articleCategories);
                CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                SelectedCategory = CategoryMenulist.FirstOrDefault();
                ArticleList = new ObservableCollection<Articles>(ArticleList_All);

                SelectedArticle = ArticleList.FirstOrDefault();
                ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleCatagoryList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleCatagoryList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleCatagoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ImportWarehouseItemsToPCM(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ImportWarehouseItemsToPCM()...", category: Category.Info, priority: Priority.Low);

                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                ArticleGridControlUnloadedCommandAction(gridControl);                       
                ImportWarehouseItemToPCMView importWarehouseItemToPCMView = new ImportWarehouseItemToPCMView();
                ImportWarehouseItemToPCMModel importWarehouseItemToPCMModel = new ImportWarehouseItemToPCMModel();
                EventHandler handle = delegate { importWarehouseItemToPCMView.Close(); };
                importWarehouseItemToPCMModel.RequestClose += handle;
                bool status = IsArticleColumnChooserVisible;
                IsArticleColumnChooserVisible = false;
                importWarehouseItemToPCMModel.Init();
                importWarehouseItemToPCMView.DataContext = importWarehouseItemToPCMModel;
                var ownerInfo = (detailView as FrameworkElement);
                importWarehouseItemToPCMView.Owner = Window.GetWindow(ownerInfo);
                importWarehouseItemToPCMView.ShowDialog();

                if (importWarehouseItemToPCMModel.IsSaveChanges)
                {
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    FillArticleList();
                    gridControl.RefreshData();
                    gridControl.UpdateLayout();
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                }
                FillArticleCatagoryList();
                ArticleGridControlLoadedAction(gridControl);
                ArticleItemListTableViewLoadedAction(detailView);
                IsArticleColumnChooserVisible = status;
                GeosApplication.Instance.Logger.Log("Method ImportWarehouseItemsToPCM()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ImportWarehouseItemsToPCM()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintArticle(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }

                    else
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        FillArticleList();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                    }

                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                    FillArticleList();
                }

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                IsBusy = true;

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ArticlesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ArticlesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportArticle(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportModules()...", category: Category.Info, priority: Priority.Low);

                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    else
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        FillArticleList();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                    FillArticleList();
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                }

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Articles List";
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
                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportModules()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportModules()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        /// 
        private void OnDragRecordOverArticleCatagoriesGrid(DragRecordOverEventArgs e)
        {
            try
            {

                //[001]
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()...", category: Category.Info, priority: Priority.Low);

                if (e.DropPosition == DropPosition.Inside)
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverDetectionsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        /// 
        private void CompleteRecordDragDropArticleCatagories(CompleteRecordDragDropEventArgs e)
        {
            try
            {

                //[001]
                if (ConfirmationYesNo == true)
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        /// 
        private void TreeListViewDropRecordArticleCategories(DropRecordEventArgs e)
        {
            try
            {

                //[001]
                ProductTypeArticleView ptav = new ProductTypeArticleView();
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["SaveArticleCatagory"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    ConfirmationYesNo = true;

                    uint tmpIdPCMArticleCategoryIndexOf;
                    bool isParentChange = false;
                    List<PCMArticleCategory> PCMArticleCategory_ParentChange = new List<PCMArticleCategory>();
                    PCMArticleCategory = new List<PCMArticleCategory>();


                    var data1 = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PCMArticleCategory> newRecords1 = data1.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName, Position = x.Position }).ToList();

                    PCMArticleCategory SelectedOrderCategory = (PCMArticleCategory)e.TargetRecord;
                    PCMArticleCategory PcmArticleCategory = new PCMArticleCategory();

                    List<PCMArticleCategory> lstUpdateItem = new List<PCMArticleCategory>();
                    lstUpdateItem = newRecords1.ToList();

                    UpdatedItem = newRecords1.FirstOrDefault();

                    uint pos = 1;
                    uint status = 0;

                    #region OneParentoAnaterParent
                    if (UpdatedItem.Parent != null)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();

                        if (ClonedPCMArticleCategory.Count > 0)
                        {
                            if (e.DropPosition == DropPosition.Inside)
                            {
                                ulong? UpdatedItemParent = UpdatedItem.Parent;
                                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
                                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_Selectorder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).ToList();
                                List<uint> indexCollection = new List<uint>();
                                pos = 1;
                                foreach (var updateArt in lstUpdateItem)
                                {
                                    indexCollection.Add(updateArt.IdPCMArticleCategory);

                                }
                                pos = 1;
                                foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
                                {
                                    if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
                                    {
                                        CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                    }
                                }

                                pos = 1;
                                foreach (var updateArt in lstUpdateItem)
                                {
                                    CategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = SelectedOrderCategory.KeyName; });
                                    PCMArticleCategory.Add(CategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                }

                                foreach (var updateArt in pcmArticleCategory_ForSetOrder_Selectorder)
                                {

                                    PCMArticleCategory.Add(CategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                }
                                if (PCMArticleCategory.Count > 0)
                                {
                                    pos = 1;
                                    foreach (PCMArticleCategory updateArt in PCMArticleCategory)
                                    {
                                        updateArt.Position = pos++;
                                    }
                                }


                                if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
                                {
                                    PCMArticleCategory_ParentChange = CategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
                                }



                            }
                            else
                            {


                                if (e.DropPosition != DropPosition.Inside && (SelectedOrderCategory.Parent == null || UpdatedItem.Parent != SelectedOrderCategory.Parent))
                                {

                                    ulong? UpdatedItemParent = UpdatedItem.Parent;
                                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
                                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_Selectorder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).ToList();
                                    List<PCMArticleCategory> pcmArticleMenulist = CategoryMenulist.Where(x => x.Parent == SelectedOrderCategory.Parent && x.Position != 0).ToList();
                                    int count = 0;
                                    List<uint> indexCollection = new List<uint>();
                                    pos = 1;
                                    foreach (var updateArt in lstUpdateItem)
                                    {
                                        indexCollection.Add(updateArt.IdPCMArticleCategory);

                                    }
                                    pos = 1;
                                    foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
                                    {
                                        if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
                                        {
                                            CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                        }
                                    }

                                    if (SelectedOrderCategory.Parent == null || (e.DropPosition == DropPosition.Inside && SelectedOrderCategory.Parent != null))
                                    {
                                        count = pcmArticleMenulist.Count;
                                        switch (e.DropPosition)
                                        {
                                            case DropPosition.Append:
                                                break;
                                            case DropPosition.Before:
                                                {
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {
                                                        pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                        if (UpdatedItem.Parent != null && SelectedOrderCategory.Parent == null)
                                                        {
                                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleMenulist.IndexOf(pcmArticleMenulist.Where(a => a.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));
                                                            pcmArticleMenulist.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                        }
                                                        else
                                                        {
                                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));
                                                            pcmArticleCategory_ForSetOrder_Selectorder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                        }
                                                    }
                                                }
                                                break;
                                            case DropPosition.After:
                                                {
                                                    tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {
                                                        pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                        if (SelectedOrderCategory.Parent == null && SelectedOrderCategory.ParentName == null)
                                                        {
                                                            updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                                            updateArt.ParentName = SelectedOrderCategory.KeyName;
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleMenulist.IndexOf(pcmArticleMenulist.Where(a => a.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault())) + 1;
                                                            pcmArticleMenulist.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                        }
                                                        else
                                                        {
                                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder_Selectorder.IndexOf(pcmArticleCategory_ForSetOrder_Selectorder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault())) + 1;
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                        }
                                                        tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                    }

                                                }
                                                break;
                                            case DropPosition.Inside:
                                                {
                                                    //lstUpdateItem.ForEach(a => { a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory; });
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {
                                                        updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                                        updateArt.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory;
                                                        updateArt.Position = 0;
                                                        PCMArticleCategory.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                    }
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();

                                        switch (e.DropPosition)
                                        {
                                            case DropPosition.Append:
                                                break;
                                            case DropPosition.Before:
                                                {
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {

                                                        updateArt.Parent = SelectedOrderCategory.Parent;
                                                        updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                        updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                                        pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                    }
                                                }
                                                break;
                                            case DropPosition.After:
                                                {
                                                    tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {

                                                        updateArt.Parent = SelectedOrderCategory.Parent;
                                                        updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                        updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                        pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                        tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                    }
                                                }
                                                break;
                                            case DropPosition.Inside:
                                                {

                                                }
                                                break;
                                            default:
                                                break;
                                        }


                                    }

                                    if (pcmArticleMenulist.Count != count)
                                    {
                                        pos = 1;
                                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleMenulist)
                                        {
                                            CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
                                        }
                                    }

                                    if (SelectedOrderCategory.ParentName == null && SelectedOrderCategory.Parent == null)
                                    {
                                        pos = 1;
                                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder_Selectorder)
                                        {
                                            CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
                                        }
                                    }
                                    else
                                    {
                                        pos = 1;
                                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                                        {
                                            CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
                                        }

                                        if (UpdatedItem.Parent != SelectedOrderCategory.Parent)
                                        {
                                            if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
                                            {
                                                PCMArticleCategory_ParentChange = CategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();
                                    if (UpdatedItem.Parent == SelectedOrderCategory.Parent)
                                    {
                                        if (SelectedOrderCategory.Position > UpdatedItem.Position)
                                        {
                                            switch (e.DropPosition)
                                            {
                                                case DropPosition.Append:
                                                    break;
                                                case DropPosition.Before:
                                                    {

                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                                        }


                                                    }
                                                    break;
                                                case DropPosition.After:
                                                    {
                                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                        }

                                                    }
                                                    break;
                                                case DropPosition.Inside:
                                                    {


                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }


                                        }
                                        else
                                        {
                                            switch (e.DropPosition)
                                            {
                                                case DropPosition.Append:
                                                    break;
                                                case DropPosition.Before:
                                                    {

                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                                        }


                                                    }
                                                    break;
                                                case DropPosition.After:
                                                    {
                                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                        }


                                                    }
                                                    break;
                                                case DropPosition.Inside:
                                                    {

                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        if (e.DropPosition != DropPosition.Inside)
                                        {
                                            pos = 1;
                                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                                            {
                                                CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                    else if (SelectedOrderCategory.Parent == null && UpdatedItem.Parent == null)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_Selectorder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).ToList();

                        if (SelectedOrderCategory.Position > UpdatedItem.Position)
                        {
                            switch (e.DropPosition)
                            {
                                case DropPosition.Append:
                                    break;
                                case DropPosition.Before:
                                    {

                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                case DropPosition.After:
                                    {
                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                        if (pcmArticleCategory_ForSetOrder_Selectorder != null)
                                        {
                                            PcmArticleCategory = new PCMArticleCategory();
                                            PcmArticleCategory = pcmArticleCategory_ForSetOrder_Selectorder.Where(x => x.Parent == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault();
                                        }
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            if (SelectedOrderCategory.IdPCMArticleCategory == PcmArticleCategory.Parent)
                                            {
                                                updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                                updateArt.ParentName = SelectedOrderCategory.KeyName;
                                                updateArt.Position = 0;
                                                pcmArticleCategory_ForSetOrder_Selectorder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                            }
                                            else
                                            {
                                                updateArt.Parent = SelectedOrderCategory.Parent;
                                                updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                            }
                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                        }

                                    }
                                    break;
                                case DropPosition.Inside:
                                    {
                                        //lstUpdateItem.ForEach(a => { a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory; a.Position = 0; });
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.Position = 0;
                                            PCMArticleCategory.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }


                        }
                        else
                        {
                            switch (e.DropPosition)
                            {
                                case DropPosition.Append:
                                    break;
                                case DropPosition.Before:
                                    {

                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                        }

                                    }
                                    break;
                                case DropPosition.After:
                                    {
                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                        }


                                    }
                                    break;
                                case DropPosition.Inside:
                                    {
                                        //lstUpdateItem.ForEach(a => { a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory; });
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.Position = 0;
                                            pcmArticleCategory_ForSetOrder_Selectorder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                        pos = 0;
                                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder_Selectorder)
                                        {
                                            pcmArticleCategory.Position = pos;
                                            PCMArticleCategory.Add(pcmArticleCategory);
                                            pos++;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (PcmArticleCategory.Parent == SelectedOrderCategory.IdPCMArticleCategory)
                        {
                            if (e.DropPosition != DropPosition.Inside)
                            {
                                pos = 1;
                                foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder_Selectorder)
                                {
                                    CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                                }
                            }
                        }
                        else
                        {
                            if (e.DropPosition != DropPosition.Inside)
                            {
                                pos = 1;
                                foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                                {
                                    CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                                }
                            }
                        }
                    }


                    #region 
                    else if (SelectedOrderCategory.Parent != null && UpdatedItem.Parent == null)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_SelectedOrder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(x => x.Position).ToList();
                        //List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();

                        if (SelectedOrderCategory.Position > UpdatedItem.Position)
                        {
                            switch (e.DropPosition)
                            {
                                case DropPosition.Append:
                                    break;

                                case DropPosition.Before:
                                    {
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder_SelectedOrder.IndexOf(pcmArticleCategory_ForSetOrder_SelectedOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));
                                            pcmArticleCategory_ForSetOrder_SelectedOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                case DropPosition.After:
                                    {
                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder_SelectedOrder.IndexOf(pcmArticleCategory_ForSetOrder_SelectedOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault())) + 1;
                                            pcmArticleCategory_ForSetOrder_SelectedOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                case DropPosition.Inside:
                                    {
                                        //lstUpdateItem.ForEach(a => { a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory; a.Position = 0; });
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.Position = 0;
                                            PCMArticleCategory.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (e.DropPosition)
                            {
                                case DropPosition.Append:
                                    break;

                                case DropPosition.Before:
                                    {
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder_SelectedOrder.IndexOf(pcmArticleCategory_ForSetOrder_SelectedOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));
                                            pcmArticleCategory_ForSetOrder_SelectedOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                case DropPosition.After:
                                    {
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Parent = SelectedOrderCategory.Parent;
                                            updateArt.ParentName = SelectedOrderCategory.ParentName;
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder_SelectedOrder.IndexOf(pcmArticleCategory_ForSetOrder_SelectedOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault())) + 1;
                                            pcmArticleCategory_ForSetOrder_SelectedOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                case DropPosition.Inside:
                                    {
                                        //lstUpdateItem.ForEach(a => { a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory; a.Position = 0; });
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            updateArt.Parent = SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.ParentName = "Group_" + SelectedOrderCategory.IdPCMArticleCategory;
                                            updateArt.Position = 0;
                                            PCMArticleCategory.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (e.DropPosition != DropPosition.Inside)
                        {
                            pos = 1;
                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder_SelectedOrder)
                            {
                                CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                            }
                        }
                    }
                    #endregion

                    if (UpdatedItem.Parent != null)
                    {
                        if (e.DropPosition == DropPosition.Inside)
                        {
                            PCMArticleCategory.OrderBy(a => a.Position);
                            //PCMArticleCategory = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
                        }
                        else
                        {
                            PCMArticleCategory = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();
                        }


                        if (PCMArticleCategory_ParentChange.Count > 0)
                        {
                            PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
                        }
                    }
                    else if (UpdatedItem.Parent == null && SelectedOrderCategory.Parent == null)
                    {
                        PCMArticleCategory = CategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
                        if (PCMArticleCategory_ParentChange.Count > 0)
                        {
                            PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
                        }
                    }

                    if (e.IsFromOutside == false && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                    {
                        if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                        {
                            var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                            List<PCMArticleCategory> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName }).ToList();

                            PCMArticleCategory temp = newRecords.FirstOrDefault();

                            PCMArticleCategory target_record = (PCMArticleCategory)e.TargetRecord;
                            if ((temp.Parent == null && target_record.Parent == null) || (temp.Parent != null && target_record.Parent != null) || target_record.Parent == null) // && temp.Parent == target_record.Parent
                            {
                                e.Effects = DragDropEffects.Move;
                                e.Handled = true;

                            }
                            else
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = true;
                            }
                        }
                    }

                    ////save data from darag and dropped data.
                    if (PCMArticleCategory != null)
                    {
                        if (PCMArticleCategory.Count > 0)
                        {

                            IsSave = PCMService.IsUpdatedPCMArticleCategoryOrder(PCMArticleCategory, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateCategorySuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                            FillArticleCatagoryList();

                            GeosApplication.Instance.Logger.Log(string.Format("Method AcceptArticleCategoryAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                        }
                    }
                }


                else
                {
                    ConfirmationYesNo = false;
                }
            }


            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {

            if (e.Value != null && e.Value.ToString() != "ECOS Visibility" && e.ColumnFieldName == "IdECOSVisibility")
            {
                e.Value = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == (int)e.Value).Value;
            }

            if (e.Value != null && e.Value.ToString() != "Status" && e.ColumnFieldName == "IdPCMStatus")
            {
                e.Value = StatusList.FirstOrDefault(a => a.IdLookupValue == (int)e.Value).Value;
            }

            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        private List<string> getIdsForRetriveArticlesByParentClick()
        {
            List<string> ids = new List<string>();
            if (CategoryMenulist.Any(a => a.Parent == SelectedCategory.IdPCMArticleCategory))
            {
                List<PCMArticleCategory> getFirstList = CategoryMenulist.Where(a => a.Parent == SelectedCategory.IdPCMArticleCategory).ToList();
                foreach (PCMArticleCategory item1 in getFirstList)
                {
                    if (item1.Article_count_original != null)
                    {
                        ids.Add(item1.IdPCMArticleCategory.ToString());
                    }
                    if (CategoryMenulist.Any(a => a.Parent == item1.IdPCMArticleCategory))
                    {
                        List<PCMArticleCategory> getSecondList = CategoryMenulist.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                        foreach (PCMArticleCategory item2 in getSecondList)
                        {
                            if (item2.Article_count_original != null)
                            {
                                ids.Add(item2.IdPCMArticleCategory.ToString());
                            }
                            if (CategoryMenulist.Any(a => a.Parent == item2.IdPCMArticleCategory))
                            {
                                List<PCMArticleCategory> getThirdList = CategoryMenulist.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                foreach (PCMArticleCategory item3 in getThirdList)
                                {
                                    if (item3.Article_count_original != null)
                                    {
                                        ids.Add(item3.IdPCMArticleCategory.ToString());
                                    }
                                    if (CategoryMenulist.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                    {
                                        List<PCMArticleCategory> getForthList = CategoryMenulist.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                        foreach (PCMArticleCategory item4 in getForthList)
                                        {
                                            if (item4.Article_count_original != null)
                                            {
                                                ids.Add(item4.IdPCMArticleCategory.ToString());
                                            }
                                            if (CategoryMenulist.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                            {
                                                List<PCMArticleCategory> getFifthList = CategoryMenulist.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                foreach (PCMArticleCategory item5 in getFifthList)
                                                {
                                                    if (item5.Article_count_original != null)
                                                    {
                                                        ids.Add(item5.IdPCMArticleCategory.ToString());
                                                    }
                                                    if (CategoryMenulist.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                    {
                                                        List<PCMArticleCategory> getSixthList = CategoryMenulist.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                        foreach (PCMArticleCategory item6 in getSixthList)
                                                        {
                                                            if (item6.Article_count_original != null)
                                                            {
                                                                ids.Add(item6.IdPCMArticleCategory.ToString());
                                                            }
                                                            if (CategoryMenulist.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                            {
                                                                List<PCMArticleCategory> getSeventhList = CategoryMenulist.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                foreach (PCMArticleCategory item7 in getSeventhList)
                                                                {
                                                                    if (item7.Article_count_original != null)
                                                                    {
                                                                        ids.Add(item7.IdPCMArticleCategory.ToString());
                                                                    }
                                                                    if (CategoryMenulist.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                    {
                                                                        List<PCMArticleCategory> getEightthList = CategoryMenulist.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                        foreach (PCMArticleCategory item8 in getEightthList)
                                                                        {
                                                                            if (item8.Article_count_original != null)
                                                                            {
                                                                                ids.Add(item8.IdPCMArticleCategory.ToString());
                                                                            }
                                                                            if (CategoryMenulist.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                            {
                                                                                List<PCMArticleCategory> getNinethList = CategoryMenulist.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                foreach (PCMArticleCategory item9 in getNinethList)
                                                                                {
                                                                                    if (item9.Article_count_original != null)
                                                                                    {
                                                                                        ids.Add(item9.IdPCMArticleCategory.ToString());
                                                                                    }
                                                                                    if (CategoryMenulist.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                    {
                                                                                        List<PCMArticleCategory> gettenthList = CategoryMenulist.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
                                                                                        foreach (PCMArticleCategory item10 in gettenthList)
                                                                                        {
                                                                                            if (item10.Article_count_original != null)
                                                                                            {
                                                                                                ids.Add(item10.IdPCMArticleCategory.ToString());
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ids;
        }
        private void UpdatePCMCategoryCount()
        {
            foreach (PCMArticleCategory item in CategoryMenulist)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (CategoryMenulist.Any(a => a.Parent == item.IdPCMArticleCategory))
                {
                    List<PCMArticleCategory> getFirstList = CategoryMenulist.Where(a => a.Parent == item.IdPCMArticleCategory).ToList();
                    foreach (PCMArticleCategory item1 in getFirstList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (CategoryMenulist.Any(a => a.Parent == item1.IdPCMArticleCategory))
                        {
                            List<PCMArticleCategory> getSecondList = CategoryMenulist.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                            foreach (PCMArticleCategory item2 in getSecondList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (CategoryMenulist.Any(a => a.Parent == item2.IdPCMArticleCategory))
                                {
                                    List<PCMArticleCategory> getThirdList = CategoryMenulist.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                    foreach (PCMArticleCategory item3 in getThirdList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (CategoryMenulist.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                        {
                                            List<PCMArticleCategory> getForthList = CategoryMenulist.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                            foreach (PCMArticleCategory item4 in getForthList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (CategoryMenulist.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                                {
                                                    List<PCMArticleCategory> getFifthList = CategoryMenulist.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                    foreach (PCMArticleCategory item5 in getFifthList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (CategoryMenulist.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                        {
                                                            List<PCMArticleCategory> getSixthList = CategoryMenulist.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                            foreach (PCMArticleCategory item6 in getSixthList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (CategoryMenulist.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                                {
                                                                    List<PCMArticleCategory> getSeventhList = CategoryMenulist.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                    foreach (PCMArticleCategory item7 in getSeventhList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (CategoryMenulist.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                        {
                                                                            List<PCMArticleCategory> getEightthList = CategoryMenulist.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                            foreach (PCMArticleCategory item8 in getEightthList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (CategoryMenulist.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                                {
                                                                                    List<PCMArticleCategory> getNinethList = CategoryMenulist.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                    foreach (PCMArticleCategory item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (CategoryMenulist.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                        {
                                                                                            List<PCMArticleCategory> gettenthList = CategoryMenulist.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
                                                                                            foreach (PCMArticleCategory item10 in gettenthList)
                                                                                            {
                                                                                                if (item10.Article_count_original != null)
                                                                                                {
                                                                                                    count = count + item10.Article_count_original;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                item.Article_count = count;
                item.NameWithArticleCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.Article_count) + "]");
            }
        }
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList..."), category: Category.Info, priority: Priority.Low);

                tempStatusList = PCMService.GetLookupValues(45).ToList();
                StatusList = new List<LookupValue>(tempStatusList);
                PCMCommon.Instance.tempStatusList = StatusList;
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillECOSVisibilityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillECOSVisibilityList..."), category: Category.Info, priority: Priority.Low);

                tempECOSVisibilityList = PCMService.GetLookupValues(67).ToList();
                ECOSVisibilityList = new List<LookupValue>(tempECOSVisibilityList);
                PCMCommon.Instance.tempECOSVisibilityList = ECOSVisibilityList;

                GeosApplication.Instance.Logger.Log(string.Format("Method FillECOSVisibilityList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillECOSVisibilityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillECOSVisibilityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillECOSVisibilityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to save data of multiple rows on main Article Grid
        /// [GEOS2-2574][avpawar][Improvement in the categories manager [item 1 - Manage SubCategories] [#PCM49]]
        /// [002] [vsana][07-01-2021][GEOS2-2785] [Manage the visibility of attachments in PCM Articles [#PCM57]]
        /// [003] [cpatil][27-09-2021][GEOS2-3340] [Sr N  4 - Synchronization between PCM and ECOS. [#PLM69]]
        /// </summary>
        /// <param name="obj"></param>
        public async void UpdateMultipleRowsArticleGridCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsArticleGridCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                if (DataTableForGridLayoutCopy != null && DataTableForGridLayoutCopy.AsEnumerable().Any(x => (float)x["PurchaseQtyMin"] > (float)x["PurchaseQtyMax"] 
                || (float)x["PurchaseQtyMax"] < (float)x["PurchaseQtyMin"]))
                {
                    MinMaxError = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("MinMaxError"));

                    error = EnableValidationAndGetError();

                    if (string.IsNullOrEmpty(error))
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        return;
                    }
                    return;
                }
                else if (DataTableForGridLayoutCopy.AsEnumerable().Any(x => Convert.ToInt32(x["IdECOSVisibility"]) != 326 && ((float)x["PurchaseQtyMin"] == 0 || (float)x["PurchaseQtyMax"] == 0)))
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    return;
                }
                else
                {
                    MinMaxError = " ";
                }
                List<Articles> UpdatedArticleList = new List<Articles>();
                view = obj as TableView;
                GridControl gridControl = (view).Grid;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                IsArticleSave = false;
                IsAllSave = false;
                string cellStatus = null;
                uint cellIdPCMArticleCategory = 0;
                int MinValue = 0;
                int MaxValue = 0;
                string cellECOSVisibilityValue = null;
                string Name = string.Empty;
                cellStatus = StatusList.Where(sl => sl.Value == ProductTypeArticleViewMultipleCellEditHelper.Status).Select(u => u.Value).FirstOrDefault();
                if (ProductTypeArticleViewMultipleCellEditHelper.SelectedCategory != null)
                    cellIdPCMArticleCategory = CategoryMenulist.Where(sl => sl.IdPCMArticleCategory == ProductTypeArticleViewMultipleCellEditHelper.SelectedCategory.IdPCMArticleCategory).Select(u => u.IdPCMArticleCategory).FirstOrDefault();
                MinValue = ProductTypeArticleViewMultipleCellEditHelper.Min;
                MaxValue = ProductTypeArticleViewMultipleCellEditHelper.Max;
                cellECOSVisibilityValue = ECOSVisibilityList.Where(a => a.Value == ProductTypeArticleViewMultipleCellEditHelper.ECOSVisibilityValue).Select(u => u.Value).FirstOrDefault();
                Name = ProductTypeArticleViewMultipleCellEditHelper.Name;

                if (Name != null)
                {
                    Name = Name.Trim(' ', '\r');
                }
                DataRow[] foundRow = DataTableForGridLayoutCopy.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();
                
                //Articles[] foundRow = ArticleList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                int count = 0;
                if (foundRow.Any(i => i["Description"] == null || i["Description"].ToString().Trim() == string.Empty))
                    foreach (DataRow item in foundRow.Where(i => i["Description"] == null || i["Description"].ToString().Trim() == string.Empty))
                    {
                        if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                        {
                            if (Name != null)
                            {
                                if (item["Description"] == null)
                                    item["Description"] = string.Empty;

                                item["Description"] = item["Description"].ToString().Trim(' ', '\r');

                                if (string.IsNullOrEmpty(item["Description"].ToString()))
                                {
                                    item["Description"] = string.Empty;
                                    count++;
                                }
                            }
                        }
                    }


                if (count > 0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmptySpacesNotAllowed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                IsArticleSave = true;
                #region 
                foreach (DataRow item in foundRow)
                {
                    DataRow AI = item;
                    ChangeLogsEntry = new List<PCMArticleLogEntry>();
                    WMSArticleChangeLogEntry = new List<LogEntriesByArticle>();

                    Articles _Articles = ArticleList.First(i => i.Reference == item["Reference"].ToString());             
                    PCMArticleCategory ArticleCategory = CategoryMenulist.FirstOrDefault(i => i.IdPCMArticleCategory == (item["PcmArticleCategory"] as PCMArticleCategory).IdPCMArticleCategory);
                    _Articles.IdPCMArticleCategory = ArticleCategory.IdPCMArticleCategory;
                    LookupValue status = StatusList.FirstOrDefault(a => a.Value == item["PCMStatus"].ToString());
                    _Articles.IdPCMStatus = status.IdLookupValue;
                    _Articles.PCMStatus = status.Value;
                    LookupValue ECOSVisibility =  ECOSVisibilityList.FirstOrDefault(a => a.Value == item["ECOSVisibilityValue"].ToString());                  
                    _Articles.IdECOSVisibility = ECOSVisibility.IdLookupValue;
                    _Articles.ECOSVisibilityValue = ECOSVisibility.Value;
                    if (ECOSVisibility.Value == "Read Only")
                    {
                        _Articles.PurchaseQtyMin = 0;
                        _Articles.PurchaseQtyMax = 0;
                    }
                    else
                    {
                        _Articles.PurchaseQtyMin = 1;
                        _Articles.PurchaseQtyMax = 1;
                    }
                    if (string.IsNullOrEmpty(item["Description"]?.ToString()))
                        return;
                    item["Description"] = item["Description"]?.ToString().Trim(' ', '\r');
                    _Articles.Description = item["Description"]?.ToString();
                    _Articles.PCMArticleLogEntiryList = new List<PCMArticleLogEntry>();
                    _Articles.WarehouseArticleLogEntiryList = new List<LogEntriesByArticle>();

                    Articles TempArticleList = new Articles();
                    TempArticleList = PCMService.GetArticleByIdArticleForInformationData(_Articles.IdArticle);

                    _Articles.PurchaseQtyMin = (float)AI["PurchaseQtyMin"];
                    _Articles.PurchaseQtyMax = (float)AI["PurchaseQtyMax"];

                    if ((!string.IsNullOrEmpty(cellStatus)) || (cellIdPCMArticleCategory != 0) || (MinValue != 0) || (MaxValue != 0) || (!string.IsNullOrEmpty(cellECOSVisibilityValue)) || (!(string.IsNullOrEmpty(Name))))
                    {
                        if (_Articles.IdPCMStatus != TempArticleList.IdPCMStatus)
                        {

                            ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), TempArticleList.PCMStatus, _Articles.PCMStatus) });
                        }
                        #region ECOSVisibility
                        if (_Articles.IdECOSVisibility != TempArticleList.IdECOSVisibility)
                        {

                            if (_Articles.IdECOSVisibility == 323)
                            {
                                _Articles.IsShareWithCustomer = 1;
                                _Articles.IsSparePartOnly = 0;
                                IsReadOnlyMinMax = false;

                                if (_Articles.PurchaseQtyMin == 0)
                                {
                                    _Articles.PurchaseQtyMin = 1;
                                    AI["PurchaseQtyMin"] = 1;
                                }

                                if (_Articles.PurchaseQtyMax == 0)
                                {
                                    _Articles.PurchaseQtyMax = 1;
                                    AI["PurchaseQtyMax"] = 1;
                                }
                            }
                            else if (_Articles.IdECOSVisibility == 324)
                            {
                                _Articles.IsShareWithCustomer = 0;
                                _Articles.IsSparePartOnly = 1;
                                IsReadOnlyMinMax = false;

                                if (_Articles.PurchaseQtyMin == 0)
                                {
                                    _Articles.PurchaseQtyMin = 1;
                                    AI["PurchaseQtyMin"] = 1;
                                }

                                if (_Articles.PurchaseQtyMax == 0)
                                {
                                    _Articles.PurchaseQtyMax = 1;
                                    AI["PurchaseQtyMax"] = 1;
                                }
                            }
                            else if (_Articles.IdECOSVisibility == 325)
                            {
                                _Articles.IsShareWithCustomer = 0;
                                _Articles.IsSparePartOnly = 0;
                                IsReadOnlyMinMax = false;

                                if (_Articles.PurchaseQtyMin == 0)
                                {
                                    _Articles.PurchaseQtyMin = 1;
                                    AI["PurchaseQtyMin"] = 1;
                                }

                                if (_Articles.PurchaseQtyMax == 0)
                                {
                                    _Articles.PurchaseQtyMax = 1;
                                    AI["PurchaseQtyMax"] = 1;
                                }
                            }
                            else if (_Articles.IdECOSVisibility == 326)
                            {
                                _Articles.IsShareWithCustomer = 0;
                                _Articles.IsSparePartOnly = 0;
                                _Articles.PurchaseQtyMin = 0;
                                _Articles.PurchaseQtyMax = 0;
                                IsReadOnlyMinMax = true;
                            }
                            // _Articles.IdECOSVisibility = AI.IdECOSVisibility;
                            if (TempArticleList.IdECOSVisibility == 0)
                                ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityChangeLogStatus").ToString(), "None", _Articles.ECOSVisibilityValue, TempArticleList.Reference) });
                            else
                                ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityChangeLogStatus").ToString(), ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == TempArticleList.IdECOSVisibility).Value, _Articles.ECOSVisibilityValue, TempArticleList.Reference) });
                        }
                        #endregion

                        if ((float)AI["PurchaseQtyMin"] != TempArticleList.PurchaseQtyMin)
                        {
                            changeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMin").ToString(), TempArticleList.PurchaseQtyMin, AI["PurchaseQtyMin"].ToString(), TempArticleList.Reference) });
                        }

                        if ((float)AI["PurchaseQtyMax"] != TempArticleList.PurchaseQtyMax)
                        {
                            changeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMax").ToString(), TempArticleList.PurchaseQtyMax, AI["PurchaseQtyMax"]?.ToString(), TempArticleList.Reference) });
                        }

                        if (ArticleCategory != null && ArticleCategory.IdArticleCategory != TempArticleList.PcmArticleCategory.IdArticleCategory)
                        {
                            _Articles.IdPCMArticleCategory = ArticleCategory.IdPCMArticleCategory;
                            ArticleList.Where(a => a.IdArticle == _Articles.IdArticle).ToList().ForEach(b => b.IdPCMArticleCategory = ArticleCategory.IdPCMArticleCategory);
                            ArticleList_All.Where(a => a.IdArticle == _Articles.IdArticle).ToList().ForEach(b => b.IdPCMArticleCategory = ArticleCategory.IdPCMArticleCategory);

                            ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleChangeLogCategory").ToString(), TempArticleList.PcmArticleCategory.Name, AI["PcmArticleCategory"].ToString()) });
                        }

                        if (AI["Description"]?.ToString() != TempArticleList.Description)
                        {
                            ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdate").ToString(), TempArticleList.Description, AI["Description"].ToString(), TempArticleList.Reference) });
                            WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)_Articles.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdate").ToString(), TempArticleList.Description, AI["Description"].ToString(), TempArticleList.Reference) });
                        }

                        #region PurchaseQtyMin & PurchaseQtyMax
                        if ((_Articles.IdPCMStatus != TempArticleList.IdPCMStatus) || (ArticleCategory != null && ArticleCategory.IdPCMArticleCategory != TempArticleList.PcmArticleCategory.IdPCMArticleCategory) ||
                            ((float)AI["PurchaseQtyMin"] != TempArticleList.PurchaseQtyMin) || ((float)AI["PurchaseQtyMax"] != TempArticleList.PurchaseQtyMax) || (_Articles.IdECOSVisibility != TempArticleList.IdECOSVisibility) || (AI["Description"]?.ToString() != TempArticleList.Description))
                        {
                            if (ChangeLogsEntry != null || WMSArticleChangeLogEntry != null)
                            {
                                _Articles.PCMArticleLogEntiryList.AddRange(ChangeLogsEntry);
                                _Articles.WarehouseArticleLogEntiryList.AddRange(WMSArticleChangeLogEntry);
                                _Articles.TransactionOperation = ModelBase.TransactionOperations.Update;
                                _Articles.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            }                         
                            if (Convert.ToInt32(AI["IdECOSVisibility"]) == 323)
                            {
                                _Articles.IsShareWithCustomer = 1;
                                _Articles.IsSparePartOnly = 0;
                                IsReadOnlyMinMax = false;

                                if (_Articles.PurchaseQtyMin == 0)
                                {
                                    _Articles.PurchaseQtyMin = 1;
                                    AI["PurchaseQtyMin"] = 1;
                                }

                                if (_Articles.PurchaseQtyMax == 0)
                                {
                                    _Articles.PurchaseQtyMax = 1;
                                    AI["PurchaseQtyMax"] = 1;
                                }
                            }
                            else if (Convert.ToInt32(AI["IdECOSVisibility"]) == 324)
                            {
                                _Articles.IsShareWithCustomer = 0;
                                _Articles.IsSparePartOnly = 1;
                                IsReadOnlyMinMax = false;

                                if (_Articles.PurchaseQtyMin == 0)
                                {
                                    _Articles.PurchaseQtyMin = 1;
                                    AI["PurchaseQtyMin"] = 1;
                                }

                                if (_Articles.PurchaseQtyMax == 0)
                                {
                                    _Articles.PurchaseQtyMax = 1;
                                    AI["PurchaseQtyMax"] = 1;
                                }
                            }
                            else if (Convert.ToInt32(AI["IdECOSVisibility"]) == 325)
                            {
                                _Articles.IsShareWithCustomer = 0;
                                _Articles.IsSparePartOnly = 0;
                                IsReadOnlyMinMax = false;

                                if (_Articles.PurchaseQtyMin == 0)
                                {
                                    _Articles.PurchaseQtyMin = 1;
                                    AI["PurchaseQtyMin"] = 1;
                                }

                                if (_Articles.PurchaseQtyMax == 0)
                                {
                                    _Articles.PurchaseQtyMax = 1;
                                    AI["PurchaseQtyMax"] = 1;
                                }
                            }
                            else if (Convert.ToInt32(AI["IdECOSVisibility"]) == 326)
                            {
                                _Articles.IsShareWithCustomer = 0;
                                _Articles.IsSparePartOnly = 0;
                                _Articles.PurchaseQtyMin = 0;
                                _Articles.PurchaseQtyMax = 0;
                                IsReadOnlyMinMax = true;
                            }
                            if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle != true)
                            {
                                Name = TempArticleList.Description;
                            }

                            _Articles.PCMDescription = TempArticleList.PCMDescription;
                            _Articles.IsRtfText = TempArticleList.IsRtfText;

                            if (TempArticleList.PCMDescription != null)
                            {
                                _Articles.IsRichText = "Yes";
                                isSaveFromReference = true;
                            }

                            IsArticleSave = PCMService.IsUpdatePCMArticleCategoryInArticleWithStatus_V2110(_Articles.IdPCMArticleCategory, _Articles);
                        }
                        #endregion
                        item["IsChecked"] = false;
                    }
                    UpdatedArticleList.Add(_Articles);
                }
                #endregion
                if (IsArticleSave)
                {
                    IsAllSave = true;
                    if (isSaveFromReference == false)
                    {
                        FillArticleList();
                        FillArticleCatagoryList();
                    }
                    gridControl.RefreshData();
                    gridControl.UpdateLayout();
                    isSaveFromReference = false;
                }
                else
                    IsAllSave = false;

                if (IsAllSave == null)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
                else if (IsAllSave.Value == true)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                else if (IsAllSave.Value == false)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                //[003] Added code for synchronization
                if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                {
                    GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59");
                    if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                    {
                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 57))
                        {
                            if ((!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) && (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue))))    //(!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                            {
                                #region synchronization Code
                                var ownerInfo = (obj as FrameworkElement);
                                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                if (MessageBoxResult == MessageBoxResult.Yes)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "The Synchronization is running";

                                    if (foundRow.Count() > 0)
                                        if (!string.IsNullOrEmpty(string.Join(",", foundRow.Select(i => i["Reference"]).ToList())))
                                        {
                                            //GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59");
                                            if (!DXSplashScreen.IsActive)
                                            {
                                                DXSplashScreen.Show(x =>
                                                {
                                                    Window win = new Window()
                                                    {
                                                        Focusable = true,
                                                        ShowActivated = false,
                                                        WindowStyle = WindowStyle.None,
                                                        ResizeMode = ResizeMode.NoResize,
                                                        AllowsTransparency = false,
                                                        Background = new SolidColorBrush(Colors.Transparent),
                                                        ShowInTaskbar = false,
                                                        Topmost = false,
                                                        SizeToContent = SizeToContent.WidthAndHeight,
                                                        WindowStartupLocation = WindowStartupLocation.CenterScreen,

                                                    };
                                                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);


                                                    return win;
                                                }, x =>
                                                {
                                                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                                                }, new object[] { new SplashScreenOwner(ownerInfo), WindowStartupLocation.CenterOwner }, null);
                                            }

                                            try
                                            {
                                                APIErrorDetail values = new APIErrorDetail();
                                                if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                                {
                                                    string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                                    if (tokeninformations.Count() >= 2)
                                                    {
                                                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 57))
                                                        {
                                                            APIErrorDetailForErrorFalse valuesErrorFalse = await PCMService.IsPCMProductTypeArticleSynchronization(GeosAppSettingList, UpdatedArticleList?.ToArray());
                                                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                            if (valuesErrorFalse != null && valuesErrorFalse.Error == "false")
                                                            {

                                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                            }
                                                            else
                                                            {
                                                                if (values != null && values.Message != null)
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), values.Message._Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                }
                                                                else if (valuesErrorFalse != null && valuesErrorFalse.Message != null)
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), valuesErrorFalse.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                }
                                                            }
                                                        }
                                                    }

                                                }

                                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                                            }
                                            catch (Exception ex)
                                            {
                                                GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                            }

                                            //GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                        }
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                }
                                #endregion
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsArticleGridCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsArticleGridCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsArticleGridCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateMultipleRowsArticleGridCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to add checked list for column StatusList
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);
                if (e.Column.FieldName == "ArticleImageCount")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("ArticleImageCount is null")
                    });

                    foreach (Articles item in ArticleList)
                    {
                        if (item.ArticleImageCount == null)
                        {
                            continue;
                        }
                        else
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "Image(s)",
                                EditValue = CriteriaOperator.Parse("ArticleImageCount is not null")
                            });
                            break;
                        }
                    }
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
               
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopup()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void OnViewLoaded(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;

                if (ProductTypeArticleViewMultipleCellEditHelper.IsLoad == true)
                {
                    Articles[] foundRow = ArticleList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                    foreach (Articles item in foundRow)
                    {
                        Articles oldData = ClonedArticleList.FirstOrDefault(a => a.IdArticle == item.IdArticle);
                        item.Description = oldData.Description;
                        item.IdECOSVisibility = oldData.IdECOSVisibility;
                        item.ECOSVisibilityValue = oldData.ECOSVisibilityValue;
                        item.ECOSVisibilityHTMLColor = oldData.ECOSVisibilityHTMLColor;
                        item.PurchaseQtyMin = oldData.PurchaseQtyMin;
                        item.PurchaseQtyMax = oldData.PurchaseQtyMax;
                        item.IdPCMStatus = oldData.IdPCMStatus;
                        item.PcmArticleCategory = oldData.CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == oldData.IdPCMArticleCategory);
                        item.IsImageButtonEnabled = oldData.IsImageButtonEnabled;
                        item.StatusHTMLColor = oldData.StatusHTMLColor;               
                    }
                }
                ((TableView)obj).DataControl.RefreshData();
                ((TableView)obj).DataControl.UpdateLayout();
                GeosApplication.Instance.Logger.Log("Method OnViewLoaded() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method OnViewLoaded()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PasteInSearchControlCommandAction(object obj)//[rdixit][01.08.2022][GEOS2-3373]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PasteInSearchControlCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                var clipboardData = Clipboard.GetText();
                if (clipboardData != null)
                {
                    string[] rows = clipboardData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rows.Length > 0)
                    {
                        string search = string.Join(" ", rows.Select(a => a.ToString()));
                        tableView.SearchString = search;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method PasteInSearchControlCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on PasteInSearchControlCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[24.11.2022][rdixit][GEOS2-2718]
        private void ExpandAndCollapsCategoriesCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsExpand)
            {
                t.ExpandAllNodes();
                IsExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsExpand = true;
            }
        }
        //[sshegaonkar][GEOS2-2922][14.02.2023]
        private void OpenArticleImageCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenArticleImageCommand()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                System.Data.DataRowView SelectedRow = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                Articles selectedArticle = ArticleList.FirstOrDefault(i => i.Reference == SelectedRow.Row["Reference"].ToString());
                ArticleGridImageView articleGridImageView = new ArticleGridImageView();
                ArticleGridImageViewModel articleGridImageViewModel = new ArticleGridImageViewModel();
                EventHandler handle = delegate { articleGridImageView.Close(); };
                articleGridImageViewModel.RequestClose += handle;
                articleGridImageViewModel.Articles = new Articles();
                articleGridImageViewModel.Init(selectedArticle);
                articleGridImageView.DataContext = articleGridImageViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articleGridImageView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method OpenArticleImageCommand()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenArticleImageCommand() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-4897][01.12.2023]
        void ColumnName()
        {
            string Symbol = GeosApplication.Instance.PCMCurrentCurrency.Symbol;
            EAESPrice = "EAES (" + Symbol + ") ";
            EAMXPrice = "EAMX (" + Symbol + ") ";
            EAROPrice = "EARO (" + Symbol + ") ";            
            EBROPrice = "EBRO (" + Symbol + ") ";
            EEPYPrice = "EEPY (" + Symbol + ") ";
            EIBRPrice = "EIBR (" + Symbol + ") ";
            EJMX1Price = "EJMX1 (" + Symbol + ") ";
            EJMX2Price = "EJMX2 (" + Symbol + ") ";
            ENRUPrice = "ENRU (" + Symbol + ") ";
            EPINPrice = "EPIN (" + Symbol + ") ";
            ESCNPrice = "ESCN (" + Symbol + ") ";
            ESHNPrice = "ESHN (" + Symbol + ") ";
            ESMA1Price = "ESMA1 (" + Symbol + ") ";
            ESMA2Price = "ESMA2 (" + Symbol + ") ";
            ESMXPrice = "ESMX (" + Symbol + ") ";
            ETCNPrice = "ETCN (" + Symbol + ") ";
            ETHQPrice = "ETHQ (" + Symbol + ") ";
            ETMAPrice = "ETMA (" + Symbol + ") ";
            ETTNPrice = "ETTN (" + Symbol + ") ";

            #region C1
            if (ArticleList != null && CompanyList != null)
            {
                if (!ArticleList.Any(i => i.C1?.Price > 0))
                {
                    C1Show = false;
                }
                else
                {
                    var firstArticleC1 = ArticleList.FirstOrDefault()?.C1;

                    if (firstArticleC1 != null)
                    {
                        var Company = CompanyList.FirstOrDefault(i => i.IdCompany == firstArticleC1.IdCompany);

                        if (Company != null)
                        {
                            C1 = $"{Company.ShortName} ({Symbol})";
                            C1Show = true;
                        }
                        else
                            C1Show = false;
                    }
                    else
                        C1Show = false;
                }
                #endregion

                #region C2
                if (!ArticleList.Any(i => i.C2?.Price > 0))
                {
                    C2Show = false;
                }
                else
                {
                    var firstArticleC2 = ArticleList.FirstOrDefault()?.C2;

                    if (firstArticleC2 != null)
                    {
                        var Company = CompanyList.FirstOrDefault(i => i.IdCompany == firstArticleC2.IdCompany);

                        if (Company != null)
                        {
                            C2 = $"{Company.ShortName} ({Symbol})";
                            C2Show = true;
                        }
                        else
                            C2Show = false;
                    }
                    else
                        C2Show = false;
                }
                #endregion

                #region C3
                if (!ArticleList.Any(i => i.C3?.Price > 0))
                {
                    C3Show = false;
                }
                else
                {
                    var firstArticleC3 = ArticleList.FirstOrDefault()?.C3;

                    if (firstArticleC3 != null)
                    {
                        var Company = CompanyList.FirstOrDefault(i => i.IdCompany == firstArticleC3.IdCompany);

                        if (Company != null)
                        {
                            C3 = $"{Company.ShortName} ({Symbol})";
                            C3Show = true;
                        }
                        else
                            C3Show = false;
                    }
                    else
                        C3Show = false;
                }
                #endregion

                #region C4
                if (!ArticleList.Any(i => i.C4?.Price > 0))
                {
                    C4Show = false;
                }
                else
                {
                    var firstArticleC4 = ArticleList.FirstOrDefault()?.C4;

                    if (firstArticleC4 != null)
                    {
                        var Company = CompanyList.FirstOrDefault(i => i.IdCompany == firstArticleC4.IdCompany);

                        if (Company != null)
                        {
                            C4 = $"{Company.ShortName} ({Symbol})";
                            C4Show = true;
                        }
                        else
                            C4Show = false;
                    }
                    else
                        C4Show = false;
                }
                #endregion

                #region C5
                if (!ArticleList.Any(i => i.C5?.Price > 0))
                {
                    C5Show = false;
                }
                else
                {
                    var firstArticleC5 = ArticleList.FirstOrDefault()?.C5;

                    if (firstArticleC5 != null)
                    {
                        var Company = CompanyList.FirstOrDefault(i => i.IsCompany == firstArticleC5.IdCompany);

                        if (Company != null)
                        {
                            C5 = $"{Company.ShortName} ({Symbol})";
                            C5Show = true;
                        }
                        else
                            C5Show = false;
                    }
                    else
                        C5Show = false;
                }
                #endregion
            }
        }

        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error = me[BindableBase.GetPropertyName(() => MinMaxError)];


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

                string gridMinMax = BindableBase.GetPropertyName(() => MinMaxError);

                if (columnName == gridMinMax)
                    return PCMArticleMinAndMaxValidation.GetErrorMessage(gridMinMax, MinMaxError);

                return null;
            }
        }
        /// <summary>
        /// If any feild is of Information has error set isInformationError = true;
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>

        #endregion

        #region Column Chooser
        private void ArticleGridControlLoadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);

                if (!(obj is GridControl gridControl)) return;

                TableView tableView = (TableView)gridControl.View;
                int visibleFalseColumn = 0;

                gridControl.BeginInit();

                // Restore layout if the settings file exists
                if (File.Exists(ArticleGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(ArticleGridSettingFilePath);
                }

                // Save the current layout
                gridControl.SaveLayoutToXml(ArticleGridSettingFilePath);

                // Iterate through the grid columns and handle visibility and position changes
                DependencyPropertyDescriptor visibleDescriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                DependencyPropertyDescriptor positionDescriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));

                foreach (GridColumn column in gridControl.Columns)
                {
                    visibleDescriptor?.AddValueChanged(column, ArticleVisibleChanged);
                    positionDescriptor?.AddValueChanged(column, ArticleVisibleIndexChanged);

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                // Set column chooser visibility based on hidden columns count
                IsArticleColumnChooserVisible = visibleFalseColumn > 0;

                gridControl.EndInit();

                // Reset search and group panel settings
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                // Ensure splash screen is closed
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ArticleGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ArticleGridControlLoadedAction: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        void ArticleVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleVisibleChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    if (column.ShowInColumnChooser)
                    {                     
                            ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ArticleGridSettingFilePath);
                    }

                    if (!column.Visible)
                    {
                        IsArticleColumnChooserVisible = true;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ArticleVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ArticleVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void ArticleVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ArticleGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ArticleVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ArticleVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ArticleItemListTableViewLoadedAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new Point(20, 180),
                Size = new Size(250, 250)
            };
        }

        private void ArticleGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(ArticleGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method ArticleGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ArticleGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
