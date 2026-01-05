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
using DevExpress.Compression;
using System.Windows.Documents;
using System.Windows.Controls;
using Microsoft.Win32;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Xpf.RichEdit;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.Spreadsheet;
//OrderEditViewModel
namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class OrderEditViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
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
        private bool isControlEnable = false;//readonly
        private string informationError;
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
        private string showStatusDescription = " ";
        public int LeadsGenerateDays { get; set; }
        //public List<Company> LeadCompanylst;
        private List<Offer> offerDataLst;
        private List<CustomerPurchaseOrder> customerPurchaseOrderList;
        private List<Shipment> listShipment;
        private List<PackingBox> listBox;
        private string leadComment;

        private int selectedIndexOfferType = 0;
        public int TempSelectedIndexOfferType { get; set; }
        private double orderAmount;
        Double max_Value;
        private bool IsInIt;
        public bool IsSaleOwnerNull { get; set; }
        private DateTime offerCloseDate;

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
        private ObservableCollection<LogEntryByOffer> listLogComments = new ObservableCollection<LogEntryByOffer>();
        private ObservableCollection<LogEntryByOffer> listChangeLog = new ObservableCollection<LogEntryByOffer>();
        private ObservableCollection<OfferContact> listOfferContact = new ObservableCollection<OfferContact>();
        byte[] UserProfileImageByte = null;
        DataTable dttable { get; set; }
        private List<string> errorList;
        //bool isTechnicalTemplateChange = false;
        private Offer offerData;

        private int productAndServicesCount;
        ObservableCollection<GeosStatus> geosEnabledStatusList = new ObservableCollection<GeosStatus>();
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
        private List<OfferOption> offerOptions { get; set; }
        private List<LogEntryByOffer> changeLogCommentEntry = new List<LogEntryByOffer>();
        private string oldLeadComment;
        private DateTime deliveryDate;
        private string rfq;
        private bool gridRowHeight;
        private bool gridRowHeightForRfq;
        private bool gridRowHeightForQuoteSent;
        private Offer showStatusList;

        private OfferContact primaryOfferContact;
        private ObservableCollection<People> listCustomerContact;
        private Int16 idSite;
        private ObservableCollection<People> listAddedContact = new ObservableCollection<People>();
        private string previousPrimaryContact;
        private bool isPrimayContactChanged;
        private bool isFirstPrimaryContact;
        private ObservableCollection<Activity> orderActivityList;
        private Activity selectedActivity;
        private ObservableCollection<Activity> existingActivitiesTobeLinked;

        private bool isRtf;
        private bool isNormal = true;
        private ImageSource userProfileImage;
        private ObservableCollection<Company> entireCompanyPlantList;

        //[001][2018-07-09][skhade][CRM-M042-18] Eng. analysis option in Orders.
        private EngineeringAnalysis existingEngineeringAnalysis;
        bool isEngAnalysis;
        bool isEngAnalysisEnable = false;
        bool isSiteResponsibleRemoved;
        private ObservableCollection<ProductCategory> listProductCategory;
        private ProductCategory selectedCategory;
        private bool isCategoryControlEnable = true;

        //[001] added
        private List<User> offerOwnerList;
        private int selectedIndexOfferOwner;
        private List<Object> selectedOfferToList = new List<object>();
        private List<OfferContact> offerToList;
        private ObservableCollection<OfferContact> listAddedOfferContact;


        #endregion

        #region  public Properties
        public bool IsCategoryControlEnable
        {
            get { return isCategoryControlEnable; }
            set
            {
                isCategoryControlEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCategoryControlEnable"));
            }
        }

        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }

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

        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set
            {
                userProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage"));
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

        public ObservableCollection<Activity> ExistingActivitiesTobeLinked
        {
            get { return existingActivitiesTobeLinked; }
            set
            {
                existingActivitiesTobeLinked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistingActivitiesTobeLinked"));
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

        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                selectedIndexCompanyGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

                if (selectedIndexCompanyGroup > 0)
                {
                    IsBusy = true;

                    CompanyPlantList = new List<Company>();
                    CompanyPlantList = EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[selectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList();
                    //Sprint_60 (#69156) Blank site when editing some offer---Sdesai
                    bool isPlant = EntireCompanyPlantList.Any(x => x.IdCompany == SelectedPlantDetails.IdCompany);
                    if (!isPlant)
                    {
                        if (SelectedPlantDetails.Country != null)
                            SelectedPlantDetails.Name = SelectedPlantDetails.Name + "(" + SelectedPlantDetails.Country.Name + ")";
                        CompanyPlantList.Add(SelectedPlantDetails);
                    }
                    SelectedIndexCompanyPlant = CompanyPlantList.FindIndex(i => i.IdCompany == SelectedLeadList[0].Site.IdCompany);
                    if (SelectedIndexCompanyPlant == -1 || SelectedIndexCompanyPlant == 0)
                        SelectedIndexCompanyPlant = 0;
                    IsBusy = false;
                }
                else
                {
                    SelectedIndexCompanyPlant = -1;
                    CompanyPlantList = null;
                    GeosProjectsListTemp = null;
                }
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

        public double OrderAmount
        {
            get { return orderAmount; }
            set
            {
                orderAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderAmount"));
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

        public DateTime OfferCloseDate
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
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
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
                selectedShipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedShipment"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("ListLogComments"));
            }
        }

        public List<CustomerPurchaseOrder> CustomerPurchaseOrderList
        {
            get { return customerPurchaseOrderList; }
            set
            {
                customerPurchaseOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPurchaseOrderList"));
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
                SetProperty(ref listChangeLog, value, () => ListChangeLog);
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
                selectedStarIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStarIndex"));

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

                if (!IsInIt && SelectedIndexGeosProject != -1)
                {
                    if (GeosProjectsList != null && GeosProjectsList.Count != 0 && GeosProjectsList[SelectedIndexGeosProject] != null)
                        SelectedIndexCarOEM = CaroemsList.FindIndex(cr => cr.IdCarOEM == GeosProjectsList[SelectedIndexGeosProject].IdCarOem);
                }
                else if (!IsInIt && SelectedIndexGeosProject == -1)
                    SelectedIndexCarOEM = 0;
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
            get { return selectedGeosStatus; }
            set
            {
                selectedGeosStatus = value;

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

                    StatusChangeAction();
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedGeosStatus"));

                    //Start  This Method convert Lead to OT and vice versa.

                    //SetOfferCodeByCondition();

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


        public bool IsControlEnable
        {
            get { return isControlEnable; }
            set
            {
                isControlEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsControlEnable"));
            }
        }

        // This variable for stop status change in order because we can not change of order
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
            get { return isAdd; }
            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }
        public string CommentText
        {
            get { return commentText; }
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
            get { return selectedIndex; }
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
            get { return isSplitVisible; }
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

        public ObservableCollection<Activity> OrderActivityList
        {
            get { return orderActivityList; }
            set
            {
                orderActivityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderActivityList"));
            }
        }

        /// <summary>
        /// [001] The engineering analysis.
        /// </summary>
        public EngineeringAnalysis ExistingEngineeringAnalysis
        {
            get { return existingEngineeringAnalysis; }
            set
            {
                existingEngineeringAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistingEngineeringAnalysis"));
            }
        }

        public bool IsEngAnalysis
        {
            get { return isEngAnalysis; }
            set
            {
                isEngAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysis"));
            }
        }

        public bool IsEngAnalysisEnable
        {
            get { return isEngAnalysisEnable; }
            set
            {
                isEngAnalysisEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisEnable"));
            }
        }
        public Company SelectedPlantDetails { get; set; }

        //[001] added
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
        #endregion

        #region public ICommand

        public ICommand OrderViewWindowCancelButtonCommand { get; set; }
        public ICommand OrderEditViewAcceptButtonCommand { get; set; }
        public ICommand OrderEditViewCancelButtonCommand { get; set; }
        public ICommand QuantityEditValueChangedCommand { get; set; }
        public ICommand CheckEditQuantityValueChangedCommand { get; set; }
        public ICommand ShowAllCheckedCommand { get; set; }
        public ICommand ShowAllUncheckedCommand { get; set; }
        public ICommand OrderAmountLostFocusCommand { get; set; }
        public ICommand CustomNodeFilterCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand LostOpportunityWindowCommand { get; set; }
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand CommentsShipmentGridDoubleClickCommand { get; set; }
        public ICommand GetCustomerContactCommand { get; set; }
        public ICommand SetPrimaryContactCommand { get; set; }
       
        public ICommand AddProjectButtonCommand { get; set; }
        public ICommand AddNewActivityCommand { get; set; }
        public ICommand ActivitiesGridDoubleClickCommand { get; set; }
        //[001] comment
        //public ICommand GetSalesContactCommand { get; set; }
        //public ICommand HyperlinkForEmail { get; set; }
        //public ICommand AssignedSalesCancelCommand { get; set; }
        //public ICommand SetSalesResponsibleCommand { get; set; }
        //public ICommand LinkedContactDoubleClickCommand { get; set; }
        public ICommand ExistingActivitiesGridDoubleClickCommand { get; set; }
        public ICommand ExcelexportButtonCheckedCommand { get; set; }
        public ICommand RichTextResizingCommand { get; set; }
       
        public ICommand EditEngineeringAnalysisCommand { get; set; }
        public ICommand SelectionClearCommand { get; set; }
        public ICommand IsEditorChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
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
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        public OrderEditViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OrderEditViewModel ...", category: Category.Info, priority: Priority.Low);
                OrderViewWindowCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                OrderEditViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveOrder));
                QuantityEditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(QuantityEditValueChangedAction);
                CheckEditQuantityValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(QuantityEditAfterCheckChangedAction);
                OrderEditViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                OrderAmountLostFocusCommand = new DelegateCommand<object>(OrderAmountLostFocusCommandAction);
                IsShowAll = false;
                AddNewActivityCommand = new DelegateCommand<object>(AddActivityViewWindowShow);
                ActivitiesGridDoubleClickCommand = new DelegateCommand<object>(EditActivityViewWindowShow);
                ShowAllCheckedCommand = new RelayCommand(new Action<object>(ShowAllCheckedCommandAction));
                ShowAllUncheckedCommand = new RelayCommand(new Action<object>(ShowAllUncheckedCommandAction));

                CustomNodeFilterCommand = new DelegateCommand<DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs>(CustomNodeFilterCommandAction);
                AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                LostOpportunityWindowCommand = new RelayCommand(new Action<object>(ShowLostOpportunityWindow));
                CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
                CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(EditCommentAction);
                CommentsShipmentGridDoubleClickCommand = new DelegateCommand<object>(EditShipmentAction);
                GetCustomerContactCommand = new DelegateCommand<GridColumnDataEventArgs>(CutomerContactCheckedAction);
                SetPrimaryContactCommand = new DelegateCommand<object>(SetCommandAction);

                ExistingActivitiesGridDoubleClickCommand = new DelegateCommand<object>(LinkExistingActivityToOrder);
                AddProjectButtonCommand = new DelegateCommand<object>(AddNewProjectCommandAction);

                ExcelexportButtonCheckedCommand = new DelegateCommand<object>(ExporttoExcel);
                RichTextResizingCommand = new DelegateCommand<object>(ResizeRichTextEditor);
                //[001] add comment
               // GetSalesContactCommand = new DelegateCommand<object>(GetSalesContactCommandAction);
               // HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
               // AssignedSalesCancelCommand = new DelegateCommand<object>(AssignedSalesCancelCommandAction);
               // SetSalesResponsibleCommand = new DelegateCommand<object>(SetSalesResponsibleCommandAction);
               //LinkedContactDoubleClickCommand = new DelegateCommand<object>(LinkedContactDoubleClickCommandAction);


                SelectionClearCommand = new DelegateCommand<object>(SelectionClearCommandAction);
                IsEditorChangedCommand = new DelegateCommand<object>(IsEditorChangedAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //EngAnalysis
                EditEngineeringAnalysisCommand = new DelegateCommand<object>(EditEngineeringAnalysisCommandAction);

                //Get Max Value for offer amount
                Max_Value = CrmStartUp.GetOfferMaxValue();

                //fill users current site detail.
                CurrentCompany = CrmStartUp.GetCurrentPlantId(GeosApplication.Instance.ActiveUser.IdUser);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                }
                //GridRowHeight = false;
                // LeftPanelHeight = new GridLength(180, GridUnitType.Auto);
                GeosApplication.Instance.Logger.Log("Constructor OrderEditViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OrderEditViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => SelectedIndexLeadSource)] + // Lead Source
                    me[BindableBase.GetPropertyName(() => SelectedIndexBusinessUnit)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => IsSiteResponsibleRemoved)]+
                    me[BindableBase.GetPropertyName(() => SelectedIndexOfferOwner)];



                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string selectedIndexLeadSourceProp = BindableBase.GetPropertyName(() => SelectedIndexLeadSource);   // Lead Source
                string selectedIndexBusinessUnitProp = BindableBase.GetPropertyName(() => SelectedIndexBusinessUnit);
                string isSiteResponsibleRemovedProp = BindableBase.GetPropertyName(() => IsSiteResponsibleRemoved);
                string SelectedIndexOfferOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexOfferOwner);

                string informationError = BindableBase.GetPropertyName(() => InformationError);
                if (columnName == selectedIndexLeadSourceProp) // Lead Source
                    return RequiredValidationRule.GetErrorMessage(selectedIndexLeadSourceProp, SelectedIndexLeadSource);
                else if (columnName == selectedIndexBusinessUnitProp)
                    return RequiredValidationRule.GetErrorMessage(selectedIndexBusinessUnitProp, SelectedIndexBusinessUnit);
                else if (columnName == isSiteResponsibleRemovedProp)
                    return RequiredValidationRule.GetErrorMessage(isSiteResponsibleRemovedProp, IsSiteResponsibleRemoved);
                else if (columnName == informationError)
                    return RequiredValidationRule.GetErrorMessage(informationError, InformationError);
                //[001] added
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
                GeosApplication.Instance.Logger.Log("Get an error in GetContactImage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return null;
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetContactImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
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
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="leadList"></param>
        public void InItOrder(IList<Offer> leadList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InItOrder ...", category: Category.Info, priority: Priority.Low);

                IsInIt = true;
                IsSplitVisible = false;
                SelectedLeadList = new List<Offer>();
                SelectedLeadList = leadList;
                VisibilityShipment = Visibility.Visible;

                if (!GeosApplication.Instance.IsPermissionReadOnly)
                {
                    IsReadonly = false;
                    IsAcceptControlEnableorder = true;
                }
                else
                {
                    IsReadonly = true;
                    IsAcceptControlEnableorder = false;
                }

                TempOfferCode = SelectedLeadList[0].Code;
                OfferCode = SelectedLeadList[0].Code;
                Description = SelectedLeadList[0].Description;

                ProductAndService();
                SelectedPlantDetails = SelectedLeadList[0].Site;
                if (!IsControlEnableorder)
                {
                    if (SelectedLeadList[0].POReceivedInDate != null)
                        OfferCloseDate = DateTime.Parse(SelectedLeadList[0].POReceivedInDate.ToString());
                }

                else if (SelectedLeadList[0].DeliveryDate != null)
                {
                    OfferCloseDate = DateTime.Parse(SelectedLeadList[0].DeliveryDate.ToString());
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
                OrderAmount = SelectedLeadList[0].Value;
                CompanyName = SelectedLeadList[0].Site.Customers[0].CustomerName + "-" + SelectedLeadList[0].Site.Name;
                IdSite = (Int16)SelectedLeadList[0].Site.IdCompany;
                FillCurrencyList();
                FillStatusList();
                FillCaroemsList();
                FillCompanyPlantList();
                FillGroupList();
                FillOfferType();
                FillConfidentialLevelList();
                FillGeosProjectsList();
                FillExistingActivitiesToBeLinkedToOffer();
                FillBusinessUnitList();
                FillLeadSourceList();   // Lead Source
                FillActivity();
                FillAllProductCategory();

                //OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime.Date;

                // If GeosStatus is "17-LOST" or "4-Cancelled" then do not set any MinValue to date edit.
                // Other than "17-LOST" or "4-Cancelled" then Set MinValue to Todays date.
                if (SelectedLeadList[0].GeosStatus != null && (SelectedLeadList[0].GeosStatus.IdOfferStatusType != 17 && SelectedLeadList[0].GeosStatus.IdOfferStatusType != 4))
                {
                    if (SelectedLeadList[0].OfferExpectedDate != null)
                        OfferCloseDateMinValue = DateTime.Parse(SelectedLeadList[0].OfferExpectedDate.ToString());
                    else
                        OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime;
                }

                try
                {
                    ListLogComments = new ObservableCollection<LogEntryByOffer>(CrmStartUp.GetAllCommentsByIdOffer(SelectedLeadList[0].IdOffer, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId).Select(cmpslt => cmpslt.ConnectPlantConstr).FirstOrDefault()).AsEnumerable());
                    SetUserProfileImage(ListLogComments);
                    RtfToPlaintext();

                    SelectedLeadList[0].CommentsByOffers = new List<LogEntryByOffer>(ListLogComments.Select(x => (LogEntryByOffer)x.Clone()));
                    ListChangeLog = new ObservableCollection<LogEntryByOffer>(CrmStartUp.GetAllLogEntriesByIdOffer(SelectedLeadList[0].IdOffer, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId).Select(cmpslt => cmpslt.ConnectPlantConstr).FirstOrDefault()).AsEnumerable());
                    ListShipment = new List<Shipment>();
                    ListShipment = CrmStartUp.GetAllShipmentsByOfferId((GeosApplication.Instance.CompanyList.Where(fgt => fgt.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId.ToString()).FirstOrDefault()), SelectedLeadList[0].IdOffer).OrderByDescending(a => a.DeliveryDate).ToList();
                   

                    CustomerPurchaseOrderList = CrmStartUp.GetOfferPurchaseOrders(GeosApplication.Instance.CompanyList.FirstOrDefault(cmp => cmp.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId),
                                                                                  GeosApplication.Instance.IdCurrencyByRegion,
                                                                                  SelectedLeadList[0].IdOffer,
                                                                     GeosApplication.Instance.CrmOfferYear);
                    // FillOrderContactList();
                    FillOfferOwnerList();
                    FillOfferToList();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in InItOrder() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in InItOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }

                // Background = Colors.Transparent;
                HoverBackground = Colors.Red;
                SelectedBackground = Colors.Transparent;
                //[001] added Change Method
                ShowStatusList = CrmStartUp.GetOfferDetailsById_V2037(SelectedLeadList[0].IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == selectedLeadList[0].Site.ConnectPlantId).FirstOrDefault());

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
                if (SelectedLeadList != null && SelectedLeadList.Count > 0
                    && SelectedLeadList[0].OptionsByOffers != null)
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

                //001
                if (SelectedLeadList[0].EngineeringAnalysis != null)
                {
                    IsEngAnalysisEnable = false;
                    IsEngAnalysis = true;
                    ExistingEngineeringAnalysis = (EngineeringAnalysis)SelectedLeadList[0].EngineeringAnalysis.Clone();
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

                GeosApplication.Instance.Logger.Log("Method InItOrder() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InItOrder() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Activity View
        /// </summary>
        /// <param name="obj"></param>
        private void LinkExistingActivityToOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LinkExistingActivityToOrder...", category: Category.Info, priority: Priority.Low);

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

                OrderActivityList.Add(activity);
                SelectedActivity = activity;

                if (SelectedLeadList[0].NewlyLinkedActivities == null)
                    SelectedLeadList[0].NewlyLinkedActivities = new List<Activity>();

                SelectedLeadList[0].NewlyLinkedActivities.Add(activity);
                ExistingActivitiesTobeLinked.Remove(activity);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method LinkExistingActivityToOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LinkExistingActivityToOrder() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LinkExistingActivityToOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LinkExistingActivityToOrder() method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    GeosApplication.Instance.Logger.Log("Get an error in QuantityEditValueChangedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
        }

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
                    GeosApplication.Instance.Logger.Log("Get an error in QuantityEditAfterCheckChangedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

            ChangeLogsEntry = new List<LogEntryByOffer>();

            if (SelectedLeadList[0].IdOffer != 0)
            {
                // Lead BusinessUnit
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

                // Geos Project
                string GeosProjectName = "";
                if (SelectedIndexGeosProject == -1)
                {
                    if (SelectedLeadList[0].IdCarProject.HasValue)
                    {
                        GeosProjectName = GeosProjectsList.Where(pr => pr.IdCarProject == SelectedLeadList[0].IdCarProject.Value).Select(gpr => gpr.Name).SingleOrDefault();

                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewProjectNullChanged").ToString(), GeosProjectName, "None"), IdLogEntryType = 7 });
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

                //Car OEM
                if (SelectedLeadList[0].IdCarOEM == null)
                {
                    if (SelectedIndexCarOEM > 0)
                        ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCarsOemNullChanged").ToString(), CaroemsList[SelectedIndexCarOEM].Name), IdLogEntryType = 7 });
                }
                else if (SelectedLeadList[0].IdCarOEM != CaroemsList[SelectedIndexCarOEM].IdCarOEM)
                {
                    if (SelectedLeadList[0].IdCarOEM.HasValue)
                        CaroemName = CaroemsList.Where(coem => coem.IdCarOEM == SelectedLeadList[0].IdCarOEM.Value).Select(gcoem => gcoem.Name).SingleOrDefault();
                    else
                        CaroemName = "---";

                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = SelectedLeadList[0].IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCarsOemChanged").ToString(), CaroemName, CaroemsList[SelectedIndexCarOEM].Name), IdLogEntryType = 7 });
                }

                //foreach (LogEntryByOffer item in ChangeLogCommentEntry)
                //{
                //    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = item.IdOffer, IdUser = item.IdUser, DateTime = item.DateTime, Comments = item.Comments, IdLogEntryType = item.IdLogEntryType, IdLogEntry = item.IdLogEntry, IsDeleted = item.IsDeleted, IsUpdate = item.IsUpdate, IsRtfText = item.IsRtfText });
                //}

                if (SelectedLeadList[0].NewlyLinkedActivities != null)
                {
                    foreach (Activity itemActivity in SelectedLeadList[0].NewlyLinkedActivities)
                    {
                        LogEntryByOffer logEntry = new LogEntryByOffer()
                        {
                            IdOffer = SelectedLeadList[0].IdOffer,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            DateTime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("OrderEditViewLogAddActivity").ToString(), itemActivity.LookupValue.Value, itemActivity.Subject),
                            IdLogEntryType = 18
                        };

                        ChangeLogsEntry.Add(logEntry);
                    }
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
                //[001] added
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


            }

            GeosApplication.Instance.Logger.Log("Method ChangeLogsDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for Order Offer details.
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="obj"></param>
        private void SaveOrder(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method SaveOrder ...", category: Category.Info, priority: Priority.Low);
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

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexLeadSource")); // Lead Source
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessUnit")); // Business Unit
                PropertyChanged(this, new PropertyChangedEventArgs("IsSiteResponsibleRemoved"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOfferOwner"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    ChangeLogsDetails();

                    OfferData = new Offer();
                    //OfferData = CrmStartUp.GetOfferDetailsById(selectedLeadList[0].IdOffer, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.CrmOfferYear, OfferData.Site.ConnectPlantConstr);
                    //[001] added Change Method
                    OfferData = CrmStartUp.GetOfferDetailsById_V2037(selectedLeadList[0].IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == selectedLeadList[0].Site.ConnectPlantId).FirstOrDefault());

                    OfferData.IdBusinessUnit = Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue);
                    OfferData.IdSource = Convert.ToByte(LeadSourceList[SelectedIndexLeadSource].IdLookupValue); // Lead Source
                    OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    OfferData.IdSalesOwner = SalesOwnerList[SelectedIndexSalesOwner].IdPerson; // Lead Source

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

                    //code for take site connection string for save.
                    Company _Company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == selectedLeadList[0].Site.ConnectPlantId.ToString()).FirstOrDefault();
                    OfferData.Site.ConnectPlantId = _Company.ConnectPlantId;
                    OfferData.Site.ConnectPlantConstr = _Company.ConnectPlantConstr;

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
                            notification.Title = System.Windows.Application.Current.FindResource("AddNewProjectViewTitle").ToString();  // "New Project";
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


                    #region Contact
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
                        //            offerContact.IsDeleted = false;
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
                        GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                    #endregion 

                    OfferData.OfferedBy = OfferOwnerList[SelectedIndexOfferOwner].IdUser;

                    if (ChangeLogsEntry != null)
                    {
                        OfferData.LogEntryByOffers = ChangeLogsEntry;
                    }

                    //Add existing activity in an order.
                    OfferData.NewlyLinkedActivities = SelectedLeadList[0].NewlyLinkedActivities;

                    // For Comments
                    //SelectedLeadList[0].
                    OfferData.CommentsByOffers = new List<LogEntryByOffer>();
                    // OfferData.CommentsByOffers = ListLogComments.Where(x => x.IdLogEntry == 0 || x.IsDeleted || x.IsUpdate).ToList();

                    OfferData.CommentsByOffers = ListLogComments.Where(x => x.IdLogEntry == 0 || x.IsDeleted || x.IsUpdate || x.IdLogEntry == 1).ToList();

                    foreach (LogEntryByOffer item in SelectedLeadList[0].CommentsByOffers)
                    {
                        if (!ListLogComments.Any(x => x.IdLogEntry == item.IdLogEntry))
                        {
                            item.IsDeleted = true;
                            OfferData.CommentsByOffers.Add(item);
                        }
                    }

                    OfferData.CommentsByOffers.ForEach(it => it.People = null);
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
                    //bool isOrderUpdate = CrmStartUp.UpdateOrderByIdOffer_V2031(OfferData);
                    bool isOrderUpdate = CrmStartUp.UpdateOrderByIdOffer_V2037(OfferData);
                    OfferData.Source = LeadSourceList[SelectedIndexLeadSource];
                    OfferData.BusinessUnit = BusinessUnitList[SelectedIndexBusinessUnit];
                    OfferData.SalesOwner = SalesOwnerList[SelectedIndexSalesOwner];

                    IsBusy = false;

                    if (isOrderUpdate)
                    {
                       CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OrderMsgUpdateOrderSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OrderMsgUpdateOrderFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method SaveOrder() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveOrder() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in SaveOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in SaveOrder() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            IsBusy = false;
        }


        /// <summary>
        /// Method for fill Company group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                IList<Customer> TempCompanyGroupList = null;

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    // TempCompanyGroupList = CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                        //SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer);
                    }
                    else
                    {

                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                        //SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer);
                    }
                }
                else
                {
                    //TempCompanyGroupList = CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                        //SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer);
                    }
                    else
                    {

                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                        // SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer);
                    }
                }

                //CompanyGroupList = new List<Customer>();
                //CompanyGroupList.Insert(0, new Customer() { CustomerName = "---" });
                //CompanyGroupList.AddRange(TempCompanyGroupList);

             SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedLeadList[0].Site.Customers[0].IdCustomer));

                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosProjectsListTemp = GeosProjectsList.Select(tpg => tpg.Name).ToList();

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

                GeosApplication.Instance.Logger.Log("Method FillGeosProjectsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                            GeosStatusList.RemoveAt(i);
                        }
                        else
                            GeosStatusList[i].IsEnabled = true;
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
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                        || GeosStatusListSplit[i].IdOfferStatusType == 14 || GeosStatusListSplit[i].IdOfferStatusType == 4 || GeosStatusListSplit[i].IdOfferStatusType == 17)
                        {
                            GeosStatusListSplit.RemoveAt(i);

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
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusListSplit() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusListSplit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusListSplit() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
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

                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    else
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
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
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
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
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                OrderActivityList = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesByIdOffer(SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId)));
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return null;
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
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return imgSrc;
        }

        /// <summary>
        /// Method for fill Company Plant list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    // CompanyPlantList = CrmStartUp.GetSelectedUserCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, salesOwnersIds);
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
                    // CompanyPlantList = CrmStartUp.GetCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);

                    }

                }

              SelectedIndexCompanyPlant = EntireCompanyPlantList.IndexOf(EntireCompanyPlantList.FirstOrDefault(i => i.IdCompany == SelectedLeadList[0].Site.IdCompany));
                //if (SelectedIndexCompanyPlant == -1)
                //    SelectedIndexCompanyPlant = 0;

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

                if (!IsReadonly)
                {

                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderOrderEdit").ToString();
                    LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadEditPODate").ToString();
                }
                else
                {
                    LeadsEditViewTitle = System.Windows.Application.Current.FindResource("LeadsEditViewHeaderOrderView").ToString();
                    LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadEditPODate").ToString();
                }

                GeosApplication.Instance.Logger.Log("Method FillOfferType() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferType() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                SelectedIndexCurrency = Currencies.FindIndex(i => i.IdCurrency == SelectedLeadList[0].Currency.IdCurrency);

                GeosApplication.Instance.Logger.Log("Method FillCurrencyList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in FillConfidentialLevelList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for show Lost,Accept and Forecasted detail and popup windows,
        /// for fill reason and close date on window if they selected on GeosStatus List.
        /// </summary>
        private void StatusChangeAction()
        {
            try
            {
                IsBusy = true;
                IsSplitVisible = true;
                GeosApplication.Instance.Logger.Log("Method StatusChangeAction ...", category: Category.Info, priority: Priority.Low);
                //IsStatusChangeAction = false;
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

                                OfferCloseDate = GeosApplication.Instance.ServerDateTime.Date;

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
                    if (SelectedGeosStatus != null && SelectedGeosStatus.IdOfferStatusType == 17)
                    {
                        VisibilityLost = Visibility.Visible;
                        VisibilityAccept = Visibility.Hidden;
                    }
                    else
                    {
                        VisibilityLost = Visibility.Hidden;
                        VisibilityAccept = Visibility.Visible;
                    }
                    IsSplitVisible = false;
                }


                //ONLY QUOTED
                if (SelectedGeosStatus.IdOfferStatusType == 1)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;
                    else
                        GridRowHeightForRfq = false;

                    if (SelectedLeadList[0].SendIn != null && SelectedLeadList[0].SendIn != DateTime.MinValue)
                        GridRowHeightForQuoteSent = true;
                    else
                        GridRowHeightForQuoteSent = true;
                }
                //WAITING FOR QUOTE
                if (SelectedGeosStatus.IdOfferStatusType == 2)
                {
                    if (SelectedLeadList[0].RFQReception != null && SelectedLeadList[0].RFQReception != DateTime.MinValue)
                        GridRowHeightForRfq = true;
                    else
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
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method StatusChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in StatusChangeAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

                offerOptions = CrmStartUp.GetAllOfferOptions();

                //code for items to remove from products
                List<Int64> tempremovelist = new List<Int64>() { 6, 19, 21, 23, 25, 27 };
                offerOptions = offerOptions.Where(t => !tempremovelist.Contains(t.IdOfferOption)).ToList();

                string gp;
                int i = 1;

                foreach (var offerOptionsitem in offerOptions.GroupBy(c => c.IdOfferOptionType))
                {
                    List<OfferOption> offerOptionsitems = offerOptionsitem.ToList();
                    drw = Dttable.NewRow();

                    drw["Name"] = offerOptionsitems[0].OfferOptionType.Name.ToString();
                    drw["idOfferOption"] = "Group" + i.ToString();
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

                List<DataRow> list = Dttable.AsEnumerable().ToList();

                GeosApplication.Instance.Logger.Log("Method ProductAndService() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Warning in amounts greater than 50000 EUR.
        /// [001][skale][2019-08-04][GEOS2-239] Wrong warning message in offer popup
        /// </summary>
        /// <param name="obj"></param>
        private void OrderAmountLostFocusCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method OrderAmountLostFocusCommandAction ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            if (OrderAmount > 0)
            {
                Double amount = OrderAmount * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : 1);
                if (amount > Max_Value && Max_Value > 0) //[001] Added
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadsAddUpdateAmountWarning"].ToString(), Max_Value), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }
            }
            IsBusy = false;

            GeosApplication.Instance.Logger.Log("Method OrderAmountLostFocusCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
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
                isFromShowAll = false;

                GeosApplication.Instance.Logger.Log("Method ShowAllCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllCheckedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                isFromShowAll = false;

                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllUncheckedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

            // Update comment.
            if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString())
            {
                if (!string.IsNullOrEmpty(LeadComment) && !string.IsNullOrEmpty(LeadComment.Trim()))
                {
                    LogEntryByOffer comment = ListLogComments.FirstOrDefault(x => x.Comments == OldLeadComment);

                    if (comment != null)
                    {
                        comment.Comments = string.Copy(LeadComment.Trim());
                        comment.DateTime = GeosApplication.Instance.ServerDateTime;
                        SelectedComment = comment;
                        comment.IsUpdate = true;
                        comment.IsDeleted = false;
                        comment.IsRtfText = comment.IsRtfText;
                        if (IsRtf)
                            comment.IsRtfText = true;
                        else if (IsNormal)
                            comment.IsRtfText = false;
                    }

                    OldLeadComment = null;
                    LeadComment = null;

                    ListLogComments = new ObservableCollection<LogEntryByOffer>(ListLogComments);
                }
            }

            // add comment
            else if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString()) //Add comment.
            {
                if (!string.IsNullOrEmpty(LeadComment) && !string.IsNullOrEmpty(LeadComment.Trim())) // Add Comment
                {
                    if (IsRtf)
                    {
                        LogEntryByOffer comment = new LogEntryByOffer()
                        {
                            People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            IdOffer = SelectedLeadList[0].IdOffer,
                            DateTime = GeosApplication.Instance.ServerDateTime,
                            IsUpdate = false,
                            IsDeleted = false,
                            Comments = string.Copy(LeadComment.Trim()),
                            IdLogEntryType = 1,
                            IsRtfText = true
                        };
                        comment.People.OwnerImage = SetUserProfileImage();
                        ListLogComments.Add(comment);
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
                            IsRtfText = false
                        };
                        comment.People.OwnerImage = SetUserProfileImage();
                        ListLogComments.Add(comment);
                    }

                    OldLeadComment = null;
                    LeadComment = null;
                }
            }

            document.Blocks.Clear();
            ShowCommentsFlyout = false;
            //((GridControl)gcComments).Focus();
            IsRtf = false;
            IsNormal = true;
            //IsBusy = false;
            //LeadComment = "";

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
                    if (ListLogComments != null && ListLogComments.Count > 0)
                    {
                        ListLogComments.Remove(ListLogComments.FirstOrDefault(x => x.IdLogEntry == commentObject.IdLogEntry && x.Comments == commentObject.Comments));
                    }
                }
            }

            ShowCommentsFlyout = false;
            LeadComment = "";

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Show Lost Opportunity Window
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
            }

            GeosApplication.Instance.Logger.Log("Method ShowLostOpportunityWindow() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CommentButtonCheckedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentButtonCheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            LeadComment = "";
            OldLeadComment = "";


            GeosApplication.Instance.Logger.Log("Method CommentButtonCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CommentButtonUncheckedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentButtonUncheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            LeadComment = "";
            OldLeadComment = "";
            IsRtf = false;
            IsNormal = true;

            GeosApplication.Instance.Logger.Log("Method CommentButtonUncheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void EditShipmentAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditShipmentAction ...", category: Category.Info, priority: Priority.Low);

            if (obj == null) return;
            Shipment ship = (Shipment)obj;

            try
            {
                ListBox = new List<PackingBox>();
                ListBox = CrmStartUp.GetAllPackingBoxesByShipmentId(GeosApplication.Instance.CompanyList.Where(fgt => fgt.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId.ToString()).FirstOrDefault(), ship.IdShipment);
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
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in EditShipmentAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
        public void CutomerContactCheckedAction(GridColumnDataEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method CutomerContactCheckedAction ...", category: Category.Info, priority: Priority.Low);

            var item = this.ListCustomerContact[e.ListSourceRowIndex];
            if (e.IsGetData)
            {
                People p = this.ListAddedContact.FirstOrDefault(x => x.IdPerson == item.IdPerson);
                if (p != null)
                {
                    e.Value = true;
                }
                else
                {
                    e.Value = false;
                }
            }
            else
            {
                var isSelected = (bool)e.Value;
                if (isSelected)
                {
                    if (item.OwnerImage == null)
                    {
                        User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(item.IdPerson));
                        try
                        {
                            UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(user.Login);
                            item.OwnerImage = byteArrayToImage(UserProfileImageByte);
                        }
                        catch (Exception ex)
                        {
                            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (item.IdPersonGender == 1)
                                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                else
                                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                            }
                            else
                            {
                                if (item.IdPersonGender == 1)
                                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                else
                                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            }
                        }
                    }
                    this.ListAddedContact.Add(item);
                    //isSelected = false;
                }
                else
                {
                    People p = this.ListAddedContact.FirstOrDefault(x => x.IdPerson == item.IdPerson);
                    if (p != null)
                    {
                        this.ListAddedContact.Remove(p);
                    }
                }
            }

            GeosApplication.Instance.Logger.Log("Method CutomerContactCheckedAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// This Method for Add or Remove Contact in offer through check uncheck
        /// </summary>
        /// <param name="e"></param>
        //public void GetSalesContactCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method GetSalesContactCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    if (GeosApplication.Instance.IsPermissionReadOnly)
        //    {
        //        return;
        //    }
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
        //            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
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
        //        if (GeosApplication.Instance.IsPermissionReadOnly)
        //        {
        //            return;
        //        }

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
        //        GeosApplication.Instance.Logger.Log("Get an error in AssignedSalesCancelCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
        //        GeosApplication.Instance.Logger.Log("Get an error in SetSalesResponsibleCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}

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

        //private void FillOrderContactList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillOfferContactList ...", category: Category.Info, priority: Priority.Low);

        //        ListCustomerContact = new ObservableCollection<People>(CrmStartUp.GetContactsOfSiteByOfferId(IdSite).AsEnumerable().OrderBy(x => x.FullName));
        //        //ListOfferContact = new ObservableCollection<OfferContact>(CrmStartUp.GetOfferContact(SelectedLeadList[0].IdOffer, SelectedLeadList[0].Site.ConnectPlantConstr).AsEnumerable());
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
        //                if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
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
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOfferContactList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOfferContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOfferContactList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}
    

        ///// <summary>
        ///// Method for open MailTo in Outlook for send Email. 
        ///// </summary>
        ///// <param name="obj"></param>
        //public void SendMailtoPerson(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
        //        IsBusy = true;
        //        string emailAddess = Convert.ToString(obj);
        //        string command = "mailto:" + emailAddess;
        //        System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
        //        myProcess.StartInfo.FileName = command;
        //        myProcess.StartInfo.UseShellExecute = true;
        //        myProcess.StartInfo.RedirectStandardOutput = false;
        //        myProcess.Start();
        //        GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        IsBusy = false;
        //        GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}

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
        /// This Method for add new project.
        /// </summary>
        public void AddNewProjectCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddNewProjectCommandAction ...", category: Category.Info, priority: Priority.Low);
            if (!IsReadonly)
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

                        //workbook.Worksheets.Insert(0,SheetName);
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
                GeosApplication.Instance.Logger.Log("Get an error in ExportCommentsLeadGridButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

        /// <summary>
        /// Method for add new activity for order.
        /// </summary>
        private void AddActivity()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCtivity ...", category: Category.Info, priority: Priority.Low);

                List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();
                List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();
                Activity NewActivity = new Activity();

                NewActivity.IdActivityType = 38;
                NewActivity.Subject = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                NewActivity.Description = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                NewActivity.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                NewActivity.IsCompleted = 0;
                NewActivity.IdOwner = SalesOwnerList[SelectedIndexSalesOwner].IdPerson;               // ActivityOwnerList[SelectedIndexOwner].IdUser;
                NewActivity.ActivityTags = null;
                NewActivity.IsSentMail = 0;
                NewActivity.FromDate = GeosApplication.Instance.ServerDateTime.AddDays(1);
                NewActivity.ToDate = GeosApplication.Instance.ServerDateTime.AddDays(1);


                //Fill Account details.
                ActivityLinkedItem _ActivityLinkedItem = new ActivityLinkedItem();
                _ActivityLinkedItem.Company = new Company();
                _ActivityLinkedItem.Company.Customers = new List<Customer>();
                _ActivityLinkedItem.IdLinkedItemType = 42;
                _ActivityLinkedItem.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                _ActivityLinkedItem.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                _ActivityLinkedItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                _ActivityLinkedItem.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;

                _ActivityLinkedItem.LinkedItemType = new LookupValue();
                _ActivityLinkedItem.LinkedItemType.IdLookupValue = 42;
                _ActivityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                _ActivityLinkedItem.IsVisible = false;

                listActivityLinkedItems.Add(_ActivityLinkedItem);

                //Fill Opportunity details.
                ActivityLinkedItem _ActivityLinkedItem1 = new ActivityLinkedItem();
                _ActivityLinkedItem1 = (ActivityLinkedItem)_ActivityLinkedItem.Clone();
                _ActivityLinkedItem1.IdLinkedItemType = 44;
                _ActivityLinkedItem1.Name = offerCode;
                _ActivityLinkedItem1.IdSite = null;
                _ActivityLinkedItem1.IdOffer = SelectedLeadList[0].IdOffer;
                _ActivityLinkedItem1.IdEmdepSite = Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId);

                _ActivityLinkedItem1.LinkedItemType = new LookupValue();
                _ActivityLinkedItem1.LinkedItemType.IdLookupValue = 44;
                _ActivityLinkedItem1.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOrder").ToString();

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
                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOrder").ToString(), item.Name), IdLogEntryType = 2 });
                    }
                }

                NewActivity.LogEntriesByActivity = logEntriesByActivity;
                NewActivity.ActivityLinkedItem = listActivityLinkedItems;
                NewActivity.IsDeleted = 0;
                NewActivity = CrmStartUp.AddActivity(NewActivity);

                GeosApplication.Instance.Logger.Log("Method AddCtivity() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

                    ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(salesOwnersIds, 21, "0", CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId)).ToList());
                }
                else if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(GeosApplication.Instance.ActiveUser.IdUser.ToString(), 22, plantOwnersIds, CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId)).ToList());
                }
                else
                {
                    ExistingActivitiesTobeLinked = new ObservableCollection<Activity>(CrmStartUp.GetActivitiesLinkedToAccount(GeosApplication.Instance.ActiveUser.IdUser.ToString(), 20, "0", CompanyPlantList[SelectedIndexCompanyPlant].IdCompany, SelectedLeadList[0].IdOffer, Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId)).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method FillExistingActivitiesToBeLinkedToOffer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExistingActivitiesToBeLinkedToOffer() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExistingActivitiesToBeLinkedToOffer() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillExistingActivitiesToBeLinkedToOffer() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for add new activity.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                AddActivityView addActivityView = new AddActivityView();
                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();

                List<Activity> _ActivityList = new List<Activity>();

                //**[Start] code for add Account Detail.

                Activity _Activity = new Activity();
                _Activity.ActivityLinkedItem = new List<ActivityLinkedItem>();

                //Fill Account details.
                ActivityLinkedItem _ActivityLinkedItem = new ActivityLinkedItem();
                _ActivityLinkedItem.Company = new Company();
                _ActivityLinkedItem.Company.Customers = new List<Customer>();
                _ActivityLinkedItem.IdLinkedItemType = 42;
                _ActivityLinkedItem.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                _ActivityLinkedItem.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                _ActivityLinkedItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                _ActivityLinkedItem.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                _ActivityLinkedItem.ActivityLinkedItemImage = null;
                _ActivityLinkedItem.LinkedItemType = new LookupValue();
                _ActivityLinkedItem.LinkedItemType.IdLookupValue = 42;
                _ActivityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                _ActivityLinkedItem.IsVisible = false;

                addActivityViewModel.IsAddedFromOutSide = true;
                addActivityViewModel.SelectedIndexCompanyGroup = SelectedIndexCompanyGroup;
                addActivityViewModel.SelectedIndexCompanyPlant = addActivityViewModel.CompanyPlantList.IndexOf(addActivityViewModel.CompanyPlantList.FirstOrDefault(x => x.IdCompany == CompanyPlantList[SelectedIndexCompanyPlant].IdCompany));
                //addActivityViewModel.ListPlantOpportunity


                _Activity.ActivityLinkedItem.Add(_ActivityLinkedItem);

                //Fill Opportunity details.
                ActivityLinkedItem _ActivityLinkedItem1 = new ActivityLinkedItem();
                _ActivityLinkedItem1 = (ActivityLinkedItem)_ActivityLinkedItem.Clone();
                _ActivityLinkedItem1.IdLinkedItemType = 44;
                _ActivityLinkedItem1.Name = offerCode;
                _ActivityLinkedItem1.IdSite = null;
                _ActivityLinkedItem1.IdOffer = SelectedLeadList[0].IdOffer;
                _ActivityLinkedItem1.IdEmdepSite = Convert.ToInt32(SelectedLeadList[0].Site.ConnectPlantId);
                _ActivityLinkedItem.ActivityLinkedItemImage = null;
                _ActivityLinkedItem1.LinkedItemType = new LookupValue();
                _ActivityLinkedItem1.LinkedItemType.IdLookupValue = 44;
                _ActivityLinkedItem1.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOrder").ToString();

                _Activity.ActivityLinkedItem.Add(_ActivityLinkedItem1);

                _ActivityList.Add(_Activity);
                addActivityViewModel.IsInternalEnable = false;


                addActivityViewModel.Init(_ActivityList);

                if (IsActivityCreateFromSaveOffer)
                {
                    addActivityViewModel.SelectedIndexType = addActivityViewModel.TypeList.IndexOf(tl => tl.IdLookupValue == 38);
                    addActivityViewModel.Subject = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                    addActivityViewModel.Description = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                    addActivityViewModel.DueDate = GeosApplication.Instance.ServerDateTime.AddDays(1);
                }
                //**[End] code for add Account Detail.

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

                if (addActivityViewModel.IsActivitySave)
                {
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

                        OrderActivityList.Add(newActivity);

                    }

                    OrderActivityList = new ObservableCollection<Activity>(OrderActivityList);
                    SelectedActivity = OrderActivityList.Last();

                }

                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShow() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

                    foreach (ActivityLinkedItem item in activity.ActivityLinkedItem)
                    {
                        item.ActivityLinkedItemImage = null;
                    }

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
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error On ConvertZipToByte Method", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in SelectedConfidentialLevel() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][2018-07-10][skhade][CRM-M042-18] Eng. analysis option in Orders.
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

            addEngineeringAnalysisViewModel.EngAnalysis = ExistingEngineeringAnalysis;
            addEngineeringAnalysisViewModel.EngAnalysisDuplicate = ExistingEngineeringAnalysis;

            addEngineeringAnalysisViewModel.InitOrder(SelectedGeosStatus);
            addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            addEngineeringAnalysisView.ShowDialogWindow();
        }

        public void Dispose()
        {
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
        /// </summary>
        private void FillOfferToList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOfferToList() ...", category: Category.Info, priority: Priority.Low);

                OfferToList = new List<OfferContact>();

                Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];

                OfferToList = CrmStartUp.GetContactsOfCustomerGroupByOfferId(customer.IdCustomer);

                ListOfferContact = new ObservableCollection<OfferContact>(CrmStartUp.GetOfferContact(SelectedLeadList[0].IdOffer, GeosApplication.Instance.CompanyList.Where(fgt => fgt.ConnectPlantId == SelectedLeadList[0].Site.ConnectPlantId.ToString()).Select(fgt => fgt.ConnectPlantConstr).FirstOrDefault()).AsEnumerable());

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
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


    }

    #endregion
}

