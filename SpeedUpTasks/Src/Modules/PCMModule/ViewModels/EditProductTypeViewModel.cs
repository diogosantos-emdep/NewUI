using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.WindowsUI;
using WindowsUIDemo;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.IO;
using Microsoft.Win32;
using System.Windows.Data;
using DevExpress.Xpf.LayoutControl;
using System.Drawing;
using System.Drawing.Imaging;
using Emdep.Geos.Modules.PCM.Common_Classes;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class EditProductTypeViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service

        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

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

        private ObservableCollection<Template> templatesMenuList;
        private Template selectedTemplate;

        private ObservableCollection<Options> optionsMenulist;
        private ObservableCollection<Options> options;
        private ObservableCollection<Options> selectedOptionsType;
        private Options selectedOption;

        private ObservableCollection<Ways> waysMenulist;
        private ObservableCollection<Ways> ways;
        private List<Object> selectedWaysType;
        private Ways selectedWay;

        private ObservableCollection<Detections> detectionsMenulist;
        private ObservableCollection<Detections> detections;
        private List<Object> selectedDetectionsType;
        private Detections selectedDetection;

        private ObservableCollection<SpareParts> sparePartsMenulist;
        private ObservableCollection<SpareParts> spareParts;
        private List<Object> selectedSparePartsType;
        private SpareParts selectedSparePart;

        private ObservableCollection<ConnectorFamilies> familyMenulist;
        private ObservableCollection<ConnectorFamilies> families;
        private ConnectorFamilies selectedFamily;

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

        #endregion

        #region Properties

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
        public ICommand EditWayCommand { get; set; }
        public ICommand EditDetectionsCommand { get; set; }
        public ICommand EditSparePartsCommand { get; set; }


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

                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
                ChangeProductTypeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeProductTypeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                UncheckedCopyDescriptionCommand = new DelegateCommand<object>(UncheckedCopyDescription);

                AddNewOptionCommand = new DelegateCommand<object>(AddNewOption);
                AddNewWayCommand = new DelegateCommand<object>(AddNewWay);
                AddNewDetectionCommand = new DelegateCommand<object>(AddNewDetection);
                AddNewSparePartCommand = new DelegateCommand<object>(AddNewSparePart);

                EditOptionCommand = new DelegateCommand<object>(EditOption);
                EditWayCommand = new DelegateCommand<object>(EditWay);
                EditDetectionsCommand = new DelegateCommand<object>(EditDetections);
                EditSparePartsCommand = new DelegateCommand<object>(EditSpareParts);

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

                AddProductTypeItemAcceptButtonCommand = new RelayCommand(new Action<object>(EditSaveModule));
                AddProductTypeItemCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));

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
                }
                else if (e.IsFromOutside == false && typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                    {
                        var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                        List<Options> newRecords = data.Records.OfType<Options>().Select(x => new Options { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdOptions = x.IdOptions, OrderNumber = x.OrderNumber }).ToList();

                        Options temp = newRecords.FirstOrDefault();

                        Options target_record = (Options)e.TargetRecord;

                        if (temp.IdGroup == target_record.IdGroup && temp.Parent == target_record.Parent)
                        {
                            if (temp.IdGroup == null && temp.Parent == null)
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = true;
                            }
                            else
                            {
                                e.Effects = DragDropEffects.Move;
                                e.Handled = false;
                            }
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
                        }
                    }
                }

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

                e.Handled = true;
                IsEnabled = true;

                if (Ways != null)
                    Ways = new ObservableCollection<Ways>(Ways.GroupBy(opt => opt.IdWays).Select(g => g.First()));

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

                if ((e.IsFromOutside) && e.TargetRecord != null && typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
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
                }
                else if (e.IsFromOutside == false && typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                    {
                        var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                        List<Detections> newRecords = data.Records.OfType<Detections>().Select(x => new Detections { Key = x.Key, Name = x.Name, Parent = x.Parent, IdGroup = x.IdGroup, IdDetections = x.IdDetections, OrderNumber = x.OrderNumber }).ToList();

                        Detections temp = newRecords.FirstOrDefault();

                        Detections target_record = (Detections)e.TargetRecord;
                        if (temp.IdGroup == target_record.IdGroup && temp.Parent == target_record.Parent)
                        {
                            if (temp.IdGroup == null && temp.Parent == null)
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = true;
                            }
                            else
                            {
                                e.Effects = DragDropEffects.Move;
                                e.Handled = false;
                            }
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
                        }
                    }
                }
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


                if ((e.IsFromOutside) && typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSparePartsGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSparePartsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropSpareParts(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropSpareParts()...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;

                if (SpareParts != null)
                    SpareParts = new ObservableCollection<SpareParts>(SpareParts.GroupBy(opt => opt.IdSpareParts).Select(g => g.First()));

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
                }

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

                e.Handled = true;

                if (Families != null)
                    Families = new ObservableCollection<ConnectorFamilies>(Families.GroupBy(opt => opt.IdFamily).Select(g => g.First()));

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

        #endregion

        #region Method

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

                MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

                FillTemplateList();
                AddOptionsMenu();
                AddWaysMenu();
                AddDetectionsMenu();
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

                ProductTypesDetails = PCMService.GetProductTypeByIdCpType(ProductTypeItem.IdCPType, ProductTypeItem.IdTemplate);

                ClonedProductType = (ProductTypes)ProductTypesDetails.Clone();

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
                Name_en = ProductTypesDetails.Name;
                Name_es = ProductTypesDetails.Name_es;
                Name_fr = ProductTypesDetails.Name_fr;
                Name_pt = ProductTypesDetails.Name_pt;
                Name_ro = ProductTypesDetails.Name_ro;
                Name_ru = ProductTypesDetails.Name_ru;
                Name_zh = ProductTypesDetails.Name_zh;

                Description = ProductTypesDetails.Description;
                Description_en = ProductTypesDetails.Description;
                Description_es = ProductTypesDetails.Description_es;
                Description_fr = ProductTypesDetails.Description_fr;
                Description_pt = ProductTypesDetails.Description_pt;
                Description_ro = ProductTypesDetails.Description_ro;
                Description_ru = ProductTypesDetails.Description_ru;
                Description_zh = ProductTypesDetails.Description_zh;

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

                SelectedDefaultWayType = ProductTypesDetails.WayList.FirstOrDefault(x => x.IdWays == ProductTypesDetails.IdDefaultWayType);

                if (SelectedDefaultWayType == null && ProductTypesDetails.WayList.Count() == 0)
                    IsEnabled = false;
                else
                    IsEnabled = true;

                if (ProductTypesDetails.IdStatus > 0)
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == ProductTypesDetails.IdStatus);
                }
                else
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 225);
                }
                
                SelectedTemplate = TemplatesMenuList.FirstOrDefault(x => x.IdTemplate == ProductTypesDetails.IdTemplate);


                Families = new ObservableCollection<ConnectorFamilies>(ProductTypesDetails.FamilyList);

                Ways = new ObservableCollection<Ways>(ProductTypesDetails.WayList);

                Detections = new ObservableCollection<Detections>(ProductTypesDetails.DetectionList_Group);

                Options = new ObservableCollection<Options>(ProductTypesDetails.OptionList_Group);

                SpareParts = new ObservableCollection<SpareParts>(ProductTypesDetails.SparePartList);

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

                FourRecordsProductTypeFilesList = new ObservableCollection<ProductTypeAttachedDoc>(ProductTypeFilesList.OrderBy(x => x.IdCPType).Take(4).ToList());
                FourRecordsProductTypeLinksList = new ObservableCollection<ProductTypeAttachedLink>(ProductTypeLinksList.OrderBy(x => x.IdCPType).Take(4).ToList());
                FourRecordsProductTypeImagesList = new ObservableCollection<ProductTypeImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

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

        private void FillTemplateList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTemplateList()...", category: Category.Info, priority: Priority.Low);

                TemplatesMenuList = new ObservableCollection<Template>(PCMService.GetAllTemplates());

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

        public void AddOptionsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddOptionsMenu()...", category: Category.Info, priority: Priority.Low);

                Options = new ObservableCollection<Options>();
                OptionsMenulist = new ObservableCollection<Options>(PCMService.GetAllOptionsWithGroups());

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

        public void AddWaysMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddWaysMenu()...", category: Category.Info, priority: Priority.Low);

                Ways = new ObservableCollection<Ways>();
                WaysMenulist = new ObservableCollection<Ways>(PCMService.GetAllWayList());

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

        public void AddDetectionsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDetectionsMenu()...", category: Category.Info, priority: Priority.Low);

                Detections = new ObservableCollection<Detections>();
                DetectionsMenulist = new ObservableCollection<Detections>(PCMService.GetAllDetectionsWithGroups());

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

        public void AddSparePartsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSparePartsMenu()...", category: Category.Info, priority: Priority.Low);

                SpareParts = new ObservableCollection<SpareParts>();
                SparePartsMenulist = new ObservableCollection<SpareParts>(PCMService.GetAllSparePartList());

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

        public void AddFamilyMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFamilyMenu()...", category: Category.Info, priority: Priority.Low);

                Families = new ObservableCollection<ConnectorFamilies>();
                FamilyMenulist = new ObservableCollection<ConnectorFamilies>(PCMService.GetAllFamilies());

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

        public void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PCMService.GetAllLanguages());
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

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = PCMService.GetLookupValues(45);
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

        private void FillCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCode()...", category: Category.Info, priority: Priority.Low);

                Reference = PCMService.GetLatestProuductTypeReference();

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

        private void FillDefaultWayTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDefaultWayTypeList()...", category: Category.Info, priority: Priority.Low);

                DefaultWayTypeList = new ObservableCollection<DefaultWayType>(PCMService.GetAllDefaultWayTypeList());
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

        private void FillCustomersList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomersList()...", category: Category.Info, priority: Priority.Low);

                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(PCMService.GetCustomersWithRegions());

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

        private void FillModuleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillModuleMenuList()...", category: Category.Info, priority: Priority.Low);

                ModuleMenulist = new ObservableCollection<ProductTypesTemplate>(PCMService.GetProductTypesWithTemplate());

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

        private void FillArticleMenuList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleMenuList()...", category: Category.Info, priority: Priority.Low);

                ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticlesWithCategory_V2060());
                SelectedArticle = ArticleMenuList.FirstOrDefault();

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

        private void FillReferenceView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReferenceView()...", category: Category.Info, priority: Priority.Low);

                ArticleMenuList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticlesWithCategoryForReference_V2060());
                SelectedArticle = ArticleMenuList.FirstOrDefault();
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

        private void FillRelationShipList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillRelationShipList..."), category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempRelationShipList = PCMService.GetLookupValues(50);
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
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    SparePartsMenulist.Add(tempSparePart);
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
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    SparePartsMenulist.Add(tempSparePart);
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
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    SparePartsMenulist.Add(tempSparePart);
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
                addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Collapsed;
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
                    tempSparePart.Name = addEditOptionsDetectionsViewModel.NewSparePart.Name;
                    tempSparePart.Code = addEditOptionsDetectionsViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addEditOptionsDetectionsViewModel.NewSparePart.IdTestType;
                    SparePartsMenulist.Add(tempSparePart);
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
                        SelectedOption.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                        SelectedOption.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                        SelectedOption.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                        SelectedOption.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                        SelectedOption.Key = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections.ToString();
                        SelectedOption.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                        SelectedOption.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                        SelectedOption.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                        SelectedOption.Parent = "Group_" + SelectedOption.IdGroup;
                        OptionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditOption()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

                if (SelectedWaysType == null)
                    return;

                Ways selectedWay = SelectedWaysType.Cast<Ways>().ToList().LastOrDefault();

                addOptionWayDetectionSparePartViewModel.EditInitWays(selectedWay);
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsWaySave)
                {
                    SelectedWaysType.Cast<Ways>().ToList().LastOrDefault().IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                    SelectedWaysType.Cast<Ways>().ToList().LastOrDefault().Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                    SelectedWaysType.Cast<Ways>().ToList().LastOrDefault().Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
                }
                GeosApplication.Instance.Logger.Log("Method EditWay()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                        SelectedDetection.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                        SelectedDetection.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                        SelectedDetection.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                        SelectedDetection.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                        SelectedDetection.Key = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections.ToString();
                        SelectedDetection.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                        SelectedDetection.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                        SelectedDetection.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                        SelectedDetection.Parent = "Group_" + SelectedDetection.IdGroup;
                        DetectionsMenulist.OrderBy(x => x.OrderNumber).ThenBy(x => x.Name);
                    }
                    GeosApplication.Instance.Logger.Log("Method EditDetections()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditSpareParts(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSpareParts()...", category: Category.Info, priority: Priority.Low);

                EditDetectionView addOptionWayDetectionSparePartView = new EditDetectionView();
                EditDetectionViewModel addOptionWayDetectionSparePartViewModel = new EditDetectionViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = false;
                addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Collapsed;
                addOptionWayDetectionSparePartViewModel.Init(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());

                if (SelectedSparePartsType == null)
                    return;

                SpareParts selectedSpareParts = SelectedSparePartsType.Cast<SpareParts>().ToList().LastOrDefault();

                addOptionWayDetectionSparePartViewModel.EditInitSparePart(selectedSpareParts);
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsSparepartSave)
                {
                    SelectedSparePartsType.Cast<SpareParts>().ToList().LastOrDefault().IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                    SelectedSparePartsType.Cast<SpareParts>().ToList().LastOrDefault().Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                    SelectedSparePartsType.Cast<SpareParts>().ToList().LastOrDefault().Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
                }
                GeosApplication.Instance.Logger.Log("Method EditSpareParts()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditSpareParts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                addFileInProductTypeView.DataContext = addFileInProductTypeViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addFileInProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addFileInProductTypeView.ShowDialog();

                if (addFileInProductTypeViewModel.IsSave)
                {
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
                    SelectedProductTypeFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
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
                    
                    var List2 = ClonedProductType.CustomerList.Where(x => x.IsChecked == true).ToList();

                    ChangeLogTableView.DataControl.ItemsSource = List2;
                    ChangeLogTableView.ShowFixedTotalSummary = false;
                    ChangeLogTableView.ExportToXlsx(ResultFileName);
                    ChangeLogTableView.DataControl.ItemsSource = CustomersMenuList;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
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
                GeosApplication.Instance.Logger.Log("Method DeleteWay()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteWay"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                Ways ways = (Ways)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
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

        private void DeleteDetection(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDetection()...", category: Category.Info, priority: Priority.Low);
                if (SelectedDetection.IdDetections > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDetection"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        List<Detections> GetDetections_child = new List<Detections>(Detections.Where(d => d.Parent == SelectedDetection.Parent));
                        if (GetDetections_child.Count == 1 && SelectedDetection.Parent != null)
                        {
                            Detections Detection_Group = Detections.Where(a => a.Key == SelectedDetection.Parent).FirstOrDefault();
                            Detections.Remove(Detection_Group);
                            Detections.Remove(SelectedDetection);
                        }
                        else
                        {
                            if (SelectedDetection.IdDetections == 0)
                            {
                                List<Detections> DetectionList = Detections.Where(a => a.Parent == SelectedDetection.Key).ToList();
                                foreach (Detections detection in DetectionList)
                                {
                                    Detections.Remove(detection);
                                }
                            }
                            Detections.Remove(SelectedDetection);
                        }

                        SelectedDetection = Detections.FirstOrDefault();

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
                        List<Detections> GetDetections_child = new List<Detections>(Detections.Where(d => d.Parent == SelectedDetection.Parent));
                        if (GetDetections_child.Count == 1 && SelectedDetection.Parent != null)
                        {
                            Detections Detection_Group = Detections.Where(a => a.Key == SelectedDetection.Parent).FirstOrDefault();
                            Detections.Remove(Detection_Group);
                            Detections.Remove(SelectedDetection);
                        }
                        else
                        {
                            if (SelectedDetection.IdDetections == 0)
                            {
                                List<Detections> DetectionList = Detections.Where(a => a.Parent == SelectedDetection.Key).ToList();
                                foreach (Detections detection in DetectionList)
                                {
                                    Detections.Remove(detection);
                                }
                            }
                            Detections.Remove(SelectedDetection);
                        }
                        SelectedDetection = Detections.FirstOrDefault();


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
                if (SelectedOption.IdOptions > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteOption"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        List<Options> GetOptions_child = new List<Options>(Options.Where(d => d.Parent == SelectedOption.Parent));
                        if (GetOptions_child.Count == 1 && SelectedOption.Parent != null)
                        {
                            Options Option_Group = Options.Where(a => a.Key == SelectedOption.Parent).FirstOrDefault();
                            Options.Remove(Option_Group);
                            Options.Remove(SelectedOption);
                        }
                        else
                        {
                            if (SelectedOption.IdOptions == 0)
                            {
                                List<Options> OptionList = Options.Where(a => a.Parent == SelectedOption.Key).ToList();
                                foreach (Options Option in OptionList)
                                {
                                    Options.Remove(Option);
                                }
                            }
                            Options.Remove(SelectedOption);
                        }

                        SelectedOption = Options.FirstOrDefault();
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
                        List<Options> GetOptions_child = new List<Options>(Options.Where(d => d.Parent == SelectedOption.Parent));
                        if (GetOptions_child.Count == 1 && SelectedOption.Parent != null)
                        {
                            Options Option_Group = Options.Where(a => a.Key == SelectedOption.Parent).FirstOrDefault();
                            Options.Remove(Option_Group);
                            Options.Remove(SelectedOption);
                        }
                        else
                        {
                            if (SelectedOption.IdOptions == 0)
                            {
                                List<Options> OptionList = Options.Where(a => a.Parent == SelectedOption.Key).ToList();
                                foreach (Options Option in OptionList)
                                {
                                    Options.Remove(Option);
                                }
                            }
                            Options.Remove(SelectedOption);
                        }

                        SelectedOption = Options.FirstOrDefault();
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
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteSparePart"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                SpareParts spareParts = (SpareParts)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    SpareParts.Remove(SelectedSparePart);
                    SpareParts = new ObservableCollection<SpareParts>(SpareParts);
                    SelectedSparePart = SpareParts.FirstOrDefault();
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
                GeosApplication.Instance.Logger.Log("Method DeleteFamily()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteFamily"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                ConnectorFamilies connectorFamilies = (ConnectorFamilies)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    Families.Remove(SelectedFamily);
                    Families = new ObservableCollection<ConnectorFamilies>(Families);
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
                if (Name == null)
                {
                    InformationError = null;
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                    if (error != null)
                    {
                        return;
                    }
                }

                else
                {
                    InformationError = " ";
                }

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

                if (Ways.ToList().Count > 0 && SelectedDefaultWayType == null)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDefaultWayTypePresent").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method EditSaveModule()...default way type is mandatory.", category: Category.Info, priority: Priority.Low);
                    return;
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

                if (SelectedDefaultWayType != null)
                {
                    UpdateProductTypes.IdDefaultWayType = SelectedDefaultWayType.IdWays;
                }
                else
                {
                    UpdateProductTypes.IdDefaultWayType = 103;
                }

                UpdateProductTypes.IdStatus = SelectedStatus.IdLookupValue;

                UpdateProductTypes.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                if (ClonedProductType.IdTemplate == 0)
                {
                    UpdateProductTypes.IsTemplate_NotExist = true;
                }

                if (Detections.Count > 0)
                    Detections = new ObservableCollection<Detections>(Detections.Where(a => a.IdDetections > 0));

                if (Options.Count > 0)
                    Options = new ObservableCollection<Options>(Options.Where(a => a.IdOptions > 0));

                //Way
                UpdateProductTypes.WayList = new List<Ways>();

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

                //SpareParts
                UpdateProductTypes.SparePartList = new List<SpareParts>();

                //Deleted SpareParts
                foreach (SpareParts itemSpareParts in ClonedProductType.SparePartList)
                {
                    if (!SpareParts.Any(x => x.IdSpareParts == itemSpareParts.IdSpareParts))
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
                    if (!ClonedProductType.SparePartList.Any(x => x.IdSpareParts == itemSpareParts.IdSpareParts))
                    {
                        SpareParts connectorFamilies = (SpareParts)itemSpareParts.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateProductTypes.SparePartList.Add(connectorFamilies);
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSparePartsAdd").ToString(), itemSpareParts.Name) });
                    }
                }

                UpdateProductTypes.OptionList = new List<Options>();

                if (Options.Count > 0)
                {
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

                //Updated options
                foreach (Options originalOption in ClonedProductType.OptionList_Group)
                {
                    if (Options.Any(x => x.IdOptions == originalOption.IdOptions))
                    {
                        Options OptionsUpdated = Options.FirstOrDefault(x => x.IdOptions == originalOption.IdOptions);
                        if (OptionsUpdated.OrderNumber != originalOption.OrderNumber)
                        {
                            Options Options = (Options)OptionsUpdated.Clone();
                            Options.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            Options.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdateProductTypes.OptionList.Add(Options);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogOrderOption").ToString(), OptionsUpdated.Name, originalOption.OrderNumber, OptionsUpdated.OrderNumber) });
                        }
                    }
                }

                UpdateProductTypes.DetectionList = new List<Detections>();

                if (Detections.Count > 0)
                {
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

                //Updated Detections
                foreach (Detections originalDetection in ClonedProductType.DetectionList_Group)
                {
                    if (Detections.Any(x => x.IdDetections == originalDetection.IdDetections))
                    {
                        Detections DetectionsUpdated = Detections.FirstOrDefault(x => x.IdDetections == originalDetection.IdDetections);
                        if (DetectionsUpdated.OrderNumber != originalDetection.OrderNumber)
                        {
                            Detections Detections = (Detections)DetectionsUpdated.Clone();
                            Detections.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            Detections.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdateProductTypes.DetectionList.Add(Detections);
                        }
                    }
                }

                //Families
                UpdateProductTypes.FamilyList = new List<ConnectorFamilies>();

                //Deleted family
                foreach (ConnectorFamilies itemFamily in ClonedProductType.FamilyList)
                {
                    if (!Families.Any(x => x.IdFamily == itemFamily.IdFamily))
                    {
                        ConnectorFamilies connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateProductTypes.FamilyList.Add(connectorFamilies);
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFamilyDelete").ToString(), itemFamily.Name) });
                    }
                }
                //Added family
                foreach (ConnectorFamilies itemFamily in Families)
                {
                    if (!ClonedProductType.FamilyList.Any(x => x.IdFamily == itemFamily.IdFamily))
                    {
                        ConnectorFamilies connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateProductTypes.FamilyList.Add(connectorFamilies);
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFamilyAdd").ToString(), itemFamily.Name) });
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
                        if ((productTypeAttachedDocUpdated.SavedFileName != originalProductType.SavedFileName) || (productTypeAttachedDocUpdated.OriginalFileName != originalProductType.OriginalFileName) || (productTypeAttachedDocUpdated.Description != originalProductType.Description))
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
                                    ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesNameUpdate").ToString(), originalProductType.OriginalFileName, "None") });
                                else
                                {
                                    if (string.IsNullOrEmpty(originalProductType.OriginalFileName))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesNameUpdate").ToString(), "None", productTypeAttachedDocUpdated.OriginalFileName) });
                                    else
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesNameUpdate").ToString(), originalProductType.OriginalFileName, productTypeAttachedDocUpdated.OriginalFileName) });
                                }
                            }
                            if (productTypeAttachedDocUpdated.Description != originalProductType.Description)
                            {
                                if (string.IsNullOrEmpty(productTypeAttachedDocUpdated.Description))
                                    ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), originalProductType.Description, "None") });
                                else
                                {
                                    if (string.IsNullOrEmpty(originalProductType.Description))
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), "None", productTypeAttachedDocUpdated.Description) });
                                    else
                                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogFilesDescriptionUpdate").ToString(), originalProductType.Description, productTypeAttachedDocUpdated.Description) });
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
                        if ((productTypeImageUpdated.OriginalFileName != originalproductTypeImage.OriginalFileName) || (productTypeImageUpdated.SavedFileName != originalproductTypeImage.SavedFileName) || (productTypeImageUpdated.Description != originalproductTypeImage.Description || productTypeImageUpdated.Position != originalproductTypeImage.Position))
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

                //Customers add and Delete
                UpdateProductTypes.CustomerList = new List<RegionsByCustomer>();
                List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                foreach (RegionsByCustomer item in ClonedProductType.CustomerList)
                {
                    if (item.IsChecked == true)
                    {
                        RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        if (obj1.IsChecked == false)
                        {
                            RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProductTypes.CustomerList.Add(connectorFamilies);
                            ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                        }
                    }
                }

                foreach (RegionsByCustomer item in tempCustomersList)
                {
                    RegionsByCustomer obj1 = ClonedProductType.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                    if (obj1.IsChecked == false)
                    {
                        RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateProductTypes.CustomerList.Add(connectorFamilies);
                        ChangeLogList.Add(new ProductTypeLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                    }
                }

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

                UpdateProductTypes.ProductTypeImageList.ForEach(x => x.AttachmentImage = null);

                AddProductTypeLogDetails();
                UpdateProductTypes.ProductTypeLogEntryList = ChangeLogList.ToList();
                IsSaveChanges = PCMService.UpdateProductType(UpdateProductTypes);
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ModuleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method EditSaveModule()....executed successfully", category: Category.Info, priority: Priority.Low);

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
                if (results.Count() > 0 || List1.Count()!=List.Count())
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
                                me[BindableBase.GetPropertyName(() => CompatibilityError)];


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
