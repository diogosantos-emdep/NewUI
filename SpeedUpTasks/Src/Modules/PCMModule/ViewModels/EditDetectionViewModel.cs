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

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class EditDetectionViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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
        private uint weldOrder;
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
        private ObservableCollection<DetectionLogEntry> changeLogList;
        private DetectionLogEntry selectedDetectionChangeLog;

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




        #endregion

        #region Properties

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

        public uint WeldOrder
        {
            get
            {
                return weldOrder;
            }

            set
            {
                weldOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeldOrder"));
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
            }
        }

        public DetectionImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTest"));
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


        public ICommand ExportToExcelCommand { get; set; }

        //public ICommand SelectedTestChangedCommand { get; set; }










        #endregion

        #region Constructor

        public EditDetectionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditDetectionViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;

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
                EscapeButtonCommand = new DelegateCommand<object>(CancelOptionAction);

                AddOrderGroupCommand = new DelegateCommand<object>(AddOrderGroup);
                GroupDoubleClickCommand = new DelegateCommand<object>(EditOrderGroup);
                DeleteGroupCommand = new DelegateCommand<object>(DeleteOrderGroup);


                OpenImageGalleryCommand = new RelayCommand(new Action<object>(OpenImageGalleryAction));
                OpenSelectedImageCommand = new DelegateCommand<object>(OpenSelectedImageAction);

                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
                //SelectedTestChangedCommand = new DelegateCommand<object>(SelectedTestChangedAction);


                IsCheckedCopyNameDescription = true;
                SelectedCustomersList = new ObservableCollection<RegionsByCustomer>();

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

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelOptionAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelOptionAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewOptionWaysDetectionsSparePartsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewOptionWaysDetectionsSparePartsAction()..."), category: Category.Info, priority: Priority.Low);

                    UpdatedItem = new DetectionDetails();
                    ChangeLogList = new ObservableCollection<DetectionLogEntry>();

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
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;
                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

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
                            }
                        }

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
                                }
                            }
                        }

                        //Customers add and Delete
                        UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        {
                            if (item.IsChecked == true)
                            {
                                RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                                if (obj1.IsChecked == false)
                                {
                                    RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                    connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    UpdatedItem.CustomerList.Add(connectorFamilies);
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                                }
                            }
                        }

                        foreach (RegionsByCustomer item in tempCustomersList)
                        {
                            RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                            if (obj1.IsChecked == false)
                            {
                                RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.CustomerList.Add(connectorFamilies);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                            }
                        }

                        DetectionImage tempDefaultImage = ClonedDetections.DetectionImageList.FirstOrDefault(x => x.Position == 1);
                        DetectionImage tempDefaultImage_updated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdDetectionImage != tempDefaultImage_updated.IdDetectionImage)
                        {
                            if (tempDefaultImage_updated.Position == 1)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.SavedFileName, tempDefaultImage_updated.SavedFileName) });
                        }

                        AddDWOSLogDetails();
                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);

                        IsWaySave = PCMService.UpdateDetection(UpdatedItem);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WayItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);


                        RequestClose(null, null);
                    }
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
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;
                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

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
                            }
                        }

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
                                }
                            }
                        }

                        //Customers add and Delete
                        UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        {
                            if (item.IsChecked == true)
                            {
                                RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                                if (obj1.IsChecked == false)
                                {
                                    RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                    connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    UpdatedItem.CustomerList.Add(connectorFamilies);
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                                }
                            }
                        }

                        foreach (RegionsByCustomer item in tempCustomersList)
                        {
                            RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                            if (obj1.IsChecked == false)
                            {
                                RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.CustomerList.Add(connectorFamilies);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                            }
                        }

                        DetectionImage tempDefaultImage = ClonedDetections.DetectionImageList.FirstOrDefault(x => x.Position == 1);
                        DetectionImage tempDefaultImage_updated = OptionWayDetectionSparePartImagesList.FirstOrDefault(x => x.Position == 1);
                        if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdDetectionImage != tempDefaultImage_updated.IdDetectionImage)
                        {
                            if (tempDefaultImage_updated.Position == 1)
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                        }

                        AddDWOSLogDetails();
                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);


                        IsSparepartSave = PCMService.UpdateDetection(UpdatedItem);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SparePartItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        RequestClose(null, null);
                    }

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

                        //Files
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
                            }
                        }

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

                        //Customers add and Delete
                        UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        {
                            if (item.IsChecked == true)
                            {
                                RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                                if (obj1.IsChecked == false)
                                {
                                    RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                    connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    UpdatedItem.CustomerList.Add(connectorFamilies);
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                                }
                            }
                        }

                        foreach (RegionsByCustomer item in tempCustomersList)
                        {
                            RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                            if (obj1.IsChecked == false)
                            {
                                RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.CustomerList.Add(connectorFamilies);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });

                            }
                        }

                        AddDWOSLogDetails();
                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);

                        IsOptionSave = PCMService.UpdateDetection(UpdatedItem);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OptionItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);


                        RequestClose(null, null);
                    }
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
                        UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                        UpdatedItem.IdDetectionType = IdDetectionType;
                        UpdatedItem.Orientation = null;
                        UpdatedItem.NameToShow = "";
                        UpdatedItem.IdDetectionType = SelectedTest.IdDetectionType;
                        UpdatedItem.LastUpdate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        UpdatedItem.WeldOrder = WeldOrder;

                        if (SelectedOrder != null)
                        {
                            UpdatedItem.IdGroup = SelectedOrder.IdGroup;
                            UpdatedItem.DetectionOrderGroup = SelectedOrder;
                        }

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
                            }
                        }

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

                        //Customers add and Delete
                        UpdatedItem.CustomerList = new List<RegionsByCustomer>();
                        List<RegionsByCustomer> tempCustomersList = CustomersMenuList.Where(x => x.IsChecked == true).ToList();

                        foreach (RegionsByCustomer item in ClonedDetections.CustomerList)
                        {
                            if (item.IsChecked == true)
                            {
                                RegionsByCustomer obj1 = CustomersMenuList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                                if (obj1.IsChecked == false)
                                {
                                    RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                    connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    UpdatedItem.CustomerList.Add(connectorFamilies);
                                    ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomerDelete").ToString(), item.GroupName, item.RegionName) });
                                }
                            }
                        }

                        foreach (RegionsByCustomer item in tempCustomersList)
                        {
                            RegionsByCustomer obj1 = ClonedDetections.CustomerList.Where(x => x.IdRegion == item.IdRegion && x.IdGroup == item.IdGroup).FirstOrDefault();
                            if (obj1.IsChecked == false)
                            {
                                RegionsByCustomer connectorFamilies = (RegionsByCustomer)item.Clone();
                                connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedItem.CustomerList.Add(connectorFamilies);
                                ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogCustomersAdd").ToString(), item.GroupName, item.RegionName) });
                            }
                        }

                        AddDWOSLogDetails();
                        UpdatedItem.DetectionLogEntryList = ChangeLogList.ToList();
                        UpdatedItem.DetectionImageList.ForEach(x => x.AttachmentImage = null);


                        IsDetectionSave = PCMService.UpdateDetection(UpdatedItem);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        RequestClose(null, null);
                    }
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewOptionWaysDetectionsSparePartsAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewOptionWaysDetectionsSparePartsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewOptionWaysDetectionsSparePartsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
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
                    SelectedOptionWayDetectionSparePartFile.SavedFileName = addFileInOptionWayDetectionSparePartViewModel.FileName;
                    SelectedOptionWayDetectionSparePartFile.Description = addFileInOptionWayDetectionSparePartViewModel.Description;
                    SelectedOptionWayDetectionSparePartFile.DetectionAttachedDocInBytes = addFileInOptionWayDetectionSparePartViewModel.FileInBytes;
                    SelectedOptionWayDetectionSparePartFile.OriginalFileName = addFileInOptionWayDetectionSparePartViewModel.OptionWayDetectionSparePartSavedFileName;
                    SelectedOptionWayDetectionSparePartFile.UpdatedDate = addFileInOptionWayDetectionSparePartViewModel.UpdatedDate;

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

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();

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

                OptionWayDetectionSparePartFilesList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartLinksList = new ObservableCollection<DetectionAttachedLink>();
                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();
                DetectionChangeLogList = new ObservableCollection<DetectionLogEntry>();

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

        public void EditInitOptions(Options tempSelectedType)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInitOptions..."), category: Category.Info, priority: Priority.Low);

                DetectionDetails temp = (PCMService.GetDetectionByIdDetection(tempSelectedType.IdOptions));
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
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;

                if (TestTypesMenuList != null)
                    SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                if (TestList != null)
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

        public void EditInitDetections(Detections tempSelectedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);

                DetectionDetails temp = (PCMService.GetDetectionByIdDetection(tempSelectedDetection.IdDetections));
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
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

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

        public void EditInitDetections(DetectionDetails tempSelectedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);

                DetectionDetails temp = (PCMService.GetDetectionByIdDetection(tempSelectedDetection.IdDetections));
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
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                //SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == temp.IdDetectionType);
                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == tempSelectedDetection.IdDetectionType);

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

        public void EditInitWaysAndSparepart(DetectionDetails tempSelectedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);

                DetectionDetails temp = (PCMService.GetDetectionByIdDetection(tempSelectedDetection.IdDetections));
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
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                SelectedTest = TestList.FirstOrDefault(x => x.IdDetectionType == tempSelectedDetection.IdDetectionType);

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

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "DetectionHistory_" + Name + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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
                    DetectionGroup group_updated = GroupList.FirstOrDefault(x => x.IdGroup == UpdatedItem.IdGroup);
                    if (string.IsNullOrEmpty(group_updated.Name))
                        ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), ClonedDetections.DetectionGroup.Name, "None") });
                    else
                    {
                        if (ClonedDetections.IdGroup == null)
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), "None", group_updated.Name) });
                        else
                            ChangeLogList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), ClonedDetections.DetectionGroup.Name, group_updated.Name) });
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

        public void EditInitWays(Ways selectedWay)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWays()...", category: Category.Info, priority: Priority.Low);

                var temp = (PCMService.GetDetectionByIdDetection(selectedWay.IdWays));
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
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

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

        public void EditInitSparePart(SpareParts tempSelectedSpareParts)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitSparePart()...", category: Category.Info, priority: Priority.Low);

                var temp = (PCMService.GetDetectionByIdDetection(tempSelectedSpareParts.IdSpareParts));
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
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

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



        #endregion
    }
}
