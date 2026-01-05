using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
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
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class EditPCMArticleViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged, IDataErrorInfo
    {
        public void Dispose()
        {

        }

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

        #region Declaration

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        public ObservableCollection<PCMArticleCategory> tempCategoryList { get; set; }
        public IList<LookupValue> tempStatusList { get; set; }
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
        private ObservableCollection<PCMArticleLogEntry> changeLogList;
        private PCMArticleLogEntry selectedArticleChangeLog;

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
      

        #endregion

        #region Properties

        public Articles ClonedArticle
        {
            get { return clonedArticle; }
            set
            {
                clonedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedArticle"));
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
            }
        }

        public float Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Length"));
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
            }
        }

        public ImageSource OldReferenceImage
        {
            get { return oldReferenceImage; }
            set
            {
                oldReferenceImage = value; OnPropertyChanged(new PropertyChangedEventArgs("OldReferenceImage"));
            }
        }

        public bool IsReferenceImageExist
        {
            get { return isReferenceImageExist; }
            set
            {
                isReferenceImageExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReferenceImageExist"));
            }
        }

        public string ArticleWeightSymbol
        {
            get { return articleWeightSymbol; }
            set
            {
                articleWeightSymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleWeightSymbol"));
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


        #endregion

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }

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

        public ICommand EditImageCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }

        public ICommand ExportToExcelCommand { get; set; }

        public ICommand AddFileCommand { get; set; }

        public ICommand OpenSelectedImageCommand { get; set; }
        public ICommand OpenImageGalleryCommand { get; set; }
        public ICommand RestrictOpeningPopUpCommand { get; set; }

        public ICommand OpenPDFDocumentCommand { get; set; }

        public ICommand ShowReferenceViewCommand { get; set; }
        public ICommand ShowDescriptionViewCommand { get; set; }


        #endregion

        #region Constructor

        public EditPCMArticleViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Constructor EditPCMArticleViewModel()..."), category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;

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

                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));


                OpenImageGalleryCommand = new RelayCommand(new Action<object>(OpenImageGalleryAction));
                RestrictOpeningPopUpCommand = new DelegateCommand<object>(RestrictOpeningPopUpAction);
                OpenSelectedImageCommand = new DelegateCommand<object>(OpenSelectedImageAction);

                ShowReferenceViewCommand = new DelegateCommand<object>(ShowReferenceViewCommandAction);
                ShowDescriptionViewCommand = new DelegateCommand<object>(ShowDescriptionViewCommandAction);

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


                GeosApplication.Instance.Logger.Log(string.Format("Constructor EditPCMArticleViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor EditPCMArticleViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

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
                        List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 249, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
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
                        List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 249, Code = x.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
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
                    List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 249, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();

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
                    List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 249, Code = x.Reference, Name = x.Description, Remarks = "", MinimumElements = 1, MaximumElements = 1 }).ToList();
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
                        List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 250, Code = x.ProductType.Reference, Name = x.Name, Remarks = "" }).ToList();
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
                        List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 250, Code = x.Reference, Name = x.Name, Remarks = "" }).ToList();
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
                    List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 250, Code = x.ProductType.Reference, Name = x.Name, Remarks = "" }).ToList();
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
                    List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 250, Code = x.Reference, Name = x.Description, Remarks = "" }).ToList();
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
                        List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 251, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251 }).ToList();
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
                        List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 251, Code = x.Reference, Name = x.Name, Remarks = "",  IdRelationshipType = 246 }).ToList();


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
                    List<ArticleCompatibility> newRecords = data.Records.OfType<ProductTypesTemplate>().Select(x => new ArticleCompatibility { IdCPtypeCompatibility = (byte)x.IdCPType, IdTypeCompatibility = 251, Code = x.ProductType.Reference, Name = x.Name, Remarks = "", IdRelationshipType = 251 }).ToList();
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
                    List<ArticleCompatibility> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new ArticleCompatibility { IdArticleCompatibility = x.IdArticle, IdTypeCompatibility = 251, Code = x.Reference, Name = x.Description, Remarks = "", IdRelationshipType = 251 }).ToList();
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


        #endregion

        #region Methods

        public void Init()
        {
            MaximizedElementPosition = PCMCommon.Instance.SetMaximizedElementPosition();

            FillCategoryList();
            FillStatusList();

            FillModuleMenuList();
            FillArticleMenuList();
            FillReferenceView();
            FillRelationShipList();
            AddChangeLogsMenu();
        }


        public void EditInit(Articles selectedArticle)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);

                Init();

                Articles temp = (PCMService.GetArticleByIdArticle_V2060(selectedArticle.IdArticle));
                ClonedArticle = (Articles)temp.Clone();

                Reference = temp.Reference;
                Description = temp.Description;

                IdPCMArticleCategory = temp.IdPCMArticleCategory;
                IdArticle = temp.IdArticle;

                Width = temp.Width;
                Height = temp.Height;
                Length = temp.Length;

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
                if (temp.IdPCMStatus !=null)
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdPCMStatus);
                }
                else
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 225);
                    ClonedArticle.IdPCMStatus = 225;
                }
                WarehouseStatus = temp.WarehouseStatus;
                Supplier = temp.SupplierName;

                //Compatiblity
                if (temp.ArticleCompatibilityList != null)
                {
                    MandatoryList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 249));
                    SuggestedList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 250));
                    IncompatibleList = new ObservableCollection<ArticleCompatibility>(temp.ArticleCompatibilityList.Where(a => a.IdTypeCompatibility == 251));
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
                    PCMArticleImage Image = new PCMArticleImage();
                    Image.PCMArticleImageInBytes = temp.ArticleImageInBytes;
                    Image.IsWarehouseImage = 1;
                    Image.SavedFileName = Reference;
                    Image.OriginalFileName = Reference;
                    Image.Position = 1;
                    ImagesList.Add(Image);
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
                FourRecordsArticleImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).Take(4).ToList());

                //Attachments
                ArticleFilesList = new ObservableCollection<ArticleDocument>(temp.PCMArticleAttachmentList);
                FourRecordsArticleFilesList= new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.IdArticle).Take(4).ToList());
                 
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList..."), category: Category.Info, priority: Priority.Low);

                CategoryList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                UpdatePCMCategoryCount();
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

                tempStatusList = PCMService.GetLookupValues(45);
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

        public void AddChangeLogsMenu()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangeLogsMenu()...", category: Category.Info, priority: Priority.Low);

                ArticleChangeLogList = new ObservableCollection<PCMArticleLogEntry>();
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
                        //[001]
                         if(tempObject.IsWarehouseImage==1)
                        {
                            tempProductTypeImage_old.IsWarehouseImage = 1;

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
                        //[002]
                        if (tempObject.IsWarehouseImage == 1)
                        {
                            tempProductTypeImage.IsWarehouseImage = 1;

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

                        if(addEditPCMArticleImageViewModel.OldDefaultImage.IsWarehouseImage==1)
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

                AddEditFileInPCMArticleView addEditFileInPCMArticleView = new AddEditFileInPCMArticleView();
                AddEditFileInPCMArticleViewModel addEditFileInPCMArticleViewModel = new AddEditFileInPCMArticleViewModel();
                EventHandler handle = delegate { addEditFileInPCMArticleView.Close(); };
                addEditFileInPCMArticleViewModel.RequestClose += handle;
                addEditFileInPCMArticleViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addEditFileInPCMArticleViewModel.IsNew = true;
                addEditFileInPCMArticleView.DataContext = addEditFileInPCMArticleViewModel;
                //var ownerInfo = (obj as FrameworkElement);
                //addEditFileInPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                addEditFileInPCMArticleView.ShowDialog();

                if (addEditFileInPCMArticleViewModel.IsSave)
                {
                    ArticleFilesList.Add(addEditFileInPCMArticleViewModel.SelectedArticleFile);
                    SelectedArticleFile = addEditFileInPCMArticleViewModel.SelectedArticleFile;
                    FourRecordsArticleFilesList = new ObservableCollection<ArticleDocument>(ArticleFilesList.OrderBy(x => x.IdArticle).Take(4).ToList());
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
               SelectedArticleFile =(ArticleDocument)obj;
                
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

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);

                ChangeLogList = new ObservableCollection<PCMArticleLogEntry>();
                allowValidation = true;

                GroupBox groupBox = (GroupBox)obj;


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

                allowValidation = true;

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatus"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));

                if (error != null)
                {
                    return;
                }
                
                UpdatedArticle = new Articles();

                UpdatedArticle.IdPCMArticleCategory = SelectedCategory.IdPCMArticleCategory;
                UpdatedArticle.IdPCMStatus = SelectedStatus.IdLookupValue;
                UpdatedArticle.IdArticle = IdArticle;
                UpdatedArticle.Reference = Reference;
                //// Compatibility

                UpdatedArticle.ArticleCompatibilityList = new List<ArticleCompatibility>();

                // Delete Compatibility
                foreach (ArticleCompatibility item in ClonedArticle.ArticleCompatibilityList)
                {
                    if (item.IdTypeCompatibility == 249 && MandatoryList != null && !MandatoryList.Any(x => x.IdCompatibility == item.IdCompatibility))
                    {
                        ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                        articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMandatoryDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                    }
                    if (item.IdTypeCompatibility == 250 && SuggestedList != null && !SuggestedList.Any(x => x.IdCompatibility == item.IdCompatibility))
                    {
                        ArticleCompatibility articleCompatibility = (ArticleCompatibility)item.Clone();
                        articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);
                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogSuggestedDelete").ToString(), item.IdCPtypeCompatibility > 0 ? "Module" : "Article", item.Name) });
                    }
                    if (item.IdTypeCompatibility == 251 && IncompatibleList != null && !IncompatibleList.Any(x => x.IdCompatibility == item.IdCompatibility))
                    {
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
                    if (originalCompatibility.IdTypeCompatibility == 249 && MandatoryList != null && MandatoryList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
                    {
                        ArticleCompatibility MandatoryUpdated = MandatoryList.FirstOrDefault(x => x.IdCompatibility == originalCompatibility.IdCompatibility);
                        if ((MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements) || (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements) || (MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                        {
                            ArticleCompatibility articleCompatibility = (ArticleCompatibility)MandatoryUpdated.Clone();
                            articleCompatibility.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            articleCompatibility.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdatedArticle.ArticleCompatibilityList.Add(articleCompatibility);

                            if (MandatoryUpdated.MinimumElements != originalCompatibility.MinimumElements)
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMinUpdate").ToString(), originalCompatibility.MinimumElements, MandatoryUpdated.MinimumElements) });

                            if (MandatoryUpdated.MaximumElements != originalCompatibility.MaximumElements)
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogMaxUpdate").ToString(), originalCompatibility.MaximumElements, MandatoryUpdated.MaximumElements) });

                            if ((MandatoryUpdated.Remarks != originalCompatibility.Remarks))
                            {
                                if (string.IsNullOrEmpty(MandatoryUpdated.Remarks))
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", MandatoryUpdated.IdTypeCompatibility == 249 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                else
                                {
                                    if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 249 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    else
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, MandatoryUpdated.Remarks, MandatoryUpdated.IdTypeCompatibility == 249 ? "Mandatory" : MandatoryUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                }
                            }
                        }
                    }

                    if (originalCompatibility.IdTypeCompatibility == 250 && SuggestedList != null && SuggestedList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
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
                                if (string.IsNullOrEmpty(SuggestedUpdated.Remarks))
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", SuggestedUpdated.IdTypeCompatibility == 249 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                else
                                {
                                    if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 249 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    else
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, SuggestedUpdated.Remarks, SuggestedUpdated.IdTypeCompatibility == 249 ? "Mandatory" : SuggestedUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                }
                            }
                        }
                    }

                    if (originalCompatibility.IdTypeCompatibility == 251 && IncompatibleList != null && IncompatibleList.Any(x => x.IdCompatibility == originalCompatibility.IdCompatibility))
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
                                if (originalCompatibility.RelationshipType == null)
                                    originalCompatibility.RelationshipType = new LookupValue();

                                string relationShip = RelationShipList.FirstOrDefault(a => a.IdLookupValue == articleCompatibility.IdRelationshipType).Value;

                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRelationshipUpdate").ToString(), originalCompatibility.RelationshipType.Value, relationShip) });
                            }

                            if ((IncompatibleUpdated.Quantity != originalCompatibility.Quantity))
                            {
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
                                if (string.IsNullOrEmpty(IncompatibleUpdated.Remarks))
                                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, "None", IncompatibleUpdated.IdTypeCompatibility == 249 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                else
                                {
                                    if (string.IsNullOrEmpty(originalCompatibility.Remarks))
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), "None", IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 249 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                    else
                                        ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogRemarkUpdate").ToString(), originalCompatibility.Remarks, IncompatibleUpdated.Remarks, IncompatibleUpdated.IdTypeCompatibility == 249 ? "Mandatory" : IncompatibleUpdated.IdTypeCompatibility == 250 ? "Suggested" : "Incompatible") });
                                }
                            }
                        }
                    }
                }

                //Images
                UpdatedArticle.PCMArticleImageList = new List<PCMArticleImage>();

                foreach (PCMArticleImage item in ClonedArticle.PCMArticleImageList)
                {
                   
                        if (!ImagesList.Any(x => x.IdPCMArticleImage == item.IdPCMArticleImage))
                        {
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
                        if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName) || (articleImageUpdated.SavedFileName != originalArticleTypeImage.SavedFileName) || (articleImageUpdated.Description != originalArticleTypeImage.Description) ||
                            (articleImageUpdated.Position != originalArticleTypeImage.Position))
                        {
                            PCMArticleImage productTypeImage = (PCMArticleImage)articleImageUpdated.Clone();
                            productTypeImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            productTypeImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdatedArticle.PCMArticleImageList.Add(productTypeImage);
                            if (articleImageUpdated.PCMArticleImageInBytes != originalArticleTypeImage.PCMArticleImageInBytes)
                                ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogImagesUpdate").ToString(), originalArticleTypeImage.SavedFileName, articleImageUpdated.SavedFileName) });
                            if ((articleImageUpdated.OriginalFileName != originalArticleTypeImage.OriginalFileName))
                            {
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
                        }
                    }
                }
                //if ((ImagesList.Any(a => a.IsWarehouseImage == 1)))
                {
                    PCMArticleImage tempDefaultImage = ClonedArticle.PCMArticleImageList.FirstOrDefault(x => x.Position == 1);
                    PCMArticleImage tempDefaultImage_updated = ImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempDefaultImage != null && tempDefaultImage_updated != null && tempDefaultImage.IdPCMArticleImage != tempDefaultImage_updated.IdPCMArticleImage)
                    {
                        if (tempDefaultImage_updated.Position == 1)
                            ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogDefaultImagesUpdate").ToString(), tempDefaultImage.OriginalFileName, tempDefaultImage_updated.OriginalFileName) });
                    }
                }
                UpdatedArticle.PCMArticleImageList.ForEach(x => x.AttachmentImage = null);

                AddArticleLogDetails();
                UpdatedArticle.PCMArticleLogEntiryList = ChangeLogList.ToList();
                UpdatedArticle.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                IsSave = PCMService.IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(UpdatedArticle.IdPCMArticleCategory, UpdatedArticle);
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditSaveModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
   
        private void AddArticleLogDetails()
        {
            //Status
            if (ClonedArticle.IdPCMStatus != UpdatedArticle.IdPCMStatus)
            {
                LookupValue tempStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == UpdatedArticle.IdPCMStatus);
                if(ClonedArticle.IdPCMStatus== null)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), ClonedArticle.PCMStatus="Draft", tempStatus.Value) });
                if(tempStatus != null)
               // else
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), ClonedArticle.PCMStatus, tempStatus.Value) });
            }

            if (ClonedArticle.IdPCMArticleCategory != UpdatedArticle.IdPCMArticleCategory)
            {
                PCMArticleCategory tempPCMArticleCategory = ArticleMenuList.FirstOrDefault(x => x.IdPCMArticleCategory == UpdatedArticle.IdPCMArticleCategory);
                if (tempPCMArticleCategory != null)
                    ChangeLogList.Add(new PCMArticleLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleChangeLogCategory").ToString(), ClonedArticle.PcmArticleCategory.Name, tempPCMArticleCategory.Name) });
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

                string selectedStatus = BindableBase.GetPropertyName(() => SelectedStatus);
                string selectedCategory = BindableBase.GetPropertyName(() => SelectedCategory);

                if (columnName == selectedStatus)
                {
                    return EditPCMArticleValidation.GetErrorMessage(selectedStatus, SelectedStatus);
                }

                if (columnName == selectedCategory)
                {
                    return EditPCMArticleValidation.GetErrorMessage(selectedCategory, SelectedCategory);
                }

                string gridCompatibilityError = BindableBase.GetPropertyName(() => CompatibilityError);

                if (columnName == gridCompatibilityError)
                    return EditPCMArticleValidation.GetErrorMessage(gridCompatibilityError, CompatibilityError);


                return null;
            }
        }



        #endregion

       
    }
}
