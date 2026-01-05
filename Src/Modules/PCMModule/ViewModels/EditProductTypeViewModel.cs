using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
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
using Microsoft.Win32;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Data.Common.PLM;
using System.Data;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Data.Filtering;
using Emdep.Geos.Modules.PLM.CommonClasses;
using System.Threading.Tasks;
using System.Threading;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class EditProductTypeViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        //[GEOS2-4098][rdixit][28.12.2022][001] 
        #region Service
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());         
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());        
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());        
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //IPCMService PCMService = new PCMServiceController("localhost:6699");
        //IPLMService PLMService = new PLMServiceController("localhost:6699");

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
        bool isSparePartExpand;
        bool isArticleExpand;
        bool isModuleExpand;
        bool isOptionExpand;
        bool isDetectionExpand;
        private ObservableCollection<Template> templatesMenuList;
        private Template selectedTemplate;

        private ObservableCollection<Options> optionsMenulist;
        private ObservableCollection<Options> options;
        private ObservableCollection<Options> selectedOptionsType;
        private Options selectedOption;
        private Options selectedOptionForCurrentModule;

        private ObservableCollection<Ways> waysMenulist;
        private ObservableCollection<Ways> ways;
        private List<Object> selectedWaysType;
        private Ways selectedWay;

        private ObservableCollection<Detections> detectionsMenulist;
        private ObservableCollection<Detections> detections;
        private List<Object> selectedDetectionsType;
        private Detections selectedDetection;
        private Detections selectedDetectionForCurrentModule;

        private ObservableCollection<SpareParts> sparePartsMenulist;
        private ObservableCollection<SpareParts> spareParts;
        private List<Object> selectedSparePartsType;
        private SpareParts selectedSparePart;
        private SpareParts selectedSparePartForCurrentModule;

        private ObservableCollection<ConnectorFamilies> familyMenulist;
        private ObservableCollection<ConnectorFamilies> families;
        private ConnectorFamilies selectedFamily;
        private ConnectorFamilies selectedFamilyAutoHide;
        private ObservableCollection<Language> languages;
        private Language languageSelected;

        private ObservableCollection<ProductTypeLogEntry> productTypesChangeLogList;
        private ObservableCollection<ProductTypeLogEntry> changeLogList;
        private ProductTypeLogEntry selectedProductTypesChangeLog;

        private ObservableCollection<ProductTypeImage> imagesList;
        private ObservableCollection<ProductTypeImage> fourRecordsProductTypeImagesList;
        private ProductTypeImage selectedImage;
        private ProductTypeImage selectedDefaultImage;
        private ProductTypeImage selectedContentTemplateImage;
        private int selectedContentTemplateImageIndex;

        private ObservableCollection<ProductTypeAttachedDoc> productTypeFilesList;
        private ObservableCollection<ProductTypeAttachedDoc> fourRecordsProductTypeFilesList;
        private ProductTypeAttachedDoc selectedProductTypeFile;

        private ObservableCollection<ProductTypeAttachedLink> productTypeLinksList;
        private ObservableCollection<ProductTypeAttachedLink> fourRecordsProductTypeLinksList;
        private ProductTypeAttachedLink selectedProductTypeLink;

        private List<LookupValue> statusList;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;

        private DateTime? selectedCreated;
        private DateTime? lastUpdate;

        private string reference;

        private bool isCheckedCopyDescription;

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

        private ulong idCPType;

        private ulong selectedIndexForRadioButton = 0;
        private ObservableCollection<DefaultWayType> defaultWayTypeList;
        private Ways selectedDefaultWayType;

        private Photo selectedItem;
        private ProductTypes clonedProductType;
        private uint modifyBy;
        private int selectedImageIndex;
        private string abbrivation;
        private DateTime modifiedIn;

        private ProductTypes productTypesDetails;
        private ProductTypes updatedProductTypesDetails;
        private ProductTypes productTypeItem;

        private bool isEnabled;

        private int options_count;
        private int options_Group_count;
        private int detections_count;
        private int detections_group_count;

        private int spareparts_count;
        private int spareparts_group_count;

        private double dialogHeight;
        private double dialogWidth;
        private Int16 selectedBoxIndex;
        private ProductTypeImage maximizedElement;

        private bool isSaveChanges;

        private char? _SelectedLetter;
        string filterStringForName;
        private ObservableCollection<char> _Letters;

        private ObservableCollection<RegionsByCustomer> customersMenuList;
        private ObservableCollection<RegionsByCustomer> selectedCustomersList;
        private ObservableCollection<FourRegionsWithCustomerCount> fourRecordsCustomersList;
        private int selectedRegionCount;
        MaximizedElementPosition maximizedElementPosition;

        private ObservableCollection<ProductTypesTemplate> moduleMenuList;
        private ProductTypesTemplate selectedModule;

        private ObservableCollection<ProductTypeCompatibility> mandatoryList;
        private ProductTypeCompatibility selectedMandatory;

        private ObservableCollection<ProductTypeCompatibility> suggestedList;
        private ProductTypeCompatibility selectedSuggested;

        private ObservableCollection<ProductTypeCompatibility> incompatibleList;
        private ProductTypeCompatibility selectedIncompatible;

        private int compatibilityCount;

        private ObservableCollection<PCMArticleCategory> articleMenuList;
        private PCMArticleCategory selectedArticle;

        private List<LookupValue> relationShipList;
        private LookupValue selectedRelationShip;

        private string compatibilityError;
        private string error = string.Empty;
        private string informationError;
        private Visibility isExportVisible;
        private bool? isCheckedCustomer;
        private bool isCellChecked;

        private bool isNew = false;
        private string duplicateCode;
        private string productTypeOldCode;
        private string productTypeOldName;
        private Visibility isDuplicateModulesButtonVisible;
        private bool isDuplicateModulesButtonEnabled;
        public bool isDuplicateClicked = false;
        private string visible;
        private bool isReadOnlyField;
        private bool allowDragDrop;
        public bool isEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][14/02/2023]
        private string isImageScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][10/03/2023]
        private string isAttachmentScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][13/03/2023]
        private string isLinkScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][13/03/2023]

        private bool isDetectionDrag = false;//[GEOS2-4221][Sudhir.Jangra]
        private bool isOptionDrag = false;//[GEOS2-4221][Sudhir.Jangra]
        private bool isSparePartDrag = false;//[GEOS2-4221][Sudhir.Jangra]

        private string windowHeader;//[Sudhir.Jangra][GEOS2-4733][22/08/2023]
        private Visibility isDefaultWayTypeVisible;//[Sudhir.Jangra][GEOS2-4733][22/08/2023]
        private Visibility isWayVisible;//[Sudhir.Jangra][GEOS2-4733]
        private bool isModuleView;//[Sudhir.Jangra][GEOS2-4797]

        private string oldName;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_en;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_es;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_fr;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_pt;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_ro;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_ru;//[Sudhir.Jangra][GEOS2-4905]
        private string oldName_zh;//[Sudhir.Jangra][GEOS2-4905]

        private string oldDescription;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_en;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_es;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_fr;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_pt;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_ro;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_ru;//[Sudhir.Jangra][GEOS2-4905]
        private string oldDescription_zh;//[Sudhir.Jangra][GEOS2-4905]



        private ObservableCollection<ProductTypeLogEntry> commentList;//[Sudhir.Jangra][GEOS2-4935]
        private string commentText;//[Sudhir.Jangra][GEOS2-4935]
        private DateTime? commentDateTimeText;//[Sudhir.Jangra][GEOS2-4935]
        private string commentFullNameText;//[Sudhir.Jangra][GEOS2-4935]
        private byte[] userProfileImageByte;//[Sudhir.Jangra][GEOS2-4935]
        private List<ProductTypeLogEntry> addCommentsList;//[Sudhir.jangra][GEOS2-4935]
        private ProductTypeLogEntry selectedComment;//[Sudhir.jangra][GEOSS2-4935]
        private ObservableCollection<ProductTypeLogEntry> deleteCommentsList;//[Sudhir.Jangra][GEOS2-4935]
        private bool isDeleted;//[Sudhir.Jangra][GEOS2-4935]
        private List<ProductTypeLogEntry> updatedCommentList;//[Sudhir.Jangra][GEOS2-4935]

        #region GEOS2-3276     
        //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
        private ObservableCollection<Group> groupList;
        private Group selectedGroup;
        private ObservableCollection<Region> regionList;
        private List<object> selectedRegion;
        private ObservableCollection<Country> countryList;
        private List<object> selectedCountry;
        private ObservableCollection<Site> plantList;
        private List<object> selectedPlant;
        private int isFilterStatus;
        private bool isRetrive;
        private List<CPLCustomer> pcmCustomerList;
        private ObservableCollection<CPLCustomer> includedCustomerList;
        private ObservableCollection<CPLCustomer> notIncludedCustomerList;
        private int group;
        private int region;
        private int country;
        private int plant;
        private ObservableCollection<object> selectedIncludedCustomer;
        private ObservableCollection<object> selectedNotIncludedCustomer;
        //private Visibility isExportVisible;
        public List<CPLCustomer> PCMCustomerList
        {
            get
            {
                return pcmCustomerList;
            }
            set
            {
                pcmCustomerList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("PCMCustomerList"));
                //if(pcmCustomerList!=null && pcmCustomerList.Count>0)
                if (pcmCustomerList != null)
                {
                    IncludedCustomerList = new ObservableCollection<CPLCustomer>(pcmCustomerList.Where(i => i.IsIncluded == 1).ToList());
                    NotIncludedCustomerList = new ObservableCollection<CPLCustomer>(pcmCustomerList.Where(i => i.IsIncluded == 0).ToList());
                }
            }
        }


        public ObservableCollection<object> SelectedIncludedCustomer
        {
            get
            {
                return selectedIncludedCustomer;
            }
            set
            {
                selectedIncludedCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncludedCustomer"));
                IsEnabledCancelButton = true;
            }
        }

        public ObservableCollection<object> SelectedNotIncludedCustomer
        {
            get
            {
                return selectedNotIncludedCustomer;
            }
            set
            {
                selectedNotIncludedCustomer = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNotIncludedCustomer"));
                IsEnabledCancelButton = true;
            }
        }

        public ObservableCollection<CPLCustomer> IncludedCustomerList
        {
            get
            {
                return includedCustomerList;
            }
            set
            {
                includedCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedCustomerList"));
            }
        }

        public ObservableCollection<CPLCustomer> NotIncludedCustomerList
        {
            get
            {
                return notIncludedCustomerList;
            }
            set
            {
                notIncludedCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotIncludedCustomerList"));
            }
        }
        public ObservableCollection<Group> GroupList
        {
            get
            {
                return groupList;
            }

            set
            {
                groupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList"));
            }
        }

        public Group SelectedGroup
        {
            get
            {
                return selectedGroup;
            }

            set
            {
                selectedGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup"));
            }
        }

        public ObservableCollection<Region> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));
            }
        }

        public List<object> SelectedRegion
        {
            get
            {
                return selectedRegion;
            }

            set
            {
                selectedRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion"));
            }
        }

        public ObservableCollection<Country> CountryList
        {
            get
            {
                return countryList;
            }

            set
            {
                countryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryList"));
            }
        }

        public List<object> SelectedCountry
        {
            get
            {
                return selectedCountry;
            }

            set
            {
                selectedCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountry"));
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

        public int IsFilterStatus
        {
            get
            {
                return isFilterStatus;
            }

            set
            {
                isFilterStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFilterStatus"));
            }
        }
        public bool IsRetrive
        {
            get
            {
                return isRetrive;
            }

            set
            {
                isRetrive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRetrive"));
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
            }
        }
        public string Groups { get; set; }
        public string Regions { get; set; }
        public string Countries { get; set; }
        public string Plants { get; set; }
        //public Visibility IsExportVisible
        //{
        //    get
        //    {
        //        return isExportVisible;
        //    }
        //    set
        //    {
        //        isExportVisible = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsExportVisible"));
        //    }
        //}
        #endregion

        #endregion

        #region Properties
        //[29.11.2022][rdixit][GEOS2-2718]
        public bool IsSparePartExpand
        {
            get
            {
                return isSparePartExpand;
            }

            set
            {
                isSparePartExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSparePartExpand"));
            }
        }
        //[29.11.2022][rdixit][GEOS2-2718]
        public bool IsOptionExpand
        {
            get
            {
                return isOptionExpand;
            }

            set
            {
                isOptionExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOptionExpand"));
            }
        }
        public bool IsDetectionExpand
        {
            get
            {
                return isDetectionExpand;
            }

            set
            {
                isDetectionExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetectionExpand"));
            }
        }

        //[29.11.2022][sshegaonkar][GEOS2-2718]
        public bool IsModuleExpand
        {
            get
            {
                return isModuleExpand;
            }

            set
            {
                isModuleExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModuleExpand"));
            }
        }
        //[29.11.2022][sshegaonkar][GEOS2-2718]
        public bool IsArticleExpand
        {
            get
            {
                return isArticleExpand;
            }

            set
            {
                isArticleExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleExpand"));
            }
        }

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public ProductTypes UpdateProductTypes { get; set; }

        public ObservableCollection<Template> TemplatesMenuList
        {
            get { return templatesMenuList; }
            set
            {
                templatesMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplatesMenuList"));
            }
        }

        public Template SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTemplate"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]

            }
        }

        public ObservableCollection<Options> OptionsMenulist
        {
            get { return optionsMenulist; }
            set
            {
                optionsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionsMenulist"));
            }
        }

        public ObservableCollection<Options> Options
        {
            get { return options; }
            set
            {
                options = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Options"));
            }
        }

        public virtual ObservableCollection<Options> SelectedOptionsType
        {
            get
            {
                return selectedOptionsType;
            }
            set
            {
                selectedOptionsType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionsType"));
            }
        }

        public Options SelectedOption
        {
            get
            {
                return selectedOption;
            }

            set
            {
                selectedOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOption"));
            }
        }

        public Options SelectedOptionForCurrentModule
        {
            get
            {
                return selectedOptionForCurrentModule;
            }

            set
            {
                selectedOptionForCurrentModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionForCurrentModule"));
            }
        }

        public ObservableCollection<Ways> WaysMenulist
        {
            get { return waysMenulist; }
            set
            {
                waysMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WaysMenulist"));
            }
        }

        public ObservableCollection<Ways> Ways
        {
            get { return ways; }
            set
            {
                ways = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Ways"));
            }
        }


        public virtual List<object> SelectedWaysType
        {
            get
            {
                return selectedWaysType;
            }
            set
            {
                selectedWaysType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWaysType"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]

            }
        }

        public Ways SelectedWay
        {
            get
            {
                return selectedWay;
            }

            set
            {
                selectedWay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWay"));
            }
        }

        public ObservableCollection<Detections> DetectionsMenulist
        {
            get { return detectionsMenulist; }
            set
            {
                detectionsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionsMenulist"));
            }
        }

        public ObservableCollection<Detections> Detections
        {
            get { return detections; }
            set
            {
                detections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Detections"));
            }
        }

        public List<Object> SelectedDetectionsType
        {
            get
            {
                return selectedDetectionsType;
            }
            set
            {
                selectedDetectionsType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetectionsType"));
            }
        }

        public Detections SelectedDetection
        {
            get
            {
                return selectedDetection;
            }

            set
            {
                selectedDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetection"));
            }
        }

        public Detections SelectedDetectionForCurrentModule
        {
            get
            {
                return selectedDetectionForCurrentModule;
            }

            set
            {
                selectedDetectionForCurrentModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetectionForCurrentModule"));
            }
        }

        public ObservableCollection<SpareParts> SparePartsMenulist
        {
            get { return sparePartsMenulist; }
            set
            {
                sparePartsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SparePartsMenulist"));
            }
        }

        public ObservableCollection<SpareParts> SpareParts
        {
            get { return spareParts; }
            set
            {
                spareParts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpareParts"));
            }
        }

        public List<object> SelectedSparePartsType
        {
            get
            {
                return selectedSparePartsType;
            }
            set
            {
                selectedSparePartsType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSparePartsType"));
            }
        }

        public SpareParts SelectedSparePart
        {
            get
            {
                return selectedSparePart;
            }

            set
            {
                selectedSparePart = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSparePart"));
            }
        }

        public SpareParts SelectedSparePartForCurrentModule
        {
            get
            {
                return selectedSparePartForCurrentModule;
            }

            set
            {
                selectedSparePartForCurrentModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSparePartForCurrentModule"));
            }
        }

        public ObservableCollection<ConnectorFamilies> FamilyMenulist
        {
            get { return familyMenulist; }
            set
            {
                familyMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FamilyMenulist"));
            }
        }

        public ObservableCollection<ConnectorFamilies> Families
        {
            get { return families; }
            set
            {
                families = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Families"));
            }
        }

        public ConnectorFamilies SelectedFamilyAutoHide
        {
            get
            {
                return selectedFamilyAutoHide;
            }
            set
            {
                selectedFamilyAutoHide = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamilyAutoHide"));
            }
        }

        public ConnectorFamilies SelectedFamily
        {
            get
            {
                return selectedFamily;
            }
            set
            {
                selectedFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamily"));
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

        public ObservableCollection<ProductTypeLogEntry> ProductTypesChangeLogList
        {
            get
            {
                return productTypesChangeLogList;
            }

            set
            {
                productTypesChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesChangeLogList"));
            }
        }

        public ObservableCollection<ProductTypeLogEntry> ChangeLogList
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

        public ProductTypeLogEntry SelectedProductTypesChangeLog
        {
            get
            {
                return selectedProductTypesChangeLog;
            }

            set
            {
                selectedProductTypesChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductTypesChangeLog"));
            }
        }

        public ObservableCollection<ProductTypeImage> ImagesList
        {
            get { return imagesList; }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }

        public ObservableCollection<ProductTypeImage> FourRecordsProductTypeImagesList
        {
            get
            {
                return fourRecordsProductTypeImagesList;
            }

            set
            {
                fourRecordsProductTypeImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsProductTypeImagesList"));
            }
        }

        public ProductTypeImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
            }
        }

        public ProductTypeImage SelectedDefaultImage
        {
            get
            {
                return selectedDefaultImage;
            }

            set
            {
                selectedDefaultImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDefaultImage"));
            }
        }

        public ProductTypeImage SelectedContentTemplateImage
        {
            get
            {
                return selectedContentTemplateImage;
            }

            set
            {
                selectedContentTemplateImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContentTemplateImage"));

            }
        }

        public int SelectedContentTemplateImageIndex
        {
            get
            {
                return selectedContentTemplateImageIndex;
            }

            set
            {
                selectedContentTemplateImageIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContentTemplateImageIndex"));
            }
        }

        public ObservableCollection<ProductTypeAttachedDoc> ProductTypeFilesList
        {
            get
            {
                return productTypeFilesList;
            }
            set
            {
                productTypeFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeFilesList"));
            }
        }

        public ObservableCollection<ProductTypeAttachedDoc> FourRecordsProductTypeFilesList
        {
            get
            {
                return fourRecordsProductTypeFilesList;
            }

            set
            {
                fourRecordsProductTypeFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsProductTypeFilesList"));
            }
        }

        public ProductTypeAttachedDoc SelectedProductTypeFile
        {
            get
            {
                return selectedProductTypeFile;
            }
            set
            {
                selectedProductTypeFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductTypeFile"));
            }
        }

        public ObservableCollection<ProductTypeAttachedLink> ProductTypeLinksList
        {
            get
            {
                return productTypeLinksList;
            }
            set
            {
                productTypeLinksList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeLinksList"));
            }
        }

        public ObservableCollection<ProductTypeAttachedLink> FourRecordsProductTypeLinksList
        {
            get
            {
                return fourRecordsProductTypeLinksList;
            }

            set
            {
                fourRecordsProductTypeLinksList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsProductTypeLinksList"));
            }
        }

        public ProductTypeAttachedLink SelectedProductTypeLink
        {
            get
            {
                return selectedProductTypeLink;
            }
            set
            {
                selectedProductTypeLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductTypeLink"));
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

        public DateTime? SelectedCreated
        {
            get { return selectedCreated; }
            set
            {
                selectedCreated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCreated"));
            }
        }

        public DateTime? LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdate"));
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public bool IsCheckedCopyDescription
        {
            get
            {
                return isCheckedCopyDescription;
            }
            set
            {
                isCheckedCopyDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyDescription"));
                UncheckedCopyDescription(null);
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ulong IdCPType
        {
            get
            {
                return idCPType;
            }
            set
            {
                idCPType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCPType"));
            }
        }

        public ulong SelectedIndexForRadioButton
        {
            get
            {
                return selectedIndexForRadioButton;
            }
            set
            {
                selectedIndexForRadioButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForRadioButton"));
            }
        }

        public ObservableCollection<DefaultWayType> DefaultWayTypeList
        {
            get
            {
                return defaultWayTypeList;
            }
            set
            {
                defaultWayTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultWayTypeList"));
            }
        }
        //[cpatil][GEOS2-3728][20-05-2022]
        public Ways SelectedDefaultWayType
        {
            get
            {
                return selectedDefaultWayType;
            }
            set
            {
                selectedDefaultWayType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDefaultWayType"));

                //  Set IsDefaultWay in All ways
                if (value != null)
                {
                    foreach (var item in Ways)
                    {

                        if (SelectedDefaultWayType != null && item.IdWays == SelectedDefaultWayType.IdWays)
                        {
                            item.IsDefaultWay = 1;
                        }
                        else
                        {
                            item.IsDefaultWay = 0;
                        }

                    }
                    IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                }
            }
        }

        public Photo SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public ProductTypes ClonedProductType
        {
            get
            {
                return clonedProductType;
            }
            set
            {
                clonedProductType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedProductType"));
            }
        }

        public uint ModifyBy
        {
            get { return modifyBy; }
            set
            {
                modifyBy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifyBy"));
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
            }
        }

        public string Abbrivation
        {
            get
            {
                return abbrivation;
            }

            set
            {
                abbrivation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Abbrivation"));
                IsEnabledCancelButton = true;//
            }
        }

        public DateTime ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedIn"));
            }
        }

        public ProductTypes ProductTypesDetails
        {
            get
            {
                return productTypesDetails;
            }

            set
            {
                productTypesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesDetails"));
            }
        }

        public ProductTypes UpdatedProductTypesDetails
        {
            get
            {
                return updatedProductTypesDetails;
            }

            set
            {
                updatedProductTypesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedProductTypesDetails"));
            }
        }

        public ProductTypes ProductTypeItem
        {
            get
            {
                return productTypeItem;
            }

            set
            {
                productTypeItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeItem"));
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

        public int Options_count
        {
            get
            {
                return options_count;
            }

            set
            {
                options_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Options_count"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int Options_Group_count
        {
            get
            {
                return options_Group_count;
            }

            set
            {
                options_Group_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Options_Group_count"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int Detections_count
        {
            get
            {
                return detections_count;
            }

            set
            {
                detections_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Detections_count"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int Detections_group_count
        {
            get
            {
                return detections_group_count;
            }

            set
            {
                detections_group_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Detections_group_count"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int SpareParts_count
        {
            get
            {
                return spareparts_count;
            }

            set
            {
                spareparts_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpareParts_count"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int SpareParts_group_count
        {
            get
            {
                return spareparts_group_count;
            }

            set
            {
                spareparts_group_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpareParts_group_count"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public short SelectedBoxIndex
        {
            get { return selectedBoxIndex; }
            set
            {
                selectedBoxIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBoxIndex"));
            }
        }

        public ProductTypeImage MaximizedElement
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

        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }

        public char? SelectedLetter
        {
            get { return this._SelectedLetter; }
            set
            {
                if (this._SelectedLetter != value)
                {
                    this._SelectedLetter = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedLetter"));

                    if (this._SelectedLetter == null)
                    {
                        FilterStringForName = string.Empty;
                    }
                    else
                    {
                        FilterStringForName = "StartsWith([GroupName], '" + this.SelectedLetter + "')";
                    }
                }
            }
        }

        public string FilterStringForName
        {
            get { return filterStringForName; }
            set
            {
                filterStringForName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStringForName"));
            }
        }

        public ObservableCollection<char> Letters
        {
            get
            {
                if (this._Letters == null)
                {
                    this._Letters = new ObservableCollection<char>();
                    char c = 'A';
                    while (c <= 'Z')
                    {
                        this._Letters.Add(c);
                        c++;
                    }
                }
                return this._Letters;
            }
            set
            {
                _Letters = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Letters"));
            }
        }

        public ObservableCollection<RegionsByCustomer> CustomersMenuList
        {
            get
            {
                return customersMenuList;
            }

            set
            {
                customersMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomersMenuList"));
            }
        }
        private bool? _allSelected = true;
        public bool? AllSelected
        {
            get { return _allSelected; }
            set
            {
                _allSelected = value;

                // MyProperty.ForEach(x => x.IsChecked = value);

                //OnPropertyChange("AllSelected");
            }
        }



        public ObservableCollection<RegionsByCustomer> SelectedCustomersList
        {
            get
            {
                return selectedCustomersList;
            }

            set
            {
                selectedCustomersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomersList"));
            }
        }

        public ObservableCollection<FourRegionsWithCustomerCount> FourRecordsCustomersList
        {
            get { return fourRecordsCustomersList; }
            set
            {
                fourRecordsCustomersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsCustomersList"));
            }
        }

        public int SelectedRegionCount
        {
            get
            {
                return selectedRegionCount;
            }

            set
            {
                selectedRegionCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsCustomersList"));
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
                //IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<ProductTypeCompatibility> MandatoryList
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

        public ProductTypeCompatibility SelectedMandatory
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

        public ObservableCollection<ProductTypeCompatibility> SuggestedList
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

        public ProductTypeCompatibility SelectedSuggested
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

        public ObservableCollection<ProductTypeCompatibility> IncompatibleList
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

        public ProductTypeCompatibility SelectedIncompatible
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

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }

        public Visibility IsExportVisible
        {
            get
            {
                return isExportVisible;
            }

            set
            {
                isExportVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExportVisible"));
            }
        }

        public bool? IsCheckedCustomer
        {
            get
            {
                return isCheckedCustomer;
            }

            set
            {
                isCheckedCustomer = value;
                if (IsCellChecked == false)
                {


                    if (isCheckedCustomer == true)
                    {
                        CustomersMenuList.ToList().ForEach(a => a.IsChecked = true);
                        CustomerHeaderClickCommandAction(null);
                    }
                    else
                    {
                        CustomersMenuList.ToList().ForEach(a => a.IsChecked = false);
                        CustomerHeaderClickCommandAction(null);
                    }
                }
                else
                {
                    IsCellChecked = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCustomer"));
            }
        }
        public bool IsCellChecked
        {
            get
            {
                return isCellChecked;
            }

            set
            {
                isCellChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCellChecked"));
            }
        }

        public ProductTypes DuplicateProductTypesDetails { get; set; }

        public ProductTypes NewProductTypesDetails { get; set; }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }


        public Visibility IsDuplicateModulesButtonVisible
        {
            get
            {
                return isDuplicateModulesButtonVisible;
            }

            set
            {
                isDuplicateModulesButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateModulesButtonVisible"));
            }
        }

        public bool IsDuplicateModulesButtonEnabled
        {
            get
            {
                return isDuplicateModulesButtonEnabled;
            }

            set
            {
                isDuplicateModulesButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateModulesButtonEnabled"));
            }
        }

        public string ProductTypeOldCode
        {
            get
            {
                return productTypeOldCode;
            }

            set
            {
                productTypeOldCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeOldCode"));
            }
        }
        public string ProductTypeOldName
        {
            get
            {
                return productTypeOldName;
            }

            set
            {
                productTypeOldName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeOldName"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string DuplicateCode
        {
            get
            {
                return duplicateCode;
            }

            set
            {
                duplicateCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DuplicateCode"));
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
        public bool IsEnabledCancelButton
        {
            get { return isEnabledCancelButton; }
            set
            {
                isEnabledCancelButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancelButton"));
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
        //[Sudhir.Jangra][GEOS2-1960][13/03/2023]
        public string IsLinkScrollVisible
        {
            get { return isLinkScrollVisible; }
            set
            {
                isLinkScrollVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLinkScrollViisible"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4221]
        public bool IsDetectionDrag
        {
            get { return isDetectionDrag; }
            set
            {
                isDetectionDrag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetectionDrag"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4221]
        public bool IsOptionDrag
        {
            get { return isOptionDrag; }
            set
            {
                isOptionDrag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOptionDrag"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4221]
        public bool IsSparePartDrag
        {
            get { return isSparePartDrag; }
            set
            {
                isSparePartDrag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSparePartDrag"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4733][28/08/2023]
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4733][28/08/2023]
        public Visibility IsDefaultWayTypeVisible
        {
            get { return isDefaultWayTypeVisible; }
            set
            {
                isDefaultWayTypeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDefaultWayTypeVisible"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4733]
        public Visibility IsWayVisible
        {
            get { return isWayVisible; }
            set
            {
                isWayVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWayVisible"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4797]
        public bool IsModuleView
        {
            get { return isModuleView; }
            set
            {
                isModuleView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModuleView"));
            }
        }


        //[Sudhir.jangra][GEOS2-4935]
        public ObservableCollection<ProductTypeLogEntry> CommentsList
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

        public List<ProductTypeLogEntry> AddCommentsList
        {
            get { return addCommentsList; }
            set
            {
                addCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public ProductTypeLogEntry SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public ObservableCollection<ProductTypeLogEntry> DeleteCommentsList
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
        public List<ProductTypeLogEntry> UpdatedCommentsList
        {
            get { return updatedCommentList; }
            set
            {
                updatedCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }




        ObservableCollection<PLMModulePrice> includedPLMModulePriceList;
        public ObservableCollection<PLMModulePrice> IncludedPLMModulePriceList
        {
            get
            {
                return includedPLMModulePriceList;
            }

            set
            {
                includedPLMModulePriceList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IncludedPLMModulePriceList"));
            }
        }

        ObservableCollection<PLMModulePrice> notIncludedPLMModulePriceList;
        public ObservableCollection<PLMModulePrice> NotIncludedPLMModulePriceList
        {
            get
            {
                return notIncludedPLMModulePriceList;
            }

            set
            {
                notIncludedPLMModulePriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotIncludedPLMModulePriceList"));
            }
        }

        PLMModulePrice selectedIncludedPLMModulePrice;
        public PLMModulePrice SelectedIncludedPLMModulePrice
        {
            get
            {
                return selectedIncludedPLMModulePrice;
            }

            set
            {
                selectedIncludedPLMModulePrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncludedPLMModulePrice"));
            }
        }

        PLMModulePrice selectedNotIncludedPLMModulePrice;
        public PLMModulePrice SelectedNotIncludedPLMModulePrice
        {
            get
            {
                return selectedNotIncludedPLMModulePrice;
            }

            set
            {
                selectedNotIncludedPLMModulePrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNotIncludedPLMModulePrice"));
            }
        }
        ProductTypes clonedProductTypes;
        public ProductTypes ClonedProductTypes
        {
            get
            {
                return clonedProductTypes;
            }

            set
            {
                clonedProductTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedProductTypes"));
            }
        }
        string selectedCurrencySymbol;
        public string SelectedCurrencySymbol
        {
            get { return selectedCurrencySymbol; }
            set
            {
                selectedCurrencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrencySymbol"));

            }
        }

        Currency selectedCurrency;
        public Currency SelectedCurrency
        {
            get { return selectedCurrency; }
            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }

        string includedFirstActiveName;
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


        ImageSource includedFirstActiveCurrencyIconImage;
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
            }
        }

        private double? includedFirstActiveSellPrice;
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

            }
        }

        Int64 includedActiveCount;
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
        string currencySymbol;
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
            }
        }

        bool isFirstTimeLoad;
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


        bool isModuleColumnChooserVisible;
        public bool IsModuleColumnChooserVisible
        {
            get { return isModuleColumnChooserVisible; }
            set
            {
                isModuleColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModuleColumnChooserVisible"));
            }
        }
        public string PCMModulePriceListIncludedGridSetting = GeosApplication.Instance.UserSettingFolderName + "PCMModulePriceListIncludedGridSetting.Xml";
        public ProductTypes UpdatedItem { get; set; }
        List<LookupValue> logicList;
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
        bool isBPLMessageShow;
        public bool IsBPLMessageShow
        {
            get { return isBPLMessageShow; }
            set
            {

                isBPLMessageShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBPLMessageShow"));
            }
        }

        bool isCPLMessageShow;
        public bool IsCPLMessageShow
        {
            get { return isCPLMessageShow; }
            set
            {

                isCPLMessageShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLMessageShow"));
            }
        }
        bool isBPLCalculateRuleValue;
        public bool IsBPLCalculateRuleValue
        {
            get { return isBPLCalculateRuleValue; }
            set
            {

                isBPLCalculateRuleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBPLCalculateRuleValue"));
            }
        }

        bool isCPLCalculateRuleValue;
        public bool IsCPLCalculateRuleValue
        {
            get { return isCPLCalculateRuleValue; }
            set
            {

                isCPLCalculateRuleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLCalculateRuleValue"));
            }
        }


        #endregion

        #region ICommands

        public ICommand CommandOnDragRecordOverOptions { get; set; }
        public ICommand CommandOnDragRecordOverOptionsGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropOptions { get; set; }
        public ICommand CommandTreeListViewDropRecordOption { get; set; }

        public ICommand CommandOnDragRecordOverWay { get; set; }
        public ICommand CommandOnDragRecordOverWayGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropWay { get; set; }

        public ICommand CommandOnDragRecordOverDetections { get; set; }
        public ICommand CommandOnDragRecordOverDetectionsGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropDetections { get; set; }
        public ICommand CommandTreeListViewDropRecordDetection { get; set; }

        public ICommand CommandOnDragRecordOverSpareParts { get; set; }
        public ICommand CommandOnDragRecordOverSparePartsGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropSpareParts { get; set; }
        public ICommand CommandTreeListViewDropRecordSpareParts { get; set; }

        public ICommand CommandOnDragRecordOverFamily { get; set; }
        public ICommand CommandOnDragRecordOverFamilyGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropFamily { get; set; }
        public ICommand CommandOnDragRecordOverCustomers { get; set; }


        public ICommand OptionsCancelCommand { get; set; }
        public ICommand WayCancelCommand { get; set; }
        public ICommand DetectionsCancelCommand { get; set; }
        public ICommand SparePartsCancelCommand { get; set; }
        public ICommand FamilyCancelCommand { get; set; }


        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeProductTypeDescriptionCommand { get; set; }
        public ICommand ChangeProductTypeNameCommand { get; set; }
        public ICommand UncheckedCopyDescriptionCommand { get; set; }


        public ICommand AddNewOptionCommand { get; set; }
        public ICommand AddNewWayCommand { get; set; }
        public ICommand AddNewDetectionCommand { get; set; }
        public ICommand AddNewSparePartCommand { get; set; }

        public ICommand EditOptionCommand { get; set; }
        public ICommand EditOptionForCurrentModuleCommand { get; set; }
        public ICommand EditWayCommand { get; set; }
        public ICommand EditWayForCurrentModuleCommand { get; set; }
        public ICommand EditDetectionsCommand { get; set; }
        public ICommand EditDetectionsForCurrentModuleCommand { get; set; }
        public ICommand EditSparePartsCommand { get; set; }
        public ICommand EditSparePartsForCurrentModuleCommand { get; set; }


        public ICommand AddFileCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand AddLinkCommand { get; set; }
        public ICommand EditLinkCommand { get; set; }
        public ICommand DeleteLinkCommand { get; set; }

        public ICommand EditImageCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }

        public ICommand ExportToExcelCommand { get; set; }
        public ICommand ExportToExcelCustomersCommand { get; set; }



        public ICommand DeleteWayCommand { get; set; }
        public ICommand StarWayCommand { get; set; }
        public ICommand DeleteDetectionCommand { get; set; }
        public ICommand DeleteOptionCommand { get; set; }
        public ICommand DeleteSparePartCommand { get; set; }
        public ICommand DeleteFamilyCommand { get; set; }

        public ICommand EscapeButtonCommand { get; set; }

        public ICommand ForSelectAllCommand { get; set; }
        public ICommand PrintCommand { get; set; }

        public ICommand OpenSelectedImageCommand { get; set; }
        public ICommand OpenImageGalleryCommand { get; set; }

        public ICommand RestrictOpeningPopUpCommand { get; set; }

        public ICommand AddProductTypeItemAcceptButtonCommand { get; set; }
        public ICommand AddProductTypeItemCancelButtonCommand { get; set; }

        public ICommand DeleteMandatoryCommand { get; set; }
        public ICommand DeleteSuggestedCommand { get; set; }
        public ICommand DeleteIncompatibleCommand { get; set; }


        public ICommand CommandOnDragRecordOverMandatoryGrid { get; set; }
        public ICommand CommandDropRecordMandatoryGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropMandatoryGrid { get; set; }

        public ICommand CommandOnDragRecordOverSuggestedGrid { get; set; }
        public ICommand CommandDropRecordSuggestedGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropSuggestedGrid { get; set; }
        public ICommand CommandOnDragRecordOverIncompatibleGrid { get; set; }
        public ICommand CommandDropRecordIncompatibleGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropIncompatibleGrid { get; set; }

        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand CustomerHeaderClickCommand { get; set; }

        public ICommand CellValueChangedCommand { get; set; }
        public ICommand ShowReferenceViewCommand { get; set; }
        public ICommand ShowDescriptionViewCommand { get; set; }

        public ICommand DuplicateModulesCommand { get; set; }

        //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
        public ICommand SelectedGroupIndexChangedCommand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCountryCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand AddIncludedCustomerCommand { get; set; }
        public ICommand AddNotIncludedCustomerCommand { get; set; }
        public ICommand DeleteIncludedCustomerCommand { get; set; }
        public ICommand DeleteNotIncludedCustomerCommand { get; set; }
        public ICommand IncludedCellValueChangingCommand { get; set; }
        public ICommand NotIncludedCellValueChangingCommand { get; set; }
        public ICommand ItemPositionChangedCommand { get; set; }//[rdixit][GEOS2-2694][01.08.2022]
        public ICommand ExpandAndCollapseOptionsCommand { get; set; }//[rdixit][29.11.2022][GEOS2-2718]
        public ICommand ExpandAndCollapseDetectionsCommand { get; set; }//[rdixit][29.11.2022][GEOS2-2718]

        public ICommand ExpandAndCollapseModuleCommand { get; set; }   //[29.11.2022][sshegaonkar][GEOS2-2718]

        public ICommand ExpandAndCollapseArticleCommand { get; set; }  //[29.11.2022][sshegaonkar][GEOS2-2718]
        public ICommand ExpandAndCollapseSparePartCommand { get; set; }//[rdixit][29.11.2022][GEOS2-2718]

        public ICommand CommandTreeListViewDropRecordWay { get; set; }//[Sudhir.jangra][GEOS2-4221][05/04/2023]

        public ICommand AddCommentsCommand { get; set; }//[Sudhir.jangra][GEOS2-4935][21/11/2023]

        public ICommand DeleteCommentRowCommand { get; set; }//[Sudhir.Jangra][GEOS2-4935]

        public ICommand CommentsGridDoubleClickCommand { get; set; }//[Sudhir.Jangra][GEOS2-4935]


        public ICommand CommandOnDragRecordOverNotIncludedModuleGrid { get; set; }
        public ICommand CommandOnDragRecordOverIncludedModuleGrid { get; set; }
        public ICommand RuleChangedCommand { get; set; }
        public ICommand LostFocusRuleChangedCommand { get; set; }
        public ICommand CommandShowFilterPopupForIncludedClick { get; set; }
        public ICommand CommandShowFilterPopupForNotIncludedClick { get; set; }
        public ICommand ModuleGridControlLoadedCommand { get; set; }
        public ICommand ModuleGridControlUnloadedCommand { get; set; }
        //[rdixit][GEOS2-6624][10.12.2024]
        public ICommand CommandTreeListViewDropRecordFamily { get; set; }
        #endregion

        #region Constructor

        public EditProductTypeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 130;

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
                IsCheckedCopyDescription = true;
                IsOptionExpand = true;
                IsDetectionExpand = true;
                IsModuleExpand = true;
                IsArticleExpand = true;
                IsSparePartExpand = true;
                ExpandAndCollapseArticleCommand = new DelegateCommand<object>(ExpandAndCollapseArticleCommandAction); //[29.11.2022][sshegaonkar][GEOS2-2718]
                ExpandAndCollapseModuleCommand = new DelegateCommand<object>(ExpandAndCollapseModuleCommandAction);  //[29.11.2022][sshegaonkar][GEOS2-2718]
                ExpandAndCollapseOptionsCommand = new DelegateCommand<object>(ExpandAndCollapseOptionsCommandAction);//[rdixit][29.11.2022][GEOS2-2718]
                ExpandAndCollapseDetectionsCommand = new DelegateCommand<object>(ExpandAndCollapseDetectionsCommandAction);//[rdixit][29.11.2022][GEOS2-2718]
                ExpandAndCollapseSparePartCommand = new DelegateCommand<object>(ExpandAndCollapseSparePartCommandAction);//[rdixit][29.11.2022][GEOS2-2718]

                CommandOnDragRecordOverOptions = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverOptions);
                CommandOnDragRecordOverOptionsGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverOptionsGrid);
                CommandTreeListViewDropRecordOption = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordOption);
                CommandCompleteRecordDragDropOptions = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropOptions);

                CommandOnDragRecordOverWay = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverWay);
                CommandOnDragRecordOverWayGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverWayGrid);
                CommandCompleteRecordDragDropWay = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropWay);

                CommandOnDragRecordOverDetections = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverDetections);
                CommandOnDragRecordOverDetectionsGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverDetectionsGrid);
                CommandCompleteRecordDragDropDetections = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropDetections);
                CommandTreeListViewDropRecordDetection = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordDetection);

                CommandOnDragRecordOverSpareParts = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSpareParts);
                CommandOnDragRecordOverSparePartsGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSparePartsGrid);
                CommandCompleteRecordDragDropSpareParts = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropSpareParts);
                CommandTreeListViewDropRecordSpareParts = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordSpareParts);

                CommandOnDragRecordOverFamily = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverFamily);
                CommandOnDragRecordOverFamilyGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverFamilyGrid);
                CommandCompleteRecordDragDropFamily = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropFamily);

                CommandOnDragRecordOverCustomers = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverCustomers);

                //compatibility commands
                CommandOnDragRecordOverMandatoryGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverMandatoryGrid);
                CommandDropRecordMandatoryGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordMandatoryGrid);
                CommandCompleteRecordDragDropMandatoryGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropMandatoryGrid);

                CommandOnDragRecordOverSuggestedGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSuggestedGrid);
                CommandDropRecordSuggestedGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordSuggestedGrid);
                CommandCompleteRecordDragDropSuggestedGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropSuggestedGrid);

                CommandOnDragRecordOverIncompatibleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverIncompatibleGrid);
                CommandDropRecordIncompatibleGrid = new DelegateCommand<DropRecordEventArgs>(DropRecordIncompatibleGrid);
                CommandCompleteRecordDragDropIncompatibleGrid = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropIncompatibleGrid);


                OptionsCancelCommand = new DelegateCommand<object>(OptionsCancelAction);
                WayCancelCommand = new DelegateCommand<object>(WayCancelAction);
                DetectionsCancelCommand = new DelegateCommand<object>(DetectionsCancelAction);
                SparePartsCancelCommand = new DelegateCommand<object>(SparePartsCancelAction);
                FamilyCancelCommand = new DelegateCommand<object>(FamilyCancelAction);


                ChangeProductTypeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeProductTypeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                UncheckedCopyDescriptionCommand = new DelegateCommand<object>(UncheckedCopyDescription);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);

                AddNewOptionCommand = new DelegateCommand<object>(AddNewOption);
                AddNewWayCommand = new DelegateCommand<object>(AddNewWay);
                AddNewDetectionCommand = new DelegateCommand<object>(AddNewDetection);
                AddNewSparePartCommand = new DelegateCommand<object>(AddNewSparePart);

                EditOptionCommand = new DelegateCommand<object>(EditOption);
                EditOptionForCurrentModuleCommand = new DelegateCommand<object>(EditOptionForCurrentModule);
                EditWayCommand = new DelegateCommand<object>(EditWay);
                EditWayForCurrentModuleCommand = new DelegateCommand<object>(EditWayForCurrentModule);
                EditDetectionsCommand = new DelegateCommand<object>(EditDetections);
                EditDetectionsForCurrentModuleCommand = new DelegateCommand<object>(EditDetectionsForCurrentModule);
                EditSparePartsCommand = new DelegateCommand<object>(EditSpareParts);
                EditSparePartsForCurrentModuleCommand = new DelegateCommand<object>(EditSparePartsForCurrentModule);

                AddFileCommand = new DelegateCommand<object>(AddFile);
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));
                DeleteFileCommand = new DelegateCommand<object>(DeleteFile);
                EditFileCommand = new DelegateCommand<object>(EditFile);

                AddLinkCommand = new DelegateCommand<object>(AddLink);
                EditLinkCommand = new DelegateCommand<object>(EditLink);
                DeleteLinkCommand = new DelegateCommand<object>(DeleteLink);

                EditImageCommand = new DelegateCommand<object>(EditImageAction);
                AddImageCommand = new DelegateCommand<object>(AddImageAction);
                DeleteImageCommand = new DelegateCommand<object>(DeleteImageAction);

                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
                ExportToExcelCustomersCommand = new DelegateCommand<object>(ExportToExcelCustomers);

                DeleteWayCommand = new DelegateCommand<object>(DeleteWay);
                StarWayCommand = new DelegateCommand<object>(SetStarWay);
                DeleteDetectionCommand = new DelegateCommand<object>(DeleteDetection);
                DeleteOptionCommand = new DelegateCommand<object>(DeleteOption);
                DeleteSparePartCommand = new DelegateCommand<object>(DeleteSparePart);
                DeleteFamilyCommand = new DelegateCommand<object>(DeleteFamily);

                EscapeButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                ForSelectAllCommand = new Prism.Commands.DelegateCommand<object>(SelectAllAction);
                PrintCommand = new RelayCommand(new Action<object>(PrintCommandAction));

                OpenImageGalleryCommand = new RelayCommand(new Action<object>(OpenImageGalleryAction));
                OpenSelectedImageCommand = new DelegateCommand<object>(OpenSelectedImageAction);

                RestrictOpeningPopUpCommand = new DelegateCommand<object>(RestrictOpeningPopUpAction);

                DeleteMandatoryCommand = new DelegateCommand<object>(DeleteMandatory);
                DeleteSuggestedCommand = new DelegateCommand<object>(DeleteSuggested);
                DeleteIncompatibleCommand = new DelegateCommand<object>(DeleteIncompatible);

                SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChangedCommandAction);
                CustomerHeaderClickCommand = new DelegateCommand<object>(CustomerHeaderClickCommandAction);

                CellValueChangedCommand = new DelegateCommand<object>(CellValueChangedCommandAction);

                ShowReferenceViewCommand = new DelegateCommand<object>(ShowReferenceViewCommandAction);
                ShowDescriptionViewCommand = new DelegateCommand<object>(ShowDescriptionViewCommandAction);

                DuplicateModulesCommand = new RelayCommand(new Action<object>(DuplicateModulesCommandAction));
                AddProductTypeItemAcceptButtonCommand = new RelayCommand(new Action<object>(EditSaveModule));
                AddProductTypeItemCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));

                //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
                SelectedGroupIndexChangedCommand = new DelegateCommand<object>(SelectedGroupIndexChangedCommandAction);
                ChangeRegionCommand = new DelegateCommand<object>(ChangeRegionCommandAction);
                ChangeCountryCommand = new DelegateCommand<object>(ChangeCountryCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                AddIncludedCustomerCommand = new DelegateCommand<object>(AddIncludedCustomerCommandAction);
                AddNotIncludedCustomerCommand = new DelegateCommand<object>(AddNotIncludedCustomerCommandAction);
                DeleteIncludedCustomerCommand = new DelegateCommand<object>(DeleteIncludedCustomerCommandAction);
                DeleteNotIncludedCustomerCommand = new DelegateCommand<object>(DeleteNotIncludedCustomerCommandAction);
                IncludedCellValueChangingCommand = new DelegateCommand<object>(IncludedCellValueChangingCommandAction);
                NotIncludedCellValueChangingCommand = new DelegateCommand<object>(NotIncludedCellValueChangingCommandAction);
                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                CommandTreeListViewDropRecordWay = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordWayAction);//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                AddCommentsCommand = new RelayCommand(new Action<object>(AddCommentsCommandAction));//[Sudhir.Jangra][GEOS2-4935]
                DeleteCommentRowCommand = new RelayCommand(new Action<object>(DeleteCommentRowCommandAction));//[Sudhir.Jangra][GEOS2-4935]
                CommentsGridDoubleClickCommand = new RelayCommand(new Action<object>(CommentDoubleClickCommandAction));//[Sudhir.Jangra][GEOS2-4935]

                CommandTreeListViewDropRecordFamily = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordFamily);
                try
                {
                    FillLogicList();
                    CommandOnDragRecordOverNotIncludedModuleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverNotIncludedModuleGrid);
                    CommandOnDragRecordOverIncludedModuleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverIncludedModuleGrid);
                    RuleChangedCommand = new DelegateCommand<object>(RuleChangedCommandAction);
                    LostFocusRuleChangedCommand = new DelegateCommand<object>(LostFocusRuleChangedCommandAction);
                    CommandShowFilterPopupForIncludedClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForIncluded);
                    CommandShowFilterPopupForNotIncludedClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForNotIncluded);
                    ModuleGridControlLoadedCommand = new DelegateCommand<object>(ModuleGridControlLoadedAction);
                    ModuleGridControlUnloadedCommand = new DelegateCommand<object>(ModuleGridControlUnloadedAction);

                    List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                    SelectedCurrencySymbol =  CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"]).Symbol;
                    SelectedCurrency =  CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"]);


                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor EditProductTypeViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }




                GeosApplication.Instance.Logger.Log("Constructor EditProductTypeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor EditProductTypeViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Command Action

        // Perform all command actions
        //[rdixit][GEOS2-6624][10.12.2024]
        private void TreeListViewDropRecordFamily(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordFamily()...", category: Category.Info, priority: Priority.Low);
                if (e.Data.GetDataPresent(typeof(RecordDragDropData)) && e.IsFromOutside == true && typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ConnectorFamilies> newRecords = data.Records.OfType<ConnectorFamilies>().Select(x => new ConnectorFamilies { Key = x.Key, Name = x.Name, Parent = x.Parent, IdFamily = x.IdFamily, IdSubFamilyConnector = x.IdSubFamilyConnector}).ToList();
                    if (newRecords != null)
                    {
                        ConnectorFamilies temp = newRecords.FirstOrDefault();
                        if (Families.Any(x => x.IdFamily == temp.IdFamily) && temp.IdSubFamilyConnector > 0)
                        {
                            Families.Add(temp);
                            e.Handled = true;
                        }
                        else if (Families.Any(x => x.IdFamily == temp.IdFamily) && temp.IdSubFamilyConnector == 0)
                        {                           
                            foreach (ConnectorFamilies item in FamilyMenulist.Where(x => x.IdFamily == temp.IdFamily))
                            {
                                if (!Families.Any(x => x.Key == item.Key))
                                    Families.Add(item);
                            }
                            e.Handled = true;

                        }
                        else if (!Families.Any(x => x.IdFamily == temp.IdFamily))
                        {
                            if (temp.IdSubFamilyConnector > 0)
                            {
                                ConnectorFamilies itemclass = FamilyMenulist.FirstOrDefault(x => x.IdFamily == temp.IdFamily && x.IdSubFamilyConnector == 0);
                                if (itemclass != null && !Families.Any(x => x.IdFamily == itemclass.IdFamily && x.IdSubFamilyConnector == 0))
                                {
                                    Families.Add(temp);
                                    Families.Add(itemclass);
                                }
                                else
                                {
                                    Families.Add(temp);
                                }
                            }
                            else
                            {
                                foreach (ConnectorFamilies item in FamilyMenulist.Where(x => x.IdFamily == temp.IdFamily))
                                {
                                    if (!Families.Any(x => x.Key == item.Key))
                                        Families.Add(item);
                                }
                            }
                            e.Handled = true;
                        }
                        if (e.Handled == true)
                        {
                            if (temp.IdSubFamilyConnector > 0)
                                FamilyMenulist.Where(a => a.IdSubFamilyConnector == temp.IdSubFamilyConnector).ToList().ForEach(b => b.IsCurrentFamily = true);
                            else
                            {
                                FamilyMenulist.Where(x => x.IdFamily == temp.IdFamily && x.IdSubFamilyConnector > 0)?.ToList()?.ForEach(j => j.IsCurrentFamily = true);
                            }
                        }
                    }
                }               
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordFamily()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOverOptions(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverOptions()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverOptions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverOptionsGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverOptionsGrid()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && e.TargetRecord != null && typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else if ((e.IsFromOutside) && typeof(Options).IsAssignableFrom(e.GetRecordType()) && e.TargetRecord == null)
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else if (e.DropPosition == DropPosition.Inside && typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                else if (typeof(Options).IsAssignableFrom(e.GetRecordType()) && (e.DropPosition == DropPosition.Before || e.DropPosition == DropPosition.After))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverOptionsGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverOptionsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TreeListViewDropRecordOption(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordOption()...", category: Category.Info, priority: Priority.Low);

                if (e.Data.GetDataPresent(typeof(RecordDragDropData)) && e.IsFromOutside == true && typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<Options> newRecords = data.Records.OfType<Options>().Select(x => new Options { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdOptions = x.IdOptions, OrderNumber = x.OrderNumber }).ToList();

                    Options temp = newRecords.FirstOrDefault();
                    if (temp.Parent == null && !Options.Any(x => x.Key == temp.Key))
                    {
                        if (OptionsMenulist.Any(x => x.Parent == temp.Key))
                        {
                            Options.Add(temp);
                            foreach (Options item in OptionsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (!Options.Any(x => x.Key == item.Key))
                                    Options.Add(item);
                            }
                            e.Handled = true;
                        }

                        if (temp.Parent == null && temp.IdGroup == null && !Options.Any(x => x.Key == temp.Key))
                        {
                            Options.Add(temp);
                            e.Handled = true;
                        }

                        if (newRecords.Count > 0)
                        {
                            DataObject dataObject = new DataObject();
                            dataObject.SetData(new RecordDragDropData(newRecords.ToArray()));
                            e.Data = dataObject;
                        }
                    }
                    else if (temp.Parent == null && Options.Any(x => x.Key == temp.Key))
                    {
                        if (OptionsMenulist.Any(x => x.Parent == temp.Key))
                        {
                            foreach (Options item in OptionsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (!Options.Any(x => x.Key == item.Key))
                                    Options.Add(item);
                            }
                        }
                        e.Handled = true;
                    }
                    else if (temp.Parent != null && !Options.Any(x => x.Key == temp.Key))
                    {
                        Options itemclass = OptionsMenulist.FirstOrDefault(x => x.Key == temp.Parent);
                        if (itemclass != null && !Options.Any(x => x.Key == itemclass.Key))
                        {
                            Options.Add(temp);
                            Options.Add(itemclass);
                        }
                        else
                        {
                            Options.Add(temp);
                        }
                        e.Handled = true;
                    }
                    else if (temp.Parent != null && Options.Any(x => x.Key == temp.Key))
                    {
                        e.Handled = true;
                    }

                    if (Options.Count > 0)
                    {
                        int childcount_withoutgroup = Options.Where(a => a.IdGroup == null && a.Parent == null).Count();
                        if (temp.IdGroup == null && temp.Parent == null && Options.Where(a => a.IdOptions == temp.IdOptions).Count() == 0)
                        {
                            childcount_withoutgroup = childcount_withoutgroup + 1;
                        }
                        if (temp.Parent != null && Options.Where(a => a.Key == temp.Key && a.Parent == temp.Parent).Count() == 0)
                        {
                            Options_count = Options.Where(a => a.Parent != null).Count() + 1 + childcount_withoutgroup;
                        }
                        else
                        {
                            Options_count = Options.Where(a => a.Parent != null).Count() + childcount_withoutgroup;
                        }
                        if (temp.Parent == null && temp.IdGroup != null && Options.Where(a => a.Key == temp.Key).Count() == 0)
                        {
                            Options_Group_count = Options.Where(a => a.Parent == null && a.IdGroup != null).Count() + 1;
                        }
                        else
                        {
                            Options_Group_count = Options.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                    }
                    else if (temp.Parent == null && temp.IdGroup == null)
                    {
                        Options_count = Options_count + 1;
                    }
                    //set gray color
                    //[sdeshpande][GEOS2-4098][26-12-2022]
                    //[001] 
                    if (temp.IdOptions > 0)
                        OptionsMenulist.Where(a => a.IdOptions == temp.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = true);
                    else
                    {
                        foreach (Options item in OptionsMenulist.Where(x => x.Parent == temp.Key))
                        {
                            if (Options.Any(x => x.Key == item.Key))
                            {
                                OptionsMenulist.Where(a => a.IdOptions == item.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = true);
                            }
                        }
                    }
                }
                else if (e.IsFromOutside == false && typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                    {
                        var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                        List<Options> newRecords = data.Records.OfType<Options>().Select(x => new Options { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdOptions = x.IdOptions, OrderNumber = x.OrderNumber }).ToList();

                        Options temp = newRecords.FirstOrDefault();

                        Options target_record = (Options)e.TargetRecord;

                        //if (temp.IdGroup == target_record.IdGroup && temp.Parent == target_record.Parent)
                        //{
                        //    if (temp.IdGroup == null && temp.Parent == null)
                        //    {
                        //        e.Effects = DragDropEffects.None;
                        //        e.Handled = true;
                        //    }
                        //    else
                        //    {
                        //        e.Effects = DragDropEffects.Move;
                        //        e.Handled = false;
                        //    }
                        //}
                        //else
                        //{
                        //    e.Effects = DragDropEffects.None;
                        //    e.Handled = true;
                        //}
                        e.Effects = DragDropEffects.Move;
                        e.Handled = false;
                        // Upper Part Comment For Without Order By [Sudhir.Jangra][GEOS2-4221][05/04/2023]
                        // e.Effects = DragDropEffects.Move;
                        // e.Handled = false;
                        //set gray color
                        //[sdeshpande][GEOS2-4098][26-12-2022]
                        //[001]
                        if (temp.IdOptions > 0)
                            OptionsMenulist.Where(a => a.IdOptions == temp.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = true);
                        else
                        {
                            foreach (Options item in OptionsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (Options.Any(x => x.Key == item.Key))
                                {
                                    OptionsMenulist.Where(a => a.IdOptions == item.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = true);
                                }
                            }
                        }
                    }
                }
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordOption()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropOptions(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropOptions()...", category: Category.Info, priority: Priority.Low);

                e.Handled = false;
                IsEnabledCancelButton = true;
                IsOptionDrag = true;

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropOptions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverWay(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverWay()...", category: Category.Info, priority: Priority.Low);


                if (typeof(Ways).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverWay()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverWay() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverWayGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverWayGrid()...", category: Category.Info, priority: Priority.Low);


                if ((e.IsFromOutside) && typeof(Ways).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                    IsEnabled = true;
                }
                else if ((e.IsFromOutside) && typeof(Ways).IsAssignableFrom(e.GetRecordType()) && e.TargetRecord == null)//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else if (e.DropPosition == DropPosition.Inside && typeof(Ways).IsAssignableFrom(e.GetRecordType()))//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                else if (typeof(Ways).IsAssignableFrom(e.GetRecordType()) && (e.DropPosition == DropPosition.Before || e.DropPosition == DropPosition.After))//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverWayGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverWayGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropWay(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropWay()...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;//rajashri[GEOS2-4957] -changed false to true

                //IsEnabled = true;
                //IsEnabledCancelButton = true;
                //[GEOS2-4098][rdixit][28.12.2022]
                //if (Ways != null)
                //    Ways = new ObservableCollection<Ways>(Ways.GroupBy(opt => opt.IdWays).Select(g => g.First()));
                //if (Ways != null)
                //{
                //    foreach (var item in Ways)
                //    {
                //        WaysMenulist.Where(a => a.IdWays == item.IdWays).ToList().ForEach(b => b.IsCurrentWays = true);
                //    }
                //}
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropWay()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropWay() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverDetections(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetections()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverDetectionsGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                    IsEnabled = true;
                }
                else if ((e.IsFromOutside) && typeof(Detections).IsAssignableFrom(e.GetRecordType()) && e.TargetRecord == null)
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else if (e.DropPosition == DropPosition.Inside && typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                else if (typeof(Detections).IsAssignableFrom(e.GetRecordType()) && (e.DropPosition == DropPosition.Before || e.DropPosition == DropPosition.After))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverDetectionsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropDetections(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropDetections()...", category: Category.Info, priority: Priority.Low);
                e.Handled = false;
                IsEnabledCancelButton = true;
                IsDetectionDrag = true;
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropDetections()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TreeListViewDropRecordDetection(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordDetection()...", category: Category.Info, priority: Priority.Low);
                if (e.Data.GetDataPresent(typeof(RecordDragDropData)) && e.IsFromOutside == true && typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<Detections> newRecords = data.Records.OfType<Detections>().Select(x => new Detections { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdDetections = x.IdDetections, OrderNumber = x.OrderNumber }).ToList();

                    Detections temp = newRecords.FirstOrDefault();

                    if (temp.Parent == null && !Detections.Any(x => x.Key == temp.Key))
                    {
                        if (DetectionsMenulist.Any(x => x.Parent == temp.Key))
                        {
                            Detections.Add(temp);
                            foreach (Detections item in DetectionsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (!Detections.Any(x => x.Key == item.Key))
                                    Detections.Add(item);
                            }
                            e.Handled = true;
                        }

                        if (temp.Parent == null && temp.IdGroup == null && !Detections.Any(x => x.Key == temp.Key))
                        {
                            Detections.Add(temp);
                            e.Handled = true;
                        }

                        if (newRecords.Count > 0)
                        {
                            DataObject dataObject = new DataObject();
                            dataObject.SetData(new RecordDragDropData(newRecords.ToArray()));
                            e.Data = dataObject;
                        }
                    }
                    else if (temp.Parent == null && Detections.Any(x => x.Key == temp.Key))
                    {
                        if (DetectionsMenulist.Any(x => x.Parent == temp.Key))
                        {
                            foreach (Detections item in DetectionsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (!Detections.Any(x => x.Key == item.Key))
                                    Detections.Add(item);
                            }
                        }
                        e.Handled = true;
                    }
                    else if (temp.Parent != null && !Detections.Any(x => x.Key == temp.Key))
                    {
                        Detections itemclass = DetectionsMenulist.FirstOrDefault(x => x.Key == temp.Parent);
                        if (itemclass != null && !Detections.Any(x => x.Key == itemclass.Key))
                        {
                            Detections.Add(temp);
                            Detections.Add(itemclass);
                        }
                        else
                        {
                            Detections.Add(temp);
                        }
                        e.Handled = true;
                    }
                    else if (temp.Parent != null && Detections.Any(x => x.Key == temp.Key))
                    {
                        e.Handled = true;
                    }

                    if (Detections.Count > 0)
                    {
                        int childcount_withoutgroup = Detections.Where(a => a.IdGroup == null && a.Parent == null).Count();
                        if (temp.IdGroup == null && temp.Parent == null && Detections.Where(a => a.IdDetections == temp.IdDetections).Count() == 0)
                        {
                            childcount_withoutgroup = childcount_withoutgroup + 1;
                        }
                        if (temp.Parent != null && Detections.Where(a => a.Key == temp.Key && a.Parent == temp.Parent).Count() == 0)
                        {
                            Detections_count = Detections.Where(a => a.Parent != null).Count() + 1 + childcount_withoutgroup;
                        }
                        else
                        {
                            Detections_count = Detections.Where(a => a.Parent != null).Count() + childcount_withoutgroup;
                        }
                        if (temp.Parent == null && temp.IdGroup != null && Detections.Where(a => a.Key == temp.Key).Count() == 0)
                        {
                            Detections_group_count = Detections.Where(a => a.Parent == null && a.IdGroup != null).Count() + 1;
                        }
                        else
                        {
                            Detections_group_count = Detections.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                    }
                    else if (temp.Parent == null && temp.IdGroup == null)
                    {
                        Detections_count = Detections_count + 1;
                    }
                    //[sdeshpande][GEOS2-4098][26-12-2022]
                    //set gray color   
                    //[001]               
                    if (temp.IdDetections > 0)
                        DetectionsMenulist.Where(a => a.IdDetections == temp.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = true);
                    else
                    {
                        foreach (Detections item in DetectionsMenulist.Where(x => x.Parent == temp.Key))
                        {
                            if (Detections.Any(x => x.Key == item.Key))
                            {
                                DetectionsMenulist.Where(a => a.IdDetections == item.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = true);
                            }
                        }
                    }
                }
                else if (e.IsFromOutside == false && typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                    {
                        var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                        List<Detections> newRecords = data.Records.OfType<Detections>().Select(x => new Detections { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdDetections = x.IdDetections, OrderNumber = x.OrderNumber }).ToList();

                        Detections temp = newRecords.FirstOrDefault();

                        Detections target_record = (Detections)e.TargetRecord;
                        //if (temp.IdGroup == target_record.IdGroup && temp.Parent == target_record.Parent)
                        //{
                        //    if (temp.IdGroup == null && temp.Parent == null)
                        //    {
                        //        e.Effects = DragDropEffects.None;
                        //        e.Handled = true;
                        //    }
                        //    else
                        //    {
                        //        e.Effects = DragDropEffects.Move;
                        //        e.Handled = false;
                        //    }
                        //}
                        //else
                        //{
                        //    e.Effects = DragDropEffects.None;
                        //    e.Handled = true;
                        //}
                        // Upper Part Comment For Without Order By [Sudhir.Jangra][GEOS2-4221][05/04/2023]
                        e.Effects = DragDropEffects.Move;
                        e.Handled = false;
                        //[sdeshpande][GEOS2-4098][26-12-2022]
                        //set gray color
                        //[001]
                        if (temp.IdDetections > 0)
                            DetectionsMenulist.Where(a => a.IdDetections == temp.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = true);
                        else
                        {
                            foreach (Detections item in DetectionsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                //if (!Detections.Any(x => x.Key == item.Key))
                                //{
                                //    Detections.Add(item);
                                DetectionsMenulist.Where(a => a.IdDetections == item.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = true);
                                // }
                            }
                        }
                    }
                }
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordDetection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverSpareParts(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSpareParts()...", category: Category.Info, priority: Priority.Low);

                if (typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSpareParts()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSpareParts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverSparePartsGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSparePartsGrid()...", category: Category.Info, priority: Priority.Low);

                if ((e.IsFromOutside) && e.TargetRecord != null && typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else if ((e.IsFromOutside) && typeof(SpareParts).IsAssignableFrom(e.GetRecordType()) && e.TargetRecord == null)
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else if (e.DropPosition == DropPosition.Inside && typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                else if (typeof(SpareParts).IsAssignableFrom(e.GetRecordType()) && (e.DropPosition == DropPosition.Before || e.DropPosition == DropPosition.After))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSparePartsGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSparePartsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TreeListViewDropRecordSpareParts(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordSpareParts()...", category: Category.Info, priority: Priority.Low);
                if (e.Data.GetDataPresent(typeof(RecordDragDropData)) && e.IsFromOutside == true && typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<SpareParts> newRecords = data.Records.OfType<SpareParts>().Select(x => new SpareParts { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdSpareParts = x.IdSpareParts, OrderNumber = x.OrderNumber }).ToList();

                    SpareParts temp = newRecords.FirstOrDefault();

                    if (temp.Parent == null && !SpareParts.Any(x => x.Key == temp.Key))
                    {
                        if (SparePartsMenulist.Any(x => x.Parent == temp.Key))
                        {
                            SpareParts.Add(temp);
                            foreach (SpareParts item in SparePartsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (!SpareParts.Any(x => x.Key == item.Key))
                                    SpareParts.Add(item);
                            }
                            e.Handled = true;
                        }

                        if (temp.Parent == null && temp.IdGroup == null && !SpareParts.Any(x => x.Key == temp.Key))
                        {
                            SpareParts.Add(temp);
                            e.Handled = true;
                        }

                        if (newRecords.Count > 0)
                        {
                            DataObject dataObject = new DataObject();
                            dataObject.SetData(new RecordDragDropData(newRecords.ToArray()));
                            e.Data = dataObject;
                        }
                    }
                    else if (temp.Parent == null && SpareParts.Any(x => x.Key == temp.Key))
                    {
                        if (SparePartsMenulist.Any(x => x.Parent == temp.Key))
                        {
                            foreach (SpareParts item in SparePartsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (!SpareParts.Any(x => x.Key == item.Key))
                                    SpareParts.Add(item);
                            }
                        }
                        e.Handled = true;
                    }
                    else if (temp.Parent != null && !SpareParts.Any(x => x.Key == temp.Key))
                    {
                        SpareParts itemclass = SparePartsMenulist.FirstOrDefault(x => x.Key == temp.Parent);
                        if (itemclass != null && !SpareParts.Any(x => x.Key == itemclass.Key))
                        {
                            SpareParts.Add(temp);
                            SpareParts.Add(itemclass);
                        }
                        else
                        {
                            SpareParts.Add(temp);
                        }
                        e.Handled = true;
                    }
                    else if (temp.Parent != null && SpareParts.Any(x => x.Key == temp.Key))
                    {
                        e.Handled = true;
                    }

                    if (SpareParts.Count > 0)
                    {
                        int childcount_withoutgroup = SpareParts.Where(a => a.IdGroup == null && a.Parent == null).Count();
                        if (temp.IdGroup == null && temp.Parent == null && SpareParts.Where(a => a.IdSpareParts == temp.IdSpareParts).Count() == 0)
                        {
                            childcount_withoutgroup = childcount_withoutgroup + 1;
                        }
                        if (temp.Parent != null && SpareParts.Where(a => a.Key == temp.Key && a.Parent == temp.Parent).Count() == 0)
                        {
                            SpareParts_count = SpareParts.Where(a => a.Parent != null).Count() + 1 + childcount_withoutgroup;
                        }
                        else
                        {
                            SpareParts_count = SpareParts.Where(a => a.Parent != null).Count() + childcount_withoutgroup;
                        }
                        if (temp.Parent == null && temp.IdGroup != null && SpareParts.Where(a => a.Key == temp.Key).Count() == 0)
                        {
                            SpareParts_group_count = SpareParts.Where(a => a.Parent == null && a.IdGroup != null).Count() + 1;
                        }
                        else
                        {
                            SpareParts_group_count = SpareParts.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                    }
                    else if (temp.Parent == null && temp.IdGroup == null)
                    {
                        SpareParts_count = SpareParts_count + 1;
                    }
                    //set gray color [sdeshpande][GEOS2-4098][26-12-2022]
                    //[001]
                    if (temp.IdSpareParts > 0)
                        SparePartsMenulist.Where(a => a.IdSpareParts == temp.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = true);
                    else
                    {
                        foreach (SpareParts item in SparePartsMenulist.Where(x => x.Parent == temp.Key))
                        {
                            if (SpareParts.Any(x => x.Key == item.Key))
                            {
                                SparePartsMenulist.Where(a => a.IdSpareParts == item.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = true);
                            }
                        }
                    }
                }
                else if (e.IsFromOutside == false && typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                    {
                        var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                        List<SpareParts> newRecords = data.Records.OfType<SpareParts>().Select(x => new SpareParts { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdSpareParts = x.IdSpareParts, OrderNumber = x.OrderNumber }).ToList();

                        SpareParts temp = newRecords.FirstOrDefault();

                        SpareParts target_record = (SpareParts)e.TargetRecord;
                        //if (temp.IdGroup == target_record.IdGroup && temp.Parent == target_record.Parent)
                        //{
                        //    if (temp.IdGroup == null && temp.Parent == null)
                        //    {
                        //        e.Effects = DragDropEffects.None;
                        //        e.Handled = true;
                        //    }
                        //    else
                        //    {
                        //        e.Effects = DragDropEffects.Move;
                        //        e.Handled = false;
                        //    }
                        //}
                        //else
                        //{
                        //    e.Effects = DragDropEffects.None;
                        //    e.Handled = true;
                        //}
                        // Upper Part Comment For Without Order By [Sudhir.Jangra][GEOS2-4221][05/04/2023]
                        e.Effects = DragDropEffects.Move;
                        e.Handled = false;
                        //[sdeshpande][GEOS2-4098][26-12-2022]
                        //set gray color
                        //[001]
                        if (temp.IdSpareParts > 0)
                            SparePartsMenulist.Where(a => a.IdSpareParts == temp.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = true);
                        else
                        {
                            foreach (SpareParts item in SparePartsMenulist.Where(x => x.Parent == temp.Key))
                            {
                                if (SpareParts.Any(x => x.Key == item.Key))
                                {
                                    SparePartsMenulist.Where(a => a.IdSpareParts == item.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = true);
                                }
                            }
                        }
                    }
                }
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordSpareparts()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordSpareParts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropSpareParts(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropSpareParts()...", category: Category.Info, priority: Priority.Low);
                e.Handled = false;
                IsEnabledCancelButton = true;
                IsSparePartDrag = true;
                //e.Handled = true;

                //if (SpareParts != null)
                //    SpareParts = new ObservableCollection<SpareParts>(SpareParts.GroupBy(opt => opt.IdSpareParts).Select(g => g.First()));

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropSpareParts()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropSpareParts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverFamily(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamily()...", category: Category.Info, priority: Priority.Low);

                if (typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamily()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverFamilyGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamilyGrid()...", category: Category.Info, priority: Priority.Low);


                if ((e.IsFromOutside) && typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                    //IsEnabled = true;
                }
                //else if ((e.IsFromOutside) && typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()) && e.TargetRecord == null)//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                //{
                //    e.Effects = DragDropEffects.Copy;
                //    e.Handled = true;
                //}
                //else if (e.DropPosition == DropPosition.Inside && typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()))//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                //{
                //    e.Effects = DragDropEffects.None;
                //    e.Handled = false;
                //}
                //else if (typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()) && (e.DropPosition == DropPosition.Before || e.DropPosition == DropPosition.After))//[Sudhir.Jangra][GEOS2-4221][05/04/2023]
                //{
                //    e.Effects = DragDropEffects.Move;
                //    e.Handled = false;
                //}
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamilyGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSparePartsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropFamily(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFamily()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-4098][rdixit][28.12.2022]          				
                //[GEOS2-4957][rajashri[19-10-2023]
                e.Handled = true;
                // IsEnabled = true;
                if (Families != null)
                    Families = new ObservableCollection<ConnectorFamilies>(Families.GroupBy(opt => opt.IdFamily).Select(g => g.First()));

                if (Families != null)
                {
                    foreach (var item in Families)
                    {
                        FamilyMenulist.Where(a => a.IdFamily == item.IdFamily).ToList().ForEach(b => b.IsCurrentFamily = true);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFamily()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverCustomers(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverCustomers()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Customer).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverCustomers()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverCustomers() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OptionsCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OptionsCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is Options)
                {
                    Options option = e as Options;
                    Options.Remove(option);
                }
                GeosApplication.Instance.Logger.Log("Method OptionsCancelAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OptionsCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void WayCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WayCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is Ways)
                {
                    Ways way = e as Ways;
                    Ways.Remove(way);
                }
                GeosApplication.Instance.Logger.Log("Method WayCancelAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method WayCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DetectionsCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionsCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is Detections)
                {
                    Detections detection = e as Detections;
                    Detections.Remove(detection);
                }
                GeosApplication.Instance.Logger.Log("Method DetectionsCancelAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DetectionsCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SparePartsCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SparePartsCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is SpareParts)
                {
                    SpareParts part = e as SpareParts;
                    SpareParts.Remove(part);
                }
                GeosApplication.Instance.Logger.Log("Method SparePartsCancelAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SparePartsCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FamilyCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FamilyCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is ConnectorFamilies)
                {
                    ConnectorFamilies connectorFamilies = e as ConnectorFamilies;
                    Families.Remove(connectorFamilies);
                }
                GeosApplication.Instance.Logger.Log("Method FamilyCancelAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FamilyCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


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
                        List<ProductTypeCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ProductTypeCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 249, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
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
                        List<ProductTypeCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ProductTypeCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 249, Code = x.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
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
                    List<ProductTypeCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ProductTypeCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 249, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();

                    if (newRecords.Count > 0)
                    {
                        MandatoryList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                        IsEnabledCancelButton = true;
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ProductTypeCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ProductTypeCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 249, Code = x.Reference, Name = x.Description, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
                    if (newRecords.Count > 0)
                    {
                        MandatoryList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                        IsEnabledCancelButton = true;
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
                IsEnabledCancelButton = true;
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
                        List<ProductTypeCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ProductTypeCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 250, Code = x.ProductType.Reference, Name = x.Name, Remarks = "" }).ToList();
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
                        List<ProductTypeCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ProductTypeCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 250, Code = x.Reference, Name = x.Name, Remarks = "" }).ToList();
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
                    List<ProductTypeCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ProductTypeCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 250, Code = x.ProductType.Reference, Name = x.Name, Remarks = "" }).ToList();
                    if (newRecords.Count > 0)
                    {
                        SuggestedList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                        IsEnabledCancelButton = true;
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ProductTypeCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ProductTypeCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 250, Code = x.Reference, Name = x.Description, Remarks = "" }).ToList();
                    if (newRecords.Count > 0)
                    {
                        SuggestedList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                        IsEnabledCancelButton = true;
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
                IsEnabledCancelButton = true;
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
                        List<ProductTypeCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ProductTypeCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 251, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251, Quantity = null }).ToList();
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
                        List<ProductTypeCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ProductTypeCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 251, Code = x.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 246 }).ToList();
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
                    List<ProductTypeCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ProductTypeCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 251, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251, Quantity = null }).ToList();

                    if (newRecords.Count > 0)
                    {
                        IncompatibleList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                        IsEnabledCancelButton = true;
                    }
                }
                else if ((e.IsFromOutside) && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<ProductTypeCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ProductTypeCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 251, Code = x.Reference, Name = x.Description, Remarks = "", IdRelationshipType = 251 }).ToList();
                    if (newRecords.Count > 0)
                    {
                        IncompatibleList.Add(newRecords.FirstOrDefault());
                        CompatibilityCount = CompatibilityCount + 1;
                        e.Handled = true;
                        IsEnabledCancelButton = true;
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
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropIncompatibleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropIncompatibleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method
        /// [001][cpatil][20-06-2022][GEOS2-3787]
        public void Init(int IdScope)
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

                IsDuplicateModulesButtonVisible = Visibility.Visible;
                IsDuplicateModulesButtonEnabled = true;

                MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

                /*
                FillTemplateList();
                AddOptionsMenu(IdScope);
                AddWaysMenu();
                AddDetectionsMenu(IdScope);
                AddSparePartsMenu();
                AddFamilyMenu();
                AddLanguages();
                AddChangeLogsMenu();
                FillStatusList();
                FillCode();
                FillLastUpdateCreatedDate();
                FillDefaultWayTypeList();
                FillCustomersList();

                FillModuleMenuList();
                FillArticleMenuList();
                FillReferenceView();
                FillRelationShipList();
                */
				//[nsatpute][19-05-2025][GEOS2-6691]
                var tasks = new List<Task>();

                // Run all methods in parallel
                tasks.Add(Task.Run(() => FillTemplateList()));
                tasks.Add(Task.Run(() => AddOptionsMenu(IdScope)));
                tasks.Add(Task.Run(() => AddWaysMenu()));
                tasks.Add(Task.Run(() => AddDetectionsMenu(IdScope)));
                tasks.Add(Task.Run(() => AddSparePartsMenu()));
                tasks.Add(Task.Run(() => AddFamilyMenu()));
                tasks.Add(Task.Run(() => AddLanguages()));
                tasks.Add(Task.Run(() => AddChangeLogsMenu()));
                tasks.Add(Task.Run(() => FillStatusList()));
                tasks.Add(Task.Run(() => FillCode()));
                tasks.Add(Task.Run(() => FillLastUpdateCreatedDate()));
                tasks.Add(Task.Run(() => FillDefaultWayTypeList()));
                tasks.Add(Task.Run(() => FillCustomersList()));
                tasks.Add(Task.Run(() => FillModuleMenuList()));
                tasks.Add(Task.Run(() => FillArticleMenuList()));
                tasks.Add(Task.Run(() => FillReferenceView()));
                tasks.Add(Task.Run(() => FillRelationShipList()));
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                tasks.Add(Task.Run(() => ProductTypesDetails = PCMServiceThreadLocal.GetProductTypeByIdCpType_V2640(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate, GeosApplication.Instance.ActiveUser.IdUser))); //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
                Task.WaitAll(tasks.ToArray());

                #region
                //ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2250(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate);
                // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 10 01 2023
                // ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2350(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate);
                //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
                //[sudhir.jangra][geos2-4221]
                // ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2390(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate);
                //[Sudhir.Jangra][GEOS2-4935]
                //ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2470(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate);
                //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
                //PCMService = new PCMServiceController("localhost:6699");
                // ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2490(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate, GeosApplication.Instance.ActiveUser.IdUser);
                //Updated service from GetProductTypeByIdCpType_V2530 to GetProductTypeByIdCpType_V2590 by [rdixit][GEOS2-6624][10.12.2024]
                #endregion

                

                if (ProductTypesDetails.IncludedPLMModuleList != null)
                    ProductTypesDetails.IncludedPLMModuleList.RemoveAll(r => r.IdPermission == null);
                if (ProductTypesDetails.NotIncludedPLMModuleList != null)
                    ProductTypesDetails.NotIncludedPLMModuleList.RemoveAll(r => r.IdPermission == null);
                ClonedProductTypes = (ProductTypes)ProductTypesDetails.Clone();
                InitTheIncludedAndNotIncludedPriceList(ProductTypesDetails);

                CommentsList = new ObservableCollection<ProductTypeLogEntry>(ProductTypesDetails.ProductTypeCommentList.OrderByDescending(x => x.Datetime));
                if (CommentsList.Count > 0)
                {
                    CommentText = CommentsList.FirstOrDefault().Comments;
                    CommentDateTimeText = CommentsList.FirstOrDefault().Datetime;
                    CommentFullNameText = CommentsList.FirstOrDefault().UserName;
                }
                else
                {
                    CommentText = string.Empty; // or some default value if there are no comments
                    CommentDateTimeText = null;
                    CommentFullNameText =string.Empty;
                }

                SetUserProfileImage(CommentsList);

                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();
                //[001]
                //PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegions_V2280(ProductTypeItem.IdCPType));

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegions_V2530(ProductTypeItem.IdCPType));

                if (PCMCustomerList != null && PCMCustomerList.Count > 0)
                {
                    Group = (from x in PCMCustomerList select x.GroupName).Distinct().Count();
                    Region = (from x in PCMCustomerList select x.RegionName).Distinct().Count();
                    Country = (from x in PCMCustomerList select x.Country.Name).Distinct().Count();
                    Plant = (from x in PCMCustomerList select x.Plant.Name).Distinct().Count();

                    Groups = String.Join(", ", PCMCustomerList.Select(a => a.GroupName).Distinct());
                    Regions = String.Join(", ", PCMCustomerList.Select(a => a.RegionName).Distinct());
                    Countries = String.Join(", ", PCMCustomerList.Select(a => a.Country.Name).Distinct());
                    Plants = String.Join(", ", PCMCustomerList.Select(a => a.Plant.Name).Distinct());

                    IncludedCustomerList = new ObservableCollection<CPLCustomer>(PCMCustomerList.Where(i => i.IsIncluded == 1).ToList());
                    NotIncludedCustomerList = new ObservableCollection<CPLCustomer>(PCMCustomerList.Where(i => i.IsIncluded == 0).ToList());
                }

                ClonedProductType = (ProductTypes)ProductTypesDetails.Clone();
                ClonedProductType.CustomerListByCPType = PCMCustomerList;
                ModifyBy = ProductTypesDetails.ModifiedBy;

                if (ProductTypesDetails.LastUpdate != null)
                {
                    LastUpdate = (DateTime)ProductTypesDetails.LastUpdate;
                }
                else
                {
                    LastUpdate = null;
                }
                ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                Name = ProductTypesDetails.Name;
                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                Name_en = ProductTypesDetails.Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                Name_es = ProductTypesDetails.Name_es;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                Name_fr = ProductTypesDetails.Name_fr;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                Name_pt = ProductTypesDetails.Name_pt;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                Name_ro = ProductTypesDetails.Name_ro;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                Name_ru = ProductTypesDetails.Name_ru;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                Name_zh = ProductTypesDetails.Name_zh;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;


                Description = ProductTypesDetails.Description;
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                Description_en = ProductTypesDetails.Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                Description_es = ProductTypesDetails.Description_es;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                Description_fr = ProductTypesDetails.Description_fr;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                Description_pt = ProductTypesDetails.Description_pt;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                Description_ro = ProductTypesDetails.Description_ro;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                Description_ru = ProductTypesDetails.Description_ru;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                Description_zh = ProductTypesDetails.Description_zh;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                if (ProductTypesDetails.CreatedIn != null)
                {
                    SelectedCreated = ProductTypesDetails.CreatedIn.Value;
                }
                else
                {
                    SelectedCreated = null;
                }

                Reference = ProductTypesDetails.Reference;
                Abbrivation = ProductTypesDetails.Code;


                if (SelectedDefaultWayType == null && ProductTypesDetails.WayList.Count() == 0)
                    IsEnabled = false;
                else
                    IsEnabled = true;

                if (ProductTypesDetails.IdStatus > 0)
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == ProductTypesDetails.IdStatus);
                else
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 225);

                SelectedTemplate = TemplatesMenuList.FirstOrDefault(x => x.IdTemplate == ProductTypesDetails.IdTemplate);


                Families = new ObservableCollection<ConnectorFamilies>(ProductTypesDetails.FamilyList);
                foreach (ConnectorFamilies item in Families.Where(i=>i.IdSubFamilyConnector>0)?.ToList())
                {
                    FamilyMenulist.Where(a => a.IdSubFamilyConnector == item.IdSubFamilyConnector).ToList().ForEach(b => b.IsCurrentFamily = true);                  
                }

                FamilyMenulist = new ObservableCollection<ConnectorFamilies>(FamilyMenulist);
                Ways = new ObservableCollection<Ways>(ProductTypesDetails.WayList);
                foreach (Ways item in Ways)
                {
                    WaysMenulist.Where(a => a.IdWays == item.IdWays).ToList().ForEach(b => b.IsCurrentWays = true);
                }
                SelectedDefaultWayType = ProductTypesDetails.WayList.FirstOrDefault(x => x.IdWays == ProductTypesDetails.IdDefaultWayType);

                Detections = new ObservableCollection<Detections>(ProductTypesDetails.DetectionList_Group);
                //[001]
                foreach (Detections item in Detections)
                {
                    if (item.IdDetections > 0)
                        DetectionsMenulist.Where(a => a.IdDetections == item.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = true);
                    else
                    {
                        foreach (Detections item1 in DetectionsMenulist.Where(x => x.Parent == item.Key))
                        {
                            if (Detections.Any(x => x.Key == item1.Key))
                            {
                                DetectionsMenulist.Where(a => a.IdDetections == item1.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = true);
                            }
                        }
                    }
                }
                Options = new ObservableCollection<Options>(ProductTypesDetails.OptionList_Group);
                //[001]
                foreach (Options item in Options)
                {
                    if (item.IdOptions > 0)
                        OptionsMenulist.Where(a => a.IdOptions == item.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = true);
                    else
                    {
                        foreach (Options item1 in OptionsMenulist.Where(x => x.Parent == item.Key))
                        {
                            if (SpareParts.Any(x => x.Key == item1.Key))
                            {
                                OptionsMenulist.Where(a => a.IdOptions == item1.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = true);
                            }
                        }
                    }
                }
                SpareParts = new ObservableCollection<SpareParts>(ProductTypesDetails.SparePartsList_Group);
                //[001]
                foreach (SpareParts item in SpareParts)
                {
                    if (item.IdSpareParts > 0)
                        SparePartsMenulist.Where(a => a.IdSpareParts == item.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = true);
                    else
                    {
                        foreach (SpareParts item1 in SparePartsMenulist.Where(x => x.Parent == item.Key))
                        {
                            if (SpareParts.Any(x => x.Key == item1.Key))
                            {
                                SparePartsMenulist.Where(a => a.IdSpareParts == item1.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = true);
                            }
                        }
                    }
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                    Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyDescription = true;
                }
                else
                {
                    IsCheckedCopyDescription = false;
                }

                ImagesList = new ObservableCollection<ProductTypeImage>(ProductTypesDetails.ProductTypeImageList);

                if (ImagesList.Count > 0)
                {
                    List<ProductTypeImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                    List<ProductTypeImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    ProductTypeImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = ImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);
                    SelectedDefaultImage = new ProductTypeImage();
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);

                    SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                }

                if (Options.Count > 0)
                {
                    Options_count = Options.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                    Options_Group_count = Options.Where(a => a.Parent == null && a.IdGroup != null).Count();
                }

                if (Detections.Count > 0)
                {
                    Detections_count = Detections.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                    Detections_group_count = Detections.Where(a => a.Parent == null && a.IdGroup != null).Count();
                }

                if (SpareParts.Count > 0)
                {
                    SpareParts_count = SpareParts.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                    SpareParts_group_count = SpareParts.Where(a => a.Parent == null && a.IdGroup != null).Count();
                }

                AddFiles(ProductTypesDetails.IdCPType);
                AddLinks(ProductTypesDetails.IdCPType);

                ProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypesDetails.ProductTypeAttachedDocList);
                if (ProductTypeFilesList.Count > 0)
                    SelectedProductTypeFile = ProductTypeFilesList.FirstOrDefault();

                ImagesList = new ObservableCollection<ProductTypeImage>(ProductTypesDetails.ProductTypeImageList);
                if (ImagesList.Count > 0)
                    SelectedImage = ImagesList.FirstOrDefault();

                foreach (ProductTypeImage item in ImagesList)
                {
                    item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(item.ProductTypeImageInBytes);
                }

                ProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypesDetails.ProductTypeAttachedLinkList);
                if (ProductTypeLinksList.Count > 0)
                    SelectedProductTypeLink = ProductTypeLinksList.FirstOrDefault();

                ProductTypesChangeLogList = new ObservableCollection<ProductTypeLogEntry>(ProductTypesDetails.ProductTypeLogEntryList);
                if (ProductTypesChangeLogList.Count > 0)
                    SelectedProductTypesChangeLog = ProductTypesChangeLogList.FirstOrDefault();
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3"
                        && GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (ProductTypeFilesList.Count() >= 5)
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
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).ToList());
                    }
                }
                else
                {
                    FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(4).ToList());
                }


                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][GEOS2-1960][03/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3" &&
                        GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (ProductTypeLinksList.Count() >= 5)
                        {
                            IsLinkScrollVisible = "Visible";
                        }
                        else
                        {
                            IsLinkScrollVisible = "Disabled";
                        }
                    }
                    else
                    {
                        IsLinkScrollVisible = "Disabled";
                    }
                    if (GeosApplication.Instance.PCMLinks == "1")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).ToList());
                    }
                }
                else
                {
                    FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(4).ToList());
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
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                else
                {
                    FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                }
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(ProductTypesDetails.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(ProductTypesDetails.CustomerList.Where(a => a.IsChecked == true));

                var List = CustomersMenuList.GroupBy(info => info.IdRegion)
                            .Select(group => new
                            {
                                RegionName = CustomersMenuList.FirstOrDefault(a => a.IdRegion == group.Key).RegionName,
                                Count = CustomersMenuList.Where(b => b.IsChecked == true && b.IdRegion == group.Key).Count(),
                            }).ToList();

                List<FourRegionsWithCustomerCount> FourRecordsList = new List<FourRegionsWithCustomerCount>();

                foreach (var record in List)
                {
                    FourRegionsWithCustomerCount fourRegionsWithCustomerCount = new FourRegionsWithCustomerCount();
                    fourRegionsWithCustomerCount.RegionName = record.RegionName;
                    fourRegionsWithCustomerCount.Count = record.Count;
                    FourRecordsList.Add(fourRegionsWithCustomerCount);
                }

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList.ToList());

                //Compatiblity
                if (ProductTypesDetails.ProductTypeCompatibilityList != null)
                {
                    MandatoryList = new ObservableCollection<ProductTypeCompatibility>(ProductTypesDetails.ProductTypeCompatibilityList.Where(a => a.IdTypeCompatibility == 249));
                    SuggestedList = new ObservableCollection<ProductTypeCompatibility>(ProductTypesDetails.ProductTypeCompatibilityList.Where(a => a.IdTypeCompatibility == 250));
                    IncompatibleList = new ObservableCollection<ProductTypeCompatibility>(ProductTypesDetails.ProductTypeCompatibilityList.Where(a => a.IdTypeCompatibility == 251));
                }
                else
                {
                    MandatoryList = new ObservableCollection<ProductTypeCompatibility>();
                    SuggestedList = new ObservableCollection<ProductTypeCompatibility>();
                    IncompatibleList = new ObservableCollection<ProductTypeCompatibility>();
                }

                GetCompatibilityCount();

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
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Error in ShowReferenceViewCommandAction Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CellValueChangedCommandAction(object obj)
        {
            var List = ClonedProductType.CustomerList.Where(x => x.IsChecked == true).ToList();
            var List1 = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

            RegionsByCustomer customer = (RegionsByCustomer)obj;
            if ((List.Any((a => (a.IdGroup == customer.IdGroup && a.IdRegion == customer.IdRegion)))))
            {
                if (customer.IsChecked)
                {
                    IsExportVisible = Visibility.Visible;
                }
                else
                {
                    IsExportVisible = Visibility.Hidden;
                }
            }
            else
            {
                IsExportVisible = Visibility.Hidden;
            }
        }

        private void CustomerHeaderClickCommandAction(object obj)
        {
            var List = ClonedProductType.CustomerList.Where(x => x.IsChecked == true).ToList();
            var List1 = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

            if (obj == null)
            {
                if (List.Count() != List1.Count())
                {
                    IsExportVisible = Visibility.Hidden;
                }
                else
                {
                    IsExportVisible = Visibility.Visible;
                }
            }
            else
            {
                RegionsByCustomer customer = (RegionsByCustomer)obj;
                if (List.Any(a => a.IdGroup == customer.IdGroup && a.IdRegion == customer.IdRegion && a.IsChecked == customer.IsChecked) || List.Count() == List1.Count())
                {
                    HashSet<string> IdUniques = new HashSet<string>(List.Select(s => s.UniqueId));

                    var results = List1.Where(m => !IdUniques.Contains(m.UniqueId));
                    if (results.Count() > 0)
                    {
                        IsExportVisible = Visibility.Hidden;
                    }
                    else
                    {
                        IsExportVisible = Visibility.Visible;
                    }
                }
                else
                    IsExportVisible = Visibility.Hidden;
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillTemplateList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTemplateList()...", category: Category.Info, priority: Priority.Low);

                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                TemplatesMenuList = new ObservableCollection<Template>(PCMServiceThreadLocal.GetAllTemplates());

                GeosApplication.Instance.Logger.Log("Method FillTemplateList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTemplateList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        public void AddOptionsMenu(int IdScope)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddOptionsMenu()...", category: Category.Info, priority: Priority.Low);

                Options = new ObservableCollection<Options>();
                //OptionsMenulist = new ObservableCollection<Options>(PCMService.GetAllOptionsWithGroups());
                // shubham[skadam] GEOS2-3778 In the modules window, can not be Shown the DOWS with  IsEnabled = 0 in the left menu. 12 Aug 2022 
                // OptionsMenulist = new ObservableCollection<Options>(PCMService.GetAllOptionsWithGroups_V2300());
                //[Sudhir.Jangra][GEOS2-4733][22/08/2023]
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                OptionsMenulist = new ObservableCollection<Options>(PCMServiceThreadLocal.GetAllOptionsWithGroups_V2430(IdScope));
                GeosApplication.Instance.Logger.Log("Method AddOptionsMenu()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddOptionsMenu() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddOptionsMenu() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddOptionsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        public void AddWaysMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddWaysMenu()...", category: Category.Info, priority: Priority.Low);

                Ways = new ObservableCollection<Ways>();
                //WaysMenulist = new ObservableCollection<Ways>(PCMService.GetAllWayList());
                // shubham[skadam] GEOS2-3778 In the modules window, can not be Shown the DOWS with  IsEnabled = 0 in the left menu. 12 Aug 2022 
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WaysMenulist = new ObservableCollection<Ways>(PCMServiceThreadLocal.GetAllWayList_V2300());

                GeosApplication.Instance.Logger.Log("Method AddWaysMenu()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddWaysMenu() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddWaysMenu() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddWaysMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        public void AddDetectionsMenu(int IdScope)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDetectionsMenu()...", category: Category.Info, priority: Priority.Low);

                Detections = new ObservableCollection<Detections>();
                //DetectionsMenulist = new ObservableCollection<Detections>(PCMService.GetAllDetectionsWithGroups());
                // shubham[skadam] GEOS2-3778 In the modules window, can not be Shown the DOWS with  IsEnabled = 0 in the left menu. 12 Aug 2022 
                //DetectionsMenulist = new ObservableCollection<Detections>(PCMService.GetAllDetectionsWithGroups_V2300());
                //[Sudhir.Jangra][GEOS2-4733][22/08/2023]
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                DetectionsMenulist = new ObservableCollection<Detections>(PCMServiceThreadLocal.GetAllDetectionsWithGroups_V2430(IdScope));
                GeosApplication.Instance.Logger.Log("Method AddDetectionsMenu()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDetectionsMenu() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDetectionsMenu() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddDetectionsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        public void AddSparePartsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSparePartsMenu()...", category: Category.Info, priority: Priority.Low);

                SpareParts = new ObservableCollection<SpareParts>();
                //SparePartsMenulist = new ObservableCollection<SpareParts>(PCMService.GetAllSparepartsWithGroups());
                // shubham[skadam] GEOS2-3778 In the modules window, can not be Shown the DOWS with  IsEnabled = 0 in the left menu. 12 Aug 2022 
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                SparePartsMenulist = new ObservableCollection<SpareParts>(PCMServiceThreadLocal.GetAllSparepartsWithGroups_V2300());

                GeosApplication.Instance.Logger.Log("Method AddSparePartsMenu()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddSparePartsMenu() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddSparePartsMenu() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddSparePartsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        public void AddFamilyMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFamilyMenu()...", category: Category.Info, priority: Priority.Low);
                //[rdixit][GEOS2-6624][10.12.2024]
                Families = new ObservableCollection<ConnectorFamilies>();
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                FamilyMenulist = new ObservableCollection<ConnectorFamilies>(PCMServiceThreadLocal.GetAllFamiliesWithSubFamily_V2590());
                SelectedFamilyAutoHide = FamilyMenulist?.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method AddFamilyMenu()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddFamilyMenu() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddFamilyMenu() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddFamilyMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        public void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                Languages = new ObservableCollection<Language>(PCMServiceThreadLocal.GetAllLanguages());
                LanguageSelected = Languages.FirstOrDefault();

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

        public void AddFiles(ulong IdCpType)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFiles()...", category: Category.Info, priority: Priority.Low);

                ProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>();

                GeosApplication.Instance.Logger.Log("Method AddFiles()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddFiles() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddLinks(ulong IdCpType)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLinks()...", category: Category.Info, priority: Priority.Low);

                ProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>();

                GeosApplication.Instance.Logger.Log("Method AddLinks()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddLinks() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddChangeLogsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangeLogsMenu()...", category: Category.Info, priority: Priority.Low);

                ProductTypesChangeLogList = new ObservableCollection<ProductTypeLogEntry>();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddChangeLogsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                IList<LookupValue> tempStatusList = PCMServiceThreadLocal.GetLookupValues(45);
                StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatusIndex = StatusList.FindIndex(x => x.IdLookupValue == 225);

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCode()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                if (!isDuplicateClicked)
                    Reference = PCMServiceThreadLocal.GetLatestProuductTypeReference();
                else
                    DuplicateCode = PCMServiceThreadLocal.GetLatestProuductTypeReference();

                GeosApplication.Instance.Logger.Log("Method FillCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCode() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCode() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillLastUpdateCreatedDate()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLastUpdateCreatedDate()...", category: Category.Info, priority: Priority.Low);

                SelectedCreated = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                GeosApplication.Instance.Logger.Log("Method FillLastUpdateCreatedDate()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillLastUpdateCreatedDate() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillDefaultWayTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDefaultWayTypeList()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                DefaultWayTypeList = new ObservableCollection<DefaultWayType>(PCMServiceThreadLocal.GetAllDefaultWayTypeList());
                SelectedDefaultWayType = Ways.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method FillDefaultWayTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDefaultWayTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDefaultWayTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDefaultWayTypeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillCustomersList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomersList()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(PCMServiceThreadLocal.GetCustomersWithRegions());

                GeosApplication.Instance.Logger.Log("Method FillCustomersList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomersList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomersList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCustomersList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillModuleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillModuleMenuList()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ModuleMenulist = new ObservableCollection<ProductTypesTemplate>(PCMServiceThreadLocal.GetProductTypesWithTemplate());

                SelectedModule = ModuleMenulist.FirstOrDefault();

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
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillArticleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleMenuList()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PCMServiceThreadLocal.GetPCMArticlesWithCategory_V2290());//[rdixit][GEOS2-2571][08.07.2022]
                SelectedArticle = ArticleMenuList.FirstOrDefault();
                foreach (PCMArticleCategory category in ArticleMenuList)
                {
                    if (category.InUse == "NO")
                        category.IspcmCategoryNOTInUse = true;
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
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillReferenceView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReferenceView()...", category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PCMServiceThreadLocal.GetPCMArticlesWithCategoryForReference_V2290());//[rdixit][GEOS2-2571][07.07.2022]
                SelectedArticle = ArticleMenuList.FirstOrDefault();
                foreach (PCMArticleCategory category in ArticleMenuList)//[rdixit][GEOS2- 2571][07.07.2022][added field pcmCategoryInUse]
                {
                    if (category.InUse == "NO")
                        category.IspcmCategoryNOTInUse = true;
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
		//[nsatpute][19-05-2025][GEOS2-6691]
        private void FillRelationShipList()
        {
            try
            {

                GeosApplication.Instance.Logger.Log(string.Format("Method FillRelationShipList..."), category: Category.Info, priority: Priority.Low);
                IPCMService PCMServiceThreadLocal = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                IList<LookupValue> tempRelationShipList = PCMServiceThreadLocal.GetLookupValues(50);
                RelationShipList = new List<LookupValue>();
                RelationShipList = new List<LookupValue>(tempRelationShipList);
                SelectedRelationShip = RelationShipList.FirstOrDefault();

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

        private void RetrieveDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()...", category: Category.Info, priority: Priority.Low);
                UpdateProductTypes = new ProductTypes();

                if (IsCheckedCopyDescription == false)
                {

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

                    // IsEnabledCancelButton = false;
                }

                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveDescriptionByLanguge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetDescriptionToLanguage()...", category: Category.Info, priority: Priority.Low);
                UpdateProductTypes = new ProductTypes();

                if (IsCheckedCopyDescription == false && LanguageSelected != null)
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
                if (IsCheckedCopyDescription == false && LanguageSelected != null)
                {
                    if ((oldDescription_en != Description_en && Description_en != "" && !string.IsNullOrEmpty(Description_en)) ||
                   (oldDescription_es != Description_es && Description_es != "" && !string.IsNullOrEmpty(Description_es)) || (oldDescription_fr != Description_fr && Description_fr != "" && !string.IsNullOrEmpty(Description_fr)) ||
                  (oldDescription_pt != Description_pt && Description_pt != "" && !string.IsNullOrEmpty(Description_pt)) || (oldDescription_ro != Description_ro && Description_ro != "" && !string.IsNullOrEmpty(Description_ro)) ||
                           (oldDescription_ru != Description_ru && Description_ru != "" && !string.IsNullOrEmpty(Description_ru)) || (oldDescription_zh != Description_zh && Description_zh != "" && !string.IsNullOrEmpty(Description_zh)))
                    {
                        IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    }
                    else
                    {
                        IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    }
                }
                else
                {
                    if ((oldDescription_en != Description && Description != "" && !string.IsNullOrEmpty(Description)) ||
                 (oldDescription_es != Description_es && Description_es != "" && !string.IsNullOrEmpty(Description_es)) || (oldDescription_fr != Description_fr && Description_fr != "" && !string.IsNullOrEmpty(Description_fr)) ||
                (oldDescription_pt != Description_pt && Description_pt != "" && !string.IsNullOrEmpty(Description_pt)) || (oldDescription_ro != Description_ro && Description_ro != "" && !string.IsNullOrEmpty(Description_ro)) ||
                         (oldDescription_ru != Description_ru && Description_ru != "" && !string.IsNullOrEmpty(Description_ru)) || (oldDescription_zh != Description_zh && Description_zh != "" && !string.IsNullOrEmpty(Description_zh)))
                    {
                        IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    }
                    else
                    {
                        IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    }
                }



                GeosApplication.Instance.Logger.Log("Method SetDescriptionToLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetDescriptionToLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()...", category: Category.Info, priority: Priority.Low);
                UpdateProductTypes = new ProductTypes();

                if (IsCheckedCopyDescription == false && LanguageSelected != null)
                {
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
                }
                if (IsCheckedCopyDescription == false && LanguageSelected != null)
                {
                    if ((oldName_en != Name_en && Name_en != "" && !string.IsNullOrEmpty(Name_en)) ||
                   (oldName_es != Name_es && Name_es != "" && !string.IsNullOrEmpty(Name_es)) || (oldName_fr != Name_fr && Name_fr != "" && !string.IsNullOrEmpty(Name_fr)) ||
                  (oldName_pt != Name_pt && Name_pt != "" && !string.IsNullOrEmpty(Name_pt)) || (oldName_ro != Name_ro && Name_ro != "" && !string.IsNullOrEmpty(Name_ro)) ||
                  (oldName_ru != Name_ru && Name_ru != "" && !string.IsNullOrEmpty(Name_ru)) || (oldName_zh != Name_zh && Name_zh != "" && !string.IsNullOrEmpty(Name_zh)))
                    {
                        IsEnabledCancelButton = true;
                    }
                    else
                    {
                        IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    }
                }
                else
                {
                    if ((oldName_en != Name && Name != "" && !string.IsNullOrEmpty(Name)) ||
                   (oldName_es != Name_es && Name_es != "" && !string.IsNullOrEmpty(Name_es)) || (oldName_fr != Name_fr && Name_fr != "" && !string.IsNullOrEmpty(Name_fr)) ||
                  (oldName_pt != Name_pt && Name_pt != "" && !string.IsNullOrEmpty(Name_pt)) || (oldName_ro != Name_ro && Name_ro != "" && !string.IsNullOrEmpty(Name_ro)) ||
                  (oldName_ru != Name_ru && Name_ru != "" && !string.IsNullOrEmpty(Name_ru)) || (oldName_zh != Name_zh && Name_zh != "" && !string.IsNullOrEmpty(Name_zh)))
                    {
                        IsEnabledCancelButton = true;
                    }
                    else
                    {
                        IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    }
                }


                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetNameToLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UncheckedCopyDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()...", category: Category.Info, priority: Priority.Low);

                if (LanguageSelected != null)
                {
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
                }

                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UncheckedCopyDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewOption(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewOption()...", category: Category.Info, priority: Priority.Low);

                AddDetectionView addEditOptionsDetectionsView = new AddDetectionView();
                AddDetectionViewModel addEditOptionsDetectionsViewModel = new AddDetectionViewModel();
                EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                addEditOptionsDetectionsViewModel.RequestClose += handle;
                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddOptionHeader").ToString();
                addEditOptionsDetectionsViewModel.IsNew = true;
                addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());
                addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                addEditOptionsDetectionsView.ShowDialog();

                if (addEditOptionsDetectionsViewModel.IsOptionSave)
                {
                    Options tempOption = new Options();
                    tempOption.IdOptions = addEditOptionsDetectionsViewModel.NewOption.IdDetections;
                    tempOption.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewOption.IdDetectionType;
                    tempOption.Key = addEditOptionsDetectionsViewModel.NewOption.IdDetections.ToString();
                    tempOption.Name = addEditOptionsDetectionsViewModel.NewOption.Name;
                    tempOption.Code = addEditOptionsDetectionsViewModel.NewOption.Code;
                    tempOption.IdTestType = addEditOptionsDetectionsViewModel.NewOption.IdTestType;
                    tempOption.IdGroup = addEditOptionsDetectionsViewModel.NewOption.IdGroup;
                    tempOption.Parent = "Group_" + tempOption.IdGroup;
                    OptionsMenulist.Add(tempOption);
                    OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsDetectionSave)
                {
                    Detections tempDetection = new Detections();
                    tempDetection.IdDetections = addEditOptionsDetectionsViewModel.NewDetection.IdDetections;
                    tempDetection.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewDetection.IdDetectionType;
                    tempDetection.Key = addEditOptionsDetectionsViewModel.NewDetection.IdDetections.ToString();
                    tempDetection.Name = addEditOptionsDetectionsViewModel.NewDetection.Name;
                    tempDetection.Code = addEditOptionsDetectionsViewModel.NewDetection.Code;
                    tempDetection.IdTestType = addEditOptionsDetectionsViewModel.NewDetection.IdTestType;
                    tempDetection.IdGroup = addEditOptionsDetectionsViewModel.NewDetection.IdGroup;
                    tempDetection.Parent = "Group_" + tempDetection.IdGroup;
                    DetectionsMenulist.Add(tempDetection);
                    DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsWaySave)
                {
                    Ways tempWay = new Ways();
                    tempWay.IdWays = addEditOptionsDetectionsViewModel.NewWay.IdDetections;
                    tempWay.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewWay.IdDetectionType;
                    tempWay.Name = addEditOptionsDetectionsViewModel.NewWay.Name;
                    tempWay.Code = addEditOptionsDetectionsViewModel.NewWay.Code;
                    tempWay.IdTestType = addEditOptionsDetectionsViewModel.NewWay.IdTestType;
                    WaysMenulist.Add(tempWay);
                }
                if (addEditOptionsDetectionsViewModel.IsSparepartSave)
                {
                    SpareParts tempSparePart = new SpareParts();
                    tempSparePart.IdSpareParts = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections;
                    tempSparePart.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewSparePart.IdDetectionType;
                    tempSparePart.Key = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections.ToString();
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    tempSparePart.IdGroup = addEditOptionsDetectionsViewModel.NewSparePart.IdGroup;
                    tempSparePart.Parent = "Group_" + tempSparePart.IdGroup;
                    SparePartsMenulist.Add(tempSparePart);
                    SparePartsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                GeosApplication.Instance.Logger.Log("Method AddNewOption()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewWay(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewWay()...", category: Category.Info, priority: Priority.Low);

                AddDetectionView addEditOptionsDetectionsView = new AddDetectionView();
                AddDetectionViewModel addEditOptionsDetectionsViewModel = new AddDetectionViewModel();
                EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                addEditOptionsDetectionsViewModel.RequestClose += handle;
                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddWayHeader").ToString();
                addEditOptionsDetectionsViewModel.IsNew = true;
                addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Collapsed;
                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionWay").ToString());
                addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                addEditOptionsDetectionsView.ShowDialog();
                if (addEditOptionsDetectionsViewModel.IsWaySave)
                {
                    Ways tempWay = new Ways();
                    tempWay.IdWays = addEditOptionsDetectionsViewModel.NewWay.IdDetections;
                    tempWay.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewWay.IdDetectionType;
                    tempWay.Name = addEditOptionsDetectionsViewModel.NewWay.Name;
                    tempWay.Code = addEditOptionsDetectionsViewModel.NewWay.Code;
                    tempWay.IdTestType = addEditOptionsDetectionsViewModel.NewWay.IdTestType;
                    WaysMenulist.Add(tempWay);
                }
                if (addEditOptionsDetectionsViewModel.IsOptionSave)
                {
                    Options tempOption = new Options();
                    tempOption.IdOptions = addEditOptionsDetectionsViewModel.NewOption.IdDetections;
                    tempOption.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewOption.IdDetectionType;
                    tempOption.Key = addEditOptionsDetectionsViewModel.NewOption.IdDetections.ToString();
                    tempOption.Name = addEditOptionsDetectionsViewModel.NewOption.Name;
                    tempOption.Code = addEditOptionsDetectionsViewModel.NewOption.Code;
                    tempOption.IdTestType = addEditOptionsDetectionsViewModel.NewOption.IdTestType;
                    tempOption.IdGroup = addEditOptionsDetectionsViewModel.NewOption.IdGroup;
                    tempOption.Parent = "Group_" + tempOption.IdGroup;
                    OptionsMenulist.Add(tempOption);
                    OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsDetectionSave)
                {
                    Detections tempDetection = new Detections();
                    tempDetection.IdDetections = addEditOptionsDetectionsViewModel.NewDetection.IdDetections;
                    tempDetection.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewDetection.IdDetectionType;
                    tempDetection.Key = addEditOptionsDetectionsViewModel.NewDetection.IdDetections.ToString();
                    tempDetection.Name = addEditOptionsDetectionsViewModel.NewDetection.Name;
                    tempDetection.Code = addEditOptionsDetectionsViewModel.NewDetection.Code;
                    tempDetection.IdTestType = addEditOptionsDetectionsViewModel.NewDetection.IdTestType;
                    tempDetection.IdGroup = addEditOptionsDetectionsViewModel.NewDetection.IdGroup;
                    tempDetection.Parent = "Group_" + tempDetection.IdGroup;
                    DetectionsMenulist.Add(tempDetection);
                    DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsSparepartSave)
                {
                    SpareParts tempSparePart = new SpareParts();
                    tempSparePart.IdSpareParts = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections;
                    tempSparePart.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewSparePart.IdDetectionType;
                    tempSparePart.Key = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections.ToString();
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    tempSparePart.IdGroup = addEditOptionsDetectionsViewModel.NewSparePart.IdGroup;
                    tempSparePart.Parent = "Group_" + tempSparePart.IdGroup;
                    SparePartsMenulist.Add(tempSparePart);
                    SparePartsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewWay()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewWay() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewDetection(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewDetection()...", category: Category.Info, priority: Priority.Low);

                AddDetectionView addEditOptionsDetectionsView = new AddDetectionView();
                AddDetectionViewModel addEditOptionsDetectionsViewModel = new AddDetectionViewModel();
                EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                addEditOptionsDetectionsViewModel.RequestClose += handle;
                addEditOptionsDetectionsViewModel.IsNew = true;
                addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddDetectionHeader").ToString();
                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());
                addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                addEditOptionsDetectionsView.ShowDialog();

                if (addEditOptionsDetectionsViewModel.IsDetectionSave)
                {
                    Detections tempDetection = new Detections();
                    tempDetection.IdDetections = addEditOptionsDetectionsViewModel.NewDetection.IdDetections;
                    tempDetection.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewDetection.IdDetectionType;
                    tempDetection.Key = addEditOptionsDetectionsViewModel.NewDetection.IdDetections.ToString();
                    tempDetection.Name = addEditOptionsDetectionsViewModel.NewDetection.Name;
                    tempDetection.Code = addEditOptionsDetectionsViewModel.NewDetection.Code;
                    tempDetection.IdTestType = addEditOptionsDetectionsViewModel.NewDetection.IdTestType;
                    tempDetection.IdGroup = addEditOptionsDetectionsViewModel.NewDetection.IdGroup;
                    tempDetection.Parent = "Group_" + tempDetection.IdGroup;
                    DetectionsMenulist.Add(tempDetection);
                    DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }
                if (addEditOptionsDetectionsViewModel.IsOptionSave)
                {
                    Options tempOption = new Options();
                    tempOption.IdOptions = addEditOptionsDetectionsViewModel.NewOption.IdDetections;
                    tempOption.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewOption.IdDetectionType;
                    tempOption.Key = addEditOptionsDetectionsViewModel.NewOption.IdDetections.ToString();
                    tempOption.Name = addEditOptionsDetectionsViewModel.NewOption.Name;
                    tempOption.Code = addEditOptionsDetectionsViewModel.NewOption.Code;
                    tempOption.IdTestType = addEditOptionsDetectionsViewModel.NewOption.IdTestType;
                    tempOption.IdGroup = addEditOptionsDetectionsViewModel.NewOption.IdGroup;
                    tempOption.Parent = "Group_" + tempOption.IdGroup;
                    OptionsMenulist.Add(tempOption);
                    OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsWaySave)
                {
                    Ways tempWay = new Ways();
                    tempWay.IdWays = addEditOptionsDetectionsViewModel.NewWay.IdDetections;
                    tempWay.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewWay.IdDetectionType;
                    tempWay.Name = addEditOptionsDetectionsViewModel.NewWay.Name;
                    tempWay.Code = addEditOptionsDetectionsViewModel.NewWay.Code;
                    tempWay.IdTestType = addEditOptionsDetectionsViewModel.NewWay.IdTestType;
                    WaysMenulist.Add(tempWay);
                }
                if (addEditOptionsDetectionsViewModel.IsSparepartSave)
                {
                    SpareParts tempSparePart = new SpareParts();
                    tempSparePart.IdSpareParts = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections;
                    tempSparePart.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewSparePart.IdDetectionType;
                    tempSparePart.Key = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections.ToString();
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    tempSparePart.IdGroup = addEditOptionsDetectionsViewModel.NewSparePart.IdGroup;
                    tempSparePart.Parent = "Group_" + tempSparePart.IdGroup;
                    SparePartsMenulist.Add(tempSparePart);
                    SparePartsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewDetection()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewSparePart(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewSparePart()...", category: Category.Info, priority: Priority.Low);

                AddDetectionView addEditOptionsDetectionsView = new AddDetectionView();
                AddDetectionViewModel addEditOptionsDetectionsViewModel = new AddDetectionViewModel();
                EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                addEditOptionsDetectionsViewModel.RequestClose += handle;
                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddSparePartHeader").ToString();
                addEditOptionsDetectionsViewModel.IsNew = true;
                addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());
                addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                addEditOptionsDetectionsView.ShowDialog();

                if (addEditOptionsDetectionsViewModel.IsSparepartSave)
                {

                    SpareParts tempSparePart = new SpareParts();
                    tempSparePart.IdSpareParts = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections;
                    tempSparePart.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewSparePart.IdDetectionType;
                    tempSparePart.Key = addEditOptionsDetectionsViewModel.NewSparePart.IdDetections.ToString();
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    tempSparePart.IdGroup = addEditOptionsDetectionsViewModel.NewSparePart.IdGroup;
                    tempSparePart.Parent = "Group_" + tempSparePart.IdGroup;
                    SparePartsMenulist.Add(tempSparePart);
                    SparePartsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsOptionSave)
                {
                    Options tempOption = new Options();
                    tempOption.IdOptions = addEditOptionsDetectionsViewModel.NewOption.IdDetections;
                    tempOption.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewOption.IdDetectionType;
                    tempOption.Key = addEditOptionsDetectionsViewModel.NewOption.IdDetections.ToString();
                    tempOption.Name = addEditOptionsDetectionsViewModel.NewOption.Name;
                    tempOption.Code = addEditOptionsDetectionsViewModel.NewOption.Code;
                    tempOption.IdTestType = addEditOptionsDetectionsViewModel.NewOption.IdTestType;
                    tempOption.IdGroup = addEditOptionsDetectionsViewModel.NewOption.IdGroup;
                    tempOption.Parent = "Group_" + tempOption.IdGroup;
                    OptionsMenulist.Add(tempOption);
                    OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsDetectionSave)
                {
                    Detections tempDetection = new Detections();
                    tempDetection.IdDetections = addEditOptionsDetectionsViewModel.NewDetection.IdDetections;
                    tempDetection.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewDetection.IdDetectionType;
                    tempDetection.Key = addEditOptionsDetectionsViewModel.NewDetection.IdDetections.ToString();
                    tempDetection.Name = addEditOptionsDetectionsViewModel.NewDetection.Name;
                    tempDetection.Code = addEditOptionsDetectionsViewModel.NewDetection.Code;
                    tempDetection.IdTestType = addEditOptionsDetectionsViewModel.NewDetection.IdTestType;
                    tempDetection.IdGroup = addEditOptionsDetectionsViewModel.NewDetection.IdGroup;
                    tempDetection.Parent = "Group_" + tempDetection.IdGroup;
                    DetectionsMenulist.Add(tempDetection);
                    DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                }

                if (addEditOptionsDetectionsViewModel.IsWaySave)
                {
                    Ways tempWay = new Ways();
                    tempWay.IdWays = addEditOptionsDetectionsViewModel.NewWay.IdDetections;
                    tempWay.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.NewWay.IdDetectionType;
                    tempWay.Name = addEditOptionsDetectionsViewModel.NewWay.Name;
                    tempWay.Code = addEditOptionsDetectionsViewModel.NewWay.Code;
                    tempWay.IdTestType = addEditOptionsDetectionsViewModel.NewWay.IdTestType;
                    WaysMenulist.Add(tempWay);
                }

                GeosApplication.Instance.Logger.Log("Method AddNewSparePart()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewSparePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditOption(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditOption()...", category: Category.Info, priority: Priority.Low);
                if (SelectedOption.Parent != null || (SelectedOption.Parent == null && SelectedOption.IdGroup == null))
                {
                    TreeListView detailView = (TreeListView)obj;
                    EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                    EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                    EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                    addEditOptionsDetectionsViewModel.RequestClose += handle;
                    addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOptionHeader").ToString();
                    addEditOptionsDetectionsViewModel.IsNew = false;
                    addEditOptionsDetectionsViewModel.IsSelectedTestReadOnly = true;
                    addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());

                    addEditOptionsDetectionsViewModel.EditInitOptions(SelectedOption);
                    addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                    addEditOptionsDetectionsView.ShowDialog();

                    if (addEditOptionsDetectionsViewModel.IsOptionSave)
                    {
                        //SelectedOption.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                        //SelectedOption.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                        //SelectedOption.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                        //SelectedOption.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                        //SelectedOption.Key = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections.ToString();
                        //SelectedOption.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                        //SelectedOption.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                        //SelectedOption.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                        //SelectedOption.Parent = "Group_" + SelectedOption.IdGroup;
                        //OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        var selectedOptionItem = OptionsMenulist.FirstOrDefault(x => x.IdOptions == SelectedOption.IdOptions);
                        SetUpdatedDataOnUIForOption(selectedOptionItem, addEditOptionsDetectionsViewModel);


                        selectedOptionItem = Options.FirstOrDefault(x => x.IdOptions == SelectedOption.IdOptions);
                        SetUpdatedDataOnUIForOption(selectedOptionItem, addEditOptionsDetectionsViewModel);
                        OptionsMenulist = new ObservableCollection<Options>(PCMService.GetAllOptionsWithGroups_V2300()); //[001][pjadhav][GEOS2-2048]
                        OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        //  Options.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditOption()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void EditOptionForCurrentModule(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditOptionForCurrentModule()...", category: Category.Info, priority: Priority.Low);
                if (SelectedOptionForCurrentModule.Parent != null || (SelectedOptionForCurrentModule.Parent == null && SelectedOptionForCurrentModule.IdGroup == null))
                {
                    TreeListView detailView = (TreeListView)obj;
                    EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                    EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                    EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                    addEditOptionsDetectionsViewModel.RequestClose += handle;
                    addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOptionHeader").ToString();
                    addEditOptionsDetectionsViewModel.IsNew = false;
                    addEditOptionsDetectionsViewModel.IsSelectedTestReadOnly = true;
                    addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());

                    addEditOptionsDetectionsViewModel.EditInitOptions(SelectedOptionForCurrentModule);
                    addEditOptionsDetectionsViewModel.IsEnabledCancelButton = false;
                    addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                    addEditOptionsDetectionsView.ShowDialog();

                    if (addEditOptionsDetectionsViewModel.IsOptionSave)
                    {
                        var selectedOptionItem = OptionsMenulist.FirstOrDefault(x => x.IdOptions == SelectedOptionForCurrentModule.IdOptions);
                        SetUpdatedDataOnUIForOption(selectedOptionItem, addEditOptionsDetectionsViewModel);
                        OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);

                        selectedOptionItem = Options.FirstOrDefault(x => x.IdOptions == SelectedOptionForCurrentModule.IdOptions);
                        SetUpdatedDataOnUIForOption(selectedOptionItem, addEditOptionsDetectionsViewModel);
                        //  Options.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        IsEnabledCancelButton = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditOptionForCurrentModule()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditOptionForCurrentModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void SetUpdatedDataOnUIForOption(Options selectedOptionItem, EditDetectionViewModel addEditOptionsDetectionsViewModel)
        {
            if (selectedOptionItem != null)
            {
                selectedOptionItem.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                selectedOptionItem.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                selectedOptionItem.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                selectedOptionItem.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                selectedOptionItem.Key = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections.ToString();
                selectedOptionItem.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                selectedOptionItem.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                selectedOptionItem.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                selectedOptionItem.Parent = "Group_" + selectedOptionItem.IdGroup;
            }
        }

        private void EditWay(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWay()...", category: Category.Info, priority: Priority.Low);

                EditDetectionView addOptionWayDetectionSparePartView = new EditDetectionView();
                EditDetectionViewModel addOptionWayDetectionSparePartViewModel = new EditDetectionViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditWayHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = false;
                addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Collapsed;
                addOptionWayDetectionSparePartViewModel.Init(System.Windows.Application.Current.FindResource("CaptionWay").ToString());

             //   if (SelectedWaysType == null)
                 //   return;
                
              //  Ways selectedWay = SelectedWaysType.Cast<Ways>().ToList().LastOrDefault();

               // addOptionWayDetectionSparePartViewModel.EditInitWays(selectedWay);
                //[Sudhir.jangra][GEOS2-4935]
                addOptionWayDetectionSparePartViewModel.EditInitWays(SelectedWay);

                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                if (ownerInfo!=null)
                {
                    addOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                }
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsWaySave)
                {
                    var wayItem = WaysMenulist.FirstOrDefault(x => x.IdWays == SelectedWay.IdWays);
                    SetUpdatedDataOnUIForWay(addOptionWayDetectionSparePartViewModel, wayItem);

                    wayItem = Ways.FirstOrDefault(x => x.IdWays == SelectedWay.IdWays);
                    SetUpdatedDataOnUIForWay(addOptionWayDetectionSparePartViewModel, wayItem);
                }
                GeosApplication.Instance.Logger.Log("Method EditWay()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditWayForCurrentModule(object obj)
        {
            try
            {


                GeosApplication.Instance.Logger.Log("Method EditWayForCurrentModule()...", category: Category.Info, priority: Priority.Low);

                EditDetectionView addOptionWayDetectionSparePartView = new EditDetectionView();
                EditDetectionViewModel addOptionWayDetectionSparePartViewModel = new EditDetectionViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditWayHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = false;
                addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Collapsed;
                addOptionWayDetectionSparePartViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionWay").ToString());

                if (SelectedWay == null)
                    return;

                Ways selectedWay = SelectedWay;

                addOptionWayDetectionSparePartViewModel.EditInitWays(selectedWay);
                addOptionWayDetectionSparePartViewModel.IsEnabledCancelButton = false;
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsWaySave)
                {
                    var wayItem = WaysMenulist.FirstOrDefault(x => x.IdWays == SelectedWay.IdWays);
                    SetUpdatedDataOnUIForWay(addOptionWayDetectionSparePartViewModel, wayItem);

                    wayItem = Ways.FirstOrDefault(x => x.IdWays == SelectedWay.IdWays);
                    SetUpdatedDataOnUIForWay(addOptionWayDetectionSparePartViewModel, wayItem);
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditWayForCurrentModule()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditWayForCurrentModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private static void SetUpdatedDataOnUIForWay(EditDetectionViewModel addOptionWayDetectionSparePartViewModel, Ways wayItem)
        {
            if (wayItem != null)
            {
                wayItem.IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                wayItem.Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                wayItem.Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
            }
        }

        private void EditDetections(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDetections()...", category: Category.Info, priority: Priority.Low);
                if (SelectedDetection.Parent != null || (SelectedDetection.Parent == null && SelectedDetection.IdGroup == null))
                {
                    EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                    EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                    EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                    addEditOptionsDetectionsViewModel.RequestClose += handle;

                    addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDetectionHeader").ToString();
                    addEditOptionsDetectionsViewModel.IsNew = false;
                    addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                    addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());

                    addEditOptionsDetectionsViewModel.EditInitDetections(SelectedDetection);
                    addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                    addEditOptionsDetectionsView.ShowDialog();

                    if (addEditOptionsDetectionsViewModel.IsDetectionSave)
                    {
                        Detections itemFromDetectionsMenulist = DetectionsMenulist.FirstOrDefault(x => x.IdDetections == addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections);
                        SetUpdatedDataOnUIForDetections(addEditOptionsDetectionsViewModel, itemFromDetectionsMenulist);

                        Detections itemFromDetections = Detections.FirstOrDefault(x => x.IdDetections == addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections);
                        SetUpdatedDataOnUIForDetections(addEditOptionsDetectionsViewModel, itemFromDetections);
                        DetectionsMenulist = new ObservableCollection<Detections>(PCMService.GetAllDetectionsWithGroups_V2300());
                        DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        //  Detections.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                    }
                    GeosApplication.Instance.Logger.Log("Method EditDetections()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditDetectionsForCurrentModule(object obj)
        {
            try
            {

                Visible = Visibility.Visible.ToString();

                GeosApplication.Instance.Logger.Log("Method EditDetectionsForCurrentModule()...", category: Category.Info, priority: Priority.Low);
                if (SelectedDetectionForCurrentModule.Parent != null || (SelectedDetectionForCurrentModule.Parent == null && SelectedDetectionForCurrentModule.IdGroup == null))
                {
                    EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                    EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                    EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                    addEditOptionsDetectionsViewModel.RequestClose += handle;

                    addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDetectionHeader").ToString();
                    addEditOptionsDetectionsViewModel.IsNew = false;
                    addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                    addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());

                    addEditOptionsDetectionsViewModel.EditInitDetections(SelectedDetectionForCurrentModule);
                    addEditOptionsDetectionsViewModel.IsEnabledCancelButton = false;
                    addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                    addEditOptionsDetectionsView.ShowDialog();

                    if (addEditOptionsDetectionsViewModel.IsDetectionSave)
                    {
                        Detections itemFromDetectionsMenulist = DetectionsMenulist.FirstOrDefault(x => x.IdDetections == addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections);
                        SetUpdatedDataOnUIForDetections(addEditOptionsDetectionsViewModel, itemFromDetectionsMenulist);

                        Detections itemFromDetections = Detections.FirstOrDefault(x => x.IdDetections == addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections);
                        SetUpdatedDataOnUIForDetections(addEditOptionsDetectionsViewModel, itemFromDetections);

                        DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        //   Detections.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        IsEnabledCancelButton = true;
                    }

                    GeosApplication.Instance.Logger.Log("Method EditDetectionsForCurrentModule()....executed successfully", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditDetectionsForCurrentModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private static void SetUpdatedDataOnUIForDetections(EditDetectionViewModel addEditOptionsDetectionsViewModel, Detections itemFromDetectionsMenulist)
        {
            if (itemFromDetectionsMenulist != null)
            {
                itemFromDetectionsMenulist.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                itemFromDetectionsMenulist.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                itemFromDetectionsMenulist.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                itemFromDetectionsMenulist.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                itemFromDetectionsMenulist.Key = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections.ToString();
                itemFromDetectionsMenulist.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                itemFromDetectionsMenulist.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                itemFromDetectionsMenulist.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                itemFromDetectionsMenulist.Parent = "Group_" + itemFromDetectionsMenulist.IdGroup;
            }
        }

        private void EditSpareParts(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSpareParts()...", category: Category.Info, priority: Priority.Low);

                if (SelectedSparePart?.Parent != null || (SelectedSparePart?.Parent == null && SelectedSparePart?.IdGroup == null))
                {
                    EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                    EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                    EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                    addEditOptionsDetectionsViewModel.RequestClose += handle;

                    addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString();
                    addEditOptionsDetectionsViewModel.IsNew = false;
                    addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                    addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());

                    addEditOptionsDetectionsViewModel.EditInitSparePart(SelectedSparePart);
                    addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                    addEditOptionsDetectionsView.ShowDialog();

                    if (addEditOptionsDetectionsViewModel.IsSparepartSave)
                    {
                        var item = SparePartsMenulist.FirstOrDefault(x => x.IdSpareParts == SelectedSparePart.IdSpareParts);
                        SetDataOnUIForSpareParts(addEditOptionsDetectionsViewModel, item);


                        item = SpareParts.FirstOrDefault(x => x.IdSpareParts == SelectedSparePart.IdSpareParts);
                        SetDataOnUIForSpareParts(addEditOptionsDetectionsViewModel, item);
                        SparePartsMenulist = new ObservableCollection<SpareParts>(PCMService.GetAllSparepartsWithGroups_V2300());
                        SparePartsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        //  SpareParts.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditSpareParts()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditSpareParts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditSparePartsForCurrentModule(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditSparePartsForCurrentModule()...", category: Category.Info, priority: Priority.Low);

                if (SelectedSparePartForCurrentModule.Parent != null || (SelectedSparePartForCurrentModule.Parent == null && SelectedSparePartForCurrentModule.IdGroup == null))
                {
                    EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                    EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                    EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                    addEditOptionsDetectionsViewModel.RequestClose += handle;

                    addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString();
                    addEditOptionsDetectionsViewModel.IsNew = false;
                    addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;
                    addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());

                    addEditOptionsDetectionsViewModel.EditInitSparePart(SelectedSparePartForCurrentModule);
                    addEditOptionsDetectionsViewModel.IsEnabledCancelButton = false;
                    addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                    addEditOptionsDetectionsView.ShowDialog();

                    if (addEditOptionsDetectionsViewModel.IsSparepartSave)
                    {
                        var item = SparePartsMenulist.FirstOrDefault(x => x.IdSpareParts == SelectedSparePartForCurrentModule.IdSpareParts);
                        SetDataOnUIForSpareParts(addEditOptionsDetectionsViewModel, item);
                        SparePartsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);

                        item = SpareParts.FirstOrDefault(x => x.IdSpareParts == SelectedSparePartForCurrentModule.IdSpareParts);
                        SetDataOnUIForSpareParts(addEditOptionsDetectionsViewModel, item);
                        //  SpareParts.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                        IsEnabledCancelButton = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditSparePartsForCurrentModule()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditSparePartsForCurrentModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetDataOnUIForSpareParts(EditDetectionViewModel addEditOptionsDetectionsViewModel, SpareParts item)
        {
            if (item != null)
            {
                item.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                item.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                item.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                item.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                item.Key = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections.ToString();
                item.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                item.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                item.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                item.Parent = "Group_" + item.IdGroup;
            }
        }

        private void DuplicateModulesCommandAction(object obj)
        {
            if (isDuplicateClicked == true)
                return;

            DuplicateModulesAdditionalInformationView duplicateModulesAdditionalInformationView = new DuplicateModulesAdditionalInformationView();
            DuplicateModulesAdditionalInformationViewModel duplicateModulesAdditionalInformationViewModel = new DuplicateModulesAdditionalInformationViewModel(duplicateModulesAdditionalInformationView);
            #region[GEOS2-4262][rdixit][29.03.2023]
            duplicateModulesAdditionalInformationViewModel.IsDuplicateDetectionButtonVisible = Visibility.Collapsed;
            duplicateModulesAdditionalInformationViewModel.IsDuplicateModuleButtonVisible = Visibility.Visible;
            #endregion
            duplicateModulesAdditionalInformationView.ShowDialog();

            if (duplicateModulesAdditionalInformationViewModel.isDuplicateClicked)
            {
                isDuplicateClicked = true;
                IsDuplicateModulesButtonEnabled = false;
                DuplicateAcceptCommandAction(duplicateModulesAdditionalInformationViewModel);

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDuplicatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                IsNew = true;
            }
        }
        void DuplicateAcceptCommandAction(DuplicateModulesAdditionalInformationViewModel duplicateModulesViewModel)
        {
            try
            {
                DuplicateProductTypesDetails = new ProductTypes();
                ProductTypeOldCode = ClonedProductType.Reference;
                ProductTypeOldName = ClonedProductType.Name;
                FillCode();

                DuplicateProductTypesDetails = ClonedProductType;
                Reference = DuplicateCode;
                ModifyBy = DuplicateProductTypesDetails.ModifiedBy;

                if (ProductTypesDetails.LastUpdate != null)
                    LastUpdate = (DateTime)DuplicateProductTypesDetails.LastUpdate;
                else
                    LastUpdate = null;

                ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                if (IsCheckedCopyDescription == true)
                {
                    Name = "Duplicated_" + DuplicateProductTypesDetails.Name;
                    Name_en = "Duplicated_" + DuplicateProductTypesDetails.Name;
                    Name_es = "Duplicated_" + DuplicateProductTypesDetails.Name_es;
                    Name_fr = "Duplicated_" + DuplicateProductTypesDetails.Name_fr;
                    Name_pt = "Duplicated_" + DuplicateProductTypesDetails.Name_pt;
                    Name_ro = "Duplicated_" + DuplicateProductTypesDetails.Name_ro;
                    Name_ru = "Duplicated_" + DuplicateProductTypesDetails.Name_ru;
                    Name_zh = "Duplicated_" + DuplicateProductTypesDetails.Name_zh;

                    Description = DuplicateProductTypesDetails.Description;
                    Description_en = DuplicateProductTypesDetails.Description;
                    Description_es = DuplicateProductTypesDetails.Description_es;
                    Description_fr = DuplicateProductTypesDetails.Description_fr;
                    Description_pt = DuplicateProductTypesDetails.Description_pt;
                    Description_ro = DuplicateProductTypesDetails.Description_ro;
                    Description_ru = DuplicateProductTypesDetails.Description_ru;
                    Description_zh = DuplicateProductTypesDetails.Description_zh;
                }
                else
                {
                    Name = "Duplicated_" + DuplicateProductTypesDetails.Name;
                    Name_en = "Duplicated_" + DuplicateProductTypesDetails.Name;
                    Name_es = "Duplicated_" + DuplicateProductTypesDetails.Name_es;
                    Name_fr = "Duplicated_" + DuplicateProductTypesDetails.Name_fr;
                    Name_pt = "Duplicated_" + DuplicateProductTypesDetails.Name_pt;
                    Name_ro = "Duplicated_" + DuplicateProductTypesDetails.Name_ro;
                    Name_ru = "Duplicated_" + DuplicateProductTypesDetails.Name_ru;
                    Name_zh = "Duplicated_" + DuplicateProductTypesDetails.Name_zh;

                    Description = DuplicateProductTypesDetails.Description;
                    Description_en = DuplicateProductTypesDetails.Description;
                    Description_es = DuplicateProductTypesDetails.Description_es;
                    Description_fr = DuplicateProductTypesDetails.Description_fr;
                    Description_pt = DuplicateProductTypesDetails.Description_pt;
                    Description_ro = DuplicateProductTypesDetails.Description_ro;
                    Description_ru = DuplicateProductTypesDetails.Description_ru;
                    Description_zh = DuplicateProductTypesDetails.Description_zh;
                }

                Abbrivation = DuplicateProductTypesDetails.Code;
                SelectedDefaultWayType = new Data.Common.PCM.Ways();
                SelectedDefaultWayType = DuplicateProductTypesDetails.WayList.FirstOrDefault(x => x.IdWays == DuplicateProductTypesDetails.IdDefaultWayType);

                if (SelectedDefaultWayType == null && DuplicateProductTypesDetails.WayList.Count() == 0)
                    IsEnabled = false;
                else
                    IsEnabled = true;

                SelectedStatus = StatusList.LastOrDefault();
                SelectedTemplate = TemplatesMenuList.FirstOrDefault(x => x.IdTemplate == DuplicateProductTypesDetails.IdTemplate);

                Families = new ObservableCollection<ConnectorFamilies>(DuplicateProductTypesDetails.FamilyList);
                Ways = new ObservableCollection<Ways>(DuplicateProductTypesDetails.WayList);
                Detections = new ObservableCollection<Detections>(DuplicateProductTypesDetails.DetectionList_Group);
                Options = new ObservableCollection<Options>(DuplicateProductTypesDetails.OptionList_Group);
                SpareParts = new ObservableCollection<SpareParts>(DuplicateProductTypesDetails.SparePartsList_Group);

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                    Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                    IsCheckedCopyDescription = true;
                else
                    IsCheckedCopyDescription = false;

                if (duplicateModulesViewModel.IsCheckedImages)
                {
                    ImagesList = new ObservableCollection<ProductTypeImage>(DuplicateProductTypesDetails.ProductTypeImageList);
                    if (ImagesList.Count > 0)
                    {
                        List<ProductTypeImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                        List<ProductTypeImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                        if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                        {
                            ulong PositionCount = 1;
                            ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                        }

                        ProductTypeImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempProductTypeImage != null)
                        {
                            SelectedImage = tempProductTypeImage;
                            SelectedDefaultImage = tempProductTypeImage;
                        }
                        else
                        {
                            SelectedImage = ImagesList.FirstOrDefault();
                        }

                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);
                        SelectedDefaultImage = new ProductTypeImage();
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);

                        SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                    }
                }

                if (Options.Count > 0)
                {
                    Options_count = Options.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                    Options_Group_count = Options.Where(a => a.Parent == null && a.IdGroup != null).Count();
                }

                if (Detections.Count > 0)
                {
                    Detections_count = Detections.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                    Detections_group_count = Detections.Where(a => a.Parent == null && a.IdGroup != null).Count();
                }

                if (SpareParts.Count > 0)
                {
                    SpareParts_count = SpareParts.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                    SpareParts_group_count = SpareParts.Where(a => a.Parent == null && a.IdGroup != null).Count();
                }

                AddFiles(DuplicateProductTypesDetails.IdCPType);
                if (duplicateModulesViewModel.IsCheckedLinks == true)
                    AddLinks(DuplicateProductTypesDetails.IdCPType);

                if (duplicateModulesViewModel.IsCheckedAttachment == true)
                {
                    ProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(DuplicateProductTypesDetails.ProductTypeAttachedDocList);

                    ProductTypeFilesList.ToList().ForEach(im => { im.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser); });
                    if (ProductTypeFilesList.Count > 0)
                        SelectedProductTypeFile = ProductTypeFilesList.FirstOrDefault();
                }
                else
                {
                    ProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>();
                    SelectedProductTypeFile = new ProductTypeAttachedDoc();
                }

                if (duplicateModulesViewModel.IsCheckedImages == true)
                {
                    ImagesList = new ObservableCollection<ProductTypeImage>(DuplicateProductTypesDetails.ProductTypeImageList);

                    ImagesList.ToList().ForEach(im => { im.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser); });

                    if (ImagesList.Count > 0)
                        SelectedImage = ImagesList.FirstOrDefault();

                    foreach (ProductTypeImage item in ImagesList)
                    {
                        item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(item.ProductTypeImageInBytes);
                    }
                }
                else
                {
                    ImagesList = new ObservableCollection<ProductTypeImage>();
                    SelectedImage = new ProductTypeImage();
                }

                if (duplicateModulesViewModel.IsCheckedLinks == true)
                {
                    ProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(DuplicateProductTypesDetails.ProductTypeAttachedLinkList);

                    ProductTypeLinksList.ToList().ForEach(im => { im.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser); });

                    if (ProductTypeLinksList.Count > 0)
                        SelectedProductTypeLink = ProductTypeLinksList.FirstOrDefault();
                }
                else
                {
                    ProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>();
                    if (ProductTypeLinksList.Count > 0)
                        SelectedProductTypeLink = new ProductTypeAttachedLink();
                }

                FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(4).ToList());
                FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(4).ToList());
                FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                if (duplicateModulesViewModel.IsCheckedCustomers == true)
                {
                    CustomersMenuList = new ObservableCollection<RegionsByCustomer>(DuplicateProductTypesDetails.CustomerList);
                    SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(DuplicateProductTypesDetails.CustomerList.Where(a => a.IsChecked == true));

                    var List = CustomersMenuList.GroupBy(info => info.IdRegion)
                                .Select(group => new
                                {
                                    RegionName = CustomersMenuList.FirstOrDefault(a => a.IdRegion == group.Key).RegionName,
                                    Count = CustomersMenuList.Where(b => b.IsChecked == true && b.IdRegion == group.Key).Count(),
                                }).ToList();

                    List<FourRegionsWithCustomerCount> FourRecordsList = new List<FourRegionsWithCustomerCount>();

                    foreach (var record in List)
                    {
                        FourRegionsWithCustomerCount fourRegionsWithCustomerCount = new FourRegionsWithCustomerCount();
                        fourRegionsWithCustomerCount.RegionName = record.RegionName;
                        fourRegionsWithCustomerCount.Count = record.Count;
                        FourRecordsList.Add(fourRegionsWithCustomerCount);
                    }

                    FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList.ToList());
                }
                else
                {
                    CustomersMenuList = new ObservableCollection<RegionsByCustomer>();
                    SelectedCustomersList = new ObservableCollection<RegionsByCustomer>();
                    FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>();
                }

                //Compatiblity
                if (duplicateModulesViewModel.IsCheckedCompatibility == true)
                {
                    if (DuplicateProductTypesDetails.ProductTypeCompatibilityList != null)
                    {
                        MandatoryList = new ObservableCollection<ProductTypeCompatibility>(DuplicateProductTypesDetails.ProductTypeCompatibilityList.Where(a => a.IdTypeCompatibility == 249));
                        SuggestedList = new ObservableCollection<ProductTypeCompatibility>(DuplicateProductTypesDetails.ProductTypeCompatibilityList.Where(a => a.IdTypeCompatibility == 250));
                        IncompatibleList = new ObservableCollection<ProductTypeCompatibility>(DuplicateProductTypesDetails.ProductTypeCompatibilityList.Where(a => a.IdTypeCompatibility == 251));
                    }
                    else
                    {
                        MandatoryList = new ObservableCollection<ProductTypeCompatibility>();
                        SuggestedList = new ObservableCollection<ProductTypeCompatibility>();
                        IncompatibleList = new ObservableCollection<ProductTypeCompatibility>();
                        GetCompatibilityCount();
                    }
                }
                else
                {
                    MandatoryList = new ObservableCollection<ProductTypeCompatibility>();
                    SuggestedList = new ObservableCollection<ProductTypeCompatibility>();
                    IncompatibleList = new ObservableCollection<ProductTypeCompatibility>();
                    GetCompatibilityCount();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method DuplicateAcceptCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

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

        private void AddFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                AddFileInProductTypeView addFileInProductTypeView = new AddFileInProductTypeView();
                AddFileInProductTypeViewModel addFileInProductTypeViewModel = new AddFileInProductTypeViewModel();
                EventHandler handle = delegate { addFileInProductTypeView.Close(); };
                addFileInProductTypeViewModel.RequestClose += handle;
                addFileInProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addFileInProductTypeViewModel.IsNew = true;
                addFileInProductTypeViewModel.Init();//[Sudhir.Jangra][GEOS2-4072][Add Type In Module][08/12/2022]
                addFileInProductTypeView.DataContext = addFileInProductTypeViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addFileInProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addFileInProductTypeView.ShowDialog();

                if (addFileInProductTypeViewModel.IsSave)
                {
                    IsEnabledCancelButton = true;
                    if (ProductTypeFilesList == null)//[Sudhir.Jangra][GEOS2-4072][Add Type In Module][12/12/2022]
                        ProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>();//[Sudhir.Jangra][GEOS2-4072][Add Type In Module][12/12/2022]
                    addFileInProductTypeViewModel.SelectedProductTypeFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;//[Sudhir.Jangra][GEOS2-4072][Add Type In Module][12/12/2022]
                    ProductTypeFilesList.Add(addFileInProductTypeViewModel.SelectedProductTypeFile);
                    SelectedProductTypeFile = addFileInProductTypeViewModel.SelectedProductTypeFile;
                    FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(4).ToList());
                }
                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                documentViewModel.OpenPdf(SelectedProductTypeFile, obj);
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

        private void DeleteFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                ProductTypeAttachedDoc productTypeAttachedDoc = (ProductTypeAttachedDoc)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    ProductTypeFilesList.Remove(SelectedProductTypeFile);
                    ProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList);
                    SelectedProductTypeFile = ProductTypeFilesList.FirstOrDefault();
                    FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(4).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method DeleteFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditFile(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ProductTypeAttachedDoc productTypeAttachedDoc = (ProductTypeAttachedDoc)detailView.DataControl.CurrentItem;
                AddFileInProductTypeView addFileInProductTypeView = new AddFileInProductTypeView();
                AddFileInProductTypeViewModel addFileInProductTypeViewModel = new AddFileInProductTypeViewModel();
                EventHandler handle = delegate { addFileInProductTypeView.Close(); };
                addFileInProductTypeViewModel.RequestClose += handle;
                addFileInProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                addFileInProductTypeViewModel.IsNew = false;
                addFileInProductTypeViewModel.EditInit(productTypeAttachedDoc);
                addFileInProductTypeView.DataContext = addFileInProductTypeViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addFileInProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addFileInProductTypeView.ShowDialog();

                if (addFileInProductTypeViewModel.IsSave == true)
                {
                    SelectedProductTypeFile.IdCPTypeAttachedDoc = addFileInProductTypeViewModel.IdCPTypeAttachedDoc;
                    SelectedProductTypeFile.OriginalFileName = addFileInProductTypeViewModel.FileName;
                    SelectedProductTypeFile.Description = addFileInProductTypeViewModel.Description;
                    SelectedProductTypeFile.ProductTypeAttachedDocInBytes = addFileInProductTypeViewModel.FileInBytes;
                    SelectedProductTypeFile.SavedFileName = addFileInProductTypeViewModel.ProductTypeSavedFileName;
                    SelectedProductTypeFile.AttachmentType = addFileInProductTypeViewModel.ModuleSelectedType;//[Sudhir.Jangra][Geos2-4072][13/12/2022]
                    SelectedProductTypeFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddLink(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddLink()...", category: Category.Info, priority: Priority.Low);

                AddLinkInProductTypeView addLinkInProductTypeView = new AddLinkInProductTypeView();
                AddLinkInProductTypeViewModel addLinkInProductTypeViewModel = new AddLinkInProductTypeViewModel();
                EventHandler handle = delegate { addLinkInProductTypeView.Close(); };
                addLinkInProductTypeViewModel.RequestClose += handle;
                addLinkInProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddLinkHeader").ToString();
                addLinkInProductTypeViewModel.IsNew = true;
                addLinkInProductTypeView.DataContext = addLinkInProductTypeViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addLinkInProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addLinkInProductTypeView.ShowDialog();

                if (addLinkInProductTypeViewModel.IsSave == true)
                {
                    ProductTypeLinksList.Add(addLinkInProductTypeViewModel.SelectedProductTypeLink);
                    SelectedProductTypeLink = addLinkInProductTypeViewModel.SelectedProductTypeLink;
                    FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(4).ToList());
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method AddLink()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLink() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditLink(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditLink()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ProductTypeAttachedLink productTypeAttachedLink = (ProductTypeAttachedLink)detailView.DataControl.CurrentItem;
                AddLinkInProductTypeView addLinkInProductTypeView = new AddLinkInProductTypeView();
                AddLinkInProductTypeViewModel addLinkInProductTypeViewModel = new AddLinkInProductTypeViewModel();
                EventHandler handle = delegate { addLinkInProductTypeView.Close(); };
                addLinkInProductTypeViewModel.RequestClose += handle;
                addLinkInProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditLinkHeader").ToString();
                addLinkInProductTypeViewModel.IsNew = false;
                addLinkInProductTypeViewModel.EditInit(productTypeAttachedLink);
                addLinkInProductTypeView.DataContext = addLinkInProductTypeViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addLinkInProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addLinkInProductTypeView.ShowDialog();

                if (addLinkInProductTypeViewModel.IsSave == true)
                {

                    SelectedProductTypeLink.IdCPTypeAttachedLink = addLinkInProductTypeViewModel.IdCatalogueItemAttachedLink;
                    SelectedProductTypeLink.Name = addLinkInProductTypeViewModel.LinkName;
                    SelectedProductTypeLink.Address = addLinkInProductTypeViewModel.LinkAddress;
                    SelectedProductTypeLink.Description = addLinkInProductTypeViewModel.Description;

                    SelectedProductTypeLink.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditLink()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditLink() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteLink(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteLink()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentLink"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                CatalogueItemAttachedLink catalogueItemAttachedLink = (CatalogueItemAttachedLink)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    ProductTypeLinksList.Remove(SelectedProductTypeLink);
                    ProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList);
                    SelectedProductTypeLink = ProductTypeLinksList.FirstOrDefault();
                    FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(4).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method DeleteLink()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteLink()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void EditImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditImageAction()...", category: Category.Info, priority: Priority.Low);

                AddImageInProductTypeView addImageInProductTypeView = new AddImageInProductTypeView();
                AddImageInProductTypeViewModel addImageInProductTypeViewModel = new AddImageInProductTypeViewModel();
                EventHandler handle = delegate { addImageInProductTypeView.Close(); };
                addImageInProductTypeViewModel.RequestClose += handle;
                addImageInProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditImageHeader").ToString();
                addImageInProductTypeViewModel.IsNew = false;

                ProductTypeImage tempObject = new ProductTypeImage();
                tempObject = (ProductTypeImage)obj;

                addImageInProductTypeViewModel.EditInit(tempObject);

                addImageInProductTypeViewModel.ImageList = ImagesList;
                int index_new = ImagesList.IndexOf(tempObject);

                addImageInProductTypeView.DataContext = addImageInProductTypeViewModel;
                addImageInProductTypeView.ShowDialog();

                if (addImageInProductTypeViewModel.IsSave == true)
                {

                    if (addImageInProductTypeViewModel.OldDefaultImage != null)
                    {
                        int index_old = ImagesList.IndexOf(addImageInProductTypeViewModel.OldDefaultImage);
                        ImagesList.Remove(tempObject);
                        ProductTypeImage tempProductTypeImage_old = new ProductTypeImage();
                        tempProductTypeImage_old.IdCPTypeImage = addImageInProductTypeViewModel.IdImage;
                        tempProductTypeImage_old.OriginalFileName = addImageInProductTypeViewModel.ImageName;
                        tempProductTypeImage_old.Description = addImageInProductTypeViewModel.Description;
                        tempProductTypeImage_old.ProductTypeImageInBytes = addImageInProductTypeViewModel.FileInBytes;
                        tempProductTypeImage_old.SavedFileName = addImageInProductTypeViewModel.ProductTypeSavedImageName;
                        tempProductTypeImage_old.Position = addImageInProductTypeViewModel.SelectedImage.Position;
                        tempProductTypeImage_old.CreatedBy = addImageInProductTypeViewModel.SelectedImage.CreatedBy;
                        tempProductTypeImage_old.ModifiedBy = addImageInProductTypeViewModel.SelectedImage.ModifiedBy;
                        tempProductTypeImage_old.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                        ImagesList.Insert(index_old, tempProductTypeImage_old);

                        ImagesList.Remove(addImageInProductTypeViewModel.OldDefaultImage);
                        ProductTypeImage tempProductTypeImage_new = new ProductTypeImage();
                        tempProductTypeImage_new = addImageInProductTypeViewModel.OldDefaultImage;

                        ImagesList.Insert(index_new, tempProductTypeImage_new);
                        SelectedImage = ImagesList[index_old];
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage_old.ProductTypeImageInBytes);
                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage_old.ProductTypeImageInBytes);
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    }
                    else
                    {
                        int index = ImagesList.IndexOf(tempObject);
                        ImagesList.Remove(tempObject);
                        ProductTypeImage tempProductTypeImage = new ProductTypeImage();
                        tempProductTypeImage.IdCPTypeImage = addImageInProductTypeViewModel.IdImage;
                        tempProductTypeImage.OriginalFileName = addImageInProductTypeViewModel.ImageName;
                        tempProductTypeImage.Description = addImageInProductTypeViewModel.Description;
                        tempProductTypeImage.ProductTypeImageInBytes = addImageInProductTypeViewModel.FileInBytes;
                        tempProductTypeImage.SavedFileName = addImageInProductTypeViewModel.ProductTypeSavedImageName;
                        tempProductTypeImage.Position = addImageInProductTypeViewModel.SelectedImage.Position;
                        tempProductTypeImage.CreatedBy = addImageInProductTypeViewModel.SelectedImage.CreatedBy;
                        tempProductTypeImage.ModifiedBy = addImageInProductTypeViewModel.SelectedImage.ModifiedBy;
                        tempProductTypeImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                        ImagesList.Insert(index, tempProductTypeImage);
                        SelectedImage = ImagesList[index];
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.ProductTypeImageInBytes);

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.ProductTypeImageInBytes);

                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    }
                    ImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(a => a.Position));

                    if (addImageInProductTypeViewModel.OldDefaultImage != null)
                    {
                        SelectedImageIndex = 1;
                    }
                    IsEnabledCancelButton = true;
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

                AddImageInProductTypeView addImageInProductTypeView = new AddImageInProductTypeView();
                AddImageInProductTypeViewModel addImageInProductTypeViewModel = new AddImageInProductTypeViewModel();
                EventHandler handle = delegate { addImageInProductTypeView.Close(); };
                addImageInProductTypeViewModel.RequestClose += handle;
                addImageInProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddImageHeader").ToString();
                addImageInProductTypeViewModel.IsNew = true;
                addImageInProductTypeViewModel.ImageList = ImagesList;
                addImageInProductTypeView.DataContext = addImageInProductTypeViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addImageInProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addImageInProductTypeView.ShowDialog();

                if (addImageInProductTypeViewModel.IsSave == true)
                {
                    SelectedImage = new ProductTypeImage();
                    SelectedImage = addImageInProductTypeViewModel.SelectedImage;
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);
                    FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    if (addImageInProductTypeViewModel.OldDefaultImage != null)
                    {
                        int index_old = ImagesList.IndexOf(addImageInProductTypeViewModel.OldDefaultImage);
                        ImagesList.Insert(index_old, addImageInProductTypeViewModel.SelectedImage);

                        int index_new = ImagesList.IndexOf(SelectedImage) + 1;

                        ImagesList.Remove(addImageInProductTypeViewModel.OldDefaultImage);
                        ProductTypeImage tempProductTypeImage = new ProductTypeImage();
                        tempProductTypeImage.IdCPTypeImage = addImageInProductTypeViewModel.OldDefaultImage.IdCPTypeImage;
                        tempProductTypeImage.SavedFileName = addImageInProductTypeViewModel.OldDefaultImage.SavedFileName;
                        tempProductTypeImage.Description = addImageInProductTypeViewModel.OldDefaultImage.Description;
                        tempProductTypeImage.ProductTypeImageInBytes = addImageInProductTypeViewModel.OldDefaultImage.ProductTypeImageInBytes;
                        tempProductTypeImage.OriginalFileName = addImageInProductTypeViewModel.OldDefaultImage.OriginalFileName;
                        tempProductTypeImage.Position = addImageInProductTypeViewModel.OldDefaultImage.Position;
                        tempProductTypeImage.CreatedBy = addImageInProductTypeViewModel.OldDefaultImage.CreatedBy;
                        tempProductTypeImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        tempProductTypeImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.ProductTypeImageInBytes);

                        ImagesList.Insert(index_new, tempProductTypeImage);
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else
                    {
                        ImagesList.Add(addImageInProductTypeViewModel.SelectedImage);
                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }

                    ImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(a => a.Position));
                    if (addImageInProductTypeViewModel.OldDefaultImage != null)
                    {
                        SelectedImageIndex = 1;
                    }
                    else
                    {
                        SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                    }

                    SelectedDefaultImage = ImagesList.FirstOrDefault();
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedDefaultImage.ProductTypeImageInBytes);
                    IsEnabledCancelButton = true;
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
                    IsEnabledCancelButton = true;
                    ProductTypeImage tempObj = new ProductTypeImage();
                    tempObj = (ProductTypeImage)obj;

                    if (ImagesList.Count > 0)
                    {
                        List<ProductTypeImage> imageList_ForSetPositions = ImagesList.Where(a => a.Position > tempObj.Position).ToList();
                        ImagesList.Remove(tempObj);

                        foreach (ProductTypeImage productTypeImage in imageList_ForSetPositions)
                        {
                            ImagesList.Where(a => a.Position == productTypeImage.Position).ToList().ForEach(a => { a.Position--; });
                        }
                    }

                    if (!(ImagesList.Count == 0))
                    {
                        SelectedImage = ImagesList.FirstOrDefault();
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);

                        SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);

                        FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else if (ImagesList.Count == 0)
                    {
                        FourRecordsProductTypeImagesList.Remove((ProductTypeImage)(obj));
                        SelectedDefaultImage.AttachmentImage = null;
                    }
                }

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
                saveFile.FileName = "PCMHistory_" + Reference + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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

        private void ExportToExcelCustomers(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcelCustomers()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Customers_" + Reference + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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
                    //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
                    var SelectedList = PCMCustomerList.ToList();
                    if (SelectedList == null)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        return;
                    }
                    else
                    {
                        ChangeLogTableView.DataControl.ItemsSource = SelectedList;
                        ChangeLogTableView.ShowFixedTotalSummary = false;
                        ChangeLogTableView.ExportToXlsx(ResultFileName);
                        ChangeLogTableView.DataControl.ItemsSource = PCMCustomerList;

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        System.Diagnostics.Process.Start(ResultFileName);
                        ChangeLogTableView.ShowTotalSummary = false;
                        ChangeLogTableView.ShowFixedTotalSummary = true;
                    }
                    //var List2 = ClonedProductType.CustomerList.Where(x => x.IsChecked == true).ToList();

                    //ChangeLogTableView.DataControl.ItemsSource = List2;
                    //ChangeLogTableView.ShowFixedTotalSummary = false;
                    //ChangeLogTableView.ExportToXlsx(ResultFileName);
                    //ChangeLogTableView.DataControl.ItemsSource = CustomersMenuList;

                    //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    //System.Diagnostics.Process.Start(ResultFileName);
                    //ChangeLogTableView.ShowTotalSummary = false;
                    //ChangeLogTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcelCustomers()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcelCustomers()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteWay(object obj)
        {
            try
            {
                //[GEOS2-4098][rdixit][28.12.2022] 
                GeosApplication.Instance.Logger.Log("Method DeleteWay()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteWay"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                Ways ways = (Ways)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    WaysMenulist.Where(a => a.IdWays == SelectedWay.IdWays).FirstOrDefault().IsCurrentWays = false;
                    Ways.Remove(SelectedWay);
                    Ways = new ObservableCollection<Ways>(Ways);
                    SelectedWay = Ways.FirstOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method DeleteWay()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteWay()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetStarWay(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetStarWay()...", category: Category.Info, priority: Priority.Low);
                var objWay = ((DevExpress.Xpf.Grid.CellEditorBase)((System.Windows.FrameworkElement)((System.Windows.RoutedEventArgs)obj).Source).TemplatedParent).RowData.Row;
                Ways ways = (Ways)objWay;
                SelectedDefaultWayType = SelectedWay;

                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method SetStarWay()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetStarWay()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteDetection(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDetection()...", category: Category.Info, priority: Priority.Low);
                if (SelectedDetectionForCurrentModule.IdDetections > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDetection"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsEnabledCancelButton = true;
                        //DetectionsMenulist.Where(a => a.IdDetections == SelectedDetectionForCurrentModule.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //Remove Gray colour [sdeshpande][GEOS2-4098][26-12-2022]
                        List<Detections> GetDetections_child = new List<Detections>(Detections.Where(d => d.Parent == SelectedDetectionForCurrentModule.Parent));
                        if (GetDetections_child.Count == 1 && SelectedDetectionForCurrentModule.Parent != null)
                        {
                            Detections Detection_Group = Detections.Where(a => a.Key == SelectedDetectionForCurrentModule.Parent).FirstOrDefault();
                            DetectionsMenulist.Where(a => a.IdDetections == Detection_Group.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //[001]
                            Detections.Remove(Detection_Group);
                            Detections.Remove(SelectedDetectionForCurrentModule);
                        }
                        else
                        {
                            if (SelectedDetectionForCurrentModule.IdDetections == 0)
                            {
                                List<Detections> DetectionList = Detections.Where(a => a.Parent == SelectedDetectionForCurrentModule.Key).ToList();
                                foreach (Detections detection in DetectionList)
                                {
                                    DetectionsMenulist.Where(a => a.IdDetections == detection.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //[001]
                                    Detections.Remove(detection);
                                }
                            }
                            DetectionsMenulist.Where(a => a.IdDetections == SelectedDetectionForCurrentModule.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //[001]
                            Detections.Remove(SelectedDetectionForCurrentModule);
                        }

                        SelectedDetectionForCurrentModule = Detections.FirstOrDefault();

                        if (Detections.Count > 0)
                        {
                            Detections_count = Detections.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                            Detections_group_count = Detections.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                        else
                        {
                            Detections_count = 0;
                            Detections_group_count = 0;
                        }
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteGroup"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        //DetectionsMenulist.Where(a => a.IdDetections == SelectedDetectionForCurrentModule.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //Remove Gray colour [sdeshpande][GEOS2-4098][26-12-2022]                        List<Detections> GetDetections_child = new List<Detections>(Detections.Where(d => d.Parent == SelectedDetectionForCurrentModule.Parent));
                        List<Detections> GetDetections_child = new List<Detections>(Detections.Where(d => d.Parent == SelectedDetectionForCurrentModule.Parent));
                        if (GetDetections_child.Count == 1 && SelectedDetectionForCurrentModule.Parent != null)
                        {
                            Detections Detection_Group = Detections.Where(a => a.Key == SelectedDetectionForCurrentModule.Parent).FirstOrDefault();
                            DetectionsMenulist.Where(a => a.IdDetections == Detection_Group.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //[001]
                            Detections.Remove(Detection_Group);
                            Detections.Remove(SelectedDetectionForCurrentModule);
                        }
                        else
                        {
                            if (SelectedDetectionForCurrentModule.IdDetections == 0)
                            {
                                List<Detections> DetectionList = Detections.Where(a => a.Parent == SelectedDetectionForCurrentModule.Key).ToList();
                                foreach (Detections detection in DetectionList)
                                {
                                    DetectionsMenulist.Where(a => a.IdDetections == detection.IdDetections).ToList().ForEach(b => b.IsCurrentDetection = false); //[001]
                                    Detections.Remove(detection);
                                }
                            }
                            Detections.Remove(SelectedDetectionForCurrentModule);
                        }
                        SelectedDetectionForCurrentModule = Detections.FirstOrDefault();


                        if (Detections.Count > 0)
                        {
                            Detections_count = Detections.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                            Detections_group_count = Detections.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                        else
                        {
                            Detections_count = 0;
                            Detections_group_count = 0;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteDetection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDetection()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteOption(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteOption()...", category: Category.Info, priority: Priority.Low);
                if (selectedOptionForCurrentModule.IdOptions > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteOption"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsEnabledCancelButton = true;
                        //OptionsMenulist.Where(a => a.IdOptions == selectedOptionForCurrentModule.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //Remove Gray colour [sdeshpande][GEOS2-4098][26-12-2022]
                        List<Options> GetOptions_child = new List<Options>(Options.Where(d => d.Parent == selectedOptionForCurrentModule.Parent));
                        if (GetOptions_child.Count == 1 && selectedOptionForCurrentModule.Parent != null)
                        {
                            Options Option_Group = Options.Where(a => a.Key == selectedOptionForCurrentModule.Parent).FirstOrDefault();
                            OptionsMenulist.Where(a => a.IdOptions == Option_Group.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //[001]
                            Options.Remove(Option_Group);
                            Options.Remove(selectedOptionForCurrentModule);
                        }
                        else
                        {
                            if (selectedOptionForCurrentModule.IdOptions == 0)
                            {
                                List<Options> OptionList = Options.Where(a => a.Parent == selectedOptionForCurrentModule.Key).ToList();
                                foreach (Options Option in OptionList)
                                {
                                    OptionsMenulist.Where(a => a.IdOptions == Option.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //[001]
                                    Options.Remove(Option);
                                }
                            }
                            OptionsMenulist.Where(a => a.IdOptions == selectedOptionForCurrentModule.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //[001]
                            Options.Remove(selectedOptionForCurrentModule);
                        }

                        selectedOptionForCurrentModule = Options.FirstOrDefault();
                        if (Options.Count > 0)
                        {
                            Options_count = Options.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                            Options_Group_count = Options.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                        else
                        {
                            Options_count = 0;
                            Options_Group_count = 0;
                        }

                    }

                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteGroup"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        //OptionsMenulist.Where(a => a.IdOptions == selectedOptionForCurrentModule.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //Remove Gray colour [sdeshpande][GEOS2-4098][26-12-2022]
                        List<Options> GetOptions_child = new List<Options>(Options.Where(d => d.Parent == selectedOptionForCurrentModule.Parent));
                        if (GetOptions_child.Count == 1 && selectedOptionForCurrentModule.Parent != null)
                        {
                            Options Option_Group = Options.Where(a => a.Key == selectedOptionForCurrentModule.Parent).FirstOrDefault();
                            OptionsMenulist.Where(a => a.IdOptions == Option_Group.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //[001]
                            Options.Remove(Option_Group);
                            Options.Remove(selectedOptionForCurrentModule);
                        }
                        else
                        {
                            if (selectedOptionForCurrentModule.IdOptions == 0)
                            {
                                List<Options> OptionList = Options.Where(a => a.Parent == selectedOptionForCurrentModule.Key).ToList();
                                foreach (Options Option in OptionList)
                                {
                                    OptionsMenulist.Where(a => a.IdOptions == Option.IdOptions).ToList().ForEach(b => b.IsCurrentOptions = false); //[001]
                                    Options.Remove(Option);
                                }
                            }
                            Options.Remove(selectedOptionForCurrentModule);
                        }

                        selectedOptionForCurrentModule = Options.FirstOrDefault();
                        if (Options.Count > 0)
                        {
                            Options_count = Options.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                            Options_Group_count = Options.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                        else
                        {
                            Options_count = 0;
                            Options_Group_count = 0;
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteOption()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteOption()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteSparePart(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteSparePart()...", category: Category.Info, priority: Priority.Low);
                if (SelectedSparePartForCurrentModule.IdSpareParts > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteSparePart"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsEnabledCancelButton = true;
                        //SparePartsMenulist.Where(a => a.IdSpareParts == SelectedSparePartForCurrentModule.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //Remove Gray colour [sdeshpande][GEOS2-4098][28-12-2022]                        
                        List<SpareParts> GetSpareParts_child = new List<SpareParts>(SpareParts.Where(d => d.Parent == SelectedSparePartForCurrentModule.Parent));
                        if (GetSpareParts_child.Count == 1 && SelectedSparePartForCurrentModule.Parent != null)
                        {
                            SpareParts SparePart_Group = SpareParts.Where(a => a.Key == SelectedSparePartForCurrentModule.Parent).FirstOrDefault();
                            SparePartsMenulist.Where(a => a.IdSpareParts == SparePart_Group.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //[001]
                            SpareParts.Remove(SparePart_Group);
                            SpareParts.Remove(SelectedSparePartForCurrentModule);
                        }
                        else
                        {
                            if (SelectedSparePartForCurrentModule.IdSpareParts == 0)
                            {
                                List<SpareParts> SparePartList = SpareParts.Where(a => a.Parent == SelectedSparePartForCurrentModule.Key).ToList();
                                foreach (SpareParts spareParts in SparePartList)
                                {
                                    SparePartsMenulist.Where(a => a.IdSpareParts == spareParts.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //[001]
                                    SpareParts.Remove(spareParts);
                                }
                            }
                            SparePartsMenulist.Where(a => a.IdSpareParts == SelectedSparePartForCurrentModule.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //[001]
                            SpareParts.Remove(SelectedSparePartForCurrentModule);
                        }

                        SelectedSparePartForCurrentModule = SpareParts.FirstOrDefault();
                        if (SpareParts.Count > 0)
                        {
                            SpareParts_count = SpareParts.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                            SpareParts_group_count = SpareParts.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                        else
                        {
                            SpareParts_count = 0;
                            SpareParts_group_count = 0;
                        }
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteGroup"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        //SparePartsMenulist.Where(a => a.IdSpareParts == SelectedSparePartForCurrentModule.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //Remove Gray colour [sdeshpande][GEOS2-4098][28-12-2022]
                        List<SpareParts> GetSpareParts_child = new List<SpareParts>(SpareParts.Where(d => d.Parent == SelectedSparePartForCurrentModule.Parent));
                        if (GetSpareParts_child.Count == 1 && SelectedSparePartForCurrentModule.Parent != null)
                        {
                            SpareParts SpareParts_Group = SpareParts.Where(a => a.Key == SelectedSparePartForCurrentModule.Parent).FirstOrDefault();
                            SparePartsMenulist.Where(a => a.IdSpareParts == SpareParts_Group.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //[001]
                            SpareParts.Remove(SpareParts_Group);
                            SpareParts.Remove(SelectedSparePartForCurrentModule);
                        }
                        else
                        {
                            if (SelectedSparePartForCurrentModule.IdSpareParts == 0)
                            {
                                List<SpareParts> SparePartList = SpareParts.Where(a => a.Parent == SelectedSparePartForCurrentModule.Key).ToList();
                                foreach (SpareParts spareParts in SparePartList)
                                {
                                    SparePartsMenulist.Where(a => a.IdSpareParts == spareParts.IdSpareParts).ToList().ForEach(b => b.IsCurrentSpareParts = false); //[001]
                                    SpareParts.Remove(spareParts);
                                }
                            }
                            SpareParts.Remove(SelectedSparePartForCurrentModule);
                        }

                        SelectedSparePartForCurrentModule = SpareParts.FirstOrDefault();
                        if (SpareParts.Count > 0)
                        {
                            SpareParts_count = SpareParts.Where(a => a.Parent != null || (a.Parent == null && a.IdGroup == null)).Count();
                            SpareParts_group_count = SpareParts.Where(a => a.Parent == null && a.IdGroup != null).Count();
                        }
                        else
                        {
                            SpareParts_count = 0;
                            SpareParts_group_count = 0;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteSparePart()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteSparePart()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteFamily(object obj)
        {
            try
            {
                //[GEOS2-4098][rdixit][28.12.2022] 
                GeosApplication.Instance.Logger.Log("Method DeleteFamily()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteFamily"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                ConnectorFamilies connectorFamilies = (ConnectorFamilies)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    //[rdixit][GEOS2-6624][10.12.2024]
                    var remo = (ConnectorFamilies)SelectedFamily.Clone();
                    if (remo.IdSubFamilyConnector > 0)
                    {
                        var subFamily = FamilyMenulist.FirstOrDefault(a => a.IdSubFamilyConnector == remo.IdSubFamilyConnector);
                        if (subFamily != null) subFamily.IsCurrentFamily = false;

                        var familyMenu = FamilyMenulist.FirstOrDefault(a => a.IdFamily == remo.IdFamily && a.IdSubFamilyConnector == 0); //Parent
                        if (familyMenu != null) familyMenu.IsCurrentFamily = false;

                        Families.Remove(SelectedFamily);
                        if (Families != null)
                        {
                            if (Families.Count(x => x.IdFamily == remo.IdFamily) == 1)
                            {
                                var remainingFamily = Families.FirstOrDefault(i => i.IdFamily == remo.IdFamily);//Remaining Parent
                                if (remainingFamily != null) Families.Remove(remainingFamily);
                            }
                        }
                        Families = new ObservableCollection<ConnectorFamilies>(Families);
                    }
                    else
                    {
                        var familyItems = FamilyMenulist.Where(x => x.IdFamily == remo.IdFamily)?.ToList();
                        foreach (var item in familyItems)
                        {
                            item.IsCurrentFamily = false;
                        };
                        Families = new ObservableCollection<ConnectorFamilies>(Families.Where(x => x.IdFamily != remo.IdFamily).ToList());                     
                    }
                    
                    SelectedFamily = Families.FirstOrDefault();
                }


                GeosApplication.Instance.Logger.Log("Method DeleteFamily()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteFamily()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                        EditSaveModule(null);
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



        /// <summary>
        /// For Filter grid Sellect All button Command.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectAllAction(object obj)
        {
            SelectedLetter = null;
        }

        private void PrintCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction ...", category: Category.Info, priority: Priority.Low);
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
                // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 06 01 2023
                PCMPrintModuleView pCMPrintModuleView = new PCMPrintModuleView();
                PCMPrintModuleViewModel pCMPrintModuleViewModel = new PCMPrintModuleViewModel();
                EventHandler handle = delegate { pCMPrintModuleView.Close(); };
                pCMPrintModuleViewModel.RequestClose += handle;
                if (pCMPrintModuleViewModel.ProductTypesDetails == null)
                {
                    pCMPrintModuleViewModel.ProductTypesDetails = new ProductTypes();
                }
                pCMPrintModuleViewModel.ProductTypesDetails = ProductTypesDetails;
                pCMPrintModuleViewModel.IncludedCustomersList = includedCustomerList?.ToList(); //[GEOS2-6531][rdixit][07.01.2025]
                pCMPrintModuleView.DataContext = pCMPrintModuleViewModel;
                //var ownerInfo = (detailView as FrameworkElement);
                //pCMPrintModuleView.Owner = Window.GetWindow(ownerInfo);
                pCMPrintModuleView.ShowDialog();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void RestrictOpeningPopUpAction(object obj)
        {
            ImageEdit img1 = obj as ImageEdit;
            img1.ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never;
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
                IsEnabledCancelButton = true;
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
                IsEnabledCancelButton = true;
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
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method DeleteIncompatible()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteIncompatible()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][31-07-2020][GEOS2-2159][avpawar][Change the Validation message like other module HRM, CRM etc]
        /// </summary>
        /// <param name="obj"></param>
        private void EditSaveModule(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSaveModule()...", category: Category.Info, priority: Priority.Low);

                //[001] Start
                allowValidation = true;

                //InformationError = null;
                //allowValidation = true;
                //error = EnableValidationAndGetError();
                //OnPropertyChanged(new PropertyChangedEventArgs("Abbrivation"));   //[GEOS2-3759][gulab lakade][02 01 2023]
                if (Name == null)
                {
                    InformationError = null;
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedTemplate"));

                    if (error != null)
                    {
                        return;
                    }
                }
                else
                {
                    InformationError = " ";
                }
                //if (error != null)
                //{
                //    return;
                //}

                GroupBox groupBox = (GroupBox)obj;

                if (IncompatibleList != null && IncompatibleList.Any(x => x.IdRelationshipType != 251 && x.Quantity == null))
                {
                    CompatibilityError = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("CompatibilityError"));

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
                //[001] End

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

                if (IsNew == true)
                {
                    if (IsModuleView == true)//[Sudhir.Jangra][GEOS2-4797]
                    {
                        if (Ways.ToList().Count > 0 && SelectedDefaultWayType == null)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDefaultWayTypePresent").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method EditSaveModule()...default way type is mandatory.", category: Category.Info, priority: Priority.Low);
                            return;
                        }
                    }


                    NewProductTypesDetails = new ProductTypes();
                    NewProductTypesDetails.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    NewProductTypesDetails.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    NewProductTypesDetails.Reference = Reference;
                    NewProductTypesDetails.Code = Abbrivation;


                    NewProductTypesDetails.IdTemplate = SelectedTemplate.IdTemplate;

                    NewProductTypesDetails.LastUpdate = LastUpdate;

                    if (IsCheckedCopyDescription == true)
                    {
                        NewProductTypesDetails.Name = Name;
                        NewProductTypesDetails.Name_es = Name;
                        NewProductTypesDetails.Name_fr = Name;
                        NewProductTypesDetails.Name_pt = Name;
                        NewProductTypesDetails.Name_ro = Name;
                        NewProductTypesDetails.Name_ru = Name;
                        NewProductTypesDetails.Name_zh = Name;

                        NewProductTypesDetails.Description = Description;
                        NewProductTypesDetails.Description_es = Description;
                        NewProductTypesDetails.Description_fr = Description;
                        NewProductTypesDetails.Description_pt = Description;
                        NewProductTypesDetails.Description_ro = Description;
                        NewProductTypesDetails.Description_ru = Description;
                        NewProductTypesDetails.Description_zh = Description;
                    }
                    else
                    {
                        NewProductTypesDetails.Name = Name;
                        NewProductTypesDetails.Name_es = Name;
                        NewProductTypesDetails.Name_fr = Name;
                        NewProductTypesDetails.Name_pt = Name;
                        NewProductTypesDetails.Name_ro = Name;
                        NewProductTypesDetails.Name_ru = Name;
                        NewProductTypesDetails.Name_zh = Name;

                        NewProductTypesDetails.Description = Description;
                        NewProductTypesDetails.Description_es = Description;
                        NewProductTypesDetails.Description_fr = Description;
                        NewProductTypesDetails.Description_pt = Description;
                        NewProductTypesDetails.Description_ro = Description;
                        NewProductTypesDetails.Description_ru = Description;
                        NewProductTypesDetails.Description_zh = Description;
                    }

                    NewProductTypesDetails.IdStatus = SelectedStatus.IdLookupValue;
                    NewProductTypesDetails.NameToShow = "";
                    NewProductTypesDetails.Standard = SelectedIndexForRadioButton;

                    if (IsModuleView == true)//[Sudhir.Jangra][GEOS2-4797]
                    {
                        if (SelectedDefaultWayType != null)
                            NewProductTypesDetails.IdDefaultWayType = SelectedDefaultWayType.IdWays;
                        else
                            NewProductTypesDetails.IdDefaultWayType = 103;
                    }


                    if (Detections.Count > 0)
                    {
                        Detections = new ObservableCollection<Detections>(Detections.Where(a => a.IdDetections > 0));
                        //order detection
                        List<Detections> Detection_List = Detections.Where(a => a.Parent != null).ToList();
                        List<string> Groups_Detection = Detection_List.Select(a => a.Parent).Distinct().ToList();
                        foreach (string group in Groups_Detection)
                        {
                            List<Detections> Detection_List_child = Detection_List.Where(a => a.Parent == group).ToList();
                            int Order = 0;
                            foreach (Detections child in Detection_List_child)
                            {
                                Order++;
                                Detections.Where(a => a.IdDetections == child.IdDetections && a.Parent == child.Parent).ToList().ForEach(a => { a.OrderNumber = Order; });
                            }
                        }
                    }

                    if (Options.Count > 0)
                    {
                        Options = new ObservableCollection<Options>(Options.Where(a => a.IdOptions > 0));
                        //order options
                        List<Options> option_List = Options.Where(a => a.Parent != null).ToList();
                        List<string> Groups = option_List.Select(a => a.Parent).Distinct().ToList();
                        foreach (string group in Groups)
                        {
                            List<Options> option_List_child = option_List.Where(a => a.Parent == group).ToList();
                            int Order = 0;
                            foreach (Options child in option_List_child)
                            {
                                Order++;
                                Options.Where(a => a.IdOptions == child.IdOptions && a.Parent == child.Parent).ToList().ForEach(a => { a.OrderNumber = Order; });
                            }
                        }
                    }

                    if (SpareParts.Count > 0)
                    {
                        SpareParts = new ObservableCollection<SpareParts>(SpareParts.Where(a => a.IdSpareParts > 0));
                        //order SpareParts
                        List<SpareParts> SpareParts_List = SpareParts.Where(a => a.Parent != null).ToList();
                        List<string> Groups = SpareParts_List.Select(a => a.Parent).Distinct().ToList();
                        foreach (string group in Groups)
                        {
                            List<SpareParts> SpareParts_List_child = SpareParts_List.Where(a => a.Parent == group).ToList();
                            int Order = 0;
                            foreach (SpareParts child in SpareParts_List_child)
                            {
                                Order++;
                                SpareParts.Where(a => a.IdSpareParts == child.IdSpareParts && a.Parent == child.Parent).ToList().ForEach(a => { a.OrderNumber = Order; });
                            }
                        }
                    }

                    if (IsModuleView == true)//[Sudhir.Jangra][GEOS2-4797]
                    {
                        NewProductTypesDetails.WayList = Ways.ToList();
                    }
                    else
                    {
                        NewProductTypesDetails.WayList = new List<Data.Common.PCM.Ways>();
                    }



                    NewProductTypesDetails.DetectionList = Detections.ToList();
                    NewProductTypesDetails.SparePartList = SpareParts.ToList();
                    NewProductTypesDetails.OptionList = Options.ToList();
                    NewProductTypesDetails.FamilyList = Families.ToList();

                    NewProductTypesDetails.IsEnabled = 1;
                    NewProductTypesDetails.ProductTypeAttachedLinkList = ProductTypeLinksList.ToList();
                    NewProductTypesDetails.ProductTypeAttachedDocList = ProductTypeFilesList.ToList();
                    NewProductTypesDetails.ProductTypeImageList = ImagesList.ToList();
                    //NewProductTypesDetails.CustomerList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();
                    NewProductTypesDetails.CustomerListByCPType = PCMCustomerList;

                    ProductTypeLogEntry tempProductTypeLogEntry = new ProductTypeLogEntry();
                    tempProductTypeLogEntry.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                    tempProductTypeLogEntry.UserName = GeosApplication.Instance.ActiveUser.FullName;
                    tempProductTypeLogEntry.Datetime = GeosApplication.Instance.ServerDateTime;
                    tempProductTypeLogEntry.Comments = string.Format(System.Windows.Application.Current.FindResource("DuplicateProductTypeChangeLog").ToString(), NewProductTypesDetails.Name, ProductTypeOldName);
                    ProductTypesChangeLogList = new ObservableCollection<ProductTypeLogEntry>();
                    ProductTypesChangeLogList.Add(tempProductTypeLogEntry);
                    NewProductTypesDetails.ProductTypeLogEntryList = ProductTypesChangeLogList.ToList();
                    NewProductTypesDetails.ProductTypeCompatibilityList = new List<ProductTypeCompatibility>();

                    foreach (ProductTypeCompatibility item in MandatoryList)
                    {
                        ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                        productTypeCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        NewProductTypesDetails.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                    }

                    foreach (ProductTypeCompatibility item in SuggestedList)
                    {
                        ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                        productTypeCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        NewProductTypesDetails.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                    }

                    foreach (ProductTypeCompatibility item in IncompatibleList)
                    {
                        ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                        productTypeCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        NewProductTypesDetails.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                    }

                    NewProductTypesDetails.ProductTypeImageList.ForEach(x => x.AttachmentImage = null);
                    //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
                    //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
                    // NewProductTypesDetails = PCMService.AddProductType_V2280(NewProductTypesDetails);
                    //UpdateTheIncludedAndNotIncludedPriceList();
                    //[Sudhir.Jangra][Geos-4072][12/12/2022]
                    //NewProductTypesDetails = PCMService.AddProductType_V2340(NewProductTypesDetails);

                    UpdateTheIncludedAndNotIncludedPriceList(NewProductTypesDetails);
                    //PCMService = new PCMServiceController("localhost:6699");

                   // NewProductTypesDetails = PCMService.AddProductType_V2490(NewProductTypesDetails);


                    NewProductTypesDetails = PCMService.AddProductType_V2590(NewProductTypesDetails);          //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]

                    if (IsModuleView)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ModuleAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("StructureAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    }

                    IsNew = false;
                    IsSaveChanges = true;
                    RequestClose(null, null);
                    

                    GeosApplication.Instance.Logger.Log("Method EditSaveModule()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    if (IsModuleView == true)//[Sudhir.Jangra][GEOS2-4797]
                    {
                        if (Ways.ToList().Count > 0 && SelectedDefaultWayType == null)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDefaultWayTypePresent").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method EditSaveModule()...default way type is mandatory.", category: Category.Info, priority: Priority.Low);
                            return;
                        }
                    }


                    ChangeLogList = new ObservableCollection<ProductTypeLogEntry>();

                    UpdateProductTypes = new ProductTypes();
                    UpdateProductTypes.Reference = Reference;
                    UpdateProductTypes.Code = Abbrivation;
                    UpdateProductTypes.IdCPType = ClonedProductType.IdCPType;
                    UpdateProductTypes.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    UpdateProductTypes.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                    if (IsCheckedCopyDescription == true)
                    {
                        UpdateProductTypes.Description = Description;
                        UpdateProductTypes.Description_es = Description;
                        UpdateProductTypes.Description_fr = Description;
                        UpdateProductTypes.Description_pt = Description;
                        UpdateProductTypes.Description_ro = Description;
                        UpdateProductTypes.Description_ru = Description;
                        UpdateProductTypes.Description_zh = Description;

                        UpdateProductTypes.Name = Name;
                        UpdateProductTypes.Name_es = Name;
                        UpdateProductTypes.Name_fr = Name;
                        UpdateProductTypes.Name_pt = Name;
                        UpdateProductTypes.Name_ro = Name;
                        UpdateProductTypes.Name_ru = Name;
                        UpdateProductTypes.Name_zh = Name;
                    }
                    else
                    {
                        UpdateProductTypes.Description = Description_en;
                        UpdateProductTypes.Description_es = Description_es;
                        UpdateProductTypes.Description_fr = Description_fr;
                        UpdateProductTypes.Description_pt = Description_pt;
                        UpdateProductTypes.Description_ro = Description_ro;
                        UpdateProductTypes.Description_ru = Description_ru;
                        UpdateProductTypes.Description_zh = Description_zh;

                        UpdateProductTypes.Name = Name_en;
                        UpdateProductTypes.Name_es = Name_es;
                        UpdateProductTypes.Name_fr = Name_fr;
                        UpdateProductTypes.Name_pt = Name_pt;
                        UpdateProductTypes.Name_ro = Name_ro;
                        UpdateProductTypes.Name_ru = Name_ru;
                        UpdateProductTypes.Name_zh = Name_zh;
                    }

                    UpdateProductTypes.NameToShow = "";
                    UpdateProductTypes.IdTemplate_old = ClonedProductType.IdTemplate;

                    if (SelectedTemplate != null)
                        UpdateProductTypes.IdTemplate = SelectedTemplate.IdTemplate;

                    if (IsModuleView == true)//[Sudhir.Jangra][GEOS2-4797]
                    {
                        if (SelectedDefaultWayType != null)
                        {
                            UpdateProductTypes.IdDefaultWayType = SelectedDefaultWayType.IdWays;
                        }
                        else
                        {
                            UpdateProductTypes.IdDefaultWayType = 103;
                        }
                    }


                    UpdateProductTypes.IdStatus = SelectedStatus.IdLookupValue;

                    UpdateProductTypes.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                    if (ClonedProductType.IdTemplate == 0)
                    {
                        UpdateProductTypes.IsTemplate_NotExist = true;
                    }

                    //if (Detections.Count > 0)
                    //    Detections = new ObservableCollection<Detections>(Detections.Where(a => a.IdDetections > 0));

                    //if (Options.Count > 0)
                    //    Options = new ObservableCollection<Options>(Options.Where(a => a.IdOptions > 0));

                    //if (SpareParts.Count > 0)
                    //    SpareParts = new ObservableCollection<SpareParts>(SpareParts.Where(a => a.IdSpareParts > 0));

                    if (IsModuleView == true)//[Sudhir.Jangra][GEOS2-4797]
                    {
                        //Way
                        UpdateProductTypes.WayList = new List<Ways>();

                        if (Ways.Count > 0)
                        {
                            //order Ways                      
                            uint Order = 1;
                            Ways.ToList().ForEach(a => { a.OrderNumber = Order++; });
                        }

                        foreach (Ways itemWays in ClonedProductType.WayList)
                        {
                            if (!Ways.Any(x => x.IdWays == itemWays.IdWays))
                            {
                                Ways connectorFamilies = (Ways)itemWays.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdateProductTypes.WayList.Add(connectorFamilies);
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogWaysDelete").ToString(), itemWays.Name) });
                            }
                        }

                        foreach (Ways itemWays in Ways)
                        {
                            if (!ClonedProductType.WayList.Any(x => x.IdWays == itemWays.IdWays))
                            {
                                Ways connectorFamilies = (Ways)itemWays.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateProductTypes.WayList.Add(connectorFamilies);
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogWaysAdd").ToString(), itemWays.Name) });
                            }
                        }

                        //Updated Ways
                        foreach (Ways original in ClonedProductType.WayList)
                        {
                            if (Ways.Any(x => x.IdWays == original.IdWays))
                            {
                                Ways updated = Ways.FirstOrDefault(x => x.IdWays == original.IdWays);
                                if (updated.OrderNumber != original.OrderNumber)
                                {
                                    Ways updatedClone = (Ways)updated.Clone();
                                    updatedClone.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    updatedClone.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdateProductTypes.WayList.Add(updatedClone);
                                    using (var productTypeLogEntry = new ProductTypeLogEntry
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOrderWay").ToString(),
                                        updated.Name, original.OrderNumber,
                                        updated.OrderNumber)
                                    })
                                    {
                                        ChangeLogList.Add(productTypeLogEntry);
                                    }
                                }
                            }
                        }

                    }



                    //if (Detections.Count > 0)
                    //{
                    //    List<Detections> Detection_List = Detections.Where(a => a.Parent != null).ToList();
                    //    List<string> Groups_Detection = Detection_List.Select(a => a.Parent).Distinct().ToList();
                    //    foreach (string group in Groups_Detection)
                    //    {
                    //        List<Detections> Detection_List_child = Detection_List.Where(a => a.Parent == group).ToList();
                    //        int Order = 0;
                    //        foreach (Detections child in Detection_List_child)
                    //        {
                    //            Order++;
                    //            Detections.Where(a => a.IdDetections == child.IdDetections && a.Parent == child.Parent).ToList().ForEach(a => { a.OrderNumber = Order; });
                    //        }
                    //    }
                    //}


                    UpdateProductTypes.SparePartList = new List<SpareParts>();

                    // Deleted SpareParts
                    foreach (SpareParts itemSpareParts in ClonedProductType.SparePartsList_Group)
                    {
                        if (!SpareParts.Any(x => x.IdSpareParts == itemSpareParts.IdSpareParts) && itemSpareParts.IdSpareParts != 0)
                        {
                            SpareParts connectorFamilies = (SpareParts)itemSpareParts.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.SparePartList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSparePartsDelete").ToString(), itemSpareParts.Name) });
                        }
                    }

                    //Added SpareParts
                    foreach (SpareParts itemSpareParts in SpareParts)
                    {
                        if (!ClonedProductType.SparePartsList_Group.Any(x => x.IdSpareParts == itemSpareParts.IdSpareParts))
                        {
                            SpareParts connectorFamilies = (SpareParts)itemSpareParts.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProductTypes.SparePartList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSparePartsAdd").ToString(), itemSpareParts.Name) });
                        }
                    }

                    //SpareParts

                    if (SpareParts.Count > 0)
                    {
                        if (IsSparePartDrag == true)
                        {
                            List<SpareParts> withoutGroup = SpareParts.Where(a => a.IdGroup == null).ToList();
                            List<SpareParts> withGroup = SpareParts.Where(a => a.Parent != null).ToList();
                            int order = 1;
                            List<string> groups = withGroup.Select(a => a.Parent).Distinct().ToList();
                            foreach (string group in groups)
                            {
                                List<SpareParts> groupItems = withGroup.Where(a => a.Parent == group).ToList();
                                int group_order = 1;
                                foreach (var item in groupItems)
                                {
                                    if (item.OrderNumber != group_order)
                                    {
                                        item.OrderNumber = group_order;
                                    }
                                    group_order++;
                                }
                            }
                            foreach (var item in withoutGroup)
                            {
                                if (item.OrderNumber != order)
                                {
                                    item.OrderNumber = order;
                                }
                                order++;
                            }

                            //Updated Spareparts
                            foreach (SpareParts originalSparePart in ClonedProductType.SparePartListWithoutGroup)
                            {
                                if (SpareParts.Any(x => x.IdSpareParts == originalSparePart.IdSpareParts))
                                {
                                    SpareParts SparePartsUpdated = SpareParts.FirstOrDefault(x => x.IdSpareParts == originalSparePart.IdSpareParts);
                                    if (SparePartsUpdated.OrderNumber != originalSparePart.OrderNumber)
                                    {
                                        string originalOrderNumber;
                                        if (originalSparePart.OrderNumber == null)
                                        {
                                            originalOrderNumber = "None";
                                        }
                                        else
                                        {
                                            originalOrderNumber = originalSparePart.OrderNumber.ToString();
                                        }
                                        SpareParts SpareParts = (SpareParts)SparePartsUpdated.Clone();
                                        SpareParts.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                        SpareParts.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        UpdateProductTypes.SparePartList.Add(SpareParts);

                                        using (var productTypeLogEntry = new ProductTypeLogEntry
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOrderSparePart").ToString(),
                                            SparePartsUpdated.Name, originalOrderNumber,
                                            SparePartsUpdated.OrderNumber)
                                        })
                                        {
                                            ChangeLogList.Add(productTypeLogEntry);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    UpdateProductTypes.OptionList = new List<Options>();



                    //Deleted Options
                    foreach (Options itemOptions in ClonedProductType.OptionList_Group)
                    {
                        if (!Options.Any(x => x.IdOptions == itemOptions.IdOptions) && itemOptions.IdOptions != 0)
                        {

                            Options connectorFamilies = (Options)itemOptions.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.OptionList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOptionsDelete").ToString(), itemOptions.Name) });
                        }
                    }

                    //Added Options
                    foreach (Options itemOptions in Options)
                    {
                        if (!ClonedProductType.OptionList_Group.Any(x => x.IdOptions == itemOptions.IdOptions))
                        {
                            Options connectorFamilies = (Options)itemOptions.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProductTypes.OptionList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOptionsAdd").ToString(), itemOptions.Name) });
                        }
                    }
                    if (Options.Count > 0)
                    {
                        if (IsOptionDrag == true)
                        {
                            List<Options> withoutGroup = Options.Where(a => a.IdGroup == null).ToList();
                            List<Options> withGroup = Options.Where(a => a.Parent != null).ToList();
                            int order = 1;
                            List<string> groups = withGroup.Select(a => a.Parent).Distinct().ToList();
                            foreach (string group in groups)
                            {
                                List<Options> groupItems = withGroup.Where(a => a.Parent == group).ToList();
                                int group_order = 1;
                                foreach (var item in groupItems)
                                {
                                    if (item.OrderNumber != group_order)
                                    {
                                        item.OrderNumber = group_order;
                                    }
                                    group_order++;
                                }
                            }
                            foreach (var item in withoutGroup)
                            {
                                if (item.OrderNumber != order)
                                {
                                    item.OrderNumber = order;
                                }
                                order++;
                            }




                            //Updated options
                            foreach (Options originalOption in ClonedProductType.OptionListWithoutGroup)
                            {
                                if (Options.Any(x => x.IdOptions == originalOption.IdOptions))
                                {
                                    Options OptionsUpdated = Options.FirstOrDefault(x => x.IdOptions == originalOption.IdOptions);
                                    if (OptionsUpdated.OrderNumber != originalOption.OrderNumber)
                                    {
                                        string originalOrderNumber;
                                        if (originalOption.OrderNumber == null)
                                        {
                                            originalOrderNumber = "None";
                                        }
                                        else
                                        {
                                            originalOrderNumber = originalOption.OrderNumber.ToString();
                                        }
                                        Options Options = (Options)OptionsUpdated.Clone();
                                        Options.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                        Options.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        UpdateProductTypes.OptionList.Add(Options);
                                        // ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOrderOption").ToString(), OptionsUpdated.Name, originalOption.OrderNumber, OptionsUpdated.OrderNumber) });

                                        using (var productTypeLogEntry = new ProductTypeLogEntry
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOrderOption").ToString(),
                                            OptionsUpdated.Name, originalOrderNumber,
                                            OptionsUpdated.OrderNumber)
                                        })
                                        {
                                            ChangeLogList.Add(productTypeLogEntry);
                                        }
                                        // }
                                    }
                                }
                            }
                        }
                    }

                    UpdateProductTypes.DetectionList = new List<Detections>();



                    // Deleted Detections
                    foreach (Detections itemDetection in ClonedProductType.DetectionList_Group)
                    {
                        if (!Detections.Any(x => x.IdDetections == itemDetection.IdDetections) && itemDetection.IdDetections != 0)
                        {
                            Detections connectorFamilies = (Detections)itemDetection.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.DetectionList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDetectionsDelete").ToString(), itemDetection.Name) });
                        }
                    }

                    //Added Detections
                    foreach (Detections itemDetection in Detections)
                    {
                        if (!ClonedProductType.DetectionList_Group.Any(x => x.IdDetections == itemDetection.IdDetections))
                        {
                            Detections connectorFamilies = (Detections)itemDetection.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProductTypes.DetectionList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDetectionsAdd").ToString(), itemDetection.Name) });
                        }
                    }

                    if (Detections.Count > 0)
                    {
                        if (IsDetectionDrag == true)
                        {
                            List<Detections> withoutGroup = Detections.Where(a => a.IdGroup == null).ToList();
                            List<Detections> withGroup = Detections.Where(a => a.Parent != null).ToList();
                            int order = 1;
                            List<string> groups = withGroup.Select(a => a.Parent).Distinct().ToList();
                            foreach (string group in groups)
                            {
                                List<Detections> groupItems = withGroup.Where(a => a.Parent == group).ToList();
                                int group_order = 1;
                                foreach (var item in groupItems)
                                {
                                    if (item.OrderNumber != group_order)
                                    {
                                        item.OrderNumber = group_order;
                                    }
                                    group_order++;
                                }
                            }
                            foreach (var item in withoutGroup)
                            {
                                if (item.OrderNumber != order)
                                {
                                    item.OrderNumber = order;
                                }
                                order++;
                            }



                            //Updated Detections
                            foreach (Detections originalDetection in ClonedProductType.DetectionListWithoutGroup)
                            {
                                if (Detections.Any(x => x.IdDetections == originalDetection.IdDetections))
                                {
                                    Detections DetectionsUpdated = Detections.FirstOrDefault(x => x.IdDetections == originalDetection.IdDetections);
                                    if (DetectionsUpdated.OrderNumber != originalDetection.OrderNumber)
                                    {
                                        string originalOrderNumber;
                                        if (originalDetection.OrderNumber == null)
                                        {
                                            originalOrderNumber = "None";
                                        }
                                        else
                                        {
                                            originalOrderNumber = originalDetection.OrderNumber.ToString();
                                        }
                                        Detections Detections = (Detections)DetectionsUpdated.Clone();
                                        Detections.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                        Detections.TransactionOperation = ModelBase.TransactionOperations.Update;


                                        UpdateProductTypes.DetectionList.Add(Detections);
                                        using (var productTypeLogEntry = new ProductTypeLogEntry
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOrderDetection").ToString(),
                                            DetectionsUpdated.Name, originalOrderNumber,
                                            DetectionsUpdated.OrderNumber)
                                        })
                                        {
                                            ChangeLogList.Add(productTypeLogEntry);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Ensure all lists are initialized //[rdixit][GEOS2-6624][10.12.2024]
                    UpdateProductTypes.FamilyList = new List<ConnectorFamilies>();

                    if (ClonedProductType?.FamilyList != null && Families != null && ChangeLogList != null)
                    {
                        // Deleted Subfamily
                        var clonedSubFamilies = ClonedProductType.FamilyList.Where(i => i.IdSubFamilyConnector > 0);
                        foreach (var itemFamily in clonedSubFamilies)
                        {
                            if (!Families.Any(x => x.IdSubFamilyConnector > 0 && x.IdSubFamilyConnector == itemFamily.IdSubFamilyConnector))
                            {
                                var connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdateProductTypes.FamilyList.Add(connectorFamilies);
                                var FamilyName = ClonedProductType.FamilyList.FirstOrDefault(i => i.IdFamily == itemFamily.IdFamily && i.IdSubFamilyConnector == 0)?.Name;
                                ChangeLogList.Add(new ProductTypeLogEntry
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSubFamilyDelete")?.ToString() ?? string.Empty, itemFamily.Name, FamilyName)
                                });
                            }
                        }

                        // Added Subfamily
                        var familiesWithSubFamily = Families.Where(i => i.IdSubFamilyConnector > 0);
                        foreach (var itemFamily in familiesWithSubFamily)
                        {
                            if (!ClonedProductType.FamilyList.Any(x => x.IdSubFamilyConnector > 0 && x.IdSubFamilyConnector == itemFamily.IdSubFamilyConnector))
                            {
                                var connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateProductTypes.FamilyList.Add(connectorFamilies);
                                var FamilyName = Families.FirstOrDefault(i => i.IdFamily == itemFamily.IdFamily && i.IdSubFamilyConnector == 0)?.Name;
                                ChangeLogList.Add(new ProductTypeLogEntry
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSubFamilyAdd")?.ToString() ?? string.Empty, itemFamily.Name, FamilyName)
                                });
                            }
                        }

                        // Deleted Family
                        var clonedFamilies = ClonedProductType.FamilyList.Where(i => i.IdSubFamilyConnector == 0);
                        foreach (var itemFamily in clonedFamilies)
                        {
                            if (!Families.Any(x => x.IdSubFamilyConnector == 0 && x.IdFamily == itemFamily.IdFamily))
                            {
                                var connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdateProductTypes.FamilyList.Add(connectorFamilies);

                                ChangeLogList.Add(new ProductTypeLogEntry
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFamilyDelete")?.ToString() ?? string.Empty, itemFamily.Name)
                                });
                            }
                        }

                        // Added Family
                        var familiesWithoutSubFamily = Families.Where(i => i.IdSubFamilyConnector == 0);
                        foreach (var itemFamily in familiesWithoutSubFamily)
                        {
                            if (!ClonedProductType.FamilyList.Any(x => x.IdSubFamilyConnector == 0 && x.IdFamily == itemFamily.IdFamily))
                            {
                                var connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateProductTypes.FamilyList.Add(connectorFamilies);

                                ChangeLogList.Add(new ProductTypeLogEntry
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFamilyAdd")?.ToString() ?? string.Empty, itemFamily.Name)
                                });
                            }
                        }
                    }


                    //Files
                    UpdateProductTypes.ProductTypeAttachedDocList = new List<ProductTypeAttachedDoc>();
                    // Delete ProductType file
                    foreach (ProductTypeAttachedDoc item in ClonedProductType.ProductTypeAttachedDocList)
                    {

                        if (ProductTypeFilesList != null && !ProductTypeFilesList.Any(x => x.IdCPTypeAttachedDoc == item.IdCPTypeAttachedDoc))
                        {
                            ProductTypeAttachedDoc productTypeAttachedDoc = (ProductTypeAttachedDoc)item.Clone();
                            productTypeAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeAttachedDocList.Add(productTypeAttachedDoc);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                        }
                    }

                    //Added ProductType file
                    if (ProductTypeFilesList != null)
                    {
                        foreach (ProductTypeAttachedDoc item in ProductTypeFilesList)
                        {
                            if (!ClonedProductType.ProductTypeAttachedDocList.Any(x => x.IdCPTypeAttachedDoc == item.IdCPTypeAttachedDoc))
                            {
                                ProductTypeAttachedDoc productTypeAttachedDoc = (ProductTypeAttachedDoc)item.Clone();
                                productTypeAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateProductTypes.ProductTypeAttachedDocList.Add(productTypeAttachedDoc);
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                    }

                    //Updated ProductType file

                    foreach (ProductTypeAttachedDoc originalProductType in ClonedProductType.ProductTypeAttachedDocList)
                    {
                        if (ProductTypeFilesList != null && ProductTypeFilesList.Any(x => x.IdCPTypeAttachedDoc == originalProductType.IdCPTypeAttachedDoc))
                        {
                            ProductTypeAttachedDoc productTypeAttachedDocUpdated = ProductTypeFilesList.FirstOrDefault(x => x.IdCPTypeAttachedDoc == originalProductType.IdCPTypeAttachedDoc);
                            //Shubham[skadam] GEOS2-4126 Nothing happens when pressing Accept button in Edit Module  10 01 2023
                            if (productTypeAttachedDocUpdated.AttachmentType != null)
                                if ((productTypeAttachedDocUpdated.SavedFileName != originalProductType.SavedFileName) || (productTypeAttachedDocUpdated.OriginalFileName != originalProductType.OriginalFileName) || (productTypeAttachedDocUpdated.Description != originalProductType.Description) || (productTypeAttachedDocUpdated.AttachmentType.IdLookupValue != originalProductType.AttachmentType.IdLookupValue))
                                {
                                    ProductTypeAttachedDoc ProductTypeAttachedDoc = (ProductTypeAttachedDoc)productTypeAttachedDocUpdated.Clone();
                                    ProductTypeAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    ProductTypeAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdateProductTypes.ProductTypeAttachedDocList.Add(ProductTypeAttachedDoc);
                                    if (productTypeAttachedDocUpdated.ProductTypeAttachedDocInBytes != originalProductType.ProductTypeAttachedDocInBytes)
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesUpdate").ToString(), originalProductType.SavedFileName, productTypeAttachedDocUpdated.SavedFileName) });
                                    if ((productTypeAttachedDocUpdated.OriginalFileName != originalProductType.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(productTypeAttachedDocUpdated.OriginalFileName))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesNameUpdate").ToString(), ClonedProductType.Name, originalProductType.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalProductType.OriginalFileName))
                                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesNameUpdate").ToString(), ClonedProductType.Name, "None", productTypeAttachedDocUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesNameUpdate").ToString(), ClonedProductType.Name, originalProductType.OriginalFileName, productTypeAttachedDocUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (productTypeAttachedDocUpdated.Description != originalProductType.Description)
                                    {
                                        if (string.IsNullOrEmpty(productTypeAttachedDocUpdated.Description))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), ClonedProductType.Name, originalProductType.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalProductType.Description))
                                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), ClonedProductType.Name, "None", productTypeAttachedDocUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), ClonedProductType.Name, originalProductType.Description, productTypeAttachedDocUpdated.Description) });
                                        }
                                    }
                                    if (productTypeAttachedDocUpdated.AttachmentType != originalProductType.AttachmentType)
                                    {
                                        if (originalProductType.AttachmentType != null)
                                        {
                                            if (originalProductType.AttachmentType.IdLookupValue == 0)
                                            {
                                                ChangeLogList.Add(new ProductTypeLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesTypeUpdate").ToString(),
                                               productTypeAttachedDocUpdated.OriginalFileName, "None", productTypeAttachedDocUpdated.AttachmentType.Value)
                                                });
                                            }
                                        }
                                        if (productTypeAttachedDocUpdated.AttachmentType == null)
                                        {
                                            if (productTypeAttachedDocUpdated.AttachmentType.IdLookupValue == 0)
                                                ChangeLogList.Add(new ProductTypeLogEntry()
                                                {
                                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesTypeUpdate").ToString(),
                                                    productTypeAttachedDocUpdated.OriginalFileName, originalProductType.AttachmentType.Value, "None")
                                                });
                                        }
                                        if (originalProductType.AttachmentType.IdLookupValue != productTypeAttachedDocUpdated.AttachmentType.IdLookupValue && originalProductType.AttachmentType.IdLookupValue != 0 && productTypeAttachedDocUpdated.AttachmentType.IdLookupValue != 0)
                                        {
                                            ChangeLogList.Add(new ProductTypeLogEntry()
                                            {
                                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,
                                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesTypeUpdate").ToString(),
                                            productTypeAttachedDocUpdated.OriginalFileName, originalProductType.AttachmentType.Value, productTypeAttachedDocUpdated.AttachmentType.Value)
                                            });
                                        }

                                    }
                                }
                        }

                    }
                    //Links
                    UpdateProductTypes.ProductTypeAttachedLinkList = new List<ProductTypeAttachedLink>();
                    // Delete ProductType link
                    foreach (ProductTypeAttachedLink item in ClonedProductType.ProductTypeAttachedLinkList)
                    {
                        if (ProductTypeLinksList != null && !ProductTypeLinksList.Any(x => x.IdCPTypeAttachedLink == item.IdCPTypeAttachedLink))
                        {
                            ProductTypeAttachedLink productTypeAttachedLink = (ProductTypeAttachedLink)item.Clone();
                            productTypeAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeAttachedLinkList.Add(productTypeAttachedLink);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinksDelete").ToString(), item.Name) });
                        }
                    }

                    //Added ProductType link
                    //if (ProductTypeLinksList != null)
                    {
                        foreach (ProductTypeAttachedLink item in ProductTypeLinksList)
                        {
                            if (!ClonedProductType.ProductTypeAttachedLinkList.Any(x => x.IdCPTypeAttachedLink == item.IdCPTypeAttachedLink))
                            {
                                ProductTypeAttachedLink productTypeAttachedLink = (ProductTypeAttachedLink)item.Clone();
                                productTypeAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateProductTypes.ProductTypeAttachedLinkList.Add(productTypeAttachedLink);
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinksAdd").ToString(), item.Name) });
                            }
                        }
                    }

                    //Updated ProductType link
                    foreach (ProductTypeAttachedLink originalProductType in ClonedProductType.ProductTypeAttachedLinkList)
                    {
                        if (ProductTypeLinksList != null && ProductTypeLinksList.Any(x => x.IdCPTypeAttachedLink == originalProductType.IdCPTypeAttachedLink))
                        {
                            ProductTypeAttachedLink productTypeAttachedLinkUpdated = ProductTypeLinksList.FirstOrDefault(x => x.IdCPTypeAttachedLink == originalProductType.IdCPTypeAttachedLink);
                            if ((productTypeAttachedLinkUpdated.Name != originalProductType.Name) || (productTypeAttachedLinkUpdated.Description != originalProductType.Description))
                            {
                                ProductTypeAttachedLink productTypeAttachedLink = (ProductTypeAttachedLink)productTypeAttachedLinkUpdated.Clone();
                                productTypeAttachedLink.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                productTypeAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.ProductTypeAttachedLinkList.Add(productTypeAttachedLink);

                                if (productTypeAttachedLinkUpdated.Address != originalProductType.Address)
                                {
                                    if (string.IsNullOrEmpty(productTypeAttachedLinkUpdated.Address))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkURLUpdate").ToString(), originalProductType.Address, "None") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalProductType.Address))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkURLUpdate").ToString(), "None", productTypeAttachedLinkUpdated.Address) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkURLUpdate").ToString(), originalProductType.Address, productTypeAttachedLinkUpdated.Address) });
                                    }
                                }
                                if ((productTypeAttachedLinkUpdated.Name != originalProductType.Name))
                                {
                                    if (string.IsNullOrEmpty(productTypeAttachedLinkUpdated.Name))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkNameUpdate").ToString(), originalProductType.Name, "None") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalProductType.Name))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkNameUpdate").ToString(), "None", productTypeAttachedLinkUpdated.Name) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkNameUpdate").ToString(), originalProductType.Name, productTypeAttachedLinkUpdated.Name) });
                                    }
                                }

                                if (productTypeAttachedLinkUpdated.Description != originalProductType.Description)
                                {
                                    if (string.IsNullOrEmpty(productTypeAttachedLinkUpdated.Description))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkDescriptionUpdate").ToString(), originalProductType.Description, "None") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalProductType.Description))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkDescriptionUpdate").ToString(), "None", productTypeAttachedLinkUpdated.Description) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogLinkDescriptionUpdate").ToString(), originalProductType.Description, productTypeAttachedLinkUpdated.Description) });
                                    }
                                }
                            }
                        }
                    }

                    //Images
                    UpdateProductTypes.ProductTypeImageList = new List<ProductTypeImage>();

                    foreach (ProductTypeImage item in ClonedProductType.ProductTypeImageList)
                    {
                        if (!ImagesList.Any(x => x.IdCPTypeImage == item.IdCPTypeImage))
                        {
                            ProductTypeImage productTypeImage = (ProductTypeImage)item.Clone();
                            productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeImageList.Add(productTypeImage);
                            if (productTypeImage.Position == 1)
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                            else
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                        }
                    }
                    //Added ProductType Image
                    foreach (ProductTypeImage item in ImagesList)
                    {
                        if (!ClonedProductType.ProductTypeImageList.Any(x => x.IdCPTypeImage == item.IdCPTypeImage))
                        {
                            ProductTypeImage productTypeImage = (ProductTypeImage)item.Clone();
                            productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProductTypes.ProductTypeImageList.Add(productTypeImage);
                            if (productTypeImage.Position == 1)
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                            else
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                        }
                    }
                    //Updated ProductType Image
                    foreach (ProductTypeImage originalproductTypeImage in ClonedProductType.ProductTypeImageList)
                    {
                        if (ImagesList != null && ImagesList.Any(x => x.IdCPTypeImage == originalproductTypeImage.IdCPTypeImage))
                        {
                            ProductTypeImage productTypeImageUpdated = ImagesList.FirstOrDefault(x => x.IdCPTypeImage == originalproductTypeImage.IdCPTypeImage);
                            if ((productTypeImageUpdated.OriginalFileName != originalproductTypeImage.OriginalFileName) ||
                                (productTypeImageUpdated.SavedFileName != originalproductTypeImage.SavedFileName) ||
                                (productTypeImageUpdated.Description != originalproductTypeImage.Description ||
                                productTypeImageUpdated.Position != originalproductTypeImage.Position))
                            {
                                ProductTypeImage productTypeImage = (ProductTypeImage)productTypeImageUpdated.Clone();
                                productTypeImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.ProductTypeImageList.Add(productTypeImage);
                                if (productTypeImageUpdated.ProductTypeImageInBytes != originalproductTypeImage.ProductTypeImageInBytes)
                                    ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesUpdate").ToString(), originalproductTypeImage.SavedFileName, productTypeImageUpdated.SavedFileName) });
                                if ((productTypeImageUpdated.OriginalFileName != originalproductTypeImage.OriginalFileName))
                                {
                                    if (string.IsNullOrEmpty(productTypeImageUpdated.OriginalFileName))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalproductTypeImage.OriginalFileName, "None") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalproductTypeImage.OriginalFileName))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), "None", productTypeImageUpdated.OriginalFileName) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageNameUpdate").ToString(), originalproductTypeImage.OriginalFileName, productTypeImageUpdated.OriginalFileName) });
                                    }
                                }
                                if (productTypeImageUpdated.Description != originalproductTypeImage.Description)
                                {
                                    if (string.IsNullOrEmpty(productTypeImageUpdated.Description))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalproductTypeImage.Description, "None") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalproductTypeImage.Description))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), "None", productTypeImageUpdated.Description) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImageDescriptionUpdate").ToString(), originalproductTypeImage.Description, productTypeImageUpdated.Description) });
                                    }
                                }
                                if (productTypeImageUpdated.Position != originalproductTypeImage.Position)//[rdixit][GEOS2-2694][01.08.2022]
                                {
                                    if (productTypeImageUpdated.IdCPTypeImage != 1)
                                    {
                                        if (productTypeImageUpdated.Position == 1)
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalproductTypeImage.Position, productTypeImageUpdated.Position, originalproductTypeImage.OriginalFileName) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalproductTypeImage.Position, productTypeImageUpdated.Position, originalproductTypeImage.OriginalFileName) });
                                    }
                                }
                            }
                        }
                    }
                    ProductTypeImage tempDefaultImage = ClonedProductType.ProductTypeImageList.FirstOrDefault(x => x.Position == 1);
                    ProductTypeImage tempDefaultImage_updated = ImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdCPTypeImage != tempDefaultImage_updated.IdCPTypeImage)
                    {
                        if (tempDefaultImage_updated.Position == 1)
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                    }
                    //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
                    //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
                    //Customers add and Delete
                    if (UpdateProductTypes.CustomerListByCPType == null)
                        UpdateProductTypes.CustomerListByCPType = new List<CPLCustomer>();
                    List<CPLCustomer> tempCustomersList = PCMCustomerList;
                   // List<CPLCustomer> clonedCustomerListByCPType = new List<CPLCustomer>(PCMService.GetCustomersWithRegions_V2280(ProductTypeItem.IdCPType));

                    List<CPLCustomer> clonedCustomerListByCPType = new List<CPLCustomer>(PCMService.GetCustomersWithRegions_V2530(ProductTypeItem.IdCPType));

                    // Delete Customer
                    foreach (CPLCustomer item in clonedCustomerListByCPType)
                    {
                        if (PCMCustomerList != null && !PCMCustomerList.Any(x => x.IdCustomerPriceListCustomer == item.IdCustomerPriceListCustomer))
                        {
                            CPLCustomer CPLCustomer = (CPLCustomer)item.Clone();
                            CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.CustomerListByCPType.Add(CPLCustomer);
                            ChangeLogList.Add(new ProductTypeLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(),
                             item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name)
                            });
                        }
                    }

                    //Added Customer
                    if (PCMCustomerList != null)
                    {
                        foreach (CPLCustomer item in PCMCustomerList)
                        {
                            if (!clonedCustomerListByCPType.Any(x => x.IdCustomerPriceListCustomer == item.IdCustomerPriceListCustomer))
                            {
                                CPLCustomer CPLCustomer = (CPLCustomer)item.Clone();
                                CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateProductTypes.CustomerListByCPType.Add(CPLCustomer);
                                ChangeLogList.Add(new ProductTypeLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(),
                                 item.GroupName, item.RegionName, item.Plant.Name, item.Country.Name)
                                });

                            }
                        }
                    }


                    //Updated Customer
                    foreach (CPLCustomer originalCustomer in clonedCustomerListByCPType)
                    {
                        if (PCMCustomerList != null && PCMCustomerList.Any(x => x.IdCustomerPriceListCustomer == originalCustomer.IdCustomerPriceListCustomer))
                        {
                            CPLCustomer CPLCustomerUpdated = PCMCustomerList.FirstOrDefault(x => x.IdCustomerPriceListCustomer == originalCustomer.IdCustomerPriceListCustomer);
                            if ((CPLCustomerUpdated.IdGroup != originalCustomer.IdGroup) || (CPLCustomerUpdated.IdRegion != originalCustomer.IdRegion) || (CPLCustomerUpdated.IdCountry != originalCustomer.IdCountry) || (CPLCustomerUpdated.IdPlant != originalCustomer.IdPlant))
                            {
                                CPLCustomer CPLCustomer = (CPLCustomer)CPLCustomerUpdated.Clone();
                                CPLCustomer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.CustomerListByCPType.Add(CPLCustomer);
                                ChangeLogList.Add(new ProductTypeLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersUpdate").ToString(),
                                    originalCustomer.GroupName, originalCustomer.RegionName,
                                    originalCustomer.Country.Name, originalCustomer.Plant.Name,
                                    CPLCustomer.GroupName, CPLCustomer.RegionName,
                                    CPLCustomer.Country.Name, CPLCustomer.Plant.Name)
                                });
                            }
                        }
                    }


                    #region oldcodeGEOS2-3276
                    //UpdateProductTypes.CustomerList = new List<RegionsByCustomer>();
                    //List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                    //foreach (RegionsByCustomer item in ClonedProductType.CustomerList)
                    //{
                    //    if (item.IsChecked == true)
                    //    {
                    //        RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                    //        if (obj1.IsChecked == false)
                    //        {
                    //            RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                    //            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    //            UpdateProductTypes.CustomerList.Add(connectorFamilies);
                    //            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                    //        }
                    //    }
                    //}

                    //foreach (RegionsByCustomer item in tempCustomersList)
                    //{
                    //    RegionsByCustomer obj1 = ClonedProductType.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                    //    if (obj1.IsChecked == false)
                    //    {
                    //        RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                    //        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                    //        UpdateProductTypes.CustomerList.Add(connectorFamilies);
                    //        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                    //    }
                    //}
                    #endregion
                    // Compatibility
                    UpdateProductTypes.ProductTypeCompatibilityList = new List<ProductTypeCompatibility>();

                    // Delete Compatibility
                    foreach (ProductTypeCompatibility item in ClonedProductType.ProductTypeCompatibilityList)
                    {
                        if (item.IdTypeCompatibility == 249 && MandatoryList != null && !MandatoryList.Any(x => x.IdCompatibility == item.IdCompatibility))
                        {
                            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                            productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                        }
                        if (item.IdTypeCompatibility == 250 && SuggestedList != null && !SuggestedList.Any(x => x.IdCompatibility == item.IdCompatibility))
                        {
                            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                            productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                        }
                        if (item.IdTypeCompatibility == 251 && IncompatibleList != null && !IncompatibleList.Any(x => x.IdCompatibility == item.IdCompatibility))
                        {
                            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                            productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                        }
                    }

                    //Added Compatibility
                    foreach (ProductTypeCompatibility item in MandatoryList)
                    {
                        if (!ClonedProductType.ProductTypeCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                        {
                            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                            productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                            productTypeCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                        }
                    }
                    foreach (ProductTypeCompatibility item in SuggestedList)
                    {
                        if (!ClonedProductType.ProductTypeCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                        {
                            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                            productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                            productTypeCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                        }
                    }
                    foreach (ProductTypeCompatibility item in IncompatibleList)
                    {
                        if (!ClonedProductType.ProductTypeCompatibilityList.Any(x => x.IdCompatibility == item.IdCompatibility))
                        {
                            ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)item.Clone();
                            productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Add;
                            productTypeCompatibility.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogIncompatibleAdd").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                        }
                    }

                    //Updated Compatibility
                    foreach (ProductTypeCompatibility originalCompatibility in ClonedProductType.ProductTypeCompatibilityList)
                    {
                        if (originalCompatibility.IdTypeCompatibility == 249 && MandatoryList != null && MandatoryList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                        {
                            ProductTypeCompatibility MandatoryUpdated = MandatoryList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                            if ((MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements) || (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements) || (MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                            {
                                ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)MandatoryUpdated.Clone();
                                productTypeCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);

                                if (MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements)
                                    ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMinUpdate").ToString(), originalCompatibility.MinimumElements, MandatoryUpdated.MinimumElements) });

                                if (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements)
                                    ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMaxUpdate").ToString(), originalCompatibility.MaximumElements, MandatoryUpdated.MaximumElements) });

                                if ((MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                                {
                                    if (string.IsNullOrEmpty(MandatoryUpdated.Remarks))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", MandatoryUpdated.IdTypeCompatibility == 249 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 249 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 249 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    }
                                }
                            }
                        }

                        if (originalCompatibility.IdTypeCompatibility == 250 && SuggestedList != null && SuggestedList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                        {
                            ProductTypeCompatibility SuggestedUpdated = SuggestedList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                            if (SuggestedUpdated.Remarks != originalCompatibility.Remarks)
                            {
                                ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)SuggestedUpdated.Clone();
                                productTypeCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);

                                if ((SuggestedUpdated.Remarks != originalCompatibility.Remarks))
                                {
                                    if (string.IsNullOrEmpty(SuggestedUpdated.Remarks))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", SuggestedUpdated.IdTypeCompatibility == 249 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 249 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 249 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    }
                                }
                            }
                        }

                        if (originalCompatibility.IdTypeCompatibility == 251 && IncompatibleList != null && IncompatibleList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                        {
                            ProductTypeCompatibility IncompatibleUpdated = IncompatibleList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);

                            if ((IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType) || (IncompatibleUpdated.Quantity != originalCompatibility.Quantity) || (IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                            {
                                ProductTypeCompatibility productTypeCompatibility = (ProductTypeCompatibility)IncompatibleUpdated.Clone();
                                productTypeCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                                productTypeCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.ProductTypeCompatibilityList.Add(productTypeCompatibility);

                                if (IncompatibleUpdated.IdRelationshipType != originalCompatibility.IdRelationshipType)
                                {
                                    if (originalCompatibility.RelationshipType == null)
                                        originalCompatibility.RelationshipType = new LookupValue();

                                    string relationShip = RelationShipList.FirstOrDefault(a => a.IdLookupValue == productTypeCompatibility.IdRelationshipType).Value;

                                    ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelationshipUpdate").ToString(), originalCompatibility.RelationshipType.Value, relationShip) });
                                }

                                if ((IncompatibleUpdated.Quantity != originalCompatibility.Quantity))
                                {
                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Quantity.ToString()))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, "None") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalCompatibility.Quantity.ToString()))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), "None", IncompatibleUpdated.Quantity) });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogQtyUpdate").ToString(), originalCompatibility.Quantity, IncompatibleUpdated.Quantity) });
                                    }
                                }


                                if ((IncompatibleUpdated.Remarks != originalCompatibility.Remarks))
                                {
                                    if (string.IsNullOrEmpty(IncompatibleUpdated.Remarks))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", IncompatibleUpdated.IdTypeCompatibility == 249 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    else
                                    {
                                        if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 249 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                        else
                                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 249 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    }
                                }
                            }
                        }
                    }

                    #region GEOS2-4935 [Sudhir.Jangra]
                    UpdateProductTypes.ProductTypeCommentList = new List<ProductTypeLogEntry>();
                    //Deleted Comments
                    foreach (ProductTypeLogEntry itemComments in ClonedProductType.ProductTypeCommentList)
                    {
                        if (!CommentsList.Any(x => x.IdLogEntryByCptype == itemComments.IdLogEntryByCptype) && itemComments.IdLogEntryByCptype != 0)
                        {
                            ProductTypeLogEntry comments = (ProductTypeLogEntry)itemComments.Clone();
                            comments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.ProductTypeCommentList.Add(comments);                   
                        }
                    }

                    //Added Comments
                    foreach (ProductTypeLogEntry itemComments in CommentsList)
                    {
                        if (!ClonedProductType.ProductTypeCommentList.Any(x => x.IdLogEntryByCptype == itemComments.IdLogEntryByCptype))
                        {
                            ProductTypeLogEntry comments = (ProductTypeLogEntry)itemComments.Clone();
                            comments.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProductTypes.ProductTypeCommentList.Add(comments);                       
                        }
                    }


                    //Update Comments
                    foreach (ProductTypeLogEntry originalComments in ClonedProductType.ProductTypeCommentList)
                    {
                        if (CommentsList.Any(x => x.IdLogEntryByCptype == originalComments.IdLogEntryByCptype))
                        {
                            ProductTypeLogEntry commentsUpdated = CommentsList.FirstOrDefault(x => x.IdLogEntryByCptype == originalComments.IdLogEntryByCptype);
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
                                ProductTypeLogEntry comments = (ProductTypeLogEntry)commentsUpdated.Clone();
                                comments.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                comments.ModifiedDate = DateTime.Now;
                                comments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProductTypes.ProductTypeCommentList.Add(comments);
                            }
                        }
                    }
                    #endregion

                    UpdateProductTypes.ProductTypeCommentList.ForEach(x => x.People.OwnerImage = null);

                    UpdateProductTypes.ProductTypeImageList.ForEach(x => x.AttachmentImage = null);

                    AddProductTypeLogDetails();
                    UpdateTheIncludedAndNotIncludedPriceList();
                    UpdateProductTypes.ProductTypeLogEntryList = ChangeLogList.ToList();
                    #region
                    //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
                    //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
                    //  IsSaveChanges = PCMService.UpdateProductType_V2280(UpdateProductTypes);
                    //[Sudhir.Jangra][Geos2-4072][12/12/2022][Add New Service Version Wise]
                    //IsSaveChanges = PCMService.UpdateProductType_V2340(UpdateProductTypes);
                    //[Sudhir.Jangra][GEOS2-4935]
                    //IsSaveChanges = PCMService.UpdateProductType_V2470(UpdateProductTypes);
                    //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
                    //PCMService = new PCMServiceController("localhost:6699");                    
                    // IsSaveChanges = PCMService.UpdateProductType_V2490(UpdateProductTypes);
                    //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
                    //Service updated from UpdateProductType_V2530 to UpdateProductType_V2590 by [rdixit][GEOS2-6624][10.12.2024]
                    #endregion
                    IsSaveChanges = PCMService.UpdateProductType_V2590(UpdateProductTypes); 

                    if (IsModuleView)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ModuleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("StructureUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }

                    RequestClose(null, null);

                    GeosApplication.Instance.Logger.Log("Method EditSaveModule()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditSaveModule() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditSaveModule() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditSaveModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddProductTypeLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddProductTypeLogDetails()...", category: Category.Info, priority: Priority.Low);

                //Status
                if (ProductTypesDetails.Status.IdLookupValue != UpdateProductTypes.IdStatus)
                {
                    LookupValue tempStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == UpdateProductTypes.IdStatus);
                    if (tempStatus != null)
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), ProductTypesDetails.Status.Value, tempStatus.Value) });
                }

                if (ProductTypesDetails.IdTemplate != UpdateProductTypes.IdTemplate)
                {
                    Template tempTemplate = TemplatesMenuList.FirstOrDefault(x => x.IdTemplate == UpdateProductTypes.IdTemplate);
                    if (string.IsNullOrEmpty(tempTemplate.Name))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogTemplateUpdate").ToString(), ProductTypesDetails.Template.Name, "None") });
                    else
                    {
                        if (ProductTypesDetails.Template == null)
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogTemplateUpdate").ToString(), "None", tempTemplate.Name) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogTemplateUpdate").ToString(), ProductTypesDetails.Template.Name, tempTemplate.Name) });
                    }
                }

                //Name and description

                if (ProductTypesDetails.Name != UpdateProductTypes.Name)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogName").ToString(), ProductTypesDetails.Name, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ProductTypesDetails.Name))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogName").ToString(), "None", UpdateProductTypes.Name) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogName").ToString(), ProductTypesDetails.Name, UpdateProductTypes.Name) });
                    }
                }

                if (ProductTypesDetails.Name_es != UpdateProductTypes.Name_es)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name_es))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameES").ToString(), ProductTypesDetails.Name_es, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Name_es))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameES").ToString(), "None", UpdateProductTypes.Name_es) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameES").ToString(), ProductTypesDetails.Name_es, UpdateProductTypes.Name_es) });
                    }
                }

                if (ProductTypesDetails.Name_fr != UpdateProductTypes.Name_fr)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name_fr))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameFR").ToString(), ProductTypesDetails.Name_fr, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Name_fr))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameFR").ToString(), "None", UpdateProductTypes.Name_fr) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameFR").ToString(), ProductTypesDetails.Name_fr, UpdateProductTypes.Name_fr) });
                    }
                }

                if (ProductTypesDetails.Name_pt != UpdateProductTypes.Name_pt)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name_pt))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNamePT").ToString(), ProductTypesDetails.Name_pt, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Name_pt))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNamePT").ToString(), "None", UpdateProductTypes.Name_pt) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNamePT").ToString(), ProductTypesDetails.Name_pt, UpdateProductTypes.Name_pt) });
                    }
                }

                if (ProductTypesDetails.Name_ro != UpdateProductTypes.Name_ro)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name_ro))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameRO").ToString(), ProductTypesDetails.Name_ro, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Name_ro))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameRO").ToString(), "None", UpdateProductTypes.Name_ro) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameRO").ToString(), ProductTypesDetails.Name_ro, UpdateProductTypes.Name_ro) });
                    }
                }

                if (ProductTypesDetails.Name_ru != UpdateProductTypes.Name_ru)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name_ru))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameRU").ToString(), ProductTypesDetails.Name_ru, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Name_ru))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameRU").ToString(), "None", UpdateProductTypes.Name_ru) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameRU").ToString(), ProductTypesDetails.Name_ru, UpdateProductTypes.Name_ru) });
                    }
                }

                if (ProductTypesDetails.Name_zh != UpdateProductTypes.Name_zh)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Name_zh))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameZH").ToString(), ProductTypesDetails.Name_zh, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Name_zh))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameZH").ToString(), "None", UpdateProductTypes.Name_zh) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogNameZH").ToString(), ProductTypesDetails.Name_zh, UpdateProductTypes.Name_zh) });
                    }
                }

                if (ProductTypesDetails.Description != UpdateProductTypes.Description)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescription").ToString(), ProductTypesDetails.Description, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ProductTypesDetails.Description))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescription").ToString(), "None", UpdateProductTypes.Description) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescription").ToString(), ProductTypesDetails.Description, UpdateProductTypes.Description) });
                    }
                }

                if (ProductTypesDetails.Description_es != UpdateProductTypes.Description_es)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description_es))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionES").ToString(), ProductTypesDetails.Description_es, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Description_es))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionES").ToString(), "None", UpdateProductTypes.Description_es) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionES").ToString(), ProductTypesDetails.Description_es, UpdateProductTypes.Description_es) });
                    }
                }

                if (ProductTypesDetails.Description_fr != UpdateProductTypes.Description_fr)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description_fr))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionFR").ToString(), ProductTypesDetails.Description_es, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Description_fr))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionFR").ToString(), "None", UpdateProductTypes.Description_fr) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionFR").ToString(), ProductTypesDetails.Description_fr, UpdateProductTypes.Description_fr) });
                    }
                }

                if (ProductTypesDetails.Description_pt != UpdateProductTypes.Description_pt)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description_pt))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionPT").ToString(), ProductTypesDetails.Description_pt, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Description_pt))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionPT").ToString(), "None", UpdateProductTypes.Description_pt) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionPT").ToString(), ProductTypesDetails.Description_pt, UpdateProductTypes.Description_pt) });
                    }
                }

                if (ProductTypesDetails.Description_ro != UpdateProductTypes.Description_ro)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description_ro))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionRO").ToString(), ProductTypesDetails.Description_ro, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Description_ro))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionRO").ToString(), "None", UpdateProductTypes.Description_ro) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionRO").ToString(), ProductTypesDetails.Description_ro, UpdateProductTypes.Description_ro) });
                    }
                }

                if (ProductTypesDetails.Description_ru != UpdateProductTypes.Description_ru)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description_ru))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionRU").ToString(), ProductTypesDetails.Description_ru, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Description_ru))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionRU").ToString(), "None", UpdateProductTypes.Description_ru) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionRU").ToString(), ProductTypesDetails.Description_ru, UpdateProductTypes.Description_ru) });
                    }
                }

                if (ProductTypesDetails.Description_zh != UpdateProductTypes.Description_zh)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Description_zh))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionZH").ToString(), ProductTypesDetails.Description_ru, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(productTypesDetails.Description_zh))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionZH").ToString(), "None", UpdateProductTypes.Description_zh) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDescriptionZH").ToString(), ProductTypesDetails.Description_zh, UpdateProductTypes.Description_zh) });
                    }
                }


                //Default Way Type
                if (ProductTypesDetails.DefaultWayType.IdDefaultWayType != UpdateProductTypes.IdDefaultWayType)
                {
                    Ways tempDefaultWayType = Ways.FirstOrDefault(x => x.IdWays == UpdateProductTypes.IdDefaultWayType);
                    if (tempDefaultWayType != null)
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultWayType").ToString(), productTypesDetails.DefaultWayType.Name, tempDefaultWayType.Name) });
                }

                //Abbrivation
                if (ProductTypesDetails.Code != UpdateProductTypes.Code)
                {
                    if (string.IsNullOrEmpty(UpdateProductTypes.Code))
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogAbbrivation").ToString(), productTypesDetails.Code, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ProductTypesDetails.Code))
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogAbbrivation").ToString(), "None", UpdateProductTypes.Code) });
                        else
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogAbbrivation").ToString(), productTypesDetails.Code, UpdateProductTypes.Code) });
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddProductTypeLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddProductTypeLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void SelectedItemChangedCommandAction(object obj)
        {
            var List = ClonedProductType.CustomerList.Where(x => x.IsChecked == true).ToList();
            var List1 = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

            RegionsByCustomer customer = (RegionsByCustomer)obj;

            if (List.Any(a => a.IdGroup == customer.IdGroup && a.IdRegion == customer.IdRegion && a.IsChecked == customer.IsChecked) || List.Count() == List1.Count())
            {
                HashSet<string> IdUniques = new HashSet<string>(List.Select(s => s.UniqueId));

                var results = List1.Where(m => !IdUniques.Contains(m.UniqueId));
                if (results.Count() > 0 || List1.Count() != List.Count())
                {
                    IsExportVisible = Visibility.Hidden;
                }
                else
                {
                    IsExportVisible = Visibility.Visible;
                }
            }
            else
                IsExportVisible = Visibility.Hidden;

            IsCellChecked = true;

            if (CustomersMenuList.Count() == List1.Count())
            {
                IsCheckedCustomer = true;
            }
            else
            {
                IsCheckedCustomer = false;
            }

            IsCellChecked = false;
        }

        private void ItemPositionChangedCommandAction(object obj)//[rdixit][GEOS2-2694][01.08.2022]
        {
            ulong pos = 1;
            foreach (ProductTypeImage img in ImagesList)
            {
                img.Position = pos;
                pos++;
            }
        }

        #region GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
        //shubham[skadam] GEOS2-3276 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 1
        private void AddIncludedCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddIncludedCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);

                PLM.Views.AddEditCPLCustomerView addEditCPLCustomerView = new PLM.Views.AddEditCPLCustomerView();
                PLM.ViewModels.AddEditCPLCustomerViewModel addEditCPLCustomerViewModel = new PLM.ViewModels.AddEditCPLCustomerViewModel(obj);
                EventHandler handle = delegate { addEditCPLCustomerView.Close(); };
                addEditCPLCustomerViewModel.RequestClose += handle;
                addEditCPLCustomerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCPLCustomerHeader").ToString();
                addEditCPLCustomerViewModel.IsNew = true;

                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                addEditCPLCustomerViewModel.CPLCustomerList = PCMCustomerList;
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
                                if (!PCMCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    CPLCustomer customer = new CPLCustomer();
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
                                    customer.IsIncluded = 1;
                                    PCMCustomerList.Add(customer);
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
                                            if (!PCMCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().IdRegion && ccl.IdCountry == country.IdCountry))
                                            {
                                                CPLCustomer customer = new CPLCustomer();
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
                                                customer.IsIncluded = 1;
                                                PCMCustomerList.Add(customer);
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    if (!PCMCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == region.IdRegion))
                                    {
                                        CPLCustomer customer = new CPLCustomer();
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
                                        customer.IsIncluded = 1;
                                        PCMCustomerList.Add(customer);
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
                                if (!PCMCustomerList.Any(ccl => ccl.IdPlant == site.IdSite))
                                {
                                    CPLCustomer customer = new CPLCustomer();
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
                                    customer.IsIncluded = 1;
                                    PCMCustomerList.Add(customer);
                                }
                            }

                        }
                        if (addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                        {
                            foreach (Country country in addEditCPLCustomerViewModel.SelectedCountry_Save)
                            {
                                if (country != null)
                                {
                                    if (!PCMCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry))
                                    {
                                        CPLCustomer customer = new CPLCustomer();
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
                                        customer.IsIncluded = 1;
                                        PCMCustomerList.Add(customer);
                                    }
                                }
                            }

                            foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    if (!PCMCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion))
                                    {
                                        CPLCustomer customer = new CPLCustomer();
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
                                        customer.IsIncluded = 1;
                                        PCMCustomerList.Add(customer);
                                    }
                                }
                                else
                                {
                                    CPLCustomer customer = new CPLCustomer();
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
                                    customer.IsIncluded = 1;
                                    PCMCustomerList.Add(customer);
                                }
                            }
                        }

                    }

                    PCMCustomerList = new List<CPLCustomer>(PCMCustomerList.OrderBy(a => a.GroupName));

                    List<CPLCustomer> IsCheckedList = PCMCustomerList.ToList();

                    Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                    Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

                    IsExportVisible = Visibility.Collapsed;
                    IsEnabledCancelButton = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddIncludedCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddIncludedCustomerCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNotIncludedCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNotIncludedCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);

                PLM.Views.AddEditCPLCustomerView addEditCPLCustomerView = new PLM.Views.AddEditCPLCustomerView();
                PLM.ViewModels.AddEditCPLCustomerViewModel addEditCPLCustomerViewModel = new PLM.ViewModels.AddEditCPLCustomerViewModel(obj);
                EventHandler handle = delegate { addEditCPLCustomerView.Close(); };
                addEditCPLCustomerViewModel.RequestClose += handle;
                addEditCPLCustomerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCPLCustomerHeader").ToString();
                addEditCPLCustomerViewModel.IsNew = true;

                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                addEditCPLCustomerViewModel.CPLCustomerList = PCMCustomerList;
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
                                if (!PCMCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdPlant == site.IdSite))
                                {
                                    CPLCustomer customer = new CPLCustomer();
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
                                    customer.IsIncluded = 0;
                                    PCMCustomerList.Add(customer);
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
                                            if (!PCMCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == addEditCPLCustomerViewModel.RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().IdRegion && ccl.IdCountry == country.IdCountry))
                                            {
                                                CPLCustomer customer = new CPLCustomer();
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
                                                customer.IsIncluded = 0;
                                                PCMCustomerList.Add(customer);
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    if (!PCMCustomerList.Any(ccl => ccl.IdGroup == 0 && ccl.IdRegion == region.IdRegion))
                                    {
                                        CPLCustomer customer = new CPLCustomer();
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
                                        customer.IsIncluded = 0;
                                        PCMCustomerList.Add(customer);
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
                                if (!PCMCustomerList.Any(ccl => ccl.IdPlant == site.IdSite))
                                {
                                    CPLCustomer customer = new CPLCustomer();
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
                                    customer.IsIncluded = 0;
                                    PCMCustomerList.Add(customer);
                                }
                            }

                        }
                        if (addEditCPLCustomerViewModel.SelectedPlant_Save.FirstOrDefault() == null)
                        {
                            foreach (Country country in addEditCPLCustomerViewModel.SelectedCountry_Save)
                            {
                                if (country != null)
                                {
                                    if (!PCMCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry))
                                    {
                                        CPLCustomer customer = new CPLCustomer();
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
                                        customer.IsIncluded = 0;
                                        PCMCustomerList.Add(customer);
                                    }
                                }
                            }

                            foreach (Region region in addEditCPLCustomerViewModel.SelectedRegion_Save)
                            {
                                if (region != null)
                                {
                                    if (!PCMCustomerList.Any(ccl => ccl.IdGroup == (uint)addEditCPLCustomerViewModel.SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion))
                                    {
                                        CPLCustomer customer = new CPLCustomer();
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
                                        customer.IsIncluded = 0;
                                        PCMCustomerList.Add(customer);
                                    }
                                }
                                else
                                {
                                    CPLCustomer customer = new CPLCustomer();
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
                                    customer.IsIncluded = 0;
                                    PCMCustomerList.Add(customer);
                                }
                            }
                        }

                    }

                    PCMCustomerList = new List<CPLCustomer>(PCMCustomerList.OrderBy(a => a.GroupName));

                    List<CPLCustomer> IsCheckedList = PCMCustomerList.ToList();

                    Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                    Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                    Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                    Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                    Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                    Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                    Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                    Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

                    IsExportVisible = Visibility.Collapsed;
                    //IsEnabledSaveButton = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddNotIncludedCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNotIncludedCustomerCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteIncludedCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteIncludedCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    DevExpress.Xpf.Grid.EditGridCellData tempRecords = (DevExpress.Xpf.Grid.EditGridCellData)obj;
                    if (PCMCustomerList.Any(a => a.IsChecked == true && a.IsIncluded == 1))
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCPLCustomer"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        //CPLCustomer SelectedCustomer = (CPLCustomer)tempRecords.Row;
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            PCMCustomerList.RemoveAll(r => r.IsChecked == true && r.IsIncluded == 1);
                            PCMCustomerList = new List<CPLCustomer>(PCMCustomerList.OrderBy(a => a.GroupName));
                            //SelectedCustomer = PCMCustomerList.FirstOrDefault();

                            List<CPLCustomer> IsCheckedList = PCMCustomerList.ToList();

                            Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                            Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                            Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                            Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                            Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                            Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                            Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                            Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());

                            IsExportVisible = Visibility.Collapsed;
                        }

                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteIncludedCustomer").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }

                }


                GeosApplication.Instance.Logger.Log("Method DeleteIncludedCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteIncludedCustomerCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteNotIncludedCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteNotIncludedCustomerCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    DevExpress.Xpf.Grid.EditGridCellData tempRecords = (DevExpress.Xpf.Grid.EditGridCellData)obj;
                    if (PCMCustomerList.Any(a => a.IsChecked == true && a.IsIncluded == 0))
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCPLCustomer"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        // CPLCustomer SelectedCustomer = (CPLCustomer)tempRecords.Row;
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            PCMCustomerList.RemoveAll(r => r.IsChecked == true && r.IsIncluded == 0);
                            PCMCustomerList = new List<CPLCustomer>(PCMCustomerList.OrderBy(a => a.GroupName));
                            //SelectedCustomer = PCMCustomerList.FirstOrDefault();
                            List<CPLCustomer> IsCheckedList = PCMCustomerList.ToList();
                            Group = (from x in IsCheckedList select x.GroupName).Distinct().Count();
                            Region = (from x in IsCheckedList select x.RegionName).Distinct().Count();
                            Country = (from x in IsCheckedList select x.Country.Name).Distinct().Count();
                            Plant = (from x in IsCheckedList select x.Plant.Name).Distinct().Count();

                            Groups = String.Join(", ", IsCheckedList.Select(a => a.GroupName).Distinct());
                            Regions = String.Join(", ", IsCheckedList.Select(a => a.RegionName).Distinct());
                            Countries = String.Join(", ", IsCheckedList.Select(a => a.Country.Name).Distinct());
                            Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant.Name).Distinct());
                            IsExportVisible = Visibility.Collapsed;
                            //IsEnabledSaveButton = true;
                        }

                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteNotIncludedCustomer").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }


                }

                GeosApplication.Instance.Logger.Log("Method DeleteNotIncludedCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteNotIncludedCustomerCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedGroupIndexChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedGroupIndexChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (IsRetrive == false)
                {
                    RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));

                    //SelectedRegion = new List<object>();
                    //foreach (Region reg in RegionList)
                    //{
                    //    SelectedRegion.Add(reg);
                    //}
                    //SelectedRegion = new List<object>(SelectedRegion);


                    //SelectedCountry = new List<object>();
                    //foreach (Country cntry in CountryList)
                    //{
                    //    SelectedCountry.Add(cntry);
                    //}
                    //SelectedCountry = new List<object>(SelectedCountry);

                    //SelectedPlant = new List<object>();
                    //foreach (Site plnt in PlantList)
                    //{
                    //    SelectedPlant.Add(plnt);
                    //}
                    //SelectedPlant = new List<object>(SelectedPlant);
                }
                else
                {
                    RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));

                    SelectedRegion = new List<object>();
                    SelectedRegion.Add(RegionList.FirstOrDefault(/*a => a.IdRegion == SelectedCustomer.IdRegion*/));
                    SelectedRegion = new List<object>(SelectedRegion);

                    SelectedCountry = new List<object>();
                    SelectedCountry.Add(CountryList.FirstOrDefault(/*a => a.IdCountry == SelectedCustomer.IdCountry*/));
                    SelectedCountry = new List<object>(SelectedCountry);

                    SelectedPlant = new List<object>();
                    SelectedPlant.Add(PlantList.FirstOrDefault(/*a => a.IdSite == SelectedCustomer.IdPlant*/));
                    SelectedPlant = new List<object>(SelectedPlant);

                    IsRetrive = false;
                }

                GeosApplication.Instance.Logger.Log("Method SelectedGroupIndexChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SelectedGroupIndexChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeRegionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    IsFilterStatus = 1;
                    if (SelectedRegion != null)
                    {
                        //FillCountries();
                        //FillPlants();
                    }
                    else
                    {
                        SelectedCountry = new List<object>();
                        SelectedPlant = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeRegionCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCountryCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCountryCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    IsFilterStatus = 2;
                    if (SelectedCountry != null)
                    {
                        //[adhatkar][22-02-2021] comment because luis said that no need to set group by country
                        //List<object> countries = new List<object>(SelectedCountry);

                        //Country selectedCountry_First = (Country)countries.FirstOrDefault();
                        //SelectedGroup = GroupList.FirstOrDefault(a => a.IdGroup == selectedCountry_First.IdGroup);

                        //SelectedCountry = new List<object>();
                        //foreach (Country cntry in CountryList)
                        //{
                        //    if (countries.Cast<Country>().Any(a => a.IdCountry == cntry.IdCountry))
                        //    {
                        //        SelectedCountry.Add(cntry);
                        //    }
                        //}
                        //SelectedCountry = new List<object>(SelectedCountry);

                        FillRegions();
                    }
                    else
                    {
                        //SelectedRegion = new List<object>();
                        SelectedPlant = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeCountryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeCountryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangePlantCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    IsFilterStatus = 3;
                    if (SelectedPlant != null)
                    {
                        List<object> plants = new List<object>(SelectedPlant);

                        Site selectedPlant_First = (Site)plants.FirstOrDefault();

                        SelectedGroup = GroupList.FirstOrDefault(a => a.IdGroup == selectedPlant_First.IdGroup);

                        SelectedPlant = new List<object>();
                        foreach (Site plnt in PlantList)
                        {
                            if (plants.Cast<Site>().Any(a => a.IdSite == plnt.IdSite))
                            {
                                SelectedPlant.Add(plnt);
                            }
                        }
                        SelectedPlant = new List<object>(SelectedPlant);

                        FillRegions();
                        FillCountries();
                    }
                    else
                    {
                        //SelectedRegion = new List<object>();
                        //SelectedCountry = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillGroups()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroups ...", category: Category.Info, priority: Priority.Low);
                GroupList = new ObservableCollection<Group>(PLMService.GetGroups());
                GroupList.Insert(0, new Group() { GroupName = "", IdGroup = 0 });
                SelectedGroup = GroupList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillGroups() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillRegions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegions ...", category: Category.Info, priority: Priority.Low);

                string CountryNames = "0";
                string SiteNames = "0";
                if (SelectedCountry == null)
                    SelectedCountry = new List<object>();

                if (SelectedPlant == null)
                    SelectedPlant = new List<object>();

                if (IsFilterStatus == 2)
                {
                    foreach (Country country in SelectedCountry)
                    {
                        if (CountryNames == "0")
                            CountryNames = country.Name;
                        else
                            CountryNames = CountryNames + "," + country.Name;
                    }
                }
                else
                {
                    CountryNames = "0";
                }

                if (isFilterStatus == 3)
                {
                    foreach (Site site in SelectedPlant)
                    {
                        if (SiteNames == "0")
                            SiteNames = site.Name;
                        else
                            SiteNames = SiteNames + "," + site.Name;
                    }
                }
                else
                {
                    SiteNames = "0";
                }

                List<Region> SelectedRegionList = new List<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, CountryNames, SiteNames));

                SelectedRegion = new List<object>();
                foreach (Region reg in SelectedRegionList)
                {
                    SelectedRegion.Add(RegionList.FirstOrDefault(a => a.IdRegion == reg.IdRegion));
                }

                SelectedRegion = new List<object>(SelectedRegion);
                GeosApplication.Instance.Logger.Log("Method FillRegions() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCountries()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountries ...", category: Category.Info, priority: Priority.Low);

                string RegionNames = "0";
                string SiteNames = "0";
                if (SelectedRegion == null)
                    SelectedRegion = new List<object>();

                if (SelectedPlant == null)
                    SelectedPlant = new List<object>();

                if (isFilterStatus == 1)
                {
                    foreach (Region Region in SelectedRegion)
                    {
                        if (RegionNames == "0")
                            RegionNames = Region.RegionName;
                        else
                            RegionNames = RegionNames + "," + Region.RegionName;
                    }
                }
                else
                {
                    RegionNames = "0";
                }

                if (isFilterStatus == 3)
                {
                    foreach (Site site in SelectedPlant)
                    {
                        if (SiteNames == "0")
                            SiteNames = site.Name;
                        else
                            SiteNames = SiteNames + "," + site.Name;
                    }
                }
                else
                {
                    SiteNames = "0";
                }

                List<Country> SelectedCountryList = new List<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, RegionNames, SiteNames));

                SelectedCountry = new List<object>();
                foreach (Country cntry in SelectedCountryList)
                {
                    SelectedCountry.Add(CountryList.FirstOrDefault(a => a.IdCountry == cntry.IdCountry));
                }
                SelectedCountry = new List<object>(SelectedCountry);
                GeosApplication.Instance.Logger.Log("Method FillCountries() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlants ...", category: Category.Info, priority: Priority.Low);

                string RegionNames = "0";
                string CountryNames = "0";
                if (SelectedRegion == null)
                    SelectedRegion = new List<object>();

                if (SelectedCountry == null)
                    SelectedCountry = new List<object>();

                if (isFilterStatus == 1)
                {
                    foreach (Region Region in SelectedRegion)
                    {
                        if (RegionNames == "0")
                            RegionNames = Region.RegionName;
                        else
                            RegionNames = RegionNames + "," + Region.RegionName;
                    }
                }
                else
                {
                    RegionNames = "0";
                }

                if (IsFilterStatus == 2)
                {
                    foreach (Country country in SelectedCountry)
                    {
                        if (CountryNames == "0")
                            CountryNames = country.Name;
                        else
                            CountryNames = CountryNames + "," + country.Name;
                    }
                }
                else
                {
                    CountryNames = "0";
                }


                List<Site> SelectedPlantList = new List<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, RegionNames, CountryNames));

                SelectedPlant = new List<object>();
                foreach (Site plnt in SelectedPlantList)
                {
                    SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plnt.IdSite));
                }
                SelectedPlant = new List<object>(SelectedPlant);
                GeosApplication.Instance.Logger.Log("Method FillPlants() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
        private void IncludedCellValueChangingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method IncludedCellValueChangingCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    CPLCustomer SelectedCustomer = (CPLCustomer)obj;
                    if (SelectedIncludedCustomer == null)
                    {
                        SelectedIncludedCustomer = new ObservableCollection<object>();
                    }
                    if (!SelectedIncludedCustomer.Any(a => a == SelectedCustomer))
                    {
                        SelectedIncludedCustomer.Add(SelectedCustomer);
                    }
                    else
                    {
                        SelectedIncludedCustomer.Remove(SelectedCustomer);
                        SelectedIncludedCustomer.Add(SelectedCustomer);

                    }

                }

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method IncludedCellValueChangingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
        private void NotIncludedCellValueChangingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NotIncludedCellValueChangingCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    CPLCustomer SelectedCustomer = (CPLCustomer)obj;
                    if (SelectedNotIncludedCustomer == null)
                    {
                        SelectedNotIncludedCustomer = new ObservableCollection<object>();
                    }
                    if (!SelectedNotIncludedCustomer.Any(a => a == SelectedCustomer))
                    {
                        SelectedNotIncludedCustomer.Add(SelectedCustomer);
                    }
                    else
                    {
                        SelectedNotIncludedCustomer.Remove(SelectedCustomer);
                        SelectedNotIncludedCustomer.Add(SelectedCustomer);

                    }

                }

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method NotIncludedCellValueChangingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        //[29.11.2022][rdixit][GEOS2-2718]
        private void ExpandAndCollapseDetectionsCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsDetectionExpand)
            {
                t.ExpandAllNodes();
                IsDetectionExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsDetectionExpand = true;
            }
        }
        //[29.11.2022][rdixit][GEOS2-2718]
        private void ExpandAndCollapseOptionsCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsOptionExpand)
            {
                t.ExpandAllNodes();
                IsOptionExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsOptionExpand = true;
            }
        }

        //[29.11.2022][sshegaonkar][GEOS2-2718]
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

        //[29.11.2022][sshegaonkar][GEOS2-2718]
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
        //[29.11.2022][rdixit][GEOS2-2718]
        private void ExpandAndCollapseSparePartCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsSparePartExpand)
            {
                t.ExpandAllNodes();
                IsSparePartExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsSparePartExpand = true;
            }
        }

        //[Sudhir.Jangra][GEOS2-4221][05/04/2023]
        private void TreeListViewDropRecordWayAction(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordDetection()...", category: Category.Info, priority: Priority.Low);


                if (e.Data.GetDataPresent(typeof(RecordDragDropData)) && e.IsFromOutside == true && typeof(Ways).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<Ways> records = data.Records.OfType<Ways>().ToList();

                    foreach (var item in records)
                    {
                        WaysMenulist.Where(a => a.IdWays == item.IdWays).ToList().ForEach(b => b.IsCurrentWays = true);
                    }
                }
                else if (e.IsFromOutside == false && typeof(Ways).IsAssignableFrom(e.GetRecordType()))
                {
                    if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                    {
                        var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                        List<Ways> newRecords = data.Records.OfType<Ways>().Select(x => new Ways { Name = x.Name, IdGroup = x.IdGroup, IdDetections = x.IdDetections, OrderNumber = x.OrderNumber }).ToList();

                        Ways temp = newRecords.FirstOrDefault();
                        Ways target_record = (Ways)e.TargetRecord;
                        e.Effects = DragDropEffects.Move;
                        e.Handled = false;
                        if (temp.IdWays > 0)
                        {
                            WaysMenulist.Where(a => a.IdWays == temp.IdWays).ToList().ForEach(b => b.IsCurrentWays = true);
                        }
                        else
                        {
                            foreach (Ways item in WaysMenulist)
                            {
                                WaysMenulist.Where(a => a.IdWays == temp.IdWays).ToList().ForEach(b => b.IsCurrentWays = true);
                            }
                        }
                    }
                }
                IsEnabledCancelButton = true;
                GeosApplication.Instance.Logger.Log("Method TreeListViewDropRecordWay()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordWay() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        //[Sudhir.Jangra][GEOS2-4935][21/11/2023]
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);
                GridControl gridControlView = (GridControl)obj;
                AddEditCommentsView addCommentsView = new AddEditCommentsView();
                AddEditCommentsViewModel addCommentsViewModel = new AddEditCommentsViewModel();
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
                        CommentsList = new ObservableCollection<ProductTypeLogEntry>();

                    //  addCommentsViewModel.SelectedComment.IdCPType = contacts.IdArticleSupplier;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<ProductTypeLogEntry>();

                    AddCommentsList.Add(new ProductTypeLogEntry()
                    {
                        IdUser = addCommentsViewModel.SelectedComment.IdUser,
                        IdCPType = addCommentsViewModel.SelectedComment.IdCPType,
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
                ProductTypeLogEntry commentObject = (ProductTypeLogEntry)parameter;
                if (commentObject.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
                {
                    bool result = false;

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProductTypeComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (CommentsList != null && CommentsList?.Count > 0)
                        {
                            ProductTypeLogEntry Comment = (ProductTypeLogEntry)commentObject;

                            CommentsList.Remove(Comment);

                            if (DeleteCommentsList == null)
                                DeleteCommentsList = new ObservableCollection<ProductTypeLogEntry>();

                            DeleteCommentsList.Add(new ProductTypeLogEntry()
                            {
                                IdUser = Comment.IdUser,
                                IdCPType = Comment.IdCPType,
                                Comments = Comment.Comments,
                                IdLogEntryByCptype = Comment.IdLogEntryByCptype

                            });
                            CommentsList = new ObservableCollection<ProductTypeLogEntry>(CommentsList);

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
            ProductTypeLogEntry logcomments = (ProductTypeLogEntry)obj;
            if (logcomments.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                AddEditCommentsView editCommentsView = new AddEditCommentsView();
                AddEditCommentsViewModel editCommentsViewModel = new AddEditCommentsViewModel();
                EventHandler handle = delegate { editCommentsView.Close(); };
                editCommentsViewModel.RequestClose += handle;
                editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
                editCommentsViewModel.NewItemComment = SelectedComment.Comments;
                editCommentsViewModel.IdLogEntryByCpType = SelectedComment.IdLogEntryByCptype;
                editCommentsView.DataContext = editCommentsViewModel;
                editCommentsView.ShowDialog();

                if (editCommentsViewModel.SelectedComment != null)
                {
                    SelectedComment.Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByCptype == SelectedComment.IdLogEntryByCptype).Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByCptype == SelectedComment.IdLogEntryByCptype).Datetime = GeosApplication.Instance.ServerDateTime;

                    if (UpdatedCommentsList == null)
                        UpdatedCommentsList = new List<ProductTypeLogEntry>();

                    // editCommentsViewModel.SelectedComment.IdCPType = SelectedContacts.;
                    UpdatedCommentsList.Add(new ProductTypeLogEntry()
                    {
                        IdUser = SelectedComment.IdUser,
                        IdCPType = SelectedComment.IdCPType,
                        Comments = SelectedComment.Comments,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdLogEntryByCptype = SelectedComment.IdLogEntryByCptype
                    });
                }
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["EditProductTypeCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

        }

        //[Sudhir.jangra][GEOS2-4935]
        private void SetUserProfileImage(ObservableCollection<ProductTypeLogEntry> CommentsList)
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

        //[Sudhir.Jangra][GEOS2-4935]
        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
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
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        #region GEOS2-5307
        private void OnDragRecordOverNotIncludedModuleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverNotIncludedModuleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PLMModulePrice).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PLMModulePrice> record = data.Records.OfType<PLMModulePrice>().ToList();
                    //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
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
                        List<PLMModulePrice> deleteCPL = new List<PLMModulePrice>();
                        foreach (PLMModulePrice item in record)
                        {
                            if (item.IdStatus == 224 || item.Type == "CPL")
                                continue;
                            if (IncludedPLMModulePriceList.Any(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList))
                            {
                                PLMModulePrice checkCPLDrag = IncludedPLMModulePriceList.FirstOrDefault(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList);
                                if (!record.Any(a => a.Type == "CPL" && a.IdCustomerOrBasePriceList == checkCPLDrag.IdCustomerOrBasePriceList))
                                {
                                    deleteCPL.AddRange(IncludedPLMModulePriceList.Where(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList));
                                    Bplcodes.Add(item.Code);
                                    count++;
                                }
                            }
                        }

                        if (count > 0)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["DeletePCMModulePriceFromBPLCPLValidationMessage"].ToString(), string.Join(",", Bplcodes.Select(a => a.ToString()))), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMModulePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMModulePriceList.Remove(item);
                                    IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(IncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMModulePriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMModulePriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;

                                        }


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
                                    if (!NotIncludedPLMModulePriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMModulePriceList.Add(item);
                                }
                                foreach (PLMModulePrice item in deleteCPL)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMModulePriceList.Remove(item);
                                    IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(IncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMModulePriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMModulePriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;
      
                                        }


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
                                    if (!NotIncludedPLMModulePriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMModulePriceList.Add(item);
                                }
                                NotIncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(NotIncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
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
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePCMModulePriceFromBPLCPLMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMModulePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMModulePriceList.Remove(item);
                                    IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(IncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMModulePriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMModulePriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;
                                        }


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
                                    if (!NotIncludedPLMModulePriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMModulePriceList.Add(item);
                                }
                                NotIncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(NotIncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
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

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverNotIncludedModuleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverNotIncludedModuleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOverIncludedModuleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncludedModuleGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PLMModulePrice).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PLMModulePrice> record = data.Records.OfType<PLMModulePrice>().ToList();
                    //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
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
                        List<PLMModulePrice> addBPL = new List<PLMModulePrice>();

                        foreach (PLMModulePrice item in record)
                        {
                            if (item.IdStatus == 224)
                                continue;
                            if (item.Type == "CPL")
                            {
                                if (!IncludedPLMModulePriceList.Any(a => a.Type == "BPL" && a.IdCustomerOrBasePriceList == item.IdBasePriceList))
                                {
                                    addBPL.Add(NotIncludedPLMModulePriceList.FirstOrDefault(a => a.Type == "BPL" && a.IdCustomerOrBasePriceList == item.IdBasePriceList));
                                    cplcodes.Add(item.Code);
                                    count++;
                                }
                            }
                        }

                        if (count > 0)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["AddPCMModulePriceInBPLCPLValidationMessage"].ToString(), string.Join(",", cplcodes.Select(a => a.ToString()))), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMModulePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMModulePriceList.Remove(item);
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
                                    if (!IncludedPLMModulePriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMModulePriceList.Add(item);

                                }

                                foreach (PLMModulePrice item in addBPL)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMModulePriceList.Remove(item);
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
                                    if (!IncludedPLMModulePriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMModulePriceList.Add(item);
                                }
                                IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(IncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                                if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                                {
                                    if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                    {
                                        IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                        if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                        {
                                            IncludedFirstActiveSellPrice = null;
                                            CurrencySymbol = "";
                                        }
                                        else
                                        {
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        }
                                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                                        if (IncludedPLMModulePriceList[0].SellPrice != null)
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                                        else
                                            IncludedFirstActiveSellPrice = null;

                                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        if (IncludedPLMModulePriceList[0].SellPrice == null)
                                            CurrencySymbol = "";
                                        else
                                            CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;
 
                                    }


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

                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AddPCMModulePriceInBPLCPLMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMModulePrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMModulePriceList.Remove(item);
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
                                    if (!IncludedPLMModulePriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMModulePriceList.Add(item);
                                }
                                IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(IncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                                if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                                {
                                    if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                    {
                                        IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                        if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                        {
                                            IncludedFirstActiveSellPrice = null;
                                            CurrencySymbol = "";
                                        }
                                        else
                                        {
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        }
                                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                                        if (IncludedPLMModulePriceList[0].SellPrice != null)
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                                        else
                                            IncludedFirstActiveSellPrice = null;

                                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                                        if (IncludedPLMModulePriceList[0].SellPrice == null)
                                            CurrencySymbol = "";
                                        else
                                            CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;

                                    }


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

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncludedModuleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverIncludedModuleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][cpatil][GEOS2-3747][10-05-2022]
        private void CalculatesellPriceRulechanged(PLMModulePrice PLMModulePrice)
        {
            if (PLMModulePrice.MaxCost == null)
            {
                PLMModulePrice.MaxCost = 0;
            }
            if (PLMModulePrice.RuleValue == null)
            {
                PLMModulePrice.RuleValue = 0;
            }
            // calculate sellprice (common)
            if (PLMModulePrice.IdRule == 307)
            {
                PLMModulePrice.SellPrice = Convert.ToDouble(PLMModulePrice.MaxCost) + ((Convert.ToDouble(PLMModulePrice.MaxCost) * (Convert.ToDouble(PLMModulePrice.RuleValue) / 100)));
            }
            else if (PLMModulePrice.IdRule == 308)
            {
                if (PLMModulePrice.RuleValue == null || PLMModulePrice.RuleValue == null)
                {
                    PLMModulePrice.RuleValue = null;
                    PLMModulePrice.SellPrice = null;
                }
                else if (PLMModulePrice.RuleValue != null && Convert.ToDouble(PLMModulePrice.RuleValue) <= 0)
                {
                    PLMModulePrice.RuleValue = null;
                    PLMModulePrice.SellPrice = null;
                }
                else
                {
                    PLMModulePrice.SellPrice = PLMModulePrice.RuleValue;
                }
            }
            else if (PLMModulePrice.IdRule == 1518)
            {
                PLMModulePrice.RuleValue = 0;
                PLMModulePrice.SellPrice = 0;
            }
            else if (PLMModulePrice.IdRule == 309)
            {
                PLMModulePrice.SellPrice = Convert.ToDouble(PLMModulePrice.MaxCost) + (Convert.ToDouble(PLMModulePrice.RuleValue));
            }

            double SellPriceValue = 0;
            double MaxCost = 0;

            if (PLMModulePrice.SellPrice != null)
                SellPriceValue = Convert.ToDouble(PLMModulePrice.SellPrice);

            if (PLMModulePrice.MaxCost != null)
                MaxCost = Convert.ToDouble(PLMModulePrice.MaxCost);

            PLMModulePrice.Profit = Convert.ToDouble(CalculateBasePriceProfitValue(MaxCost, SellPriceValue));
            PLMModulePrice.CostMargin = Convert.ToDouble(CalculateBasePriceCostMarginValue(MaxCost, SellPriceValue));

            PLMModulePrice.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMModulePrice.IdRule);
        }
        //shubham[skadam]GEOS2-3794 Improvement in the PRICES section -  Do the exchange currency automatically
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
                            PLMModulePrice PLMModulePrice = ((DevExpress.Xpf.Grid.EditGridCellData)OriginalSource.DataContext).Row as PLMModulePrice;
                            if (PLMModulePrice.Type == "BPL")
                            {
                                List<GeosAppSetting> GeosAppSettingRuleValue = WorkbenchService.GetSelectedGeosAppSettings("89");
                                if (PLMModulePrice.Currency != null)
                                    if (PLMModulePrice.Currency.Name.Equals(GeosAppSettingRuleValue[0].DefaultValue))
                                    {
                                        if (IncludedPLMModulePriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IsEnabledPermission == true).Count() > 1)
                                        {
                                            IsBPLMessageShow = true;
                                            MessageBoxResult MessageBoxResultForCurrencyConversion = CustomMessageBox.Show(string.Format(Application.Current.Resources["PCMRuleValueCurrencyConversionMessage"].ToString(), "Base Price List"), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                                            if (MessageBoxResultForCurrencyConversion == MessageBoxResult.Yes)
                                            {
                                                IsBPLCalculateRuleValue = true;
                                                if (PLMCommon.Instance.CurrencyConversionList == null || PLMCommon.Instance.CurrencyConversionList.Count == 0)
                                                {
                                                    PLMCommon.Instance.CurrencyConversionList = new List<CurrencyConversion>(PLMService.GetCurrencyConversionsDetailsByLatestDate());
                                                }


                                                CalculatesellPriceRulechanged(PLMModulePrice);
                                                foreach (PLMModulePrice item in IncludedPLMModulePriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IdBasePriceList != PLMModulePrice.IdBasePriceList && i.IsEnabledPermission == true && i.IsChecked == true))
                                                {
                                                    if (PLMCommon.Instance.CurrencyConversionList.Any(i => i.Idcurrencyto == item.IdCurrency && i.Idcurrencyfrom == PLMModulePrice.IdCurrency))
                                                    {
                                                        item.RuleValue = PLMModulePrice.RuleValue * (PLMCommon.Instance.CurrencyConversionList.Where(i => i.Idcurrencyto == item.IdCurrency && i.Idcurrencyfrom == PLMModulePrice.IdCurrency).FirstOrDefault().ExchangeRate);
                                                        CalculatesellPriceRulechanged(item);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                IsBPLCalculateRuleValue = false;
                                                CalculatesellPriceRulechanged(PLMModulePrice);
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
        //[001][cpatil][GEOS2-3747][10-05-2022]
        private void RuleChangedCommandAction(object obj)
        {
            try
            {
                if (obj == null)
                    return;

                PLMModulePrice PLMModulePrice = ((DevExpress.Xpf.Grid.RowEventArgs)obj).Row as PLMModulePrice;
                IsEnabledCancelButton = true;
                if (PLMModulePrice.MaxCost == null)
                {
                    PLMModulePrice.MaxCost = 0;
                }
                if (PLMModulePrice.RuleValue == null)
                {
                    PLMModulePrice.RuleValue = 0;
                }
                // calculate sellprice (common)
                if (PLMModulePrice.IdRule == 307)
                {
                    PLMModulePrice.SellPrice = Convert.ToDouble(PLMModulePrice.MaxCost) + ((Convert.ToDouble(PLMModulePrice.MaxCost) * (Convert.ToDouble(PLMModulePrice.RuleValue) / 100)));
                }
                else if (PLMModulePrice.IdRule == 308)
                {
                    if (PLMModulePrice.RuleValue == null || PLMModulePrice.RuleValue == null)
                    {
                        PLMModulePrice.RuleValue = null;
                        PLMModulePrice.SellPrice = null;
                    }
                    else if (PLMModulePrice.RuleValue != null && Convert.ToDouble(PLMModulePrice.RuleValue) <= 0)
                    {
                        PLMModulePrice.RuleValue = null;
                        PLMModulePrice.SellPrice = null;
                    }
                    else
                    {
                        PLMModulePrice.SellPrice = PLMModulePrice.RuleValue;
                    }
                }
                else if (PLMModulePrice.IdRule == 1518)
                {
                    PLMModulePrice.RuleValue = 0;
                    PLMModulePrice.SellPrice = 0;
                }
                else if (PLMModulePrice.IdRule == 309)
                {
                    PLMModulePrice.SellPrice = Convert.ToDouble(PLMModulePrice.MaxCost) + (Convert.ToDouble(PLMModulePrice.RuleValue));
                }

                double SellPriceValue = 0;
                double MaxCost = 0;

                if (PLMModulePrice.SellPrice != null)
                    SellPriceValue = Convert.ToDouble(PLMModulePrice.SellPrice);

                if (PLMModulePrice.MaxCost != null)
                    MaxCost = Convert.ToDouble(PLMModulePrice.MaxCost);

                PLMModulePrice.Profit = Convert.ToDouble(CalculateBasePriceProfitValue(MaxCost, SellPriceValue));
                PLMModulePrice.CostMargin = Convert.ToDouble(CalculateBasePriceCostMarginValue(MaxCost, SellPriceValue));

                PLMModulePrice.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMModulePrice.IdRule);

                if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }
                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMModulePriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMModulePriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;
                    }
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

                    foreach (PLMModulePrice item in IncludedPLMModulePriceList)
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

                            foreach (var dataObject in IncludedPLMModulePriceList)
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
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == IncludedPLMModulePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = IncludedPLMModulePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", IncludedPLMModulePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
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

                    foreach (PLMModulePrice item in NotIncludedPLMModulePriceList)
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

                            foreach (var dataObject in NotIncludedPLMModulePriceList)
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
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == NotIncludedPLMModulePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = NotIncludedPLMModulePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", NotIncludedPLMModulePriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
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
        private void InitTheIncludedAndNotIncludedPriceList(ProductTypes temp)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method InitTheIncludedAndNotIncludedPriceList ...", category: Category.Info, priority: Priority.Low);

                IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(temp.IncludedPLMModuleList);
                NotIncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(temp.NotIncludedPLMModuleList);

                if (IncludedPLMModulePriceList != null)
                {
                    foreach (var included in IncludedPLMModulePriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(included.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        included.ToList().Where(inc => inc.Currency.Name == included.Key).ToList().ForEach(inc => inc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                if (NotIncludedPLMModulePriceList != null)
                {
                    foreach (var notIncluded in NotIncludedPLMModulePriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(notIncluded.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        notIncluded.ToList().Where(notinc => notinc.Currency.Name == notIncluded.Key).ToList().ForEach(notinc => notinc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                SelectedIncludedPLMModulePrice = IncludedPLMModulePriceList.FirstOrDefault();
                SelectedNotIncludedPLMModulePrice = NotIncludedPLMModulePriceList.FirstOrDefault();
                IncludedPLMModulePriceList = new ObservableCollection<PLMModulePrice>(IncludedPLMModulePriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                if (IncludedPLMModulePriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMModulePriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMModulePriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMModulePriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }

                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMModulePriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMModulePriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMModulePriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMModulePriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMModulePriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMModulePriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMModulePriceList[0].Currency.Symbol;
                    }
                }
                else
                {
                    IncludedFirstActiveName = "";
                    IncludedFirstActiveCurrencyIconImage = null;
                    IncludedFirstActiveSellPrice = null;
                    IncludedActiveCount = 0;
                    CurrencySymbol = "";
                }
                GeosApplication.Instance.Logger.Log("Method InitTheIncludedAndNotIncludedPriceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method InitTheIncludedAndNotIncludedPriceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       

        private void UpdateTheIncludedAndNotIncludedPriceList()
        {
            //[GEOS2-3199]
            try
            {
                GeosApplication.Instance.Logger.Log(" Method UpdateTheIncludedAndNotIncludedPriceList ...", category: Category.Info, priority: Priority.Low);
                UpdatedItem = UpdateProductTypes;
                /// PLM Module Prices
                UpdatedItem.ModifiedPLMModuleList = new List<PLMModulePrice>();
                UpdatedItem.BasePriceLogEntryList = new List<BasePriceLogEntry>();
                UpdatedItem.CustomerPriceLogEntryList = new List<CustomerPriceLogEntry>();

                // Delete PLM Module Prices
                if (NotIncludedPLMModulePriceList != null)
                {
                    foreach (PLMModulePrice item in NotIncludedPLMModulePriceList)
                    {
                        if (!ClonedProductTypes.NotIncludedPLMModuleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                        {
                            PLMModulePrice PLMModulePrice = (PLMModulePrice)item.Clone();
                            PLMModulePrice.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdatedItem.ModifiedPLMModuleList.Add(PLMModulePrice);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogDeleteInPCM").ToString(), item.Code, item.Type) });
                            if (item.Type == "BPL")
                                UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList,
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogDelete").ToString(), item.Code) });
                            else
                                UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList,
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogDelete").ToString(), item.Code) });
                        }
                    }
                }

                //Added PLM Module Prices
                if (IncludedPLMModulePriceList != null)
                {
                    foreach (PLMModulePrice item in IncludedPLMModulePriceList)
                    {
                        if (!ClonedProductTypes.IncludedPLMModuleList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                        {
                            PLMModulePrice PLMModulePrice = (PLMModulePrice)item.Clone();
                            PLMModulePrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdatedItem.ModifiedPLMModuleList.Add(PLMModulePrice);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogAddInPCM").ToString(), item.Code, item.Type) });
                            if (item.Type == "BPL")
                                UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList,
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogAdd").ToString(), item.Code) });
                            else
                                UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList,
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogAdd").ToString(), item.Code) });

                        }
                    }
                }

                //Updated PLM Module Prices
                foreach (PLMModulePrice originalModule in ClonedProductTypes.IncludedPLMModuleList)
                {
                    if (IncludedPLMModulePriceList != null && IncludedPLMModulePriceList.Any(x => x.IdCustomerOrBasePriceList == originalModule.IdCustomerOrBasePriceList && x.Type == originalModule.Type))
                    {
                        PLMModulePrice PLMModulePriceUpdated = IncludedPLMModulePriceList.FirstOrDefault(x => x.IdCustomerOrBasePriceList == originalModule.IdCustomerOrBasePriceList && x.Type == originalModule.Type);
                        if ((PLMModulePriceUpdated.RuleValue != originalModule.RuleValue) ||
                            (PLMModulePriceUpdated.IdRule != originalModule.IdRule) ||
                            (PLMModulePriceUpdated.MaxCost != originalModule.MaxCost)
                            )
                        {
                            PLMModulePrice PLMModulePrice = (PLMModulePrice)PLMModulePriceUpdated.Clone();
                            PLMModulePrice.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdatedItem.ModifiedPLMModuleList.Add(PLMModulePrice);


                            if (PLMModulePriceUpdated.IdRule != originalModule.IdRule)
                            {
                                string newRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMModulePriceUpdated.IdRule).Value;
                                string oldRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == originalModule.IdRule).Value;
                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridRuleLogicInPCM").ToString(),
                                    oldRuleLogic, newRuleLogic, PLMModulePriceUpdated.Type, PLMModulePriceUpdated.Code) });
                                if (PLMModulePriceUpdated.Type == "BPL")
                                    UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMModulePriceUpdated.IdCustomerOrBasePriceList,
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridRuleLogic").ToString(),
                                        oldRuleLogic, newRuleLogic, PLMModulePriceUpdated.Code) });
                                else
                                    UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMModulePriceUpdated.IdCustomerOrBasePriceList,
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridRuleLogic").ToString(),
                                        oldRuleLogic, newRuleLogic, PLMModulePriceUpdated.Code) });
                            }

                            if (PLMModulePriceUpdated.RuleValue != originalModule.RuleValue)
                            {
                                string oldValue = "";
                                string newValue = "";
                                if (PLMModulePriceUpdated.RuleValue == null)
                                    newValue = "None";
                                else
                                    newValue = PLMModulePriceUpdated.RuleValue.Value.ToString("0." + new string('#', 339));

                                //Reference
                                //https://stackoverflow.com/questions/1546113/double-to-string-conversion-without-scientific-notation

                                if (originalModule.RuleValue == null)
                                    oldValue = "None";
                                else
                                    oldValue = originalModule.RuleValue.Value.ToString("0." + new string('#', 339));

                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridRuleValueInPCM").ToString(),
                                    oldValue, newValue, PLMModulePriceUpdated.Type, PLMModulePriceUpdated.Code) });
                                if (PLMModulePriceUpdated.Type == "BPL")
                                    UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMModulePriceUpdated.IdCustomerOrBasePriceList,
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridRuleValue").ToString(),
                                        oldValue, newValue, PLMModulePriceUpdated.Code) });
                                else
                                    UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMModulePriceUpdated.IdCustomerOrBasePriceList,
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridRuleValue").ToString(),
                                        oldValue, newValue, PLMModulePriceUpdated.Code) });
                            }

                            if (PLMModulePriceUpdated.MaxCost != originalModule.MaxCost)
                            {
                                string oldValue = "";
                                string newValue = "";
                                if (PLMModulePriceUpdated.MaxCost == null)
                                    newValue = "None";
                                else
                                    newValue = PLMModulePriceUpdated.MaxCost.Value.ToString("0." + new string('#', 339));

                                if (originalModule.MaxCost == null)
                                    oldValue = "None";
                                else
                                    oldValue = originalModule.MaxCost.Value.ToString("0." + new string('#', 339));

                                ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridMaxCostInPCM").ToString(),
                                    oldValue, newValue, PLMModulePriceUpdated.Type, PLMModulePriceUpdated.Code) });
                                if (PLMModulePriceUpdated.Type == "BPL")
                                    UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMModulePriceUpdated.IdCustomerOrBasePriceList,
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridMaxCost").ToString(),
                                        oldValue, newValue, PLMModulePriceUpdated.Code) });
                                else
                                    UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMModulePriceUpdated.IdCustomerOrBasePriceList,
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("PCMModulePriceChangeLogModuleGridMaxCost").ToString(),
                                        oldValue, newValue, PLMModulePriceUpdated.Code) });
                            }
                        }
                    }
                }

                UpdatedItem.ModifiedPLMModuleList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);
                GeosApplication.Instance.Logger.Log("Method UpdateTheIncludedAndNotIncludedPriceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateTheIncludedAndNotIncludedPriceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ModuleGridControlLoadedAction(object obj)
        {
            try
            {
                //if (IsSwitch == true)
                //    return;

                GeosApplication.Instance.Logger.Log("Method ModuleGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                {
                    int visibleFalseColumn = 0;
                    GridControl gridControl = obj as GridControl;
                    TableView tableView = (TableView)gridControl.View;

                    gridControl.BeginInit();

                    if (File.Exists(PCMModulePriceListIncludedGridSetting))
                    {
                        gridControl.RestoreLayoutFromXml(PCMModulePriceListIncludedGridSetting);
                    }
                    //This code for save grid layout.
                    gridControl.SaveLayoutToXml(PCMModulePriceListIncludedGridSetting);

                    if (IsFirstTimeLoad == false)
                    {
                        if (visibleFalseColumn > 0)
                        {
                            IsModuleColumnChooserVisible = true;
                        }
                        else
                        {
                            IsModuleColumnChooserVisible = false;
                        }
                    }
                    IsFirstTimeLoad = false;
                    gridControl.EndInit();
                    tableView.SearchString = null;
                    tableView.ShowGroupPanel = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ModuleGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ModuleGridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        private void ModuleGridControlUnloadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ModuleGridControlUnloadedAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                if (gridControl.Columns.Count() > 0)
                {
                    TableView tableView = (TableView)gridControl.View;
                    tableView.SearchString = string.Empty;
                    if (gridControl.GroupCount > 0)
                        gridControl.ClearGrouping();
                    gridControl.ClearSorting();
                    gridControl.FilterString = null;
                    gridControl.SaveLayoutToXml(PCMModulePriceListIncludedGridSetting);
                }
                else
                {
                    IsFirstTimeLoad = true;
                    //IsSwitch = true;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ModuleGridControlUnloadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ModuleGridControlUnloadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void UpdateTheIncludedAndNotIncludedPriceList(ProductTypes UpdatedItem)
        {
            //[GEOS2-3199]
            try
            {
                GeosApplication.Instance.Logger.Log(" Method UpdateTheIncludedAndNotIncludedPriceList ...", category: Category.Info, priority: Priority.Low);
                /// PLM Module Prices
                UpdatedItem.ModifiedPLMModuleList = new List<PLMModulePrice>();
                //Added PLM Module Prices
                if (IncludedPLMModulePriceList != null)
                {
                    foreach (PLMModulePrice item in IncludedPLMModulePriceList)
                    {
                        PLMModulePrice PLMModulePrice = (PLMModulePrice)item.Clone();
                        PLMModulePrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdatedItem.ModifiedPLMModuleList.Add(PLMModulePrice);
                    }
                }


                UpdatedItem.ModifiedPLMModuleList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);
                GeosApplication.Instance.Logger.Log("Method UpdateTheIncludedAndNotIncludedPriceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateTheIncludedAndNotIncludedPriceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                                me[BindableBase.GetPropertyName(() => Name)] +
                                me[BindableBase.GetPropertyName(() => InformationError)] +
                                me[BindableBase.GetPropertyName(() => CompatibilityError)] +
                                me[BindableBase.GetPropertyName(() => SelectedTemplate)] +
                                me[BindableBase.GetPropertyName(() => Abbrivation)];        //[GEOS2-3759][gulab lakade][02 01 2023]


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
                string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);
                string selectedTemplateError = BindableBase.GetPropertyName(() => SelectedTemplate);
                string abbrivation = BindableBase.GetPropertyName(() => Abbrivation);   //[GEOS2-3759][gulab lakade][02 01 2023]

                if (columnName == name)
                {
                    return AddEditModuleValidation.GetErrorMessage(name, Name);
                }

                if (columnName == headerInformtionError)
                {
                    return AddEditModuleValidation.GetErrorMessage(headerInformtionError, InformationError);
                }

                string gridCompatibilityError = BindableBase.GetPropertyName(() => CompatibilityError);

                if (columnName == gridCompatibilityError)
                    return AddEditModuleValidation.GetErrorMessage(gridCompatibilityError, CompatibilityError);

                if (columnName == selectedTemplateError)
                {
                    return AddEditModuleValidation.GetErrorMessage(name, selectedTemplateError);
                }
                //#region [GEOS2-3759][gulab lakade][02 01 2023]
                //if (columnName == abbrivation)
                //{
                //    return AddEditModuleValidation.GetErrorMessage(abbrivation, Abbrivation);
                //}
                //#endregion

                return null;
            }
        }
        /// <summary>
        /// If any feild is of Information has error set isInformationError = true;
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>

        #endregion

    }
}
