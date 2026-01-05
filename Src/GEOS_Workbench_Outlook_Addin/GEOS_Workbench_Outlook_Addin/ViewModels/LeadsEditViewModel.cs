using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using System.Windows.Media.Imaging;
using System.IO;
using System.Data;
using DevExpress.Xpf.Editors;
using Prism.Logging;
using System.ServiceModel;
using Emdep.Geos.UI.Validations;
using System.Windows.Data;
using System.Globalization;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;
//using DevExpress.Compression;
using System.Windows.Documents;
using System.Windows.Controls;
using Microsoft.Win32;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.RichEdit;
//using DevExpress.XtraRichEdit.API.Layout;
//using DevExpress.XtraRichEdit.API.Native;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.Spreadsheet;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    [POCOViewModel]
    public class LeadsEditViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        // [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        #endregion

        #region Services
       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion // Services

        #region Declaration

        //private IList<Customer> companyList;
        private bool split;
        private int screenHeight;
        private int windowHeight;
        private int commentboxHeight;
        private ObservableCollection<Customer> companyGroupList;
        private List<Company> companyPlantList;
        private List<People> salesOwnerList;
        List<LogEntryByOffer> changeLogsEntry;
        ObservableCollection<GeosStatus> geosStatusList;
        ObservableCollection<GeosStatus> geosStatusListSplit;
        List<Currency> currencies;
        private List<Quotation> templateDetailList;
        private bool isBusy;
        private bool isReadonly = false;
        private bool isControlEnableorder = true;
        private bool isBoxControlEnable = false;
        private bool isAcceptControlEnableorder = true;
        private bool isAcceptEnable = true;
        private bool isControlEnable = false;//readonly
        private bool isCategoryControlEnable = true;
        private bool isStatusChangeAction = false;
        private bool isOldOT = false;
        public List<OptionsByOffer> OptionsByOfferList { get; set; }
        private IList<Template> templateList;
        private Visibility visibilityLost = Visibility.Hidden;
        private Visibility visibilityAccept = Visibility.Hidden;
        private Visibility visibilityShipment = Visibility.Collapsed;
        private Visibility visibilityForecast = Visibility.Hidden;

        private int selectedIndexCompanyGroup = 0;
        private int selectedIndexCompanyPlant = 0;
        private long tempIdOfferStatusType = 0;
        private int selectedIndexBusinessUnit = 0;
        private int selectedIndexLeadSource = 0;    // Lead Source
        private int selectedIndexConfidentialLevel = 0;
        private int selectedIndexCurrency = 0;
        private int selectedStarIndex;
        private int selectedIndexSalesOwner;
        private int selectedIndexCarOEM;
        private int selectedIndexGeosProject;
        private GeosStatus selectedGeosStatus;
        private GeosStatus selectedGeosStatusSplit;
        private int selectedIndexGeosStatus;
        private string informationError;
        public int LeadsGenerateDays { get; set; }
        public int LeadsSleepDays { get; set; }
        //public List<Company> LeadCompanylst;
        private List<Offer> offerDataLst;
        private List<Shipment> listShipment;
        private List<PackingBox> listBox;

        private int selectedIndexOfferType = 0;
        public int TempSelectedIndexOfferType { get; set; }
        private double offerAmount;
        Double max_Value;
        //Double max_Value_TaskSuggestion;
        private bool IsInIt;
        public bool IsSaleOwnerNull { get; set; }
        private DateTime? offerCloseDate;

        private DateTime? rfqReceptionDate;
        private DateTime? quoteSentDate;
        private string offerCode;
        public string TempOfferCode { get; set; }
        private string description;
        private string companyName;
        private bool isShowAll;
        public bool IsLostStatusSet { get; set; }

        public virtual Color Background { get; set; }
        private Color hoverBackground;
        private Color selectedBackground;
        private long OfferNumber;

        private IList<Offer> selectedLeadList;
        private Object selectedShipment;
        private IList<Offer> selectedSplitLeadList;
        private DateTime offerCloseDateMinValue;

        //private List<LogEntryByOffer> listLogEntry{get;set;}

        public List<OfferType> OfferTypeList { get; set; }

        //public string LeadsEditViewTitle { get; set; }
        private int selectedIndex;

        public string leadsEditViewTitle;

        public string LeadsEditViewCloseDate { get; set; }
        public List<int> ConfidentialLevelList { get; set; }
        public List<LookupValue> BusinessUnitList { get; set; }

        private ObservableCollection<Template> leadsList;

        private List<CarOEM> caroemsList;
        private List<CarProject> geosProjectsList;
        private List<string> geosProjectsListTemp;
        private string salesOwnersIds = "";
        private bool isFromShowAll = false;



        private byte[] UserProfileImageByte = null;
        private DataTable dttable { get; set; }
        private List<string> errorList;
        //bool isTechnicalTemplateChange = false;
        private Offer offerData;

        private int productAndServicesCount;
        private ObservableCollection<GeosStatus> geosEnabledStatusList = new ObservableCollection<GeosStatus>();
        private ObservableCollection<LookupValue> leadSourceList;   // Lead Source
        bool showCommentsFlyout;
        private string commentButtonText;
        private bool isAdd;
        private string commentText;
        private Object selectedComment;
        private int selectedViewIndex;
        bool forLeadOpen;
        private Company CurrentCompany;
        bool isSplitVisible = true;
        private ObservableCollection<Task> tasks = new ObservableCollection<Task>();

        private string showStatusReason = " ";

        private List<OfferOption> MainOfferOptions { get; set; }
        private List<OfferOption> offerOptions { get; set; }
        private List<LogEntryByOffer> changeLogCommentEntry = new List<LogEntryByOffer>();
        private string oldLeadComment;
        private DateTime deliveryDate;
        private string rfq;
        private bool gridRowHeight;
        private bool gridRowHeightForRfq;
        private bool gridRowHeightForQuoteSent;
        private Offer showStatusList;

        private ObservableCollection<OfferContact> listOfferContact = new ObservableCollection<OfferContact>();
        private ObservableCollection<LogEntryByOffer> listChangeLog = new ObservableCollection<LogEntryByOffer>();
        private string leadComment;
        private ObservableCollection<LogEntryByOffer> listLogComments = new ObservableCollection<LogEntryByOffer>();
        private OfferContact primaryOfferContact;
        private ObservableCollection<People> listCustomerContact;
        private Int16 idSite;
        private ObservableCollection<People> listAddedContact = new ObservableCollection<People>();
        private string previousPrimaryContact;
        private bool isPrimayContactChanged;
        private bool isFirstPrimaryContact;

        private ObservableCollection<Activity> leadActivityList;
        private Activity selectedActivity;
        private ObservableCollection<Activity> existingActivitiesTobeLinked;
        private Visibility isSleepDaysVisible;
        private bool isCommentChange;
        private bool isActivityChange;
        private bool isRtf;
        private bool isNormal = true;
        private ImageSource userProfileImage;
        private List<ActivityTemplateTrigger> activityTemplateTriggers;


        //EngneeringAnalysis

        bool isEngAnalysis;
        Visibility isEngAnalysisVisible;
        //Visibility isEngAnalysisButtonVisible = Visibility.Collapsed;
        private bool isExistEngAnalysis;
        private bool isEngAnalysisEnable = true;

        //private int rowNumberChangebyEngAnalysisRow0;//for adjustment of column height for Engineering analysis
        //private int rowNumberChangebyEngAnalysisRow1;//for adjustment of column height for Engineering analysis
        //private int rowNumberChangebyEngAnalysisRow2;//for adjustment of column height for Engineering analysis
        //private int rowNumberChangebyEngAnalysisRow3;//for adjustment of column height for Engineering analysis

        private EngineeringAnalysis existedEngineeringAnalysis;
        private EngineeringAnalysis existedEngineeringAnalysisDuplicate;
        FileUploader engAnalysisAttachmentFileUploadIndicator;
        private ObservableCollection<Company> entireCompanyPlantList;
        //private List<object> attacmentListForEdit;
        bool isSiteResponsibleRemoved;
        bool isStatusDisabled;
        private ObservableCollection<ProductCategory> listProductCategory;
        private ProductCategory selectedCategory;

        private string convertedOfferAmount;
        private bool gridRowHeightForAmount;
        private double convertedAmount;
        //EngneeringAnalysis

        // [001] added
        private List<User> offerOwnerList;
        private int selectedIndexOfferOwner;
        private List<Object> selectedOfferToList = new List<object>();
        private List<OfferContact> offerToList;
        private ObservableCollection<OfferContact> listAddedOfferContact;
        private ActiveSite offerActiveSite;

        #endregion

        #region  public Properties
                
        public ProductCategory SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCategory"));
            }
        }
        public ObservableCollection<ProductCategory> ListProductCategory
        {
            get { return listProductCategory; }
            set
            {
                listProductCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListProductCategory"));
            }
        }
        public bool IsStatusDisabled
        {
            get
            {
                return isStatusDisabled;
            }

            set
            {
                isStatusDisabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStatusDisabled"));
            }
        }


        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }
        public bool IsSiteResponsibleRemoved
        {
            get
            {
                return isSiteResponsibleRemoved;
            }

            set
            {
                isSiteResponsibleRemoved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSiteResponsibleRemoved"));
            }
        }
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }

            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
            }
        }
        public int CommentboxHeight
        {
            get
            {
                return commentboxHeight;
            }

            set
            {
                commentboxHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentboxHeight"));
            }
        }
        public FileUploader EngAnalysisAttachmentFileUploadIndicator
        {
            get { return engAnalysisAttachmentFileUploadIndicator; }
            set
            {
                engAnalysisAttachmentFileUploadIndicator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysisAttachmentFileUploadIndicator"));
            }
        }
        public EngineeringAnalysis ExistedEngineeringAnalysis
        {
            get
            {
                return existedEngineeringAnalysis;
            }

            set
            {
                existedEngineeringAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistedEngineeringAnalysis"));
            }
        }

        public EngineeringAnalysis ExistedEngineeringAnalysisDuplicate
        {
            get
            {
                return existedEngineeringAnalysisDuplicate;
            }

            set
            {
                existedEngineeringAnalysisDuplicate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistedEngineeringAnalysisDuplicate"));
            }
        }
        #region
        //public Visibility IsEngAnalysisButtonVisible
        //{
        //    get
        //    {
        //        return isEngAnalysisButtonVisible;
        //    }

        //    set
        //    {
        //        isEngAnalysisButtonVisible = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisButtonVisible"));
        //    }
        //}
        #endregion

        public bool IsEngAnalysisEnable
        {
            get
            {
                return isEngAnalysisEnable;
            }

            set
            {
                isEngAnalysisEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisEnable"));
            }
        }

        public Visibility IsEngAnalysisVisible
        {
            get
            {
                return isEngAnalysisVisible;
            }

            set
            {
                isEngAnalysisVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisVisible"));
            }
        }
        public bool IsEngAnalysis
        {
            get
            {
                return isEngAnalysis;
            }
            set
            {
                isEngAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysis"));

                #region Old commented code

                //if (value)
                //{
                //    //IsEngAnalysis = true;
                //    if (ExistedEngineeringAnalysis == null || ExistedEngineeringAnalysis.Attachments == null || ExistedEngineeringAnalysis.Attachments.Count <= 0)
                //    {
                //        GeosApplication.Instance.Logger.Log("Method LeadsNewViewWindowShow...", category: Category.Info, priority: Priority.Low);
                //        DXSplashScreen.Show(x =>
                //        {
                //            Window win = new Window()
                //            {
                //                ShowActivated = false,
                //                WindowStyle = WindowStyle.None,
                //                ResizeMode = ResizeMode.NoResize,
                //                AllowsTransparency = true,
                //                Background = new SolidColorBrush(Colors.Transparent),
                //                ShowInTaskbar = false,
                //                Topmost = true,
                //                SizeToContent = SizeToContent.WidthAndHeight,
                //                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //            };
                //            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //            win.Topmost = false;
                //            return win;
                //        }, x =>
                //        {
                //            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //        }, null, null);

                //        AddEngineeringAnalysisViewModel addEngineeringAnalysisViewModel = new AddEngineeringAnalysisViewModel();
                //        AddEngineeringAnalysisView addEngineeringAnalysisView = new AddEngineeringAnalysisView();
                //        EventHandler handle = delegate { addEngineeringAnalysisView.Close(); };
                //        addEngineeringAnalysisViewModel.RequestClose += handle;
                //        addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;
                //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //        addEngineeringAnalysisView.ShowDialogWindow();
                //        if (addEngineeringAnalysisViewModel.IsSave)
                //        {
                //            ExistedEngineeringAnalysis = addEngineeringAnalysisViewModel.EngAnalysis;
                //            ExistedEngineeringAnalysisDuplicate = addEngineeringAnalysisViewModel.EngAnalysisDuplicate;
                //            EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;

                //            if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                //            {
                //                OptionsByOffer optionsByOffer = new OptionsByOffer();
                //                optionsByOffer.IdOption = 25;
                //                optionsByOffer.OfferOption = MainOfferOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                //                optionsByOffer.IsSelected = true;
                //                optionsByOffer.Quantity = 1;
                //                OptionsByOfferList.Add(optionsByOffer);
                //            }
                //            IsEngAnalysisButtonVisible = Visibility.Visible;
                //        }
                //        else
                //        {
                //            IsEngAnalysisButtonVisible = Visibility.Collapsed;
                //            IsEngAnalysis = false;
                //        }
                //    }
                //    else
                //    {
                //        if (!IsInIt)
                //        {
                //            if (ExistedEngineeringAnalysis != null && ExistedEngineeringAnalysis.Attachments != null && ExistedEngineeringAnalysis.Attachments.Count > 0)
                //            {
                //                ExistedEngineeringAnalysis = SelectedLeadList[0].EngineeringAnalysis;
                //                IsExistEngAnalysis = true;
                //                IsEngAnalysisEnable = true;
                //                IsEngAnalysisButtonVisible = Visibility.Visible;
                //                ExistedEngineeringAnalysisDuplicate = SelectedLeadList[0].EngineeringAnalysis;
                //            }
                //            else
                //            {
                //                AddEngineeringAnalysisViewModel addEngineeringAnalysisViewModel = new AddEngineeringAnalysisViewModel();
                //                AddEngineeringAnalysisView addEngineeringAnalysisView = new AddEngineeringAnalysisView();
                //                EventHandler handle = delegate { addEngineeringAnalysisView.Close(); };
                //                addEngineeringAnalysisViewModel.RequestClose += handle;
                //                addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;
                //                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //                addEngineeringAnalysisView.ShowDialogWindow();
                //                if (addEngineeringAnalysisViewModel.IsSave)
                //                {
                //                    ExistedEngineeringAnalysis = addEngineeringAnalysisViewModel.EngAnalysis;
                //                    ExistedEngineeringAnalysisDuplicate = addEngineeringAnalysisViewModel.EngAnalysisDuplicate;
                //                    EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;
                //                    if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                //                    {
                //                        OptionsByOffer optionsByOffer = new OptionsByOffer();
                //                        optionsByOffer.IdOption = 25;
                //                        optionsByOffer.OfferOption = MainOfferOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                //                        optionsByOffer.IsSelected = true;
                //                        optionsByOffer.Quantity = 1;
                //                        OptionsByOfferList.Add(optionsByOffer);
                //                    }
                //                    else
                //                    {
                //                        OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                //                        if (opt.IsSelected == false)
                //                        {
                //                            opt.IsSelected = true;
                //                            opt.Quantity = 1;
                //                        }
                //                    }
                //                    IsEngAnalysisButtonVisible = Visibility.Visible;
                //                    IsEngAnalysis = false;
                //                }
                //                else
                //                {
                //                    IsEngAnalysisButtonVisible = Visibility.Collapsed;
                //                    IsEngAnalysis = false;
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    if (ExistedEngineeringAnalysis != null)
                //    {
                //        if (IsEngAnalysis != false)
                //        {
                //            ExistedEngineeringAnalysis = SelectedLeadList[0].EngineeringAnalysis;
                //            IsEngAnalysisButtonVisible = Visibility.Collapsed;
                //            ExistedEngineeringAnalysisDuplicate = SelectedLeadList[0].EngineeringAnalysis;
                //        }
                //        else
                //        {
                //            ExistedEngineeringAnalysis = new EngineeringAnalysis();
                //            ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();
                //            EngAnalysisAttachmentFileUploadIndicator = new FileUploader();

                //            if (OptionsByOfferList != null && OptionsByOfferList.Count > 0)
                //            {
                //                if (OptionsByOfferList.Any(ps => ps.IdOption == 25))
                //                {
                //                    OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                //                    opt.IsSelected = false;
                //                    opt.Quantity = 0;
                //                }
                //            }
                //            IsEngAnalysisButtonVisible = Visibility.Collapsed;
                //        }
                //    }
                //    else
                //    {
                //        ExistedEngineeringAnalysis = new EngineeringAnalysis();
                //        ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();
                //        EngAnalysisAttachmentFileUploadIndicator = new FileUploader();

                //        if (OptionsByOfferList != null && OptionsByOfferList.Count > 0)
                //        {
                //            if (OptionsByOfferList.Any(ps => ps.IdOption == 25))
                //            {
                //                OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                //                opt.IsSelected = false;
                //                opt.Quantity = 0;
                //            }
                //        }
                //        IsEngAnalysisButtonVisible = Visibility.Collapsed;
                //    }
                //}

                #endregion
            }
        }

        public bool IsExistEngAnalysis
        {
            get { return isExistEngAnalysis; }
            set
            {
                isExistEngAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExistEngAnalysis"));

                #region Old commented code
                //if (isExistEngAnalysis)
                //{
                //    RowNumberChangebyEngAnalysisRow0 = 1;
                //    RowNumberChangebyEngAnalysisRow1 = 2;
                //    RowNumberChangebyEngAnalysisRow2 = 3;
                //    RowNumberChangebyEngAnalysisRow3 = 4;

                //}
                //else
                //{
                //    RowNumberChangebyEngAnalysisRow0 = 0;
                //    RowNumberChangebyEngAnalysisRow1 = 1;
                //    RowNumberChangebyEngAnalysisRow2 = 2;
                //    RowNumberChangebyEngAnalysisRow3 = 3;
                //}
                #endregion
            }
        }

        #region Old commented code
        //public int RowNumberChangebyEngAnalysisRow1
        //{
        //    get { return rowNumberChangebyEngAnalysisRow1; }
        //    set
        //    {
        //        rowNumberChangebyEngAnalysisRow1 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("RowNumberChangebyEngAnalysisRow1"));
        //    }
        //}

        //public int RowNumberChangebyEngAnalysisRow2
        //{
        //    get { return rowNumberChangebyEngAnalysisRow2; }
        //    set
        //    {
        //        rowNumberChangebyEngAnalysisRow2 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("RowNumberChangebyEngAnalysisRow2"));
        //    }
        //}
        //public int RowNumberChangebyEngAnalysisRow3
        //{
        //    get { return rowNumberChangebyEngAnalysisRow3; }
        //    set
        //    {
        //        rowNumberChangebyEngAnalysisRow3 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("RowNumberChangebyEngAnalysisRow3"));
        //    }
        //}

        //public int RowNumberChangebyEngAnalysisRow0
        //{
        //    get { return rowNumberChangebyEngAnalysisRow0; }
        //    set
        //    {
        //        rowNumberChangebyEngAnalysisRow0 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("RowNumberChangebyEngAnalysisRow0"));
        //    }
        //}

        //EngneeringAnalysis
        #endregion

        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set { userProfileImage = value; OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage")); }
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

        public bool IsActivityChange
        {
            get { return isActivityChange; }
            set { isActivityChange = value; OnPropertyChanged(new PropertyChangedEventArgs("IsActivityChange")); }
        }

        public bool IsCommentChange
        {
            get { return isCommentChange; }
            set { isCommentChange = value; OnPropertyChanged(new PropertyChangedEventArgs("IsCommentChange")); }
        }

        public Visibility IsSleepDaysVisible
        {
            get
            {
                return isSleepDaysVisible;
            }

            set
            {
                isSleepDaysVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSleepDaysVisible"));
            }
        }
        public string GuidCode { get; set; }
        public bool IsActivityCreateFromSaveOffer = false;
        public ObservableCollection<ActivityAttachment> ListAttachmentFinal { get; set; }

        public bool IsBoxControlEnable
        {
            get { return isBoxControlEnable; }
            set { isBoxControlEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsBoxControlEnable")); }
        }

        public Object SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        public Offer ShowStatusList
        {
            get { return showStatusList; }
            set
            {
                showStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowStatusList"));
            }
        }

        public string ShowStatusReason
        {
            get { return showStatusReason; }
            set { showStatusReason = value; }
        }

        private string showStatusDescription = " ";
        public string ShowStatusDescription
        {
            get { return showStatusDescription; }
            set { showStatusDescription = value; }
        }

        public int ProductAndServicesCount
        {
            get { return productAndServicesCount; }
            set
            {
                productAndServicesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductAndServicesCount"));
            }
        }
        public List<Offer> OfferDataLst
        {
            get { return offerDataLst; }
            set
            {
                offerDataLst = value;

            }
        }

        public Offer OfferData
        {
            get { return offerData; }
            set
            {
                offerData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferData"));
            }
        }
        public int SelectedIndexGeosStatus
        {
            get
            {
                return selectedIndexGeosStatus;
            }

            set
            {
                selectedIndexGeosStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGeosStatus"));
            }
        }

        public IList<Template> TemplateList
        {
            get { return templateList; }
            set { templateList = value; }
        }

        public ObservableCollection<Template> LeadsList
        {
            get { return leadsList; }
            set { leadsList = value; }
        }

        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }

        public List<Company> CompanyPlantList
        {
            get { return companyPlantList; }
            set
            {
                companyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyPlantList"));
            }
        }

        public ObservableCollection<GeosStatus> GeosStatusList
        {
            get { return geosStatusList; }
            set
            {
                geosStatusList = value;
            }
        }
        public ObservableCollection<GeosStatus> GeosStatusListSplit
        {
            get { return geosStatusListSplit; }
            set
            {
                geosStatusListSplit = value;
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

        List<OfferOption> OfferOptions { get; set; }
        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                selectedIndexCompanyGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                //if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                //{
                //    // DXSplashScreen.Show<SplashScreenView>(); 
                //}

                if (selectedIndexCompanyGroup > 0)
                {
                    IsBusy = true;
                    FillCompanyPlantList();

                    CompanyPlantList = new List<Company>();
                    CompanyPlantList = EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList();
                    //Sprint_60 (#69156) Blank site when editing some offer---Sdesai
                    bool isPlant = EntireCompanyPlantList.Any(x => x.IdCompany == SelectedPlantDetails.IdCompany);
                    if (!isPlant)
                    {
                        if (SelectedPlantDetails.Country != null)
                            SelectedPlantDetails.Name = SelectedPlantDetails.Name + "(" + SelectedPlantDetails.Country.Name + ")";

                        SelectedPlantDetails.IsStillActive = 0;
                        CompanyPlantList.Add(SelectedPlantDetails);
                    }
                    SelectedIndexCompanyPlant = CompanyPlantList.FindIndex(i => i.IdCompany == SelectedLeadList[0].Site.IdCompany);
                    if (SelectedIndexCompanyPlant == -1 || SelectedIndexCompanyPlant == 0)
                        SelectedIndexCompanyPlant = 0;
                    //FillGeosProjectsList();
                    IsBusy = false;
                }
                else
                {
                    SelectedIndexCompanyPlant = -1;
                    CompanyPlantList = null;
                    GeosProjectsListTemp = null;
                }

                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
        }

        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set
            {
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));

                if (selectedIndexCompanyPlant > -1)
                {
                    FillSalesOwnerList();
                }
                else
                {
                    SelectedIndexSalesOwner = -1;
                    SalesOwnerList = null;
                }
            }
        }

        public long TempIdOfferStatusType
        {
            get { return tempIdOfferStatusType; }
            set
            {
                tempIdOfferStatusType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexStatus"));
            }
        }

        public int SelectedIndexBusinessUnit
        {
            get { return selectedIndexBusinessUnit; }
            set
            {
                selectedIndexBusinessUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexBusinessUnit"));
            }
        }

        // Lead Source
        public ObservableCollection<LookupValue> LeadSourceList
        {
            get { return leadSourceList; }
            set
            {
                leadSourceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeadSourceList"));
            }
        }

        // Lead Source
        public int SelectedIndexLeadSource
        {
            get { return selectedIndexLeadSource; }
            set
            {
                selectedIndexLeadSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLeadSource"));
            }
        }

        public List<People> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerList"));
            }
        }

        public int SelectedIndexCurrency
        {
            get { return selectedIndexCurrency; }
            set
            {
                selectedIndexCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCurrency"));
            }
        }

        public List<Currency> Currencies
        {
            get { return currencies; }
            set { currencies = value; }
        }

        public int SelectedIndexConfidentialLevel
        {
            get { return selectedIndexConfidentialLevel; }
            set
            {
                selectedIndexConfidentialLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexConfidentialLevel"));
                SelectedConfidentialLevel();
            }
        }


        public List<LogEntryByOffer> ChangeLogCommentEntry
        {
            get { return changeLogCommentEntry; }
            set { changeLogCommentEntry = value; }
        }

        public string LeadComment
        {
            get { return leadComment; }
            set
            {
                leadComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeadComment"));
            }
        }

        public string OldLeadComment
        {
            get { return oldLeadComment; }
            set
            {
                oldLeadComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldLeadComment"));
            }
        }

        public int SelectedIndexOfferType
        {
            get { return selectedIndexOfferType; }
            set
            {
                selectedIndexOfferType = value; OnPropertyChanged(new PropertyChangedEventArgs("selectedIndexOfferType"));


            }
        }

        public double OfferAmount
        {
            get { return offerAmount; }
            set
            {
                offerAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferAmount"));
            }
        }

        /// <summary>
        /// Max_Value for Offer Amount
        /// </summary>
        public Double Max_Value
        {
            get { return max_Value; }
            set
            {
                max_Value = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Max_Value"));
            }
        }

        /// <summary>
        /// Max_Value_TaskSuggestion for Offer Amount to show Task suggestion.
        /// </summary>
        //public Double Max_Value_TaskSuggestion
        //{
        //    get { return max_Value_TaskSuggestion; }
        //    set
        //    {
        //        max_Value_TaskSuggestion = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Max_Value_TaskSuggestion"));
        //    }
        //}

        public DateTime? OfferCloseDate
        {
            get { return offerCloseDate; }
            set
            {
                offerCloseDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDate"));
            }
        }

        public DateTime? RFQReceptionDate
        {
            get { return rfqReceptionDate; }
            set
            {
                rfqReceptionDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RFQReceptionDate"));
            }
        }

        public DateTime? QuoteSentDate
        {
            get { return quoteSentDate; }
            set
            {
                quoteSentDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuoteSentDate"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (!string.IsNullOrEmpty(value))
                {
                    description = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public IList<Offer> SelectedLeadList
        {
            get { return selectedLeadList; }
            set
            {
                selectedLeadList = value;
            }
        }

        public Object SelectedShipment
        {
            get { return selectedShipment; }
            set
            {
                selectedShipment = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedShipment"));
            }
        }

        public IList<Offer> SelectedSplitLeadList
        {
            get { return selectedSplitLeadList; }
            set
            {
                selectedSplitLeadList = value;
            }
        }
        public Visibility VisibilityLost
        {
            get { return visibilityLost; }
            set
            {
                visibilityLost = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityLost"));
            }
        }

        public Visibility VisibilityAccept
        {
            get { return visibilityAccept; }
            set
            {
                visibilityAccept = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityAccept"));
            }
        }

        public Visibility VisibilityShipment
        {
            get { return visibilityShipment; }
            set
            {
                visibilityShipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityShipment"));
            }
        }


        public Visibility VisibilityForecast
        {
            get { return visibilityForecast; }
            set
            {
                visibilityForecast = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityForecast"));
            }
        }

        public List<Quotation> TemplateDetailList
        {
            get { return templateDetailList; }
            set
            {
                templateDetailList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateDetailList"));
            }
        }

        public ObservableCollection<LogEntryByOffer> ListLogComments
        {
            get { return listLogComments; }
            set
            {
                listLogComments = value;
                //SetProperty(ref listLogComments, value, () => ListLogComments);
                OnPropertyChanged(new PropertyChangedEventArgs("ListLogComments"));
            }
        }

        public List<Shipment> ListShipment
        {
            get { return listShipment; }
            set
            {
                listShipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListShipment"));
            }
        }

        public List<PackingBox> ListBox
        {
            get { return listBox; }
            set
            {
                listBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListBox"));
            }
        }

        public ObservableCollection<LogEntryByOffer> ListChangeLog
        {
            get { return listChangeLog; }
            set
            {

                //SetProperty(ref listChangeLog, value, () => ListChangeLog);
                listChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListChangeLog"));
            }
        }

        public DateTime OfferCloseDateMinValue
        {
            get { return offerCloseDateMinValue; }
            set
            {
                offerCloseDateMinValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDateMinValue"));
            }
        }

        public virtual Color SelectedBackground
        {
            get { return selectedBackground; }
            set
            {
                selectedBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBackground"));
            }
        }

        public virtual Color HoverBackground
        {
            get { return hoverBackground; }
            set
            {
                hoverBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HoverBackground"));
            }
        }

        public int SelectedStarIndex
        {
            get { return selectedStarIndex; }
            set
            {
                selectedStarIndex = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedStarIndex"));

                switch (selectedStarIndex)
                {
                    case 1:
                        SelectedBackground = Colors.Red;
                        break;

                    case 2:
                        SelectedBackground = Colors.Orange;
                        break;

                    case 3:
                        SelectedBackground = Colors.Yellow;
                        break;

                    case 4:
                        SelectedBackground = Colors.DeepSkyBlue;
                        break;

                    case 5:
                        SelectedBackground = Colors.Green;
                        break;
                    default:
                        SelectedBackground = Colors.Transparent;
                        break;
                }
            }
        }

        public int SelectedIndexSalesOwner
        {
            get { return selectedIndexSalesOwner; }
            set
            {
                selectedIndexSalesOwner = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSalesOwner"));
                if (SelectedIndexSalesOwner > -1)
                {
                    if (SelectedIndexSalesOwner == SalesOwnerList.FindIndex(i => i.IdPerson == SelectedLeadList[0].IdSalesOwner))
                    {
                        People ppl = SalesOwnerList[SelectedIndexSalesOwner];
                        if (ppl.IsSiteResponsibleExist == false)
                            IsSiteResponsibleRemoved = true;
                        else
                            IsSiteResponsibleRemoved = false;
                    }
                    else
                    {
                        IsSiteResponsibleRemoved = false;
                    }
                }
            }
        }

        public int SelectedIndexCarOEM
        {
            get { return selectedIndexCarOEM; }
            set
            {
                selectedIndexCarOEM = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCarOEM"));
            }
        }

        public int SelectedIndexGeosProject
        {
            get { return selectedIndexGeosProject; }
            set
            {
                selectedIndexGeosProject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGeosProject"));

                if (!IsInIt && SelectedIndexGeosProject != -1) //&& SelectedIndexGeosProject!=0)
                {

                    if (GeosProjectsList != null && GeosProjectsList.Count != 0 && GeosProjectsList[SelectedIndexGeosProject] != null)
                        SelectedIndexCarOEM = CaroemsList.FindIndex(cr => cr.IdCarOEM == GeosProjectsList[SelectedIndexGeosProject].IdCarOem);
                }
                else if (!IsInIt && SelectedIndexGeosProject == -1) //|| SelectedIndexGeosProject == 0)
                {
                    SelectedIndexCarOEM = 0;
                    //if(SelectedIndexGeosProject!=0)
                    //SelectedIndexGeosProject = 0;
                }
            }
        }

        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }

        public bool IsShowAll
        {
            get { return isShowAll; }
            set
            {
                isShowAll = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowAll"));
            }
        }

        public List<CarOEM> CaroemsList
        {
            get { return caroemsList; }
            set
            {
                caroemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CaroemsList"));
            }
        }

        public List<CarProject> GeosProjectsList
        {
            get { return geosProjectsList; }
            set
            {
                geosProjectsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosProjectsList"));
            }
        }

        public List<string> GeosProjectsListTemp
        {
            get { return geosProjectsListTemp; }
            set
            {
                geosProjectsListTemp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosProjectsListTemp"));
            }
        }

        public List<LogEntryByOffer> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }

        public GeosStatus SelectedGeosStatus
        {
            get
            {
                return selectedGeosStatus;
            }
            set
            {
                selectedGeosStatus = value;

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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

                if (selectedGeosStatus.IdOfferStatusType > 0)
                {
                    if (selectedGeosStatus.IdOfferStatusType == 17 || selectedGeosStatus.IdOfferStatusType == 0)
                    {
                    }
                    else
                    {
                        TempIdOfferStatusType = selectedGeosStatus.IdOfferStatusType;
                        IsLostStatusSet = false;
                    }

                    // Added this code - Close date not mandatory if the status is 17-lost or 4-cancelled
                    if (selectedGeosStatus.IdOfferStatusType == 4 || selectedGeosStatus.IdOfferStatusType == 17)
                    {
                        OfferCloseDateMinValue = DateTime.MinValue;
                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("OfferCloseDate"));
                        }
                    }
                    else
                    {
                        OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
                    }

                    if (selectedGeosStatus.IdOfferStatusType == 1 || selectedGeosStatus.IdOfferStatusType == 2)
                    {
                        IsEngAnalysisEnable = true;

                        //if (IsEngAnalysis == true)
                        //{
                        //    //IsExistEngAnalysis = true;
                        //    //IsEngAnalysisButtonVisible = Visibility.Visible;
                        //}
                        //else
                        //{
                        //    //IsExistEngAnalysis = true;
                        //}
                    }
                    else
                    {
                        //IsEngAnalysisButtonVisible = Visibility.Collapsed;
                        //IsExistEngAnalysis = false;

                        IsEngAnalysisEnable = false;
                    }

                    StatusChangeAction();
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedGeosStatus"));

                    //Start  This Method convert Lead to OT and vice versa.

                    SetOfferCodeByCondition();

                    //END This Method convert Lead to OT and vice versa.

                    string error = EnableValidationAndGetError();

                    // This part is used to apply validation on RFQReceptionDate/QuoteSentDate.
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("RFQReceptionDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("QuoteSentDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("OfferAmount"));
                    }
                }
                if (selectedGeosStatus.IsEnabled == true)
                    IsStatusDisabled = false;
                else
                    IsStatusDisabled = true;
                //IsStatusDisabled = false;


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
        }

        public bool IsReadonly
        {
            get { return isReadonly; }
            set
            {
                isReadonly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadonly"));
            }
        }

        public bool IsControlEnableorder
        {
            get { return isControlEnableorder; }
            set
            {
                isControlEnableorder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsControlEnableorder"));
            }
        }
        public bool IsAcceptControlEnableorder
        {
            get { return isAcceptControlEnableorder; }
            set
            {
                isAcceptControlEnableorder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptControlEnableorder"));
            }
        }

        public bool IsAcceptEnable
        {
            get { return isAcceptEnable; }
            set
            {
                isAcceptEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptControlEnableorder"));
            }
        }

        public bool IsControlEnable
        {
            get { return isControlEnable; }
            set
            {
                isControlEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsControlEnable"));
            }
        }

        public bool IsCategoryControlEnable
        {
            get { return isCategoryControlEnable; }
            set
            {
                isCategoryControlEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCategoryControlEnable"));
            }
        }

        public bool IsStatusChangeAction
        {
            get { return isStatusChangeAction; }
            set
            {
                isStatusChangeAction = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStatusChangeAction"));
            }
        }

        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
            }
        }

        public string CommentButtonText
        {
            get { return commentButtonText; }
            set
            {
                commentButtonText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentButtonText"));
            }
        }
        public bool IsAdd
        {
            get
            {
                return isAdd;
            }

            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }
        public string CommentText
        {
            get
            {
                return commentText;
            }

            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }
        public string OfferCode
        {
            get { return offerCode; }
            set
            {
                offerCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCode"));
            }
        }

        //lsharma

        public ObservableCollection<OfferContact> ListOfferContact
        {
            get { return listOfferContact; }
            set
            {
                listOfferContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfferContact"));
            }
        }


        public OfferContact PrimaryOfferContact
        {
            get { return primaryOfferContact; }
            set { primaryOfferContact = value; }
        }

        public ObservableCollection<People> ListCustomerContact
        {
            get { return listCustomerContact; }
            set
            {
                listCustomerContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListCustomerContact"));
                //  SetProperty(ref customerContactList, value, () => CustomerContactList);
            }
        }

        public Int16 IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        public ObservableCollection<People> ListAddedContact
        {
            get { return listAddedContact; }
            set
            {

                listAddedContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAddedContact"));
            }
        }

        public string PreviousPrimaryContact
        {
            get { return previousPrimaryContact; }
            set { previousPrimaryContact = value; }
        }

        public bool IsPrimayContactChanged
        {
            get { return isPrimayContactChanged; }
            set { isPrimayContactChanged = value; }
        }

        public bool IsFirstPrimaryContact
        {
            get { return isFirstPrimaryContact; }
            set { isFirstPrimaryContact = value; }
        }

        public int SelectedViewIndex
        {
            get { return selectedViewIndex; }
            set
            {
                selectedViewIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedViewIndex"));
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndex"));

            }
        }
        public string LeadsEditViewTitle
        {
            get { return leadsEditViewTitle; }
            set
            {
                leadsEditViewTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeadsEditViewTitle"));
            }
        }
        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tasks"));
            }
        }
        public Offer SelectedTask
        {
            get;
            set;
        }
        public bool IsSplitVisible
        {
            get
            {
                return isSplitVisible;
            }

            set
            {
                isSplitVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSplitVisible"));
            }
        }
        public bool ForLeadOpen
        {
            get { return forLeadOpen; }
            set { forLeadOpen = value; }
        }

        public ObservableCollection<GeosStatus> GeosEnabledStatusList
        {
            get { return geosEnabledStatusList; }
            set { geosEnabledStatusList = value; }
        }

        public bool GridRowHeight
        {
            get { return gridRowHeight; }
            set
            {
                gridRowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeight"));
            }
        }

        public bool GridRowHeightForRfq
        {
            get { return gridRowHeightForRfq; }
            set
            {
                gridRowHeightForRfq = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeightForRfq"));
            }
        }

        public bool GridRowHeightForQuoteSent
        {
            get { return gridRowHeightForQuoteSent; }
            set
            {
                gridRowHeightForQuoteSent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeightForQuoteSent"));
            }
        }

        public string Rfq
        {
            get { return rfq; }
            set
            {
                rfq = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Rfq"));
            }
        }

        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryDate"));
            }
        }

        public ObservableCollection<Activity> LeadActivityList
        {
            get { return leadActivityList; }
            set
            {
                leadActivityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeadActivityList"));
            }
        }

        public Activity SelectedActivity
        {
            get { return selectedActivity; }
            set
            {
                selectedActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivity"));
            }
        }

        public ObservableCollection<Activity> ExistingActivitiesTobeLinked
        {
            get { return existingActivitiesTobeLinked; }
            set
            {
                existingActivitiesTobeLinked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivity"));
            }
        }
        public Company SelectedPlantDetails { get; set; }
        public string ConvertedOfferAmount
        {
            get { return convertedOfferAmount; }
            set
            {
                convertedOfferAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConvertedOfferAmount"));
            }
        }

        public bool GridRowHeightForAmount
        {
            get
            {
                return gridRowHeightForAmount;
            }

            set
            {
                gridRowHeightForAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeightForAmount"));
            }
        }
        public double ConvertedAmount
        {
            get
            {
                return convertedAmount;
            }

            set
            {
                convertedAmount = Math.Round(value, 2);
                OnPropertyChanged(new PropertyChangedEventArgs("ConvertedAmount"));
            }
        }

        //[001]added
        public List<User> OfferOwnerList
        {
            get { return offerOwnerList; }
            set
            {
                offerOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferOwnerList"));
            }
        }

        public int SelectedIndexOfferOwner
        {
            get { return selectedIndexOfferOwner; }
            set
            {
                selectedIndexOfferOwner = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOfferOwner"));
            }
        }

        public List<OfferContact> OfferToList
        {
            get { return offerToList; }
            set
            {
                offerToList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferToList"));
            }
        }

        public List<Object> SelectedOfferToList
        {
            get { return selectedOfferToList; }
            set
            {
                selectedOfferToList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOfferToList"));
            }
        }

        public ObservableCollection<OfferContact> ListAddedOfferContact
        {
            get { return listAddedOfferContact; }
            set
            {

                listAddedOfferContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAddedOfferContact"));
            }
        }


        public ActiveSite OfferActiveSite
        {
            get { return offerActiveSite; }
            set
            {
                offerActiveSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferActiveSite"));
            }
        }

        #endregion

        #region public ICommand

        public ICommand SplitOtViewAcceptButtonCommand { get; set; }
        public ICommand SplitOtViewCancelButtonCommand { get; set; }
        public ICommand LeadsViewWindowCancelButtonCommand { get; set; }
        public ICommand LeadsEditViewAcceptButtonCommand { get; set; }
        public ICommand LeadsEditViewCancelButtonCommand { get; set; }
        public ICommand QuantityEditValueChangedCommand { get; set; }
        public ICommand QuantityEditValueChangedSplitCommand { get; set; }
        public ICommand CheckEditQuantityValueChangedCommand { get; set; }
        public ICommand CheckEditQuantityValueChangedSplitCommand { get; set; }
        public ICommand ShowAllCheckedCommand { get; set; }
        public ICommand ShowAllUncheckedCommand { get; set; }
        public ICommand ShowAllCheckedSplitCommand { get; set; }
        public ICommand ShowAllUncheckedSplitCommand { get; set; }
        public ICommand OfferAmountLostFocusCommand { get; set; }
        public ICommand OfferAmountSplitLostFocusCommand { get; set; }
        public ICommand CustomNodeFilterCommand { get; set; }
        public ICommand CustomNodeFilterSplitCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand LostOpportunityWindowCommand { get; set; }
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand CommentsShipmentGridDoubleClickCommand { get; set; }
        // public ICommand GetCustomerContactCommand { get; set; }
        public ICommand SetPrimaryContactCommand { get; set; }
        public ICommand SplitLeadOfferButtonCommand { get; set; }
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand CloseTaskCommand { get; set; }
        public ICommand SplitOtStatusSelectedIndexChangedCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand AddProjectButtonCommand { get; set; }
        public ICommand AddNewActivityCommand { get; set; }
        public ICommand ActivitiesGridDoubleClickCommand { get; set; }
        public ICommand ExistingActivitiesGridDoubleClickCommand { get; set; }

        //[001]added
        //public ICommand GetSalesContactCommand { get; set; }
        //public ICommand AssignedSalesCancelCommand { get; set; }
        //public ICommand SetSalesResponsibleCommand { get; set; }
        //public ICommand HyperlinkForEmail { get; set; }
        //public ICommand LinkedContactDoubleClickCommand { get; set; }

        public ICommand ExcelexportButtonCheckedCommand { get; set; }

        public ICommand EditEngineeringAnalysisCommand { get; set; }

        public ICommand IsEngineeringAnalysisCommand { get; set; }

        public ICommand RichTextResizingCommand { get; set; }

        public ICommand SelectionClearCommand { get; set; }

        public ICommand FocusedRowChangingCommand { get; set; }
        public ICommand IsEditorChangedCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
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

        #region Constructor
        public LeadsEditViewModel()
        {
            try
            {
                IsSplitVisible = true;
                screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                if (screenHeight > 1000)
                {
                    WindowHeight = 700;
                    CommentboxHeight = 490;
                }
                else
                {
                    WindowHeight = 600;
                    CommentboxHeight = 390;
                }

                GeosApplication.Instance.Logger.Log("Constructor LeadsEditViewModel ...", category: Category.Info, priority: Priority.Low);
                SplitOtViewCancelButtonCommand = new DelegateCommand<object>(CloseWindowForSplit);
                LeadsViewWindowCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                LeadsEditViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveOffer));
                QuantityEditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(QuantityEditValueChangedAction);
                QuantityEditValueChangedSplitCommand = new DelegateCommand<EditValueChangedEventArgs>(SplitQuantityEditValueChangedAction);
                CheckEditQuantityValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(QuantityEditAfterCheckChangedAction);
                CheckEditQuantityValueChangedSplitCommand = new DelegateCommand<EditValueChangedEventArgs>(SplitQuantityEditAfterCheckChangedAction);
                LeadsEditViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                OfferAmountLostFocusCommand = new DelegateCommand<object>(OfferAmountLostFocusCommandAction);
                IsShowAll = false;

                ShowAllCheckedCommand = new RelayCommand(new Action<object>(ShowAllCheckedCommandAction));
                ShowAllUncheckedCommand = new RelayCommand(new Action<object>(ShowAllUncheckedCommandAction));
                ShowAllCheckedSplitCommand = new RelayCommand(new Action<object>(ShowAllCheckedSplitCommandAction));
                ShowAllUncheckedSplitCommand = new RelayCommand(new Action<object>(ShowAllUncheckedSplitCommandAction));
                OfferAmountSplitLostFocusCommand = new DelegateCommand<object>(OfferAmountSplitLostFocusCommandAction);

                CustomNodeFilterCommand = new DelegateCommand<DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs>(CustomNodeFilterCommandAction);
                CustomNodeFilterSplitCommand = new DelegateCommand<DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs>(CustomNodeFilterSplitCommandAction);
                AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                LostOpportunityWindowCommand = new RelayCommand(new Action<object>(ShowLostOpportunityWindow));
                CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
                CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(EditCommentAction);
                CommentsShipmentGridDoubleClickCommand = new DelegateCommand<object>(EditShipmentAction);

                //GetCustomerContactCommand = new DelegateCommand<GridColumnDataEventArgs>(CutomerContactCheckedAction);
                
                //[001] Added
                //GetSalesContactCommand = new DelegateCommand<object>(GetSalesContactCommandAction);
                // AssignedSalesCancelCommand = new DelegateCommand<object>(AssignedSalesCancelCommandAction);
                // SetSalesResponsibleCommand = new DelegateCommand<object>(SetSalesResponsibleCommandAction);
                // HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                // LinkedContactDoubleClickCommand = new DelegateCommand<object>(LinkedContactDoubleClickCommandAction);

                SetPrimaryContactCommand = new DelegateCommand<object>(SetCommandAction);

              

                SplitLeadOfferButtonCommand = new RelayCommand(new Action<object>(SplitLeadOfferViewWindowShow));

                //SplitLeadOfferButtonCommand = new RelayCommand(new Action<object>(SplitOT));
                SplitOtViewAcceptButtonCommand = new DelegateCommand<object>(SaveOfferWithSplit);
                AddNewTaskCommand = new RelayCommand(new Action<object>(AddNewTaskCommandAction));
                SelectionChangedCommand = new RelayCommand(new Action<object>(SelectionChangedCommandAction));
                CloseTaskCommand = new DelegateCommand<Task>(CloseTask, CanCloseTask);

                AddProjectButtonCommand = new DelegateCommand<object>(AddNewProjectCommandAction);
                AddNewActivityCommand = new DelegateCommand<object>(AddActivityViewWindowShow);
                ActivitiesGridDoubleClickCommand = new DelegateCommand<object>(EditActivityViewWindowShow);
                ExistingActivitiesGridDoubleClickCommand = new DelegateCommand<object>(LinkExistingActivityToOffer);
                ExcelexportButtonCheckedCommand = new DelegateCommand<object>(ExporttoExcel);

                //EngAnalysis
                EditEngineeringAnalysisCommand = new DelegateCommand<object>(EditEngineeringAnalysisCommandAction);
                IsEngineeringAnalysisCommand = new RelayCommand(new Action<object>(ShowIsAnalysisWindow));
                RichTextResizingCommand = new DelegateCommand<object>(ResizeRichTextEditor);
              

                SelectionClearCommand = new DelegateCommand<object>(SelectionClearCommandAction);
                FocusedRowChangingCommand = new DelegateCommand<CanceledEventArgs>(MyTableView_FocusedRowChanging);
                IsEditorChangedCommand = new DelegateCommand<object>(IsEditorChangedAction);
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                SelectedIndexChangedCommand = new DelegateCommand<object>(new Action<object>((obj) => { SelectedIndexChangedCommandAction(obj); }));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangedEventArgs>(OnDateEditValueChangingCommandAction);

                //new DelegateCommand<Task>(CloseTaskCommandAction);
                //SplitOtStatusSelectedIndexChangedCommand = new RelayCommand(SelectedIndexChangedSplitOtStatus);
                // LeadCompanylst = CrmStartUp.GetAllCompaniesDetails(GeosApplication.Instance.ActiveUser.IdUser);

                //Parameter 1 for show warning for max ammout.
                Max_Value = CrmStartUp.GetOfferMaxValueById(1);

                //Parameter 2 for show task suggestion.
                // Max_Value_TaskSuggestion = CrmStartUp.GetOfferMaxValueById(2);

                activityTemplateTriggers = CrmStartUp.GetActivityTemplateTriggers();

                //Max_Value_TaskSuggestion = Convert.ToDouble(activityTemplateTriggers[0].ActivityTemplateTriggerCondition.ConditionFieldValue);

                //fill users current site detail.
                CurrentCompany = CrmStartUp.GetCurrentPlantId(GeosApplication.Instance.ActiveUser.IdUser);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                }

                IsActivityChange = false;
                IsCommentChange = false;
                //GridRowHeight = false;
                // LeftPanelHeight = new GridLength(180, GridUnitType.Auto);
                GeosApplication.Instance.Logger.Log("Constructor LeadsEditViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

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
                    me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexLeadSource)] + // Lead Source
                    me[BindableBase.GetPropertyName(() => OfferAmount)] +
                    me[BindableBase.GetPropertyName(() => OfferCloseDate)] +
                    me[BindableBase.GetPropertyName(() => RFQReceptionDate)] +
                    me[BindableBase.GetPropertyName(() => QuoteSentDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexBusinessUnit)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexSalesOwner)] +
                    me[BindableBase.GetPropertyName(() => ProductAndServicesCount)] +
                    me[BindableBase.GetPropertyName(() => IsSiteResponsibleRemoved)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => IsStatusDisabled)]+
                    me[BindableBase.GetPropertyName(() => SelectedIndexOfferOwner)];


                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string descriptionProp = BindableBase.GetPropertyName(() => Description);
                string selectedIndexLeadSourceProp = BindableBase.GetPropertyName(() => SelectedIndexLeadSource);   // Lead Source
                string amountProp = BindableBase.GetPropertyName(() => OfferAmount);
                string offerCloseDateProp = BindableBase.GetPropertyName(() => OfferCloseDate);
                string rfqReceptionDateProp = BindableBase.GetPropertyName(() => RFQReceptionDate);
                string quoteSentDateProp = BindableBase.GetPropertyName(() => QuoteSentDate);
                string selectedIndexCompanyGroupProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup);
                string selectedIndexCompanyPlantProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);
                string selectedIndexBusinessUnitProp = BindableBase.GetPropertyName(() => SelectedIndexBusinessUnit);
                string selectedIndexSalesOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexSalesOwner);
                string productAndServicesCountProp = BindableBase.GetPropertyName(() => ProductAndServicesCount);
                string isSiteResponsibleRemovedProp = BindableBase.GetPropertyName(() => IsSiteResponsibleRemoved);
                string isStatusDisabledProp = BindableBase.GetPropertyName(() => IsStatusDisabled);
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                string SelectedIndexOfferOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexOfferOwner);


                if (columnName == descriptionProp)
                    return RequiredValidationRule.GetErrorMessage(descriptionProp, Description);
                else if (columnName == selectedIndexLeadSourceProp) // Lead Source
                    return RequiredValidationRule.GetErrorMessage(selectedIndexLeadSourceProp, SelectedIndexLeadSource);
                else if (columnName == amountProp)
                    return RequiredValidationRule.GetErrorMessage(amountProp, OfferAmount, SelectedGeosStatus.IdOfferStatusType);
                else if (columnName == offerCloseDateProp)
                    return RequiredValidationRule.GetErrorMessage(offerCloseDateProp, OfferCloseDate, SelectedGeosStatus.IdOfferStatusType);
                else if (SelectedGeosStatus != null && columnName == rfqReceptionDateProp)
                    return RequiredValidationRule.GetErrorMessage(rfqReceptionDateProp, RFQReceptionDate, SelectedGeosStatus.IdOfferStatusType);
                else if (SelectedGeosStatus != null && columnName == quoteSentDateProp)
                    return RequiredValidationRule.GetErrorMessage(quoteSentDateProp, QuoteSentDate, SelectedGeosStatus.IdOfferStatusType);
                else if (columnName == selectedIndexCompanyGroupProp)
                    return RequiredValidationRule.GetErrorMessage(selectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);
                else if (columnName == selectedIndexCompanyPlantProp)
                    return RequiredValidationRule.GetErrorMessage(selectedIndexCompanyPlantProp, SelectedIndexCompanyPlant);
                else if (columnName == selectedIndexBusinessUnitProp)
                    return RequiredValidationRule.GetErrorMessage(selectedIndexBusinessUnitProp, SelectedIndexBusinessUnit);
                else if (columnName == selectedIndexSalesOwnerProp)
                    return RequiredValidationRule.GetErrorMessage(selectedIndexSalesOwnerProp, SelectedIndexSalesOwner);
                else if (columnName == productAndServicesCountProp)
                    return RequiredValidationRule.GetErrorMessage(productAndServicesCountProp, ProductAndServicesCount);
                else if (columnName == isSiteResponsibleRemovedProp)
                {
                    if (CompanyPlantList != null && SalesOwnerList != null)
                    {
                        if (SelectedLeadList[0].Site.IdSalesResponsible != SelectedLeadList[0].IdSalesOwner && SelectedLeadList[0].Site.IdSalesResponsibleAssemblyBU != SelectedLeadList[0].IdSalesOwner)
                            return RequiredValidationRule.GetErrorMessage(isSiteResponsibleRemovedProp, IsSiteResponsibleRemoved);
                    }

                }

                //[001] added
                else if (columnName == isStatusDisabledProp)
                {
                    if (!ShowStatusList.IsGoAheadProduction)
                        return RequiredValidationRule.GetErrorMessage(isStatusDisabledProp, IsStatusDisabled);
                }
                else if (columnName == informationError)
                    return RequiredValidationRule.GetErrorMessage(informationError, InformationError);
                //[002]added
                else if (columnName == SelectedIndexOfferOwnerProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexOfferOwnerProp, SelectedIndexOfferOwner);

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for changing text on radio button selection
        /// </summary>
        /// <param name="obj"></param>
        public void IsEditorChangedAction(object obj)
        {
            var document = ((RichTextBox)obj).Document;
            LeadComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
            string s = string.Empty;
            if (!string.IsNullOrEmpty(LeadComment.Trim()))
            {
                if (IsRtf)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Rtf);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            s = sr.ReadToEnd();
                        }
                    }
                }
                else if (IsNormal)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Text);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            s = sr.ReadToEnd();
                        }
                    }
                }
            }
            LeadComment = s;

        }
        /// <summary>
        /// Method for clear button under category lookupEdit
        /// </summary>
        /// <param name="obj"></param>
        public void SelectionClearCommandAction(object obj)
        {
            SelectedCategory = null;
        }
        public void FillAllProductCategory()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllProductCategory...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.IsPermissionAuditor)
                    IsCategoryControlEnable = false;
                else
                    IsCategoryControlEnable = true;

                ListProductCategory = new ObservableCollection<ProductCategory>(CrmStartUp.GetAllCategory());
                List<ProductCategory> ListParent = new List<ProductCategory>();
                ListParent = ListProductCategory.Where(x => x.IdParent == 0).ToList();
                foreach (ProductCategory item in ListProductCategory)
                {
                    if (item.IdParent == 0)
                        item.MergedCategoryAndProduct = item.Name;
                    else
                    {

                        List<ProductCategory> parent = new List<Data.Common.ProductCategory>(ListParent.Where(i => i.IdProductCategory == item.IdParent));
                        item.MergedCategoryAndProduct = parent[0].Name + " -> " + item.Name;
                        parent[0].IsDisabled = true;
                    }

                    if (item.IdProductCategory == selectedLeadList[0].IdProductCategory)
                        SelectedCategory = item;

                }

                GeosApplication.Instance.Logger.Log("Method FillAllProductCategory...executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAllProductCategory() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        /// <summary>
        /// Method to open contact on double click
        /// </summary>
        /// <param name="obj"></param>
        //public void LinkedContactDoubleClickCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method EditContactAction...", category: Category.Info, priority: Priority.Low);
        //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }
        //        People ppl = obj as People;
        //        People peopleData = CrmStartUp.GetContactsByIdPerson(ppl.IdPerson);

        //        EditContactViewModel editContactViewModel = new EditContactViewModel();
        //        EditContactView editContactView = new EditContactView();
        //        editContactViewModel.InIt(peopleData);
        //        EventHandler handle = delegate { editContactView.Close(); };
        //        editContactViewModel.RequestClose += handle;
        //        editContactView.DataContext = editContactViewModel;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        editContactView.ShowDialogWindow();
        //        if (editContactViewModel.IsSave && editContactViewModel.SelectedContact[0] != null)
        //        {

        //            ppl.Name = editContactViewModel.SelectedContact[0].Name;
        //            ppl.Surname = editContactViewModel.SelectedContact[0].Surname;
        //            ppl.FullName = editContactViewModel.SelectedContact[0].FullName;
        //            ppl.JobTitle = editContactViewModel.SelectedContact[0].JobTitle;
        //            ppl.Phone = editContactViewModel.SelectedContact[0].Phone;
        //            ppl.Email = editContactViewModel.SelectedContact[0].Email;
        //            ppl.OwnerImage = GetContactImage(editContactViewModel.SelectedContact[0]);
        //            ListAddedContact = new ObservableCollection<People>(ListAddedContact);
        //        }
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Method EditContactAction...executed successfully", category: Category.Info, priority: Priority.Low);
        //    }

        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in EditContactAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// Method for getting ContactImage
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ImageSource GetContactImage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetContactImage ...", category: Category.Info, priority: Priority.Low);

                People people = obj as People;
                if (!string.IsNullOrEmpty(people.ImageText))
                {
                    byte[] imageBytes = Convert.FromBase64String(people.ImageText);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    people.OwnerImage = byteArrayToImage(imageBytes);
                }
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (people != null && people.IdPersonGender == 1)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        else if (people != null && people.IdPersonGender == 2)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        //else if (people != null && people.IdPersonGender == null)
                        //    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                    }
                    else
                    {
                        if (people != null && people.IdPersonGender == 1)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (people != null && people.IdPersonGender == 2)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        //else if (people != null && people.IdPersonGender == null)
                        //    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                    }
                }

                GeosApplication.Instance.Logger.Log("Method GetContactImage() executed successfully", category: Category.Info, priority: Priority.Low);
                return people.OwnerImage;

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetContactImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return null;
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetContactImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                return null;
            }

        }
        /// <summary>
        /// Method For Resizing Rich Text Box on Load and Content Changed
        /// </summary>
        /// <param name="obj"></param>
        public void ResizeRichTextEditor(object obj)
        {

            RichEditControl edit = (RichEditControl)obj;
            Document currentDocument = edit.Document;
            DocumentLayout currentDocumentLayout = edit.DocumentLayout;

            edit.BeginInvoke(() =>
            {
                SubDocument subDocument = currentDocument.CaretPosition.BeginUpdateDocument();
                DocumentPosition docPosition = subDocument.CreatePosition(((currentDocument.CaretPosition.ToInt() == 0) ? 0 : currentDocument.CaretPosition.ToInt() - 1));

                double height = 0;
                System.Drawing.Point pos = PageLayoutHelper.GetInformationAboutCurrentPage(currentDocumentLayout, docPosition);
                height = DevExpress.Office.Utils.Units.TwipsToPixels(pos, edit.DpiX, edit.DpiY).Y;

                edit.Height = height + 10;
                edit.VerticalScrollValue = 0;
                currentDocument.CaretPosition.EndUpdateDocument(subDocument);

            });

        }
        /// <summary>
        /// This method show Engineering Analysis Window and that raises click on toggle switch.
        /// </summary>
        private void ShowIsAnalysisWindow(object obj)
        {
            try
            {
                if (IsEngAnalysis)
                {
                    //IsEngAnalysis = true;
                    if (ExistedEngineeringAnalysis == null) // || ExistedEngineeringAnalysis.Attachments == null || ExistedEngineeringAnalysis.Attachments.Count <= 0)
                    {
                        GeosApplication.Instance.Logger.Log("Method LeadsNewViewWindowShow...", category: Category.Info, priority: Priority.Low);
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

                        AddEngineeringAnalysisViewModel addEngineeringAnalysisViewModel = new AddEngineeringAnalysisViewModel();
                        AddEngineeringAnalysisView addEngineeringAnalysisView = new AddEngineeringAnalysisView();
                        EventHandler handle = delegate { addEngineeringAnalysisView.Close(); };
                        addEngineeringAnalysisViewModel.RequestClose += handle;
                        addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        addEngineeringAnalysisView.ShowDialogWindow();

                        if (addEngineeringAnalysisViewModel.IsSave)
                        {
                            ExistedEngineeringAnalysis = addEngineeringAnalysisViewModel.EngAnalysis;
                            ExistedEngineeringAnalysisDuplicate = addEngineeringAnalysisViewModel.EngAnalysisDuplicate;
                            EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;

                            if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                            {
                                OptionsByOffer optionsByOffer = new OptionsByOffer();
                                optionsByOffer.IdOption = 25;
                                optionsByOffer.OfferOption = MainOfferOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                                optionsByOffer.IsSelected = true;
                                optionsByOffer.Quantity = 1;
                                OptionsByOfferList.Add(optionsByOffer);
                            }
                            //IsEngAnalysisButtonVisible = Visibility.Visible;
                        }
                        else
                        {
                            //IsEngAnalysisButtonVisible = Visibility.Collapsed;
                            IsEngAnalysis = false;
                        }
                    }
                    else
                    {
                        if (!IsInIt)
                        {
                            if (ExistedEngineeringAnalysis != null)
                            {
                                IsExistEngAnalysis = true;
                                IsEngAnalysisEnable = true;

                                if (SelectedLeadList[0].EngineeringAnalysis != null)
                                {
                                    ExistedEngineeringAnalysis = (EngineeringAnalysis)SelectedLeadList[0].EngineeringAnalysis.Clone();
                                    ExistedEngineeringAnalysisDuplicate = (EngineeringAnalysis)SelectedLeadList[0].EngineeringAnalysis.Clone();

                                    if (OptionsByOfferList != null && OptionsByOfferList.Count > 0)
                                    {
                                        if (OptionsByOfferList.Any(ps => ps.IdOption == 25))
                                        {
                                            OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                                            opt.IsSelected = true;
                                            opt.Quantity = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                AddEngineeringAnalysisViewModel addEngineeringAnalysisViewModel = new AddEngineeringAnalysisViewModel();
                                AddEngineeringAnalysisView addEngineeringAnalysisView = new AddEngineeringAnalysisView();
                                EventHandler handle = delegate { addEngineeringAnalysisView.Close(); };
                                addEngineeringAnalysisViewModel.RequestClose += handle;
                                addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                addEngineeringAnalysisView.ShowDialogWindow();

                                if (addEngineeringAnalysisViewModel.IsSave)
                                {
                                    ExistedEngineeringAnalysis = addEngineeringAnalysisViewModel.EngAnalysis;
                                    ExistedEngineeringAnalysisDuplicate = addEngineeringAnalysisViewModel.EngAnalysisDuplicate;
                                    EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;

                                    if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                                    {
                                        OptionsByOffer optionsByOffer = new OptionsByOffer();
                                        optionsByOffer.IdOption = 25;
                                        optionsByOffer.OfferOption = MainOfferOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                                        optionsByOffer.IsSelected = true;
                                        optionsByOffer.Quantity = 1;
                                        OptionsByOfferList.Add(optionsByOffer);
                                    }
                                    else
                                    {
                                        OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                                        if (opt.IsSelected == false)
                                        {
                                            opt.IsSelected = true;
                                            opt.Quantity = 1;
                                        }
                                    }

                                    IsEngAnalysis = false;
                                }
                                else
                                {
                                    IsEngAnalysis = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ExistedEngineeringAnalysis != null)
                    {
                        if (IsEngAnalysis != false)
                        {
                            ExistedEngineeringAnalysis = SelectedLeadList[0].EngineeringAnalysis;
                            ExistedEngineeringAnalysisDuplicate = SelectedLeadList[0].EngineeringAnalysis;
                        }
                        else
                        {
                            ExistedEngineeringAnalysis = new EngineeringAnalysis();
                            ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();
                            EngAnalysisAttachmentFileUploadIndicator = new FileUploader();

                            if (OptionsByOfferList != null && OptionsByOfferList.Count > 0)
                            {
                                if (OptionsByOfferList.Any(ps => ps.IdOption == 25))
                                {
                                    OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                                    opt.IsSelected = false;
                                    opt.Quantity = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        ExistedEngineeringAnalysis = new EngineeringAnalysis();
                        ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();
                        EngAnalysisAttachmentFileUploadIndicator = new FileUploader();

                        if (OptionsByOfferList != null && OptionsByOfferList.Count > 0)
                        {
                            if (OptionsByOfferList.Any(ps => ps.IdOption == 25))
                            {
                                OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                                opt.IsSelected = false;
                                opt.Quantity = 0;
                            }
                        }
                        //IsEngAnalysisButtonVisible = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ShowIsAnalysisWindow() method - {0}" + ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Edit Engineering Analysis
        /// </summary>
        /// <param name="obj"></param>
        public void EditEngineeringAnalysisCommandAction(object obj)
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

            AddEngineeringAnalysisViewModel addEngineeringAnalysisViewModel = new AddEngineeringAnalysisViewModel();
            AddEngineeringAnalysisView addEngineeringAnalysisView = new AddEngineeringAnalysisView();
            EventHandler handle = delegate { addEngineeringAnalysisView.Close(); };
            addEngineeringAnalysisViewModel.RequestClose += handle;

            addEngineeringAnalysisViewModel.EngAnalysis = (EngineeringAnalysis)ExistedEngineeringAnalysis.Clone();
            addEngineeringAnalysisViewModel.EngAnalysisDuplicate = (EngineeringAnalysis)ExistedEngineeringAnalysisDuplicate.Clone();

            addEngineeringAnalysisViewModel.InIt(SelectedGeosStatus);

            addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            addEngineeringAnalysisView.ShowDialogWindow();

            if (addEngineeringAnalysisViewModel.IsSave)
            {
                ExistedEngineeringAnalysis = (EngineeringAnalysis)addEngineeringAnalysisViewModel.EngAnalysis.Clone();
                ExistedEngineeringAnalysisDuplicate = (EngineeringAnalysis)addEngineeringAnalysisViewModel.EngAnalysisDuplicate.Clone();

                EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;

                if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                {
                    OptionsByOffer optionsByOffer = new OptionsByOffer();
                    optionsByOffer.IdOption = 25;
                    optionsByOffer.OfferOption = MainOfferOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                    optionsByOffer.IsSelected = true;
                    optionsByOffer.Quantity = 1;
                    OptionsByOfferList.Add(optionsByOffer);
                }
            }

        }

        /// <summary>
        /// Method for add new activity.
        /// [001][cpatil][GEOS2-1977] The code added in the offer code must be taken from the application selected site
        /// [001][cpatil][GEOS2-2074] CRM - OPPORTUNITIES - Timeline
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();

                List<Activity> _ActivityList = new List<Activity>();
                AddActivityView addActivityView = new AddActivityView();
                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                // [001] Added 
                ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                //**[Start] code for add Account Detail.

                Activity _Activity = new Activity();
                _Activity.ActivityLinkedItem = new List<ActivityLinkedItem>();

                //Fill Account details - 42.
                ActivityLinkedItem _ActivityLinkedItem = new ActivityLinkedItem();
                _ActivityLinkedItem.IdLinkedItemType = 42;
                _ActivityLinkedItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                _ActivityLinkedItem.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                _ActivityLinkedItem.IsVisible = false;
                _Activity.ActivityLinkedItem.Add(_ActivityLinkedItem);

                _ActivityLinkedItem.Company = new Company();
                _ActivityLinkedItem.Company.Customers = new List<Customer>();
                _ActivityLinkedItem.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                _ActivityLinkedItem.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);

                _ActivityLinkedItem.LinkedItemType = new LookupValue();
                _ActivityLinkedItem.LinkedItemType.IdLookupValue = 42;
                _ActivityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                //Fill Opportunity details.
                ActivityLinkedItem _ActivityLinkedItem1 = new ActivityLinkedItem();
                _ActivityLinkedItem1.IdLinkedItemType = 44;
                _ActivityLinkedItem1.Name = offerCode;
                _ActivityLinkedItem1.IdSite = null;
                _ActivityLinkedItem1.IdOffer = SelectedLeadList[0].IdOffer;
                _ActivityLinkedItem1.IsVisible = false;
                _ActivityLinkedItem1.IdEmdepSite = Convert.ToInt32(SelectedLeadList[0].OfferActiveSite.IdSite);
                _Activity.ActivityLinkedItem.Add(_ActivityLinkedItem1);

                //_ActivityLinkedItem1 = (ActivityLinkedItem)_ActivityLinkedItem.Clone();
                _ActivityLinkedItem1.LinkedItemType = new LookupValue();
                _ActivityLinkedItem1.LinkedItemType.IdLookupValue = 44;
                _ActivityLinkedItem1.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();

                _Activity.Location = CompanyPlantList[SelectedIndexCompanyPlant].Address;
                _Activity.Longitude = CompanyPlantList[SelectedIndexCompanyPlant].Longitude;
                _Activity.Latitude = CompanyPlantList[SelectedIndexCompanyPlant].Latitude;

                _ActivityList.Add(_Activity);
                addActivityViewModel.IsInternalEnable = false;
                addActivityViewModel.IsAddedFromOutSide = true;
                addActivityViewModel.SelectedIndexCompanyGroup = SelectedIndexCompanyGroup;
                addActivityViewModel.SelectedIndexCompanyPlant = addActivityViewModel.CompanyPlantList.IndexOf(addActivityViewModel.CompanyPlantList.FirstOrDefault(x => x.IdCompany == CompanyPlantList[SelectedIndexCompanyPlant].IdCompany));

                addActivityViewModel.Init(_ActivityList);


                if (IsActivityCreateFromSaveOffer)
                {
                    ActivityTemplate activityTemplate = (ActivityTemplate)obj;
                    addActivityViewModel.SelectedIndexType = addActivityViewModel.TypeList.IndexOf(tl => tl.IdLookupValue == activityTemplate.IdActivityType);
                    addActivityViewModel.Subject = activityTemplate.Subject;                // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                    addActivityViewModel.Description = activityTemplate.Description;        // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                    addActivityViewModel.DueDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                }

                //**[End] code for add Account Detail.

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

                if (addActivityViewModel.IsActivitySave)
                {
                    LogEntryByOffer logEntry = new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddActivity").ToString(), addActivityViewModel.SelectedActivityType.Value, addActivityViewModel.Subject),
                        IdLogEntryType = 18
                    };
                    // [001] Changed service method name and controller
                    CrmStartUpOfferActiveSite.AddLogEntryByOffer_V2040(logEntry);
                    // [001] Changed service method name and controller
                    ListChangeLog = new ObservableCollection<LogEntryByOffer>(CrmStartUpOfferActiveSite.GetAllLogEntriesByIdOffer_V2040(SelectedLeadList[0].IdOffer).AsEnumerable());
                    foreach (Activity newActivity in addActivityViewModel.NewCreatedActivityList)
                    {
                        if (newActivity.IsCompleted == 1)
                        {
                            newActivity.ActivityGridStatus = "Completed";
                            newActivity.CloseDate = GeosApplication.Instance.ServerDateTime;
                        }
                        else
                        {
                            newActivity.ActivityGridStatus = newActivity.ActivityStatus != null ? newActivity.ActivityStatus.Value : "";
                            newActivity.CloseDate = null;
                            newActivity.ActivityLinkedItem = new List<ActivityLinkedItem>();
                            newActivity.ActivityLinkedItem.Add(new ActivityLinkedItem { CreationDate = DateTime.Now });
                        }
                        LeadActivityList.Add(newActivity);

                    }
                    LeadActivityList = new ObservableCollection<Activity>(LeadActivityList);
                    SelectedActivity = LeadActivityList.Last();
                    LeadsSleepDays = 0;
                    IsActivityChange = true;
                }

                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method for Edit Activity View
        /// </summary>
        /// <param name="obj"></param>
        private void EditActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                Activity activity = ((Activity)obj);

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

                Activity tempActivity = new Activity();

                tempActivity = CrmStartUp.GetActivityByIdActivity_V2035(activity.IdActivity);
                EditActivityViewModel editActivityViewModel = new EditActivityViewModel();
                EditActivityView editActivityView = new EditActivityView();
                editActivityViewModel.IsEditedFromOutSide = true;
                foreach (var item in tempActivity.ActivityLinkedItem)
                {
                    if (item.IdLinkedItemType == 42 || item.IdLinkedItemType == 44)
                        item.IsVisible = false;
                }
                editActivityViewModel.IsInternalEnable = false;
                editActivityViewModel.Init(tempActivity);

                EventHandler handle = delegate { editActivityView.Close(); };
                editActivityViewModel.RequestClose += handle;
                editActivityView.DataContext = editActivityViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                editActivityView.ShowDialogWindow();

                if (editActivityViewModel.objActivity != null)
                {
                    activity.Subject = editActivityViewModel.objActivity.Subject;
                    activity.Description = editActivityViewModel.objActivity.Description;
                    activity.LookupValue = editActivityViewModel.objActivity.LookupValue;
                    //activity.ToDate = editActivityViewModel.objActivity.ToDate;
                    //activity.FromDate = editActivityViewModel.objActivity.FromDate;
                    activity.IsCompleted = editActivityViewModel.objActivity.IsCompleted;
                    //activity.ActivityLinkedItem[0] = editActivityViewModel.objActivity.ActivityLinkedItem[0].Customer;
                    activity.People = editActivityViewModel.objActivity.People;
                    activity.ActivityLinkedItem = editActivityViewModel.objActivity.ActivityLinkedItem;

                    if (activity.ActivityLinkedItem != null)
                    {
                        foreach (ActivityLinkedItem item in activity.ActivityLinkedItem)
                        {
                            item.ActivityLinkedItemImage = null;
                        }
                    }

                    IsActivityChange = true;
                    if (activity.IsCompleted == 1)
                    {
                        activity.ActivityGridStatus = "Completed";
                        activity.CloseDate = GeosApplication.Instance.ServerDateTime;
                    }
                    else
                    {
                        activity.ActivityGridStatus = editActivityViewModel.objActivity.ActivityStatus != null ? editActivityViewModel.objActivity.ActivityStatus.Value : "";
                        activity.CloseDate = null;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Activity View
        /// </summary>
        /// <param name="obj"></param>
        private void LinkExistingActivityToOffer(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IsPermissionReadOnly) return;
                if (obj == null) return;

                Activity activity = obj as Activity;

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

                activity.ActivityLinkedItem = new List<ActivityLinkedItem>();
                activity.ActivityLinkedItem.Add(new ActivityLinkedItem { CreationDate = DateTime.Now });
                LeadActivityList.Add(activity);
                SelectedActivity = activity;
                LeadsSleepDays = 0;
                if (SelectedLeadList[0].NewlyLinkedActivities == null)
                    SelectedLeadList[0].NewlyLinkedActivities = new List<Activity>();

                SelectedLeadList[0].NewlyLinkedActivities.Add(activity);
                IsActivityChange = true;
                ExistingActivitiesTobeLinked.Remove(activity);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectionChangedCommandAction(object obj)
        {
            //throw new NotImplementedException();
        }

        private void AddNewTaskCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddNewTaskCommandAction ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            if (SelectedLeadList[0].DeliveryDate == null)
            {
                SelectedLeadList[0].DeliveryDate = DateTime.MinValue;
            }
            SplitOtView SplitOtView = new SplitOtView();
            // SelectedIndex = Tasks.Count;
            SelectedIndex = Tasks.Count;
            foreach (DataRow row in Dttable.Rows)
            {
                row["SplitTabIndex"] = SelectedIndex;
            }

            Tasks.Add(new Task()
            {
                CurrentCompanyTask = CurrentCompany,
                isOldOTSplit = isOldOT,
                GeosStatusListTask = GeosStatusListSplit,
                TaskOffer = SelectedLeadList[0],
                SelectedIndexTab = SelectedIndex,
                IsTabIndexZero = false,
                IsComplete = true,
                OfferTypeListTask = OfferTypeList,
                IdOfferType = SelectedLeadList[0].IdOfferType,
                OfferAmountSplit = SelectedLeadList[0].OfferValue,
                OfferCloseDateSplit = SelectedLeadList[0].DeliveryDate.Value,
                OfferCloseDateSplitMinValue = GeosApplication.Instance.ServerDateTime.Date,
                QuoteSentDateSplit = SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue ? DateTime.Parse(SelectedLeadList[0].SendIn.ToString()) : (DateTime?)null,
                RFQReceptionDateSplit = SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue ? DateTime.Parse(SelectedLeadList[0].RFQReception.ToString()) : (DateTime?)null,
                IsShowAllSplit = false,
                DttableSplittemp = Dttable.Copy(),
                DttableSplit = Dttable.Copy(),
                OptionsByOfferListSplit = new List<OptionsByOffer>(), //SelectedLeadList[0].OptionsByOffers.ToList()),
                ProductAndServicesSplitCount = SelectedLeadList[0].OptionsByOffers.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList().Count(),
                SelectedIndexCurrencyTask = Currencies.FindIndex(crr => crr.IdCurrency == SelectedLeadList[0].IdOfferCurrency),
                SelectedGeosStatusTask = GeosStatusListSplit.IndexOf(GeosStatusListSplit.FirstOrDefault(geosS => geosS.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType)),
                HoverBackgroundTask = Colors.Red,
                SelectedBackgroundTask = Colors.Transparent,
                SelectedIndexConfidentialLevelTask = SelectedLeadList[0].ProbabilityOfSuccess,
                IdSourceOffer = selectedLeadList[0].IdOffer,
            });

            IsBusy = false;


            GeosApplication.Instance.Logger.Log("Method AddNewTaskCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            // SelectedIndex = Tasks.Count-1;
        }

        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// [002][GEOS2-2217][cpatil][03-04-2020]Eng. Analysis type field not working as expected
        public void SaveOfferWithSplit(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SaveOfferWithSplit ...", category: Category.Info, priority: Priority.Low);
            IsBusy = true;
            try
            {
                //string error = null;
                bool IsResult = false;
                foreach (var item in Tasks)
                {
                    IsBusy = true;
                    if (item.DttableSplit.AsEnumerable().Any(row => row["IsChecked"] != DBNull.Value && Convert.ToBoolean(row["IsChecked"])))
                    {
                        if (item.DttableSplit.AsEnumerable().Any(row => row["IsChecked"] != DBNull.Value
                                                                    && Convert.ToBoolean(row["IsChecked"])
                                                                    && Convert.ToDouble(row["Qty"]) == 0))
                        {
                            item.ProductAndServicesSplitCount = 0;
                        }
                        else
                        {
                            item.ProductAndServicesSplitCount = 1;
                        }
                    }
                    else
                    {
                        item.ProductAndServicesSplitCount = 0;
                    }

                    string error = item.CheckValidation();
                    if (error != null)
                    {
                        IsResult = true;
                        IsBusy = false;
                        return;
                    }
                }

                if (!IsResult)
                {
                    IsBusy = true;
                    long _MainIdOffer = 0;
                    ActiveSite _OfferActiveSite = null;
                    string _savedOfferTypeName = string.Empty;
                    string _MainOfferCode = string.Empty;
                    OfferDataLst = new List<Offer>();

                    if (Tasks.Count > 0)
                    {
                        _MainIdOffer = Tasks[0].TaskOffer.IdOffer;
                        _OfferActiveSite = Tasks[0].TaskOffer.OfferActiveSite;
                        _MainOfferCode = Tasks[0].TaskOffer.Code;
                    }

                    if (Tasks != null)
                    {
                        int taskTabIndex = 0;
                        foreach (var item in Tasks)
                        {
                            IsBusy = true;
                            if (taskTabIndex == 0)
                            {
                                bool isofferSplitsave = false;
                                taskTabIndex++;

                                try
                                {
                                    foreach (DataRow row in item.DttableSplit.Rows)
                                    {
                                        if (row != null && row["IsChecked"] != DBNull.Value && Convert.ToBoolean(row["IsChecked"]))
                                        {
                                            OptionsByOffer option = new OptionsByOffer();
                                            option.IdOption = Convert.ToInt64(row["idOfferOption"]);
                                            option.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == option.IdOption);
                                            option.Quantity = Convert.ToInt32(row["Qty"]);
                                            option.IsSelected = true;
                                            item.OptionsByOfferListSplit.Add(option);
                                        }
                                    }

                                    try
                                    {
                                        if (item.SelectedIndexOfferTypeTask > -1)
                                        {
                                            if (item.SelectedIndexOfferTypeTask == 0)
                                            {
                                                item.OfferNumberSplit = CrmStartUp.GetNextNumberOfSuppliesFromGCM(OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType);
                                            }
                                            else
                                            {
                                                //[001] Added
                                                item.OfferNumberSplit = CrmStartUp.GetNextNumberOfOfferFromCounters_V2040(OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType, GeosApplication.Instance.ActiveUser.IdUser);
                                            }
                                            if (SelectedLeadList[0].Code != item.TaskOfferCode)
                                            {
                                                //[001] Added
                                                item.TaskOfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType, Convert.ToInt32(item.TaskOffer.OfferActiveSite.IdSite), GeosApplication.Instance.ActiveUser.IdUser);

                                            }
                                        }
                                    }
                                    catch (FaultException<ServiceException> ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                    catch (ServiceUnexceptedException ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                                    }

                                    OfferData = new Offer();

                                    OfferData = (Offer)item.TaskOffer.Clone();
                                    OfferData.Code = item.TaskOfferCode;
                                    OfferData.Number = item.OfferNumberSplit;
                                    OfferData.IdOfferType = OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType;
                                    OfferData.IdStatus = GeosStatusList[item.SelectedGeosStatusTask].IdOfferStatusType;//SelectedGeosStatus.IdOfferStatusType; //GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
                                    OfferData.GeosStatus = GeosStatusList[item.SelectedGeosStatusTask];//SelectedGeosStatus; //GeosStatusList[SelectedIndexStatus];
                                    OfferData.Value = item.OfferAmountSplit;
                                    if (GridRowHeightForAmount)
                                        OfferData.OfferValue = item.ConvertedAmountSplit;
                                    else
                                        OfferData.OfferValue = item.OfferAmountSplit;
                                    OfferData.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    OfferData.IdSourceOffer = item.IdSourceOffer;

                                    OfferData.IdCurrency = Currencies[item.SelectedIndexCurrencyTask].IdCurrency;
                                    OfferData.Currency = new Currency { IdCurrency = Currencies[item.SelectedIndexCurrencyTask].IdCurrency, Name = Currencies[item.SelectedIndexCurrencyTask].Name };
                                    // if code is different then update it.
                                    if (!item.TaskOfferCode.Equals(item.TaskOfferCodeold))
                                    {
                                        OfferData.IsUpdateLeadToOT = true;
                                    }

                                    OfferData.DeliveryDate = item.OfferCloseDateSplit;
                                    OfferData.RFQReception = item.RFQReceptionDateSplit != null ? (DateTime)item.RFQReceptionDateSplit : DateTime.MinValue;
                                    OfferData.SendIn = item.QuoteSentDateSplit != null ? (DateTime)item.QuoteSentDateSplit : DateTime.MinValue;
                                    OfferData.ProbabilityOfSuccess = Convert.ToSByte(item.SelectedIndexConfidentialLevelTask.ToString());
                                    OfferData.OfferExpectedDate = item.OfferCloseDateSplit;
                                    OfferData.Comments = "";

                                    if (string.IsNullOrEmpty(Rfq.Trim()))
                                    {
                                        OfferData.Rfq = "";
                                    }
                                    else
                                    {
                                        OfferData.Rfq = Rfq.Trim();
                                    }

                                    OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;

                                    if (SelectedLeadList[0].Code != item.TaskOfferCode)
                                    {
                                        OfferData.OfferContacts = ListOfferContact.ToList();
                                    }
                                    else
                                    {
                                        OfferData.OfferContacts = null;
                                    }

                                    OfferData.Site.FullName = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;

                                    if (OfferData.OfferContacts != null)
                                    {
                                        foreach (var itemListOfferContact in OfferData.OfferContacts)
                                        {
                                            itemListOfferContact.People.OwnerImage = null;
                                        }
                                    }

                                    //Company _Company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == item.TaskOffer.Site.ConnectPlantId.ToString()).FirstOrDefault();
                                    //OfferData.Site.ConnectPlantId = _Company.ConnectPlantId;
                                    //OfferData.Site.ConnectPlantConstr = _Company.ConnectPlantConstr;

                                    OfferData.Site.SiteNameWithoutCountry = OfferData.Site.Name;
                                    OfferData.OfferActiveSite = item.TaskOffer.OfferActiveSite;
                                    //Added for CreateIssue in jira
                                    OfferData.OfferType = OfferTypeList[SelectedIndexOfferType];
                                    offerData.JiraUserReporter = GeosApplication.Instance.ActiveUser;
                                    OfferData.Site.ShortName = item.TaskOffer.OfferActiveSite.SiteAlias;

                                    List<LogEntryByOffer> logEntryByOffers = new List<LogEntryByOffer>();


                                    if (SelectedLeadList[0].OfferValue != item.OfferAmountSplit || SelectedLeadList[0].IdOfferCurrency != Currencies[item.SelectedIndexCurrencyTask].IdCurrency)
                                    {
                                        Currency _SelectedCurrency = Currencies[item.SelectedIndexCurrencyTask];
                                        Currency _Currency = Currencies.FirstOrDefault(x => x.IdCurrency == item.TaskOffer.IdOfferCurrency);
                                        if (_Currency != null)
                                            logEntryByOffers.Add(new LogEntryByOffer() { IdOffer = item.TaskOffer.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewAmountChanged").ToString(), SelectedLeadList[0].OfferValue + " " + _Currency.Name, item.OfferAmountSplit + " " + _SelectedCurrency.Name), IdLogEntryType = 8 });
                                    }


                                    OfferData.LogEntryByOffers = logEntryByOffers;

                                    OfferData.LostReasonsByOffer = new LostReasonsByOffer();
                                    //Offer offerReturnValue = null;

                                    OfferData.OptionsByOffers = item.OptionsByOfferListSplit;

                                    try
                                    {
                                        //[001] Added
                                        ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(OfferData.OfferActiveSite.SiteServiceProvider);
                                        //[001] Changed service method and controller
                                        //[002] Changed controller and service method UpdateOffer_V2040 to UpdateOffer_V2041
                                        Offer offerUpdated = CrmStartUpOfferActiveSite.UpdateOffer_V2041(OfferData, OfferData.OfferActiveSite.IdSite, GeosApplication.Instance.ActiveUser.IdUser);
                                        isofferSplitsave = offerUpdated.IsUpdated;
                                        if (isofferSplitsave == true && OfferData.IdOfferType == 1 && OfferData.IsUpdateLeadToOT == true)
                                        {
                                            EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OfferData.OfferActiveSite.IdSite));
                                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                                            ICrmService CrmStartUpCreateFolder = new CrmServiceController(serviceurl);
                                            if (OfferData.IsUpdateLeadToOT)
                                            {
                                                offerData.Year = GeosApplication.Instance.ServerDateTime.Year;
                                            }
                                            bool isCreated = CrmStartUpCreateFolder.CreateFolderOffer(OfferData, true);
                                        }

                                        if (!string.IsNullOrEmpty(offerUpdated.ErrorFromJira))
                                        {
                                            GeosApplication.Instance.Logger.Log(offerUpdated.ErrorFromJira, Category.Exception, Priority.Low);
                                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("JiraNotWorking").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        }

                                        if (isofferSplitsave)
                                        {
                                            if (OfferData != null)
                                                OfferDataLst.Add(OfferData);

                                            OfferData = null;
                                        }

                                        //IsBusy = false;
                                    }
                                    catch (FaultException<ServiceException> ex)
                                    {
                                        IsBusy = false;
                                        OfferData = null;
                                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                        {
                                            DXSplashScreen.Close();
                                        }
                                        GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                    catch (ServiceUnexceptedException ex)
                                    {
                                        IsBusy = false;
                                        OfferData = null;
                                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                        {
                                            DXSplashScreen.Close();
                                        }
                                        GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);

                                        //CustomMessageBox.Show((ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }

                                    GeosApplication.Instance.Logger.Log("Method SaveOfferWithSplit() executed successfully", category: Category.Info, priority: Priority.Low);

                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }


                                continue;
                            }

                            //bool isoffersave = false;
                            try
                            {
                                foreach (DataRow row in item.DttableSplit.Rows)
                                {
                                    if (row != null && row["IsChecked"] != DBNull.Value && Convert.ToBoolean(row["IsChecked"]))
                                    {
                                        OptionsByOffer option = new OptionsByOffer();
                                        option.IdOption = Convert.ToInt64(row["idOfferOption"]);
                                        option.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == option.IdOption);
                                        option.Quantity = Convert.ToInt32(row["Qty"]);
                                        option.IsSelected = true;
                                        item.OptionsByOfferListSplit.Add(option);
                                    }
                                }

                                try
                                {
                                    if (item.SelectedIndexOfferTypeTask > -1)
                                    {
                                        if (item.SelectedIndexOfferTypeTask == 0)
                                        {
                                            item.OfferNumberSplit = CrmStartUp.GetNextNumberOfSuppliesFromGCM(OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType);
                                        }
                                        else
                                        {
                                            //[001] Added
                                            item.OfferNumberSplit = CrmStartUp.GetNextNumberOfOfferFromCounters_V2040(OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType, GeosApplication.Instance.ActiveUser.IdUser);
                                        }

                                        //[001] Added
                                        item.TaskOfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType, OfferActiveSite.IdSite, GeosApplication.Instance.ActiveUser.IdUser);

                                    }
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                                }

                                OfferData = new Offer();
                                OfferData = (Offer)item.TaskOffer.Clone();
                                OfferData.Code = item.TaskOfferCode;
                                OfferData.Number = item.OfferNumberSplit;
                                OfferData.IdOfferType = OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType;
                                OfferData.IdStatus = GeosStatusList[item.SelectedGeosStatusTask].IdOfferStatusType;//SelectedGeosStatus.IdOfferStatusType; //GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
                                OfferData.GeosStatus = GeosStatusList[item.SelectedGeosStatusTask];//SelectedGeosStatus; //GeosStatusList[SelectedIndexStatus];
                                OfferData.Value = item.OfferAmountSplit;
                                if (item.GridRowHeightForAmountSplit)
                                    OfferData.OfferValue = item.ConvertedAmountSplit;
                                else
                                    OfferData.OfferValue = item.OfferAmountSplit;
                                OfferData.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                OfferData.IdSourceOffer = item.IdSourceOffer;
                                OfferData.IdCurrency = Currencies[item.SelectedIndexCurrencyTask].IdCurrency;
                                OfferData.Currency = new Currency { IdCurrency = Currencies[item.SelectedIndexCurrencyTask].IdCurrency, Name = Currencies[item.SelectedIndexCurrencyTask].Name };

                                if (OfferData.IdOfferType == 1)
                                {
                                    OfferData.IsUpdateLeadToOT = true;
                                }

                                OfferData.DeliveryDate = item.OfferCloseDateSplit;
                                OfferData.RFQReception = item.RFQReceptionDateSplit != null ? (DateTime)item.RFQReceptionDateSplit : DateTime.MinValue;
                                OfferData.SendIn = item.QuoteSentDateSplit != null ? (DateTime)item.QuoteSentDateSplit : DateTime.MinValue;
                                OfferData.ProbabilityOfSuccess = Convert.ToSByte(item.SelectedIndexConfidentialLevelTask.ToString());
                                OfferData.OfferExpectedDate = item.OfferCloseDateSplit;
                                OfferData.CreatedIn = GeosApplication.Instance.ServerDateTime;
                                OfferData.Comments = "";

                                if (string.IsNullOrEmpty(Rfq))
                                {
                                    OfferData.Rfq = "";
                                }
                                else
                                {
                                    OfferData.Rfq = Rfq.Trim();
                                }

                                OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                OfferData.OfferContacts = ListOfferContact.ToList();
                                OfferData.Site.FullName = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                                foreach (var itemListOfferContact in OfferData.OfferContacts)
                                {
                                    itemListOfferContact.People.OwnerImage = null;
                                }

                                //Company _Company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == item.TaskOffer.Site.ConnectPlantId.ToString()).FirstOrDefault();
                                //OfferData.Site.ConnectPlantId = _Company.ConnectPlantId;
                                //OfferData.Site.ConnectPlantConstr = _Company.ConnectPlantConstr;
                                OfferData.Site.SiteNameWithoutCountry = OfferData.Site.Name;
                                OfferData.OfferActiveSite = item.OfferActiveSite;
                                List<LogEntryByOffer> logEntryByOffers = new List<LogEntryByOffer>();
                                OfferData.LogEntryByOffers = logEntryByOffers;

                                OfferData.LostReasonsByOffer = new LostReasonsByOffer();
                                Offer offerReturnValue = null;

                                OfferData.OptionsByOffers = item.OptionsByOfferListSplit;

                                try
                                {
                                    //[001] Added
                                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(item.OfferActiveSite.SiteServiceProvider);
                                    //offerReturnValue = CrmStartUp.AddOffer(OfferData, GeosApplication.Instance.ActiveUser.IdCompany.Value, GeosApplication.Instance.ActiveUser.IdUser);
                                    //[001] Changed service method and controller
                                    offerReturnValue = CrmStartUpOfferActiveSite.AddOfferWithIdSourceOffer_V2040(OfferData, item.OfferActiveSite.IdSite, GeosApplication.Instance.ActiveUser.IdUser);
                                    OfferData.IdOffer = offerReturnValue.IdOffer;
                                    if (offerReturnValue.IdOffer > 0 && offerReturnValue.IdOfferType == 1 && OfferData.IsUpdateLeadToOT == true)
                                    {
                                        EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OfferData.OfferActiveSite.IdSite));
                                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                                        ICrmService CrmStartUpCreateFolder = new CrmServiceController(serviceurl);
                                        if (OfferData.IsUpdateLeadToOT)
                                        {
                                            offerReturnValue.Year = GeosApplication.Instance.ServerDateTime.Year;
                                        }
                                        bool isCreated = CrmStartUpCreateFolder.CreateFolderOffer(offerReturnValue, true);
                                    }

                                    //IsBusy = false;
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    IsBusy = false;
                                    OfferData = null;
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }
                                    GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    IsBusy = false;
                                    OfferData = null;
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }
                                    GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);

                                    //CustomMessageBox.Show((ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }

                                string OfferTypeName = "OFFER";
                                if (OfferTypeList[item.SelectedIndexOfferTypeTask].IdOfferType == 10)
                                {
                                    OfferTypeName = OfferTypeList[item.SelectedIndexOfferTypeTask].Name;
                                }

                                if (offerReturnValue != null && offerReturnValue.IdOffer > 0)
                                {
                                    if (string.IsNullOrEmpty(_savedOfferTypeName))
                                        _savedOfferTypeName = OfferTypeName + " " + item.TaskOfferCode;
                                    else
                                        _savedOfferTypeName = _savedOfferTypeName + " , " + OfferTypeName + " " + item.TaskOfferCode;

                                    if (OfferData != null)
                                        OfferDataLst.Add(OfferData);
                                    OfferData = null;
                                }
                                else
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsAddViewOfferNotCreated").ToString(), OfferTypeName)
                                        , "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }

                                GeosApplication.Instance.Logger.Log("Method SaveOfferWithSplit() executed successfully", category: Category.Info, priority: Priority.Low);

                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        //**[Start][This code section for generate activity for related split offer]
                        if (OfferDataLst.Count > 0)
                        {
                            string activityMsg = "";
                            List<ActivityTemplate> activityTemplateList = new List<ActivityTemplate>();
                            Dictionary<ActivityTemplate, List<Offer>> onlyQuotedForActivityDict = new Dictionary<ActivityTemplate, List<Offer>>();

                            foreach (ActivityTemplateTrigger activityTemplateTrigger in activityTemplateTriggers)
                            {
                                int offerIndex = 0;
                                List<Offer> onlyQuoted = new List<Offer>();

                                foreach (Offer itemOffer in OfferDataLst)
                                {
                                    //zero means it is the main offer.
                                    if (offerIndex == 0)
                                    {
                                        if (SelectedLeadList[0].GeosStatus.IdOfferStatusType != itemOffer.GeosStatus.IdOfferStatusType &&
                                            string.Format(itemOffer.GeosStatus.IdOfferStatusType + "(" + itemOffer.GeosStatus.Name + ")") == activityTemplateTrigger.LinkedObjectFieldValue)
                                        {
                                            Currency selectedcurrency = Currencies.FirstOrDefault(c => c.Name == activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldType);
                                            Double amount = itemOffer.Value * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : selectedcurrency.IdCurrency);

                                            if (Operator(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionOperator, amount, Convert.ToDouble(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldValue)))
                                            {
                                                //Get offers that are Satisfies these condotions.
                                                onlyQuoted.Add(itemOffer);
                                            }
                                        }
                                    }
                                    else if (offerIndex > 0)
                                    {
                                        //New splitted offer.
                                        if (string.Format(itemOffer.GeosStatus.IdOfferStatusType + "(" + itemOffer.GeosStatus.Name + ")") == activityTemplateTrigger.LinkedObjectFieldValue)
                                        {
                                            Currency selectedcurrency = Currencies.FirstOrDefault(c => c.Name == activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldType);
                                            Double amount = itemOffer.Value * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : selectedcurrency.IdCurrency);

                                            if (Operator(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionOperator, amount, Convert.ToDouble(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldValue)))
                                            {
                                                //Get offers that are Satisfies these condotions.
                                                onlyQuoted.Add(itemOffer);
                                            }
                                        }
                                    }

                                    offerIndex++;
                                }

                                if (onlyQuoted.Count > 0)
                                {
                                    if (activityTemplateTrigger.ActivityTemplateTriggerCondition.IsUserConfirmationRequired == 1)
                                    {

                                        foreach (var template1 in activityTemplateTrigger.ActivityTemplates)
                                        {
                                            if (activityTemplateTriggers.Count > 1)
                                            {
                                                if (string.IsNullOrWhiteSpace(activityMsg))
                                                {
                                                    activityMsg = string.Format(Application.Current.Resources["ActivityCreateMoreThenOne"].ToString());
                                                    activityMsg += System.Environment.NewLine + "-" + "\"" + activityTemplateTrigger.ActivityTemplates[0].Subject + "\" " + activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value + " activity";
                                                }

                                                else
                                                    activityMsg += System.Environment.NewLine + "-" + "\"" + activityTemplateTrigger.ActivityTemplates[0].Subject + "\" " + activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value + " activity";
                                            }
                                            else
                                            {
                                                activityMsg = string.Format(Application.Current.Resources["ActivityCreate"].ToString(), activityTemplateTrigger.ActivityTemplates[0].Subject, activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value);
                                            }

                                            if (!onlyQuotedForActivityDict.ContainsKey(template1))
                                                onlyQuotedForActivityDict.Add(template1, onlyQuoted);
                                        }


                                        //MessageBoxResult MessageBoxResult = CustomMessageBox.Show(activityMsg, "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                        //if (MessageBoxResult == MessageBoxResult.Yes)
                                        //{
                                        //    if (activityTemplateTrigger.ActivityTemplates != null && activityTemplateTrigger.ActivityTemplates.Count > 0)
                                        //    {

                                        //        foreach (ActivityTemplate activityTemplate in activityTemplateTrigger.ActivityTemplates)
                                        //        {
                                        //            AddActivityForSplitOpportunity(onlyQuoted, activityTemplate);
                                        //        }
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        foreach (ActivityTemplate activityTemplate in activityTemplateTrigger.ActivityTemplates)
                                        {
                                            AddActivityForSplitOpportunity(onlyQuoted, activityTemplate);
                                        }
                                    }
                                }
                            }

                            if (onlyQuotedForActivityDict.Count > 0)
                            {
                                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(activityMsg, Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                if (MessageBoxResult == MessageBoxResult.Yes)
                                {
                                    foreach (var item in onlyQuotedForActivityDict)
                                    {

                                        AddActivityForSplitOpportunity(item.Value, item.Key);

                                    }



                                }
                            }
                            //if (onlyQuoteLessAmount.Count > 0)
                            //{
                            //    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ActivityCreateForOpportunity"].ToString(), onlyQouteOfferCode), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                            //    if (MessageBoxResult == MessageBoxResult.Yes)
                            //    {
                            //        IsActivityCreateFromSaveOffer = true;
                            //        AddActivityViewWindowShowForSplitOpportunity(onlyQuoteLessAmount);
                            //    }
                            //}

                            //if (onlyQuoteMaxAmount.Count > 0)
                            //{
                            //    AddActivityForSplitOpportunity(onlyQuoteMaxAmount);
                            //}

                        }


                        //**[End][this code section for generate activity for related split offer]

                        if (!string.IsNullOrEmpty(_savedOfferTypeName))
                        {
                            string _MainOfferTypeName = OfferTypeList.Where(off => off.IdOfferType == SelectedLeadList[0].IdOfferType).Select(offs => offs.Name).FirstOrDefault();

                            int nInterval = 120;
                            string _savedOfferTypeNameSplit = String.Concat(_savedOfferTypeName.Select((c, i) => i > 0 && (i % nInterval) == 0 ? c.ToString() + Environment.NewLine : c.ToString()));

                            LogEntryByOffer logEntryByOffers = new LogEntryByOffer();
                            logEntryByOffers.IdOffer = _MainIdOffer;
                            logEntryByOffers.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            logEntryByOffers.DateTime = GeosApplication.Instance.ServerDateTime;
                            //logEntryByOffers.Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsAddViewOfferCreatedSplitLog").ToString(), _MainOfferTypeName, _savedOfferTypeNameSplit);
                            logEntryByOffers.Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsAddViewOfferCreatedSplitLog").ToString(), _savedOfferTypeNameSplit);
                            logEntryByOffers.IdLogEntryType = 7;
                            //[001] Added
                            ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(_OfferActiveSite.SiteServiceProvider);
                            //[001] Changed service method and controller
                            bool isLogAdd = CrmStartUpOfferActiveSite.AddLogEntryByOffer_V2040(logEntryByOffers);
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsAddViewOfferCreatedSplit").ToString(), _savedOfferTypeNameSplit)
                                                    , "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                            IsBusy = false;
                            RequestClose(null, null);
                        }
                    }
                }

                IsBusy = false;
                SelectedViewIndex = 0;
                GeosApplication.Instance.Logger.Log("Method SaveOfferWithSplit() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOfferWithSplit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsBusy = false;

        }

        public bool CanCloseTask(Task task)
        {
            if (task != null)
            {
                return task.IsComplete;
            }

            return true;
        }

        public void CloseTask(Task task)
        {
            //Tasks.Remove(task);
            GeosApplication.Instance.Logger.Log("Method CloseTask ...", category: Category.Info, priority: Priority.Low);

            if (task.IsComplete == true)
            {
                //** reset the index of ahead task if we close task tab from middel.
                int _DeletedTabIndex = 0;
                foreach (DataRow row in task.DttableSplit.Rows)
                {
                    _DeletedTabIndex = int.Parse(row["SplitTabIndex"].ToString());
                    break;
                }

                for (int i = Tasks.Count - 1; i > _DeletedTabIndex; i--)
                {

                    foreach (DataRow row in Tasks[i].DttableSplit.Rows)
                    {
                        row["SplitTabIndex"] = int.Parse(row["SplitTabIndex"].ToString()) - 1;
                    }

                }

                Tasks.Remove(task);
            }

            GeosApplication.Instance.Logger.Log("Method CloseTask() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method for Initialize window as per selected offer list.
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [002][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        /// <param name="leadList"></param>
        public void InIt(IList<Offer> leadList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt ...", category: Category.Info, priority: Priority.Low);
                /// [002] Added
                if (leadList[0].OfferActiveSite == null)
                {
                    if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == leadList[0].Site.ConnectPlantId))
                    {
                        Company company = GeosApplication.Instance.CompanyList.Where(cl => cl.IdCompany.ToString() == leadList[0].Site.ConnectPlantId).FirstOrDefault();
                        leadList[0].OfferActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                    }
                }

                //[002] Added to create controller of service to hit offer plant service
                ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(leadList[0].OfferActiveSite.SiteServiceProvider);
                IsInIt = true;
                SelectedLeadList = new List<Offer>();
                SelectedLeadList = leadList;
                VisibilityShipment = Visibility.Collapsed;
                IsSleepDaysVisible = Visibility.Collapsed;
                //Tasks = new ObservableCollection<Offer>();
                //OfferData.IdStatus = SelectedGeosStatus.IdOfferStatusType; //GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
                //OfferData.GeosStatus = SelectedGeosStatus
                //SelectedGeosStatusSplit = new GeosStatus();
                //SelectedGeosStatusSplit = SelectedGeosStatus;

                SelectedPlantDetails = SelectedLeadList[0].Site;

                GridRowHeight = false;

                if (SelectedLeadList[0].IdOfferType == 1)
                {
                    isOldOT = true;
                }

                //CrmStartUp.GetOfferType
                Description = SelectedLeadList[0].Description;
                OfferCode = SelectedLeadList[0].Code;
                TempOfferCode = SelectedLeadList[0].Code;
                OfferCode = SelectedLeadList[0].Code;
                OfferActiveSite = SelectedLeadList[0].OfferActiveSite;
                ProductAndService();

                if (SelectedLeadList[0].OfferExpectedDate != null)
                {
                    OfferCloseDate = DateTime.Parse(SelectedLeadList[0].OfferExpectedDate.ToString());
                }

                if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                {
                    RFQReceptionDate = DateTime.Parse(SelectedLeadList[0].RFQReception.ToString());
                }

                if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                {
                    QuoteSentDate = DateTime.Parse(SelectedLeadList[0].SendIn.ToString());
                }

                if (SelectedLeadList[0].DeliveryDate != null && SelectedLeadList[0].DeliveryDate != DateTime.MinValue)
                {
                    DeliveryDate = DateTime.Parse(SelectedLeadList[0].DeliveryDate.ToString());
                }
                Rfq = SelectedLeadList[0].Rfq;

                //ONLY QUOTED
                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType == 1)
                {
                    //if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                    //    GridRowHeightForRfq = true;
                    //else
                    //    GridRowHeightForRfq = true;

                    GridRowHeightForRfq = true;


                    if (SelectedLeadList[0].SendIn != null)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = true;

                    IsSleepDaysVisible = Visibility.Visible;
                }

                //WAITING FOR QUOTED
                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType == 2)
                {
                    //if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                    //    GridRowHeightForRfq = true;
                    //else
                    //    GridRowHeightForRfq = true;

                    GridRowHeightForRfq = true;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;
                }
                //CANCELLED
                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType == 4)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;

                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;

                }
                //FORCASTED
                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType == 15)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;

                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;

                }
                //QUALIFIED
                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType == 16)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;

                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;
                }

                //LOST
                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType == 17)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;
                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;
                }

                OfferAmount = SelectedLeadList[0].OfferValue;
                // ConvertedOfferAmount = SelectedLeadList[0].Value + currencies[ConvertedIndexCurrency].Name;
                CompanyName = SelectedLeadList[0].Site.Customers[0].CustomerName + "-" + SelectedLeadList[0].Site.Name;
                IdSite = (Int16)SelectedLeadList[0].Site.IdCompany;
                FillCurrencyList();
                FillStatusList();
                FillStatusListSplit();
                FillCaroemsList();
                FillCompanyPlantList();
                FillGroupList();
                FillExistingActivitiesToBeLinkedToOffer();
                FillOfferType();
                FillConfidentialLevelList();
                FillGeosProjectsList();
                // FillLeadsEdit();
                FillBusinessUnitList();
                FillLeadSourceList();   // Lead Source
                StatusChangeAction();
                FillActivity();
                FillAllProductCategory();

                if (SelectedLeadList[0].EngineeringAnalysis != null)
                    FillEngineeringAnalysis(SelectedLeadList[0].EngineeringAnalysis);
                // If GeosStatus is "17-LOST" or "4-Cancelled" then do not set any MinValue to date edit.
                // Other than "17-LOST" or "4-Cancelled" then Set MinValue to Todays date.
                if (SelectedLeadList[0].GeosStatus != null && (SelectedLeadList[0].GeosStatus.IdOfferStatusType != 17 && SelectedLeadList[0].GeosStatus.IdOfferStatusType != 4))
                {
                    OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
                }
                try
                {
                    //[002] Changed controller and service method GetAllCommentsByIdOffer to GetAllCommentsByIdOffer_V2040
                    ListLogComments = new ObservableCollection<LogEntryByOffer>(CrmStartUpOfferActiveSite.GetAllCommentsByIdOffer_V2040(SelectedLeadList[0].IdOffer).AsEnumerable());
                    SetUserProfileImage(ListLogComments);
                    RtfToPlaintext();
                    //[002] Changed controller and service method GetAllLogEntriesByIdOffer to GetAllLogEntriesByIdOffer_V2040
                    ListChangeLog = new ObservableCollection<LogEntryByOffer>(CrmStartUpOfferActiveSite.GetAllLogEntriesByIdOffer_V2040(SelectedLeadList[0].IdOffer).AsEnumerable());

                    //FillOfferContactList();

                    FillOfferOwnerList();
                    FillOfferToList();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in InIt() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }

                #region Sleeping opportunities (Last_activity_date to Sleep(d) Calculation)

                if (SelectedLeadList[0].GeosStatus != null && SelectedLeadList[0].GeosStatus.IdOfferStatusType == 1)
                {
                    /// If some comment exists then Last activity date = latest comment date
                    if (ListLogComments.Count > 0)
                    {
                        int result = Nullable.Compare<DateTime>(ListLogComments[0].DateTime, SelectedLeadList[0].LastActivityDate);

                        if (result > 0)
                            SelectedLeadList[0].LastActivityDate = ListLogComments[0].DateTime;
                    }

                    /// If some activity exists then Last activity date = latest activity date.
                    if (LeadActivityList.Count > 0)
                    {
                        var activityDates = LeadActivityList.SelectMany(x => x.ActivityLinkedItem.Select(y => y.CreationDate));
                        var activityDate = activityDates.Max(x => x);
                        int result = Nullable.Compare<DateTime>(activityDate, SelectedLeadList[0].LastActivityDate);

                        if (result > 0)
                            SelectedLeadList[0].LastActivityDate = activityDate;
                    }
                    if (ListLogComments.Count == 0 && LeadActivityList.Count == 0)
                    {
                        SelectedLeadList[0].LastActivityDate = SelectedLeadList[0].SendIn;
                    }
                    if (SelectedLeadList[0].LastActivityDate.HasValue)
                    {
                        if (SelectedLeadList[0].LastActivityDate.Value.Year < 1700)
                            SelectedLeadList[0].LastActivityDate = DateTime.Now;
                    }
                    else
                    {
                        SelectedLeadList[0].LastActivityDate = DateTime.Now;
                    }

                    if (SelectedLeadList[0].LastActivityDate != null)
                    {

                        LeadsSleepDays = Convert.ToInt32((GeosApplication.Instance.ServerDateTime.Date - SelectedLeadList[0].LastActivityDate.Value.Date).TotalDays);
                    }
                }

                #endregion //sleeping opportunities

                // Background = Colors.Transparent;
                HoverBackground = Colors.Red;
                SelectedBackground = Colors.Transparent;

                // [001]added
               // ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                ShowStatusList = CrmStartUpOfferActiveSite.GetOfferDetailsById_V2040(SelectedLeadList[0].IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, SelectedLeadList[0].OfferActiveSite);

                SelectedIndexBusinessUnit = BusinessUnitList.FindIndex(i => i.IdLookupValue == SelectedLeadList[0].IdBusinessUnit);
                if (SelectedIndexBusinessUnit == -1)
                    SelectedIndexBusinessUnit = 0;

                // Lead Source
                SelectedIndexLeadSource = LeadSourceList.IndexOf(LeadSourceList.FirstOrDefault(i => i.IdLookupValue == SelectedLeadList[0].IdSource));
                if (SelectedIndexLeadSource == -1)
                    SelectedIndexLeadSource = 0;

                SelectedIndexConfidentialLevel = SelectedLeadList[0].ProbabilityOfSuccess;
                SelectedConfidentialLevel();

                LeadsGenerateDays = (GeosApplication.Instance.ServerDateTime.Date - SelectedLeadList[0].CreatedIn.Date).Days;
                //LeadsSleepDays = (GeosApplication.Instance.ServerDateTime.Date - SelectedLeadList[0].ModifiedIn.Value.Date).Days;
                //BusinessUnitList = CrmStartUp.GetLookupValues(2);

                OptionsByOfferList = new List<OptionsByOffer>();
                //OptionsByOfferList = SelectedLeadList[0].OptionsByOffers.ToList();

                // This is Added to Copy the quantity to another list.
                if (SelectedLeadList != null && SelectedLeadList.Count > 0)
                {
                    foreach (var item in SelectedLeadList[0].OptionsByOffers)
                    {
                        OptionsByOffer offerOption = new OptionsByOffer();
                        offerOption.IdOffer = item.IdOffer;
                        offerOption.IdOption = item.IdOption;
                        offerOption.IsSelected = item.IsSelected;
                        offerOption.Offer = item.Offer;
                        offerOption.Quantity = item.Quantity != null ? item.Quantity : 0;
                        offerOption.OfferOption = item.OfferOption;
                        OptionsByOfferList.Add(offerOption);
                    }
                }

                ProductAndServicesCount = OptionsByOfferList.Count;

                if (leadList[0].SalesOwner != null && !string.IsNullOrEmpty(leadList[0].SalesOwner.FullName.Trim()))
                {
                    // SelectedIndexSalesOwner = -1;
                }
                else if (leadList[0].Site.People != null && !string.IsNullOrEmpty(leadList[0].Site.People.FullName.Trim()) && !string.IsNullOrEmpty(leadList[0].Site.PeopleSalesResponsibleAssemblyBU.FullName.Trim()))
                {
                    // SelectedIndexSalesOwner = -1; 
                }
                else if (leadList[0].Site.People != null && !string.IsNullOrEmpty(leadList[0].Site.People.FullName.Trim()) && string.IsNullOrEmpty(leadList[0].Site.PeopleSalesResponsibleAssemblyBU.FullName.Trim()))
                {
                    SelectedIndexSalesOwner = -1;
                }
                else if (leadList[0].Site.People != null && string.IsNullOrEmpty(leadList[0].Site.People.FullName.Trim()) && !string.IsNullOrEmpty(leadList[0].Site.PeopleSalesResponsibleAssemblyBU.FullName.Trim()))
                {
                    SelectedIndexSalesOwner = -1;
                }

                TempSelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == SelectedLeadList[0].IdOfferType);
                if (GeosStatusList[0].IdOfferStatusType > 0)
                {
                    if (GeosStatusList.Any(gl => gl.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType))
                        SelectedGeosStatus = GeosStatusList.Where(gl => gl.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType).FirstOrDefault();
                }

                if (SelectedGeosStatus != null)
                    TempIdOfferStatusType = SelectedGeosStatus.IdOfferStatusType;

                IsInIt = false;

                GeosApplication.Instance.Logger.Log("Method InIt() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [002][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        /// <param name="leadList"></param>
        /// <param name="SelectedIndexTab"></param>
        public void InItLeadSplit(IList<Offer> leadList, int SelectedIndexTab)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method InItLeadSplit ...", category: Category.Info, priority: Priority.Low);
                /// [002] Added
                if (SelectedLeadList[0].OfferActiveSite == null)
                {
                    if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == SelectedLeadList[0].Site.ConnectPlantId))
                    {
                        Company company = GeosApplication.Instance.CompanyList.Where(cl => cl.IdCompany.ToString() == SelectedLeadList[0].Site.ConnectPlantId).FirstOrDefault();
                        SelectedLeadList[0].OfferActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                    }
                }

                if (SelectedLeadList[0].DeliveryDate == null)
                {
                    SelectedLeadList[0].DeliveryDate = DateTime.MinValue;
                }

                foreach (DataRow row in Dttable.Rows)
                {
                    row["SplitTabIndex"] = SelectedIndexTab;
                }


                Tasks.Add(new Task()
                {

                    CurrentCompanyTask = CurrentCompany,
                    SelectedIndexTab = SelectedIndexTab,
                    IsTabIndexZero = true,
                    IsComplete = false,
                    TaskOffer = SelectedLeadList[0],
                    TaskOfferCode = SelectedLeadList[0].Code,
                    OfferTypeListTask = OfferTypeList,
                    IdOfferType = SelectedLeadList[0].IdOfferType,
                    OfferAmountSplit = SelectedLeadList[0].OfferValue,
                    OfferCloseDateSplit = SelectedLeadList[0].DeliveryDate.Value,
                    OfferCloseDateSplitMinValue = GeosApplication.Instance.ServerDateTime.Date,
                    QuoteSentDateSplit = SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue ? DateTime.Parse(SelectedLeadList[0].SendIn.ToString()) : (DateTime?)null,
                    RFQReceptionDateSplit = SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue ? DateTime.Parse(SelectedLeadList[0].RFQReception.ToString()) : (DateTime?)null,
                    GeosStatusListTask = GeosStatusListSplit,
                    IsShowAllSplit = false,
                    DttableSplittemp = Dttable.Copy(),
                    DttableSplit = Dttable.Copy(),
                    OptionsByOfferListSplit = new List<OptionsByOffer>(), //SelectedLeadList[0].OptionsByOffers.ToList()),
                    ProductAndServicesSplitCount = SelectedLeadList[0].OptionsByOffers.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList().Count(),
                    SelectedIndexCurrencyTask = Currencies.FindIndex(crr => crr.IdCurrency == SelectedLeadList[0].IdOfferCurrency),
                    SelectedGeosStatusTask = GeosStatusListSplit.IndexOf(GeosStatusListSplit.FirstOrDefault(geosS => geosS.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType)),
                    SelectedGeosStatusTaskold = GeosStatusListSplit.IndexOf(GeosStatusListSplit.FirstOrDefault(geosS => geosS.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType)),
                    HoverBackgroundTask = Colors.Red,
                    SelectedBackgroundTask = Colors.Transparent,
                    SelectedIndexConfidentialLevelTask = SelectedLeadList[0].ProbabilityOfSuccess,
                    SelectedOfferTypeIdTasktemp = SelectedLeadList[0].IdOfferType.Value,
                    OfferActiveSite= SelectedLeadList[0].OfferActiveSite,//[002]Added to get offer plant detail(Idsite,Alias,ServiceProvider)
                    //IdSourceOffer= selectedLeadList[0].IdOffer,

                });

                //***
                if (Tasks != null && Tasks.Count > 0)
                {
                    SelectedIndex = Tasks.Count;
                }

                Tasks[0].Init(SelectedLeadList[0], Currencies);

                Tasks[0].SelectedGeosStatusTaskold = Tasks[0].SelectedGeosStatusTask;
                Tasks[0].TaskOfferCodeold = Tasks[0].TaskOfferCode;

                IsBusy = true;

                foreach (DataRow row in Dttable.Rows)
                {
                    row["SplitTabIndex"] = SelectedIndex;
                }
                //List<Int64> tempremovelist = new List<Int64>() { 19, 23, 25 };

                //offerOptions = offerOptions.Where(t => !tempremovelist.Contains(t.IdOfferOption)).ToList();
                Tasks.Add(new Task()
                {
                    CurrentCompanyTask = CurrentCompany,
                    TaskOffer = SelectedLeadList[0],
                    SelectedIndexTab = SelectedIndex,
                    IsTabIndexZero = false,
                    IsComplete = true,
                    OfferTypeListTask = OfferTypeList,
                    IdOfferType = SelectedLeadList[0].IdOfferType,
                    OfferAmountSplit = SelectedLeadList[0].OfferValue,
                    OfferCloseDateSplit = SelectedLeadList[0].DeliveryDate.Value,
                    OfferCloseDateSplitMinValue = GeosApplication.Instance.ServerDateTime.Date,
                    QuoteSentDateSplit = SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue ? DateTime.Parse(SelectedLeadList[0].SendIn.ToString()) : (DateTime?)null,
                    RFQReceptionDateSplit = SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue ? DateTime.Parse(SelectedLeadList[0].RFQReception.ToString()) : (DateTime?)null,
                    GeosStatusListTask = GeosStatusListSplit,
                    IsShowAllSplit = false,
                    DttableSplittemp = Dttable.Copy(),
                    DttableSplit = Dttable.Copy(),
                    OptionsByOfferListSplit = new List<OptionsByOffer>(), //SelectedLeadList[0].OptionsByOffers.ToList()),
                    ProductAndServicesSplitCount = SelectedLeadList[0].OptionsByOffers.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList().Count(),
                    SelectedIndexCurrencyTask = Currencies.FindIndex(crr => crr.IdCurrency == SelectedLeadList[0].IdOfferCurrency),
                    SelectedGeosStatusTask = GeosStatusListSplit.IndexOf(GeosStatusListSplit.FirstOrDefault(geosS => geosS.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType)),
                    //SelectedGeosStatusTaskold = GeosStatusListSplit.FindIndex(geosS => geosS.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType),
                    HoverBackgroundTask = Colors.Red,
                    SelectedBackgroundTask = Colors.Transparent,
                    SelectedIndexConfidentialLevelTask = SelectedLeadList[0].ProbabilityOfSuccess,
                    IdSourceOffer = selectedLeadList[0].IdOffer,
                    OfferActiveSite = SelectedLeadList[0].OfferActiveSite,//[002]Added to get offer plant detail(Idsite,Alias,ServiceProvider)
                });

                Tasks[1].Init(SelectedLeadList[0], Currencies);
                Tasks[1].SplitOffer.IsGoAheadProduction = false;//[SP-67-001] added

                foreach (var item in Tasks)
                {
                    string error = item.CheckValidation();
                    if (error != null)
                    {
                        IsBusy = false;
                        return;
                    }
                }


                IsBusy = false;
                // SelectedIndex = 0;
                // Tasks[0].IsTabIndexZero = false;
                GeosApplication.Instance.Logger.Log("Method InItLeadSplit() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Get an error in InItLeadSplit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [002][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        /// <param name="leadList"></param>
        public void InItLeadsEditReadonly(IList<Offer> leadList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InItLeadsEditReadonly ...", category: Category.Info, priority: Priority.Low);
                /// [002] Added
                if (leadList[0].OfferActiveSite == null)
                {
                    if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == leadList[0].Site.ConnectPlantId))
                    {
                        Company company = GeosApplication.Instance.CompanyList.Where(cl => cl.IdCompany.ToString() == leadList[0].Site.ConnectPlantId).FirstOrDefault();
                        leadList[0].OfferActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                    }
                }
                ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(leadList[0].OfferActiveSite.SiteServiceProvider);
                IsInIt = true;
                IsSplitVisible = false;
                SelectedLeadList = new List<Offer>();
                SelectedLeadList = leadList;
                VisibilityShipment = Visibility.Visible;
                IsSleepDaysVisible = Visibility.Hidden;
                TempOfferCode = SelectedLeadList[0].Code;
                OfferCode = SelectedLeadList[0].Code;
                Description = SelectedLeadList[0].Description;
                //[002]Added to get offer plant detail(Idsite,Alias,ServiceProvider)
                OfferActiveSite = SelectedLeadList[0].OfferActiveSite;
                ProductAndService();
                SelectedPlantDetails = SelectedLeadList[0].Site;
                if (!IsControlEnableorder)
                {
                    if (SelectedLeadList[0].POReceivedInDate != null)
                        OfferCloseDate = DateTime.Parse(SelectedLeadList[0].POReceivedInDate.ToString());
                }

                else if (SelectedLeadList[0].OfferExpectedDate != null)
                {
                    OfferCloseDate = DateTime.Parse(SelectedLeadList[0].OfferExpectedDate.ToString());
                }

                if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                {
                    RFQReceptionDate = DateTime.Parse(SelectedLeadList[0].RFQReception.ToString());
                }

                if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                {
                    QuoteSentDate = DateTime.Parse(SelectedLeadList[0].SendIn.ToString());
                }

                if (!IsControlEnableorder)
                {
                    if (SelectedLeadList[0].OTSDeliveryDate != null && SelectedLeadList[0].OTSDeliveryDate != DateTime.MinValue)
                    {
                        DeliveryDate = DateTime.Parse(SelectedLeadList[0].OTSDeliveryDate.ToString());
                    }
                }
                else if (SelectedLeadList[0].DeliveryDate != null && SelectedLeadList[0].DeliveryDate != DateTime.MinValue)
                {
                    DeliveryDate = DateTime.Parse(SelectedLeadList[0].DeliveryDate.ToString());
                }
                //8888
                Rfq = SelectedLeadList[0].Rfq;

                GridRowHeight = true;
                // LeftPanelHeight = new GridLength(250, GridUnitType.Auto);
                OfferAmount = SelectedLeadList[0].OfferValue;
                //ConvertedOfferAmount = SelectedLeadList[0].Value + currencies[ConvertedIndexCurrency].Name;
                CompanyName = SelectedLeadList[0].Site.Customers[0].CustomerName + "-" + SelectedLeadList[0].Site.Name;
                FillCurrencyList();
                FillStatusList();
                FillCaroemsList();
                FillGroupList();
                //FillCompanyPlantList();
                FillExistingActivitiesToBeLinkedToOffer();
                FillOfferType();
                FillConfidentialLevelList();
                FillGeosProjectsList();
                FillActivity();
                FillBusinessUnitList();
                FillLeadSourceList();   // Lead Source
                FillAllProductCategory();//Product Category


                if (SelectedLeadList[0].EngineeringAnalysis != null)
                    FillEngineeringAnalysis(SelectedLeadList[0].EngineeringAnalysis);
                //StatusChangeAction();
                //OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime.Date;

                // If GeosStatus is "17-LOST" or "4-Cancelled" then do not set any MinValue to date edit.
                // Other than "17-LOST" or "4-Cancelled" then Set MinValue to Todays date.
                if (SelectedLeadList[0].GeosStatus != null && (SelectedLeadList[0].GeosStatus.IdOfferStatusType != 17 && SelectedLeadList[0].GeosStatus.IdOfferStatusType != 4))
                {
                    if (SelectedLeadList[0].DeliveryDate != null)
                        OfferCloseDateMinValue = DateTime.Parse(SelectedLeadList[0].OfferExpectedDate.ToString());
                    else
                        OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime;
                }

                try
                {
                    //[002] Changed controller and service method GetAllCommentsByIdOffer to GetAllCommentsByIdOffer_V2040
                    ListLogComments = new ObservableCollection<LogEntryByOffer>(CrmStartUpOfferActiveSite.GetAllCommentsByIdOffer_V2040(SelectedLeadList[0].IdOffer).AsEnumerable());
                    RtfToPlaintext();
                    //[002] Changed controller and service method GetAllLogEntriesByIdOffer to GetAllLogEntriesByIdOffer_V2040
                    ListChangeLog = new ObservableCollection<LogEntryByOffer>(CrmStartUpOfferActiveSite.GetAllLogEntriesByIdOffer_V2040(SelectedLeadList[0].IdOffer).AsEnumerable());
                    ListShipment = new List<Shipment>();
                    //[002] Changed controller and service method GetAllShipmentsByOfferId to GetAllShipmentsByOfferId_V2040
                    ListShipment = CrmStartUpOfferActiveSite.GetAllShipmentsByOfferId_V2040(SelectedLeadList[0].IdOffer).OrderByDescending(a => a.DeliveryDate).ToList();

                    //FillOfferContactList();
                    FillOfferOwnerList();
                    FillOfferToList();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in InItLeadsEditReadonly() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in InItLeadsEditReadonly() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }

                #region Spleeping opportunities (Last_activity_date to Sleep(d) Calculation)

                if (SelectedLeadList[0].GeosStatus != null && SelectedLeadList[0].GeosStatus.IdOfferStatusType == 1)
                {
                    /// If some comment exists then Last activity date = latest comment date.
                    if (ListLogComments.Count > 0)
                    {
                        int result = Nullable.Compare<DateTime>(ListLogComments[0].DateTime, SelectedLeadList[0].LastActivityDate);

                        if (result > 0)
                            SelectedLeadList[0].LastActivityDate = ListLogComments[0].DateTime;
                    }

                    /// If some activity exists then Last activity date = activity creation date.
                    if (LeadActivityList.Count > 0)
                    {

                        var activityDates = LeadActivityList.SelectMany(x => x.ActivityLinkedItem.Select(y => y.CreationDate));
                        var activityDate = activityDates.Max(x => x);
                        int result = Nullable.Compare<DateTime>(activityDate, SelectedLeadList[0].LastActivityDate);
                        if (result > 0)
                            SelectedLeadList[0].LastActivityDate = activityDate;
                    }

                    //If no comments and no linked activities then Last_activity_date= quote sent date
                    if (ListLogComments.Count == 0 && LeadActivityList.Count == 0)
                    {
                        SelectedLeadList[0].LastActivityDate = SelectedLeadList[0].SendIn;
                    }

                    if (SelectedLeadList[0].LastActivityDate != null)
                    {
                        LeadsSleepDays = Convert.ToInt32((GeosApplication.Instance.ServerDateTime.Date - SelectedLeadList[0].LastActivityDate.Value.Date).TotalDays);
                    }
                }

                #endregion

                // Background = Colors.Transparent;
                HoverBackground = Colors.Red;
                SelectedBackground = Colors.Transparent;
                //[001] Added 
                // [002] Changed service method and controller
                ShowStatusList = CrmStartUpOfferActiveSite.GetOfferDetailsById_V2040(SelectedLeadList[0].IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, SelectedLeadList[0].OfferActiveSite);
                SelectedIndexBusinessUnit = BusinessUnitList.FindIndex(i => i.IdLookupValue == SelectedLeadList[0].IdBusinessUnit);
                if (SelectedIndexBusinessUnit == -1)
                    SelectedIndexBusinessUnit = 0;

                // Lead Source
                SelectedIndexLeadSource = LeadSourceList.IndexOf(LeadSourceList.FirstOrDefault(i => i.IdLookupValue == SelectedLeadList[0].IdSource));
                if (SelectedIndexLeadSource == -1)
                    SelectedIndexLeadSource = 0;

                SelectedIndexConfidentialLevel = SelectedLeadList[0].ProbabilityOfSuccess;
                SelectedConfidentialLevel();

                LeadsGenerateDays = (GeosApplication.Instance.ServerDateTime.Date - SelectedLeadList[0].CreatedIn.Date).Days;

                //BusinessUnitList = CrmStartUp.GetLookupValues(2);

                OptionsByOfferList = new List<OptionsByOffer>();
                //OptionsByOfferList = SelectedLeadList[0].OptionsByOffers.ToList();

                // This is Added to Copy the quantity to another list.
                if (SelectedLeadList != null && SelectedLeadList.Count > 0)
                {
                    foreach (var item in SelectedLeadList[0].OptionsByOffers)
                    {
                        OptionsByOffer offerOption = new OptionsByOffer();
                        offerOption.IdOffer = item.IdOffer;
                        offerOption.IdOption = item.IdOption;
                        offerOption.IsSelected = item.IsSelected;
                        offerOption.Offer = item.Offer;
                        offerOption.Quantity = item.Quantity != null ? item.Quantity : 0;
                        offerOption.OfferOption = item.OfferOption;
                        OptionsByOfferList.Add(offerOption);
                    }
                }

                ProductAndServicesCount = OptionsByOfferList.Count;

                if (leadList[0].SalesOwner != null && !string.IsNullOrEmpty(leadList[0].SalesOwner.FullName.Trim()))
                {
                    //SelectedIndexSalesOwner = -1;
                }
                else if (leadList[0].Site.People != null && !string.IsNullOrEmpty(leadList[0].Site.People.FullName.Trim()) && leadList[0].Site.PeopleSalesResponsibleAssemblyBU != null && !string.IsNullOrEmpty(leadList[0].Site.PeopleSalesResponsibleAssemblyBU.FullName.Trim()))
                {
                    // SelectedIndexSalesOwner = -1; 
                }
                else if (leadList[0].Site.People != null && !string.IsNullOrEmpty(leadList[0].Site.People.FullName.Trim()) && leadList[0].Site.PeopleSalesResponsibleAssemblyBU != null && string.IsNullOrEmpty(leadList[0].Site.PeopleSalesResponsibleAssemblyBU.FullName.Trim()))
                {
                    SelectedIndexSalesOwner = -1;
                }
                else if (leadList[0].Site.People != null && string.IsNullOrEmpty(leadList[0].Site.People.FullName.Trim()) && leadList[0].Site.PeopleSalesResponsibleAssemblyBU != null && !string.IsNullOrEmpty(leadList[0].Site.PeopleSalesResponsibleAssemblyBU.FullName.Trim()))
                {
                    SelectedIndexSalesOwner = -1;
                }

                if (GeosStatusList[0].IdOfferStatusType > 0)
                {
                    if (GeosStatusList.Any(gl => gl.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType))
                        SelectedGeosStatus = GeosStatusList.Where(gl => gl.IdOfferStatusType == SelectedLeadList[0].GeosStatus.IdOfferStatusType).FirstOrDefault();
                }
                if (SelectedGeosStatus != null)
                    TempIdOfferStatusType = SelectedGeosStatus.IdOfferStatusType;
                IsInIt = false;

                GeosApplication.Instance.Logger.Log("Method InItLeadsEditReadonly() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InItLeadsEditReadonly() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Set Offer Code By Condition.
        /// </summary>
        private void SetOfferCodeByCondition()
        {
            GeosApplication.Instance.Logger.Log("Method SetOfferCodeByCondition ...", category: Category.Info, priority: Priority.Low);

            if (SelectedGeosStatus != null && !IsStatusChangeAction)
            {
                //Start This part of code convert Lead to OT and vice versa.
                //if status is Only quoted and Waiting for quote then as per condition generate the code or set old code.
                if (SelectedGeosStatus.IdOfferStatusType == 1 || SelectedGeosStatus.IdOfferStatusType == 2)
                {
                    if (SelectedIndexOfferType == OfferTypeList.FindIndex(i => i.IdOfferType == 1))
                    {
                        // if status is changed from lost/cancelled to quoted/Waitingforquote and offer type is Leads then generate new code for OT.
                        if (!isOldOT)
                        {
                            SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == 1);
                            GenerateOfferCode();
                            SelectedLeadList[0].IsUpdateLeadToOT = true;
                        }
                    }
                    else
                    {
                        //If user choose except Forecasted, Qualified, Only quoted and Waiting for quoted then code must be old one .
                        if (isOldOT)
                        {
                            SelectedIndexOfferType = TempSelectedIndexOfferType;
                            if (TempOfferCode != null)
                            {
                                OfferCode = TempOfferCode;
                            }
                            SelectedLeadList[0].IsUpdateLeadToOT = false;
                        }
                        else
                        {
                            SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == 1);
                            GenerateOfferCode();
                            SelectedLeadList[0].IsUpdateLeadToOT = true;
                        }
                    }
                }
                else
                {

                    if (SelectedGeosStatus.IdOfferStatusType == 15 || SelectedGeosStatus.IdOfferStatusType == 16)
                    {
                        //if user choose Forecasted or Qualified then don't convert code just set old code. 
                        if (SelectedIndexOfferType == OfferTypeList.FindIndex(i => i.IdOfferType == 10))
                        {
                        }
                        else
                        {
                            if (isOldOT)
                            {
                                SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == 10);
                                GenerateOfferCode();
                                SelectedLeadList[0].IsUpdateLeadToOT = true;
                            }
                            else
                            {
                                SelectedIndexOfferType = TempSelectedIndexOfferType;
                                if (TempOfferCode != null)
                                    OfferCode = TempOfferCode;
                                SelectedLeadList[0].IsUpdateLeadToOT = false;
                            }
                        }
                    }
                    //If user choose except Forecasted, Qualified, Only quoted and Waiting for quoted then code must be old one .
                    else
                    {
                        SelectedIndexOfferType = TempSelectedIndexOfferType;
                        if (TempOfferCode != null)
                            OfferCode = TempOfferCode;
                        SelectedLeadList[0].IsUpdateLeadToOT = false;
                    }
                }

                //END This Method convert Lead to OT and vice versa.
            }

            GeosApplication.Instance.Logger.Log("Method SetOfferCodeByCondition() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method for generate new offer code as per condition.
        /// [001][cpatil][GEOS2-1977] The code added in the offer code must be taken from the application selected site
        /// </summary>
        private void GenerateOfferCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GenerateOfferCode ...", category: Category.Info, priority: Priority.Low);

                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 1 || OfferTypeList[SelectedIndexOfferType].IdOfferType == 2 || OfferTypeList[SelectedIndexOfferType].IdOfferType == 3 || OfferTypeList[SelectedIndexOfferType].IdOfferType == 4)
                {
                    OfferNumber = CrmStartUp.GetNextNumberOfSuppliesFromGCM(OfferTypeList[SelectedIndexOfferType].IdOfferType);
                }
                else
                {
                    //[001] Changed service method GetNextNumberOfOfferFromCounters to GetNextNumberOfOfferFromCounters_V2040
                    OfferNumber = CrmStartUp.GetNextNumberOfOfferFromCounters_V2040(OfferTypeList[SelectedIndexOfferType].IdOfferType,  GeosApplication.Instance.ActiveUser.IdUser);
                }

                SelectedLeadList[0].NumberOfOffers = OfferNumber;

                // [001] Changed service method MakeOfferCode to MakeOfferCode_V2040
                OfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeList[SelectedIndexOfferType].IdOfferType, OfferActiveSite.IdSite, GeosApplication.Instance.ActiveUser.IdUser);

                GeosApplication.Instance.Logger.Log("Method GenerateOfferCode() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCode() " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCode() " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }

        }


        /// <summary>
        /// Method for fill all technichal template detail.
        /// </summary>
        private void FillLeadsEdit()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadsEdit ...", category: Category.Info, priority: Priority.Low);

                TemplateList = CrmStartUp.GetTemplates();
                TemplateDetailList = new List<Quotation>();

                foreach (Template Item in TemplateList)
                {
                    Quotation quotation = new Quotation();
                    quotation.Template = new Template { IdTemplate = Item.IdTemplate, Name = Item.Name, Suffix = Item.Suffix };
                    quotation.Template.Name = Item.Name;
                    quotation.IdDetectionsTemplate = Item.IdTemplate;

                    TemplateDetailList.Add(quotation);
                }

                TemplateDetailList = TemplateDetailList.Select(i => { i.QuotQuantity = SelectedLeadList[0].Quotations.Where(j => j.Template.Name == i.Template.Name).Select(z => z.QuotQuantity).FirstOrDefault(); i.IdOffer = SelectedLeadList[0].Quotations.Where(j => j.Template.Name == i.Template.Name).Select(z => z.IdOffer).FirstOrDefault(); i.IdQuotation = SelectedLeadList[0].Quotations.Where(j => j.Template.Name == i.Template.Name).Select(z => z.IdQuotation).FirstOrDefault(); return i; }).ToList();

                GeosApplication.Instance.Logger.Log("Method FillLeadsEdit() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsEdit() Method - ServiceUnexceptedException " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsEdit() Method  " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsEdit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for select rating star as per confidential Level.
        /// </summary>
        private void SelectedConfidentialLevel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedConfidentialLevel ...", category: Category.Info, priority: Priority.Low);

                if (SelectedIndexConfidentialLevel > 0 && SelectedIndexConfidentialLevel <= 20)
                {
                    SelectedBackground = Colors.Red;
                }
                if (SelectedIndexConfidentialLevel > 20 && SelectedIndexConfidentialLevel <= 40)
                {
                    SelectedBackground = Colors.Orange;
                }
                if (SelectedIndexConfidentialLevel > 40 && SelectedIndexConfidentialLevel <= 60)
                {
                    SelectedBackground = Colors.Yellow;
                }
                if (SelectedIndexConfidentialLevel > 60 && SelectedIndexConfidentialLevel <= 80)
                {
                    SelectedBackground = Colors.DeepSkyBlue;
                }
                if (SelectedIndexConfidentialLevel > 80 && SelectedIndexConfidentialLevel <= 100)
                {
                    SelectedBackground = Colors.Green;
                }

                GeosApplication.Instance.Logger.Log("Method SelectedConfidentialLevel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedConfidentialLevel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindowForSplit(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CloseWindowForSplit ...", category: Category.Info, priority: Priority.Low);

            SelectedViewIndex = 0;
            if (SelectedViewIndex != 1)
            {
                IsSplitVisible = true;
                //SplitVisible = "Visible";
                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 1)
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderOffer").ToString();
                    LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadsEditViewCloseDate").ToString();
                }
                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 10)
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderLead").ToString();
                    LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadsEditViewCloseDate").ToString();
                }
            }

            foreach (Task task in Tasks.ToList())
            {
                Tasks.Remove(task);
            }

            GeosApplication.Instance.Logger.Log("Method CloseWindowForSplit() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method for when there is any change in the Quanity of project.
        /// </summary>
        /// <param name="obj"></param>
        public void QuantityEditValueChangedAction(EditValueChangedEventArgs obj)
        {
            if (!isFromShowAll)     // If showall is checked/unchecked then do not modify any values.
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method QuantityEditValueChangedAction ...", category: Category.Info, priority: Priority.Low);

                    SpinEdit pcl = (SpinEdit)(obj.Source);

                    // This statement for if parent tag then return.
                    if (pcl.Tag == null) return;
                    if (pcl.Tag != null && pcl.Tag.ToString().Contains("Group")) return;

                    if (OptionsByOfferList.Any(option => option.IdOption == Convert.ToInt64(pcl.Tag)))
                    {
                        OptionsByOffer optionsByOffer = OptionsByOfferList.Where(x => x.IdOption == Convert.ToInt64(pcl.Tag)).SingleOrDefault();
                        optionsByOffer.Quantity = Convert.ToInt32(obj.NewValue);

                        // After change in SpinEdit value.
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());

                        if (dataRow != null && Convert.ToDouble(dataRow["Qty"]) == Convert.ToDouble(obj.NewValue))
                        {
                            dataRow["IsChecked"] = Convert.ToDouble(obj.NewValue) > 0 ? true : false; // quantity > 0 then check product
                        }
                        optionsByOffer.IsSelected = Convert.ToDouble(optionsByOffer.Quantity) > 0 ? true : false;

                        if (dataRow != null && Convert.ToDouble(dataRow["Qty"]) != Convert.ToDouble(obj.OldValue))
                        {
                            //if (Convert.ToInt64(optionsByOffer.OfferOption.IdOfferOptionType) == 1 || Convert.ToInt64(optionsByOffer.OfferOption.IdOfferOption) == 11)
                            //{ }
                            if (Convert.ToBoolean(dataRow["IsChecked"]) == true && Convert.ToDouble(dataRow["Qty"]) > 0)
                            {
                                //ChangeDescription(dataRow, optionsByOffer.Quantity);
                                ChangeDescription(dataRow, Convert.ToInt32(obj.OldValue));
                            }
                            else
                            {
                                ChangeDescription(dataRow, Convert.ToInt32(obj.OldValue));
                            }

                        }
                    }
                    else if (obj != null && Convert.ToInt32(obj.NewValue) != 0)     // If quantity is zero then donot add in list.
                    {
                        OptionsByOffer optionsByOffer = new OptionsByOffer();
                        optionsByOffer.IdOption = Convert.ToInt64(pcl.Tag);
                        optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                        optionsByOffer.Quantity = Convert.ToInt32(obj.NewValue);

                        if (optionsByOffer.IdOption != 0)
                        {
                            OptionsByOfferList.Add(optionsByOffer);
                        }

                        // After change in SpinEdit value.
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());
                        if (dataRow != null)
                        {
                            dataRow["IsChecked"] = Convert.ToDouble(obj.NewValue) > 0 ? true : false;

                            if (Convert.ToBoolean(dataRow["IsChecked"]) == true && Convert.ToDouble(dataRow["Qty"]) > 0)
                            {
                                ChangeDescription(dataRow, optionsByOffer.Quantity);
                            }
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method QuantityEditValueChangedAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in QuantityEditValueChangedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        /// <summary>
        /// Methdo for create description as per product and serviece options. 
        /// </summary>
        /// <param name="Dt"></param>
        /// <param name="Qty"></param>
        public void ChangeDescription(DataRow Dt, int? Qty)
        {
            GeosApplication.Instance.Logger.Log("Method ChangeDescription ...", category: Category.Info, priority: Priority.Low);

            string addnoplusString = "";
            if (string.IsNullOrEmpty(Description))
            {
                addnoplusString = Dt["Qty"] + " " + Dt["Name"];
                Description = addnoplusString;
            }
            string addOldString = "";
            if (addnoplusString == "")
            {
                //addOldString = Qty.ToString() + " " + Dt["Name"];

                string[] str = Description.Split(new string[] { "+" }, StringSplitOptions.None);
                addnoplusString = str[0].ToString();
            }
            if (string.IsNullOrEmpty(Description))
            {
                addOldString = Qty.ToString() + " " + Dt["Name"];
            }
            else
            {

                if (addnoplusString.Contains(Dt["Name"].ToString()))
                {
                    addOldString = Qty.ToString() + " " + Dt["Name"];
                }
                else
                {
                    addOldString = " + " + Qty.ToString() + " " + Dt["Name"];
                }

            }


            string addOldStringNoPlus = addOldString.Trim().Trim(new char[] { '+' }).Trim();
            string addNewString = addNewString = " + " + Dt["Qty"] + " " + Dt.ItemArray[0].ToString();

            if (string.IsNullOrEmpty(Description))
            {
                addnoplusString = Dt["Qty"] + " " + Dt["Name"];
                Description = addnoplusString;
            }
            else
            {

                if (Description.Contains(Dt["Name"].ToString()))
                {
                    if (Convert.ToInt32(Dt["Qty"]) == 0)
                    {
                        if (Description.Contains(Dt["Name"].ToString()))
                        {
                            if (addnoplusString.Contains(Dt["Name"].ToString()))
                            {
                                addnoplusString = "";
                            }
                        }
                        Description = Description.Replace(addOldString, "").Trim();
                        Description = Description.Replace(addOldStringNoPlus, "").Trim();
                        Dt["IsChecked"] = false;


                    }
                    else
                    {
                        Description = Description.Replace(addOldString, addNewString).Trim();

                    }
                }
                else
                {
                    if (Convert.ToInt32(Dt["Qty"]) != 0)
                    {
                        Description = Description + addNewString;
                    }
                }

                Description = Description.Trim().Trim(new char[] { '+' }).Trim();

                GeosApplication.Instance.Logger.Log("Method ChangeDescription() executed successfully", category: Category.Info, priority: Priority.Low);

            }
        }

        /// <summary>
        /// This method for when there is any change in the Quanity of project.
        /// </summary>
        /// <param name="obj"></param>
        public void SplitQuantityEditValueChangedAction(EditValueChangedEventArgs obj)
        {
            if (!isFromShowAll)     // If showall is checked/unchecked then do not modify any values.
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method SplitQuantityEditValueChangedAction ...", category: Category.Info, priority: Priority.Low);

                    SpinEdit pcl = (SpinEdit)(obj.Source);

                    // _TabIndex is using for to identify from which data is comming.
                    int _TabIndex = int.Parse(pcl.Uid);

                    // This statement for if parent tag then return.
                    if (pcl.Tag == null) return;
                    if (pcl.Tag != null && pcl.Tag.ToString().Contains("Group")) return;

                    // SpinEdit - if Value changed then Check or Uncheck on Condition.
                    DataRow dataRow = Tasks[_TabIndex].DttableSplit.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());

                    if (dataRow != null && Convert.ToDouble(dataRow["Qty"]) == Convert.ToDouble(obj.NewValue))
                    {
                        if (Convert.ToDouble(dataRow["Qty"]) == 0)  // If quantity is zero then CheckEdit is false.
                        {
                            dataRow["IsChecked"] = false;
                        }
                        else if (Convert.ToDouble(dataRow["Qty"]) > 0)  // If quantity is > 0 then CheckEdit is true.
                        {
                            dataRow["IsChecked"] = true;
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method SplitQuantityEditValueChangedAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in SplitQuantityEditValueChangedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        /// <summary>
        /// This method for when there user select any template by checkbox of project.
        /// </summary>
        /// <param name="obj"></param>

        public void SplitQuantityEditAfterCheckChangedAction(EditValueChangedEventArgs obj)
        {
            if (!isFromShowAll)     // If showall is checked/unchecked then do not modify any values.
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method SplitQuantityEditAfterCheckChangedAction ...", category: Category.Info, priority: Priority.Low);

                    // _TabIndex is using for to identify from which data is comming.
                    int _TabIndex = 0;

                    CheckEdit pcl = (CheckEdit)(obj.Source);
                    _TabIndex = int.Parse(pcl.Uid);

                    // This statement for if parent tag then return.
                    if (pcl.Tag == null) return;
                    if (pcl.Tag != null && pcl.Tag.ToString().Contains("Group")) return;

                    // SpinEdit - if Value changed then Check or Uncheck on Condition.
                    if (Tasks.Count != 0)
                    {
                        DataRow dataRow = Tasks[_TabIndex].DttableSplit.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());
                        if (dataRow != null)
                        {
                            if (!Convert.ToBoolean(dataRow["IsChecked"]))
                            {
                                dataRow["Qty"] = 0;
                            }
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method SplitQuantityEditAfterCheckChangedAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in SplitQuantityEditAfterCheckChangedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        /// 
        /// <summary>
        /// This method for when there user select any template by checkbox of project.
        /// </summary>
        /// <param name="obj"></param>
        public void QuantityEditAfterCheckChangedAction(EditValueChangedEventArgs obj)
        {
            if (!isFromShowAll)     // If showall is checked/unchecked then do not modify any values.
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method QuantityEditAfterCheckChangedAction ...", category: Category.Info, priority: Priority.Low);

                    CheckEdit pcl = (CheckEdit)(obj.Source);

                    // This statement for if parent tag then return.
                    if (pcl.Tag == null) return;
                    if (pcl.Tag != null && pcl.Tag.ToString().Contains("Group")) return;

                    // Check in existing list.
                    if (OptionsByOfferList.Any(qtdQuantity => qtdQuantity.IdOption == Convert.ToInt64(pcl.Tag)))
                    {
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());

                        if (dataRow != null && Convert.ToBoolean(dataRow["IsChecked"]) == Convert.ToBoolean(obj.NewValue))
                        {
                            OptionsByOffer optionsByOffer = OptionsByOfferList.Where(x => x.IdOption == Convert.ToInt64(pcl.Tag)).SingleOrDefault();
                            optionsByOffer.IsSelected = Convert.ToBoolean(obj.NewValue);

                            if (!optionsByOffer.IsSelected)     // If selected is false, then set quantity to zero.
                            {
                                optionsByOffer.Quantity = 0;
                                dataRow["Qty"] = Convert.ToDouble(0);       // optionsByOffer.Quantity
                            }
                        }
                    }
                    else
                    {
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());

                        if (dataRow != null && Convert.ToBoolean(dataRow["IsChecked"]) == Convert.ToBoolean(obj.NewValue))
                        {
                            OptionsByOffer optionsByOffer = new OptionsByOffer();
                            optionsByOffer.IdOption = Convert.ToInt64(pcl.Tag);
                            optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                            optionsByOffer.IsSelected = Convert.ToBoolean(obj.NewValue);
                            optionsByOffer.Quantity = 0;

                            if (optionsByOffer.IdOption != 0)  // || optionsByOffer.Quantity != 0
                            {
                                OptionsByOfferList.Add(optionsByOffer);
                            }
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method QuantityEditAfterCheckChangedAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in QuantityEditAfterCheckChangedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }


        /// <summary>
        /// Method for create change log list.
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        private void ChangeLogsDetails()
        {
            GeosApplication.Instance.Logger.Log("Method ChangeLogsDetails ...", category: Category.Info, priority: Priority.Low);
            //if (ChangeLogsEntry == null)
            ChangeLogsEntry = new List<LogEntryByOffer>();

            if (SelectedLeadList[0].IdOffer != 0)
            {
                bool isCodeSame = SelectedLeadList[0].Code.Equals(OfferCode, StringComparison.OrdinalIgnoreCase);

                if (!isCodeSame)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewOfferCodeChanged").ToString(), SelectedLeadList[0].Code, OfferCode), IdLogEntryType = 19 });
                }

                if (SelectedLeadList[0].Site.Customers[0].IdCustomer != CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer)
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCompanyGroupChanged").ToString(), SelectedLeadList[0].Site.Customers[0].CustomerName, CompanyGroupList[SelectedIndexCompanyGroup].CustomerName), IdLogEntryType = 7 });

                if (SelectedLeadList[0].Site.IdCompany != CompanyPlantList[SelectedIndexCompanyPlant].IdCompany)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDeliverySiteChanged").ToString(), SelectedLeadList[0].Site.Name, CompanyPlantList[SelectedIndexCompanyPlant].Name), IdLogEntryType = 7 });
                }

                if (!String.ReferenceEquals(SelectedLeadList[0].Description, Description))
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDescriptionChanged").ToString(), SelectedLeadList[0].Description, Description), IdLogEntryType = 9 });
                }


                if (!String.ReferenceEquals(SelectedLeadList[0].Rfq.Trim(), Rfq))
                {
                    if (string.IsNullOrEmpty(SelectedLeadList[0].Rfq.Trim()))
                    {
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRfqAdded").ToString(), Rfq), IdLogEntryType = 9 });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Rfq))
                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRfqChanged").ToString(), SelectedLeadList[0].Rfq, "None"), IdLogEntryType = 9 });
                        else
                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRfqChanged").ToString(), SelectedLeadList[0].Rfq, Rfq), IdLogEntryType = 9 });
                    }

                }

                //Sprint_60----(#69300) Price wrongly updated in offer-----sdesai
                if (SelectedLeadList[0].OfferValue != OfferAmount || SelectedLeadList[0].IdOfferCurrency != Currencies[SelectedIndexCurrency].IdCurrency)
                {
                    Currency _SelectedCurrency = Currencies[SelectedIndexCurrency];
                    Currency _Currency = Currencies.FirstOrDefault(x => x.IdCurrency == SelectedLeadList[0].IdOfferCurrency);
                    if (_Currency != null)
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewAmountChanged").ToString(), SelectedLeadList[0].OfferValue + " " + _Currency.Name, OfferAmount + " " + _SelectedCurrency.Name), IdLogEntryType = 8 });
                }

                if (SelectedLeadList[0].DeliveryDate != null && SelectedLeadList[0].DeliveryDate.Value.Date != OfferCloseDate.Value.Date)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDeliveryDateChanged").ToString(),
                        Convert.ToDateTime(SelectedLeadList[0].DeliveryDate).ToShortDateString(), Convert.ToDateTime(OfferCloseDate.Value.Date).ToShortDateString()),
                        IdLogEntryType = 7
                    });
                }

                if (SelectedLeadList[0].RFQReception == DateTime.MinValue && (RFQReceptionDate != null && RFQReceptionDate != DateTime.MinValue))
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRFQReceptionDate­­Add").ToString(),
                                             Convert.ToDateTime(RFQReceptionDate.Value.Date).ToShortDateString()),
                        IdLogEntryType = 7
                    });
                }
                else if ((RFQReceptionDate != null && RFQReceptionDate != DateTime.MinValue) && SelectedLeadList[0].RFQReception != RFQReceptionDate)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRFQReceptionDateChd").ToString(),
                        Convert.ToDateTime(SelectedLeadList[0].RFQReception).ToShortDateString(), Convert.ToDateTime(RFQReceptionDate.Value.Date).ToShortDateString()),
                        IdLogEntryType = 7
                    });
                }

                if (SelectedLeadList[0].SendIn == DateTime.MinValue && (QuoteSentDate != null && QuoteSentDate != DateTime.MinValue))
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewQuoteSentDate­­Add").ToString(),
                                             Convert.ToDateTime(QuoteSentDate.Value.Date).ToShortDateString()),
                        IdLogEntryType = 7
                    });
                }
                else if ((QuoteSentDate != null && QuoteSentDate != DateTime.MinValue) && SelectedLeadList[0].SendIn != QuoteSentDate)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewQuoteSentDateChd").ToString(),
                        Convert.ToDateTime(SelectedLeadList[0].SendIn).ToShortDateString(), Convert.ToDateTime(QuoteSentDate.Value.Date).ToShortDateString()),
                        IdLogEntryType = 7
                    });
                }

                if (SelectedLeadList[0].ProbabilityOfSuccess != Convert.ToSByte(SelectedIndexConfidentialLevel))
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewConfidenceChanged").ToString(), SelectedLeadList[0].ProbabilityOfSuccess.ToString() + " %", SelectedIndexConfidentialLevel.ToString() + " %"), IdLogEntryType = 7 });
                }

                if (SelectedLeadList[0].IdBusinessUnit == null)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewBusinessUnitNullChanged").ToString(), BusinessUnitList[SelectedIndexBusinessUnit].Value.ToString()), IdLogEntryType = 7 });
                }

                else if (SelectedLeadList[0].IdBusinessUnit != Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue))
                {
                    string businessUnitName = BusinessUnitList.Where(bu => bu.IdLookupValue == Convert.ToByte(SelectedLeadList[0].IdBusinessUnit.ToString())).Select(bun => bun.Value).SingleOrDefault();
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewBusinessUnitChanged").ToString(), businessUnitName, BusinessUnitList[SelectedIndexBusinessUnit].Value.ToString()), IdLogEntryType = 7 });
                }

                // Lead Source
                if (SelectedLeadList[0].IdSource == null)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSourceNullChanged").ToString(), LeadSourceList[SelectedIndexLeadSource].Value.ToString()), IdLogEntryType = 7 });
                }
                else if (SelectedLeadList[0].IdSource != Convert.ToByte(LeadSourceList[SelectedIndexLeadSource].IdLookupValue))
                {
                    string leadSourceName = LeadSourceList.Where(ls => ls.IdLookupValue == Convert.ToByte(SelectedLeadList[0].IdSource.ToString())).Select(bun => bun.Value).SingleOrDefault();
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSourceChanged").ToString(), leadSourceName, LeadSourceList[SelectedIndexLeadSource].Value.ToString()), IdLogEntryType = 7 });
                }

                if (SelectedLeadList[0].IdSalesOwner == null)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSalesOwnerNullChanged").ToString(), SalesOwnerList[SelectedIndexSalesOwner].FullName), IdLogEntryType = 5 });
                }

                else if (SalesOwnerList != null && SalesOwnerList.Count > 0 &&
                         SelectedLeadList[0].IdSalesOwner != SalesOwnerList[SelectedIndexSalesOwner].IdPerson)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSalesOwnerChanged").ToString(), SelectedLeadList[0].SalesOwner.FullName, SalesOwnerList[SelectedIndexSalesOwner].FullName), IdLogEntryType = 5 });
                }


                string GeosProjectName = "";
                if (SelectedIndexGeosProject == -1) //|| SelectedIndexGeosProject == 0
                {
                    if (SelectedLeadList[0].IdCarProject.HasValue)
                    {
                        GeosProjectName = GeosProjectsList.Where(pr => pr.IdCarProject == SelectedLeadList[0].IdCarProject.Value).Select(gpr => gpr.Name).SingleOrDefault();

                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewProjectChanged").ToString(), GeosProjectName, "None"), IdLogEntryType = 7 });
                    }
                }
                else if (SelectedLeadList[0].IdCarProject != GeosProjectsList[SelectedIndexGeosProject].IdCarProject)
                {
                    if (SelectedLeadList[0].IdCarProject.HasValue)
                        GeosProjectName = GeosProjectsList.Where(pr => pr.IdCarProject == SelectedLeadList[0].IdCarProject.Value).Select(gpr => gpr.Name).SingleOrDefault();
                    else
                        GeosProjectName = "None";

                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewProjectChanged").ToString(), GeosProjectName, GeosProjectsList[SelectedIndexGeosProject].Name), IdLogEntryType = 7 });
                }

                string CaroemName = "";

                if (SelectedLeadList[0].IdCarOEM == null)
                {
                    if (CaroemsList[SelectedIndexCarOEM].IdCarOEM != 0)
                    {
                        CaroemName = "None";
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCarsOemChanged").ToString(), CaroemName, CaroemsList[SelectedIndexCarOEM].Name), IdLogEntryType = 7 });
                    }
                }
                else if (SelectedLeadList[0].IdCarOEM != CaroemsList[SelectedIndexCarOEM].IdCarOEM)
                {
                    if (SelectedLeadList[0].IdCarOEM.HasValue)
                        CaroemName = CaroemsList.Where(coem => coem.IdCarOEM == SelectedLeadList[0].IdCarOEM.Value).Select(gcoem => gcoem.Name).SingleOrDefault();
                    else
                        CaroemName = "None";
                    if (CaroemsList[SelectedIndexCarOEM].Name == "---")
                    {
                        CaroemsList[SelectedIndexCarOEM].Name = "None";
                    }
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCarsOemChanged").ToString(), CaroemName, CaroemsList[SelectedIndexCarOEM].Name), IdLogEntryType = 7 });
                }

                if (SelectedLeadList[0].GeosStatus.IdOfferStatusType != SelectedGeosStatus.IdOfferStatusType)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewGeosStatusChanged").ToString(), SelectedLeadList[0].GeosStatus.Name, SelectedGeosStatus.Name), IdLogEntryType = 7 });
                }

                foreach (LogEntryByOffer item in ChangeLogCommentEntry)
                {
                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = item.IdOffer, IdUser = item.IdUser, DateTime = item.DateTime, Comments = item.Comments, IdLogEntryType = item.IdLogEntryType, IdLogEntry = item.IdLogEntry, IsDeleted = item.IsDeleted, IsUpdate = item.IsUpdate, IsRtfText = item.IsRtfText });
                }
            }

            if (SelectedLeadList[0].NewlyLinkedActivities != null)
            {
                foreach (Activity itemActivity in SelectedLeadList[0].NewlyLinkedActivities)
                {
                    LogEntryByOffer logEntry = new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddActivity").ToString(), itemActivity.LookupValue.Value, itemActivity.Subject),
                        IdLogEntryType = 18
                    };

                    ChangeLogsEntry.Add(logEntry);
                }
            }
            // [001] added
            if (SelectedIndexOfferOwner != -1)
            {
                if (SelectedLeadList[0].OfferedBy != OfferOwnerList[SelectedIndexOfferOwner].IdUser)
                {
                    User user = OfferOwnerList.Where(x => x.IdUser == SelectedLeadList[0].OfferedBy).FirstOrDefault();

                    if (user != null)
                    {
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewOfferownerLog").ToString(), user.FullName, OfferOwnerList[SelectedIndexOfferOwner].FullName), IdLogEntryType = 7 });
                    }
                    else
                    {
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewOfferownerLog").ToString(), "None", OfferOwnerList[SelectedIndexOfferOwner].FullName), IdLogEntryType = 7 });

                    }
                }
            }

            GeosApplication.Instance.Logger.Log("Method ChangeLogsDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for save Offer details.
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// [003][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [003][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// [004][GEOS2-2217][cpatil][03-04-2020]Eng. Analysis type field not working as expected
        /// </summary>
        /// <param name="obj"></param>
        private void SaveOffer(object obj)
        {
            try
            {

                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method SaveOffer ...", category: Category.Info, priority: Priority.Low);
                // ProductAndServicesCount = OptionsByOfferList.Count;

                ProductAndServicesCount = OptionsByOfferList.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList().Count();

                if (ProductAndServicesCount > 0)
                {
                    List<OptionsByOffer> TempOptionsByOfferList = OptionsByOfferList.Where(opt => opt.Quantity == 0 && opt.IsSelected == true).ToList();

                    if (TempOptionsByOfferList.Count > 0)
                    {
                        ProductAndServicesCount = 0;
                    }
                }
                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";

                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexLeadSource")); // Lead Source
                PropertyChanged(this, new PropertyChangedEventArgs("OfferAmount"));
                PropertyChanged(this, new PropertyChangedEventArgs("OfferCloseDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("RFQReceptionDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("QuoteSentDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessUnit"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSalesOwner"));
                PropertyChanged(this, new PropertyChangedEventArgs("ProductAndServicesCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsSiteResponsibleRemoved"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsStatusDisabled"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOfferOwner"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    if (SelectedLeadList[0].IsUpdateLeadToOT == true)
                    {
                        GenerateOfferCode();
                    }
                    ChangeLogsDetails();

                    OfferData = new Offer();

                    //[003] Added to create controller of service to hit offer plant service
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                    // [001] added //[003] Changed controller and service method GetOfferDetailsById_V2037 to GetOfferDetailsById_V2040
                    OfferData = CrmStartUpOfferActiveSite.GetOfferDetailsById_V2040(selectedLeadList[0].IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, selectedLeadList[0].OfferActiveSite);
                    //END
                    OfferData.IdCustomer = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                    OfferData.IdOfferType = OfferTypeList[SelectedIndexOfferType].IdOfferType;

                    if (SelectedLeadList[0].NumberOfOffers > 0)
                        OfferData.Number = SelectedLeadList[0].NumberOfOffers;

                    OfferData.Code = OfferCode;
                    OfferData.IdBusinessUnit = Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue);
                    OfferData.BusinessUnit = BusinessUnitList[SelectedIndexBusinessUnit];
                    OfferData.IdSource = Convert.ToByte(LeadSourceList[SelectedIndexLeadSource].IdLookupValue); // Lead Source
                    OfferData.Source = LeadSourceList[SelectedIndexLeadSource];
                    OfferData.OfferActiveSite = selectedLeadList[0].OfferActiveSite;//[003]Added to get offer plant detail(Idsite,Alias,ServiceProvider)
                    OfferData.Site = CompanyPlantList[SelectedIndexCompanyPlant];
                    //OfferData.Site.ConnectPlantId = selectedLeadList[0].Site.ConnectPlantId;
                    //OfferData.Site.ConnectPlantConstr = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == selectedLeadList[0].Site.ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault();

                    //Added for CreateIssue in jira
                    OfferData.OfferType = OfferTypeList[SelectedIndexOfferType];
                    offerData.JiraUserReporter = GeosApplication.Instance.ActiveUser;
                    OfferData.Site.ShortName = selectedLeadList[0].OfferActiveSite.SiteAlias;

                    OfferData.Site.Customers = new List<Customer>();
                    OfferData.Site.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);

                    OfferData.IdProject = null;

                    if (SelectedIndexGeosProject == -1)
                        OfferData.IdCarProject = null;
                    else
                    {
                        OfferData.IdCarProject = Convert.ToInt32(GeosProjectsList[SelectedIndexGeosProject].IdCarProject);
                        OfferData.CarProject = GeosProjectsList[SelectedIndexGeosProject];
                    }
                    if (SelectedIndexCarOEM == -1 || SelectedIndexCarOEM == 0)
                        OfferData.IdCarOEM = null;

                    else
                    {
                        OfferData.IdCarOEM = Convert.ToInt32(CaroemsList[SelectedIndexCarOEM].IdCarOEM);
                        OfferData.CarOEM = CaroemsList[SelectedIndexCarOEM];
                    }

                    if (selectedIndexSalesOwner == -1)
                    {
                        OfferData.IdSalesOwner = 0;
                    }
                    else if (SalesOwnerList != null && SalesOwnerList.Count > 0)
                    {
                        OfferData.IdSalesOwner = SalesOwnerList[SelectedIndexSalesOwner].IdPerson;
                    }

                    OfferData.IsUpdateLeadToOT = SelectedLeadList[0].IsUpdateLeadToOT;
                    OfferData.ProbabilityOfSuccess = Convert.ToSByte(SelectedIndexConfidentialLevel);
                    OfferData.OfferExpectedDate = OfferCloseDate;
                    if (!string.IsNullOrEmpty(Description))
                        OfferData.Description = Description.Trim();

                    if (string.IsNullOrEmpty(Rfq))
                    {
                        OfferData.Rfq = "";
                    }
                    else
                    {
                        OfferData.Rfq = Rfq.Trim();
                    }

                    OfferData.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                    OfferData.Currency = new Currency { IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency, Name = Currencies[SelectedIndexCurrency].Name };

                    OfferData.DeliveryDate = OfferCloseDate;
                    //OfferData.RFQReception = RFQReceptionDate;
                    //OfferData.SendIn = QuoteSentDate;
                    OfferData.RFQReception = RFQReceptionDate != null ? (DateTime)RFQReceptionDate : DateTime.MinValue;
                    OfferData.SendIn = QuoteSentDate != null ? (DateTime)QuoteSentDate : DateTime.MinValue;
                    OfferData.Value = OfferAmount;
                    if (GridRowHeightForAmount)
                        OfferData.OfferValue = ConvertedAmount;
                    else
                        OfferData.OfferValue = OfferAmount;
                    OfferData.GeosStatus = GeosStatusList.FirstOrDefault(x => x.Name == SelectedGeosStatus.Name); //[SelectedIndexStatus];

                    OfferData.IdStatus = OfferData.GeosStatus.IdOfferStatusType;
                    OfferData.Site.FullName = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;

                    OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    OfferData.ModifiedByUser = GeosApplication.Instance.ActiveUser;
                    OfferData.LostReasonsByOffer = SelectedLeadList[0].LostReasonsByOffer;
                    List<string> selectedOptions = new List<string>();      // selectedOptions
                    List<string> unselectedOptions = new List<string>();    // unselectedOptions
                    List<string> modifiedOptions = new List<string>();      // modifiedOptions

                    //List for save final selected Product and servies (Templates)
                    List<OptionsByOffer> FinalOptionsByOfferList = OptionsByOfferList.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList();

                    //Engineering Analysis
                    if ((IsEngAnalysis && ExistedEngineeringAnalysis != null))
                    {
                        if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                        {
                            OptionsByOffer optionsByOffer = new OptionsByOffer();
                            optionsByOffer.IdOption = 25;
                            optionsByOffer.OfferOption = MainOfferOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                            optionsByOffer.IsSelected = true;
                            optionsByOffer.Quantity = 1;
                            OptionsByOfferList.Add(optionsByOffer);
                        }

                        List<EngineeringAnalysisType> FinalEngAnalysisTypes = new List<EngineeringAnalysisType>();
                        if (SelectedLeadList[0].EngineeringAnalysis != null)
                        {
                            if(SelectedLeadList[0].EngineeringAnalysis.EngineeringAnalysisTypes!=null)
                            foreach (EngineeringAnalysisType item in SelectedLeadList[0].EngineeringAnalysis.EngineeringAnalysisTypes)
                            {
                                if (ExistedEngineeringAnalysis.EngineeringAnalysisTypes != null
                                    && !ExistedEngineeringAnalysis.EngineeringAnalysisTypes.Any(x => x.IdArticle == item.IdArticle))
                                {
                                    item.IsSelected = false;
                                    FinalEngAnalysisTypes.Add(item);
                                }
                            }
                        }

                        if (ExistedEngineeringAnalysis != null && ExistedEngineeringAnalysis.EngineeringAnalysisTypes != null)
                        {
                            foreach (EngineeringAnalysisType item in ExistedEngineeringAnalysis.EngineeringAnalysisTypes)
                            {
                                if (SelectedLeadList[0].EngineeringAnalysis == null)
                                {
                                    item.IsSelected = true;
                                    FinalEngAnalysisTypes.Add(item);
                                }
                                else if (SelectedLeadList[0].EngineeringAnalysis != null
                                     && !SelectedLeadList[0].EngineeringAnalysis.EngineeringAnalysisTypes.Any(x => x.IdArticle == item.IdArticle))
                                {
                                    item.IsSelected = true;
                                    FinalEngAnalysisTypes.Add(item);
                                }
                            }
                        }

                        OfferData.EngineeringAnalysis = ExistedEngineeringAnalysis;
                        OfferData.EngineeringAnalysis.EngineeringAnalysisTypes = FinalEngAnalysisTypes;

                        if (OfferData.EngineeringAnalysis.GUIDString != null)
                        {
                            FileUploadReturnMessage fileUploadedReturnMessage = new FileUploadReturnMessage();
                            //[002] Changed to create controller of service to hit offer plant service
                            IGeosRepositoryService GeosRepositoryServiceOfferController = new GeosRepositoryServiceController(OfferData.OfferActiveSite.SiteServiceProvider);
                            fileUploadedReturnMessage = GeosRepositoryServiceOfferController.UploaderEngineeringAnalysisZipFile(EngAnalysisAttachmentFileUploadIndicator);
                        }

                        //Created for Jira Create issue. 1-OnlyQuoted, 4-Cancelled, 17-Lost
                        if (OfferData.IdStatus == 1 || OfferData.IdStatus == 4 || OfferData.IdStatus == 17)
                        {
                            if (SelectedLeadList[0].IdStatus != OfferData.IdStatus)
                            {
                                OfferData.IsEngAnalysisONAndStatusChangedToQuoted = true;
                                OfferData.PreviousOfferStatus = SelectedLeadList[0].GeosStatus.Name;
                            }
                        }
                    }
                    else
                    {
                        if (OptionsByOfferList.Any(ps => ps.IdOption == 25))
                        {
                            OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                            opt.IsSelected = false;
                            opt.Quantity = 0;
                        }
                        //Send flag to service - true when engAnalysis Set ON to OFF.
                        if (SelectedLeadList[0].EngineeringAnalysis != null && !IsEngAnalysis)
                        {
                            OfferData.IsEngAnalysisSetONToOFF = true;
                        }
                    }

                    foreach (var itema in SelectedLeadList[0].OptionsByOffers)
                    {
                        OptionsByOffer optionByOffer = FinalOptionsByOfferList.FirstOrDefault(x => x.IdOption == itema.IdOption);
                        if (optionByOffer == null)
                        {
                            OptionsByOffer optionByOfferselected = SelectedLeadList[0].OptionsByOffers.FirstOrDefault(x => x.IdOption == itema.IdOption);
                            optionByOfferselected.IsSelected = false;
                            optionByOfferselected.Quantity = 0;
                            FinalOptionsByOfferList.Add(optionByOfferselected);
                        }
                    }

                    foreach (var item in FinalOptionsByOfferList)
                    {
                        if (item.IsSelected)
                        {
                            //  if option exist prev. list then compare quantity
                            if (SelectedLeadList[0].OptionsByOffers.Exists(x => x.IdOption == item.IdOption))
                            {
                                OptionsByOffer optionByOffer = SelectedLeadList[0].OptionsByOffers.FirstOrDefault(x => x.IdOption == item.IdOption);

                                if (optionByOffer != null && item.Quantity != optionByOffer.Quantity)
                                {
                                    modifiedOptions.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewselectedOptionModify").ToString(), item.OfferOption.Name, optionByOffer.Quantity, item.Quantity));
                                }
                            }
                            else    // if not exists means this is new option added.
                            {
                                selectedOptions.Add(string.Format("{0} ({1})", item.OfferOption.Name, item.Quantity));
                            }
                        }
                        else    // if offeroption is false means is unselected.
                        {
                            unselectedOptions.Add(string.Format("{0}", item.OfferOption.Name));
                        }
                    }

                    // Log entry for Selected Options
                    if (selectedOptions.Count > 0)
                    {
                        string selectedOptionsJoined = string.Join(", ", selectedOptions);
                        string selectedOptionsComments = string.Format("{0}{1}{2}", System.Windows.Application.Current.FindResource("LeadsEditViewOptionAddedChanged").ToString(), Environment.NewLine, selectedOptionsJoined);
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = selectedOptionsComments, IdLogEntryType = 7 });
                    }
                    // Log entry for Unselected Options
                    if (unselectedOptions.Count > 0)
                    {
                        string unselectedOptionsJoined = string.Join(", ", unselectedOptions);
                        string unselectedOptionsComments = string.Format("{0}{1}{2}", System.Windows.Application.Current.FindResource("LeadsEditViewOptionUnselectedChanged").ToString(), Environment.NewLine, unselectedOptionsJoined);
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = unselectedOptionsComments, IdLogEntryType = 7 });
                    }
                    // Log entry for Modified Options
                    if (modifiedOptions.Count > 0)
                    {
                        string modifiedOptionsJoined = string.Join(", ", modifiedOptions);
                        string modifiedOptionsComments = string.Format("{0}{1}{2}", System.Windows.Application.Current.FindResource("LeadsEditViewOptionModifyChanged").ToString(), Environment.NewLine, modifiedOptionsJoined);
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = modifiedOptionsComments, IdLogEntryType = 7 });
                    }

                    // Removed offeroption 25 from list becoz it is not editable by status other than only quoted and waiting for quote.
                    if (FinalOptionsByOfferList.Any(x => x.IdOption == 25) && SelectedGeosStatus.IdOfferStatusType != 1 && SelectedGeosStatus.IdOfferStatusType != 2)
                    {
                        FinalOptionsByOfferList.Remove(FinalOptionsByOfferList.FirstOrDefault(x => x.IdOption == 25));
                    }

                    OfferData.OptionsByOffers = FinalOptionsByOfferList;

                    try
                    {

                        #region  Old functionality

                        //if (ListAddedContact != null)
                        //{
                        //    foreach (var item in ListAddedContact)
                        //    {
                        //        if (item.IsSelected == true)
                        //        {
                        //            OfferContact offerContact = new OfferContact();
                        //            offerContact.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        //            offerContact.IdContact = item.IdPerson;
                        //            offerContact.IdOffer = OfferData.IdOffer;
                        //            offerContact.Site = new Company();
                        //            offerContact.Site.ConnectPlantId = OfferData.Site.ConnectPlantId;
                        //            offerContact.Site.ConnectPlantConstr = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == offerContact.Site.ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault();
                        //            offerContact.IsPrimaryOfferContact = 1;
                        //            bool isset = CrmStartUp.IsSetPrimaryOfferContact(offerContact);
                        //            if (IsPrimayContactChanged)
                        //            {
                        //                LogEntryByOffer contactComment = new LogEntryByOffer()
                        //                {
                        //                    IdOffer = SelectedLeadList[0].IdOffer,
                        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                    DateTime = GeosApplication.Instance.ServerDateTime,
                        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogChangeFavoriteContact").ToString(), PreviousPrimaryContact, item.FullName),
                        //                    IdLogEntryType = 13
                        //                };
                        //                ChangeLogsEntry.Add(contactComment);
                        //            }

                        //            if (IsFirstPrimaryContact)
                        //            {
                        //                LogEntryByOffer contactComment = new LogEntryByOffer()
                        //                {
                        //                    IdOffer = SelectedLeadList[0].IdOffer,
                        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                    DateTime = GeosApplication.Instance.ServerDateTime,
                        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddFavoriteContact").ToString(), item.FullName),
                        //                    IdLogEntryType = 13
                        //                };
                        //                ChangeLogsEntry.Add(contactComment);
                        //            }

                        //        }
                        //        else
                        //        {
                        //            OfferContact offerContact = new OfferContact();
                        //            offerContact.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        //            offerContact.IdContact = item.IdPerson;
                        //            offerContact.IdOffer = OfferData.IdOffer;
                        //            offerContact.Site = new Company();
                        //            offerContact.Site.ConnectPlantId = OfferData.Site.ConnectPlantId;
                        //            offerContact.Site.ConnectPlantConstr = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == offerContact.Site.ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault();
                        //            offerContact.IsPrimaryOfferContact = 0;
                        //            bool isset = CrmStartUp.IsSetPrimaryOfferContact(offerContact);
                        //        }
                        //    }

                        //    foreach (var itemListOfferContact in ListOfferContact)
                        //    {
                        //        if (ListAddedContact.Any(la => la.IdPerson == itemListOfferContact.IdContact))
                        //        {
                        //            itemListOfferContact.IsDeleted = false;
                        //            itemListOfferContact.People = null;
                        //        }
                        //        else
                        //        {
                        //            itemListOfferContact.IsDeleted = true;
                        //            if (itemListOfferContact.IsDeleted)
                        //            {

                        //                LogEntryByOffer contactComment = new LogEntryByOffer()
                        //                {
                        //                    IdOffer = SelectedLeadList[0].IdOffer,
                        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                    DateTime = GeosApplication.Instance.ServerDateTime,
                        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogRemoveContact").ToString(), itemListOfferContact.People.FullName),
                        //                    IdLogEntryType = 13
                        //                };
                        //                ChangeLogsEntry.Add(contactComment);
                        //            }
                        //            itemListOfferContact.People = null;
                        //        }
                        //    }

                        //    List<Int32> databaseidPerson = ListOfferContact.Select(loc => loc.IdContact).ToList();
                        //    List<Int32> GrididPerson = ListAddedContact.Select(locg => locg.IdPerson).ToList();
                        //    List<Int32> notcommonperson = GrididPerson.Except(databaseidPerson).ToList();
                        //    ListOfferContact = new ObservableCollection<OfferContact>(ListOfferContact.Where(loca => loca.IsDeleted != false).ToList());
                        //    foreach (var itemListOfferContacts in notcommonperson)
                        //    {
                        //        People pp = ListAddedContact.Where(ki => ki.IdPerson == itemListOfferContacts).FirstOrDefault();
                        //        OfferContact offerContact = new OfferContact();
                        //        offerContact.IdContact = itemListOfferContacts;
                        //        if (pp.IsSelected == true)
                        //        {
                        //            offerContact.IsPrimaryOfferContact = 1;
                        //            if (IsPrimayContactChanged == true && offerContact.IsPrimaryOfferContact == 1)
                        //            {
                        //                LogEntryByOffer contactComment = new LogEntryByOffer()
                        //                {
                        //                    IdOffer = SelectedLeadList[0].IdOffer,
                        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                    DateTime = GeosApplication.Instance.ServerDateTime,
                        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddFavoriteContact").ToString(), pp.FullName),
                        //                    IdLogEntryType = 13
                        //                };
                        //                ChangeLogsEntry.Add(contactComment);
                        //            }
                        //            else
                        //            {
                        //                LogEntryByOffer contactComment = new LogEntryByOffer()
                        //                {
                        //                    IdOffer = SelectedLeadList[0].IdOffer,
                        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                    DateTime = GeosApplication.Instance.ServerDateTime,
                        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddFavoriteContact").ToString(), pp.FullName),
                        //                    IdLogEntryType = 13
                        //                };
                        //                ChangeLogsEntry.Add(contactComment);
                        //            }
                        //        }
                        //        if (pp.IsSelected == false)
                        //        {
                        //            offerContact.IsPrimaryOfferContact = 0;
                        //            LogEntryByOffer contactComment = new LogEntryByOffer()
                        //            {
                        //                IdOffer = SelectedLeadList[0].IdOffer,
                        //                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                DateTime = GeosApplication.Instance.ServerDateTime,
                        //                Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddContact").ToString(), pp.FullName),
                        //                IdLogEntryType = 13
                        //            };
                        //            ChangeLogsEntry.Add(contactComment);
                        //        }
                        //        offerContact.IsDeleted = false;
                        //        offerContact.People = null;
                        //        ListOfferContact.Add(offerContact);
                        //    }

                        //    OfferData.OfferContacts = ListOfferContact.ToList();
                        //}

                        #endregion

                        //[002] added
                        listAddedOfferContact = new ObservableCollection<OfferContact>();

                        if (SelectedOfferToList != null)
                        {
                            foreach (object SelectedContact in SelectedOfferToList)
                            {
                                OfferContact contact = SelectedContact as OfferContact;

                                listAddedOfferContact.Add(contact);
                            }
                        }
                        else
                        {
                            listAddedOfferContact = null;
                        }

                        if (listAddedOfferContact != null)
                        {

                            foreach (var itemListOfferContact in ListOfferContact)
                            {

                                if (listAddedOfferContact.Any(la => la.IdContact == itemListOfferContact.IdContact))
                                {
                                    itemListOfferContact.IsDeleted = false;
                                    itemListOfferContact.People = null;
                                }
                                else
                                {
                                    itemListOfferContact.IsDeleted = true;
                                    if (itemListOfferContact.IsDeleted)
                                    {

                                        LogEntryByOffer contactComment = new LogEntryByOffer()
                                        {
                                            IdOffer = SelectedLeadList[0].IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogRemoveContact").ToString(), itemListOfferContact.People.FullName),
                                            IdLogEntryType = 13
                                        };
                                        ChangeLogsEntry.Add(contactComment);
                                    }
                                    itemListOfferContact.People = null;
                                }

                            }

                            List<Int32> databaseidPerson = ListOfferContact.Select(loc => loc.IdContact).ToList();
                            List<Int32> GrididPerson = listAddedOfferContact.Select(locg => locg.IdContact).ToList();
                            List<Int32> notcommonperson = GrididPerson.Except(databaseidPerson).ToList();
                            ListOfferContact = new ObservableCollection<OfferContact>(ListOfferContact.Where(loca => loca.IsDeleted != false).ToList());

                            foreach (var itemListOfferContacts in notcommonperson)
                            {
                                OfferContact offerContact1 = listAddedOfferContact.Where(ki => ki.People.IdPerson == itemListOfferContacts).FirstOrDefault();

                                OfferContact offerContact = new OfferContact();
                                offerContact.IdContact = itemListOfferContacts;


                                offerContact.IsPrimaryOfferContact = 0;
                                LogEntryByOffer contactComment = new LogEntryByOffer()
                                {
                                    IdOffer = SelectedLeadList[0].IdOffer,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddContact").ToString(), offerContact1.People.FullName),
                                    IdLogEntryType = 13
                                };
                                ChangeLogsEntry.Add(contactComment);

                                offerContact.IsDeleted = false;
                                offerContact.People = null;
                                ListOfferContact.Add(offerContact);
                            }

                            OfferData.OfferContacts = ListOfferContact.ToList();

                        }
                        else
                        {

                            foreach (var itemListOfferContact in ListOfferContact)
                            {

                                itemListOfferContact.IsDeleted = true;
                                if (itemListOfferContact.IsDeleted)
                                {

                                    LogEntryByOffer contactComment = new LogEntryByOffer()
                                    {
                                        IdOffer = SelectedLeadList[0].IdOffer,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        DateTime = GeosApplication.Instance.ServerDateTime,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogRemoveContact").ToString(), itemListOfferContact.People.FullName),
                                        IdLogEntryType = 13
                                    };
                                    ChangeLogsEntry.Add(contactComment);
                                }
                                itemListOfferContact.People = null;

                            }
                            OfferData.OfferContacts = ListOfferContact.ToList();
                        }

                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    if (ChangeLogsEntry != null)
                    {
                        OfferData.LogEntryByOffers = ChangeLogsEntry;
                    }
                    bool isoffersave = false;
                    //try
                    //{

                    OfferData.OfferedBy = OfferOwnerList[SelectedIndexOfferOwner].IdUser;

                    //Add existing activity in an offer.
                    OfferData.NewlyLinkedActivities = SelectedLeadList[0].NewlyLinkedActivities;

                    if (SelectedIndexGeosProject > -1 && GeosProjectsList[SelectedIndexGeosProject].IdCarProject == 0)
                    {
                        GeosProjectsList[SelectedIndexGeosProject].CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                        CarProject carProject = CrmStartUp.AddCarProjectWithCreatedBy(GeosProjectsList[SelectedIndexGeosProject]);
                        OfferData.IdCarProject = carProject.IdCarProject;
                        OfferData.CarProject = carProject;
                        GeosProjectsList[SelectedIndexGeosProject].IdCarProject = carProject.IdCarProject;

                        // Create notification and email to send to admin when project created
                        if (carProject != null && carProject.IdCarProject > 0)
                        {
                            Notification notification = new Notification();
                            notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                            notification.Title = System.Windows.Application.Current.FindResource("AddNewProjectViewTitle").ToString(); // "New Project";
                            notification.Message = string.Format(System.Windows.Application.Current.FindResource("AddNewProjectNotification").ToString(), carProject.Name, carProject.CarOEM.Name, GeosApplication.Instance.ActiveUser.FullName);
                            notification.IdModule = 5;
                            notification.Status = "Unread";
                            notification.IsNew = 1;

                            MailTemplateFormat mailTemplateFormat = new MailTemplateFormat();
                            // mailTemplateFormat.SendToUserName = "" because it is added from service. (fullname)
                            mailTemplateFormat.EmailForSection = System.Windows.Application.Current.FindResource("ActivityProject").ToString();
                            mailTemplateFormat.ProjectName = carProject.Name;
                            mailTemplateFormat.CarOEMName = carProject.CarOEM.Name;
                            mailTemplateFormat.CreatedByUserName = GeosApplication.Instance.ActiveUser.FullName;
                            notification.MailTemplateFormat = mailTemplateFormat;

                            notification = CrmStartUp.AddCommonNotification(notification);
                        }

                        ChangeLogsDetails();
                    }

                    // sleep days
                    if (OfferData.GeosStatus.IdOfferStatusType == 1)
                    {
                        // If some comment exists then Last activity date = latest comment date
                        if (ListLogComments.Count > 0)
                        {
                            int result = Nullable.Compare<DateTime>(ListLogComments[0].DateTime, SelectedLeadList[0].LastActivityDate);

                            if (result > 0)
                                OfferData.LastActivityDate = ListLogComments[0].DateTime;
                        }

                        /// If some activity exists then Last activity date = latest activity date.
                        if (LeadActivityList.Count > 0)
                        {
                            var activityDates = LeadActivityList.SelectMany(x => x.ActivityLinkedItem.Select(y => y?.CreationDate)).ToList();
                            var activityDate = activityDates.Max(x => x);
                            int result = Nullable.Compare<DateTime>(activityDate, SelectedLeadList[0].LastActivityDate);
                            if (result > 0)
                                OfferData.LastActivityDate = activityDate;
                        }

                        if (ListLogComments.Count == 0 && LeadActivityList.Count == 0)
                        {
                            OfferData.LastActivityDate = OfferData.SendIn;
                        }
                    }
                    if (SelectedCategory != null)
                    {
                        if (OfferData.IdProductCategory != SelectedCategory.IdProductCategory)
                        {

                            ProductCategory Parent;
                            if (SelectedCategory.IdParent == 0)
                            {
                                Parent = ListProductCategory.Where(x => x.IdProductCategory == SelectedCategory.IdProductCategory).FirstOrDefault();
                                OfferData.ProductCategory = SelectedCategory;
                                OfferData.ProductCategory.Category = null;
                            }
                            else
                            {
                                Parent = ListProductCategory.Where(x => x.IdProductCategory == SelectedCategory.IdParent).FirstOrDefault();
                                OfferData.ProductCategory = SelectedCategory;
                                OfferData.ProductCategory.Category = Parent;
                            }



                            OfferData.IdProductCategory = SelectedCategory.IdProductCategory;
                            OfferData.IsManualCategory = 1;
                        }
                        else
                        {
                            OfferData.IdProductCategory = SelectedCategory.IdProductCategory;
                            OfferData.IsManualCategory = 0;

                        }
                    }
                    else
                    {
                        //OfferData.ProductCategory = null;
                        OfferData.IdProductCategory = 0;
                        OfferData.IsManualCategory = 0;

                    }
                    //[002] Changed controller and service method UpdateOffer_V2037 to UpdateOffer_V2040
                    //[004] Changed controller and service method UpdateOffer_V2040 to UpdateOffer_V2041
                    Offer offerUpdated = CrmStartUpOfferActiveSite.UpdateOffer_V2041(OfferData, OfferData.OfferActiveSite.IdSite, GeosApplication.Instance.ActiveUser.IdUser);
                    isoffersave = offerUpdated.IsUpdated;

                    if (isoffersave && OfferData.IdOfferType == 1)  // && OfferData.IsUpdateLeadToOT == true)
                    {
                        EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OfferData.OfferActiveSite.IdSite));
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                        ICrmService CrmStartUpCreateFolder = new CrmServiceController(serviceurl);
                        if (OfferData.IsUpdateLeadToOT)
                        {
                            offerData.Year = GeosApplication.Instance.ServerDateTime.Year;
                        }
                        bool isCreated = CrmStartUpCreateFolder.CreateFolderOffer(OfferData, true);
                    }

                    if (!string.IsNullOrEmpty(offerUpdated.ErrorFromJira))
                    {
                        GeosApplication.Instance.Logger.Log(offerUpdated.ErrorFromJira, Category.Exception, Priority.Low);
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("JiraNotWorking").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }

                    //[Start]code for create activity as per conditions. 

                    List<ActivityTemplate> activityTemplateList = new List<ActivityTemplate>();
                    string activityMsg = "";

                    foreach (var activityTemplateTrigger in activityTemplateTriggers)
                    {
                        if (isoffersave && (selectedLeadList[0].IdStatus != SelectedGeosStatus.IdOfferStatusType) &&
                            string.Format(SelectedGeosStatus.IdOfferStatusType + "(" + SelectedGeosStatus.Name + ")") == activityTemplateTrigger.LinkedObjectFieldValue)
                        {
                            Currency selectedcurrency = Currencies.FirstOrDefault(c => c.Name == activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldType);
                            Double amount = OfferAmount * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : selectedcurrency.IdCurrency);

                            if (Operator(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionOperator, amount, Convert.ToDouble(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldValue)))
                            {
                                if (activityTemplateTrigger.ActivityTemplateTriggerCondition.IsUserConfirmationRequired == 1)
                                {
                                    foreach (var template1 in activityTemplateTrigger.ActivityTemplates)
                                    {
                                        if (activityTemplateTrigger.ActivityTemplates.Count > 1)
                                        {
                                            if (string.IsNullOrWhiteSpace(activityMsg))
                                            {
                                                activityMsg = string.Format(Application.Current.Resources["ActivityCreateMoreThenOne"].ToString());
                                                activityMsg += System.Environment.NewLine + "-" + "\"" + activityTemplateTrigger.ActivityTemplates[0].Subject + "\" " + activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value + " activity";
                                            }
                                            else
                                                activityMsg += System.Environment.NewLine + "-" + "\"" + activityTemplateTrigger.ActivityTemplates[0].Subject + "\" " + activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value + " activity";
                                        }
                                        else
                                        {
                                            activityMsg = string.Format(Application.Current.Resources["ActivityCreate"].ToString(), activityTemplateTrigger.ActivityTemplates[0].Subject, activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value);
                                        }

                                        activityTemplateList.Add(template1);
                                    }
                                }
                                else
                                {
                                    foreach (ActivityTemplate activityTemplate in activityTemplateTrigger.ActivityTemplates)
                                    {
                                        AddActivity(activityTemplate);
                                    }
                                }
                            }
                        }
                    }

                    if (activityTemplateList.Count > 0)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(activityMsg, Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            foreach (ActivityTemplate activityTemplate in activityTemplateList)
                            {
                                AddActivity(activityTemplate);
                            }
                        }
                    }


                    //[End]code for create activity as per conditions. 

                    OfferData.CarOEM = CaroemsList[SelectedIndexCarOEM];
                    OfferData.SalesOwner = SalesOwnerList[SelectedIndexSalesOwner];

                    IsBusy = false;

                    //...
                    if (isoffersave)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsMsgUpdateOfferSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsMsgUpdateOfferFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method SaveOffer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsBusy = false;
        }

        public Boolean Operator(string logic, double amount, double ConditionFieldValue)
        {
            switch (logic)
            {
                case ">": return amount > ConditionFieldValue;
                case "<": return amount < ConditionFieldValue;
                case ">=": return amount >= ConditionFieldValue;
                case "<=": return amount <= ConditionFieldValue;
                case "==": return amount == ConditionFieldValue;
                case "!=": return amount != ConditionFieldValue;
                default: return false;
            }
        }

        /// <summary>
        /// Method for add new activity for offer.
        /// </summary>
        private void AddActivity(ActivityTemplate activityTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCtivity ...", category: Category.Info, priority: Priority.Low);

                List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();
                List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();
                Activity NewActivity = new Activity();

                NewActivity.IdActivityType = activityTemplate.IdActivityType;
                NewActivity.Subject = activityTemplate.Subject;             // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                NewActivity.Description = activityTemplate.Description;     // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                NewActivity.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                NewActivity.IsCompleted = 0;
                NewActivity.IdOwner = SalesOwnerList[SelectedIndexSalesOwner].IdPerson;               // ActivityOwnerList[SelectedIndexOwner].IdUser;
                NewActivity.ActivityTags = null;
                NewActivity.IsSentMail = 0;
                NewActivity.FromDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                NewActivity.ToDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                NewActivity.IsDeleted = 0;

                // NewActivity location
                NewActivity.Location = CompanyPlantList[SelectedIndexCompanyPlant].Address;
                NewActivity.Longitude = CompanyPlantList[SelectedIndexCompanyPlant].Longitude;
                NewActivity.Latitude = CompanyPlantList[SelectedIndexCompanyPlant].Latitude;

                //Fill Account details.
                ActivityLinkedItem _ActivityLinkedItem = new ActivityLinkedItem();
                _ActivityLinkedItem.IdLinkedItemType = 42;
                _ActivityLinkedItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                _ActivityLinkedItem.IsVisible = false;
                listActivityLinkedItems.Add(_ActivityLinkedItem);

                // For Add Attendies
                List<ActivityAttendees> listattendees = new List<ActivityAttendees>();
                listattendees.Add(new ActivityAttendees() { IdUser = SalesOwnerList[SelectedIndexSalesOwner].IdPerson });
                NewActivity.ActivityAttendees = listattendees;

                //_ActivityLinkedItem.Company = new Company();
                //_ActivityLinkedItem.Company.Customers = new List<Customer>();
                //_ActivityLinkedItem.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                //_ActivityLinkedItem.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                //_ActivityLinkedItem.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                //_ActivityLinkedItem.LinkedItemType = new LookupValue();
                //_ActivityLinkedItem.LinkedItemType.IdLookupValue = 42;
                //_ActivityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                //Fill Opportunity details.
                ActivityLinkedItem _ActivityLinkedItem1 = new ActivityLinkedItem();
                _ActivityLinkedItem1.IdLinkedItemType = 44;
                _ActivityLinkedItem1.IdSite = null;
                _ActivityLinkedItem1.IdOffer = SelectedLeadList[0].IdOffer;
                _ActivityLinkedItem1.IdEmdepSite = Convert.ToInt32(SelectedLeadList[0].OfferActiveSite.IdSite);

                //_ActivityLinkedItem1 = (ActivityLinkedItem)_ActivityLinkedItem.Clone();
                //_ActivityLinkedItem1.Name = offerCode;
                //_ActivityLinkedItem1.LinkedItemType = new LookupValue();
                //_ActivityLinkedItem1.LinkedItemType.IdLookupValue = 44;
                //_ActivityLinkedItem1.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();

                listActivityLinkedItems.Add(_ActivityLinkedItem1);

                foreach (ActivityLinkedItem item in listActivityLinkedItems)
                {
                    item.ActivityLinkedItemImage = null;

                    if (item.IdLinkedItemType == 42)        //Account
                    {
                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                    }

                    else if (item.IdLinkedItemType == 44)   // Opportunity
                    {
                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Name), IdLogEntryType = 2 });
                    }
                }

                NewActivity.LogEntriesByActivity = logEntriesByActivity;
                NewActivity.ActivityLinkedItem = listActivityLinkedItems;
                NewActivity = CrmStartUp.AddActivity_V2031(NewActivity);

                GeosApplication.Instance.Logger.Log("Method AddCtivity() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for add new Add Activity For Split Opportunity.if max value is greater then 20000.
        /// </summary>
        private void AddActivityForSplitOpportunity(List<Offer> selectedOfferList, ActivityTemplate activityTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityForSplitOpportunity ...", category: Category.Info, priority: Priority.Low);

                foreach (var selecteditem in selectedOfferList)
                {
                    List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();
                    List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();
                    Activity NewActivity = new Activity();

                    NewActivity.IdActivityType = activityTemplate.IdActivityType;
                    NewActivity.Subject = activityTemplate.Subject;                         // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                    NewActivity.Description = activityTemplate.Description;                 // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                    NewActivity.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    NewActivity.IsCompleted = 0;
                    NewActivity.IdOwner = SalesOwnerList[SelectedIndexSalesOwner].IdPerson;               // ActivityOwnerList[SelectedIndexOwner].IdUser;
                    NewActivity.ActivityTags = null;
                    NewActivity.IsSentMail = 0;
                    NewActivity.FromDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                    NewActivity.ToDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);

                    NewActivity.Location = CompanyPlantList[SelectedIndexCompanyPlant].Address;
                    NewActivity.Latitude = CompanyPlantList[SelectedIndexCompanyPlant].Latitude;
                    NewActivity.Longitude = CompanyPlantList[SelectedIndexCompanyPlant].Longitude;

                    //Fill Account details.
                    ActivityLinkedItem _aliAccount = new ActivityLinkedItem();

                    _aliAccount.IdLinkedItemType = 42;
                    _aliAccount.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                    _aliAccount.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                    _aliAccount.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                    _aliAccount.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);

                    // For Add Attendies
                    List<ActivityAttendees> listattendees = new List<ActivityAttendees>();
                    listattendees.Add(new ActivityAttendees() { IdUser = SalesOwnerList[SelectedIndexSalesOwner].IdPerson });
                    NewActivity.ActivityAttendees = listattendees;


                    //_aliAccount.Company = new Company();
                    //_aliAccount.Company.Customers = new List<Customer>();
                    //_aliAccount.LinkedItemType = new LookupValue();
                    //_aliAccount.LinkedItemType.IdLookupValue = 42;
                    //_aliAccount.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                    _aliAccount.IsVisible = false;
                    listActivityLinkedItems.Add(_aliAccount);

                    //Fill Opportunity details.
                    ActivityLinkedItem _aliOpportunity = new ActivityLinkedItem();
                    _aliOpportunity.IdLinkedItemType = 44;
                    _aliOpportunity.Name = selecteditem.Code;
                    _aliOpportunity.IdSite = null;
                    _aliOpportunity.IdOffer = selecteditem.IdOffer;
                    _aliOpportunity.IdEmdepSite = Convert.ToInt32(selecteditem.OfferActiveSite.IdSite);

                    //_aliOpportunity = (ActivityLinkedItem)_aliAccount.Clone();
                    //_aliOpportunity.LinkedItemType = new LookupValue();
                    //_aliOpportunity.LinkedItemType.IdLookupValue = 44;
                    //_aliOpportunity.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();

                    listActivityLinkedItems.Add(_aliOpportunity);

                    foreach (ActivityLinkedItem item in listActivityLinkedItems)
                    {
                        item.ActivityLinkedItemImage = null;

                        if (item.IdLinkedItemType == 42)        //Account
                        {
                            logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                        }
                        else if (item.IdLinkedItemType == 44)   // Opportunity
                        {
                            logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), selecteditem.Code), IdLogEntryType = 2 });
                        }
                    }

                    NewActivity.LogEntriesByActivity = logEntriesByActivity;
                    NewActivity.ActivityLinkedItem = listActivityLinkedItems;
                    NewActivity.IsDeleted = 0;
                    NewActivity = CrmStartUp.AddActivity(NewActivity);
                    IsActivityChange = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddActivityForSplitOpportunity() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityForSplitOpportunity() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityForSplitOpportunity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityForSplitOpportunity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for add new activity.
        /// in this method first create activity from add activity form and
        /// after that create activity for other offer in background.
        /// same data as first activity. if max value is less then 20000.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShowForSplitOpportunity(List<Offer> onlyQuotedOffersList, ActivityTemplate activityTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShowForSplitOpportunity...", category: Category.Info, priority: Priority.Low);

                List<Activity> savedActivity = new List<Activity>();

                int offerIndex = 0;
                foreach (var item in onlyQuotedOffersList)
                {
                    if (offerIndex == 0)
                    {
                        offerIndex++;

                        AddActivityView addActivityView = new AddActivityView();
                        AddActivityViewModel addActivityViewModel = new AddActivityViewModel();

                        List<Activity> _ActivityList = new List<Activity>();

                        //**[Start] code for add Account Detail.

                        Activity _Activity = new Activity();
                        _Activity.ActivityLinkedItem = new List<ActivityLinkedItem>();

                        //Fill Account details.
                        ActivityLinkedItem _aliAccount = new ActivityLinkedItem();
                        _aliAccount.IdLinkedItemType = 42;
                        _aliAccount.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                        _aliAccount.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                        _aliAccount.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                        _aliAccount.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;

                        _aliAccount.LinkedItemType = new LookupValue();
                        _aliAccount.LinkedItemType.IdLookupValue = 42;
                        _aliAccount.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                        _aliAccount.IsVisible = false;
                        _Activity.ActivityLinkedItem.Add(_aliAccount);

                        //_aliAccount.Company = new Company();
                        //_aliAccount.Company.Customers = new List<Customer>();

                        //Fill Opportunity details.
                        ActivityLinkedItem _aliOpportunity = new ActivityLinkedItem();
                        _aliOpportunity.IdLinkedItemType = 44;
                        _aliOpportunity.Name = item.Code;
                        _aliOpportunity.IdSite = null;
                        _aliOpportunity.IdOffer = item.IdOffer;
                        _aliOpportunity.IdEmdepSite = Convert.ToInt32(item.OfferActiveSite.IdSite);
                        _aliOpportunity.LinkedItemType = new LookupValue();
                        _aliOpportunity.LinkedItemType.IdLookupValue = 44;
                        _aliOpportunity.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                        _aliOpportunity.IsVisible = false;
                        _Activity.ActivityLinkedItem.Add(_aliOpportunity);
                        ////_aliOpportunity = (ActivityLinkedItem)_ActivityLinkedItem.Clone();

                        _Activity.Location = CompanyPlantList[SelectedIndexCompanyPlant].Address;
                        _Activity.Latitude = CompanyPlantList[SelectedIndexCompanyPlant].Latitude;
                        _Activity.Longitude = CompanyPlantList[SelectedIndexCompanyPlant].Longitude;

                        addActivityViewModel.IsAddedFromOutSide = true;
                        addActivityViewModel.SelectedIndexCompanyGroup = SelectedIndexCompanyGroup;
                        addActivityViewModel.SelectedIndexCompanyPlant = addActivityViewModel.CompanyPlantList.IndexOf(addActivityViewModel.CompanyPlantList.FirstOrDefault(x => x.IdCompany == CompanyPlantList[SelectedIndexCompanyPlant].IdCompany));

                        _ActivityList.Add(_Activity);

                        addActivityViewModel.Init(_ActivityList);

                        if (IsActivityCreateFromSaveOffer)
                        {
                            addActivityViewModel.SelectedIndexType = addActivityViewModel.TypeList.IndexOf(tl => tl.IdLookupValue == activityTemplate.IdActivityType);
                            addActivityViewModel.Subject = activityTemplate.Subject;                // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                            addActivityViewModel.Description = activityTemplate.Description;        //string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                            addActivityViewModel.DueDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                        }

                        //**[End] code for add Account Detail.

                        EventHandler handle = delegate { addActivityView.Close(); };
                        addActivityViewModel.RequestClose += handle;
                        addActivityView.DataContext = addActivityViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        addActivityView.ShowDialog();


                        if (addActivityViewModel.IsActivitySave)
                        {
                            IsActivityChange = true;
                            foreach (Activity newActivity in addActivityViewModel.NewCreatedActivityList)
                            {
                                if (newActivity.IsCompleted == 1)
                                {
                                    newActivity.ActivityGridStatus = "Completed";
                                    newActivity.CloseDate = GeosApplication.Instance.ServerDateTime;
                                }
                                else
                                {
                                    newActivity.ActivityGridStatus = newActivity.ActivityStatus != null ? newActivity.ActivityStatus.Value : "";
                                    newActivity.CloseDate = null;
                                }

                                savedActivity.Add(newActivity);
                                ListAttachmentFinal = addActivityViewModel.ListAttachment;
                            }
                        }
                    }
                    else
                    {
                        if (savedActivity != null && savedActivity.Count > 0)
                        {
                            foreach (Activity savedActivityitem in savedActivity)
                            {
                                savedActivityitem.IdActivity = 0;

                                if (ListAttachmentFinal.Count > 0)
                                {
                                    bool isupload = UploadActivityAttachment();

                                    //**[start] this section for upload attachment.

                                    if (isupload == true)
                                    {
                                        savedActivityitem.ActivityAttachment = new List<ActivityAttachment>();
                                        savedActivityitem.ActivityAttachment = ListAttachmentFinal.ToList();
                                        savedActivityitem.GUIDString = GuidCode;

                                        foreach (var itemAttachment in ListAttachmentFinal)
                                        {
                                            itemAttachment.AttachmentImage = null;
                                        }
                                    }
                                    else
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        savedActivityitem.ActivityAttachment = new List<ActivityAttachment>();
                                    }
                                }
                                else
                                {
                                    savedActivityitem.ActivityAttachment = new List<ActivityAttachment>();
                                }

                                //**[Emdep] this section for upload attachment.

                                List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();

                                //**[start] this section for add change log.
                                foreach (ActivityLinkedItem ActivityLinkedItem in savedActivityitem.ActivityLinkedItem)
                                {
                                    ActivityLinkedItem.ActivityLinkedItemImage = null;

                                    // code for remove ownerimage for contact
                                    if (ActivityLinkedItem.IdLinkedItemType == 43 && ActivityLinkedItem.People != null)
                                        ActivityLinkedItem.People.OwnerImage = null;

                                    if (ActivityLinkedItem.IdLinkedItemType == 42)        //Account
                                    {
                                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = savedActivityitem.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), ActivityLinkedItem.Name), IdLogEntryType = 2 });
                                    }
                                    else if (ActivityLinkedItem.IdLinkedItemType == 43)   // Contact
                                    {
                                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = savedActivityitem.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddContact").ToString(), ActivityLinkedItem.Name), IdLogEntryType = 2 });
                                    }
                                    else if (ActivityLinkedItem.IdLinkedItemType == 44)   // Opportunity
                                    {
                                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = savedActivityitem.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Code), IdLogEntryType = 2 });
                                    }
                                    else if (ActivityLinkedItem.IdLinkedItemType == 45)   // Project
                                    {
                                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = savedActivityitem.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddProject").ToString(), ActivityLinkedItem.Name), IdLogEntryType = 2 });
                                    }
                                }

                                savedActivityitem.LogEntriesByActivity = logEntriesByActivity;
                                //**[End] this section for add change log.

                                ActivityLinkedItem _ActivityLinkedItem1 = new ActivityLinkedItem();
                                _ActivityLinkedItem1 = savedActivityitem.ActivityLinkedItem.FirstOrDefault(act => act.IdLinkedItemType == 44);
                                _ActivityLinkedItem1.IdOffer = item.IdOffer;
                                _ActivityLinkedItem1.Name = item.Code;
                                _ActivityLinkedItem1.IdEmdepSite = Convert.ToInt32(item.OfferActiveSite.IdSite);

                                savedActivityitem.IsDeleted = 0;
                                Activity savedNewActivity = CrmStartUp.AddActivity(savedActivityitem);
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShowForSplitOpportunity() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShowForSplitOpportunity() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShowForSplitOpportunity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShowForSplitOpportunity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Upload File
        /// </summary>
        public bool UploadActivityAttachment()
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFile() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                List<FileInfo> FileDetail = new List<FileInfo>();
                FileUploader activityAttachmentFileUploader = new FileUploader();
                activityAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = activityAttachmentFileUploader.FileUploadName;

                if (ListAttachmentFinal != null && ListAttachmentFinal.Count > 0)
                {
                    foreach (ActivityAttachment fs in ListAttachmentFinal)
                    {
                        FileInfo file = new FileInfo(fs.FilePath);
                        fs.AttachmentImage = null;
                        FileDetail.Add(file);
                    }
                    activityAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, activityAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderActivityAttachmentZipFile(activityAttachmentFileUploader);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File successfully", category: Category.Info, priority: Priority.Low);
                }
                if (fileUploadReturnMessage.IsFileUpload == true)
                {
                    isupload = true;
                    IsBusy = false;

                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
                    if (!string.IsNullOrEmpty(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    GeosApplication.Instance.Logger.Log("Method UploadFile() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    IsBusy = false;
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }

        /// <summary>
        /// Method for convert zip to byte.
        /// </summary>
        /// <param name="filesDetail"></param>
        /// <param name="GuidCode"></param>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolder\";
            if (!Directory.Exists(tempfolderPath))
            {
                System.IO.Directory.CreateDirectory(tempfolderPath);
            }
            try
            {
                GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);
                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            if (!File.Exists(tempfolderPath + ListAttachmentFinal[i].FileUploadName))
                            {
                                System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + ListAttachmentFinal[i].FileUploadName);
                            }

                            string s = tempfolderPath + ListAttachmentFinal[i].FileUploadName;
                            archive.AddFile(s, @"/");
                            ListAttachmentFinal[i].FilePath = s;
                        }

                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }

                GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On ConvertZipToByte Method" + ex.Message, category: Category.Exception, priority: Priority.Low);
                DeleteTempFolder();
                return filedetails;
            }
        }

        /// <summary>
        /// Method for delete TempFolder folders.
        /// </summary>
        private void DeleteTempFolder()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }


        /// <summary>
        /// Method for check mendatory fields to fill.
        /// </summary>
        /// <returns></returns>
        private bool CheckMendatoryField()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckMendatoryField ...", category: Category.Info, priority: Priority.Low);

                bool isError = false;
                errorList = new List<string>();
                if (SelectedIndexCompanyGroup == -1 || SelectedIndexCompanyGroup == 0)
                {
                    errorList.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsErrorMsgGroup").ToString()));
                }

                if (String.IsNullOrEmpty(Description.Trim()))
                {
                    errorList.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsErrorMsgDescription").ToString()));
                }

                if (OfferCloseDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                {
                    errorList.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsErrorMsgCloseDate").ToString()));
                }

                if (SelectedIndexBusinessUnit == 0)
                {
                    errorList.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsErrorMsgBusinessUnit").ToString()));
                }

                if (SelectedIndexSalesOwner == -1)
                {
                    errorList.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsErrorMsgSalesOwner").ToString()));
                }

                int tqt = OptionsByOfferList.Select(qt => qt.Quantity.Value).ToList().Sum();

                if (tqt < 1)
                {
                    errorList.Add(string.Format(System.Windows.Application.Current.FindResource("LeadsErrorMsgTemplate").ToString()));
                }

                if (errorList.Count > 0)
                {
                    isError = true;
                    string errorMessage = "";
                    foreach (string error in errorList)
                    {
                        if (errorMessage == "")
                            errorMessage = error;
                        else
                            errorMessage = errorMessage + "\n" + error;
                    }

                    //  CustomMessageBox.Show(errorMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method CheckMendatoryField() executed successfully", category: Category.Info, priority: Priority.Low);

                return isError;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckMendatoryField() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                return false;
            }
        }

        /// <summary>
        /// Method for fill Company group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                //IList<Customer> TempCompanyGroupList = null;

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    //TempCompanyGroupList = CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                        SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer));
                    }
                    else
                    {

                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                        SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer));
                    }
                }
                else
                {
                    //TempCompanyGroupList = CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                        SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer));
                    }
                    else
                    {

                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                        SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer));
                    }
                }

                //CompanyGroupList = new List<Customer>();
                //CompanyGroupList.Insert(0, new Customer() { CustomerName = "---" });
                //CompanyGroupList.AddRange(TempCompanyGroupList);

                //SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer);

                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillExistingActivitiesToBeLinkedToOffer()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExistingActivitiesToBeLinkedToOffer ...", category: Category.Info, priority: Priority.Low);

                //ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId)).ToList());
                if (GeosApplication.Instance.IdUserPermission == 21 && GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                    ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(salesOwnersIds, 21, "0", CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].OfferActiveSite.IdSite)).ToList());
                }
                else if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(GeosApplication.Instance.ActiveUser.IdUser.ToString(), 22, plantOwnersIds, CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].OfferActiveSite.IdSite)).ToList());
                }
                else
                {
                    ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(GeosApplication.Instance.ActiveUser.IdUser.ToString(), 20, "0", CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].OfferActiveSite.IdSite)).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method FillExistingActivitiesToBeLinkedToOffer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExistingActivitiesToBeLinkedToOffer() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExistingActivitiesToBeLinkedToOffer() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillExistingActivitiesToBeLinkedToOffer() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Company group list.
        /// </summary>
        private void FillCaroemsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCaroemsList ...", category: Category.Info, priority: Priority.Low);

                CaroemsList = CrmStartUp.GetCarOEM();
                CaroemsList.Insert(0, new CarOEM() { Name = "---" });
                SelectedIndexCarOEM = CaroemsList.FindIndex(i => i.IdCarOEM == SelectedLeadList[0].IdCarOEM);

                if (SelectedIndexCarOEM == -1)
                    SelectedIndexCarOEM = 0;

                GeosApplication.Instance.Logger.Log("Method FillCaroemsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Company group list.
        /// </summary>
        private void FillGeosProjectsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGeosProjectsList ...", category: Category.Info, priority: Priority.Low);

                GeosProjectsList = CrmStartUp.GetCarProject(0);
                //GeosProjectsListTemp = GeosProjectsList.Select(tpg => tpg.Name).ToList();
                // GeosProjectsList.Insert(0, new CarProject { IdCarProject = 0, Name = "---", IdCarOem = 0 , CarOEM=new CarOEM { IdCarOEM=0,Name="---"}});
                // GeosProjectsList.Insert(0, new CarOEM() { Name = "---" });


                if (GeosProjectsList != null && GeosProjectsList.Count > 0)
                {
                    //SelectedIndexGeosProject = GeosProjectsList.FindIndex(gp => gp.IdCustomer == CompanyGroupList[selectedIndexCompanyGroup].IdCustomer);
                    SelectedIndexGeosProject = GeosProjectsList.FindIndex(i => i.IdCarProject == SelectedLeadList[0].IdCarProject);
                }
                else
                {
                    SelectedLeadList[0].IdCarProject = null;
                    SelectedIndexGeosProject = -1;

                }

                //if (SelectedIndexGeosProject == -1)
                //    SelectedIndexGeosProject = 0;

                GeosApplication.Instance.Logger.Log("Method FillGeosProjectsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill BusinessUnit List.
        /// </summary>
        private void FillBusinessUnitList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList ...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission);
                //IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupValues(2);
                //CrmStartUp.GetLookupValues(2);
                BusinessUnitList = new List<LookupValue>();
                BusinessUnitList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                BusinessUnitList.AddRange(tempBusinessUnitList);

                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Lead Source list. // Lead Source
        /// </summary>
        private void FillLeadSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList ...", category: Category.Info, priority: Priority.Low);

                LeadSourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(4).AsEnumerable());
                LeadSourceList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                //ObservableCollection<LookupValue> TempLeadSourceList = new ObservableCollection<LookupValue>(LeadSourceList.Where(inUseOption => inUseOption.InUse == true));
                //TempLeadSourceList.Insert(0, new LookupValue() { Value = "---" });
                //LeadSourceList = TempLeadSourceList;

                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Status list.
        /// </summary>
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList ...", category: Category.Info, priority: Priority.Low);

                GeosStatusList = new ObservableCollection<GeosStatus>(CrmStartUp.GetGeosOfferStatus().AsEnumerable());

                if (ForLeadOpen)
                {
                    for (int i = GeosStatusList.Count - 1; i >= 0; i--)
                    {
                        if (GeosStatusList[i].IdOfferStatusType == 3 || GeosStatusList[i].IdOfferStatusType == 5 || GeosStatusList[i].IdOfferStatusType == 6 || GeosStatusList[i].IdOfferStatusType == 7 | GeosStatusList[i].IdOfferStatusType == 8
                            || GeosStatusList[i].IdOfferStatusType == 9 || GeosStatusList[i].IdOfferStatusType == 10 || GeosStatusList[i].IdOfferStatusType == 11 || GeosStatusList[i].IdOfferStatusType == 12 || GeosStatusList[i].IdOfferStatusType == 13 || GeosStatusList[i].IdOfferStatusType == 14)
                        {
                            //GeosStatusList.RemoveAt(i);
                            GeosStatusList[i].IsEnabled = false;
                        }
                        else
                        {
                            GeosStatusList[i].IsEnabled = true;

                        }
                    }
                }
                else
                {
                    GeosStatusList.ToList().ForEach(geosStatus => geosStatus.IsEnabled = true);
                }

                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Status list.
        /// </summary>
        private void FillStatusListSplit()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusListSplit ...", category: Category.Info, priority: Priority.Low);

                if (GeosStatusList != null)
                {
                    GeosStatusListSplit = new ObservableCollection<GeosStatus>(CrmStartUp.GetGeosOfferStatus().AsEnumerable());

                    for (int i = GeosStatusListSplit.Count - 1; i >= 0; i--)
                    {
                        if (GeosStatusListSplit[i].IdOfferStatusType == 3 || GeosStatusListSplit[i].IdOfferStatusType == 5 || GeosStatusListSplit[i].IdOfferStatusType == 6 || GeosStatusListSplit[i].IdOfferStatusType == 7 | GeosStatusListSplit[i].IdOfferStatusType == 8
                        || GeosStatusListSplit[i].IdOfferStatusType == 9 || GeosStatusListSplit[i].IdOfferStatusType == 10 || GeosStatusListSplit[i].IdOfferStatusType == 11 || GeosStatusListSplit[i].IdOfferStatusType == 12 || GeosStatusListSplit[i].IdOfferStatusType == 13
                        || GeosStatusListSplit[i].IdOfferStatusType == 14)
                        {
                            // GeosStatusListSplit.RemoveAt(i);
                            GeosStatusListSplit[i].IsEnabled = false;
                        }
                        else
                        {
                            GeosStatusListSplit[i].IsEnabled = true;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillStatusListSplit() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusListSplit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusListSplit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusListSplit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Sales Owner list with use image.
        /// </summary>
        private void FillSalesOwnerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList ...", category: Category.Info, priority: Priority.Low);

                SalesOwnerList = CrmStartUp.GetSalesOwnerBySiteId(CompanyPlantList[SelectedIndexCompanyPlant].IdCompany);
                if (!SalesOwnerList.Any(i => i.IdPerson == SelectedLeadList[0].IdSalesOwner))
                {
                    int index = SalesOwnerList.Count;
                    People people = new People();
                    PeopleDetails peopleDetails = new PeopleDetails();
                    peopleDetails = GeosApplication.Instance.PeopleList.Where(i => i.IdPerson == SelectedLeadList[0].IdSalesOwner).FirstOrDefault();
                    if (peopleDetails != null)
                    {
                        people.IdPerson = peopleDetails.IdPerson;
                        people.Name = peopleDetails.Name;
                        people.Surname = peopleDetails.Surname;
                        people.FullName = peopleDetails.FullName;
                        people.Email = peopleDetails.Email;
                        SalesOwnerList.Add(people);
                        SalesOwnerList[index].IsSiteResponsibleExist = false;
                    }

                }

                SelectedIndexSalesOwner = SalesOwnerList.FindIndex(i => i.IdPerson == SelectedLeadList[0].IdSalesOwner);

                for (int i = 0; i < SalesOwnerList.Count; i++)
                {
                    User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(SalesOwnerList[i].IdPerson));

                    try
                    {
                        UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(user.Login);
                        SalesOwnerList[i].OwnerImage = byteArrayToImage(UserProfileImageByte);
                    }
                    catch (Exception ex)
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (user.IdUserGender == 1)
                                SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else
                                SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (user.IdUserGender == 1)
                                SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else
                                SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }

                }

                SalesOwnerList = new List<People>(SalesOwnerList);

                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        /// Method for Set Comment User  image.
        /// </summary>
        private void SetUserProfileImage(ObservableCollection<LogEntryByOffer> ListLogComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in ListLogComments)
                {

                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    // byteArrayToImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method for Set Comment User  image.
        /// </summary>
        private ImageSource SetUserProfileImage()
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(GeosApplication.Instance.ActiveUser.Login);

                if (UserProfileImageByte != null)
                    UserProfileImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        else
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");

                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                    }
                }


                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return UserProfileImage;
        }

        /// <summary>
        /// method for displaying activities
        /// </summary>
        private void FillActivity()
        {
            try
            {
                LeadActivityList = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesByIdOffer_V2031(SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].OfferActiveSite.IdSite)).ToList());
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillEngineeringAnalysis(EngineeringAnalysis existingAnalysis)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysis ...", category: Category.Info, priority: Priority.Low);
                if (existingAnalysis != null)
                {
                    ExistedEngineeringAnalysis = (EngineeringAnalysis)existingAnalysis.Clone();
                    IsExistEngAnalysis = true;

                    if (GeosApplication.Instance.IsPermissionReadOnly)
                        IsEngAnalysisEnable = false;
                    else
                        IsEngAnalysisEnable = true;

                    IsEngAnalysis = true;
                    //IsEngAnalysisButtonVisible = Visibility.Visible;
                    ExistedEngineeringAnalysisDuplicate = (EngineeringAnalysis)existingAnalysis.Clone();
                }

                GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysis() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysis() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEngineeringAnalysis() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            ImageSource imgSrc = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(byteArrayIn);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                biImg.DecodePixelHeight = 10;
                biImg.DecodePixelWidth = 10;

                imgSrc = biImg as ImageSource;

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return imgSrc;
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
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        /// Method for fill Company Plant list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                List<Company> TempcompanyPlant = new List<Company>();
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    //CompanyPlantList = CrmStartUp.GetSelectedUserCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, salesOwnersIds);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                    }

                }
                else
                {
                    //CompanyPlantList = CrmStartUp.GetCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                    }

                }

                //SelectedIndexCompanyPlant = CompanyPlantList.FindIndex(i => i.IdCompany == SelectedLeadList[0].Site.IdCompany);
                // if (SelectedIndexCompanyPlant == -1)
                //     SelectedIndexCompanyPlant = 0;

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill OfferType list.
        /// </summary>
        private void FillOfferType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOfferType ...", category: Category.Info, priority: Priority.Low);

                IList<OfferType> tempOfferTypeList;
                tempOfferTypeList = CrmStartUp.GetOfferType();

                OfferTypeList = new List<OfferType>();
                foreach (OfferType ft in tempOfferTypeList)
                {
                    if (ft.IdOfferType == 1 || ft.IdOfferType == 10)
                        OfferTypeList.Add(ft);
                }

                //OfferTypeList = CrmStartUp.GetOfferType();

                if (SelectedLeadList[0].IdOfferType != null)
                    SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == SelectedLeadList[0].IdOfferType);


                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 1)
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderOffer").ToString();
                    LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadsEditViewCloseDate").ToString();
                }
                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 10)
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderLead").ToString();
                    LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadsEditViewCloseDate").ToString();
                }


                GeosApplication.Instance.Logger.Log("Method FillOfferType() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Currency list.
        /// </summary>
        private void FillCurrencyList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyList ...", category: Category.Info, priority: Priority.Low);

                Currencies = CrmStartUp.GetCurrencyByExchangeRate().ToList();    //GeosApplication.Instance.Currencies;
                SelectedIndexCurrency = Currencies.FindIndex(i => i.IdCurrency == SelectedLeadList[0].IdOfferCurrency);
                GeosApplication.Instance.Logger.Log("Method FillCurrencyList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill ConfidentialLevel List Items.
        /// </summary>
        private void FillConfidentialLevelList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillConfidentialLevelList ...", category: Category.Info, priority: Priority.Low);

                ConfidentialLevelList = new List<int>();
                ConfidentialLevelList.Add(10);
                ConfidentialLevelList.Add(20);
                ConfidentialLevelList.Add(30);
                ConfidentialLevelList.Add(40);
                ConfidentialLevelList.Add(50);
                ConfidentialLevelList.Add(60);
                ConfidentialLevelList.Add(70);
                ConfidentialLevelList.Add(80);
                ConfidentialLevelList.Add(90);
                ConfidentialLevelList.Add(100);

                GeosApplication.Instance.Logger.Log("Method FillConfidentialLevelList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillConfidentialLevelList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for show Lost,Accept and Forecasted detail and popup windows,
        /// for fill reason and close date on window if they selected on GeosStatus List.
        /// [001][skale][30-09-2019][GEOS2-1756] Add the possibility to Edit the Offer LOST Date
        /// </summary>
        private void StatusChangeAction()
        {
            try
            {
                IsBusy = true;
                IsSplitVisible = true;
                GeosApplication.Instance.Logger.Log("Method StatusChangeAction ...", category: Category.Info, priority: Priority.Low);

                if (!IsStatusChangeAction)
                {

                    if (IsInIt == false && !IsLostStatusSet)
                    {
                        if (SelectedGeosStatus != null && SelectedGeosStatus.IdOfferStatusType == 17)
                        {
                            IsBusy = true;
                            IsSplitVisible = false;
                            LostOpportunityView lostOpportunityView = new LostOpportunityView();
                            LostOpportunityViewModel lostOpportunityViewModel = new LostOpportunityViewModel();
                            EventHandler handle = delegate { lostOpportunityView.Close(); };
                            lostOpportunityViewModel.RequestClose += handle;
                            //lostOpportunityViewModel.IdOffer = SelectedLeadList[0].IdOffer;
                            lostOpportunityViewModel.Offer = SelectedLeadList;
                            lostOpportunityViewModel.InIt();
                            lostOpportunityView.DataContext = lostOpportunityViewModel;
                            IsBusy = false;
                            lostOpportunityView.ShowDialog();

                            if (!lostOpportunityViewModel.IsCancel)
                            {
                                IsLostStatusSet = true;
                                showStatusReason = string.Empty;
                                foreach (OfferLostReason item in lostOpportunityViewModel.SelectedItems)
                                {
                                    showStatusReason += item.Name.ToString() + " ";
                                }
                                //showStatusReason+= lostOpportunityViewModel.LostReasonDescription;
                                ShowStatusDescription = string.Empty;
                                ShowStatusDescription += lostOpportunityViewModel.LostReasonDescription;

                                //OfferCloseDate = GeosApplication.Instance.ServerDateTime.Date;
                                //[001] added
                                OfferCloseDate = lostOpportunityViewModel.OfferLostDate.Value.Date;

                                if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                                    GridRowHeightForRfq = true;
                                else
                                    GridRowHeightForRfq = false;

                                if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                                    GridRowHeightForQuoteSent = true;
                                else
                                    GridRowHeightForQuoteSent = false;
                            }

                            if (lostOpportunityViewModel.IsCancel && !IsLostStatusSet)
                            {
                                IsSplitVisible = true;
                                lostOpportunityViewModel.IsCancel = false;
                                SelectedGeosStatus = GeosStatusList.FirstOrDefault(gl => gl.IdOfferStatusType == TempIdOfferStatusType);
                                // SelectedIndexStatus = TempSelectedIndexStatus;
                            }
                            else
                            {
                                TempIdOfferStatusType = SelectedGeosStatus.IdOfferStatusType;
                            }
                            SelectedLeadList = lostOpportunityViewModel.Offer;
                        }
                    }
                    if (SelectedGeosStatus != null && SelectedGeosStatus.IdOfferStatusType == 17)
                    {
                        VisibilityLost = Visibility.Visible;
                        IsSplitVisible = false;
                        if (IsInIt == true)
                        {
                            List<OfferLostReason> OfferLostReasons = CrmStartUp.GetOfferLostReason();
                            if (SelectedLeadList[0].LostReasonsByOffer.IdLostReasonList != null)
                            {
                                List<string> selectedReason = new List<string>(SelectedLeadList[0].LostReasonsByOffer.IdLostReasonList.Split(';'));
                                string SelectedItems = "";
                                for (int i = 0; i < selectedReason.Count; i++)
                                {
                                    //SelectedItems.Add(OfferLostReasons.Where(n => n.IdLostReason == int.Parse(selectedReason[i].ToString())).SingleOrDefault());
                                    SelectedItems += OfferLostReasons.Where(n => n.IdLostReason == int.Parse(selectedReason[i].ToString())).Select(asdf => asdf.Name).SingleOrDefault().ToString() + " ";
                                }
                                showStatusReason += SelectedItems;
                                // showStatusReason +=" "+SelectedLeadList[0].LostReasonsByOffer.Comments;
                                ShowStatusDescription += SelectedLeadList[0].LostReasonsByOffer.Comments;
                            }
                        }
                    }

                    else
                    {
                        VisibilityLost = Visibility.Hidden;
                    }

                    if (SelectedGeosStatus != null && SelectedGeosStatus.IdOfferStatusType == 4)
                    {
                        IsSplitVisible = false;
                    }
                    VisibilityAccept = Visibility.Hidden;

                    if (SelectedGeosStatus != null && (SelectedGeosStatus.IdOfferStatusType == 1 ||
                        SelectedGeosStatus.IdOfferStatusType == 2 ||
                        SelectedGeosStatus.IdOfferStatusType == 15 ||
                        SelectedGeosStatus.IdOfferStatusType == 16))
                    {
                        VisibilityForecast = Visibility.Visible;
                    }
                    else
                    {
                        VisibilityForecast = Visibility.Hidden;
                    }
                }
                else
                {
                    //VisibilityAccept = Visibility.Visible;
                    //IsSplitVisible = false;

                    // [001][sdesai][20-12-2018][CRM-M053-15] Wrong WON info displayed in Offers

                    if (SelectedGeosStatus != null && SelectedGeosStatus.IdOfferStatusType == 17)
                    {
                        VisibilityLost = Visibility.Visible;
                        IsSplitVisible = false;
                        if (IsInIt == true)
                        {
                            List<OfferLostReason> OfferLostReasons = CrmStartUp.GetOfferLostReason();
                            if (SelectedLeadList[0].LostReasonsByOffer.IdLostReasonList != null)
                            {
                                List<string> selectedReason = new List<string>(SelectedLeadList[0].LostReasonsByOffer.IdLostReasonList.Split(';'));
                                string SelectedItems = "";
                                for (int i = 0; i < selectedReason.Count; i++)
                                {
                                    SelectedItems += OfferLostReasons.Where(n => n.IdLostReason == int.Parse(selectedReason[i].ToString())).Select(asdf => asdf.Name).SingleOrDefault().ToString() + " ";
                                }
                                showStatusReason += SelectedItems;
                                ShowStatusDescription += SelectedLeadList[0].LostReasonsByOffer.Comments;
                            }
                        }
                    }
                    else
                    {
                        VisibilityLost = Visibility.Hidden;
                    }

                    if (SelectedGeosStatus != null && SelectedGeosStatus.IdOfferStatusType == 4)
                    {
                        IsSplitVisible = false;
                    }
                    VisibilityAccept = Visibility.Hidden;
                    if (SelectedGeosStatus != null && (SelectedGeosStatus.IdOfferStatusType == 1 ||
                       SelectedGeosStatus.IdOfferStatusType == 2 ||
                       SelectedGeosStatus.IdOfferStatusType == 15 ||
                       SelectedGeosStatus.IdOfferStatusType == 16))
                    {
                        VisibilityForecast = Visibility.Visible;
                    }
                    else
                    {
                        VisibilityForecast = Visibility.Hidden;
                    }
                }


                //ONLY QUOTED
                if (SelectedGeosStatus.IdOfferStatusType == 1)
                {
                    GridRowHeightForRfq = true;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = true;

                    IsSleepDaysVisible = Visibility.Visible;
                }
                else
                {
                    IsSleepDaysVisible = Visibility.Collapsed;
                }

                //WAITING FOR QUOTE
                if (SelectedGeosStatus.IdOfferStatusType == 2)
                {

                    GridRowHeightForRfq = true;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;


                }
                //CANCELLED
                if (SelectedGeosStatus.IdOfferStatusType == 4)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;
                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;
                }
                //FORCASTED
                if (SelectedGeosStatus.IdOfferStatusType == 15)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;
                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;
                }
                //QUALIFIED
                if (SelectedGeosStatus.IdOfferStatusType == 16)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;
                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = false;
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method StatusChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in StatusChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for to create Product and Service tree list
        /// </summary>
        private void ProductAndService()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductAndService ...", category: Category.Info, priority: Priority.Low);

                Dttable = new DataTable();
                DataRow drw;
                //  Dttable.Columns.Add("Offeroptiontype", typeof(string));
                Dttable.Columns.Add("Name", typeof(string));
                Dttable.Columns.Add("idOfferOption", typeof(string));
                Dttable.Columns.Add("IdOfferOptionType", typeof(string));
                Dttable.Columns.Add("Qty", typeof(double));
                Dttable.Columns.Add("IsChecked", typeof(bool));
                Dttable.Columns.Add("SplitTabIndex", typeof(int));
                Dttable.Columns.Add("IsObsolete", typeof(int));

                MainOfferOptions = CrmStartUp.GetAllOfferOptions();

                //code for items to remove from products
                List<Int64> tempremovelist = new List<Int64>() { 6, 19, 21, 23, 25, 27 };
                offerOptions = MainOfferOptions.Where(t => !tempremovelist.Contains(t.IdOfferOption)).ToList();

                string gp;
                int i = 1;

                foreach (var offerOptionsitem in offerOptions.GroupBy(c => c.IdOfferOptionType))
                {
                    List<OfferOption> offerOptionsitems = offerOptionsitem.ToList();
                    drw = Dttable.NewRow();

                    drw["Name"] = offerOptionsitems[0].OfferOptionType.Name.ToString();
                    drw["idOfferOption"] = "Group" + i.ToString();
                    drw["IsObsolete"] = 0;
                    drw["IsChecked"] = false;
                    Dttable.Rows.Add(drw);

                    foreach (OfferOption itemofferopt in offerOptionsitems)
                    {
                        gp = "Group" + i.ToString();
                        drw = Dttable.NewRow();

                        if (itemofferopt.Name.ToString().Equals("Material") || itemofferopt.Name.ToString().Equals("Assembly"))
                        {
                            drw["Name"] = itemofferopt.Name.ToString() + ".";
                        }
                        else
                        {
                            drw["Name"] = itemofferopt.Name.ToString();
                        }

                        drw["idOfferOption"] = itemofferopt.IdOfferOption.ToString();
                        drw["IdOfferOptionType"] = gp;
                        drw["Qty"] = 0;
                        drw["IsChecked"] = false;
                        drw["SplitTabIndex"] = 0;
                        drw["IsObsolete"] = itemofferopt.IsObsolete;
                        //itemofferopt.IdOfferOptionType;
                        Dttable.Rows.Add(drw);

                        foreach (OptionsByOffer itemofferopt1 in SelectedLeadList[0].OptionsByOffers.Where(tech => tech.OfferOption.IdOfferOption == Convert.ToInt64(drw["idOfferOption"].ToString())).ToList())
                        {
                            if (Convert.ToInt64(drw["idOfferOption"].ToString()) == itemofferopt1.OfferOption.IdOfferOption)
                            {

                                if (itemofferopt1.Quantity != null)
                                {
                                    drw["Qty"] = itemofferopt1.Quantity;
                                }

                                drw["IsChecked"] = true;
                                itemofferopt1.IsSelected = true;
                            }
                        }
                    }

                    i++;
                }
                DataTable newDt = new DataTable();
                newDt = Dttable.Copy();
                newDt = newDt.AsEnumerable().Where(r => r.Field<Int32>("IsObsolete") != 1 || (r.Field<Int32>("IsObsolete") == 1 && r.Field<bool>("IsChecked") == true)).CopyToDataTable();
                Dttable = newDt;
                List<DataRow> list = Dttable.AsEnumerable().ToList();

                GeosApplication.Instance.Logger.Log("Method ProductAndService() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Warning in amounts greater than 50000 EUR.
        /// [001][skale][2019-08-04][GEOS2-239] Wrong warning message in offer popup
        /// </summary>
        /// <param name="obj"></param>
        private void OfferAmountLostFocusCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method OfferAmountLostFocusCommandAction ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;

            if (OfferAmount > 0)
            {
                Double amount = OfferAmount * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : 1);
                if (amount > Max_Value && Max_Value > 0) //[001] Added
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadsAddUpdateAmountWarning"].ToString(), Max_Value), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }
            }

            IsBusy = false;
            GeosApplication.Instance.Logger.Log("Method OfferAmountLostFocusCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Warning in amounts greater than 50000 EUR.
        /// [001][skale][2019-08-04][GEOS2-239] Wrong warning message in offer popup
        /// </summary>
        /// <param name="obj"></param>
        private void OfferAmountSplitLostFocusCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method OfferAmountSplitLostFocusCommandAction ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;

            try
            {
                if (OfferAmount > 0 && ((RoutedEventArgs)(obj)).OriginalSource is System.Windows.Controls.TextBox)
                {
                    Double OfferAmountSplit = Convert.ToDouble(((System.Windows.Controls.TextBox)(((RoutedEventArgs)(obj)).OriginalSource)).Text);
                    Double amount = OfferAmountSplit * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : 1);
                    if (amount > Max_Value && Max_Value > 0)  //[001] Added
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadsAddUpdateAmountWarning"].ToString(), Max_Value), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OfferAmountSplitLostFocusCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in method OfferAmountSplitLostFocusCommandAction()" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }

        /// <summary>
        /// This Method is used to filter used nodes. ByDefault only fields with value must be displayed.
        /// </summary>
        /// <param name="e"></param>
        private void CustomNodeFilterCommandAction(DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method CustomNodeFilterCommandAction ...", category: Category.Info, priority: Priority.Low);

            System.Data.DataRow data = ((System.Data.DataRowView)(e.Node.Content)).Row;
            e.Visible = true;

            if (!IsShowAll)
            {
                if (!string.IsNullOrEmpty(data["IdOfferOptionType"].ToString()))
                {
                    if (Convert.ToBoolean(data["IsChecked"]))
                    {
                        e.Visible = true;
                    }
                    else
                    {
                        e.Visible = false;
                    }

                    if (!Convert.ToBoolean(data["IsChecked"]))
                    {
                        string addOldString = data["Qty"].ToString() + " " + data["Name"];

                        if (Description != null)
                        {
                            if (Description.Contains("+ " + addOldString))
                            {
                                addOldString = "+ " + addOldString;
                            }
                            else if (Description.Contains(addOldString + " +"))
                            {
                                addOldString = addOldString + " +";
                            }

                            Description = Description.Replace(addOldString, "").Trim();

                            Description = Description.Trim().Trim(new char[] { '+' }).Trim();

                        }
                    }
                }
            }

            e.Handled = true;

            GeosApplication.Instance.Logger.Log("Method CustomNodeFilterCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        private void CustomNodeFilterSplitCommandAction(DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method CustomNodeFilterSplitCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                // _TabIndex is using for to identify from which tabwindow data is comming.
                int _TabIndex = 0;

                System.Data.DataRow data = ((System.Data.DataRowView)(e.Node.Content)).Row;
                _TabIndex = int.Parse(data["SplitTabIndex"].ToString());

                e.Visible = true;
                if (Tasks.Count != 0)
                {
                    if (!Tasks[_TabIndex].IsShowAllSplit)
                    {
                        if (!string.IsNullOrEmpty(data["IdOfferOptionType"].ToString()))
                        {
                            if (Convert.ToBoolean(data["IsChecked"]))
                            {
                                e.Visible = true;

                            }
                            else
                            {
                                e.Visible = false;
                            }
                        }
                    }
                }

                e.Handled = true;

                GeosApplication.Instance.Logger.Log("Method CustomNodeFilterSplitCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomNodeFilterSplitCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to expand all nodes in treelist on checked show all.
        /// </summary>
        /// <param name="obj">The treelistview</param>
        public void ShowAllCheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAllCheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

                isFromShowAll = true;

                TreeListView treeListView = (TreeListView)obj;
                treeListView.ExpandAllNodes();
                treeListView.ExpandAllNodes();
                treeListView.ExpandAllNodes();
                isFromShowAll = false;

                GeosApplication.Instance.Logger.Log("Method ShowAllCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllCheckedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to expand all nodes in treelist on checked show all.
        /// </summary>
        /// <param name="obj">The treelistview</param>
        public void ShowAllCheckedSplitCommandAction(object obj)
        {
            try
            {
                // _TabIndex is using for to identify from which data is comming.
                int _TabIndex = 0;


                foreach (DataRow dataRow in ((Emdep.Geos.Modules.Crm.ViewModels.LeadsEditViewModel.Task)((System.Windows.FrameworkElement)obj).DataContext).DttableSplit.Rows)
                {
                    _TabIndex = int.Parse(dataRow["SplitTabIndex"].ToString());
                    break;
                }

                GeosApplication.Instance.Logger.Log("Method ShowAllCheckedCommandAction ...", category: Category.Info, priority: Priority.Low);
                Tasks[_TabIndex].IsShowAllSplit = true;

                TreeListView treeListView = (TreeListView)obj;
                treeListView.ExpandAllNodes();
                treeListView.ExpandAllNodes();
                treeListView.ExpandAllNodes();

                GeosApplication.Instance.Logger.Log("Method ShowAllCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllCheckedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// This method is used to collapse all nodes in treelist on unchecked show all
        /// </summary>
        /// <param name="obj">The treelistview</param>
        public void ShowAllUncheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

                isFromShowAll = true;
                TreeListView treeListView = (TreeListView)obj;
                treeListView.CollapseAllNodes();
                treeListView.ExpandAllNodes();
                treeListView.ExpandAllNodes();
                isFromShowAll = false;

                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllUncheckedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// This method is used to collapse all nodes in treelist on unchecked show all
        /// </summary>
        /// <param name="obj">The treelistview</param>
        public void ShowAllUncheckedSplitCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedSplitCommandAction ...", category: Category.Info, priority: Priority.Low);

                // _TabIndex is using for to identify from which data is comming.
                int _TabIndex = 0;

                foreach (DataRow dataRow in ((Emdep.Geos.Modules.Crm.ViewModels.LeadsEditViewModel.Task)((System.Windows.FrameworkElement)obj).DataContext).DttableSplit.Rows)
                {
                    _TabIndex = int.Parse(dataRow["SplitTabIndex"].ToString());
                    break;
                }

                Tasks[_TabIndex].IsShowAllSplit = false;

                isFromShowAll = true;
                TreeListView treeListView = (TreeListView)obj;
                treeListView.CollapseAllNodes();
                treeListView.ExpandAllNodes();
                treeListView.ExpandAllNodes();
                isFromShowAll = false;

                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedSplitCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllUncheckedSplitCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add Comment
        /// </summary>
        /// <param name="leadComment"></param>
        public void AddCommentCommandAction(object gcComments)
        {
            GeosApplication.Instance.Logger.Log("Method AddCommentCommandAction ...", category: Category.Info, priority: Priority.Low);
            //IsBusy = true;
            var document = ((RichTextBox)gcComments).Document;
            LeadComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
            string s = string.Empty;
            if (!string.IsNullOrEmpty(LeadComment.Trim()))
            {
                if (IsRtf)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Rtf);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            s = sr.ReadToEnd();
                        }
                    }
                }
                else if (IsNormal)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Text);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            s = sr.ReadToEnd();
                        }
                    }
                }
            }
            LeadComment = s;
            if (OldLeadComment != null && !string.IsNullOrEmpty(OldLeadComment.Trim()) && OldLeadComment.Equals(LeadComment.Trim()))
            {
                ShowCommentsFlyout = false;
                return;
            }

            // Update Comment.
            if (!string.IsNullOrEmpty(OldLeadComment) && !string.IsNullOrEmpty(LeadComment.Trim()))
            {
                LogEntryByOffer comment = ListLogComments.FirstOrDefault(x => x.Comments == OldLeadComment);

                int xc = ListLogComments.IndexOf(ListLogComments.FirstOrDefault(x => x.Comments == OldLeadComment));

                LogEntryByOffer UpdateComment = new LogEntryByOffer();
                UpdateComment.IsUpdate = true;
                UpdateComment.IsDeleted = false;
                UpdateComment.IdOffer = comment.IdOffer;
                UpdateComment.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                UpdateComment.People = comment.People;
                // new People { Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName };
                // UpdateComment.People.FullName = GeosApplication.Instance.ActiveUser.FullName.ToString();
                UpdateComment.DateTime = GeosApplication.Instance.ServerDateTime;
                UpdateComment.Comments = string.Copy(LeadComment);
                UpdateComment.IdLogEntry = comment.IdLogEntry;
                UpdateComment.IdLogEntryType = comment.IdLogEntryType;
                UpdateComment.IsRtfText = comment.IsRtfText;
                if (IsRtf)
                    UpdateComment.IsRtfText = true;
                else if (IsNormal)
                    UpdateComment.IsRtfText = false;

                ListLogComments.RemoveAt(xc);
                ListLogComments.Insert(xc, UpdateComment);

                if (ChangeLogCommentEntry.Exists(x => x.Comments == OldLeadComment))
                {
                    LogEntryByOffer temp = ChangeLogCommentEntry.FirstOrDefault(x => x.Comments == OldLeadComment);
                    temp.Comments = string.Copy(LeadComment);
                    temp.IsUpdate = (temp.IdLogEntry > 1) ? true : false;
                }
                else
                {
                    ChangeLogCommentEntry.Add(UpdateComment);
                }

                SelectedComment = UpdateComment;
                IsCommentChange = true;
                OldLeadComment = null;
                LeadComment = null;
            }
            else if (!string.IsNullOrEmpty(LeadComment)) // Add Comment
            {
                if (IsRtf)
                {
                    LogEntryByOffer comment = new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        People = new People { Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Copy(LeadComment),
                        IdLogEntry = 1,
                        IdLogEntryType = 1,
                        IsRtfText = true
                    };
                    if (comment.IdLogEntry > 0)
                    {
                        comment.People.FullName = GeosApplication.Instance.ActiveUser.FullName.ToString();
                        comment.People.OwnerImage = SetUserProfileImage();
                        ListLogComments.Add(comment);
                        //SetUserProfileImage(ListLogComments);
                        ChangeLogCommentEntry.Add(comment);
                        SelectedComment = comment;
                    }
                }
                else if (IsNormal)
                {
                    LogEntryByOffer comment = new LogEntryByOffer()
                    {
                        IdOffer = SelectedLeadList[0].IdOffer,
                        People = new People { Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Copy(LeadComment),
                        IdLogEntry = 1,
                        IdLogEntryType = 1,
                        IsRtfText = false,

                    };
                    if (comment.IdLogEntry > 0)
                    {
                        comment.People.FullName = GeosApplication.Instance.ActiveUser.FullName.ToString();
                        comment.People.OwnerImage = SetUserProfileImage();
                        ListLogComments.Add(comment);
                        //SetUserProfileImage(ListLogComments);
                        ChangeLogCommentEntry.Add(comment);
                        SelectedComment = comment;
                    }
                }


                //SelectedComment = LeadComment;
                LeadComment = null;
                IsCommentChange = true;
            }
            document.Blocks.Clear();
            ShowCommentsFlyout = false;
            //((GridControl)gcComments).Focus();
            LeadComment = "";
            IsRtf = false;
            IsNormal = true;
            //IsBusy = false;

            GeosApplication.Instance.Logger.Log("Method AddCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Deleting Comments
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteCommentCommandAction(object parameter)
        {
            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            LogEntryByOffer commentObject = (LogEntryByOffer)parameter;

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (commentObject.IdUser != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show("Not Allowed to delete Comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteComment").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ListLogComments != null)
                {
                    foreach (LogEntryByOffer item in ListLogComments.ToList())
                    {
                        if (item.Comments == commentObject.Comments)
                        {
                            item.IsDeleted = true;
                            ListLogComments.Remove((LogEntryByOffer)commentObject);
                        }

                        if (item.IsDeleted == true)
                            ChangeLogCommentEntry.Add(new LogEntryByOffer() { IdOffer = item.IdOffer, IdUser = item.IdUser, DateTime = item.DateTime, Comments = item.Comments, IdLogEntryType = 1, IdLogEntry = item.IdLogEntry, IsDeleted = item.IsDeleted });
                    }
                }
            }

            ShowCommentsFlyout = false;
            LeadComment = "";

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Show Lost Opportunity Window
        /// [001][skale][30-09-2019][GEOS2-1756] Add the possibility to Edit the Offer LOST Date
        /// </summary>
        /// <param name="obj"></param>
        private void ShowLostOpportunityWindow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShowLostOpportunityWindow ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            LostOpportunityView lostOpportunityView = new LostOpportunityView();
            LostOpportunityViewModel lostOpportunityViewModel = new LostOpportunityViewModel();
            EventHandler handle = delegate { lostOpportunityView.Close(); };
            lostOpportunityViewModel.RequestClose += handle;
            //lostOpportunityViewModel.IdOffer = SelectedLeadList[0].IdOffer;
            lostOpportunityViewModel.Offer = SelectedLeadList;
            //[001] added
            lostOpportunityViewModel.OfferCloseDate = OfferCloseDate;

            lostOpportunityViewModel.InIt();
            lostOpportunityView.DataContext = lostOpportunityViewModel;
            IsBusy = false;
            lostOpportunityView.ShowDialog();

            if (!lostOpportunityViewModel.IsCancel)
            {
                IsLostStatusSet = true;
                showStatusReason = string.Empty;
                foreach (OfferLostReason item in lostOpportunityViewModel.SelectedItems)
                {
                    showStatusReason += item.Name.ToString() + " ";
                }
                //showStatusReason+= lostOpportunityViewModel.LostReasonDescription;
                ShowStatusDescription = string.Empty;
                ShowStatusDescription += lostOpportunityViewModel.LostReasonDescription;
                OfferCloseDate = lostOpportunityViewModel.OfferLostDate;
            }

            GeosApplication.Instance.Logger.Log("Method ShowLostOpportunityWindow() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CommentButtonCheckedCommandAction(object obj)
        {
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            LeadComment = "";
            OldLeadComment = "";
        }

        private void CommentButtonUncheckedCommandAction(object obj)
        {
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            LeadComment = "";
            OldLeadComment = "";
            IsRtf = false;
            IsNormal = true;
        }
        /// <summary>
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        /// <param name="obj"></param>
        private void EditShipmentAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditShipmentAction ...", category: Category.Info, priority: Priority.Low);

            if (obj == null) return;
            Shipment ship = (Shipment)obj;

            try
            {

                ListBox = new List<PackingBox>();
                //[002] Added to create controller of service to hit offer plant service
                ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                //[002] Changed controller and service method GetAllPackingBoxesByShipmentId to GetAllPackingBoxesByShipmentId_V2040
                ListBox = CrmStartUpOfferActiveSite.GetAllPackingBoxesByShipmentId_V2040(ship.IdShipment);
                if (ListBox.Count > 0)
                {
                    IsBoxControlEnable = true;
                    string LWH = string.Empty;
                    int i = 0;
                    foreach (PackingBox item in ListBox)
                    {
                        double length = item.Length;
                        double width = item.Width;
                        double height = item.Height;
                        LWH = length + " x " + width + " x " + height;
                        ListBox[i].PackingBoxDimension = LWH;
                        i++;
                    }

                }

                GeosApplication.Instance.Logger.Log("Method EditShipmentAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in EditShipmentAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in EditShipmentAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditShipmentAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditCommentAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditCommentAction ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (obj == null) return;

            LogEntryByOffer commentOffer = (LogEntryByOffer)obj;

            if (commentOffer.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString();
                IsAdd = false;
                ShowCommentsFlyout = true;
                OldLeadComment = String.Copy(commentOffer.Comments);
                LeadComment = String.Copy(commentOffer.Comments);

                if (commentOffer.IsRtfText == true)
                    IsRtf = true;
                else
                    IsNormal = true;

            }
            else
            {
                LeadComment = null;
                OldLeadComment = null;
                ShowCommentsFlyout = false;
                CustomMessageBox.Show("Not Allowed to update comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.Logger.Log("Method EditCommentAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This Method for Add or Remove Contact in offer through check uncheck
        /// </summary>
        /// <param name="e"></param>
        //public void GetSalesContactCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method GetSalesContactCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    if (obj is People)
        //    {
        //        People people = obj as People;
        //        //UserProfileImageByte = null;

        //        //User user = WorkbenchStartUp.GetUserById(people.IdPerson);
        //        //try
        //        //{
        //        //    if (user != null)
        //        //        UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
        //        //}
        //        //catch (FaultException<ServiceException> ex)
        //        //{
        //        //    if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
        //        //    {
        //        //        if (people != null && people.IdPersonGender == 1)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
        //        //        else if (people != null && people.IdPersonGender == 2)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
        //        //    }
        //        //    else
        //        //    {
        //        //        if (people != null && people.IdPersonGender == 1)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
        //        //        else if (people != null && people.IdPersonGender == 2)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
        //        //    }

        //        //}
        //        //catch (ServiceUnexceptedException ex)
        //        //{
        //        //    if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
        //        //    {
        //        //        if (people != null && people.IdPersonGender == 1)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
        //        //        else if (people != null && people.IdPersonGender == 2)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
        //        //    }
        //        //    else
        //        //    {
        //        //        if (people != null && people.IdPersonGender == 1)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
        //        //        else if (people != null && people.IdPersonGender == 2)
        //        //            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
        //        //    }
        //        //}

        //        //if (UserProfileImageByte != null)
        //        //{
        //        //    people.OwnerImage = byteArrayToImage(UserProfileImageByte);
        //        //}
        //        if (!string.IsNullOrEmpty(people.ImageText))
        //        {
        //            byte[] imageBytes = Convert.FromBase64String(people.ImageText);
        //            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        //            ms.Write(imageBytes, 0, imageBytes.Length);
        //            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        //            people.OwnerImage = byteArrayToImage(imageBytes);
        //        }
        //        else
        //        {
        //            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
        //            {
        //                if (people != null && people.IdPersonGender == 1)
        //                    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
        //                else if (people != null && people.IdPersonGender == 2)
        //                    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
        //            }
        //            else
        //            {
        //                if (people != null && people.IdPersonGender == 1)
        //                    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
        //                else if (people != null && people.IdPersonGender == 2)
        //                    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
        //            }
        //        }
        //        ListAddedContact.Add(people);
        //        ListCustomerContact.Remove(people);
        //    }

        //    GeosApplication.Instance.Logger.Log("Method GetSalesContactCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        /// <summary>
        /// Method to delete linkeditem from linked items.
        /// </summary>
        /// <param name="obj"></param>
        //private void AssignedSalesCancelCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method AssignedSalesCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        if (obj is People)
        //        {

        //            People people = obj as People;
        //            People peopleRemoved = new People();

        //            if (ListAddedContact != null && ListAddedContact.Count > 0)
        //            {
        //                foreach (var item in ListAddedContact)
        //                {
        //                    if (item.IdPerson == people.IdPerson)
        //                    {
        //                        if (item.IsSelected == true)
        //                        {
        //                            item.IsSelected = false;
        //                            PrimaryOfferContact = null;
        //                        }
        //                        peopleRemoved = item;
        //                    }
        //                }

        //                if (peopleRemoved.IdPerson != 0)
        //                {
        //                    ListAddedContact.Remove(peopleRemoved);
        //                    ListCustomerContact.Add(peopleRemoved);
        //                }
        //            }

        //            GeosApplication.Instance.Logger.Log("Method AssignedSalesCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in AssignedSalesCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// This Method is for set a sales responsible
        /// </summary>
        /// <param name="obj"></param>
        //public void SetSalesResponsibleCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method SetSalesResponsibleCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        People data = obj as People;
        //        if (ListAddedContact != null && ListAddedContact.Any(x => x.IdPerson == data.IdPerson))
        //        {
        //            foreach (var item in ListAddedContact)
        //            {
        //                if (item.IdPerson == data.IdPerson)
        //                {
        //                    // Added PrimaryOfferContact is null as we need this when changing the Primary Contact.
        //                    if (PrimaryOfferContact == null)
        //                    {
        //                        PrimaryOfferContact = new OfferContact();
        //                        IsFirstPrimaryContact = true;
        //                    }

        //                    item.IsSelected = true;
        //                    PrimaryOfferContact.People = item;
        //                }
        //                else
        //                {
        //                    if (item.IsSelected)
        //                    {
        //                        IsPrimayContactChanged = true;
        //                        item.IsSelected = false;
        //                        PreviousPrimaryContact = item.FullName;
        //                    }
        //                }
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method SetSalesResponsibleCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in SetSalesResponsibleCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void FillOfferContactList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillOfferContactList ...", category: Category.Info, priority: Priority.Low);

        //        ListCustomerContact = new ObservableCollection<People>(CrmStartUp.GetContactsOfSiteByOfferId(IdSite).AsEnumerable().OrderBy(x => x.FullName));
        //        ListOfferContact = new ObservableCollection<OfferContact>(CrmStartUp.GetOfferContact(SelectedLeadList[0].IdOffer, GeosApplication.Instance.CompanyList.Where(fgt => fgt.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId.ToString()).Select(fgt => fgt.ConnectPlantConstr).FirstOrDefault()).AsEnumerable());

        //        PrimaryOfferContact = ListOfferContact.FirstOrDefault(x => x.IsPrimaryOfferContact == 1);
        //        for (int i = 0; i < ListOfferContact.Count; i++)
        //        {
        //            //User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(ListOfferContact[i].People.IdPerson));
        //            //if (user != null)
        //            //{
        //            //    try
        //            //    {
        //            //        UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(user.Login);
        //            //        ListOfferContact[i].People.OwnerImage = byteArrayToImage(UserProfileImageByte);
        //            //    }
        //            //    catch (Exception ex)
        //            //    {
        //            //        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
        //            //        {
        //            //            if (user.IdUserGender == 1)
        //            //                ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
        //            //            else
        //            //                ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
        //            //        }
        //            //        else
        //            //        {
        //            //            if (user.IdUserGender == 1)
        //            //                ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
        //            //            else
        //            //                ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
        //            //        }
        //            //    }
        //            //}
        //            if (!string.IsNullOrEmpty(ListOfferContact[i].People.ImageText))
        //            {
        //                byte[] imageBytes = Convert.FromBase64String(ListOfferContact[i].People.ImageText);
        //                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        //                ms.Write(imageBytes, 0, imageBytes.Length);
        //                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        //                ListOfferContact[i].People.OwnerImage = byteArrayToImage(imageBytes);
        //            }
        //            else
        //            {
        //                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
        //                {
        //                    if (ListOfferContact[i].People.IdPersonGender == 1)
        //                        ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
        //                    else
        //                        ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
        //                }
        //                else
        //                {
        //                    if (ListOfferContact[i].People.IdPersonGender == 1)
        //                        ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
        //                    else
        //                        ListOfferContact[i].People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
        //                }
        //            }

        //            foreach (People item in ListCustomerContact)
        //            {
        //                if (item.IdPerson == ListOfferContact[i].IdContact)
        //                {
        //                    if (ListOfferContact[i].IsPrimaryOfferContact == 1)
        //                    {
        //                        ListOfferContact[i].People.IsSelected = true;
        //                    }
        //                    ListAddedContact.Add(ListOfferContact[i].People);
        //                }
        //            }

        //            if (ListAddedContact != null && ListAddedContact.Count > 0)
        //            {
        //                foreach (People item in ListAddedContact)
        //                {
        //                    People people = listCustomerContact.SingleOrDefault(lcc => lcc.IdPerson == item.IdPerson);
        //                    ListCustomerContact.Remove(people);
        //                }
        //            }

        //            //Old code.......................
        //            //foreach (People item in ListCustomerContact)
        //            //{
        //            //    if (item.IdPerson == ListOfferContact[i].IdContact)
        //            //    {
        //            //        if (ListOfferContact[i].IsPrimaryOfferContact == 1)
        //            //        {
        //            //            ListOfferContact[i].People.IsSelected = true;
        //            //        }
        //            //        ListAddedContact.Add(ListOfferContact[i].People);
        //            //    }
        //            //}
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillOfferContactList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOfferContactList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOfferContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOfferContactList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// This method is for set primary contact through context menu
        /// </summary>
        /// <param name="obj"></param>
        public void SetCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SetCommandAction ...", category: Category.Info, priority: Priority.Low);

            People data = (People)((DevExpress.Xpf.Grid.RowData)(obj)).Row;
            if (ListAddedContact != null && ListAddedContact.Any(x => x.IdPerson == data.IdPerson))
            {
                foreach (var item in ListAddedContact)
                {
                    if (item.IdPerson == data.IdPerson)
                    {
                        // Added PrimaryOfferContact is null as we need this when changing the Primary Contact.
                        if (PrimaryOfferContact == null)
                        {
                            PrimaryOfferContact = new OfferContact();
                            IsFirstPrimaryContact = true;
                        }
                        PrimaryOfferContact.People = item;
                        item.IsSelected = true;


                    }
                    else
                    {
                        if (item.IsSelected)
                        {
                            IsPrimayContactChanged = true;
                            PreviousPrimaryContact = item.FullName;
                        }
                        item.IsSelected = false;
                    }
                }
            }

            GeosApplication.Instance.Logger.Log("Method SetCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for open MailTo in Outlook for send Email. 
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        
        private void SplitLeadOfferViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow ...", category: Category.Info, priority: Priority.Low);

            if (split)
            {
                AddNewTaskCommandAction(obj);
                split = false;
            }
            else
            {
                IsBusy = true;
                SplitOtView SplitOtView = new SplitOtView();
                InItLeadSplit(SelectedLeadList, 0);




                SelectedViewIndex = 1;
                if (SelectedViewIndex == 1)
                {
                    IsSplitVisible = false;
                    //SplitVisible = "Hidden";
                }
                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 1)
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderOfferSplit").ToString();
                }
                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 10)
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderLeadSplit").ToString();
                }


                split = true;
                IsBusy = false;
            }
            GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This Method for add new project.
        /// </summary>
        public void AddNewProjectCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddNewProjectCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (!IsControlEnable)
            {
                List<CarProject> projectNameList = new List<CarProject>();
                projectNameList = GeosProjectsList.ToList();
                AddNewProjectViewModel addNewProjectViewModel = new AddNewProjectViewModel();
                AddNewProjectView addNewProjectView = new AddNewProjectView();

                EventHandler handle = delegate { addNewProjectView.Close(); };
                addNewProjectViewModel.RequestClose += handle;

                addNewProjectViewModel.GeosProjectsList = GeosProjectsList;
                addNewProjectViewModel.ProjectNameList = projectNameList;
                addNewProjectView.DataContext = addNewProjectViewModel;
                addNewProjectView.ShowDialogWindow();

                if (addNewProjectViewModel.IsSave)
                {
                    addNewProjectViewModel.NewGeosProject.IdCustomer = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;

                    List<CarProject> TempCaroemsList = new List<CarProject>();
                    if (GeosProjectsList != null)
                    {
                        TempCaroemsList = GeosProjectsList;
                    }
                    TempCaroemsList.Add(addNewProjectViewModel.NewGeosProject);


                    GeosProjectsList = new List<CarProject>(TempCaroemsList);
                    GeosProjectsList = GeosProjectsList.OrderByDescending(ap => ap.CreationDate).ToList();

                    SelectedIndexGeosProject = GeosProjectsList.FindIndex(gp => gp.Name == addNewProjectViewModel.NewGeosProject.Name);
                }

            }
            GeosApplication.Instance.Logger.Log("Method AddNewProjectCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method to change Converted amount as per Offer amount 
        /// </summary>
        /// <param name="obj"></param>
        private void OnTextEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                if (SelectedLeadList[0].IdCurrency == Currencies[SelectedIndexCurrency].IdCurrency)
                    GridRowHeightForAmount = false;
                else
                    GridRowHeightForAmount = true;

                if (GridRowHeightForAmount)
                {
                    DateTime? _OfferCloseDate = OfferCloseDate;

                    if (SelectedLeadList[0].DeliveryDate == null)
                        _OfferCloseDate = null;
                    if (OfferCloseDate >= GeosApplication.Instance.ServerDateTime)
                        _OfferCloseDate = OfferCloseDate;

                    DateTime? _QuoteSentDate = QuoteSentDate;
                    DateTime? _RFQReceptionDate = RFQReceptionDate;
                    DateTime? _CreatedIn = SelectedLeadList[0].CreatedIn;
                    DateTime? _POReceivedInDate = SelectedLeadList[0].POReceivedInDate;
                    if (QuoteSentDate <= DateTime.MinValue || RFQReceptionDate <= DateTime.MinValue || SelectedLeadList[0].CreatedIn <= DateTime.MinValue || SelectedLeadList[0].POReceivedInDate <= DateTime.MinValue)
                    {
                        if (QuoteSentDate <= DateTime.MinValue)
                            _QuoteSentDate = null;
                        if (RFQReceptionDate <= DateTime.MinValue)
                            _RFQReceptionDate = null;
                        if (SelectedLeadList[0].CreatedIn <= DateTime.MinValue)
                            _CreatedIn = null;
                        if (SelectedLeadList[0].POReceivedInDate <= DateTime.MinValue)
                            _POReceivedInDate = null;

                    }
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                    ConvertedAmount = CrmStartUpOfferActiveSite.GetOfferAmountByCurrencyConversion(SelectedLeadList[0].IdOffer, OfferAmount, Currencies[SelectedIndexCurrency].IdCurrency, SelectedLeadList[0].Currency.IdCurrency, _OfferCloseDate, _QuoteSentDate, _RFQReceptionDate, _CreatedIn, _POReceivedInDate);
                    System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                                  where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                                  let region = new System.Globalization.RegionInfo(culture.LCID)
                                                                  where String.Equals(region.ISOCurrencySymbol, SelectedLeadList[0].Currency.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                  select region).First();
                    ConvertedOfferAmount = ConvertedAmount + " " + regionInfo.CurrencySymbol;

                }
                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OnTextEditValueChanging() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to change converted amount as per selected currency
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedIndexChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                //  ConvertedOfferAmount = CrmStartUp.GetOfferAmountByCurrencyConversion(SelectedLeadList[0].IdOffer, OfferAmount, currencies[SelectedIndexCurrency].IdCurrency, currencies[ConvertedIndexCurrency].IdCurrency);
                if (SelectedLeadList[0].IdCurrency == Currencies[SelectedIndexCurrency].IdCurrency)
                    GridRowHeightForAmount = false;
                else
                    GridRowHeightForAmount = true;

                if (GridRowHeightForAmount)
                {
                    DateTime? _OfferCloseDate = OfferCloseDate;

                    if (SelectedLeadList[0].DeliveryDate == null)
                        _OfferCloseDate = null;
                    if (OfferCloseDate >= GeosApplication.Instance.ServerDateTime)
                        _OfferCloseDate = OfferCloseDate;
                    DateTime? _QuoteSentDate = QuoteSentDate;
                    DateTime? _RFQReceptionDate = RFQReceptionDate;
                    DateTime? _CreatedIn = SelectedLeadList[0].CreatedIn;
                    DateTime? _POReceivedInDate = SelectedLeadList[0].POReceivedInDate;
                    if (QuoteSentDate <= DateTime.MinValue || RFQReceptionDate <= DateTime.MinValue || SelectedLeadList[0].CreatedIn <= DateTime.MinValue || SelectedLeadList[0].POReceivedInDate <= DateTime.MinValue)
                    {
                        if (QuoteSentDate <= DateTime.MinValue)
                            _QuoteSentDate = null;
                        if (RFQReceptionDate <= DateTime.MinValue)
                            _RFQReceptionDate = null;
                        if (SelectedLeadList[0].CreatedIn <= DateTime.MinValue)
                            _CreatedIn = null;
                        if (SelectedLeadList[0].POReceivedInDate <= DateTime.MinValue)
                            _POReceivedInDate = null;
                    }
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                    ConvertedAmount = CrmStartUpOfferActiveSite.GetOfferAmountByCurrencyConversion(SelectedLeadList[0].IdOffer, OfferAmount, Currencies[SelectedIndexCurrency].IdCurrency, SelectedLeadList[0].Currency.IdCurrency, _OfferCloseDate, _QuoteSentDate, _RFQReceptionDate, _CreatedIn, _POReceivedInDate);
                    System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                                  where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                                  let region = new System.Globalization.RegionInfo(culture.LCID)
                                                                  where String.Equals(region.ISOCurrencySymbol, SelectedLeadList[0].Currency.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                  select region).First();
                    ConvertedOfferAmount = ConvertedAmount + " " + regionInfo.CurrencySymbol;
                }


                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDateEditValueChangingCommandAction(EditValueChangedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnCloseDateEditValueChangingCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (SelectedLeadList[0].IdCurrency == Currencies[SelectedIndexCurrency].IdCurrency)
                    GridRowHeightForAmount = false;
                else
                    GridRowHeightForAmount = true;

                if (GridRowHeightForAmount)
                {
                    DateTime? _OfferCloseDate = OfferCloseDate;
                    if (SelectedLeadList[0].DeliveryDate == null)
                        _OfferCloseDate = null;
                    if (OfferCloseDate >= GeosApplication.Instance.ServerDateTime)
                        _OfferCloseDate = OfferCloseDate;

                    DateTime? _QuoteSentDate = QuoteSentDate;
                    DateTime? _RFQReceptionDate = RFQReceptionDate;
                    DateTime? _CreatedIn = SelectedLeadList[0].CreatedIn;
                    DateTime? _POReceivedInDate = SelectedLeadList[0].POReceivedInDate;
                    if (QuoteSentDate <= DateTime.MinValue || RFQReceptionDate <= DateTime.MinValue || SelectedLeadList[0].CreatedIn <= DateTime.MinValue || SelectedLeadList[0].POReceivedInDate <= DateTime.MinValue)
                    {
                        if (QuoteSentDate <= DateTime.MinValue)
                            _QuoteSentDate = null;
                        if (RFQReceptionDate <= DateTime.MinValue)
                            _RFQReceptionDate = null;
                        if (SelectedLeadList[0].CreatedIn <= DateTime.MinValue)
                            _CreatedIn = null;
                        if (SelectedLeadList[0].POReceivedInDate <= DateTime.MinValue)
                            _POReceivedInDate = null;
                    }
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                    ConvertedAmount = CrmStartUpOfferActiveSite.GetOfferAmountByCurrencyConversion(SelectedLeadList[0].IdOffer, OfferAmount, Currencies[SelectedIndexCurrency].IdCurrency, SelectedLeadList[0].Currency.IdCurrency, _OfferCloseDate, _QuoteSentDate, _RFQReceptionDate, _CreatedIn, _POReceivedInDate);
                    System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                                  where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                                  let region = new System.Globalization.RegionInfo(culture.LCID)
                                                                  where String.Equals(region.ISOCurrencySymbol, SelectedLeadList[0].Currency.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                  select region).First();
                    ConvertedOfferAmount = ConvertedAmount + " " + regionInfo.CurrencySymbol;
                }
                GeosApplication.Instance.Logger.Log("Method OnCloseDateEditValueChangingCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OnCloseDateEditValueChangingCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // Export Excel .xlsx


        #region
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        //public ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion // Properties.
        private void ExporttoExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportAccountsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (ListLogComments.Count > 0)
                {
                    //SaveFileDialog saveFile = new SaveFileDialog();
                    //saveFile.DefaultExt = "xlsx";
                    //saveFile.FileName = selectedLeadList[0].Code+"_Comments";
                    //saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    //saveFile.FilterIndex = 1;
                    //saveFile.Title = "Save Excel Report";
                    //DialogResult = (Boolean)saveFile.ShowDialog();

                    SaveFileDialogService.DefaultExt = "xlsx";
                    SaveFileDialogService.DefaultFileName = selectedLeadList[0].Code + "_Comments";
                    SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                        IsBusy = true;
                        if (!DXSplashScreen.IsActive)
                        {
                            // DXSplashScreen.Show<SplashScreenView>(); 
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

                        //ResultFileName = (saveFile.FileName);
                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;


                        TextRange range = null;
                        //Workbook workbook = new Workbook();
                        //string SheetName = selectedLeadList[0].Code + "_Comments";
                        //workbook.Worksheets.Insert(0, SheetName);
                        //Worksheet ws = workbook.Worksheets[SheetName];

                        SpreadsheetControl control = new SpreadsheetControl();
                        Worksheet ws = control.ActiveWorksheet;
                        ws.Name = selectedLeadList[0].Code + "_Comments";
                        ws.Cells[0, 0].Value = "User";
                        ws.Cells[0, 0].Font.Bold = true;
                        ws.Cells[0, 0].ColumnWidth = 400;
                        ws.Cells[0, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        ws.Cells[0, 1].Value = "Comments Date";
                        ws.Cells[0, 1].Font.Bold = true;
                        ws.Cells[0, 1].ColumnWidth = 400;
                        ws.Cells[0, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        ws.Cells[0, 2].Value = "Comments";
                        ws.Cells[0, 2].Font.Bold = true;
                        ws.Cells[0, 2].ColumnWidth = 1000;
                        ws.Cells[0, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                        int counter = 1;
                        if (ListLogComments.Count > 0)
                        {
                            for (int i = 0; i < ListLogComments.Count; i++)
                            {
                                var rtb = new RichTextBox();
                                var doc = new FlowDocument();
                                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ListLogComments[i].Comments.ToString()));
                                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                range.Load(stream, DataFormats.Rtf);
                                //ListLogComments[i].Comments = range.Text;
                                ws.Cells[counter, 0].Value = ListLogComments[i].People.FullName;
                                ws.Cells[counter, 1].Value = ListLogComments[i].DateTime;
                                ws.Cells[counter, 2].Value = range.Text;
                                ws.Cells[counter, 2].Alignment.WrapText = true;
                                counter++;
                            }
                        }

                        //using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                        //{
                        //    workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                        //}

                        ////TableView CommentsLeadTableView = ((TableView)obj);
                        ////GridControl gridControl = (CommentsLeadTableView).Grid;
                        ////CommentsLeadTableView.ShowTotalSummary = false;
                        ////CommentsLeadTableView.ShowFixedTotalSummary = false;
                        ////CommentsLeadTableView.ExportToXlsx(ResultFileName);

                        //IsBusy = false;
                        //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        //System.Diagnostics.Process.Start(ResultFileName);

                        control.SaveDocument(ResultFileName);
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AboutChangelogExportSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        System.Diagnostics.Process.Start(ResultFileName);

                    }
                }
                GeosApplication.Instance.Logger.Log("Method ExportCommentsLeadGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportCommentsLeadGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void RtfToPlaintext()
        {
            TextRange range = null;
            if (ListLogComments.Count > 0)
            {
                if (ListLogComments[0].IsRtfText)
                {
                    var rtb = new RichTextBox();
                    var doc = new FlowDocument();
                    MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ListLogComments[0].Comments.ToString()));
                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
                else
                {
                    CommentText = ListLogComments[0].Comments.ToString();
                }
            }

            if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                CommentText = range.Text;
        }

        private void MyTableView_FocusedRowChanging(CanceledEventArgs e)
        {
            //object sender,
            //TableView view = sender as TableView;

            //GridControl grid = view.DataControl as GridControl;
            //Problem problem = grid.GetRow(e.NewRowHandle) as Problem;
            //if (problem.IsDisabled)
            //{
            //    e.Cancel = true;
            //}

            //TreeListView view = sender as TreeListView;

            //GridControl grid = view.DataControl as GridControl;
            //ProductCategory problem = grid.GetRow(e.NewRowHandle) as ProductCategory;
            //if (problem.IsDisabled)
            //{
            //    e.Cancel = true;
            //}
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// this method use for fill Offer owner list
        /// [000][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        private void FillOfferOwnerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOfferOwnerList() ...", category: Category.Info, priority: Priority.Low);

                OfferOwnerList = new List<User>();

                OfferOwnerList = CrmStartUp.GetSalesAndCommericalUsers();

                OfferOwnerList.Insert(0, new User() { FullName = "---", FirstName = "---" });

                SelectedIndexOfferOwner = OfferOwnerList.FindIndex(i => i.IdUser == SelectedLeadList[0].OfferedBy);

                if (SelectedIndexOfferOwner == -1)
                {
                    SelectedIndexOfferOwner = 0;
                }


                GeosApplication.Instance.Logger.Log("Method FillOfferOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferOwnerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferOwnerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        ///  this method use for fill Offer To list
        ///  [000][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        private void FillOfferToList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOfferToList() ...", category: Category.Info, priority: Priority.Low);

                OfferToList = new List<OfferContact>();

                Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];

                OfferToList = CrmStartUp.GetContactsOfCustomerGroupByOfferId(customer.IdCustomer);
                //[002] Added to create controller of service to hit offer plant service
                ICrmService CrmStartUpOfferActiveSiteMO = new CrmServiceController(SelectedLeadList[0].OfferActiveSite.SiteServiceProvider);
                //[002] Changed controller and service method GetOfferContact to GetOfferContact_V2040
                ListOfferContact = new ObservableCollection<OfferContact>(CrmStartUpOfferActiveSiteMO.GetOfferContact_V2040(SelectedLeadList[0].IdOffer).AsEnumerable());

                if (ListOfferContact != null)
                {
                    SelectedOfferToList = new List<object>();
                    foreach (OfferContact item in ListOfferContact)
                    {
                        SelectedOfferToList.Add(OfferToList.FirstOrDefault(x => x.IdContact == item.People.IdPerson));
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillOfferToList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferToList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferToList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferToList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion


        public class Task : INotifyPropertyChanged, IDataErrorInfo
        {
            #region Validation

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

            /// <summary>
            /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
            /// </summary>
            /// <param name="columnName"></param>
            /// <returns></returns>
            string IDataErrorInfo.this[string columnName]
            {
                get
                {
                    if (!allowValidation) return null;

                    string amountSplitProp = BindableBase.GetPropertyName(() => OfferAmountSplit);
                    string offerSplitCloseDateProp = BindableBase.GetPropertyName(() => OfferCloseDateSplit);
                    string rfqSplitReceptionDateProp = BindableBase.GetPropertyName(() => RFQReceptionDateSplit);
                    string quoteSplitSentDateProp = BindableBase.GetPropertyName(() => QuoteSentDateSplit);
                    string productAndServicesSplitCountProp = BindableBase.GetPropertyName(() => ProductAndServicesSplitCount);
                    string isSplitOfferStatusDisabledProp = BindableBase.GetPropertyName(() => IsSplitOfferStatusDisabled);

                    if (columnName == amountSplitProp)
                        return SplitOtValidationRule.GetErrorMessage(amountSplitProp, OfferAmountSplit, GeosStatusListTask[SelectedGeosStatusTask].IdOfferStatusType);
                    else if (columnName == offerSplitCloseDateProp)
                        return SplitOtValidationRule.GetErrorMessage(offerSplitCloseDateProp, OfferCloseDateSplit);
                    else if (SelectedGeosStatusTask > -1 && columnName == rfqSplitReceptionDateProp)
                        return SplitOtValidationRule.GetErrorMessage(rfqSplitReceptionDateProp, RFQReceptionDateSplit, GeosStatusListTask[SelectedGeosStatusTask].IdOfferStatusType);
                    else if (SelectedGeosStatusTask > -1 && columnName == quoteSplitSentDateProp)
                        return SplitOtValidationRule.GetErrorMessage(quoteSplitSentDateProp, QuoteSentDateSplit, GeosStatusListTask[SelectedGeosStatusTask].IdOfferStatusType);
                    else if (columnName == productAndServicesSplitCountProp)
                        return SplitOtValidationRule.GetErrorMessage(productAndServicesSplitCountProp, ProductAndServicesSplitCount);
                    //[001] added
                    else if (columnName == isSplitOfferStatusDisabledProp)
                    {
                        if (!SplitOffer.IsGoAheadProduction)
                            return SplitOtValidationRule.GetErrorMessage(isSplitOfferStatusDisabledProp, IsSplitOfferStatusDisabled);
                    }

                    return null;
                }
            }

            string IDataErrorInfo.Error
            {
                get
                {
                    //if (!allowValidation) return null;
                    IDataErrorInfo me = (IDataErrorInfo)this;
                    string error =
                    me[BindableBase.GetPropertyName(() => OfferAmountSplit)] +
                    me[BindableBase.GetPropertyName(() => OfferCloseDateSplit)] +
                    me[BindableBase.GetPropertyName(() => RFQReceptionDateSplit)] +
                    me[BindableBase.GetPropertyName(() => QuoteSentDateSplit)] +
                    me[BindableBase.GetPropertyName(() => ProductAndServicesSplitCount)] +
                    me[BindableBase.GetPropertyName(() => IsSplitOfferStatusDisabled)];

                    if (!string.IsNullOrEmpty(error))
                        return "Please check inputted data.";

                    return null;
                }
            }

            #endregion

            #region Declaration

            //public DataTable DttableSplit { get; set; }
            public DataTable dttableSplit;

            public DataTable DttableSplit
            {
                get { return dttableSplit; }
                set
                {
                    dttableSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DttableSplit"));
                }
            }

            public DataTable DttableSplittemp { get; set; }
            private bool isShowAllSplit;

            private ObservableCollection<GeosStatus> geosStatusListTask;
            private DateTime offerCloseDateSplit;
            private DateTime offerCloseDateSplitMinValue;
            private DateTime? rfqReceptionDateSplit;
            private DateTime? quoteSentDateSplit;
            private double offerAmountSplit;
            public long OfferNumberSplit;
            private int selectedGeosStatusTask;
            private string taskOfferCode;
            public Company CurrentCompanyTask { get; set; }
            private int selectedIndexOfferTypeTask;
            public byte SelectedOfferTypeIdTasktemp { get; set; }
            private int selectedIndexConfidentialLevelTask;
            private Color selectedBackgroundTask;
            private Color hoverBackgroundTask;
            private int productAndServicesSplitCount;
            private GeosStatus geosStatusTask;
            public Offer TaskOffer { get; set; }
            public int SelectedGeosStatusTaskold { get; set; }
            public List<OfferType> OfferTypeListTask { get; set; }
            private List<OptionsByOffer> optionsByOfferListSplit;
            private int selectedIndexTab;
            public bool isOldOTSplit = false;
            public string TempSplitOfferCode { get; set; }
            public int TempSplitSelectedIndexOfferType { get; set; }
            //private bool isComplete;
            int count = 0;
            public string TaskOfferCodeold { get; set; }
            public bool isModifyCode = false;

            private bool isBusy;
            ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            private Int64 idSourceOffer;
            private bool allowValidation = false;
            private bool isTabIndexZero;
            bool isSplitOfferStatusDisabled;
            private byte? idOfferType;
            private string convertedOfferAmountSplit;
            private bool gridRowHeightForAmountSplit;
            public Offer SplitOffer;
            private double convertedAmountSplit;
            private ActiveSite offerActiveSite ;
            public List<Currency> SplitCurrencyList { get; set; }
            #endregion

            #region ICommand
            public ICommand SplitSelectedIndexChangedCommand { get; set; }
            public ICommand SplitTextEditValueChangingCommand { get; set; }
            public ICommand OnSplitDateEditValueChangingCommand { get; set; }

            #endregion

            #region Constructor
            public Task()
            {
                SplitSelectedIndexChangedCommand = new DelegateCommand<object>(new Action<object>((obj) => { SplitSelectedIndexChangedCommandAction(obj); }));
                SplitTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChangingACtion);
                OnSplitDateEditValueChangingCommand = new DelegateCommand<EditValueChangedEventArgs>(OnSplitDateEditValueChangingCommandAction);
            }

            #endregion

            #region public Properties
            public byte? IdOfferType
            {
                get { return idOfferType; }
                set
                {
                    idOfferType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IdOfferType"));
                }
            }

            public bool IsSplitOfferStatusDisabled
            {
                get
                {
                    return isSplitOfferStatusDisabled;
                }

                set
                {
                    isSplitOfferStatusDisabled = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsSplitOfferStatusDisabled"));

                }
            }
            public Int64 IdSourceOffer
            {
                get { return idSourceOffer; }
                set { idSourceOffer = value; }
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
            public bool IsComplete
            {
                get;
                set;
            }
            public int SelectedIndexTab
            {
                get
                {
                    return selectedIndexTab;
                }
                set
                {
                    selectedIndexTab = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTab"));

                }
            }

            public bool IsTabIndexZero
            {
                get
                {
                    return isTabIndexZero;
                }
                set
                {
                    isTabIndexZero = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsTabIndexZero"));

                }
            }

            public bool IsShowAllSplit
            {
                get { return isShowAllSplit; }
                set
                {
                    isShowAllSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsShowAllSplit"));
                }
            }
            public DateTime OfferCloseDateSplit
            {
                get { return offerCloseDateSplit; }
                set
                {
                    offerCloseDateSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDateSplit"));
                }
            }

            public DateTime OfferCloseDateSplitMinValue
            {
                get { return offerCloseDateSplitMinValue; }
                set
                {
                    offerCloseDateSplitMinValue = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDateSplitMinValue"));
                }
            }

            public List<OptionsByOffer> OptionsByOfferListSplit
            {
                get
                {
                    return optionsByOfferListSplit;
                }

                set
                {
                    optionsByOfferListSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("OptionsByOfferListSplit"));
                }
            }

            public int ProductAndServicesSplitCount
            {
                get { return productAndServicesSplitCount; }
                set
                {
                    productAndServicesSplitCount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ProductAndServicesSplitCount"));
                }
            }
            public DateTime? RFQReceptionDateSplit
            {
                get { return rfqReceptionDateSplit; }
                set
                {
                    rfqReceptionDateSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RFQReceptionDateSplit"));
                }
            }

            public DateTime? QuoteSentDateSplit
            {
                get { return quoteSentDateSplit; }
                set
                {
                    quoteSentDateSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("QuoteSentDateSplit"));
                }
            }
            public double OfferAmountSplit
            {
                get { return offerAmountSplit; }
                set
                {
                    offerAmountSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("OfferAmountSplit"));
                }
            }
            public ObservableCollection<GeosStatus> GeosStatusListTask
            {
                get { return geosStatusListTask; }
                set
                {
                    geosStatusListTask = value;
                }
            }

            public int SelectedIndexCurrencyTask
            {
                get;
                set;
            }

            public int SelectedGeosStatusTask
            {
                get
                {
                    return selectedGeosStatusTask;
                }

                set
                {
                    selectedGeosStatusTask = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedGeosStatusTask"));

                    if (selectedGeosStatusTask == 2 || selectedGeosStatusTask == 3)
                        SelectedIndexOfferTypeTask = 0;
                    if (selectedGeosStatusTask == 0 || selectedGeosStatusTask == 1)
                        SelectedIndexOfferTypeTask = 1;

                    if (selectedGeosStatusTask == 4 || selectedGeosStatusTask == 5 || selectedGeosStatusTask == 6 ||
                        selectedGeosStatusTask == 7 || selectedGeosStatusTask == 9 || selectedGeosStatusTask == 10 ||
                        selectedGeosStatusTask == 11 || selectedGeosStatusTask == 12 || selectedGeosStatusTask == 13 ||
                        selectedGeosStatusTask == 15 || selectedGeosStatusTask == 16 || selectedGeosStatusTask == 17)
                    {
                        if (IdOfferType == 1)
                            SelectedIndexOfferTypeTask = 0;
                        else
                            SelectedIndexOfferTypeTask = 1;
                        IsSplitOfferStatusDisabled = true;

                    }
                    else
                    {
                        IsSplitOfferStatusDisabled = false;

                    }
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("RFQReceptionDateSplit"));
                        PropertyChanged(this, new PropertyChangedEventArgs("QuoteSentDateSplit"));
                        PropertyChanged(this, new PropertyChangedEventArgs("OfferAmountSplit"));

                    }

                }
            }
            public string Description
            {
                get;
                set;
            }

            public string TaskOfferCode
            {
                get { return taskOfferCode; }
                set
                {
                    taskOfferCode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TaskOfferCode"));
                }
            }

            public int SelectedIndexOfferTypeTask
            {
                get
                {
                    return selectedIndexOfferTypeTask;
                }

                set
                {

                    selectedIndexOfferTypeTask = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOfferTypeTask"));


                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        // DXSplashScreen.Show<SplashScreenView>(); 
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



                    GenerateOfferCodeSplitTab();
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                }
            }

            public int SelectedIndexConfidentialLevelTask
            {
                get
                {
                    return selectedIndexConfidentialLevelTask;
                }

                set
                {
                    selectedIndexConfidentialLevelTask = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexConfidentialLevelTask"));
                    SelectedConfidentialLevel();
                }
            }


            public Color SelectedBackgroundTask
            {
                get
                {
                    return selectedBackgroundTask;
                }

                set
                {
                    selectedBackgroundTask = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedBackgroundTask"));
                }

            }

            public GeosStatus GeosStatusTask
            {
                get
                {
                    return geosStatusTask;
                }

                set
                {
                    geosStatusTask = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("GeosStatusTask"));

                }
            }

            public Color HoverBackgroundTask
            {
                get { return hoverBackgroundTask; }
                set
                {
                    hoverBackgroundTask = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("HoverBackgroundTask"));
                }
            }
            public string ConvertedOfferAmountSplit
            {
                get { return convertedOfferAmountSplit; }
                set
                {
                    convertedOfferAmountSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ConvertedOfferAmountSplit"));
                }
            }

            public bool GridRowHeightForAmountSplit
            {
                get
                {
                    return gridRowHeightForAmountSplit;
                }

                set
                {
                    gridRowHeightForAmountSplit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeightForAmountSplit"));
                }
            }

            public double ConvertedAmountSplit
            {
                get
                {
                    return convertedAmountSplit;
                }

                set
                {
                    convertedAmountSplit = Math.Round(value, 2);
                    OnPropertyChanged(new PropertyChangedEventArgs("ConvertedAmountSplit"));
                }
            }


            public ActiveSite OfferActiveSite
            {
                get
                {
                    return offerActiveSite;
                }

                set
                {
                    offerActiveSite = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("OfferActiveSite"));
                }
            }
            #endregion

            #region Methods
            private void OnTextEditValueChangingACtion(EditValueChangingEventArgs obj)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method OnTextEditValueChangingACtion ...", category: Category.Info, priority: Priority.Low);
                    if (SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency == SplitOffer.IdCurrency)
                    {
                        GridRowHeightForAmountSplit = false;
                    }
                    else
                        GridRowHeightForAmountSplit = true;

                    if (GridRowHeightForAmountSplit)
                    {
                        DateTime? _OfferCloseDate = OfferCloseDateSplit;
                        if (SplitOffer.DeliveryDate == null)
                            _OfferCloseDate = null;
                        if (OfferCloseDateSplit >= GeosApplication.Instance.ServerDateTime)
                            _OfferCloseDate = OfferCloseDateSplit;

                        DateTime? _QuoteSentDate = QuoteSentDateSplit;
                        DateTime? _RFQReceptionDate = RFQReceptionDateSplit;
                        DateTime? _CreatedIn = SplitOffer.CreatedIn;
                        DateTime? _POReceivedInDate = SplitOffer.POReceivedInDate;
                        if (QuoteSentDateSplit <= DateTime.MinValue || RFQReceptionDateSplit <= DateTime.MinValue || SplitOffer.CreatedIn <= DateTime.MinValue || SplitOffer.POReceivedInDate <= DateTime.MinValue)
                        {
                            if (QuoteSentDateSplit <= DateTime.MinValue)
                                _QuoteSentDate = null;
                            if (RFQReceptionDateSplit <= DateTime.MinValue)
                                _RFQReceptionDate = null;
                            if (SplitOffer.CreatedIn <= DateTime.MinValue)
                                _CreatedIn = null;
                            if (SplitOffer.POReceivedInDate <= DateTime.MinValue)
                                _POReceivedInDate = null;
                        }
                        ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SplitOffer.OfferActiveSite.SiteServiceProvider);
                        ConvertedAmountSplit = CrmStartUpOfferActiveSite.GetOfferAmountByCurrencyConversion(SplitOffer.IdOffer, OfferAmountSplit, SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency, SplitOffer.Currency.IdCurrency, _OfferCloseDate, _QuoteSentDate, _RFQReceptionDate, _CreatedIn, _POReceivedInDate);
                        System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                                      where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                                      let region = new System.Globalization.RegionInfo(culture.LCID)
                                                                      where String.Equals(region.ISOCurrencySymbol, SplitOffer.Currency.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                      select region).First();
                        ConvertedOfferAmountSplit = ConvertedAmountSplit + " " + regionInfo.CurrencySymbol;
                    }
                    GeosApplication.Instance.Logger.Log("Method OnTextEditValueChangingACtion() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in OnTextEditValueChangingACtion() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            public string CheckValidation()
            {
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("OfferAmountSplit"));
                PropertyChanged(this, new PropertyChangedEventArgs("OfferCloseDateSplit"));
                PropertyChanged(this, new PropertyChangedEventArgs("RFQReceptionDateSplit"));
                PropertyChanged(this, new PropertyChangedEventArgs("QuoteSentDateSplit"));
                PropertyChanged(this, new PropertyChangedEventArgs("ProductAndServicesSplitCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsSplitOfferStatusDisabled"));

                return error;
            }

            public string CheckValidationForStatus()
            {
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("IsSplitOfferStatusDisabled"));
                return error;
            }

            /// <summary>
            /// Method for select rating star as per confidential Level.
            /// </summary>
            private void SelectedConfidentialLevel()
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method SelectedConfidentialLevel ...", category: Category.Info, priority: Priority.Low);

                    if (SelectedIndexConfidentialLevelTask > 0 && SelectedIndexConfidentialLevelTask <= 20)
                    {
                        SelectedBackgroundTask = Colors.Red;
                    }
                    if (SelectedIndexConfidentialLevelTask > 20 && SelectedIndexConfidentialLevelTask <= 40)
                    {
                        SelectedBackgroundTask = Colors.Orange;
                    }
                    if (SelectedIndexConfidentialLevelTask > 40 && SelectedIndexConfidentialLevelTask <= 60)
                    {
                        SelectedBackgroundTask = Colors.Yellow;
                    }
                    if (SelectedIndexConfidentialLevelTask > 60 && SelectedIndexConfidentialLevelTask <= 80)
                    {
                        SelectedBackgroundTask = Colors.DeepSkyBlue;
                    }
                    if (SelectedIndexConfidentialLevelTask > 80 && SelectedIndexConfidentialLevelTask <= 100)
                    {
                        SelectedBackgroundTask = Colors.Green;
                    }

                    GeosApplication.Instance.Logger.Log("Method SelectedConfidentialLevel() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in SelectedConfidentialLevel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            public void GenerateOfferCodeSplit()
            {
                IsBusy = true;
                try
                {
                    GeosApplication.Instance.Logger.Log("Method GenerateOfferCodeSplit ...", category: Category.Info, priority: Priority.Low);

                    if (SelectedIndexOfferTypeTask > -1)
                    {
                        if (SelectedIndexOfferTypeTask == 0)
                        {
                            OfferNumberSplit = CrmStartUp.GetNextNumberOfSuppliesFromGCM(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType);
                        }
                        else
                        {
                            OfferNumberSplit = CrmStartUp.GetNextNumberOfOfferFromCounters_V2040(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType,0);
                        }


                        if (SelectedIndexOfferTypeTask > -1)
                        {
                            if (SelectedIndexTab == 0)
                            {
                            }
                            else
                            {
                                TaskOfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType,Convert.ToInt32(TaskOffer.OfferActiveSite.IdSite), GeosApplication.Instance.ActiveUser.IdUser);

                            }
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method GenerateOfferCodeSplit() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCodeSplit " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCodeSplit " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCodeSplit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
            public void GenerateOfferCodeSplitTab()
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method GenerateOfferCodeSplitTab ...", category: Category.Info, priority: Priority.Low);
                    if (SelectedIndexOfferTypeTask > -1)
                    {
                        if (SelectedIndexOfferTypeTask == 0)
                        {
                            OfferNumberSplit = CrmStartUp.GetNextNumberOfSuppliesFromGCM(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType);
                        }
                        else
                        {
                             // [001] Changed service method 
                             OfferNumberSplit = CrmStartUp.GetNextNumberOfOfferFromCounters_V2040(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType, 0);
                        }
                        count++;
                        if (SelectedIndexTab == 0 && IsTabIndexZero)
                        {
                            if (isModifyCode)
                            {

                                if (OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType == SelectedOfferTypeIdTasktemp)
                                {
                                    TaskOfferCode = TaskOfferCodeold;
                                }
                                else
                                {
                                    // [001] Changed service method 
                                    TaskOfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType, Convert.ToInt32(TaskOffer.OfferActiveSite.IdSite), GeosApplication.Instance.ActiveUser.IdUser);
                                 }


                            }
                            if (count == 1)
                            {
                                isModifyCode = true;
                            }
                        }
                        else
                        {
                            // [001] Changed service method 
                            TaskOfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeListTask[SelectedIndexOfferTypeTask].IdOfferType, Convert.ToInt32(TaskOffer.OfferActiveSite.IdSite), GeosApplication.Instance.ActiveUser.IdUser);
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method GenerateOfferCode() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCode " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in GenerateOfferCode " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
            }


            private void SetOfferCodeBySplitCondition()
            {
                GeosApplication.Instance.Logger.Log("Method SetOfferCodeBySplitCondition ...", category: Category.Info, priority: Priority.Low);
                if (GeosStatusListTask != null)
                {
                    //Start This part of code convert Lead to OT and vice versa.
                    //if status is Only quoted and Waiting for quote then as per condition generate the code or set old code.
                    if (GeosStatusListTask[SelectedIndexOfferTypeTask].IdOfferStatusType == 1 || GeosStatusListTask[SelectedIndexOfferTypeTask].IdOfferStatusType == 2)
                    {
                        if (SelectedIndexOfferTypeTask == OfferTypeListTask.FindIndex(i => i.IdOfferType == 1))
                        {
                            // if status is changed from lost/cancelled to quoted/Waitingforquote and offer type is Leads then generate new code for OT.
                            if (!isOldOTSplit)
                            {
                                SelectedIndexOfferTypeTask = OfferTypeListTask.FindIndex(i => i.IdOfferType == 1);
                                GenerateOfferCodeSplitTab();
                                TaskOffer.IsUpdateLeadToOT = true;
                            }
                        }
                        else
                        {
                            //If user choose except Forecasted, Qualified, Only quoted and Waiting for quoted then code must be old one .
                            if (isOldOTSplit)
                            {
                                SelectedIndexOfferTypeTask = TempSplitSelectedIndexOfferType;
                                if (TempSplitOfferCode != null)
                                {
                                    TaskOfferCode = TempSplitOfferCode;
                                }
                                TaskOffer.IsUpdateLeadToOT = false;
                            }
                            else
                            {
                                SelectedIndexOfferTypeTask = OfferTypeListTask.FindIndex(i => i.IdOfferType == 1);
                                GenerateOfferCodeSplitTab();
                                TaskOffer.IsUpdateLeadToOT = true;
                            }
                        }
                    }
                    else
                    {
                        if (GeosStatusListTask[SelectedIndexOfferTypeTask].IdOfferStatusType == 15 || GeosStatusListTask[SelectedIndexOfferTypeTask].IdOfferStatusType == 16)
                        {
                            //if user choose Forecasted or Qualified then don't convert code just set old code. 
                            if (SelectedIndexOfferTypeTask == OfferTypeListTask.FindIndex(i => i.IdOfferType == 10))
                            {
                            }
                            else
                            {
                                if (isOldOTSplit)
                                {
                                    SelectedIndexOfferTypeTask = OfferTypeListTask.FindIndex(i => i.IdOfferType == 10);
                                    GenerateOfferCodeSplitTab();
                                    TaskOffer.IsUpdateLeadToOT = true;
                                }
                                else
                                {
                                    SelectedIndexOfferTypeTask = TempSplitSelectedIndexOfferType;
                                    if (TempSplitOfferCode != null)
                                        TaskOfferCode = TempSplitOfferCode;
                                    TaskOffer.IsUpdateLeadToOT = false;
                                }
                            }
                        }
                        //If user choose except Forecasted, Qualified, Only quoted and Waiting for quoted then code must be old one .
                        else
                        {
                            SelectedIndexOfferTypeTask = TempSplitSelectedIndexOfferType;
                            if (TempSplitOfferCode != null)
                                TaskOfferCode = TempSplitOfferCode;
                            TaskOffer.IsUpdateLeadToOT = false;
                        }
                    }

                    //END This Method convert Lead to OT and vice versa.
                }

                GeosApplication.Instance.Logger.Log("Method SetOfferCodeBySplitCondition() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            private void SplitSelectedIndexChangedCommandAction(object obj)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method SplitSelectedIndexChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                    // ConvertedOfferAmountSplit = CrmStartUp.GetOfferAmountByCurrencyConversion(SplitIdOffer, OfferAmountSplit, SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency, SplitCurrencyList[ConvertedIndexCurrencyTask].IdCurrency);
                    if (SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency == SplitOffer.IdCurrency)
                    {
                        GridRowHeightForAmountSplit = false;
                    }
                    else
                        GridRowHeightForAmountSplit = true;

                    if (GridRowHeightForAmountSplit)
                    {
                        DateTime? _OfferCloseDate = OfferCloseDateSplit;
                        if (SplitOffer.DeliveryDate == null)
                            _OfferCloseDate = null;
                        if (OfferCloseDateSplit >= GeosApplication.Instance.ServerDateTime)
                            _OfferCloseDate = OfferCloseDateSplit;

                        DateTime? _QuoteSentDate = QuoteSentDateSplit;
                        DateTime? _RFQReceptionDate = RFQReceptionDateSplit;
                        DateTime? _CreatedIn = SplitOffer.CreatedIn;
                        DateTime? _POReceivedInDate = SplitOffer.POReceivedInDate;
                        if (QuoteSentDateSplit <= DateTime.MinValue || RFQReceptionDateSplit <= DateTime.MinValue || SplitOffer.CreatedIn <= DateTime.MinValue || SplitOffer.POReceivedInDate <= DateTime.MinValue)
                        {
                            if (QuoteSentDateSplit <= DateTime.MinValue)
                                _QuoteSentDate = null;
                            if (RFQReceptionDateSplit <= DateTime.MinValue)
                                _RFQReceptionDate = null;
                            if (SplitOffer.CreatedIn <= DateTime.MinValue)
                                _CreatedIn = null;
                            if (SplitOffer.POReceivedInDate <= DateTime.MinValue)
                                _POReceivedInDate = null;
                        }
                        ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SplitOffer.OfferActiveSite.SiteServiceProvider);
                        ConvertedAmountSplit = CrmStartUpOfferActiveSite.GetOfferAmountByCurrencyConversion(SplitOffer.IdOffer, OfferAmountSplit, SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency, SplitOffer.Currency.IdCurrency, _OfferCloseDate, _QuoteSentDate, _RFQReceptionDate, _CreatedIn, _POReceivedInDate);
                        System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                                      where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                                      let region = new System.Globalization.RegionInfo(culture.LCID)
                                                                      where String.Equals(region.ISOCurrencySymbol, SplitOffer.Currency.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                      select region).First();
                        ConvertedOfferAmountSplit = ConvertedAmountSplit + " " + regionInfo.CurrencySymbol;
                    }
                    GeosApplication.Instance.Logger.Log("Method SplitSelectedIndexChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in SplitSelectedIndexChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            private void OnSplitDateEditValueChangingCommandAction(EditValueChangedEventArgs obj)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method OnSplitDateEditValueChangingCommandAction ...", category: Category.Info, priority: Priority.Low);
                    if (SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency == SplitOffer.IdCurrency)
                    {
                        GridRowHeightForAmountSplit = false;
                    }
                    else
                        GridRowHeightForAmountSplit = true;

                    if (GridRowHeightForAmountSplit)
                    {
                        DateTime? _OfferCloseDate = OfferCloseDateSplit;
                        if (SplitOffer.DeliveryDate == null)
                            _OfferCloseDate = null;
                        if (OfferCloseDateSplit >= GeosApplication.Instance.ServerDateTime)
                            _OfferCloseDate = OfferCloseDateSplit;

                        DateTime? _QuoteSentDate = QuoteSentDateSplit;
                        DateTime? _RFQReceptionDate = RFQReceptionDateSplit;
                        DateTime? _CreatedIn = SplitOffer.CreatedIn;
                        DateTime? _POReceivedInDate = SplitOffer.POReceivedInDate;
                        if (QuoteSentDateSplit <= DateTime.MinValue || RFQReceptionDateSplit <= DateTime.MinValue || SplitOffer.CreatedIn <= DateTime.MinValue || SplitOffer.POReceivedInDate <= DateTime.MinValue)
                        {
                            if (QuoteSentDateSplit <= DateTime.MinValue)
                                _QuoteSentDate = null;
                            if (RFQReceptionDateSplit <= DateTime.MinValue)
                                _RFQReceptionDate = null;
                            if (SplitOffer.CreatedIn <= DateTime.MinValue)
                                _CreatedIn = null;
                            if (SplitOffer.POReceivedInDate <= DateTime.MinValue)
                                _POReceivedInDate = null;
                        }
                        ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(SplitOffer.OfferActiveSite.SiteServiceProvider);
                        ConvertedAmountSplit = CrmStartUpOfferActiveSite.GetOfferAmountByCurrencyConversion(SplitOffer.IdOffer, OfferAmountSplit, SplitCurrencyList[SelectedIndexCurrencyTask].IdCurrency, SplitOffer.Currency.IdCurrency, _OfferCloseDate, _QuoteSentDate, _RFQReceptionDate, _CreatedIn, _POReceivedInDate);
                        System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                                      where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                                      let region = new System.Globalization.RegionInfo(culture.LCID)
                                                                      where String.Equals(region.ISOCurrencySymbol, SplitOffer.Currency.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                      select region).First();
                        ConvertedOfferAmountSplit = ConvertedAmountSplit + " " + regionInfo.CurrencySymbol;
                    }
                    GeosApplication.Instance.Logger.Log("Method OnSplitDateEditValueChangingCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in OnSplitDateEditValueChangingCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            public void Init(Offer offer, List<Currency> currencies)
            {
                SplitOffer = (Offer)offer.Clone();
                SplitCurrencyList = currencies;
            }

            #endregion

            #region Events

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }

            #endregion

        }


        public class TempCategory
        {

            public ProductCategory Category { get; set; }

            public List<ProductCategory> ChildProductCategory { get; set; }

            public long? IdParent { get; set; }

            public long IdProductCategory { get; set; }

            public int Level { get; set; }

            public string Name { get; set; }

            public long Position { get; set; }

            public string PCName { get; set; }
        }
    }


}

