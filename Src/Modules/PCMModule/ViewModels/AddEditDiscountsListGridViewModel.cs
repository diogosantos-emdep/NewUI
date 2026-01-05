using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Editors;
using System.ComponentModel;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using System.Windows.Input;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Modules.PLM.CommonClasses;
using DevExpress.Xpf.LayoutControl;
using System.Data;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Modules.PCM;
using Emdep.Geos.Modules.PLM;
using DevExpress.Data;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using Emdep.Geos.Modules.PLM.Views;
using Emdep.Geos.Modules.PLM.ViewModels;
using System.Windows.Media.Imaging;
using System.IO;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddEditDiscountsListGridViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Task_Log
        //[rdixit][13.09.2022][GEOS2-3099]
        #endregion

        #region Service

       IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        // IPCMService PCMService = new PCMServiceController("localhost:6699");
        // IPLMService PLMService = new PLMServiceController("localhost:6699");
        // ICrmService CrmStartUp = new CrmServiceController("localhost:6699");

        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
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

        #region Declarations
        #region[rdixit][12.10.2022][GEOS2-3967]
        List<DiscountArticles> addeddiscountArticles;
        List<DiscountArticles> deleteddiscountArticles;
        private List<DiscountLogEntry> newdiscountChangeLogList;
        #endregion
        private int group;
        private int region;
        private int country;
        private int plant;
        bool isCheckedReadOnly;
        private DateTime? oldStartDate;
        private DateTime? oldEndDate;
        private string oldName;
        private string oldDescription;
        private string oldSelectedLanguage;
        public bool isDuplicateCliked = false;
        private bool isInUse;
        private bool isNewDiscountSave;
        private bool isDiscountUpdated;
        private LookupValue selectedPlatform;
        private LookupValue selectedScope;
        private ObservableCollection<BandItem> bands;
        private string myFilterString;
        private bool isFirstTimeLoad;
        private bool isCalculated;
        private bool isEnabledAcceptButton;
        private Visibility readOnlyAndValueFieldVisbility;
        public bool isEnabledValue;
        private DataTable dataTable;
        private DataTable dataTableForGridLayout;
        private double dialogHeight;
        private double dialogWidth;
        #region Language
        private ObservableCollection<Language> languages;
        private Language languageSelected;
        private string description;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;
        private string name;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;
        #endregion
        MaximizedElementPosition maximizedElementPosition;
        private string windowHeader;
        private bool isNew;
        private ObservableCollection<Site> plantList;
        private List<object> selectedPlant;
        private DateTime endDate;
        private DateTime startDate;
        private DateTime lastUpdated;
        public ObservableCollection<LookupValue> scopeList;
        public ObservableCollection<LookupValue> platformList;
        decimal discountvalue;
        ObservableCollection<PCMArticleCategory> articleMenuList;
        DataTable dtArticle;
        PCMArticleCategory selectedArticle;
        List<UInt64> addArticles;
        string informationError;
        List<DiscountArticles> discountArticles;
        Visibility articleGridVisibility;
        public ObservableCollection<Site> selectedPlantList;
        List<DiscountCustomers> discountCustomersClone;
        List<DiscountCustomers> discountCustomers;
        List<DiscountCustomers> diffDiscountCustomer;
        string customerCountry;
        string customerPlant;
        string customerRegion;
        string customerGroup;
        private ObservableCollection<DiscountLogEntry> discountChangeLogList;
        private DiscountLogEntry selecteddisccountChangeLog;
        private string disccountOldName;
        private Discounts discountsDetails;
        private ObservableCollection<Discounts> discountsList;
        public bool isEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][14/02/2023]



        private ObservableCollection<DiscountLogEntry> commentList;//[Sudhir.Jangra][GEOS2-4935]
        private string commentText;//[Sudhir.Jangra][GEOS2-4935]
        private DateTime? commentDateTimeText;//[Sudhir.Jangra][GEOS2-4935]
        private string commentFullNameText;//[Sudhir.Jangra][GEOS2-4935]
        private byte[] userProfileImageByte;//[Sudhir.Jangra][GEOS2-4935]
        private List<DiscountLogEntry> addCommentsList;//[Sudhir.jangra][GEOS2-4935]
        private DiscountLogEntry selectedComment;//[Sudhir.jangra][GEOSS2-4935]
        private ObservableCollection<DiscountLogEntry> deleteCommentsList;//[Sudhir.Jangra][GEOS2-4935]
        private bool isDeleted;//[Sudhir.Jangra][GEOS2-4935]
        private List<DiscountLogEntry> updatedCommentList;//[Sudhir.Jangra][GEOS2-4935]
        private List<DiscountLogEntry> clonedDiscountCommentsList;//[Sudhir.jangra][GEOS2-4935]

        #endregion

        #region Properties
        #region[rdixit][12.10.2022][GEOS2-3967]
        public List<DiscountArticles> AddedDiscountArticles
        {
            get
            {
                return addeddiscountArticles;
            }
            set
            {
                addeddiscountArticles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddedDiscountArticles"));

            }
        }
        public List<DiscountArticles> DeletedDiscountArticles
        {
            get
            {
                return deleteddiscountArticles;
            }
            set
            {
                deleteddiscountArticles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedDiscountArticles"));

            }
        }
        public List<DiscountLogEntry> NewDiscountChangeLogList
        {
            get
            {
                return newdiscountChangeLogList;
            }

            set
            {
                newdiscountChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewDiscountChangeLogList"));

            }
        }
        #endregion
        public ObservableCollection<Discounts> DiscountsList
        {
            get
            {
                return discountsList;
            }

            set
            {
                discountsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountsList"));

            }
        }
        public List<DiscountArticles> DiscountArticles
        {
            get
            {
                return discountArticles;
            }
            set
            {
                discountArticles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountArticles"));

            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public List<UInt64> AddArticles
        {
            get
            {
                return addArticles;
            }

            set
            {
                addArticles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddArticles"));
            }
        }
        public bool IsInUse
        {
            get
            {
                return isInUse;
            }

            set
            {
                isInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInUse"));
            }
        }
        public bool IsNewDiscountSave
        {
            get
            {
                return isNewDiscountSave;
            }

            set
            {
                isNewDiscountSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNewDiscountSave"));
            }
        }
        public bool IsDiscountUpdated
        {
            get
            {
                return isDiscountUpdated;
            }

            set
            {
                isDiscountUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDiscountUpdated"));
            }
        }
        public Discounts NewDiscount { get; set; }
        public Discounts EditDiscount { get; set; }
        public Discounts ClonedDiscount { get; set; }
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));

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
        public ObservableCollection<LookupValue> ScopeList
        {
            get { return scopeList; }
            set
            {
                scopeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScopeList"));

            }
        }
        public LookupValue SelectedScope
        {
            get { return selectedScope; }
            set
            {
                selectedScope = value;
                if (SelectedScope.Value == "Order")
                {
                    ReadOnlyAndValueFieldVisbility = Visibility.Visible;
                    ArticleGridVisibility = Visibility.Hidden;
                }
                else
                {
                    ReadOnlyAndValueFieldVisbility = Visibility.Hidden;
                    ArticleGridVisibility = Visibility.Visible;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScope"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        public LookupValue SelectedPlatform
        {
            get { return selectedPlatform; }
            set
            {
                selectedPlatform = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlatform"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public ObservableCollection<LookupValue> PlatformList
        {
            get { return platformList; }
            set
            {
                platformList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlatformList"));
            }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }
        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }
        public Language LanguageSelected
        {
            get
            {
                return languageSelected;
            }
            set
            {
                languageSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelected"));
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public decimal Value
        {
            get { return discountvalue; }
            set
            {
                discountvalue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Value"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Description_en
        {
            get
            {
                return description_en;
            }
            set
            {
                description_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_en"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }

        }
        public string Description_es
        {
            get
            {
                return description_es;
            }
            set
            {
                description_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Description_fr
        {
            get
            {
                return description_fr;
            }
            set
            {
                description_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Description_pt
        {
            get
            {
                return description_pt;
            }
            set
            {
                description_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Description_ro
        {
            get
            {
                return description_ro;
            }
            set
            {
                description_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Description_ru
        {
            get
            {
                return description_ru;
            }
            set
            {
                description_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Description_zh
        {
            get
            {
                return description_zh;
            }
            set
            {
                description_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_en
        {
            get
            {
                return name_en;
            }
            set
            {
                name_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_en"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_es
        {
            get
            {
                return name_es;
            }
            set
            {
                name_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_es"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_fr
        {
            get
            {
                return name_fr;
            }
            set
            {
                name_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_fr"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_pt
        {
            get
            {
                return name_pt;
            }
            set
            {
                name_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_pt"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_ro
        {
            get
            {
                return name_ro;
            }
            set
            {
                name_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ro"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_ru
        {
            get
            {
                return name_ru;
            }
            set
            {
                name_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ru"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string Name_zh
        {
            get
            {
                return name_zh;
            }
            set
            {
                name_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_zh"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }
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
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }
        public ObservableCollection<Site> SelectedPlantList
        {
            get
            {
                return selectedPlantList;
            }

            set
            {
                selectedPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantList"));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }
            set
            {
                lastUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdated"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public bool IsCalculated
        {
            get
            {
                return isCalculated;
            }

            set
            {
                isCalculated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalculated"));
            }
        }
        public bool IsFirstTimeLoad
        {
            get
            {
                return isFirstTimeLoad;
            }

            set
            {
                isFirstTimeLoad = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFirstTimeLoad"));
            }
        }
        public bool IsEnabledAcceptButton
        {
            get { return isEnabledAcceptButton; }
            set
            {
                isEnabledAcceptButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledAcceptButton"));
            }
        }
        public bool IsEnabledValue
        {
            get { return isEnabledValue; }
            set
            {
                isEnabledValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledValue"));
            }
        }
        public Visibility ReadOnlyAndValueFieldVisbility
        {
            get { return readOnlyAndValueFieldVisbility; }
            set { readOnlyAndValueFieldVisbility = value; OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyAndValueFieldVisbility")); }
        }
        public Visibility ArticleGridVisibility
        {
            get { return articleGridVisibility; }
            set { articleGridVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("ArticleGridVisibility")); }
        }
        public bool IsCheckedReadOnly
        {
            get
            {
                return isCheckedReadOnly;
            }
            set
            {
                isCheckedReadOnly = value;
                if (isCheckedReadOnly)
                    IsEnabledValue = false;
                else
                    IsEnabledValue = true;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedReadOnly"));
            }
        }
        public ObservableCollection<PCMArticleCategory> ArticleMenuList
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
        public DataTable DtArticle
        {
            get { return dtArticle; }
            set
            {
                dtArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtArticle"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public PCMArticleCategory SelectedArticle
        {
            get
            {
                return selectedArticle;
            }

            set
            {
                selectedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticle"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public List<DiscountCustomers> DiscountCustomersClone
        {
            get
            {
                return discountCustomersClone;
            }

            set
            {
                discountCustomersClone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountCustomersClone"));
            }
        }
        public List<DiscountCustomers> DiscountCustomers
        {
            get
            {
                return discountCustomers;
            }

            set
            {
                discountCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountCustomers"));
            }
        }
        public string CustomerGroup
        {
            get
            {
                return customerGroup;
            }
            set
            {
                customerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroup"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string CustomerRegion
        {
            get
            {
                return customerRegion;
            }
            set
            {
                customerRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerRegion"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string CustomerPlant
        {
            get
            {
                return customerPlant;
            }
            set
            {
                customerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlant"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string CustomerCountry
        {
            get
            {
                return customerCountry;
            }
            set
            {
                customerCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerCountry"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        public int Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Group"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public int Region
        {
            get
            {
                return region;
            }
            set
            {
                region = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Region"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public int Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public int Plant
        {
            get
            {
                return plant;
            }
            set
            {
                plant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Plant"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public string DisccountOldName
        {
            get
            {
                return disccountOldName;
            }

            set
            {
                disccountOldName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DisccountOldName"));
            }
        }

        public ObservableCollection<DiscountLogEntry> DiscountChangeLogList
        {
            get
            {
                return discountChangeLogList;
            }

            set
            {
                discountChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountChangeLogList"));
            }
        }

        public bool IsEnabledCancelButton//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
        {
            get { return isEnabledCancelButton; }
            set
            {
                isEnabledCancelButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancelButton"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public ObservableCollection<DiscountLogEntry> CommentsList
        {
            get { return commentList; }
            set
            {
                commentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public DateTime? CommentDateTimeText
        {
            get { return commentDateTimeText; }
            set
            {
                commentDateTimeText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentDateTimeText"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public string CommentFullNameText
        {
            get { return commentFullNameText; }
            set
            {
                commentFullNameText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentFullNameText"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public byte[] UserProfileImageByte
        {
            get { return userProfileImageByte; }
            set
            {
                userProfileImageByte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImageByte"));
            }
        }

        public List<DiscountLogEntry> AddCommentsList
        {
            get { return addCommentsList; }
            set
            {
                addCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public DiscountLogEntry SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public ObservableCollection<DiscountLogEntry> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public List<DiscountLogEntry> UpdatedCommentsList
        {
            get { return updatedCommentList; }
            set
            {
                updatedCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public List<DiscountLogEntry> ClonedDiscountCommentsList
        {
            get { return clonedDiscountCommentsList; }
            set
            {
                clonedDiscountCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDiscountCommentsList"));
            }
        }

        #endregion

        #region Command
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand ChangeScopeCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand ChangeStartDateCommand { get; set; }
        public ICommand ChangeEndDateCommand { get; set; }
        public ICommand ChangeValueCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ItemListTableViewLoadedCommand { get; set; }
        public ICommand CommandDropRecordArticleGrid { get; set; }
        public ICommand IsCheckedReadOnlyCommand { get; set; }
        public ICommand CommandCompleteRecordDragDropArticleGrid { get; set; }
        public ICommand CommandOnDragRecordOverArticleGrid { get; set; }
        public ICommand DeleteArticleCommand { get; set; }
        public ICommand ExportToExcelDiscountLogsCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        public ICommand EditCustomerCommand { get; set; }

        public ICommand AddCommentsCommand { get; set; }//[Sudhir.jangra][GEOS2-4935][21/11/2023]

        public ICommand DeleteCommentRowCommand { get; set; }//[Sudhir.Jangra][GEOS2-4935]

        public ICommand CommentsGridDoubleClickCommand { get; set; }//[Sudhir.Jangra][GEOS2-4935]

        #endregion

        #region Constructor
        public AddEditDiscountsListGridViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor AddEditDiscountsListGridViewModel ...", category: Category.Info, priority: Priority.Low);
            //Directory.CreateDirectory(tempDirectory);
            DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
            DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<Views.SplashScreenView>(); }
            AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            DeleteArticleCommand = new DelegateCommand<object>(DeleteArticleCommandAction);
            ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameDescriptionByLanguge);
            ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
            ChangeValueCommand = new DelegateCommand<object>(ChangeValueCommandAction);
            ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
            ChangeStartDateCommand = new DelegateCommand(ChangeStartDateCommandAction);
            ChangeEndDateCommand = new DelegateCommand(ChangeEndDateCommandAction);
            ChangeScopeCommand = new DelegateCommand<object>(ChangeScopeCommandAction);
            ChangePlantCommand = new RelayCommand(new Action<object>(ChangePlantCommandAction));
            ItemListTableViewLoadedCommand = new DelegateCommand<object>(ItemListTableViewLoadedAction);
            CommandDropRecordArticleGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordArticleGrid);
            IsCheckedReadOnlyCommand = new DelegateCommand(IsCheckedReadOnlyCommandAction);
            CommandOnDragRecordOverArticleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticleGrid);
            CommandCompleteRecordDragDropArticleGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropArticleGrid);
            ExportToExcelDiscountLogsCommand = new DelegateCommand<object>(ExportToExcelDiscountLogs);
            AddCustomerCommand = new DelegateCommand<object>(AddCustomerCommandAction);
            DeleteCustomerCommand = new DelegateCommand<object>(DeleteCustomerCommandAction);
            EditCustomerCommand = new DelegateCommand<object>(EditCustomerCommandAction);

            AddCommentsCommand = new RelayCommand(new Action<object>(AddCommentsCommandAction));//[Sudhir.Jangra][GEOS2-4935]
            DeleteCommentRowCommand = new RelayCommand(new Action<object>(DeleteCommentRowCommandAction));//[Sudhir.Jangra][GEOS2-4935]
            CommentsGridDoubleClickCommand = new RelayCommand(new Action<object>(CommentDoubleClickCommandAction));//[Sudhir.Jangra][GEOS2-4935]

            IsEnabledValue = true;
            MyFilterString = string.Empty;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Constructor AddEditDiscountsListGridViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Methods

        private void IsCheckedReadOnlyCommandAction()
        {
            if (!IsCheckedReadOnly)
            {
                IsEnabledValue = true;
            }
        }
        public void Init(string SelectedHeader)
        {
            MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();
            WindowHeader = SelectedHeader;
            GetPlants();
            FillLanguages();
            FillScopeList();
            FillPlatformList();
            AddColumnsToDataTableWithoutBands();
            FillArticleMenuList();
            IsEnabledAcceptButton = true;
            StartDate = new DateTime(DateTime.Now.Year, 01, 01);
            LastUpdated = DateTime.Now;
            EndDate = new DateTime(DateTime.Now.Year, 12, 31);
            MyFilterString = string.Empty;
            DiscountArticles = new List<DiscountArticles>();
            CommentsList = new ObservableCollection<DiscountLogEntry>();

        }

        //[rdixit][28.09.2022][GEOS2-3101]
        public void EditInit(Discounts SelectedDiscounts)
        {
            MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();
            EditDiscount = new Discounts();
            Discounts temp = SelectedDiscounts;

            ClonedDiscount = (Discounts)SelectedDiscounts.Clone();
            GetChangelogs(temp.Id);
            GetCommentsList(temp.Id);
            //ClonedDiscount.DiscountCommentsList = CommentsList.ToList();
            // ClonedDiscountCommentsList = new List<DiscountLogEntry>();
            // ClonedDiscountCommentsList = CommentsList.ToList();
            ClonedDiscountCommentsList = CommentsList.Select(item => (DiscountLogEntry)item.Clone()).ToList();
            // List<MyClass> clonedList = originalList.Select(item => item.Clone()).ToList();
            GetPlants();
            FillLanguages();
            FillScopeList();
            FillPlatformList();
            AddColumnsToDataTableWithoutBands();
            FillArticleMenuList();
            IsEnabledAcceptButton = true;
            GetDiscountCustomerAll();
            MyFilterString = string.Empty;
            DiscountArticles = new List<DiscountArticles>();



            Description = temp.Description;
            oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
            Description_en = temp.Description;
            Description_es = temp.Description_es;
            Description_fr = temp.Description_fr;
            Description_pt = temp.Description_pt;
            Description_ro = temp.Description_ro;
            Description_ru = temp.Description_ru;
            Description_zh = temp.Description_zh;


            if (temp.StartDateNew != null)
                StartDate = temp.StartDateNew;
            else
                StartDate = new DateTime(DateTime.Now.Year, 01, 01);

            if (temp.StartDateNew != null)
                EndDate = temp.EndDateNew;
            else
                EndDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (temp.LastUpdateNew != null)
                LastUpdated = temp.LastUpdateNew;
            else
                LastUpdated = new DateTime(DateTime.Now.Year, 12, 31);

            Name = temp.Name;
            oldName = string.IsNullOrEmpty(Name) ? "" : Name;
            Name_en = temp.Name;
            Name_es = temp.Name_es;
            Name_fr = temp.Name_fr;
            Name_pt = temp.Name_pt;
            Name_ro = temp.Name_ro;
            Name_ru = temp.Name_ru;
            Name_zh = temp.Name_zh;

            if (SelectedScope != null)
                SelectedScope = new LookupValue();
            SelectedScope = ScopeList.Where(i => i.Value == temp.Scope).FirstOrDefault();

            if (SelectedPlatform != null)
                SelectedPlatform = new LookupValue();
            SelectedPlatform = PlatformList.Where(i => i.Value == temp.Platform).FirstOrDefault();

            if (SelectedPlant != null)
                SelectedPlant = new List<object>();

            if (temp.Plants.ToLower() == "all")
                SelectedPlant.AddRange(PlantList.ToList());
            else
                SelectedPlant.AddRange(PlantList.Where(i => temp.Plants.Split('\n').Contains(i.Name)).ToList());

            if (temp.InUse.ToLower() == "yes")
                IsInUse = true;
            else
                IsInUse = false;

            if (selectedScope.IdLookupValue != 1520)//[Order Scope]
            {
                IsCheckedReadOnly = temp.IsReadOnly;
                Value = temp.Value;
            }

            else//[Product Scope]
            {
                IsCheckedReadOnly = true;
                Value = 0;
                DiscountArticles = temp.DiscountArticles;
                if (DiscountArticles != null)
                {
                    foreach (DiscountArticles DiscountArticle in DiscountArticles)
                    {
                        if (DataTableForGridLayout == null)
                            DataTableForGridLayout = new DataTable();
                        DiscountArticle.Name = ArticleMenuList.Where(i => i.IdArticle == DiscountArticle.IdArticle).FirstOrDefault().Name;
                        PCMArticleCategory PCMArticle = ArticleMenuList.Where(i => i.IdArticle == DiscountArticle.IdArticle).FirstOrDefault();

                        DataRow dr = DataTableForGridLayout.NewRow();
                        dr["CheckBox"] = false;
                        dr["Reference"] = PCMArticle.Reference;
                        dr["Name"] = PCMArticle.Name;
                        dr["Category"] = PCMArticle.CategoryName;
                        dr["Value"] = DiscountArticle.Value;
                        dr["IdArticle"] = DiscountArticle.IdArticle;
                        ArticleMenuList.Where(a => a.IdArticle == DiscountArticle.IdArticle).ToList().ForEach(b => b.IsBPLArticle_Current = true);
                        DataTableForGridLayout.Rows.Add(dr);
                    }
                }
                DtArticle = DataTableForGridLayout;
            }


        }
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                if (IsEnabledCancelButton == true)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonCommandAction(null);
                    }
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                #region GEOS2-3132 Sudhir.Jangra 14/02/2023


                if (IsEnabledCancelButton == true)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonCommandAction(null);
                    }
                }
                #endregion
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeValueCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeValueCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((DevExpress.Xpf.Grid.CellValueEventArgs)obj).Column.FieldName == "Value")
                {
                    DataRow MainRow = ((System.Data.DataRowView)((DevExpress.Xpf.Grid.RowEventArgs)obj).Row).Row;
                    DiscountArticles.Where(i => i.IdArticle == Convert.ToUInt64(MainRow["IdArticle"])).FirstOrDefault().Value = Convert.ToDouble(MainRow["Value"].ToString());
                    DataTableForGridLayout.AcceptChanges();
                    GeosApplication.Instance.Logger.Log("Method ChangeValueCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ChangeValueCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][28.09.2022][GEOS2-3101]
        public void ChangePlantCommandAction(object obj)
        {
            try
            {
                if (SelectedPlant.Count == 0)
                    return;

                List<string> Plant = new List<string>();
                if (ClonedDiscount.Plants.ToLower() == "all")
                    Plant = PlantList.Select(j => j.Name).ToList();
                else
                    Plant.AddRange(ClonedDiscount.Plants.Split('\n'));

                SelectedPlantList = new ObservableCollection<Site>();
                foreach (Site item in SelectedPlant)
                {
                    SelectedPlantList.Add(item);
                }
                var plantdeletedCount = Plant.Except(SelectedPlantList.Select(j => j.Name).ToList()).ToList();
                if (SelectedPlant.Count() < Plant.Count() || plantdeletedCount.Count > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDiscountArticlePlant"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.No)
                    {
                        if (SelectedPlant != null)
                            SelectedPlant = new List<object>();
                        SelectedPlant.AddRange(PlantList.Where(i => Plant.Contains(i.Name)).ToList());
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][28.09.2022][GEOS2-3101]
        public void ChangeScopeCommandAction(object obj)
        {
            if (ClonedDiscount != null)
                if (SelectedScope.IdLookupValue != 1520 && ClonedDiscount.Scope.ToLower() != SelectedScope.Value.ToLower() && ClonedDiscount.DiscountArticles != null)
                {

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ScopeChangedFromProductToOrder"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.No)
                    {
                        SelectedScope = ScopeList.Where(i => i.Value.ToLower() == ClonedDiscount.Scope.ToLower()).FirstOrDefault();
                        return;
                    }
                }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                InformationError = null;
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";

                if (error != null)
                {
                    return;
                }

                if (SelectedPlant.Count == 0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PlantMandatoryValidationDiscount").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...atleast one plant.", category: Category.Info, priority: Priority.Low);
                    return;
                }
                if (StartDate > EndDate)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("StartAndEndDateValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...End date must be equal or greater than Start date.", category: Category.Info, priority: Priority.Low);
                    return;
                }

                #region Add_Discount         
                if (IsNew)
                {
                    NewDiscount = new Discounts();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<Views.SplashScreenView>(); }
                    NewDiscount.Name = Name_en == null ? "" : Name_en;
                    NewDiscount.Name_es = Name_es == null ? "" : Name_es;
                    NewDiscount.Name_fr = Name_fr == null ? "" : Name_fr;
                    NewDiscount.Name_pt = Name_pt == null ? "" : Name_pt;
                    NewDiscount.Name_ro = Name_ro == null ? "" : Name_ro;
                    NewDiscount.Name_ru = Name_ru == null ? "" : Name_ru;
                    NewDiscount.Name_zh = Name_zh == null ? "" : Name_zh;
                    NewDiscount.Description = Description_en == null ? "" : Description_en;
                    NewDiscount.Description_es = Description_es == null ? "" : Description_es;
                    NewDiscount.Description_fr = Description_fr == null ? "" : Description_fr;
                    NewDiscount.Description_pt = Description_pt == null ? "" : Description_pt;
                    NewDiscount.Description_ro = Description_ro == null ? "" : Description_ro;
                    NewDiscount.Description_ru = Description_ru == null ? "" : Description_ru;
                    NewDiscount.Description_zh = Description_zh == null ? "" : Description_zh;
                    NewDiscount.Scope = selectedScope.Value;
                    NewDiscount.IdScope = SelectedScope.IdLookupValue;
                    NewDiscount.IdPlatform = SelectedPlatform.IdLookupValue;
                    NewDiscount.PlantList = new List<Site>();
                    NewDiscount.Platform = SelectedPlatform.Value;
                    foreach (Site site in SelectedPlant)
                    {
                        NewDiscount.PlantList.Add(site);
                    }
                    if (IsInUse)
                        NewDiscount.InUse = "Yes";
                    else
                        NewDiscount.InUse = "No";
                    NewDiscount.StartDate = StartDate.ToString("yyyy/MM/dd HH:mm:ss");
                    NewDiscount.EndDate = EndDate.ToString("yyyy/MM/dd HH:mm:ss");
                    if (NewDiscount.IdScope != 1520)
                    {
                        NewDiscount.IsReadOnly = IsCheckedReadOnly;
                        NewDiscount.Value = Value;
                        NewDiscount.DiscountArticles = null;
                    }
                    else
                    {
                        NewDiscount.IsReadOnly = true;
                        NewDiscount.Value = 0;
                        NewDiscount.DiscountArticles = DiscountArticles;
                    }
                    NewDiscount.CreationDate = DateTime.Now;
                    NewDiscount.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                    NewDiscount.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                    if (DataTableForGridLayout == null)
                        DataTableForGridLayout = new DataTable();
                    if (NewDiscount.NewDiscountCustomer == null)//[GEOS2-3102][03.10.2022][rdixit]
                    {
                        NewDiscount.NewDiscountCustomer = new List<Data.Common.PCM.DiscountCustomers>();
                        NewDiscount.NewDiscountCustomer = DiscountCustomers;
                    }
                    /*Service Method To Add Discount*/
                    DiscountLogEntry discountLogEntry = new DiscountLogEntry();
                    discountLogEntry.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                    discountLogEntry.UserName = GeosApplication.Instance.ActiveUser.FullName;
                    discountLogEntry.Datetime = GeosApplication.Instance.ServerDateTime;
                    discountLogEntry.Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLog").ToString(), NewDiscount.Name, DisccountOldName);
                    DiscountChangeLogList = new ObservableCollection<DiscountLogEntry>();
                    DiscountChangeLogList.Add(discountLogEntry);

                    #region GEOS2-4935
                    //foreach (var item in CommentsList)
                    //{
                    //    DiscountChangeLogList.Add(new DiscountLogEntry()
                    //    {
                    //        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                    //        Datetime = GeosApplication.Instance.ServerDateTime,
                    //        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerPriceChangeLogForComment").ToString(),
                    //           item.Comments)

                    //    });
                    //}

                    #endregion
                    NewDiscount.DiscountCommentsList = CommentsList.ToList();
                    NewDiscount.DiscountCommentsList.ForEach(x => x.People.OwnerImage = null);


                    NewDiscount.DiscountLogEntryList = DiscountChangeLogList.ToList();
                    NewDiscount.DiscountArticles = new List<DiscountArticles>();
                    // NewDiscount = PCMService.AddDiscount(NewDiscount);

                    //[Sudhir.Jangra][GEOS2-4935]
                    NewDiscount = PCMService.AddDiscount_V2470(NewDiscount);



                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DiscountAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    IsNewDiscountSave = true;

                    RequestClose(null, null);
                }
                #endregion
                //[rdixit][28.09.2022][GEOS2-3101]
                #region Edit_Discount 
                else
                {
                    NewDiscountChangeLogList = new List<DiscountLogEntry>();
                    #region Add_Delete_Plant
                    List<string> Plant = new List<string>();
                    if (ClonedDiscount.Plants.ToLower() == "all")
                        Plant = PlantList.Select(j => j.Name).ToList();
                    else
                        Plant.AddRange(ClonedDiscount.Plants.Split('\n'));
                    SelectedPlantList = new ObservableCollection<Site>();
                    foreach (Site site in SelectedPlant)
                    {
                        SelectedPlantList.Add(site);
                        if (Plant.Any(i => i == site.Name))
                        { }
                        else
                        {
                            if (EditDiscount.AddedPlantList == null)
                                EditDiscount.AddedPlantList = new List<Site>();
                            EditDiscount.AddedPlantList.Add(site);
                        }
                    }
                    foreach (var item in Plant)
                    {
                        if (SelectedPlantList.Any(i => i.Name == item))
                        {

                        }
                        else
                        {
                            if (EditDiscount.DeletedPlantList == null)
                                EditDiscount.DeletedPlantList = new List<Site>();
                            EditDiscount.DeletedPlantList.Add((PlantList.Where(j => j.Name == item).FirstOrDefault()));
                        }
                    }
                    #endregion
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<Views.SplashScreenView>(); }
                    EditDiscount.Id = ClonedDiscount.Id;
                    EditDiscount.Name = Name_en == null ? "" : Name_en;
                    EditDiscount.Name_es = Name_es == null ? "" : Name_es;
                    EditDiscount.Name_fr = Name_fr == null ? "" : Name_fr;
                    EditDiscount.Name_pt = Name_pt == null ? "" : Name_pt;
                    EditDiscount.Name_ro = Name_ro == null ? "" : Name_ro;
                    EditDiscount.Name_ru = Name_ru == null ? "" : Name_ru;
                    EditDiscount.Name_zh = Name_zh == null ? "" : Name_zh;
                    EditDiscount.Description = Description_en == null ? "" : Description_en;
                    EditDiscount.Description_es = Description_es == null ? "" : Description_es;
                    EditDiscount.Description_fr = Description_fr == null ? "" : Description_fr;
                    EditDiscount.Description_pt = Description_pt == null ? "" : Description_pt;
                    EditDiscount.Description_ro = Description_ro == null ? "" : Description_ro;
                    EditDiscount.Description_ru = Description_ru == null ? "" : Description_ru;
                    EditDiscount.Description_zh = Description_zh == null ? "" : Description_zh;
                    EditDiscount.Scope = selectedScope.Value;
                    EditDiscount.IdScope = SelectedScope.IdLookupValue;
                    EditDiscount.IdPlatform = SelectedPlatform.IdLookupValue;
                    EditDiscount.PlantList = new List<Site>();
                    EditDiscount.Platform = SelectedPlatform.Value;
                    foreach (Site site in SelectedPlant)
                    {
                        EditDiscount.PlantList.Add(site);
                    }
                    if (IsInUse)
                        EditDiscount.InUse = "Yes";
                    else
                        EditDiscount.InUse = "No";
                    EditDiscount.StartDateNew = Convert.ToDateTime(StartDate);
                    EditDiscount.EndDateNew = Convert.ToDateTime(EndDate);
                    if (EditDiscount.IdScope != 1520)
                    {
                        EditDiscount.IsReadOnly = IsCheckedReadOnly;
                        EditDiscount.Value = Value;
                        EditDiscount.DiscountArticles = null;
                    }
                    else
                    {
                        EditDiscount.IsReadOnly = true;
                        EditDiscount.Value = 0;
                        EditDiscount.DiscountArticles = DiscountArticles;
                    }
                    EditDiscount.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                    EditDiscount.LastUpdateNew = DateTime.Now;
                    if (DataTableForGridLayout == null)
                        DataTableForGridLayout = new DataTable();

                    EditDiscount.UpdateDiscountCustomer = new List<Data.Common.PCM.DiscountCustomers>();

                    #region Customer_CRUD
                    // Delete Customer [GEOS2-3102][03.10.2022][rdixit]
                    foreach (DiscountCustomers item in DiscountCustomersClone)
                    {
                        if (DiscountCustomers != null && !DiscountCustomers.Any(x => x.IdCustomerDiscountCustomer == item.IdCustomerDiscountCustomer))
                        {
                            DiscountCustomers DiscountCustomer = (DiscountCustomers)item.Clone();
                            DiscountCustomer.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            EditDiscount.UpdateDiscountCustomer.Add(DiscountCustomer);
                            NewDiscountChangeLogList.Add(new DiscountLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountCustomerChangeLogDelete").ToString(),
                                DiscountCustomer.GroupName, DiscountCustomer.RegionName,
                                DiscountCustomer.Country.Name, DiscountCustomer.Plant.Name)
                            });
                        }
                    }

                    //Added Customer [GEOS2-3102][03.10.2022][rdixit]
                    if (DiscountCustomers != null)
                    {
                        foreach (DiscountCustomers item in DiscountCustomers)
                        {
                            if (!DiscountCustomersClone.Any(x => x.IdCustomerDiscountCustomer == item.IdCustomerDiscountCustomer))
                            {

                                DiscountCustomers DiscountCustomer = (DiscountCustomers)item.Clone();
                                DiscountCustomer.TransactionOperation = ModelBase.TransactionOperations.Add;
                                EditDiscount.UpdateDiscountCustomer.Add(DiscountCustomer);
                                NewDiscountChangeLogList.Add(new DiscountLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountCustomerChangeLogAdd").ToString(),
                                    DiscountCustomer.GroupName, DiscountCustomer.RegionName,
                                    DiscountCustomer.Country.Name, DiscountCustomer.Plant.Name)
                                });
                            }
                        }
                    }

                    //Updated Customer [GEOS2-3102][03.10.2022][rdixit]
                    foreach (DiscountCustomers originalCustomer in DiscountCustomersClone)
                    {
                        if (DiscountCustomers != null && DiscountCustomers.Any(x => x.IdCustomerDiscountCustomer == originalCustomer.IdCustomerDiscountCustomer))
                        {
                            DiscountCustomers DiscountCustomerUpdated = DiscountCustomers.FirstOrDefault(x => x.IdCustomerDiscountCustomer == originalCustomer.IdCustomerDiscountCustomer);
                            if ((DiscountCustomerUpdated.IdGroup != originalCustomer.IdGroup) || (DiscountCustomerUpdated.IdRegion != originalCustomer.IdRegion) || (DiscountCustomerUpdated.IdCountry != originalCustomer.IdCountry) || (DiscountCustomerUpdated.IdPlant != originalCustomer.IdPlant))
                            {
                                DiscountCustomers Customer = (DiscountCustomers)DiscountCustomerUpdated.Clone();
                                Customer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                Customer.TransactionOperation = ModelBase.TransactionOperations.Update;
                                EditDiscount.UpdateDiscountCustomer.Add(Customer);
                                NewDiscountChangeLogList.Add(new DiscountLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountCustomerChangeLogUpdate").ToString(),
                                    originalCustomer.GroupName, originalCustomer.RegionName,
                                    originalCustomer.Country.Name, originalCustomer.Plant.Name,
                                    Customer.GroupName, Customer.RegionName,
                                    Customer.Country.Name, Customer.Plant.Name)
                                });
                            }
                        }
                    }
                    #endregion
                    /*Service Method To Add Discount*/
                    #region Change_Log_Insert
                    //if (DiscountChangeLogList!=null)
                    //{                        
                    //    DiscountChangeLogList.Clear();
                    //}
                    if (ClonedDiscount.Name != EditDiscount.Name)
                    {
                        if (string.IsNullOrEmpty(EditDiscount.Name))
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogName").ToString(), ClonedDiscount.Name, "None") });
                        else
                        {
                            if (string.IsNullOrEmpty(ClonedDiscount.Name))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogName").ToString(), "None", EditDiscount.Name) });
                            else
                                NewDiscountChangeLogList.Add(new DiscountLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogName").ToString(),
                                    ClonedDiscount.Name,
                                    EditDiscount.Name)
                                });
                        }
                    }
                    if (ClonedDiscount.Name_es != EditDiscount.Name_es)
                    {
                        if (!(ClonedDiscount.Name_es == null && EditDiscount.Name_es == "")) //[pjadhav][15.10.2022]
                        {

                            if (string.IsNullOrEmpty(EditDiscount.Name_es))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameES").ToString(), ClonedDiscount.Name_es, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Name_es))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameES").ToString(), "None", EditDiscount.Name_es) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameES").ToString(), ClonedDiscount.Name_es, EditDiscount.Name_es) });
                            }
                        }
                    }
                    if (ClonedDiscount.Name_fr != EditDiscount.Name_fr)
                    {
                        if (!(ClonedDiscount.Name_fr == null && EditDiscount.Name_fr == "")) //[pjadhav][15.10.2022]
                        {

                            if (string.IsNullOrEmpty(EditDiscount.Name_fr))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameFR").ToString(), ClonedDiscount.Name_fr, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Name_fr))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameFR").ToString(), "None", EditDiscount.Name_fr) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameFR").ToString(), ClonedDiscount.Name_fr, EditDiscount.Name_fr) });
                            }
                        }
                    }
                    if (ClonedDiscount.Name_pt != EditDiscount.Name_pt)
                    {
                        if (!(ClonedDiscount.Name_pt == null && EditDiscount.Name_pt == "")) //[pjadhav][15.10.2022]
                        {

                            if (string.IsNullOrEmpty(EditDiscount.Name_pt))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNamePT").ToString(), ClonedDiscount.Name_pt, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Name_pt))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNamePT").ToString(), "None", EditDiscount.Name_pt) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNamePT").ToString(), ClonedDiscount.Name_pt, EditDiscount.Name_pt) });
                            }
                        }
                    }
                    if (ClonedDiscount.Name_ro != EditDiscount.Name_ro)
                    {
                        if (!(ClonedDiscount.Name_ro == null && EditDiscount.Name_ro == "")) //[pjadhav][15.10.2022]
                        {

                            if (string.IsNullOrEmpty(EditDiscount.Name_ro))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameRO").ToString(), ClonedDiscount.Name_ro, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Name_ro))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameRO").ToString(), "None", EditDiscount.Name_ro) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameRO").ToString(), ClonedDiscount.Name_ro, EditDiscount.Name_ro) });
                            }
                        }
                    }
                    if (ClonedDiscount.Name_ru != EditDiscount.Name_ru)
                    {
                        if (!(ClonedDiscount.Name_ru == null && EditDiscount.Name_ru == ""))//[pjadhav][15.10.2022]
                        {

                            if (string.IsNullOrEmpty(EditDiscount.Name_ru))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameRU").ToString(), ClonedDiscount.Name_ru, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Name_ru))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameRU").ToString(), "None", EditDiscount.Name_ru) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameRU").ToString(), ClonedDiscount.Name_ru, EditDiscount.Name_ru) });
                            }
                        }
                    }
                    if (ClonedDiscount.Name_zh != EditDiscount.Name_zh)
                    {

                        if (!(ClonedDiscount.Name_zh == null && EditDiscount.Name_zh == ""))//[pjadhav][15.10.2022]
                        {
                            if (string.IsNullOrEmpty(EditDiscount.Name_zh))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameZH").ToString(), ClonedDiscount.Name_zh, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Name_zh))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameZH").ToString(), "None", EditDiscount.Name_zh) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNameZH").ToString(), ClonedDiscount.Name_zh, EditDiscount.Name_zh) });
                            }
                        }
                    }
                    if (ClonedDiscount.Id != EditDiscount.Id)
                    {
                        Discounts discount_edit = DiscountsList.FirstOrDefault(x => x.Id == EditDiscount.Id);
                        if (discount_edit != null)
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogNames").ToString(), ClonedDiscount.Name, discount_edit.Name) });
                    }

                    if (ClonedDiscount.Description != EditDiscount.Description)
                    {


                        if (string.IsNullOrEmpty(EditDiscount.Description))
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription").ToString(), ClonedDiscount.Description, "None") });
                        else
                        {
                            if (string.IsNullOrEmpty(ClonedDiscount.Description))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription").ToString(), "None", EditDiscount.Description) });
                            else
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription").ToString(), ClonedDiscount.Description, EditDiscount.Description) });
                        }
                    }
                    if (ClonedDiscount.Description_es != EditDiscount.Description_es)
                    {
                        if (!(ClonedDiscount.Description_es == null && EditDiscount.Description_es == "")) //[pjadhav][15.10.2022]
                        {
                            if (string.IsNullOrEmpty(EditDiscount.Description_es))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_es").ToString(), ClonedDiscount.Description_es, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Description_es))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_es").ToString(), "None", EditDiscount.Description_es) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_es").ToString(), ClonedDiscount.Description_es, EditDiscount.Description_es) });
                            }
                        }
                    }
                    if (ClonedDiscount.Description_fr != EditDiscount.Description_fr)
                    {
                        if (!(ClonedDiscount.Description_fr == null && EditDiscount.Description_fr == ""))//[pjadhav][15.10.2022]
                        {
                            if (string.IsNullOrEmpty(EditDiscount.Description_fr))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_fr").ToString(), ClonedDiscount.Description_fr, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Description_fr))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_fr").ToString(), "None", EditDiscount.Description_fr) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_fr").ToString(), ClonedDiscount.Description_fr, EditDiscount.Description_fr) });
                            }
                        }
                    }
                    if (ClonedDiscount.Description_pt != EditDiscount.Description_pt)
                    {
                        if (!(ClonedDiscount.Description_pt == null && EditDiscount.Description_pt == ""))//[pjadhav][15.10.2022]
                        {

                            if (string.IsNullOrEmpty(EditDiscount.Description_pt))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_pt").ToString(), ClonedDiscount.Description_pt, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Description_pt))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_pt").ToString(), "None", EditDiscount.Description_pt) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_pt").ToString(), ClonedDiscount.Description_pt, EditDiscount.Description_pt) });
                            }
                        }
                    }
                    if (ClonedDiscount.Description_ro != EditDiscount.Description_ro)
                    {
                        if (!(ClonedDiscount.Description_ro == null && EditDiscount.Description_ro == ""))//[pjadhav][15.10.2022]
                        {
                            if (string.IsNullOrEmpty(EditDiscount.Description_ro))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_ro").ToString(), ClonedDiscount.Description_ro, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Description_ro))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_ro").ToString(), "None", EditDiscount.Description_ro) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_ro").ToString(), ClonedDiscount.Description_ro, EditDiscount.Description_ro) });
                            }
                        }
                    }
                    if (ClonedDiscount.Description_ru != EditDiscount.Description_ru)
                    {
                        if (!(ClonedDiscount.Description_ru == null && EditDiscount.Description_ru == ""))//[pjadhav][15.10.2022]
                        {
                            if (string.IsNullOrEmpty(EditDiscount.Description_ru))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_ru").ToString(), ClonedDiscount.Description_ru, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Description_ru))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_ru").ToString(), "None", EditDiscount.Description_ru) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_ru").ToString(), ClonedDiscount.Description_ru, EditDiscount.Description_ru) });
                            }
                        }
                    }
                    if (ClonedDiscount.Description_zh != EditDiscount.Description_zh)
                    {
                        if (!(ClonedDiscount.Description_zh == null && EditDiscount.Description_zh == ""))//[pjadhav][15.10.2022]
                        {
                            if (string.IsNullOrEmpty(EditDiscount.Description_zh))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_zh").ToString(), ClonedDiscount.Description_zh, "None") });
                            else
                            {
                                if (string.IsNullOrEmpty(ClonedDiscount.Description_zh))
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_zh").ToString(), "None", EditDiscount.Description_zh) });
                                else
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogDescription_zh").ToString(), ClonedDiscount.Description_zh, EditDiscount.Description_zh) });
                            }
                        }
                    }
                    if (ClonedDiscount.Scope.ToLower() != EditDiscount.Scope.ToLower())
                    {

                        NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogScope").ToString(), ClonedDiscount.Scope, EditDiscount.Scope) });
                    }



                    #region Selected_Plant_ChangeLog
                    if (SelectedPlant != null && ClonedDiscount.Plants != null)
                    {

                        if (EditDiscount.AddedPlantList != null)
                        {
                            foreach (var item in EditDiscount.AddedPlantList)
                            {
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogPlantAdd").ToString(), ClonedDiscount.Plants, item.Name) });
                            }
                        }
                        if (EditDiscount.DeletedPlantList != null)
                        {
                            foreach (var item in EditDiscount.DeletedPlantList)
                            {
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogPlantDeleted").ToString(), ClonedDiscount.Plants, item.Name) });
                            }
                        }


                    }
                    #endregion

                    if (ClonedDiscount.Value != EditDiscount.Value)
                    {

                        if (string.IsNullOrEmpty(Convert.ToString(EditDiscount.Value)))
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogValue").ToString(), ClonedDiscount.Value, "None") });
                        else
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedDiscount.Value)))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogValue").ToString(), "None", EditDiscount.Value) });
                            else
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogValue").ToString(), ClonedDiscount.Value, EditDiscount.Value) });
                        }
                    }
                    if (Convert.ToString(ClonedDiscount.StartDateNew) != Convert.ToString(EditDiscount.StartDateNew))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(EditDiscount.StartDateNew)))
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogStartDate").ToString(), ClonedDiscount.StartDateNew.ToShortDateString(), "None") });
                        else
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedDiscount.StartDateNew)))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogStartDate").ToString(), "None", EditDiscount.StartDateNew.ToShortDateString()) });
                            else
                                NewDiscountChangeLogList.Add(new DiscountLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogStartDate").ToString(), ClonedDiscount.StartDateNew.ToShortDateString(), EditDiscount.StartDateNew.ToShortDateString())
                                });
                        }
                    }
                    if (Convert.ToString(ClonedDiscount.EndDateNew) != Convert.ToString(EditDiscount.EndDateNew))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(EditDiscount.EndDateNew)))
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogEndDate").ToString(), ClonedDiscount.EndDateNew.ToShortDateString(), "None") });
                        else
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedDiscount.EndDateNew)))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogEndDate").ToString(), "None", EditDiscount.EndDateNew.ToShortDateString()) });
                            else
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogEndDate").ToString(), ClonedDiscount.EndDateNew.ToShortDateString(), EditDiscount.EndDateNew.ToShortDateString()) });
                        }
                    }

                    if (ClonedDiscount.InUse.ToLower() != EditDiscount.InUse.ToLower())
                    {
                        if (string.IsNullOrEmpty(EditDiscount.InUse))
                            NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogInUse").ToString(), ClonedDiscount.InUse, "None") });
                        else
                        {
                            if (string.IsNullOrEmpty(ClonedDiscount.InUse))
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogInUse").ToString(), "None", EditDiscount.InUse) });
                            else
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DiscountChangeLogInUse").ToString(), ClonedDiscount.InUse, EditDiscount.InUse) });
                        }
                    }
                    //[rdixit][12.10.2022][GEOS2-3967]
                    #region Article_Change_Log 
                    AddedDiscountArticles = new List<Data.Common.PCM.DiscountArticles>();

                    DeletedDiscountArticles = new List<Data.Common.PCM.DiscountArticles>();

                    if (ClonedDiscount.Scope.ToLower() == "product" && EditDiscount.Scope.ToLower() == "order")
                    {
                        if (ClonedDiscount.DiscountArticles != null)
                        {
                            foreach (var item in ClonedDiscount.DiscountArticles)
                            {
                                DiscountArticles temp = ClonedDiscount.DiscountArticles.Where(i => i.IdArticle == item.IdArticle).FirstOrDefault();
                                temp.Name = ArticleMenuList.Where(j => j.IdArticle == item.IdArticle).First().Name;
                                DeletedDiscountArticles.Add(ClonedDiscount.DiscountArticles.Where(i => i.IdArticle == item.IdArticle).FirstOrDefault());
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountChangeLogArticleGridRemoved").ToString(), temp.Name) });
                            }
                        }
                    }

                    if (EditDiscount.DiscountArticles != null)
                    {
                        foreach (var item in EditDiscount.DiscountArticles)
                        {
                            if (ClonedDiscount.Scope.ToLower() == "order" && EditDiscount.Scope.ToLower() == "product" || ClonedDiscount.DiscountArticles == null)
                            {
                                AddedDiscountArticles.Add(item);
                                NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountChangeLogArticleGridAdd").ToString(), item.Name) });
                            }
                            else
                            {
                                if ((!ClonedDiscount.DiscountArticles.Any(i => i.IdArticle == item.IdArticle)))
                                {
                                    AddedDiscountArticles.Add(item);
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountChangeLogArticleGridAdd").ToString(), item.Name) });
                                }
                            }
                        }

                        if (ClonedDiscount.DiscountArticles != null)
                        {
                            foreach (var item in ClonedDiscount.DiscountArticles)
                            {
                                if (!EditDiscount.DiscountArticles.Any(i => i.IdArticle == item.IdArticle))
                                {
                                    DiscountArticles temp = ClonedDiscount.DiscountArticles.Where(i => i.IdArticle == item.IdArticle).FirstOrDefault();
                                    temp.Name = ArticleMenuList.Where(j => j.IdArticle == item.IdArticle).First().Name;
                                    DeletedDiscountArticles.Add(ClonedDiscount.DiscountArticles.Where(i => i.IdArticle == item.IdArticle).FirstOrDefault());
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountChangeLogArticleGridRemoved").ToString(), temp.Name) });
                                }
                            }
                        }

                        foreach (var item in EditDiscount.DiscountArticles)
                        {
                            if (!AddedDiscountArticles.Any(i => i.IdArticle == item.IdArticle) && !DeletedDiscountArticles.Any(i => i.IdArticle == item.IdArticle))
                            {
                                DiscountArticles temp = new Data.Common.PCM.DiscountArticles();
                                temp = ClonedDiscount.DiscountArticles.Where(i => i.IdArticle == item.IdArticle).FirstOrDefault();
                                if (item.Value != temp.Value)
                                    NewDiscountChangeLogList.Add(new DiscountLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("EditDiscountChangeLogArticleGridvalue").ToString(), temp.Value, item.Value, item.Name) });
                            }
                        }
                    }
                    #endregion



                    #endregion

                    #region GEOS2-4935 [Sudhir.Jangra]
                    EditDiscount.DiscountCommentsList = new List<DiscountLogEntry>();
                    //Deleted Comments
                    foreach (DiscountLogEntry itemComments in ClonedDiscountCommentsList)
                    {
                        if (!CommentsList.Any(x => x.IdLogEntryByDiscount == itemComments.IdLogEntryByDiscount) && itemComments.IdLogEntryByDiscount != 0)
                        {
                            DiscountLogEntry comments = (DiscountLogEntry)itemComments.Clone();
                            comments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            EditDiscount.DiscountCommentsList.Add(comments);

                            //NewDiscountChangeLogList.Add(new DiscountLogEntry()
                            //{
                            //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                            //    Datetime = GeosApplication.Instance.ServerDateTime,
                            //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                            //                    " " + GeosApplication.Instance.ActiveUser.LastName,
                            //    Comments = string.Format(System.Windows.Application.Current.FindResource("BasePriceChangeLogForCommentDeleted").ToString(),
                            //                   comments.Comments)
                            //});


                        }
                    }

                    //Added Comments
                    foreach (DiscountLogEntry itemComments in CommentsList)
                    {
                        if (!ClonedDiscountCommentsList.Any(x => x.IdLogEntryByDiscount == itemComments.IdLogEntryByDiscount))
                        {
                            DiscountLogEntry comments = (DiscountLogEntry)itemComments.Clone();
                            comments.TransactionOperation = ModelBase.TransactionOperations.Add;
                            EditDiscount.DiscountCommentsList.Add(comments);
                            //NewDiscountChangeLogList.Add(new DiscountLogEntry()
                            //{
                            //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                            //    Datetime = GeosApplication.Instance.ServerDateTime,
                            //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                            //                   " " + GeosApplication.Instance.ActiveUser.LastName,
                            //    Comments = string.Format(System.Windows.Application.Current.FindResource("BasePriceChangeLogForComment").ToString(),
                            //         comments.Comments)
                            //});
                        }
                    }


                    //Update Comments
                    foreach (DiscountLogEntry originalComments in ClonedDiscountCommentsList)
                    {
                        if (CommentsList.Any(x => x.IdLogEntryByDiscount == originalComments.IdLogEntryByDiscount))
                        {
                            DiscountLogEntry commentsUpdated = CommentsList.FirstOrDefault(x => x.IdLogEntryByDiscount == originalComments.IdLogEntryByDiscount);
                            if (commentsUpdated.Comments != originalComments.Comments)
                            {
                                string originalComment;
                                if (originalComments.Comments == null)
                                {
                                    originalComment = string.Empty;
                                }
                                else
                                {
                                    originalComment = originalComments.Comments;
                                }
                                DiscountLogEntry comments = (DiscountLogEntry)commentsUpdated.Clone();
                                comments.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                comments.ModifiedDate = DateTime.Now;
                                comments.Datetime = DateTime.Now;
                                comments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                EditDiscount.DiscountCommentsList.Add(comments);


                                //using (var productTypeCommentEntry = new DiscountLogEntry
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //          " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("BasePriceChangeLogForCommentUpdate").ToString(),
                                // originalComments.Comments, originalComments.Comments, comments.Comments)
                                //})
                                //{
                                //    NewDiscountChangeLogList.Add(productTypeCommentEntry);
                                //}


                            }
                        }
                    }
                    #endregion




                    EditDiscount.DiscountLogEntryList = null;
                    EditDiscount.DiscountLogEntryList = new List<DiscountLogEntry>();
                    EditDiscount.DiscountLogEntryList.AddRange(NewDiscountChangeLogList);


                    EditDiscount.DiscountCommentsList.ForEach(x => x.People.OwnerImage = null);
                    // ClonedDiscount.DiscountCommentsList.ForEach(x => x.People.OwnerImage = null);
                    // EditDiscount = PCMService.UpdateDiscount(EditDiscount, ClonedDiscount);

                    //[Sudhir.Jangra][GEOs2-4935]
                    EditDiscount = PCMService.UpdateDiscount_V2470(EditDiscount, ClonedDiscount);


                    // DiscountChangeLogList.ToList().AddRange((NewDiscountChangeLogList));
                    DiscountChangeLogList.ToList().AddRange((NewDiscountChangeLogList));
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DiscountUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    IsDiscountUpdated = true;
                    RequestClose(null, null);
                }
                #endregion

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);
                PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                SelectedPlant = new List<object>();
                if (PlantList != null)
                {
                    SelectedPlant.Add(PlantList.FirstOrDefault());
                }

                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguages()...", category: Category.Info, priority: Priority.Low);
                Languages = new ObservableCollection<Language>(PLMService.GetLanguages());
                LanguageSelected = Languages.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillLanguages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillScopeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AddEditDiscountsListGridViewModel Method  FillScopeList()...", category: Category.Info, priority: Priority.Low);
                ScopeList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(78).AsEnumerable());
                SelectedScope = ScopeList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("AddEditDiscountsListGridViewModel Method FillScopeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEditDiscountsListGridViewModel Method FillLeavesTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillPlatformList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AddEditDiscountsListGridViewModel Method  FillPlatformList()...", category: Category.Info, priority: Priority.Low);
                PlatformList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(79).AsEnumerable());
                SelectedPlatform = PlatformList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("AddEditDiscountsListGridViewModel Method FillPlatformList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEditDiscountsListGridViewModel Method FillPlatformList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddColumnsToDataTableWithoutBands()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                BandItem band1 = new BandItem() { BandName = "all", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band1.Columns = new ObservableCollection<ColumnItem>();
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "CheckBox", HeaderText = " ", Width = 30, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Reference", HeaderText = "Reference", Width = 100, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Reference, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Name", HeaderText = "Name", Width = 350, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Category", HeaderText = "Category", Width = 100, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Value", HeaderText = "Value", Width = 150, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Value, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Delete", HeaderText = "Delete", Width = 150, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdArticle", HeaderText = "IdArticle", Width = 0, IsVertical = false, DiscountArticleSettings = DiscountArticleSettingsType.Default, Visible = false });
                Bands.Add(band1);
                DataTableForGridLayout = new DataTable();

                DataTableForGridLayout.Columns.Add("CheckBox", typeof(string));
                DataTableForGridLayout.Columns.Add("Reference", typeof(string));
                DataTableForGridLayout.Columns.Add("Name", typeof(string));
                DataTableForGridLayout.Columns.Add("Category", typeof(string));
                DataTableForGridLayout.Columns.Add("Value", typeof(string));
                DataTableForGridLayout.Columns.Add("Delete", typeof(string));
                DataTableForGridLayout.Columns.Add("IdArticle", typeof(string));
                DataTable = DataTableForGridLayout;
                Bands = new ObservableCollection<BandItem>(Bands);
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
        private void ItemListTableViewLoadedAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new Point(20, 180),
                Size = new Size(250, 250)
            };
        }
        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Name_en = Name;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Name_es = Name;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Name_fr = Name;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Name_pt = Name;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Name_ro = Name;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Name_ru = Name;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Name_zh = Name;
                }
                if (oldName == null)
                {
                    oldName = Name;
                }
                else
                {
                    if (oldName != Name)
                    {
                        oldName = Name;
                        IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                    }
                    else
                    {
                        IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetNameToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage..."), category: Category.Info, priority: Priority.Low);
                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Description_en = Description;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Description_es = Description;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Description_fr = Description;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Description_pt = Description;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Description_ro = Description;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Description_ru = Description;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Description_zh = Description;
                }
                if (oldDescription == null)
                {
                    oldDescription = Description;
                }
                else
                {
                    if (oldDescription != Description)
                    {
                        oldDescription = Description;
                        IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                    }
                    else
                    {
                        IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                    }
                }


                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RetrieveNameDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()...", category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Description = Description_en;
                    Name = Name_en;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Description = Description_es;
                    Name = Name_es;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Description = Description_fr;
                    Name = Name_fr;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Description = Description_pt;
                    Name = Name_pt;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Description = Description_ro;
                    Name = Name_ro;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Description = Description_ru;
                    Name = Name_ru;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Description = Description_zh;
                    Name = Name_zh;
                }

                if (oldSelectedLanguage == null)
                {
                    oldSelectedLanguage = LanguageSelected.TwoLetterISOLanguage;
                }
                else
                {
                    if (oldSelectedLanguage != LanguageSelected.TwoLetterISOLanguage)
                    { oldSelectedLanguage = LanguageSelected.TwoLetterISOLanguage; }
                }
                IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveDescriptionByLanguge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeStartDateCommandAction()
        {
            if (oldStartDate == null)
            {
                oldStartDate = StartDate;
            }
            else
            {
                if (oldStartDate != StartDate)
                {
                    oldStartDate = StartDate;
                }
            }
        }
        private void ChangeEndDateCommandAction()
        {
            if (oldEndDate == null)
            {
                oldEndDate = EndDate;
            }
            else
            {
                if (oldEndDate != EndDate)
                {
                    oldEndDate = EndDate;
                }
            }
        }
        private void FillArticleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleMenuList()...", category: Category.Info, priority: Priority.Low);
                //ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PLMService.GetPCMArticlesWithCategory_V2160());
                ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PLMService.GetPCMArticlesWithCategoryForReference_V2160());
                if (DtArticle == null)
                {
                    DtArticle = new DataTable();
                }
                foreach (DataRow row in DtArticle.Rows)
                {
                    ArticleMenuList.Where(a => a.IdArticle == Convert.ToUInt64(row["IdArticle"])).ToList().ForEach(b => b.IsBPLArticle_Current = true);
                }
                SelectedArticle = new PCMArticleCategory();
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
        private void OnDragRecordOverArticleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PCMArticleCategory> record = data.Records.OfType<PCMArticleCategory>().ToList();
                    int count = 0;
                    foreach (PCMArticleCategory article in record.Where(a => a.IdArticle > 0))
                    {
                        DataRow[] found = DataTableForGridLayout.Select("Name = '" + article.Name + "'");
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
        private void DropRecordArticleGrid(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PCMArticleCategory> records = data.Records.OfType<PCMArticleCategory>().ToList();
                    foreach (PCMArticleCategory record in records.Where(a => a.IdArticle > 0))
                    {
                        DataRow[] found = DataTableForGridLayout.Select("Name = '" + record.Name + "'");
                        if (found.Length == 0)
                        {
                            if (AddArticles == null)
                                AddArticles = new List<UInt64>();
                            AddArticles.Add(record.IdArticle);
                            IsCalculated = false;
                            if (DataTableForGridLayout == null)
                                DataTableForGridLayout = new DataTable();
                            DataRow dr = DataTableForGridLayout.NewRow();
                            dr["CheckBox"] = false;
                            dr["Reference"] = record.Reference;
                            dr["Name"] = record.Name;
                            dr["Category"] = record.CategoryName;
                            dr["Value"] = 0.0;
                            dr["IdArticle"] = record.IdArticle;
                            DiscountArticles discountArticles = new DiscountArticles();
                            discountArticles.IdArticle = record.IdArticle;
                            discountArticles.Name = record.Name;
                            discountArticles.Reference = record.Reference;
                            discountArticles.Category = record.CategoryName;
                            if (DiscountArticles == null)
                                DiscountArticles = new List<Data.Common.PCM.DiscountArticles>();
                            DiscountArticles.Add(discountArticles);
                            ArticleMenuList.Where(a => a.IdArticle == record.IdArticle).ToList().ForEach(b => b.IsBPLArticle_Current = true);
                            DataTableForGridLayout.Rows.Add(dr);
                            e.Handled = true;
                        }
                    }
                    DtArticle = DataTableForGridLayout;
                }
                GeosApplication.Instance.Logger.Log("Method DropRecordArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][28.09.2022][GEOS2-3101]
        private void DeleteArticleCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteArticleCommandAction()...", category: Category.Info, priority: Priority.Low);
                DataRow MainRow = ((System.Data.DataRowView)obj).Row;
                if (DtArticle == null)
                    DtArticle = new DataTable();
                else
                {
                    DiscountArticles.Remove(DiscountArticles.Where(i => i.IdArticle == Convert.ToUInt64(MainRow["IdArticle"])).FirstOrDefault());
                    DataTableForGridLayout.Rows.Remove(MainRow);
                }

                DtArticle = DataTableForGridLayout;
                GeosApplication.Instance.Logger.Log("Method DeleteArticleCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteArticleCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetDiscountCustomerAll()//[GEOS2-3102][03.10.2022][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetDiscountCustomerAll()...", category: Category.Info, priority: Priority.Low);

                DiscountCustomers = new List<DiscountCustomers>(PCMService.GetDiscountCustomers(ClonedDiscount.Id));
                List<DiscountCustomers> IsCheckedList = DiscountCustomers.ToList();
                DiscountCustomersClone = new List<Data.Common.PCM.DiscountCustomers>(DiscountCustomers.Select(i => (DiscountCustomers)i.Clone()).ToList());

                group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                CustomerGroup = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                CustomerRegion = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                CustomerCountry = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                CustomerPlant = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());
                GeosApplication.Instance.Logger.Log("Method GetDiscountCustomerAll()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetDiscountCustomerAll() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetDiscountCustomerAll() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetDiscountCustomerAll() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportToExcelDiscountLogs(object obj)//[GEOS2-3102][03.10.2022][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcelDiscounts()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Discount_" + Name + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();
                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
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
                            return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }
                    ResultFileName = (saveFile.FileName);
                    TableView tableView = ((TableView)obj);
                    tableView.ShowTotalSummary = false;

                    TableView ChangeLogTableView = ((TableView)obj);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = false;
                    ChangeLogTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
                    //[rdixit][12.10.2022][GEOS2-3967]
                    //    var SelectedList = DiscountChangeLogList.ToList();
                    //    if (SelectedList == null)
                    //    {
                    //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        tableView.DataControl.ItemsSource = SelectedList;
                    //        tableView.ShowFixedTotalSummary = false;
                    //        tableView.ExportToXlsx(ResultFileName);
                    //        tableView.DataControl.ItemsSource = DiscountChangeLogList;

                    //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    //        System.Diagnostics.Process.Start(ResultFileName);
                    //        tableView.ShowTotalSummary = false;
                    //        tableView.ShowFixedTotalSummary = true;
                    //    }
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcelDiscounts()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcelDiscounts()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ExportToExcelDiscounts()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void AddCustomerCommandAction(object obj)//[GEOS2-3102][03.10.2022][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);

                AddEditCPLCustomerView addEditCPLCustomerView = new AddEditCPLCustomerView();
                AddEditCPLCustomerViewModel addEditCPLCustomerViewModel = new AddEditCPLCustomerViewModel(obj);
                addEditCPLCustomerViewModel.IsDiscountCustomer = true;
                // addEditCPLCustomerViewModel.DiscountCustomers = DiscountCustomers;
                EventHandler handle = delegate { addEditCPLCustomerView.Close(); };
                addEditCPLCustomerViewModel.RequestClose += handle;
                addEditCPLCustomerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCPLCustomerHeader").ToString();
                addEditCPLCustomerViewModel.IsNew = true;

                if (DiscountCustomers == null)
                    DiscountCustomers = new List<DiscountCustomers>();
                addEditCPLCustomerViewModel.DiscountCustomers = DiscountCustomers;
                addEditCPLCustomerView.DataContext = addEditCPLCustomerViewModel;
                addEditCPLCustomerView.ShowDialog();

                if (addEditCPLCustomerViewModel.IsSave == true)
                {
                    if (addEditCPLCustomerViewModel.SelectedGroup.IdGroup == 0)
                    {
                        foreach (Site site in addEditCPLCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    DiscountCustomers customer = new DiscountCustomers();
                                    customer.IdGroup = site.IdGroup;
                                    customer.GroupName = "ALL";
                                    customer.IdRegion = site.IdRegion;
                                    customer.RegionName = site.RegionName;
                                    customer.IdCountry = site.IdCountry;
                                    customer.Country = new Country();
                                    customer.Country.Name = site.CountryName;
                                    customer.IdPlant = site.IdSite;
                                    customer.Plant = new Site();
                                    customer.Plant.Name = site.Name;
                                    DiscountCustomers.Add(customer);
                                }
                            }
                        }

                        if (addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                        {

                            foreach (Country country in addEditCPLCustomerViewModel.SelectedCountry_Save)
                            {
                                if (country != null)
                                {
                                    foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                                    {
                                        if (region != null)
                                        {
                                            if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().IdRegion && ccl.IdCountry == country.IdCountry))
                                            {
                                                DiscountCustomers customer = new DiscountCustomers();
                                                customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                                customer.GroupName = "ALL";
                                                customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                                customer.RegionName = addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                                customer.IdCountry = country.IdCountry;
                                                customer.Country = new Country();
                                                customer.Country.Name = country.Name;
                                                customer.IdPlant = null;
                                                customer.Plant = new Site();
                                                customer.Plant.Name = "ALL";
                                                DiscountCustomers.Add(customer);
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == region.IdRegion))
                                    {
                                        DiscountCustomers customer = new DiscountCustomers();
                                        customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                        customer.GroupName = "ALL";
                                        customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        customer.RegionName = region.RegionName;
                                        customer.IdCountry = null;
                                        customer.Country = new Country();
                                        customer.Country.Name = "ALL";
                                        customer.IdPlant = null;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = "ALL";
                                        DiscountCustomers.Add(customer);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Site site in addEditCPLCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdPlant == site.IdSite))
                                {
                                    DiscountCustomers customer = new DiscountCustomers();
                                    customer.IdGroup = site.IdGroup;
                                    customer.GroupName = site.GroupName;
                                    customer.IdRegion = site.IdRegion;
                                    customer.RegionName = site.RegionName;
                                    customer.IdCountry = site.IdCountry;
                                    customer.Country = new Country();
                                    customer.Country.Name = site.CountryName;
                                    customer.IdPlant = site.IdSite;
                                    customer.Plant = new Site();
                                    customer.Plant.Name = site.Name;
                                    DiscountCustomers.Add(customer);
                                }
                            }

                        }
                        if (addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                        {
                            foreach (Country country in addEditCPLCustomerViewModel.SelectedCountry_Save)
                            {
                                if (country != null)
                                {
                                    if (!DiscountCustomers.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry))
                                    {
                                        DiscountCustomers customer = new DiscountCustomers();
                                        customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                        customer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;
                                        customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                        customer.RegionName = addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                        customer.IdCountry = country.IdCountry;
                                        customer.Country = new Country();
                                        customer.Country.Name = country.Name;
                                        customer.IdPlant = null;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = "ALL";
                                        DiscountCustomers.Add(customer);
                                    }
                                }
                            }

                            foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    if (!DiscountCustomers.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion))
                                    {
                                        DiscountCustomers customer = new DiscountCustomers();
                                        customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                        customer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;
                                        customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        customer.RegionName = region.RegionName;
                                        customer.IdCountry = null;
                                        customer.Country = new Country();
                                        customer.Country.Name = "ALL";
                                        customer.IdPlant = null;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = "ALL";
                                        DiscountCustomers.Add(customer);
                                    }
                                }
                                else
                                {
                                    DiscountCustomers customer = new DiscountCustomers();
                                    customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                    customer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;
                                    customer.IdRegion = null;
                                    customer.RegionName = "ALL";
                                    customer.IdCountry = null;
                                    customer.Country = new Country();
                                    customer.Country.Name = "ALL";
                                    customer.IdPlant = null;
                                    customer.Plant = new Site();
                                    customer.Plant.Name = "ALL";
                                    DiscountCustomers.Add(customer);
                                }
                            }
                        }

                    }

                    DiscountCustomers = new List<DiscountCustomers>(DiscountCustomers.OrderBy(a => a.GroupName));
                    //DiffDiscountCustomer = DiscountCustomers.Except(DiscountCustomers).ToList();
                    List<DiscountCustomers> IsCheckedList = DiscountCustomers.ToList();

                    group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                    CustomerGroup = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    CustomerRegion = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    CustomerCountry = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    CustomerPlant = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

                    //IsExportVisible = Visibility.Collapsed;
                    //IsEnabledSaveButton = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddCustomerCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteCustomerCommandAction(object obj)//[GEOS2-3102][03.10.2022][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCPLCustomer"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                DiscountCustomers SelectedCustomer = (DiscountCustomers)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    DiscountCustomers.Remove(SelectedCustomer);
                    DiscountCustomers = new List<DiscountCustomers>(DiscountCustomers.OrderBy(a => a.GroupName));
                    SelectedCustomer = DiscountCustomers.FirstOrDefault();

                    List<DiscountCustomers> IsCheckedList = DiscountCustomers.ToList();

                    Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                    CustomerGroup = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    CustomerRegion = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    CustomerCountry = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    CustomerPlant = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());
                }

                GeosApplication.Instance.Logger.Log("Method DeleteCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteCustomerCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditCustomerCommandAction(object obj)//[GEOS2-3102][03.10.2022][rdixit]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                DiscountCustomers SelectedCustomer = (DiscountCustomers)detailView.DataControl.CurrentItem;

                AddEditCPLCustomerView addEditCPLCustomerView = new AddEditCPLCustomerView();
                AddEditCPLCustomerViewModel addEditCPLCustomerViewModel = new AddEditCPLCustomerViewModel(obj);
                addEditCPLCustomerViewModel.IsDiscountCustomer = true;
                EventHandler handle = delegate { addEditCPLCustomerView.Close(); };
                addEditCPLCustomerViewModel.RequestClose += handle;
                addEditCPLCustomerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCPLCustomerHeader").ToString();
                addEditCPLCustomerViewModel.IsNew = false;

                addEditCPLCustomerViewModel.EditInit(SelectedCustomer);
                addEditCPLCustomerViewModel.DiscountCustomers = DiscountCustomers;
                addEditCPLCustomerView.DataContext = addEditCPLCustomerViewModel;
                addEditCPLCustomerView.ShowDialog();
                int count = 0;
                if (addEditCPLCustomerViewModel.IsSave == true)
                {

                    bool isupdatedRow = false;

                    if (SelectedCustomer.IdGroup == 0 && addEditCPLCustomerViewModel.SelectedGroup.IdGroup == 0)
                    {
                        #region 0 & 0
                        foreach (Site site in addEditCPLCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                                            SelectedCustomer = DiscountCustomers.Where(ccl => ccl.IdCustomerDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer).FirstOrDefault();

                                        SelectedCustomer.IdGroup = site.IdGroup;
                                        SelectedCustomer.GroupName = "ALL";
                                        SelectedCustomer.IdRegion = site.IdRegion;
                                        SelectedCustomer.RegionName = site.RegionName;
                                        SelectedCustomer.IdCountry = site.IdCountry;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = site.CountryName;
                                        SelectedCustomer.IdPlant = site.IdSite;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = site.Name;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        DiscountCustomers customer = new DiscountCustomers();
                                        customer.IdGroup = site.IdGroup;
                                        customer.GroupName = "ALL";
                                        customer.IdRegion = site.IdRegion;
                                        customer.RegionName = site.RegionName;
                                        customer.IdCountry = site.IdCountry;
                                        customer.Country = new Country();
                                        customer.Country.Name = site.CountryName;
                                        customer.IdPlant = site.IdSite;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = site.Name;
                                        DiscountCustomers.Add(customer);
                                    }
                                }
                            }
                        }

                        foreach (Country country in addEditCPLCustomerViewModel.SelectedCountry_Save)
                        {
                            if (country != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdCountry == country.IdCountry && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                                            SelectedCustomer = DiscountCustomers.Where(ccl => ccl.IdCustomerDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                        SelectedCustomer.GroupName = "ALL";
                                        SelectedCustomer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                        SelectedCustomer.RegionName = addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                        SelectedCustomer.IdCountry = country.IdCountry;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = country.Name;
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            DiscountCustomers customer = new DiscountCustomers();
                                            customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                            customer.GroupName = "ALL";
                                            customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                            customer.RegionName = addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                            customer.IdCountry = country.IdCountry;
                                            customer.Country = new Country();
                                            customer.Country.Name = country.Name;
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            DiscountCustomers.Add(customer);
                                        }
                                    }
                                }
                            }
                        }


                        foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                        {
                            if (region != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == region.IdRegion && ccl.IdCountry == null && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                                            SelectedCustomer = DiscountCustomers.Where(ccl => ccl.IdCustomerDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                        SelectedCustomer.GroupName = "ALL";
                                        SelectedCustomer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        SelectedCustomer.RegionName = region.RegionName;
                                        SelectedCustomer.IdCountry = null;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = "ALL";
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditCPLCustomerViewModel.SelectedCountry_Save.FirstOrDefault() == null && addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            DiscountCustomers customer = new DiscountCustomers();
                                            customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;
                                            customer.GroupName = "ALL";
                                            customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                            customer.RegionName = region.RegionName;
                                            customer.IdCountry = null;
                                            customer.Country = new Country();
                                            customer.Country.Name = "ALL";
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            DiscountCustomers.Add(customer);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        foreach (Site site in addEditCPLCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                                            SelectedCustomer = DiscountCustomers.Where(ccl => ccl.IdCustomerDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer).FirstOrDefault();
                                        SelectedCustomer.IdGroup = site.IdGroup;

                                        if (string.IsNullOrEmpty(site.GroupName) || site.GroupName == "")
                                            SelectedCustomer.GroupName = "ALL";
                                        else
                                            SelectedCustomer.GroupName = site.GroupName;

                                        SelectedCustomer.IdRegion = site.IdRegion;
                                        SelectedCustomer.RegionName = site.RegionName;
                                        SelectedCustomer.IdCountry = site.IdCountry;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = site.CountryName;
                                        SelectedCustomer.IdPlant = site.IdSite;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = site.Name;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        DiscountCustomers customer = new DiscountCustomers();
                                        customer.IdGroup = site.IdGroup;

                                        if (string.IsNullOrEmpty(site.GroupName) || site.GroupName == "")
                                            customer.GroupName = "ALL";
                                        else
                                            customer.GroupName = site.GroupName;

                                        customer.IdRegion = site.IdRegion;
                                        customer.RegionName = site.RegionName;
                                        customer.IdCountry = site.IdCountry;
                                        customer.Country = new Country();
                                        customer.Country.Name = site.CountryName;
                                        customer.IdPlant = site.IdSite;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = site.Name;
                                        DiscountCustomers.Add(customer);
                                    }
                                }
                            }
                        }

                        foreach (Country country in addEditCPLCustomerViewModel.SelectedCountry_Save)
                        {
                            if (country != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                                            SelectedCustomer = DiscountCustomers.Where(ccl => ccl.IdCustomerDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;

                                        if (string.IsNullOrEmpty(addEditCPLCustomerViewModel.SelectedGroup.GroupName) || addEditCPLCustomerViewModel.SelectedGroup.GroupName == "")
                                            SelectedCustomer.GroupName = "ALL";
                                        else
                                            SelectedCustomer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;

                                        SelectedCustomer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                        SelectedCustomer.RegionName = addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                        SelectedCustomer.IdCountry = country.IdCountry;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = country.Name;
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            DiscountCustomers customer = new DiscountCustomers();
                                            customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;

                                            if (string.IsNullOrEmpty(addEditCPLCustomerViewModel.SelectedGroup.GroupName) || addEditCPLCustomerViewModel.SelectedGroup.GroupName == "")
                                                customer.GroupName = "ALL";
                                            else
                                                customer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;

                                            customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                            customer.RegionName = addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                            customer.IdCountry = country.IdCountry;
                                            customer.Country = new Country();
                                            customer.Country.Name = country.Name;
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            DiscountCustomers.Add(customer);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                        {
                            if (region != null)
                            {
                                if (!DiscountCustomers.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion && ccl.IdCountry == null && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                                            SelectedCustomer = DiscountCustomers.Where(ccl => ccl.IdCustomerDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;

                                        if (string.IsNullOrEmpty(addEditCPLCustomerViewModel.SelectedGroup.GroupName) || addEditCPLCustomerViewModel.SelectedGroup.GroupName == "")
                                            SelectedCustomer.GroupName = "ALL";
                                        else
                                            SelectedCustomer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;

                                        SelectedCustomer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        SelectedCustomer.RegionName = region.RegionName;
                                        SelectedCustomer.IdCountry = null;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = "ALL";
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditCPLCustomerViewModel.SelectedCountry_Save.FirstOrDefault() == null && addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            DiscountCustomers customer = new DiscountCustomers();
                                            customer.IdGroup = (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup;

                                            if (string.IsNullOrEmpty(addEditCPLCustomerViewModel.SelectedGroup.GroupName) || addEditCPLCustomerViewModel.SelectedGroup.GroupName == "")
                                                customer.GroupName = "ALL";
                                            else
                                                customer.GroupName = addEditCPLCustomerViewModel.SelectedGroup.GroupName;

                                            customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                            customer.RegionName = region.RegionName;
                                            customer.IdCountry = null;
                                            customer.Country = new Country();
                                            customer.Country.Name = "ALL";
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            DiscountCustomers.Add(customer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    DiscountCustomers = new List<DiscountCustomers>(DiscountCustomers.OrderBy(a => a.GroupName));

                    List<DiscountCustomers> IsCheckedList = DiscountCustomers.ToList();

                    Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                    CustomerGroup = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    CustomerRegion = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    CustomerCountry = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    CustomerPlant = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

                    //if (addEditCPLCustomerViewModel.IdDiscountCustomer > 0)
                    //{
                    //    CPLCustomer customer_old = ClonedDiscount.disc.CustomerList.FirstOrDefault(a => a.IdDiscountCustomer == addEditCPLCustomerViewModel.IdDiscountCustomer);
                    //    if ((SelectedCustomer.IdGroup != customer_old.IdGroup) || (SelectedCustomer.IdRegion != customer_old.IdRegion) || (SelectedCustomer.IdCountry != customer_old.IdCountry) || (SelectedCustomer.IdPlant != customer_old.IdPlant))
                    //    {
                    //        IsExportVisible = Visibility.Collapsed;
                    //    }
                    //}

                }
                GeosApplication.Instance.Logger.Log("Method EditCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditCustomerCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetChangelogs(int IdCustomerDiscount)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetChangelogs()...", category: Category.Info, priority: Priority.Low);
                //DiscountChangeLogList = new ObservableCollection<DiscountLogEntry>(PCMService.GetDiscountLogEntriesByDiscountstring(IdCustomerDiscount));
                //[Sudhir.Jangra][GEOS2-4935]
                DiscountChangeLogList = new ObservableCollection<DiscountLogEntry>(PCMService.GetDiscountLogEntriesByDiscountstring_V2470(IdCustomerDiscount));


                GeosApplication.Instance.Logger.Log("Method GetChangelogs()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetChangelogs() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetChangelogs() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetChangelogs() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4935][21/11/2023]
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);
                GridControl gridControlView = (GridControl)obj;
                AddEditPCMDiscountsCommentsView addCommentsView = new AddEditPCMDiscountsCommentsView();
                AddEditPCMDiscountsCommentsViewModel addCommentsViewModel = new AddEditPCMDiscountsCommentsViewModel();
                EventHandler handle = delegate { addCommentsView.Close(); };
                addCommentsViewModel.RequestClose += handle;
                addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                var ownerInfo = (gridControlView as FrameworkElement);
                addCommentsView.Owner = Window.GetWindow(ownerInfo);
                //addCommentsViewModel.IsNew = true;
                //addCommentsViewModel.Init();

                addCommentsView.DataContext = addCommentsViewModel;
                addCommentsView.ShowDialog();
                if (addCommentsViewModel.SelectedComment != null)
                {

                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<DiscountLogEntry>();

                    //  addCommentsViewModel.SelectedComment.IdCPType = contacts.IdArticleSupplier;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<DiscountLogEntry>();

                    AddCommentsList.Add(new DiscountLogEntry()
                    {
                        IdUser = addCommentsViewModel.SelectedComment.IdUser,
                        // IdCPType = addCommentsViewModel.SelectedComment.IdCPType,
                        Comments = addCommentsViewModel.SelectedComment.Comments
                    });
                    CommentsList.Add(addCommentsViewModel.SelectedComment);
                    SelectedComment = addCommentsViewModel.SelectedComment;
                    CommentText = SelectedComment.Comments;
                    CommentDateTimeText = DateTime.Now;
                    CommentFullNameText = GeosApplication.Instance.ActiveUser.FullName;
                }
                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public void DeleteCommentRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);
                DiscountLogEntry commentObject = (DiscountLogEntry)parameter;
                if (commentObject.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
                {
                    bool result = false;

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteBasePriceComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (CommentsList != null && CommentsList?.Count > 0)
                        {
                            DiscountLogEntry Comment = (DiscountLogEntry)commentObject;

                            CommentsList.Remove(Comment);

                            if (DeleteCommentsList == null)
                                DeleteCommentsList = new ObservableCollection<DiscountLogEntry>();

                            DeleteCommentsList.Add(new DiscountLogEntry()
                            {
                                IdUser = Comment.IdUser,
                                //  IdCPType = Comment.IdCPType,
                                Comments = Comment.Comments,
                                //  IdLogEntryByCptype = Comment.IdLogEntryByCptype

                            });
                            CommentsList = new ObservableCollection<DiscountLogEntry>(CommentsList);

                            IsDeleted = true;
                            if (CommentsList?.Count > 0)
                            {
                                CommentText = CommentsList[CommentsList.Count - 1].Comments;
                                CommentDateTimeText = CommentsList[CommentsList.Count - 1].Datetime;
                                CommentFullNameText = CommentsList[CommentsList.Count - 1].People.FullName;
                            }
                            else
                            {
                                CommentText = string.Empty;
                                CommentDateTimeText = null;
                                CommentFullNameText = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["PCMDeleteProductTypeCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteCommentRowCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }


        //[Sudhir.Jangra][GEOS2-4935]
        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
            DiscountLogEntry logcomments = (DiscountLogEntry)obj;
            if (logcomments.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                AddEditPCMDiscountsCommentsView editCommentsView = new AddEditPCMDiscountsCommentsView();
                AddEditPCMDiscountsCommentsViewModel editCommentsViewModel = new AddEditPCMDiscountsCommentsViewModel();
                EventHandler handle = delegate { editCommentsView.Close(); };
                editCommentsViewModel.RequestClose += handle;
                editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
                editCommentsViewModel.NewItemComment = SelectedComment.Comments;
                // editCommentsViewModel.IdLogEntryByCpType = SelectedComment.IdLogEntryByCptype;
                editCommentsView.DataContext = editCommentsViewModel;
                editCommentsView.ShowDialog();

                if (editCommentsViewModel.SelectedComment != null)
                {
                    SelectedComment.Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByDiscount == SelectedComment.IdLogEntryByDiscount).Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByDiscount == SelectedComment.IdLogEntryByDiscount).Datetime = GeosApplication.Instance.ServerDateTime;

                    if (UpdatedCommentsList == null)
                        UpdatedCommentsList = new List<DiscountLogEntry>();

                    // editCommentsViewModel.SelectedComment.IdCPType = SelectedContacts.;
                    UpdatedCommentsList.Add(new DiscountLogEntry()
                    {
                        IdUser = SelectedComment.IdUser,
                        IdCustomerDiscount = SelectedComment.IdCustomerDiscount,
                        Comments = SelectedComment.Comments,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdLogEntryByDiscount = SelectedComment.IdLogEntryByDiscount
                    });
                }
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["EditProductTypeCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

        }

        //[Sudhir.jangra][GEOS2-4935]
        private void SetUserProfileImage(ObservableCollection<DiscountLogEntry> CommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in CommentsList)
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            //if (item.People.IdPersonGender == 1)
                            //    item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            //else if (item.People.IdPersonGender == 2)
                            //    item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                            //else if (item.People.IdPersonGender == null)
                            item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");

                        }
                        else
                        {
                            //if (item.People.IdPersonGender == 1)
                            //    item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            //else if (item.People.IdPersonGender == 2)
                            //    item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            //else if (item.People.IdPersonGender == null)
                            item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Sudhir.Jangra][GEOS2-4935]
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
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

                //GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }


        //[Sudhir.Jangra][GEOS2-4935]
        private void GetCommentsList(int IdCustomerDiscount)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetChangelogs()...", category: Category.Info, priority: Priority.Low);
                CommentsList = new ObservableCollection<DiscountLogEntry>(PCMService.GetDiscountCommentsByDiscountstring_V2470(IdCustomerDiscount).OrderByDescending(x=>x.Datetime));

                if (CommentsList?.Count > 0)
                {
                    CommentText = CommentsList.FirstOrDefault().Comments;
                    CommentDateTimeText = CommentsList.FirstOrDefault().Datetime;
                    CommentFullNameText = CommentsList.FirstOrDefault().UserName;

                    //CommentText = CommentsList[CommentsList.Count - 1].Comments;
                    //CommentDateTimeText = CommentsList[CommentsList.Count - 1].Datetime;
                    //CommentFullNameText = CommentsList[CommentsList.Count - 1].UserName;
                }
                else
                {
                    CommentText = string.Empty; // or some default value if there are no comments
                    CommentDateTimeText = null;
                    CommentFullNameText = string.Empty;
                }

                SetUserProfileImage(CommentsList);


                GeosApplication.Instance.Logger.Log("Method GetChangelogs()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetChangelogs() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetChangelogs() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetChangelogs() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                me[BindableBase.GetPropertyName(() => Name)];
                /*me[BindableBase.GetPropertyName(() => InformationError)];*/


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

                string name = BindableBase.GetPropertyName(() => Name);
                //string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);

                if (columnName == name)
                {
                    return AddEditDiscountValidation.GetErrorMessage(name, Name);
                }

                //if (columnName == headerInformtionError)
                //{
                //    return AddEditBasePriceValidation.GetErrorMessage(headerInformtionError, InformationError);
                //}



                return null;
            }
        }

        #endregion

    }
}
