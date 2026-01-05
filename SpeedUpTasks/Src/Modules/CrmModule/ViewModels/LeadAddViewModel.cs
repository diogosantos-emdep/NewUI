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
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Xpf.Core;
using System.Data;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.Editors;
using Prism.Logging;
using System.ServiceModel;
using Emdep.Geos.UI.Validations;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    [POCOViewModel]
    public class LeadAddViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {

        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        // [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities

        #endregion

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private IList<Customer> companyList;
        private List<Customer> companyListnew;
        ObservableCollection<GeosStatus> geosStatusList;
        List<Currency> currencies;
        //private IList<string> selectedLeadList;
        //private IList<Template> leadsList;
        private List<string> geosProjectsListTemp;
        public List<OfferType> OfferTypeList { get; set; }
        public List<int> ConfidentialLevelList { get; private set; }
        private Visibility visibilityForecast = Visibility.Hidden;
        private string informationError;


        private string offerCode;
        private string description;
        private string salesOwnersIds = "";
        private long OfferNumber;
        private int selectedIndexOfferType = 0;
        private int selectedIndexCustomer = 0;
        private int selectedIndexCurrency = 0;
        private int selectedIndexConfidentialLevel;
        private int selectedIndexSalesOwner = 0;
        private int selectedIndexBusinessUnit = 0;
        private int selectedIndexLeadSource = 0;    // Lead Source

        private ObservableCollection<Customer> companyGroupList;
        private List<Company> companyPlantList;
        private List<People> salesOwnerList;

        private int selectedIndexCompanyGroup; // = 0;
        private int selectedIndexCompanyPlant = 0;
        private double offerAmount;
        Double max_Value;
        //Double max_Value_TaskSuggestion;
        private DateTime? offerCloseDate;
        private DateTime? rfqReceptionDate;
        private DateTime? quoteSentDate;
        public string MinDeliveryDate { get; set; }
        private DateTime offerCloseDateMinValue;

        private IList<Template> templateList;
        private IList<Offer> listSelectedLead;
        public List<Quotation> TemplateDetailList { get; set; }
        public List<OptionsByOffer> OptionsByOfferList { get; set; }

        private int selectedStarIndex;
        public virtual Color Background { get; set; }
        private Color hoverBackground;
        private Color selectedBackground;
        public IList<LookupValue> BusinessUnitList { get; set; }
        byte[] UserProfileImageByte = null;
        private float quantity;
        private List<string> errorList;
        private bool isShowAll;

        private int selectedIndexCarOEM;
        private int selectedIndexGeosProject;

        private List<CarOEM> caroemsList;
        private List<CarProject> geosProjectsList;
        private bool isFromShowAll = false;
        bool ischeck = false;
        private Offer offerData;
        bool isBusy;
        private int productAndServicesCount;
        private GeosStatus selectedGeosStatus;

        private ObservableCollection<LookupValue> leadSourceList;   // Lead Source
        private bool gridRowHeightForRfq;
        private bool gridRowHeightForQuoteSent;
        private string rfq;

        private ObservableCollection<LogEntryByOffer> listLogComments = new ObservableCollection<LogEntryByOffer>();
        private ObservableCollection<LogEntryByOffer> listChangeLog = new ObservableCollection<LogEntryByOffer>();
        private OfferContact primaryOfferContact;

        private ObservableCollection<OfferContact> listOfferContact = new ObservableCollection<OfferContact>();
        private ObservableCollection<People> listAddedContact = new ObservableCollection<People>();
        private string previousPrimaryContact;
        private bool isPrimayContactChanged;
        private bool isFirstPrimaryContact;
        private List<ActivityTemplateTrigger> activityTemplateTriggers;


        //Engineering Analysis

        bool isEngAnalysis;
        //Visibility isEngAnalysisVisible;
        Visibility isEngAnalysisButtonVisible = Visibility.Collapsed;

        //private bool isExistEngAnalysis;
        private bool isEngAnalysisEnable = true;
        //private int rowNumberChangebyEngAnalysisRow0;//for adjustment of column height for Engineering analysis
        //private int rowNumberChangebyEngAnalysisRow1;//for adjustment of column height for Engineering analysis
        //private int rowNumberChangebyEngAnalysisRow2;//for adjustment of column height for Engineering analysis
        //private int rowNumberChangebyEngAnalysisRow3;//for adjustment of column height for Engineering analysis

        private EngineeringAnalysis existedEngineeringAnalysis;
        private EngineeringAnalysis existedEngineeringAnalysisDuplicate;
        FileUploader engAnalysisAttachmentFileUploadIndicator;
        private ObservableCollection<Company> entireCompanyPlantList;

        //EngineeringAnalysis

        //[001] added

        private List<OfferContact> offerToList;
        private List<Object> selectedOfferToList = new List<object>();
        private List<User> offerOwnerList;
        private int selectedIndexOfferOwner;
        private string visible;
        private LostReasonsByOffer lostReasonsByOffer;
        #endregion

        #region  public Properties
        private bool IsOpenLostOpportunity { get; set; }
        public Offer OfferReturnValue = null;
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }

        //EngineeringAnalysis
        public EngineeringAnalysis ExistedEngineeringAnalysisDuplicate
        {
            get { return existedEngineeringAnalysisDuplicate; }
            set
            {
                existedEngineeringAnalysisDuplicate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistedEngineeringAnalysisDuplicate"));
            }
        }


        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
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
            get { return existedEngineeringAnalysis; }
            set
            {
                existedEngineeringAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistedEngineeringAnalysis"));
            }
        }

        public Visibility IsEngAnalysisButtonVisible
        {
            get { return isEngAnalysisButtonVisible; }
            set
            {
                isEngAnalysisButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisButtonVisible"));
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

        //public Visibility IsEngAnalysisVisible
        //{
        //    get { return isEngAnalysisVisible; }
        //    set
        //    {
        //        isEngAnalysisVisible = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisVisible"));
        //    }
        //}

        public bool IsEngAnalysis
        {
            get { return isEngAnalysis; }
            set
            {
                isEngAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysis"));
            }
        }

        //public bool IsExistEngAnalysis
        //{
        //    get { return isExistEngAnalysis; }
        //    set
        //    {
        //        isExistEngAnalysis = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsExistEngAnalysis"));
        //    }
        //}

        //EngineeringAnalysis

        public bool IsActivityCreateFromSaveOffer = false;
        public int ProductAndServicesCount
        {
            get { return productAndServicesCount; }
            set
            {
                productAndServicesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductAndServicesCount"));
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

        List<OfferOption> offerOptions { get; set; }

        public Offer OfferData
        {
            get { return offerData; }
            set
            {
                offerData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferData"));
            }
        }

        public IList<Template> TemplateList
        {
            get { return templateList; }
            set { templateList = value; }
        }

        public IList<Customer> CompanyList
        {
            get { return companyList; }
            set { companyList = value; }
        }

        public List<Customer> CompanyListnew
        {
            get { return companyListnew; }
            set { companyListnew = value; }
        }

        public ObservableCollection<GeosStatus> GeosStatusList
        {
            get { return geosStatusList; }
            set
            {
                geosStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosStatusList"));
            }
        }

        public int SelectedIndexCustomer
        {
            get { return selectedIndexCustomer; }
            set
            {
                selectedIndexCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCustomer"));
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

        public string Description
        {
            get { return description; }
            set
            {
                if (value != null)
                {
                    description = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public int SelectedIndexOfferType
        {
            get { return selectedIndexOfferType; }
            set
            {

                if (value != SelectedIndexOfferType)
                {
                    //IsBusy = true;
                    selectedIndexOfferType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOfferType"));

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

                    //if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 10)
                    //    ProductAndService();
                    //else
                    //    ProductAndService();

                    GenerateOfferCode();
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //IsBusy = false;
                }

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

        public string OfferCode
        {
            get { return offerCode; }
            set
            {
                offerCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCode"));
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

        public Visibility VisibilityForecast
        {
            get { return visibilityForecast; }
            set
            {
                visibilityForecast = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibilityForecast"));
            }
        }

        public IList<Offer> ListSelectedLead
        {
            get { return listSelectedLead; }
            set { listSelectedLead = value; }
        }

        public float Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(new PropertyChangedEventArgs("Quantity")); }
        }

        public ObservableCollection<LogEntryByOffer> ListLogComments
        {
            get { return listLogComments; }
            set
            {
                SetProperty(ref listLogComments, value, () => ListLogComments);
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

        public List<People> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerList"));
            }
        }

        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                if (value != SelectedIndexCompanyGroup)
                {
                    //IsBusy = true;
                    selectedIndexCompanyGroup = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

                    if (SelectedIndexCompanyGroup > 0)
                    {
                        //FillCompanyPlantList();

                        CompanyPlantList = new List<Company>();
                        CompanyPlantList = EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList();
                        if (CompanyPlantList.Count > 0)
                            SelectedIndexCompanyPlant = 1;
                        else
                            SelectedIndexCompanyPlant = 0;
                        //FillGeosProjectsList();
                    }
                    else
                    {
                        SelectedIndexCompanyPlant = -1;
                        CompanyPlantList = null;
                        GeosProjectsListTemp = null;
                    }
                    // IsBusy = false;
                }

                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
        }

        public int SelectedIndexSalesOwner
        {
            get { return selectedIndexSalesOwner; }
            set
            {
                selectedIndexSalesOwner = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSalesOwner"));
            }
        }

        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set
            {
                if (selectedIndexCompanyPlant != value)
                {
                    //ListAddedContact.Clear();
                    PrimaryOfferContact = null;
                }
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));

                if (selectedIndexCompanyPlant > 0)
                {
                    FillSalesOwnerList();
                    //FillContactAsPerIdSite();
                    FillOfferToList();
                }
                else
                {
                    SelectedIndexSalesOwner = -1;
                    SalesOwnerList = null;
                    OfferToList = null;
                    SelectedOfferToList = null;

                }
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

        public int SelectedIndexLeadSource
        {
            get { return selectedIndexLeadSource; }
            set
            {
                selectedIndexLeadSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLeadSource"));
            }
        }

        DataTable dttable;
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

                //if (SelectedIndexGeosProject !=-1 && GeosProjectsList != null && GeosProjectsList.Count != 0 && GeosProjectsList[SelectedIndexGeosProject] != null)
                if (SelectedIndexGeosProject != -1)
                    SelectedIndexCarOEM = CaroemsList.FindIndex(cr => cr.IdCarOEM == GeosProjectsList[SelectedIndexGeosProject].IdCarOem);

                else if (SelectedIndexGeosProject == -1)
                    SelectedIndexCarOEM = 0;
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

        public ObservableCollection<Column> Columns { get; private set; }

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

        public GeosStatus SelectedGeosStatus
        {
            get { return selectedGeosStatus; }
            set
            {
                selectedGeosStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGeosStatus"));
                //IsExistEngAnalysis = false;
                try
                {
                    //IsExistEngAnalysis = true;
                    //IsEngAnalysisButtonVisible = Visibility.Visible;
                    if (SelectedGeosStatus.IdOfferStatusType == 19)
                    {
                        if (IsOpenLostOpportunity == false)
                        {
                            IsOpenLostOpportunity = true;
                            ShowLostOpportunityWindow(null);
                        }
                    }
                    // This part is used to apply validation on RFQReceptionDate/QuoteSentDate.
                    if (SelectedGeosStatus.IdOfferStatusType == 2 || SelectedGeosStatus.IdOfferStatusType == 1)
                    {
                        IsEngAnalysisEnable = true;

                        //if (IsEngAnalysis == true)
                        //{
                        //    IsExistEngAnalysis = true;
                        //}
                        //else
                        //{
                        //    IsExistEngAnalysis = true;
                        //    //IsEngAnalysisButtonVisible = Visibility.Visible;
                        //}

                        //IsEngAnalysisButtonVisible = Visibility.Visible;
                        SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == 1);
                        string error = EnableValidationAndGetError();
                        IsOpenLostOpportunity = false;
                    }
                    else
                    {
                        IsEngAnalysisEnable = false;
                        SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == 10);
                        if (SelectedGeosStatus.IdOfferStatusType != 19 && IsOpenLostOpportunity == true)
                        {
                            IsOpenLostOpportunity = false;
                        }
                            //ExistedEngineeringAnalysis = new EngineeringAnalysis();
                            //ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();

                            //ExistedEngineeringAnalysis = new EngineeringAnalysis();
                            //ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();
                            //EngAnalysisAttachmentFileUploadIndicator = new FileUploader();

                            //OptionsByOffer optionsByOffer = new OptionsByOffer();
                            //optionsByOffer.IdOption = 25;
                            //optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                            //optionsByOffer.IsSelected = false;
                            //optionsByOffer.Quantity = 1;
                            //if (optionsByOffer.IdOption != 0)
                            //{
                            //    if (OptionsByOfferList == null)
                            //    {
                            //        OptionsByOfferList = new List<OptionsByOffer>();

                            //    }
                            //    OptionsByOfferList.Add(optionsByOffer);
                            //}
                            //IsExistEngAnalysis = false;

                            //IsEngAnalysis = false;
                            //IsEngAnalysisButtonVisible = Visibility.Collapsed;
                        }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in SelectedGeosStatus Change " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RFQReceptionDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("QuoteSentDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("OfferAmount"));
                }

                if (SelectedGeosStatus.IdOfferStatusType == 1)
                {
                    GridRowHeightForQuoteSent = true;
                    GridRowHeightForRfq = true;
                    //IsExistEngAnalysis = true;
                    IsOpenLostOpportunity = false;
                }
                if (SelectedGeosStatus.IdOfferStatusType == 2)
                {
                    GridRowHeightForQuoteSent = false;
                    GridRowHeightForRfq = true;
                    //IsExistEngAnalysis = true;
                    IsOpenLostOpportunity = false;
                }
                if (SelectedGeosStatus.IdOfferStatusType == 15)
                {
                    GridRowHeightForQuoteSent = false;
                    GridRowHeightForRfq = false;
                    IsOpenLostOpportunity = false;
                }
                if (SelectedGeosStatus.IdOfferStatusType == 16)
                {
                    GridRowHeightForQuoteSent = false;
                    GridRowHeightForRfq = false;
                    IsOpenLostOpportunity = false;
                }
                
            }
        }

        bool fromNewLead;

        public bool FromNewLead
        {
            get { return fromNewLead; }
            set { fromNewLead = value; }
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

        private Int16 idSite;
        public Int16 IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        //public ObservableCollection<People> ListAddedContact
        //{
        //    get { return listAddedContact; }
        //    set
        //    {
        //        listAddedContact = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ListAddedContact"));
        //    }
        //}
        //public ObservableCollection<People> ListCustomerContact
        //{
        //    get { return listCustomerContact; }
        //    set
        //    {
        //        listCustomerContact = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ListCustomerContact"));

        //    }
        //}
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

        public string Rfq
        {
            get { return rfq; }
            set
            {
                rfq = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Rfq"));
            }
        }

        //[001]added
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

        public LostReasonsByOffer LostReasonsByOffer
        {
            get
            {
                return lostReasonsByOffer;
            }

            set
            {
                lostReasonsByOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LostReasonsByOffer"));
            }
        }
        #endregion

        #region public ICommand

        public ICommand LeadsViewWindowCancelButtonCommand { get; set; }
        public ICommand LeadsEditViewAcceptButtonCommand { get; set; }
        public ICommand LeadsEditViewCancelButtonCommand { get; set; }
        public ICommand QuantityEditValueChangedCommand { get; set; }
        public ICommand CheckEditQuantityValueChangedCommand { get; set; }
        public ICommand ShowAllCheckedCommand { get; set; }
        public ICommand ShowAllUncheckedCommand { get; set; }
        public ICommand CustomNodeFilterCommand { get; set; }
        //public ICommand GetCustomerContactCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand OfferAmountLostFocusCommand { get; set; }
        public ICommand CurrencySelectedIndexChangedCommand { get; set; }
        public ICommand AddProjectButtonCommand { get; set; }
        //[001]
        //public ICommand GetSalesContactCommand { get; set; }
        //public ICommand AssignedSalesCancelCommand { get; set; }
        //public ICommand SetSalesResponsibleCommand { get; set; }
        //public ICommand SetPrimaryContactCommand { get; set; }

        public ICommand EditEngineeringAnalysisCommand { get; set; }
        public ICommand IsEngineeringAnalysisCommand { get; set; }

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
        public LeadAddViewModel()
        {
            try
            {
                //AddLangRef addLangRef = new AddLangRef();
                //addLangRef.Init();
                CRMCommon.Instance.Init();

                GeosApplication.Instance.Logger.Log("Constructor LeadAddViewModel ...", category: Category.Info, priority: Priority.Low);

                LeadsViewWindowCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                LeadsEditViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveOffer));
                LeadsEditViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                QuantityEditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(QuantityEditValueChangedAction);
                CheckEditQuantityValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(QuantityEditAfterCheckChangedAction);
                CustomNodeFilterCommand = new DelegateCommand<DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs>(CustomNodeFilterCommandAction);
                OfferAmountLostFocusCommand = new DelegateCommand<object>(OfferAmountLostFocusCommandAction);
                CurrencySelectedIndexChangedCommand = new DelegateCommand<object>(CurrencySelectedIndexChangedCommandAction);

                ShowAllCheckedCommand = new RelayCommand(new Action<object>(ShowAllCheckedCommandAction));
                ShowAllUncheckedCommand = new RelayCommand(new Action<object>(ShowAllUncheckedCommandAction));
                //[001]
                //GetSalesContactCommand = new DelegateCommand<object>(GetSalesContactCommandAction);
                //AssignedSalesCancelCommand = new DelegateCommand<object>(AssignedSalesCancelCommandAction);
                //SetSalesResponsibleCommand = new DelegateCommand<object>(SetSalesResponsibleCommandAction);
                // SetPrimaryContactCommand = new DelegateCommand<object>(SetCommandAction);

                EditEngineeringAnalysisCommand = new DelegateCommand<object>(EditEngineeringAnalysisCommandAction);
                IsEngineeringAnalysisCommand = new RelayCommand(new Action<object>(ShowIsAnalysisWindow));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) =>
                {
                    SendMailtoPerson(obj);
                }));

                // SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChangedCommandAction);
                AddProjectButtonCommand = new DelegateCommand<object>(AddNewProjectCommandAction);

                MinDeliveryDate = GeosApplication.Instance.ServerDateTime.Date.ToString();

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                }

                //Parameter 1 for show warning for max ammout.
                Max_Value = CrmStartUp.GetOfferMaxValueById(1);

                //Parameter 2 for show task suggestion.
                activityTemplateTriggers = CrmStartUp.GetActivityTemplateTriggers();

                FillCurrencyList();
                FillStatusList();
                FillLeadsEdit();
                FillCustomerList();
                FillConfidentialLevelList();
                FillOfferType();
                FillGroupList();
                FillCompanyPlantList();
                FillCaroemsList();
                FillGeosProjectsList();
                FillBusinessUnitList();
                FillLeadSourceList();   // Lead Source
                ProductAndService();

                SelectedGeosStatus = GeosStatusList.FirstOrDefault(gsl => gsl.IdOfferStatusType == 15);

                HoverBackground = Colors.Red;
                SelectedBackground = Colors.Transparent;

                SelectedIndexConfidentialLevel = 10;
                SelectedConfidentialLevel();

                OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
                OfferCloseDate = null;

                OptionsByOfferList = new List<OptionsByOffer>();
                ProductAndServicesCount = OptionsByOfferList.Count;
                IsShowAll = true;
                FillOfferOwnerList();

                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }

                GeosApplication.Instance.Logger.Log("Constructor LeadAddViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LeadAddViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region validation

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
                    me[BindableBase.GetPropertyName(() => SelectedIndexLeadSource)] +   // Lead Source
                    me[BindableBase.GetPropertyName(() => OfferAmount)] +
                    me[BindableBase.GetPropertyName(() => OfferCloseDate)] +
                    me[BindableBase.GetPropertyName(() => RFQReceptionDate)] +
                    me[BindableBase.GetPropertyName(() => QuoteSentDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexBusinessUnit)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexSalesOwner)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => ProductAndServicesCount)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexOfferOwner)];


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
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                string SelectedIndexOfferOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexOfferOwner);

                if (columnName == descriptionProp)
                    return RequiredValidationRule.GetErrorMessage(descriptionProp, Description);
                else if (columnName == selectedIndexLeadSourceProp) // Lead Source
                    return RequiredValidationRule.GetErrorMessage(selectedIndexLeadSourceProp, SelectedIndexLeadSource);
                else if (columnName == amountProp)
                    return RequiredValidationRule.GetErrorMessage(amountProp, OfferAmount, SelectedGeosStatus.IdOfferStatusType);
                else if (columnName == offerCloseDateProp)
                    return RequiredValidationRule.GetErrorMessage(offerCloseDateProp, OfferCloseDate);
                else if (columnName == rfqReceptionDateProp)
                    return RequiredValidationRule.GetErrorMessage(rfqReceptionDateProp, RFQReceptionDate, SelectedGeosStatus.IdOfferStatusType);
                else if (columnName == quoteSentDateProp)
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
                else if (columnName == informationError)
                    return RequiredValidationRule.GetErrorMessage(informationError, InformationError);

                else if (columnName == SelectedIndexOfferOwnerProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexOfferOwnerProp, SelectedIndexOfferOwner);


                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method show Engineering Analysis Window and that raises click on toggle switch.
        /// </summary>

        public void InitAddin(string mailSubject, string senderEmail)
        {
            try
            {
                SelectedGeosStatus = GeosStatusList.FirstOrDefault(gsl => gsl.IdOfferStatusType == 2);
                SelectedIndexBusinessUnit = BusinessUnitList.IndexOf(BusinessUnitList.FirstOrDefault(x => x.IdLookupValue == 5));
                SelectedIndexLeadSource = LeadSourceList.IndexOf(LeadSourceList.FirstOrDefault(x => x.IdLookupValue == 11));
                Description = mailSubject;

                PeopleDetails pd = CrmStartUp.GetGroupPlantByMailId(senderEmail);

                if (pd != null)
                {
                    SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(x => x.IdCustomer == pd.IdCustomer));

                    if (SelectedIndexCompanyGroup != -1)
                        SelectedIndexCompanyPlant = CompanyPlantList.IndexOf(CompanyPlantList.FirstOrDefault(x => x.IdCompany == pd.IdSite));

                    if (SalesOwnerList.Any(x => x.IdPerson == GeosApplication.Instance.ActiveUser.IdUser))
                    {
                        SelectedIndexSalesOwner = SalesOwnerList.FindIndex(x => x.IdPerson == GeosApplication.Instance.ActiveUser.IdUser);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in InitAddin() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][cpatil][15-05-2020] GEOS2-2279 Error when trying to modify offers with eng.analysis
        private void ShowIsAnalysisWindow(object obj)
        {
            try
            {
                if (IsEngAnalysis)
                {
                    GeosApplication.Instance.Logger.Log("Method ShowIsAnalysisWindow...", category: Category.Info, priority: Priority.Low);
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
                   
                    addEngineeringAnalysisViewModel.EngAnalysis = ExistedEngineeringAnalysis;

                    if (ExistedEngineeringAnalysisDuplicate != null)
                        addEngineeringAnalysisViewModel.EngAnalysisDuplicate = (EngineeringAnalysis)ExistedEngineeringAnalysisDuplicate.Clone();
                   
                    addEngineeringAnalysisViewModel.InIt(SelectedGeosStatus);

                    addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    addEngineeringAnalysisView.ShowDialogWindow();

                    if (addEngineeringAnalysisViewModel.IsSave)
                    {
                        ExistedEngineeringAnalysis = addEngineeringAnalysisViewModel.EngAnalysis;
                        ExistedEngineeringAnalysisDuplicate = addEngineeringAnalysisViewModel.EngAnalysisDuplicate;
                        EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;
                        //[001]
                        if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                        {
                            OptionsByOffer optionsByOffer = new OptionsByOffer();
                            optionsByOffer.IdOption = 25;
                            optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                            optionsByOffer.IsSelected = true;
                            optionsByOffer.Quantity = 1;
                            OptionsByOfferList.Add(optionsByOffer);
                        }
                        else
                        {
                                OptionsByOffer opt = OptionsByOfferList.FirstOrDefault(x => x.IdOption == 25);
                                opt.IsSelected = true;
                                opt.Quantity = 1;
                          
                        }
                        IsEngAnalysisButtonVisible = Visibility.Visible;
                        //IsEngAnalysisEnable = false;
                    }
                    else
                    {
                        IsEngAnalysisButtonVisible = Visibility.Collapsed;
                        IsEngAnalysis = false;
                    }
                }
                else
                {
                    //ExistedEngineeringAnalysis = new EngineeringAnalysis();
                    //ExistedEngineeringAnalysisDuplicate = new EngineeringAnalysis();
                    //EngAnalysisAttachmentFileUploadIndicator = new FileUploader();

                    IsEngAnalysisButtonVisible = Visibility.Collapsed;
                }

                GeosApplication.Instance.Logger.Log("Method ShowIsAnalysisWindow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ShowIsAnalysisWindow() - {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Edit Engineering Analysis
        /// [001][cpatil][15-05-2020] GEOS2-2279 Error when trying to modify offers with eng.analysis
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
            addEngineeringAnalysisViewModel.EngAnalysis = ExistedEngineeringAnalysis;
            addEngineeringAnalysisViewModel.EngAnalysisDuplicate = ExistedEngineeringAnalysisDuplicate;
            addEngineeringAnalysisViewModel.InIt(SelectedGeosStatus);
            addEngineeringAnalysisView.DataContext = addEngineeringAnalysisViewModel;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            addEngineeringAnalysisView.ShowDialogWindow();

            if (addEngineeringAnalysisViewModel.IsSave)
            {
                ExistedEngineeringAnalysis = addEngineeringAnalysisViewModel.EngAnalysis;
                ExistedEngineeringAnalysisDuplicate = addEngineeringAnalysisViewModel.EngAnalysisDuplicate;
                EngAnalysisAttachmentFileUploadIndicator = addEngineeringAnalysisViewModel.EngAnalysisAttachmentFileUploaderIndicator;

                OptionsByOffer optionsByOffer = new OptionsByOffer();
                optionsByOffer.IdOption = 25;
                optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                optionsByOffer.IsSelected = true;
                optionsByOffer.Quantity = 1;
                OptionsByOfferList = new List<OptionsByOffer>();

                if (optionsByOffer.IdOption != 0)
                {
                    OptionsByOfferList.Add(optionsByOffer);
                }

                IsEngAnalysisButtonVisible = Visibility.Visible;
            }
        }
        /// <summary>
        /// This method is for to generate offer code as per condition.
        /// [cpatil][GEOS2-1977] The code added in the offer code must be taken from the application selected site
        /// </summary>
        private void GenerateOfferCode()
        {
            GeosApplication.Instance.Logger.Log("Method GenerateOfferCode ...", category: Category.Info, priority: Priority.Low);

            // Company objcompany = CrmStartUp.GetCurrentPlantId(GeosApplication.Instance.ActiveUser.IdUser);
            try
            {
                if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 1 || OfferTypeList[SelectedIndexOfferType].IdOfferType == 2 || OfferTypeList[SelectedIndexOfferType].IdOfferType == 3 || OfferTypeList[SelectedIndexOfferType].IdOfferType == 4)
                {
                    OfferNumber = CrmStartUp.GetNextNumberOfSuppliesFromGCM(OfferTypeList[SelectedIndexOfferType].IdOfferType);

                    foreach (GeosStatus geosStatus in GeosStatusList)
                    {
                        if (geosStatus.IdOfferStatusType == 1 || geosStatus.IdOfferStatusType == 2)
                        {
                            geosStatus.IsEnabled = true;
                        }
                        else
                        {
                            geosStatus.IsEnabled = false;
                        }
                    }

                }
                else
                {
                    //Changed service method GetNextNumberOfOfferFromCounters to GetNextNumberOfOfferFromCounters_V2040
                    OfferNumber = CrmStartUp.GetNextNumberOfOfferFromCounters_V2040(OfferTypeList[SelectedIndexOfferType].IdOfferType, 0);

                    foreach (GeosStatus geosStatus in GeosStatusList)
                    {
                        if (geosStatus.IdOfferStatusType == 1 || geosStatus.IdOfferStatusType == 2 ||
                           geosStatus.IdOfferStatusType == 15 || geosStatus.IdOfferStatusType == 16)
                        {
                            geosStatus.IsEnabled = true;
                        }
                        else
                        {
                            geosStatus.IsEnabled = false;
                        }
                    }

                    //SelectedGeosStatus = GeosStatusList.FirstOrDefault(gsl => gsl.IdOfferStatusType == 15);

                }

                //Changed service method MakeOfferCode to MakeOfferCode_V2040
                OfferCode = CrmStartUp.MakeOfferCode_V2040(OfferTypeList[SelectedIndexOfferType].IdOfferType, GeosApplication.Instance.ActiveIdSite, 0);


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

                    if (OptionsByOfferList.Any(qtdQuantity => qtdQuantity.IdOption == Convert.ToInt64(pcl.Tag)))
                    {
                        OptionsByOffer optionsByOffer = OptionsByOfferList.Where(x => x.IdOption == Convert.ToInt64(pcl.Tag)).SingleOrDefault();
                        optionsByOffer.Quantity = Convert.ToInt32(obj.NewValue);

                        // After change in spinedit value update in Dttable also.
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());

                        if (dataRow != null && Convert.ToDouble(dataRow["Qty"]) == Convert.ToDouble(obj.NewValue))
                        {
                            dataRow["IsChecked"] = Convert.ToDouble(obj.NewValue) > 0 ? true : false;   // quantity > 0 then check product

                        }
                        if (dataRow != null && Convert.ToDouble(dataRow["Qty"]) != Convert.ToDouble(obj.OldValue))
                        {
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
                    else if (obj != null && Convert.ToInt32(obj.NewValue) != 0) // If quantity is zero then donot add in list.
                    {
                        OptionsByOffer optionsByOffer = new OptionsByOffer();
                        optionsByOffer.IdOption = Convert.ToInt64(pcl.Tag);
                        optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                        optionsByOffer.Quantity = Convert.ToInt32(obj.NewValue);

                        if (optionsByOffer.IdOption != 0)
                        {
                            OptionsByOfferList.Add(optionsByOffer);
                        }

                        // After change in spinedit value update in Dttable also.
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
                    ischeck = true;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in QuantityEditValueChangedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

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
                    if (pcl.Tag != null && pcl.Tag.ToString().Contains("Group"))
                        return;

                    // Check in existing list.
                    if (OptionsByOfferList.Any(qtdQuantity => qtdQuantity.IdOption == Convert.ToInt64(pcl.Tag)))
                    {
                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => row["idOfferOption"].ToString() == pcl.Tag.ToString());

                        if (dataRow != null && Convert.ToBoolean(dataRow["IsChecked"]) == Convert.ToBoolean(obj.NewValue))
                        {
                            OptionsByOffer optionsByOffer = OptionsByOfferList.Where(x => x.IdOption == Convert.ToInt64(pcl.Tag)).SingleOrDefault();
                            optionsByOffer.IsSelected = Convert.ToBoolean(obj.NewValue);

                            if (!optionsByOffer.IsSelected)     // If selected is false, then quantity is zero.
                            {
                                optionsByOffer.Quantity = 0;
                                dataRow["Qty"] = Convert.ToDouble(0);        // optionsByOffer.Quantity
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

                            if (optionsByOffer.IdOption != 0)   // || optionsByOffer.Quantity != 0
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
            }

            GeosApplication.Instance.Logger.Log("Method ChangeDescription() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CloseWindow(object obj)
        {
            //CRMCommon.Instance.EndInit();
            RequestClose(null, null);
        }

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
                    quotation.Template = new Template { Suffix = Item.Suffix };
                    quotation.Template.Name = Item.Name;
                    quotation.IdDetectionsTemplate = Item.IdTemplate;

                    TemplateDetailList.Add(quotation);
                }
                GeosApplication.Instance.Logger.Log("Method FillLeadsEdit() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsEdit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsEdit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsEdit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCustomerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerList ...", category: Category.Info, priority: Priority.Low);


                CompanyListnew.AddRange(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                GeosApplication.Instance.Logger.Log("Method FillCustomerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList ...", category: Category.Info, priority: Priority.Low);

                GeosStatusList = new ObservableCollection<GeosStatus>(CrmStartUp.GetGeosOfferStatus().AsEnumerable());

                //if (FromNewLead)
                //{
                for (int i = GeosStatusList.Count - 1; i >= 0; i--)
                {
                    if (GeosStatusList[i].IdOfferStatusType == 3 || GeosStatusList[i].IdOfferStatusType == 5 || GeosStatusList[i].IdOfferStatusType == 6 || GeosStatusList[i].IdOfferStatusType == 7 | GeosStatusList[i].IdOfferStatusType == 8
                        || GeosStatusList[i].IdOfferStatusType == 9 || GeosStatusList[i].IdOfferStatusType == 10 || GeosStatusList[i].IdOfferStatusType == 11 || GeosStatusList[i].IdOfferStatusType == 12 || GeosStatusList[i].IdOfferStatusType == 13
                        || GeosStatusList[i].IdOfferStatusType == 14 || GeosStatusList[i].IdOfferStatusType == 4 || GeosStatusList[i].IdOfferStatusType == 17)
                    {
                        GeosStatusList.RemoveAt(i);

                    }
                    else
                        GeosStatusList[i].IsEnabled = true;
                    //  }
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

        private void FillCurrencyList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyList ...", category: Category.Info, priority: Priority.Low);

                Currencies = CrmStartUp.GetCurrencyByExchangeRate().ToList();   //GeosApplication.Instance.Currencies;
                SelectedIndexCurrency = Currencies.FindIndex(i => i.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);
                GeosApplication.Instance.Logger.Log("Method FillCurrencyList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOfferType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOfferType ...", category: Category.Info, priority: Priority.Low);

                IList<OfferType> tempOfferTypeList;
                tempOfferTypeList = CrmStartUp.GetOfferType();

                OfferTypeList = new List<OfferType>();
                //OfferTypeList.Add(new OfferType() { IdOfferType = 0, Name = "---", Name_es = "---", Name_fr = "---", Name_pt = "---", Name_ro = "---", Name_zh = "---" });

                foreach (OfferType ft in tempOfferTypeList)
                {
                    if (ft.IdOfferType == 1 || ft.IdOfferType == 10)
                        OfferTypeList.Add(ft);
                }
                try
                {
                    SelectedIndexOfferType = OfferTypeList.FindIndex(i => i.IdOfferType == 10);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexOfferType fill " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        /// Method for save Offer details.
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// [002][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [002][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// [003][GEOS2-2217][cpatil][03-04-2020]Eng. Analysis type field not working as expected
        /// </summary>
        /// <param name="obj"></param>
        private void SaveOffer(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveOffer ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;

                //OptionsByOfferList = OptionsByOfferList.Where(opt => opt.IsSelected == true).ToList();
                ProductAndServicesCount = OptionsByOfferList.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList().Count();

                if (ProductAndServicesCount > 0)
                {
                    List<OptionsByOffer> TempOptionsByOfferList = OptionsByOfferList.Where(opt => (opt.Quantity == null || opt.Quantity == 0) && opt.IsSelected == true).ToList();

                    if (TempOptionsByOfferList.Count > 0)
                    {
                        ProductAndServicesCount = 0;
                    }
                    else if (SelectedGeosStatus.IdOfferStatusType != 1 && SelectedGeosStatus.IdOfferStatusType != 2 && OptionsByOfferList.Count == 1 && OptionsByOfferList.Any(x => x.IdOption == 25))
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
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexLeadSource"));     // Lead Source
                PropertyChanged(this, new PropertyChangedEventArgs("OfferAmount"));
                PropertyChanged(this, new PropertyChangedEventArgs("OfferCloseDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("RFQReceptionDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("QuoteSentDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessUnit"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSalesOwner"));
                PropertyChanged(this, new PropertyChangedEventArgs("ProductAndServicesCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOfferOwner"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    //Save offer code.
                    GenerateOfferCode();

                    //string TempOfferCode = OfferCode;
                    // Company _Company = WorkbenchStartUp.GetCompanyByAlias(GeosApplication.Instance.SiteName);
                    // OfferData.Site.ConnectPlantId = _Company.IdCompany.ToString();

                    OfferData = new Offer();
                    OfferData.Code = OfferCode;
                    OfferData.IdCustomer = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                    OfferData.Number = OfferNumber;
                    OfferData.IdOfferType = OfferTypeList[SelectedIndexOfferType].IdOfferType;
                    if (!string.IsNullOrEmpty(Description))
                        OfferData.Description = Description.Trim();
                    OfferData.IdSource = Convert.ToByte(LeadSourceList[SelectedIndexLeadSource].IdLookupValue);     // Lead Source
                    OfferData.Source = LeadSourceList[SelectedIndexLeadSource];
                    OfferData.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                    OfferData.Currency = new Currency { IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency, Name = Currencies[SelectedIndexCurrency].Name };
                    OfferData.IdStatus = SelectedGeosStatus.IdOfferStatusType; //GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
                    OfferData.GeosStatus = SelectedGeosStatus; //GeosStatusList[SelectedIndexStatus];
                    OfferData.Value = OfferAmount;
                    OfferData.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    OfferData.CreatedByUser = GeosApplication.Instance.ActiveUser;
                    OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    OfferData.IdBusinessUnit = Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue);
                    OfferData.BusinessUnit = BusinessUnitList[SelectedIndexBusinessUnit];

                    OfferData.Site = CompanyPlantList[SelectedIndexCompanyPlant];
                    //Company _Company = CrmStartUp.GetCurrentPlantId(GeosApplication.Instance.ActiveUser.IdUser);
                    //OfferData.Site.ConnectPlantId = _Company.ConnectPlantId;
                    //OfferData.Site.ConnectPlantConstr = _Company.ConnectPlantConstr;

                    //Added for create issue in Jira
                    OfferData.Site.ShortName = GeosApplication.Instance.SiteName;
                    offerData.OfferType = OfferTypeList[SelectedIndexOfferType];
                    offerData.JiraUserReporter = GeosApplication.Instance.ActiveUser;

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

                    if (SelectedIndexSalesOwner == -1)
                    {
                        OfferData.IdSalesOwner = 0;
                    }
                    else
                    {
                        OfferData.IdSalesOwner = SalesOwnerList[SelectedIndexSalesOwner].IdPerson;

                        if (SalesOwnerList != null)
                            OfferData.SalesOwner = (People)SalesOwnerList[SelectedIndexSalesOwner].Clone();

                        OfferData.SalesOwner.OwnerImage = null;
                    }

                    OfferData.DeliveryDate = OfferCloseDate;
                    OfferData.RFQReception = RFQReceptionDate != null ? (DateTime)RFQReceptionDate : DateTime.MinValue;
                    OfferData.SendIn = QuoteSentDate != null ? (DateTime)QuoteSentDate : DateTime.MinValue;
                    OfferData.ProbabilityOfSuccess = Convert.ToSByte(SelectedIndexConfidentialLevel.ToString());
                    OfferData.OfferExpectedDate = OfferCloseDate;
                    OfferData.Comments = "";
                    //[001] Added
                    OfferData.OfferActiveSite = new ActiveSite { IdSite = GeosApplication.Instance.ActiveIdSite, SiteAlias = GeosApplication.Instance.SiteName, SiteServiceProvider = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString() };
                    OfferData.Rfq = "";
                    if (string.IsNullOrEmpty(Rfq))
                    {
                        OfferData.Rfq = "";
                    }
                    else
                    {
                        OfferData.Rfq = Rfq.Trim();
                    }

                    OfferData.Site.FullName = OfferData.Site.Customers[0].CustomerName + " - " + OfferData.Site.Name;
                    List<LogEntryByOffer> logEntryByOffers = new List<LogEntryByOffer>();
                    OfferData.LogEntryByOffers = logEntryByOffers;

                    // OfferData.Quotations = new System.Collections.ObjectModel.ObservableCollection<Quotation>(TemplateDetailList.Where(i => i.QuotQuantity != null).ToList());

                    OfferData.LostReasonsByOffer = LostReasonsByOffer;
                    //List for save final selected Product and servies (Templates)
                    List<OptionsByOffer> FinalOptionsByOfferList = OptionsByOfferList.Where(opt => opt.Quantity != 0 && opt.IsSelected == true).ToList();
                    OfferData.OptionsByOffers = FinalOptionsByOfferList;

                    if (IsEngAnalysis && ExistedEngineeringAnalysis != null)
                    {
                        if (!OptionsByOfferList.Any(ps => ps.IdOption == 25))
                        {
                            OptionsByOffer optionsByOffer = new OptionsByOffer();
                            optionsByOffer.IdOption = 25;
                            optionsByOffer.IsSelected = true;
                            optionsByOffer.Quantity = 1;
                            OptionsByOfferList.Add(optionsByOffer);
                        }

                        if (ExistedEngineeringAnalysis.EngineeringAnalysisTypes != null)
                            ExistedEngineeringAnalysis.EngineeringAnalysisTypes.ForEach(e => e.IsSelected = true);

                        OfferData.EngineeringAnalysis = ExistedEngineeringAnalysis;

                        if (EngAnalysisAttachmentFileUploadIndicator != null)
                        {
                            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                            //[001] Added
                            IGeosRepositoryService OfferGeosRepositoryServiceController = new GeosRepositoryServiceController(OfferData.OfferActiveSite.SiteServiceProvider);
                            fileUploadReturnMessage = OfferGeosRepositoryServiceController.UploaderEngineeringAnalysisZipFile(EngAnalysisAttachmentFileUploadIndicator);
                        }
                    }

                    try
                    {
                        //[001] added

                        #region Old Code 
                        //if (ListAddedContact != null)
                        //{
                        //    foreach (var item in ListAddedContact)
                        //    {
                        //        if (item.IsSelected == true)
                        //        {
                        //            OfferContact offerContact = new OfferContact();
                        //            offerContact.IdContact = item.IdPerson;
                        //            offerContact.IsPrimaryOfferContact = 1;
                        //            ListOfferContact.Add(offerContact);
                        //            LogEntryByOffer contactComment = new LogEntryByOffer()
                        //            {
                        //                IdOffer = 0,
                        //                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                DateTime = GeosApplication.Instance.ServerDateTime,
                        //                Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddFavoriteContact").ToString(), item.FullName),
                        //                IdLogEntryType = 13
                        //            };
                        //            ListChangeLog.Add(contactComment);
                        //            logEntryByOffers.Add(contactComment);
                        //        }
                        //        else
                        //        {
                        //            OfferContact offerContact = new OfferContact();
                        //            offerContact.IdContact = item.IdPerson;
                        //            offerContact.IsPrimaryOfferContact = 0;
                        //            ListOfferContact.Add(offerContact);
                        //            LogEntryByOffer contactComment = new LogEntryByOffer()
                        //            {
                        //                IdOffer = 0,
                        //                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //                DateTime = GeosApplication.Instance.ServerDateTime,
                        //                Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddContact").ToString(), item.FullName),
                        //                IdLogEntryType = 13
                        //            };
                        //            ListChangeLog.Add(contactComment);
                        //            logEntryByOffers.Add(contactComment);
                        //        }
                        //    }
                        //    OfferData.OfferContacts = ListOfferContact.ToList();
                        //}

                        #endregion

                        if (SelectedOfferToList != null)
                        {
                            foreach (object SelectedContact in SelectedOfferToList)
                            {
                                OfferContact contact = SelectedContact as OfferContact;

                                OfferContact offerContact = new OfferContact();
                                offerContact.IdContact = contact.People.IdPerson;
                                offerContact.IsPrimaryOfferContact = 0;
                                offerContact.People = contact.People;

                                ListOfferContact.Add(offerContact);
                                LogEntryByOffer contactComment = new LogEntryByOffer()
                                {
                                    IdOffer = 0,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewLogAddContact").ToString(), contact.People.FullName),
                                    IdLogEntryType = 13
                                };
                                ListChangeLog.Add(contactComment);
                                logEntryByOffers.Add(contactComment);

                            }
                            OfferData.OfferContacts = ListOfferContact.ToList();
                        }
                        else
                        {
                            OfferData.OfferContacts = null;
                        }


                        OfferData.OfferedBy = OfferOwnerList[SelectedIndexOfferOwner].IdUser;

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

                    try
                    {
                        if (SelectedIndexGeosProject > -1 && GeosProjectsList[SelectedIndexGeosProject].IdCarProject == 0)
                        {
                            GeosProjectsList[SelectedIndexGeosProject].CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                            CarProject carProject = CrmStartUp.AddCarProjectWithCreatedBy(GeosProjectsList[SelectedIndexGeosProject]);
                            OfferData.IdCarProject = carProject.IdCarProject;
                            OfferData.CarProject = carProject;

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
                        }

                        //[001] Changed service method and 2 input parameter for method(IdCompany)
                        //[003] Changed service method AddOffer_V2040 to AddOffer_V2041
                        OfferReturnValue = CrmStartUp.AddOffer_V2045(OfferData, GeosApplication.Instance.ActiveIdSite, GeosApplication.Instance.ActiveUser.IdUser);

                        if (OfferReturnValue.IdOffer > 0 && OfferReturnValue.IdOfferType == 1)
                        {
                            EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OfferData.OfferActiveSite.IdSite));
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                            ICrmService CrmStartUpCreateFolder = new CrmServiceController(serviceurl);
                            bool isCreated = CrmStartUpCreateFolder.CreateFolderOffer(OfferReturnValue, true);
                        }

                        if (!string.IsNullOrEmpty(OfferReturnValue.ErrorFromJira))
                        {
                            GeosApplication.Instance.Logger.Log(OfferReturnValue.ErrorFromJira, Category.Exception, Priority.Low);
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("JiraNotWorking").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }


                        //[Start]code for create activity as per conditions. 

                        List<ActivityTemplate> activityTemplateList = new List<ActivityTemplate>();
                        string activityMsg = "";


                        foreach (var activityTemplateTrigger in activityTemplateTriggers)
                        {
                            if (OfferReturnValue != null && SelectedGeosStatus.IdOfferStatusType == 1 &&
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
                                            AddActivity(OfferReturnValue.IdOffer, Convert.ToInt32(OfferData.OfferActiveSite.IdSite), activityTemplate);
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
                                    //AddActivity(activityTemplate);
                                    AddActivity(OfferReturnValue.IdOffer, Convert.ToInt32(OfferData.OfferActiveSite.IdSite), activityTemplate);
                                }
                            }
                        }

                        //[End]code for create activity as per conditions. 

                        if (SelectedIndexSalesOwner != -1)
                            OfferData.SalesOwner = SalesOwnerList[SelectedIndexSalesOwner];

                        OfferData.IdOffer = OfferReturnValue.IdOffer;
                        IsBusy = false;
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        IsBusy = false;
                        OfferData = null;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        IsBusy = false;
                        OfferData = null;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SaveOffer() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    }

                    string OfferTypeName = "Offer";
                    if (OfferTypeList[SelectedIndexOfferType].IdOfferType == 10)
                    {
                        OfferTypeName = OfferTypeList[SelectedIndexOfferType].Name;
                    }

                    if (OfferReturnValue != null && OfferReturnValue.IdOffer > 0)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsAddViewOfferCreated").ToString(), OfferTypeName, OfferCode)
                                            , "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsAddViewOfferNotCreated").ToString(), OfferTypeName)
                            , "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }

                    GeosApplication.Instance.Logger.Log("Method SaveOffer() executed successfully", category: Category.Exception, priority: Priority.Low);
                }
                IsBusy = false;
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
        /// Method for check mendatory fields to fill.
        /// </summary>
        /// <returns></returns>
        private bool CheckMendatoryField()
        {
            IsBusy = false;
            bool isError = false;
            errorList = new List<string>();
            if (SelectedIndexCompanyGroup == -1 || SelectedIndexCompanyGroup == 0)
            {
                errorList.Add("Please select Company group.");
            }

            if (String.IsNullOrEmpty(Description))
            {
                errorList.Add("Please fill some description.");
            }

            if (OfferCloseDate == null)
            {
                errorList.Add("Please select close date.");
            }

            if (SelectedIndexBusinessUnit == 0)
            {
                errorList.Add("Please select business unit.");
            }

            if (SelectedIndexSalesOwner == -1)
            {
                errorList.Add("Please select sales owner.");
            }

            int tqt = OptionsByOfferList.Select(qt => qt.Quantity.Value).ToList().Sum();

            if (tqt < 1)
            {
                errorList.Add("Please Choose atleast 1 Template.");
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

                CustomMessageBox.Show(errorMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            return isError;
        }

        /// <summary>
        /// Method for fill and choose Confidential Level.
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
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                //IList<Customer> TempCompanyGroupList = null;

                if (GeosApplication.Instance.IdUserPermission == 21)
                {

                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                        SelectedIndexCompanyGroup = 0;
                    }
                    else
                    {
                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                        SelectedIndexCompanyGroup = 0;
                    }
                }
                else
                {
                    //TempCompanyGroupList = CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                        SelectedIndexCompanyGroup = 0;
                    }
                    else
                    {

                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                        SelectedIndexCompanyGroup = 0;
                    }
                }

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

        /// <summary>
        /// Method for fill Company list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                //List<Company> TempcompanyPlant = new List<Company>();
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

                //if (CompanyPlantList.Count > 0)
                //    SelectedIndexCompanyPlant = 0;
                //else
                //    SelectedIndexCompanyPlant = -1;

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Sales Owner list.
        /// </summary>
        private void FillSalesOwnerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList ...", category: Category.Info, priority: Priority.Low);

                if (CompanyPlantList != null && CompanyPlantList.Count > 0)
                {
                    SalesOwnerList = CrmStartUp.GetSalesOwnerBySiteId(CompanyPlantList[SelectedIndexCompanyPlant].IdCompany);


                    for (int i = 0; i < SalesOwnerList.Count; i++)
                    {
                        User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(SalesOwnerList[i].IdPerson));

                        try
                        {
                            UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(user.Login);
                            SalesOwnerList[i].OwnerImage = byteArrayToImage(UserProfileImageByte);
                        }
                        catch (Exception)
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

                    //if (SelectedIndexSalesOwner == 0)
                    SelectedIndexSalesOwner = SalesOwnerList.FindIndex(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser);
                    if (SelectedIndexSalesOwner < 0)
                        SelectedIndexSalesOwner = 0;

                    SetOfferOwner();

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
        /// Method for fill Company group list.
        /// </summary>
        private void FillCaroemsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCaroemsList ...", category: Category.Info, priority: Priority.Low);

                CaroemsList = CrmStartUp.GetCarOEM();

                CaroemsList.Insert(0, new CarOEM() { Name = "---" });
                // SelectedSIndexCarOem = CaroemsList.FindIndex(i => i.IdCarOEM == SelectedLeadList[0].IdCarOEM);

                //if (SelectedSIndexCarOem == -1)
                //    SelectedSIndexCarOem = 0;
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
                GeosProjectsListTemp = GeosProjectsList.Select(tpg => tpg.Name).ToList();

                SelectedIndexGeosProject = -1;

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
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(byteArrayIn);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            biImg.DecodePixelHeight = 10;
            biImg.DecodePixelWidth = 10;

            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        /// <summary>
        /// Method for fill BusinessUnit list.
        /// </summary>
        private void FillBusinessUnitList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission);
                BusinessUnitList = new List<LookupValue>();
                BusinessUnitList = new List<LookupValue>(tempBusinessUnitList.Where(inUseOption => inUseOption.InUse == true));
                BusinessUnitList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                //BusinessUnitList.AddRange(tempBusinessUnitList);
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
        /// Method for Fill Lead Source list.   // Lead Source
        /// </summary>
        private void FillLeadSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList ...", category: Category.Info, priority: Priority.Low);

                LeadSourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(4).AsEnumerable());
                //LeadSourceList.Insert(0, new LookupValue() { Value = "---" });
                ObservableCollection<LookupValue> TempLeadSourceList = new ObservableCollection<LookupValue>(LeadSourceList.Where(inUseOption => inUseOption.InUse == true));
                TempLeadSourceList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                LeadSourceList = TempLeadSourceList;

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
        /// Method for to create Product and Service tree list
        /// </summary>
        private void ProductAndService()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductAndService ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Column>() {
                     // new Column() { FieldName="Offeroptiontype",HeaderText="Offeroptiontype", Settings = SettingsType.Default, AllowCellMerge=true, Width=85,AllowEditing=false,Visible=true,IsVertical= false },
                     new Column() { FieldName="Name",HeaderText="Name", Settings = SettingsType.Array, AllowCellMerge=true,Width=85,AllowEditing=false,Visible=true,IsVertical= false},
                     new Column() { FieldName="Qty",HeaderText="Qty", Settings = SettingsType.Amount, AllowCellMerge=true,Width=150,AllowEditing=false,Visible=true,IsVertical= false},
                     new Column() { FieldName="idOfferOption",HeaderText="idOfferOption", Settings = SettingsType.Default, AllowCellMerge=true,Width=150,AllowEditing=false,Visible=false,IsVertical= false },
                     new Column() { FieldName="IdOfferOptionType",HeaderText="IdOfferOptionType", Settings = SettingsType.Default, AllowCellMerge=true,Width=150,AllowEditing=false,Visible=false,IsVertical= false},
                };

                Dttable = new DataTable();
                DataRow drw;
                //  Dttable.Columns.Add("Offeroptiontype", typeof(string));
                Dttable.Columns.Add("Name", typeof(string));
                Dttable.Columns.Add("idOfferOption", typeof(string));
                Dttable.Columns.Add("IdOfferOptionType", typeof(string));
                Dttable.Columns.Add("Qty", typeof(double));
                Dttable.Columns.Add("IsChecked", typeof(bool));
                offerOptions = CrmStartUp.GetAllOfferOptions();

                //code for items to remove from products
                List<Int64> tempremovelist = new List<Int64>() { 6, 19, 21, 23, 25, 27 };
                offerOptions = offerOptions.Where(t => !tempremovelist.Contains(t.IdOfferOption)).ToList();


                offerOptions = offerOptions.Where(oo => oo.IsObsolete != 1).ToList();

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
                        //itemofferopt.IdOfferOptionType;
                        Dttable.Rows.Add(drw);
                    }
                    i++;
                }
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
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                if (amount > Max_Value && Max_Value > 0) //[01] added
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadsAddUpdateAmountWarning"].ToString(), Max_Value), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }
            }

            IsBusy = false;
            GeosApplication.Instance.Logger.Log("Method OfferAmountLostFocusCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Warning in amounts greater than 50000 EUR, if currency is changed.
        /// [001][skale][2019-08-04][GEOS2-239] Wrong warning message in offer popup
        /// </summary>
        /// <param name="obj"></param>
        private void CurrencySelectedIndexChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CurrencySelectedIndexChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;

            if (OfferAmount > 0)
            {
                Double amount = OfferAmount * (Currencies[SelectedIndexCurrency].CurrencyConversions.Count > 0 ? Currencies[SelectedIndexCurrency].CurrencyConversions[0].ExchangeRate : 1);
                if (amount > Max_Value && Max_Value > 0) //[01] added
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadsAddUpdateAmountWarning"].ToString(), Max_Value), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }
            }

            IsBusy = false;
            GeosApplication.Instance.Logger.Log("Method CurrencySelectedIndexChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This Method is used to filter used nodes. ByDefault all nodes be displayed.
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
                    if (!Convert.ToBoolean(data["IsChecked"]))
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

                TreeListView treeListView = (TreeListView)obj;
                isFromShowAll = true;
                treeListView.ExpandAllNodes();
                isFromShowAll = false;
                treeListView.ExpandAllNodes();

                GeosApplication.Instance.Logger.Log("Method ShowAllCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllCheckedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to collapse all nodes in treelist on unchecked show all.
        /// </summary>
        /// <param name="obj">The treelistview</param>
        public void ShowAllUncheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

                TreeListView treeListView = (TreeListView)obj;

                isFromShowAll = true;
                treeListView.ExpandAllNodes();
                isFromShowAll = false;
                treeListView.ExpandAllNodes();

                GeosApplication.Instance.Logger.Log("Method ShowAllUncheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAllUncheckedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

        /// <summary>
        /// This Method for setting Primary Contact through context menu
        /// </summary>
        /// <param name="obj"></param>

        //public void SetCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method SetCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    People data = (People)((DevExpress.Xpf.Grid.RowData)(obj)).Row;
        //    if (ListAddedContact != null && ListAddedContact.Any(x => x.IdPerson == data.IdPerson))
        //    {
        //        foreach (var item in ListAddedContact)
        //        {
        //            if (item.IdPerson == data.IdPerson)
        //            {
        //                // Added PrimaryOfferContact is null as we need this when changing the Primary Contact.
        //                if (PrimaryOfferContact == null)
        //                {
        //                    PrimaryOfferContact = new OfferContact();
        //                    IsFirstPrimaryContact = true;
        //                }
        //                PrimaryOfferContact.People = item;
        //                item.IsSelected = true;

        //            }
        //            else
        //            {
        //                if (item.IsSelected)
        //                {
        //                    IsPrimayContactChanged = true;
        //                    PreviousPrimaryContact = item.FullName;
        //                }
        //                item.IsSelected = false;
        //            }
        //        }
        //    }

        //    GeosApplication.Instance.Logger.Log("Method SetCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

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
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This Method for getting contacts from selection changed in cmbSite
        /// </summary>
        /// <param name="obj"></param>
        //public void SelectedItemChangedCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        IdSite = (Int16)CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
        //        ListCustomerContact = new ObservableCollection<People>(CrmStartUp.GetContactsOfSiteByOfferId(IdSite).AsEnumerable());
        //        GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //}


        //public void FillContactAsPerIdSite()
        //{
        //    GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        IdSite = (Int16)CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
        //        ListCustomerContact = new ObservableCollection<People>(CrmStartUp.GetContactsOfSiteByOfferId(IdSite).AsEnumerable());
        //        GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        /// <summary>
        /// This Method for add new project.
        /// </summary>
        public void AddNewProjectCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddNewProjectCommandAction ...", category: Category.Info, priority: Priority.Low);

            List<CarProject> projectNameList = new List<CarProject>();
            projectNameList = GeosProjectsList.ToList();

            AddNewProjectViewModel addNewProjectViewModel = new AddNewProjectViewModel();
            AddNewProjectView addNewProjectView = new AddNewProjectView();

            EventHandler handle = delegate { addNewProjectView.Close(); };
            addNewProjectViewModel.RequestClose += handle;

            addNewProjectViewModel.GeosProjectsList = GeosProjectsList;
            addNewProjectViewModel.ProjectNameList = projectNameList;
            addNewProjectView.DataContext = addNewProjectViewModel;
            var ownerInfo = (obj as FrameworkElement);
            addNewProjectView.Owner = Window.GetWindow(ownerInfo);
            addNewProjectView.ShowDialogWindow();

            if (addNewProjectViewModel.IsSave)
            {
                addNewProjectViewModel.NewGeosProject.IdCustomer = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                //IsCreateCustomer = addCustomerNameViewModel.IsSave;//true
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

            GeosApplication.Instance.Logger.Log("Method AddNewProjectCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for add new activity.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(long idOffer, int idSite, ActivityTemplate activityTemplate)
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
                ActivityLinkedItem _aliAccount = new ActivityLinkedItem();
                _aliAccount.Company = new Company();
                _aliAccount.Company.Customers = new List<Customer>();
                _aliAccount.IdLinkedItemType = 42;
                _aliAccount.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                _aliAccount.LinkedItemType = new LookupValue();
                _aliAccount.LinkedItemType.IdLookupValue = 42;
                _aliAccount.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();
                _aliAccount.IsVisible = false;
                _Activity.ActivityLinkedItem.Add(_aliAccount);

                //_aliAccount.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                //_aliAccount.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                //_aliAccount.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;

                //Fill Opportunity details.
                ActivityLinkedItem _aliOpportunity = new ActivityLinkedItem();
                _aliOpportunity = (ActivityLinkedItem)_aliAccount.Clone();
                _aliOpportunity.IdLinkedItemType = 44;
                _aliOpportunity.Name = offerCode;
                _aliOpportunity.IdSite = null;
                _aliOpportunity.IdOffer = idOffer;
                _aliOpportunity.IdEmdepSite = idSite;
                _aliOpportunity.LinkedItemType = new LookupValue();
                _aliOpportunity.LinkedItemType.IdLookupValue = 44;
                _aliOpportunity.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();

                _Activity.ActivityLinkedItem.Add(_aliOpportunity);

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
                    addActivityViewModel.Subject = activityTemplate.Subject;            // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                    addActivityViewModel.Description = activityTemplate.Description;    // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
                    addActivityViewModel.DueDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                }

                //**[End] code for add Account Detail.

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

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
        /// Method for add new activity for offer.
        /// </summary>
        private void AddActivity(long idOffer, int idSite, ActivityTemplate activityTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCtivity ...", category: Category.Info, priority: Priority.Low);

                Activity NewActivity = new Activity();
                List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();
                List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();

                NewActivity.IdActivityType = activityTemplate.IdActivityType;
                NewActivity.Subject = activityTemplate.Subject;             //string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivitySubject").ToString());
                NewActivity.Description = activityTemplate.Description;     // string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewActivityDescription").ToString());
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
                NewActivity.IsDeleted = 0;

                //Fill Account details.
                ActivityLinkedItem _aliAccount = new ActivityLinkedItem();
                _aliAccount.IdLinkedItemType = 42;
                _aliAccount.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                _aliAccount.Name = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName + " - " + CompanyPlantList[SelectedIndexCompanyPlant].Name;
                _aliAccount.IsVisible = false;
                listActivityLinkedItems.Add(_aliAccount);

                // For Add Attendies
                List<ActivityAttendees> listattendees = new List<ActivityAttendees>();
                listattendees.Add(new ActivityAttendees() { IdUser = SalesOwnerList[SelectedIndexSalesOwner].IdPerson });
                NewActivity.ActivityAttendees = listattendees;

                //_ActivityLinkedItem.Company = new Company();
                //_ActivityLinkedItem.Company.Customers = new List<Customer>();
                //_ActivityLinkedItem.Company = CompanyPlantList[SelectedIndexCompanyPlant];
                //_ActivityLinkedItem.Company.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                //_ActivityLinkedItem.LinkedItemType = new LookupValue();
                //_ActivityLinkedItem.LinkedItemType.IdLookupValue = 42;
                //_ActivityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                //Fill Opportunity details.
                ActivityLinkedItem _aliOpportunity = new ActivityLinkedItem();
                _aliOpportunity.IdLinkedItemType = 44;
                _aliOpportunity.Name = offerCode;
                _aliOpportunity.IdSite = null;
                _aliOpportunity.IdOffer = idOffer;
                _aliOpportunity.IdEmdepSite = idSite;
                _aliOpportunity.IsVisible = false;
                listActivityLinkedItems.Add(_aliOpportunity);

                //_ActivityLinkedItem1 = (ActivityLinkedItem)_ActivityLinkedItem.Clone();
                //_ActivityLinkedItem1.LinkedItemType = new LookupValue();
                //_ActivityLinkedItem1.LinkedItemType.IdLookupValue = 44;
                //_ActivityLinkedItem1.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();

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
                NewActivity = CrmStartUp.AddActivity(NewActivity);

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

        public void Dispose()
        {
            //throw new NotImplementedException();
            GC.Collect();
        }


        /// <summary>
        /// 
        /// </summary>

        private void FillOfferToList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOfferToList() ...", category: Category.Info, priority: Priority.Low);

                OfferToList = new List<OfferContact>();

                Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];

                OfferToList = CrmStartUp.GetContactsOfCustomerGroupByOfferId(customer.IdCustomer);

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

        /// <summary>
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


                //OfferOwnerList = CrmStartUp.GetSalesAndCommericalUsers();

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
        /// [000][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        public void SetOfferOwner()
        {

            if (SelectedIndexSalesOwner == -1)
            {
                SelectedIndexOfferOwner = 0;
            }
            else
            {

                SelectedIndexOfferOwner = OfferOwnerList.FindIndex(i => i.IdUser == SalesOwnerList[SelectedIndexSalesOwner].IdPerson);

                if (SelectedIndexOfferOwner == -1)
                    SelectedIndexOfferOwner = 0;
            }
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
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
        private void ShowLostOpportunityWindow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShowLostOpportunityWindow ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            LostOpportunityView lostOpportunityView = new LostOpportunityView();
            LostOpportunityViewModel lostOpportunityViewModel = new LostOpportunityViewModel();
            EventHandler handle = delegate { lostOpportunityView.Close(); };
            lostOpportunityViewModel.RequestClose += handle;
            lostOpportunityViewModel.LostReasonsByOfferForRetrive = LostReasonsByOffer;
            //[001] added
            lostOpportunityViewModel.OfferCloseDate = OfferCloseDate;

            lostOpportunityViewModel.InIt();
            lostOpportunityView.DataContext = lostOpportunityViewModel;
            IsBusy = false;
            lostOpportunityView.ShowDialog();

            if (!lostOpportunityViewModel.IsCancel)
            {
                LostReasonsByOffer = new LostReasonsByOffer();
                LostReasonsByOffer.Comments = lostOpportunityViewModel.LostReasonDescription;
                LostReasonsByOffer.IdCompetitor = lostOpportunityViewModel.Competitors[lostOpportunityViewModel.SelectedIndexCompetitors].IdCompetitor;
                foreach (OfferLostReason item in lostOpportunityViewModel.SelectedItems)
                {
                    if (!string.IsNullOrEmpty(LostReasonsByOffer.IdLostReasonList))
                    {
                        LostReasonsByOffer.IdLostReasonList = LostReasonsByOffer.IdLostReasonList + ";" + item.IdLostReason.ToString();
                    }
                    else
                    {
                        LostReasonsByOffer.IdLostReasonList = item.IdLostReason.ToString();
                    }
                }
            }
            else if(lostOpportunityViewModel.IsCancel)
            {
                if(LostReasonsByOffer==null)
                {
                    if(GeosStatusList!=null)
                    SelectedGeosStatus = GeosStatusList.FirstOrDefault(gsl => gsl.IdOfferStatusType == 15);

                }
            }

            GeosApplication.Instance.Logger.Log("Method ShowLostOpportunityWindow() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion  // Methods
    }
}
