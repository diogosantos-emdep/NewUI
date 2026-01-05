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
using System.IO;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class EditCatalogueItemViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        #endregion

        #region Public ICommand

        public ICommand ChangeProductTypeListByTemplateCommand { get; set; }
        public ICommand GroupBoxPictureDoubleClickCommand { get; set; }
        //public ICommand ChangeLanguageCommand { get; set; }
        public ICommand CommandOnDragRecordOverProductType { get; set; }
        public ICommand CommandOnDragRecordOverOptions { get; set; }
        public ICommand CommandOnDragRecordOverWay { get; set; }
        public ICommand CommandOnDragRecordOverDetections { get; set; }
        public ICommand CommandOnDragRecordOverSpareParts { get; set; }
        public ICommand CommandOnDragRecordOverFamily { get; set; }
        public ICommand CommandOnDragRecordOverCustomers { get; set; }
        public ICommand CommandAddPictureInGrid { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand ProductTypeCancelCommand { get; set; }
        public ICommand OptionsCancelCommand { get; set; }
        public ICommand WayCancelCommand { get; set; }
        public ICommand DetectionsCancelCommand { get; set; }
        public ICommand SparePartsCancelCommand { get; set; }
        public ICommand FamilyCancelCommand { get; set; }
        public ICommand CustomersCancelCommand { get; set; }
        public ICommand AddNewCatalogueItemCommand { get; set; }
        public ICommand CancelButtonClickCommand { get; set; }
        public ICommand AcceptButtonClickCommand { get; set; }

        public ICommand AddNewProductCommand { get; set; }
        public ICommand AddNewOptionCommand { get; set; }
        public ICommand AddNewWayCommand { get; set; }
        public ICommand AddNewDetectionCommand { get; set; }
        public ICommand AddNewSparePartCommand { get; set; }

        public ICommand EditProductTypeCommand { get; set; }
        public ICommand EditOptionCommand { get; set; }
        public ICommand EditWayCommand { get; set; }
        public ICommand EditDetectionsCommand { get; set; }
        public ICommand EditSparePartsCommand { get; set; }
        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeCatalogueDescriptionCommand { get; set; }
        public ICommand UncheckedCopyDescriptionCommand { get; set; }
        public ICommand AddRowItemFilesCommand { get; set; }
        public ICommand EditFileGridDoubleClickCommand { get; set; }
        public ICommand DeleteRowItemFilesCommand { get; set; }

        

        #endregion

        #region Declarations

        private List<LookupValue> statusList;
        private LookupValue selectedStatus;

        private ObservableCollection<CatalogueItem> catalogueItemsMenulist;
        private ObservableCollection<CatalogueItem> catalogueItemsTypes;

        private ObservableCollection<ProductTypes> productTypesMenulist;
        private ObservableCollection<ProductTypes> productTypes;
        private ProductTypes selectedCpType;

        private ObservableCollection<Template> templatesMenulist;
        private ObservableCollection<Template> templates;

        private ObservableCollection<Ways> waysMenulist;
        private ObservableCollection<Ways> ways;
        private List<Object> selectedWaysType;

        private ObservableCollection<Options> optionsMenulist;
        private ObservableCollection<Options> options;
        //private List<Options> selectedOptionsType;
        private List<Object> selectedOptionsType;

        private ObservableCollection<Detections> detectionsMenulist;
        private ObservableCollection<Detections> detections;
        private List<Object> selectedDetectionsType;

        private ObservableCollection<SpareParts> sparePartsMenulist;
        private ObservableCollection<SpareParts> spareParts;
        private List<Object> selectedSparePartsType;

        private ObservableCollection<ConnectorFamilies> familyMenulist;
        private ObservableCollection<ConnectorFamilies> families;

        private ObservableCollection<Customer> customersMenulist;
        private ObservableCollection<Customer> customers;

        //private ObservableCollection<Item> itemsTreeMenulist;
        private ObservableCollection<User> userList;

        private ObservableCollection<ChangeLog> changeLogsMenulist;
        private ObservableCollection<Language> languages;

        private ObservableCollection<Picture> picturelist;
        private Picture selectedPicture;

        private ObservableCollection<CatalogueItemAttachedDoc> catalogueFilesList;
        private ObservableCollection<Link> links;

        private Template selectedTemplate;
        private DateTime lastUpdate;
        private DateTime selectedCreated;

        private int languageSelectedIndex;
        private int pdfFileIndex;
        private int selectedIndex;

        private string language;
        private string name;
        private string description;
        private string code;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;
        private string twoLetterISOLanguage;
        private int idLanguage;

        private CatalogueItem clonedCatalogueItem;
        private uint modifyBy;
        private Photo selectedItem;
        private bool isCheckedCopyDescription;
        private Language languageSelected;

        private ProductTypeAttachedDoc selectedFile;
        private CatalogueItemAttachedDoc selectedCatalogueFile;

        #endregion

        #region Properties

        public ObservableCollection<Picture> PictureList
        {
            get { return picturelist; }
            set
            {
                picturelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PictureList"));
            }
        }

        public ObservableCollection<ProductTypes> ProductTypesMenulist
        {
            get { return productTypesMenulist; }
            set
            {
                productTypesMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesMenulist"));
            }
        }

        public ProductTypes SelectedCpType
        {
            get { return selectedCpType; }
            set
            {
                selectedCpType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCpType"));
            }
        }

        public ObservableCollection<CatalogueItemAttachedDoc> CatalogueFilesList
        {
            get { return catalogueFilesList; }
            set
            {
                catalogueFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CatalogueFilesList"));
            }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public ObservableCollection<ProductTypes> ProductTypes
        {
            get { return productTypes; }
            set
            {
                productTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypes"));
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

        public ObservableCollection<Link> Links
        {
            get { return links; }
            set
            {
                links = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Links"));
            }
        }

        public Picture SelectedPicture
        {
            get { return selectedPicture; }
            set
            {
                selectedPicture = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPicture"));
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

        public ObservableCollection<Template> TemplatesMenulist
        {
            get { return templatesMenulist; }
            set
            {
                templatesMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplatesMenulist"));
            }
        }

        public int LanguageSelectedIndex
        {
            get { return languageSelectedIndex; }
            set
            {
                languageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelectedIndex"));
            }
        }

        public ObservableCollection<Template> Templates
        {
            get { return templates; }
            set
            {
                templates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Templates"));
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

        public virtual List<Object> SelectedOptionsType
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

        public ObservableCollection<Customer> CustomersMenulist
        {
            get { return customersMenulist; }
            set
            {
                customersMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomersMenulist"));
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set
            {
                customers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customers"));
            }
        }

        //public ObservableCollection<Item> ItemsTreeMenulist
        //{
        //    get { return itemsTreeMenulist; }
        //    set
        //    {
        //        itemsTreeMenulist = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ItemsTreeMenulist"));
        //    }
        //}

        public ObservableCollection<ChangeLog> ChangeLogsMenulist
        {
            get { return changeLogsMenulist; }
            set
            {
                changeLogsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogsMenulist"));
            }
        }

        public ObservableCollection<User> UserList
        {
            get { return userList; }

            set
            {
                userList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserList"));
            }
        }

        public string pdfFile
        {
            get { return pdfFile; }
            set { pdfFile = value; }
        }

        public int PdfFileIndex
        {
            get { return pdfFileIndex; }
            set
            {
                pdfFileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfFileIndex"));
            }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndex"));
            }
        }

        public ObservableCollection<CatalogueItem> CatalogueItemsMenulist
        {
            get { return catalogueItemsMenulist; }

            set
            {
                catalogueItemsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CatalogueItemsMenulist"));
            }
        }

        public ObservableCollection<CatalogueItem> CatalogueItemsTypes
        {
            get
            {
                return catalogueItemsTypes;
            }

            set
            {
                catalogueItemsTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CatalogueItemsTypes"));
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

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
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

        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
            }
        }

        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
            }
        }

        public string Description_pt
        {
            get { return description_pt; }
            set
            {
                description_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
            }
        }

        public string Description_ro
        {
            get { return description_ro; }
            set
            {
                description_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
            }
        }

        public string Description_ru
        {
            get { return description_ru; }
            set
            {
                description_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
            }
        }

        public string Description_zh
        {
            get { return description_zh; }
            set
            {
                description_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
            }
        }

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdate"));
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

        public LookupValue SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }

        public DateTime SelectedCreated
        {
            get { return selectedCreated; }
            set
            {
                selectedCreated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCreated"));
            }
        }

        public CatalogueItem ClonedCatalogueItem
        {
            get { return clonedCatalogueItem; }
            set
            {
                clonedCatalogueItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedCatalogueItem"));
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

        public string TwoLetterISOLanguage
        {
            get
            {
                return twoLetterISOLanguage;
            }

            set
            {
                twoLetterISOLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TwoLetterISOLanguage"));
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

        public ProductTypeAttachedDoc SelectedFile
        {
            get
            {
                return selectedFile;
            }

            set
            {
                selectedFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFile"));
            }
        }

        public CatalogueItemAttachedDoc SelectedCatalogueFile
        {
            get
            {
                return selectedCatalogueFile;
            }

            set
            {
                selectedCatalogueFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCatalogueFile"));

            }
        }

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

        #region Constructor
        public EditCatalogueItemViewModel()
        {
            CommandOnDragRecordOverProductType = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverProduct);
            CommandOnDragRecordOverOptions = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverOptions);
            CommandOnDragRecordOverWay = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverWay);
            CommandOnDragRecordOverDetections = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverDetections);
            CommandOnDragRecordOverSpareParts = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSpareParts);
            CommandOnDragRecordOverFamily = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverFamily);
            CommandOnDragRecordOverCustomers = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverCustomers);

            //ChangeLanguageCommand = new RelayCommand(new Action<object>(ChangeLanguage));
            ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
            ChangeCatalogueDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
            UncheckedCopyDescriptionCommand = new DelegateCommand<object>(UncheckedCopyDescription);
            CommandAddPictureInGrid = new RelayCommand(new Action<object>(AddPictureInGrid));
            OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));
            GroupBoxPictureDoubleClickCommand = new DelegateCommand<object>(OpenPictureAction);

            ProductTypeCancelCommand = new DelegateCommand<object>(ProductCancelAction);
            OptionsCancelCommand = new DelegateCommand<object>(OptionsCancelAction);
            WayCancelCommand = new DelegateCommand<object>(WayCancelAction);
            DetectionsCancelCommand = new DelegateCommand<object>(DetectionsCancelAction);
            SparePartsCancelCommand = new DelegateCommand<object>(SparePartsCancelAction);
            FamilyCancelCommand = new DelegateCommand<object>(FamilyCancelAction);
            CustomersCancelCommand = new DelegateCommand<object>(CustomersCancelAction);

            CancelButtonClickCommand = new DelegateCommand<object>(CancelButtonClickAction);
            AcceptButtonClickCommand = new DelegateCommand<object>(AcceptButtonClickAction);

            ChangeProductTypeListByTemplateCommand = new RelayCommand(new Action<object>(ChangeProductTypeListByTemplateAction));

            AddNewProductCommand = new DelegateCommand<object>(AddNewProductItem);
            AddNewOptionCommand = new DelegateCommand<object>(AddNewOption);
            AddNewWayCommand = new DelegateCommand<object>(AddNewWay);
            AddNewDetectionCommand = new DelegateCommand<object>(AddNewDetection);
            AddNewSparePartCommand = new DelegateCommand<object>(AddNewSparePart);


            EditProductTypeCommand = new DelegateCommand<object>(EditProductType);
            EditOptionCommand = new DelegateCommand<object>(EditOption);
            EditWayCommand = new DelegateCommand<object>(EditWay);
            EditDetectionsCommand = new DelegateCommand<object>(EditDetections);
            EditSparePartsCommand = new DelegateCommand<object>(EditSpareParts);

            NextImageCommand = new DelegateCommand<object>(NextImage);
            PreviousImageCommand = new DelegateCommand<object>(PreviousImage);

            AddRowItemFilesCommand = new DelegateCommand<object>(AddRowItemFiles);
            EditFileGridDoubleClickCommand = new DelegateCommand<object>(EditFileGridDoubleClickCommandAction);
            DeleteRowItemFilesCommand = new DelegateCommand<object>(DeleteRowItemFiles);


            AddLanguages();
            AddProductTypesMenu();
            AddOptionsMenu();
            AddWaysMenu();
            AddDetectionsMenu();
            AddSparePartsMenu();
            AddFamilyMenu();
            AddCustomersMenu();
            AddChangeLogsMenu();
            AddPictures();
            //AddFiles();
            AddLinks();
            AddUser();

            AddCatalogueItemsMenu();
            AddTemplatesMenu();
            FillStatusList();
            IsCheckedCopyDescription = true;
        }

        #endregion

        #region Methods

        // methods for drag and drop functionality of menus
        // Drags the items in particular grid
        private void OnDragRecordOverProduct(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AttachFile()...", category: Category.Info, priority: Priority.Low);

                if (e.IsFromOutside && typeof(ProductTypes).IsAssignableFrom(e.GetRecordType()))
                    if (ProductTypes != null && ProductTypes.Count == 0)
                    {
                        e.Effects = DragDropEffects.Move;
                        e.Handled = true;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = false;
                    }
                GeosApplication.Instance.Logger.Log("Method AttachFile() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AttachFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverOptions(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverOptions()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Options).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverOptions() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverWay(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverWay()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Ways).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverWay() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverWay() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverDetections(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetections()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Detections).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetections() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverSpareParts(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSpareParts()...", category: Category.Info, priority: Priority.Low);

                if (typeof(SpareParts).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSpareParts() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSpareParts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverFamily(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamily()...", category: Category.Info, priority: Priority.Low);

                if (typeof(ConnectorFamilies).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamily() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverCustomers(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverCustomers()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Customer).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverCustomers() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverCustomers() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeLanguage(object obj)
        {
            //ResourceDictionary dict = new ResourceDictionary();
            //Language = Languages[ChangeLanguageCommand].Name;
            //try
            //{
            //    if (Languages[LanguageSelectedIndex].Name == "ES")
            //    {
            //        string TempLanguage = "es-ES";
            //        dict.Source = new Uri("/Emdep.Geos.Modules.PCM;component/Resources/Language." + TempLanguage + ".xaml", UriKind.Relative);
            //    }
            //    else
            //    {
            //        string TempLanguage = "";
            //        dict.Source = new Uri("/Emdep.Geos.Modules.PCM;component/Resources/Language" + TempLanguage + ".xaml", UriKind.Relative);
            //    }
            //    Application.Current.Resources.MergedDictionaries.Add(dict);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void AddPictureInGrid(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddPictureInGrid()...", category: Category.Info, priority: Priority.Low);

                for (int i = 0; i < PictureList.Count; i++)
                {
                    if (i == 0)
                    {
                        PictureList.Add(new Picture { Id = i, Name = "Picture_1.jpg", Image = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/tools.png")) });
                    }
                    if (i == 1)
                    {
                        PictureList.Add(new Picture { Id = i, Name = "Picture_2.jpg", Image = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/file.png")) });
                    }
                    if (i == 2)
                    {
                        PictureList.Add(new Picture { Id = i, Name = "Picture_3.jpg", Image = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/link.png")) });
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddPictureInGrid() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddPictureInGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                //documentViewModel.OpenPdf(SelectedProductTypeFile, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                //else
                //{

                //}
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
                //employeeEducationQualification.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //CustomMessageBox.Show(string.Format("Could not find file '{0}'.", employeeEducationQualification.QualificationFileName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPictureAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPictureAction()...", category: Category.Info, priority: Priority.Low);

                //if (obj is TableView)
                //{
                //    TableView detailView = (TableView)obj;
                //    AppBarControlView appBarControlView = new AppBarControlView();
                //    AppBarControlViewModel appBarControlViewModel = new AppBarControlViewModel();

                //    appBarControlViewModel.Init(SelectedPicture);
                //    EventHandler handle = delegate { appBarControlView.Close(); };
                //    // appBarControlViewModel.RequestClose += handle;
                //    appBarControlView.DataContext = appBarControlViewModel;
                //    appBarControlView.ShowDialog();
                //}

                if (obj is ImageEdit)
                {
                    ImageEdit detailView = (ImageEdit)obj;
                    AppBarControlView appBarControlView = new AppBarControlView();
                    AppBarControlViewModel appBarControlViewModel = new AppBarControlViewModel();

                    appBarControlViewModel.Init(SelectedPicture);
                    EventHandler handle = delegate { appBarControlView.Close(); };
                    appBarControlViewModel.RequestClose += handle;
                    appBarControlView.DataContext = appBarControlViewModel;
                    appBarControlView.ShowDialog();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPictureAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenPictureAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        // methods for removing / cancelling drag and drop functionality of menus
        // Removes the item from grid and adds removed item at the end of list in autohidepanel
        private void ProductCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is ProductTypes)
                {
                    ProductTypes way = e as ProductTypes;
                    ProductTypes.Remove(way);
                    ProductTypesMenulist.Add(way);
                }
                GeosApplication.Instance.Logger.Log("Method ProductCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ProductCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                    OptionsMenulist.Add(option);
                }
                GeosApplication.Instance.Logger.Log("Method OptionsCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

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
                    WaysMenulist.Add(way);
                }
                GeosApplication.Instance.Logger.Log("Method WayCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

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
                    DetectionsMenulist.Add(detection);
                }
                GeosApplication.Instance.Logger.Log("Method DetectionsCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

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
                    SpareParts sparePart = e as SpareParts;
                    SpareParts.Remove(sparePart);
                    SparePartsMenulist.Add(sparePart);
                }
                GeosApplication.Instance.Logger.Log("Method SparePartsCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

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
                    ConnectorFamilies way = e as ConnectorFamilies;
                    Families.Remove(way);
                    FamilyMenulist.Add(way);
                }
                GeosApplication.Instance.Logger.Log("Method FamilyCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FamilyCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CustomersCancelAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomersCancelAction()...", category: Category.Info, priority: Priority.Low);

                if (e is Customer)
                {
                    Customer way = e as Customer;
                    Customers.Remove(way);
                    CustomersMenulist.Add(way);
                }
                GeosApplication.Instance.Logger.Log("Method CustomersCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CustomersCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelButtonClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonClickAction()...", category: Category.Info, priority: Priority.Low);

                CatalogueItem catalogueItem = new CatalogueItem();
                Service.GoBack(catalogueItem);
                GeosApplication.Instance.Logger.Log("Method CancelButtonClickAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CancelButtonClickAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonClickAction()...", category: Category.Info, priority: Priority.Low);

                //return;
                CatalogueItem UpdateCatalogueItem = new CatalogueItem();
                UpdateCatalogueItem.Code = Code;
                UpdateCatalogueItem.IdCatalogueItem = ClonedCatalogueItem.IdCatalogueItem;
                UpdateCatalogueItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                if (IsCheckedCopyDescription == true)
                {
                    UpdateCatalogueItem.Description = Description;
                    UpdateCatalogueItem.Description_es = Description;
                    UpdateCatalogueItem.Description_fr = Description;
                    UpdateCatalogueItem.Description_pt = Description;
                    UpdateCatalogueItem.Description_ro = Description;
                    UpdateCatalogueItem.Description_ru = Description;
                    UpdateCatalogueItem.Description_zh = Description;
                }
                else
                {
                    UpdateCatalogueItem.Description = Description_en;
                    UpdateCatalogueItem.Description_es = Description_es;
                    UpdateCatalogueItem.Description_fr = Description_fr;
                    UpdateCatalogueItem.Description_pt = Description_pt;
                    UpdateCatalogueItem.Description_ro = Description_ro;
                    UpdateCatalogueItem.Description_ru = Description_ru;
                    UpdateCatalogueItem.Description_zh = Description_zh;
                }
                UpdateCatalogueItem.IdCPType = ProductTypes.Select(x => x.IdCPType).FirstOrDefault();
                UpdateCatalogueItem.ProductType = ProductTypes.FirstOrDefault();
                UpdateCatalogueItem.IdTemplate = SelectedTemplate.IdTemplate;
                UpdateCatalogueItem.Template = SelectedTemplate;
                UpdateCatalogueItem.Name = "Emdep";
                UpdateCatalogueItem.Name_es = "Emdep";
                UpdateCatalogueItem.Name_fr = "Emdep";
                UpdateCatalogueItem.Name_pt = "Emdep";
                UpdateCatalogueItem.Name_ro = "Emdep";
                UpdateCatalogueItem.Name_ru = "Emdep";
                UpdateCatalogueItem.Name_zh = "Emdep";
                UpdateCatalogueItem.IdStatus = SelectedStatus.IdLookupValue;
                UpdateCatalogueItem.Status = SelectedStatus;
                UpdateCatalogueItem.LastUpdate = LastUpdate;

                //UpdateCatalogueItem.WayList = Ways.ToList();
                //UpdateCatalogueItem.DetectionList = Detections.ToList();
                //UpdateCatalogueItem.SparePartList = SpareParts.ToList();
                //UpdateCatalogueItem.OptionList = Options.ToList();
                //UpdateCatalogueItem.FamilyList = Families.ToList();
                // UpdateCatalogueItem.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                if (ClonedCatalogueItem.Description != Description)
                {
                    //Change log
                }

                if (ClonedCatalogueItem.Code != Code)
                {
                    //Change log
                }

                UpdateCatalogueItem.WayList = new List<Ways>();

                //Deleted family
                foreach (Ways itemFamily in ClonedCatalogueItem.WayList)
                {
                    if (!Ways.Any(x => x.IdWays == itemFamily.IdWays))
                    {
                        Ways connectorFamilies = (Ways)itemFamily.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateCatalogueItem.WayList.Add(connectorFamilies);
                    }
                }
                //Added family
                foreach (Ways itemFamily in Ways)
                {
                    if (!ClonedCatalogueItem.WayList.Any(x => x.IdWays == itemFamily.IdWays))
                    {
                        Ways connectorFamilies = (Ways)itemFamily.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateCatalogueItem.WayList.Add(connectorFamilies);
                    }
                }


                //SpareParts
                UpdateCatalogueItem.SparePartList = new List<SpareParts>();

                //Deleted SpareParts
                foreach (SpareParts itemSpareParts in ClonedCatalogueItem.SparePartList)
                {
                    if (!SpareParts.Any(x => x.IdSpareParts == itemSpareParts.IdSpareParts))
                    {
                        SpareParts connectorFamilies = (SpareParts)itemSpareParts.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateCatalogueItem.SparePartList.Add(connectorFamilies);
                    }
                }
                //Added family
                foreach (SpareParts itemSpareParts in SpareParts)
                {
                    if (!ClonedCatalogueItem.SparePartList.Any(x => x.IdSpareParts == itemSpareParts.IdSpareParts))
                    {
                        SpareParts connectorFamilies = (SpareParts)itemSpareParts.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateCatalogueItem.SparePartList.Add(connectorFamilies);
                    }
                }

                
                UpdateCatalogueItem.OptionList = new List<Options>();

                //Deleted Options
                foreach (Options itemOptions in ClonedCatalogueItem.OptionList)
                {
                    if (!Options.Any(x => x.IdOptions == itemOptions.IdOptions))
                    {
                        Options connectorFamilies = (Options)itemOptions.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateCatalogueItem.OptionList.Add(connectorFamilies);
                    }
                }
                //Added family
                foreach (Options itemOptions in Options)
                {
                    if (!ClonedCatalogueItem.OptionList.Any(x => x.IdOptions == itemOptions.IdOptions))
                    {
                        Options connectorFamilies = (Options)itemOptions.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateCatalogueItem.OptionList.Add(connectorFamilies);
                    }
                }

                UpdateCatalogueItem.DetectionList = new List<Detections>();

                //Deleted Detections
                foreach (Detections itemDetection in ClonedCatalogueItem.DetectionList)
                {
                    if (!Detections.Any(x => x.IdDetections == itemDetection.IdDetections))
                    {
                        Detections connectorFamilies = (Detections)itemDetection.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateCatalogueItem.DetectionList.Add(connectorFamilies);
                    }
                }
                //Added Detections
                foreach (Detections itemDetection in Detections)
                {
                    if (!ClonedCatalogueItem.DetectionList.Any(x => x.IdDetections == itemDetection.IdDetections))
                    {
                        Detections connectorFamilies = (Detections)itemDetection.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateCatalogueItem.DetectionList.Add(connectorFamilies);
                    }
                }

                //Families
                UpdateCatalogueItem.FamilyList = new List<ConnectorFamilies>();

                //Deleted family
                foreach (ConnectorFamilies itemFamily in ClonedCatalogueItem.FamilyList)
                {
                    if (!Families.Any(x => x.IdFamily == itemFamily.IdFamily))
                    {
                        ConnectorFamilies connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateCatalogueItem.FamilyList.Add(connectorFamilies);
                    }
                }
                //Added family
                foreach (ConnectorFamilies itemFamily in Families)
                {
                    if (!ClonedCatalogueItem.FamilyList.Any(x => x.IdFamily == itemFamily.IdFamily))
                    {
                        ConnectorFamilies connectorFamilies = (ConnectorFamilies)itemFamily.Clone();
                        connectorFamilies.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateCatalogueItem.FamilyList.Add(connectorFamilies);
                    }
                }

                UpdateCatalogueItem.FileList = new List<CatalogueItemAttachedDoc>();
               // CatalogueItem

                foreach (CatalogueItemAttachedDoc item in ClonedCatalogueItem.FileList)
                {
                    if (!CatalogueFilesList.Any(x => x.IdCatalogueItemAttachedDoc == item.IdCatalogueItemAttachedDoc))
                    {
                        CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)item.Clone();
                        catalogueItemAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdateCatalogueItem.FileList.Add(catalogueItemAttachedDoc);
                    }
                }
                //Added ProductType
                foreach (CatalogueItemAttachedDoc item in CatalogueFilesList)
                {
                    if (!ClonedCatalogueItem.FileList.Any(x => x.IdCatalogueItemAttachedDoc == item.IdCatalogueItemAttachedDoc))
                    {
                        CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)item.Clone();
                        catalogueItemAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateCatalogueItem.FileList.Add(catalogueItemAttachedDoc);
                    }
                }
                //Updated ProductType
                foreach (CatalogueItemAttachedDoc originalCatalogue in ClonedCatalogueItem.FileList)
                {
                    if (CatalogueFilesList.Any(x => x.IdCatalogueItemAttachedDoc == originalCatalogue.IdCatalogueItemAttachedDoc))
                    {
                        CatalogueItemAttachedDoc catalogueAttachedDocUpdated = CatalogueFilesList.FirstOrDefault(x => x.IdCatalogueItemAttachedDoc == originalCatalogue.IdCatalogueItemAttachedDoc);
                        if ((catalogueAttachedDocUpdated.SavedFileName != originalCatalogue.SavedFileName) || (catalogueAttachedDocUpdated.OriginalFileName != originalCatalogue.OriginalFileName) || (catalogueAttachedDocUpdated.Description != originalCatalogue.Description))
                        {
                            CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)catalogueAttachedDocUpdated.Clone();
                            catalogueItemAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            catalogueItemAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdateCatalogueItem.FileList.Add(catalogueItemAttachedDoc);
                        }
                    }
                }

                var result = PCMService.UpdateCatalogueItem(UpdateCatalogueItem);
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CatalogueInformationUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                Service.GoBack(UpdateCatalogueItem);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonClickAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonClickAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonClickAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonClickAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeProductTypeListByTemplateAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeProductTypeListByTemplateAction()...", category: Category.Info, priority: Priority.Low);

                ProductTypesMenulist = new ObservableCollection<ProductTypes>(PCMService.GetProductTypesByTemplate(SelectedTemplate.IdTemplate));

                if (productTypes.Count() > 0)
                {
                    if (ProductTypesMenulist.Count() > 0)
                    {
                        if (ProductTypesMenulist.Where(a => a.IdCPType == productTypes[0].IdCPType).Count() == 0)
                        {
                            ProductTypes = new ObservableCollection<ProductTypes>();
                        }
                        else
                        {
                            ProductTypesMenulist = new ObservableCollection<ProductTypes>(ProductTypesMenulist.Where(a => a.IdCPType != productTypes[0].IdCPType));
                        }
                    }
                    else
                    {
                        ProductTypes = new ObservableCollection<ProductTypes>();
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeProductTypeListByTemplateAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeProductTypeListByTemplateAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeProductTypeListByTemplateAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        public void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PCMService.GetAllLanguages());
                LanguageSelected = Languages.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method AddLanguages() executed successfully", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        public void AddProductTypesMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddProductTypesMenu()...", category: Category.Info, priority: Priority.Low);

                ProductTypes = new ObservableCollection<ProductTypes>();
                ProductTypesMenulist = new ObservableCollection<ProductTypes>(PCMService.GetAllProductTypes());

                GeosApplication.Instance.Logger.Log("Method AddProductTypesMenu() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddProductTypesMenu() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddProductTypesMenu() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddProductTypesMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddOptionsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddOptionsMenu()...", category: Category.Info, priority: Priority.Low);

                Options = new ObservableCollection<Options>();
                OptionsMenulist = new ObservableCollection<Options>(PCMService.GetAllOptionList());

                GeosApplication.Instance.Logger.Log("Method AddOptionsMenu() executed successfully", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddProductTypesMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddWaysMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddWaysMenu()...", category: Category.Info, priority: Priority.Low);

                Ways = new ObservableCollection<Ways>();
                WaysMenulist = new ObservableCollection<Ways>(PCMService.GetAllWayList());

                GeosApplication.Instance.Logger.Log("Method AddWaysMenu() executed successfully", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddWaysMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddDetectionsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDetectionsMenu()...", category: Category.Info, priority: Priority.Low);

                Detections = new ObservableCollection<Detections>();
                DetectionsMenulist = new ObservableCollection<Detections>(PCMService.GetAllDetectionList());
                GeosApplication.Instance.Logger.Log("Method AddDetectionsMenu() executed successfully", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddDetectionsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddSparePartsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSparePartsMenu()...", category: Category.Info, priority: Priority.Low);

                SpareParts = new ObservableCollection<SpareParts>();
                SparePartsMenulist = new ObservableCollection<SpareParts>(PCMService.GetAllSparePartList());

                GeosApplication.Instance.Logger.Log("Method AddSparePartsMenu() executed successfully", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddSparePartsMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddFamilyMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFamilyMenu()...", category: Category.Info, priority: Priority.Low);

                Families = new ObservableCollection<ConnectorFamilies>();
                FamilyMenulist = new ObservableCollection<ConnectorFamilies>(PCMService.GetAllFamilies());

                GeosApplication.Instance.Logger.Log("Method AddFamilyMenu() executed successfully", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFamilyMenu() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddCustomersMenu()
        {
            //Customers = new ObservableCollection<Customer>();

            //CustomersMenulist = new ObservableCollection<Customer>();
            //CustomersMenulist.Add(new Customer { Id = 1, Name = "Customer 1" });
            //CustomersMenulist.Add(new Customer { Id = 2, Name = "Customer 2" });
            //CustomersMenulist.Add(new Customer { Id = 3, Name = "Customer 3" });
            //CustomersMenulist.Add(new Customer { Id = 4, Name = "Customer 4" });
            //CustomersMenulist.Add(new Customer { Id = 5, Name = "Customer 5" });
            //CustomersMenulist.Add(new Customer { Id = 6, Name = "Customer 6" });
            //CustomersMenulist.Add(new Customer { Id = 7, Name = "Customer 7" });
            //CustomersMenulist.Add(new Customer { Id = 8, Name = "Customer 8" });
            //CustomersMenulist.Add(new Customer { Id = 9, Name = "Customer 9" });
            //CustomersMenulist.Add(new Customer { Id = 10, Name = "Customer 10" });
        }

        public void AddChangeLogsMenu()
        {
            // DateTime dt = new DateTime();
            DateTime now = DateTime.Now;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime DayBeforeYesterDay = DateTime.Today.AddDays(-2);

            ChangeLogsMenulist = new ObservableCollection<ChangeLog>();
            ChangeLogsMenulist.Add(new ChangeLog { Id = 1, User = "Puja Sutar", Date = now, Action = "Template has been changed from Template 1 to Template 2" });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 2, User = "Swapnali Desai", Date = yesterday, Action = "Template has been changed from Template 1 to Template 2" });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 3, User = "Swapnil Kale", Date = now, Action = "Name has been changed from Emdep1243 to Emdep1243 STD" });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 4, User = "Chanchal Patil", Date = now, Action = "Language has been changed from Emdep1243 to Emdep1243 STD" });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 5, User = "Akshay Pawar", Date = yesterday, Action = "Picture_1 has been added." });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 6, User = "Swati Behere", Date = DayBeforeYesterDay, Action = "Picture_4 has been deleted." });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 7, User = "Sagar Khade", Date = now, Action = "File_1 has been added." });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 8, User = "Pranita Nikam", Date = now, Action = "File_4 has been deleted." });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 9, User = "Ashish Kadam", Date = yesterday, Action = "Link_1 has been added." });
            ChangeLogsMenulist.Add(new ChangeLog { Id = 10, User = "Megha Jadhav", Date = DayBeforeYesterDay, Action = "Link_4 has been deleted." });
        }

        public void AddPictures()
        {
            PictureList = new ObservableCollection<Picture>();
            PictureList.Add(new Picture { Id = 1, Name = "Picture_1.jpg", Image = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/tools.png")) });
            PictureList.Add(new Picture { Id = 2, Name = "Picture_2.jpg", Image = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/file.png")) });
            PictureList.Add(new Picture { Id = 3, Name = "Picture_3.jpg", Image = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/link.png")) });
            SelectedPicture = PictureList.First();
        }

        public void AddFiles(uint IdCatalogueItem)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFiles()...", category: Category.Info, priority: Priority.Low);

                CatalogueFilesList = new ObservableCollection<CatalogueItemAttachedDoc>(PCMService.GetCatalogueItemAttachedDocsByIdCatalogueItem(IdCatalogueItem));

                if (CatalogueFilesList.Count > 0)
                    SelectedCatalogueFile = CatalogueFilesList.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method AddFiles()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddFiles() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddFiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddFiles() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddLinks()
        {
            Links = new ObservableCollection<Link>();
            Links.Add(new Link { Id = 1, Name = "https://www.emdep.com" });
            Links.Add(new Link { Id = 2, Name = "http://www.mecwide.com" });
            Links.Add(new Link { Id = 3, Name = "https://www.emdep.com/geos" });
        }

        public void AddUser()
        {
            UserList = new ObservableCollection<User>();
            UserList.Add(new User { Id = 1, Name = "User 1" });
            UserList.Add(new User { Id = 2, Name = "User 2" });
            UserList.Add(new User { Id = 3, Name = "User 3" });
            UserList.Add(new User { Id = 4, Name = "User 4" });
            UserList.Add(new User { Id = 5, Name = "User 5" });
        }

        private void AddCatalogueItemsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCatalogueItemsMenu()...", category: Category.Info, priority: Priority.Low);

                CatalogueItemsTypes = new ObservableCollection<CatalogueItem>();
                CatalogueItemsMenulist = new ObservableCollection<CatalogueItem>(); 

                GeosApplication.Instance.Logger.Log("Method AddCatalogueItemsMenu() executed successfully", category: Category.Info, priority: Priority.Low);
                // new ObservableCollection<CatalogueItem>(PCMService.GetAllCatalogueItems());
                //CatalogueItemsMenulist = new ObservableCollection<CatalogueItem>(PCMService.GetAllCatalogueItems());
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCatalogueItemsMenu() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCatalogueItemsMenu() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCatalogueItemsMenu()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddTemplatesMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddTemplatesMenu()...", category: Category.Info, priority: Priority.Low);

                templates = new ObservableCollection<Template>();
                TemplatesMenulist = new ObservableCollection<Template>(PCMService.GetAllTemplates());
                GeosApplication.Instance.Logger.Log("Method AddTemplatesMenu() executed successfully", category: Category.Info, priority: Priority.Low);

            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddTemplatesMenu() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddTemplatesMenu() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddTemplatesMenu()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                StatusList = PCMService.GetLookupValues(45).ToList();
                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);

            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        protected override void OnParameterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnParameterChanged()...", category: Category.Info, priority: Priority.Low);

                CatalogueItem catalogueItem = (CatalogueItem)parameter;

                catalogueItem = PCMService.GetCatalogueItemByIdCatalogueItem(catalogueItem.IdCatalogueItem);

                ClonedCatalogueItem = (CatalogueItem)catalogueItem.Clone();

                ModifyBy = catalogueItem.ModifiedBy;
                Name = catalogueItem.Name;
                Description = catalogueItem.Description;
                Description_en = catalogueItem.Description;
                Description_es = catalogueItem.Description_es;
                Description_fr = catalogueItem.Description_fr;
                Description_pt = catalogueItem.Description_pt;
                Description_ro = catalogueItem.Description_ro;
                Description_ru = catalogueItem.Description_ru;
                Description_zh = catalogueItem.Description_zh;
                LastUpdate = catalogueItem.LastUpdate.Value;
                SelectedCreated = catalogueItem.CreatedIn.Value;
                Code = catalogueItem.Code;
                SelectedTemplate = TemplatesMenulist.FirstOrDefault(x => x.IdTemplate == catalogueItem.Template.IdTemplate);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == catalogueItem.Status.IdLookupValue);

                ProductTypes = new ObservableCollection<ProductTypes>();
                productTypes.Add(new ProductTypes { IdCPType = catalogueItem.ProductType.IdCPType, Name = catalogueItem.ProductType.Name });


                Families = new ObservableCollection<ConnectorFamilies>(catalogueItem.FamilyList);
                HashSet<ulong> idFamilys = new HashSet<ulong>(Families.Select(x => x.IdFamily));
                FamilyMenulist = new ObservableCollection<ConnectorFamilies>(FamilyMenulist.Where(x => !idFamilys.Contains(x.IdFamily)));

                Ways = new ObservableCollection<Ways>(catalogueItem.WayList);
                HashSet<uint> idWays = new HashSet<uint>(Ways.Select(x => x.IdWays));
                WaysMenulist = new ObservableCollection<Ways>(WaysMenulist.Where(x => !idWays.Contains(x.IdWays)));

                Detections = new ObservableCollection<Detections>(catalogueItem.DetectionList);
                HashSet<uint> idDetections = new HashSet<uint>(Detections.Select(x => x.IdDetections));
                DetectionsMenulist = new ObservableCollection<Detections>(DetectionsMenulist.Where(x => !idDetections.Contains(x.IdDetections)));

                Options = new ObservableCollection<Options>(catalogueItem.OptionList);
                HashSet<uint> idOptions = new HashSet<uint>(Options.Select(x => x.IdOptions));
                OptionsMenulist = new ObservableCollection<Options>(OptionsMenulist.Where(x => !idOptions.Contains(x.IdOptions)));

                SpareParts = new ObservableCollection<SpareParts>(catalogueItem.SparePartList);
                HashSet<uint> idSpareParts = new HashSet<uint>(SpareParts.Select(x => x.IdSpareParts));
                SparePartsMenulist = new ObservableCollection<SpareParts>(SparePartsMenulist.Where(x => !idSpareParts.Contains(x.IdSpareParts)));

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCheckedCopyDescription = true;
                }
                else
                {
                    IsCheckedCopyDescription = false;
                }

                AddFiles(catalogueItem.IdCatalogueItem);

                GeosApplication.Instance.Logger.Log("Method OnParameterChanged() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnParameterChanged() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnParameterChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OnParameterChanged() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            base.OnParameterChanged(parameter);
        }

        private void AddNewProductItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewProductItem()...", category: Category.Info, priority: Priority.Low);

                OldAddProductTypeView addNewProductTypeView = new OldAddProductTypeView();
                OldAddProductTypeViewModel addNewProductTypeItemViewModel = new OldAddProductTypeViewModel();
                EventHandler handle = delegate { addNewProductTypeView.Close(); };
                addNewProductTypeItemViewModel.RequestClose += handle;
                addNewProductTypeItemViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddProductTypeHeader").ToString();
                addNewProductTypeItemViewModel.IsNew = true;
                addNewProductTypeItemViewModel.InitTemplate(SelectedTemplate);
                addNewProductTypeView.DataContext = addNewProductTypeItemViewModel;
                addNewProductTypeView.ShowDialog();
                if (addNewProductTypeItemViewModel.IsSave)
                {
                    ProductTypes productType = new ProductTypes();
                    productType.IdCPType = (byte)addNewProductTypeItemViewModel.NewProductType.IdCPType;
                    productType.Name = addNewProductTypeItemViewModel.NewProductType.Name;
                    ProductTypesMenulist.Add(productType);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewProductItem() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewProductItem() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewOption(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewOption()...", category: Category.Info, priority: Priority.Low);

                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddOptionHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = true;
                addOptionWayDetectionSparePartViewModel.Init(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();
                if (addOptionWayDetectionSparePartViewModel.IsSave)
                {
                    Options tempOption = new Options();
                    tempOption.IdOptions = addOptionWayDetectionSparePartViewModel.NewOption.IdDetections;
                    tempOption.IdDetectionType = (byte)addOptionWayDetectionSparePartViewModel.NewOption.IdDetectionType;
                    tempOption.Name = addOptionWayDetectionSparePartViewModel.NewOption.Name;
                    tempOption.Code = addOptionWayDetectionSparePartViewModel.NewOption.Code;
                    tempOption.IdTestType = addOptionWayDetectionSparePartViewModel.NewOption.IdTestType;
                    OptionsMenulist.Add(tempOption);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewOption() executed successfully", category: Category.Info, priority: Priority.Low);

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

                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddWayHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = true;
                addOptionWayDetectionSparePartViewModel.Init(System.Windows.Application.Current.FindResource("CaptionWay").ToString());
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();
                if (addOptionWayDetectionSparePartViewModel.IsSave)
                {
                    Ways tempWay = new Ways();
                    tempWay.IdWays = addOptionWayDetectionSparePartViewModel.NewWay.IdDetections;
                    tempWay.IdDetectionType = (byte)addOptionWayDetectionSparePartViewModel.NewWay.IdDetectionType;
                    tempWay.Name = addOptionWayDetectionSparePartViewModel.NewWay.Name;
                    tempWay.Code = addOptionWayDetectionSparePartViewModel.NewWay.Code;
                    tempWay.IdTestType = addOptionWayDetectionSparePartViewModel.NewWay.IdTestType;
                    WaysMenulist.Add(tempWay);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewWay() executed successfully", category: Category.Info, priority: Priority.Low);

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

                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddDetectionHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = true;
                addOptionWayDetectionSparePartViewModel.Init(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();
                if (addOptionWayDetectionSparePartViewModel.IsSave)
                {
                    Detections tempDetection = new Detections();
                    tempDetection.IdDetections = addOptionWayDetectionSparePartViewModel.NewDetection.IdDetections;
                    tempDetection.IdDetectionType = (byte)addOptionWayDetectionSparePartViewModel.NewDetection.IdDetectionType;
                    tempDetection.Name = addOptionWayDetectionSparePartViewModel.NewDetection.Name;
                    tempDetection.Code = addOptionWayDetectionSparePartViewModel.NewDetection.Code;
                    tempDetection.IdTestType = addOptionWayDetectionSparePartViewModel.NewDetection.IdTestType;
                    DetectionsMenulist.Add(tempDetection);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewDetection() executed successfully", category: Category.Info, priority: Priority.Low);

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

                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;
                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddSparePartHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = true;
                addOptionWayDetectionSparePartViewModel.Init(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();
                if (addOptionWayDetectionSparePartViewModel.IsSave)
                {
                    SpareParts tempSparePart = new SpareParts();
                    tempSparePart.IdSpareParts = addOptionWayDetectionSparePartViewModel.NewSparePart.IdDetections;
                    tempSparePart.IdDetectionType = (byte)addOptionWayDetectionSparePartViewModel.NewSparePart.IdDetectionType;
                    tempSparePart.Name = addOptionWayDetectionSparePartViewModel.NewSparePart.Name;
                    tempSparePart.Code = addOptionWayDetectionSparePartViewModel.NewSparePart.Code;
                    tempSparePart.IdTestType = addOptionWayDetectionSparePartViewModel.NewSparePart.IdTestType;
                    SparePartsMenulist.Add(tempSparePart);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewSparePart() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewSparePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditProductType(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditProductType()...", category: Category.Info, priority: Priority.Low);

                OldAddProductTypeView addNewProductTypeView = new OldAddProductTypeView();
                OldAddProductTypeViewModel addNewProductTypeItemViewModel = new OldAddProductTypeViewModel();
                EventHandler handle = delegate { addNewProductTypeView.Close(); };
                addNewProductTypeItemViewModel.RequestClose += handle;

                addNewProductTypeItemViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditProductTypeHeader").ToString();
                addNewProductTypeItemViewModel.IsNew = false;
                addNewProductTypeItemViewModel.EditInit(SelectedTemplate, SelectedCpType);
                addNewProductTypeView.DataContext = addNewProductTypeItemViewModel;
                addNewProductTypeView.ShowDialog();
                if (addNewProductTypeItemViewModel.IsSave)
                {
                    // ProductTypes UpdatedproductType = new ProductTypes();
                    SelectedCpType.IdCPType = (byte)addNewProductTypeItemViewModel.UpdateProductType.IdCPType;
                    SelectedCpType.Name = addNewProductTypeItemViewModel.UpdateProductType.Name;
                    SelectedCpType.Description = addNewProductTypeItemViewModel.UpdateProductType.Description;
                }

                GeosApplication.Instance.Logger.Log("Method EditProductType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditProductType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditOption(object obj)
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method EditOption()...", category: Category.Info, priority: Priority.Low);

            //    EditOptionWayDetectionSparePartView editOptionWayDetectionSparePartView = new EditOptionWayDetectionSparePartView();
            //    EditOptionWayDetectionSparePartViewModel editOptionWayDetectionSparePartViewModel = new EditOptionWayDetectionSparePartViewModel();
            //    EventHandler handle = delegate { editOptionWayDetectionSparePartView.Close(); };
            //    editOptionWayDetectionSparePartViewModel.RequestClose += handle;

            //    editOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOptionHeader").ToString();
            //    editOptionWayDetectionSparePartViewModel.IsNew = false;
            //    editOptionWayDetectionSparePartViewModel.InitOption(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());

            //    if (SelectedOptionsType == null)
            //        return;

            //    Options selectedOption = SelectedOptionsType.Cast<Options>().ToList().LastOrDefault();

            //    editOptionWayDetectionSparePartViewModel.EditInitOptions(selectedOption);
            //    editOptionWayDetectionSparePartView.DataContext = editOptionWayDetectionSparePartViewModel;
            //    editOptionWayDetectionSparePartView.ShowDialog();

            //    if (editOptionWayDetectionSparePartViewModel.IsSave)
            //    {
            //        SelectedOptionsType.Cast<Options>().ToList().LastOrDefault().IdDetections = editOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
            //        SelectedOptionsType.Cast<Options>().ToList().LastOrDefault().Name = editOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
            //        SelectedOptionsType.Cast<Options>().ToList().LastOrDefault().Description = editOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
            //    }
            //    GeosApplication.Instance.Logger.Log("Method EditOption()....executed successfully", category: Category.Info, priority: Priority.Low);

            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            //}


            AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
            AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
            EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
            addOptionWayDetectionSparePartViewModel.RequestClose += handle;

            addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOptionHeader").ToString();
            addOptionWayDetectionSparePartViewModel.IsNew = false;
            addOptionWayDetectionSparePartViewModel.InitType(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());

            if (SelectedOptionsType == null)
                return;

            Options selectedOption = SelectedOptionsType.Cast<Options>().ToList().LastOrDefault();

            addOptionWayDetectionSparePartViewModel.EditInitOptions(selectedOption);
            addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
            addOptionWayDetectionSparePartView.ShowDialog();
            if (addOptionWayDetectionSparePartViewModel.IsSave)
            {
                // ProductTypes UpdatedproductType = new ProductTypes();
                SelectedOptionsType.Cast<Options>().ToList().LastOrDefault().IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                SelectedOptionsType.Cast<Options>().ToList().LastOrDefault().Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                SelectedOptionsType.Cast<Options>().ToList().LastOrDefault().Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
            }
        }

        private void EditWay(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWay()...", category: Category.Info, priority: Priority.Low);
                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditWayHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = false;
                addOptionWayDetectionSparePartViewModel.InitType(System.Windows.Application.Current.FindResource("CaptionWay").ToString());

                if (SelectedWaysType == null)
                    return;

                Ways selectedWay = SelectedWaysType.Cast<Ways>().ToList().LastOrDefault();

                addOptionWayDetectionSparePartViewModel.EditInitWays(selectedWay);
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsSave)
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

                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDetectionHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = false;
                addOptionWayDetectionSparePartViewModel.InitType(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());

                if (SelectedDetectionsType == null)
                    return;

                Detections selectedDetection = SelectedDetectionsType.Cast<Detections>().ToList().LastOrDefault();

                addOptionWayDetectionSparePartViewModel.EditInitDetections(selectedDetection);
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsSave)
                {
                    SelectedDetectionsType.Cast<Detections>().ToList().LastOrDefault().IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                    SelectedDetectionsType.Cast<Detections>().ToList().LastOrDefault().Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                    SelectedDetectionsType.Cast<Detections>().ToList().LastOrDefault().Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
                }
                GeosApplication.Instance.Logger.Log("Method EditDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

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

                AddOptionWayDetectionSparePartView addOptionWayDetectionSparePartView = new AddOptionWayDetectionSparePartView();
                AddOptionWayDetectionSparePartViewModel addOptionWayDetectionSparePartViewModel = new AddOptionWayDetectionSparePartViewModel();
                EventHandler handle = delegate { addOptionWayDetectionSparePartView.Close(); };
                addOptionWayDetectionSparePartViewModel.RequestClose += handle;

                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString();
                addOptionWayDetectionSparePartViewModel.IsNew = false;
                addOptionWayDetectionSparePartViewModel.InitType(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());

                if (SelectedSparePartsType == null)
                    return;

                SpareParts selectedSpareParts = SelectedSparePartsType.Cast<SpareParts>().ToList().LastOrDefault();

                addOptionWayDetectionSparePartViewModel.EditInitSparePart(selectedSpareParts);
                addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                addOptionWayDetectionSparePartView.ShowDialog();

                if (addOptionWayDetectionSparePartViewModel.IsSave)
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

        private void NextImage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NextImage()...", category: Category.Info, priority: Priority.Low);

                if (SelectedPicture.Id != PictureList.Count && PictureList.Count > 0)
                {
                    SelectedItem = new Photo();

                    var item = PictureList.IndexOf(SelectedPicture);
                    SelectedPicture = PictureList[item + 1];
                    SelectedItem.source = selectedPicture.Image;
                }
                GeosApplication.Instance.Logger.Log("Method NextImage()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NextImage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PreviousImage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PreviousImage()...", category: Category.Info, priority: Priority.Low);

                if (SelectedPicture.Id <= PictureList.Count && PictureList.Count > 0)
                {
                    SelectedItem = new Photo();

                    var item = PictureList.IndexOf(SelectedPicture);
                    SelectedPicture = PictureList[item - 1];
                    SelectedItem.source = selectedPicture.Image;
                }
                GeosApplication.Instance.Logger.Log("Method PreviousImage()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PreviousImage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyDescription == false)
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
                }
                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UncheckedCopyDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddRowItemFiles(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddRowItemFiles()...", category: Category.Info, priority: Priority.Low);
                //TableView detailView = (TableView)obj;
                //CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)detailView.DataControl.CurrentItem;
                AddFileInCatalogueView addFileInCatalogueView = new AddFileInCatalogueView();
                AddFileInCatalogueViewModel addFileInCatalogueViewModel = new AddFileInCatalogueViewModel();
                EventHandler handle = delegate { addFileInCatalogueView.Close(); };
                addFileInCatalogueViewModel.RequestClose += handle;
                addFileInCatalogueViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addFileInCatalogueViewModel.IsNew = false;
                //EmployeeLeave employeeLeave = (EmployeeLeave)detailView.DataControl.CurrentItem;
                //addFileInCatalogueViewModel.EditInit(Name, Description, SelectedCatalogueFile);
                //addFileInCatalogueViewModel.EditInit(catalogueItemAttachedDoc);
                addFileInCatalogueView.DataContext = addFileInCatalogueViewModel;
                addFileInCatalogueView.ShowDialog();

                if (addFileInCatalogueViewModel.IsSave == true)
                {
                    CatalogueFilesList.Add(addFileInCatalogueViewModel.SelectedCatalogueFile);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddRowItemFiles() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditFileGridDoubleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditFileGridDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)detailView.DataControl.CurrentItem;
                AddFileInCatalogueView addFileInCatalogueView = new AddFileInCatalogueView();
                AddFileInCatalogueViewModel addFileInCatalogueViewModel = new AddFileInCatalogueViewModel();
                EventHandler handle = delegate { addFileInCatalogueView.Close(); };
                addFileInCatalogueViewModel.RequestClose += handle;
                addFileInCatalogueViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                addFileInCatalogueViewModel.IsNew = false;
                addFileInCatalogueViewModel.EditInit(catalogueItemAttachedDoc);
                addFileInCatalogueView.DataContext = addFileInCatalogueViewModel;
                addFileInCatalogueView.ShowDialog();

                if (addFileInCatalogueViewModel.IsSave == true)
                {
                    //CatalogueFilesList.Add(addFileInCatalogueViewModel.SelectedCatalogueFile);
                    //SelectedCpType.IdCPType = (byte)addNewProductTypeItemViewModel.UpdateProductType.IdCPType;
                    //SelectedCpType.Name = addNewProductTypeItemViewModel.UpdateProductType.Name;
                    //SelectedCpType.Description = addNewProductTypeItemViewModel.UpdateProductType.Description;
                    //SelectedCatalogueFile=addFileInCatalogueViewModel.SelectedCatalogueFile;
                    SelectedCatalogueFile.IdCatalogueItemAttachedDoc = addFileInCatalogueViewModel.IdCatalogueItemAttachedDoc;
                    SelectedCatalogueFile.SavedFileName = addFileInCatalogueViewModel.FileName;
                    SelectedCatalogueFile.Description = addFileInCatalogueViewModel.Description;
                    SelectedCatalogueFile.CatalogueItemAttachedDocInBytes = addFileInCatalogueViewModel.FileInBytes;
                    SelectedCatalogueFile.OriginalFileName = addFileInCatalogueViewModel.CatalogueItemFileName;
                   // SelectedCatalogueFile = catalogueItemAttachedDoc;

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFileGridDoubleClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteRowItemFiles(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteRowItemFiles()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    CatalogueFilesList.Remove(SelectedCatalogueFile);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteRowItemFiles()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteRowItemFiles()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        #endregion


    }

    public class Picture
    {
        private Int32 id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ImageSource Image { get; set; }
        public virtual double Scale { get; set; }
        public Size ViewSize { get; set; }

        public Picture()
        {
            Scale = 1.1;
            ViewSize = new Size(double.NaN, double.NaN);
        }
    }
   

    public class Customer
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public override string ToString()
        {
            return string.Format("Customer : {0}", Name); // base.ToString();
        }
    }


    public class ChangeLog
    {
        public int Id { get; set; }
        public String User { get; set; }
        public DateTime Date { get; set; }
        public String Action { get; set; }
    }

    //public class Language
    //{
    //    public int Id { get; set; }
    //    public String Name { get; set; }
    //}

    public class Files
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
        //public File Attachment { get; set; }
    }

    public class Link
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }

}
