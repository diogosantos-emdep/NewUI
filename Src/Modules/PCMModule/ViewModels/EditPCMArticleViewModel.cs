using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Export;
using DevExpress.XtraRichEdit.Export.Html;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using Emdep.Geos.Data.Common.SynchronizationClass;
using DevExpress.Data.Filtering;
using Emdep.Geos.Modules.PLM.CommonClasses;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.Modules.PLM.ViewModels;
using DevExpress.Xpf.Core.Serialization;
using static Emdep.Geos.UI.CustomControls.CustomMessageBox;
using DevExpress.Xpf.Editors.Helpers;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class EditPCMArticleViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged, IDataErrorInfo
    {
        public void Dispose()
        {
        }

        #region Service

        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

       //  IPCMService PCMService = new PCMServiceController("localhost:6699");
       //  IPLMService PLMService = new PLMServiceController("localhost:6699");
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

        #region Declaration

        bool isModuleExpand;
        bool isArticleExpand;

        public string PCMPriceListIncludedGridSetting = GeosApplication.Instance.UserSettingFolderName + "PCMArticlePriceListIncludedGridSetting.Xml";
        //private ObservableCollection<PCMArticleCategory> inUseCategoryMenulist;
        private EditPCMArticleView EditPCMArticleViewInstance;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private TableView view;
        public ObservableCollection<PCMArticleCategory> tempCategoryList { get; set; }
        public IList<LookupValue> tempStatusList { get; set; }
        public IList<LookupValue> tempECOSVisibilityList { get; set; }
        private bool isWorkOrderColumnChooserVisible;
        public Articles UpdatedArticle { get; set; }

        private double dialogHeight;
        private double dialogWidth;

        private bool isNew;

        private string reference;
        private string description;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;

        private ObservableCollection<PCMArticleCategory> categoryList;
        private PCMArticleCategory selectedCategory;
        private IList<LookupValue> statusList;
        private string status;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;
        private int idPCMStatus;
        private double weight;
        private float length;
        private string supplier;
        private float width;
        private float height;
        private bool isSave;

        private uint? idArticleCategory;
        private uint idPCMArticleCategory;
        private uint idArticle;

        private WarehouseStatus warehouseStatus;
        private ImageSource referenceImage;
        private ImageSource oldReferenceImage;
        private bool isReferenceImageExist;
        private string error = string.Empty;

        private string articleWeightSymbol;

        MaximizedElementPosition maximizedElementPosition;

        private ObservableCollection<ProductTypesTemplate> moduleMenuList;
        private ProductTypesTemplate selectedModule;

        private ObservableCollection<ArticleCompatibility> mandatoryList;
        private ArticleCompatibility selectedMandatory;

        private ObservableCollection<ArticleCompatibility> suggestedList;
        private ArticleCompatibility selectedSuggested;

        private ObservableCollection<ArticleCompatibility> incompatibleList;
        private ArticleCompatibility selectedIncompatible;

        private int compatibilityCount;

        private ObservableCollection<PCMArticleCategory> articleMenuList;
        private PCMArticleCategory selectedArticle;

        private List<LookupValue> relationShipList;
        private LookupValue selectedRelationShip;
        private string compatibilityError;

        private ObservableCollection<PCMArticleLogEntry> articleChangeLogList;
        private ObservableCollection<ArticleDecomposition> articleDecompositionList;
        private ObservableCollection<PCMArticleLogEntry> changeLogList;
        private PCMArticleLogEntry selectedArticleChangeLog;

        private int articleComponentsCount;/* [001][kshinde][07/06/2022][GEOS2-3270]*/

        private ObservableCollection<ArticleDocument> articleFilesList;
        private ObservableCollection<ArticleDocument> fourRecordsArticleFilesList;
        private ArticleDocument selectedArticleFile;

        private ObservableCollection<PCMArticleImage> imagesList;
        private ObservableCollection<PCMArticleImage> fourRecordsArticleImagesList;
        private PCMArticleImage selectedImage;
        private PCMArticleImage selectedDefaultImage;
        private PCMArticleImage selectedContentTemplateImage;
        private int selectedImageIndex;

        private PCMArticleImage maximizedElement;

        private Articles clonedArticle;
        private ArticleCustomer clonedArticleCustomer;
        string pCMDescription;
        public string Desc;

        private bool isRtf;
        private bool isNormal;
        private Visibility textboxnormal;
        private bool isPOArticleNull;
        private Visibility richtextboxrtf;
        string pCMDescription_Richtext;
        private byte isImageShareWithCustomer;
        private bool isImageShareWithCustomerVisible;

        private PCMArticleImage oldDefaultImage;
        private float pQuantityMin;
        private float pQuantityMax;
        private string informationError;
        private string pQuantityMinErrorMessage = string.Empty;
        private string pQuantityMaxErrorMessage = string.Empty;
        private IList<LookupValue> eCOSVisibilityList;
        private LookupValue selectedECOSVisibility;
        private bool isEnabledMinMax;
        public bool WarehouseImageOrNot = false;
        public PCMArticleImage OldWarehouseImage { get; set; }
        public ulong oldWarehouseposition = 0;
        private string name;
        private List<LogEntriesByArticle> wMSArticleChangeLogEntry;
        private bool isReadOnlyName;
        private bool isCheckedCopyName;

        private ObservableCollection<Language> languages;
        private Language languageSelected;
        private Language languageSelectedInCatelogueDescription;


        private bool isCheckedCopyNameReadOnly;
        private bool IsCopyDescription;
        private bool isEnabledCopyNameReadOnly;

        private bool isCheckedCopyCatelogueDescriptionReadOnly;
        private bool isCheckedCopyCatelogueDescription;

        public bool isOnlyDescriptionEmptySpaces = false;
        public bool isOnlyPCMDescriptionEmptySpaces = false;
        public string RedText = string.Empty;

        public bool IsFromInformation = false;

        public bool IsFromCatelogueDesc = false;
        private bool isEmptySpace;
        private bool isEmptySpaceFromPCMCatelogue;

        private bool enableListBoxForLanguagePCM;

        private ObservableCollection<PLMArticlePrice> includedPLMArticlePriceList;
        private ObservableCollection<PLMArticlePrice> notIncludedPLMArticlePriceList;

        private PLMArticlePrice selectedIncludedPLMArticlePrice;
        private PLMArticlePrice selectedNotIncludedPLMArticlePrice;

        private List<LookupValue> logicList;
        private LookupValue selectedLogic;
        private Visibility groupBoxVisible;
        private Int64 includedActiveCount;
        private string includedFirstActiveName;
        private ImageSource includedFirstActiveCurrencyIconImage;
        private double? includedFirstActiveSellPrice;
        private string currencySymbol;
        private string onFocusFgColor;

        private bool isColumnChooserVisible;
        private bool isFirstTimeLoad;

        private string selectedCurrencySymbol;
        private Currency selectedCurrency;

        private ITokenService tokenService;
        List<GeosAppSetting> geosAppSettingList;
        private bool isAdded;
        private bool isAcceptButtonEnabled;
        private bool isBPLMessageShow;
        private bool isCPLMessageShow;
        private bool isBPLCalculateRuleValue;
        private bool isCPLCalculateRuleValue;
        private bool isOnlyAcceptButtonEnabled;
        private ArticleDecomposition selectedItemArticleDecomposition;
        private string visible;
        private bool isReadOnlyField;
        private bool allowDragDrop;
        private bool isEnabled;
        private ObservableCollection<ArticleCustomer> articleCustomerList;
        private int partNumberCount;
        private ArticleCustomer selectedArticleCustomer;
        private int group;
        private int region;
        private int country;
        private int plant;

        public string Groups { get; set; }
        public string Regions { get; set; }
        public string Countries { get; set; }
        public string Plants { get; set; }
        private string customerReference;
        private ObservableCollection<ArticleSuppliers> articleSuppliersList;
        private List<ArticleCustomer> pCMArticleCustomerList;
        public string CustomerGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "PCM_CustomerGridSettingFilePath.Xml";
        public bool isEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][14/02/2023]
        public string oldDescription;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]

        private string isImageScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][10/03/2023]
        private string isAttachmentScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][13/03/2023]

        private string oldDescription_en;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_es;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_fr;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_pt;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_ro;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_ru;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_zh;//[Sudhir.Jangra][GEOS2-4905]

        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        private ObservableCollection<LinkedArticle> linkedarticleList;
        private int linkedarticleCount;
        private bool isDetailsChecked;
        private bool isPricesChecked;
        #endregion

        #region Properties
        List<PCMArticleImage> pCMArticleImageList;

        public List<PCMArticleImage> PCMArticleImageList
        {
            get
            {
                return pCMArticleImageList;
            }

            set
            {
                pCMArticleImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArticleImageList"));              
            }
        }
        //[30.11.2022][sshegaonkar][GEOS2-2718]
        public bool IsModuleExpand
        {
            get { return isModuleExpand; }
            set
            {
                isModuleExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModuleExpand"));
            }
        }

        //[30.11.2022][sshegaonkar][GEOS2-2718]
        public bool IsArticleExpand
        {
            get { return isArticleExpand; }
            set
            {
                isArticleExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleExpand"));
            }
        }

        bool issArticleSync;
        public bool IsArticleSync
        {
            get { return issArticleSync; }
            set
            {
                issArticleSync = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleSync"));
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

        public bool IsColumnChooserVisible
        {
            get { return isColumnChooserVisible; }
            set
            {
                isColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColumnChooserVisible"));
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

        public Articles ClonedArticle
        {
            get { return clonedArticle; }
            set
            {
                clonedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedArticle"));
            }
        }

        public ArticleCustomer ClonedArticleCustomer
        {
            get { return clonedArticleCustomer; }
            set
            {
                clonedArticleCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedArticleCustomer"));
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

        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;

                if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                {
                    IsReadOnlyName = false;
                    IsCheckedCopyNameReadOnly = false;
                    IsEnabledCopyNameReadOnly = true;

                    if (!(string.IsNullOrEmpty(description)))
                        InformationError = " ";

                    else
                        InformationError = null;
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
                else
                {
                    description = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                    IsReadOnlyName = true;
                    IsCheckedCopyNameReadOnly = false;
                    IsEnabledCopyNameReadOnly = false;
                }
             //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_en = description_en.Trim(' ', '\r');
                if (!(string.IsNullOrEmpty(description_en)))
                    InformationError = " ";

                else
                    InformationError = null;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_en"));
              //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_es = description_es.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
             //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_fr = description_fr.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
            //    IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_pt = description_pt.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
               // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_ro = description_ro.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
              //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_ru = description_ru.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
              //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                description_zh = description_zh.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
             //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<PCMArticleCategory> CategoryList
        {
            get
            {
                return categoryList;
            }

            set
            {
                categoryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryList"));
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
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public IList<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        public int SelectedStatusIndex
        {
            get { return selectedStatusIndex; }
            set
            {
                selectedStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusIndex"));
            }
        }

        public LookupValue SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int IdPCMStatus
        {
            get
            {
                return idPCMStatus;
            }

            set
            {
                idPCMStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPCMStatus"));
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public float Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Length"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string Supplier
        {
            get
            {
                return supplier;
            }

            set
            {
                supplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Supplier"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public float Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public uint? IdArticleCategory
        {
            get
            {
                return idArticleCategory;
            }

            set
            {
                idArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticleCategory"));
            }
        }

        public uint IdPCMArticleCategory
        {
            get
            {
                return idPCMArticleCategory;
            }

            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPCMArticleCategory"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public uint IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticle"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public WarehouseStatus WarehouseStatus
        {
            get
            {
                return warehouseStatus;
            }

            set
            {
                warehouseStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseStatus"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ImageSource ReferenceImage
        {
            get { return referenceImage; }
            set
            {
                referenceImage = value;
                if (referenceImage != null)
                {
                    IsReferenceImageExist = true;
                }
                else
                {
                    ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/ImageEditLogo.png"));
                    IsReferenceImageExist = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceImage"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ImageSource OldReferenceImage
        {
            get { return oldReferenceImage; }
            set
            {
                oldReferenceImage = value; OnPropertyChanged(new PropertyChangedEventArgs("OldReferenceImage"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public bool IsReferenceImageExist
        {
            get { return isReferenceImageExist; }
            set
            {
                isReferenceImageExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReferenceImageExist"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string ArticleWeightSymbol
        {
            get { return articleWeightSymbol; }
            set
            {
                articleWeightSymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleWeightSymbol"));
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

        public ObservableCollection<ProductTypesTemplate> ModuleMenulist
        {
            get
            {
                return moduleMenuList;
            }

            set
            {
                moduleMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleMenulist"));
            }
        }

        public ProductTypesTemplate SelectedModule
        {
            get
            {
                return selectedModule;
            }

            set
            {
                selectedModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedModule"));
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<ArticleCompatibility> MandatoryList
        {
            get
            {
                return mandatoryList;
            }

            set
            {
                mandatoryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MandatoryList"));
            }
        }

        public ArticleCompatibility SelectedMandatory
        {
            get
            {
                return selectedMandatory;
            }

            set
            {
                selectedMandatory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMandatory"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<ArticleCompatibility> SuggestedList
        {
            get
            {
                return suggestedList;
            }

            set
            {
                suggestedList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuggestedList"));
            }
        }

        public ArticleCompatibility SelectedSuggested
        {
            get
            {
                return selectedSuggested;
            }

            set
            {
                selectedSuggested = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSuggested"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<ArticleCompatibility> IncompatibleList
        {
            get
            {
                return incompatibleList;
            }

            set
            {
                incompatibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncompatibleList"));
            }
        }

        public ArticleCompatibility SelectedIncompatible
        {
            get
            {
                return selectedIncompatible;
            }

            set
            {
                selectedIncompatible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncompatible"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int CompatibilityCount
        {
            get
            {
                return compatibilityCount;
            }

            set
            {
                compatibilityCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompatibilityCount"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public List<LookupValue> RelationShipList
        {
            get
            {
                return relationShipList;
            }

            set
            {
                relationShipList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RelationShipList"));
            }
        }

        public LookupValue SelectedRelationShip
        {
            get
            {
                return selectedRelationShip;
            }

            set
            {
                selectedRelationShip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRelationShip"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string CompatibilityError
        {
            get { return compatibilityError; }
            set
            {
                compatibilityError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompatibilityError"));
            }
        }

        public ObservableCollection<PCMArticleLogEntry> ArticleChangeLogList
        {
            get
            {
                return articleChangeLogList;
            }

            set
            {
                articleChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleChangeLogList"));
            }
        }

        public ObservableCollection<ArticleDecomposition> ArticleDecompositionList
        {
            get
            {
                return articleDecompositionList;
            }

            set
            {
                articleDecompositionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleDecompositionList"));
            }
        }

        public ObservableCollection<PCMArticleLogEntry> ChangeLogList
        {
            get
            {
                return changeLogList;
            }

            set
            {
                changeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogList"));
            }
        }

        public PCMArticleLogEntry SelectedArticleChangeLog
        {
            get
            {
                return selectedArticleChangeLog;
            }

            set
            {
                selectedArticleChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleChangeLog"));
            }
        }
        /* [001][kshinde][07/06/2022][GEOS2-3270]*/
        public int ArticleComponentsCount
        {
            get
            {
                return articleComponentsCount;
            }

            set
            {
                articleComponentsCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleComponentsCount"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<ArticleDocument> ArticleFilesList
        {
            get
            {
                return articleFilesList;
            }

            set
            {
                articleFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleFilesList"));
            }
        }

        public ObservableCollection<ArticleDocument> FourRecordsArticleFilesList
        {
            get
            {
                return fourRecordsArticleFilesList;
            }

            set
            {
                fourRecordsArticleFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsArticleFilesList"));
            }
        }

        public ArticleDocument SelectedArticleFile
        {
            get
            {
                return selectedArticleFile;
            }

            set
            {
                selectedArticleFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleFile"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<PCMArticleImage> ImagesList
        {
            get { return imagesList; }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }

        public ObservableCollection<PCMArticleImage> FourRecordsArticleImagesList
        {
            get
            {
                return fourRecordsArticleImagesList;
            }

            set
            {
                fourRecordsArticleImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsArticleImagesList"));
            }
        }

        public PCMArticleImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public PCMArticleImage SelectedDefaultImage
        {
            get
            {
                return selectedDefaultImage;
            }

            set
            {
                selectedDefaultImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDefaultImage"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public PCMArticleImage SelectedContentTemplateImage
        {
            get
            {
                return selectedContentTemplateImage;
            }

            set
            {
                selectedContentTemplateImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContentTemplateImage"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int SelectedImageIndex
        {
            get
            {
                return selectedImageIndex;
            }

            set
            {
                selectedImageIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImageIndex"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public PCMArticleImage MaximizedElement
        {
            get
            {
                return maximizedElement;
            }
            set
            {
                maximizedElement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElement"));
            }
        }
        public string PCMDescription
        {
            get { return pCMDescription; }
            set
            {
                pCMDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMDescription"));
               // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public bool IsRtf
        {
            get { return isRtf; }
            set
            {
                isRtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRtf"));
            }
        }

        public bool IsNormal
        {
            get { return isNormal; }
            set
            {
                isNormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormal"));
            }
        }

        public Visibility Textboxnormal
        {
            get
            {
                return textboxnormal;
            }

            set
            {
                textboxnormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Textboxnormal"));
            }
        }

        public bool IsPOArticleNull//[GEOS2-3785][rdixit][21.07.2022]
        {
            get { return isPOArticleNull; }
            set
            {
                isPOArticleNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOArticleNull"));
            }
        }
        public Visibility Richtextboxrtf
        {
            get
            {
                return richtextboxrtf;
            }

            set
            {
                richtextboxrtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Richtextboxrtf"));
            }
        }
        public string PCMDescription_Richtext
        {
            get { return pCMDescription_Richtext; }
            set
            {
                pCMDescription_Richtext = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMDescription_Richtext"));
              //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public byte IsImageShareWithCustomer
        {
            get { return isImageShareWithCustomer; }
            set
            {
                isImageShareWithCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageShareWithCustomer"));
            }
        }

        public bool IsImageShareWithCustomerVisible
        {
            get
            {
                return isImageShareWithCustomerVisible;
            }

            set
            {
                isImageShareWithCustomerVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageShareWithCustomerVisible"));
            }
        }

        public PCMArticleImage OldDefaultImage
        {
            get
            {
                return oldDefaultImage;
            }
            set
            {
                oldDefaultImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldDefaultImage"));
            }
        }

        public float PQuantityMin
        {
            get
            {
                return pQuantityMin;
            }

            set
            {
                pQuantityMin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PQuantityMin"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public float PQuantityMax
        {
            get
            {
                return pQuantityMax;
            }

            set
            {
                pQuantityMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PQuantityMax"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public LookupValue SelectedECOSVisibility
        {
            get
            {
                return selectedECOSVisibility;
            }

            set
            {
                selectedECOSVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedECOSVisibility"));
            }
        }

        public bool IsEnabledMinMax
        {
            get
            {
                return isEnabledMinMax;
            }

            set
            {
                isEnabledMinMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledMinMax"));
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
              //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public bool IsCheckedCopyName
        {
            get
            {
                return isCheckedCopyName;
            }

            set
            {
                isCheckedCopyName = value;
                if (isCheckedCopyName)
                {
                    EnableListBoxForLanguagePCM = false;
                }
                else
                {
                    EnableListBoxForLanguagePCM = true;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyName"));
            }
        }

        public bool IsCheckedCopyNameReadOnly
        {
            get
            {
                return isCheckedCopyNameReadOnly;
            }

            set
            {
                isCheckedCopyNameReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyNameReadOnly"));
            }
        }

        public Language LanguageSelectedInCatelogueDescription
        {
            get
            {
                return languageSelectedInCatelogueDescription;
            }

            set
            {
                languageSelectedInCatelogueDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelectedInCatelogueDescription"));
            }
        }

        public bool IsCheckedCopyCatelogueDescriptionReadOnly
        {
            get
            {
                return isCheckedCopyCatelogueDescriptionReadOnly;
            }

            set
            {
                isCheckedCopyCatelogueDescriptionReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyCatelogueDescriptionReadOnly"));
            }
        }

        public bool IsCheckedCopyCatelogueDescription
        {
            get
            {
                return isCheckedCopyCatelogueDescription;
            }

            set
            {
                isCheckedCopyCatelogueDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyCatelogueDescription"));
                EnableListBoxForLanguagePCMCatelogue = !isCheckedCopyCatelogueDescription;
                CheckOrUncheckCheckBoxPCMCatelogueCopyNameDescription();
            }
        }
        public bool enableListBoxForLanguagePCMCatelogue;
        public bool EnableListBoxForLanguagePCMCatelogue
        {
            get
            {
                return enableListBoxForLanguagePCMCatelogue;
            }
            set
            {
                enableListBoxForLanguagePCMCatelogue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnableListBoxForLanguagePCMCatelogue"));
            }
        }

        public bool EnableListBoxForLanguagePCM
        {
            get
            {
                return enableListBoxForLanguagePCM;
            }

            set
            {
                enableListBoxForLanguagePCM = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnableListBoxForLanguagePCM"));
            }
        }

        public bool IsEmptySpace
        {
            get
            {
                return isEmptySpace;
            }

            set
            {
                isEmptySpace = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMDescription_zh"));
            }
        }

        public bool IsEmptySpaceFromPCMCatelogue
        {
            get
            {
                return isEmptySpaceFromPCMCatelogue;
            }

            set
            {
                isEmptySpaceFromPCMCatelogue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmptySpaceFromPCMCatelogue"));
            }
        }

        public bool IsEnabledCopyNameReadOnly
        {
            get
            {
                return isEnabledCopyNameReadOnly;
            }

            set
            {
                isEnabledCopyNameReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCopyNameReadOnly"));
            }
        }

        public ObservableCollection<PLMArticlePrice> IncludedPLMArticlePriceList
        {
            get
            {
                return includedPLMArticlePriceList;
            }

            set
            {
                includedPLMArticlePriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedPLMArticlePriceList"));
            }
        }

        public ObservableCollection<PLMArticlePrice> NotIncludedPLMArticlePriceList
        {
            get
            {
                return notIncludedPLMArticlePriceList;
            }

            set
            {
                notIncludedPLMArticlePriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotIncludedPLMArticlePriceList"));
            }
        }

        public PLMArticlePrice SelectedIncludedPLMArticlePrice
        {
            get
            {
                return selectedIncludedPLMArticlePrice;
            }

            set
            {
                selectedIncludedPLMArticlePrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncludedPLMArticlePrice"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public PLMArticlePrice SelectedNotIncludedPLMArticlePrice
        {
            get
            {
                return selectedNotIncludedPLMArticlePrice;
            }

            set
            {
                selectedNotIncludedPLMArticlePrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNotIncludedPLMArticlePrice"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public List<LookupValue> LogicList
        {
            get
            {
                return logicList;
            }

            set
            {
                logicList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LogicList"));
            }
        }
        public LookupValue SelectedLogic
        {
            get
            {
                return selectedLogic;
            }
            set
            {
                selectedLogic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLogic"));
            }
        }
        public Visibility GroupBoxVisible
        {
            get
            {
                return groupBoxVisible;
            }

            set
            {
                groupBoxVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupBoxVisible"));
            }
        }
        public string IncludedFirstActiveName
        {
            get
            {
                return includedFirstActiveName;
            }
            set
            {
                includedFirstActiveName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedFirstActiveName"));
            }
        }
        public ImageSource IncludedFirstActiveCurrencyIconImage
        {
            get
            {
                return includedFirstActiveCurrencyIconImage;
            }
            set
            {
                includedFirstActiveCurrencyIconImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedFirstActiveCurrencyIconImage"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public double? IncludedFirstActiveSellPrice
        {
            get
            {
                return includedFirstActiveSellPrice;
            }
            set
            {
                includedFirstActiveSellPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedFirstActiveSellPrice"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public Int64 IncludedActiveCount
        {
            get
            {
                return includedActiveCount;
            }
            set
            {
                includedActiveCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedActiveCount"));
            }
        }

        public string CurrencySymbol
        {
            get
            {
                return currencySymbol;
            }
            set
            {
                currencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencySymbol"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string OnFocusFgColor
        {
            get { return onFocusFgColor; }
            set
            {
                onFocusFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnFocusFgColor"));
            }
        }

        public string SelectedCurrencySymbol
        {
            get { return selectedCurrencySymbol; }
            set
            {
                selectedCurrencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrencySymbol"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public Currency SelectedCurrency
        {
            get { return selectedCurrency; }
            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public bool IsAdded
        {
            get { return isAdded; }
            set
            {

                isAdded = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsAdded"));
            }
        }
        public bool IsAcceptButtonEnabled
        {
            get { return isAcceptButtonEnabled; }
            set
            {

                isAcceptButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButtonEnabled"));
            }
        }
        public bool IsBPLMessageShow
        {
            get { return isBPLMessageShow; }
            set
            {

                isBPLMessageShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBPLMessageShow"));
            }
        }

        public bool IsCPLMessageShow
        {
            get { return isCPLMessageShow; }
            set
            {

                isCPLMessageShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLMessageShow"));
            }
        }

        public bool IsBPLCalculateRuleValue
        {
            get { return isBPLCalculateRuleValue; }
            set
            {

                isBPLCalculateRuleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBPLCalculateRuleValue"));
            }
        }

        public bool IsCPLCalculateRuleValue
        {
            get { return isCPLCalculateRuleValue; }
            set
            {

                isCPLCalculateRuleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLCalculateRuleValue"));
            }
        }

        public bool IsOnlyAcceptButtonEnabled
        {
            get { return isOnlyAcceptButtonEnabled; }
            set
            {

                isOnlyAcceptButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOnlyAcceptButtonEnabled"));
            }
        }

        public ArticleDecomposition SelectedItemArticleDecomposition
        {
            get { return selectedItemArticleDecomposition; }
            set
            {

                selectedItemArticleDecomposition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemArticleDecomposition"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }

        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool AllowDragDrop
        {
            get
            {
                return allowDragDrop;
            }

            set
            {
                allowDragDrop = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllowDragDrop"));
            }
        }
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabled"));
            }
        }

        public ObservableCollection<ArticleCustomer> ArticleCustomerList
        {
            get
            {
                return articleCustomerList;
            }
            set
            {
                articleCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleCustomerList"));
            }
        }

        public ObservableCollection<ArticleSuppliers> ArticleSuppliersList
        {
            get
            {
                return articleSuppliersList;
            }
            set
            {
                articleSuppliersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSuppliersList"));
            }
        }

        public int PartNumberCount
        {
            get
            {
                return partNumberCount;
            }
            set
            {
                partNumberCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PartNumberCount"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ArticleCustomer SelectedArticleCustomer
        {
            get
            {
                return selectedArticleCustomer;
            }
            set
            {
                selectedArticleCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleCustomer"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
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
        public string CustomerReference
        {
            get
            {
                return customerReference;
            }

            set
            {
                customerReference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerReference"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }


        public List<ArticleCustomer> PCMArticleCustomerList
        {
            get
            {
                return pCMArticleCustomerList;
            }
            set
            {
                pCMArticleCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArticleCustomerList"));
            }
        }

        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
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
        public string OldDescription//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
        {
            get { return oldDescription; }
            set
            {
                oldDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldDescription"));
            }
        }

        //[Sudhir.Jangra][GEOS2-1960][10/03/2023]
        public string IsImageScrollVisible
        {
            get { return isImageScrollVisible; }
            set
            {
                isImageScrollVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageScrollVisible"));
            }
        }
        //[Sudhir.Jangra][GEOS2-1960][13/03/2023]
        public string IsAttachmentScrollVisible
        {
            get { return isAttachmentScrollVisible; }
            set
            {
                isAttachmentScrollVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAttachmentScrollVisible"));
            }
        }
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        public ObservableCollection<LinkedArticle> LinkedArticleList
        {
            get
            {
                return linkedarticleList;
            }
            set
            {
                linkedarticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedArticleList"));
            }
        }
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        public int LinkedArticleCount
        {
            get
            {
                return linkedarticleCount;
            }

            set
            {
                linkedarticleCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedArticleCount"));
            }
        }
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        private LinkedArticle _selectedItemLinkedArticle;
        public LinkedArticle SelectedItemLinkedArticle
        {
            get
            {
                return _selectedItemLinkedArticle;
            }
            set
            {
                _selectedItemLinkedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemLinkedArticle"));
                IsEnabledCancelButton = true; 
            }
        }
        //[pramod.misal][GEOS2-8321][Date:11-06-2025]
        public bool IsDetailsChecked
        {
            get { return isDetailsChecked; }
            set
            {
                isDetailsChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetailsChecked"));

            }
        }

        //[pramod.misal][GEOS2-8321][Date:11-06-2025]
        public bool IsPricesChecked
        {
            get { return isPricesChecked; }
            set
            {
                isPricesChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPricesChecked"));
            }
        }
        #endregion

        #region ICommands
        #region[rdixit][21.04.2023][GEOS2-2725]
        public ICommand DragOverRichEditControlEventCommand { get; set; }
        public ICommand LoadedRichEditControlEventCommand { get; set; }
        #endregion
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }

        public ICommand DeleteMandatoryCommand { get; set; }
        public ICommand DeleteSuggestedCommand { get; set; }
        public ICommand DeleteIncompatibleCommand { get; set; }

        public ICommand ImportWarehouseItemsToPCMCommand { get; set; }

        public ICommand CommandOnDragRecordOverMandatoryGrid { get; set; }
        public ICommand CommandDropRecordMandatoryGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropMandatoryGrid { get; set; }

        public ICommand CommandOnDragRecordOverSuggestedGrid { get; set; }
        public ICommand CommandDropRecordSuggestedGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropSuggestedGrid { get; set; }
        public ICommand CommandOnDragRecordOverIncompatibleGrid { get; set; }
        public ICommand CommandDropRecordIncompatibleGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropIncompatibleGrid { get; set; }

        public ICommand EditImageCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }

        public ICommand ExportToExcelCommand { get; set; }

        public ICommand AddFileCommand { get; set; }

        public ICommand OpenSelectedImageCommand { get; set; }
        public ICommand OpenImageGalleryCommand { get; set; }
        public ICommand RestrictOpeningPopUpCommand { get; set; }

        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand ShowReferenceViewCommand { get; set; }
        public ICommand ShowDescriptionViewCommand { get; set; }

        public ICommand SwitchTheTextModeBetweenRichAndNormalCommand { get; set; }
        public ICommand IsRichTextPreviewMouseRightButtonDown { get; set; }

        public ICommand RichEditControl_DocumentLoadedCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand LoadedEventCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand ItemPositionChangedCommand { get; set; }
        public ICommand SelectedeCOSChangedCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeLanguageCommand_PCMCatelogue { get; set; }

        public ICommand ChangeNameCommand { get; set; }
        public ICommand PCMCatalogueDescription_RichEditControlTextChangedCommand { get; set; }

        public ICommand UncheckedCopyNameDescriptionCommand { get; set; }

        public ICommand PCMCatalogueDescription_TextEditControlEditValueChangedCommand { get; set; }

        public ICommand CommandOnDragRecordOverNotIncludedArticleGrid { get; set; }
        public ICommand CommandOnDragRecordOverIncludedArticleGrid { get; set; }

        public ICommand RuleChangedCommand { get; set; }
        public ICommand LostFocusRuleChangedCommand { get; set; }

        public ICommand CommandShowFilterPopupForIncludedClick { get; set; }
        public ICommand CommandShowFilterPopupForNotIncludedClick { get; set; }

        public ICommand GridControlLoadedCommand { get; set; }
        public ICommand GridControlUnloadedCommand { get; set; }

        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand EditArticleCommand { get; set; }

        public ICommand ExpandAndCollapseModuleCommand { get; set; }   //[30.11.2022][sshegaonkar][GEOS2-2718]

        public ICommand ExpandAndCollapseArticleCommand { get; set; }   //[30.11.2022][sshegaonkar][GEOS2-2718]
        public ICommand AddCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }

        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand EditCustomerCommand { get; set; }
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        public ICommand EditLinkedArticleCommand { get; set; }
        #endregion

        #region Constructor

        public EditPCMArticleViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log(string.Format("Constructor EditPCMArticleViewModel()..."), category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;

                LoadedRichEditControlEventCommand = new RelayCommand(new Action<object>(RichEditControl_Loaded));//[rdixit][21.04.2023][GEOS2-2725]
                DragOverRichEditControlEventCommand = new RelayCommand(new Action<object>(RichEditControl_DragOver));//[rdixit][21.04.2023][GEOS2-2725]
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);

                DeleteMandatoryCommand = new DelegateCommand<object>(DeleteMandatory);
                DeleteSuggestedCommand = new DelegateCommand<object>(DeleteSuggested);
                DeleteIncompatibleCommand = new DelegateCommand<object>(DeleteIncompatible);

                EditImageCommand = new DelegateCommand<object>(EditImageAction);
                AddImageCommand = new DelegateCommand<object>(AddImageAction);
                DeleteImageCommand = new DelegateCommand<object>(DeleteImageAction);

                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);

                AddFileCommand = new DelegateCommand<object>(AddFile);
                EditFileCommand = new DelegateCommand<object>(EditFile);
                EditCustomerCommand = new DelegateCommand<object>(EditCustomerCommandAction);
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));


                OpenImageGalleryCommand = new RelayCommand(new Action<object>(OpenImageGalleryAction));
                RestrictOpeningPopUpCommand = new DelegateCommand<object>(RestrictOpeningPopUpAction);
                OpenSelectedImageCommand = new DelegateCommand<object>(OpenSelectedImageAction);

                ShowReferenceViewCommand = new DelegateCommand<object>(ShowReferenceViewCommandAction);
                ShowDescriptionViewCommand = new DelegateCommand<object>(ShowDescriptionViewCommandAction);

                //compatibility commands
                IsModuleExpand = true;
                IsArticleExpand = true;
                ExpandAndCollapseArticleCommand = new DelegateCommand<object>(ExpandAndCollapseArticleCommandAction);  //[30.11.2022][sshegaonkar][GEOS2-2718]
                ExpandAndCollapseModuleCommand = new DelegateCommand<object>(ExpandAndCollapseModuleCommandAction);  //[30.11.2022][sshegaonkar][GEOS2-2718]
                CommandOnDragRecordOverMandatoryGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverMandatoryGrid);
                CommandDropRecordMandatoryGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordMandatoryGrid);
                CommandCompleteRecordDragDropMandatoryGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropMandatoryGrid);

                CommandOnDragRecordOverSuggestedGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSuggestedGrid);
                CommandDropRecordSuggestedGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordSuggestedGrid);
                CommandCompleteRecordDragDropSuggestedGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropSuggestedGrid);

                CommandOnDragRecordOverIncompatibleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverIncompatibleGrid);
                CommandDropRecordIncompatibleGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordIncompatibleGrid);
                CommandCompleteRecordDragDropIncompatibleGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropIncompatibleGrid);

                SwitchTheTextModeBetweenRichAndNormalCommand = new DelegateCommand<object>(SwitchTheTextModeBetweenRichAndNormalCommandAction);
                RichEditControl_DocumentLoadedCommand = new DelegateCommand<object>(RichEditControl_DocumentLoadedCommandAction);
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowing);

                LoadedEventCommand = new RelayCommand(new Action<object>(UserControl_Loaded));
                DeleteFileCommand = new DelegateCommand<object>(DelateAttachmentFile);

                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                SelectedeCOSChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedeCOSChangedCommandAction);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameByLanguge);
                ChangeLanguageCommand_PCMCatelogue = new DelegateCommand<object>(RetrieveNameByLanguge_PCMCatelogue);

                ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                PCMCatalogueDescription_RichEditControlTextChangedCommand = new DelegateCommand<object>(PCMCatalogueDescription_RichEditControlTextChangedCommandAction);
                UncheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UncheckedCopyNameDescription);

                PCMCatalogueDescription_TextEditControlEditValueChangedCommand = new DelegateCommand<object>(PCMCatalogueDescription_TextEditControlEditValueChangedCommandAction);

                CommandOnDragRecordOverNotIncludedArticleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverNotIncludedArticleGrid);

                CommandOnDragRecordOverIncludedArticleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverIncludedArticleGrid);

                RuleChangedCommand = new DelegateCommand<object>(RuleChangedCommandAction);
                LostFocusRuleChangedCommand = new DelegateCommand<object>(LostFocusRuleChangedCommandAction);

                CommandShowFilterPopupForIncludedClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForIncluded);
                CommandShowFilterPopupForNotIncludedClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForNotIncluded);

                GridControlLoadedCommand = new DelegateCommand<object>(GridControlLoadedAction);
                GridControlUnloadedCommand = new DelegateCommand<object>(GridControlUnloadedAction);
                EditArticleCommand = new RelayCommand(new Action<object>(EditArticleAction));
                //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
                EditLinkedArticleCommand = new RelayCommand(new Action<object>(EditLinkedArticleAction));
                AddCustomerCommand = new DelegateCommand<object>(AddCustomerCommandAction);
                DeleteCustomerCommand = new DelegateCommand<object>(DeleteCustomerCommandAction);
                //CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                ImportWarehouseItemsToPCMCommand = new DelegateCommand<object>(ImportWarehouseItemsToPCM);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                {
                    IsReadOnlyName = false;
                    IsCheckedCopyNameReadOnly = false;
                    IsEnabledCopyNameReadOnly = true;
                }
                else
                {
                    IsReadOnlyName = true;
                    IsCheckedCopyNameReadOnly = false;
                    IsEnabledCopyNameReadOnly = false;
                }


                if (GeosApplication.Instance.IsPermissionForManageItemPrice == true)
                {
                    GroupBoxVisible = Visibility.Visible;
                }
                else
                {
                    GroupBoxVisible = Visibility.Collapsed;
                }

                if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    OnFocusFgColor = "White";
                }
                else
                {
                    OnFocusFgColor = "Black";
                }

                List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                SelectedCurrencySymbol = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"]).Symbol;
                SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"]);
                IsOnlyAcceptButtonEnabled = true;
                GeosApplication.Instance.Logger.Log(string.Format("Constructor EditPCMArticleViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);




            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor EditPCMArticleViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditArticleAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()..."), category: Category.Info, priority: Priority.Low);



                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                Int64 IdArticle = 0;
                TreeListView detailView = (TreeListView)obj;
                if (((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows[0] != null && ((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows.Count > 0)
                {
                    IdArticle = ((Emdep.Geos.Data.Common.ArticleDecomposition)(((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows[0])).IdArticle;
                    SelectedItemArticleDecomposition = ArticleDecompositionList.Where(i => i.IdArticle == IdArticle).FirstOrDefault();
                    if (SelectedItemArticleDecomposition != null)
                    {
                        Articles articles = new Articles();
                        articles.IdArticle = Convert.ToUInt32(SelectedItemArticleDecomposition.IdArticle);
                        if (SelectedItemArticleDecomposition.PCMArticle != null)
                        {
                            EditPCMArticleView editPCMArticleView = new EditPCMArticleView();
                            EditPCMArticleViewModel editPCMArticleViewModel = new EditPCMArticleViewModel();
                            EventHandler handle = delegate { editPCMArticleView.Close(); };
                            editPCMArticleViewModel.RequestClose += handle;
                            editPCMArticleViewModel.IsNew = false;

                            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                            editPCMArticleViewModel.EditInit(articles);
                            editPCMArticleView.DataContext = editPCMArticleViewModel;
                            var ownerInfo = (detailView as FrameworkElement);
                            editPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            editPCMArticleView.ShowDialog();
                        }
                        else
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(Application.Current.Resources["PCM_NotPCMArticle"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                        }
                    }

                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }





                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }

        private void PCMCatalogueDescription_TextEditControlEditValueChangedCommandAction(object obj)
        {
            try
            {
                //// Not added log because the method is executed continuously while typing
                //GeosApplication.Instance.Logger.Log("Method PCMCatalogueDescription_TextEditControlEditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);

                if (IsRtf)
                    return;
                TextEdit txt = ((DevExpress.Xpf.Editors.TextEditBase)(obj)) as TextEdit;
                if (txt.DisplayText == Convert.ToString(txt.EditValue))
                {
                    return;
                }
                var ObjTextEditControl = (TextEdit)obj;

                PcmArticleCatalogueDescriptionManager.UpdateEnteredNormalTextForCurrentOnelanguage(ObjTextEditControl);

                //GeosApplication.Instance.Logger.Log("Method PCMCatalogueDescription_TextEditControlEditValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage_PCMCatelogue() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion

        #region Command Actions

        //compatibility command actions

        private void OnDragRecordOverMandatoryGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverMandatoryGrid()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && typeof(ProductTypesTemplate).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));

                    if (data.Records.OfType<ProductTypesTemplate>().Select(a => a.IdCPType).FirstOrDefault() > 0)
                    {
                        List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 254, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
                        if (newRecords.Count > 0)
                        {
                            int countMandatoryList = MandatoryList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            int countSuggestedList = SuggestedList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            int countIncompatibleList = IncompatibleList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            if (countMandatoryList == 0 && countSuggestedList == 0 && countIncompatibleList == 0)
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
                    }

                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    if (data.Records.OfType<PCMArticleCategory>().Select(a => a.IdArticle).FirstOrDefault() > 0)
                    {
                        List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 254, Code = x.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
                        if (newRecords.Count > 0)
                        {
                            int countMandatoryList = MandatoryList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            int countSuggestedList = SuggestedList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            int countIncompatibleList = IncompatibleList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            if (countMandatoryList == 0 && countSuggestedList == 0 && countIncompatibleList == 0)
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
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverMandatoryGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverMandatoryGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DropRecordMandatoryGrid(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordMandatoryGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(ProductTypesTemplate).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 254, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();

                    if (newRecords.Count > 0)
                    {
                        MandatoryList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 254, Code = x.Reference, Name = x.Description, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
                    if (newRecords.Count > 0)
                    {
                        MandatoryList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DropRecordMandatoryGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordMandatoryGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropMandatoryGrid(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropMandatoryGrid()...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropMandatoryGrid()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropMandatoryGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverSuggestedGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSuggestedGrid()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && typeof(ProductTypesTemplate).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    if (data.Records.OfType<ProductTypesTemplate>().Select(a => a.IdCPType).FirstOrDefault() > 0)
                    {
                        List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 255, Code = x.ProductType.Reference, Name = x.Name, Remarks = "" }).ToList();
                        if (newRecords.Count > 0)
                        {
                            int countMandatoryList = MandatoryList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            int countSuggestedList = SuggestedList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            int countIncompatibleList = IncompatibleList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            if (countMandatoryList == 0 && countSuggestedList == 0 && countIncompatibleList == 0)
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
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    if (data.Records.OfType<PCMArticleCategory>().Select(a => a.IdArticle).FirstOrDefault() > 0)
                    {
                        List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 255, Code = x.Reference, Name = x.Name, Remarks = "" }).ToList();
                        if (newRecords.Count > 0)
                        {
                            int countMandatoryList = MandatoryList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            int countSuggestedList = SuggestedList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            int countIncompatibleList = IncompatibleList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            if (countMandatoryList == 0 && countSuggestedList == 0 && countIncompatibleList == 0)
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
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSuggestedGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSuggestedGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DropRecordSuggestedGrid(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordSuggestedGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(ProductTypesTemplate).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 255, Code = x.ProductType.Reference, Name = x.Name, Remarks = "" }).ToList();
                    if (newRecords.Count > 0)
                    {
                        SuggestedList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 255, Code = x.Reference, Name = x.Description, Remarks = "" }).ToList();
                    if (newRecords.Count > 0)
                    {
                        SuggestedList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DropRecordSuggestedGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordSuggestedGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropSuggestedGrid(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropSuggestedGrid()...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropSuggestedGrid()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropSuggestedGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void OnDragRecordOverIncompatibleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncompatibleGrid()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && typeof(ProductTypesTemplate).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    if (data.Records.OfType<ProductTypesTemplate>().Select(a => a.IdCPType).FirstOrDefault() > 0)
                    {
                        List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 256, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251 }).ToList();
                        if (newRecords.Count > 0)
                        {
                            int countMandatoryList = MandatoryList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            int countSuggestedList = SuggestedList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            int countIncompatibleList = IncompatibleList.Where(a => a.IdCPtypeCompatibility == newRecords.FirstOrDefault().IdCPtypeCompatibility).Count();
                            if (countMandatoryList == 0 && countSuggestedList == 0 && countIncompatibleList == 0)
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
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    if (data.Records.OfType<PCMArticleCategory>().Select(a => a.IdArticle).FirstOrDefault() > 0)
                    {
                        List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 256, Code = x.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251 }).ToList();


                        if (newRecords.Count > 0)
                        {
                            int countMandatoryList = MandatoryList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            int countSuggestedList = SuggestedList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();
                            int countIncompatibleList = IncompatibleList.Where(a => a.IdArticleCompatibility == newRecords.FirstOrDefault().IdArticleCompatibility).Count();


                            if (countMandatoryList == 0 && countSuggestedList == 0 && countIncompatibleList == 0)
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
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncompatibleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverIncompatibleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DropRecordIncompatibleGrid(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordIncompatibleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(ProductTypesTemplate).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 256, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251 }).ToList();
                    if (newRecords.Count > 0)
                    {
                        IncompatibleList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 256, Code = x.Reference, Name = x.Description, Remarks = "", IdRelationshipType = 251 }).ToList();
                    if (newRecords.Count > 0)
                    {
                        IncompatibleList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DropRecordIncompatibleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordIncompatibleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropIncompatibleGrid(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropIncompatibleGrid()...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropIncompatibleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropIncompatibleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SwitchTheTextModeBetweenRichAndNormalCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SwitchTheTextModeBetweenRichAndNormalCommandAction()...", category: Category.Info, priority: Priority.Low);

                PcmArticleCatalogueDescriptionManager.SwitchTheTextModeBetweenRichAndNormal();

                var PCMDescription_Richtext1 = PCMDescription_Richtext;
                var PCMDescription1 = PCMDescription;

                PcmArticleCatalogueDescriptionManager.GetTextForCurrentOnelanguage(
                    ref PCMDescription_Richtext1, ref PCMDescription1);

                PCMDescription = PCMDescription1;
                PCMDescription_Richtext = PCMDescription_Richtext1;

                if (IsRtf)
                {
                    Richtextboxrtf = Visibility.Visible;
                    Textboxnormal = Visibility.Collapsed;
                }
                else
                {
                    Textboxnormal = Visibility.Visible;
                    Richtextboxrtf = Visibility.Collapsed;
                }
                GeosApplication.Instance.Logger.Log("Method SwitchTheTextModeBetweenRichAndNormalCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SwitchTheTextModeBetweenRichAndNormalCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RichEditControl_DocumentLoadedCommandAction(object sender)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method RichEditControl_DocumentLoadedCommandAction()...", category: Category.Info, priority: Priority.Low);
                RichEditControl ObjRichEditControl = (RichEditControl)sender;

                Document document = ObjRichEditControl.Document;
                document.Unit = DevExpress.Office.DocumentUnit.Inch;
                document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
                document.Sections[0].Page.Landscape = true;
                document.Sections[0].Margins.Left = 0.0f;
                document.Sections[0].Margins.Top = 0.0f;
                document.Sections[0].Margins.Right = 0.0f;
                document.Sections[0].Margins.Bottom = 0.0f;
                GeosApplication.Instance.Logger.Log("Method RichEditControl_DocumentLoadedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method RichEditControl_DocumentLoadedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void PopupMenuShowing(PopupMenuShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()...", category: Category.Info, priority: Priority.Low);
                ((DevExpress.Xpf.RichEdit.Menu.RichEditPopupMenu)obj.Menu).Items.Clear();
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PopupMenuShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods

        public void Init()
        {
            MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

            FillCategoryList();
            FillStatusList();
            FillECOSVisibilityList();

            FillModuleMenuList();
            //FillArticleMenuList();
            FillReferenceView();
            FillRelationShipList();
            AddChangeLogsMenu();
            AddLanguages();
            FillLogicList();
            PQuantityMin = 1;
            PQuantityMax = 1;
            IsAcceptButtonEnabled = true;
        }

        /// <summary>
        /// [001] [vsana][07-01-2021][GEOS2-2785] [Manage the visibility of attachments in PCM Articles [#PCM57]]
        /// </summary>
        /// <returns></returns>
        public void EditInit(Articles selectedArticle)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);

                Init();
                IsAcceptButtonEnabled = true;
                //[001] Changed service method GetArticleByIdArticle_V2090 to GetArticleByIdArticle_V2100
                //[001][adhatkar][3196] Changed service method GetArticleByIdArticle_V2120 to GetArticleByIdArticle_V2170
                //[001][skadam][GEOS2-3607] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 1    

                //Articles temp = (PCMService.GetArticleByIdArticle_V2290(selectedArticle.IdArticle, GeosApplication.Instance.ActiveUser.IdUser));
                Articles temp = new Articles();
                //PCMService = new PCMServiceController("localhost:6699");
                if (selectedArticle.IsHardLockPluginEditView)
                {
                    //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
                    //temp = (PCMService.GetArticleByIdArticle_V2440(selectedArticle.IdArticle, selectedArticle.IdPCMArticle, GeosApplication.Instance.ActiveUser.IdUser));
                    temp = (PCMService.GetArticleByIdArticle_V2660(selectedArticle.IdArticle, selectedArticle.IdPCMArticle, GeosApplication.Instance.ActiveUser.IdUser));
                }
                else
                {
                    //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
                    //temp = (PCMService.GetArticleByIdArticle_V2350(selectedArticle.IdArticle, GeosApplication.Instance.ActiveUser.IdUser));
                    temp = (PCMService.GetArticleByIdArticle_V2660_temp(selectedArticle.IdArticle, GeosApplication.Instance.ActiveUser.IdUser));
                }


                if (temp.IncludedPLMArticleList != null)
                    temp.IncludedPLMArticleList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMArticleList != null)
                    temp.NotIncludedPLMArticleList.RemoveAll(r => r.IdPermission == null);
                ClonedArticle = (Articles)temp.Clone();
                ClonedArticle.ArticleCustomerList.Select(item => (ArticleCustomer)item.Clone()).ToList();
                IdPCMArticleCategory = temp.IdPCMArticleCategory;
                IdArticle = temp.IdArticle;

                Reference = temp.Reference;
                OldDescription = temp.Description;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                Description = temp.Description;
                if (!string.IsNullOrEmpty(temp.Description))
                {
                    Description_en = temp.Description;
                    oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                }
                if (!string.IsNullOrEmpty(temp.Description_es))
                {
                    Description_es = temp.Description_es;
                    oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                }
                if (!string.IsNullOrEmpty(temp.Description_fr))
                {
                    Description_fr = temp.Description_fr;
                    oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                }
                if (!string.IsNullOrEmpty(temp.Description_pt))
                {
                    Description_pt = temp.Description_pt;
                    oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                }
                if (!string.IsNullOrEmpty(temp.Description_ro))
                {
                    Description_ro = temp.Description_ro;
                    oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                }
                if (!string.IsNullOrEmpty(temp.Description_ru))
                {
                    Description_ru = temp.Description_ru;
                    oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                }
                if (!string.IsNullOrEmpty(temp.Description_zh))
                {
                    Description_zh = temp.Description_zh;
                    oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;
                }



                bool IsRtf1 = IsRtf;

                bool IsNormal1 = IsNormal;
                string PCMDescription_Richtext1 = pCMDescription_Richtext;
                string PCMDescription1 = PCMDescription;
                bool IsCheckedCopyCatelogueDescription1 = IsCheckedCopyCatelogueDescription;
                Visibility Richtextboxrtf1 = Richtextboxrtf;
                Visibility Textboxnormal1 = Textboxnormal;
                PcmArticleCatalogueDescriptionManager.SetNormalAndRichTextForAllLanguagesFromDatabaseData(
                    temp,
                    ref IsRtf1, ref IsNormal1,
                    ref IsCheckedCopyCatelogueDescription1,
                    ref PCMDescription_Richtext1,
                    ref PCMDescription1, ref Richtextboxrtf1, ref Textboxnormal1,
                    this);


                IsRtf = IsRtf1;
                IsNormal = IsNormal1;
                IsCheckedCopyCatelogueDescription = IsCheckedCopyCatelogueDescription1;
                PCMDescription = PCMDescription1;
                PCMDescription_Richtext = PCMDescription_Richtext1;
                Richtextboxrtf = Richtextboxrtf1;
                Textboxnormal = Textboxnormal1;

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyDescription == true)
                {
                    IsCheckedCopyName = true;
                }
                else
                {
                    IsCheckedCopyName = false;
                    //IsEnabledCopyNameReadOnly = false;
                }

                Width = temp.Width;
                Height = temp.Height;
                Length = temp.Length;

                PQuantityMin = temp.PurchaseQtyMin;
                PQuantityMax = temp.PurchaseQtyMax;

                double weightToConvert = System.Convert.ToDouble(temp.Weight);

                if (System.Convert.ToDouble(temp.Weight) < 1)
                {
                    if (Math.Round(weightToConvert * 1000, 0) == 1000)
                    {
                        Weight = Math.Round(weightToConvert * 1000, 0);
                        ArticleWeightSymbol = " (Kg) :";
                    }
                    Weight = Math.Round(weightToConvert * 1000, 0);
                    ArticleWeightSymbol = " (gr) :";
                }
                else
                {
                    Weight = Math.Round(weightToConvert, 3);
                    ArticleWeightSymbol = " (Kg) :";
                }

                SelectedCategory = CategoryList.FirstOrDefault(x => x.IdPCMArticleCategory == temp.IdPCMArticleCategory);
                if (temp.IdPCMStatus != null)
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdPCMStatus);
                }
                else
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 225);
                    ClonedArticle.IdPCMStatus = 225;
                }

                if (temp.IdECOSVisibility != 0)
                {
                    SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == temp.IdECOSVisibility);
                }
                else
                {
                    SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == 323);
                    ClonedArticle.IdECOSVisibility = 323;
                }

                WarehouseStatus = temp.WarehouseStatus;
                Supplier = temp.SupplierName;

                //Compatiblity
                if (temp.ArticleCompatibilityList != null)
                {
                    MandatoryList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 254));
                    SuggestedList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 255));
                    IncompatibleList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 256));
                }
                else
                {
                    MandatoryList = new ObservableCollection<ArticleCompatibility>();
                    SuggestedList = new ObservableCollection<ArticleCompatibility>();
                    IncompatibleList = new ObservableCollection<ArticleCompatibility>();
                }

                GetCompatibilityCount();
                ArticleDecompositionList = new ObservableCollection<ArticleDecomposition>(temp.ArticleDecompostionList);
                ArticleComponentsCount = ArticleDecompositionList.Where(i => i.IdParent == temp.IdArticle).ToList().Count();
                if (ArticleDecompositionList.Any(i => i.PCMArticle == null))//[GEOS2-3785][rdixit][21.07.2022]
                {
                    IsPOArticleNull = true;
                }
                else
                {
                    IsPOArticleNull = false;
                }
                foreach (ArticleDecomposition item in ArticleDecompositionList.Where(i => i.PCMArticle != null))
                {
                    if (ECOSVisibilityList.Any(x => x.IdLookupValue == item.PCMArticle.IdECOSVisibility))
                    {
                        item.PCMArticle.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == item.PCMArticle.IdECOSVisibility).HtmlColor;
                        item.PCMArticle.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == item.PCMArticle.IdECOSVisibility).Value;
                    }


                }
                //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
                LinkedArticleList = new ObservableCollection<LinkedArticle>(temp.LinkedArticleList);
                LinkedArticleCount = LinkedArticleList.Count;
                ArticleChangeLogList = new ObservableCollection<PCMArticleLogEntry>(ClonedArticle.PCMArticleLogEntiryList);
                if (ArticleChangeLogList.Count > 0)
                    SelectedArticleChangeLog = ArticleChangeLogList.FirstOrDefault();

                ImagesList = new ObservableCollection<PCMArticleImage>(temp.PCMArticleImageList);

                if (temp.ArticleImageInBytes != null)
                {
                    if (ImagesList != null && ImagesList.Count > 0)
                    {
                        for (ulong pos = 1; pos <= ImagesList.Max(a => a.Position) + 1; pos++)
                        {
                            if (!(ImagesList.Any(a => a.Position == pos)))
                            {
                                PCMArticleImage Image = new PCMArticleImage();
                                Image.PCMArticleImageInBytes = temp.ArticleImageInBytes;
                                Image.IsWarehouseImage = 1;
                                Image.SavedFileName = Reference;
                                Image.OriginalFileName = Reference;
                                Image.Position = pos;
                                Image.IsImageShareWithCustomer = temp.IsImageShareWithCustomer;
                                ImagesList.Add(Image);
                                OldWarehouseImage = ImagesList.FirstOrDefault(a => a.IsWarehouseImage == 1);
                                oldWarehouseposition = OldWarehouseImage.Position;

                                break;
                            }
                        }
                    }
                    else
                    {
                        PCMArticleImage Image = new PCMArticleImage();
                        Image.PCMArticleImageInBytes = temp.ArticleImageInBytes;
                        Image.IsWarehouseImage = 1;
                        Image.SavedFileName = Reference;
                        Image.OriginalFileName = Reference;
                        Image.Position = 1;
                        Image.IsImageShareWithCustomer = temp.IsImageShareWithCustomer;
                        ImagesList.Add(Image);
                    }
                }

                ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).ToList());

                if (temp.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                {
                    ReferenceImage = ByteArrayToBitmapImage(temp.ArticleImageInBytes);
                }
                else
                {
                    if (temp.ArticleImageInBytes == null && (ImagesList.Count == 0))
                    {
                        ReferenceImage = ByteArrayToBitmapImage(selectedArticle.ArticleImageInBytes);
                        OldReferenceImage = ReferenceImage;
                    }
                    else
                        ReferenceImage = ByteArrayToBitmapImage(ImagesList.FirstOrDefault(a => a.Position == 1).PCMArticleImageInBytes);
                }
                OldReferenceImage = ReferenceImage;

                if (ImagesList.Count > 0)
                {
                    List<PCMArticleImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                    List<PCMArticleImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    PCMArticleImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = ImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);
                    SelectedDefaultImage = new PCMArticleImage();
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);

                    SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                }

                if (ImagesList.Count > 0)
                    SelectedImage = ImagesList.FirstOrDefault();

                foreach (PCMArticleImage item in ImagesList)
                {
                    item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(item.PCMArticleImageInBytes);
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3" &&
                        GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (ImagesList.Count() >= 5)
                        {
                            IsImageScrollVisible = "Visible";
                        }
                        else
                        {
                            IsImageScrollVisible = "Disabled";
                        }
                    }
                    else
                    {
                        IsImageScrollVisible = "Disabled";
                    }
                    if (GeosApplication.Instance.PCMImage == "1")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                IsImageShareWithCustomer = temp.IsImageShareWithCustomer;

                //Attachments
                ArticleFilesList = new ObservableCollection<ArticleDocument>(temp.PCMArticleAttachmentList);
                ArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn));
                if (ArticleFilesList.Count > 0)
                    SelectedArticleFile = ArticleFilesList.FirstOrDefault();
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3"
                        && GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (ArticleFilesList.Count() >= 5)
                        {
                            IsAttachmentScrollVisible = "Visible";
                        }
                        else
                        {
                            IsAttachmentScrollVisible = "Disabled";
                        }
                    }
                    else
                    {
                        IsAttachmentScrollVisible = "Disabled";
                    }
                    if (GeosApplication.Instance.PCMAttachment == "1")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).ToList());
                    }
                }
                //PART NUMBER
                ArticleCustomerList = new ObservableCollection<ArticleCustomer>(temp.ArticleCustomerList);
                ArticleCustomerList = new ObservableCollection<ArticleCustomer>(ArticleCustomerList.OrderBy(x => x.ModificationDate));
                //if (ArticleCustomerList.Count > 0)
                //    SelectedArticleCustomer = ArticleCustomerList.FirstOrDefault();
                ArticleSuppliersList = new ObservableCollection<ArticleSuppliers>(temp.ArticleSuppliersList);
                ArticleSuppliersList = new ObservableCollection<ArticleSuppliers>(ArticleSuppliersList.OrderBy(x => x.IdGroup));
                if (ArticleCustomerList.Count > 0 || ArticleSuppliersList.Count > 0)
                {
                    if (ArticleCustomerList == null || ArticleSuppliersList == null)
                    {
                        ArticleCustomerList = new ObservableCollection<ArticleCustomer>();
                        ArticleSuppliersList = new ObservableCollection<ArticleSuppliers>();
                    }
                    PartNumberCount = ArticleCustomerList.Count + ArticleSuppliersList.Count;
                }

                //[adhatkar][GEOS2-3196][30-07-21]
                IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(temp.IncludedPLMArticleList);
                NotIncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(temp.NotIncludedPLMArticleList);

                if (IncludedPLMArticlePriceList != null)
                {
                    foreach (var included in IncludedPLMArticlePriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(included.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        included.ToList().Where(inc => inc.Currency.Name == included.Key).ToList().ForEach(inc => inc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                if (NotIncludedPLMArticlePriceList != null)
                {
                    foreach (var notIncluded in NotIncludedPLMArticlePriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(notIncluded.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        notIncluded.ToList().Where(notinc => notinc.Currency.Name == notIncluded.Key).ToList().ForEach(notinc => notinc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                SelectedIncludedPLMArticlePrice = IncludedPLMArticlePriceList.FirstOrDefault();
                SelectedNotIncludedPLMArticlePrice = NotIncludedPLMArticlePriceList.FirstOrDefault();
                IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }
                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                        //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMArticlePriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMArticlePriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                        //{
                        //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                        //    {

                        //    }
                        //    CurrencySymbol = SelectedCurrencySymbol;
                        //}
                        //else
                        //{
                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                        //}
                    }



                    //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                    //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                    //else
                    //    IncludedFirstActiveSellPrice = null;

                    //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                    //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                    //    CurrencySymbol = "";
                    //else
                    //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                }
                else
                {
                    IncludedFirstActiveName = "";
                    IncludedFirstActiveCurrencyIconImage = null;
                    IncludedFirstActiveSellPrice = null;
                    IncludedActiveCount = 0;
                    CurrencySymbol = "";
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001] [vsana][07-01-2021][GEOS2-2785] [Manage the visibility of attachments in PCM Articles [#PCM57]]
        /// [002] [cpatil][28-04-2022][GEOS2-3726] 
        /// </summary>
        /// <returns></returns>
        public void PLMEditInit(UInt32 idArticle)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method PLMEditInit..."), category: Category.Info, priority: Priority.Low);

                Init();
                IsAcceptButtonEnabled = true;
                IsOnlyAcceptButtonEnabled = false;
                //[001] Changed service method GetArticleByIdArticle_V2090 to GetArticleByIdArticle_V2100
                //[001][adhatkar][3196] Changed service method GetArticleByIdArticle_V2120 to GetArticleByIdArticle_V2170
                //[001][skadam][GEOS2-3607] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 1

                //Articles temp = (PCMService.GetArticleByIdArticle_V2250(idArticle, GeosApplication.Instance.ActiveUser.IdUser));

                //[pramod.misal][GEOS2-8321][Date:27-06-2025]
                //PCMService = new PCMServiceController("localhost:6699");
                Articles temp = (PCMService.GetPLMArticleByIdArticle_V2660(idArticle, GeosApplication.Instance.ActiveUser.IdUser));
                LinkedArticleList = new ObservableCollection<LinkedArticle>(temp.LinkedArticleList);
                LinkedArticleCount = LinkedArticleList.Count;
                //[002] Added
                if (temp.IncludedPLMArticleList != null)
                    temp.IncludedPLMArticleList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMArticleList != null)
                    temp.NotIncludedPLMArticleList.RemoveAll(r => r.IdPermission == null);
                ClonedArticle = (Articles)temp.Clone();

                IdPCMArticleCategory = temp.IdPCMArticleCategory;
                IdArticle = temp.IdArticle;

                Reference = temp.Reference;

                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;

                bool IsRtf1 = IsRtf;

                bool IsNormal1 = IsNormal;
                string PCMDescription_Richtext1 = pCMDescription_Richtext;
                string PCMDescription1 = PCMDescription;
                bool IsCheckedCopyCatelogueDescription1 = IsCheckedCopyCatelogueDescription;
                Visibility Richtextboxrtf1 = Richtextboxrtf;
                Visibility Textboxnormal1 = Textboxnormal;
                PcmArticleCatalogueDescriptionManager.SetNormalAndRichTextForAllLanguagesFromDatabaseData(
                    temp,
                    ref IsRtf1, ref IsNormal1,
                    ref IsCheckedCopyCatelogueDescription1,
                    ref PCMDescription_Richtext1,
                    ref PCMDescription1, ref Richtextboxrtf1, ref Textboxnormal1,
                    this);


                IsRtf = IsRtf1;
                IsNormal = IsNormal1;
                IsCheckedCopyCatelogueDescription = IsCheckedCopyCatelogueDescription1;
                PCMDescription = PCMDescription1;
                PCMDescription_Richtext = PCMDescription_Richtext1;
                Richtextboxrtf = Richtextboxrtf1;
                Textboxnormal = Textboxnormal1;

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyDescription == true)
                {
                    IsCheckedCopyName = true;
                }
                else
                {
                    IsCheckedCopyName = false;
                    //IsEnabledCopyNameReadOnly = false;
                }

                Width = temp.Width;
                Height = temp.Height;
                Length = temp.Length;

                PQuantityMin = temp.PurchaseQtyMin;
                PQuantityMax = temp.PurchaseQtyMax;

                double weightToConvert = System.Convert.ToDouble(temp.Weight);

                if (System.Convert.ToDouble(temp.Weight) < 1)
                {
                    if (Math.Round(weightToConvert * 1000, 0) == 1000)
                    {
                        Weight = Math.Round(weightToConvert * 1000, 0);
                        ArticleWeightSymbol = " (Kg) :";
                    }
                    Weight = Math.Round(weightToConvert * 1000, 0);
                    ArticleWeightSymbol = " (gr) :";
                }
                else
                {
                    Weight = Math.Round(weightToConvert, 3);
                    ArticleWeightSymbol = " (Kg) :";
                }

                SelectedCategory = CategoryList.FirstOrDefault(x => x.IdPCMArticleCategory == temp.IdPCMArticleCategory);
                if (temp.IdPCMStatus != null)
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdPCMStatus);
                }
                else
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 225);
                    ClonedArticle.IdPCMStatus = 225;
                }

                if (temp.IdECOSVisibility != 0)
                {
                    SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == temp.IdECOSVisibility);
                }
                else
                {
                    SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == 323);
                    ClonedArticle.IdECOSVisibility = 323;
                }

                WarehouseStatus = temp.WarehouseStatus;
                Supplier = temp.SupplierName;

                //Compatiblity
                if (temp.ArticleCompatibilityList != null)
                {
                    MandatoryList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 254));
                    SuggestedList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 255));
                    IncompatibleList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 256));
                }
                else
                {
                    MandatoryList = new ObservableCollection<ArticleCompatibility>();
                    SuggestedList = new ObservableCollection<ArticleCompatibility>();
                    IncompatibleList = new ObservableCollection<ArticleCompatibility>();
                }

                GetCompatibilityCount();

                ArticleChangeLogList = new ObservableCollection<PCMArticleLogEntry>(ClonedArticle.PCMArticleLogEntiryList);
                if (ArticleChangeLogList.Count > 0)
                    SelectedArticleChangeLog = ArticleChangeLogList.FirstOrDefault();

                ImagesList = new ObservableCollection<PCMArticleImage>(temp.PCMArticleImageList);

                if (temp.ArticleImageInBytes != null)
                {
                    if (ImagesList != null && ImagesList.Count > 0)
                    {
                        for (ulong pos = 1; pos <= ImagesList.Max(a => a.Position) + 1; pos++)
                        {
                            if (!(ImagesList.Any(a => a.Position == pos)))
                            {
                                PCMArticleImage Image = new PCMArticleImage();
                                Image.PCMArticleImageInBytes = temp.ArticleImageInBytes;
                                Image.IsWarehouseImage = 1;
                                Image.SavedFileName = Reference;
                                Image.OriginalFileName = Reference;
                                Image.Position = pos;
                                Image.IsImageShareWithCustomer = temp.IsImageShareWithCustomer;
                                ImagesList.Add(Image);
                                OldWarehouseImage = ImagesList.FirstOrDefault(a => a.IsWarehouseImage == 1);
                                oldWarehouseposition = OldWarehouseImage.Position;

                                break;
                            }
                        }
                    }
                    else
                    {
                        PCMArticleImage Image = new PCMArticleImage();
                        Image.PCMArticleImageInBytes = temp.ArticleImageInBytes;
                        Image.IsWarehouseImage = 1;
                        Image.SavedFileName = Reference;
                        Image.OriginalFileName = Reference;
                        Image.Position = 1;
                        Image.IsImageShareWithCustomer = temp.IsImageShareWithCustomer;
                        ImagesList.Add(Image);
                    }
                }

                ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).ToList());

                if (temp.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                {
                    ReferenceImage = ByteArrayToBitmapImage(temp.ArticleImageInBytes);
                }

                OldReferenceImage = ReferenceImage;

                if (ImagesList.Count > 0)
                {
                    List<PCMArticleImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                    List<PCMArticleImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    PCMArticleImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = ImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);
                    SelectedDefaultImage = new PCMArticleImage();
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);

                    SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                }

                if (ImagesList.Count > 0)
                    SelectedImage = ImagesList.FirstOrDefault();

                foreach (PCMArticleImage item in ImagesList)
                {
                    item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(item.PCMArticleImageInBytes);
                }
                FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                IsImageShareWithCustomer = temp.IsImageShareWithCustomer;

                //Attachments
                ArticleFilesList = new ObservableCollection<ArticleDocument>(temp.PCMArticleAttachmentList);
                ArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn));
                if (ArticleFilesList.Count > 0)
                    SelectedArticleFile = ArticleFilesList.FirstOrDefault();
                FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.ModifiedIn).Take(4).ToList());

                //[adhatkar][GEOS2-3196][30-07-21]
                IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(temp.IncludedPLMArticleList);
                NotIncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(temp.NotIncludedPLMArticleList);

                if (IncludedPLMArticlePriceList != null)
                {
                    foreach (var included in IncludedPLMArticlePriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(included.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        included.ToList().Where(inc => inc.Currency.Name == included.Key).ToList().ForEach(inc => inc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                if (NotIncludedPLMArticlePriceList != null)
                {
                    foreach (var notIncluded in NotIncludedPLMArticlePriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(notIncluded.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        notIncluded.ToList().Where(notinc => notinc.Currency.Name == notIncluded.Key).ToList().ForEach(notinc => notinc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                SelectedIncludedPLMArticlePrice = IncludedPLMArticlePriceList.FirstOrDefault();
                SelectedNotIncludedPLMArticlePrice = NotIncludedPLMArticlePriceList.FirstOrDefault();
                IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }
                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                        //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMArticlePriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMArticlePriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                        //{
                        //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                        //    {

                        //    }
                        //    CurrencySymbol = SelectedCurrencySymbol;
                        //}
                        //else
                        //{
                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                        //}
                    }
                    //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                    //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                    //else
                    //    IncludedFirstActiveSellPrice = null;

                    //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                    //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                    //    CurrencySymbol = "";
                    //else
                    //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                }
                else
                {
                    IncludedFirstActiveName = "";
                    IncludedFirstActiveCurrencyIconImage = null;
                    IncludedFirstActiveSellPrice = null;
                    IncludedActiveCount = 0;
                    CurrencySymbol = "";
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method PLMEditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PLMEditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ItemPositionChangedCommandAction(object obj)
        {
            ulong pos = 1;
            foreach (PCMArticleImage img in ImagesList)
            {
                img.Position = pos;
                pos++;
            }
        }

        //private void FillCategoryList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList..."), category: Category.Info, priority: Priority.Low);
        //        if (PCMCommon.Instance.CategoryList==null || PCMCommon.Instance.CategoryList?.Count==0)
        //        {
        //            PCMCommon.Instance.CategoryList= new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2290());

        //        }
        //        if(PCMCommon.Instance.CategoryList!=null)
        //        {
        //            CategoryList = PCMCommon.Instance.CategoryList;
        //            foreach (PCMArticleCategory category in CategoryList)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
        //            {
        //                if (category.InUse == "NO")
        //                    category.IspcmCategoryNOTInUse = true;
        //            }
        //            UpdatePCMCategoryCount();
        //            CategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
        //            CategoryList = new ObservableCollection<PCMArticleCategory>(CategoryList.OrderBy(x => x.Position));
        //            tempCategoryList = new ObservableCollection<PCMArticleCategory>(CategoryList);
        //            SelectedCategory = tempCategoryList.FirstOrDefault();
        //        }

        //        GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList()....executed successfully"), category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        private void FillCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList..."), category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.CategoryList == null || PCMCommon.Instance.CategoryList?.Count == 0)
                {
                    PCMCommon.Instance.CategoryList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2290());
                    CategoryList = PCMCommon.Instance.CategoryList;
                    foreach (PCMArticleCategory category in CategoryList)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
                    {
                        if (category.InUse == "NO")
                            category.IspcmCategoryNOTInUse = true;
                    }
                    UpdatePCMCategoryCount();
                }
                else
                {
                    CategoryList = PCMCommon.Instance.CategoryList;
                }
                CategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                CategoryList = new ObservableCollection<PCMArticleCategory>(CategoryList.OrderBy(x => x.Position));
                tempCategoryList = new ObservableCollection<PCMArticleCategory>(CategoryList);
                SelectedCategory = tempCategoryList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList..."), category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.tempStatusList == null || PCMCommon.Instance.tempStatusList?.Count == 0)
                {
                    PCMCommon.Instance.tempStatusList = PCMService.GetLookupValues(45);
                }
                else
                {
                    tempStatusList = PCMCommon.Instance.tempStatusList;
                }
                StatusList = new List<LookupValue>(tempStatusList);
                StatusList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                SelectedStatus = StatusList.FirstOrDefault();
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
                if (PCMCommon.Instance.tempECOSVisibilityList == null || PCMCommon.Instance.tempECOSVisibilityList?.Count == 0)
                {
                    PCMCommon.Instance.tempECOSVisibilityList = PCMService.GetLookupValues(67);
                    
                }
                else
                {
                    tempECOSVisibilityList = PCMCommon.Instance.tempECOSVisibilityList;
                }
                    ECOSVisibilityList = new List<LookupValue>(tempECOSVisibilityList);
                    SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault();
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

        private void FillModuleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillModuleMenuList()...", category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.ModuleMenulist == null || PCMCommon.Instance.ModuleMenulist?.Count == 0)
                {
                    PCMCommon.Instance.ModuleMenulist = new ObservableCollection<ProductTypesTemplate>(PCMService.GetProductTypesWithTemplate());
                    
                }
                if(PCMCommon.Instance.ModuleMenulist != null)
                {
                    ModuleMenulist = PCMCommon.Instance.ModuleMenulist;
                    SelectedModule = ModuleMenulist.FirstOrDefault();
                }
                GeosApplication.Instance.Logger.Log("Method FillModuleMenuList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillModuleMenuList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillModuleMenuList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillModuleMenuList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillArticleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleMenuList()...", category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.ArticleMenuList == null || PCMCommon.Instance.ArticleMenuList?.Count == 0)
                {
                    PCMCommon.Instance.ArticleMenuList= new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticlesWithCategory_V2290());
                    //ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticlesWithCategory_V2060());
                   
                }
                if(PCMCommon.Instance.ArticleMenuList!=null)
                {
                    ArticleMenuList = PCMCommon.Instance.ArticleMenuList;//[rdixit][GEOS2-2571][08.07.2022]
                    SelectedArticle = ArticleMenuList.FirstOrDefault();
                    foreach (PCMArticleCategory category in ArticleMenuList)//[rdixit][GEOS2- 2571][07.07.2022][added field pcmCategoryInUse]
                    {
                        if (category.InUse == "NO")
                            category.IspcmCategoryNOTInUse = true;
                    }
                }
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

        private void FillRelationShipList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillRelationShipList..."), category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.tempRelationShipList == null || PCMCommon.Instance.tempRelationShipList?.Count == 0)
                {
                    PCMCommon.Instance.tempRelationShipList = PCMService.GetLookupValues(50);
                 
                }
                if(PCMCommon.Instance.tempRelationShipList!=null)
                {
                    IList<LookupValue> tempRelationShipList = PCMCommon.Instance.tempRelationShipList;
                    RelationShipList = new List<LookupValue>();
                    RelationShipList = new List<LookupValue>(tempRelationShipList);
                    SelectedRelationShip = RelationShipList.FirstOrDefault();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method FillRelationShipList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillRelationShipList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillRelationShipList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillRelationShipList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddChangeLogsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangeLogsMenu()...", category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.ArticleChangeLogList == null || PCMCommon.Instance.ArticleChangeLogList?.Count == 0)
                {
                    PCMCommon.Instance.ArticleChangeLogList= new ObservableCollection<PCMArticleLogEntry>();
                   
                }
                if(PCMCommon.Instance.ArticleChangeLogList!=null)
                {
                    ArticleChangeLogList = PCMCommon.Instance.ArticleChangeLogList;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddChangeLogsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillReferenceView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReferenceView()...", category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.ArticleMenuList == null || PCMCommon.Instance.ArticleMenuList?.Count == 0)
                {
                    // ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticlesWithCategoryForReference_V2060());
                    PCMCommon.Instance.ArticleMenuList= new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticlesWithCategoryForReference_V2290());//[rdixit][GEOS2-2571][07.07.2022]
                  
                }
                if(PCMCommon.Instance.ArticleMenuList!=null)
                {
                    ArticleMenuList = PCMCommon.Instance.ArticleMenuList;
                    SelectedArticle = ArticleMenuList.FirstOrDefault();
                    foreach (PCMArticleCategory category in ArticleMenuList)//[rdixit][GEOS2- 2571][07.07.2022][added field pcmCategoryInUse]
                    {
                        if (category.InUse == "NO")
                            category.IspcmCategoryNOTInUse = true;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReferenceView() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReferenceView() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillReferenceView() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method ByteArrayToBitmapImage..."), category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ByteArrayToBitmapImage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }


        private void DeleteMandatory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteMandatory()...", category: Category.Info, priority: Priority.Low);
                if (SelectedMandatory.IdCPtypeCompatibility > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCM_Compatibility_DeleteModuleInMandatory"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        MandatoryList.Remove(SelectedMandatory);
                        SelectedMandatory = MandatoryList.FirstOrDefault();
                        CompatibilityCount = CompatibilityCount - 1;
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCM_Compatibility_DeleteArticleInMandatory"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        MandatoryList.Remove(SelectedMandatory);
                        SelectedMandatory = MandatoryList.FirstOrDefault();
                        CompatibilityCount = CompatibilityCount - 1;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteMandatory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteMandatory()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteSuggested(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteSuggested()...", category: Category.Info, priority: Priority.Low);
                if (SelectedSuggested.IdCPtypeCompatibility > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCM_Compatibility_DeleteModuleInSuggested"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SuggestedList.Remove(SelectedSuggested);
                        SelectedSuggested = SuggestedList.FirstOrDefault();
                        CompatibilityCount = CompatibilityCount - 1;
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCM_Compatibility_DeleteArticleInSuggested"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SuggestedList.Remove(SelectedSuggested);
                        SelectedSuggested = SuggestedList.FirstOrDefault();
                        CompatibilityCount = CompatibilityCount - 1;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteSuggested()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteSuggested()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteIncompatible(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteIncompatible()...", category: Category.Info, priority: Priority.Low);
                if (SelectedIncompatible.IdCPtypeCompatibility > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCM_Compatibility_DeleteModuleInInCompatible"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IncompatibleList.Remove(SelectedIncompatible);
                        SelectedIncompatible = IncompatibleList.FirstOrDefault();
                        CompatibilityCount = CompatibilityCount - 1;
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCM_Compatibility_DeleteArticleInInCompatible"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IncompatibleList.Remove(SelectedIncompatible);
                        SelectedIncompatible = IncompatibleList.FirstOrDefault();
                        CompatibilityCount = CompatibilityCount - 1;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteIncompatible()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteIncompatible()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [001][smazhar][21-08-2020][GEOS2-2551]Must be possible "Set as main picture" to the warehouse Article picture 
        /// [002][smazhar][27-08-2020][GEOS2-2568]After edit and accept , appear multiple image copies
        private void EditImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditImageAction()...", category: Category.Info, priority: Priority.Low);

                AddEditPCMArticleImageView addEditPCMArticleImageView = new AddEditPCMArticleImageView();
                AddEditPCMArticleImageViewModel addEditPCMArticleImageViewModel = new AddEditPCMArticleImageViewModel(obj);
                EventHandler handle = delegate { addEditPCMArticleImageView.Close(); };
                addEditPCMArticleImageViewModel.RequestClose += handle;
                addEditPCMArticleImageViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditImageHeader").ToString();
                addEditPCMArticleImageViewModel.IsNew = false;

                PCMArticleImage tempObject = new PCMArticleImage();
                tempObject = (PCMArticleImage)obj;

                addEditPCMArticleImageViewModel.EditInit(tempObject);

                addEditPCMArticleImageViewModel.ImageList = ImagesList;
                int index_new = ImagesList.IndexOf(tempObject);
                if (tempObject.IsWarehouseImage == 1)
                {
                    addEditPCMArticleImageViewModel.IsImageShareWithCustomer = IsImageShareWithCustomer;
                }
                addEditPCMArticleImageView.DataContext = addEditPCMArticleImageViewModel;

                addEditPCMArticleImageView.ShowDialog();

                if (addEditPCMArticleImageViewModel.IsSave == true)
                {

                    if (addEditPCMArticleImageViewModel.OldDefaultImage != null)
                    {
                        int index_old = ImagesList.IndexOf(addEditPCMArticleImageViewModel.OldDefaultImage);
                        ImagesList.Remove(tempObject);
                        PCMArticleImage tempProductTypeImage_old = new PCMArticleImage();
                        tempProductTypeImage_old.IdPCMArticleImage = (uint)addEditPCMArticleImageViewModel.IdImage;
                        tempProductTypeImage_old.OriginalFileName = addEditPCMArticleImageViewModel.ImageName;
                        tempProductTypeImage_old.Description = addEditPCMArticleImageViewModel.Description;
                        tempProductTypeImage_old.PCMArticleImageInBytes = addEditPCMArticleImageViewModel.FileInBytes;
                        tempProductTypeImage_old.SavedFileName = addEditPCMArticleImageViewModel.ArticleSavedImageName;
                        tempProductTypeImage_old.Position = addEditPCMArticleImageViewModel.SelectedImage.Position;
                        tempProductTypeImage_old.CreatedBy = addEditPCMArticleImageViewModel.SelectedImage.CreatedBy;
                        tempProductTypeImage_old.ModifiedBy = addEditPCMArticleImageViewModel.SelectedImage.ModifiedBy;
                        tempProductTypeImage_old.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        tempProductTypeImage_old.IsImageShareWithCustomer = addEditPCMArticleImageViewModel.IsImageShareWithCustomer;

                        //[001]
                        if (tempObject.IsWarehouseImage == 1)
                        {
                            tempProductTypeImage_old.IsWarehouseImage = 1;
                            IsImageShareWithCustomer = addEditPCMArticleImageViewModel.IsImageShareWithCustomer;
                        }
                        else
                        {
                            tempProductTypeImage_old.IsWarehouseImage = addEditPCMArticleImageViewModel.SelectedImage.IsWarehouseImage;
                        }


                        ImagesList.Insert(index_old, tempProductTypeImage_old);

                        ImagesList.Remove(addEditPCMArticleImageViewModel.OldDefaultImage);
                        PCMArticleImage tempProductTypeImage_new = new PCMArticleImage();
                        tempProductTypeImage_new = addEditPCMArticleImageViewModel.OldDefaultImage;

                        ImagesList.Insert(index_new, tempProductTypeImage_new);
                        SelectedImage = ImagesList[index_old];
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage_old.PCMArticleImageInBytes);
                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage_old.PCMArticleImageInBytes);
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    }
                    else
                    {
                        int index = ImagesList.IndexOf(tempObject);
                        ImagesList.Remove(tempObject);
                        PCMArticleImage tempProductTypeImage = new PCMArticleImage();
                        tempProductTypeImage.IdPCMArticleImage = (uint)addEditPCMArticleImageViewModel.IdImage;
                        tempProductTypeImage.OriginalFileName = addEditPCMArticleImageViewModel.ImageName;
                        tempProductTypeImage.Description = addEditPCMArticleImageViewModel.Description;
                        tempProductTypeImage.PCMArticleImageInBytes = addEditPCMArticleImageViewModel.FileInBytes;
                        tempProductTypeImage.SavedFileName = addEditPCMArticleImageViewModel.ArticleSavedImageName;
                        tempProductTypeImage.Position = addEditPCMArticleImageViewModel.SelectedImage.Position;
                        tempProductTypeImage.CreatedBy = addEditPCMArticleImageViewModel.SelectedImage.CreatedBy;
                        tempProductTypeImage.ModifiedBy = addEditPCMArticleImageViewModel.SelectedImage.ModifiedBy;
                        tempProductTypeImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        tempProductTypeImage.IsImageShareWithCustomer = addEditPCMArticleImageViewModel.IsImageShareWithCustomer;

                        //[002]
                        if (tempObject.IsWarehouseImage == 1)
                        {
                            tempProductTypeImage.IsWarehouseImage = 1;
                            IsImageShareWithCustomer = addEditPCMArticleImageViewModel.IsImageShareWithCustomer;
                        }
                        else
                        {
                            tempProductTypeImage.IsWarehouseImage = addEditPCMArticleImageViewModel.SelectedImage.IsWarehouseImage;
                        }
                        ImagesList.Insert(index, tempProductTypeImage);
                        SelectedImage = ImagesList[index];
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.PCMArticleImageInBytes);

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.PCMArticleImageInBytes);

                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    }
                    ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(a => a.Position));

                    if (addEditPCMArticleImageViewModel.OldDefaultImage != null)
                    {
                        SelectedImageIndex = 1;
                    }
                    if (ClonedArticle.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                    {
                        ReferenceImage = ByteArrayToBitmapImage(ClonedArticle.ArticleImageInBytes);
                    }
                    else
                    {
                        ReferenceImage = ByteArrayToBitmapImage(SelectedDefaultImage.PCMArticleImageInBytes);
                    }
                    OldReferenceImage = ReferenceImage;
                }
                GeosApplication.Instance.Logger.Log("Method EditImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddImageAction()...", category: Category.Info, priority: Priority.Low);

                AddEditPCMArticleImageView addEditPCMArticleImageView = new AddEditPCMArticleImageView();
                AddEditPCMArticleImageViewModel addEditPCMArticleImageViewModel = new AddEditPCMArticleImageViewModel(obj);
                EventHandler handle = delegate { addEditPCMArticleImageView.Close(); };
                addEditPCMArticleImageViewModel.RequestClose += handle;
                addEditPCMArticleImageViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddImageHeader").ToString();
                addEditPCMArticleImageViewModel.IsNew = true;
                addEditPCMArticleImageViewModel.IsImageShareWithCustomer = 1;
                addEditPCMArticleImageViewModel.ImageList = ImagesList;
                addEditPCMArticleImageView.DataContext = addEditPCMArticleImageViewModel;
                //var ownerInfo = (obj as FrameworkElement);
                //addEditPCMArticleImageView.Owner = Window.GetWindow(ownerInfo);
                addEditPCMArticleImageView.ShowDialog();

                if (addEditPCMArticleImageViewModel.IsSave == true)
                {
                    SelectedImage = new PCMArticleImage();
                    SelectedImage = addEditPCMArticleImageViewModel.SelectedImage;
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);
                    FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    if (addEditPCMArticleImageViewModel.OldDefaultImage != null)
                    {
                        int index_old = ImagesList.IndexOf(addEditPCMArticleImageViewModel.OldDefaultImage);
                        ImagesList.Insert(index_old, addEditPCMArticleImageViewModel.SelectedImage);

                        int index_new = ImagesList.IndexOf(SelectedImage) + 1;

                        ImagesList.Remove(addEditPCMArticleImageViewModel.OldDefaultImage);
                        PCMArticleImage tempProductTypeImage = new PCMArticleImage();
                        tempProductTypeImage.IdPCMArticleImage = addEditPCMArticleImageViewModel.OldDefaultImage.IdPCMArticleImage;
                        tempProductTypeImage.SavedFileName = addEditPCMArticleImageViewModel.OldDefaultImage.SavedFileName;
                        tempProductTypeImage.Description = addEditPCMArticleImageViewModel.OldDefaultImage.Description;
                        tempProductTypeImage.PCMArticleImageInBytes = addEditPCMArticleImageViewModel.OldDefaultImage.PCMArticleImageInBytes;
                        tempProductTypeImage.OriginalFileName = addEditPCMArticleImageViewModel.OldDefaultImage.OriginalFileName;
                        tempProductTypeImage.Position = addEditPCMArticleImageViewModel.OldDefaultImage.Position;
                        tempProductTypeImage.CreatedBy = addEditPCMArticleImageViewModel.OldDefaultImage.CreatedBy;
                        tempProductTypeImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        tempProductTypeImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.PCMArticleImageInBytes);

                        if (addEditPCMArticleImageViewModel.OldDefaultImage.IsWarehouseImage == 1)
                        {
                            tempProductTypeImage.IsWarehouseImage = 1;
                        }
                        else
                        {
                            tempProductTypeImage.IsWarehouseImage = addEditPCMArticleImageViewModel.SelectedImage.IsWarehouseImage;
                        }

                        ImagesList.Insert(index_new, tempProductTypeImage);
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else
                    {
                        ImagesList.Add(addEditPCMArticleImageViewModel.SelectedImage);
                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }

                    ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(a => a.Position));
                    if (addEditPCMArticleImageViewModel.OldDefaultImage != null)
                    {
                        SelectedImageIndex = 1;
                    }
                    else
                    {
                        SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                    }

                    SelectedDefaultImage = ImagesList.FirstOrDefault();
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedDefaultImage.PCMArticleImageInBytes);

                    if (ClonedArticle.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                    {
                        ReferenceImage = ByteArrayToBitmapImage(ClonedArticle.ArticleImageInBytes);
                    }
                    else
                    {
                        ReferenceImage = ByteArrayToBitmapImage(SelectedDefaultImage.PCMArticleImageInBytes);
                    }
                    OldReferenceImage = ReferenceImage;
                }
                GeosApplication.Instance.Logger.Log("Method AddImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteImageAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteImageMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    PCMArticleImage tempObj = new PCMArticleImage();
                    tempObj = (PCMArticleImage)obj;

                    if (ImagesList.Count > 0)
                    {
                        List<PCMArticleImage> imageList_ForSetPositions = ImagesList.Where(a => a.Position > tempObj.Position).ToList();
                        ImagesList.Remove(tempObj);

                        foreach (PCMArticleImage productTypeImage in imageList_ForSetPositions)
                        {
                            ImagesList.Where(a => a.Position == productTypeImage.Position).ToList().ForEach(a => { a.Position--; });
                        }
                    }

                    if (!(ImagesList.Count == 0))
                    {
                        SelectedImage = ImagesList.FirstOrDefault();
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);

                        SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);

                        FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else if (ImagesList.Count == 0)
                    {
                        FourRecordsArticleImagesList.Remove((PCMArticleImage)(obj));
                        SelectedDefaultImage.AttachmentImage = null;
                    }
                }
                if (ClonedArticle.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                {
                    ReferenceImage = ByteArrayToBitmapImage(ClonedArticle.ArticleImageInBytes);
                }
                else
                {
                    ReferenceImage = ByteArrayToBitmapImage(SelectedDefaultImage.PCMArticleImageInBytes);
                }
                OldReferenceImage = ReferenceImage;

                GeosApplication.Instance.Logger.Log("Method DeleteImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteImageAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "PCMArticleHistory_" + Reference + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }
                    ResultFileName = (saveFile.FileName);
                    TableView ChangeLogTableView = ((TableView)obj);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = false;
                    ChangeLogTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                AddEditAttachmentFileInPCMArticleView addEditAttachmentFileInPCMArticleView = new AddEditAttachmentFileInPCMArticleView();
                AddEditAttachmentFileInPCMArticleViewModel addEditAttachmentFileInPCMArticleViewModel = new AddEditAttachmentFileInPCMArticleViewModel();
                EventHandler handle = delegate { addEditAttachmentFileInPCMArticleView.Close(); };
                addEditAttachmentFileInPCMArticleViewModel.RequestClose += handle;
                addEditAttachmentFileInPCMArticleViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addEditAttachmentFileInPCMArticleViewModel.IsNew = true;
                addEditAttachmentFileInPCMArticleViewModel.Init();

                addEditAttachmentFileInPCMArticleView.DataContext = addEditAttachmentFileInPCMArticleViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditAttachmentFileInPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                addEditAttachmentFileInPCMArticleView.ShowDialog();

                if (addEditAttachmentFileInPCMArticleViewModel.IsSave)
                {
                    addEditAttachmentFileInPCMArticleViewModel.SelectedArticleFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    ArticleFilesList.Add(addEditAttachmentFileInPCMArticleViewModel.SelectedArticleFile);
                    SelectedArticleFile = addEditAttachmentFileInPCMArticleViewModel.SelectedArticleFile;
                    FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderByDescending(x => x.ModifiedIn).Take(4).ToList());
                }
                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditFile(object obj)
        {
            try
            {




                GeosApplication.Instance.Logger.Log("Method EditFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ArticleDocument articleAttachedDoc = (ArticleDocument)detailView.DataControl.CurrentItem;
                AddEditAttachmentFileInPCMArticleView addEditAttachmentFileInPCMArticleView = new AddEditAttachmentFileInPCMArticleView();
                AddEditAttachmentFileInPCMArticleViewModel addEditAttachmentFileInPCMArticleViewModel = new AddEditAttachmentFileInPCMArticleViewModel();
                EventHandler handle = delegate { addEditAttachmentFileInPCMArticleView.Close(); };
                addEditAttachmentFileInPCMArticleViewModel.RequestClose += handle;
                addEditAttachmentFileInPCMArticleViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                addEditAttachmentFileInPCMArticleViewModel.IsNew = false;
                addEditAttachmentFileInPCMArticleViewModel.ClearAllProperties();
                addEditAttachmentFileInPCMArticleViewModel.EditInit(articleAttachedDoc);
                addEditAttachmentFileInPCMArticleView.DataContext = addEditAttachmentFileInPCMArticleViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditAttachmentFileInPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                addEditAttachmentFileInPCMArticleView.ShowDialog();

                if (addEditAttachmentFileInPCMArticleViewModel.IsSave == true)
                {
                    SelectedArticleFile.IdDocType = addEditAttachmentFileInPCMArticleViewModel.SelectedType.IdDocumentType;
                    SelectedArticleFile.OriginalFileName = addEditAttachmentFileInPCMArticleViewModel.FileName;
                    SelectedArticleFile.Description = addEditAttachmentFileInPCMArticleViewModel.Description;
                    SelectedArticleFile.PCMArticleFileInBytes = addEditAttachmentFileInPCMArticleViewModel.FileInBytes;
                    SelectedArticleFile.SavedFileName = addEditAttachmentFileInPCMArticleViewModel.ArticleSavedFileName;
                    SelectedArticleFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedArticleFile.ArticleDocumentType = new ArticleDocumentType();
                    SelectedArticleFile.ArticleDocumentType.DocumentType = addEditAttachmentFileInPCMArticleViewModel.SelectedType.Name;
                    SelectedArticleFile.IsShareWithCustomer = addEditAttachmentFileInPCMArticleViewModel.IsShareWithCustomer;
                }
                GeosApplication.Instance.Logger.Log("Method EditFile()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DelateAttachmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DelateAttachmentFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ArticleFilesList.Remove(SelectedArticleFile);
                    ArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList);
                    SelectedArticleFile = ArticleFilesList.FirstOrDefault();
                    FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.IdArticleDoc).Take(4).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method DelateAttachmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DelateAttachmentFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                SelectedArticleFile = (ArticleDocument)obj;

                documentViewModel.OpenArticlePdf(SelectedArticleFile, obj, Reference);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenImageGalleryAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenImageGalleryAction()...", category: Category.Info, priority: Priority.Low);

                FlowLayoutControl flc = obj as FlowLayoutControl;
                flc.MaximizedElement = flc.Children[1] as FrameworkElement;
                MaximizedElement = SelectedDefaultImage;

                GeosApplication.Instance.Logger.Log("Method OpenImageGalleryAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenImageGalleryAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RestrictOpeningPopUpAction(object obj)
        {
            ImageEdit img1 = obj as ImageEdit;
            img1.ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never;
        }

        private void OpenSelectedImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenSelectedImageAction()...", category: Category.Info, priority: Priority.Low);

                FlowLayoutControl flc = obj as FlowLayoutControl;
                flc.MaximizedElement = flc.Children[1] as FrameworkElement;
                MaximizedElement = SelectedContentTemplateImage;

                GeosApplication.Instance.Logger.Log("Method OpenSelectedImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenSelectedImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowDescriptionViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowReferenceViewCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillArticleMenuList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowReferenceViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowReferenceViewCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillReferenceView();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                #region GEOS2-3132 Sudhir.Jangra 14/02/2023


                if (IsEnabledCancelButton == true)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonCommandAction(obj);
                    }
                }
                #endregion
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private bool IsEmptySpaceAction(string text)
        {
            text = text.Trim(' ', '\r');
            if (text == null || text.Trim() == string.Empty || text == "")
            {
                isOnlyDescriptionEmptySpaces = true;
                text = string.Empty;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmptySpacesNotAllowed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// [001] [vsana][07-01-2021][GEOS2-2785] [Manage the visibility of attachments in PCM Articles [#PCM57]]
        /// [002] [cpatil][27-09-2021][GEOS2-3340] [Sr N  4 - Synchronization between PCM and ECOS. [#PLM69]]
        /// [003] [cpatil][15-04-2022][GEOS2-3552] [PCM Change and the system, change (wrongly ) the Articles table (Warehouse sytem) : Modify In and Modify by  fields]
        /// </summary>
        /// <returns></returns>
        private async void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);


                //[pramod.misal][GEOS2-8321][11.07.2025]
                if (LinkedArticleList == null || LinkedArticleList.Count == 0)
                {
                    if (!IsAdded)
                    {

                        ChangeLogList = new ObservableCollection<PCMArticleLogEntry>();
                        WMSArticleChangeLogEntry = new List<LogEntriesByArticle>();
                        UpdatedArticle = new Articles();

                        if (PQuantityMin > PQuantityMax)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMinLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                            return;

                        }
                        if (PQuantityMax < PQuantityMin)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMaxLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                            return;
                        }

                        if (SelectedECOSVisibility.IdLookupValue != 326 && (PQuantityMin == 0 || PQuantityMax == 0))
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityReadonly").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                            return;
                        }

                        if (Description != null)
                        {
                            IsEmptySpace = IsEmptySpaceAction(Description);
                            if (IsEmptySpace)
                            {
                                Description = string.Empty;
                                UpdatedArticle.WarehouseArticle.Description = Description;
                                if (Description == "" || Description == string.Empty || Description == null)
                                {
                                    return;
                                }
                            }
                        }

                        allowValidation = true;

                        GroupBox groupBox = (GroupBox)((object[])obj)[0];
                        GroupBox groupBox1 = (GroupBox)((object[])obj)[1];

                        if (MandatoryList != null && MandatoryList.Any(x => x.MinimumElements > x.MaximumElements))
                        {
                            CompatibilityError = null;
                            PropertyChanged(this, new PropertyChangedEventArgs("CompatibilityError"));

                            TableView foundTextBox = UI.Helper.FindInnerChild.FindChild<TableView>(groupBox, "MandatoryListTableView");
                            if (foundTextBox != null)
                            {
                                foundTextBox.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;
                                foundTextBox.UpdateLayout();
                            }

                            error = EnableValidationAndGetError();

                            if (string.IsNullOrEmpty(error))
                            {
                                return;
                            }
                            return;
                        }
                        else
                        {
                            CompatibilityError = " ";
                        }

                        if (SelectedStatus.IdLookupValue == 0 || SelectedCategory.IdPCMArticleCategory == 0)
                        {
                            InformationError = null;
                            error = EnableValidationAndGetError();

                            PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatus"));
                            PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
                            PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));


                            if (error != null)
                            {
                                return;
                            }

                        }
                        if (Description == null)
                        {
                            error = EnableValidationAndGetError();
                            PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                            //PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                            PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                            if (error != null)
                            {
                                return;
                            }
                        }
                        if (Description != null && Description_en == null)
                        {
                            InformationError = null;
                            error = EnableValidationAndGetError();
                            PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                            PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                            if (error != null)
                            {
                                return;
                            }
                        }
                        else
                            InformationError = " ";

                        if (UpdatedArticle.WarehouseArticle == null)
                        {
                            UpdatedArticle.WarehouseArticle = new Article();
                        }

                        UpdatedArticle.IdPCMArticleCategory = SelectedCategory.IdPCMArticleCategory;
                        UpdatedArticle.IdPCMStatus = SelectedStatus.IdLookupValue;
                        UpdatedArticle.IdArticle = IdArticle;
                        UpdatedArticle.WarehouseArticle.IdArticle = (int)IdArticle;
                        UpdatedArticle.Reference = Reference;

                        if (IsCheckedCopyName == true)
                        {
                            IsFromInformation = true;
                            UpdatedArticle.WarehouseArticle.Description = Description == null ? "" : Description.Trim();
                            UpdatedArticle.WarehouseArticle.Description_es = Description == null ? "" : Description.Trim();
                            UpdatedArticle.WarehouseArticle.Description_fr = Description == null ? "" : Description.Trim();
                            UpdatedArticle.WarehouseArticle.Description_pt = Description == null ? "" : Description.Trim();
                            UpdatedArticle.WarehouseArticle.Description_ro = Description == null ? "" : Description.Trim();
                            UpdatedArticle.WarehouseArticle.Description_ru = Description == null ? "" : Description.Trim();
                            UpdatedArticle.WarehouseArticle.Description_zh = Description == null ? "" : Description.Trim();
                        }
                        else
                        {
                            IsFromInformation = true;
                            UpdatedArticle.WarehouseArticle.Description = Description_en == null ? "" : Description_en.Trim();
                            UpdatedArticle.WarehouseArticle.Description_es = Description_es == null ? "" : Description_es.Trim();
                            UpdatedArticle.WarehouseArticle.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                            UpdatedArticle.WarehouseArticle.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                            UpdatedArticle.WarehouseArticle.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                            UpdatedArticle.WarehouseArticle.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                            UpdatedArticle.WarehouseArticle.Description_zh = Description_zh == null ? "" : Description_zh.Trim();
                        }

                        UpdatedArticle = PcmArticleCatalogueDescriptionManager.UpdateArticleNormalOrRtfPCMDescriptionFromLocalData(UpdatedArticle);

                        if (IsRtf)
                        {
                            if (IsCheckedCopyCatelogueDescription == true)
                            {
                                IsFromInformation = false;
                            }
                            else
                            {
                                IsFromInformation = false;
                            }
                        }
                        if (IsFromInformation)
                        {
                            if (IsCheckedCopyCatelogueDescription == true)
                            {
                                IsFromInformation = false;
                            }
                            else
                            {
                                IsFromInformation = false;
                            }
                        }


                        UpdatedArticle.IdECOSVisibility = SelectedECOSVisibility.IdLookupValue;
                        UpdatedArticle.ECOSVisibilityValue = SelectedECOSVisibility.Value;

                        UpdatedArticle.IsImageShareWithCustomer = IsImageShareWithCustomer;

                        UpdatedArticle.PurchaseQtyMin = PQuantityMin;
                        UpdatedArticle.PurchaseQtyMax = PQuantityMax;

                        UpdatedArticle.ECOSVisibilityHTMLColor = SelectedECOSVisibility.HtmlColor;

                        UpdatedArticle.ArticleCompatibilityList = new List<ArticleCompatibility>();

                        // Delete Compatibility
                        foreach (ArticleCompatibility item in ClonedArticle.ArticleCompatibilityList)
                        {
                            if (item.IdTypeCompatibility == 254 && MandatoryList != null && !MandatoryList.Any(x => x.IdCompatibility == item.IdCompatibility))
                            {
                                IsArticleSync = true;
                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                            }
                            if (item.IdTypeCompatibility == 255 && SuggestedList != null && !SuggestedList.Any(x => x.IdCompatibility == item.IdCompatibility))
                            {
                                IsArticleSync = true;
                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                            }
                            if (item.IdTypeCompatibility == 256 && IncompatibleList != null && !IncompatibleList.Any(x => x.IdCompatibility == item.IdCompatibility))
                            {
                                IsArticleSync = true;
                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                            }
                        }


                        //Added Compatibility
                        foreach (ArticleCompatibility item in MandatoryList)
                        {
                            if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                            {
                                IsArticleSync = true;
                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                            }
                        }
                        foreach (ArticleCompatibility item in SuggestedList)
                        {
                            if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                            {
                                IsArticleSync = true;
                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                            }
                        }
                        foreach (ArticleCompatibility item in IncompatibleList)
                        {
                            if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                            {
                                IsArticleSync = true;
                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                            }
                        }

                        //Updated Compatibility
                        foreach (ArticleCompatibility originalCompatibility in ClonedArticle.ArticleCompatibilityList)
                        {
                            if (originalCompatibility.IdTypeCompatibility == 254 && MandatoryList != null && MandatoryList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                            {
                                ArticleCompatibility MandatoryUpdated = MandatoryList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                if ((MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements) || (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements) || (MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                {
                                    ArticleCompatibility articleCompatibility = (ArticleCompatibility)MandatoryUpdated.Clone();
                                    articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                    if (MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements)
                                    {
                                        IsArticleSync = true;
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMinUpdate").ToString(), originalCompatibility.MinimumElements, MandatoryUpdated.MinimumElements) });
                                    }
                                    if (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements)
                                    {
                                        IsArticleSync = true;
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMaxUpdate").ToString(), originalCompatibility.MaximumElements, MandatoryUpdated.MaximumElements) });
                                    }
                                    if ((MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(MandatoryUpdated.Remarks))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                        }
                                    }
                                }
                            }

                            if (originalCompatibility.IdTypeCompatibility == 255 && SuggestedList != null && SuggestedList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                            {
                                ArticleCompatibility SuggestedUpdated = SuggestedList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                if (SuggestedUpdated.Remarks != originalCompatibility.Remarks)
                                {
                                    ArticleCompatibility articleCompatibility = (ArticleCompatibility)SuggestedUpdated.Clone();
                                    articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                    if ((SuggestedUpdated.Remarks != originalCompatibility.Remarks))
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(SuggestedUpdated.Remarks))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                        }
                                    }
                                }
                            }

                            if (originalCompatibility.IdTypeCompatibility == 256 && IncompatibleList != null && IncompatibleList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                            {
                                ArticleCompatibility IncompatibleUpdated = IncompatibleList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);

                                if ((IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType) || (IncompatibleUpdated.Quantity != originalCompatibility.Quantity) || (IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                {
                                    ArticleCompatibility articleCompatibility = (ArticleCompatibility)IncompatibleUpdated.Clone();
                                    articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                                    articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                    if (IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType)
                                    {
                                        IsArticleSync = true;
                                        if (originalCompatibility.RelationshipType == null)
                                            originalCompatibility.RelationshipType = new LookupValue();

                                        string relationShip = RelationShipList.FirstOrDefault(a => a.IdLookupValue == articleCompatibility.IdRelationshipType).Value;

                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelationshipUpdate").ToString(), originalCompatibility.RelationshipType.Value, relationShip) });
                                    }

                                    if ((IncompatibleUpdated.Quantity != originalCompatibility.Quantity))
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(IncompatibleUpdated.Quantity.ToString()))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCompatibility.Quantity.ToString()))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), "None", IncompatibleUpdated.Quantity) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, IncompatibleUpdated.Quantity) });
                                        }
                                    }


                                    if ((IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(IncompatibleUpdated.Remarks))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                        }
                                    }
                                }
                            }
                        }

                        if (ImagesList != null)
                        {
                            ulong pos = 1;
                            foreach (PCMArticleImage img in ImagesList)
                            {
                                img.Position = pos;
                                pos++;
                            }
                        }


                        //Images
                        UpdatedArticle.PCMArticleImageList = new List<PCMArticleImage>();

                        foreach (PCMArticleImage item in ClonedArticle.PCMArticleImageList)
                        {

                            if (!ImagesList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                            {
                                IsArticleSync = true;
                                PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                if (pCMArticleImage.Position == 1)
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                            }
                        }



                        //Added Article Image
                        foreach (PCMArticleImage item in ImagesList)
                        {
                            if (!(item.IsWarehouseImage == 1))
                            {
                                if (!ClonedArticle.PCMArticleImageList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                {
                                    IsArticleSync = true;
                                    PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                    pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                    if (pCMArticleImage.Position == 1)
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                    else
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                                }
                            }
                        }
                        //Updated Article Image

                        foreach (PCMArticleImage originalArticleTypeImage in ClonedArticle.PCMArticleImageList)
                        {
                            if (ImagesList != null && ImagesList.Any(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage))
                            {
                                PCMArticleImage articleImageUpdated = ImagesList.FirstOrDefault(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage);
                                if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName) ||
                                    (articleImageUpdated.SavedFileName != originalArticleTypeImage.SavedFileName) ||
                                    (articleImageUpdated.Description != originalArticleTypeImage.Description) ||
                                    (articleImageUpdated.Position != originalArticleTypeImage.Position) ||
                                    (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer))
                                {
                                    PCMArticleImage productTypeImage = (PCMArticleImage)articleImageUpdated.Clone();
                                    productTypeImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedArticle.PCMArticleImageList.Add(productTypeImage);
                                    if (articleImageUpdated.PCMArticleImageInBytes != originalArticleTypeImage.PCMArticleImageInBytes)
                                    {
                                        IsArticleSync = true;
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesUpdate").ToString(), originalArticleTypeImage.SavedFileName, articleImageUpdated.SavedFileName) });
                                    }
                                    if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName))
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(articleImageUpdated.OriginalFileName))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalArticleTypeImage.OriginalFileName))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), "None", articleImageUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, articleImageUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (articleImageUpdated.Description != originalArticleTypeImage.Description)
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(articleImageUpdated.Description))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalArticleTypeImage.Description))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), "None", articleImageUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, articleImageUpdated.Description) });
                                        }
                                    }

                                    if (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer)
                                    {
                                        IsArticleSync = true;
                                        if (articleImageUpdated.IsImageShareWithCustomer == 1 && originalArticleTypeImage.IsImageShareWithCustomer == 0)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "OFF", "ON") });
                                        else if (articleImageUpdated.IsImageShareWithCustomer == 0 && originalArticleTypeImage.IsImageShareWithCustomer == 1)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "ON", "OFF") });
                                    }

                                    if (articleImageUpdated.Position != originalArticleTypeImage.Position)
                                    {
                                        IsArticleSync = true;
                                        if (originalArticleTypeImage.IsWarehouseImage != 1)
                                        {
                                            if (originalArticleTypeImage.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                        }
                                    }
                                }
                            }

                        }
                        //if ((ImagesList.Any(a => a.IsWarehouseImage == 1)))
                        {
                            PCMArticleImage tempDefaultImage = ClonedArticle.PCMArticleImageList.FirstOrDefault(x => x.Position == 1);
                            PCMArticleImage tempDefaultImage_updated = ImagesList.FirstOrDefault(x => x.Position == 1);
                            PCMArticleImage WarehouseImg = ImagesList.FirstOrDefault(x => x.IsWarehouseImage == 1);

                            if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdPCMArticleImage != tempDefaultImage_updated.IdPCMArticleImage)
                            {
                                IsArticleSync = true;
                                if (tempDefaultImage_updated.Position == 1)
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                else if (tempDefaultImage_updated.Position == 1 && tempDefaultImage_updated.IsWarehouseImage == 1)
                                {
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), tempDefaultImage, tempDefaultImage_updated.OriginalFileName) });
                                }
                            }


                            if (OldWarehouseImage != null && WarehouseImg != null)
                            {
                                IsArticleSync = true;
                                if (oldWarehouseposition != WarehouseImg.Position)
                                {
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), oldWarehouseposition, WarehouseImg.Position, WarehouseImg.OriginalFileName) });
                                }
                                if (oldWarehouseposition != WarehouseImg.Position && oldWarehouseposition == 1)
                                {
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), WarehouseImg.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                }

                            }
                        }

                        if (ClonedArticle.IsImageShareWithCustomer != IsImageShareWithCustomer)
                        {
                            IsArticleSync = true;
                            if (ClonedArticle.IsImageShareWithCustomer == 1 && IsImageShareWithCustomer == 0)
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "ON", "OFF") });
                            else if (ClonedArticle.IsImageShareWithCustomer == 0 && IsImageShareWithCustomer == 1)
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "OFF", "ON") });
                        }

                        UpdatedArticle.PCMArticleImageList.ForEach(x => x.AttachmentImage = null);

                        /// Attchment Files
                        UpdatedArticle.PCMArticleAttachmentList = new List<ArticleDocument>();

                        // Delete Article Attachment file
                        foreach (ArticleDocument item in ClonedArticle.PCMArticleAttachmentList)
                        {

                            if (ArticleFilesList != null && !ArticleFilesList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                            {
                                IsArticleSync = true;
                                ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                articleDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                            }
                        }

                        //Added Article Attachment file
                        if (ArticleFilesList != null)
                        {
                            foreach (ArticleDocument item in ArticleFilesList)
                            {
                                if (!ClonedArticle.PCMArticleAttachmentList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                {
                                    IsArticleSync = true;
                                    ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                    articleDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                                }
                            }
                        }

                        //Updated Article Attachment file
                        foreach (ArticleDocument originalArticle in ClonedArticle.PCMArticleAttachmentList)
                        {
                            if (ArticleFilesList != null && ArticleFilesList.Any(x => x.IdArticleDoc == originalArticle.IdArticleDoc))
                            {
                                ArticleDocument articleAttachedDocUpdated = ArticleFilesList.FirstOrDefault(x => x.IdArticleDoc == originalArticle.IdArticleDoc);
                                if (originalArticle.ArticleDocumentType == null)
                                    originalArticle.ArticleDocumentType = new ArticleDocumentType();

                                if (articleAttachedDocUpdated.ArticleDocumentType == null)
                                    articleAttachedDocUpdated.ArticleDocumentType = new ArticleDocumentType();

                                if ((articleAttachedDocUpdated.SavedFileName != originalArticle.SavedFileName) || (articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName) || (articleAttachedDocUpdated.Description != originalArticle.Description) || (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer) || (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType) || (articleAttachedDocUpdated.PCMArticleFileInBytes != originalArticle.PCMArticleFileInBytes))
                                {
                                    ArticleDocument articleAttachedDoc = (ArticleDocument)articleAttachedDocUpdated.Clone();
                                    articleAttachedDoc.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    articleAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedArticle.PCMArticleAttachmentList.Add(articleAttachedDoc);

                                    if ((articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName))
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(articleAttachedDocUpdated.OriginalFileName))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalArticle.OriginalFileName))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, "None", articleAttachedDocUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, articleAttachedDocUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (articleAttachedDocUpdated.Description != originalArticle.Description)
                                    {
                                        IsArticleSync = true;
                                        if (string.IsNullOrEmpty(articleAttachedDocUpdated.Description))
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), originalArticle.OriginalFileName, originalArticle.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalArticle.Description))
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, "None", articleAttachedDoc.Description) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.Description, articleAttachedDoc.Description) });
                                        }
                                    }
                                    if (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType)
                                    {
                                        IsArticleSync = true;
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDocumentTypeUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.ArticleDocumentType.DocumentType, articleAttachedDocUpdated.ArticleDocumentType.DocumentType) });
                                    }
                                    if (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer)
                                    {
                                        IsArticleSync = true;
                                        if (articleAttachedDocUpdated.IsShareWithCustomer == 0)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString())) });
                                        else
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString())) });
                                    }
                                }
                            }
                        }

                        if (SelectedECOSVisibility.IdLookupValue == 323)
                        {
                            UpdatedArticle.IsShareWithCustomer = 1;
                            UpdatedArticle.IsSparePartOnly = 0;

                            if (UpdatedArticle.PurchaseQtyMin == 0)
                            {
                                UpdatedArticle.PurchaseQtyMin = 1;
                                IsEnabledMinMax = true;
                            }

                            if (UpdatedArticle.PurchaseQtyMax == 0)
                            {
                                UpdatedArticle.PurchaseQtyMax = 1;
                                IsEnabledMinMax = true;
                            }
                        }
                        else if (SelectedECOSVisibility.IdLookupValue == 324)
                        {
                            UpdatedArticle.IsShareWithCustomer = 0;
                            UpdatedArticle.IsSparePartOnly = 1;

                            if (UpdatedArticle.PurchaseQtyMin == 0)
                            {
                                UpdatedArticle.PurchaseQtyMin = 1;
                                IsEnabledMinMax = true;
                            }

                            if (UpdatedArticle.PurchaseQtyMax == 0)
                            {
                                UpdatedArticle.PurchaseQtyMax = 1;
                                IsEnabledMinMax = true;
                            }
                        }
                        else if (SelectedECOSVisibility.IdLookupValue == 325)
                        {
                            UpdatedArticle.IsShareWithCustomer = 0;
                            UpdatedArticle.IsSparePartOnly = 0;

                            if (UpdatedArticle.PurchaseQtyMin == 0)
                            {
                                UpdatedArticle.PurchaseQtyMin = 1;
                                IsEnabledMinMax = true;
                            }

                            if (UpdatedArticle.PurchaseQtyMax == 0)
                            {
                                UpdatedArticle.PurchaseQtyMax = 1;
                                IsEnabledMinMax = true;
                            }
                        }
                        else if (SelectedECOSVisibility.IdLookupValue == 326)
                        {
                            UpdatedArticle.IsShareWithCustomer = 0;
                            UpdatedArticle.IsSparePartOnly = 0;
                            UpdatedArticle.PurchaseQtyMin = 0;
                            UpdatedArticle.PurchaseQtyMax = 0;
                            IsEnabledMinMax = true;
                        }

                        if (UpdatedArticle.PCMDescription.Contains(" ") || UpdatedArticle.PCMDescription_es.Contains(" ") ||
                            UpdatedArticle.PCMDescription_fr.Contains(" ") || UpdatedArticle.PCMDescription_pt.Contains(" ") ||
                            UpdatedArticle.PCMDescription_ro.Contains(" ") || UpdatedArticle.PCMDescription_ru.Contains(" ") ||
                            UpdatedArticle.PCMDescription_zh.Contains(" "))
                        {
                            UpdatedArticle.PCMDescription = UpdatedArticle.PCMDescription.Trim(' ');
                            UpdatedArticle.PCMDescription_es = UpdatedArticle.PCMDescription_es.Trim(' ');
                            UpdatedArticle.PCMDescription_fr = UpdatedArticle.PCMDescription_fr.Trim(' ');
                            UpdatedArticle.PCMDescription_pt = UpdatedArticle.PCMDescription_pt.Trim(' ');
                            UpdatedArticle.PCMDescription_ro = UpdatedArticle.PCMDescription_ro.Trim(' ');
                            UpdatedArticle.PCMDescription_ru = UpdatedArticle.PCMDescription_ru.Trim(' ');
                            UpdatedArticle.PCMDescription_zh = UpdatedArticle.PCMDescription_zh.Trim(' ');
                        }


                        //[adhatkar][GEOS2-3196]
                        /// PLM Article Prices
                        UpdatedArticle.ModifiedPLMArticleList = new List<PLMArticlePrice>();
                        UpdatedArticle.BasePriceLogEntryList = new List<BasePriceLogEntry>();
                        UpdatedArticle.CustomerPriceLogEntryList = new List<CustomerPriceLogEntry>();

                        // Delete PLM Article Prices
                        if (NotIncludedPLMArticlePriceList != null)
                        {
                            foreach (PLMArticlePrice item in NotIncludedPLMArticlePriceList)
                            {
                                if (!ClonedArticle.NotIncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                {
                                    PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                    PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDeleteInPCM").ToString(), item.Code, item.Type) });
                                    if (item.Type == "BPL")
                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                    else
                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                }
                            }
                        }

                        //Added PLM Article Prices
                        if (IncludedPLMArticlePriceList != null)
                        {
                            foreach (PLMArticlePrice item in IncludedPLMArticlePriceList)
                            {
                                if (!ClonedArticle.IncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                {
                                    PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                    PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAddInPCM").ToString(), item.Code, item.Type) });
                                    if (item.Type == "BPL")
                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });
                                    else
                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });

                                }
                            }
                        }

                        //Updated PLM Article Prices
                        foreach (PLMArticlePrice originalArticle in ClonedArticle.IncludedPLMArticleList)
                        {
                            if (IncludedPLMArticlePriceList != null && IncludedPLMArticlePriceList.Any(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type))
                            {
                                PLMArticlePrice PLMArticlePriceUpdated = IncludedPLMArticlePriceList.FirstOrDefault(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type);
                                if ((PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue) || (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule))
                                {
                                    PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)PLMArticlePriceUpdated.Clone();
                                    PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);


                                    if (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule)
                                    {
                                        string newRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMArticlePriceUpdated.IdRule).Value;
                                        string oldRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == originalArticle.IdRule).Value;
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogicInPCM").ToString(), oldRuleLogic, newRuleLogic, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                        if (PLMArticlePriceUpdated.Type == "BPL")
                                            UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                        else
                                            UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                    }

                                    if (PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue)
                                    {
                                        string oldValue = "";
                                        string newValue = "";
                                        if (PLMArticlePriceUpdated.RuleValue == null)
                                            newValue = "None";
                                        else
                                            newValue = PLMArticlePriceUpdated.RuleValue.Value.ToString("0." + new string('#', 339));

                                        if (originalArticle.RuleValue == null)
                                            oldValue = "None";
                                        else
                                            oldValue = originalArticle.RuleValue.Value.ToString("0." + new string('#', 339));

                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValueInPCM").ToString(), oldValue, newValue, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                        if (PLMArticlePriceUpdated.Type == "BPL")
                                            UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                        else
                                            UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                    }
                                }
                            }
                        }


                        //Customers add and Delete
                        if (UpdatedArticle.ArticleCustomerList == null)
                            UpdatedArticle.ArticleCustomerList = new List<ArticleCustomer>();
                        //ObservableCollection<ArticleCustomer> tempCustomersList = ArticleCustomerList;
                        //ObservableCollection<ArticleCustomer> clonedCustomerListByArticle = new ObservableCollection<ArticleCustomer>(PCMService.GetCustomersByIdArticleCustomerReferences(IdArticle));
                        // Delete Customer
                        if (ArticleCustomerList != null)
                        {

                            foreach (ArticleCustomer item in ClonedArticle.ArticleCustomerList)
                            {
                                if (!ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == item.IdArticleCustomerReferences))
                                {
                                    IsArticleSync = true;
                                    ArticleCustomer CustomerReference = (ArticleCustomer)item.Clone();
                                    CustomerReference.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    UpdatedArticle.ArticleCustomerList.Add(CustomerReference);
                                    ChangeLogList.Add(new PCMArticleLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomerDelete").ToString(),
                                     item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                    });
                                }
                            }
                        }

                        //Added Customer
                        if (ArticleCustomerList != null)
                        {
                            foreach (ArticleCustomer item in ArticleCustomerList)
                            {
                                if (!ClonedArticle.ArticleCustomerList.Any(x => x.IdArticleList == item.IdArticleList))
                                {
                                    IsArticleSync = true;
                                    ArticleCustomer articleCustomer = (ArticleCustomer)item.Clone();
                                    articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                    ChangeLogList.Add(new PCMArticleLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersAdd").ToString(),
                                     item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                    });

                                }
                            }
                        }


                        //Updated Customer
                        if (ClonedArticle.ArticleCustomerList != null)
                        {
                            foreach (ArticleCustomer originalCustomer in ClonedArticle.ArticleCustomerList)
                            {
                                if (ArticleCustomerList != null && ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences))
                                {
                                    ArticleCustomer ArticleCustomerListUpdated = ArticleCustomerList.FirstOrDefault(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences);
                                    if ((ArticleCustomerListUpdated.IdGroup != originalCustomer.IdGroup) || (ArticleCustomerListUpdated.IdRegion != originalCustomer.IdRegion) || (ArticleCustomerListUpdated.IdCountry != originalCustomer.IdCountry) || (ArticleCustomerListUpdated.IdPlant != originalCustomer.IdPlant || (ArticleCustomerListUpdated.ReferenceCustomer != originalCustomer.ReferenceCustomer.Trim())))
                                    {
                                        IsArticleSync = true;
                                        ArticleCustomer articleCustomer = (ArticleCustomer)ArticleCustomerListUpdated.Clone();
                                        articleCustomer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                        articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                        ChangeLogList.Add(new PCMArticleLogEntry()
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersUpdate").ToString(),
                                            originalCustomer.GroupName, originalCustomer.RegionName,
                                            originalCustomer.Country.Name, originalCustomer.Plant.Name, articleCustomer.ReferenceCustomer,
                                            articleCustomer.GroupName, articleCustomer.RegionName,
                                            articleCustomer.Country.Name, articleCustomer.Plant.Name, articleCustomer.ReferenceCustomer)
                                        });
                                    }
                                }
                            }
                        }

                        UpdatedArticle.ModifiedPLMArticleList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);

                        AddArticleLogDetails();
                        UpdatedArticle.PCMArticleLogEntiryList = ChangeLogList.ToList();
                        if (UpdatedArticle.WarehouseArticleLogEntiryList == null)
                            UpdatedArticle.WarehouseArticleLogEntiryList = new List<LogEntriesByArticle>();
                        UpdatedArticle.WarehouseArticleLogEntiryList = WMSArticleChangeLogEntry.ToList();
                        //[003] Added
                        if (WMSArticleChangeLogEntry.Count == 0)
                        {
                            UpdatedArticle.WarehouseArticle = null;
                        }
                        UpdatedArticle.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        UpdatedArticle.IdCreator = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                        //[003] Changes service method
                        IsSave = PCMService.IsUpdatePCMArticle_V2350(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                        // IsSave = PCMService.IsUpdatePCMArticle_V2260(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsAdded = true;
                        IsAcceptButtonEnabled = false;
                        //[002] Added code for synchronization
                        if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                        {
                            GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59,61");
                            if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                            {
                                if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 57))
                                {
                                    if (!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                                    {
                                        if (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue)))
                                        {
                                        #region Synchronization code
                                        CancelSync:;
                                            var ownerInfo = (groupBox as FrameworkElement);
                                            MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                            if (MessageBoxResult == MessageBoxResult.Yes)
                                            {
                                                GeosApplication.Instance.SplashScreenMessage = "Synchronization is running";

                                                if (!string.IsNullOrEmpty(UpdatedArticle.Reference.Trim()))
                                                {
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

                                                        APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
                                                        APIErrorDetail values = new APIErrorDetail();
                                                        tokenService = new AuthTokenService();
                                                        List<ErrorDetails> LstErrorDetail = new List<ErrorDetails>();
                                                        bool IsArticleSynchronization = true;
                                                        //GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59");
                                                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                                        {
                                                            string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                                            if (tokeninformations.Count() >= 2)
                                                            {
                                                                if (UpdatedArticle.ModifiedPLMArticleList.Any(i => i.IdStatus == 223) && GeosAppSettingList.Any(i => i.IdAppSetting == 61))
                                                                {
                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                    //[rdixit][22.02.2023][GEOS2-4176]
                                                                    PCMArticleSynchronizationViewModel PCMArticleSynchronizationViewModel = new PCMArticleSynchronizationViewModel();
                                                                    PCMArticleSynchronizationView PCMArticleSynchronizationView = new PCMArticleSynchronizationView();
                                                                    EventHandler handle = delegate { PCMArticleSynchronizationView.Close(); };
                                                                    PCMArticleSynchronizationViewModel.RequestClose += handle;
                                                                    PCMArticleSynchronizationView.DataContext = PCMArticleSynchronizationViewModel;
                                                                    PCMArticleSynchronizationViewModel.Init(IncludedPLMArticlePriceList, UpdatedArticle);
                                                                    PCMArticleSynchronizationView.ShowDialogWindow();
                                                                    if (PCMArticleSynchronizationViewModel.IsSave)
                                                                    {
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

                                                                        if (!string.IsNullOrEmpty((GeosAppSettingList[2].DefaultValue)))
                                                                        {
                                                                            string IdsBPL = "";
                                                                            if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "BPL" && pd.IdStatus == 223))
                                                                            {
                                                                                IdsBPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                            }
                                                                            string IdsCPL = "";
                                                                            if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "CPL" && pd.IdStatus == 223))
                                                                            {
                                                                                IdsCPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                            }
                                                                            //[rdixit][22.02.2023][GEOS2-4176]
                                                                            if (PCMArticleSynchronizationViewModel.BPLPlantCurrencyList != null)
                                                                            {
                                                                                List<BPLPlantCurrencyDetail> BPLPlantCurrencyDetailList = PCMArticleSynchronizationViewModel.BPLPlantCurrencyList.ToList();//PLMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Convert.ToInt32(UpdatedArticle.IdArticle), IdsBPL, IdsCPL, "Article");

                                                                                if (GeosAppSettingList.Any(i => i.IdAppSetting == 61) && BPLPlantCurrencyDetailList != null)
                                                                                {

                                                                                    foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                                                                    {
                                                                                        if (IsArticleSync == true && IsArticleSynchronization == true)
                                                                                        {
                                                                                            IsArticleSynchronization = true;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            IsArticleSynchronization = false;
                                                                                        }
                                                                                        GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                        List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization_V2310(GeosAppSettingList, itemBPLPlantCurrency, UpdatedArticle, IsArticleSynchronization);
                                                                                        IsArticleSynchronization = false;
                                                                                        #region
                                                                                        foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                        {
                                                                                            if (item != null && item.Error == string.Empty)
                                                                                            {
                                                                                                GeosApplication.Instance.SplashScreenMessage = "Synchronization is Done for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (item != null && item.Error != null)
                                                                                                {
                                                                                                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is failed for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                    ErrorDetails errorDetails = new ErrorDetails();
                                                                                                    errorDetails.CompanyName = itemBPLPlantCurrency.CompanyName;
                                                                                                    errorDetails.CurrencyName = itemBPLPlantCurrency.CurrencyName;
                                                                                                    errorDetails.Error = item.Error;
                                                                                                    LstErrorDetail.Add(errorDetails);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        #endregion
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                        //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationCancelled").ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                        goto CancelSync;
                                                                    }
                                                                }
                                                                else
                                                                {

                                                                    List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization(GeosAppSettingList, null, UpdatedArticle);
                                                                    foreach (ErrorDetails item in TempLstErrorDetail)
                                                                    {
                                                                        if (item != null && item.Error == string.Empty)
                                                                        {

                                                                        }
                                                                        else
                                                                        {
                                                                            if (item != null && item.Error != null)
                                                                            {
                                                                                ErrorDetails errorDetails = new ErrorDetails();
                                                                                errorDetails.CompanyName = "None";
                                                                                errorDetails.CurrencyName = "None";
                                                                                errorDetails.Error = item.Error;
                                                                                LstErrorDetail.Add(errorDetails);
                                                                            }
                                                                        }
                                                                    }


                                                                }
                                                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                if (LstErrorDetail != null && LstErrorDetail.Count > 0)
                                                                {
                                                                    String FinalMessage = null;
                                                                    string msg = string.Empty;
                                                                    foreach (ErrorDetails item in LstErrorDetail)
                                                                    {

                                                                        if (string.IsNullOrEmpty(FinalMessage))
                                                                            FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error), Window.GetWindow(ownerInfo));
                                                                        else
                                                                            FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error);

                                                                    }
                                                                    if (!string.IsNullOrEmpty(FinalMessage))
                                                                    {
                                                                        CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                }
                                                            }

                                                        }

                                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                        GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                    }

                                                }
                                                //CancelSync:;                                          
                                                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                            }
                                            #endregion Synchronization
                                        }
                                    }
                                }
                            }
                        }
                        RequestClose(null, null);
                        IsAdded = false;
                        IsAcceptButtonEnabled = true;
                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                        GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                    }

                }
                else
                {
                    if (LinkedArticleList.Count != 0 && LinkedArticleList.Any(x => x.IdLinkType == 5))
                    {
                        List<LinkedArticle> linkType5Articles = LinkedArticleList.Where(article => article.IdLinkType == 5).ToList();
                        CustomPromptResult promptResult = CustomMessageBox.ShowbPCMPrompt(string.Format(Application.Current.Resources["PCMEditArticlePrompt"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (promptResult.Result == MessageBoxResult.Yes)
                        {
                            IsDetailsChecked = promptResult.ViewModel.IsDetailsChecked;
                            IsPricesChecked = promptResult.ViewModel.IsPricesChecked;

                           if((IsDetailsChecked==true && IsPricesChecked==true)|| (IsDetailsChecked == false && IsPricesChecked == true)|| (IsDetailsChecked == true && IsPricesChecked == false))
                            {
                                if (!IsAdded)
                                {

                                    ChangeLogList = new ObservableCollection<PCMArticleLogEntry>();
                                    WMSArticleChangeLogEntry = new List<LogEntriesByArticle>();
                                    UpdatedArticle = new Articles();

                                    if (PQuantityMin > PQuantityMax)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMinLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;

                                    }
                                    if (PQuantityMax < PQuantityMin)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMaxLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;
                                    }

                                    if (SelectedECOSVisibility.IdLookupValue != 326 && (PQuantityMin == 0 || PQuantityMax == 0))
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityReadonly").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;
                                    }

                                    if (Description != null)
                                    {
                                        IsEmptySpace = IsEmptySpaceAction(Description);
                                        if (IsEmptySpace)
                                        {
                                            Description = string.Empty;
                                            UpdatedArticle.WarehouseArticle.Description = Description;
                                            if (Description == "" || Description == string.Empty || Description == null)
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    allowValidation = true;

                                    GroupBox groupBox = (GroupBox)((object[])obj)[0];
                                    GroupBox groupBox1 = (GroupBox)((object[])obj)[1];

                                    if (MandatoryList != null && MandatoryList.Any(x => x.MinimumElements > x.MaximumElements))
                                    {
                                        CompatibilityError = null;
                                        PropertyChanged(this, new PropertyChangedEventArgs("CompatibilityError"));

                                        TableView foundTextBox = UI.Helper.FindInnerChild.FindChild<TableView>(groupBox, "MandatoryListTableView");
                                        if (foundTextBox != null)
                                        {
                                            foundTextBox.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;
                                            foundTextBox.UpdateLayout();
                                        }

                                        error = EnableValidationAndGetError();

                                        if (string.IsNullOrEmpty(error))
                                        {
                                            return;
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        CompatibilityError = " ";
                                    }

                                    if (SelectedStatus.IdLookupValue == 0 || SelectedCategory.IdPCMArticleCategory == 0)
                                    {
                                        InformationError = null;
                                        error = EnableValidationAndGetError();

                                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatus"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));


                                        if (error != null)
                                        {
                                            return;
                                        }

                                    }
                                    if (Description == null)
                                    {
                                        error = EnableValidationAndGetError();
                                        PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                                        //PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                                        if (error != null)
                                        {
                                            return;
                                        }
                                    }
                                    if (Description != null && Description_en == null)
                                    {
                                        InformationError = null;
                                        error = EnableValidationAndGetError();
                                        PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                                        if (error != null)
                                        {
                                            return;
                                        }
                                    }
                                    else
                                        InformationError = " ";

                                    if (UpdatedArticle.WarehouseArticle == null)
                                    {
                                        UpdatedArticle.WarehouseArticle = new Article();
                                    }

                                    UpdatedArticle.IdPCMArticleCategory = SelectedCategory.IdPCMArticleCategory;
                                    UpdatedArticle.IdPCMStatus = SelectedStatus.IdLookupValue;
                                    UpdatedArticle.IdArticle = IdArticle;
                                    UpdatedArticle.WarehouseArticle.IdArticle = (int)IdArticle;
                                    UpdatedArticle.Reference = Reference;

                                    if (IsCheckedCopyName == true)
                                    {
                                        IsFromInformation = true;
                                        UpdatedArticle.WarehouseArticle.Description = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_es = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_fr = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_pt = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ro = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ru = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_zh = Description == null ? "" : Description.Trim();
                                    }
                                    else
                                    {
                                        IsFromInformation = true;
                                        UpdatedArticle.WarehouseArticle.Description = Description_en == null ? "" : Description_en.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_es = Description_es == null ? "" : Description_es.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_zh = Description_zh == null ? "" : Description_zh.Trim();
                                    }

                                    UpdatedArticle = PcmArticleCatalogueDescriptionManager.UpdateArticleNormalOrRtfPCMDescriptionFromLocalData(UpdatedArticle);

                                    if (IsRtf)
                                    {
                                        if (IsCheckedCopyCatelogueDescription == true)
                                        {
                                            IsFromInformation = false;
                                        }
                                        else
                                        {
                                            IsFromInformation = false;
                                        }
                                    }
                                    if (IsFromInformation)
                                    {
                                        if (IsCheckedCopyCatelogueDescription == true)
                                        {
                                            IsFromInformation = false;
                                        }
                                        else
                                        {
                                            IsFromInformation = false;
                                        }
                                    }


                                    UpdatedArticle.IdECOSVisibility = SelectedECOSVisibility.IdLookupValue;
                                    UpdatedArticle.ECOSVisibilityValue = SelectedECOSVisibility.Value;

                                    UpdatedArticle.IsImageShareWithCustomer = IsImageShareWithCustomer;

                                    UpdatedArticle.PurchaseQtyMin = PQuantityMin;
                                    UpdatedArticle.PurchaseQtyMax = PQuantityMax;

                                    UpdatedArticle.ECOSVisibilityHTMLColor = SelectedECOSVisibility.HtmlColor;

                                    UpdatedArticle.ArticleCompatibilityList = new List<ArticleCompatibility>();

                                    // Delete Compatibility
                                    foreach (ArticleCompatibility item in ClonedArticle.ArticleCompatibilityList)
                                    {
                                        if (item.IdTypeCompatibility == 254 && MandatoryList != null && !MandatoryList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                        if (item.IdTypeCompatibility == 255 && SuggestedList != null && !SuggestedList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                        if (item.IdTypeCompatibility == 256 && IncompatibleList != null && !IncompatibleList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }


                                    //Added Compatibility
                                    foreach (ArticleCompatibility item in MandatoryList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }
                                    foreach (ArticleCompatibility item in SuggestedList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }
                                    foreach (ArticleCompatibility item in IncompatibleList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }

                                    //Updated Compatibility
                                    foreach (ArticleCompatibility originalCompatibility in ClonedArticle.ArticleCompatibilityList)
                                    {
                                        if (originalCompatibility.IdTypeCompatibility == 254 && MandatoryList != null && MandatoryList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility MandatoryUpdated = MandatoryList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                            if ((MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements) || (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements) || (MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)MandatoryUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if (MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMinUpdate").ToString(), originalCompatibility.MinimumElements, MandatoryUpdated.MinimumElements) });
                                                }
                                                if (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMaxUpdate").ToString(), originalCompatibility.MaximumElements, MandatoryUpdated.MaximumElements) });
                                                }
                                                if ((MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(MandatoryUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }

                                        if (originalCompatibility.IdTypeCompatibility == 255 && SuggestedList != null && SuggestedList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility SuggestedUpdated = SuggestedList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                            if (SuggestedUpdated.Remarks != originalCompatibility.Remarks)
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)SuggestedUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if ((SuggestedUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(SuggestedUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }

                                        if (originalCompatibility.IdTypeCompatibility == 256 && IncompatibleList != null && IncompatibleList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility IncompatibleUpdated = IncompatibleList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);

                                            if ((IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType) || (IncompatibleUpdated.Quantity != originalCompatibility.Quantity) || (IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)IncompatibleUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if (IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType)
                                                {
                                                    IsArticleSync = true;
                                                    if (originalCompatibility.RelationshipType == null)
                                                        originalCompatibility.RelationshipType = new LookupValue();

                                                    string relationShip = RelationShipList.FirstOrDefault(a => a.IdLookupValue == articleCompatibility.IdRelationshipType).Value;

                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelationshipUpdate").ToString(), originalCompatibility.RelationshipType.Value, relationShip) });
                                                }

                                                if ((IncompatibleUpdated.Quantity != originalCompatibility.Quantity))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Quantity.ToString()))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Quantity.ToString()))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), "None", IncompatibleUpdated.Quantity) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, IncompatibleUpdated.Quantity) });
                                                    }
                                                }


                                                if ((IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (ImagesList != null)
                                    {
                                        ulong pos = 1;
                                        foreach (PCMArticleImage img in ImagesList)
                                        {
                                            img.Position = pos;
                                            pos++;
                                        }
                                    }


                                    //Images
                                    UpdatedArticle.PCMArticleImageList = new List<PCMArticleImage>();

                                    foreach (PCMArticleImage item in ClonedArticle.PCMArticleImageList)
                                    {

                                        if (!ImagesList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                        {
                                            IsArticleSync = true;
                                            PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                            pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                            if (pCMArticleImage.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                                        }
                                    }



                                    //Added Article Image
                                    foreach (PCMArticleImage item in ImagesList)
                                    {
                                        if (!(item.IsWarehouseImage == 1))
                                        {
                                            if (!ClonedArticle.PCMArticleImageList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                            {
                                                IsArticleSync = true;
                                                PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                                pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                                if (pCMArticleImage.Position == 1)
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                                else
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                                            }
                                        }
                                    }
                                    //Updated Article Image

                                    foreach (PCMArticleImage originalArticleTypeImage in ClonedArticle.PCMArticleImageList)
                                    {
                                        if (ImagesList != null && ImagesList.Any(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage))
                                        {
                                            PCMArticleImage articleImageUpdated = ImagesList.FirstOrDefault(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage);
                                            if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName) ||
                                                (articleImageUpdated.SavedFileName != originalArticleTypeImage.SavedFileName) ||
                                                (articleImageUpdated.Description != originalArticleTypeImage.Description) ||
                                                (articleImageUpdated.Position != originalArticleTypeImage.Position) ||
                                                (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer))
                                            {
                                                PCMArticleImage productTypeImage = (PCMArticleImage)articleImageUpdated.Clone();
                                                productTypeImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.PCMArticleImageList.Add(productTypeImage);
                                                if (articleImageUpdated.PCMArticleImageInBytes != originalArticleTypeImage.PCMArticleImageInBytes)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesUpdate").ToString(), originalArticleTypeImage.SavedFileName, articleImageUpdated.SavedFileName) });
                                                }
                                                if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleImageUpdated.OriginalFileName))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticleTypeImage.OriginalFileName))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), "None", articleImageUpdated.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, articleImageUpdated.OriginalFileName) });
                                                    }
                                                }
                                                if (articleImageUpdated.Description != originalArticleTypeImage.Description)
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleImageUpdated.Description))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticleTypeImage.Description))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), "None", articleImageUpdated.Description) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, articleImageUpdated.Description) });
                                                    }
                                                }

                                                if (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer)
                                                {
                                                    IsArticleSync = true;
                                                    if (articleImageUpdated.IsImageShareWithCustomer == 1 && originalArticleTypeImage.IsImageShareWithCustomer == 0)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "OFF", "ON") });
                                                    else if (articleImageUpdated.IsImageShareWithCustomer == 0 && originalArticleTypeImage.IsImageShareWithCustomer == 1)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "ON", "OFF") });
                                                }

                                                if (articleImageUpdated.Position != originalArticleTypeImage.Position)
                                                {
                                                    IsArticleSync = true;
                                                    if (originalArticleTypeImage.IsWarehouseImage != 1)
                                                    {
                                                        if (originalArticleTypeImage.Position == 1)
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    //if ((ImagesList.Any(a => a.IsWarehouseImage == 1)))
                                    {
                                        PCMArticleImage tempDefaultImage = ClonedArticle.PCMArticleImageList.FirstOrDefault(x => x.Position == 1);
                                        PCMArticleImage tempDefaultImage_updated = ImagesList.FirstOrDefault(x => x.Position == 1);
                                        PCMArticleImage WarehouseImg = ImagesList.FirstOrDefault(x => x.IsWarehouseImage == 1);

                                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdPCMArticleImage != tempDefaultImage_updated.IdPCMArticleImage)
                                        {
                                            IsArticleSync = true;
                                            if (tempDefaultImage_updated.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                            else if (tempDefaultImage_updated.Position == 1 && tempDefaultImage_updated.IsWarehouseImage == 1)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), tempDefaultImage, tempDefaultImage_updated.OriginalFileName) });
                                            }
                                        }


                                        if (OldWarehouseImage != null && WarehouseImg != null)
                                        {
                                            IsArticleSync = true;
                                            if (oldWarehouseposition != WarehouseImg.Position)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), oldWarehouseposition, WarehouseImg.Position, WarehouseImg.OriginalFileName) });
                                            }
                                            if (oldWarehouseposition != WarehouseImg.Position && oldWarehouseposition == 1)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), WarehouseImg.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                            }

                                        }
                                    }

                                    if (ClonedArticle.IsImageShareWithCustomer != IsImageShareWithCustomer)
                                    {
                                        IsArticleSync = true;
                                        if (ClonedArticle.IsImageShareWithCustomer == 1 && IsImageShareWithCustomer == 0)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "ON", "OFF") });
                                        else if (ClonedArticle.IsImageShareWithCustomer == 0 && IsImageShareWithCustomer == 1)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "OFF", "ON") });
                                    }

                                    UpdatedArticle.PCMArticleImageList.ForEach(x => x.AttachmentImage = null);

                                    /// Attchment Files
                                    UpdatedArticle.PCMArticleAttachmentList = new List<ArticleDocument>();

                                    // Delete Article Attachment file
                                    foreach (ArticleDocument item in ClonedArticle.PCMArticleAttachmentList)
                                    {

                                        if (ArticleFilesList != null && !ArticleFilesList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                        {
                                            IsArticleSync = true;
                                            ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                            articleDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                                        }
                                    }

                                    //Added Article Attachment file
                                    if (ArticleFilesList != null)
                                    {
                                        foreach (ArticleDocument item in ArticleFilesList)
                                        {
                                            if (!ClonedArticle.PCMArticleAttachmentList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                            {
                                                IsArticleSync = true;
                                                ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                                articleDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                                            }
                                        }
                                    }

                                    //Updated Article Attachment file
                                    foreach (ArticleDocument originalArticle in ClonedArticle.PCMArticleAttachmentList)
                                    {
                                        if (ArticleFilesList != null && ArticleFilesList.Any(x => x.IdArticleDoc == originalArticle.IdArticleDoc))
                                        {
                                            ArticleDocument articleAttachedDocUpdated = ArticleFilesList.FirstOrDefault(x => x.IdArticleDoc == originalArticle.IdArticleDoc);
                                            if (originalArticle.ArticleDocumentType == null)
                                                originalArticle.ArticleDocumentType = new ArticleDocumentType();

                                            if (articleAttachedDocUpdated.ArticleDocumentType == null)
                                                articleAttachedDocUpdated.ArticleDocumentType = new ArticleDocumentType();

                                            if ((articleAttachedDocUpdated.SavedFileName != originalArticle.SavedFileName) || (articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName) || (articleAttachedDocUpdated.Description != originalArticle.Description) || (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer) || (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType) || (articleAttachedDocUpdated.PCMArticleFileInBytes != originalArticle.PCMArticleFileInBytes))
                                            {
                                                ArticleDocument articleAttachedDoc = (ArticleDocument)articleAttachedDocUpdated.Clone();
                                                articleAttachedDoc.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.PCMArticleAttachmentList.Add(articleAttachedDoc);

                                                if ((articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleAttachedDocUpdated.OriginalFileName))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticle.OriginalFileName))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, "None", articleAttachedDocUpdated.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, articleAttachedDocUpdated.OriginalFileName) });
                                                    }
                                                }
                                                if (articleAttachedDocUpdated.Description != originalArticle.Description)
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleAttachedDocUpdated.Description))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), originalArticle.OriginalFileName, originalArticle.Description, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticle.Description))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, "None", articleAttachedDoc.Description) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.Description, articleAttachedDoc.Description) });
                                                    }
                                                }
                                                if (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDocumentTypeUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.ArticleDocumentType.DocumentType, articleAttachedDocUpdated.ArticleDocumentType.DocumentType) });
                                                }
                                                if (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer)
                                                {
                                                    IsArticleSync = true;
                                                    if (articleAttachedDocUpdated.IsShareWithCustomer == 0)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString())) });
                                                    else
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString())) });
                                                }
                                            }
                                        }
                                    }

                                    if (SelectedECOSVisibility.IdLookupValue == 323)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 1;
                                        UpdatedArticle.IsSparePartOnly = 0;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 324)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 1;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 325)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 0;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 326)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 0;
                                        UpdatedArticle.PurchaseQtyMin = 0;
                                        UpdatedArticle.PurchaseQtyMax = 0;
                                        IsEnabledMinMax = true;
                                    }

                                    if (UpdatedArticle.PCMDescription.Contains(" ") || UpdatedArticle.PCMDescription_es.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_fr.Contains(" ") || UpdatedArticle.PCMDescription_pt.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_ro.Contains(" ") || UpdatedArticle.PCMDescription_ru.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_zh.Contains(" "))
                                    {
                                        UpdatedArticle.PCMDescription = UpdatedArticle.PCMDescription.Trim(' ');
                                        UpdatedArticle.PCMDescription_es = UpdatedArticle.PCMDescription_es.Trim(' ');
                                        UpdatedArticle.PCMDescription_fr = UpdatedArticle.PCMDescription_fr.Trim(' ');
                                        UpdatedArticle.PCMDescription_pt = UpdatedArticle.PCMDescription_pt.Trim(' ');
                                        UpdatedArticle.PCMDescription_ro = UpdatedArticle.PCMDescription_ro.Trim(' ');
                                        UpdatedArticle.PCMDescription_ru = UpdatedArticle.PCMDescription_ru.Trim(' ');
                                        UpdatedArticle.PCMDescription_zh = UpdatedArticle.PCMDescription_zh.Trim(' ');
                                    }


                                    //[adhatkar][GEOS2-3196]
                                    /// PLM Article Prices
                                    UpdatedArticle.ModifiedPLMArticleList = new List<PLMArticlePrice>();
                                    UpdatedArticle.BasePriceLogEntryList = new List<BasePriceLogEntry>();
                                    UpdatedArticle.CustomerPriceLogEntryList = new List<CustomerPriceLogEntry>();

                                    // Delete PLM Article Prices
                                    if (NotIncludedPLMArticlePriceList != null)
                                    {
                                        foreach (PLMArticlePrice item in NotIncludedPLMArticlePriceList)
                                        {
                                            if (!ClonedArticle.NotIncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDeleteInPCM").ToString(), item.Code, item.Type) });
                                                if (item.Type == "BPL")
                                                    UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                                else
                                                    UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                            }
                                        }
                                    }

                                    //Added PLM Article Prices
                                    if (IncludedPLMArticlePriceList != null)
                                    {
                                        foreach (PLMArticlePrice item in IncludedPLMArticlePriceList)
                                        {
                                            if (!ClonedArticle.IncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAddInPCM").ToString(), item.Code, item.Type) });
                                                if (item.Type == "BPL")
                                                    UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });
                                                else
                                                    UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });

                                            }
                                        }
                                    }

                                    //Updated PLM Article Prices
                                    foreach (PLMArticlePrice originalArticle in ClonedArticle.IncludedPLMArticleList)
                                    {
                                        if (IncludedPLMArticlePriceList != null && IncludedPLMArticlePriceList.Any(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type))
                                        {
                                            PLMArticlePrice PLMArticlePriceUpdated = IncludedPLMArticlePriceList.FirstOrDefault(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type);
                                            if ((PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue) || (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)PLMArticlePriceUpdated.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);


                                                if (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule)
                                                {
                                                    string newRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMArticlePriceUpdated.IdRule).Value;
                                                    string oldRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == originalArticle.IdRule).Value;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogicInPCM").ToString(), oldRuleLogic, newRuleLogic, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                                    if (PLMArticlePriceUpdated.Type == "BPL")
                                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                                    else
                                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                                }

                                                if (PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue)
                                                {
                                                    string oldValue = "";
                                                    string newValue = "";
                                                    if (PLMArticlePriceUpdated.RuleValue == null)
                                                        newValue = "None";
                                                    else
                                                        newValue = PLMArticlePriceUpdated.RuleValue.Value.ToString("0." + new string('#', 339));

                                                    if (originalArticle.RuleValue == null)
                                                        oldValue = "None";
                                                    else
                                                        oldValue = originalArticle.RuleValue.Value.ToString("0." + new string('#', 339));

                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValueInPCM").ToString(), oldValue, newValue, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                                    if (PLMArticlePriceUpdated.Type == "BPL")
                                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                                    else
                                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                                }
                                            }
                                        }
                                    }


                                    //Customers add and Delete
                                    if (UpdatedArticle.ArticleCustomerList == null)
                                        UpdatedArticle.ArticleCustomerList = new List<ArticleCustomer>();
                                    //ObservableCollection<ArticleCustomer> tempCustomersList = ArticleCustomerList;
                                    //ObservableCollection<ArticleCustomer> clonedCustomerListByArticle = new ObservableCollection<ArticleCustomer>(PCMService.GetCustomersByIdArticleCustomerReferences(IdArticle));
                                    // Delete Customer
                                    if (ArticleCustomerList != null)
                                    {

                                        foreach (ArticleCustomer item in ClonedArticle.ArticleCustomerList)
                                        {
                                            if (!ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == item.IdArticleCustomerReferences))
                                            {
                                                IsArticleSync = true;
                                                ArticleCustomer CustomerReference = (ArticleCustomer)item.Clone();
                                                CustomerReference.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                UpdatedArticle.ArticleCustomerList.Add(CustomerReference);
                                                ChangeLogList.Add(new PCMArticleLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomerDelete").ToString(),
                                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                                });
                                            }
                                        }
                                    }

                                    //Added Customer
                                    if (ArticleCustomerList != null)
                                    {
                                        foreach (ArticleCustomer item in ArticleCustomerList)
                                        {
                                            if (!ClonedArticle.ArticleCustomerList.Any(x => x.IdArticleList == item.IdArticleList))
                                            {
                                                IsArticleSync = true;
                                                ArticleCustomer articleCustomer = (ArticleCustomer)item.Clone();
                                                articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                                ChangeLogList.Add(new PCMArticleLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersAdd").ToString(),
                                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                                });

                                            }
                                        }
                                    }


                                    //Updated Customer
                                    if (ClonedArticle.ArticleCustomerList != null)
                                    {
                                        foreach (ArticleCustomer originalCustomer in ClonedArticle.ArticleCustomerList)
                                        {
                                            if (ArticleCustomerList != null && ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences))
                                            {
                                                ArticleCustomer ArticleCustomerListUpdated = ArticleCustomerList.FirstOrDefault(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences);
                                                if ((ArticleCustomerListUpdated.IdGroup != originalCustomer.IdGroup) || (ArticleCustomerListUpdated.IdRegion != originalCustomer.IdRegion) || (ArticleCustomerListUpdated.IdCountry != originalCustomer.IdCountry) || (ArticleCustomerListUpdated.IdPlant != originalCustomer.IdPlant || (ArticleCustomerListUpdated.ReferenceCustomer != originalCustomer.ReferenceCustomer.Trim())))
                                                {
                                                    IsArticleSync = true;
                                                    ArticleCustomer articleCustomer = (ArticleCustomer)ArticleCustomerListUpdated.Clone();
                                                    articleCustomer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                    articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                    UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                                    ChangeLogList.Add(new PCMArticleLogEntry()
                                                    {
                                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersUpdate").ToString(),
                                                        originalCustomer.GroupName, originalCustomer.RegionName,
                                                        originalCustomer.Country.Name, originalCustomer.Plant.Name, articleCustomer.ReferenceCustomer,
                                                        articleCustomer.GroupName, articleCustomer.RegionName,
                                                        articleCustomer.Country.Name, articleCustomer.Plant.Name, articleCustomer.ReferenceCustomer)
                                                    });
                                                }
                                            }
                                        }
                                    }

                                    UpdatedArticle.ModifiedPLMArticleList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);

                                    AddArticleLogDetails();
                                    UpdatedArticle.PCMArticleLogEntiryList = ChangeLogList.ToList();
                                    if (UpdatedArticle.WarehouseArticleLogEntiryList == null)
                                        UpdatedArticle.WarehouseArticleLogEntiryList = new List<LogEntriesByArticle>();
                                    UpdatedArticle.WarehouseArticleLogEntiryList = WMSArticleChangeLogEntry.ToList();
                                    //[003] Added
                                    if (WMSArticleChangeLogEntry.Count == 0)
                                    {
                                        UpdatedArticle.WarehouseArticle = null;
                                    }
                                    UpdatedArticle.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    UpdatedArticle.IdCreator = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                    //[003] Changes service method
                                    IsSave = PCMService.IsUpdatePCMArticle_V2350(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                                    //[Rahul.Gadhave][GEOS2-8318][Date:18-07-2025]
                                    foreach (var linked in linkType5Articles)
                                    {//PRMD
                                        // Clone or adapt the updated article
                                        Articles clonedArticle = GetDeepCopyOfUpdatedArticle(UpdatedArticle);
                                        clonedArticle.IdArticle = (uint)linked.IdArticle;
                                        clonedArticle.Reference = linked.Reference;
                                        //PCMService = new PCMServiceController("localhost:6699");
                                        PCMArticleImageList = PCMService.GetLinkedArticleImage_V2660(clonedArticle.IdArticle);


                                        if (clonedArticle.PCMArticleImageList != null)
                                        {
                                            foreach (var image in clonedArticle.PCMArticleImageList)
                                            {
                                                if (image.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                                {
                                                    var match = PCMArticleImageList.FirstOrDefault(p => string.Equals(p.OriginalFileName, image.OriginalFileName, StringComparison.OrdinalIgnoreCase));

                                                    if (match != null)
                                                    {
                                                        image.IdPCMArticleImage = match.IdPCMArticleImage;
                                                    }


                                                }



                                            }
                                        }
                                        clonedArticle.PCMArticleLogEntiryList = null;
                                        clonedArticle.WarehouseArticleLogEntiryList = null;
                                        clonedArticle.BasePriceLogEntryList = null;
                                        clonedArticle.CustomerPriceLogEntryList = null;
                                        clonedArticle.ModifiedPLMArticleList = null;


                                        // Update IdArticle in nested lists (if applicable)
                                        clonedArticle.ArticleCompatibilityList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        //clonedArticle.PCMArticleLogEntiryList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        clonedArticle.PCMArticleImageList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        // If needed, uncomment and use this
                                        // clonedArticle.PCMArticleAttachmentList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        //clonedArticle.WarehouseArticleLogEntiryList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        clonedArticle.ArticleCustomerList?.ForEach(c => c.IdArticleList = clonedArticle.IdArticle);

                                        // These may or may not have IdArticle depending on the model; include only if they do:
                                        //clonedArticle.ModifiedPLMArticleList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        //clonedArticle.BasePriceLogEntryList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);
                                        //clonedArticle.CustomerPriceLogEntryList?.ForEach(c => c.IdArticle = clonedArticle.IdArticle);

                                        // Optional: also for WarehouseArticle if needed
                                        if (clonedArticle.WarehouseArticle != null)
                                            clonedArticle.WarehouseArticle.IdArticle =  (Int32)clonedArticle.IdArticle;
                                        //PCMService = new PCMServiceController("localhost:6699");
                                        PCMService.IsUpdatePCMArticle_V2660(clonedArticle.IdPCMArticleCategory, clonedArticle, IsDetailsChecked, IsPricesChecked);
                                    }
                                    // IsSave = PCMService.IsUpdatePCMArticle_V2260(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                    IsAdded = true;
                                    IsAcceptButtonEnabled = false;
                                    //[002] Added code for synchronization
                                    if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                                    {
                                        GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59,61");
                                        if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                                        {
                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 57))
                                            {
                                                if (!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                                                {
                                                    if (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue)))
                                                    {
                                                    #region Synchronization code
                                                    CancelSync:;
                                                        var ownerInfo = (groupBox as FrameworkElement);
                                                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                                        if (MessageBoxResult == MessageBoxResult.Yes)
                                                        {
                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is running";

                                                            if (!string.IsNullOrEmpty(UpdatedArticle.Reference.Trim()))
                                                            {
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

                                                                    APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
                                                                    APIErrorDetail values = new APIErrorDetail();
                                                                    tokenService = new AuthTokenService();
                                                                    List<ErrorDetails> LstErrorDetail = new List<ErrorDetails>();
                                                                    bool IsArticleSynchronization = true;
                                                                    //GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59");
                                                                    if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                                                    {
                                                                        string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                                                        if (tokeninformations.Count() >= 2)
                                                                        {
                                                                            if (UpdatedArticle.ModifiedPLMArticleList.Any(i => i.IdStatus == 223) && GeosAppSettingList.Any(i => i.IdAppSetting == 61))
                                                                            {
                                                                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                                //[rdixit][22.02.2023][GEOS2-4176]
                                                                                PCMArticleSynchronizationViewModel PCMArticleSynchronizationViewModel = new PCMArticleSynchronizationViewModel();
                                                                                PCMArticleSynchronizationView PCMArticleSynchronizationView = new PCMArticleSynchronizationView();
                                                                                EventHandler handle = delegate { PCMArticleSynchronizationView.Close(); };
                                                                                PCMArticleSynchronizationViewModel.RequestClose += handle;
                                                                                PCMArticleSynchronizationView.DataContext = PCMArticleSynchronizationViewModel;
                                                                                PCMArticleSynchronizationViewModel.Init(IncludedPLMArticlePriceList, UpdatedArticle);
                                                                                PCMArticleSynchronizationView.ShowDialogWindow();
                                                                                if (PCMArticleSynchronizationViewModel.IsSave)
                                                                                {
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

                                                                                    if (!string.IsNullOrEmpty((GeosAppSettingList[2].DefaultValue)))
                                                                                    {
                                                                                        string IdsBPL = "";
                                                                                        if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "BPL" && pd.IdStatus == 223))
                                                                                        {
                                                                                            IdsBPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                                        }
                                                                                        string IdsCPL = "";
                                                                                        if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "CPL" && pd.IdStatus == 223))
                                                                                        {
                                                                                            IdsCPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                                        }
                                                                                        //[rdixit][22.02.2023][GEOS2-4176]
                                                                                        if (PCMArticleSynchronizationViewModel.BPLPlantCurrencyList != null)
                                                                                        {
                                                                                            List<BPLPlantCurrencyDetail> BPLPlantCurrencyDetailList = PCMArticleSynchronizationViewModel.BPLPlantCurrencyList.ToList();//PLMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Convert.ToInt32(UpdatedArticle.IdArticle), IdsBPL, IdsCPL, "Article");

                                                                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 61) && BPLPlantCurrencyDetailList != null)
                                                                                            {

                                                                                                foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                                                                                {
                                                                                                    if (IsArticleSync == true && IsArticleSynchronization == true)
                                                                                                    {
                                                                                                        IsArticleSynchronization = true;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        IsArticleSynchronization = false;
                                                                                                    }
                                                                                                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                    List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization_V2310(GeosAppSettingList, itemBPLPlantCurrency, UpdatedArticle, IsArticleSynchronization);
                                                                                                    IsArticleSynchronization = false;
                                                                                                    #region
                                                                                                    foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                                    {
                                                                                                        if (item != null && item.Error == string.Empty)
                                                                                                        {
                                                                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is Done for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (item != null && item.Error != null)
                                                                                                            {
                                                                                                                GeosApplication.Instance.SplashScreenMessage = "Synchronization is failed for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                                ErrorDetails errorDetails = new ErrorDetails();
                                                                                                                errorDetails.CompanyName = itemBPLPlantCurrency.CompanyName;
                                                                                                                errorDetails.CurrencyName = itemBPLPlantCurrency.CurrencyName;
                                                                                                                errorDetails.Error = item.Error;
                                                                                                                LstErrorDetail.Add(errorDetails);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    #endregion
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationCancelled").ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                                                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                                    goto CancelSync;
                                                                                }
                                                                            }
                                                                            else
                                                                            {

                                                                                List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization(GeosAppSettingList, null, UpdatedArticle);
                                                                                foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                {
                                                                                    if (item != null && item.Error == string.Empty)
                                                                                    {

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (item != null && item.Error != null)
                                                                                        {
                                                                                            ErrorDetails errorDetails = new ErrorDetails();
                                                                                            errorDetails.CompanyName = "None";
                                                                                            errorDetails.CurrencyName = "None";
                                                                                            errorDetails.Error = item.Error;
                                                                                            LstErrorDetail.Add(errorDetails);
                                                                                        }
                                                                                    }
                                                                                }


                                                                            }
                                                                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                            if (LstErrorDetail != null && LstErrorDetail.Count > 0)
                                                                            {
                                                                                String FinalMessage = null;
                                                                                string msg = string.Empty;
                                                                                foreach (ErrorDetails item in LstErrorDetail)
                                                                                {

                                                                                    if (string.IsNullOrEmpty(FinalMessage))
                                                                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error), Window.GetWindow(ownerInfo));
                                                                                    else
                                                                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error);

                                                                                }
                                                                                if (!string.IsNullOrEmpty(FinalMessage))
                                                                                {
                                                                                    CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                            }
                                                                        }

                                                                    }

                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                                }

                                                            }
                                                            //CancelSync:;                                          
                                                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                        }
                                                        #endregion Synchronization
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    RequestClose(null, null);
                                    IsAdded = false;
                                    IsAcceptButtonEnabled = true;
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                                }
                            }
                            else
                            {
                                if (!IsAdded)
                                {

                                    ChangeLogList = new ObservableCollection<PCMArticleLogEntry>();
                                    WMSArticleChangeLogEntry = new List<LogEntriesByArticle>();
                                    UpdatedArticle = new Articles();

                                    if (PQuantityMin > PQuantityMax)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMinLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;

                                    }
                                    if (PQuantityMax < PQuantityMin)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMaxLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;
                                    }

                                    if (SelectedECOSVisibility.IdLookupValue != 326 && (PQuantityMin == 0 || PQuantityMax == 0))
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityReadonly").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;
                                    }

                                    if (Description != null)
                                    {
                                        IsEmptySpace = IsEmptySpaceAction(Description);
                                        if (IsEmptySpace)
                                        {
                                            Description = string.Empty;
                                            UpdatedArticle.WarehouseArticle.Description = Description;
                                            if (Description == "" || Description == string.Empty || Description == null)
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    allowValidation = true;

                                    GroupBox groupBox = (GroupBox)((object[])obj)[0];
                                    GroupBox groupBox1 = (GroupBox)((object[])obj)[1];

                                    if (MandatoryList != null && MandatoryList.Any(x => x.MinimumElements > x.MaximumElements))
                                    {
                                        CompatibilityError = null;
                                        PropertyChanged(this, new PropertyChangedEventArgs("CompatibilityError"));

                                        TableView foundTextBox = UI.Helper.FindInnerChild.FindChild<TableView>(groupBox, "MandatoryListTableView");
                                        if (foundTextBox != null)
                                        {
                                            foundTextBox.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;
                                            foundTextBox.UpdateLayout();
                                        }

                                        error = EnableValidationAndGetError();

                                        if (string.IsNullOrEmpty(error))
                                        {
                                            return;
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        CompatibilityError = " ";
                                    }

                                    if (SelectedStatus.IdLookupValue == 0 || SelectedCategory.IdPCMArticleCategory == 0)
                                    {
                                        InformationError = null;
                                        error = EnableValidationAndGetError();

                                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatus"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));


                                        if (error != null)
                                        {
                                            return;
                                        }

                                    }
                                    if (Description == null)
                                    {
                                        error = EnableValidationAndGetError();
                                        PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                                        //PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                                        if (error != null)
                                        {
                                            return;
                                        }
                                    }
                                    if (Description != null && Description_en == null)
                                    {
                                        InformationError = null;
                                        error = EnableValidationAndGetError();
                                        PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                                        if (error != null)
                                        {
                                            return;
                                        }
                                    }
                                    else
                                        InformationError = " ";

                                    if (UpdatedArticle.WarehouseArticle == null)
                                    {
                                        UpdatedArticle.WarehouseArticle = new Article();
                                    }

                                    UpdatedArticle.IdPCMArticleCategory = SelectedCategory.IdPCMArticleCategory;
                                    UpdatedArticle.IdPCMStatus = SelectedStatus.IdLookupValue;
                                    UpdatedArticle.IdArticle = IdArticle;
                                    UpdatedArticle.WarehouseArticle.IdArticle = (int)IdArticle;
                                    UpdatedArticle.Reference = Reference;

                                    if (IsCheckedCopyName == true)
                                    {
                                        IsFromInformation = true;
                                        UpdatedArticle.WarehouseArticle.Description = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_es = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_fr = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_pt = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ro = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ru = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_zh = Description == null ? "" : Description.Trim();
                                    }
                                    else
                                    {
                                        IsFromInformation = true;
                                        UpdatedArticle.WarehouseArticle.Description = Description_en == null ? "" : Description_en.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_es = Description_es == null ? "" : Description_es.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_zh = Description_zh == null ? "" : Description_zh.Trim();
                                    }

                                    UpdatedArticle = PcmArticleCatalogueDescriptionManager.UpdateArticleNormalOrRtfPCMDescriptionFromLocalData(UpdatedArticle);

                                    if (IsRtf)
                                    {
                                        if (IsCheckedCopyCatelogueDescription == true)
                                        {
                                            IsFromInformation = false;
                                        }
                                        else
                                        {
                                            IsFromInformation = false;
                                        }
                                    }
                                    if (IsFromInformation)
                                    {
                                        if (IsCheckedCopyCatelogueDescription == true)
                                        {
                                            IsFromInformation = false;
                                        }
                                        else
                                        {
                                            IsFromInformation = false;
                                        }
                                    }


                                    UpdatedArticle.IdECOSVisibility = SelectedECOSVisibility.IdLookupValue;
                                    UpdatedArticle.ECOSVisibilityValue = SelectedECOSVisibility.Value;

                                    UpdatedArticle.IsImageShareWithCustomer = IsImageShareWithCustomer;

                                    UpdatedArticle.PurchaseQtyMin = PQuantityMin;
                                    UpdatedArticle.PurchaseQtyMax = PQuantityMax;

                                    UpdatedArticle.ECOSVisibilityHTMLColor = SelectedECOSVisibility.HtmlColor;

                                    UpdatedArticle.ArticleCompatibilityList = new List<ArticleCompatibility>();

                                    // Delete Compatibility
                                    foreach (ArticleCompatibility item in ClonedArticle.ArticleCompatibilityList)
                                    {
                                        if (item.IdTypeCompatibility == 254 && MandatoryList != null && !MandatoryList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                        if (item.IdTypeCompatibility == 255 && SuggestedList != null && !SuggestedList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                        if (item.IdTypeCompatibility == 256 && IncompatibleList != null && !IncompatibleList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }


                                    //Added Compatibility
                                    foreach (ArticleCompatibility item in MandatoryList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }
                                    foreach (ArticleCompatibility item in SuggestedList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }
                                    foreach (ArticleCompatibility item in IncompatibleList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }

                                    //Updated Compatibility
                                    foreach (ArticleCompatibility originalCompatibility in ClonedArticle.ArticleCompatibilityList)
                                    {
                                        if (originalCompatibility.IdTypeCompatibility == 254 && MandatoryList != null && MandatoryList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility MandatoryUpdated = MandatoryList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                            if ((MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements) || (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements) || (MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)MandatoryUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if (MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMinUpdate").ToString(), originalCompatibility.MinimumElements, MandatoryUpdated.MinimumElements) });
                                                }
                                                if (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMaxUpdate").ToString(), originalCompatibility.MaximumElements, MandatoryUpdated.MaximumElements) });
                                                }
                                                if ((MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(MandatoryUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }

                                        if (originalCompatibility.IdTypeCompatibility == 255 && SuggestedList != null && SuggestedList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility SuggestedUpdated = SuggestedList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                            if (SuggestedUpdated.Remarks != originalCompatibility.Remarks)
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)SuggestedUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if ((SuggestedUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(SuggestedUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }

                                        if (originalCompatibility.IdTypeCompatibility == 256 && IncompatibleList != null && IncompatibleList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility IncompatibleUpdated = IncompatibleList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);

                                            if ((IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType) || (IncompatibleUpdated.Quantity != originalCompatibility.Quantity) || (IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)IncompatibleUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if (IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType)
                                                {
                                                    IsArticleSync = true;
                                                    if (originalCompatibility.RelationshipType == null)
                                                        originalCompatibility.RelationshipType = new LookupValue();

                                                    string relationShip = RelationShipList.FirstOrDefault(a => a.IdLookupValue == articleCompatibility.IdRelationshipType).Value;

                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelationshipUpdate").ToString(), originalCompatibility.RelationshipType.Value, relationShip) });
                                                }

                                                if ((IncompatibleUpdated.Quantity != originalCompatibility.Quantity))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Quantity.ToString()))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Quantity.ToString()))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), "None", IncompatibleUpdated.Quantity) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, IncompatibleUpdated.Quantity) });
                                                    }
                                                }


                                                if ((IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (ImagesList != null)
                                    {
                                        ulong pos = 1;
                                        foreach (PCMArticleImage img in ImagesList)
                                        {
                                            img.Position = pos;
                                            pos++;
                                        }
                                    }


                                    //Images
                                    UpdatedArticle.PCMArticleImageList = new List<PCMArticleImage>();

                                    foreach (PCMArticleImage item in ClonedArticle.PCMArticleImageList)
                                    {

                                        if (!ImagesList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                        {
                                            IsArticleSync = true;
                                            PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                            pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                            if (pCMArticleImage.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                                        }
                                    }



                                    //Added Article Image
                                    foreach (PCMArticleImage item in ImagesList)
                                    {
                                        if (!(item.IsWarehouseImage == 1))
                                        {
                                            if (!ClonedArticle.PCMArticleImageList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                            {
                                                IsArticleSync = true;
                                                PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                                pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                                if (pCMArticleImage.Position == 1)
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                                else
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                                            }
                                        }
                                    }
                                    //Updated Article Image

                                    foreach (PCMArticleImage originalArticleTypeImage in ClonedArticle.PCMArticleImageList)
                                    {
                                        if (ImagesList != null && ImagesList.Any(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage))
                                        {
                                            PCMArticleImage articleImageUpdated = ImagesList.FirstOrDefault(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage);
                                            if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName) ||
                                                (articleImageUpdated.SavedFileName != originalArticleTypeImage.SavedFileName) ||
                                                (articleImageUpdated.Description != originalArticleTypeImage.Description) ||
                                                (articleImageUpdated.Position != originalArticleTypeImage.Position) ||
                                                (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer))
                                            {
                                                PCMArticleImage productTypeImage = (PCMArticleImage)articleImageUpdated.Clone();
                                                productTypeImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.PCMArticleImageList.Add(productTypeImage);
                                                if (articleImageUpdated.PCMArticleImageInBytes != originalArticleTypeImage.PCMArticleImageInBytes)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesUpdate").ToString(), originalArticleTypeImage.SavedFileName, articleImageUpdated.SavedFileName) });
                                                }
                                                if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleImageUpdated.OriginalFileName))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticleTypeImage.OriginalFileName))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), "None", articleImageUpdated.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, articleImageUpdated.OriginalFileName) });
                                                    }
                                                }
                                                if (articleImageUpdated.Description != originalArticleTypeImage.Description)
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleImageUpdated.Description))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticleTypeImage.Description))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), "None", articleImageUpdated.Description) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, articleImageUpdated.Description) });
                                                    }
                                                }

                                                if (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer)
                                                {
                                                    IsArticleSync = true;
                                                    if (articleImageUpdated.IsImageShareWithCustomer == 1 && originalArticleTypeImage.IsImageShareWithCustomer == 0)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "OFF", "ON") });
                                                    else if (articleImageUpdated.IsImageShareWithCustomer == 0 && originalArticleTypeImage.IsImageShareWithCustomer == 1)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "ON", "OFF") });
                                                }

                                                if (articleImageUpdated.Position != originalArticleTypeImage.Position)
                                                {
                                                    IsArticleSync = true;
                                                    if (originalArticleTypeImage.IsWarehouseImage != 1)
                                                    {
                                                        if (originalArticleTypeImage.Position == 1)
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    //if ((ImagesList.Any(a => a.IsWarehouseImage == 1)))
                                    {
                                        PCMArticleImage tempDefaultImage = ClonedArticle.PCMArticleImageList.FirstOrDefault(x => x.Position == 1);
                                        PCMArticleImage tempDefaultImage_updated = ImagesList.FirstOrDefault(x => x.Position == 1);
                                        PCMArticleImage WarehouseImg = ImagesList.FirstOrDefault(x => x.IsWarehouseImage == 1);

                                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdPCMArticleImage != tempDefaultImage_updated.IdPCMArticleImage)
                                        {
                                            IsArticleSync = true;
                                            if (tempDefaultImage_updated.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                            else if (tempDefaultImage_updated.Position == 1 && tempDefaultImage_updated.IsWarehouseImage == 1)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), tempDefaultImage, tempDefaultImage_updated.OriginalFileName) });
                                            }
                                        }


                                        if (OldWarehouseImage != null && WarehouseImg != null)
                                        {
                                            IsArticleSync = true;
                                            if (oldWarehouseposition != WarehouseImg.Position)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), oldWarehouseposition, WarehouseImg.Position, WarehouseImg.OriginalFileName) });
                                            }
                                            if (oldWarehouseposition != WarehouseImg.Position && oldWarehouseposition == 1)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), WarehouseImg.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                            }

                                        }
                                    }

                                    if (ClonedArticle.IsImageShareWithCustomer != IsImageShareWithCustomer)
                                    {
                                        IsArticleSync = true;
                                        if (ClonedArticle.IsImageShareWithCustomer == 1 && IsImageShareWithCustomer == 0)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "ON", "OFF") });
                                        else if (ClonedArticle.IsImageShareWithCustomer == 0 && IsImageShareWithCustomer == 1)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "OFF", "ON") });
                                    }

                                    UpdatedArticle.PCMArticleImageList.ForEach(x => x.AttachmentImage = null);

                                    /// Attchment Files
                                    UpdatedArticle.PCMArticleAttachmentList = new List<ArticleDocument>();

                                    // Delete Article Attachment file
                                    foreach (ArticleDocument item in ClonedArticle.PCMArticleAttachmentList)
                                    {

                                        if (ArticleFilesList != null && !ArticleFilesList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                        {
                                            IsArticleSync = true;
                                            ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                            articleDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                                        }
                                    }

                                    //Added Article Attachment file
                                    if (ArticleFilesList != null)
                                    {
                                        foreach (ArticleDocument item in ArticleFilesList)
                                        {
                                            if (!ClonedArticle.PCMArticleAttachmentList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                            {
                                                IsArticleSync = true;
                                                ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                                articleDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                                            }
                                        }
                                    }

                                    //Updated Article Attachment file
                                    foreach (ArticleDocument originalArticle in ClonedArticle.PCMArticleAttachmentList)
                                    {
                                        if (ArticleFilesList != null && ArticleFilesList.Any(x => x.IdArticleDoc == originalArticle.IdArticleDoc))
                                        {
                                            ArticleDocument articleAttachedDocUpdated = ArticleFilesList.FirstOrDefault(x => x.IdArticleDoc == originalArticle.IdArticleDoc);
                                            if (originalArticle.ArticleDocumentType == null)
                                                originalArticle.ArticleDocumentType = new ArticleDocumentType();

                                            if (articleAttachedDocUpdated.ArticleDocumentType == null)
                                                articleAttachedDocUpdated.ArticleDocumentType = new ArticleDocumentType();

                                            if ((articleAttachedDocUpdated.SavedFileName != originalArticle.SavedFileName) || (articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName) || (articleAttachedDocUpdated.Description != originalArticle.Description) || (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer) || (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType) || (articleAttachedDocUpdated.PCMArticleFileInBytes != originalArticle.PCMArticleFileInBytes))
                                            {
                                                ArticleDocument articleAttachedDoc = (ArticleDocument)articleAttachedDocUpdated.Clone();
                                                articleAttachedDoc.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.PCMArticleAttachmentList.Add(articleAttachedDoc);

                                                if ((articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleAttachedDocUpdated.OriginalFileName))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticle.OriginalFileName))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, "None", articleAttachedDocUpdated.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, articleAttachedDocUpdated.OriginalFileName) });
                                                    }
                                                }
                                                if (articleAttachedDocUpdated.Description != originalArticle.Description)
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleAttachedDocUpdated.Description))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), originalArticle.OriginalFileName, originalArticle.Description, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticle.Description))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, "None", articleAttachedDoc.Description) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.Description, articleAttachedDoc.Description) });
                                                    }
                                                }
                                                if (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDocumentTypeUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.ArticleDocumentType.DocumentType, articleAttachedDocUpdated.ArticleDocumentType.DocumentType) });
                                                }
                                                if (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer)
                                                {
                                                    IsArticleSync = true;
                                                    if (articleAttachedDocUpdated.IsShareWithCustomer == 0)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString())) });
                                                    else
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString())) });
                                                }
                                            }
                                        }
                                    }

                                    if (SelectedECOSVisibility.IdLookupValue == 323)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 1;
                                        UpdatedArticle.IsSparePartOnly = 0;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 324)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 1;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 325)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 0;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 326)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 0;
                                        UpdatedArticle.PurchaseQtyMin = 0;
                                        UpdatedArticle.PurchaseQtyMax = 0;
                                        IsEnabledMinMax = true;
                                    }

                                    if (UpdatedArticle.PCMDescription.Contains(" ") || UpdatedArticle.PCMDescription_es.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_fr.Contains(" ") || UpdatedArticle.PCMDescription_pt.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_ro.Contains(" ") || UpdatedArticle.PCMDescription_ru.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_zh.Contains(" "))
                                    {
                                        UpdatedArticle.PCMDescription = UpdatedArticle.PCMDescription.Trim(' ');
                                        UpdatedArticle.PCMDescription_es = UpdatedArticle.PCMDescription_es.Trim(' ');
                                        UpdatedArticle.PCMDescription_fr = UpdatedArticle.PCMDescription_fr.Trim(' ');
                                        UpdatedArticle.PCMDescription_pt = UpdatedArticle.PCMDescription_pt.Trim(' ');
                                        UpdatedArticle.PCMDescription_ro = UpdatedArticle.PCMDescription_ro.Trim(' ');
                                        UpdatedArticle.PCMDescription_ru = UpdatedArticle.PCMDescription_ru.Trim(' ');
                                        UpdatedArticle.PCMDescription_zh = UpdatedArticle.PCMDescription_zh.Trim(' ');
                                    }


                                    //[adhatkar][GEOS2-3196]
                                    /// PLM Article Prices
                                    UpdatedArticle.ModifiedPLMArticleList = new List<PLMArticlePrice>();
                                    UpdatedArticle.BasePriceLogEntryList = new List<BasePriceLogEntry>();
                                    UpdatedArticle.CustomerPriceLogEntryList = new List<CustomerPriceLogEntry>();

                                    // Delete PLM Article Prices
                                    if (NotIncludedPLMArticlePriceList != null)
                                    {
                                        foreach (PLMArticlePrice item in NotIncludedPLMArticlePriceList)
                                        {
                                            if (!ClonedArticle.NotIncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDeleteInPCM").ToString(), item.Code, item.Type) });
                                                if (item.Type == "BPL")
                                                    UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                                else
                                                    UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                            }
                                        }
                                    }

                                    //Added PLM Article Prices
                                    if (IncludedPLMArticlePriceList != null)
                                    {
                                        foreach (PLMArticlePrice item in IncludedPLMArticlePriceList)
                                        {
                                            if (!ClonedArticle.IncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAddInPCM").ToString(), item.Code, item.Type) });
                                                if (item.Type == "BPL")
                                                    UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });
                                                else
                                                    UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });

                                            }
                                        }
                                    }

                                    //Updated PLM Article Prices
                                    foreach (PLMArticlePrice originalArticle in ClonedArticle.IncludedPLMArticleList)
                                    {
                                        if (IncludedPLMArticlePriceList != null && IncludedPLMArticlePriceList.Any(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type))
                                        {
                                            PLMArticlePrice PLMArticlePriceUpdated = IncludedPLMArticlePriceList.FirstOrDefault(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type);
                                            if ((PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue) || (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)PLMArticlePriceUpdated.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);


                                                if (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule)
                                                {
                                                    string newRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMArticlePriceUpdated.IdRule).Value;
                                                    string oldRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == originalArticle.IdRule).Value;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogicInPCM").ToString(), oldRuleLogic, newRuleLogic, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                                    if (PLMArticlePriceUpdated.Type == "BPL")
                                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                                    else
                                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                                }

                                                if (PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue)
                                                {
                                                    string oldValue = "";
                                                    string newValue = "";
                                                    if (PLMArticlePriceUpdated.RuleValue == null)
                                                        newValue = "None";
                                                    else
                                                        newValue = PLMArticlePriceUpdated.RuleValue.Value.ToString("0." + new string('#', 339));

                                                    if (originalArticle.RuleValue == null)
                                                        oldValue = "None";
                                                    else
                                                        oldValue = originalArticle.RuleValue.Value.ToString("0." + new string('#', 339));

                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValueInPCM").ToString(), oldValue, newValue, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                                    if (PLMArticlePriceUpdated.Type == "BPL")
                                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                                    else
                                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                                }
                                            }
                                        }
                                    }


                                    //Customers add and Delete
                                    if (UpdatedArticle.ArticleCustomerList == null)
                                        UpdatedArticle.ArticleCustomerList = new List<ArticleCustomer>();
                                    //ObservableCollection<ArticleCustomer> tempCustomersList = ArticleCustomerList;
                                    //ObservableCollection<ArticleCustomer> clonedCustomerListByArticle = new ObservableCollection<ArticleCustomer>(PCMService.GetCustomersByIdArticleCustomerReferences(IdArticle));
                                    // Delete Customer
                                    if (ArticleCustomerList != null)
                                    {

                                        foreach (ArticleCustomer item in ClonedArticle.ArticleCustomerList)
                                        {
                                            if (!ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == item.IdArticleCustomerReferences))
                                            {
                                                IsArticleSync = true;
                                                ArticleCustomer CustomerReference = (ArticleCustomer)item.Clone();
                                                CustomerReference.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                UpdatedArticle.ArticleCustomerList.Add(CustomerReference);
                                                ChangeLogList.Add(new PCMArticleLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomerDelete").ToString(),
                                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                                });
                                            }
                                        }
                                    }

                                    //Added Customer
                                    if (ArticleCustomerList != null)
                                    {
                                        foreach (ArticleCustomer item in ArticleCustomerList)
                                        {
                                            if (!ClonedArticle.ArticleCustomerList.Any(x => x.IdArticleList == item.IdArticleList))
                                            {
                                                IsArticleSync = true;
                                                ArticleCustomer articleCustomer = (ArticleCustomer)item.Clone();
                                                articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                                ChangeLogList.Add(new PCMArticleLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersAdd").ToString(),
                                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                                });

                                            }
                                        }
                                    }


                                    //Updated Customer
                                    if (ClonedArticle.ArticleCustomerList != null)
                                    {
                                        foreach (ArticleCustomer originalCustomer in ClonedArticle.ArticleCustomerList)
                                        {
                                            if (ArticleCustomerList != null && ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences))
                                            {
                                                ArticleCustomer ArticleCustomerListUpdated = ArticleCustomerList.FirstOrDefault(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences);
                                                if ((ArticleCustomerListUpdated.IdGroup != originalCustomer.IdGroup) || (ArticleCustomerListUpdated.IdRegion != originalCustomer.IdRegion) || (ArticleCustomerListUpdated.IdCountry != originalCustomer.IdCountry) || (ArticleCustomerListUpdated.IdPlant != originalCustomer.IdPlant || (ArticleCustomerListUpdated.ReferenceCustomer != originalCustomer.ReferenceCustomer.Trim())))
                                                {
                                                    IsArticleSync = true;
                                                    ArticleCustomer articleCustomer = (ArticleCustomer)ArticleCustomerListUpdated.Clone();
                                                    articleCustomer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                    articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                    UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                                    ChangeLogList.Add(new PCMArticleLogEntry()
                                                    {
                                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersUpdate").ToString(),
                                                        originalCustomer.GroupName, originalCustomer.RegionName,
                                                        originalCustomer.Country.Name, originalCustomer.Plant.Name, articleCustomer.ReferenceCustomer,
                                                        articleCustomer.GroupName, articleCustomer.RegionName,
                                                        articleCustomer.Country.Name, articleCustomer.Plant.Name, articleCustomer.ReferenceCustomer)
                                                    });
                                                }
                                            }
                                        }
                                    }

                                    UpdatedArticle.ModifiedPLMArticleList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);

                                    AddArticleLogDetails();
                                    UpdatedArticle.PCMArticleLogEntiryList = ChangeLogList.ToList();
                                    if (UpdatedArticle.WarehouseArticleLogEntiryList == null)
                                        UpdatedArticle.WarehouseArticleLogEntiryList = new List<LogEntriesByArticle>();
                                    UpdatedArticle.WarehouseArticleLogEntiryList = WMSArticleChangeLogEntry.ToList();
                                    //[003] Added
                                    if (WMSArticleChangeLogEntry.Count == 0)
                                    {
                                        UpdatedArticle.WarehouseArticle = null;
                                    }
                                    UpdatedArticle.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    UpdatedArticle.IdCreator = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                    //[003] Changes service method
                                    IsSave = PCMService.IsUpdatePCMArticle_V2350(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                                    // IsSave = PCMService.IsUpdatePCMArticle_V2260(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                    IsAdded = true;
                                    IsAcceptButtonEnabled = false;
                                    //[002] Added code for synchronization
                                    if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                                    {
                                        GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59,61");
                                        if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                                        {
                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 57))
                                            {
                                                if (!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                                                {
                                                    if (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue)))
                                                    {
                                                    #region Synchronization code
                                                    CancelSync:;
                                                        var ownerInfo = (groupBox as FrameworkElement);
                                                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                                        if (MessageBoxResult == MessageBoxResult.Yes)
                                                        {
                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is running";

                                                            if (!string.IsNullOrEmpty(UpdatedArticle.Reference.Trim()))
                                                            {
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

                                                                    APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
                                                                    APIErrorDetail values = new APIErrorDetail();
                                                                    tokenService = new AuthTokenService();
                                                                    List<ErrorDetails> LstErrorDetail = new List<ErrorDetails>();
                                                                    bool IsArticleSynchronization = true;
                                                                    //GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59");
                                                                    if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                                                    {
                                                                        string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                                                        if (tokeninformations.Count() >= 2)
                                                                        {
                                                                            if (UpdatedArticle.ModifiedPLMArticleList.Any(i => i.IdStatus == 223) && GeosAppSettingList.Any(i => i.IdAppSetting == 61))
                                                                            {
                                                                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                                //[rdixit][22.02.2023][GEOS2-4176]
                                                                                PCMArticleSynchronizationViewModel PCMArticleSynchronizationViewModel = new PCMArticleSynchronizationViewModel();
                                                                                PCMArticleSynchronizationView PCMArticleSynchronizationView = new PCMArticleSynchronizationView();
                                                                                EventHandler handle = delegate { PCMArticleSynchronizationView.Close(); };
                                                                                PCMArticleSynchronizationViewModel.RequestClose += handle;
                                                                                PCMArticleSynchronizationView.DataContext = PCMArticleSynchronizationViewModel;
                                                                                PCMArticleSynchronizationViewModel.Init(IncludedPLMArticlePriceList, UpdatedArticle);
                                                                                PCMArticleSynchronizationView.ShowDialogWindow();
                                                                                if (PCMArticleSynchronizationViewModel.IsSave)
                                                                                {
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

                                                                                    if (!string.IsNullOrEmpty((GeosAppSettingList[2].DefaultValue)))
                                                                                    {
                                                                                        string IdsBPL = "";
                                                                                        if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "BPL" && pd.IdStatus == 223))
                                                                                        {
                                                                                            IdsBPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                                        }
                                                                                        string IdsCPL = "";
                                                                                        if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "CPL" && pd.IdStatus == 223))
                                                                                        {
                                                                                            IdsCPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                                        }
                                                                                        //[rdixit][22.02.2023][GEOS2-4176]
                                                                                        if (PCMArticleSynchronizationViewModel.BPLPlantCurrencyList != null)
                                                                                        {
                                                                                            List<BPLPlantCurrencyDetail> BPLPlantCurrencyDetailList = PCMArticleSynchronizationViewModel.BPLPlantCurrencyList.ToList();//PLMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Convert.ToInt32(UpdatedArticle.IdArticle), IdsBPL, IdsCPL, "Article");

                                                                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 61) && BPLPlantCurrencyDetailList != null)
                                                                                            {

                                                                                                foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                                                                                {
                                                                                                    if (IsArticleSync == true && IsArticleSynchronization == true)
                                                                                                    {
                                                                                                        IsArticleSynchronization = true;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        IsArticleSynchronization = false;
                                                                                                    }
                                                                                                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                    List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization_V2310(GeosAppSettingList, itemBPLPlantCurrency, UpdatedArticle, IsArticleSynchronization);
                                                                                                    IsArticleSynchronization = false;
                                                                                                    #region
                                                                                                    foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                                    {
                                                                                                        if (item != null && item.Error == string.Empty)
                                                                                                        {
                                                                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is Done for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (item != null && item.Error != null)
                                                                                                            {
                                                                                                                GeosApplication.Instance.SplashScreenMessage = "Synchronization is failed for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                                ErrorDetails errorDetails = new ErrorDetails();
                                                                                                                errorDetails.CompanyName = itemBPLPlantCurrency.CompanyName;
                                                                                                                errorDetails.CurrencyName = itemBPLPlantCurrency.CurrencyName;
                                                                                                                errorDetails.Error = item.Error;
                                                                                                                LstErrorDetail.Add(errorDetails);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    #endregion
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationCancelled").ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                                                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                                    goto CancelSync;
                                                                                }
                                                                            }
                                                                            else
                                                                            {

                                                                                List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization(GeosAppSettingList, null, UpdatedArticle);
                                                                                foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                {
                                                                                    if (item != null && item.Error == string.Empty)
                                                                                    {

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (item != null && item.Error != null)
                                                                                        {
                                                                                            ErrorDetails errorDetails = new ErrorDetails();
                                                                                            errorDetails.CompanyName = "None";
                                                                                            errorDetails.CurrencyName = "None";
                                                                                            errorDetails.Error = item.Error;
                                                                                            LstErrorDetail.Add(errorDetails);
                                                                                        }
                                                                                    }
                                                                                }


                                                                            }
                                                                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                            if (LstErrorDetail != null && LstErrorDetail.Count > 0)
                                                                            {
                                                                                String FinalMessage = null;
                                                                                string msg = string.Empty;
                                                                                foreach (ErrorDetails item in LstErrorDetail)
                                                                                {

                                                                                    if (string.IsNullOrEmpty(FinalMessage))
                                                                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error), Window.GetWindow(ownerInfo));
                                                                                    else
                                                                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error);

                                                                                }
                                                                                if (!string.IsNullOrEmpty(FinalMessage))
                                                                                {
                                                                                    CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                            }
                                                                        }

                                                                    }

                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                                }

                                                            }
                                                            //CancelSync:;                                          
                                                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                        }
                                                        #endregion Synchronization
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    RequestClose(null, null);
                                    IsAdded = false;
                                    IsAcceptButtonEnabled = true;
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                                }
                            }
                        }
                        else 
                        {
                            if (promptResult.Result == MessageBoxResult.No)
                            {
                                if (!IsAdded)
                                {

                                    ChangeLogList = new ObservableCollection<PCMArticleLogEntry>();
                                    WMSArticleChangeLogEntry = new List<LogEntriesByArticle>();
                                    UpdatedArticle = new Articles();

                                    if (PQuantityMin > PQuantityMax)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMinLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;

                                    }
                                    if (PQuantityMax < PQuantityMin)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseQuantityMaxLess").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;
                                    }

                                    if (SelectedECOSVisibility.IdLookupValue != 326 && (PQuantityMin == 0 || PQuantityMax == 0))
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityReadonly").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Expiry date must be equal or greater than effective date.", category: Category.Info, priority: Priority.Low);
                                        return;
                                    }

                                    if (Description != null)
                                    {
                                        IsEmptySpace = IsEmptySpaceAction(Description);
                                        if (IsEmptySpace)
                                        {
                                            Description = string.Empty;
                                            UpdatedArticle.WarehouseArticle.Description = Description;
                                            if (Description == "" || Description == string.Empty || Description == null)
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    allowValidation = true;

                                    GroupBox groupBox = (GroupBox)((object[])obj)[0];
                                    GroupBox groupBox1 = (GroupBox)((object[])obj)[1];

                                    if (MandatoryList != null && MandatoryList.Any(x => x.MinimumElements > x.MaximumElements))
                                    {
                                        CompatibilityError = null;
                                        PropertyChanged(this, new PropertyChangedEventArgs("CompatibilityError"));

                                        TableView foundTextBox = UI.Helper.FindInnerChild.FindChild<TableView>(groupBox, "MandatoryListTableView");
                                        if (foundTextBox != null)
                                        {
                                            foundTextBox.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;
                                            foundTextBox.UpdateLayout();
                                        }

                                        error = EnableValidationAndGetError();

                                        if (string.IsNullOrEmpty(error))
                                        {
                                            return;
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        CompatibilityError = " ";
                                    }

                                    if (SelectedStatus.IdLookupValue == 0 || SelectedCategory.IdPCMArticleCategory == 0)
                                    {
                                        InformationError = null;
                                        error = EnableValidationAndGetError();

                                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatus"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));


                                        if (error != null)
                                        {
                                            return;
                                        }

                                    }
                                    if (Description == null)
                                    {
                                        error = EnableValidationAndGetError();
                                        PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                                        //PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                                        if (error != null)
                                        {
                                            return;
                                        }
                                    }
                                    if (Description != null && Description_en == null)
                                    {
                                        InformationError = null;
                                        error = EnableValidationAndGetError();
                                        PropertyChanged(this, new PropertyChangedEventArgs("Description_en"));
                                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                                        if (error != null)
                                        {
                                            return;
                                        }
                                    }
                                    else
                                        InformationError = " ";

                                    if (UpdatedArticle.WarehouseArticle == null)
                                    {
                                        UpdatedArticle.WarehouseArticle = new Article();
                                    }

                                    UpdatedArticle.IdPCMArticleCategory = SelectedCategory.IdPCMArticleCategory;
                                    UpdatedArticle.IdPCMStatus = SelectedStatus.IdLookupValue;
                                    UpdatedArticle.IdArticle = IdArticle;
                                    UpdatedArticle.WarehouseArticle.IdArticle = (int)IdArticle;
                                    UpdatedArticle.Reference = Reference;

                                    if (IsCheckedCopyName == true)
                                    {
                                        IsFromInformation = true;
                                        UpdatedArticle.WarehouseArticle.Description = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_es = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_fr = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_pt = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ro = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ru = Description == null ? "" : Description.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_zh = Description == null ? "" : Description.Trim();
                                    }
                                    else
                                    {
                                        IsFromInformation = true;
                                        UpdatedArticle.WarehouseArticle.Description = Description_en == null ? "" : Description_en.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_es = Description_es == null ? "" : Description_es.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                                        UpdatedArticle.WarehouseArticle.Description_zh = Description_zh == null ? "" : Description_zh.Trim();
                                    }

                                    UpdatedArticle = PcmArticleCatalogueDescriptionManager.UpdateArticleNormalOrRtfPCMDescriptionFromLocalData(UpdatedArticle);

                                    if (IsRtf)
                                    {
                                        if (IsCheckedCopyCatelogueDescription == true)
                                        {
                                            IsFromInformation = false;
                                        }
                                        else
                                        {
                                            IsFromInformation = false;
                                        }
                                    }
                                    if (IsFromInformation)
                                    {
                                        if (IsCheckedCopyCatelogueDescription == true)
                                        {
                                            IsFromInformation = false;
                                        }
                                        else
                                        {
                                            IsFromInformation = false;
                                        }
                                    }


                                    UpdatedArticle.IdECOSVisibility = SelectedECOSVisibility.IdLookupValue;
                                    UpdatedArticle.ECOSVisibilityValue = SelectedECOSVisibility.Value;

                                    UpdatedArticle.IsImageShareWithCustomer = IsImageShareWithCustomer;

                                    UpdatedArticle.PurchaseQtyMin = PQuantityMin;
                                    UpdatedArticle.PurchaseQtyMax = PQuantityMax;

                                    UpdatedArticle.ECOSVisibilityHTMLColor = SelectedECOSVisibility.HtmlColor;

                                    UpdatedArticle.ArticleCompatibilityList = new List<ArticleCompatibility>();

                                    // Delete Compatibility
                                    foreach (ArticleCompatibility item in ClonedArticle.ArticleCompatibilityList)
                                    {
                                        if (item.IdTypeCompatibility == 254 && MandatoryList != null && !MandatoryList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                        if (item.IdTypeCompatibility == 255 && SuggestedList != null && !SuggestedList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                        if (item.IdTypeCompatibility == 256 && IncompatibleList != null && !IncompatibleList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }


                                    //Added Compatibility
                                    foreach (ArticleCompatibility item in MandatoryList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }
                                    foreach (ArticleCompatibility item in SuggestedList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }
                                    foreach (ArticleCompatibility item in IncompatibleList)
                                    {
                                        if (!ClonedArticle.ArticleCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                                        {
                                            IsArticleSync = true;
                                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            articleCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                                        }
                                    }

                                    //Updated Compatibility
                                    foreach (ArticleCompatibility originalCompatibility in ClonedArticle.ArticleCompatibilityList)
                                    {
                                        if (originalCompatibility.IdTypeCompatibility == 254 && MandatoryList != null && MandatoryList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility MandatoryUpdated = MandatoryList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                            if ((MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements) || (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements) || (MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)MandatoryUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if (MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMinUpdate").ToString(), originalCompatibility.MinimumElements, MandatoryUpdated.MinimumElements) });
                                                }
                                                if (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMaxUpdate").ToString(), originalCompatibility.MaximumElements, MandatoryUpdated.MaximumElements) });
                                                }
                                                if ((MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(MandatoryUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 254 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }

                                        if (originalCompatibility.IdTypeCompatibility == 255 && SuggestedList != null && SuggestedList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility SuggestedUpdated = SuggestedList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                                            if (SuggestedUpdated.Remarks != originalCompatibility.Remarks)
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)SuggestedUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if ((SuggestedUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(SuggestedUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 254 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }

                                        if (originalCompatibility.IdTypeCompatibility == 256 && IncompatibleList != null && IncompatibleList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                                        {
                                            ArticleCompatibility IncompatibleUpdated = IncompatibleList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);

                                            if ((IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType) || (IncompatibleUpdated.Quantity != originalCompatibility.Quantity) || (IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                            {
                                                ArticleCompatibility articleCompatibility = (ArticleCompatibility)IncompatibleUpdated.Clone();
                                                articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                                                articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                                                if (IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType)
                                                {
                                                    IsArticleSync = true;
                                                    if (originalCompatibility.RelationshipType == null)
                                                        originalCompatibility.RelationshipType = new LookupValue();

                                                    string relationShip = RelationShipList.FirstOrDefault(a => a.IdLookupValue == articleCompatibility.IdRelationshipType).Value;

                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelationshipUpdate").ToString(), originalCompatibility.RelationshipType.Value, relationShip) });
                                                }

                                                if ((IncompatibleUpdated.Quantity != originalCompatibility.Quantity))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Quantity.ToString()))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Quantity.ToString()))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), "None", IncompatibleUpdated.Quantity) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, IncompatibleUpdated.Quantity) });
                                                    }
                                                }


                                                if ((IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Remarks))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 254 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 255 ? "Suggested" : "Incompatible") });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (ImagesList != null)
                                    {
                                        ulong pos = 1;
                                        foreach (PCMArticleImage img in ImagesList)
                                        {
                                            img.Position = pos;
                                            pos++;
                                        }
                                    }


                                    //Images
                                    UpdatedArticle.PCMArticleImageList = new List<PCMArticleImage>();

                                    foreach (PCMArticleImage item in ClonedArticle.PCMArticleImageList)
                                    {

                                        if (!ImagesList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                        {
                                            IsArticleSync = true;
                                            PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                            pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                            if (pCMArticleImage.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                                        }
                                    }



                                    //Added Article Image
                                    foreach (PCMArticleImage item in ImagesList)
                                    {
                                        if (!(item.IsWarehouseImage == 1))
                                        {
                                            if (!ClonedArticle.PCMArticleImageList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                                            {
                                                IsArticleSync = true;
                                                PCMArticleImage pCMArticleImage = (PCMArticleImage)item.Clone();
                                                pCMArticleImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.PCMArticleImageList.Add(pCMArticleImage);
                                                if (pCMArticleImage.Position == 1)
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                                else
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                                            }
                                        }
                                    }
                                    //Updated Article Image

                                    foreach (PCMArticleImage originalArticleTypeImage in ClonedArticle.PCMArticleImageList)
                                    {
                                        if (ImagesList != null && ImagesList.Any(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage))
                                        {
                                            PCMArticleImage articleImageUpdated = ImagesList.FirstOrDefault(x => x.IdPCMArticleImage == originalArticleTypeImage.IdPCMArticleImage);
                                            if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName) ||
                                                (articleImageUpdated.SavedFileName != originalArticleTypeImage.SavedFileName) ||
                                                (articleImageUpdated.Description != originalArticleTypeImage.Description) ||
                                                (articleImageUpdated.Position != originalArticleTypeImage.Position) ||
                                                (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer))
                                            {
                                                PCMArticleImage productTypeImage = (PCMArticleImage)articleImageUpdated.Clone();
                                                productTypeImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.PCMArticleImageList.Add(productTypeImage);
                                                if (articleImageUpdated.PCMArticleImageInBytes != originalArticleTypeImage.PCMArticleImageInBytes)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesUpdate").ToString(), originalArticleTypeImage.SavedFileName, articleImageUpdated.SavedFileName) });
                                                }
                                                if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleImageUpdated.OriginalFileName))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticleTypeImage.OriginalFileName))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), "None", articleImageUpdated.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalArticleTypeImage.OriginalFileName, articleImageUpdated.OriginalFileName) });
                                                    }
                                                }
                                                if (articleImageUpdated.Description != originalArticleTypeImage.Description)
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleImageUpdated.Description))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticleTypeImage.Description))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), "None", articleImageUpdated.Description) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalArticleTypeImage.Description, articleImageUpdated.Description) });
                                                    }
                                                }

                                                if (articleImageUpdated.IsImageShareWithCustomer != originalArticleTypeImage.IsImageShareWithCustomer)
                                                {
                                                    IsArticleSync = true;
                                                    if (articleImageUpdated.IsImageShareWithCustomer == 1 && originalArticleTypeImage.IsImageShareWithCustomer == 0)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "OFF", "ON") });
                                                    else if (articleImageUpdated.IsImageShareWithCustomer == 0 && originalArticleTypeImage.IsImageShareWithCustomer == 1)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogImageShareWithCustomerUpdate").ToString(), articleImageUpdated.OriginalFileName, "ON", "OFF") });
                                                }

                                                if (articleImageUpdated.Position != originalArticleTypeImage.Position)
                                                {
                                                    IsArticleSync = true;
                                                    if (originalArticleTypeImage.IsWarehouseImage != 1)
                                                    {
                                                        if (originalArticleTypeImage.Position == 1)
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalArticleTypeImage.Position, articleImageUpdated.Position, originalArticleTypeImage.OriginalFileName) });
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    //if ((ImagesList.Any(a => a.IsWarehouseImage == 1)))
                                    {
                                        PCMArticleImage tempDefaultImage = ClonedArticle.PCMArticleImageList.FirstOrDefault(x => x.Position == 1);
                                        PCMArticleImage tempDefaultImage_updated = ImagesList.FirstOrDefault(x => x.Position == 1);
                                        PCMArticleImage WarehouseImg = ImagesList.FirstOrDefault(x => x.IsWarehouseImage == 1);

                                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdPCMArticleImage != tempDefaultImage_updated.IdPCMArticleImage)
                                        {
                                            IsArticleSync = true;
                                            if (tempDefaultImage_updated.Position == 1)
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                            else if (tempDefaultImage_updated.Position == 1 && tempDefaultImage_updated.IsWarehouseImage == 1)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), tempDefaultImage, tempDefaultImage_updated.OriginalFileName) });
                                            }
                                        }


                                        if (OldWarehouseImage != null && WarehouseImg != null)
                                        {
                                            IsArticleSync = true;
                                            if (oldWarehouseposition != WarehouseImg.Position)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WareHouseImagePostionUpdate").ToString(), oldWarehouseposition, WarehouseImg.Position, WarehouseImg.OriginalFileName) });
                                            }
                                            if (oldWarehouseposition != WarehouseImg.Position && oldWarehouseposition == 1)
                                            {
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), WarehouseImg.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                                            }

                                        }
                                    }

                                    if (ClonedArticle.IsImageShareWithCustomer != IsImageShareWithCustomer)
                                    {
                                        IsArticleSync = true;
                                        if (ClonedArticle.IsImageShareWithCustomer == 1 && IsImageShareWithCustomer == 0)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "ON", "OFF") });
                                        else if (ClonedArticle.IsImageShareWithCustomer == 0 && IsImageShareWithCustomer == 1)
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ChangeLogWareHoImageShareWithCustomerUpdate").ToString(), ClonedArticle.Reference, "OFF", "ON") });
                                    }

                                    UpdatedArticle.PCMArticleImageList.ForEach(x => x.AttachmentImage = null);

                                    /// Attchment Files
                                    UpdatedArticle.PCMArticleAttachmentList = new List<ArticleDocument>();

                                    // Delete Article Attachment file
                                    foreach (ArticleDocument item in ClonedArticle.PCMArticleAttachmentList)
                                    {

                                        if (ArticleFilesList != null && !ArticleFilesList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                        {
                                            IsArticleSync = true;
                                            ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                            articleDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                            UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                                        }
                                    }

                                    //Added Article Attachment file
                                    if (ArticleFilesList != null)
                                    {
                                        foreach (ArticleDocument item in ArticleFilesList)
                                        {
                                            if (!ClonedArticle.PCMArticleAttachmentList.Any(x => x.IdArticleDoc == item.IdArticleDoc))
                                            {
                                                IsArticleSync = true;
                                                ArticleDocument articleDoc = (ArticleDocument)item.Clone();
                                                articleDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.PCMArticleAttachmentList.Add(articleDoc);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                                            }
                                        }
                                    }

                                    //Updated Article Attachment file
                                    foreach (ArticleDocument originalArticle in ClonedArticle.PCMArticleAttachmentList)
                                    {
                                        if (ArticleFilesList != null && ArticleFilesList.Any(x => x.IdArticleDoc == originalArticle.IdArticleDoc))
                                        {
                                            ArticleDocument articleAttachedDocUpdated = ArticleFilesList.FirstOrDefault(x => x.IdArticleDoc == originalArticle.IdArticleDoc);
                                            if (originalArticle.ArticleDocumentType == null)
                                                originalArticle.ArticleDocumentType = new ArticleDocumentType();

                                            if (articleAttachedDocUpdated.ArticleDocumentType == null)
                                                articleAttachedDocUpdated.ArticleDocumentType = new ArticleDocumentType();

                                            if ((articleAttachedDocUpdated.SavedFileName != originalArticle.SavedFileName) || (articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName) || (articleAttachedDocUpdated.Description != originalArticle.Description) || (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer) || (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType) || (articleAttachedDocUpdated.PCMArticleFileInBytes != originalArticle.PCMArticleFileInBytes))
                                            {
                                                ArticleDocument articleAttachedDoc = (ArticleDocument)articleAttachedDocUpdated.Clone();
                                                articleAttachedDoc.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                articleAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.PCMArticleAttachmentList.Add(articleAttachedDoc);

                                                if ((articleAttachedDocUpdated.OriginalFileName != originalArticle.OriginalFileName))
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleAttachedDocUpdated.OriginalFileName))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticle.OriginalFileName))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, "None", articleAttachedDocUpdated.OriginalFileName) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesNameUpdate").ToString(), ClonedArticle.Reference, originalArticle.OriginalFileName, articleAttachedDocUpdated.OriginalFileName) });
                                                    }
                                                }
                                                if (articleAttachedDocUpdated.Description != originalArticle.Description)
                                                {
                                                    IsArticleSync = true;
                                                    if (string.IsNullOrEmpty(articleAttachedDocUpdated.Description))
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), originalArticle.OriginalFileName, originalArticle.Description, "None") });
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(originalArticle.Description))
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, "None", articleAttachedDoc.Description) });
                                                        else
                                                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.Description, articleAttachedDoc.Description) });
                                                    }
                                                }
                                                if (articleAttachedDocUpdated.ArticleDocumentType.DocumentType != originalArticle.ArticleDocumentType.DocumentType)
                                                {
                                                    IsArticleSync = true;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesDocumentTypeUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, originalArticle.ArticleDocumentType.DocumentType, articleAttachedDocUpdated.ArticleDocumentType.DocumentType) });
                                                }
                                                if (articleAttachedDocUpdated.IsShareWithCustomer != originalArticle.IsShareWithCustomer)
                                                {
                                                    IsArticleSync = true;
                                                    if (articleAttachedDocUpdated.IsShareWithCustomer == 0)
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString())) });
                                                    else
                                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticleChangeLogFilesIsSharedWithCustomerUpdate").ToString(), articleAttachedDocUpdated.OriginalFileName, string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerNO").ToString()), string.Format(System.Windows.Application.Current.FindResource("IsSharedWithCustomerYES").ToString())) });
                                                }
                                            }
                                        }
                                    }

                                    if (SelectedECOSVisibility.IdLookupValue == 323)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 1;
                                        UpdatedArticle.IsSparePartOnly = 0;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 324)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 1;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 325)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 0;

                                        if (UpdatedArticle.PurchaseQtyMin == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMin = 1;
                                            IsEnabledMinMax = true;
                                        }

                                        if (UpdatedArticle.PurchaseQtyMax == 0)
                                        {
                                            UpdatedArticle.PurchaseQtyMax = 1;
                                            IsEnabledMinMax = true;
                                        }
                                    }
                                    else if (SelectedECOSVisibility.IdLookupValue == 326)
                                    {
                                        UpdatedArticle.IsShareWithCustomer = 0;
                                        UpdatedArticle.IsSparePartOnly = 0;
                                        UpdatedArticle.PurchaseQtyMin = 0;
                                        UpdatedArticle.PurchaseQtyMax = 0;
                                        IsEnabledMinMax = true;
                                    }

                                    if (UpdatedArticle.PCMDescription.Contains(" ") || UpdatedArticle.PCMDescription_es.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_fr.Contains(" ") || UpdatedArticle.PCMDescription_pt.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_ro.Contains(" ") || UpdatedArticle.PCMDescription_ru.Contains(" ") ||
                                        UpdatedArticle.PCMDescription_zh.Contains(" "))
                                    {
                                        UpdatedArticle.PCMDescription = UpdatedArticle.PCMDescription.Trim(' ');
                                        UpdatedArticle.PCMDescription_es = UpdatedArticle.PCMDescription_es.Trim(' ');
                                        UpdatedArticle.PCMDescription_fr = UpdatedArticle.PCMDescription_fr.Trim(' ');
                                        UpdatedArticle.PCMDescription_pt = UpdatedArticle.PCMDescription_pt.Trim(' ');
                                        UpdatedArticle.PCMDescription_ro = UpdatedArticle.PCMDescription_ro.Trim(' ');
                                        UpdatedArticle.PCMDescription_ru = UpdatedArticle.PCMDescription_ru.Trim(' ');
                                        UpdatedArticle.PCMDescription_zh = UpdatedArticle.PCMDescription_zh.Trim(' ');
                                    }


                                    //[adhatkar][GEOS2-3196]
                                    /// PLM Article Prices
                                    UpdatedArticle.ModifiedPLMArticleList = new List<PLMArticlePrice>();
                                    UpdatedArticle.BasePriceLogEntryList = new List<BasePriceLogEntry>();
                                    UpdatedArticle.CustomerPriceLogEntryList = new List<CustomerPriceLogEntry>();

                                    // Delete PLM Article Prices
                                    if (NotIncludedPLMArticlePriceList != null)
                                    {
                                        foreach (PLMArticlePrice item in NotIncludedPLMArticlePriceList)
                                        {
                                            if (!ClonedArticle.NotIncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDeleteInPCM").ToString(), item.Code, item.Type) });
                                                if (item.Type == "BPL")
                                                    UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                                else
                                                    UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogDelete").ToString(), Reference) });
                                            }
                                        }
                                    }

                                    //Added PLM Article Prices
                                    if (IncludedPLMArticlePriceList != null)
                                    {
                                        foreach (PLMArticlePrice item in IncludedPLMArticlePriceList)
                                        {
                                            if (!ClonedArticle.IncludedPLMArticleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)item.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);
                                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAddInPCM").ToString(), item.Code, item.Type) });
                                                if (item.Type == "BPL")
                                                    UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });
                                                else
                                                    UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogAdd").ToString(), Reference) });

                                            }
                                        }
                                    }

                                    //Updated PLM Article Prices
                                    foreach (PLMArticlePrice originalArticle in ClonedArticle.IncludedPLMArticleList)
                                    {
                                        if (IncludedPLMArticlePriceList != null && IncludedPLMArticlePriceList.Any(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type))
                                        {
                                            PLMArticlePrice PLMArticlePriceUpdated = IncludedPLMArticlePriceList.FirstOrDefault(x => x.IdCustomerOrBasePriceList == originalArticle.IdCustomerOrBasePriceList && x.Type == originalArticle.Type);
                                            if ((PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue) || (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule))
                                            {
                                                PLMArticlePrice PLMArticlePrice = (PLMArticlePrice)PLMArticlePriceUpdated.Clone();
                                                PLMArticlePrice.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                UpdatedArticle.ModifiedPLMArticleList.Add(PLMArticlePrice);


                                                if (PLMArticlePriceUpdated.IdRule != originalArticle.IdRule)
                                                {
                                                    string newRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMArticlePriceUpdated.IdRule).Value;
                                                    string oldRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == originalArticle.IdRule).Value;
                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogicInPCM").ToString(), oldRuleLogic, newRuleLogic, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                                    if (PLMArticlePriceUpdated.Type == "BPL")
                                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                                    else
                                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, Reference) });
                                                }

                                                if (PLMArticlePriceUpdated.RuleValue != originalArticle.RuleValue)
                                                {
                                                    string oldValue = "";
                                                    string newValue = "";
                                                    if (PLMArticlePriceUpdated.RuleValue == null)
                                                        newValue = "None";
                                                    else
                                                        newValue = PLMArticlePriceUpdated.RuleValue.Value.ToString("0." + new string('#', 339));

                                                    if (originalArticle.RuleValue == null)
                                                        oldValue = "None";
                                                    else
                                                        oldValue = originalArticle.RuleValue.Value.ToString("0." + new string('#', 339));

                                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValueInPCM").ToString(), oldValue, newValue, PLMArticlePriceUpdated.Type, PLMArticlePriceUpdated.Code) });
                                                    if (PLMArticlePriceUpdated.Type == "BPL")
                                                        UpdatedArticle.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                                    else
                                                        UpdatedArticle.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMArticlePriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceChangeLogArticleGridRuleValue").ToString(), oldValue, newValue, Reference) });
                                                }
                                            }
                                        }
                                    }


                                    //Customers add and Delete
                                    if (UpdatedArticle.ArticleCustomerList == null)
                                        UpdatedArticle.ArticleCustomerList = new List<ArticleCustomer>();
                                    //ObservableCollection<ArticleCustomer> tempCustomersList = ArticleCustomerList;
                                    //ObservableCollection<ArticleCustomer> clonedCustomerListByArticle = new ObservableCollection<ArticleCustomer>(PCMService.GetCustomersByIdArticleCustomerReferences(IdArticle));
                                    // Delete Customer
                                    if (ArticleCustomerList != null)
                                    {

                                        foreach (ArticleCustomer item in ClonedArticle.ArticleCustomerList)
                                        {
                                            if (!ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == item.IdArticleCustomerReferences))
                                            {
                                                IsArticleSync = true;
                                                ArticleCustomer CustomerReference = (ArticleCustomer)item.Clone();
                                                CustomerReference.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                UpdatedArticle.ArticleCustomerList.Add(CustomerReference);
                                                ChangeLogList.Add(new PCMArticleLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomerDelete").ToString(),
                                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                                });
                                            }
                                        }
                                    }

                                    //Added Customer
                                    if (ArticleCustomerList != null)
                                    {
                                        foreach (ArticleCustomer item in ArticleCustomerList)
                                        {
                                            if (!ClonedArticle.ArticleCustomerList.Any(x => x.IdArticleList == item.IdArticleList))
                                            {
                                                IsArticleSync = true;
                                                ArticleCustomer articleCustomer = (ArticleCustomer)item.Clone();
                                                articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                                ChangeLogList.Add(new PCMArticleLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersAdd").ToString(),
                                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name, item.ReferenceCustomer.Trim())
                                                });

                                            }
                                        }
                                    }


                                    //Updated Customer
                                    if (ClonedArticle.ArticleCustomerList != null)
                                    {
                                        foreach (ArticleCustomer originalCustomer in ClonedArticle.ArticleCustomerList)
                                        {
                                            if (ArticleCustomerList != null && ArticleCustomerList.Any(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences))
                                            {
                                                ArticleCustomer ArticleCustomerListUpdated = ArticleCustomerList.FirstOrDefault(x => x.IdArticleCustomerReferences == originalCustomer.IdArticleCustomerReferences);
                                                if ((ArticleCustomerListUpdated.IdGroup != originalCustomer.IdGroup) || (ArticleCustomerListUpdated.IdRegion != originalCustomer.IdRegion) || (ArticleCustomerListUpdated.IdCountry != originalCustomer.IdCountry) || (ArticleCustomerListUpdated.IdPlant != originalCustomer.IdPlant || (ArticleCustomerListUpdated.ReferenceCustomer != originalCustomer.ReferenceCustomer.Trim())))
                                                {
                                                    IsArticleSync = true;
                                                    ArticleCustomer articleCustomer = (ArticleCustomer)ArticleCustomerListUpdated.Clone();
                                                    articleCustomer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                    articleCustomer.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                    UpdatedArticle.ArticleCustomerList.Add(articleCustomer);
                                                    ChangeLogList.Add(new PCMArticleLogEntry()
                                                    {
                                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleCustomerChangeLogCustomersUpdate").ToString(),
                                                        originalCustomer.GroupName, originalCustomer.RegionName,
                                                        originalCustomer.Country.Name, originalCustomer.Plant.Name, articleCustomer.ReferenceCustomer,
                                                        articleCustomer.GroupName, articleCustomer.RegionName,
                                                        articleCustomer.Country.Name, articleCustomer.Plant.Name, articleCustomer.ReferenceCustomer)
                                                    });
                                                }
                                            }
                                        }
                                    }

                                    UpdatedArticle.ModifiedPLMArticleList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);

                                    AddArticleLogDetails();
                                    UpdatedArticle.PCMArticleLogEntiryList = ChangeLogList.ToList();
                                    if (UpdatedArticle.WarehouseArticleLogEntiryList == null)
                                        UpdatedArticle.WarehouseArticleLogEntiryList = new List<LogEntriesByArticle>();
                                    UpdatedArticle.WarehouseArticleLogEntiryList = WMSArticleChangeLogEntry.ToList();
                                    //[003] Added
                                    if (WMSArticleChangeLogEntry.Count == 0)
                                    {
                                        UpdatedArticle.WarehouseArticle = null;
                                    }
                                    UpdatedArticle.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    UpdatedArticle.IdCreator = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                    //[003] Changes service method
                                    IsSave = PCMService.IsUpdatePCMArticle_V2350(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                                    // IsSave = PCMService.IsUpdatePCMArticle_V2260(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                    IsAdded = true;
                                    IsAcceptButtonEnabled = false;
                                    //[002] Added code for synchronization
                                    if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                                    {
                                        GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59,61");
                                        if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                                        {
                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 57))
                                            {
                                                if (!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                                                {
                                                    if (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue)))
                                                    {
                                                    #region Synchronization code
                                                    CancelSync:;
                                                        var ownerInfo = (groupBox as FrameworkElement);
                                                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                                        if (MessageBoxResult == MessageBoxResult.Yes)
                                                        {
                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is running";

                                                            if (!string.IsNullOrEmpty(UpdatedArticle.Reference.Trim()))
                                                            {
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

                                                                    APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
                                                                    APIErrorDetail values = new APIErrorDetail();
                                                                    tokenService = new AuthTokenService();
                                                                    List<ErrorDetails> LstErrorDetail = new List<ErrorDetails>();
                                                                    bool IsArticleSynchronization = true;
                                                                    //GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("57,59");
                                                                    if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                                                    {
                                                                        string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                                                        if (tokeninformations.Count() >= 2)
                                                                        {
                                                                            if (UpdatedArticle.ModifiedPLMArticleList.Any(i => i.IdStatus == 223) && GeosAppSettingList.Any(i => i.IdAppSetting == 61))
                                                                            {
                                                                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                                //[rdixit][22.02.2023][GEOS2-4176]
                                                                                PCMArticleSynchronizationViewModel PCMArticleSynchronizationViewModel = new PCMArticleSynchronizationViewModel();
                                                                                PCMArticleSynchronizationView PCMArticleSynchronizationView = new PCMArticleSynchronizationView();
                                                                                EventHandler handle = delegate { PCMArticleSynchronizationView.Close(); };
                                                                                PCMArticleSynchronizationViewModel.RequestClose += handle;
                                                                                PCMArticleSynchronizationView.DataContext = PCMArticleSynchronizationViewModel;
                                                                                PCMArticleSynchronizationViewModel.Init(IncludedPLMArticlePriceList, UpdatedArticle);
                                                                                PCMArticleSynchronizationView.ShowDialogWindow();
                                                                                if (PCMArticleSynchronizationViewModel.IsSave)
                                                                                {
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

                                                                                    if (!string.IsNullOrEmpty((GeosAppSettingList[2].DefaultValue)))
                                                                                    {
                                                                                        string IdsBPL = "";
                                                                                        if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "BPL" && pd.IdStatus == 223))
                                                                                        {
                                                                                            IdsBPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                                        }
                                                                                        string IdsCPL = "";
                                                                                        if (UpdatedArticle.ModifiedPLMArticleList.Any(pd => pd.Type == "CPL" && pd.IdStatus == 223))
                                                                                        {
                                                                                            IdsCPL = string.Join(",", UpdatedArticle.ModifiedPLMArticleList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                                                        }
                                                                                        //[rdixit][22.02.2023][GEOS2-4176]
                                                                                        if (PCMArticleSynchronizationViewModel.BPLPlantCurrencyList != null)
                                                                                        {
                                                                                            List<BPLPlantCurrencyDetail> BPLPlantCurrencyDetailList = PCMArticleSynchronizationViewModel.BPLPlantCurrencyList.ToList();//PLMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Convert.ToInt32(UpdatedArticle.IdArticle), IdsBPL, IdsCPL, "Article");

                                                                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 61) && BPLPlantCurrencyDetailList != null)
                                                                                            {

                                                                                                foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                                                                                {
                                                                                                    if (IsArticleSync == true && IsArticleSynchronization == true)
                                                                                                    {
                                                                                                        IsArticleSynchronization = true;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        IsArticleSynchronization = false;
                                                                                                    }
                                                                                                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                    List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization_V2310(GeosAppSettingList, itemBPLPlantCurrency, UpdatedArticle, IsArticleSynchronization);
                                                                                                    IsArticleSynchronization = false;
                                                                                                    #region
                                                                                                    foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                                    {
                                                                                                        if (item != null && item.Error == string.Empty)
                                                                                                        {
                                                                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is Done for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (item != null && item.Error != null)
                                                                                                            {
                                                                                                                GeosApplication.Instance.SplashScreenMessage = "Synchronization is failed for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                                                                ErrorDetails errorDetails = new ErrorDetails();
                                                                                                                errorDetails.CompanyName = itemBPLPlantCurrency.CompanyName;
                                                                                                                errorDetails.CurrencyName = itemBPLPlantCurrency.CurrencyName;
                                                                                                                errorDetails.Error = item.Error;
                                                                                                                LstErrorDetail.Add(errorDetails);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    #endregion
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationCancelled").ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                                                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                                                    goto CancelSync;
                                                                                }
                                                                            }
                                                                            else
                                                                            {

                                                                                List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMArticlesSynchronization(GeosAppSettingList, null, UpdatedArticle);
                                                                                foreach (ErrorDetails item in TempLstErrorDetail)
                                                                                {
                                                                                    if (item != null && item.Error == string.Empty)
                                                                                    {

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (item != null && item.Error != null)
                                                                                        {
                                                                                            ErrorDetails errorDetails = new ErrorDetails();
                                                                                            errorDetails.CompanyName = "None";
                                                                                            errorDetails.CurrencyName = "None";
                                                                                            errorDetails.Error = item.Error;
                                                                                            LstErrorDetail.Add(errorDetails);
                                                                                        }
                                                                                    }
                                                                                }


                                                                            }
                                                                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                            if (LstErrorDetail != null && LstErrorDetail.Count > 0)
                                                                            {
                                                                                String FinalMessage = null;
                                                                                string msg = string.Empty;
                                                                                foreach (ErrorDetails item in LstErrorDetail)
                                                                                {

                                                                                    if (string.IsNullOrEmpty(FinalMessage))
                                                                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error), Window.GetWindow(ownerInfo));
                                                                                    else
                                                                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error);

                                                                                }
                                                                                if (!string.IsNullOrEmpty(FinalMessage))
                                                                                {
                                                                                    CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                                                            }
                                                                        }

                                                                    }

                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                                                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                                }

                                                            }
                                                            //CancelSync:;                                          
                                                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                        }
                                                        #endregion Synchronization
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    RequestClose(null, null);
                                    IsAdded = false;
                                    IsAcceptButtonEnabled = true;
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                                }

                            }
                           

                        }
                    }

                }
            

                
            }
            catch (FaultException<ServiceException> ex)
            {
                IsAdded = false;
                IsAcceptButtonEnabled = true;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsAdded = false;
                IsAcceptButtonEnabled = true;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsAdded = false;
                IsAcceptButtonEnabled = true;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditSaveModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        //[Rahul.Gadhave][GEOS2-8318][Date:18-07-2025]
        private Articles GetDeepCopyOfUpdatedArticle(Articles source)
        {
            var copiedArticle = new Articles();


            copiedArticle.IdPCMArticleCategory = source.IdPCMArticleCategory;
            copiedArticle.IdModifier = source.IdModifier;
            copiedArticle.IdCreator = source.IdCreator;
            copiedArticle.IdPCMStatus = source.IdPCMStatus;
            copiedArticle.IdECOSVisibility = source.IdECOSVisibility;
            copiedArticle.ECOSVisibilityValue = source.ECOSVisibilityValue;
            copiedArticle.ECOSVisibilityHTMLColor = source.ECOSVisibilityHTMLColor;
            copiedArticle.IsImageShareWithCustomer = source.IsImageShareWithCustomer;
            copiedArticle.IsRtfText = source.IsRtfText;
            copiedArticle.PCMDescription = source.PCMDescription;
            copiedArticle.PCMDescription_es = source.PCMDescription_es;
            copiedArticle.PCMDescription_fr = source.PCMDescription_fr;
            copiedArticle.PCMDescription_pt = source.PCMDescription_pt;
            copiedArticle.PCMDescription_ro = source.PCMDescription_ro;
            copiedArticle.PCMDescription_ru = source.PCMDescription_ru;
            copiedArticle.PCMDescription_zh = source.PCMDescription_zh;
            copiedArticle.PurchaseQtyMin = source.PurchaseQtyMin;
            copiedArticle.PurchaseQtyMax = source.PurchaseQtyMax;
            copiedArticle.IsShareWithCustomer = source.IsShareWithCustomer;
            copiedArticle.IsSparePartOnly = source.IsSparePartOnly;
            copiedArticle.ArticleCompatibilityList = source.ArticleCompatibilityList?.Select(a => (ArticleCompatibility)a.Clone()).ToList();
            copiedArticle.PCMArticleLogEntiryList = source.PCMArticleLogEntiryList?.Select(a => (PCMArticleLogEntry)a.Clone()).ToList();
            copiedArticle.PCMArticleImageList = source.PCMArticleImageList?.Select(a => (PCMArticleImage)a.Clone()).ToList();
            //copiedArticle.PCMArticleAttachmentList = source.PCMArticleAttachmentList?.Select(a => (PCMArticleAttachment)a.Clone()).ToList();
            copiedArticle.WarehouseArticleLogEntiryList = source.WarehouseArticleLogEntiryList?.Select(a => (LogEntriesByArticle)a.Clone()).ToList();
            copiedArticle.ArticleCustomerList = source.ArticleCustomerList;
            copiedArticle.WarehouseArticle = source.WarehouseArticle != null ? (Article)source.WarehouseArticle.Clone() : null;
            copiedArticle.ModifiedPLMArticleList = source.ModifiedPLMArticleList;
            copiedArticle.BasePriceLogEntryList = source.BasePriceLogEntryList;
            copiedArticle.CustomerPriceLogEntryList = source.CustomerPriceLogEntryList;


            return copiedArticle;
        }



        private void SelectedeCOSChangedCommandAction(EditValueChangedEventArgs obj)
        {
            if (SelectedECOSVisibility.IdLookupValue == 326)
            {
                PQuantityMin = 0;
                PQuantityMax = 0;
                IsEnabledMinMax = false;
            }
            else
            {
                if (PQuantityMin == 0)
                {
                    PQuantityMin = 1;
                }

                if (PQuantityMax == 0)
                {
                    PQuantityMax = 1;
                }
                IsEnabledMinMax = true;
            }
			//Shubham[skadam] GEOS2-5024 Improvements in PCM module 26 12 2023
            if ((ClonedArticle.PurchaseQtyMin != PQuantityMin && PQuantityMin!=0)|| (ClonedArticle.PurchaseQtyMax != PQuantityMax && PQuantityMax != 0))
            {
                IsEnabledCancelButton = true;
            }
            else
            {
                IsEnabledCancelButton = false;
            }
        }

        private void CommandEditValueChangedAction(EditValueChangingEventArgs obj)
        {
            try
            {
                {
                    if (PQuantityMin > PQuantityMax || PQuantityMax < PQuantityMin)
                    {
                        pQuantityMinErrorMessage = System.Windows.Application.Current.FindResource("PurchaseQuantityMinLess").ToString();
                        pQuantityMaxErrorMessage = System.Windows.Application.Current.FindResource("PurchaseQuantityMaxLess").ToString();
                    }
                    else
                    {
                        pQuantityMinErrorMessage = string.Empty;
                        pQuantityMaxErrorMessage = string.Empty;
                        informationError = " ";
                    }

                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    PropertyChanged(this, new PropertyChangedEventArgs("PQuantityMin"));
                    PropertyChanged(this, new PropertyChangedEventArgs("PQuantityMax"));

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CommandEditValueChangedAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UserControl_Loaded(object obj)
        {
            EditPCMArticleViewInstance = (EditPCMArticleView)obj;
        }

        private void AddArticleLogDetails()
        {
            //Status
            if (ClonedArticle.IdPCMStatus != UpdatedArticle.IdPCMStatus)
            {
                IsArticleSync = true;
                LookupValue tempStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == UpdatedArticle.IdPCMStatus);
                if (ClonedArticle.IdPCMStatus == null)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), ClonedArticle.PCMStatus = "Draft", tempStatus.Value) });
                if (tempStatus != null)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), ClonedArticle.PCMStatus, tempStatus.Value) });
            }

            if (ClonedArticle.IdPCMArticleCategory != UpdatedArticle.IdPCMArticleCategory)
            {
                IsArticleSync = true;
                PCMArticleCategory tempPCMArticleCategory = ArticleMenuList.FirstOrDefault(x => x.IdPCMArticleCategory == UpdatedArticle.IdPCMArticleCategory);
                if (tempPCMArticleCategory != null)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleChangeLogCategory").ToString(), ClonedArticle.PcmArticleCategory.Name, tempPCMArticleCategory.Name) });
            }

            if (ClonedArticle.PurchaseQtyMin != UpdatedArticle.PurchaseQtyMin)
            {
                IsArticleSync = true;
                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMin").ToString(), ClonedArticle.PurchaseQtyMin, UpdatedArticle.PurchaseQtyMin, ClonedArticle.Reference) });
            }

            if (ClonedArticle.PurchaseQtyMax != UpdatedArticle.PurchaseQtyMax)
            {
                IsArticleSync = true;
                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMax").ToString(), ClonedArticle.PurchaseQtyMax, UpdatedArticle.PurchaseQtyMax, ClonedArticle.Reference) });
            }

            if (ClonedArticle.IdECOSVisibility != UpdatedArticle.IdECOSVisibility)
            {
                IsArticleSync = true;
                LookupValue tempECOS = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == UpdatedArticle.IdECOSVisibility);
                if (ClonedArticle.IdECOSVisibility == 0)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityChangeLogStatus").ToString(), ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == ClonedArticle.IdECOSVisibility).Value = "Available", tempECOS.Value, ClonedArticle.Reference) });//ECOSVisibilityList.FirstOrDefault(a=>a.IdLookupValue == TempArticleList.IdECOSVisibility).Value
                if (tempECOS != null)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ECOSVisibilityChangeLogStatus").ToString(), ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == ClonedArticle.IdECOSVisibility).Value, tempECOS.Value, ClonedArticle.Reference) });
            }

            if (ClonedArticle.Description != UpdatedArticle.WarehouseArticle.Description)
            {
                IsArticleSync = true;
                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdate").ToString(), ClonedArticle.Description, UpdatedArticle.WarehouseArticle.Description, ClonedArticle.Reference) });
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdate").ToString(), ClonedArticle.Description, UpdatedArticle.WarehouseArticle.Description, UpdatedArticle.Reference) });
            }

            if (ClonedArticle.Description_es != UpdatedArticle.WarehouseArticle.Description_es)
            {
                IsArticleSync = true;
                if (string.IsNullOrEmpty(UpdatedArticle.WarehouseArticle.Description_es))
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateES").ToString(), ClonedArticle.Description_es, "None", ClonedArticle.Reference) });
                else
                {
                    if (string.IsNullOrEmpty(ClonedArticle.Description_es))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateES").ToString(), "None", UpdatedArticle.WarehouseArticle.Description_es, ClonedArticle.Reference) });
                    else
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateES").ToString(), ClonedArticle.Description_es, UpdatedArticle.WarehouseArticle.Description_es, ClonedArticle.Reference) });
                }
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateES").ToString(), ClonedArticle.Description_es, UpdatedArticle.WarehouseArticle.Description_es, UpdatedArticle.Reference) });
            }

            if (ClonedArticle.Description_fr != UpdatedArticle.WarehouseArticle.Description_fr)
            {
                IsArticleSync = true;
                if (string.IsNullOrEmpty(UpdatedArticle.WarehouseArticle.Description_fr))
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateFR").ToString(), ClonedArticle.Description_fr, "None", ClonedArticle.Reference) });
                else
                {
                    if (string.IsNullOrEmpty(ClonedArticle.Description_fr))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateFR").ToString(), "None", UpdatedArticle.WarehouseArticle.Description_fr, ClonedArticle.Reference) });
                    else
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateFR").ToString(), ClonedArticle.Description_fr, UpdatedArticle.WarehouseArticle.Description_fr, ClonedArticle.Reference) });
                }
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateFR").ToString(), ClonedArticle.Description_fr, UpdatedArticle.WarehouseArticle.Description_fr, UpdatedArticle.Reference) });
            }

            if (ClonedArticle.Description_pt != UpdatedArticle.WarehouseArticle.Description_pt)
            {
                IsArticleSync = true;
                if (string.IsNullOrEmpty(UpdatedArticle.WarehouseArticle.Description_pt))
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdatePT").ToString(), ClonedArticle.Description_pt, "None", ClonedArticle.Reference) });
                else
                {
                    if (string.IsNullOrEmpty(ClonedArticle.Description_pt))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdatePT").ToString(), "None", UpdatedArticle.WarehouseArticle.Description_pt, ClonedArticle.Reference) });
                    else
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdatePT").ToString(), ClonedArticle.Description_pt, UpdatedArticle.WarehouseArticle.Description_pt, ClonedArticle.Reference) });
                }
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdatePT").ToString(), ClonedArticle.Description_pt, UpdatedArticle.WarehouseArticle.Description_pt, UpdatedArticle.Reference) });
            }

            if (ClonedArticle.Description_ro != UpdatedArticle.WarehouseArticle.Description_ro)
            {
                IsArticleSync = true;
                if (string.IsNullOrEmpty(UpdatedArticle.WarehouseArticle.Description_ro))
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateRO").ToString(), ClonedArticle.Description_ro, "None", ClonedArticle.Reference) });
                else
                {
                    if (string.IsNullOrEmpty(ClonedArticle.Description_ro))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateRO").ToString(), "None", UpdatedArticle.WarehouseArticle.Description_ro, ClonedArticle.Reference) });
                    else
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateRO").ToString(), ClonedArticle.Description_ro, UpdatedArticle.WarehouseArticle.Description_ro, ClonedArticle.Reference) });
                }
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRO").ToString(), ClonedArticle.Description_ro, UpdatedArticle.WarehouseArticle.Description_ro, UpdatedArticle.Reference) });
            }

            if (ClonedArticle.Description_ru != UpdatedArticle.WarehouseArticle.Description_ru)
            {
                IsArticleSync = true;
                if (string.IsNullOrEmpty(UpdatedArticle.WarehouseArticle.Description_ru))
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateRU").ToString(), ClonedArticle.Description_ru, "None", ClonedArticle.Reference) });
                else
                {
                    if (string.IsNullOrEmpty(ClonedArticle.Description_ru))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateRU").ToString(), "None", UpdatedArticle.WarehouseArticle.Description_ru, ClonedArticle.Reference) });
                    else
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateRU").ToString(), ClonedArticle.Description_ru, UpdatedArticle.WarehouseArticle.Description_ru, ClonedArticle.Reference) });
                }
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRU").ToString(), ClonedArticle.Description_ru, UpdatedArticle.WarehouseArticle.Description_ru, UpdatedArticle.Reference) });
            }

            if (ClonedArticle.Description_zh != UpdatedArticle.WarehouseArticle.Description_zh)
            {
                IsArticleSync = true;
                if (string.IsNullOrEmpty(UpdatedArticle.WarehouseArticle.Description_zh))
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateZH").ToString(), ClonedArticle.Description_zh, "None", ClonedArticle.Reference) });
                else
                {
                    if (string.IsNullOrEmpty(ClonedArticle.Description_zh))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateZH").ToString(), "None", UpdatedArticle.WarehouseArticle.Description_zh, ClonedArticle.Reference) });
                    else
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleNameUpdateZH").ToString(), ClonedArticle.Description_zh, UpdatedArticle.WarehouseArticle.Description_zh, ClonedArticle.Reference) });
                }
                WMSArticleChangeLogEntry.Add(new LogEntriesByArticle() { IdArticle = (uint)ClonedArticle.IdArticle, IdUser = (int)GeosApplication.Instance.ActiveUser.IdUser, LogDateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateZH").ToString(), ClonedArticle.Description_zh, UpdatedArticle.WarehouseArticle.Description_zh, UpdatedArticle.Reference) });
            }

            //*******************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
            //PCMDescription IN PRODUCT CATELOGUE in all languages

            if (!IsFromInformation)
            {
                if (UpdatedArticle.PCMDescription == " ")
                {
                    UpdatedArticle.PCMDescription = UpdatedArticle.PCMDescription.Replace(" ", string.Empty);
                }

                if (UpdatedArticle.PCMDescription_es == " ")
                {
                    UpdatedArticle.PCMDescription_es = UpdatedArticle.PCMDescription_es.Replace(" ", string.Empty);
                }

                if (UpdatedArticle.PCMDescription_fr == " ")
                {
                    UpdatedArticle.PCMDescription_fr = UpdatedArticle.PCMDescription_fr.Replace(" ", string.Empty);
                }

                if (UpdatedArticle.PCMDescription_pt == " ")
                {
                    UpdatedArticle.PCMDescription_pt = UpdatedArticle.PCMDescription_pt.Replace(" ", string.Empty);
                }

                if (UpdatedArticle.PCMDescription_ro == " ")
                {
                    UpdatedArticle.PCMDescription_ro = UpdatedArticle.PCMDescription_ro.Replace(" ", string.Empty);
                }

                if (UpdatedArticle.PCMDescription_ru == " ")
                {
                    UpdatedArticle.PCMDescription_ru = UpdatedArticle.PCMDescription_ru.Replace(" ", string.Empty);
                }

                if (UpdatedArticle.PCMDescription_zh == " ")
                {
                    UpdatedArticle.PCMDescription_zh = UpdatedArticle.PCMDescription_zh.Replace(" ", string.Empty);
                }


                if (ClonedArticle.PCMDescription == null)
                {
                    ClonedArticle.PCMDescription = string.Empty;
                }
                if (ClonedArticle.PCMDescription.Trim() != UpdatedArticle.PCMDescription.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdate").ToString(), ClonedArticle.PCMDescription, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdate").ToString(), "None", UpdatedArticle.PCMDescription, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdate").ToString(), ClonedArticle.PCMDescription, UpdatedArticle.PCMDescription, ClonedArticle.Reference) });
                    }
                }

                if (ClonedArticle.PCMDescription_es == null)
                {
                    ClonedArticle.PCMDescription_es = string.Empty;
                }
                if (ClonedArticle.PCMDescription_es.Trim() != UpdatedArticle.PCMDescription_es.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription_es))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateES").ToString(), ClonedArticle.PCMDescription_es, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription_es))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateES").ToString(), "None", UpdatedArticle.PCMDescription_es, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateES").ToString(), ClonedArticle.PCMDescription_es, UpdatedArticle.PCMDescription_es, ClonedArticle.Reference) });
                    }
                }

                if (ClonedArticle.PCMDescription_fr == null)
                {
                    ClonedArticle.PCMDescription_fr = string.Empty;
                }
                if (ClonedArticle.PCMDescription_fr.Trim() != UpdatedArticle.PCMDescription_fr.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription_fr))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateFR").ToString(), ClonedArticle.PCMDescription_fr, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription_fr))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateFR").ToString(), "None", UpdatedArticle.PCMDescription_fr, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateFR").ToString(), ClonedArticle.PCMDescription_fr, UpdatedArticle.PCMDescription_fr, ClonedArticle.Reference) });
                    }
                }

                if (ClonedArticle.PCMDescription_pt == null)
                {
                    ClonedArticle.PCMDescription_pt = string.Empty;
                }
                if (ClonedArticle.PCMDescription_pt.Trim() != UpdatedArticle.PCMDescription_pt.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription_pt))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdatePT").ToString(), ClonedArticle.PCMDescription_pt, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription_pt))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdatePT").ToString(), "None", UpdatedArticle.PCMDescription_pt, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdatePT").ToString(), ClonedArticle.PCMDescription_pt, UpdatedArticle.PCMDescription_pt, ClonedArticle.Reference) });
                    }
                }

                if (ClonedArticle.PCMDescription_ro == null)
                {
                    ClonedArticle.PCMDescription_ro = string.Empty;
                }
                if (ClonedArticle.PCMDescription_ro.Trim() != UpdatedArticle.PCMDescription_ro.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription_ro))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRO").ToString(), ClonedArticle.PCMDescription_ro, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription_ro))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRO").ToString(), "None", UpdatedArticle.PCMDescription_ro, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRO").ToString(), ClonedArticle.PCMDescription_ro, UpdatedArticle.PCMDescription_ro, ClonedArticle.Reference) });
                    }
                }

                if (ClonedArticle.PCMDescription_ru == null)
                {
                    ClonedArticle.PCMDescription_ru = string.Empty;
                }
                if (ClonedArticle.PCMDescription_ru.Trim() != UpdatedArticle.PCMDescription_ru.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription_ru))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRU").ToString(), ClonedArticle.PCMDescription_ru, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription_ru))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRU").ToString(), "None", UpdatedArticle.PCMDescription_ru, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateRU").ToString(), ClonedArticle.PCMDescription_ru, UpdatedArticle.PCMDescription_ru, ClonedArticle.Reference) });
                    }
                }

                if (ClonedArticle.PCMDescription_zh == null)
                {
                    ClonedArticle.PCMDescription_zh = string.Empty;
                }
                if (ClonedArticle.PCMDescription_zh.Trim() != UpdatedArticle.PCMDescription_zh.Trim())
                {
                    IsArticleSync = true;
                    if (string.IsNullOrEmpty(UpdatedArticle.PCMDescription_zh))
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateZH").ToString(), ClonedArticle.PCMDescription_zh, "None", ClonedArticle.Reference) });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedArticle.PCMDescription_zh))
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateZH").ToString(), "None", UpdatedArticle.PCMDescription_zh, ClonedArticle.Reference) });
                        else
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("WarehouseArticleDescriptionUpdateZH").ToString(), ClonedArticle.PCMDescription_zh, UpdatedArticle.PCMDescription_zh, ClonedArticle.Reference) });
                    }
                }

            }
        }

        private void GetCompatibilityCount()
        {
            int Mandatory_count = 0;
            int Suggested_count = 0;
            int Incompatible_count = 0;

            if (MandatoryList != null)
                Mandatory_count = MandatoryList.Count();

            if (SuggestedList != null)
                Suggested_count = SuggestedList.Count();

            if (IncompatibleList != null)
                Incompatible_count = IncompatibleList.Count();


            CompatibilityCount = Mandatory_count + Suggested_count + Incompatible_count;
        }

        private void UpdatePCMCategoryCount()
        {
            foreach (PCMArticleCategory item in CategoryList)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (CategoryList.Any(a => a.Parent == item.IdPCMArticleCategory))
                {
                    List<PCMArticleCategory> getFirstList = CategoryList.Where(a => a.Parent == item.IdPCMArticleCategory).ToList();
                    foreach (PCMArticleCategory item1 in getFirstList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (CategoryList.Any(a => a.Parent == item1.IdPCMArticleCategory))
                        {
                            List<PCMArticleCategory> getSecondList = CategoryList.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                            foreach (PCMArticleCategory item2 in getSecondList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (CategoryList.Any(a => a.Parent == item2.IdPCMArticleCategory))
                                {
                                    List<PCMArticleCategory> getThirdList = CategoryList.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                    foreach (PCMArticleCategory item3 in getThirdList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (CategoryList.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                        {
                                            List<PCMArticleCategory> getForthList = CategoryList.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                            foreach (PCMArticleCategory item4 in getForthList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (CategoryList.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                                {
                                                    List<PCMArticleCategory> getFifthList = CategoryList.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                    foreach (PCMArticleCategory item5 in getFifthList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (CategoryList.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                        {
                                                            List<PCMArticleCategory> getSixthList = CategoryList.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                            foreach (PCMArticleCategory item6 in getSixthList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (CategoryList.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                                {
                                                                    List<PCMArticleCategory> getSeventhList = CategoryList.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                    foreach (PCMArticleCategory item7 in getSeventhList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (CategoryList.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                        {
                                                                            List<PCMArticleCategory> getEightthList = CategoryList.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                            foreach (PCMArticleCategory item8 in getEightthList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (CategoryList.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                                {
                                                                                    List<PCMArticleCategory> getNinethList = CategoryList.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                    foreach (PCMArticleCategory item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (CategoryList.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                        {
                                                                                            List<PCMArticleCategory> gettenthList = CategoryList.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
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

        public void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);
                if (PCMCommon.Instance.Languages == null || PCMCommon.Instance.Languages?.Count == 0)
                {
                    PCMCommon.Instance.Languages= new ObservableCollection<Language>(PCMService.GetAllLanguages());
                    
                }
                if(PCMCommon.Instance.Languages!=null)
                {
                    Languages = PCMCommon.Instance.Languages;
                    LanguageSelected = Languages.FirstOrDefault();
                    LanguageSelectedInCatelogueDescription = Languages.FirstOrDefault();
                }
                GeosApplication.Instance.Logger.Log("Method AddLanguages()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLanguages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLanguages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillLogicList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogicList ...", category: Category.Info, priority: Priority.Low);
                LogicList = new List<LookupValue>(PCMService.GetLookupValues(63));
                //SelectedLogic = LogicList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillLogicList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLogicList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLogicList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLogicList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void RetrieveNameByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveNameByLanguge()...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyName == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description = Description_en;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description = Description_es;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description = Description_fr;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description = Description_pt;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description = Description_ro;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description = Description_ru;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description = Description_zh;
                    }
                 //   IsEnabledCancelButton = false;

                }

                GeosApplication.Instance.Logger.Log("Method RetrieveNameByLanguge()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveNameByLanguge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveNameByLanguge_PCMCatelogue(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveNameByLanguge_PCMCatelogue()...", category: Category.Info, priority: Priority.Low);

                var PCMDescription_Richtext1 = PCMDescription_Richtext;
                var PCMDescription1 = PCMDescription;

                PcmArticleCatalogueDescriptionManager.GetTextForCurrentOnelanguage(
                    ref PCMDescription_Richtext1, ref PCMDescription1);

                PCMDescription = PCMDescription1;
                PCMDescription_Richtext = PCMDescription_Richtext1;
              //  IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                GeosApplication.Instance.Logger.Log("Method RetrieveNameByLanguge_PCMCatelogue()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveNameByLanguge_PCMCatelogue()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage..."), category: Category.Info, priority: Priority.Low);
                if (IsCheckedCopyName == false && LanguageSelected != null)
                {
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
                }
                if (IsCheckedCopyName == false && LanguageSelected != null)
                {
                    if ((oldDescription_en != Description_en && Description_en != "" && !string.IsNullOrEmpty(Description_en)) ||
                    (oldDescription_es != Description_es && Description_es != "" && !string.IsNullOrEmpty(Description_es)) || (oldDescription_fr != Description_fr && Description_fr != "" && !string.IsNullOrEmpty(Description_fr)) ||
                   (oldDescription_pt != Description_pt && Description_pt != "" && !string.IsNullOrEmpty(Description_pt)) || (oldDescription_ro != Description_ro && Description_ro != "" && !string.IsNullOrEmpty(Description_ro)) ||
                            (oldDescription_ru != Description_ru && Description_ru != "" && !string.IsNullOrEmpty(Description_ru)) || (oldDescription_zh != Description_zh && Description_zh != "" && !string.IsNullOrEmpty(Description_zh)))
                    {
                        IsEnabledCancelButton = true;
                    }
                    else
                    {
                        IsEnabledCancelButton = false;
                    }
                }
                else
                {
                    if ((oldDescription_en != Description && Description != "" && !string.IsNullOrEmpty(Description)) ||
                    (oldDescription_es != Description_es && Description_es != "" && !string.IsNullOrEmpty(Description_es)) || (oldDescription_fr != Description_fr && Description_fr != "" && !string.IsNullOrEmpty(Description_fr)) ||
                   (oldDescription_pt != Description_pt && Description_pt != "" && !string.IsNullOrEmpty(Description_pt)) || (oldDescription_ro != Description_ro && Description_ro != "" && !string.IsNullOrEmpty(Description_ro)) ||
                            (oldDescription_ru != Description_ru && Description_ru != "" && !string.IsNullOrEmpty(Description_ru)) || (oldDescription_zh != Description_zh && Description_zh != "" && !string.IsNullOrEmpty(Description_zh)))
                    {
                        IsEnabledCancelButton = true;
                    }
                    else
                    {
                        IsEnabledCancelButton = false;
                    }
                }
               

                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetNameToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PCMCatalogueDescription_RichEditControlTextChangedCommandAction(object obj)
        {
            try
            {
                //// Not added log because the method is executed continuously while typing
                //GeosApplication.Instance.Logger.Log("Method PCMCatalogueDescription_RichEditControlTextChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.Logger.Log("Method PCMCatalogueDescription_TextEditControlEditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                if (!IsRtf)
                    return;
                //RichEditControl txt = (((System.Windows.FrameworkElement)(obj))) as RichEditControl;

                //if (txt.DocumentSource == txt.Content.Content)
                //{
                //    return;
                //}

                //else if (txt.HtmlText != Convert.ToString(txt.Document.HtmlText))
                //{
                //    return;
                //}

                GroupBox groupBox1 = (GroupBox)((object[])obj)[1];
                if (groupBox1.State == GroupBoxState.Normal)
                {
                    return;
                }

                var ObjRichEditControl = (RichEditControl)((object[])obj)[0];
                PcmArticleCatalogueDescriptionManager.UpdateEnteredRichTextForCurrentOnelanguage(ObjRichEditControl);

                //GeosApplication.Instance.Logger.Log("Method PCMCatalogueDescription_RichEditControlTextChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PCMCatalogueDescription_RichEditControlTextChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UncheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Description = Description_en;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Description = Description_es;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Description = Description_fr;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Description = Description_pt;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Description = Description_ro;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Description = Description_ru;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Description = Description_zh;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method UncheckedCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CheckOrUncheckCheckBoxPCMCatelogueCopyNameDescription()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckOrUncheckCheckBoxPCMCatelogueCopyNameDescription...", category: Category.Info, priority: Priority.Low);

                //PcmArticleCatalogueDescriptionManager.CopyCatelogueDescription();
                //LanguageSelectedInCatelogueDescription = Languages.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method CheckOrUncheckCheckBoxPCMCatelogueCopyNameDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckOrUncheckCheckBoxPCMCatelogueCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [rdixit][15.07.2022][GEOS2-3785]
        /// </summary>
        /// <param name="obj"></param>

        private void ImportWarehouseItemsToPCM(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ImportWarehouseItemsToPCM()...", category: Category.Info, priority: Priority.Low);
                ProductTypeArticleViewModel ProductTypeArticleViewModel = new ProductTypeArticleViewModel();
                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        ProductTypeArticleViewModel.UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                TreeListView detailView = (TreeListView)obj;
                ImportWarehouseItemToPCMView importWarehouseItemToPCMView = new ImportWarehouseItemToPCMView();
                ImportWarehouseItemToPCMModel importWarehouseItemToPCMModel = new ImportWarehouseItemToPCMModel();
                EventHandler handle = delegate { importWarehouseItemToPCMView.Close(); };
                importWarehouseItemToPCMModel.RequestClose += handle;
                if (ArticleDecompositionList != null)
                {
                    //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                    //List<string> WarehouseArticleReference = ArticleDecompositionList.Where(i => i.PCMArticle == null).Select(i => i.Reference).ToList();
                    List<string> WarehouseArticleReference = ArticleDecompositionList.Where(i => i.PCMArticle == null).Select(i => i.Reference).Distinct().ToList();
                    importWarehouseItemToPCMModel.WarehouseArticleReference = new List<string>();
                    importWarehouseItemToPCMModel.WarehouseArticleReference = WarehouseArticleReference;
                    //importWarehouseItemToPCMModel.References = "[Reference] In (" + String.Join(",", WarehouseArticleReference) + ")";
                    //importWarehouseItemToPCMModel.MyFilterString= "[Reference] In (" + String.Join(",", WarehouseArticleReference) + ")";
                    importWarehouseItemToPCMModel.WarehouseFilterString = string.Join(" ", WarehouseArticleReference);
                }
                importWarehouseItemToPCMModel.Init();
                importWarehouseItemToPCMView.DataContext = importWarehouseItemToPCMModel;
                var ownerInfo = (detailView as FrameworkElement);
                importWarehouseItemToPCMView.Owner = Window.GetWindow(ownerInfo);
                importWarehouseItemToPCMView.ShowDialog();

                if (importWarehouseItemToPCMModel.IsSaveChanges)
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
                    ArticleDecompositionList = new ObservableCollection<ArticleDecomposition>(PCMService.GetArticleDeCompostionByIdArticle(IdArticle));
                    foreach (ArticleDecomposition item in ArticleDecompositionList.Where(i => i.PCMArticle != null))
                    {
                        if (ECOSVisibilityList.Any(x => x.IdLookupValue == item.PCMArticle.IdECOSVisibility))
                        {
                            item.PCMArticle.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == item.PCMArticle.IdECOSVisibility).HtmlColor;
                            item.PCMArticle.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == item.PCMArticle.IdECOSVisibility).Value;
                        }

                    }
                    var grid = detailView.Parent as TreeListControl;
                    ((DevExpress.Xpf.Grid.TreeListView)obj).DataControl.RefreshData();
                    ((DevExpress.Xpf.Grid.TreeListView)obj).DataControl.UpdateLayout();
                    grid.RefreshData();
                    grid.UpdateLayout();

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }

                GeosApplication.Instance.Logger.Log("Method ImportWarehouseItemsToPCM()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ImportWarehouseItemsToPCM()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);


                AddEditArticleCustomerView addEditArticleCustomerView = new AddEditArticleCustomerView();
                AddEditArticleCustomerViewModel addEditArticleCustomerViewModel = new AddEditArticleCustomerViewModel(obj);
                EventHandler handle = delegate { addEditArticleCustomerView.Close(); };
                addEditArticleCustomerViewModel.RequestClose += handle;
                addEditArticleCustomerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddArticleCustomerHeader").ToString();
                addEditArticleCustomerViewModel.IsNew = true;

                if (PCMArticleCustomerList == null)
                    PCMArticleCustomerList = new List<ArticleCustomer>();

                addEditArticleCustomerViewModel.ArticleCustomerList = PCMArticleCustomerList;
                addEditArticleCustomerView.DataContext = addEditArticleCustomerViewModel;
                addEditArticleCustomerView.ShowDialog();
                CustomerReference = addEditArticleCustomerViewModel.Reference.Trim();
                Dictionary<string, byte[]> DictCompanies = new Dictionary<string, byte[]>();

                if (addEditArticleCustomerViewModel.IsSave == true)
                {
                    if (addEditArticleCustomerViewModel.SelectedGroup.IdGroup == 0)
                    {
                        foreach (Site site in addEditArticleCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    ArticleCustomer customer = new ArticleCustomer();
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
                                    customer.ReferenceCustomer = CustomerReference;
                                    ArticleCustomerList.Add(customer);
                                }
                            }
                        }

                        if (addEditArticleCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                        {

                            foreach (Country country in addEditArticleCustomerViewModel.SelectedCountry_Save)
                            {
                                if (country != null)
                                {
                                    foreach (Region region in addEditArticleCustomerViewModel.SelectedRegion_Save)
                                    {
                                        if (region != null)
                                        {
                                            if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().IdRegion && ccl.IdCountry == country.IdCountry))
                                            {
                                                ArticleCustomer customer = new ArticleCustomer();
                                                customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                                customer.GroupName = "ALL";
                                                customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                                customer.RegionName = addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                                customer.IdCountry = country.IdCountry;
                                                customer.Country = new Country();
                                                customer.Country.Name = country.Name;
                                                customer.CountryIconBytes = country.CountryIconBytes;
                                                customer.IdPlant = null;
                                                customer.Plant = new Site();
                                                customer.ReferenceCustomer = CustomerReference;
                                                customer.Plant.Name = "ALL";
                                                ArticleCustomerList.Add(customer);
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (Region region in addEditArticleCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    //[rdixit][GEOS2-4365][20.04.2023] addEditArticleCustomerViewModel.SelectedCountry_Save.Count > 0 condition added
                                    if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == region.IdRegion && addEditArticleCustomerViewModel.SelectedCountry_Save.Count > 0))
                                    {
                                        ArticleCustomer customer = new ArticleCustomer();
                                        customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                        customer.GroupName = "ALL";
                                        customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        customer.RegionName = region.RegionName;
                                        customer.IdCountry = null;
                                        customer.Country = new Country();
                                        customer.Country.Name = "ALL";
                                        customer.IdPlant = null;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = "ALL";
                                        customer.ReferenceCustomer = CustomerReference;
                                        ArticleCustomerList.Add(customer);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Site site in addEditArticleCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdPlant == site.IdSite))
                                {
                                    ArticleCustomer customer = new ArticleCustomer();
                                    customer.IdGroup = site.IdGroup;
                                    customer.GroupName = site.GroupName;
                                    customer.IdRegion = site.IdRegion;
                                    customer.RegionName = site.RegionName;
                                    customer.IdCountry = site.IdCountry;
                                    customer.Country = new Country();
                                    customer.Country.Name = site.CountryName;
                                    customer.IdPlant = site.IdSite;
                                    //     customer.CountryIconBytes = site.CountryIconBytes;
                                    customer.Plant = new Site();
                                    customer.Plant.Name = site.Name;
                                    customer.ReferenceCustomer = CustomerReference;
                                    ArticleCustomerList.Add(customer);
                                }
                            }

                        }
                        if (addEditArticleCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                        {
                            foreach (Country country in addEditArticleCustomerViewModel.SelectedCountry_Save)
                            {
                                if (country != null)
                                {
                                    //[rdixit][GEOS2-4365][20.04.2023] addEditArticleCustomerViewModel.SelectedPlant_Save.Count>0 condition added
                                    if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry && addEditArticleCustomerViewModel.SelectedPlant_Save.Count > 0))
                                    {
                                        ArticleCustomer customer = new ArticleCustomer();
                                        customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                        customer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;
                                        customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                        customer.RegionName = addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                        customer.IdCountry = country.IdCountry;
                                        customer.Country = new Country();
                                        customer.Country.Name = country.Name;
                                        customer.CountryIconBytes = country.CountryIconBytes;
                                        customer.IdPlant = null;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = "ALL";
                                        customer.ReferenceCustomer = CustomerReference;
                                        ArticleCustomerList.Add(customer);
                                    }
                                }
                            }

                            foreach (Region region in addEditArticleCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    //[rdixit][GEOS2-4365][20.04.2023] addEditArticleCustomerViewModel.SelectedCountry_Save.Count>0 Condition added
                                    if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion && addEditArticleCustomerViewModel.SelectedCountry_Save.Count > 0))
                                    {
                                        ArticleCustomer customer = new ArticleCustomer();
                                        customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                        customer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;
                                        customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        customer.RegionName = region.RegionName;
                                        customer.IdCountry = null;
                                        customer.Country = new Country();
                                        customer.Country.Name = "ALL";
                                        customer.IdPlant = null;
                                        customer.Plant = new Site();
                                        customer.Plant.Name = "ALL";
                                        //  customer.CountryIconBytes = country.CountryIconBytes;
                                        customer.ReferenceCustomer = CustomerReference;
                                        ArticleCustomerList.Add(customer);
                                    }
                                }
                                else
                                {
                                    ArticleCustomer customer = new ArticleCustomer();
                                    customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                    customer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;
                                    customer.IdRegion = null;
                                    customer.RegionName = "ALL";
                                    customer.IdCountry = null;
                                    customer.Country = new Country();
                                    customer.Country.Name = "ALL";
                                    customer.IdPlant = null;
                                    customer.Plant = new Site();
                                    customer.Plant.Name = "ALL";
                                    //customer.CountryIconBytes = country.CountryIconBytes;
                                    customer.ReferenceCustomer = CustomerReference;
                                    ArticleCustomerList.Add(customer);
                                }
                            }
                        }

                    }

                    ArticleCustomerList = new ObservableCollection<ArticleCustomer>(ArticleCustomerList.OrderBy(a => a.GroupName));

                    List<ArticleCustomer> IsCheckedList = ArticleCustomerList.ToList();

                    Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();
                    //Reference =  CustomerReference; 
                    Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

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

        private void DeleteCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteIncludedCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCPLCustomer"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    DevExpress.Xpf.Grid.EditGridCellData tempRecords = (DevExpress.Xpf.Grid.EditGridCellData)obj;
                    SelectedArticleCustomer = (ArticleCustomer)tempRecords.Row;
                    //SelectedArticleCustomer = ArticleCustomerList.FirstOrDefault();
                    ArticleCustomerList.Remove(SelectedArticleCustomer);
                    ArticleCustomerList = new ObservableCollection<ArticleCustomer>(ArticleCustomerList);
                    SelectedArticleCustomer = ArticleCustomerList.FirstOrDefault();
                    ArticleCustomerList = new ObservableCollection<ArticleCustomer>(ArticleCustomerList.OrderBy(a => a.GroupName));

                    //FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.IdArticleDoc).Take(4).ToList());
                }

                //DevExpress.Xpf.Grid.EditGridCellData tempRecords = (DevExpress.Xpf.Grid.EditGridCellData)obj;
                //if (ArticleCustomerList.Any(a => a.IsChecked == true && a.IsIncluded == 1))
                //{
                //    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCPLCustomer"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                //    //CPLCustomer SelectedCustomer = (CPLCustomer)tempRecords.Row;
                //    //if (MessageBoxResult == MessageBoxResult.Yes)
                //    //{
                //    //    // ArticleCustomerList.Remove(r => r.IsChecked == true && r.IsIncluded == 1);
                //        ArticleCustomerList = new ObservableCollection<ArticleCustomer>(ArticleCustomerList.OrderBy(a => a.GroupName));
                //        SelectedArticleCustomer = ArticleCustomerList.FirstOrDefault();

                //        List<ArticleCustomer> IsCheckedList = ArticleCustomerList.ToList();

                //        Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                //        Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                //        Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                //        Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();
                //        Reference = CustomerReference;
                //        Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                //        Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                //        Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                //        Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

                //        ArticleCustomerList.Remove(SelectedArticleCustomer);
                //        //IsExportVisible = Visibility.Collapsed;
                //        //    }

                //   // }
                //    //else
                //    //{
                //    //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteIncludedCustomer").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    //}
                //}





                GeosApplication.Instance.Logger.Log("Method DeleteIncludedCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteIncludedCustomerCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(CustomerGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(CustomerGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(CustomerGridSettingFilePath);

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
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(CustomerGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(CustomerGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void EditCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ArticleCustomer SelectedCustomer = (ArticleCustomer)detailView.DataControl.CurrentItem;

                AddEditArticleCustomerView addEditArticleCustomerView = new AddEditArticleCustomerView();
                AddEditArticleCustomerViewModel addEditArticleCustomerViewModel = new AddEditArticleCustomerViewModel(obj);
                EventHandler handle = delegate { addEditArticleCustomerView.Close(); };
                addEditArticleCustomerViewModel.RequestClose += handle;
                addEditArticleCustomerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCPLCustomerHeader").ToString();
                addEditArticleCustomerViewModel.IsNew = false;

                addEditArticleCustomerViewModel.EditInit(SelectedCustomer);
                addEditArticleCustomerViewModel.ArticleCustomerList = PCMArticleCustomerList;
                addEditArticleCustomerView.DataContext = addEditArticleCustomerViewModel;
                addEditArticleCustomerView.ShowDialog();
                int count = 0;
                if (addEditArticleCustomerViewModel.IsSave == true)
                {

                    bool isupdatedRow = false;

                    if (SelectedCustomer.IdGroup == 0 && addEditArticleCustomerViewModel.SelectedGroup.IdGroup == 0)
                    {
                        #region 0 & 0
                        foreach (Site site in addEditArticleCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!PCMArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditArticleCustomerViewModel.IdArticleCustomer > 0)
                                            SelectedCustomer = ArticleCustomerList.Where(ccl => ccl.IdArticleCustomerReferences == Convert.ToInt32(addEditArticleCustomerViewModel.IdArticleCustomer)).FirstOrDefault();

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
                                        SelectedCustomer.ReferenceCustomer = addEditArticleCustomerViewModel.Reference;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        ArticleCustomer customer = new ArticleCustomer();
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
                                        customer.ReferenceCustomer = CustomerReference;
                                        ArticleCustomerList.Add(customer);
                                    }
                                }
                            }
                        }

                        foreach (Country country in addEditArticleCustomerViewModel.SelectedCountry_Save)
                        {
                            if (country != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdCountry == country.IdCountry && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditArticleCustomerViewModel.IdArticleCustomer > 0)
                                            SelectedCustomer = ArticleCustomerList.Where(ccl => ccl.IdArticleCustomerReferences == Convert.ToInt32(addEditArticleCustomerViewModel.IdArticleCustomer)).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                        SelectedCustomer.GroupName = "ALL";
                                        SelectedCustomer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                        SelectedCustomer.RegionName = addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                        SelectedCustomer.IdCountry = country.IdCountry;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = country.Name;
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.ReferenceCustomer = addEditArticleCustomerViewModel.Reference;
                                        SelectedCustomer.Plant.Name = "ALL";
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditArticleCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            ArticleCustomer customer = new ArticleCustomer();
                                            customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                            customer.GroupName = "ALL";
                                            customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                            customer.RegionName = addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                            customer.IdCountry = country.IdCountry;
                                            customer.Country = new Country();
                                            customer.Country.Name = country.Name;
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            customer.ReferenceCustomer = CustomerReference;
                                            ArticleCustomerList.Add(customer);
                                        }
                                    }
                                }
                            }
                        }


                        foreach (Region region in addEditArticleCustomerViewModel.SelectedRegion_Save)
                        {
                            if (region != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == region.IdRegion && ccl.IdCountry == null && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditArticleCustomerViewModel.IdArticleCustomer > 0)
                                            SelectedCustomer = ArticleCustomerList.Where(ccl => ccl.IdArticleCustomerReferences == Convert.ToInt32(addEditArticleCustomerViewModel.IdArticleCustomer)).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                        SelectedCustomer.GroupName = "ALL";
                                        SelectedCustomer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        SelectedCustomer.RegionName = region.RegionName;
                                        SelectedCustomer.IdCountry = null;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = "ALL";
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        SelectedCustomer.ReferenceCustomer = addEditArticleCustomerViewModel.Reference;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditArticleCustomerViewModel.SelectedCountry_Save.FirstOrDefault() == null && addEditArticleCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            ArticleCustomer customer = new ArticleCustomer();
                                            customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;
                                            customer.GroupName = "ALL";
                                            customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                            customer.RegionName = region.RegionName;
                                            customer.IdCountry = null;
                                            customer.Country = new Country();
                                            customer.Country.Name = "ALL";
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            customer.ReferenceCustomer = CustomerReference;
                                            ArticleCustomerList.Add(customer);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        foreach (Site site in addEditArticleCustomerViewModel.SelectedPlant_Save)
                        {
                            if (site != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditArticleCustomerViewModel.IdArticleCustomer > 0)
                                            SelectedCustomer = ArticleCustomerList.Where(ccl => ccl.IdArticleCustomerReferences == Convert.ToInt32(addEditArticleCustomerViewModel.IdArticleCustomer)).FirstOrDefault();

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
                                        SelectedCustomer.ReferenceCustomer = addEditArticleCustomerViewModel.Reference;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        ArticleCustomer customer = new ArticleCustomer();
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
                                        customer.ReferenceCustomer = CustomerReference;
                                        ArticleCustomerList.Add(customer);
                                    }
                                }
                            }
                        }

                        foreach (Country country in addEditArticleCustomerViewModel.SelectedCountry_Save)
                        {
                            if (country != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditArticleCustomerViewModel.IdArticleCustomer > 0)
                                            SelectedCustomer = ArticleCustomerList.Where(ccl => ccl.IdArticleCustomerReferences == Convert.ToInt32(addEditArticleCustomerViewModel.IdArticleCustomer)).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;

                                        if (string.IsNullOrEmpty(addEditArticleCustomerViewModel.SelectedGroup.GroupName) || addEditArticleCustomerViewModel.SelectedGroup.GroupName == "")
                                            SelectedCustomer.GroupName = "ALL";
                                        else
                                            SelectedCustomer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;

                                        SelectedCustomer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                        SelectedCustomer.RegionName = addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                        SelectedCustomer.IdCountry = country.IdCountry;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = country.Name;
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        SelectedCustomer.ReferenceCustomer = addEditArticleCustomerViewModel.Reference;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditArticleCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            ArticleCustomer customer = new ArticleCustomer();
                                            customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;

                                            if (string.IsNullOrEmpty(addEditArticleCustomerViewModel.SelectedGroup.GroupName) || addEditArticleCustomerViewModel.SelectedGroup.GroupName == "")
                                                customer.GroupName = "ALL";
                                            else
                                                customer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;

                                            customer.IdRegion = Convert.ToUInt32(country.IdRegion);
                                            customer.RegionName = addEditArticleCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                            customer.IdCountry = country.IdCountry;
                                            customer.Country = new Country();
                                            customer.Country.Name = country.Name;
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            customer.ReferenceCustomer = CustomerReference;
                                            ArticleCustomerList.Add(customer);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (Region region in addEditArticleCustomerViewModel.SelectedRegion_Save)
                        {
                            if (region != null)
                            {
                                if (!ArticleCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion && ccl.IdCountry == null && ccl.IdPlant == null))
                                {
                                    if (!isupdatedRow)
                                    {
                                        if (addEditArticleCustomerViewModel.IdArticleCustomer > 0)
                                            SelectedCustomer = ArticleCustomerList.Where(ccl => ccl.IdArticleCustomerReferences == Convert.ToInt32(addEditArticleCustomerViewModel.IdArticleCustomer)).FirstOrDefault();

                                        SelectedCustomer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;

                                        if (string.IsNullOrEmpty(addEditArticleCustomerViewModel.SelectedGroup.GroupName) || addEditArticleCustomerViewModel.SelectedGroup.GroupName == "")
                                            SelectedCustomer.GroupName = "ALL";
                                        else
                                            SelectedCustomer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;

                                        SelectedCustomer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                        SelectedCustomer.RegionName = region.RegionName;
                                        SelectedCustomer.IdCountry = null;
                                        SelectedCustomer.Country = new Country();
                                        SelectedCustomer.Country.Name = "ALL";
                                        SelectedCustomer.IdPlant = null;
                                        SelectedCustomer.Plant = new Site();
                                        SelectedCustomer.Plant.Name = "ALL";
                                        SelectedCustomer.ReferenceCustomer = addEditArticleCustomerViewModel.Reference;
                                        isupdatedRow = true;
                                    }
                                    else
                                    {
                                        if (addEditArticleCustomerViewModel.SelectedCountry_Save.FirstOrDefault() == null && addEditArticleCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                                        {
                                            ArticleCustomer customer = new ArticleCustomer();
                                            customer.IdGroup = (uint)addEditArticleCustomerViewModel.SelectedGroup.IdGroup;

                                            if (string.IsNullOrEmpty(addEditArticleCustomerViewModel.SelectedGroup.GroupName) || addEditArticleCustomerViewModel.SelectedGroup.GroupName == "")
                                                customer.GroupName = "ALL";
                                            else
                                                customer.GroupName = addEditArticleCustomerViewModel.SelectedGroup.GroupName;

                                            customer.IdRegion = Convert.ToUInt32(region.IdRegion);
                                            customer.RegionName = region.RegionName;
                                            customer.IdCountry = null;
                                            customer.Country = new Country();
                                            customer.Country.Name = "ALL";
                                            customer.IdPlant = null;
                                            customer.Plant = new Site();
                                            customer.Plant.Name = "ALL";
                                            customer.ReferenceCustomer = CustomerReference;
                                            ArticleCustomerList.Add(customer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ArticleCustomerList = new ObservableCollection<ArticleCustomer>(ArticleCustomerList.OrderBy(a => a.GroupName));

                    List<ArticleCustomer> IsCheckedList = ArticleCustomerList.ToList();

                    Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();
                    // Reference = CustomerReference;
                    Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());


                    //if (addEditArticleCustomerViewModel.IdCPLCustomer > 0)
                    //{
                    //    CPLCustomer customer_old = ClonedCustomerPrice.CustomerList.FirstOrDefault(a => a.IdCustomerPriceListCustomer == addEditArticleCustomerViewModel.IdCPLCustomer);
                    //    if ((SelectedCustomer.IdGroup != customer_old.IdGroup) || (SelectedCustomer.IdRegion != customer_old.IdRegion) || (SelectedCustomer.IdCountry != customer_old.IdCountry) || (SelectedCustomer.IdPlant != customer_old.IdPlant))
                    //    {
                    //        IsExportVisible = Visibility.Collapsed;
                    //    }
                    //}
                    //IsEnabledSaveButton = true;
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditCustomerCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region[rdixit][21.04.2023][GEOS2-2725]
        private void RichEditControl_Loaded(object obj)
        {
            System.Windows.RoutedEventArgs arg = (System.Windows.RoutedEventArgs)obj;
            RichEditControl t = ((RichEditControl)arg.Source);
            t.DocumentCapabilitiesOptions.InlinePictures = DocumentCapability.Disabled;
            t.DocumentCapabilitiesOptions.FloatingObjects = DocumentCapability.Disabled;
        }

        private void RichEditControl_DragOver(object obj)
        {
            System.Windows.DragEventArgs e = (System.Windows.DragEventArgs)obj;
            if (e.Data.GetDataPresent(DataFormats.Bitmap) || e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Handled = true;
            }
        }
        #endregion
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

                string error =
                    me[BindableBase.GetPropertyName(() => SelectedStatus)] +
                    me[BindableBase.GetPropertyName(() => SelectedCategory)] +
                      me[BindableBase.GetPropertyName(() => InformationError)] +
                     me[BindableBase.GetPropertyName(() => PQuantityMin)] +
                     me[BindableBase.GetPropertyName(() => PQuantityMax)] +
                     me[BindableBase.GetPropertyName(() => CompatibilityError)] +
                     me[BindableBase.GetPropertyName(() => Description)] +
                     me[BindableBase.GetPropertyName(() => Description_en)];


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

                string selectedStatus = BindableBase.GetPropertyName(() => SelectedStatus);
                string selectedCategory = BindableBase.GetPropertyName(() => SelectedCategory);
                string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);
                string pQuantityMin = BindableBase.GetPropertyName(() => PQuantityMin);
                string pQuantityMax = BindableBase.GetPropertyName(() => PQuantityMax);
                string gridCompatibilityError = BindableBase.GetPropertyName(() => CompatibilityError);
                string description = BindableBase.GetPropertyName(() => Description);
                string description_en = BindableBase.GetPropertyName(() => Description_en);


                if (columnName == selectedStatus)
                {
                    return EditPCMArticleValidation.GetErrorMessage(selectedStatus, SelectedStatus);
                }

                if (columnName == selectedCategory)
                {
                    return EditPCMArticleValidation.GetErrorMessage(selectedCategory, SelectedCategory);
                }


                if (columnName == gridCompatibilityError)
                    return EditPCMArticleValidation.GetErrorMessage(gridCompatibilityError, CompatibilityError);

                if (columnName == pQuantityMin)
                {
                    if (!string.IsNullOrEmpty(pQuantityMinErrorMessage))
                    {
                        return pQuantityMinErrorMessage;
                    }
                    else
                    {
                        return EditPCMArticleValidation.GetErrorMessage(pQuantityMin, PQuantityMin);
                    }
                }

                if (columnName == pQuantityMax)
                {
                    if (!string.IsNullOrEmpty(pQuantityMaxErrorMessage))
                    {
                        return pQuantityMaxErrorMessage;
                    }
                    else
                    {
                        return EditPCMArticleValidation.GetErrorMessage(pQuantityMax, PQuantityMax);
                    }
                }

                if (columnName == headerInformtionError)
                {
                    return EditPCMArticleValidation.GetErrorMessage(headerInformtionError, InformationError);
                }

                if (columnName == description)
                {
                    return EditPCMArticleValidation.GetErrorMessage(description, Description);
                }

                if (columnName == description_en)
                {
                    return EditPCMArticleValidation.GetErrorMessage(description_en, Description_en);
                }


                return null;
            }
        }



        #endregion

        #region PLMArticles
        private void OnDragRecordOverNotIncludedArticleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverNotIncludedArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PLMArticlePrice).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    //[001][skadam][GEOS2-3607] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 1
                    List<PLMArticlePrice> record = data.Records.OfType<PLMArticlePrice>().ToList();
                    int flag = 0;
                    List<string> tempList = new List<string>();
                    foreach (var item in record)
                    {
                        if (item.IdPermission.Equals("50"))
                        {
                            if (flag == 0)
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceGridpermission").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            tempList.Add(item.Code);
                            flag = 1;
                        }
                    }

                    foreach (var itemRemove in tempList)
                    {
                        record.RemoveAll(r => r.IdPermission.Equals("50"));
                    }

                    //check validation
                    if (record.Count > 0)
                    {
                        int count = 0;
                        List<string> Bplcodes = new List<string>();
                        List<PLMArticlePrice> deleteCPL = new List<PLMArticlePrice>();
                        foreach (PLMArticlePrice item in record)
                        {
                            if (item.IdStatus == 224 || item.Type == "CPL")
                                continue;
                            if (IncludedPLMArticlePriceList.Any(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList))
                            {
                                PLMArticlePrice checkCPLDrag = IncludedPLMArticlePriceList.FirstOrDefault(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList);
                                if (!record.Any(a => a.Type == "CPL" && a.IdCustomerOrBasePriceList == checkCPLDrag.IdCustomerOrBasePriceList))
                                {
                                    deleteCPL.AddRange(IncludedPLMArticlePriceList.Where(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList));
                                    Bplcodes.Add(item.Code);
                                    count++;
                                }
                            }
                        }

                        if (count > 0)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["DeletePCMArticlePriceFromBPLCPLValidationMessage"].ToString(), string.Join(",", Bplcodes.Select(a => a.ToString()))), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMArticlePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMArticlePriceList.Remove(item);
                                    IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                                            //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                            //{
                                            //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                            //    {

                                            //    }
                                            //    CurrencySymbol = SelectedCurrencySymbol;
                                            //}
                                            //else
                                            //{
                                            //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                                            //}
                                        }
                                        //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                        //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                        //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                        //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                        //else
                                        //    IncludedFirstActiveSellPrice = null;

                                        //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                        //    CurrencySymbol = "";
                                        //else
                                        //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = "";
                                        IncludedFirstActiveCurrencyIconImage = null;
                                        IncludedFirstActiveSellPrice = null;
                                        IncludedActiveCount = 0;
                                        CurrencySymbol = "";
                                    }
                                    item.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == item.IdRule);
                                    if (!NotIncludedPLMArticlePriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMArticlePriceList.Add(item);
                                }
                                foreach (PLMArticlePrice item in deleteCPL)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMArticlePriceList.Remove(item);
                                    IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                                            //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                            //{
                                            //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                            //    {

                                            //    }
                                            //    CurrencySymbol = SelectedCurrencySymbol;
                                            //}
                                            //else
                                            //{
                                            //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                                            //}
                                        }
                                        //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                        //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                        //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                        //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                        //else
                                        //    IncludedFirstActiveSellPrice = null;

                                        //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                        //    CurrencySymbol = "";
                                        //else
                                        //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = "";
                                        IncludedFirstActiveCurrencyIconImage = null;
                                        IncludedFirstActiveSellPrice = null;
                                        IncludedActiveCount = 0;
                                        CurrencySymbol = "";
                                    }
                                    item.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == item.IdRule);
                                    if (!NotIncludedPLMArticlePriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMArticlePriceList.Add(item);
                                }
                                NotIncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(NotIncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                            }
                            else
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = false;
                            }
                        }
                        else
                        {
                            if (record.Count == 1)
                            {
                                if (record.FirstOrDefault().IdStatus == 224)
                                {
                                    e.Effects = DragDropEffects.None;
                                    e.Handled = false;
                                    return;
                                }
                            }
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePCMArticlePriceFromBPLCPLMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMArticlePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMArticlePriceList.Remove(item);
                                    IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                                            //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                            //{
                                            //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                            //    {

                                            //    }
                                            //    CurrencySymbol = SelectedCurrencySymbol;
                                            //}
                                            //else
                                            //{
                                            //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                                            //}
                                        }
                                        //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                        //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                        //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                        //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                        //else
                                        //    IncludedFirstActiveSellPrice = null;

                                        //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                        //    CurrencySymbol = "";
                                        //else
                                        //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = "";
                                        IncludedFirstActiveCurrencyIconImage = null;
                                        IncludedFirstActiveSellPrice = null;
                                        IncludedActiveCount = 0;
                                        CurrencySymbol = "";
                                    }
                                    item.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == item.IdRule);
                                    if (!NotIncludedPLMArticlePriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMArticlePriceList.Add(item);
                                }
                                NotIncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(NotIncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                            }
                            else
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = false;
                            }
                        }
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = false;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverNotIncludedArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverNotIncludedArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOverIncludedArticleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncludedArticleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PLMArticlePrice).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PLMArticlePrice> record = data.Records.OfType<PLMArticlePrice>().ToList();
                    //[001][skadam][GEOS2-3607] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 1
                    int flag = 0;
                    List<string> tempList = new List<string>();
                    foreach (var item in record)
                    {
                        if (item.IdPermission.Equals("50"))
                        {
                            if (flag == 0)
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMArticlePriceGridpermission").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            tempList.Add(item.Code);
                            flag = 1;
                        }
                    }

                    foreach (var itemRemove in tempList)
                    {
                        record.RemoveAll(r => r.IdPermission.Equals("50"));
                    }
                    //check validation
                    if (record.Count > 0)
                    {
                        int count = 0;
                        List<string> cplcodes = new List<string>();
                        List<PLMArticlePrice> addBPL = new List<PLMArticlePrice>();

                        foreach (PLMArticlePrice item in record)
                        {
                            if (item.IdStatus == 224)
                                continue;
                            if (item.Type == "CPL")
                            {
                                if (!IncludedPLMArticlePriceList.Any(a => a.Type == "BPL" && a.IdCustomerOrBasePriceList == item.IdBasePriceList))
                                {
                                    addBPL.Add(NotIncludedPLMArticlePriceList.FirstOrDefault(a => a.Type == "BPL" && a.IdCustomerOrBasePriceList == item.IdBasePriceList));
                                    cplcodes.Add(item.Code);
                                    count++;
                                }
                            }
                        }

                        if (count > 0)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["AddPCMArticlePriceInBPLCPLValidationMessage"].ToString(), string.Join(",", cplcodes.Select(a => a.ToString()))), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMArticlePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMArticlePriceList.Remove(item);
                                    if (item.IdRule == 0)
                                        item.IdRule = 308;

                                    if (item.IdRule == 308)
                                    {
                                        item.RuleValue = null;
                                        item.SellPrice = null;
                                    }
                                    else
                                    {
                                        if (item.RuleValue == null)
                                            item.RuleValue = 0;
                                        if (item.SellPrice == null)
                                            item.SellPrice = 0;
                                    }
                                    item.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == item.IdRule);
                                    if (!IncludedPLMArticlePriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMArticlePriceList.Add(item);

                                }

                                foreach (PLMArticlePrice item in addBPL)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMArticlePriceList.Remove(item);
                                    if (item.IdRule == 0)
                                        item.IdRule = 308;

                                    if (item.IdRule == 308)
                                    {
                                        item.RuleValue = null;
                                        item.SellPrice = null;
                                    }
                                    else
                                    {
                                        if (item.RuleValue == null)
                                            item.RuleValue = 0;
                                        if (item.SellPrice == null)
                                            item.SellPrice = 0;
                                    }
                                    item.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == item.IdRule);
                                    if (!IncludedPLMArticlePriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMArticlePriceList.Add(item);
                                }
                                IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                                if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                                {
                                    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                    {
                                        IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                        if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                        {
                                            IncludedFirstActiveSellPrice = null;
                                            CurrencySymbol = "";
                                        }
                                        else
                                        {
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        }
                                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                        //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                        if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                        else
                                            IncludedFirstActiveSellPrice = null;

                                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                            CurrencySymbol = "";
                                        else
                                            CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                        //{
                                        //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        //    {

                                        //    }
                                        //    CurrencySymbol = SelectedCurrencySymbol;
                                        //}
                                        //else
                                        //{
                                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                                        //}
                                    }
                                    //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                    //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                    //else
                                    //    IncludedFirstActiveSellPrice = null;

                                    //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                    //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                    //    CurrencySymbol = "";
                                    //else
                                    //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                                }
                                else
                                {
                                    IncludedFirstActiveName = "";
                                    IncludedFirstActiveCurrencyIconImage = null;
                                    IncludedFirstActiveSellPrice = null;
                                    IncludedActiveCount = 0;
                                    CurrencySymbol = "";
                                }
                            }
                            else
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = false;
                            }
                        }
                        else
                        {
                            if (record.Count == 1)
                            {
                                if (record.FirstOrDefault().IdStatus == 224)
                                {
                                    e.Effects = DragDropEffects.None;
                                    e.Handled = false;
                                    return;
                                }
                            }

                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AddPCMArticlePriceInBPLCPLMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMArticlePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMArticlePriceList.Remove(item);
                                    if (item.IdRule == 0)
                                        item.IdRule = 308;
                                    if (item.IdRule == 308)
                                    {
                                        item.RuleValue = null;
                                        item.SellPrice = null;
                                    }
                                    else
                                    {
                                        if (item.RuleValue == null)
                                            item.RuleValue = 0;
                                        if (item.SellPrice == null)
                                            item.SellPrice = 0;
                                    }
                                    item.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == item.IdRule);
                                    if (!IncludedPLMArticlePriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMArticlePriceList.Add(item);
                                }
                                IncludedPLMArticlePriceList = new ObservableCollection<PLMArticlePrice>(IncludedPLMArticlePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                                if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                                {
                                    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                    {
                                        IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                        if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                        {
                                            IncludedFirstActiveSellPrice = null;
                                            CurrencySymbol = "";
                                        }
                                        else
                                        {
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        }

                                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                        //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                        if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                        else
                                            IncludedFirstActiveSellPrice = null;

                                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                            CurrencySymbol = "";
                                        else
                                            CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                        //{
                                        //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        //    {

                                        //    }
                                        //    CurrencySymbol = SelectedCurrencySymbol;
                                        //}
                                        //else
                                        //{
                                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                                        //}
                                    }
                                    //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                                    //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                                    //else
                                    //    IncludedFirstActiveSellPrice = null;

                                    //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                                    //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                                    //    CurrencySymbol = "";
                                    //else
                                    //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                                }
                                else
                                {
                                    IncludedFirstActiveName = "";
                                    IncludedFirstActiveCurrencyIconImage = null;
                                    IncludedFirstActiveSellPrice = null;
                                    IncludedActiveCount = 0;
                                    CurrencySymbol = "";
                                }
                            }
                            else
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = false;
                            }
                        }
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = false;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncludedArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverIncludedArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][cpatil][GEOS2-3626][10-05-2022]
        private void CalculatesellPriceRulechanged(PLMArticlePrice PLMArticlePrice)
        {
            if (PLMArticlePrice.MaxCost == null)
            {
                PLMArticlePrice.MaxCost = 0;
            }
            if (PLMArticlePrice.RuleValue == null)
            {
                PLMArticlePrice.RuleValue = 0;
            }
            // calculate sellprice (common)
            if (PLMArticlePrice.IdRule == 307)
            {
                PLMArticlePrice.SellPrice = Convert.ToDouble(PLMArticlePrice.MaxCost) + ((Convert.ToDouble(PLMArticlePrice.MaxCost) * (Convert.ToDouble(PLMArticlePrice.RuleValue) / 100)));
            }
            else if (PLMArticlePrice.IdRule == 308)
            {
                if (PLMArticlePrice.RuleValue == null || PLMArticlePrice.RuleValue == null)
                {
                    PLMArticlePrice.RuleValue = null;
                    PLMArticlePrice.SellPrice = null;
                }
                else if (PLMArticlePrice.RuleValue != null && Convert.ToDouble(PLMArticlePrice.RuleValue) <= 0)
                {
                    PLMArticlePrice.RuleValue = null;
                    PLMArticlePrice.SellPrice = null;
                }
                else
                {
                    PLMArticlePrice.SellPrice = PLMArticlePrice.RuleValue;
                }
            }
            else if (PLMArticlePrice.IdRule == 1518)
            {
                PLMArticlePrice.RuleValue = 0;
                PLMArticlePrice.SellPrice = 0;
            }
            else if (PLMArticlePrice.IdRule == 309)
            {
                PLMArticlePrice.SellPrice = Convert.ToDouble(PLMArticlePrice.MaxCost) + (Convert.ToDouble(PLMArticlePrice.RuleValue));
            }

            double SellPriceValue = 0;
            double MaxCost = 0;

            if (PLMArticlePrice.SellPrice != null)
                SellPriceValue = Convert.ToDouble(PLMArticlePrice.SellPrice);

            if (PLMArticlePrice.MaxCost != null)
                MaxCost = Convert.ToDouble(PLMArticlePrice.MaxCost);

            PLMArticlePrice.Profit = Convert.ToDouble(CalculateBasePriceProfitValue(MaxCost, SellPriceValue));
            PLMArticlePrice.CostMargin = Convert.ToDouble(CalculateBasePriceCostMarginValue(MaxCost, SellPriceValue));

            PLMArticlePrice.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMArticlePrice.IdRule);


        }

        private void LostFocusRuleChangedCommandAction(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Windows.RoutedEventArgs routedEvent = (System.Windows.RoutedEventArgs)obj;
                    //var temp = ((DevExpress.Xpf.Grid.RowEventArgs)obj).Row;
                    if (routedEvent.OriginalSource is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox OriginalSource = (System.Windows.Controls.TextBox)routedEvent.OriginalSource;
                        if (OriginalSource.DataContext != null)
                        {
                            PLMArticlePrice PLMArticlePrice = ((DevExpress.Xpf.Grid.EditGridCellData)OriginalSource.DataContext).Row as PLMArticlePrice;
                            if (PLMArticlePrice.Type == "BPL")
                            {
                                List<GeosAppSetting> GeosAppSettingRuleValue = WorkbenchService.GetSelectedGeosAppSettings("89");
                                if (PLMArticlePrice.Currency != null)
                                    if (PLMArticlePrice.Currency.Name.Equals(GeosAppSettingRuleValue[0].DefaultValue))
                                    {
                                        if (IncludedPLMArticlePriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IsEnabledPermission == true).Count() > 1)
                                        {
                                            //GEOS2-3794 Improvement in the PRICES section -  Do the exchange currency automatically  24 06 2022 & 21 09 2022 
                                            if (IncludedPLMArticlePriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IsEnabledPermission == true && i.IsChecked == true).Count() > 1)
                                            {
                                                //IsBPLMessageShow = true;
                                                MessageBoxResult MessageBoxResultForCurrencyConversion = CustomMessageBox.Show(string.Format(Application.Current.Resources["PCMRuleValueCurrencyConversionMessage"].ToString(), "Base Price List"), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                                                if (MessageBoxResultForCurrencyConversion == MessageBoxResult.Yes)
                                                {
                                                    IsBPLCalculateRuleValue = true;
                                                    if (PLMCommon.Instance.CurrencyConversionList == null || PLMCommon.Instance.CurrencyConversionList.Count == 0)
                                                    {
                                                        PLMCommon.Instance.CurrencyConversionList = new List<CurrencyConversion>(PLMService.GetCurrencyConversionsDetailsByLatestDate());
                                                    }


                                                    CalculatesellPriceRulechanged(PLMArticlePrice);
                                                    foreach (PLMArticlePrice item in IncludedPLMArticlePriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IdBasePriceList != PLMArticlePrice.IdBasePriceList && i.IsEnabledPermission == true && i.IsChecked == true))
                                                    {
                                                        if (PLMCommon.Instance.CurrencyConversionList.Any(i => i.Idcurrencyto == item.IdCurrency && i.Idcurrencyfrom == PLMArticlePrice.IdCurrency))
                                                        {
                                                            item.RuleValue = PLMArticlePrice.RuleValue * (PLMCommon.Instance.CurrencyConversionList.Where(i => i.Idcurrencyto == item.IdCurrency && i.Idcurrencyfrom == PLMArticlePrice.IdCurrency).FirstOrDefault().ExchangeRate);
                                                            CalculatesellPriceRulechanged(item);
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    IsBPLCalculateRuleValue = false;
                                                    CalculatesellPriceRulechanged(PLMArticlePrice);
                                                }
                                            }
                                            else
                                            {
                                                IsBPLCalculateRuleValue = false;
                                                CalculatesellPriceRulechanged(PLMArticlePrice);
                                            }
                                        }
                                    }

                            }
                        }

                    }
                    else
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in Method LostFocusRuleChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        //[001][cpatil][GEOS2-3626][10-05-2022]
        private void RuleChangedCommandAction(object obj)
        {
            try
            {
                if (obj == null)
                    return;
                IsEnabledCancelButton = true;
                PLMArticlePrice PLMArticlePrice = ((DevExpress.Xpf.Grid.RowEventArgs)obj).Row as PLMArticlePrice;

                if (PLMArticlePrice.MaxCost == null)
                {
                    PLMArticlePrice.MaxCost = 0;
                }
                if (PLMArticlePrice.RuleValue == null)
                {
                    PLMArticlePrice.RuleValue = 0;
                }
                // calculate sellprice (common)
                if (PLMArticlePrice.IdRule == 307)
                {
                    PLMArticlePrice.SellPrice = Convert.ToDouble(PLMArticlePrice.MaxCost) + ((Convert.ToDouble(PLMArticlePrice.MaxCost) * (Convert.ToDouble(PLMArticlePrice.RuleValue) / 100)));
                }
                else if (PLMArticlePrice.IdRule == 308)
                {
                    if (PLMArticlePrice.RuleValue == null || PLMArticlePrice.RuleValue == null)
                    {
                        PLMArticlePrice.RuleValue = null;
                        PLMArticlePrice.SellPrice = null;
                    }
                    else if (PLMArticlePrice.RuleValue != null && Convert.ToDouble(PLMArticlePrice.RuleValue) <= 0)
                    {
                        PLMArticlePrice.RuleValue = null;
                        PLMArticlePrice.SellPrice = null;
                    }
                    else
                    {
                        PLMArticlePrice.SellPrice = PLMArticlePrice.RuleValue;
                    }
                }
                else if (PLMArticlePrice.IdRule == 1518)
                {
                    PLMArticlePrice.RuleValue = 0;
                    PLMArticlePrice.SellPrice = 0;
                }
                else if (PLMArticlePrice.IdRule == 309)
                {
                    PLMArticlePrice.SellPrice = Convert.ToDouble(PLMArticlePrice.MaxCost) + (Convert.ToDouble(PLMArticlePrice.RuleValue));
                }

                double SellPriceValue = 0;
                double MaxCost = 0;

                if (PLMArticlePrice.SellPrice != null)
                    SellPriceValue = Convert.ToDouble(PLMArticlePrice.SellPrice);

                if (PLMArticlePrice.MaxCost != null)
                    MaxCost = Convert.ToDouble(PLMArticlePrice.MaxCost);

                PLMArticlePrice.Profit = Convert.ToDouble(CalculateBasePriceProfitValue(MaxCost, SellPriceValue));
                PLMArticlePrice.CostMargin = Convert.ToDouble(CalculateBasePriceCostMarginValue(MaxCost, SellPriceValue));

                PLMArticlePrice.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMArticlePrice.IdRule);

                if (IncludedPLMArticlePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMArticlePriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }
                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                        //CurrencySymbol = IncludedPLMArticlePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMArticlePriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMArticlePriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;
                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                        //{
                        //    if (IncludedPLMArticlePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                        //    {

                        //    }
                        //    CurrencySymbol = SelectedCurrencySymbol;
                        //}
                        //else
                        //{
                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMArticlePriceList[0].Currency.Symbol;
                        //}
                    }
                    //IncludedFirstActiveName = IncludedPLMArticlePriceList[0].Name;
                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMArticlePriceList[0].Currency.CurrencyIconImage;

                    //if (IncludedPLMArticlePriceList[0].SellPrice != null)
                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMArticlePriceList[0].SellPrice.Value, 2);
                    //else
                    //    IncludedFirstActiveSellPrice = null;

                    //IncludedActiveCount = IncludedPLMArticlePriceList.Where(ip => ip.IdStatus == 223).Count();
                    //if (IncludedPLMArticlePriceList[0].SellPrice == null)
                    //    CurrencySymbol = "";
                    //else
                    //    CurrencySymbol = IncludedPLMArticlePriceList[0].Currency.Symbol;

                }
                else
                {
                    IncludedFirstActiveName = "";
                    IncludedFirstActiveCurrencyIconImage = null;
                    IncludedFirstActiveSellPrice = null;
                    IncludedActiveCount = 0;
                    CurrencySymbol = "";
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method RuleChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }




        private double CalculateBasePriceProfitValue(double MaxCost, double SellPriceValue)
        {
            double Profit = 0;
            try
            {
                if ((SellPriceValue == 0 && MaxCost == 0) || (SellPriceValue == MaxCost) || (SellPriceValue == 0 && MaxCost > 0))
                    Profit = 0.00;
                else
                    Profit = Convert.ToDouble(((((SellPriceValue) - (MaxCost)) / (SellPriceValue)) * 100));

                Profit = Math.Round(Profit, 2);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CalculateBasePriceProfitValue() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Profit;
        }

        private double CalculateBasePriceCostMarginValue(double MaxCost, double SellPriceValue)
        {
            double CostMargin = 0;
            try
            {
                if ((MaxCost == 0 && SellPriceValue == 0) || (MaxCost == SellPriceValue) || (MaxCost == 0 && SellPriceValue > 0))
                    CostMargin = 0.00;
                else
                    CostMargin = Convert.ToDouble(((((SellPriceValue) - (MaxCost)) / (MaxCost)) * 100));

                CostMargin = Math.Round(CostMargin, 2);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CalculateBasePriceCostMarginValue() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return CostMargin;
        }


        private void CustomShowFilterPopupForIncluded(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupForIncluded ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName == "IdRule")
                {
                    List<object> filterItems = new List<object>();

                    foreach (PLMArticlePrice item in IncludedPLMArticlePriceList)
                    {
                        string RuleValue = item.Rule.Value;

                        if (RuleValue == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == RuleValue))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = RuleValue;
                            customComboBoxItem.EditValue = item.IdRule;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "Country")
                {
                    if (e.Column.FieldName != "Country")
                    {
                        return;
                    }

                    try
                    {
                        List<object> filterItems = new List<object>();

                        if (e.Column.FieldName == "Country")
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Blanks)",
                                EditValue = CriteriaOperator.Parse("IsNull([Country])")//[002] added
                            });

                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Non blanks)",
                                EditValue = CriteriaOperator.Parse("!IsNull([Country])")
                            });

                            foreach (var dataObject in IncludedPLMArticlePriceList)
                            {
                                if (dataObject.Country == null)
                                {
                                    continue;
                                }
                                else if (dataObject.Country != null)
                                {
                                    if (dataObject.Country.Contains("\n"))
                                    {
                                        string tempCountry = dataObject.Country;
                                        for (int index = 0; index < tempCountry.Length; index++)
                                        {
                                            string empCountry = tempCountry.Split('\n').First();

                                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empCountry))
                                            {
                                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                                customComboBoxItem.DisplayValue = empCountry;
                                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", empCountry));
                                                filterItems.Add(customComboBoxItem);
                                            }
                                            if (tempCountry.Contains("\n"))
                                                tempCountry = tempCountry.Remove(0, empCountry.Length + 1);
                                            else
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == IncludedPLMArticlePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = IncludedPLMArticlePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", IncludedPLMArticlePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
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
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForIncluded() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopupForIncluded()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CustomShowFilterPopupForNotIncluded(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupForNotIncluded ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName == "IdRule")
                {
                    List<object> filterItems = new List<object>();

                    foreach (PLMArticlePrice item in NotIncludedPLMArticlePriceList)
                    {
                        if (item.Rule == null)
                            item.Rule = new LookupValue();

                        string RuleValue = item.Rule.Value;

                        if (RuleValue == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == RuleValue))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = RuleValue;
                            customComboBoxItem.EditValue = item.IdRule;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "Country")
                {
                    if (e.Column.FieldName != "Country")
                    {
                        return;
                    }

                    try
                    {
                        List<object> filterItems = new List<object>();

                        if (e.Column.FieldName == "Country")
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Blanks)",
                                EditValue = CriteriaOperator.Parse("IsNull([Country])")//[002] added
                            });

                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Non blanks)",
                                EditValue = CriteriaOperator.Parse("!IsNull([Country])")
                            });

                            foreach (var dataObject in NotIncludedPLMArticlePriceList)
                            {
                                if (dataObject.Country == null)
                                {
                                    continue;
                                }
                                else if (dataObject.Country != null)
                                {
                                    if (dataObject.Country.Contains("\n"))
                                    {
                                        string tempCountry = dataObject.Country;
                                        for (int index = 0; index < tempCountry.Length; index++)
                                        {
                                            string empCountry = tempCountry.Split('\n').First();

                                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empCountry))
                                            {
                                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                                customComboBoxItem.DisplayValue = empCountry;
                                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", empCountry));
                                                filterItems.Add(customComboBoxItem);
                                            }
                                            if (tempCountry.Contains("\n"))
                                                tempCountry = tempCountry.Remove(0, empCountry.Length + 1);
                                            else
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == NotIncludedPLMArticlePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = NotIncludedPLMArticlePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", NotIncludedPLMArticlePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
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
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForNotIncluded() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopupForNotIncluded()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void GridControlLoadedAction(object obj)
        {
            try
            {
                //if (IsSwitch == true)
                //    return;

                GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                {
                    int visibleFalseColumn = 0;
                    GridControl gridControl = obj as GridControl;
                    TableView tableView = (TableView)gridControl.View;

                    gridControl.BeginInit();

                    if (File.Exists(PCMPriceListIncludedGridSetting))
                    {
                        gridControl.RestoreLayoutFromXml(PCMPriceListIncludedGridSetting);
                    }

                    //This code for save grid layout.
                    gridControl.SaveLayoutToXml(PCMPriceListIncludedGridSetting);

                    //foreach (BandItem band in ((Emdep.Geos.UI.Helper.MyGridControl)gridControl).BandsSource)
                    //{
                    //    if (!band.Visible)
                    //    {
                    //        visibleFalseColumn++;
                    //    }
                    //}
                    //var list = gridControl.Columns.Where(a => Convert.ToString(((DevExpress.Xpf.Grid.BaseColumn)(a.Parent)).Header) == "Sale Price").ToList().OrderBy(x => x.VisibleIndex).ToList();

                    //if (list.Count > 0)
                    //{
                    //    if (((Currency)SelectedCurrency).Name != Convert.ToString(list.FirstOrDefault().Header))
                    //    {
                    //        int commonindex = list.FirstOrDefault(a => Convert.ToString(a.Header) == ((Currency)SelectedCurrency).Name).VisibleIndex;
                    //        int firstindex = list.FirstOrDefault().VisibleIndex;

                    //        list.Where(a => a.VisibleIndex == firstindex).ToList().ForEach(a => { a.VisibleIndex = commonindex; });
                    //        list.Where(a => Convert.ToString(a.Header) == ((Currency)SelectedCurrency).Name).ToList().ForEach(a => { a.VisibleIndex = firstindex; });
                    //    }
                    //}
                    //foreach (GridColumn column in gridControl.Columns)
                    //{
                    //    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    //    if (descriptor != null)
                    //    {
                    //        descriptor.AddValueChanged(column, VisibleChanged);
                    //    }

                    //    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    //    if (descriptorColumnPosition != null)
                    //    {
                    //        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    //    }

                    //    if (!column.Visible)
                    //    {
                    //        visibleFalseColumn++;
                    //    }
                    //}

                    if (IsFirstTimeLoad == false)
                    {
                        if (visibleFalseColumn > 0)
                        {
                            IsColumnChooserVisible = true;
                        }
                        else
                        {
                            IsColumnChooserVisible = false;
                        }
                        //IsArticleColumnChooserVisible = false;
                    }
                    IsFirstTimeLoad = false;
                    gridControl.EndInit();
                    tableView.SearchString = null;
                    tableView.ShowGroupPanel = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on GridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void GridControlUnloadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridControlUnloadedAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                if (gridControl.Columns.Count() > 0)
                {
                    TableView tableView = (TableView)gridControl.View;
                    tableView.SearchString = string.Empty;
                    if (gridControl.GroupCount > 0)
                        gridControl.ClearGrouping();
                    gridControl.ClearSorting();
                    gridControl.FilterString = null;
                    gridControl.SaveLayoutToXml(PCMPriceListIncludedGridSetting);
                }
                else
                {
                    IsFirstTimeLoad = true;
                    //IsSwitch = true;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GridControlUnloadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on GridControlUnloadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[30.11.2022][sshegaonkar][GEOS2-2718]
        private void ExpandAndCollapseModuleCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsModuleExpand)
            {
                t.ExpandAllNodes();
                IsModuleExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsModuleExpand = true;
            }
        }

        private void ExpandAndCollapseArticleCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsArticleExpand)
            {
                t.ExpandAllNodes();
                IsArticleExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsArticleExpand = true;
            }
        }


        //private void EditLinkedArticleAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()..."), category: Category.Info, priority: Priority.Low);



        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        Int64 IdArticle = 0;
        //        TreeListView detailView = (TreeListView)obj;
        //        if (((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows[0] != null && ((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows.Count > 0)
        //        {
        //            IdArticle = ((Emdep.Geos.Data.Common.ArticleDecomposition)(((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows[0])).IdArticle;
        //            SelectedItemArticleDecomposition = ArticleDecompositionList.Where(i => i.IdArticle == IdArticle).FirstOrDefault();
        //            if (SelectedItemArticleDecomposition != null)
        //            {
        //                Articles articles = new Articles();
        //                articles.IdArticle = Convert.ToUInt32(SelectedItemArticleDecomposition.IdArticle);
        //                if (SelectedItemArticleDecomposition.PCMArticle != null)
        //                {
        //                    EditPCMArticleView editPCMArticleView = new EditPCMArticleView();
        //                    EditPCMArticleViewModel editPCMArticleViewModel = new EditPCMArticleViewModel();
        //                    EventHandler handle = delegate { editPCMArticleView.Close(); };
        //                    editPCMArticleViewModel.RequestClose += handle;
        //                    editPCMArticleViewModel.IsNew = false;

        //                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //                    editPCMArticleViewModel.EditInit(articles);
        //                    editPCMArticleView.DataContext = editPCMArticleViewModel;
        //                    var ownerInfo = (detailView as FrameworkElement);
        //                    editPCMArticleView.Owner = Window.GetWindow(ownerInfo);
        //                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //                    editPCMArticleView.ShowDialog();
        //                }
        //                else
        //                {
        //                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //                    CustomMessageBox.Show(Application.Current.Resources["PCM_NotPCMArticle"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
        //                }
        //            }

        //        }
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }





        //        GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        private void EditLinkedArticleAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()..."), category: Category.Info, priority: Priority.Low);



                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                Int64 IdArticle = 0;
                TableView detailView = obj as TableView;
                if (((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows[0] != null && ((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows.Count > 0)
                {
                    IdArticle = ((Emdep.Geos.Data.Common.LinkedArticle)(((DevExpress.Xpf.Grid.DataViewBase)obj).SelectedRows[0])).IdArticle;
                    SelectedItemLinkedArticle = LinkedArticleList.Where(i => i.IdArticle == IdArticle).FirstOrDefault();
                    if (SelectedItemLinkedArticle != null)
                    {
                        Articles articles = new Articles();
                        articles.IdArticle = Convert.ToUInt32(SelectedItemLinkedArticle.IdArticle);
                        if (SelectedItemLinkedArticle != null)
                        {
                            var item = LinkedArticleList
                                .FirstOrDefault(i => i.IdArticle == SelectedItemLinkedArticle.IdArticle);

                            if (item.PCMArticle != null)
                            {
                                SelectedItemLinkedArticle.PCMArticle.IdPCMArticle = item.PCMArticle.IdPCMArticle;
                            }
                        }
                        if (SelectedItemLinkedArticle.PCMArticle != null)
                        {
                            EditPCMArticleView editPCMArticleView = new EditPCMArticleView();
                            EditPCMArticleViewModel editPCMArticleViewModel = new EditPCMArticleViewModel();
                            EventHandler handle = delegate { editPCMArticleView.Close(); };
                            editPCMArticleViewModel.RequestClose += handle;
                            editPCMArticleViewModel.IsNew = false;

                            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                            editPCMArticleViewModel.EditInit(articles);
                            editPCMArticleView.DataContext = editPCMArticleViewModel;
                            var ownerInfo = (detailView as FrameworkElement);
                            editPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            editPCMArticleView.ShowDialog();
                        }
                        else
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(Application.Current.Resources["PCM_NotPCMArticle"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                        }
                    }

                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
