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
using System.Text.RegularExpressions;
using Microsoft.Win32;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Data.Common.PLM;
using Newtonsoft.Json;
using Emdep.Geos.Data.Common.SynchronizationClass;
using DevExpress.Data.Filtering;
using Emdep.Geos.Modules.PLM.CommonClasses;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class EditDetectionViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

       // IPCMService PCMService = new PCMServiceController("localhost:6699");
        // IPLMService PLMService = new PLMServiceController("localhost:6699");
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

        #region Declaration
        #region [rdixit][29.03.2023][GEOS2-4262]
        private bool isDuplicateDetectionButtonEnabled;
        bool isDuplicateDetectionAdded;
        public bool isDuplicateClicked = false;
        private bool isCheckedImages;
        private bool isCheckedAttachment;
        private bool isCheckedLinks;
        private bool isCheckedCustomers;
        private bool isCheckedPrice;
        string groups;
        string regions;
        string countries;
        string plants;
        DetectionDetails clonedDetection;
        #endregion
        public string PrevName;
        public string PCMDetectionPriceListIncludedGridSetting = GeosApplication.Instance.UserSettingFolderName + "PCMDetectionPriceListIncludedGridSetting.Xml";
        double windowWidth;
        double windowheight;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private Ways way;
        private string header;
        private Detections detection;
        private SpareParts sparePart;
        private string name;
        private string description;
        private string selectedType;
        private uint? weldOrder;
        private string code;
        private string testType;
        private string type;
        private ObservableCollection<Language> languages;
        private ObservableCollection<TestTypes> testTypesMenuList;
        private TestTypes selectedTestType;
        private int idLanguage;
        private uint idDetectionsOption;
        private uint idDetectionTypeOption;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;
        private bool isCheckedCopyNameDescription;
        private Language languageSelected;
        private string fileName;
        private string fileDescription;
        private byte[] fileInBytes;
        private ObservableCollection<DetectionAttachedDoc> optionWayDetectionSparePartFilesList;
        private ObservableCollection<DetectionAttachedDoc> fourOptionWayDetectionSparePartFilesList;
        private string optionWayDetectionSparePartOriginalFileName;
        private List<Object> attachmentObjectList;
        private int filesCount;
        private DetectionAttachedDoc selectedOptionWayDetectionSparePartFile;
        private uint idDetections;
        private uint idDetectionType;
        private bool IsCopyName;
        private bool IsCopyDescription;
        private DetectionDetails clonedDetections;
        private ObservableCollection<DetectionAttachedLink> optionWayDetectionSparePartLinksList;
        private ObservableCollection<DetectionAttachedLink> fourOptionWayDetectionSparePartLinksList;
        private DetectionAttachedLink selectedOptionWayDetectionSparePartLink;
        private int linksCount;
        private ObservableCollection<DetectionImage> optionWayDetectionSparePartImagesList;
        private ObservableCollection<DetectionImage> fourOptionWayDetectionSparePartImagesList;
        private DetectionImage selectedOptionWayDetectionSparePartImage;
        private DetectionImage selectedContentTemplateImage;
        private DetectionImage maximizedElement;
        private DetectionImage selectedDefaultImage;
        private DetectionImage selectedImage;
        private int selectedImageIndex;
        private int imagesCount;
        private ObservableCollection<DetectionOrderGroup> orderGroupList;
        private DetectionOrderGroup selectedOrder;
        private ObservableCollection<DetectionGroup> detectionGroupList;
        private ObservableCollection<DetectionGroup> groupList;
        private ObservableCollection<DetectionGroup> groupList_old;
        private DetectionGroup selectedGroupItem;
        private List<DetectionGroup> orderGroupList_;
        private ObservableCollection<DetectionGroup> groupList_Order;
        private ObservableCollection<DetectionOrderGroup> detectionOrderGroup_New;
        private ObservableCollection<DetectionLogEntry> detectionChangeLogList;
        private ObservableCollection<ProductTypes> productTypesList;
        private ObservableCollection<DetectionLogEntry> changeLogList;
        private DetectionLogEntry selectedDetectionChangeLog;
        private ProductTypes selectedProductTypes;
        private char? _SelectedLetter;
        string filterStringForName;
        private ObservableCollection<char> _Letters;
        private ObservableCollection<RegionsByCustomer> customersMenuList;
        private ObservableCollection<RegionsByCustomer> selectedCustomersList;
        private ObservableCollection<FourRegionsWithCustomerCount> fourRecordsCustomersList;
        MaximizedElementPosition maximizedElementPosition;
        private double dialogHeight;
        private double dialogWidth;
        private ObservableCollection<DetectionTypes> testList;
        private DetectionTypes selectedTest;
        private Visibility isStackPanelVisible;
        private bool isWaySave;
        private bool isSparepartSave;
        private bool isDetectionSave;
        private bool isOptionSave;
        private bool isSelectedTestReadOnly;
        private string nameError;
        private string error = string.Empty;
        private List<LookupValue> statusList;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;
        private List<LookupValue> ecosVisibilityList;
        private LookupValue selectedECOSVisibility;
        private List<LookupValue> scopelist;//Added by [plahange]
        private LookupValue selectedScopeList;//Added by [plahange]
        //GEOS2-3199

        private ObservableCollection<PLMDetectionPrice> includedPLMDetectionPriceList;
        private ObservableCollection<PLMDetectionPrice> notIncludedPLMDetectionPriceList;

        private PLMDetectionPrice selectedIncludedPLMDetectionPrice;
        private PLMDetectionPrice selectedNotIncludedPLMDetectionPrice;

        private List<LookupValue> logicList;
        private LookupValue selectedLogic;
        private Visibility groupBoxVisible;
        private Int64 includedActiveCount;
        private string includedFirstActiveName;
        private ImageSource includedFirstActiveCurrencyIconImage;
        private double? includedFirstActiveSellPrice;
        private string currencySymbol;
        private string onFocusFgColor;
        private bool isDetectionColumnChooserVisible;
        private bool isFirstTimeLoad;
        private string selectedCurrencySymbol;
        private Currency selectedCurrency;

        private ITokenService tokenService;
        List<GeosAppSetting> geosAppSettingList;
        private bool isAdded;
        private bool isAcceptButtonEnabled = true;
        private bool isOnlyAcceptButtonEnabled;
        private bool isBPLMessageShow;
        private bool isCPLMessageShow;
        private bool isBPLCalculateRuleValue;
        private bool isCPLCalculateRuleValue;
        private bool isEnabled;
        private string visible;
        private bool isReadOnlyField;
        private bool allowDragDrop;
        private Visibility isCollapsed;
        public bool isEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][14/02/2023]
        private string isImageScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][10/03/2023]
        private string isAttachmentScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][13/03/2023]
        private string isLinkScrollVisible = "Disabled";//[Sudhir.Jangra][GEOS2-1960][13/03/2023]

        public bool isCheckedRelatedModules;//[Sudhir.Jangra][GEOS2-4468][31/05/2023]

        ObservableCollection<ProductTypeLogEntry> productTypeChangeLogList;//[Sudhir.Jangra][GEOS2-4460][28/06/2023]

        private Visibility isTestTypeVisible;//[Sudhir.Jangra][GEOS2-4691][21/08/2023]
        private Visibility isWeldTypeVisible;//[Sudhir.Jangra][GEOS2-4691][21/08/2023]

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


        private ObservableCollection<DetectionLogEntry> commentList;//[Sudhir.Jangra][GEOS2-4935]
        private string commentText;//[Sudhir.Jangra][GEOS2-4935]
        private DateTime? commentDateTimeText;//[Sudhir.Jangra][GEOS2-4935]
        private string commentFullNameText;//[Sudhir.Jangra][GEOS2-4935]
        private byte[] userProfileImageByte;//[Sudhir.Jangra][GEOS2-4935]
        private List<DetectionLogEntry> addCommentsList;//[Sudhir.jangra][GEOS2-4935]
        private DetectionLogEntry selectedComment;//[Sudhir.jangra][GEOSS2-4935]
        private ObservableCollection<DetectionLogEntry> deleteCommentsList;//[Sudhir.Jangra][GEOS2-4935]
        private bool isDeleted;//[Sudhir.Jangra][GEOS2-4935]
        private List<DetectionLogEntry> updatedCommentList;//[Sudhir.Jangra][GEOS2-4935]

        #endregion

        #region Properties
        #region [rdixit][29.03.2023][GEOS2-4262]
        public bool IsCheckedImages
        {
            get { return isCheckedImages; }
            set { isCheckedImages = value; }
        }
        public bool IsCheckedAttachment
        {
            get { return isCheckedAttachment; }
            set { isCheckedAttachment = value; }
        }
        public bool IsCheckedLinks
        {
            get { return isCheckedLinks; }
            set { isCheckedLinks = value; }
        }
        public bool IsCheckedCustomers
        {
            get { return isCheckedCustomers; }
            set { isCheckedCustomers = value; }
        }
        public bool IsCheckedPrice
        {
            get { return isCheckedPrice; }
            set { isCheckedPrice = value; }
        }
        public bool IsDuplicateDetectionAdded
        {
            get
            {
                return isDuplicateDetectionAdded;
            }

            set
            {
                isDuplicateDetectionAdded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateDetectionAdded"));

            }
        }
        public bool IsDuplicateDetectionButtonEnabled
        {
            get
            {
                return isDuplicateDetectionButtonEnabled;
            }

            set
            {
                isDuplicateDetectionButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateDetectionButtonEnabled"));
            }
        }
        public DetectionDetails ClonedDetection
        {
            get
            {
                return clonedDetection;
            }
            set
            {
                clonedDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDetection"));
            }
        }
        #endregion
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

        public bool IsDetectionColumnChooserVisible
        {
            get { return isDetectionColumnChooserVisible; }
            set
            {
                isDetectionColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetectionColumnChooserVisible"));
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

        public DetectionDetails NewOption { get; set; }
        public DetectionDetails NewDetection { get; set; }
        public DetectionDetails UpdatedItem { get; set; }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        public DetectionDetails NewWay { get; set; }
        public DetectionDetails NewSparePart { get; set; }

        public double WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
            }
        }

        public double Windowheight
        {
            get { return windowheight; }
            set
            {
                windowheight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Windowheight"));
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

        public Ways Way
        {
            get
            {
                return way;
            }

            set
            {
                way = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Way"));
            }
        }

        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }

        public Detections Detection
        {
            get
            {
                return detection;
            }

            set
            {
                detection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Detection"));
            }
        }

        public SpareParts SparePart
        {
            get
            {
                return sparePart;
            }

            set
            {
                sparePart = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SparePart"));
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

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));

                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]


            }
        }

        public string SelectedType
        {
            get
            {
                return selectedType;
            }

            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }

        public uint? WeldOrder
        {
            get
            {
                return weldOrder;
            }

            set
            {
                if (value == null || value < 0)
                    weldOrder = 0;
                else
                    weldOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeldOrder"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public string TestType
        {
            get
            {
                return testType;
            }

            set
            {
                testType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestType"));
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public int IdLanguage
        {
            get
            {
                return idLanguage;
            }

            set
            {
                idLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdLanguage"));
            }
        }

        public ObservableCollection<TestTypes> TestTypesMenuList
        {
            get
            {
                return testTypesMenuList;
            }

            set
            {
                testTypesMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestTypesMenuList"));
                // isEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public TestTypes SelectedTestType
        {
            get
            {
                return selectedTestType;
            }

            set
            {
                selectedTestType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTestType"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public uint IdDetectionsOption
        {
            get
            {
                return idDetectionsOption;
            }

            set
            {
                idDetectionsOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionsOption"));
            }
        }

        public uint IdDetectionTypeOption
        {
            get
            {
                return idDetectionTypeOption;
            }

            set
            {
                idDetectionTypeOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionTypeOption"));
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
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public bool IsCheckedCopyNameDescription
        {
            get
            {
                return isCheckedCopyNameDescription;
            }

            set
            {
                isCheckedCopyNameDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyNameDescription"));
                UncheckedCopyDescription(null);

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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //   IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public string FileDescription
        {
            get
            {
                return fileDescription;
            }

            set
            {
                fileDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileDescription"));
            }
        }

        public byte[] FileInBytes
        {
            get
            {
                return fileInBytes;
            }

            set
            {
                fileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileInBytes"));
            }
        }

        public ObservableCollection<DetectionAttachedDoc> OptionWayDetectionSparePartFilesList
        {
            get
            {
                return optionWayDetectionSparePartFilesList;
            }

            set
            {
                optionWayDetectionSparePartFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartFilesList"));
            }
        }

        public ObservableCollection<DetectionAttachedDoc> FourOptionWayDetectionSparePartFilesList
        {
            get
            {
                return fourOptionWayDetectionSparePartFilesList;
            }

            set
            {
                fourOptionWayDetectionSparePartFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourOptionWayDetectionSparePartFilesList"));
            }
        }

        public string OptionWayDetectionSparePartOriginalFileName
        {
            get
            {
                return optionWayDetectionSparePartOriginalFileName;
            }

            set
            {
                optionWayDetectionSparePartOriginalFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartOriginalFileName"));
            }
        }

        public List<object> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }

        public int FilesCount
        {
            get
            {
                return filesCount;
            }

            set
            {
                filesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilesCount"));
            }
        }

        public DetectionAttachedDoc SelectedOptionWayDetectionSparePartFile
        {
            get
            {
                return selectedOptionWayDetectionSparePartFile;
            }

            set
            {
                selectedOptionWayDetectionSparePartFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartFile"));
            }
        }

        public uint IdDetections
        {
            get
            {
                return idDetections;
            }

            set
            {
                idDetections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetections"));

            }
        }

        public uint IdDetectionType
        {
            get
            {
                return idDetectionType;
            }

            set
            {
                idDetectionType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionType"));

            }
        }

        public DetectionDetails ClonedDetections
        {
            get
            {
                return clonedDetections;
            }

            set
            {
                clonedDetections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDetections"));
            }
        }

        public ObservableCollection<DetectionAttachedLink> OptionWayDetectionSparePartLinksList
        {
            get
            {
                return optionWayDetectionSparePartLinksList;
            }

            set
            {
                optionWayDetectionSparePartLinksList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartLinksList"));
            }
        }

        public ObservableCollection<DetectionAttachedLink> FourOptionWayDetectionSparePartLinksList
        {
            get
            {
                return fourOptionWayDetectionSparePartLinksList;
            }

            set
            {
                fourOptionWayDetectionSparePartLinksList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourOptionWayDetectionSparePartLinksList"));
            }
        }

        public DetectionAttachedLink SelectedOptionWayDetectionSparePartLink
        {
            get
            {
                return selectedOptionWayDetectionSparePartLink;
            }

            set
            {
                selectedOptionWayDetectionSparePartLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartLink"));
            }
        }

        public int LinksCount
        {
            get
            {
                return linksCount;
            }

            set
            {
                linksCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinksCount"));
            }
        }

        public ObservableCollection<DetectionImage> OptionWayDetectionSparePartImagesList
        {
            get
            {
                return optionWayDetectionSparePartImagesList;
            }

            set
            {
                optionWayDetectionSparePartImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartImagesList"));
                // isEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<DetectionImage> FourOptionWayDetectionSparePartImagesList
        {
            get
            {
                return fourOptionWayDetectionSparePartImagesList;
            }

            set
            {
                fourOptionWayDetectionSparePartImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourOptionWayDetectionSparePartImagesList"));
                //  isEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public DetectionImage SelectedOptionWayDetectionSparePartImage
        {
            get
            {
                return selectedOptionWayDetectionSparePartImage;
            }

            set
            {
                selectedOptionWayDetectionSparePartImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartImage"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public DetectionImage SelectedContentTemplateImage
        {
            get
            {
                return selectedContentTemplateImage;
            }

            set
            {
                selectedContentTemplateImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContentTemplateImage"));
                IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]

            }
        }

        public DetectionImage SelectedDefaultImage
        {
            get
            {
                return selectedDefaultImage;
            }

            set
            {
                selectedDefaultImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDefaultImage"));
                IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public DetectionImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
                IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public DetectionImage MaximizedElement
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

        public int ImagesCount
        {
            get
            {
                return imagesCount;
            }

            set
            {
                imagesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesCount"));

            }
        }

        public ObservableCollection<DetectionOrderGroup> OrderGroupList
        {
            get
            {
                return orderGroupList;
            }

            set
            {
                orderGroupList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("OrderGroupList"));
                // isEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public List<DetectionGroup> OrderGroupList_
        {
            get
            {
                return orderGroupList_;
            }

            set
            {
                orderGroupList_ = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderGroupList_"));
            }
        }

        public DetectionOrderGroup SelectedOrder
        {
            get
            {
                return selectedOrder;
            }

            set
            {
                selectedOrder = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrder"));
            }
        }

        public ObservableCollection<DetectionGroup> DetectionGroupList
        {
            get
            {
                return detectionGroupList;
            }

            set
            {
                detectionGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionGroupList"));

            }
        }

        public ObservableCollection<DetectionGroup> GroupList
        {
            get
            {
                return groupList;
            }

            set
            {
                groupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<DetectionGroup> GroupList_old
        {
            get
            {
                return groupList_old;
            }

            set
            {
                groupList_old = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList_old"));
            }
        }

        public DetectionGroup SelectedGroupItem
        {
            get
            {
                return selectedGroupItem;
            }

            set
            {
                selectedGroupItem = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroupItem"));


            }
        }

        public ObservableCollection<DetectionGroup> GroupList_Order
        {
            get
            {
                return groupList_Order;
            }

            set
            {
                groupList_Order = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList_Order"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]

            }
        }

        public ObservableCollection<DetectionOrderGroup> DetectionOrderGroup_New
        {
            get
            {
                return detectionOrderGroup_New;
            }

            set
            {
                detectionOrderGroup_New = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionOrderGroup_New"));
            }
        }

        public ObservableCollection<DetectionLogEntry> DetectionChangeLogList
        {
            get
            {
                return detectionChangeLogList;
            }

            set
            {
                detectionChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionChangeLogList"));
            }
        }
        public ObservableCollection<ProductTypes> ProductTypesList
        {
            get
            {
                return productTypesList;
            }

            set
            {
                productTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesList"));
            }
        }

        public ObservableCollection<DetectionLogEntry> ChangeLogList
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

        public DetectionLogEntry SelectedDetectionChangeLog
        {
            get
            {
                return selectedDetectionChangeLog;
            }

            set
            {
                selectedDetectionChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetectionChangeLog"));
            }
        }

        public ProductTypes SelectedProductTypes
        {
            get
            {
                return selectedProductTypes;
            }

            set
            {
                selectedProductTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductTypes"));
                // IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][24/02/2023]
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
                //  isEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
                //IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public ObservableCollection<FourRegionsWithCustomerCount> FourRecordsCustomersList
        {
            get { return fourRecordsCustomersList; }
            set
            {
                fourRecordsCustomersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsCustomersList"));
                //  isEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

        public ObservableCollection<DetectionTypes> TestList
        {
            get
            {
                return testList;
            }

            set
            {
                testList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestList"));
                // IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public DetectionTypes SelectedTest
        {
            get
            {
                return selectedTest;
            }

            set
            {
                selectedTest = value;
                if (SelectedTest != null)
                {
                    if (SelectedTest.IdDetectionType == 1)
                    {
                        Header = System.Windows.Application.Current.FindResource("CaptionWay").ToString();

                    }
                    else if (SelectedTest.IdDetectionType == 4)
                    {
                        Header = System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString();
                    }
                    else if (SelectedTest.IdDetectionType == 3)
                    {
                        Header = System.Windows.Application.Current.FindResource("CaptionOptions").ToString();
                    }
                    else if (SelectedTest.IdDetectionType == 2)
                    {
                        Header = System.Windows.Application.Current.FindResource("CaptionDetections").ToString();
                    }
                    if (OrderGroupList == null)
                    {
                        OrderGroupList = new ObservableCollection<DetectionOrderGroup>();
                        GroupList = new ObservableCollection<DetectionGroup>();
                    }



                    var placeholderOrder = new DetectionOrderGroup { Name = "---", IdGroup = 0, Key = string.Empty, Parent = string.Empty };
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(SelectedTest.IdDetectionType));
                    OrderGroupList.Insert(0, placeholderOrder);
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(SelectedTest.IdDetectionType));
                    if (ClonedDetections != null)
                    {
                        ClonedDetections.DetectionGroupList = GroupList.ToList();
                    }
                    if (GroupList.Count == 0)
                    {
                        IsStackPanelVisible = Visibility.Hidden;
                    }
                    else
                    {
                        IsStackPanelVisible = Visibility.Visible;
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTest"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public List<LookupValue> ScopeList //Added by [plahange]
        {
            get { return scopelist; }
            set
            {
                scopelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScopeList"));
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public LookupValue SelectedScopeList //Added by [plahange]
        {
            get
            {
                return selectedScopeList;
            }

            set
            {
                selectedScopeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScopeList"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }
        public Visibility IsStackPanelVisible
        {
            get
            {
                return isStackPanelVisible;
            }

            set
            {
                isStackPanelVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStackPanelVisible"));
            }
        }

        public bool IsWaySave
        {
            get
            {
                return isWaySave;
            }

            set
            {
                isWaySave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWaySave"));

            }
        }

        public bool IsSparepartSave
        {
            get
            {
                return isSparepartSave;
            }

            set
            {
                isSparepartSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSparepartSave"));

            }
        }

        public bool IsDetectionSave
        {
            get
            {
                return isDetectionSave;
            }

            set
            {
                isDetectionSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetectionSave"));

            }
        }

        public bool IsOptionSave
        {
            get
            {
                return isOptionSave;
            }

            set
            {
                isOptionSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOptionSave"));

            }
        }

        public bool IsSelectedTestReadOnly
        {
            get
            {
                return isSelectedTestReadOnly;
            }

            set
            {
                isSelectedTestReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedTestReadOnly"));

            }
        }

        public string NameError
        {
            get
            {
                return nameError;
            }
            set
            {
                nameError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NameError"));

            }
        }

        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public int SelectedStatusIndex
        {
            get { return selectedStatusIndex; }
            set
            {
                selectedStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusIndex"));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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
        public List<LookupValue> ECOSVisibilityList
        {
            get { return ecosVisibilityList; }
            set
            {
                ecosVisibilityList = value;
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
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]

            }
        }

        //GEOS2-3199

        public ObservableCollection<PLMDetectionPrice> IncludedPLMDetectionPriceList
        {
            get
            {
                return includedPLMDetectionPriceList;
            }

            set
            {
                includedPLMDetectionPriceList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IncludedPLMDetectionPriceList"));
            }
        }

        public ObservableCollection<PLMDetectionPrice> NotIncludedPLMDetectionPriceList
        {
            get
            {
                return notIncludedPLMDetectionPriceList;
            }

            set
            {
                notIncludedPLMDetectionPriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotIncludedPLMDetectionPriceList"));
            }
        }

        public PLMDetectionPrice SelectedIncludedPLMDetectionPrice
        {
            get
            {
                return selectedIncludedPLMDetectionPrice;
            }

            set
            {
                selectedIncludedPLMDetectionPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncludedPLMDetectionPrice"));
                //IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
            }
        }

        public PLMDetectionPrice SelectedNotIncludedPLMDetectionPrice
        {
            get
            {
                return selectedNotIncludedPLMDetectionPrice;
            }

            set
            {
                selectedNotIncludedPLMDetectionPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNotIncludedPLMDetectionPrice"));
                //  IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
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

            }
        }

        public Currency SelectedCurrency
        {
            get { return selectedCurrency; }
            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }

        public bool IsAdded
        {
            get
            {
                return isAdded;
            }

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

        public bool IsOnlyAcceptButtonEnabled
        {
            get { return isOnlyAcceptButtonEnabled; }
            set
            {
                if (GeosApplication.Instance.IsPermissionReadOnlyForPLM)
                {
                    isOnlyAcceptButtonEnabled = false;

                }
                else
                {
                    isOnlyAcceptButtonEnabled = true;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsOnlyAcceptButtonEnabled"));
            }
        }
        //public bool IsOnlyAcceptButtonEnabled
        //        {
        //            get { return isOnlyAcceptButtonEnabled; }
        //            set
        //            {

        //                isOnlyAcceptButtonEnabled = value;
        //                OnPropertyChanged(new PropertyChangedEventArgs("IsOnlyAcceptButtonEnabled"));
        //            }
        //        }

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


        #region EOS2-3639 
        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
        private ObservableCollection<Emdep.Geos.Data.Common.PLM.Group> customerGroupList;
        private Emdep.Geos.Data.Common.PLM.Group selectedGroup;
        private ObservableCollection<Region> regionList;
        private List<object> selectedRegion;
        private ObservableCollection<Country> countryList;
        private List<object> selectedCountry;
        private ObservableCollection<Site> plantList;
        private List<object> selectedPlant;
        private int isFilterStatus;
        private bool isRetrive;
        private List<CPLCustomer> pcmCustomerList;
        private List<CPLCustomer> pcmCustomerList1;
        private int group;
        private int region;
        private int country;
        private int plant;
        private Visibility isExportVisible;
        public List<CPLCustomer> PCMCustomerList
        {
            get
            {
                return pcmCustomerList;
            }
            set
            {
                pcmCustomerList = value;
                if (pcmCustomerList != null)
                {
                    IncludedCustomerList = new ObservableCollection<CPLCustomer>(pcmCustomerList.Where(i => i.IsIncluded == 1).ToList());
                    NotIncludedCustomerList = new ObservableCollection<CPLCustomer>(pcmCustomerList.Where(i => i.IsIncluded == 0).ToList());
                }
                OnPropertyChanged(new PropertyChangedEventArgs("PCMCustomerList"));
            }
        }
        public ObservableCollection<Emdep.Geos.Data.Common.PLM.Group> CustomerGroupList
        {
            get
            {
                return customerGroupList;
            }

            set
            {
                customerGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroupList"));
            }
        }

        public Emdep.Geos.Data.Common.PLM.Group SelectedGroup
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
        #region [rdixit][29.03.2023][GEOS2-4262]
        public string Groups
        {
            get
            {
                return groups;
            }
            set
            {
                groups = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Groups"));

            }
        }
        public string Regions
        {
            get
            {
                return regions;
            }
            set
            {
                regions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Regions"));

            }
        }
        public string Countries
        {
            get
            {
                return countries;
            }
            set
            {
                countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Countries"));

            }
        }
        public string Plants
        {
            get
            {
                return plants;
            }
            set
            {
                plants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Plants"));

            }
        }
        #endregion
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
        #endregion
        #region GEOS2-3787 
        private ObservableCollection<CPLCustomer> includedCustomerList;
        private ObservableCollection<CPLCustomer> notIncludedCustomerList;
        private ObservableCollection<object> selectedIncludedCustomer;
        private ObservableCollection<object> selectedNotIncludedCustomer;
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
                IsEnabledCancelButton = true;//[Sudhir.Jangra][Geos2-3132]
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
        #endregion

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
        public Visibility IsCollapsed
        {
            get
            {
                return isCollapsed;
            }

            set
            {
                isCollapsed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCollapsed"));
            }
        }
        public bool IsEnabledCancelButton//[Sudhir.Jangra][GEOS2-3132][14/02/2023]
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

        //[Sudhir.Jangra][GEOS2-4468][31/05/2023]
        public bool IsCheckedRelatedModules
        {
            get { return isCheckedRelatedModules; }
            set
            {
                isCheckedRelatedModules = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedRelatedModules"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
        public ObservableCollection<ProductTypeLogEntry> ProductTypeChangeLogList
        {
            get { return productTypeChangeLogList; }
            set
            {
                productTypeChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeChangeLogList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4691][21/08/2023]
        public Visibility IsTestTypeVisible
        {
            get { return isTestTypeVisible; }
            set
            {
                isTestTypeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTestTypeVisible"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4691][21/08/2023]
        public Visibility IsWeldTypeVisible
        {
            get { return isWeldTypeVisible; }
            set
            {
                isWeldTypeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWeldTypeVisible"));
            }
        }


        //[Sudhir.jangra][GEOS2-4935]
        public ObservableCollection<DetectionLogEntry> CommentsList
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

        public List<DetectionLogEntry> AddCommentsList
        {
            get { return addCommentsList; }
            set
            {
                addCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4935]
        public DetectionLogEntry SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        //[Sudhir.jangra][GEOS2-4935]
        public ObservableCollection<DetectionLogEntry> DeleteCommentsList
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
        public List<DetectionLogEntry> UpdatedCommentsList
        {
            get { return updatedCommentList; }
            set
            {
                updatedCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }

        #endregion

        #region ICommands

        public ICommand AcceptButtonActionCommand { get; set; }
        public ICommand CancelButtonActionCommand { get; set; }
        public ICommand AddNewWayCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand UncheckedCopyNameDescriptionCommand { get; set; }
        public ICommand AddFileCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand AddLinkButtonCommand { get; set; }
        public ICommand DeleteLinkCommand { get; set; }
        public ICommand EditLinkCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }
        public ICommand EditImageCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AddOrderGroupCommand { get; set; }
        public ICommand GroupDoubleClickCommand { get; set; }
        public ICommand DeleteGroupCommand { get; set; }


        public ICommand OpenSelectedImageCommand { get; set; }
        public ICommand OpenImageGalleryCommand { get; set; }
        public ICommand RestrictOpeningPopUpCommand { get; set; }
        public ICommand DownloadImageCommand { get; set; }


        public ICommand ExportToExcelCommand { get; set; }

        public ICommand ProductTypeDoubleClickCommand { get; set; }

        //public ICommand SelectedTestChangedCommand { get; set; }
        //GEOS2-3199

        public ICommand CommandOnDragRecordOverNotIncludedDetectionGrid { get; set; }
        public ICommand CommandOnDragRecordOverIncludedDetectionGrid { get; set; }

        public ICommand RuleChangedCommand { get; set; }
        public ICommand LostFocusRuleChangedCommand { get; set; }

        public ICommand CommandShowFilterPopupForIncludedClick { get; set; }
        public ICommand CommandShowFilterPopupForNotIncludedClick { get; set; }

        public ICommand DetectionGridControlLoadedCommand { get; set; }
        public ICommand DetectionGridControlUnloadedCommand { get; set; }


        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
        public ICommand SelectedGroupIndexChangedCommand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCountryCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
        public ICommand ExportToExcelCustomersCommand { get; set; }
        public ICommand ItemPositionChangedCommand { get; set; }//[rdixit][GEOS2-2694][01.08.2022]
        public ICommand SelectedscopeChangedCommand { get; set; }//[plahange][GEOS2-3956]
        public ICommand PrintCommand { get; set; }//[Sudhir.Jangra][GEOS2-4091][24/01/2023]
        public ICommand DuplicateDetectionCommand { get; set; }

        public ICommand DeleteRelatedModulesCommand { get; set; }//[Sudhir.Jangra][GEOS2-4460][26/06/2023]

        public ICommand AddNewRelatedModulesCommand { get; set; }//[Sudhir.Jangra][GEOS2-4460][26/06/2023]


        #region GEOS2-3787
        public ICommand AddIncludedCustomerCommand { get; set; }
        public ICommand AddNotIncludedCustomerCommand { get; set; }
        public ICommand DeleteIncludedCustomerCommand { get; set; }
        public ICommand DeleteNotIncludedCustomerCommand { get; set; }
        public ICommand IncludedCellValueChangingCommand { get; set; }
        public ICommand NotIncludedCellValueChangingCommand { get; set; }

        public ICommand AddCommentsCommand { get; set; }//[Sudhir.jangra][GEOS2-4935][21/11/2023]

        public ICommand DeleteCommentRowCommand { get; set; }//[Sudhir.Jangra][GEOS2-4935]

        public ICommand CommentsGridDoubleClickCommand { get; set; }//[Sudhir.Jangra][GEOS2-4935]

        #endregion
        #endregion

        #region Constructor

        public EditDetectionViewModel()
        {
            try
            {


                GeosApplication.Instance.Logger.Log("Constructor EditDetectionViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                IsAcceptButtonEnabled = true;
                IsOnlyAcceptButtonEnabled = true;
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

                CancelButtonActionCommand = new DelegateCommand<object>(CancelOptionAction);
                AcceptButtonActionCommand = new DelegateCommand<object>(AddNewOptionWaysDetectionsSparePartsAction);

                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameAndDescriptionByLanguge);
                ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                UncheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UncheckedCopyNameDescription);

                AddFileCommand = new DelegateCommand<object>(AddFileAction);
                EditFileCommand = new DelegateCommand<object>(EditFileAction);
                OpenPDFDocumentCommand = new DelegateCommand<object>(OpenPDFDocument);
                DeleteFileCommand = new DelegateCommand<object>(DeleteFileAction);

                AddLinkButtonCommand = new DelegateCommand<object>(AddLinkAction);
                DeleteLinkCommand = new DelegateCommand<object>(DeleteLinkAction);
                EditLinkCommand = new DelegateCommand<object>(EditLinkAction);

                AddImageCommand = new DelegateCommand<object>(AddImageAction);
                DeleteImageCommand = new DelegateCommand<object>(DeleteImageAction);
                EditImageCommand = new DelegateCommand<object>(EditImageAction);
                DownloadImageCommand = new DelegateCommand<object>(DownloadImageAction);
                EscapeButtonCommand = new DelegateCommand<object>(CancelOptionAction);

                AddOrderGroupCommand = new DelegateCommand<object>(AddOrderGroup);
                GroupDoubleClickCommand = new DelegateCommand<object>(EditOrderGroup);
                DeleteGroupCommand = new DelegateCommand<object>(DeleteOrderGroup);


                OpenImageGalleryCommand = new RelayCommand(new Action<object>(OpenImageGalleryAction));
                OpenSelectedImageCommand = new DelegateCommand<object>(OpenSelectedImageAction);
                RestrictOpeningPopUpCommand = new DelegateCommand<object>(RestrictOpeningPopUpAction);

                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);

                ProductTypeDoubleClickCommand = new DelegateCommand<object>(EditProductTypeItem);
                PrintCommand = new RelayCommand(new Action<object>(PrintCommandAction));//[Sudhir.Jangra][GEOS2-4091][24/01/2023]

                DetectionGridControlLoadedCommand = new DelegateCommand<object>(DetectionGridControlLoadedAction);
                DetectionGridControlUnloadedCommand = new DelegateCommand<object>(DetectionGridControlUnloadedAction);

                //SelectedTestChangedCommand = new DelegateCommand<object>(SelectedTestChangedAction);

                IsCheckedCopyNameDescription = true;
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>();

                //GEOS2-3199

                CommandOnDragRecordOverNotIncludedDetectionGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverNotIncludedDetectionGrid);

                CommandOnDragRecordOverIncludedDetectionGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverIncludedDetectionGrid);

                RuleChangedCommand = new DelegateCommand<object>(RuleChangedCommandAction);
                LostFocusRuleChangedCommand = new DelegateCommand<object>(LostFocusRuleChangedCommandAction);
                CommandShowFilterPopupForIncludedClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForIncluded);
                CommandShowFilterPopupForNotIncludedClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForNotIncluded);
                //SelectedscopeChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedscopeChangedCommandAction);//[plahange][GEOS2-3956]
                //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                SelectedGroupIndexChangedCommand = new DelegateCommand<object>(SelectedGroupIndexChangedCommandAction);
                ChangeRegionCommand = new DelegateCommand<object>(ChangeRegionCommandAction);
                ChangeCountryCommand = new DelegateCommand<object>(ChangeCountryCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                //AddCustomerCommand = new DelegateCommand<object>(AddCustomerCommandAction);
                //DeleteCustomerCommand = new DelegateCommand<object>(DeleteCustomerCommandAction);
                ExportToExcelCustomersCommand = new DelegateCommand<object>(ExportToExcelCustomers);
                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                DuplicateDetectionCommand = new RelayCommand(new Action<object>(DuplicateDetectionCommandAction));//[GEOS2-4262][rdixit][29.03.2023]
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

                DeleteRelatedModulesCommand = new DelegateCommand<object>(DeleteRelatedModulesCommandAction);//[Sudhir.Jangra][GEOS2-4460][26/06/2023]
                AddNewRelatedModulesCommand = new DelegateCommand<object>(AddNewRelatedModulesCommandAction);//[Sudhir.Jangra][GEOS2-4460][26/06/2023]

                #region GEOS2-3787
                AddIncludedCustomerCommand = new DelegateCommand<object>(AddIncludedCustomerCommandAction);
                AddNotIncludedCustomerCommand = new DelegateCommand<object>(AddNotIncludedCustomerCommandAction);
                DeleteIncludedCustomerCommand = new DelegateCommand<object>(DeleteIncludedCustomerCommandAction);
                DeleteNotIncludedCustomerCommand = new DelegateCommand<object>(DeleteNotIncludedCustomerCommandAction);
                IncludedCellValueChangingCommand = new DelegateCommand<object>(IncludedCellValueChangingCommandAction);
                NotIncludedCellValueChangingCommand = new DelegateCommand<object>(NotIncludedCellValueChangingCommandAction);
                #endregion

                AddCommentsCommand = new RelayCommand(new Action<object>(AddCommentsCommandAction));//[Sudhir.Jangra][GEOS2-4935]
                DeleteCommentRowCommand = new RelayCommand(new Action<object>(DeleteCommentRowCommandAction));//[Sudhir.Jangra][GEOS2-4935]
                CommentsGridDoubleClickCommand = new RelayCommand(new Action<object>(CommentDoubleClickCommandAction));//[Sudhir.Jangra][GEOS2-4935]


                IsDuplicateDetectionButtonEnabled = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor EditDetectionViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor EditDetectionViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion

        #region Methods

        private void CancelOptionAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelOptionAction()..."), category: Category.Info, priority: Priority.Low);
                #region GEOS2-3132 Sudhir.Jangra 14/02/2023

                if (IsEnabledCancelButton == true)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AddNewOptionWaysDetectionsSparePartsAction(null);
                    }
                }
                #endregion
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelOptionAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelOptionAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [001] [cpatil][27-09-2021][GEOS2-3340] [Sr N  4 - Synchronization between PCM and ECOS. [#PLM69]]
        /// [002] [cpatil][27-10-2021][GEOS2-3334] [Sr N 5 - Synchronization between PLM and ECOS [PLM#41]]
        private async void AddNewOptionWaysDetectionsSparePartsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewOptionWaysDetectionsSparePartsAction()..."), category: Category.Info, priority: Priority.Low);
                if (!IsAdded)
                {
                    UpdatedItem = new DetectionDetails();
                    ChangeLogList = new ObservableCollection<DetectionLogEntry>();
                    ProductTypeChangeLogList = new ObservableCollection<ProductTypeLogEntry>();
                    #region CaptionWay
                    if (Header == System.Windows.Application.Current.FindResource("CaptionWay").ToString())
                    {
                        UpdatedItem.IdDetections = IdDetections;
                        UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                        if (IsCheckedCopyNameDescription == true)
                        {
                            UpdatedItem.Name = Name;
                            UpdatedItem.Name_es = Name;
                            UpdatedItem.Name_fr = Name;
                            UpdatedItem.Name_pt = Name;
                            UpdatedItem.Name_ro = Name;
                            UpdatedItem.Name_ru = Name;
                            UpdatedItem.Name_zh = Name;

                            UpdatedItem.Description = Description;
                            UpdatedItem.Description_es = Description;
                            UpdatedItem.Description_fr = Description;
                            UpdatedItem.Description_pt = Description;
                            UpdatedItem.Description_ro = Description;
                            UpdatedItem.Description_ru = Description;
                            UpdatedItem.Description_zh = Description == null ? "" : Description;
                        }
                        else
                        {
                            UpdatedItem.Name = Name_en == null ? "" : Name_en;
                            UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                            UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                            UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                            UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                            UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                            UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                            UpdatedItem.Description = Description_en;
                            UpdatedItem.Description_es = Description_es;
                            UpdatedItem.Description_fr = Description_fr;
                            UpdatedItem.Description_pt = Description_pt;
                            UpdatedItem.Description_ro = Description_ro;
                            UpdatedItem.Description_ru = Description_ru;
                            UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                        }

                        UpdatedItem.Code = Code;
                        UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                        UpdatedItem.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);

                        UpdatedItem.IdECOSVisibility = Convert.ToInt32(SelectedECOSVisibility.IdLookupValue);
                        if (UpdatedItem.IdECOSVisibility == 323)//Available
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 1;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 324)//Only Spare Part
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 1;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 325)//Only EMDEP User
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 326)//Read Only
                        {
                            UpdatedItem.PurchaseQtyMax = 0;
                            UpdatedItem.PurchaseQtyMin = 0;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }

                        UpdatedItem.WeldOrder = WeldOrder.Value;
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;

                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        if (SelectedOrder != null)
                        {
                            UpdatedItem.IdGroup = SelectedOrder.IdGroup;
                            UpdatedItem.DetectionOrderGroup = SelectedOrder;
                        }
                        #region ProductType File
                        UpdatedItem.DetectionAttachedDocList = new List<DetectionAttachedDoc>();
                        //Deleted ProductType File
                        foreach (DetectionAttachedDoc item in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (!OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType File
                        foreach (DetectionAttachedDoc item in OptionWayDetectionSparePartFilesList)
                        {
                            if (!ClonedDetections.DetectionAttachedDocList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType File
                        foreach (DetectionAttachedDoc originalCatalogue in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc catalogueAttachedDocUpdated = OptionWayDetectionSparePartFilesList.FirstOrDefault(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc);
                                if ((catalogueAttachedDocUpdated.SavedFileName != originalCatalogue.SavedFileName) || (catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName) ||
                                    (catalogueAttachedDocUpdated.Description != originalCatalogue.Description))
                                {
                                    DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)catalogueAttachedDocUpdated.Clone();
                                    DetectionAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                    if (catalogueAttachedDocUpdated.DetectionAttachedDocInBytes != originalCatalogue.DetectionAttachedDocInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesUpdate").ToString(), originalCatalogue.SavedFileName, catalogueAttachedDocUpdated.SavedFileName) });
                                    if ((catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), "None", catalogueAttachedDocUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, catalogueAttachedDocUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (catalogueAttachedDocUpdated.Description != originalCatalogue.Description)
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), "None", catalogueAttachedDocUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, catalogueAttachedDocUpdated.Description) });
                                        }
                                    }
                                }
                                //[rdixit][GEOS2-4074][12.12.2022]
                                if ((catalogueAttachedDocUpdated.AttachmentType != originalCatalogue.AttachmentType))
                                {
                                    if (originalCatalogue.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry()
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(),
                                            "None", catalogueAttachedDocUpdated.AttachmentType.Value)
                                        });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry()
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(),
                                            catalogueAttachedDocUpdated.AttachmentType.Value, "None")
                                        });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType.IdLookupValue != originalCatalogue.AttachmentType.IdLookupValue)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry()
                                        {
                                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(),
                                            originalCatalogue.AttachmentType.Value, catalogueAttachedDocUpdated.AttachmentType.Value)
                                        });
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType link
                        //Link
                        UpdatedItem.DetectionAttachedLinkList = new List<DetectionAttachedLink>();

                        foreach (DetectionAttachedLink item in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (!OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksDelete").ToString(), item.Name) });
                            }
                        }
                        //Added ProductType link
                        foreach (DetectionAttachedLink item in OptionWayDetectionSparePartLinksList)
                        {
                            if (!ClonedDetections.DetectionAttachedLinkList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksAdd").ToString(), item.Name) });
                            }
                        }
                        //Updated ProductType link
                        foreach (DetectionAttachedLink originalDetection in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLinkUpdated = OptionWayDetectionSparePartLinksList.FirstOrDefault(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink);
                                if ((detectionAttachedLinkUpdated.Name != originalDetection.Name) || (detectionAttachedLinkUpdated.Description != originalDetection.Description))
                                {
                                    DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)detectionAttachedLinkUpdated.Clone();
                                    detectionAttachedLink.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);

                                    if (detectionAttachedLinkUpdated.Address != originalDetection.Address)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Address))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Address))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), "None", detectionAttachedLinkUpdated.Address) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, detectionAttachedLinkUpdated.Address) });
                                        }
                                    }
                                    if ((detectionAttachedLinkUpdated.Name != originalDetection.Name))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Name))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Name))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), "None", detectionAttachedLinkUpdated.Name) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, detectionAttachedLinkUpdated.Name) });
                                        }
                                    }

                                    if (detectionAttachedLinkUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), "None", detectionAttachedLinkUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedLinkUpdated.Description) });
                                        }
                                    }

                                }
                            }
                        }
                        #endregion

                        #region ProductType Image
                        //Image
                        UpdatedItem.DetectionImageList = new List<DetectionImage>();

                        foreach (DetectionImage item in ClonedDetections.DetectionImageList)
                        {
                            if (!OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType Image
                        foreach (DetectionImage item in OptionWayDetectionSparePartImagesList)
                        {
                            if (!ClonedDetections.DetectionImageList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType Image
                        foreach (DetectionImage originalDetection in ClonedDetections.DetectionImageList)
                        {
                            if (OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == originalDetection.IdDetectionImage))
                            {
                                DetectionImage detectionAttachedImageUpdated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.IdDetectionImage == originalDetection.IdDetectionImage);
                                if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName) || (detectionAttachedImageUpdated.SavedFileName != originalDetection.SavedFileName) ||
                                    (detectionAttachedImageUpdated.Description != originalDetection.Description) || (detectionAttachedImageUpdated.Position != originalDetection.Position))
                                {
                                    DetectionImage detectionImage = (DetectionImage)detectionAttachedImageUpdated.Clone();
                                    detectionImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionImageList.Add(detectionImage);
                                    if (detectionAttachedImageUpdated.DetectionImageInBytes != originalDetection.DetectionImageInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesUpdate").ToString(), originalDetection.SavedFileName, detectionAttachedImageUpdated.SavedFileName) });
                                    if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), "None", detectionAttachedImageUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, detectionAttachedImageUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), "None", detectionAttachedImageUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedImageUpdated.Description) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Position != originalDetection.Position)//[rdixit][GEOS2-2694][01.08.2022]
                                    {
                                        if (detectionAttachedImageUpdated.IdDetectionImage != 1)
                                        {
                                            if (detectionAttachedImageUpdated.Position == 1)
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Customer
                        //Customers add and Delete
                        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                        if (UpdatedItem.CustomerListByDetection == null)
                            UpdatedItem.CustomerListByDetection = new List<CPLCustomer>();

                        List<CPLCustomer> tempCustomersList = PCMCustomerList;
                        List<CPLCustomer> clonedCustomerListByCPType = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(UpdatedItem.IdDetections));
                        // Delete Customer
                        foreach (CPLCustomer item in clonedCustomerListByCPType)
                        {
                            if (PCMCustomerList != null && !PCMCustomerList.Any(x => x.IdCustomerPriceListCustomer == item.IdCustomerPriceListCustomer))
                            {
                                CPLCustomer CPLCustomer = (CPLCustomer)item.Clone();
                                CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                ChangeLogList.Add(new DetectionLogEntry()
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
                                    UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                    ChangeLogList.Add(new DetectionLogEntry()
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
                        #endregion

                        #region RelatedModules

                        UpdatedItem.ProductTypesList = new List<ProductTypes>();



                        //Delete RelatedModules
                        foreach (ProductTypes item in ClonedDetections.ProductTypesList)
                        {
                            if (ProductTypesList != null && !ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                            {
                                ProductTypes relatedModule = (ProductTypes)item.Clone();
                                relatedModule.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.ProductTypesList.Add(relatedModule);
                                ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    IdCPType = item.IdCPType,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                                ChangeLogList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                            }
                        }

                        //Added RelatedModules
                        if (ProductTypesList != null)
                        {
                            foreach (ProductTypes item in ProductTypesList)
                            {
                                if (!ClonedDetections.ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                                {
                                    ProductTypes relatedModule = (ProductTypes)item.Clone();
                                    relatedModule.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedItem.ProductTypesList.Add(relatedModule);
                                    ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        IdCPType = item.IdCPType,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                    ChangeLogList.Add(new DetectionLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                }
                            }
                        }


                        #endregion

                        #region GEOS2-3639 old code
                        //UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        //List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        //foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        //{
                        //    if (item.IsChecked == true)
                        //    {
                        //        RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //        if (obj1.IsChecked == false)
                        //        {
                        //            RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        //            UpdatedItem.CustomerList.Add(connectorFamilies);
                        //            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                        //        }
                        //    }
                        //}

                        //foreach (RegionsByCustomer item in tempCustomersList)
                        //{
                        //    RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //    if (obj1.IsChecked == false)
                        //    {
                        //        RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        //        UpdatedItem.CustomerList.Add(connectorFamilies);
                        //        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                        //    }
                        //}
                        #endregion


                        DetectionImage tempDefaultImage = ClonedDetections.DetectionImageList.FirstOrDefault(x => x.Position == 1);
                        DetectionImage tempDefaultImage_updated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdDetectionImage != tempDefaultImage_updated.IdDetectionImage)
                        {
                            if (tempDefaultImage_updated.Position == 1)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.SavedFileName, tempDefaultImage_updated.SavedFileName) });
                        }

                        UpdateTheIncludedAndNotIncludedPriceList(); //[GEOS2-3199]
                        if (SelectedScopeList!=null)
                        {
                            UpdatedItem.IdScope = Convert.ToUInt32(SelectedScopeList.IdLookupValue);
                        }
                        AddDWOSLogDetails();

                        #region GEOS2-4935 [Sudhir.Jangra]
                        UpdatedItem.DetectionCommentsList = new List<DetectionLogEntry>();
                        //Deleted Comments
                        foreach (DetectionLogEntry itemComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (!CommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection) && itemComments.IdLogEntryByDetection != 0)
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                //ChangeLogList.Add(new DetectionLogEntry()
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //                " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentDeleted").ToString(),
                                //               comments.Comments)
                                //});
                            }
                        }

                        //Added Comments
                        foreach (DetectionLogEntry itemComments in CommentsList)
                        {
                            if (!ClonedDetections.DetectionCommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                //ChangeLogList.Add(new DetectionLogEntry()
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //               " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForComment").ToString(),
                                //     comments.Comments)
                                //});

                            }
                        }


                        //Update Comments
                        foreach (DetectionLogEntry originalComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (CommentsList.Any(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry commentsUpdated = CommentsList.FirstOrDefault(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection);
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
                                    DetectionLogEntry comments = (DetectionLogEntry)commentsUpdated.Clone();
                                    comments.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    comments.ModifiedDate = DateTime.Now;
                                    comments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionCommentsList.Add(comments);


                                    //using (var productTypeCommentEntry = new DetectionLogEntry
                                    //{
                                    //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                    //          " " + GeosApplication.Instance.ActiveUser.LastName,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentUpdate").ToString(),
                                    // comments.Comments, originalComments.Comments, comments.Comments)
                                    //})
                                    //{
                                    //    ChangeLogList.Add(productTypeCommentEntry);
                                    //}

                                }
                            }
                        }
                        #endregion

                        UpdatedItem.DetectionCommentsList.ForEach(x => x.People.OwnerImage = null);


                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();

                        UpdatedItem.ProductTypeChangeLogList = ProductTypeChangeLogList;
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);

                        //IsWaySave = PCMService.UpdateDetection_V2330(UpdatedItem);
                        //  IsWaySave = PCMService.UpdateDetection_V2340(UpdatedItem);


                        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
                       // IsWaySave = PCMService.UpdateDetection_V2410(UpdatedItem);
                        //[Sudhir.Jangra][GEOS2-4935]
                        IsWaySave = PCMService.UpdateDetection_V2470(UpdatedItem);

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WayItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);


                        //RequestClose(null, null);
                    }
                    #endregion
                    #region CaptionSpareParts
                    else if (Header == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                    {
                        UpdatedItem = new DetectionDetails();
                        ChangeLogList = new ObservableCollection<DetectionLogEntry>();

                        UpdatedItem.IdDetections = IdDetections;
                        UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        if (IsCheckedCopyNameDescription == true)
                        {
                            UpdatedItem.Name = Name;
                            UpdatedItem.Name_es = Name;
                            UpdatedItem.Name_fr = Name;
                            UpdatedItem.Name_pt = Name;
                            UpdatedItem.Name_ro = Name;
                            UpdatedItem.Name_ru = Name;
                            UpdatedItem.Name_zh = Name;

                            UpdatedItem.Description = Description;
                            UpdatedItem.Description_es = Description;
                            UpdatedItem.Description_fr = Description;
                            UpdatedItem.Description_pt = Description;
                            UpdatedItem.Description_ro = Description;
                            UpdatedItem.Description_ru = Description;
                            UpdatedItem.Description_zh = Description == null ? "" : Description;
                        }
                        else
                        {
                            UpdatedItem.Name = Name_en == null ? "" : Name_en;
                            UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                            UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                            UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                            UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                            UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                            UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                            UpdatedItem.Description = Description_en;
                            UpdatedItem.Description_es = Description_es;
                            UpdatedItem.Description_fr = Description_fr;
                            UpdatedItem.Description_pt = Description_pt;
                            UpdatedItem.Description_ro = Description_ro;
                            UpdatedItem.Description_ru = Description_ru;
                            UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                        }

                        UpdatedItem.Code = Code;
                        UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                        UpdatedItem.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);

                        UpdatedItem.IdECOSVisibility = Convert.ToInt32(SelectedECOSVisibility.IdLookupValue);
                        if (UpdatedItem.IdECOSVisibility == 323)//Available
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 1;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 324)//Only Spare Part
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 1;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 325)//Only EMDEP User
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 326)//Read Only
                        {
                            UpdatedItem.PurchaseQtyMax = 0;
                            UpdatedItem.PurchaseQtyMin = 0;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }

                        UpdatedItem.WeldOrder = WeldOrder.Value;
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;
                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        if (SelectedOrder != null)
                        {
                            UpdatedItem.IdGroup = SelectedOrder.IdGroup;
                            UpdatedItem.DetectionOrderGroup = SelectedOrder;
                        }
                        #region ProductType Files
                        UpdatedItem.DetectionAttachedDocList = new List<DetectionAttachedDoc>();

                        foreach (DetectionAttachedDoc item in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (!OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                            }
                        }

                        //Added ProductType
                        foreach (DetectionAttachedDoc item in OptionWayDetectionSparePartFilesList)
                        {
                            if (!ClonedDetections.DetectionAttachedDocList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType
                        foreach (DetectionAttachedDoc originalCatalogue in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc catalogueAttachedDocUpdated = OptionWayDetectionSparePartFilesList.FirstOrDefault(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc);
                                if ((catalogueAttachedDocUpdated.SavedFileName != originalCatalogue.SavedFileName) || (catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName) || (catalogueAttachedDocUpdated.Description != originalCatalogue.Description))
                                {
                                    DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)catalogueAttachedDocUpdated.Clone();
                                    DetectionAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                    if (catalogueAttachedDocUpdated.DetectionAttachedDocInBytes != originalCatalogue.DetectionAttachedDocInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesUpdate").ToString(), originalCatalogue.SavedFileName, catalogueAttachedDocUpdated.SavedFileName) });
                                    if ((catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), "None", catalogueAttachedDocUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, catalogueAttachedDocUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (catalogueAttachedDocUpdated.Description != originalCatalogue.Description)
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), "None", catalogueAttachedDocUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, catalogueAttachedDocUpdated.Description) });
                                        }
                                    }
                                }
                                //[rdixit][GEOS2-4074][12.12.2022]
                                if ((catalogueAttachedDocUpdated.AttachmentType != originalCatalogue.AttachmentType))
                                {
                                    if (originalCatalogue.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), "None", catalogueAttachedDocUpdated.AttachmentType.Value) });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), catalogueAttachedDocUpdated.AttachmentType.Value, "None") });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType.IdLookupValue != originalCatalogue.AttachmentType.IdLookupValue)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), originalCatalogue.AttachmentType.Value, catalogueAttachedDocUpdated.AttachmentType.Value) });
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType link
                        //Link
                        UpdatedItem.DetectionAttachedLinkList = new List<DetectionAttachedLink>();

                        foreach (DetectionAttachedLink item in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (!OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksDelete").ToString(), item.Name) });
                            }
                        }
                        //Added ProductType link
                        foreach (DetectionAttachedLink item in OptionWayDetectionSparePartLinksList)
                        {
                            if (!ClonedDetections.DetectionAttachedLinkList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksAdd").ToString(), item.Name) });
                            }
                        }
                        //Updated ProductType link
                        foreach (DetectionAttachedLink originalDetection in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLinkUpdated = OptionWayDetectionSparePartLinksList.FirstOrDefault(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink);
                                if ((detectionAttachedLinkUpdated.Name != originalDetection.Name) || (detectionAttachedLinkUpdated.Description != originalDetection.Description))
                                {
                                    DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)detectionAttachedLinkUpdated.Clone();
                                    detectionAttachedLink.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);

                                    if (detectionAttachedLinkUpdated.Address != originalDetection.Address)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Address))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Address))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), "None", detectionAttachedLinkUpdated.Address) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, detectionAttachedLinkUpdated.Address) });
                                        }
                                    }
                                    if ((detectionAttachedLinkUpdated.Name != originalDetection.Name))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Name))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Name))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), "None", detectionAttachedLinkUpdated.Name) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, detectionAttachedLinkUpdated.Name) });
                                        }
                                    }

                                    if (detectionAttachedLinkUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), "None", detectionAttachedLinkUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedLinkUpdated.Description) });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType Image
                        //Image
                        UpdatedItem.DetectionImageList = new List<DetectionImage>();

                        foreach (DetectionImage item in ClonedDetections.DetectionImageList)
                        {
                            if (!OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType Image
                        foreach (DetectionImage item in OptionWayDetectionSparePartImagesList)
                        {
                            if (!ClonedDetections.DetectionImageList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType Image
                        foreach (DetectionImage originalDetection in ClonedDetections.DetectionImageList)
                        {
                            if (OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == originalDetection.IdDetectionImage))
                            {
                                DetectionImage detectionAttachedImageUpdated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.IdDetectionImage == originalDetection.IdDetectionImage);
                                if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName) || (detectionAttachedImageUpdated.SavedFileName != originalDetection.SavedFileName) ||
                                    (detectionAttachedImageUpdated.Description != originalDetection.Description) || (detectionAttachedImageUpdated.Position != originalDetection.Position))
                                {
                                    DetectionImage detectionImage = (DetectionImage)detectionAttachedImageUpdated.Clone();
                                    detectionImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionImageList.Add(detectionImage);
                                    if (detectionAttachedImageUpdated.DetectionImageInBytes != originalDetection.DetectionImageInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesUpdate").ToString(), originalDetection.SavedFileName, detectionAttachedImageUpdated.SavedFileName) });
                                    if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), "None", detectionAttachedImageUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, detectionAttachedImageUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), "None", detectionAttachedImageUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedImageUpdated.Description) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Position != originalDetection.Position)//[rdixit][GEOS2-2694][01.08.2022]
                                    {
                                        if (detectionAttachedImageUpdated.IdDetectionImage != 1)
                                        {
                                            if (detectionAttachedImageUpdated.Position == 1)
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Group
                        UpdatedItem.DetectionGroupList = new List<DetectionGroup>();

                        foreach (DetectionGroup item in ClonedDetections.DetectionGroupList)
                        {
                            if (!GroupList.Any(x => x.IdGroup == item.IdGroup))
                            {
                                DetectionGroup detectionGroup = (DetectionGroup)item.Clone();
                                detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionGroupList.Add(detectionGroup);
                            }
                        }
                        //Added option group
                        foreach (DetectionGroup item in GroupList)
                        {
                            if (!ClonedDetections.DetectionGroupList.Any(x => x.IdGroup == item.IdGroup))
                            {
                                DetectionGroup detectionGroup = (DetectionGroup)item.Clone();
                                detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionGroupList.Add(detectionGroup);
                            }
                        }
                        //Updated option group
                        foreach (DetectionGroup originalDetection in ClonedDetections.DetectionGroupList)
                        {
                            if (GroupList.Any(x => x.IdGroup == originalDetection.IdGroup))
                            {
                                DetectionGroup detectionGroupUpdated = GroupList.FirstOrDefault(x => x.IdGroup == originalDetection.IdGroup);
                                if ((detectionGroupUpdated.Name != originalDetection.Name) || (detectionGroupUpdated.Description != originalDetection.Description) || (detectionGroupUpdated.OrderNumber != originalDetection.OrderNumber))
                                {
                                    DetectionGroup detectionGroup = (DetectionGroup)detectionGroupUpdated.Clone();
                                    detectionGroup.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionGroupList.Add(detectionGroup);
                                }
                            }
                        }

                        #endregion

                        #region Customer
                        //Customers add and Delete
                        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                        if (UpdatedItem.CustomerListByDetection == null)
                            UpdatedItem.CustomerListByDetection = new List<CPLCustomer>();

                        List<CPLCustomer> tempCustomersList = PCMCustomerList;
                        List<CPLCustomer> clonedCustomerListByCPType = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(UpdatedItem.IdDetections));
                        // Delete Customer
                        foreach (CPLCustomer item in clonedCustomerListByCPType)
                        {
                            if (PCMCustomerList != null && !PCMCustomerList.Any(x => x.IdCustomerPriceListCustomer == item.IdCustomerPriceListCustomer))
                            {
                                CPLCustomer CPLCustomer = (CPLCustomer)item.Clone();
                                CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                ChangeLogList.Add(new DetectionLogEntry()
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
                                    UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                    ChangeLogList.Add(new DetectionLogEntry()
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
                        #endregion

                        #region RelatedModules

                        UpdatedItem.ProductTypesList = new List<ProductTypes>();



                        //Delete RelatedModules
                        foreach (ProductTypes item in ClonedDetections.ProductTypesList)
                        {
                            if (ProductTypesList != null && !ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                            {
                                ProductTypes relatedModule = (ProductTypes)item.Clone();
                                relatedModule.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.ProductTypesList.Add(relatedModule);
                                ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    IdCPType = item.IdCPType,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                                ChangeLogList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                            }
                        }

                        //Added RelatedModules
                        if (ProductTypesList != null)
                        {
                            foreach (ProductTypes item in ProductTypesList)
                            {
                                if (!ClonedDetections.ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                                {
                                    ProductTypes relatedModule = (ProductTypes)item.Clone();
                                    relatedModule.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedItem.ProductTypesList.Add(relatedModule);
                                    ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        IdCPType = item.IdCPType,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                    ChangeLogList.Add(new DetectionLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                }
                            }
                        }


                        #endregion

                        #region GEOS2-3639 Old code
                        //UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        //List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        //foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        //{
                        //    if (item.IsChecked == true)
                        //    {
                        //        RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //        if (obj1.IsChecked == false)
                        //        {
                        //            RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        //            UpdatedItem.CustomerList.Add(connectorFamilies);
                        //            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                        //        }
                        //    }
                        //}

                        //foreach (RegionsByCustomer item in tempCustomersList)
                        //{
                        //    RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //    if (obj1.IsChecked == false)
                        //    {
                        //        RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        //        UpdatedItem.CustomerList.Add(connectorFamilies);
                        //        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                        //    }
                        //}
                        #endregion

                        DetectionImage tempDefaultImage = ClonedDetections.DetectionImageList.FirstOrDefault(x => x.Position == 1);
                        DetectionImage tempDefaultImage_updated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdDetectionImage != tempDefaultImage_updated.IdDetectionImage)
                        {
                            if (tempDefaultImage_updated.Position == 1)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                        }

                        UpdateTheIncludedAndNotIncludedPriceList(); //[GEOS2-3199]
                        UpdatedItem.IdScope = Convert.ToUInt64(SelectedScopeList.IdLookupValue);
                        AddDWOSLogDetails();

                        #region GEOS2-4935 [Sudhir.Jangra]
                        UpdatedItem.DetectionCommentsList = new List<DetectionLogEntry>();
                        //Deleted Comments
                        foreach (DetectionLogEntry itemComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (!CommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection) && itemComments.IdLogEntryByDetection != 0)
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                //ChangeLogList.Add(new DetectionLogEntry()
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //                " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentDeleted").ToString(),
                                //               comments.Comments)
                                //});
                            }
                        }

                        //Added Comments
                        foreach (DetectionLogEntry itemComments in CommentsList)
                        {
                            if (!ClonedDetections.DetectionCommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                //ChangeLogList.Add(new DetectionLogEntry()
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //               " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForComment").ToString(),
                                //     comments.Comments)
                                //});

                            }
                        }


                        //Update Comments
                        foreach (DetectionLogEntry originalComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (CommentsList.Any(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry commentsUpdated = CommentsList.FirstOrDefault(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection);
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
                                    DetectionLogEntry comments = (DetectionLogEntry)commentsUpdated.Clone();
                                    comments.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    comments.ModifiedDate = DateTime.Now;
                                    comments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionCommentsList.Add(comments);


                                    //using (var productTypeCommentEntry = new DetectionLogEntry
                                    //{
                                    //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                    //          " " + GeosApplication.Instance.ActiveUser.LastName,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentUpdate").ToString(),
                                    // comments.Comments, originalComments.Comments, comments.Comments)
                                    //})
                                    //{
                                    //    ChangeLogList.Add(productTypeCommentEntry);
                                    //}

                                }
                            }
                        }
                        #endregion

                        UpdatedItem.DetectionCommentsList.ForEach(x => x.People.OwnerImage = null);

                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();
                        UpdatedItem.ProductTypeChangeLogList = ProductTypeChangeLogList;

                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);

                        //IsSparepartSave = PCMService.UpdateDetection_V2330(UpdatedItem);
                        //  IsSparepartSave = PCMService.UpdateDetection_V2340(UpdatedItem);



                        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
                      //  IsSparepartSave = PCMService.UpdateDetection_V2410(UpdatedItem);
                        //[Sudhir.Jangra][GEOS2-4935]
                        IsSparepartSave = PCMService.UpdateDetection_V2470(UpdatedItem);


                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SparePartItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        //   RequestClose(null, null);
                    }
                    #endregion
                    #region CaptionOptions
                    else if (Header == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                    {
                        UpdatedItem.IdDetections = IdDetections;
                        UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                        if (IsCheckedCopyNameDescription == true)
                        {
                            UpdatedItem.Name = Name;
                            UpdatedItem.Name_es = Name;
                            UpdatedItem.Name_fr = Name;
                            UpdatedItem.Name_pt = Name;
                            UpdatedItem.Name_ro = Name;
                            UpdatedItem.Name_ru = Name;
                            UpdatedItem.Name_zh = Name;

                            UpdatedItem.Description = Description;
                            UpdatedItem.Description_es = Description;
                            UpdatedItem.Description_fr = Description;
                            UpdatedItem.Description_pt = Description;
                            UpdatedItem.Description_ro = Description;
                            UpdatedItem.Description_ru = Description;
                            UpdatedItem.Description_zh = Description == null ? "" : Description;
                        }
                        else
                        {
                            UpdatedItem.Name = Name_en == null ? "" : Name_en;
                            UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                            UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                            UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                            UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                            UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                            UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                            UpdatedItem.Description = Description_en;
                            UpdatedItem.Description_es = Description_es;
                            UpdatedItem.Description_fr = Description_fr;
                            UpdatedItem.Description_pt = Description_pt;
                            UpdatedItem.Description_ro = Description_ro;
                            UpdatedItem.Description_ru = Description_ru;
                            UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                        }

                        UpdatedItem.Code = Code;
                        UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                        UpdatedItem.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);

                        UpdatedItem.IdECOSVisibility = Convert.ToInt32(SelectedECOSVisibility.IdLookupValue);
                        if (UpdatedItem.IdECOSVisibility == 323)//Available
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 1;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 324)//Only Spare Part
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 1;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 325)//Only EMDEP User
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 326)//Read Only
                        {
                            UpdatedItem.PurchaseQtyMax = 0;
                            UpdatedItem.PurchaseQtyMin = 0;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }


                        UpdatedItem.WeldOrder = WeldOrder.Value;
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;
                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                        if (SelectedOrder != null)
                        {
                            UpdatedItem.IdGroup = SelectedOrder.IdGroup;
                            UpdatedItem.DetectionOrderGroup = SelectedOrder;
                        }
                        #region ProductType file
                        //Files
                        UpdatedItem.DetectionAttachedDocList = new List<DetectionAttachedDoc>();
                        //Deleted ProductType file
                        foreach (DetectionAttachedDoc item in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (!OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType file
                        foreach (DetectionAttachedDoc item in OptionWayDetectionSparePartFilesList)
                        {
                            if (!ClonedDetections.DetectionAttachedDocList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType file
                        foreach (DetectionAttachedDoc originalCatalogue in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc catalogueAttachedDocUpdated = OptionWayDetectionSparePartFilesList.FirstOrDefault(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc);
                                if ((catalogueAttachedDocUpdated.SavedFileName != originalCatalogue.SavedFileName) || (catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName) || (catalogueAttachedDocUpdated.Description != originalCatalogue.Description))
                                {
                                    DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)catalogueAttachedDocUpdated.Clone();
                                    DetectionAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                    if (catalogueAttachedDocUpdated.DetectionAttachedDocInBytes != originalCatalogue.DetectionAttachedDocInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesUpdate").ToString(), originalCatalogue.SavedFileName, catalogueAttachedDocUpdated.SavedFileName) });
                                    if ((catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), "None", catalogueAttachedDocUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, catalogueAttachedDocUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (catalogueAttachedDocUpdated.Description != originalCatalogue.Description)
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), "None", catalogueAttachedDocUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, catalogueAttachedDocUpdated.Description) });
                                        }
                                    }
                                }
                                //[rdixit][GEOS2-4074][12.12.2022]
                                if ((catalogueAttachedDocUpdated.AttachmentType != originalCatalogue.AttachmentType))
                                {
                                    if (originalCatalogue.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), "None", catalogueAttachedDocUpdated.AttachmentType.Value) });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), catalogueAttachedDocUpdated.AttachmentType.Value, "None") });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType.IdLookupValue != originalCatalogue.AttachmentType.IdLookupValue)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), originalCatalogue.AttachmentType.Value, catalogueAttachedDocUpdated.AttachmentType.Value) });
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType link
                        //Link
                        UpdatedItem.DetectionAttachedLinkList = new List<DetectionAttachedLink>();

                        foreach (DetectionAttachedLink item in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (!OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksDelete").ToString(), item.Name) });
                            }
                        }
                        //Added ProductType link
                        foreach (DetectionAttachedLink item in OptionWayDetectionSparePartLinksList)
                        {
                            if (!ClonedDetections.DetectionAttachedLinkList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksAdd").ToString(), item.Name) });
                            }
                        }
                        //Updated ProductType link
                        foreach (DetectionAttachedLink originalDetection in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLinkUpdated = OptionWayDetectionSparePartLinksList.FirstOrDefault(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink);
                                if ((detectionAttachedLinkUpdated.Name != originalDetection.Name) || (detectionAttachedLinkUpdated.Description != originalDetection.Description))
                                {
                                    DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)detectionAttachedLinkUpdated.Clone();
                                    detectionAttachedLink.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);

                                    if (detectionAttachedLinkUpdated.Address != originalDetection.Address)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Address))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Address))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), "None", detectionAttachedLinkUpdated.Address) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, detectionAttachedLinkUpdated.Address) });
                                        }
                                    }
                                    if ((detectionAttachedLinkUpdated.Name != originalDetection.Name))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Name))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Name))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), "None", detectionAttachedLinkUpdated.Name) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, detectionAttachedLinkUpdated.Name) });
                                        }
                                    }

                                    if (detectionAttachedLinkUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), "None", detectionAttachedLinkUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedLinkUpdated.Description) });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType Image
                        //Image
                        UpdatedItem.DetectionImageList = new List<DetectionImage>();

                        foreach (DetectionImage item in ClonedDetections.DetectionImageList)
                        {
                            if (!OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType Image
                        foreach (DetectionImage item in OptionWayDetectionSparePartImagesList)
                        {
                            if (!ClonedDetections.DetectionImageList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType Image
                        foreach (DetectionImage originalDetection in ClonedDetections.DetectionImageList)
                        {
                            if (OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == originalDetection.IdDetectionImage))
                            {
                                DetectionImage detectionAttachedImageUpdated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.IdDetectionImage == originalDetection.IdDetectionImage);
                                if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName) ||
                                    (detectionAttachedImageUpdated.SavedFileName != originalDetection.SavedFileName) ||
                                    (detectionAttachedImageUpdated.Description != originalDetection.Description) ||
                                    (detectionAttachedImageUpdated.Position != originalDetection.Position))
                                {
                                    DetectionImage detectionImage = (DetectionImage)detectionAttachedImageUpdated.Clone();
                                    detectionImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionImageList.Add(detectionImage);
                                    if (detectionAttachedImageUpdated.DetectionImageInBytes != originalDetection.DetectionImageInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesUpdate").ToString(), originalDetection.SavedFileName, detectionAttachedImageUpdated.SavedFileName) });
                                    if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), "None", detectionAttachedImageUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, detectionAttachedImageUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), "None", detectionAttachedImageUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedImageUpdated.Description) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Position != originalDetection.Position)//[rdixit][GEOS2-2694][01.08.2022]
                                    {
                                        if (detectionAttachedImageUpdated.IdDetectionImage != 1)
                                        {
                                            if (detectionAttachedImageUpdated.Position == 1)
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                        }
                                    }
                                }
                            }
                        }
                        DetectionImage tempDefaultImage = ClonedDetections.DetectionImageList.FirstOrDefault(x => x.Position == 1);
                        DetectionImage tempDefaultImage_updated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdDetectionImage != tempDefaultImage_updated.IdDetectionImage)
                        {
                            if (tempDefaultImage_updated.Position == 1)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                        }
                        #endregion

                        #region ProductType Group
                        //Group
                        UpdatedItem.DetectionGroupList = new List<DetectionGroup>();

                        foreach (DetectionGroup item in ClonedDetections.DetectionGroupList)
                        {
                            if (!GroupList.Any(x => x.IdGroup == item.IdGroup))
                            {
                                DetectionGroup detectionGroup = (DetectionGroup)item.Clone();
                                detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionGroupList.Add(detectionGroup);
                            }
                        }
                        //Added option group
                        foreach (DetectionGroup item in GroupList)
                        {
                            if (!ClonedDetections.DetectionGroupList.Any(x => x.IdGroup == item.IdGroup))
                            {
                                DetectionGroup detectionGroup = (DetectionGroup)item.Clone();
                                detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionGroupList.Add(detectionGroup);
                            }
                        }
                        //Updated option group
                        foreach (DetectionGroup originalDetection in ClonedDetections.DetectionGroupList)
                        {
                            if (GroupList.Any(x => x.IdGroup == originalDetection.IdGroup))
                            {
                                DetectionGroup detectionGroupUpdated = GroupList.FirstOrDefault(x => x.IdGroup == originalDetection.IdGroup);
                                if ((detectionGroupUpdated.Name != originalDetection.Name) || (detectionGroupUpdated.Description != originalDetection.Description) || (detectionGroupUpdated.OrderNumber != originalDetection.OrderNumber))
                                {
                                    DetectionGroup detectionGroup = (DetectionGroup)detectionGroupUpdated.Clone();
                                    detectionGroup.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionGroupList.Add(detectionGroup);
                                }
                            }
                        }

                        #endregion

                        #region ProductType Customer
                        //Customers add and Delete
                        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                        if (UpdatedItem.CustomerListByDetection == null)
                            UpdatedItem.CustomerListByDetection = new List<CPLCustomer>();

                        List<CPLCustomer> tempCustomersList = PCMCustomerList;
                        List<CPLCustomer> clonedCustomerListByCPType = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(UpdatedItem.IdDetections));
                        // Delete Customer
                        foreach (CPLCustomer item in clonedCustomerListByCPType)
                        {
                            if (PCMCustomerList != null && !PCMCustomerList.Any(x => x.IdCustomerPriceListCustomer == item.IdCustomerPriceListCustomer))
                            {
                                CPLCustomer CPLCustomer = (CPLCustomer)item.Clone();
                                CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                ChangeLogList.Add(new DetectionLogEntry()
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
                                    UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                    ChangeLogList.Add(new DetectionLogEntry()
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

                        #endregion

                        #region RelatedModules

                        UpdatedItem.ProductTypesList = new List<ProductTypes>();



                        //Delete RelatedModules
                        foreach (ProductTypes item in ClonedDetections.ProductTypesList)
                        {
                            if (ProductTypesList != null && !ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                            {
                                ProductTypes relatedModule = (ProductTypes)item.Clone();
                                relatedModule.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.ProductTypesList.Add(relatedModule);
                                ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    IdCPType = item.IdCPType,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                                ChangeLogList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                            }
                        }

                        //Added RelatedModules
                        if (ProductTypesList != null)
                        {
                            foreach (ProductTypes item in ProductTypesList)
                            {
                                if (!ClonedDetections.ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                                {
                                    ProductTypes relatedModule = (ProductTypes)item.Clone();
                                    relatedModule.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedItem.ProductTypesList.Add(relatedModule);
                                    ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        IdCPType = item.IdCPType,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                    ChangeLogList.Add(new DetectionLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });

                                }
                            }
                        }


                        #endregion

                        #region oldcode
                        //UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        //List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        //foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        //{
                        //    if (item.IsChecked == true)
                        //    {
                        //        RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //        if (obj1.IsChecked == false)
                        //        {
                        //            RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        //            UpdatedItem.CustomerList.Add(connectorFamilies);
                        //            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                        //        }
                        //    }
                        //}

                        //foreach (RegionsByCustomer item in tempCustomersList)
                        //{
                        //    RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //    if (obj1.IsChecked == false)
                        //    {
                        //        RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        //        UpdatedItem.CustomerList.Add(connectorFamilies);
                        //        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                        //    }
                        //}
                        #endregion


                        UpdateTheIncludedAndNotIncludedPriceList(); //[GEOS2-3199]
                        UpdatedItem.IdScope = Convert.ToUInt64(SelectedScopeList.IdLookupValue);
                        AddDWOSLogDetails();

                        #region GEOS2-4935 [Sudhir.Jangra]
                        UpdatedItem.DetectionCommentsList = new List<DetectionLogEntry>();
                        //Deleted Comments
                        foreach (DetectionLogEntry itemComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (!CommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection) && itemComments.IdLogEntryByDetection != 0)
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                    //ChangeLogList.Add(new DetectionLogEntry()
                                    //{
                                    //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                    //                " " + GeosApplication.Instance.ActiveUser.LastName,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentDeleted").ToString(),
                                    //               comments.Comments)
                                    //});
                            }
                        }

                        //Added Comments
                        foreach (DetectionLogEntry itemComments in CommentsList)
                        {
                            if (!ClonedDetections.DetectionCommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                    //ChangeLogList.Add(new DetectionLogEntry()
                                    //{
                                    //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                    //               " " + GeosApplication.Instance.ActiveUser.LastName,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForComment").ToString(),
                                    //     comments.Comments)
                                    //});

                            }
                        }


                        //Update Comments
                        foreach (DetectionLogEntry originalComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (CommentsList.Any(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry commentsUpdated = CommentsList.FirstOrDefault(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection);
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
                                    DetectionLogEntry comments = (DetectionLogEntry)commentsUpdated.Clone();
                                    comments.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    comments.ModifiedDate = DateTime.Now;
                                    comments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionCommentsList.Add(comments);

                                   
                                        //using (var productTypeCommentEntry = new DetectionLogEntry
                                        //{
                                        //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        //    Datetime = GeosApplication.Instance.ServerDateTime,
                                        //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                        //          " " + GeosApplication.Instance.ActiveUser.LastName,
                                        //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentUpdate").ToString(),
                                        // comments.Comments, originalComments.Comments, comments.Comments)
                                        //})
                                        //{
                                        //    ChangeLogList.Add(productTypeCommentEntry);
                                        //}
                                   
                                }
                            }
                        }
                        #endregion

                        UpdatedItem.DetectionCommentsList.ForEach(x => x.People.OwnerImage = null);
                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();

                        UpdatedItem.ProductTypeChangeLogList = ProductTypeChangeLogList;
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);


                        // IsOptionSave = PCMService.UpdateDetection_V2330(UpdatedItem);
                        //  IsOptionSave = PCMService.UpdateDetection_V2340(UpdatedItem);
                        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
                       // IsOptionSave = PCMService.UpdateDetection_V2410(UpdatedItem);
                        //[Sudhir.jangra][GEOS2-4935]
                        IsOptionSave = PCMService.UpdateDetection_V2470(UpdatedItem);


                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OptionItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);


                        //  RequestClose(null, null);
                    }
                    #endregion
                    #region CaptionDetections
                    else if (Header == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                    {
                        UpdatedItem.IdDetections = IdDetections;
                        UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                        if (IsCheckedCopyNameDescription == true)
                        {
                            UpdatedItem.Name = Name;
                            UpdatedItem.Name_es = Name;
                            UpdatedItem.Name_fr = Name;
                            UpdatedItem.Name_pt = Name;
                            UpdatedItem.Name_ro = Name;
                            UpdatedItem.Name_ru = Name;
                            UpdatedItem.Name_zh = Name;

                            UpdatedItem.Description = Description;
                            UpdatedItem.Description_es = Description;
                            UpdatedItem.Description_fr = Description;
                            UpdatedItem.Description_pt = Description;
                            UpdatedItem.Description_ro = Description;
                            UpdatedItem.Description_ru = Description;
                            UpdatedItem.Description_zh = Description == null ? "" : Description;
                        }
                        else
                        {
                            UpdatedItem.Name = Name_en == null ? "" : Name_en;
                            UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                            UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                            UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                            UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                            UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                            UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                            UpdatedItem.Description = Description_en;
                            UpdatedItem.Description_es = Description_es;
                            UpdatedItem.Description_fr = Description_fr;
                            UpdatedItem.Description_pt = Description_pt;
                            UpdatedItem.Description_ro = Description_ro;
                            UpdatedItem.Description_ru = Description_ru;
                            UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                        }

                        UpdatedItem.Code = Code;
                        if (SelectedTestType!=null)
                        {
                            UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                        }
                        if (SelectedStatus!=null)
                        {
                            UpdatedItem.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);
                        }
                      

                        UpdatedItem.IdECOSVisibility = Convert.ToInt32(SelectedECOSVisibility.IdLookupValue);
                        if (UpdatedItem.IdECOSVisibility == 323)//Available
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 1;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 324)//Only Spare Part
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 1;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 325)//Only EMDEP User
                        {
                            UpdatedItem.PurchaseQtyMax = 1;
                            UpdatedItem.PurchaseQtyMin = 1;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        else if (UpdatedItem.IdECOSVisibility == 326)//Read Only
                        {
                            UpdatedItem.PurchaseQtyMax = 0;
                            UpdatedItem.PurchaseQtyMin = 0;
                            UpdatedItem.IsShareWithCustomer = 0;
                            UpdatedItem.IsSparePartOnly = 0;
                        }
                        if (WeldOrder!=null)
                        {
                            UpdatedItem.WeldOrder = WeldOrder.Value;
                        }
                      
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        if (SelectedTest!=null)
                        {
                            UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;
                        }
                        
                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        // UpdatedItem.WeldOrder = WeldOrder;

                        if (SelectedOrder != null)
                        {
                            UpdatedItem.IdGroup = SelectedOrder.IdGroup;
                            UpdatedItem.DetectionOrderGroup = SelectedOrder;
                        }
                        #region ProductType Files
                        UpdatedItem.DetectionAttachedDocList = new List<DetectionAttachedDoc>();
                        //Deleted Files
                        foreach (DetectionAttachedDoc item in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (!OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType
                        foreach (DetectionAttachedDoc item in OptionWayDetectionSparePartFilesList)
                        {
                            if (!ClonedDetections.DetectionAttachedDocList.Any(x => x.IdDetectionAttachedDoc == item.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)item.Clone();
                                DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType
                        foreach (DetectionAttachedDoc originalCatalogue in ClonedDetections.DetectionAttachedDocList)
                        {
                            if (OptionWayDetectionSparePartFilesList.Any(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc))
                            {
                                DetectionAttachedDoc catalogueAttachedDocUpdated = OptionWayDetectionSparePartFilesList.FirstOrDefault(x => x.IdDetectionAttachedDoc == originalCatalogue.IdDetectionAttachedDoc);
                                if ((catalogueAttachedDocUpdated.SavedFileName != originalCatalogue.SavedFileName) || (catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName) || (catalogueAttachedDocUpdated.Description != originalCatalogue.Description))
                                {
                                    DetectionAttachedDoc DetectionAttachedDoc = (DetectionAttachedDoc)catalogueAttachedDocUpdated.Clone();
                                    DetectionAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    DetectionAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedDocList.Add(DetectionAttachedDoc);
                                    if (catalogueAttachedDocUpdated.DetectionAttachedDocInBytes != originalCatalogue.DetectionAttachedDocInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesUpdate").ToString(), originalCatalogue.SavedFileName, catalogueAttachedDocUpdated.SavedFileName) });
                                    if ((catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), "None", catalogueAttachedDocUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), originalCatalogue.OriginalFileName, catalogueAttachedDocUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (catalogueAttachedDocUpdated.Description != originalCatalogue.Description)
                                    {
                                        if (string.IsNullOrEmpty(catalogueAttachedDocUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalCatalogue.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), "None", catalogueAttachedDocUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), originalCatalogue.Description, catalogueAttachedDocUpdated.Description) });
                                        }
                                    }
                                }
                                //[rdixit][GEOS2-4074][12.12.2022]
                                if ((catalogueAttachedDocUpdated.AttachmentType != originalCatalogue.AttachmentType))
                                {
                                    if (originalCatalogue.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), "None", catalogueAttachedDocUpdated.AttachmentType.Value) });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType == null)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), catalogueAttachedDocUpdated.AttachmentType.Value, "None") });
                                    }
                                    else if (catalogueAttachedDocUpdated.AttachmentType.IdLookupValue != originalCatalogue.AttachmentType.IdLookupValue)
                                    {
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), originalCatalogue.AttachmentType.Value, catalogueAttachedDocUpdated.AttachmentType.Value) });
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType Link
                        //Link
                        UpdatedItem.DetectionAttachedLinkList = new List<DetectionAttachedLink>();

                        foreach (DetectionAttachedLink item in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (!OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksDelete").ToString(), item.Name) });
                            }
                        }
                        //Added ProductType link
                        foreach (DetectionAttachedLink item in OptionWayDetectionSparePartLinksList)
                        {
                            if (!ClonedDetections.DetectionAttachedLinkList.Any(x => x.IdDetectionAttachedLink == item.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)item.Clone();
                                detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinksAdd").ToString(), item.Name) });
                            }
                        }
                        //Updated ProductType link
                        foreach (DetectionAttachedLink originalDetection in ClonedDetections.DetectionAttachedLinkList)
                        {
                            if (OptionWayDetectionSparePartLinksList.Any(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink))
                            {
                                DetectionAttachedLink detectionAttachedLinkUpdated = OptionWayDetectionSparePartLinksList.FirstOrDefault(x => x.IdDetectionAttachedLink == originalDetection.IdDetectionAttachedLink);
                                if ((detectionAttachedLinkUpdated.Name != originalDetection.Name) || (detectionAttachedLinkUpdated.Description != originalDetection.Description))
                                {
                                    DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)detectionAttachedLinkUpdated.Clone();
                                    detectionAttachedLink.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionAttachedLink.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionAttachedLinkList.Add(detectionAttachedLink);

                                    if (detectionAttachedLinkUpdated.Address != originalDetection.Address)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Address))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Address))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), "None", detectionAttachedLinkUpdated.Address) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkURLUpdate").ToString(), originalDetection.Address, detectionAttachedLinkUpdated.Address) });
                                        }
                                    }
                                    if ((detectionAttachedLinkUpdated.Name != originalDetection.Name))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Name))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Name))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), "None", detectionAttachedLinkUpdated.Name) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkNameUpdate").ToString(), originalDetection.Name, detectionAttachedLinkUpdated.Name) });
                                        }
                                    }

                                    if (detectionAttachedLinkUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedLinkUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), "None", detectionAttachedLinkUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogLinkDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedLinkUpdated.Description) });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ProductType Image
                        //Image
                        UpdatedItem.DetectionImageList = new List<DetectionImage>();

                        foreach (DetectionImage item in ClonedDetections.DetectionImageList)
                        {
                            if (!OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesDelete").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesDelete").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Added ProductType Image
                        foreach (DetectionImage item in OptionWayDetectionSparePartImagesList)
                        {
                            if (!ClonedDetections.DetectionImageList.Any(x => x.IdDetectionImage == item.IdDetectionImage))
                            {
                                DetectionImage detectionImage = (DetectionImage)item.Clone();
                                detectionImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionImageList.Add(detectionImage);

                                if (detectionImage.Position == 1)
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesAdd").ToString(), item.OriginalFileName) });
                                else
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesAdd").ToString(), item.OriginalFileName) });
                            }
                        }
                        //Updated ProductType Image
                        foreach (DetectionImage originalDetection in ClonedDetections.DetectionImageList)
                        {
                            if (OptionWayDetectionSparePartImagesList.Any(x => x.IdDetectionImage == originalDetection.IdDetectionImage))
                            {
                                DetectionImage detectionAttachedImageUpdated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.IdDetectionImage == originalDetection.IdDetectionImage);
                                if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName) || (detectionAttachedImageUpdated.SavedFileName != originalDetection.SavedFileName) ||
                                    (detectionAttachedImageUpdated.Description != originalDetection.Description) || (detectionAttachedImageUpdated.Position != originalDetection.Position))
                                {
                                    DetectionImage detectionImage = (DetectionImage)detectionAttachedImageUpdated.Clone();
                                    detectionImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionImageList.Add(detectionImage);
                                    if (detectionAttachedImageUpdated.DetectionImageInBytes != originalDetection.DetectionImageInBytes)
                                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImagesUpdate").ToString(), originalDetection.SavedFileName, detectionAttachedImageUpdated.SavedFileName) });
                                    if ((detectionAttachedImageUpdated.OriginalFileName != originalDetection.OriginalFileName))
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.OriginalFileName))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.OriginalFileName))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), "None", detectionAttachedImageUpdated.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageNameUpdate").ToString(), originalDetection.OriginalFileName, detectionAttachedImageUpdated.OriginalFileName) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Description != originalDetection.Description)
                                    {
                                        if (string.IsNullOrEmpty(detectionAttachedImageUpdated.Description))
                                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, "None") });
                                        else
                                        {
                                            if (string.IsNullOrEmpty(originalDetection.Description))
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), "None", detectionAttachedImageUpdated.Description) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogImageDescriptionUpdate").ToString(), originalDetection.Description, detectionAttachedImageUpdated.Description) });
                                        }
                                    }
                                    if (detectionAttachedImageUpdated.Position != originalDetection.Position)//[rdixit][GEOS2-2694][01.08.2022]
                                    {
                                        if (detectionAttachedImageUpdated.IdDetectionImage != 1)
                                        {
                                            if (detectionAttachedImageUpdated.Position == 1)
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("OldDefaultImagePositionChangeLogUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                            else
                                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagePositionUpdate").ToString(), originalDetection.Position, detectionAttachedImageUpdated.Position, originalDetection.OriginalFileName) });
                                        }
                                    }
                                }
                            }
                        }


                        DetectionImage tempDefaultImage = ClonedDetections.DetectionImageList.FirstOrDefault(x => x.Position == 1);
                        DetectionImage tempDefaultImage_updated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdDetectionImage != tempDefaultImage_updated.IdDetectionImage)
                        {
                            if (tempDefaultImage_updated.Position == 1)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                        }
                        #endregion

                        #region ProductType Group
                        //Group
                        UpdatedItem.DetectionGroupList = new List<DetectionGroup>();

                        foreach (DetectionGroup item in ClonedDetections.DetectionGroupList)
                        {
                            if (!GroupList.Any(x => x.IdGroup == item.IdGroup))
                            {
                                DetectionGroup detectionGroup = (DetectionGroup)item.Clone();
                                detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionGroupList.Add(detectionGroup);
                            }
                        }
                        //Added option group
                        foreach (DetectionGroup item in GroupList)
                        {
                            if (!ClonedDetections.DetectionGroupList.Any(x => x.IdGroup == item.IdGroup))
                            {
                                DetectionGroup detectionGroup = (DetectionGroup)item.Clone();
                                detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionGroupList.Add(detectionGroup);
                            }
                        }
                        //Updated option group
                        foreach (DetectionGroup originalDetection in ClonedDetections.DetectionGroupList)
                        {
                            if (GroupList.Any(x => x.IdGroup == originalDetection.IdGroup))
                            {
                                DetectionGroup detectionGroupUpdated = GroupList.FirstOrDefault(x => x.IdGroup == originalDetection.IdGroup);
                                if ((detectionGroupUpdated.Name != originalDetection.Name) || (detectionGroupUpdated.Description != originalDetection.Description) || (detectionGroupUpdated.OrderNumber != originalDetection.OrderNumber))
                                {
                                    DetectionGroup detectionGroup = (DetectionGroup)detectionGroupUpdated.Clone();
                                    detectionGroup.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    detectionGroup.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionGroupList.Add(detectionGroup);
                                }
                            }
                        }

                        #endregion

                        #region ProductType Customers
                        //Customers add and Delete
                        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                        if (UpdatedItem.CustomerListByDetection == null)
                            UpdatedItem.CustomerListByDetection = new List<CPLCustomer>();

                        List<CPLCustomer> tempCustomersList = PCMCustomerList;
                        List<CPLCustomer> clonedCustomerListByCPType = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(UpdatedItem.IdDetections));
                        // Delete Customer
                        foreach (CPLCustomer item in clonedCustomerListByCPType)
                        {
                            if (PCMCustomerList != null && !PCMCustomerList.Any(x => x.IdCustomerPriceListCustomer == item.IdCustomerPriceListCustomer))
                            {
                                CPLCustomer CPLCustomer = (CPLCustomer)item.Clone();
                                CPLCustomer.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                ChangeLogList.Add(new DetectionLogEntry()
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
                                    UpdatedItem.CustomerListByDetection.Add(CPLCustomer);
                                    ChangeLogList.Add(new DetectionLogEntry()
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
                        #endregion

                        #region RelatedModules

                        UpdatedItem.ProductTypesList = new List<ProductTypes>();



                        //Delete RelatedModules
                        foreach (ProductTypes item in ClonedDetections.ProductTypesList)
                        {
                            if (ProductTypesList != null && !ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                            {
                                ProductTypes relatedModule = (ProductTypes)item.Clone();
                                relatedModule.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.ProductTypesList.Add(relatedModule);
                                ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    IdCPType = item.IdCPType,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                                ChangeLogList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModuleDelete").ToString(), item.Code)
                                });
                            }
                        }

                        //Added RelatedModules
                        if (ProductTypesList != null)
                        {
                            foreach (ProductTypes item in ProductTypesList)
                            {
                                if (!ClonedDetections.ProductTypesList.Any(x => x.IdCPType == item.IdCPType))
                                {
                                    ProductTypes relatedModule = (ProductTypes)item.Clone();
                                    relatedModule.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdatedItem.ProductTypesList.Add(relatedModule);
                                    ProductTypeChangeLogList.Add(new ProductTypeLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        IdCPType = item.IdCPType,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                    ChangeLogList.Add(new DetectionLogEntry()
                                    {
                                        IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelatedModulesAdd").ToString(),
                                        item.Code)
                                    });
                                }
                            }
                        }


                        #endregion

                        #region GEOS2-3276 old code
                        //UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        //List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        //foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        //{
                        //    if (item.IsChecked == true)
                        //    {
                        //        RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //        if (obj1.IsChecked == false)
                        //        {
                        //            RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //            connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        //            UpdatedItem.CustomerList.Add(connectorFamilies);
                        //            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                        //        }
                        //    }
                        //}

                        //foreach (RegionsByCustomer item in tempCustomersList)
                        //{
                        //    RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                        //    if (obj1.IsChecked == false)
                        //    {
                        //        RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                        //        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        //        UpdatedItem.CustomerList.Add(connectorFamilies);
                        //        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });
                        //    }
                        //}
                        #endregion


                        UpdateTheIncludedAndNotIncludedPriceList(); //[GEOS2-3199]
                        UpdatedItem.IdScope = Convert.ToUInt64(SelectedScopeList.IdLookupValue);
                        AddDWOSLogDetails();

                        #region GEOS2-4935 [Sudhir.Jangra]
                        UpdatedItem.DetectionCommentsList = new List<DetectionLogEntry>();
                        //Deleted Comments
                        foreach (DetectionLogEntry itemComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (!CommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection) && itemComments.IdLogEntryByDetection != 0)
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                //ChangeLogList.Add(new DetectionLogEntry()
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //                " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentDeleted").ToString(),
                                //               comments.Comments)
                                //});
                            }
                        }

                        //Added Comments
                        foreach (DetectionLogEntry itemComments in CommentsList)
                        {
                            if (!ClonedDetections.DetectionCommentsList.Any(x => x.IdLogEntryByDetection == itemComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry comments = (DetectionLogEntry)itemComments.Clone();
                                comments.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.DetectionCommentsList.Add(comments);
                                //ChangeLogList.Add(new DetectionLogEntry()
                                //{
                                //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                //    Datetime = GeosApplication.Instance.ServerDateTime,
                                //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                //               " " + GeosApplication.Instance.ActiveUser.LastName,
                                //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForComment").ToString(),
                                //     comments.Comments)
                                //});

                            }
                        }


                        //Update Comments
                        foreach (DetectionLogEntry originalComments in ClonedDetections.DetectionCommentsList)
                        {
                            if (CommentsList.Any(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection))
                            {
                                DetectionLogEntry commentsUpdated = CommentsList.FirstOrDefault(x => x.IdLogEntryByDetection == originalComments.IdLogEntryByDetection);
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
                                    DetectionLogEntry comments = (DetectionLogEntry)commentsUpdated.Clone();
                                    comments.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    comments.ModifiedDate = DateTime.Now;
                                    comments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedItem.DetectionCommentsList.Add(comments);


                                    //using (var productTypeCommentEntry = new DetectionLogEntry
                                    //{
                                    //    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    UserName = GeosApplication.Instance.ActiveUser.FirstName +
                                    //          " " + GeosApplication.Instance.ActiveUser.LastName,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("DetectionChangeLogForCommentUpdate").ToString(),
                                    // comments.Comments, originalComments.Comments, comments.Comments)
                                    //})
                                    //{
                                    //    ChangeLogList.Add(productTypeCommentEntry);
                                    //}

                                }
                            }
                        }
                        #endregion

                        UpdatedItem.DetectionCommentsList.ForEach(x => x.People.OwnerImage = null);


                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();
                        UpdatedItem.ProductTypeChangeLogList = ProductTypeChangeLogList;
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);

                        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
                        //IsDetectionSave = PCMService.UpdateDetection_V2330(UpdatedItem);




                        //  IsDetectionSave = PCMService.UpdateDetection_V2340(UpdatedItem);


                        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
                       // IsDetectionSave = PCMService.UpdateDetection_V2410(UpdatedItem);
                        //[Sudhir.Jangra][GEOS2-4935]
                        IsDetectionSave = PCMService.UpdateDetection_V2470(UpdatedItem);


                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        // RequestClose(null, null);
                    }
                    #endregion


                    IsAdded = true;
                    IsAcceptButtonEnabled = false;

                    //[001] Added code for synchronization
                    if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                    {
                        GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("58,59,60");
                        if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                        {
                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 58) && ((UpdatedItem.ModifiedPLMDetectionList.Any(i => i.IdStatus == 223)) ? GeosAppSettingList.Any(i => i.IdAppSetting == 60) : true))
                            {
                                if ((!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) && (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue))))    //(!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                                {
                                    #region Synchronization Code
                                    CancelSync:;
                                    var ownerInfo = (obj as FrameworkElement);
                                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                    if (MessageBoxResult == MessageBoxResult.Yes)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = "Synchronization is running";

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
                                            List<ErrorDetails> LstErrorDetail = new List<ErrorDetails>();
                                            tokenService = new AuthTokenService();
                                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                            {
                                                string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                                if (tokeninformations.Count() >= 2)
                                                {
                                                    if (UpdatedItem.ModifiedPLMDetectionList.Any(i => i.IdStatus == 223))
                                                    {
                                                        //[rdixit][22.02.2023][GEOS2-4176]
                                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                                        PCMArticleSynchronizationViewModel PCMArticleSynchronizationViewModel = new PCMArticleSynchronizationViewModel();
                                                        PCMArticleSynchronizationView PCMArticleSynchronizationView = new PCMArticleSynchronizationView();
                                                        EventHandler handle = delegate { PCMArticleSynchronizationView.Close(); };
                                                        PCMArticleSynchronizationViewModel.RequestClose += handle;
                                                        PCMArticleSynchronizationView.DataContext = PCMArticleSynchronizationViewModel;
                                                        PCMArticleSynchronizationViewModel.DetectionInit(IncludedPLMDetectionPriceList, UpdatedItem);
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

                                                            string IdsBPL = "";
                                                            if (UpdatedItem.ModifiedPLMDetectionList.Any(pd => pd.Type == "BPL" && pd.IdStatus == 223))
                                                            {
                                                                IdsBPL = string.Join(",", UpdatedItem.ModifiedPLMDetectionList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                            }
                                                            string IdsCPL = "";
                                                            if (UpdatedItem.ModifiedPLMDetectionList.Any(pd => pd.Type == "CPL" && pd.IdStatus == 223))
                                                            {
                                                                IdsCPL = string.Join(",", UpdatedItem.ModifiedPLMDetectionList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                                                            }
                                                            //[rdixit][22.02.2023][GEOS2-4176]
                                                            if (PCMArticleSynchronizationViewModel.BPLPlantCurrencyList != null)
                                                            {
                                                                List<BPLPlantCurrencyDetail> BPLPlantCurrencyDetailList = PCMArticleSynchronizationViewModel.BPLPlantCurrencyList.ToList(); //PLMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(0, IdsBPL, IdsCPL, "Detection");

                                                                if (GeosAppSettingList.Any(i => i.IdAppSetting == 60) && BPLPlantCurrencyDetailList != null)
                                                                {
                                                                    foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                                                    {
                                                                        try
                                                                        {
                                                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                                            List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMEditDetectionSynchronization(GeosAppSettingList, itemBPLPlantCurrency, UpdatedItem);
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
                                                                        }
                                                                        catch (Exception)
                                                                        {
                                                                            throw;
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
                                                        List<ErrorDetails> TempLstErrorDetail = await PCMService.IsPCMEditDetectionSynchronization(GeosAppSettingList, null, UpdatedItem);
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
                                            GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization failed";
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                        }
                                        //CancelSync:;
                                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    RequestClose(null, null);
                    IsAdded = false;
                    IsAcceptButtonEnabled = true;
                    //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    GeosApplication.Instance.Logger.Log(string.Format("Method AddNewOptionWaysDetectionsSparePartsAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    #region AddDuplicateDetection    
                    if (IsCheckedCopyNameDescription == true)
                    {
                        ClonedDetection.Name = Name == null ? "" : Name;
                        ClonedDetection.Name_es = Name == null ? "" : Name;
                        ClonedDetection.Name_fr = Name == null ? "" : Name;
                        ClonedDetection.Name_pt = Name == null ? "" : Name;
                        ClonedDetection.Name_ro = Name == null ? "" : Name;
                        ClonedDetection.Name_ru = Name == null ? "" : Name;
                        ClonedDetection.Name_zh = Name == null ? "" : Name;

                        ClonedDetection.Description = Description == null ? "" : Description;
                        ClonedDetection.Description_es = Description == null ? "" : Description;
                        ClonedDetection.Description_fr = Description == null ? "" : Description;
                        ClonedDetection.Description_pt = Description == null ? "" : Description;
                        ClonedDetection.Description_ro = Description == null ? "" : Description;
                        ClonedDetection.Description_ru = Description == null ? "" : Description;
                        ClonedDetection.Description_zh = Description == null ? "" : Description;
                    }
                    else
                    {
                        ClonedDetection.Name = Name == null ? "" : Name;
                        ClonedDetection.Name_es = Name_es == null ? "" : Name_es;
                        ClonedDetection.Name_fr = Name_fr == null ? "" : Name_fr;
                        ClonedDetection.Name_pt = Name_pt == null ? "" : Name_pt;
                        ClonedDetection.Name_ro = Name_ro == null ? "" : Name_ro;
                        ClonedDetection.Name_ru = Name_ru == null ? "" : Name_ru;
                        ClonedDetection.Name_zh = Name_zh == null ? "" : Name_zh;

                        ClonedDetection.Description = Description_en == null ? "" : Description_en;
                        ClonedDetection.Description_es = Description_es == null ? "" : Description_es;
                        ClonedDetection.Description_fr = Description_fr == null ? "" : Description_fr;
                        ClonedDetection.Description_pt = Description_pt == null ? "" : Description_pt;
                        ClonedDetection.Description_ro = Description_ro == null ? "" : Description_ro;
                        ClonedDetection.Description_ru = Description_ru == null ? "" : Description_ru;
                        ClonedDetection.Description_zh = Description_zh == null ? "" : Description_zh;
                    }

                    if (WeldOrder == null)
                        WeldOrder = 0;
                    ClonedDetection.WeldOrder = WeldOrder.Value;
                    ClonedDetection.IdScope = Convert.ToUInt64(SelectedScopeList.IdLookupValue);
                    ClonedDetection.Orientation = null;
                    ClonedDetection.NameToShow = "";
                    ClonedDetection.IdDetectionType = SelectedTest.IdDetectionType;
                    ClonedDetection.Code = string.Empty;
                    ClonedDetection.IdTestType = SelectedTestType.IdTestType;
                    ClonedDetection.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);
                    ClonedDetection.IdECOSVisibility = Convert.ToInt32(SelectedECOSVisibility.IdLookupValue);
                    if (SelectedOrder != null)
                    {
                        ClonedDetection.IdGroup = SelectedOrder.IdGroup;
                        ClonedDetection.DetectionOrderGroup = SelectedOrder;
                    }
                    if (ClonedDetection.IdECOSVisibility == 323)//Available
                    {
                        ClonedDetection.PurchaseQtyMax = 1;
                        ClonedDetection.PurchaseQtyMin = 1;
                        ClonedDetection.IsShareWithCustomer = 1;
                        ClonedDetection.IsSparePartOnly = 0;
                    }
                    else if (ClonedDetection.IdECOSVisibility == 324)//Only Spare Part
                    {
                        ClonedDetection.PurchaseQtyMax = 1;
                        ClonedDetection.PurchaseQtyMin = 1;
                        ClonedDetection.IsShareWithCustomer = 0;
                        ClonedDetection.IsSparePartOnly = 1;
                    }
                    else if (ClonedDetection.IdECOSVisibility == 325)//Only EMDEP User
                    {
                        ClonedDetection.PurchaseQtyMax = 1;
                        ClonedDetection.PurchaseQtyMin = 1;
                        ClonedDetection.IsShareWithCustomer = 0;
                        ClonedDetection.IsSparePartOnly = 0;
                    }
                    else if (ClonedDetection.IdECOSVisibility == 326)//Read Only
                    {
                        ClonedDetection.PurchaseQtyMax = 0;
                        ClonedDetection.PurchaseQtyMin = 0;
                        ClonedDetection.IsShareWithCustomer = 0;
                        ClonedDetection.IsSparePartOnly = 0;
                    }
                    if (IsCheckedAttachment)
                    {
                        ClonedDetection.DetectionAttachedDocList = OptionWayDetectionSparePartFilesList.ToList();
                        foreach (var item in ClonedDetection.DetectionAttachedDocList)
                        {
                            if (item.AttachmentType == null)
                                item.AttachmentType = new LookupValue();
                        }
                        ClonedDetection.DetectionAttachedDocList.ForEach(i => i.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                    }
                    if (IsCheckedLinks)
                    {
                        ClonedDetection.DetectionAttachedLinkList = OptionWayDetectionSparePartLinksList.ToList();
                        ClonedDetection.DetectionAttachedLinkList.ForEach(i => i.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                    }
                    if (IsCheckedImages)
                    {
                        ClonedDetection.DetectionImageList = OptionWayDetectionSparePartImagesList.ToList();
                        ClonedDetection.DetectionImageList.ForEach(i => i.AttachmentImage = null);
                        ClonedDetection.DetectionImageList.ForEach(i => i.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                    }
                    if (IsCheckedCustomers)
                    {
                        List<LookupValue> CustLookupValues = new List<LookupValue>(CRMService.GetLookupValues(8));
                        ClonedDetection.CustomerListByDetection = new List<CPLCustomer>();
                        //[GEOS2-4538][13.06.2023][rdixit]
                        if (PCMCustomerList != null && CustLookupValues != null)
                        {
                            foreach (var item in PCMCustomerList)
                            {

                                if (CustLookupValues.Any(i => i.IdLookupValue == item.IdRegion))
                                {
                                    //if (item.IdPlant != null || item.IdCountry != null || item.IdGroup != 0)
                                    //{
                                    ClonedDetection.CustomerListByDetection.Add(item);
                                    // }
                                }
                                else
                                {
                                    ClonedDetection.CustomerListByDetection.Add(item);
                                }

                            }
                        }
                        //ClonedDetection.CustomerListByDetection = IncludedCustomerList.Where(i=> CustLookupValues.Contains(! j=>j. i.IdRegion).ToList();
                        ClonedDetection.CustomerListByDetection.ForEach(i => i.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                    }
                    if (IsCheckedRelatedModules)//[Sudhir.Jangra][GEOS2-4468][31/05/2023]
                    {
                        ClonedDetection.ProductTypesList = ProductTypesList.ToList();
                        ClonedDetection.ProductTypesList.ForEach(i => i.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser));
                    }
                    if (IsCheckedPrice)
                    {
                        UpdateTheIncludedAndNotIncludedPriceList(ClonedDetection);
                    }
                    ClonedDetection.DetectionLogEntryList = new List<DetectionLogEntry>();
                    ClonedDetection.DetectionLogEntryList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DuplicateDetectionAddedChangeLog").ToString(), ClonedDetection.Name, PrevName) });
                    //  ClonedDetection = PCMService.AddDetection_V2340(ClonedDetection);
                    //[Sudhir.Jangra][GEOS2-4468][01/06/2023]
                    //ClonedDetection = PCMService.AddDetection_V2400(ClonedDetection);
                    //[Rahul.Gadhave][GEOS2-5896][Date:29/08/2024]
                    ClonedDetection = PCMService.AddDetection_V2560(ClonedDetection);

                    IsDuplicateDetectionAdded = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DuplicatedDetectionItemAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    GeosApplication.Instance.Logger.Log(string.Format("Method AddNewOptionWaysDetectionsSparePartsAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                    #endregion
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsAdded = false;
                IsAcceptButtonEnabled = true;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewOptionWaysDetectionsSparePartsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsAdded = false;
                IsAcceptButtonEnabled = true;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewOptionWaysDetectionsSparePartsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsAdded = false;
                IsAcceptButtonEnabled = true;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewOptionWaysDetectionsSparePartsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveNameAndDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method RetrieveNameAndDescriptionByLanguge..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameDescription == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Name = Name_en;
                        Description = Description_en;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Name = Name_es;
                        Description = Description_es;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Name = Name_fr;
                        Description = Description_fr;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Name = Name_pt;
                        Description = Description_pt;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Name = Name_ro;
                        Description = Description_ro;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Name = Name_ru;
                        Description = Description_ru;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Name = Name_zh;
                        Description = Description_zh;
                    }
                }
                //  IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                GeosApplication.Instance.Logger.Log(string.Format("Method RetrieveNameAndDescriptionByLanguge()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveNameAndDescriptionByLanguge() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage..."), category: Category.Info, priority: Priority.Low);
                if (UpdatedItem != null)
                {
                    oldName = UpdatedItem.Name;//[Sudhir.Jangra][GEOS2-3132][24/02/2023]
                }
                if (IsCheckedCopyNameDescription == false)
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
                if (IsCheckedCopyNameDescription == false)
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
                        IsEnabledCancelButton = false;
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

        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage..."), category: Category.Info, priority: Priority.Low);
                if (UpdatedItem != null)
                {
                    oldDescription = UpdatedItem.Description;//[Sudhir.Jangra][GEOS2-3132][24/02/2023]
                }
                if (IsCheckedCopyNameDescription == false)
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
                if (IsCheckedCopyNameDescription == false)
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
               

                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UncheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Name = Name_en;
                    Description = Description_en;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Name = Name_es;
                    Description = Description_es;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Name = Name_fr;
                    Description = Description_fr;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Name = Name_pt;
                    Description = Description_pt;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Name = Name_ro;
                    Description = Description_ro;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Name = Name_ru;
                    Description = Description_ru;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Name = Name_zh;
                    Description = Description_zh;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method UncheckedCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddFileAction..."), category: Category.Info, priority: Priority.Low);

                AddFileInOptionWayDetectionSparePartView addFileInOptionWayDetectionSparePartView = new AddFileInOptionWayDetectionSparePartView();
                AddFileInOptionWayDetectionSparePartViewModel addFileInOptionWayDetectionSparePartViewModel = new AddFileInOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addFileInOptionWayDetectionSparePartView.Close(); };
                addFileInOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addFileInOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addFileInOptionWayDetectionSparePartViewModel.IsNew = true;
                addFileInOptionWayDetectionSparePartView.DataContext = addFileInOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addFileInOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addFileInOptionWayDetectionSparePartView.ShowDialog();

                if (addFileInOptionWayDetectionSparePartViewModel.IsSave == true)
                {
                    if (OptionWayDetectionSparePartFilesList == null)
                        OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                    OptionWayDetectionSparePartFilesList.Add(addFileInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartFile);
                    SelectedOptionWayDetectionSparePartFile = addFileInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartFile;
                    FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method AddFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditFileAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log(string.Format("Method EditFileAction..."), category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                DetectionAttachedDoc detectionAttachedDoc = (DetectionAttachedDoc)detailView.DataControl.CurrentItem;
                AddFileInOptionWayDetectionSparePartView addFileInOptionWayDetectionSparePartView = new AddFileInOptionWayDetectionSparePartView();
                AddFileInOptionWayDetectionSparePartViewModel addFileInOptionWayDetectionSparePartViewModel = new AddFileInOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addFileInOptionWayDetectionSparePartView.Close(); };
                addFileInOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addFileInOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                addFileInOptionWayDetectionSparePartViewModel.IsNew = false;
                addFileInOptionWayDetectionSparePartViewModel.EditInit(detectionAttachedDoc);
                addFileInOptionWayDetectionSparePartView.DataContext = addFileInOptionWayDetectionSparePartViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addFileInOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addFileInOptionWayDetectionSparePartView.ShowDialog();

                if (addFileInOptionWayDetectionSparePartViewModel.IsSave == true)
                {
                    SelectedOptionWayDetectionSparePartFile.IdDetectionAttachedDoc = addFileInOptionWayDetectionSparePartViewModel.IdDetectionAttachedDoc;
                    SelectedOptionWayDetectionSparePartFile.OriginalFileName = addFileInOptionWayDetectionSparePartViewModel.FileName;
                    SelectedOptionWayDetectionSparePartFile.Description = addFileInOptionWayDetectionSparePartViewModel.Description;
                    SelectedOptionWayDetectionSparePartFile.DetectionAttachedDocInBytes = addFileInOptionWayDetectionSparePartViewModel.FileInBytes;
                    SelectedOptionWayDetectionSparePartFile.SavedFileName = addFileInOptionWayDetectionSparePartViewModel.OptionWayDetectionSparePartSavedFileName;
                    SelectedOptionWayDetectionSparePartFile.AttachmentType = addFileInOptionWayDetectionSparePartViewModel.SelectedAttachmentType; //[rdixit][GEOS2-4074][12.12.2022]
                    SelectedOptionWayDetectionSparePartFile.UpdatedDate = addFileInOptionWayDetectionSparePartViewModel.UpdatedDate;
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method EditFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument..."), category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenPdfByOptionWayDetectionSparePart(SelectedOptionWayDetectionSparePartFile, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - ServiceUnexceptedException", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteFileAction..."), category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                ProductTypeAttachedDoc productTypeAttachedDoc = (ProductTypeAttachedDoc)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    IsEnabledCancelButton = true;
                    OptionWayDetectionSparePartFilesList.Remove(SelectedOptionWayDetectionSparePartFile);
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                    FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddLanguages..."), category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PCMService.GetAllLanguages());
                LanguageSelected = Languages.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method AddLanguages()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTestTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes..."), category: Category.Info, priority: Priority.Low);

                TestTypesMenuList = new ObservableCollection<TestTypes>(PCMService.GetAllTestTypes());
                SelectedTestType = TestTypesMenuList.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCustomersList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes..."), category: Category.Info, priority: Priority.Low);

                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(PCMService.GetCustomersWithRegions());

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomersList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCustomersList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCustomersList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCustomersList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTestList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestList..."), category: Category.Info, priority: Priority.Low);

                IList<DetectionTypes> tempTestList = PCMService.GetAllDetectionTypes();
                TestList = new ObservableCollection<DetectionTypes>();
                TestList = new ObservableCollection<DetectionTypes>(tempTestList);
                //SelectedTest = TestList.FirstOrDefault();

                if (WindowHeader == System.Windows.Application.Current.FindResource("AddDetectionHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 2);
                    IsSelectedTestReadOnly = false;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("EditDetectionHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 2);
                    IsSelectedTestReadOnly = true;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("AddOptionHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 3);
                    IsSelectedTestReadOnly = false;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("EditOptionHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 3);
                    IsSelectedTestReadOnly = true;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("AddWayHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 1);
                    IsSelectedTestReadOnly = false;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("EditWayHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 1);
                    IsSelectedTestReadOnly = true;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("AddSparePartHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 4);
                    IsSelectedTestReadOnly = false;
                }
                else if (WindowHeader == System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString())
                {
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == 4);
                    IsSelectedTestReadOnly = true;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                //SelectedStatusIndex = StatusList.FindIndex(x => x.IdLookupValue == 225);
                //SelectedStatus = StatusList.Where(x => x.IdLookupValue == 225).FirstOrDefault();

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
        private void FillECOSVisibilityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillECOSVisibilityList..."), category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempECOSVisibilityList = PCMService.GetLookupValues(67).ToList();
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
        private void FillTestListLikeSelectedTest(DetectionTypes selectedTest)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestListLikeSelectedTest..."), category: Category.Info, priority: Priority.Low);

                IList<DetectionTypes> tempTestList = PCMService.GetAllDetectionTypes();
                TestList = new ObservableCollection<DetectionTypes>();
                TestList = new ObservableCollection<DetectionTypes>(tempTestList);
                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == selectedTest.IdDetectionType);

                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestListLikeSelectedTest()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestListLikeSelectedTest() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestListLikeSelectedTest() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillScopeList() // Added By [plahange]
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillScopeList..."), category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempScopeList = PCMService.GetLookupValues(100).ToList();
                ScopeList = new List<LookupValue>(tempScopeList);
                SelectedScopeList = ScopeList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillScopeList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillScopeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillScopeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillScopeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init(string selectedHeader)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

                Header = selectedHeader;
                Type = Header;

                FillOrderGroupList();
                AddLanguages();
                FillTestTypes();
                FillCustomersList();
                FillTestList();
                FillStatusList();
                FillECOSVisibilityList();
                FillGroups();
                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(3));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(3));
                }

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(2));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(2));
                }

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(4));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(4));
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();

                if (!GeosApplication.Instance.IsPermissionReadOnlyForPCM)
                {
                    IsAcceptButtonEnabled = false;

                }

                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(string selectedHeader)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

                Header = selectedHeader;
                Type = Header;

                FillOrderGroupList();
                AddLanguages();
                FillTestTypes();
                FillCustomersList();
                FillTestList();
                FillStatusList();
                FillECOSVisibilityList();
                FillLogicList();
                FillScopeList();
                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(3));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(3));
                }

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(2));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(2));
                }

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(4));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(4));
                }


                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();
                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>();

                if (!GeosApplication.Instance.IsPermissionReadOnlyForPCM)
                {
                    IsOnlyAcceptButtonEnabled = false;
                }


                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        public void EditInitWayAndSaprePart(string selectedHeader)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

                Header = selectedHeader;
                Type = Header;

                AddLanguages();
                FillTestTypes();
                FillCustomersList();
                FillTestList();

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(3));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(3));
                }

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(2));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(2));
                }
                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(4));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(4));
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();

                GeosApplication.Instance.Logger.Log(string.Format("Method EditInitWayAndSaprePart()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitWayAndSaprePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitWayAndSaprePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitWayAndSaprePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void EditInitOptions(Options tempSelectedType)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInitOptions..."), category: Category.Info, priority: Priority.Low);
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                //DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2360(tempSelectedType.IdOptions, GeosApplication.Instance.ActiveUser.IdUser));

                //[Sudhir.Jangra][GEOS2-4935]
                DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2470(tempSelectedType.IdOptions, GeosApplication.Instance.ActiveUser.IdUser));


                CommentsList = new ObservableCollection<DetectionLogEntry>(temp.DetectionCommentsList.OrderByDescending(x => x.Datetime));
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

                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();
                if (ClonedDetections.DetectionCommentsList==null)
                {
                    ClonedDetections.DetectionCommentsList = new List<DetectionLogEntry>();
                }

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;

				//Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;

                Code = temp.Code;
                WeldOrder = temp.WeldOrder;

                if (TestTypesMenuList != null)
                    SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                if (TestList != null)
                    SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == temp.IdDetectionType);

                if (StatusList != null)
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);

                SelectedScopeList = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(temp.IdScope));
                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }

                SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);

                GroupList = new ObservableCollection<DetectionGroup>();
                if (temp.DetectionGroupList.Count > 0)
                {
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                    SelectedGroupItem = GroupList.FirstOrDefault();
                }
                GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);

                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                #region Attachment
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3" &&
                        GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (OptionWayDetectionSparePartFilesList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3"
                        && GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (OptionWayDetectionSparePartLinksList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3"
                        && GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (OptionWayDetectionSparePartImagesList.Count >= 5)
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
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                #endregion
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();
                //[rdixit][26.08.2023][GEOS2-4779]
                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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

                GeosApplication.Instance.Logger.Log(string.Format("Method EditInitOptions()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditProductTypeItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditProductTypeItem()...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;


                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                //if ((System.Data.DataRowView)detailView.DataControl.CurrentItem != null)
                if ((Emdep.Geos.Data.Common.PCM.ProductTypes)detailView.DataControl.CurrentItem != null)
                {
                    //DataRowView dr = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                    ProductTypes dr = (ProductTypes)detailView.DataControl.CurrentItem;
                    int IdCPType = Convert.ToInt32(dr.IdCPType.ToString());
                    int IdTemplate = Convert.ToInt32(dr.IdTemplate.ToString());

                    EditProductTypeView editProductTypeView = new EditProductTypeView();
                    EditProductTypeViewModel editProductTypeViewModel = new EditProductTypeViewModel();

                    EventHandler handle = delegate { editProductTypeView.Close(); };
                    editProductTypeViewModel.RequestClose += handle;
                    editProductTypeView.DataContext = editProductTypeViewModel;

                    editProductTypeViewModel.ProductTypeItem = new ProductTypes();

                    editProductTypeViewModel.ProductTypeItem.IdCPType = (ulong)IdCPType;
                    editProductTypeViewModel.ProductTypeItem.IdTemplate = (byte)IdTemplate;

                    if (IdTemplate == 24)//[Structure]
                    {
                        editProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditStructureViewHeader").ToString();//[Sudhir.Jangra][GEOS2-4733][22/08/2023]
                        editProductTypeViewModel.IsDefaultWayTypeVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4733][22/08/2023]
                        editProductTypeViewModel.IsWayVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4733]
                        int IdScope = 1636;//[Sudhir.Jangra][GEOS2-4733][23/08/2023]
                        editProductTypeViewModel.Init(IdScope);
                    }
                    else
                    {
                        editProductTypeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditModuleViewHeader").ToString();//[Sudhir.Jangra][GEOS2-4733][22/08/2023]
                        int IdScope = 1635;//[Sudhir.Jangra][GEOS2-4733][23/08/2023]
                        editProductTypeViewModel.Init(IdScope);
                    }


                    //if (iSModuleVisible)
                    //{
                    //    editProductTypeViewModel.Init();
                    //}
                    //else if (iSStructureVisible)
                    //{
                    //    editProductTypeViewModel.Init();
                    //    editProductTypeViewModel.SelectedTemplate = editProductTypeViewModel.TemplatesMenuList.FirstOrDefault(x => x.IdTemplate == IdTemplate);
                    //}
                    var ownerInfo = (detailView as FrameworkElement);
                    editProductTypeView.Owner = Window.GetWindow(ownerInfo);
                    editProductTypeViewModel.isEnabledCancelButton = false;//[Sudhir.Jangra][Geos2-3132][15/02/2023]
                    editProductTypeView.ShowDialog();

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

                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditProductTypeItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditProductTypeItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void EditInitDetections(Detections tempSelectedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                // DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2360(tempSelectedDetection.IdDetections, GeosApplication.Instance.ActiveUser.IdUser));
                //[Sudhir.Jangra][GEOS2-4935]
                 DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2470(tempSelectedDetection.IdDetections, GeosApplication.Instance.ActiveUser.IdUser));

                CommentsList = new ObservableCollection<DetectionLogEntry>(temp.DetectionCommentsList.OrderByDescending(x => x.Datetime));
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
                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();
                if (ClonedDetections.DetectionCommentsList == null)
                {
                    ClonedDetections.DetectionCommentsList = new List<DetectionLogEntry>();
                }

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
				//Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;


                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                SelectedScopeList = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(temp.IdScope));
                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }

                SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);
                GroupList = new ObservableCollection<DetectionGroup>();
                if (temp.DetectionGroupList.Count > 0)
                {
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                    SelectedGroupItem = GroupList.FirstOrDefault();
                }
                GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);

                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                #region Attachment
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3" &&
                        GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (OptionWayDetectionSparePartFilesList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3"
                        && GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (OptionWayDetectionSparePartLinksList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3"
                        && GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (OptionWayDetectionSparePartImagesList.Count >= 5)
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
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                #endregion
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();
                //[rdixit][26.08.2023][GEOS2-4779]
                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                                                               //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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


                GeosApplication.Instance.Logger.Log("Method EditInitDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void EditInitDetections(DetectionDetails tempSelectedDetection)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
             //  DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2360(tempSelectedDetection.IdDetections, GeosApplication.Instance.ActiveUser.IdUser));
                //[Sudhir.Jangra][GEOS2-4935]
                DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2470(tempSelectedDetection.IdDetections, GeosApplication.Instance.ActiveUser.IdUser));

                CommentsList = new ObservableCollection<DetectionLogEntry>(temp.DetectionCommentsList.OrderByDescending(x => x.Datetime));
                if (CommentsList?.Count>0)
                {
                    CommentText = CommentsList.FirstOrDefault().Comments;
                    CommentDateTimeText = CommentsList.FirstOrDefault().Datetime;
                    CommentFullNameText = CommentsList.FirstOrDefault().UserName;
                }
                else
                {
                    CommentText = string.Empty;
                    CommentDateTimeText = null;
                    CommentFullNameText = string.Empty;
                }

                SetUserProfileImage(CommentsList);

                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();
                if (ClonedDetections.DetectionCommentsList==null)
                {
                    ClonedDetections.DetectionCommentsList = new List<DetectionLogEntry>();
                }

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                Description_en = temp.Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                Description_es = temp.Description_es;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                Description_fr = temp.Description_fr;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                Description_pt = temp.Description_pt;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                Description_ro = temp.Description_ro;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                Description_ru = temp.Description_ru;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                Description_zh = temp.Description_zh;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                Name = temp.Name;
                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                Name_en = temp.Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                Name_es = temp.Name_es;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                Name_fr = temp.Name_fr;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                Name_pt = temp.Name_pt;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                Name_ro = temp.Name_ro;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                Name_ru = temp.Name_ru;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                Name_zh = temp.Name_zh;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;

                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == temp.IdECOSVisibility);
                //SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == temp.IdDetectionType);
                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == tempSelectedDetection.IdDetectionType);
                SelectedScopeList = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(temp.IdScope));
                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }

                SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);
                GroupList = new ObservableCollection<DetectionGroup>();
                if (temp.DetectionGroupList.Count > 0)
                {
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                    SelectedGroupItem = GroupList.FirstOrDefault();
                }
                GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                #region Attachment
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3" &&
                        GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (OptionWayDetectionSparePartFilesList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3"
                        && GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (OptionWayDetectionSparePartLinksList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3"
                        && GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (OptionWayDetectionSparePartImagesList.Count >= 5)
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
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                #endregion
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();

                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199

                //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();
                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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
                //IsExportVisible = Visibility.Collapsed;

                GeosApplication.Instance.Logger.Log("Method EditInitDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void PLMEditInitDetections(UInt32 idDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PLMEditInitDetections()...", category: Category.Info, priority: Priority.Low);
                IsOnlyAcceptButtonEnabled = false;
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2360(idDetection, GeosApplication.Instance.ActiveUser.IdUser));
                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;

				//Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;

                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == temp.IdECOSVisibility);
                //SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == temp.IdDetectionType);
                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == temp.IdDetectionType);

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }

                SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);
                GroupList = new ObservableCollection<DetectionGroup>();
                if (temp.DetectionGroupList.Count > 0)
                {
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                    SelectedGroupItem = GroupList.FirstOrDefault();
                }
                GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);

                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());

                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();

                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                                                               //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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

                GeosApplication.Instance.Logger.Log("Method PLMEditInitDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PLMEditInitDetections() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PLMEditInitDetections() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PLMEditInitDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void EditInitWaysAndSparepart(DetectionDetails tempSelectedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                //[001][cpatil][03-11-2022][GEOS2-3956]
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
               // DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2360(tempSelectedDetection.IdDetections, GeosApplication.Instance.ActiveUser.IdUser));
                //[Sudhir.Jangra][GEOS2-4935]
                DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2470(tempSelectedDetection.IdDetections, GeosApplication.Instance.ActiveUser.IdUser));

                CommentsList = new ObservableCollection<DetectionLogEntry>(temp.DetectionCommentsList.OrderByDescending(x => x.Datetime));

                if (CommentsList?.Count>0)
                {
                    CommentText = CommentsList.FirstOrDefault().Comments;
                    CommentDateTimeText = CommentsList.FirstOrDefault().Datetime;
                    CommentFullNameText = CommentsList.FirstOrDefault().UserName;
                }
                else
                {
                    CommentText = string.Empty;
                    CommentDateTimeText = null;
                    CommentFullNameText = string.Empty;
                }
                SetUserProfileImage(CommentsList);

                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();
                if (ClonedDetections.DetectionCommentsList==null)
                {
                    ClonedDetections.DetectionCommentsList = new List<DetectionLogEntry>();
                }

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                Description_en = temp.Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                Description_es = temp.Description_es;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                Description_fr = temp.Description_fr;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                Description_pt = temp.Description_pt;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                Description_ro = temp.Description_ro;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                Description_ru = temp.Description_ru;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                Description_zh = temp.Description_zh;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                Name = temp.Name;
                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                Name_en = temp.Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                Name_es = temp.Name_es;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                Name_fr = temp.Name_fr;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                Name_pt = temp.Name_pt;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                Name_ro = temp.Name_ro;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                Name_ru = temp.Name_ru;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                Name_zh = temp.Name_zh;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == temp.IdECOSVisibility);
                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == tempSelectedDetection.IdDetectionType);
                SelectedScopeList = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(temp.IdScope));
                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }
                if (tempSelectedDetection.IdDetectionType == 4)
                {
                    SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);
                    GroupList = new ObservableCollection<DetectionGroup>();
                    if (temp.DetectionGroupList.Count > 0)
                    {
                        GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                        SelectedGroupItem = GroupList.FirstOrDefault();
                    }
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                }
                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());


                #region Attachment
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3" &&
                        GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (OptionWayDetectionSparePartFilesList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3"
                        && GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (OptionWayDetectionSparePartLinksList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3"
                        && GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (OptionWayDetectionSparePartImagesList.Count >= 5)
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
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                #endregion


                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();

                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();
                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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
                // IsExportVisible = Visibility.Collapsed;

                GeosApplication.Instance.Logger.Log("Method EditInitDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void PLMEditInitWaysAndSparepart(UInt32 idDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);
                IsOnlyAcceptButtonEnabled = false;
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                DetectionDetails temp = (PCMService.GetDetectionByIdDetection_V2360(idDetection, GeosApplication.Instance.ActiveUser.IdUser));
                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
				//Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;

                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == temp.IdECOSVisibility);
                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == temp.IdDetectionType);

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }
                if (temp.IdDetectionType == 4)
                {
                    SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);
                    GroupList = new ObservableCollection<DetectionGroup>();
                    if (temp.DetectionGroupList.Count > 0)
                    {
                        GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                        SelectedGroupItem = GroupList.FirstOrDefault();
                    }
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                }
                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());

                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();

                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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

                GeosApplication.Instance.Logger.Log("Method EditInitDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddLinkAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLinkAction()...", category: Category.Info, priority: Priority.Low);

                AddLinkInOptionWayDetectionSparePartView addLinkInOptionWayDetectionSparePartView = new AddLinkInOptionWayDetectionSparePartView();
                AddLinkInOptionWayDetectionSparePartViewModel addLinkInOptionWayDetectionSparePartViewModel = new AddLinkInOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addLinkInOptionWayDetectionSparePartView.Close(); };
                addLinkInOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addLinkInOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddLinkHeader").ToString();
                addLinkInOptionWayDetectionSparePartViewModel.IsNew = true;
                addLinkInOptionWayDetectionSparePartView.DataContext = addLinkInOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addLinkInOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addLinkInOptionWayDetectionSparePartView.ShowDialog();

                if (addLinkInOptionWayDetectionSparePartViewModel.IsSave == true)
                {
                    if (OptionWayDetectionSparePartLinksList == null)
                        OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                    OptionWayDetectionSparePartLinksList.Add(addLinkInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartLink);
                    SelectedOptionWayDetectionSparePartLink = addLinkInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartLink;
                    FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method AddLinkAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLinkAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteLinkAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteLinkAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentLink"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    OptionWayDetectionSparePartLinksList.Remove(SelectedOptionWayDetectionSparePartLink);
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                    FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method DeleteLinkAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteLinkAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddImageAction()...", category: Category.Info, priority: Priority.Low);

                AddImageInOptionWayDetectionSparePartView addImageInOptionWayDetectionSparePartView = new AddImageInOptionWayDetectionSparePartView();
                AddImageInOptionWayDetectionSparePartViewModel addImageInOptionWayDetectionSparePartViewModel = new AddImageInOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addImageInOptionWayDetectionSparePartView.Close(); };
                addImageInOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addImageInOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddImageHeader").ToString();
                addImageInOptionWayDetectionSparePartViewModel.IsNew = true;
                addImageInOptionWayDetectionSparePartViewModel.OptionWayDetectionSparePartImagesList = OptionWayDetectionSparePartImagesList;

                addImageInOptionWayDetectionSparePartView.DataContext = addImageInOptionWayDetectionSparePartViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addImageInOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addImageInOptionWayDetectionSparePartView.ShowDialog();

                if (addImageInOptionWayDetectionSparePartViewModel.IsSave == true)
                {
                    SelectedOptionWayDetectionSparePartImage = new DetectionImage();
                    SelectedOptionWayDetectionSparePartImage = addImageInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartImage;
                    SelectedOptionWayDetectionSparePartImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedOptionWayDetectionSparePartImage.DetectionImageInBytes);
                    FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    if (addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage != null)
                    {
                        int index_old = OptionWayDetectionSparePartImagesList.IndexOf(addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage);
                        OptionWayDetectionSparePartImagesList.Insert(index_old, addImageInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartImage);

                        int index_new = OptionWayDetectionSparePartImagesList.IndexOf(SelectedOptionWayDetectionSparePartImage) + 1;

                        OptionWayDetectionSparePartImagesList.Remove(addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage);
                        DetectionImage tempProductTypeImage = new DetectionImage();
                        tempProductTypeImage.IdDetectionImage = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.IdDetectionImage;
                        tempProductTypeImage.SavedFileName = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.SavedFileName;
                        tempProductTypeImage.Description = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.Description;
                        tempProductTypeImage.DetectionImageInBytes = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.DetectionImageInBytes;
                        tempProductTypeImage.OriginalFileName = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.OriginalFileName;
                        tempProductTypeImage.Position = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.Position;
                        tempProductTypeImage.CreatedBy = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage.CreatedBy;
                        tempProductTypeImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        tempProductTypeImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.DetectionImageInBytes);

                        OptionWayDetectionSparePartImagesList.Insert(index_new, tempProductTypeImage);
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else
                    {
                        OptionWayDetectionSparePartImagesList.Add(addImageInOptionWayDetectionSparePartViewModel.SelectedOptionWayDetectionSparePartImage);
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }

                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                    if (addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage != null)
                    {
                        SelectedImageIndex = 1;
                    }
                    else
                    {
                        SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedOptionWayDetectionSparePartImage) + 1;
                    }

                    SelectedDefaultImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedDefaultImage.DetectionImageInBytes);
                    IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                }

                GeosApplication.Instance.Logger.Log("Method AddImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditLinkAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditLinkAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                DetectionAttachedLink detectionAttachedLink = (DetectionAttachedLink)detailView.DataControl.CurrentItem;
                AddLinkInOptionWayDetectionSparePartView addLinkInOptionWayDetectionSparePartView = new AddLinkInOptionWayDetectionSparePartView();
                AddLinkInOptionWayDetectionSparePartViewModel addLinkInOptionWayDetectionSparePartViewModel = new AddLinkInOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addLinkInOptionWayDetectionSparePartView.Close(); };
                addLinkInOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addLinkInOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditLinkHeader").ToString();
                addLinkInOptionWayDetectionSparePartViewModel.IsNew = false;
                addLinkInOptionWayDetectionSparePartViewModel.EditInit(detectionAttachedLink);
                addLinkInOptionWayDetectionSparePartView.DataContext = addLinkInOptionWayDetectionSparePartViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addLinkInOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                addLinkInOptionWayDetectionSparePartView.ShowDialog();

                if (addLinkInOptionWayDetectionSparePartViewModel.IsSave == true)
                {
                    detectionAttachedLink.IdDetectionAttachedLink = addLinkInOptionWayDetectionSparePartViewModel.IdDetectionAttachedLink;
                    detectionAttachedLink.Name = addLinkInOptionWayDetectionSparePartViewModel.LinkName;
                    detectionAttachedLink.Address = addLinkInOptionWayDetectionSparePartViewModel.LinkAddress;
                    detectionAttachedLink.Description = addLinkInOptionWayDetectionSparePartViewModel.LinkDescription;
                    detectionAttachedLink.UpdatedDate = addLinkInOptionWayDetectionSparePartViewModel.UpdateDate;
                    SelectedOptionWayDetectionSparePartLink = detectionAttachedLink;
                    IsEnabledCancelButton = true;

                }

                GeosApplication.Instance.Logger.Log("Method EditLinkAction()....executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditLinkAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                    IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                    DetectionImage tempObj = new DetectionImage();
                    tempObj = (DetectionImage)obj;

                    if (OptionWayDetectionSparePartImagesList.Count > 0)
                    {
                        List<DetectionImage> imageList_ForSetPositions = OptionWayDetectionSparePartImagesList.Where(a => a.Position > tempObj.Position).ToList();
                        OptionWayDetectionSparePartImagesList.Remove(tempObj);

                        foreach (DetectionImage productTypeImage in imageList_ForSetPositions)
                        {
                            OptionWayDetectionSparePartImagesList.Where(a => a.Position == productTypeImage.Position).ToList().ForEach(a => { a.Position--; });
                        }
                    }

                    if (!(OptionWayDetectionSparePartImagesList.Count == 0))
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                        SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    else if (OptionWayDetectionSparePartImagesList.Count == 0)
                    {
                        FourOptionWayDetectionSparePartImagesList.Remove((DetectionImage)(obj));
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

        private void EditImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditImageAction()...", category: Category.Info, priority: Priority.Low);

                AddImageInOptionWayDetectionSparePartView addImageInOptionWayDetectionSparePartView = new AddImageInOptionWayDetectionSparePartView();
                AddImageInOptionWayDetectionSparePartViewModel addImageInOptionWayDetectionSparePartViewModel = new AddImageInOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addImageInOptionWayDetectionSparePartView.Close(); };
                addImageInOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addImageInOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditImageHeader").ToString();
                addImageInOptionWayDetectionSparePartViewModel.IsNew = false;

                DetectionImage tempObject = new DetectionImage();
                tempObject = (DetectionImage)obj;

                addImageInOptionWayDetectionSparePartViewModel.EditInit(tempObject);

                addImageInOptionWayDetectionSparePartViewModel.OptionWayDetectionSparePartImagesList = OptionWayDetectionSparePartImagesList;
                int index_new = OptionWayDetectionSparePartImagesList.IndexOf(tempObject);

                addImageInOptionWayDetectionSparePartView.DataContext = addImageInOptionWayDetectionSparePartViewModel;
                addImageInOptionWayDetectionSparePartView.ShowDialog();

                if (addImageInOptionWayDetectionSparePartViewModel.IsSave == true)
                {
                    if (addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage != null)
                    {
                        int index_old = OptionWayDetectionSparePartImagesList.IndexOf(addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage);
                        OptionWayDetectionSparePartImagesList.Remove(tempObject);
                        DetectionImage tempProductTypeImage_old = new DetectionImage();
                        tempProductTypeImage_old.IdDetectionImage = addImageInOptionWayDetectionSparePartViewModel.IdImage;
                        tempProductTypeImage_old.OriginalFileName = addImageInOptionWayDetectionSparePartViewModel.ImageName;
                        tempProductTypeImage_old.Description = addImageInOptionWayDetectionSparePartViewModel.Description;
                        tempProductTypeImage_old.DetectionImageInBytes = addImageInOptionWayDetectionSparePartViewModel.FileInBytes;
                        tempProductTypeImage_old.SavedFileName = addImageInOptionWayDetectionSparePartViewModel.OptionWayDetectionSparePartSavedImageName;
                        tempProductTypeImage_old.Position = addImageInOptionWayDetectionSparePartViewModel.SelectedImage.Position;
                        tempProductTypeImage_old.CreatedBy = addImageInOptionWayDetectionSparePartViewModel.SelectedImage.CreatedBy;
                        tempProductTypeImage_old.ModifiedBy = addImageInOptionWayDetectionSparePartViewModel.SelectedImage.ModifiedBy;
                        tempProductTypeImage_old.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                        OptionWayDetectionSparePartImagesList.Insert(index_old, tempProductTypeImage_old);

                        OptionWayDetectionSparePartImagesList.Remove(addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage);
                        DetectionImage tempProductTypeImage_new = new DetectionImage();
                        tempProductTypeImage_new = addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage;

                        OptionWayDetectionSparePartImagesList.Insert(index_new, tempProductTypeImage_new);
                        SelectedImage = OptionWayDetectionSparePartImagesList[index_old];
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage_old.DetectionImageInBytes);

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage_old.DetectionImageInBytes);
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    }
                    else
                    {
                        int index = OptionWayDetectionSparePartImagesList.IndexOf(tempObject);
                        OptionWayDetectionSparePartImagesList.Remove(tempObject);
                        DetectionImage tempProductTypeImage = new DetectionImage();
                        tempProductTypeImage.IdDetectionImage = addImageInOptionWayDetectionSparePartViewModel.IdImage;
                        tempProductTypeImage.OriginalFileName = addImageInOptionWayDetectionSparePartViewModel.ImageName;
                        tempProductTypeImage.Description = addImageInOptionWayDetectionSparePartViewModel.Description;
                        tempProductTypeImage.DetectionImageInBytes = addImageInOptionWayDetectionSparePartViewModel.FileInBytes;
                        tempProductTypeImage.SavedFileName = addImageInOptionWayDetectionSparePartViewModel.OptionWayDetectionSparePartSavedImageName;
                        tempProductTypeImage.Position = addImageInOptionWayDetectionSparePartViewModel.SelectedImage.Position;
                        tempProductTypeImage.CreatedBy = addImageInOptionWayDetectionSparePartViewModel.SelectedImage.CreatedBy;
                        tempProductTypeImage.ModifiedBy = addImageInOptionWayDetectionSparePartViewModel.SelectedImage.ModifiedBy;
                        tempProductTypeImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                        OptionWayDetectionSparePartImagesList.Insert(index, tempProductTypeImage);
                        SelectedImage = OptionWayDetectionSparePartImagesList[index];
                        SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.DetectionImageInBytes);

                        SelectedDefaultImage = SelectedImage;
                        SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(tempProductTypeImage.DetectionImageInBytes);
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());

                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));

                    if (addImageInOptionWayDetectionSparePartViewModel.OldDefaultImage != null)
                    {
                        SelectedImageIndex = 1;
                    }
                    IsEnabledCancelButton = true;//[Sudhir.Jangra][GEOS2-3132][27/02/2023]
                }

                GeosApplication.Instance.Logger.Log("Method EditImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillOrderGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOrderGroupList()...", category: Category.Info, priority: Priority.Low);

                if (header == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    GroupList_Order = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsList(3));
                }
                if (header == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    GroupList_Order = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsList(2));
                }

                if (header == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    GroupList_Order = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsList(4));
                }


                GeosApplication.Instance.Logger.Log("Method FillOrderGroupList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderGroupList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddOrderGroup(object obj)
        {
            AddOrderGroupInOptionsAndDetectionsView addOrderGroupInOptionsAndDetectionsView = new AddOrderGroupInOptionsAndDetectionsView();
            AddOrderGroupInOptionsAndDetectionsViewModel addOrderGroupInOptionsAndDetectionsViewModel = new AddOrderGroupInOptionsAndDetectionsViewModel();
            EventHandler handle = delegate { addOrderGroupInOptionsAndDetectionsView.Close(); };
            addOrderGroupInOptionsAndDetectionsViewModel.RequestClose += handle;
            addOrderGroupInOptionsAndDetectionsViewModel.Init();
            addOrderGroupInOptionsAndDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddOrderGroupHeader").ToString();
            addOrderGroupInOptionsAndDetectionsViewModel.IsNew = true;

            if (GroupList_Order == null)
            {
                GroupList_Order = new ObservableCollection<DetectionGroup>();
            }
            int count = GroupList_Order.Where(a => a.OrderNumber == 0).Count();
            if (count == 0)
            {
                DetectionGroup detectionGroup_def = new DetectionGroup();
                detectionGroup_def.Name = "--default--";
                detectionGroup_def.OrderNumber = 0;
                detectionGroup_def.IdGroup = 0;
                GroupList_Order.Add(detectionGroup_def);
                GroupList_Order = new ObservableCollection<DetectionGroup>(GroupList_Order.OrderBy(a => a.OrderNumber));
            }
            addOrderGroupInOptionsAndDetectionsViewModel.Init(header, GroupList_Order);

            addOrderGroupInOptionsAndDetectionsView.DataContext = addOrderGroupInOptionsAndDetectionsViewModel;
            var ownerInfo = (obj as FrameworkElement);
            addOrderGroupInOptionsAndDetectionsView.Owner = Window.GetWindow(ownerInfo);
            addOrderGroupInOptionsAndDetectionsView.ShowDialog();

            if (addOrderGroupInOptionsAndDetectionsViewModel.IsSave == true)
            {
                if (GroupList == null)
                    GroupList = new ObservableCollection<DetectionGroup>();

                if (OrderGroupList == null)
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>();

                if (addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order == null)
                    addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order = new ObservableCollection<DetectionGroup>();

                if (GroupList_Order == null)
                    GroupList_Order = new ObservableCollection<DetectionGroup>();

                // add group in list
                if (GroupList.Count > 0)
                {
                    if (addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber == 0)
                    {
                        addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.OrderNumber = GroupList.FirstOrDefault().OrderNumber + 1;
                    }
                    else
                    {
                        List<DetectionGroup> detectionGroup_GetGroupsForSetOrder = GroupList.Where(a => a.OrderNumber >= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber).OrderByDescending(a => a.OrderNumber).ToList();
                        foreach (DetectionGroup detectionGroup in detectionGroup_GetGroupsForSetOrder)
                        {
                            GroupList.Where(a => a.OrderNumber == detectionGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber++; });
                        }
                        addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.OrderNumber = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber;
                    }
                }
                else
                {
                    addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.OrderNumber = 1;
                }
                GroupList.Add(addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem);
                SelectedGroupItem = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem;
                GroupList = new ObservableCollection<DetectionGroup>(GroupList.OrderByDescending(a => a.OrderNumber));

                // add group in order list
                if (addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order.Count > 0)
                {
                    if (addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber != 0)
                    {
                        List<DetectionGroup> detectionGroup_Order_GetGroupsForSetOrder = addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order.Where(a => a.OrderNumber >= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber).OrderByDescending(a => a.OrderNumber).ToList();
                        foreach (DetectionGroup detectionGroup_order in detectionGroup_Order_GetGroupsForSetOrder)
                        {
                            addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order.Where(a => a.OrderNumber == detectionGroup_order.OrderNumber).ToList().ForEach(a => { a.OrderNumber++; a.Name = a.OrderNumber + "-" + a.OriginalName; });
                        }
                    }
                }
                DetectionGroup detectionGroup_Selected = new DetectionGroup();
                detectionGroup_Selected.OriginalName = SelectedGroupItem.Name;
                detectionGroup_Selected.OrderNumber = SelectedGroupItem.OrderNumber;
                detectionGroup_Selected.IdDetectionType = SelectedGroupItem.IdDetectionType;
                detectionGroup_Selected.Name = SelectedGroupItem.OrderNumber + "-" + SelectedGroupItem.Name;

                addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order.Add(detectionGroup_Selected);
                GroupList_Order = new ObservableCollection<DetectionGroup>(addOrderGroupInOptionsAndDetectionsViewModel.GroupList_Order.OrderBy(a => a.OrderNumber));

                // add group in order group list
                DetectionOrderGroup detectionOrderGroup = new DetectionOrderGroup();
                if (OrderGroupList.Count > 0)
                {
                    uint idGroup = OrderGroupList.OrderByDescending(a => a.IdGroup).FirstOrDefault().IdGroup + 1;
                    detectionOrderGroup.IdGroup = idGroup;
                    if (addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber == 0)
                    {
                        detectionOrderGroup.OrderNumber = GroupList.FirstOrDefault().OrderNumber;
                    }
                    else
                    {
                        List<DetectionOrderGroup> detectionOrderGroup_GetGroupsForSetOrder = OrderGroupList.Where(a => a.OrderNumber >= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber).OrderByDescending(a => a.OrderNumber).ToList();
                        foreach (DetectionOrderGroup detectionOrderGroup_order in detectionOrderGroup_GetGroupsForSetOrder)
                        {
                            OrderGroupList.Where(a => a.OrderNumber == detectionOrderGroup_order.OrderNumber).ToList().ForEach(a => { a.OrderNumber++; });
                        }
                        detectionOrderGroup.OrderNumber = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber;
                    }
                }
                else
                {
                    detectionOrderGroup.IdGroup = 1;
                    detectionOrderGroup.OrderNumber = 1;
                }

                detectionOrderGroup.Key = "Group_" + detectionOrderGroup.IdGroup;
                detectionOrderGroup.Name = SelectedGroupItem.Name;
                OrderGroupList.Add(detectionOrderGroup);
                OrderGroupList = new ObservableCollection<DetectionOrderGroup>(OrderGroupList.Where(a => a.OrderNumber > 0).OrderBy(a => a.OrderNumber));
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]

            }
        }

        private void EditOrderGroup(object obj)
        {
            try
            {



                GeosApplication.Instance.Logger.Log("Method EditOrderGroup()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                AddOrderGroupInOptionsAndDetectionsView addOrderGroupInOptionsAndDetectionsView = new AddOrderGroupInOptionsAndDetectionsView();
                AddOrderGroupInOptionsAndDetectionsViewModel addOrderGroupInOptionsAndDetectionsViewModel = new AddOrderGroupInOptionsAndDetectionsViewModel();
                EventHandler handle = delegate { addOrderGroupInOptionsAndDetectionsView.Close(); };
                addOrderGroupInOptionsAndDetectionsViewModel.RequestClose += handle;
                addOrderGroupInOptionsAndDetectionsViewModel.Init();
                addOrderGroupInOptionsAndDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOrderGroupHeader").ToString();
                addOrderGroupInOptionsAndDetectionsViewModel.IsNew = false;

                int count = GroupList_Order.Where(a => a.OrderNumber == 0).Count();
                if (count == 1)
                {
                    GroupList_Order.Remove(GroupList_Order.Where(a => a.OrderNumber == 0).FirstOrDefault());
                }
                addOrderGroupInOptionsAndDetectionsViewModel.EditInit(SelectedGroupItem, header, GroupList_Order);
                addOrderGroupInOptionsAndDetectionsView.DataContext = addOrderGroupInOptionsAndDetectionsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addOrderGroupInOptionsAndDetectionsView.Owner = Window.GetWindow(ownerInfo);
                addOrderGroupInOptionsAndDetectionsView.ShowDialog();

                Int32 OrderNo = SelectedGroupItem.OrderNumber;
                if (addOrderGroupInOptionsAndDetectionsViewModel.IsSave == true)
                {
                    OrderGroupList.Where(a => a.Name == SelectedGroupItem.Name).ToList().ForEach(a => { a.Name = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name; });

                    SelectedGroupItem.IdGroup = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.IdGroup;
                    SelectedGroupItem.Name = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name;
                    SelectedGroupItem.Name_es = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name_es;
                    SelectedGroupItem.Name_fr = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name_fr;
                    SelectedGroupItem.Name_pt = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name_pt;
                    SelectedGroupItem.Name_ro = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name_ro;
                    SelectedGroupItem.Name_ru = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name_ru;
                    SelectedGroupItem.Name_zh = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name_zh;
                    SelectedGroupItem.Description = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description;
                    SelectedGroupItem.Description_es = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description_es;
                    SelectedGroupItem.Description_fr = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description_fr;
                    SelectedGroupItem.Description_pt = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description_pt;
                    SelectedGroupItem.Description_ro = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description_ro;
                    SelectedGroupItem.Description_ru = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description_ru;
                    SelectedGroupItem.Description_zh = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Description_zh;
                    SelectedGroupItem.UpdatedDate = addOrderGroupInOptionsAndDetectionsViewModel.UpdateDate;
                    SelectedGroupItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);


                    if (addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber != OrderNo)
                    {
                        if (addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber > OrderNo)
                        {
                            List<DetectionOrderGroup> detectionOrderGroup_GetGroupsForSetOrder = OrderGroupList.Where(a => a.OrderNumber <= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber && a.OrderNumber > OrderNo).OrderBy(a => a.OrderNumber).ToList();
                            foreach (DetectionOrderGroup detectionOrderGroup_order in detectionOrderGroup_GetGroupsForSetOrder)
                            {
                                OrderGroupList.Where(a => a.OrderNumber == detectionOrderGroup_order.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; });
                            }
                            OrderGroupList.Where(a => a.Name == addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name).ToList().ForEach(a => { a.Name = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name; a.OrderNumber = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber; });

                            List<DetectionGroup> detectionGroup_GetGroupsForSetOrder = GroupList.Where(a => a.OrderNumber <= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber && a.OrderNumber > OrderNo).OrderBy(a => a.OrderNumber).ToList();
                            foreach (DetectionGroup detectionGroup in detectionGroup_GetGroupsForSetOrder)
                            {
                                GroupList.Where(a => a.OrderNumber == detectionGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; });
                            }
                        }
                        else
                        {
                            List<DetectionOrderGroup> detectionOrderGroup_GetGroupsForSetOrder = OrderGroupList.Where(a => a.OrderNumber >= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber && a.OrderNumber < OrderNo).OrderByDescending(a => a.OrderNumber).ToList();
                            foreach (DetectionOrderGroup detectionOrderGroup_order in detectionOrderGroup_GetGroupsForSetOrder)
                            {
                                OrderGroupList.Where(a => a.OrderNumber == detectionOrderGroup_order.OrderNumber).ToList().ForEach(a => { a.OrderNumber++; });
                            }
                            OrderGroupList.Where(a => a.Name == addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name).ToList().ForEach(a => { a.Name = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name; a.OrderNumber = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber; });

                            List<DetectionGroup> detectionGroup_GetGroupsForSetOrder = GroupList.Where(a => a.OrderNumber >= addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber && a.OrderNumber < OrderNo).OrderByDescending(a => a.OrderNumber).ToList();
                            foreach (DetectionGroup detectionGroup in detectionGroup_GetGroupsForSetOrder)
                            {
                                GroupList.Where(a => a.OrderNumber == detectionGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber++; });
                            }
                        }
                        SelectedGroupItem.OrderNumber = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber;
                        GroupList = new ObservableCollection<DetectionGroup>(GroupList.OrderByDescending(a => a.OrderNumber));
                        OrderGroupList = new ObservableCollection<DetectionOrderGroup>(OrderGroupList.Where(a => a.OrderNumber > 0).OrderBy(a => a.OrderNumber));

                        GroupList_Order = new ObservableCollection<DetectionGroup>();
                        foreach (DetectionGroup grouplist_order in GroupList)
                        {
                            DetectionGroup detectionGroup_order = new DetectionGroup();
                            detectionGroup_order.IdGroup = grouplist_order.IdGroup;
                            detectionGroup_order.IdDetectionType = grouplist_order.IdDetectionType;
                            detectionGroup_order.Name = grouplist_order.OrderNumber + "-" + grouplist_order.Name;
                            detectionGroup_order.OrderNumber = grouplist_order.OrderNumber;
                            detectionGroup_order.OriginalName = grouplist_order.Name;
                            GroupList_Order.Add(detectionGroup_order);
                        }
                        GroupList_Order = new ObservableCollection<DetectionGroup>(GroupList_Order.OrderBy(a => a.OrderNumber));
                    }
                    else
                    {
                        GroupList_Order.Where(a => a.OrderNumber == SelectedGroupItem.OrderNumber).ToList().ForEach(a => { a.Name = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem_Order.OrderNumber + "-" + addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name; a.OriginalName = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name; });
                        OrderGroupList.Where(a => a.OrderNumber == SelectedGroupItem.OrderNumber).ToList().ForEach(a => { a.Name = addOrderGroupInOptionsAndDetectionsViewModel.SelectedGroupItem.Name; });

                    }
                    IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                }

                GeosApplication.Instance.Logger.Log("Method EditOrderGroup()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditOrderGroup() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void DeleteOrderGroup(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteOrderGroup()...", category: Category.Info, priority: Priority.Low);

                string detections = PCMService.GetDetectionsConcatByIdGroup(SelectedGroupItem.IdGroup);
                if (detections != "")
                {
                    if (header == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                    {
                        detections = "option(s) " + detections;
                    }
                    else if (header == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                    {
                        detections = "detection(s) " + detections;
                    }
                    else if (header == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                    {
                        detections = "detection(s) " + detections;
                    }
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["DeleteGroupInformMessage"].ToString(), detections), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (GroupList.Count > 0)
                        {
                            //Order group list
                            List<DetectionOrderGroup> GroupOrderList_ForSetPositions = OrderGroupList.Where(a => a.OrderNumber > SelectedGroupItem.OrderNumber).OrderBy(a => a.OrderNumber).ToList();
                            string key = OrderGroupList.Where(a => a.Name == SelectedGroupItem.Name).FirstOrDefault().Key;
                            OrderGroupList.Remove(OrderGroupList.Where(a => a.Name == SelectedGroupItem.Name).FirstOrDefault());
                            List<DetectionOrderGroup> OrderList = new List<DetectionOrderGroup>();
                            OrderList = OrderGroupList.Where(a => a.Parent == key).ToList();
                            foreach (DetectionOrderGroup orderGroup in OrderList)
                            {
                                OrderGroupList.Remove(OrderGroupList.Where(a => a.Parent == key).FirstOrDefault());
                            }
                            foreach (DetectionOrderGroup detectionOrderGroup in GroupOrderList_ForSetPositions)
                            {
                                OrderGroupList.Where(a => a.OrderNumber == detectionOrderGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; });
                            }

                            //Order list
                            List<DetectionGroup> GroupList_Order_ForSetPositions = GroupList_Order.Where(a => a.OrderNumber > SelectedGroupItem.OrderNumber).OrderBy(a => a.OrderNumber).ToList();
                            GroupList_Order.Remove(GroupList_Order.Where(a => a.OriginalName == SelectedGroupItem.Name).FirstOrDefault());
                            foreach (DetectionGroup detectionGroup_order in GroupList_Order_ForSetPositions)
                            {
                                GroupList_Order.Where(a => a.OrderNumber == detectionGroup_order.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; a.Name = a.OrderNumber + "-" + a.OriginalName; });
                            }

                            //Group list
                            List<DetectionGroup> GroupList_ForSetPositions = GroupList.Where(a => a.OrderNumber > SelectedGroupItem.OrderNumber).OrderBy(a => a.OrderNumber).ToList();
                            GroupList.Remove(SelectedGroupItem);
                            foreach (DetectionGroup detectionGroup in GroupList_ForSetPositions)
                            {
                                GroupList.Where(a => a.OrderNumber == detectionGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; });
                            }
                        }
                        SelectedGroupItem = GroupList.FirstOrDefault();
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteGroupMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (GroupList.Count > 0)
                        {
                            //Order group list
                            List<DetectionOrderGroup> GroupOrderList_ForSetPositions = OrderGroupList.Where(a => a.OrderNumber > SelectedGroupItem.OrderNumber).OrderBy(a => a.OrderNumber).ToList();
                            string key = OrderGroupList.Where(a => a.Name == SelectedGroupItem.Name).FirstOrDefault().Key;
                            OrderGroupList.Remove(OrderGroupList.Where(a => a.Name == SelectedGroupItem.Name).FirstOrDefault());
                            List<DetectionOrderGroup> OrderList = new List<DetectionOrderGroup>();
                            OrderList = OrderGroupList.Where(a => a.Parent == key).ToList();
                            foreach (DetectionOrderGroup orderGroup in OrderList)
                            {
                                OrderGroupList.Remove(OrderGroupList.Where(a => a.Parent == key).FirstOrDefault());
                            }
                            foreach (DetectionOrderGroup detectionOrderGroup in GroupOrderList_ForSetPositions)
                            {
                                OrderGroupList.Where(a => a.OrderNumber == detectionOrderGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; });
                            }

                            //Order list
                            List<DetectionGroup> GroupList_Order_ForSetPositions = GroupList_Order.Where(a => a.OrderNumber > SelectedGroupItem.OrderNumber).OrderBy(a => a.OrderNumber).ToList();
                            GroupList_Order.Remove(GroupList_Order.Where(a => a.OriginalName == SelectedGroupItem.Name).FirstOrDefault());
                            foreach (DetectionGroup detectionGroup_order in GroupList_Order_ForSetPositions)
                            {
                                GroupList_Order.Where(a => a.OrderNumber == detectionGroup_order.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; a.Name = a.OrderNumber + "-" + a.OriginalName; });
                            }

                            //Group list
                            List<DetectionGroup> GroupList_ForSetPositions = GroupList.Where(a => a.OrderNumber > SelectedGroupItem.OrderNumber).OrderBy(a => a.OrderNumber).ToList();
                            GroupList.Remove(SelectedGroupItem);
                            foreach (DetectionGroup detectionGroup in GroupList_ForSetPositions)
                            {
                                GroupList.Where(a => a.OrderNumber == detectionGroup.OrderNumber).ToList().ForEach(a => { a.OrderNumber--; });
                            }
                        }
                        SelectedGroupItem = GroupList.FirstOrDefault();
                    }

                }
                IsEnabledCancelButton = true; //[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                GeosApplication.Instance.Logger.Log("Method DeleteOrderGroup()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteOrderGroup() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteOrderGroup() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteOrderGroup() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()...", category: Category.Info, priority: Priority.Low);
                var filePath_ = "DetectionHistory_";
                if ((obj as TableView).Name.Equals("ModulesTableView")) filePath_ = "PCM_DOWS_";
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = filePath_ + Name + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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

        public void AddDWOSLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDWOSLogDetails()...", category: Category.Info, priority: Priority.Low);

                //Name and description

                if (ClonedDetections.Name != UpdatedItem.Name)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogName").ToString(), ClonedDetections.Name, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogName").ToString(), "None", UpdatedItem.Name) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogName").ToString(), ClonedDetections.Name, UpdatedItem.Name) });
                    }
                }

                if (ClonedDetections.Name_es != UpdatedItem.Name_es)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name_es))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameES").ToString(), ClonedDetections.Name_es, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name_es))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameES").ToString(), "None", UpdatedItem.Name_es) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameES").ToString(), ClonedDetections.Name_es, UpdatedItem.Name_es) });
                    }
                }

                if (ClonedDetections.Name_fr != UpdatedItem.Name_fr)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name_fr))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameFR").ToString(), ClonedDetections.Name_fr, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name_fr))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameFR").ToString(), "None", UpdatedItem.Name_fr) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameFR").ToString(), ClonedDetections.Name_fr, UpdatedItem.Name_fr) });
                    }
                }

                if (ClonedDetections.Name_pt != UpdatedItem.Name_pt)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name_pt))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNamePT").ToString(), ClonedDetections.Name_pt, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name_pt))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNamePT").ToString(), "None", UpdatedItem.Name_pt) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNamePT").ToString(), ClonedDetections.Name_pt, UpdatedItem.Name_pt) });
                    }
                }

                if (ClonedDetections.Name_ro != UpdatedItem.Name_ro)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name_ro))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameRO").ToString(), ClonedDetections.Name_ro, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name_ro))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameRO").ToString(), "None", UpdatedItem.Name_ro) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameRO").ToString(), ClonedDetections.Name_ro, UpdatedItem.Name_ro) });
                    }
                }

                if (ClonedDetections.Name_ru != UpdatedItem.Name_ru)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name_ru))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameRU").ToString(), ClonedDetections.Name_ru, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name_ru))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameRU").ToString(), "None", UpdatedItem.Name_ru) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameRU").ToString(), ClonedDetections.Name_ru, UpdatedItem.Name_ru) });
                    }
                }

                if (ClonedDetections.Name_zh != UpdatedItem.Name_zh)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Name_zh))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameZH").ToString(), ClonedDetections.Name_zh, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Name_zh))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameZH").ToString(), "None", UpdatedItem.Name_zh) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogNameZH").ToString(), ClonedDetections.Name_zh, UpdatedItem.Name_zh) });
                    }
                }

                if (ClonedDetections.Description != UpdatedItem.Description)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescription").ToString(), ClonedDetections.Description, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescription").ToString(), "None", UpdatedItem.Description) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescription").ToString(), ClonedDetections.Description, UpdatedItem.Description) });
                    }
                }

                if (ClonedDetections.Description_es != UpdatedItem.Description_es)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description_es))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionES").ToString(), ClonedDetections.Description_es, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description_es))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionES").ToString(), "None", UpdatedItem.Description_es) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionES").ToString(), ClonedDetections.Description_es, UpdatedItem.Description_es) });
                    }
                }

                if (ClonedDetections.Description_fr != UpdatedItem.Description_fr)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description_fr))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionFR").ToString(), ClonedDetections.Description_es, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description_fr))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionFR").ToString(), "None", UpdatedItem.Description_fr) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionFR").ToString(), ClonedDetections.Description_fr, UpdatedItem.Description_fr) });
                    }
                }

                if (ClonedDetections.Description_pt != UpdatedItem.Description_pt)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description_pt))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionPT").ToString(), ClonedDetections.Description_pt, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description_pt))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionPT").ToString(), "None", UpdatedItem.Description_pt) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionPT").ToString(), ClonedDetections.Description_pt, UpdatedItem.Description_pt) });
                    }
                }

                if (ClonedDetections.Description_ro != UpdatedItem.Description_ro)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description_ro))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionRO").ToString(), ClonedDetections.Description_ro, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description_ro))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionRO").ToString(), "None", UpdatedItem.Description_ro) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionRO").ToString(), ClonedDetections.Description_ro, UpdatedItem.Description_ro) });
                    }
                }

                if (ClonedDetections.Description_ru != UpdatedItem.Description_ru)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description_ru))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionRU").ToString(), ClonedDetections.Description_ru, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description_ru))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionRU").ToString(), "None", UpdatedItem.Description_ru) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionRU").ToString(), ClonedDetections.Description_ru, UpdatedItem.Description_ru) });
                    }
                }

                if (ClonedDetections.Description_zh != UpdatedItem.Description_zh)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Description_zh))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionZH").ToString(), ClonedDetections.Description_ru, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Description_zh))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionZH").ToString(), "None", UpdatedItem.Description_zh) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescriptionZH").ToString(), ClonedDetections.Description_zh, UpdatedItem.Description_zh) });
                    }
                }

                //Test type
                if (ClonedDetections.TestTypes.IdTestType != UpdatedItem.IdTestType)
                {
                    TestTypes TestType_Updated = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == UpdatedItem.IdTestType);
                    if (TestType_Updated != null)
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogTestType").ToString(), ClonedDetections.TestTypes.Name, TestType_Updated.Name) });
                }

                //Code
                if (ClonedDetections.Code != UpdatedItem.Code)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.Code))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogCode").ToString(), ClonedDetections.Code, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.Code))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogCode").ToString(), "None", UpdatedItem.Code) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogCode").ToString(), ClonedDetections.Code, UpdatedItem.Code) });
                    }
                }

                //order group

                if (ClonedDetections.IdGroup != UpdatedItem.IdGroup)
                {
                    if (GroupList != null && GroupList.Count > 0)
                    {
                        DetectionGroup group_updated = GroupList.FirstOrDefault(x => x.IdGroup == UpdatedItem.IdGroup);
                        if (string.IsNullOrEmpty(group_updated.Name))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), ClonedDetections.DetectionGroup.Name, "None") });
                        else
                        {
                            if (ClonedDetections.IdGroup == null || ClonedDetections.IdGroup == 0)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), "None", group_updated.Name) });
                            else
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), ClonedDetections.DetectionGroup.Name, group_updated.Name) });
                        }
                    }
                    else
                    {
                        if (UpdatedItem.IdGroup == null || ClonedDetections.IdGroup == 0)
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), ClonedDetections.DetectionGroup.Name, "None") });
                    }
                }

                //Weld Order
                if (ClonedDetections.WeldOrder != UpdatedItem.WeldOrder)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.WeldOrder.ToString()))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogWeldOrder").ToString(), ClonedDetections.WeldOrder, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.WeldOrder.ToString()))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogWeldOrder").ToString(), "None", UpdatedItem.WeldOrder) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogWeldOrder").ToString(), ClonedDetections.WeldOrder, UpdatedItem.WeldOrder) });
                    }
                }
                //Type
                if (ClonedDetections.IdDetectionType != UpdatedItem.IdDetectionType)
                {
                    if (string.IsNullOrEmpty(UpdatedItem.IdDetectionType.ToString()))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDetectionType").ToString(), ClonedDetections.DetectionTypes.Name, "None") });
                    else
                    {
                        if (string.IsNullOrEmpty(ClonedDetections.IdDetectionType.ToString()))
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDetectionType").ToString(), "None", UpdatedItem.DetectionTypes.Name) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDetectionType").ToString(), ClonedDetections.DetectionTypes.Name, TestList.Where(i => i.IdDetectionType == UpdatedItem.IdDetectionType).FirstOrDefault().Name) });
                    }
                }
                //Status
                if (ClonedDetections.Status.IdLookupValue != UpdatedItem.IdStatus)
                {
                    LookupValue tempStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == UpdatedItem.IdStatus);
                    if (tempStatus != null)
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogStatus").ToString(), ClonedDetections.Status.Value, tempStatus.Value) });
                }
                //ECOS visibility
                if (ClonedDetections.IdECOSVisibility != UpdatedItem.IdECOSVisibility)
                {
                    LookupValue tempECOS = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == UpdatedItem.IdECOSVisibility);
                    LookupValue temp = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == ClonedDetections.IdECOSVisibility);
                    if (tempECOS != null)
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogECOSVisibility").ToString(), temp.Value, tempECOS.Value, ClonedDetections.Name) });
                }

                //Scope [rdixit][GEOS2-3956][02.11.2022]
                if (ClonedDetections.IdScope != UpdatedItem.IdScope)
                {
                    if (ScopeList.Any(x => x.IdLookupValue == Convert.ToInt32(ClonedDetections.IdScope)))
                    {
                        ClonedDetections.Scope = new LookupValue();
                        ClonedDetections.Scope = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(ClonedDetections.IdScope));
                        LookupValue tempScope = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(UpdatedItem.IdScope));
                        if (tempScope != null)
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogScope").ToString(), ClonedDetections.Scope.Value, tempScope.Value) });
                    }
                    else
                    {
                        if (ClonedDetections.IdScope == 0)
                        {
                            LookupValue tempScopeNone = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(UpdatedItem.IdScope));
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogScope").ToString(), "None", tempScopeNone.Value) });
                        }

                    }


                }
                GeosApplication.Instance.Logger.Log("Method AddDWOSLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddDWOSLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void SelectedTestChangedAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method SelectedTestChangedAction()...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        if (SelectedTest.IdDetectionType == 2)
        //        {
        //            IsStackPanelVisible = Visibility.Visible;
        //            IsNew = true;
        //            IsSelectedTestReadOnly = false;
        //            WindowHeader = System.Windows.Application.Current.FindResource("AddDetectionHeader").ToString();
        //            Header = System.Windows.Application.Current.FindResource("CaptionDetections").ToString();
        //            InitSelectedTest(System.Windows.Application.Current.FindResource("CaptionDetections").ToString(), SelectedTest);
        //        }

        //        else if (SelectedTest.IdDetectionType == 3)
        //        {
        //            IsStackPanelVisible = Visibility.Visible;
        //            IsNew = true;
        //            WindowHeader = System.Windows.Application.Current.FindResource("AddOptionHeader").ToString();
        //            Header = System.Windows.Application.Current.FindResource("CaptionOptions").ToString();
        //            InitSelectedTest(System.Windows.Application.Current.FindResource("CaptionOptions").ToString(), SelectedTest);
        //        }

        //        else if (SelectedTest.IdDetectionType == 1)
        //        {
        //            IsStackPanelVisible = Visibility.Collapsed;
        //            IsNew = true;
        //            WindowHeader = System.Windows.Application.Current.FindResource("AddWayHeader").ToString();
        //            Header = System.Windows.Application.Current.FindResource("CaptionWay").ToString();
        //        }

        //        else if (SelectedTest.IdDetectionType == 4)
        //        {
        //            IsStackPanelVisible = Visibility.Collapsed;
        //            IsNew = true;
        //            WindowHeader = System.Windows.Application.Current.FindResource("AddSparePartHeader").ToString();
        //            Header = System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString();
        //        }

        //        GeosApplication.Instance.Logger.Log("Method SelectedTestChangedAction()....executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an Error in Method SelectedTestChangedAction()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        public void InitSelectedTest(string selectedHeader, DetectionTypes selectedTest)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method InitSelectedTest..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

                Header = selectedHeader;
                Type = Header;

                FillOrderGroupList();
                AddLanguages();
                FillTestTypes();
                FillCustomersList();
                FillTestListLikeSelectedTest(selectedTest);

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(3));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(3));
                }

                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(2));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(2));
                }
                if (selectedHeader == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(4));
                    GroupList = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsByDetectionType(4));
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();

                GeosApplication.Instance.Logger.Log(string.Format("Method InitSelectedTest()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method InitSelectedTest() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method InitSelectedTest() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method InitSelectedTest() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedscopeChangedCommandAction(EditValueChangedEventArgs obj)//[plahange][GEOS2-3956]
        {
            SelectedScopeList.IdLookupValue = 100;
        }
        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void EditInitWays(Ways selectedWay)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWays()...", category: Category.Info, priority: Priority.Low);
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                // var temp = (PCMService.GetDetectionByIdDetection_V2360(selectedWay.IdWays, GeosApplication.Instance.ActiveUser.IdUser));
                //[Sudhir.Jangra][GEOS2-4935]
                 var temp = (PCMService.GetDetectionByIdDetection_V2470(selectedWay.IdWays, GeosApplication.Instance.ActiveUser.IdUser));

                CommentsList = new ObservableCollection<DetectionLogEntry>(temp.DetectionCommentsList.OrderByDescending(x => x.Datetime));
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

                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();
                if (ClonedDetections.DetectionCommentsList==null)
                {
                    ClonedDetections.DetectionCommentsList = new List<DetectionLogEntry>();
                }

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
				//Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;

                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                if (ScopeList!=null)
                {
                    SelectedScopeList = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(temp.IdScope));
                }
                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);


                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);
                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }

                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                #region Attachment
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3" &&
                        GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (OptionWayDetectionSparePartFilesList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3"
                        && GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (OptionWayDetectionSparePartLinksList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3"
                        && GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (OptionWayDetectionSparePartImagesList.Count >= 5)
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
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                #endregion
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();
                //[rdixit][26.08.2023][GEOS2-4779]
                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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
                GeosApplication.Instance.Logger.Log("Method EditWays()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWays() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWays() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditWays() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][03-11-2022][GEOS2-3956]
        public void EditInitSparePart(SpareParts tempSelectedSpareParts)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitSparePart()...", category: Category.Info, priority: Priority.Low);
                //[001][skadam][GEOS2-3642] Apply the Price List Permissions to the Prices Section (Article and DOWS) - 2
                ////[001] changed service method
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                // var temp = (PCMService.GetDetectionByIdDetection_V2360(tempSelectedSpareParts.IdSpareParts, GeosApplication.Instance.ActiveUser.IdUser));

                //[Sudhir.Jangra][GEOS2-4935]
                 var temp = (PCMService.GetDetectionByIdDetection_V2470(tempSelectedSpareParts.IdSpareParts, GeosApplication.Instance.ActiveUser.IdUser));

                CommentsList = new ObservableCollection<DetectionLogEntry>(temp.DetectionCommentsList.OrderByDescending(x => x.Datetime));
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


                if (temp.IncludedPLMDetectionList != null)
                    temp.IncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                if (temp.NotIncludedPLMDetectionList != null)
                    temp.NotIncludedPLMDetectionList.RemoveAll(r => r.IdPermission == null);
                ClonedDetections = (DetectionDetails)temp.Clone();
                if (ClonedDetections.DetectionCommentsList == null)
                {
                    ClonedDetections.DetectionCommentsList = new List<DetectionLogEntry>();
                }

                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;

				//Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                oldDescription_en = string.IsNullOrEmpty(Description_en) ? "" : Description_en;
                oldDescription_es = string.IsNullOrEmpty(Description_es) ? "" : Description_es;
                oldDescription_fr = string.IsNullOrEmpty(Description_fr) ? "" : Description_fr;
                oldDescription_pt = string.IsNullOrEmpty(Description_pt) ? "" : Description_pt;
                oldDescription_ro = string.IsNullOrEmpty(Description_ro) ? "" : Description_ro;
                oldDescription_ru = string.IsNullOrEmpty(Description_ru) ? "" : Description_ru;
                oldDescription_zh = string.IsNullOrEmpty(Description_zh) ? "" : Description_zh;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                oldName_en = string.IsNullOrEmpty(Name_en) ? "" : Name_en;
                oldName_es = string.IsNullOrEmpty(Name_es) ? "" : Name_es;
                oldName_fr = string.IsNullOrEmpty(Name_fr) ? "" : Name_fr;
                oldName_pt = string.IsNullOrEmpty(Name_pt) ? "" : Name_pt;
                oldName_ro = string.IsNullOrEmpty(Name_ro) ? "" : Name_ro;
                oldName_ru = string.IsNullOrEmpty(Name_ru) ? "" : Name_ru;
                oldName_zh = string.IsNullOrEmpty(Name_zh) ? "" : Name_zh;

                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);
                SelectedScopeList = ScopeList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(temp.IdScope));
                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                if (temp.DetectionAttachedDocList.Count > 0)
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);
                    SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartFilesList.FirstOrDefault();
                }
                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(temp.DetectionAttachedDocList);


                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                if (temp.DetectionAttachedLinkList.Count > 0)
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);
                    SelectedOptionWayDetectionSparePartLink = OptionWayDetectionSparePartLinksList.FirstOrDefault();
                }
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(temp.DetectionAttachedLinkList);

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(temp.DetectionImageList);
                if (OptionWayDetectionSparePartImagesList.Count > 0)
                {
                    List<DetectionImage> productTypeImage_PositionZero = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        OptionWayDetectionSparePartImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                        SelectedDefaultImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = OptionWayDetectionSparePartImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);

                    SelectedDefaultImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);


                    SelectedImageIndex = OptionWayDetectionSparePartImagesList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);

                    }
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(a => a.Position));
                }
                SelectedOrder = OrderGroupList.FirstOrDefault(x => x.IdGroup == temp.IdGroup);

                GroupList = new ObservableCollection<DetectionGroup>();
                if (temp.DetectionGroupList.Count > 0)
                {
                    GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);
                    SelectedGroupItem = GroupList.FirstOrDefault();
                }
                GroupList = new ObservableCollection<DetectionGroup>(temp.DetectionGroupList);

                FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                #region Attachment
                if (GeosApplication.Instance.PCMAttachment != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMAttachment != "4" && GeosApplication.Instance.PCMAttachment != "3" &&
                        GeosApplication.Instance.PCMAttachment != "2" && GeosApplication.Instance.PCMAttachment != "1")
                    {
                        if (OptionWayDetectionSparePartFilesList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "2")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "3")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "4")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "5")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "6")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "7")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "8")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "9")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "10")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "11")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "12")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "13")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "14")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "15")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "16")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "17")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "18")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "19")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "20")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMAttachment == "All")
                    {
                        FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>(OptionWayDetectionSparePartFilesList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMLinks != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMLinks != "4" && GeosApplication.Instance.PCMLinks != "3"
                        && GeosApplication.Instance.PCMLinks != "2" && GeosApplication.Instance.PCMLinks != "1")
                    {
                        if (OptionWayDetectionSparePartLinksList.Count() >= 5)
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
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(1).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "2")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(2).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "3")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(3).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "4")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(4).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "5")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(5).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "6")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(6).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "7")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(7).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "8")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(8).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "9")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(9).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "10")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(10).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "11")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(11).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "12")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(12).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "13")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(13).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "14")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(14).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "15")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(15).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "16")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(16).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "17")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(17).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "18")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(18).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "19")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(19).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "20")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).Take(20).ToList());
                    }
                    else if (GeosApplication.Instance.PCMLinks == "All")
                    {
                        FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>(OptionWayDetectionSparePartLinksList.OrderBy(x => x.IdDetection).ToList());
                    }
                }
                if (GeosApplication.Instance.PCMImage != null)//[Sudhir.Jangra][Geos2-1960][06/03/2023]
                {
                    if (GeosApplication.Instance.PCMImage != "4" && GeosApplication.Instance.PCMImage != "3"
                        && GeosApplication.Instance.PCMImage != "2" && GeosApplication.Instance.PCMImage != "1")
                    {
                        if (OptionWayDetectionSparePartImagesList.Count >= 5)
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
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(1).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "2")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(2).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "3")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(3).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "4")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(4).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "5")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(5).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "6")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(6).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "7")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(7).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "8")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(8).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "9")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(9).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "10")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(10).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "11")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(11).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "12")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(12).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "13")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(13).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "14")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(14).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "15")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(15).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "16")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(16).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "17")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(17).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "18")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(18).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "19")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(19).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "20")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).Take(20).ToList());
                    }
                    if (GeosApplication.Instance.PCMImage == "All")
                    {
                        FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>(OptionWayDetectionSparePartImagesList.OrderBy(x => x.Position).ToList());
                    }
                }
                #endregion
                CustomersMenuList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList);
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>(temp.CustomerList.Where(a => a.IsChecked == true));

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

                FourRecordsCustomersList = new ObservableCollection<FourRegionsWithCustomerCount>(FourRecordsList);

                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>(temp.DetectionLogEntryList);
                if (DetectionChangeLogList.Count > 0)
                    SelectedDetectionChangeLog = DetectionChangeLogList.FirstOrDefault();
                //[rdixit][26.08.2023][GEOS2-4779]
                ProductTypesList = new ObservableCollection<ProductTypes>(temp.ProductTypesList);
                if (ProductTypesList.Count > 0)
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                InitTheIncludedAndNotIncludedPriceList(temp);  //GEOS2-3199
                                                               //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
                if (PCMCustomerList == null)
                    PCMCustomerList = new List<CPLCustomer>();

                PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(IdDetections));
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
                GeosApplication.Instance.Logger.Log("Method EditInitSparePart()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitSparePart() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitSparePart() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitSparePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void DownloadImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadImageAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DownloadImageMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    DetectionImage ObjImage = (DetectionImage)obj;
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.Title = "Select path";
                    dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                      "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                      "Portable Network Graphic (*.png)|*.png";
                    dlg.FileName = ObjImage.SavedFileName;

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        System.IO.File.WriteAllBytes(dlg.FileName, ObjImage.DetectionImageInBytes);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DownloadImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DownloadImageAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ItemPositionChangedCommandAction(object obj)//[rdixit][GEOS2-2694][01.08.2022]
        {
            ulong pos = 1;
            foreach (DetectionImage img in OptionWayDetectionSparePartImagesList)
            {
                img.Position = pos;
                pos++;
            }
        }

        private void PrintCommandAction(object obj)//[Sudhir.Jangra][GEOS2-4091][24/01/2023]
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
                PCMPrintModuleDetectionView pCMPrintModuleDetectionView = new PCMPrintModuleDetectionView();
                PCMPrintModuleDetectionViewModel pCMPrintModuleDetectionViewModel = new PCMPrintModuleDetectionViewModel();
                EventHandler handle = delegate { pCMPrintModuleDetectionView.Close(); };
                pCMPrintModuleDetectionViewModel.RequestClose += handle;
                if (pCMPrintModuleDetectionViewModel.DetectionDetails == null)
                {
                    pCMPrintModuleDetectionViewModel.DetectionDetails = new DetectionDetails();
                }
                pCMPrintModuleDetectionViewModel.IncludedCustomersList = IncludedCustomerList?.ToList();
                pCMPrintModuleDetectionViewModel.DetectionDetails = ClonedDetections;
                pCMPrintModuleDetectionView.DataContext = pCMPrintModuleDetectionViewModel;
                pCMPrintModuleDetectionView.ShowDialog();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region GEOS2-4460 Sudhir.Jangra 26/06/2023
        private void AddNewRelatedModulesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewRelatedModulesCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                //DetectionDetails relatedModule = null;
                //relatedModule.ProductTypesList = (List<ProductTypes>)detailView.DataControl.CurrentItem;
                //UInt32 idDetection = relatedModule.IdDetections;
                //UInt32 idDetectionType = relatedModule.IdDetectionType;


                AddEditRelatedModulesView addEditRelatedModulesView = new AddEditRelatedModulesView();
                AddEditRelatedModulesViewModel addEditRelatedModulesViewModel = new AddEditRelatedModulesViewModel();

                EventHandler handle = delegate { addEditRelatedModulesView.Close(); };
                addEditRelatedModulesViewModel.RequestClose += handle;
                addEditRelatedModulesViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddRelatedModule").ToString();
                addEditRelatedModulesView.DataContext = addEditRelatedModulesViewModel;
                addEditRelatedModulesViewModel.IsNew = true;
                addEditRelatedModulesViewModel.Init();

                var ownerInfo = (obj as FrameworkElement);
                addEditRelatedModulesView.Owner = Window.GetWindow(ownerInfo);
                addEditRelatedModulesView.ShowDialog();

                if (addEditRelatedModulesViewModel.IsSave == true)
                {
                    if (ProductTypesList == null)
                    {
                        ProductTypesList = new ObservableCollection<ProductTypes>();
                    }

                    if (ProductTypesList.Count > 0)
                    {
                        foreach (ProductTypes temp in addEditRelatedModulesViewModel.RelatedModulesListForMainGrid)
                        {
                            for (int i = 0; i < ProductTypesList.Count; i++)
                            {
                                if (ProductTypesList[i].IdCPType == temp.IdCPType)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalRelatedModulesRepeatedModulesWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                    }

                    //    ProductTypesList.ToList().AddRange(addEditRelatedModulesViewModel.RelatedModulesListForMainGrid.ToList());
                    foreach (var item in addEditRelatedModulesViewModel.RelatedModulesListForMainGrid)
                    {
                        ProductTypesList.Add(item);
                    }


                    for (int i = 0; i < ProductTypesList.Count; i++)
                    {
                        ProductTypesList[i].SrNo = i + 1;
                    }

                    //if (ProductTypesList.Count > 20)
                    //{
                    //    MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["ProfessionalRelatedModuleCountInformationMessage"].ToString(), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                    //}
                    addEditRelatedModulesViewModel.IsSave = false;
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewRelatedModulesCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteRelatedModulesCommandAction(object obj)//[Sudhir.jangra][GEOS2-4460][26/06/2023]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteRelatedModulesCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteRelatedModulesMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ProductTypesList.Remove(SelectedProductTypes);
                    ProductTypesList = new ObservableCollection<ProductTypes>(ProductTypesList);
                    SelectedProductTypes = ProductTypesList.FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteRelatedModulesCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion





        #region PLMDetections
        private void OnDragRecordOverNotIncludedDetectionGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverNotIncludedDetectionGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PLMDetectionPrice).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PLMDetectionPrice> record = data.Records.OfType<PLMDetectionPrice>().ToList();
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
                        List<PLMDetectionPrice> deleteCPL = new List<PLMDetectionPrice>();
                        foreach (PLMDetectionPrice item in record)
                        {
                            if (item.IdStatus == 224 || item.Type == "CPL")
                                continue;
                            if (IncludedPLMDetectionPriceList.Any(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList))
                            {
                                PLMDetectionPrice checkCPLDrag = IncludedPLMDetectionPriceList.FirstOrDefault(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList);
                                if (!record.Any(a => a.Type == "CPL" && a.IdCustomerOrBasePriceList == checkCPLDrag.IdCustomerOrBasePriceList))
                                {
                                    deleteCPL.AddRange(IncludedPLMDetectionPriceList.Where(a => a.Type == "CPL" && a.IdBasePriceList == item.IdBasePriceList));
                                    Bplcodes.Add(item.Code);
                                    count++;
                                }
                            }
                        }

                        if (count > 0)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["DeletePCMDetectionPriceFromBPLCPLValidationMessage"].ToString(), string.Join(",", Bplcodes.Select(a => a.ToString()))), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMDetectionPrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMDetectionPriceList.Remove(item);
                                    IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(IncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                            //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                            //{
                                            //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                            //    {

                                            //    }
                                            //    CurrencySymbol = SelectedCurrencySymbol;
                                            //}
                                            //else
                                            //{
                                            //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                            //}
                                        }
                                        //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                        //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                        //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                        //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                        //else
                                        //    IncludedFirstActiveSellPrice = null;

                                        //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                        //    CurrencySymbol = "";
                                        //else
                                        //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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
                                    if (!NotIncludedPLMDetectionPriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMDetectionPriceList.Add(item);
                                }
                                foreach (PLMDetectionPrice item in deleteCPL)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMDetectionPriceList.Remove(item);
                                    IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(IncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                            //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                            //{
                                            //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                            //    {

                                            //    }
                                            //    CurrencySymbol = SelectedCurrencySymbol;
                                            //}
                                            //else
                                            //{
                                            //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                            //}
                                        }
                                        //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                        //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                        //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                        //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                        //else
                                        //    IncludedFirstActiveSellPrice = null;

                                        //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                        //    CurrencySymbol = "";
                                        //else
                                        //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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
                                    if (!NotIncludedPLMDetectionPriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMDetectionPriceList.Add(item);
                                }
                                NotIncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(NotIncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
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
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePCMDetectionPriceFromBPLCPLMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMDetectionPrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    IncludedPLMDetectionPriceList.Remove(item);
                                    IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(IncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                    if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                                    {
                                        if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        {
                                            IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                            if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                            {
                                                IncludedFirstActiveSellPrice = null;
                                                CurrencySymbol = "";
                                            }
                                            else
                                            {
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                                CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            }
                                            //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                            IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        }
                                        else
                                        {
                                            IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                            IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                            if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                                IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                            else
                                                IncludedFirstActiveSellPrice = null;

                                            IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                            if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                                CurrencySymbol = "";
                                            else
                                                CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                            //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                            //{
                                            //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                            //    {

                                            //    }
                                            //    CurrencySymbol = SelectedCurrencySymbol;
                                            //}
                                            //else
                                            //{
                                            //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                            //}
                                        }
                                        //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                        //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                        //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                        //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                        //else
                                        //    IncludedFirstActiveSellPrice = null;

                                        //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                        //    CurrencySymbol = "";
                                        //else
                                        //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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
                                    if (!NotIncludedPLMDetectionPriceList.Any(a => a.Code == item.Code))
                                        NotIncludedPLMDetectionPriceList.Add(item);
                                }
                                NotIncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(NotIncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
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

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverNotIncludedDetectionGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverNotIncludedDetectionGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOverIncludedDetectionGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncludedDetectionGrid()...", category: Category.Info, priority: Priority.Low);
                if ((e.IsFromOutside) && typeof(PLMDetectionPrice).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PLMDetectionPrice> record = data.Records.OfType<PLMDetectionPrice>().ToList();
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
                        List<PLMDetectionPrice> addBPL = new List<PLMDetectionPrice>();

                        foreach (PLMDetectionPrice item in record)
                        {
                            if (item.IdStatus == 224)
                                continue;
                            if (item.Type == "CPL")
                            {
                                if (!IncludedPLMDetectionPriceList.Any(a => a.Type == "BPL" && a.IdCustomerOrBasePriceList == item.IdBasePriceList))
                                {
                                    addBPL.Add(NotIncludedPLMDetectionPriceList.FirstOrDefault(a => a.Type == "BPL" && a.IdCustomerOrBasePriceList == item.IdBasePriceList));
                                    cplcodes.Add(item.Code);
                                    count++;
                                }
                            }
                        }

                        if (count > 0)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["AddPCMDetectionPriceInBPLCPLValidationMessage"].ToString(), string.Join(",", cplcodes.Select(a => a.ToString()))), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMDetectionPrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMDetectionPriceList.Remove(item);
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
                                    if (!IncludedPLMDetectionPriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMDetectionPriceList.Add(item);

                                }

                                foreach (PLMDetectionPrice item in addBPL)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMDetectionPriceList.Remove(item);
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
                                    if (!IncludedPLMDetectionPriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMDetectionPriceList.Add(item);
                                }
                                IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(IncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                                if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                                {
                                    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                    {
                                        IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                        if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                        {
                                            IncludedFirstActiveSellPrice = null;
                                            CurrencySymbol = "";
                                        }
                                        else
                                        {
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        }
                                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                        if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                        else
                                            IncludedFirstActiveSellPrice = null;

                                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                            CurrencySymbol = "";
                                        else
                                            CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                        //{
                                        //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        //    {

                                        //    }
                                        //    CurrencySymbol = SelectedCurrencySymbol;
                                        //}
                                        //else
                                        //{
                                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                        //}
                                    }
                                    //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                    //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                    //else
                                    //    IncludedFirstActiveSellPrice = null;

                                    //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                    //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                    //    CurrencySymbol = "";
                                    //else
                                    //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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

                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AddPCMDetectionPriceInBPLCPLMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                foreach (PLMDetectionPrice item in record)
                                {
                                    if (item.IdStatus == 224)
                                        continue;
                                    NotIncludedPLMDetectionPriceList.Remove(item);
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
                                    if (!IncludedPLMDetectionPriceList.Any(a => a.Code == item.Code))
                                        IncludedPLMDetectionPriceList.Add(item);
                                }
                                IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(IncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                                e.Handled = true;
                                if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                                {
                                    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                    {
                                        IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                                        if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                                        {
                                            IncludedFirstActiveSellPrice = null;
                                            CurrencySymbol = "";
                                        }
                                        else
                                        {
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                            CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        }
                                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                    }
                                    else
                                    {
                                        IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                        if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                        else
                                            IncludedFirstActiveSellPrice = null;

                                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                        if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                            CurrencySymbol = "";
                                        else
                                            CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                                        //{
                                        //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                                        //    {

                                        //    }
                                        //    CurrencySymbol = SelectedCurrencySymbol;
                                        //}
                                        //else
                                        //{
                                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                                        //}
                                    }
                                    //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                                    //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                                    //else
                                    //    IncludedFirstActiveSellPrice = null;

                                    //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                                    //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                                    //    CurrencySymbol = "";
                                    //else
                                    //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverIncludedDetectionGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverIncludedDetectionGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][cpatil][GEOS2-3747][10-05-2022]
        private void CalculatesellPriceRulechanged(PLMDetectionPrice PLMDetectionPrice)
        {
            if (PLMDetectionPrice.MaxCost == null)
            {
                PLMDetectionPrice.MaxCost = 0;
            }
            if (PLMDetectionPrice.RuleValue == null)
            {
                PLMDetectionPrice.RuleValue = 0;
            }
            // calculate sellprice (common)
            if (PLMDetectionPrice.IdRule == 307)
            {
                PLMDetectionPrice.SellPrice = Convert.ToDouble(PLMDetectionPrice.MaxCost) + ((Convert.ToDouble(PLMDetectionPrice.MaxCost) * (Convert.ToDouble(PLMDetectionPrice.RuleValue) / 100)));
            }
            else if (PLMDetectionPrice.IdRule == 308)
            {
                if (PLMDetectionPrice.RuleValue == null || PLMDetectionPrice.RuleValue == null)
                {
                    PLMDetectionPrice.RuleValue = null;
                    PLMDetectionPrice.SellPrice = null;
                }
                else if (PLMDetectionPrice.RuleValue != null && Convert.ToDouble(PLMDetectionPrice.RuleValue) <= 0)
                {
                    PLMDetectionPrice.RuleValue = null;
                    PLMDetectionPrice.SellPrice = null;
                }
                else
                {
                    PLMDetectionPrice.SellPrice = PLMDetectionPrice.RuleValue;
                }
            }
            else if (PLMDetectionPrice.IdRule == 1518)
            {
                PLMDetectionPrice.RuleValue = 0;
                PLMDetectionPrice.SellPrice = 0;
            }
            else if (PLMDetectionPrice.IdRule == 309)
            {
                PLMDetectionPrice.SellPrice = Convert.ToDouble(PLMDetectionPrice.MaxCost) + (Convert.ToDouble(PLMDetectionPrice.RuleValue));
            }

            double SellPriceValue = 0;
            double MaxCost = 0;

            if (PLMDetectionPrice.SellPrice != null)
                SellPriceValue = Convert.ToDouble(PLMDetectionPrice.SellPrice);

            if (PLMDetectionPrice.MaxCost != null)
                MaxCost = Convert.ToDouble(PLMDetectionPrice.MaxCost);

            PLMDetectionPrice.Profit = Convert.ToDouble(CalculateBasePriceProfitValue(MaxCost, SellPriceValue));
            PLMDetectionPrice.CostMargin = Convert.ToDouble(CalculateBasePriceCostMarginValue(MaxCost, SellPriceValue));

            PLMDetectionPrice.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMDetectionPrice.IdRule);
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
                            PLMDetectionPrice PLMDetectionPrice = ((DevExpress.Xpf.Grid.EditGridCellData)OriginalSource.DataContext).Row as PLMDetectionPrice;
                            if (PLMDetectionPrice.Type == "BPL")
                            {
                                List<GeosAppSetting> GeosAppSettingRuleValue = WorkbenchService.GetSelectedGeosAppSettings("89");
                                if (PLMDetectionPrice.Currency != null)
                                    if (PLMDetectionPrice.Currency.Name.Equals(GeosAppSettingRuleValue[0].DefaultValue))
                                    {
                                        if (IncludedPLMDetectionPriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IsEnabledPermission == true).Count() > 1)
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


                                                CalculatesellPriceRulechanged(PLMDetectionPrice);
                                                foreach (PLMDetectionPrice item in IncludedPLMDetectionPriceList.Where(i => i.Type == "BPL" && i.Status.IdLookupValue != 224 && i.IdBasePriceList != PLMDetectionPrice.IdBasePriceList && i.IsEnabledPermission == true && i.IsChecked == true))
                                                {
                                                    if (PLMCommon.Instance.CurrencyConversionList.Any(i => i.Idcurrencyto == item.IdCurrency && i.Idcurrencyfrom == PLMDetectionPrice.IdCurrency))
                                                    {
                                                        item.RuleValue = PLMDetectionPrice.RuleValue * (PLMCommon.Instance.CurrencyConversionList.Where(i => i.Idcurrencyto == item.IdCurrency && i.Idcurrencyfrom == PLMDetectionPrice.IdCurrency).FirstOrDefault().ExchangeRate);
                                                        CalculatesellPriceRulechanged(item);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                IsBPLCalculateRuleValue = false;
                                                CalculatesellPriceRulechanged(PLMDetectionPrice);
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

                PLMDetectionPrice PLMDetectionPrice = ((DevExpress.Xpf.Grid.RowEventArgs)obj).Row as PLMDetectionPrice;
                IsEnabledCancelButton = true;
                if (PLMDetectionPrice.MaxCost == null)
                {
                    PLMDetectionPrice.MaxCost = 0;
                }
                if (PLMDetectionPrice.RuleValue == null)
                {
                    PLMDetectionPrice.RuleValue = 0;
                }
                // calculate sellprice (common)
                if (PLMDetectionPrice.IdRule == 307)
                {
                    PLMDetectionPrice.SellPrice = Convert.ToDouble(PLMDetectionPrice.MaxCost) + ((Convert.ToDouble(PLMDetectionPrice.MaxCost) * (Convert.ToDouble(PLMDetectionPrice.RuleValue) / 100)));
                }
                else if (PLMDetectionPrice.IdRule == 308)
                {
                    if (PLMDetectionPrice.RuleValue == null || PLMDetectionPrice.RuleValue == null)
                    {
                        PLMDetectionPrice.RuleValue = null;
                        PLMDetectionPrice.SellPrice = null;
                    }
                    else if (PLMDetectionPrice.RuleValue != null && Convert.ToDouble(PLMDetectionPrice.RuleValue) <= 0)
                    {
                        PLMDetectionPrice.RuleValue = null;
                        PLMDetectionPrice.SellPrice = null;
                    }
                    else
                    {
                        PLMDetectionPrice.SellPrice = PLMDetectionPrice.RuleValue;
                    }
                }
                else if (PLMDetectionPrice.IdRule == 1518)
                {
                    PLMDetectionPrice.RuleValue = 0;
                    PLMDetectionPrice.SellPrice = 0;
                }
                else if (PLMDetectionPrice.IdRule == 309)
                {
                    PLMDetectionPrice.SellPrice = Convert.ToDouble(PLMDetectionPrice.MaxCost) + (Convert.ToDouble(PLMDetectionPrice.RuleValue));
                }

                double SellPriceValue = 0;
                double MaxCost = 0;

                if (PLMDetectionPrice.SellPrice != null)
                    SellPriceValue = Convert.ToDouble(PLMDetectionPrice.SellPrice);

                if (PLMDetectionPrice.MaxCost != null)
                    MaxCost = Convert.ToDouble(PLMDetectionPrice.MaxCost);

                PLMDetectionPrice.Profit = Convert.ToDouble(CalculateBasePriceProfitValue(MaxCost, SellPriceValue));
                PLMDetectionPrice.CostMargin = Convert.ToDouble(CalculateBasePriceCostMarginValue(MaxCost, SellPriceValue));

                PLMDetectionPrice.Rule = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMDetectionPrice.IdRule);

                if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }
                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                        //{
                        //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                        //    {

                        //    }
                        //    CurrencySymbol = SelectedCurrencySymbol;
                        //}
                        //else
                        //{
                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                        //}
                    }
                    //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                    //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                    //else
                    //    IncludedFirstActiveSellPrice = null;

                    //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                    //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                    //    CurrencySymbol = "";
                    //else
                    //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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

                    foreach (PLMDetectionPrice item in IncludedPLMDetectionPriceList)
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

                            foreach (var dataObject in IncludedPLMDetectionPriceList)
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
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == IncludedPLMDetectionPriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = IncludedPLMDetectionPriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", IncludedPLMDetectionPriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
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

                    foreach (PLMDetectionPrice item in NotIncludedPLMDetectionPriceList)
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

                            foreach (var dataObject in NotIncludedPLMDetectionPriceList)
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
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == NotIncludedPLMDetectionPriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = NotIncludedPLMDetectionPriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", NotIncludedPLMDetectionPriceList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
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


        private void InitTheIncludedAndNotIncludedPriceList(DetectionDetails temp)
        {
            //GEOS2-3199
            try
            {
                GeosApplication.Instance.Logger.Log(" Method InitTheIncludedAndNotIncludedPriceList ...", category: Category.Info, priority: Priority.Low);

                IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(temp.IncludedPLMDetectionList);
                NotIncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(temp.NotIncludedPLMDetectionList);

                if (IncludedPLMDetectionPriceList != null)
                {
                    foreach (var included in IncludedPLMDetectionPriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(included.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        included.ToList().Where(inc => inc.Currency.Name == included.Key).ToList().ForEach(inc => inc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                if (NotIncludedPLMDetectionPriceList != null)
                {
                    foreach (var notIncluded in NotIncludedPLMDetectionPriceList.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(notIncluded.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        notIncluded.ToList().Where(notinc => notinc.Currency.Name == notIncluded.Key).ToList().ForEach(notinc => notinc.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                }

                SelectedIncludedPLMDetectionPrice = IncludedPLMDetectionPriceList.FirstOrDefault();
                SelectedNotIncludedPLMDetectionPrice = NotIncludedPLMDetectionPriceList.FirstOrDefault();
                IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>(IncludedPLMDetectionPriceList.ToList().OrderBy(i => i.Type).ThenByDescending(i => i.IdStatus == 223).ThenByDescending(i => i.IdStatus == 225).ThenByDescending(i => i.IdStatus == 224).ThenBy(a => a.IdBasePriceList).ToList());
                if (IncludedPLMDetectionPriceList.Any(ip => ip.IdStatus == 223) && IncludedPLMDetectionPriceList.FirstOrDefault().IdStatus == 223)
                {
                    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                    {
                        IncludedFirstActiveName = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.CurrencyIconImage;

                        if (IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice == null)
                        {
                            IncludedFirstActiveSellPrice = null;
                            CurrencySymbol = "";
                        }
                        else
                        {
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                            CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        }
                        //IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).SellPrice.Value, 2);
                        //CurrencySymbol = IncludedPLMDetectionPriceList.FirstOrDefault(y => y.IdCurrency == SelectedCurrency.IdCurrency).Currency.Symbol; //.Currency.CurrencyIconImage;
                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                    }
                    else
                    {
                        IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                        IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                        if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                            IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                        else
                            IncludedFirstActiveSellPrice = null;

                        IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                        if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                            CurrencySymbol = "";
                        else
                            CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;
                        //    if (PCMCommon.Instance.PCM_SelectedCurrencySymbol == null)
                        //{
                        //    if (IncludedPLMDetectionPriceList.Any(x => x.Currency.IdCurrency == SelectedCurrency.IdCurrency))
                        //    {

                        //    }
                        //    CurrencySymbol = SelectedCurrencySymbol;
                        //}
                        //else
                        //{
                        //    CurrencySymbol = PCMCommon.Instance.PCM_SelectedCurrencySymbol; // IncludedPLMDetectionPriceList[0].Currency.Symbol;
                        //}
                    }
                    //IncludedFirstActiveName = IncludedPLMDetectionPriceList[0].Name;
                    //IncludedFirstActiveCurrencyIconImage = IncludedPLMDetectionPriceList[0].Currency.CurrencyIconImage;

                    //if (IncludedPLMDetectionPriceList[0].SellPrice != null)
                    //    IncludedFirstActiveSellPrice = Math.Round(IncludedPLMDetectionPriceList[0].SellPrice.Value, 2);
                    //else
                    //    IncludedFirstActiveSellPrice = null;

                    //IncludedActiveCount = IncludedPLMDetectionPriceList.Where(ip => ip.IdStatus == 223).Count();
                    //if (IncludedPLMDetectionPriceList[0].SellPrice == null)
                    //    CurrencySymbol = "";
                    //else
                    //    CurrencySymbol = IncludedPLMDetectionPriceList[0].Currency.Symbol;

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

                /// PLM Detection Prices
                UpdatedItem.ModifiedPLMDetectionList = new List<PLMDetectionPrice>();
                UpdatedItem.BasePriceLogEntryList = new List<BasePriceLogEntry>();
                UpdatedItem.CustomerPriceLogEntryList = new List<CustomerPriceLogEntry>();

                // Delete PLM Detection Prices
                if (NotIncludedPLMDetectionPriceList != null)
                {
                    foreach (PLMDetectionPrice item in NotIncludedPLMDetectionPriceList)
                    {
                        if (!ClonedDetections.NotIncludedPLMDetectionList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                        {
                            PLMDetectionPrice PLMDetectionPrice = (PLMDetectionPrice)item.Clone();
                            PLMDetectionPrice.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdatedItem.ModifiedPLMDetectionList.Add(PLMDetectionPrice);
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDeleteInPCM").ToString(), item.Code, item.Type) });
                            if (item.Type == "BPL")
                                UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDelete").ToString(), item.Code) });
                            else
                                UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDelete").ToString(), item.Code) });
                        }
                    }
                }

                //Added PLM Detection Prices
                if (IncludedPLMDetectionPriceList != null)
                {
                    foreach (PLMDetectionPrice item in IncludedPLMDetectionPriceList)
                    {
                        if (!ClonedDetections.IncludedPLMDetectionList.Any(x => x.IdCustomerOrBasePriceList == item.IdCustomerOrBasePriceList && x.Type == item.Type))
                        {
                            PLMDetectionPrice PLMDetectionPrice = (PLMDetectionPrice)item.Clone();
                            PLMDetectionPrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdatedItem.ModifiedPLMDetectionList.Add(PLMDetectionPrice);
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogAddInPCM").ToString(), item.Code, item.Type) });
                            if (item.Type == "BPL")
                                UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogAdd").ToString(), item.Code) });
                            else
                                UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = item.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogAdd").ToString(), item.Code) });

                        }
                    }
                }

                //Updated PLM Detection Prices
                foreach (PLMDetectionPrice originalDetection in ClonedDetections.IncludedPLMDetectionList)
                {
                    if (IncludedPLMDetectionPriceList != null && IncludedPLMDetectionPriceList.Any(x => x.IdCustomerOrBasePriceList == originalDetection.IdCustomerOrBasePriceList && x.Type == originalDetection.Type))
                    {
                        PLMDetectionPrice PLMDetectionPriceUpdated = IncludedPLMDetectionPriceList.FirstOrDefault(x => x.IdCustomerOrBasePriceList == originalDetection.IdCustomerOrBasePriceList && x.Type == originalDetection.Type);
                        if ((PLMDetectionPriceUpdated.RuleValue != originalDetection.RuleValue) ||
                            (PLMDetectionPriceUpdated.IdRule != originalDetection.IdRule) ||
                            (PLMDetectionPriceUpdated.MaxCost != originalDetection.MaxCost)
                            )
                        {
                            PLMDetectionPrice PLMDetectionPrice = (PLMDetectionPrice)PLMDetectionPriceUpdated.Clone();
                            PLMDetectionPrice.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdatedItem.ModifiedPLMDetectionList.Add(PLMDetectionPrice);


                            if (PLMDetectionPriceUpdated.IdRule != originalDetection.IdRule)
                            {
                                string newRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == PLMDetectionPriceUpdated.IdRule).Value;
                                string oldRuleLogic = LogicList.FirstOrDefault(a => a.IdLookupValue == originalDetection.IdRule).Value;
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridRuleLogicInPCM").ToString(), oldRuleLogic, newRuleLogic, PLMDetectionPriceUpdated.Type, PLMDetectionPriceUpdated.Code) });
                                if (PLMDetectionPriceUpdated.Type == "BPL")
                                    UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMDetectionPriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, PLMDetectionPriceUpdated.Code) });
                                else
                                    UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMDetectionPriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridRuleLogic").ToString(), oldRuleLogic, newRuleLogic, PLMDetectionPriceUpdated.Code) });
                            }

                            if (PLMDetectionPriceUpdated.RuleValue != originalDetection.RuleValue)
                            {
                                string oldValue = "";
                                string newValue = "";
                                if (PLMDetectionPriceUpdated.RuleValue == null)
                                    newValue = "None";
                                else
                                    newValue = PLMDetectionPriceUpdated.RuleValue.Value.ToString("0." + new string('#', 339));

                                //Reference
                                //https://stackoverflow.com/questions/1546113/double-to-string-conversion-without-scientific-notation

                                if (originalDetection.RuleValue == null)
                                    oldValue = "None";
                                else
                                    oldValue = originalDetection.RuleValue.Value.ToString("0." + new string('#', 339));

                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridRuleValueInPCM").ToString(), oldValue, newValue, PLMDetectionPriceUpdated.Type, PLMDetectionPriceUpdated.Code) });
                                if (PLMDetectionPriceUpdated.Type == "BPL")
                                    UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMDetectionPriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridRuleValue").ToString(), oldValue, newValue, PLMDetectionPriceUpdated.Code) });
                                else
                                    UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMDetectionPriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridRuleValue").ToString(), oldValue, newValue, PLMDetectionPriceUpdated.Code) });
                            }

                            if (PLMDetectionPriceUpdated.MaxCost != originalDetection.MaxCost)
                            {
                                string oldValue = "";
                                string newValue = "";
                                if (PLMDetectionPriceUpdated.MaxCost == null)
                                    newValue = "None";
                                else
                                    newValue = PLMDetectionPriceUpdated.MaxCost.Value.ToString("0." + new string('#', 339));

                                if (originalDetection.MaxCost == null)
                                    oldValue = "None";
                                else
                                    oldValue = originalDetection.MaxCost.Value.ToString("0." + new string('#', 339));

                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridMaxCostInPCM").ToString(), oldValue, newValue, PLMDetectionPriceUpdated.Type, PLMDetectionPriceUpdated.Code) });
                                if (PLMDetectionPriceUpdated.Type == "BPL")
                                    UpdatedItem.BasePriceLogEntryList.Add(new BasePriceLogEntry() { IdBasePriceList = PLMDetectionPriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridMaxCost").ToString(), oldValue, newValue, PLMDetectionPriceUpdated.Code) });
                                else
                                    UpdatedItem.CustomerPriceLogEntryList.Add(new CustomerPriceLogEntry() { IdCustomerPriceList = PLMDetectionPriceUpdated.IdCustomerOrBasePriceList, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("PCMDetectionPriceChangeLogDetectionGridMaxCost").ToString(), oldValue, newValue, PLMDetectionPriceUpdated.Code) });
                            }
                        }
                    }
                }

                UpdatedItem.ModifiedPLMDetectionList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);
                GeosApplication.Instance.Logger.Log("Method UpdateTheIncludedAndNotIncludedPriceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateTheIncludedAndNotIncludedPriceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DetectionGridControlLoadedAction(object obj)
        {
            try
            {
                //if (IsSwitch == true)
                //    return;

                GeosApplication.Instance.Logger.Log("Method DetectionGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                {
                    int visibleFalseColumn = 0;
                    GridControl gridControl = obj as GridControl;
                    TableView tableView = (TableView)gridControl.View;

                    gridControl.BeginInit();

                    if (File.Exists(PCMDetectionPriceListIncludedGridSetting))
                    {
                        gridControl.RestoreLayoutFromXml(PCMDetectionPriceListIncludedGridSetting);
                    }

                    //This code for save grid layout.
                    gridControl.SaveLayoutToXml(PCMDetectionPriceListIncludedGridSetting);

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
                    //        descriptor.AddValueChanged(column, DetectionVisibleChanged);
                    //    }

                    //    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    //    if (descriptorColumnPosition != null)
                    //    {
                    //        descriptorColumnPosition.AddValueChanged(column, DetectionVisibleIndexChanged);
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
                            IsDetectionColumnChooserVisible = true;
                        }
                        else
                        {
                            IsDetectionColumnChooserVisible = false;
                        }
                        //IsArticleColumnChooserVisible = false;
                    }
                    IsFirstTimeLoad = false;
                    gridControl.EndInit();
                    tableView.SearchString = null;
                    tableView.ShowGroupPanel = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on DetectionGridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void DetectionGridControlUnloadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlUnloadedAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                if (gridControl.Columns.Count() > 0)
                {
                    TableView tableView = (TableView)gridControl.View;
                    tableView.SearchString = string.Empty;
                    if (gridControl.GroupCount > 0)
                        gridControl.ClearGrouping();
                    gridControl.ClearSorting();
                    gridControl.FilterString = null;
                    gridControl.SaveLayoutToXml(PCMDetectionPriceListIncludedGridSetting);
                }
                else
                {
                    IsFirstTimeLoad = true;
                    //IsSwitch = true;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlUnloadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on DetectionGridControlUnloadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region GEOS2-3639
        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2
        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
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

                    IncludedCustomerList = new ObservableCollection<CPLCustomer>(PCMCustomerList.Where(i => i.IsIncluded == 1).ToList());
                    NotIncludedCustomerList = new ObservableCollection<CPLCustomer>(PCMCustomerList.Where(i => i.IsIncluded == 0).ToList());

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
        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
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

                    IncludedCustomerList = new ObservableCollection<CPLCustomer>(PCMCustomerList.Where(i => i.IsIncluded == 1).ToList());
                    NotIncludedCustomerList = new ObservableCollection<CPLCustomer>(PCMCustomerList.Where(i => i.IsIncluded == 0).ToList());

                    IsExportVisible = Visibility.Collapsed;
                    IsEnabledCancelButton = true;
                }

                GeosApplication.Instance.Logger.Log("Method AddNotIncludedCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNotIncludedCustomerCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
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
                            IsEnabledCancelButton = true;
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
        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
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
                            IsEnabledCancelButton = true;
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

                        SelectedGroup = CustomerGroupList.FirstOrDefault(a => a.IdGroup == selectedPlant_First.IdGroup);

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
                CustomerGroupList = new ObservableCollection<Emdep.Geos.Data.Common.PLM.Group>(PLMService.GetGroups());
                CustomerGroupList.Insert(0, new Emdep.Geos.Data.Common.PLM.Group() { GroupName = "", IdGroup = 0 });
                SelectedGroup = CustomerGroupList.FirstOrDefault();
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
        //GEOS2-3639 Add the Country and Plant to the Modules and detections customer section [PCM#70] - 2 
        private void ExportToExcelCustomers(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcelCustomers()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Customers_" + Name + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcelCustomers()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcelCustomers()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        #region[GEOS2-4262][rdixit][29.03.2023]
        private void DuplicateDetectionCommandAction(object obj)
        {
            if (isDuplicateClicked == true)
                return;

            DuplicateModulesAdditionalInformationView duplicateDetectionView = new DuplicateModulesAdditionalInformationView();
            DuplicateModulesAdditionalInformationViewModel duplicateDetectionViewModel = new DuplicateModulesAdditionalInformationViewModel(duplicateDetectionView);
            duplicateDetectionViewModel.IsDuplicateDetectionButtonVisible = Visibility.Visible;
            duplicateDetectionViewModel.IsDuplicateModuleButtonVisible = Visibility.Collapsed;
            duplicateDetectionView.ShowDialog();

            if (duplicateDetectionViewModel.isDuplicateClicked)
            {
                isDuplicateClicked = true;
                IsDuplicateDetectionButtonEnabled = false;
                DuplicateAcceptCommandAction(duplicateDetectionViewModel);
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionDuplicatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                IsNew = true;
            }
        }
        void DuplicateAcceptCommandAction(DuplicateModulesAdditionalInformationViewModel duplicateDetection)
        {
            try
            {
                PrevName = Name;
                Name = "Duplicated_" + Name;
                Name_en = "Duplicated_" + Name_en;
                Code = string.Empty;
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 225);
                //if (OrderGroupList != null)
                //    SelectedOrder = OrderGroupList.OrderBy(j => j.OrderNumber).LastOrDefault();
                //[Rahul Gadhave][GEOS2-5896][Date:28/08/2024]
                if (OrderGroupList != null)
                    SelectedOrder = OrderGroupList.FirstOrDefault();
            


                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>();

                #region DuplicateDetection   
                ClonedDetection = new DetectionDetails();

                if (duplicateDetection.IsCheckedAttachment)
                    IsCheckedAttachment = true;
                else
                {
                    OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                    FourOptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                }
                if (duplicateDetection.IsCheckedLinks)
                    IsCheckedLinks = true;
                else
                {
                    OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                    FourOptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                }
                if (duplicateDetection.IsCheckedImages)
                    IsCheckedImages = true;
                else
                {
                    OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();
                    FourOptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();
                }
                if (duplicateDetection.IsCheckedCustomers)
                    IsCheckedCustomers = true;
                else
                {
                    PCMCustomerList = new List<CPLCustomer>();
                    Groups = string.Empty;
                    Regions = string.Empty;
                    Countries = string.Empty;
                    Plants = string.Empty;
                }
                if (duplicateDetection.IsCheckedRelatedModules)//[Sudhir.Jangra][GEOS2-4468][31/05/2023]
                {
                    IsCheckedRelatedModules = true;
                }
                else
                {
                    ProductTypesList = new ObservableCollection<ProductTypes>();
                }
                if (duplicateDetection.IsCheckedPrice)
                    IsCheckedPrice = true;
                else
                {
                    IncludedPLMDetectionPriceList = new ObservableCollection<PLMDetectionPrice>();
                    IncludedFirstActiveName = "";
                    IncludedFirstActiveCurrencyIconImage = null;
                    IncludedFirstActiveSellPrice = null;
                    IncludedActiveCount = 0;
                    CurrencySymbol = "";
                }
                ClonedDetection.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                ClonedDetection.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                // ClonedDetection = PCMService.AddDetection_V2340(ClonedDetection);              
                #endregion
                IsAdded = true;
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
        private void UpdateTheIncludedAndNotIncludedPriceList(DetectionDetails UpdatedItem)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method UpdateTheIncludedAndNotIncludedPriceList ...", category: Category.Info, priority: Priority.Low);

                /// PLM Detection Prices
                UpdatedItem.ModifiedPLMDetectionList = new List<PLMDetectionPrice>();
                if (IncludedPLMDetectionPriceList != null)
                {
                    foreach (PLMDetectionPrice item in IncludedPLMDetectionPriceList)
                    {
                        PLMDetectionPrice PLMDetectionPrice = (PLMDetectionPrice)item.Clone();
                        PLMDetectionPrice.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        PLMDetectionPrice.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdatedItem.ModifiedPLMDetectionList.Add(PLMDetectionPrice);

                    }
                }
                UpdatedItem.ModifiedPLMDetectionList.Select(a => a.Currency).ToList().ForEach(x => x.CurrencyIconImage = null);
                GeosApplication.Instance.Logger.Log("Method UpdateTheIncludedAndNotIncludedPriceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateTheIncludedAndNotIncludedPriceList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-4349][10.04.2023]
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
        #endregion

        //[Sudhir.Jangra][GEOS2-4935][21/11/2023]
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);
                GridControl gridControlView = (GridControl)obj;
                AddEditPCMOptionCommentsView addCommentsView = new AddEditPCMOptionCommentsView();
                AddEditPCMOptionCommentsViewModel addCommentsViewModel = new AddEditPCMOptionCommentsViewModel();
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
                        CommentsList = new ObservableCollection<DetectionLogEntry>();

                    //  addCommentsViewModel.SelectedComment.IdCPType = contacts.IdArticleSupplier;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<DetectionLogEntry>();

                    AddCommentsList.Add(new DetectionLogEntry()
                    {
                        IdUser = addCommentsViewModel.SelectedComment.IdUser,
                        IdDetection = addCommentsViewModel.SelectedComment.IdDetection,
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
                DetectionLogEntry commentObject = (DetectionLogEntry)parameter;
                if (commentObject.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
                {
                    bool result = false;

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProductTypeComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (CommentsList != null && CommentsList?.Count > 0)
                        {
                            DetectionLogEntry Comment = (DetectionLogEntry)commentObject;

                            CommentsList.Remove(Comment);

                            if (DeleteCommentsList == null)
                                DeleteCommentsList = new ObservableCollection<DetectionLogEntry>();

                            DeleteCommentsList.Add(new DetectionLogEntry()
                            {
                                IdUser = Comment.IdUser,
                                IdDetection = Comment.IdDetection,
                                Comments = Comment.Comments,
                                IdLogEntryByDetection = Comment.IdLogEntryByDetection

                            });
                            CommentsList = new ObservableCollection<DetectionLogEntry>(CommentsList);

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
            DetectionLogEntry logcomments = (DetectionLogEntry)obj;
            if (logcomments.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                AddEditPCMOptionCommentsView editCommentsView = new AddEditPCMOptionCommentsView();
                AddEditPCMOptionCommentsViewModel editCommentsViewModel = new AddEditPCMOptionCommentsViewModel();
                EventHandler handle = delegate { editCommentsView.Close(); };
                editCommentsViewModel.RequestClose += handle;
                editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
                editCommentsViewModel.NewItemComment = SelectedComment.Comments;
                editCommentsViewModel.IdLogEntryByCpType = SelectedComment.IdLogEntryByDetection;
                editCommentsView.DataContext = editCommentsViewModel;
                editCommentsView.ShowDialog();

                if (editCommentsViewModel.SelectedComment != null)
                {
                    SelectedComment.Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByDetection == SelectedComment.IdLogEntryByDetection).Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByDetection == SelectedComment.IdLogEntryByDetection).Datetime = GeosApplication.Instance.ServerDateTime;

                    if (UpdatedCommentsList == null)
                        UpdatedCommentsList = new List<DetectionLogEntry>();

                    // editCommentsViewModel.SelectedComment.IdCPType = SelectedContacts.;
                    UpdatedCommentsList.Add(new DetectionLogEntry()
                    {
                        IdUser = SelectedComment.IdUser,
                        IdDetection = SelectedComment.IdDetection,
                        Comments = SelectedComment.Comments,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdLogEntryByDetection = SelectedComment.IdLogEntryByDetection
                    });
                }
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["EditProductTypeCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

        }

        //[Sudhir.jangra][GEOS2-4935]
        private void SetUserProfileImage(ObservableCollection<DetectionLogEntry> CommentsList)
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

        #endregion
    }
}
