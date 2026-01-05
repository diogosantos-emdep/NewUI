using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class ArticleWarehouseComments
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private string comment;
        private string name;
        public string Comment
        {

            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }

        public bool IsCommentDeleted { get; set; }




        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
    }

    public class ArticleDetailsViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged
    {
        #region TaskLog

        /// <summary>
        /// [GEOS2-72] [Add Transit location contents in article details][adadibathina]
        /// [GEOS2-1511] [Do not display TRANSIT location if stock is 0][adadibathina]
        /// [000][avpawar][17/04/2019][GEOS2-75][Export stock history]
        /// [001][avpawar][03/07/2019][GEOS2-1604][Add new colums "DN" and "Location" in Article Stock History]
        /// [002][skale][15/07/2019][GEOS2-1653] Add a new forecast section in Article Details
        /// [004][Sprint_72]__[GEOS2-1765]__[Add new field "Type" in Article details]__[sdesai]
        /// [005][Sprint_72]__[GEOS2-1656]__[Add article Sleeping days column in warehouse section]__[sdesai]
        /// [006][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// [007][Sprint_75]__[GEOS2-1860]__[Add a new field "Family" in Article details]__[sdesai]
        /// </summary>

        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        private bool isAddComment;
        private bool isDeleteComment;
        private bool isEditComment;
        private Article articleData;
        private int screenHeight;
        //private int screenWidth;
        private int viewHeight;
        private int viewWidth;
        private string currentStockBgColor;
        private string currentStockFgColor;
        private List<ArticleCategory> listArticleCategory;
        private ImageSource referenceImage;
        byte[] ReferenceImageByte = null;
        private double articleWeight;
        private string articleWeightSymbol;
        private bool articleActive;
        private bool isReplacementAvailable;
        private ArticleBySupplier favouriteSupplier;
        private ArticleWarehouseLocations inputStockLocation;
        private ArticleWarehouseLocations outputStockLocation;
        private ObservableCollection<WarehouseLocation> warehouseLocationList;
        private ObservableCollection<ArticleWarehouseLocations> articleWarehouseLocationsList;
        private ArticleWarehouseLocations selectedLocation;
        private long minimumQuantity;
        private long maximumQuantity;
        private List<LogEntriesByArticle> articleLogEntriesList;
        private bool isResult;
        private ImageSource oldReferenceImage;
        private ObservableCollection<ArticleWarehouseLocations> updatedArticleWarehouseLocationsList;
        private ObservableCollection<ArticleComment> updatedArticleCommentList;
        private ObservableCollection<LogEntriesByArticle> articleChangeLogList;
        private ObservableCollection<ArticleWarehouseLocations> updatedArticlePositionList;
        private double dialogHeight;
        private bool isReferenceImageExist;
        private ImageSource defaultReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
        //[GEOS2-72]
        private bool isFirstLocationSelected;
        private bool isLastLocationSelected;
        //[GEOS2-72]
        bool isBusy;
        private List<ArticlesStock> lstArticlesStock;
        private int screenWidth;
        private double dialogWidth;
        // [SP-67 00] Added
        private long articleForeCastPoCount;
        private long articleForeCastWoCount;
        private List<ArticleType> articleTypesList;
        private ArticleType selectedArticleType;
        private string articleAvailabilityMessage;
        private Visibility sleepDaysVisibility;
        private bool isSleepDaysReachedSettingValue;
        private int selectedFamilyIndex;
        private ArticleComment selectedComment;
        #endregion //Declaration

        #region Properties

        public Article UpdateArticle { get; set; }
        public bool SupplierTypeColor { get; set; }
        public long InternalWarehouseId { get; set; }
        public MyWarehouse UpdateMyWarehouse { get; set; }
        public ObservableCollection<ArticleWarehouseLocations> ArticleWarehouseLocationsListForLog { get; set; }
        public Boolean IsAddComment
        {
            get { return isAddComment; }
            set
            {
                isAddComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddComment"));
            }
        }
        public Boolean IsDeleteComment
        {
            get { return isDeleteComment; }
            set
            {
                isDeleteComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleteComment"));
            }
        }
        public Boolean IsEditComment
        {
            get { return isEditComment; }
            set
            {
                isEditComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditComment"));
            }
        }
        public ArticleComment SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        public ObservableCollection<ArticleComment> ArticleWarehouseCommentsList
        { get; set; }

        public int LocationsCount
        {
            get
            {
                if (ArticleWarehouseLocationsList != null)
                {
                    if (ArticleWarehouseLocationsList.FirstOrDefault(x => x.IdWarehouseLocation == WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation)?.ArticlesStock.Quantity != 0)
                    {
                        return ArticleWarehouseLocationsList.Count;
                    }
                    else
                    {
                        return ArticleWarehouseLocationsList.Count - 1;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// This is used in View to remove transit loation from grid when stock is zero
        /// </summary>
        public CriteriaOperator LocationTransitFilter
        {
            get
            {
                return CriteriaOperator.Parse(string.Format("!([ArticlesStock.Quantity] == {0} AND [IdWarehouseLocation] == {1})", 0, WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation));
            }
        }

        public ArticleWarehouseLocations InputStockLocation
        {
            get { return inputStockLocation; }
            set
            {
                inputStockLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InputStockLocation"));
            }
        }

        public ArticleWarehouseLocations OutputStockLocation
        {
            get { return outputStockLocation; }
            set
            {
                outputStockLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OutputStockLocation"));
            }
        }

        public ArticleBySupplier FavouriteSupplier
        {
            get { return favouriteSupplier; }
            set
            {
                favouriteSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FavouriteSupplier"));
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

        public double ArticleWeight
        {
            get { return articleWeight; }
            set
            {
                articleWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleWeight"));
            }
        }

        public bool ArticleActive
        {
            get { return articleActive; }
            set
            {
                articleActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleActive"));
            }
        }

        public bool IsReplacementAvailable
        {
            get { return isReplacementAvailable; }
            set
            {
                isReplacementAvailable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReplacementAvailable"));
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
                    ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
                    IsReferenceImageExist = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceImage"));
            }
        }

        public List<ArticleCategory> ListArticleCategory
        {
            get { return listArticleCategory; }
            set
            {
                listArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListArticleCategory"));
            }
        }

        public Article ArticleData
        {
            get { return articleData; }
            set
            {
                articleData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleData"));
            }
        }

        public int ViewHeight
        {
            get { return viewHeight; }
            set
            {
                viewHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ViewHeight"));
            }
        }

        public int ViewWidth
        {
            get { return viewWidth; }
            set
            {
                viewWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ViewWidth"));
            }
        }

        public string CurrentStockBgColor
        {
            get { return currentStockBgColor; }
            set
            {
                currentStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStockBgColor"));
            }
        }

        public string CurrentStockFgColor
        {
            get { return currentStockFgColor; }
            set
            {
                currentStockFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStockFgColor"));
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

        public ObservableCollection<ArticleWarehouseLocations> ArticleWarehouseLocationsList
        {
            get { return articleWarehouseLocationsList; }
            set
            {
                articleWarehouseLocationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleWarehouseLocationsList"));
            }
        }

        public ArticleWarehouseLocations SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;

                IsFirstLocationSelected = ArticleWarehouseLocationsList.IndexOf(SelectedLocation) == 1 ? true : false;
                IsLastLocationSelected = ArticleWarehouseLocationsList.IndexOf(SelectedLocation) == ArticleWarehouseLocationsList.Count - 1 ? true : false;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocation"));
            }
        }

        public long MinimumQuantity
        {
            get { return minimumQuantity; }
            set
            {
                minimumQuantity = value;
                ChangeCurrentStockColor(ArticleData.MyWarehouse.CurrentStock, minimumQuantity);
                OnPropertyChanged(new PropertyChangedEventArgs("MinimumQuantity"));
            }
        }

        public long MaximumQuantity
        {
            get { return maximumQuantity; }
            set
            {
                maximumQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximumQuantity"));
            }
        }

        public List<LogEntriesByArticle> ArticleLogEntriesList
        {
            get { return articleLogEntriesList; }
            set
            {
                articleLogEntriesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleLogEntriesList"));
            }
        }

        public bool IsResult
        {
            get { return isResult; }
            set
            {
                isResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsResult"));
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
        public ObservableCollection<ArticleWarehouseLocations> UpdatedArticleWarehouseLocationsList
        {
            get { return updatedArticleWarehouseLocationsList; }
            set
            {
                updatedArticleWarehouseLocationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedArticleWarehouseLocationsList"));
            }
        }


        public ObservableCollection<ArticleComment> UpdatedArticleCommentList
        {
            get { return updatedArticleCommentList; }
            set
            {
                updatedArticleCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedArticleCommentList"));
            }
        }
        public ObservableCollection<LogEntriesByArticle> ArticleChangeLogList
        {
            get { return articleChangeLogList; }
            set
            {
                articleChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleChangeLogList"));
            }
        }

        public ObservableCollection<ArticleWarehouseLocations> UpdatedArticlePositionList
        {
            get { return updatedArticlePositionList; }
            set
            {
                updatedArticlePositionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedArticlePositionList"));
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

        public bool IsReferenceImageExist
        {
            get { return isReferenceImageExist; }
            set
            {
                isReferenceImageExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReferenceImageExist"));
            }
        }

        //[GEOS2-72]
        public bool IsFirstLocationSelected
        {
            get { return isFirstLocationSelected; }
            set
            {
                isFirstLocationSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFirstLocationSelected"));
            }
        }

        public bool IsLastLocationSelected
        {
            get { return isLastLocationSelected; }
            set
            {
                isLastLocationSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLastLocationSelected"));
            }
        }
        //[GEOS2-72]

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        public List<ArticlesStock> LstArticlesStock
        {
            get { return lstArticlesStock; }
            set
            {
                lstArticlesStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("lstArticlesStock"));
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

        // [SP-67 00] Added
        public long ArticleForeCastWOCount
        {
            get { return articleForeCastWoCount; }
            set
            {
                articleForeCastWoCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleForeCastWOCount"));
            }
        }

        public long ArticleForeCastPOCount
        {
            get { return articleForeCastPoCount; }
            set
            {
                articleForeCastPoCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleForeCastPOCount"));
            }
        }
        public List<ArticleType> ArticleTypesList
        {
            get
            {
                return articleTypesList;
            }

            set
            {
                articleTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleTypesList"));
            }
        }
        public ArticleType SelectedArticleType
        {
            get
            {
                return selectedArticleType;
            }

            set
            {
                selectedArticleType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleType"));
            }
        }
        public string ArticleAvailabilityMessage
        {
            get
            {
                return articleAvailabilityMessage;
            }

            set
            {
                articleAvailabilityMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleAvailabilityMessage"));
            }
        }
        public int ArticleAge { get; set; }
        public Visibility SleepDaysVisibility
        {
            get
            {
                return sleepDaysVisibility;
            }

            set
            {
                sleepDaysVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SleepDaysVisibility"));
            }
        }
        public bool IsSleepDaysReachedSettingValue
        {
            get
            {
                return isSleepDaysReachedSettingValue;
            }

            set
            {
                isSleepDaysReachedSettingValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSleepDaysReachedSettingValue"));
            }
        }
        public string ArticleAgeDaysString { get; set; }
        public string ArticleAgeDays { get; set; }
        public List<LookupValue> ArticleFamilyList { get; set; }
        public int SelectedFamilyIndex
        {
            get
            {
                return selectedFamilyIndex;
            }

            set
            {
                selectedFamilyIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamilyIndex"));
            }
        }

        #endregion // Properties

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand ReferenceHyperlinkClickCommand { get; set; }
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand AddLocationGridDoubleClickCommand { get; set; }
        public ICommand ArticleLocationGridRowMoveUpButtonCommand { get; set; }
        public ICommand ArticleLocationGridRowMoveDownButtonCommand { get; set; }
        //public ICommand OnSpinEditMinimumValueChangedCommand { get; set; }
        //public ICommand OnSpinEditMaximumValueChangedCommand { get; set; }
        public ICommand DeleteLocationCommand { get; set; }
        public ICommand DeleteCommentCommand { get; set; }
        public ICommand EditLocationGridDoubleClickCommand { get; set; }
        public ICommand PrintDeliveryNoteCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand ArticleDetailsViewDNHyperlinkClickCommand { get; set; }

        //[SP-67 00] added
        public ICommand ForeCastCodeHyperlinkClickCommand { get; set; }
        // [006] added
        public ICommand PrintArticleLabelCommand { get; set; }

        public ICommand EditArticleCommentCommand { get; set; }

        #endregion //ICommmand

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

        #endregion //Events

        #region Constructor

        /// <summary>
        /// [001][avpawar][17/04/2019][GEOS2-75][Export stock history]
        /// [002][avpawar][03/07/2019][GEOS2-1604][Add new colums "DN" and "Location" in Article Stock History]
        /// [003][skale][15/07/2019][GEOS2-1653] Add a new forecast section in Article Details
        /// </summary>
        public ArticleDetailsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ArticleDetailsViewModel ...", category: Category.Info, priority: Priority.Low);
                screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                ViewHeight = screenHeight - 330;
                FillArticleCategoryList();
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                ReferenceHyperlinkClickCommand = new DelegateCommand<object>(ReferenceHyperlinkClickCommandAction);
                AttachmentClickCommand = new DelegateCommand<object>(AttachmentClickCommandAction);
                AddLocationGridDoubleClickCommand = new DelegateCommand<object>(AddLocation);
                ArticleLocationGridRowMoveUpButtonCommand = new DelegateCommand<object>(ArticleLocationGridRowMoveUpAction);
                ArticleLocationGridRowMoveDownButtonCommand = new DelegateCommand<object>(ArticleLocationGridRowMoveDownAction);
                PrintDeliveryNoteCommand = new DelegateCommand<object>(PrintDeliveryNoteAction);
                AddCommentCommand = new DelegateCommand<object>(AddCommentAction);
                DeleteLocationCommand = new DelegateCommand<object>(DeleteLocationCommandAction);
                DeleteCommentCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                EditLocationGridDoubleClickCommand = new DelegateCommand<object>(EditMinimumStock);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportStockHistoryButtonCommandAction));   // [001] added
                ArticleDetailsViewDNHyperlinkClickCommand = new DelegateCommand<object>(ArticleDetailsViewDNHyperlinkClickCommandAction); //[002] added for hyperlink for Delivery Note
                //[003]added
                ForeCastCodeHyperlinkClickCommand = new DelegateCommand<object>(ForeCastCodeHyperlinkClickCommandAction);//added hyperlink for Forecast type
                PrintArticleLabelCommand = new RelayCommand(new Action<object>(PrintArticleLabel));

                EditArticleCommentCommand = new RelayCommand(new Action<object>(EditArticleCommentAction));

                GeosApplication.Instance.Logger.Log("Constructor ArticleDetailsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor ArticleDetailsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion //Constructor

        #region Methods

        /// <summary>
        /// Method to Initialize
        /// [001][skale][15-07-2019][GEOS2-1653] Add a new forecast section in Article Details
        /// [002][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [003][cpatil][02-12-2019][GEOS2-1862]Wrong calculation of the Sleeping days
        /// [004][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        /// [005][avpawar][14-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
        ///</summry>
        /// <param name="art"></param>
        public void Init(string reference, long WarehouseId)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                //[SP67 001]added

                // [002] Changed Service method
                // ArticleData = WarehouseService.GetArticleDetailsByReference_V2037(reference, WarehouseCommon.Instance.Selectedwarehouse);

                // [003] Changed Service method
                //ArticleData = WarehouseService.GetArticleDetailsByReference_V2038(reference, WarehouseCommon.Instance.Selectedwarehouse);

                // [004] Changed Service method
                // ArticleData = WarehouseService.GetArticleDetailsByReference_V2044(reference, WarehouseCommon.Instance.Selectedwarehouse);

                // [005] Changed Service Method
                ArticleData = WarehouseService.GetArticleDetailsByReference_V2051(reference, WarehouseCommon.Instance.Selectedwarehouse);

                if (ArticleData != null)
                {
                    if (ArticleData.LstManufacturersByArticles != null)
                        foreach (var manufacturers in ArticleData.LstManufacturersByArticles.GroupBy(tpa => tpa.Country.Iso))
                        {

                            ImageSource countryFlagImage = ByteArrayToBitmapImage(manufacturers.ToList().FirstOrDefault().Country.CountryIconBytes);
                            manufacturers.ToList().Where(ma => ma.Country.Iso == manufacturers.Key).ToList().ForEach(ma => ma.Country.CountryIconImage = countryFlagImage);
                        }
                    if (ArticleData.LstArticleBySupplier != null)
                        foreach (var suppliers in ArticleData.LstArticleBySupplier.GroupBy(tpa => tpa.ArticleSupplier.Country.Iso))
                        {

                            ImageSource countryFlagImage = ByteArrayToBitmapImage(suppliers.ToList().FirstOrDefault().ArticleSupplier.Country.CountryIconBytes);
                            suppliers.ToList().Where(ma => ma.ArticleSupplier.Country.Iso == suppliers.Key).ToList().ForEach(ma => ma.ArticleSupplier.Country.CountryIconImage = countryFlagImage);
                        }

                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 43))
                {
                    IsAddComment = true;
                }
                else
                {
                    IsAddComment = false;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 44))
                {
                    IsEditComment = true;
                }
                else
                {
                    IsEditComment = false;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 45))
                {
                    IsDeleteComment = true;
                }
                else
                {
                    IsDeleteComment = false;
                }

                ArticleForeCastPOCount = ArticleData.LstArticleForecast.Where(x => x.Type == "PO").Sum(x => x.Quantity);
                ArticleForeCastWOCount = ArticleData.LstArticleForecast.Where(x => x.Type == "OT").Sum(x => x.Quantity);

                ArticleData.LstArticleDecomposition = ArticleData.LstArticleDecomposition.Where(de => de.IdParent == ArticleData.IdArticle).ToList();
                ArticleWarehouseLocationsList = new ObservableCollection<ArticleWarehouseLocations>(ArticleData.LstArticleWarehouseLocations.Select(x => (ArticleWarehouseLocations)x.Clone()).OrderBy(x => x.Position).ToList());
                ArticleWarehouseCommentsList = new ObservableCollection<ArticleComment>(ArticleData.LstArticleComment.Select(x => (ArticleComment)x.Clone()).OrderBy(x => x.IdArticleComment).ToList());
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetAllWarehouseLocationsByIdWarehouse(WarehouseId));
                List<Int64> IntArticleWarehouseLocationsList = ArticleWarehouseLocationsList.Select(awl => awl.IdWarehouseLocation).ToList();
                List<Int64> IntWarehouseLocationList = WarehouseLocationList.Select(wl => wl.IdWarehouseLocation).ToList();
                List<Int64> IntWarehouseLocationListfinal = IntWarehouseLocationList.Where(wlf => !IntArticleWarehouseLocationsList.Contains(wlf)).ToList();
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseLocationList.Where(x => IntWarehouseLocationListfinal.Contains(x.IdWarehouseLocation)).ToList());
                UpdatedArticleWarehouseLocationsList = new ObservableCollection<ArticleWarehouseLocations>();
                UpdatedArticleCommentList = new ObservableCollection<ArticleComment>();
                UpdatedArticlePositionList = new ObservableCollection<ArticleWarehouseLocations>();

                ArticleWarehouseLocationsListForLog = new ObservableCollection<ArticleWarehouseLocations>(ArticleData.LstArticleWarehouseLocations.Select(x => (ArticleWarehouseLocations)x.Clone()).ToList());



                //ArticleWarehouseCommentsList = new ObservableCollection<ArticleComment>()
                //    {
                //         new ArticleComment(){Comment="First Comment",IdStage=1},
                //         new ArticleComment(){Comment="Second Comment",IdStage=2}
                //     };

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 140;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 130;

                if (ArticleData != null)
                {
                    FavouriteSupplier = ArticleData.LstArticleBySupplier.Where(x => x.IsTheFavorite == 1).SingleOrDefault();
                    if (ArticleData.LstArticleWarehouseLocations == null || ArticleData.LstArticleWarehouseLocations.Count != 0)
                    {
                        InputStockLocation = ArticleData.LstArticleWarehouseLocations.OrderBy(x => x.WarehouseLocation.Position).Last();
                        OutputStockLocation = ArticleData.LstArticleWarehouseLocations.OrderBy(x => x.WarehouseLocation.Position).First();
                    }

                    double weight = System.Convert.ToDouble(ArticleData.Weight);

                    if (System.Convert.ToDouble(ArticleData.Weight) < 1)
                    {
                        if (Math.Round(weight * 1000, 0) == 1000)
                        {
                            ArticleWeight = Math.Round(weight * 1000, 0);
                            ArticleWeightSymbol = " (Kg) :";
                        }
                        ArticleWeight = Math.Round(weight * 1000, 0);
                        ArticleWeightSymbol = " (gr) :";
                    }
                    else
                    {
                        ArticleWeight = Math.Round(weight, 3);
                        ArticleWeightSymbol = " (Kg) :";
                    }

                    if (ArticleData.IsObsolete == 0)
                    {
                        ArticleActive = true;
                    }
                    else
                    {
                        if (ArticleData.ReplacementArticle != null && ArticleData.ReplacementArticle != "")
                            IsReplacementAvailable = true;

                        ArticleActive = false;
                    }

                    //SetReferenceImage(ArticleData.ArticleImageInBytes);
                    ReferenceImage = ByteArrayToBitmapImage(ArticleData.ArticleImageInBytes);
                    OldReferenceImage = ReferenceImage;
                }

                if (ArticleData.MyWarehouse != null)
                {
                    MinimumQuantity = ArticleData.MyWarehouse.MinimumStock;
                    MaximumQuantity = ArticleData.MyWarehouse.MaximumStock;
                }
                else
                    ChangeCurrentStockColor(0, 0);

                if (ArticleWarehouseLocationsList.Count > 0)
                {
                    CheckWarehousePositions();
                    if (ArticleWarehouseLocationsList.Count > 1)
                        SelectedLocation = ArticleWarehouseLocationsList[1];
                }

                //[004]added
                ArticleTypesList = new List<ArticleType>(WarehouseService.GetArticleTypes(WarehouseCommon.Instance.Selectedwarehouse));
                if (ArticleTypesList.Count > 0)
                    SelectedArticleType = ArticleTypesList.FirstOrDefault(x => x.IdArticleType == ArticleData.ArticlesType.IdArticleType);

                //ArticleAvailabilityMessage
                if (ArticleActive && ArticleData.MyWarehouse.CurrentStock > 0)
                    ArticleAvailabilityMessage = System.Windows.Application.Current.FindResource("ArticleAvailabilityDetails").ToString();
                else if (ArticleActive && ArticleData.MyWarehouse.CurrentStock == 0)
                    ArticleAvailabilityMessage = System.Windows.Application.Current.FindResource("ArticleOutOfStockDetails").ToString();
                else if (!ArticleActive)
                    ArticleAvailabilityMessage = System.Windows.Application.Current.FindResource("ArticleDiscontinuedDetails").ToString();

                //[005]added

                if (ArticleData.CreatedIn != null)
                {
                    DateCalculateInYearAndMonthHelper dateCalculateInYearAndMonth = new DateCalculateInYearAndMonthHelper(GeosApplication.Instance.ServerDateTime.Date, ArticleData.CreatedIn.Value.Date);
                    if (dateCalculateInYearAndMonth.Years > 0)
                    {
                        ArticleAgeDays = dateCalculateInYearAndMonth.Years.ToString() + "+";
                        ArticleAgeDaysString = string.Format(System.Windows.Application.Current.FindResource("ArticleYears").ToString());
                    }
                    else
                    {
                        ArticleAge = (GeosApplication.Instance.ServerDateTime.Date - ArticleData.CreatedIn.Value.Date).Days;

                        if (ArticleAge > 99)
                            ArticleAgeDays = "99+";
                        else
                            ArticleAgeDays = ArticleAge.ToString();

                        ArticleAgeDaysString = string.Format(System.Windows.Application.Current.FindResource("ArticleDays").ToString());
                    }
                }


                if (ArticleData.ArticleSleepDays == null)
                    SleepDaysVisibility = Visibility.Collapsed;
                else
                    SleepDaysVisibility = Visibility.Visible;

                if (ArticleData.ArticleSleepDays >= WarehouseCommon.Instance.ArticleSleepDays)
                    IsSleepDaysReachedSettingValue = false;
                else
                    IsSleepDaysReachedSettingValue = true;

                //[007]added
                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(46);
                ArticleFamilyList = new List<LookupValue>(temptypeList);
                ArticleFamilyList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                SelectedFamilyIndex = ArticleFamilyList.FindIndex(x => x.IdLookupValue == ArticleData.MaterialType);
                if (SelectedFamilyIndex == -1)
                    SelectedFamilyIndex = 0;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditArticleCommentAction(object obj)
        {
            if (IsEditComment)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleCommentAction()..."), category: Category.Info, priority: Priority.Low);
                    TableView detailView = (TableView)obj;
                    ArticleComment selectedComment = (ArticleComment)detailView.FocusedRow;
                    SelectedComment = selectedComment;
                    if (SelectedComment != null)
                    {
                        AddCommentView editCommentView = new AddCommentView();
                        AddCommentViewModel editCommentViewModel = new AddCommentViewModel();                 // warehouseDeliveryNote, ArticleData
                        EventHandler handler = delegate { editCommentView.Close(); };
                        editCommentViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditComment").ToString();
                        editCommentViewModel.RequestClose += handler;
                        editCommentView.DataContext = editCommentViewModel;
                        editCommentViewModel.ArticleWarehouseCommentsList = ArticleWarehouseCommentsList;
                        editCommentViewModel.IdArticleComment = SelectedComment.IdArticleComment;
                        editCommentViewModel.Init(selectedComment);
                        editCommentView.ShowDialogWindow();

                        if (editCommentViewModel.IsSave)
                        {
                            ArticleWarehouseCommentsList.Remove(SelectedComment);
                            //SelectedComment.Comment = editCommentViewModel.UpdatedComment.Comment;
                            //SelectedComment.IdStage = editCommentViewModel.UpdatedComment.IdStage;
                            //SelectedComment.IdArticleComment = editCommentViewModel.UpdatedComment.IdArticleComment;
                            //SelectedComment.Stage = new Stage();
                            //SelectedComment.Stage.Name = editCommentViewModel.UpdatedComment.Stage.Name;

                            SelectedComment = editCommentViewModel.UpdatedComment;
                            SelectedComment.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                            SelectedComment.IdArticle = ArticleData.IdArticle;
                            SelectedComment.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;

                            SelectedComment.TransactionOperation = ModelBase.TransactionOperations.Update;
                            RefreshCommentList(new object());
                            ArticleWarehouseCommentsList.Add(SelectedComment);

                            //UpdatedArticleCommentList.Add(editCommentViewModel.UpdatedComment);
                            //UpdatedArticleCommentList.Remove(SelectedComment);



                            //ArticleComment _updatedUpdatedArticleComment = ArticleData.LstArticleComment.Where(x => x.IdArticleComment ==editCommentViewModel.UpdatedComment.IdArticleComment).FirstOrDefault();
                            //if (_updatedUpdatedArticleComment != null && UpdatedArticleCommentList.Count != 0)
                            //{
                            //    ArticleComment updatedUpdatedArticleComment = UpdatedArticleCommentList.Where(x => x.IdArticleComment == editCommentViewModel.UpdatedComment.IdArticleComment).FirstOrDefault();
                            //    _updatedUpdatedArticleComment.MinimumStock = editCommentViewModel.UpdatedComment.MinimumStock;
                            //    editCommentViewModel.UpdatedComment.TransactionOperation = ModelBase.TransactionOperations.Update;
                            //    if (updatedUpdatedArticleComment == null)
                            //        UpdatedArticleCommentList.Add(editCommentViewModel.UpdatedComment);
                            //}
                            //else if (_updatedUpdatedArticleComment == null && UpdatedArticleCommentList.Count != 0)
                            //{
                            //    ArticleComment updatedUpdatedArticleComment = UpdatedArticleCommentList.Where(x => x.IdArticleComment == editCommentViewModel.UpdatedComment.IdArticleComment).FirstOrDefault();
                            //    updatedUpdatedArticleComment.MinimumStock = editCommentViewModel.UpdatedComment.MinimumStock;
                            //    editCommentViewModel.UpdatedComment.TransactionOperation = ModelBase.TransactionOperations.Add;
                            //}
                            //else
                            //{
                            //    editCommentViewModel.UpdatedComment.TransactionOperation = ModelBase.TransactionOperations.Update;
                            //    UpdatedArticleCommentList.Add(editCommentViewModel.UpdatedComment);
                            //    SelectedComment= editCommentViewModel.UpdatedComment;
                            //}
                        }
                    }
                    GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleCommentAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleCommentAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        /// <summary>
        /// CheckWarehousePositions or properor not if not set it properly
        /// </summary>
        public void CheckWarehousePositions()
        {
            foreach (var location in ArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation))
            {
                var i = ArticleWarehouseLocationsList.IndexOf(location);
                if (ArticleWarehouseLocationsList.IndexOf(location) != location.Position)
                {
                    int position = 1;
                    ArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation).ToList().ForEach(x =>
                    {
                        x.Position = position;
                        position++;
                    });
                    break;
                }
            }
        }

        /// <summary>
        /// Method for open article detail on hyper link click in same article view
        /// </summary>
        /// <param name="obj"></param>
        public void ReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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

                string reference = string.Empty;
                if (obj.GetType() == typeof(TableView))
                {
                    TableView tableView = (TableView)obj;

                    if (tableView.FocusedRow is Emdep.Geos.Data.Common.RelatedArticle)
                        reference = ((RelatedArticle)tableView.FocusedRow).Reference.Trim();
                    else if (tableView.FocusedRow is Emdep.Geos.Data.Common.ArticleBySupplier)
                        reference = ((ArticleBySupplier)tableView.FocusedRow).Reference.Trim();
                    else if (tableView.FocusedRow is Emdep.Geos.Data.Common.ArticleDecomposition)
                        reference = ((ArticleDecomposition)tableView.FocusedRow).Reference.Trim();
                }
                else
                {
                    reference = ((Article)obj).ReplacementArticle.Trim();
                }

                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };

                articleDetailsViewModel.RequestClose += handle;
                articleDetailsViewModel.Init(reference, InternalWarehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articleDetailsView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method ReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method use for show selected WO or PO 
        /// [001][skale][15-07-2019][GEOS2-1653] Add a new forecast section in Article Details
        /// </summary>
        /// <param name="obj"></param>
        public void ForeCastCodeHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ForeCastCodeHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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

                TableView tableView = (TableView)obj;

                if ((ArticleForecast)tableView.DataControl.CurrentItem is Emdep.Geos.Data.Common.ArticleForecast)
                {
                    ArticleForecast articleForecast = ((ArticleForecast)tableView.DataControl.CurrentItem);

                    if (articleForecast.Type == "OT")
                    {
                        WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                        WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                        EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                        workOrderItemDetailsViewModel.RequestClose += handle;
                        workOrderItemDetailsViewModel.Init(articleForecast.Id, WarehouseCommon.Instance.Selectedwarehouse);
                        workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                        var ownerInfo = (tableView as FrameworkElement);
                        workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                        workOrderItemDetailsView.ShowDialogWindow();
                    }
                    else if (articleForecast.Type == "PO")
                    {
                        PurchaseOrderItemDetailsViewModel purchaseOrderItemDetailsViewModel = new PurchaseOrderItemDetailsViewModel();
                        PurchaseOrderItemDetailsView purchaseOrderItemDetailsView = new PurchaseOrderItemDetailsView();
                        EventHandler handle = delegate { purchaseOrderItemDetailsView.Close(); };
                        purchaseOrderItemDetailsViewModel.RequestClose += handle;
                        purchaseOrderItemDetailsViewModel.Init(articleForecast.Id, WarehouseCommon.Instance.Selectedwarehouse);
                        purchaseOrderItemDetailsView.DataContext = purchaseOrderItemDetailsViewModel;
                        var ownerInfo = (tableView as FrameworkElement);
                        purchaseOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                        purchaseOrderItemDetailsView.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method ForeCastCodeHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill Article Category List
        /// </summary>
        public void FillArticleCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleCategoryList()...", category: Category.Info, priority: Priority.Low);
                ListArticleCategory = WarehouseService.GetArticleCategories();
                GeosApplication.Instance.Logger.Log("Method FillArticleCategoryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillArticleCategoryList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Add Location 
        /// </summary>
        public void AddLocation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLocation()...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;
                if (WarehouseCommon.Instance.IsPermissionReadOnly) return;
                WarehouseLocation wLocation = (WarehouseLocation)obj;
                AddMinimumStockView addMinimumStockView = new AddMinimumStockView();
                AddMinimumStockViewModel addMinimumStockViewModel = new AddMinimumStockViewModel();
                EventHandler handle = delegate { addMinimumStockView.Close(); };
                addMinimumStockViewModel.RequestClose += handle;
                addMinimumStockView.DataContext = addMinimumStockViewModel;
                addMinimumStockViewModel.IsNew = true;
                addMinimumStockViewModel.Init(wLocation);
                addMinimumStockView.ShowDialog();

                if (addMinimumStockViewModel.IsSave == true)
                {
                    addMinimumStockViewModel.NewArticleWarehouseLocations.IdArticle = ArticleData.IdArticle;

                    long position;
                    if (ArticleWarehouseLocationsList.Count > 0)
                        position = ArticleWarehouseLocationsList.Max(x => x.Position);
                    else
                        position = 0;

                    addMinimumStockViewModel.NewArticleWarehouseLocations.Position = position + 1;
                    addMinimumStockViewModel.NewArticleWarehouseLocations.TransactionOperation = ModelBase.TransactionOperations.Add;
                    addMinimumStockViewModel.NewArticleWarehouseLocations.WarehouseDeliveryNotes = new List<WarehouseDeliveryNote>();
                    UpdatedArticleWarehouseLocationsList.Add(addMinimumStockViewModel.NewArticleWarehouseLocations);
                    ArticleWarehouseLocationsList.Add(addMinimumStockViewModel.NewArticleWarehouseLocations);
                    WarehouseLocationList.Remove(wLocation);
                    SelectedLocation = addMinimumStockViewModel.NewArticleWarehouseLocations;
                }

                GeosApplication.Instance.Logger.Log("Method AddLocation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AddLocation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Move Row Up Side
        /// [001][GEOS2-72] [Add Transit location contents in article details][adadibathina]
        /// </summary>
        private void ArticleLocationGridRowMoveUpAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleLocationGridRowMoveUpAction()...", category: Category.Info, priority: Priority.Low);

                var values = (object[])obj;
                DevExpress.Xpf.Grid.TableView LocationstableView = (DevExpress.Xpf.Grid.TableView)values[0];
                DevExpress.Xpf.Grid.GridControl LocationsGridControl = (DevExpress.Xpf.Grid.GridControl)values[1];
                int currentIndex = 1;

                if (LocationstableView.FocusedRowHandle > 0)
                {
                    currentIndex = LocationsGridControl.GetListIndexByRowHandle(LocationstableView.FocusedRowHandle);
                    var targetListIndex = LocationsGridControl.GetListIndexByRowHandle(LocationstableView.FocusedRowHandle - 1);
                    int selectedIndexInList = ArticleWarehouseLocationsList.IndexOf(SelectedLocation);
                    long Position = ArticleWarehouseLocationsList[selectedIndexInList].Position;
                    ArticleWarehouseLocationsList[selectedIndexInList].Position = ArticleWarehouseLocationsList[selectedIndexInList - 1].Position;
                    ArticleWarehouseLocationsList[selectedIndexInList - 1].Position = Position;
                    ArticleWarehouseLocationsList.Move(selectedIndexInList, selectedIndexInList - 1);
                    LocationsGridControl.RefreshData();
                }

                // [001][Setting appropriate location]
                SelectedLocation = ArticleWarehouseLocationsList[currentIndex - 1];
                //[001]
                GeosApplication.Instance.Logger.Log("Method ArticleLocationGridRowMoveUpAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ArticleLocationGridRowMoveUpAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Move Row Down Side
        /// [001][GEOS2-72] [Add Transit location contents in article details][adadibathina]
        /// </summary>
        private void ArticleLocationGridRowMoveDownAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleLocationGridRowMoveDownAction()...", category: Category.Info, priority: Priority.Low);
                var values = (object[])obj;
                DevExpress.Xpf.Grid.TableView LocationstableView = (DevExpress.Xpf.Grid.TableView)values[0];
                DevExpress.Xpf.Grid.GridControl LocationsGridControl = (DevExpress.Xpf.Grid.GridControl)values[1];
                int currentIndex = 1;

                if (LocationstableView.FocusedRowHandle > 0)
                {
                    currentIndex = LocationsGridControl.GetListIndexByRowHandle(LocationstableView.FocusedRowHandle);
                    var targetListIndex = LocationsGridControl.GetListIndexByRowHandle(LocationstableView.FocusedRowHandle + 1);
                    int selectedIndexInList = ArticleWarehouseLocationsList.IndexOf(SelectedLocation);
                    long Position = ArticleWarehouseLocationsList[selectedIndexInList].Position;
                    ArticleWarehouseLocationsList[selectedIndexInList].Position = ArticleWarehouseLocationsList[selectedIndexInList + 1].Position;
                    ArticleWarehouseLocationsList[selectedIndexInList + 1].Position = Position;
                    ArticleWarehouseLocationsList.Move(selectedIndexInList, selectedIndexInList + 1);
                    LocationsGridControl.RefreshData();
                }

                //var currentIndex = ArticleWarehouseLocationsList.IndexOf(SelectedLocation);
                //if (currentIndex != ArticleWarehouseLocationsList.Count - 1 && currentIndex != -1)
                //{
                //    long Position = ArticleWarehouseLocationsList[currentIndex].Position;
                //    ArticleWarehouseLocationsList[currentIndex].Position = ArticleWarehouseLocationsList[currentIndex + 1].Position;
                //    ArticleWarehouseLocationsList[currentIndex + 1].Position = Position;
                //    ArticleWarehouseLocationsList.Move(currentIndex, currentIndex + 1);
                //}

                // [001][Setting appropriate location]
                SelectedLocation = ArticleWarehouseLocationsList[currentIndex + 1];
                // [001]

                GeosApplication.Instance.Logger.Log("Method ArticleLocationGridRowMoveDownAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ArticleLocationGridRowMoveDownAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Accept Add Locations to Article
        /// </summary>
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                UpdateArticle = new Article();
                UpdateMyWarehouse = new MyWarehouse();

                foreach (ArticleWarehouseLocations location in ArticleWarehouseLocationsList)
                {
                    ArticleWarehouseLocations articleWarehouse = ArticleData.LstArticleWarehouseLocations.Where(x => x.IdWarehouseLocation == location.IdWarehouseLocation).FirstOrDefault();
                    if (articleWarehouse != null)
                    {
                        if (location.Position != articleWarehouse.Position)
                        {
                            UpdatedArticlePositionList.Add(location);
                            UpdatedArticleWarehouseLocationsList.Add(location);
                        }
                    }
                }
                UpdateArticle.LstArticleWarehouseLocations = new List<ArticleWarehouseLocations>(UpdatedArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation));


                foreach (ArticleComment comment in ArticleWarehouseCommentsList)
                {
                    ArticleComment articleComment = ArticleData.LstArticleComment.Where(x => x.IdArticleComment == comment.IdArticleComment).FirstOrDefault();
                    if (articleComment != null)
                    {
                        if ((comment.Comment != articleComment.Comment) || (comment.DateOfExpiry != articleComment.DateOfExpiry))
                        {
                            UpdatedArticleCommentList.Add(comment);
                        }
                    }
                }
                UpdateArticle.LstArticleComment = new List<ArticleComment>(UpdatedArticleCommentList);
                //UpdateArticle.LstArticleComment = new List<AUpdateArticleDetails_V2041rticleComment>(UpdatedArticleCommentList.Where(x => x.IdArticleComment != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation));

                ArticleChangeLogDetails();
                UpdateArticle.LogEntriesByArticles = new List<LogEntriesByArticle>(ArticleChangeLogList);

                //IsResult = WarehouseService.(UpdateArticle);
                IsResult = WarehouseService.UpdateArticleDetails_V2051(UpdateArticle);
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdatedSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get Article Image
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public ImageSource SetReferenceImage(byte[] Image)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetReferenceImage ...", category: Category.Info, priority: Priority.Low);
                ReferenceImageByte = Image;

                if (ReferenceImageByte != null)
                {
                    ReferenceImage = ByteArrayToBitmapImage(ReferenceImageByte);
                    IsReferenceImageExist = true;
                }
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        ReferenceImage = GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/wNA.png");
                        IsReferenceImageExist = false;
                    }
                    else
                    {
                        ReferenceImage = GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/bNA.png");
                        IsReferenceImageExist = false;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetReferenceImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SetReferenceImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SetReferenceImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in SetReferenceImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return ReferenceImage;
        }

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
                GeosApplication.Instance.Logger.Log("Error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        ///  This method is for to get image in bitmap by path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeCurrentStockColor(Int64 CurrentStock, Int64 minQuantity)
        {
            if (CurrentStock == 0)
            {
                CurrentStockFgColor = "#FFFFFFFF";
                CurrentStockBgColor = "#FFFF0000";
            }
            else if (CurrentStock >= minQuantity)
            {
                CurrentStockFgColor = "#FFFFFFFF";
                CurrentStockBgColor = "#FF008000";
            }
            else if (CurrentStock < minQuantity)
            {
                CurrentStockFgColor = "#FF000000";
                CurrentStockBgColor = "#FFFFFF00";
            }
        }

        /// <summary>
        /// Method to close Window
        /// </summary>
        public void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for view article Attachments
        /// </summary>
        /// <param name="obj"></param>
        private void AttachmentClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AttachmentClickCommandAction()...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.Logger.Log("Method AttachmentClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AttachmentClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        ////Removed Method.
        //private void OnSpinEditMinimumValueChangedCommandAction(EditValueChangedEventArgs obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OnSpinEditMinimumValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

        //        long minimumStock = (long)(Convert.ToDouble(obj.NewValue));
        //        if (ArticleData != null)
        //        {
        //            if (ArticleData.MyWarehouse.CurrentStock == 0)
        //            {
        //                CurrentStockFgColor = "#FFFFFFFF";
        //                CurrentStockBgColor = "#FFFF0000";
        //            }
        //            else if (ArticleData.MyWarehouse.CurrentStock >= minimumStock)
        //            {
        //                CurrentStockFgColor = "#FFFFFFFF";
        //                CurrentStockBgColor = "#FF008000";
        //            }
        //            else if (ArticleData.MyWarehouse.CurrentStock < minimumStock)
        //            {
        //                CurrentStockFgColor = "#FF000000";
        //                CurrentStockBgColor = "#FFFFFF00";
        //            }
        //        }
        //        MinimumQuantity = minimumStock;
        //        GeosApplication.Instance.Logger.Log("Method OnSpinEditMinimumValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in OnSpinEditMinimumValueChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        ////Removed Method.
        //private void OnSpinEditMaximumValueChangedCommandAction(EditValueChangedEventArgs obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OnSpinEditMaximumValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        MaximumQuantity = (long)(Convert.ToDouble(obj.NewValue));
        //        GeosApplication.Instance.Logger.Log("Method OnSpinEditMaximumValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in OnSpinEditMaximumValueChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// This method is for to convert ImageSource to ByteArray
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        /// <summary>
        /// Method to print DeliveryNote
        /// </summary>
        /// <param name="obj"></param>

        private void RefreshCommentList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshCommentList...", category: Category.Info, priority: Priority.Low);
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
                FillCommentList();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshWarehouseLocationList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWarehouseLocationList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillCommentList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCommentList...", category: Category.Info, priority: Priority.Low);
                // [001] Changed Service method


                //ArticleWarehouseCommentsList= new ObservableCollection<ArticleComment>(WarehouseService.GetWarehouseLocationsByIdWarehouse_V2034(WarehouseCommon.Instance.Selectedwarehouse));


                // WarehouseLocationList=new ObservableCollection<WarehouseLocation>(WarehouseService.GetAllWarehouseLocationById(Warehouse.IdWarehouse));
                //SelectedWarehouseLocation = WarehouseLocationList[0];
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
        private void AddCommentAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCommentAction()...", category: Category.Info, priority: Priority.Low);


                AddCommentView addCommentView = new AddCommentView();
                AddCommentViewModel addCommentViewModel = new AddCommentViewModel();                 // warehouseDeliveryNote, ArticleData
                EventHandler handler = delegate { addCommentView.Close(); };
                addCommentViewModel.RequestClose += handler;
                addCommentView.DataContext = addCommentViewModel;
                addCommentViewModel.Init();
                addCommentViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddComment").ToString();
                addCommentViewModel.IsNew = true;
                addCommentViewModel.ArticleWarehouseCommentsList = ArticleWarehouseCommentsList;
                addCommentView.ShowDialogWindow();
                if (addCommentViewModel.IsSave)
                {

                    addCommentViewModel.NewComment.TransactionOperation = ModelBase.TransactionOperations.Add;
                    addCommentViewModel.NewComment.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    addCommentViewModel.NewComment.IdArticle = ArticleData.IdArticle;
                    addCommentViewModel.NewComment.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;

                    ArticleWarehouseCommentsList.Add(addCommentViewModel.NewComment);
                    UpdatedArticleCommentList.Add(addCommentViewModel.NewComment);

                    //RefreshCommentList(new object());
                }

                GeosApplication.Instance.Logger.Log("Method AddCommentAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddCommentAction() Method  " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintDeliveryNoteAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintDeliveryNoteAction()...", category: Category.Info, priority: Priority.Low);

                DeliveryNotePrintView deliveryNotePrintView = new DeliveryNotePrintView();
                WarehouseDeliveryNote warehouseDeliveryNote = obj as WarehouseDeliveryNote;
                DeliveryNotePrintViewModel deliveryNotePrintViewModel = new DeliveryNotePrintViewModel(warehouseDeliveryNote, ArticleData);
                EventHandler handler = delegate { deliveryNotePrintView.Close(); };
                deliveryNotePrintViewModel.RequestClose += handler;
                deliveryNotePrintView.DataContext = deliveryNotePrintViewModel;
                deliveryNotePrintViewModel.Init();
                deliveryNotePrintView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("WorkOrderViewModel Method PrintDeliveryNoteAction executed successfully.", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.Logger.Log("Method PrintDeliveryNoteAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in PrintDeliveryNoteAction() Method  " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to delete Location
        /// </summary>
        /// <param name="obj"></param>

        private void DeleteCommentCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction()...", category: Category.Info, priority: Priority.Low);

                ArticleComment articleComments = (ArticleComment)obj;

                //if (articleWarehouseLocations.IdWarehouseLocation == WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation)
                //    return;

                ArticleComment NewArticleWarehouseComment;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCommentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    //articleComments.IsCommentDeleted = true;
                    articleComments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    ArticleWarehouseCommentsList.Remove(articleComments);


                    if (UpdatedArticleWarehouseLocationsList.Count > 0)
                    {
                        //ArticleWarehouseLocations NewArticleWarehouseLocation =UpdatedArticleWarehouseLocationsList.FirstOrDefault(x => x.IdWarehouseLocation == articleWarehouseLocations.IdWarehouseLocation);
                        NewArticleWarehouseComment = ArticleData.LstArticleComment.FirstOrDefault(x => x.IdArticleComment == articleComments.IdArticleComment);

                        //if (NewArticleWarehouseComment != null)
                        //{
                        //    UpdatedArticleCommentList.Remove(articleComments);
                        //    UpdatedArticleCommentList.Add(articleComments);
                        //}
                        //else
                        //{
                        UpdatedArticleCommentList.Remove(articleComments);
                        UpdatedArticleCommentList.Add(articleComments);
                        //}
                    }
                    else
                    {
                        UpdatedArticleCommentList.Add(articleComments);
                    }
                    GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in DeleteCommentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteLocationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteLocationCommandAction()...", category: Category.Info, priority: Priority.Low);

                ArticleWarehouseLocations articleWarehouseLocations = (ArticleWarehouseLocations)obj;

                if (articleWarehouseLocations.IdWarehouseLocation == WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation)
                    return;

                ArticleWarehouseLocations NewArticleWarehouseLocation;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLocationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    articleWarehouseLocations.IsLocationDeleted = true;
                    articleWarehouseLocations.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    ArticleWarehouseLocationsList.Remove(articleWarehouseLocations);
                    if (UpdatedArticleWarehouseLocationsList.Count > 0)
                    {
                        //ArticleWarehouseLocations NewArticleWarehouseLocation =UpdatedArticleWarehouseLocationsList.FirstOrDefault(x => x.IdWarehouseLocation == articleWarehouseLocations.IdWarehouseLocation);
                        NewArticleWarehouseLocation = ArticleData.LstArticleWarehouseLocations.FirstOrDefault(x => x.IdWarehouseLocation == articleWarehouseLocations.IdWarehouseLocation);

                        if (NewArticleWarehouseLocation != null)
                        {
                            UpdatedArticleWarehouseLocationsList.Remove(articleWarehouseLocations);
                            UpdatedArticleWarehouseLocationsList.Add(articleWarehouseLocations);
                        }
                        else
                        {
                            UpdatedArticleWarehouseLocationsList.Remove(articleWarehouseLocations);
                            UpdatedArticleWarehouseLocationsList.Add(articleWarehouseLocations);
                        }
                    }
                    else
                    {
                        UpdatedArticleWarehouseLocationsList.Add(articleWarehouseLocations);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteLocationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in DeleteLocationCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to edit minimum stock
        ///   [001][GEOS2-72] [Add Transit location contents in article details][adadibathina]
        /// </summary>
        /// <param name="obj"></param>
        private void EditMinimumStock(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditMinimumStock()...", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;

                ArticleWarehouseLocations articleWarehouseLocations = (ArticleWarehouseLocations)obj;
                //[001]// Return on transit location
                if (articleWarehouseLocations.IdWarehouseLocation == WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation)
                    return;
                //[001]
                AddMinimumStockView addMinimumStockView = new AddMinimumStockView();
                AddMinimumStockViewModel addMinimumStockViewModel = new AddMinimumStockViewModel();
                EventHandler handle = delegate { addMinimumStockView.Close(); };
                addMinimumStockViewModel.RequestClose += handle;
                addMinimumStockView.DataContext = addMinimumStockViewModel;
                addMinimumStockViewModel.IsNew = false;
                addMinimumStockViewModel.EditInit(articleWarehouseLocations);
                addMinimumStockView.ShowDialog();

                if (addMinimumStockViewModel.IsSave)
                {
                    ArticleWarehouseLocations _updatedUpdatedArticleWarehouseLocations = ArticleData.LstArticleWarehouseLocations.Where(x => x.IdWarehouseLocation == addMinimumStockViewModel.UpdateArticleWarehouseLocations.IdWarehouseLocation).FirstOrDefault();
                    if (_updatedUpdatedArticleWarehouseLocations != null && UpdatedArticleWarehouseLocationsList.Count != 0)
                    {
                        ArticleWarehouseLocations updatedUpdatedArticleWarehouseLocations = UpdatedArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation == addMinimumStockViewModel.UpdateArticleWarehouseLocations.IdWarehouseLocation).FirstOrDefault();
                        _updatedUpdatedArticleWarehouseLocations.MinimumStock = addMinimumStockViewModel.UpdateArticleWarehouseLocations.MinimumStock;
                        addMinimumStockViewModel.UpdateArticleWarehouseLocations.TransactionOperation = ModelBase.TransactionOperations.Update;
                        if (updatedUpdatedArticleWarehouseLocations == null)
                            UpdatedArticleWarehouseLocationsList.Add(addMinimumStockViewModel.UpdateArticleWarehouseLocations);
                    }
                    else if (_updatedUpdatedArticleWarehouseLocations == null && UpdatedArticleWarehouseLocationsList.Count != 0)
                    {
                        ArticleWarehouseLocations updatedUpdatedArticleWarehouseLocations = UpdatedArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation == addMinimumStockViewModel.UpdateArticleWarehouseLocations.IdWarehouseLocation).FirstOrDefault();
                        updatedUpdatedArticleWarehouseLocations.MinimumStock = addMinimumStockViewModel.UpdateArticleWarehouseLocations.MinimumStock;
                        addMinimumStockViewModel.UpdateArticleWarehouseLocations.TransactionOperation = ModelBase.TransactionOperations.Add;
                    }
                    else
                    {
                        addMinimumStockViewModel.UpdateArticleWarehouseLocations.TransactionOperation = ModelBase.TransactionOperations.Update;
                        UpdatedArticleWarehouseLocationsList.Add(addMinimumStockViewModel.UpdateArticleWarehouseLocations);
                        SelectedLocation = addMinimumStockViewModel.UpdateArticleWarehouseLocations;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditMinimumStock()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in EditMinimumStock() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Change Log
        private void ArticleChangeLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleChangeLogDetails()...", category: Category.Info, priority: Priority.Low);

                ArticleChangeLogList = new ObservableCollection<LogEntriesByArticle>();

                //Both
                if (ArticleData.MyWarehouse.MinimumStock != MinimumQuantity && ArticleData.MyWarehouse.MaximumStock != MaximumQuantity)
                {
                    UpdateArticle.MyWarehouse = UpdateMyWarehouse;
                    UpdateArticle.MyWarehouse.IdArticle = ArticleData.IdArticle;
                    UpdateArticle.MyWarehouse.IdWarehouse = ArticleData.MyWarehouse.IdWarehouse;
                    UpdateArticle.MyWarehouse.MinimumStock = MinimumQuantity;
                    UpdateArticle.MyWarehouse.MaximumStock = MaximumQuantity;
                    ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleMinimumStockLogDetails").ToString(), WarehouseCommon.Instance.Selectedwarehouse.Name, ArticleData.MyWarehouse.MinimumStock, MinimumQuantity) });
                    ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleMaximumStockLogDetails").ToString(), WarehouseCommon.Instance.Selectedwarehouse.Name, ArticleData.MyWarehouse.MaximumStock, MaximumQuantity) });
                }
                else if (ArticleData.MyWarehouse.MinimumStock != MinimumQuantity)   //Minimum Stock
                {
                    UpdateArticle.MyWarehouse = UpdateMyWarehouse;
                    UpdateArticle.MyWarehouse.IdArticle = ArticleData.IdArticle;
                    UpdateArticle.MyWarehouse.IdWarehouse = ArticleData.MyWarehouse.IdWarehouse;
                    UpdateArticle.MyWarehouse.MinimumStock = MinimumQuantity;
                    UpdateArticle.MyWarehouse.MaximumStock = ArticleData.MyWarehouse.MaximumStock;
                    ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleMinimumStockLogDetails").ToString(), WarehouseCommon.Instance.Selectedwarehouse.Name, ArticleData.MyWarehouse.MinimumStock, MinimumQuantity) });
                }
                else if (ArticleData.MyWarehouse.MaximumStock != MaximumQuantity)   //MaximumStock
                {
                    UpdateArticle.MyWarehouse = UpdateMyWarehouse;
                    UpdateArticle.MyWarehouse.IdArticle = ArticleData.IdArticle;
                    UpdateArticle.MyWarehouse.IdWarehouse = ArticleData.MyWarehouse.IdWarehouse;
                    UpdateArticle.MyWarehouse.MinimumStock = ArticleData.MyWarehouse.MinimumStock;
                    UpdateArticle.MyWarehouse.MaximumStock = MaximumQuantity;
                    ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleMaximumStockLogDetails").ToString(), WarehouseCommon.Instance.Selectedwarehouse.Name, ArticleData.MyWarehouse.MaximumStock, MaximumQuantity) });
                }

                if (IsReferenceImageExist)
                {
                    if (Uri.Compare(((System.Windows.Media.Imaging.BitmapImage)defaultReferenceImage).UriSource,
                                      ((System.Windows.Media.Imaging.BitmapImage)OldReferenceImage).UriSource,
                                      UriComponents.AbsoluteUri,
                                      UriFormat.SafeUnescaped,
                                      StringComparison.InvariantCulture) == 0)
                    {

                        byte[] CheckImageBytes = ImageSourceToBytes(ReferenceImage);
                        UpdateArticle.IsAddedArticleImage = true;
                        //string extention = ((System.Windows.Media.Imaging.BitmapMetadata)(ReferenceImage).Metadata).Format;
                        UpdateArticle.ImagePath = ".png";
                        UpdateArticle.Reference = ArticleData.Reference;
                        UpdateArticle.IdArticle = ArticleData.IdArticle;
                        UpdateArticle.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        if (CheckImageBytes.Length <= 25000)
                            UpdateArticle.ArticleImageInBytes = CheckImageBytes;
                        else
                        {
                            var ProfileImage = ImageSourceToBytes(ReferenceImage);
                            byte[] profileImageInByte = (byte[])ProfileImage;
                            UpdateArticle.ArticleImageInBytes = profileImageInByte;
                        }
                        ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleReferenceImageAdded").ToString()) });

                    }
                    else
                    {
                        //Image updated.
                        byte[] refImageByte = ImageSourceToBytes(ReferenceImage);
                        byte[] oldImageByte = ImageSourceToBytes(OldReferenceImage);

                        bool imageResult = oldImageByte.SequenceEqual(refImageByte);

                        if (!imageResult)
                        {
                            UpdateArticle.IsAddedArticleImage = true;
                            UpdateArticle.ImagePath = ".png";
                            UpdateArticle.Reference = ArticleData.Reference;
                            UpdateArticle.IdArticle = ArticleData.IdArticle;
                            UpdateArticle.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            byte[] CheckImageBytes = ImageSourceToBytes(ReferenceImage);

                            if (CheckImageBytes.Length <= 25000)
                                UpdateArticle.ArticleImageInBytes = CheckImageBytes;
                            else
                            {
                                var ProfileImage = ImageSourceToBytes(ReferenceImage);
                                byte[] profileImageInByte = (byte[])ProfileImage;
                                UpdateArticle.ArticleImageInBytes = profileImageInByte;
                            }

                            ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleReferenceImageUpdated").ToString()) });
                        }
                    }
                }
                else
                {
                    if (((System.Windows.Media.Imaging.BitmapImage)OldReferenceImage).UriSource == null)
                    {
                        //If profile is image deleted
                        if (((System.Windows.Media.Imaging.BitmapImage)OldReferenceImage).UriSource == null)
                        {
                            if (Uri.Compare(((System.Windows.Media.Imaging.BitmapImage)defaultReferenceImage).UriSource,
                                        ((System.Windows.Media.Imaging.BitmapImage)ReferenceImage).UriSource,
                                        UriComponents.AbsoluteUri,
                                        UriFormat.SafeUnescaped,
                                        StringComparison.InvariantCulture) == 0)
                            {
                                UpdateArticle.IsDeletedArticleImage = true;
                                UpdateArticle.Reference = ArticleData.Reference;
                                UpdateArticle.IdArticle = ArticleData.IdArticle;
                                UpdateArticle.ImagePath = ArticleData.ImagePath;
                                UpdateArticle.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleReferenceImageDeleted").ToString()) });
                            }
                        }
                    }
                }

                foreach (ArticleWarehouseLocations warehouseLocation in UpdatedArticlePositionList)
                {
                    UpdatedArticleWarehouseLocationsList.Remove(warehouseLocation);
                    ArticleWarehouseLocations articleWarehouse = ArticleData.LstArticleWarehouseLocations.Where(x => x.IdWarehouseLocation == warehouseLocation.IdWarehouseLocation).FirstOrDefault();
                    ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationPositionChanged").ToString(), warehouseLocation.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name, articleWarehouse.Position, warehouseLocation.Position) });
                }

                foreach (ArticleWarehouseLocations WarehouseLoction in UpdatedArticleWarehouseLocationsList)
                {
                    if (WarehouseLoction.TransactionOperation == ModelBase.TransactionOperations.Add)
                    {
                        ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationAdd").ToString(), WarehouseLoction.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name) });
                    }

                    if (WarehouseLoction.TransactionOperation == ModelBase.TransactionOperations.Update)
                    {
                        ArticleWarehouseLocations articleWarehouseLocation = ArticleWarehouseLocationsListForLog.FirstOrDefault(x => x.IdWarehouseLocation == WarehouseLoction.IdWarehouseLocation);

                        if (articleWarehouseLocation != null)
                        {
                            if (WarehouseLoction.MaximumStock != articleWarehouseLocation.MaximumStock)
                                ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationUpdateMaximumstock").ToString(), WarehouseLoction.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name, articleWarehouseLocation.MaximumStock, WarehouseLoction.MaximumStock) });
                            if (WarehouseLoction.MinimumStock != articleWarehouseLocation.MinimumStock)
                                ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationUpdateMinimumstock").ToString(), WarehouseLoction.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name, articleWarehouseLocation.MinimumStock, WarehouseLoction.MinimumStock) });
                        }
                        else
                            ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationAdd").ToString(), WarehouseLoction.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name) });
                    }
                    if (WarehouseLoction.TransactionOperation == ModelBase.TransactionOperations.Delete)
                    {
                        ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = ArticleData.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationDelete").ToString(), WarehouseLoction.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name) });
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ArticleChangeLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ArticleChangeLogDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000][avpawar][17/04/2019][GEOS2-75][Export stock history]
        /// Method for export to excel.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportStockHistoryButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportStockHistoryButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Stock_History";
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

                    TableView tblStockHistory = ((TableView)obj);
                    tblStockHistory.ShowTotalSummary = false;
                    tblStockHistory.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    tblStockHistory.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    tblStockHistory.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportStockHistoryButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportStockHistoryButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        /// <summary>
        /// [SP66][001][avpawar][03/07/2019][GEOS2-1604][Add new colums "DN" and "Location" in Article Stock History]
        /// Method to Open Delivery Note on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
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
                ArticlesStock ac = (ArticlesStock)tblView.DataControl.CurrentItem;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);
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

        /// <summary>
        /// [001][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// </summary>
        /// <param name="obj"></param>
        private void PrintArticleLabel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintArticleLabel....", category: Category.Info, priority: Priority.Low);

                PrintArticleLabelView printArticleLabelView = new PrintArticleLabelView();
                PrintArticleLabelViewModel printArticleLabelViewModel = new PrintArticleLabelViewModel();
                EventHandler handle = delegate { printArticleLabelView.Close(); };
                printArticleLabelViewModel.RequestClose += handle;
                ObservableCollection<Article> articlesList = new ObservableCollection<Article>();
                articlesList.Add(ArticleData);
                printArticleLabelViewModel.Init(articlesList);
                printArticleLabelView.DataContext = printArticleLabelViewModel;
                var ownerInfo = (obj as FrameworkElement);
                printArticleLabelView.Owner = Window.GetWindow(ownerInfo);
                printArticleLabelView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method PrintArticleLabel....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLabel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [005][avpawar][14-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="WarehouseId"></param>
        /// <param name="Selectedwarehouse"></param>
        /// <param name="ArticleSleepDays"></param>
        public void Init_SRM(string reference, long WarehouseId, Warehouses Selectedwarehouse, int ArticleSleepDays)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init_SRM()...", category: Category.Info, priority: Priority.Low);
                WarehouseCommon.Instance.Selectedwarehouse = Selectedwarehouse;

                //[SP67 001]added

                // [002] Changed Service method
                // ArticleData = WarehouseService.GetArticleDetailsByReference_V2037(reference, Selectedwarehouse);

                // [003] Changed Service method
                //ArticleData = WarehouseService.GetArticleDetailsByReference_V2038(reference, Selectedwarehouse);

                // [004] Changed Service method
                //ArticleData = WarehouseService.GetArticleDetailsByReference_V2041(reference, Selectedwarehouse);

                // [005] Changed Service method
                ArticleData = WarehouseService.GetArticleDetailsByReference_V2051(reference, Selectedwarehouse);

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 43))
                {
                    IsAddComment = true;
                }
                else
                {
                    IsAddComment = false;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 44))
                {
                    IsEditComment = true;
                }
                else
                {
                    IsEditComment = false;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 45))
                {
                    IsDeleteComment = true;
                }
                else
                {
                    IsDeleteComment = false;
                }

                ArticleForeCastPOCount = ArticleData.LstArticleForecast.Where(x => x.Type == "PO").Sum(x => x.Quantity);
                ArticleForeCastWOCount = ArticleData.LstArticleForecast.Where(x => x.Type == "OT").Sum(x => x.Quantity);

                ArticleData.LstArticleDecomposition = ArticleData.LstArticleDecomposition.Where(de => de.IdParent == ArticleData.IdArticle).ToList();
                ArticleWarehouseLocationsList = new ObservableCollection<ArticleWarehouseLocations>(ArticleData.LstArticleWarehouseLocations.Select(x => (ArticleWarehouseLocations)x.Clone()).OrderBy(x => x.Position).ToList());
                ArticleWarehouseCommentsList = new ObservableCollection<ArticleComment>(ArticleData.LstArticleComment.Select(x => (ArticleComment)x.Clone()).OrderBy(x => x.IdArticleComment).ToList());
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetAllWarehouseLocationsByIdWarehouse(WarehouseId));
                List<Int64> IntArticleWarehouseLocationsList = ArticleWarehouseLocationsList.Select(awl => awl.IdWarehouseLocation).ToList();
                List<Int64> IntWarehouseLocationList = WarehouseLocationList.Select(wl => wl.IdWarehouseLocation).ToList();
                List<Int64> IntWarehouseLocationListfinal = IntWarehouseLocationList.Where(wlf => !IntArticleWarehouseLocationsList.Contains(wlf)).ToList();
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseLocationList.Where(x => IntWarehouseLocationListfinal.Contains(x.IdWarehouseLocation)).ToList());
                UpdatedArticleWarehouseLocationsList = new ObservableCollection<ArticleWarehouseLocations>();
                UpdatedArticleCommentList = new ObservableCollection<ArticleComment>();
                UpdatedArticlePositionList = new ObservableCollection<ArticleWarehouseLocations>();

                ArticleWarehouseLocationsListForLog = new ObservableCollection<ArticleWarehouseLocations>(ArticleData.LstArticleWarehouseLocations.Select(x => (ArticleWarehouseLocations)x.Clone()).ToList());



                //ArticleWarehouseCommentsList = new ObservableCollection<ArticleComment>()
                //    {
                //         new ArticleComment(){Comment="First Comment",IdStage=1},
                //         new ArticleComment(){Comment="Second Comment",IdStage=2}
                //     };

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 140;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 130;

                if (ArticleData != null)
                {
                    FavouriteSupplier = ArticleData.LstArticleBySupplier.Where(x => x.IsTheFavorite == 1).SingleOrDefault();
                    if (ArticleData.LstArticleWarehouseLocations == null || ArticleData.LstArticleWarehouseLocations.Count != 0)
                    {
                        InputStockLocation = ArticleData.LstArticleWarehouseLocations.OrderBy(x => x.WarehouseLocation.Position).Last();
                        OutputStockLocation = ArticleData.LstArticleWarehouseLocations.OrderBy(x => x.WarehouseLocation.Position).First();
                    }

                    double weight = System.Convert.ToDouble(ArticleData.Weight);

                    if (System.Convert.ToDouble(ArticleData.Weight) < 1)
                    {
                        if (Math.Round(weight * 1000, 0) == 1000)
                        {
                            ArticleWeight = Math.Round(weight * 1000, 0);
                            ArticleWeightSymbol = " (Kg) :";
                        }
                        ArticleWeight = Math.Round(weight * 1000, 0);
                        ArticleWeightSymbol = " (gr) :";
                    }
                    else
                    {
                        ArticleWeight = Math.Round(weight, 3);
                        ArticleWeightSymbol = " (Kg) :";
                    }

                    if (ArticleData.IsObsolete == 0)
                    {
                        ArticleActive = true;
                    }
                    else
                    {
                        if (ArticleData.ReplacementArticle != null && ArticleData.ReplacementArticle != "")
                            IsReplacementAvailable = true;

                        ArticleActive = false;
                    }

                    //SetReferenceImage(ArticleData.ArticleImageInBytes);
                    ReferenceImage = ByteArrayToBitmapImage(ArticleData.ArticleImageInBytes);
                    OldReferenceImage = ReferenceImage;
                }

                if (ArticleData.MyWarehouse != null)
                {
                    MinimumQuantity = ArticleData.MyWarehouse.MinimumStock;
                    MaximumQuantity = ArticleData.MyWarehouse.MaximumStock;
                }
                else
                    ChangeCurrentStockColor(0, 0);

                if (ArticleWarehouseLocationsList.Count > 0)
                {
                    CheckWarehousePositions();
                    if (ArticleWarehouseLocationsList.Count > 1)
                        SelectedLocation = ArticleWarehouseLocationsList[1];
                }

                //[004]added
                ArticleTypesList = new List<ArticleType>(WarehouseService.GetArticleTypes(Selectedwarehouse));
                if (ArticleTypesList.Count > 0)
                    SelectedArticleType = ArticleTypesList.FirstOrDefault(x => x.IdArticleType == ArticleData.ArticlesType.IdArticleType);

                //ArticleAvailabilityMessage
                if (ArticleActive && ArticleData.MyWarehouse.CurrentStock > 0)
                    ArticleAvailabilityMessage = System.Windows.Application.Current.FindResource("ArticleAvailabilityDetails").ToString();
                else if (ArticleActive && ArticleData.MyWarehouse.CurrentStock == 0)
                    ArticleAvailabilityMessage = System.Windows.Application.Current.FindResource("ArticleOutOfStockDetails").ToString();
                else if (!ArticleActive)
                    ArticleAvailabilityMessage = System.Windows.Application.Current.FindResource("ArticleDiscontinuedDetails").ToString();

                //[005]added

                if (ArticleData.CreatedIn != null)
                {
                    DateCalculateInYearAndMonthHelper dateCalculateInYearAndMonth = new DateCalculateInYearAndMonthHelper(GeosApplication.Instance.ServerDateTime.Date, ArticleData.CreatedIn.Value.Date);
                    if (dateCalculateInYearAndMonth.Years > 0)
                    {
                        ArticleAgeDays = dateCalculateInYearAndMonth.Years.ToString() + "+";
                        ArticleAgeDaysString = string.Format(System.Windows.Application.Current.FindResource("ArticleYears").ToString());
                    }
                    else
                    {
                        ArticleAge = (GeosApplication.Instance.ServerDateTime.Date - ArticleData.CreatedIn.Value.Date).Days;

                        if (ArticleAge > 99)
                            ArticleAgeDays = "99+";
                        else
                            ArticleAgeDays = ArticleAge.ToString();

                        ArticleAgeDaysString = string.Format(System.Windows.Application.Current.FindResource("ArticleDays").ToString());
                    }
                }


                if (ArticleData.ArticleSleepDays == null)
                    SleepDaysVisibility = Visibility.Collapsed;
                else
                    SleepDaysVisibility = Visibility.Visible;

                if (ArticleData.ArticleSleepDays >= ArticleSleepDays)
                    IsSleepDaysReachedSettingValue = false;
                else
                    IsSleepDaysReachedSettingValue = true;

                //[007]added
                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(46);
                ArticleFamilyList = new List<LookupValue>(temptypeList);
                ArticleFamilyList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                SelectedFamilyIndex = ArticleFamilyList.FindIndex(x => x.IdLookupValue == ArticleData.MaterialType);
                if (SelectedFamilyIndex == -1)
                    SelectedFamilyIndex = 0;
                GeosApplication.Instance.Logger.Log("Method Init_SRM()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init_SRM()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //Methods
    }
}
