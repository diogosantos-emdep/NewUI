using DevExpress.Mvvm;
using DevExpress.PivotGrid.QueryMode;
using DevExpress.Xpf;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.XtraBars.Docking2010.Views.BaseRegistrator;
using System.Windows;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using System.ServiceModel;
using DevExpress.Office.Utils;
using Emdep.Geos.Data.Common.TechnicalRestService;
using ShippingAddress = Emdep.Geos.Data.Common.OTM.ShippingAddress;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Modules.OTM.Views;
using System.Diagnostics;
using System.IO;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using System.Drawing;
using Emdep.Geos.Modules.OTM.CommonClass;
using DevExpress.XtraSpreadsheet.Model;
using SplashScreenView = Emdep.Geos.Modules.OTM.Views.SplashScreenView;
using DevExpress.DataProcessing;
using DevExpress.Utils.Extensions;
using System.Collections;
using System.Globalization;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core.Native;
using DevExpress.Data.Filtering;
using DevExpress.DataProcessing.InMemoryDataProcessor;


namespace Emdep.Geos.Modules.OTM.ViewModels
{
    // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
    public class EditPORequestsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");

        #endregion

        #region Declaration
        string serviceUrl;
        PORequestDetails MainObj;
        private bool isSave;
        string code;
        string customergroup;
        string plant;
        string companygroup;
        string companyplant;
        string rfq;
        string offerto;
        double amount;
        double discount;
        string carriagemethod;
        private int idSite; 
        private int idStatus; 
        private ObservableCollection<Currency> currencies;
        private ObservableCollection<LinkedOffers> linkedofferlist;
        private ObservableCollection<LinkedOffers> offertypeList;
        private ObservableCollection<LinkedOffers> linkedpolist;
        List<LinkedOffers> offerToContactlist;
        private ObservableCollection<LookupValue> statusList;
        private ObservableCollection<ShippingAddress> shippingAddressList;
        private ObservableCollection<LinkedOffers> customercontactList;
        private ObservableCollection<LookupValue> carriageMethodList;
        public List<Data.Common.Company> TempCompany { get; set; }
        public PORequestDetails PORequestdetail { get; set; }
        //private ObservableCollection<CustomerPlant> entireCompanyPlantList;
        //private ObservableCollection<Customer> customers;
        private int selectedIndexCurrency = -1;
        private LookupValue selectedIndexstatus;
        private LookupValue selectedIndexCarriageMethod;
        private int selectedIndexCarriageMethods;
        private LinkedOffers selectedIndexLinkedOffer;
        private LinkedOffers selectedIndexLinkedPo;
        private ObservableCollection<CustomerPlant> customerPlant;
        //private int selectedIndexCustomerGroup = -1;
        private int selectedIndexOfferType = -1;
        //private int selectedIndexCompanyPlant = -1;
        private int selectedIndexShipTo = -1;
        private string windowHeader;
        private ObservableCollection<Emailattachment> pOAttachementsList;
        private string attachmentCnt;
        private ObservableCollection<LookupValue> attachmentTypeList;//[pramod.misal][22.04.2025][GEOS2-7248]
        public string senderName;
        private string senderFullName;
        private string sender;
        private string senderEmployeeCode;
        private string emailbody;
        private string senderIdPerson;
        private DateTime dateTime;
        private string subject;
        private string toRecipientName;
        private string cCName;
        private LinkedOffers offerInfo;
        private bool isOfferLoaded;
        Int64 idPORequest;
        bool DoubleClickLinkOffer = false;
        public LinkedOffers initialLinkedOffers { get; set; }
        private ObservableCollection<LogEntryByPOOffer> tempPORequestLog;
        private ObservableCollection<LogEntryByPORequest> listPOChangeLog;
        public PORequestDetails initialPORequestdetail { get; set; }
        public LookupValue initialPORequestStatus { get; set; }
        public ObservableCollection<Emailattachment> initialPOAttachementList;
        private ObservableCollection<LinkedOffers> deletedlinkedofferlist;
        private string linkedOfferListCount;
        private string linkedPolistCount;


        private ObservableCollection<Emailattachment> poTypePOAttachementsList;
        private Visibility isLinkedOffersExist = Visibility.Hidden;
        private ObservableCollection<LinkedOffers> polist;
        private LinkedOffers LinkedPO;
        private bool isLinkedOfferSelected;
        private bool isEditCustomerGroupButtonEnabled;
        private string zoomfactor;
        private string newZoomfactor;
        private Visibility isContractBtnVisible = Visibility.Hidden;
        private Visibility isExpandBtnVisible = Visibility.Hidden;
        private List<ToCCName> cCNameList;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
        private List<ToRecipientName> toRecipientNameList;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
        private Visibility isEmdepContact = Visibility.Hidden;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
        private People peopleContact;

        public People PeopleContact { get; set; }
        private bool isPermissionCustomerEdit=true;//[pallavi.kale][GEOS2-8961][06.11.2025]
         //[Rahul.Gadhave][GEOS2-9880][Date:28-11-2025]
        PdfResultDto pdfResultDto;

        #endregion
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        #region Properties

        //public People PeopleContact
        //{
        //    get { return peopleContact; }
        //    set
        //    {
        //        peopleContact = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("PeopleContact"));
        //    }
        //}


        public Visibility IsEmdepContact
        {
            get
            {
                return isEmdepContact;
            }

            set
            {
                isEmdepContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmdepContact"));
            }
        }

        public List<ToRecipientName> ToRecipientNameList
        {
            get { return toRecipientNameList; }
            set
            {
                toRecipientNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToRecipientNameList"));
            }
        }

        public List<ToCCName> CCNameList
        {
            get { return cCNameList; }
            set
            {
                cCNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CCNameList"));
            }
        }

        public Visibility IsContractBtnVisible
        {
            get
            {
                return isContractBtnVisible;
            }

            set
            {
                isContractBtnVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsContractBtnVisible"));
            }
        }

        public Visibility IsExpandBtnVisible
        {
            get
            {
                return isExpandBtnVisible;
            }

            set
            {
                isExpandBtnVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpandBtnVisible"));
            }
        }

        public string Zoomfactor
        {
            get
            {
                return zoomfactor; ;
            }

            set
            {
                zoomfactor = value;               
                OnPropertyChanged(new PropertyChangedEventArgs("Zoomfactor"));
            }
        }

        public string NewZoomfactor
        {
            get
            {
                return newZoomfactor; ;
            }

            set
            {
                newZoomfactor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewZoomfactor"));
            }
        }

        public bool IsEditCustomerGroupButtonEnabled
        {
            get { return isEditCustomerGroupButtonEnabled; }
            set
            {

                isEditCustomerGroupButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditCustomerGroupButtonEnabled"));

            }
        }
        public bool IsLinkedOfferSelected
        {
            get { return isLinkedOfferSelected; }
            set
            {

                isLinkedOfferSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLinkedOfferSelected"));

            }
        }

        public Visibility IsLinkedOffersExist
        {
            get
            {
                return isLinkedOffersExist;
            }

            set
            {
                isLinkedOffersExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLinkedOffersExist"));
            }
        }


        public ObservableCollection<LinkedOffers> DeletedLinkedofferlist
        {
            get
            {
                return deletedlinkedofferlist;
            }

            set
            {
                deletedlinkedofferlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedLinkedofferlist"));
            }
        }
        public string SenderNameEmployeeCodesWithInitialLetters
        {
            get
            {
                return $"{senderEmployeeCode}_{GetInitials(senderName)}";
            }

        }
        public Int64 IdPORequest
        {
            get
            {
                return idPORequest;
            }
            set
            {
                idPORequest = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPORequest"));
            }
        }
        public int IdSite
        {
            get
            {
                return idSite;
            }
            set
            {

                idSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSite"));
            }
        }

        public int IdStatus
        {
            get
            {
                return idStatus;
            }
            set
            {

                idStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdStatus"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
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
        public string CustomerGroup
        {
            get { return customergroup; }
            set
            {
                customergroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroup"));
            }
        }
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Plant"));
            }
        }
        public string CompanyGroup
        {
            get { return companygroup; }
            set
            {
                companygroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroup"));
            }
        }
        public string CompanyPlant
        {
            get { return companyplant; }
            set
            {
                companyplant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyPlant"));
            }
        }
        public string RFQ
        {
            get { return rfq; }
            set
            {
                rfq = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RFQ"));
            }
        }
        public string OfferTo
        {
            get { return offerto; }
            set
            {
                offerto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferTo"));
            }
        }
        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Amount"));
            }
        }
        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Discount"));
            }
        }
        public string CarriageMethod
        {
            get { return carriagemethod; }
            set
            {
                carriagemethod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarriageMethod"));
            }
        }
        public int SelectedIndexShipTo
        {
            get { return selectedIndexShipTo; }
            set
            {
                selectedIndexShipTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexShipTo"));
            }
        }
        private List<Object> selectedOffer;
        public List<Object> SelectedOffer
        {
            get { return selectedOffer; }
            set
            {
                selectedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOffer"));
            }
        }
        public ObservableCollection<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
            }
        }
        public ObservableCollection<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        public ObservableCollection<LinkedOffers> Linkedofferlist
        {
            get
            {
                return linkedofferlist;
            }

            set
            {
                linkedofferlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Linkedofferlist"));
            }
        }
        public ObservableCollection<LinkedOffers> OfferTypeList
        {
            get
            {
                return offertypeList;
            }

            set
            {
                offertypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferTypeList"));
            }
        }
        public ObservableCollection<ShippingAddress> ShippingAddressList
        {
            get
            {
                return shippingAddressList;
            }

            set
            {
                shippingAddressList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShippingAddressList"));
            }
        }
        public ObservableCollection<LinkedOffers> LinkedPolist
        {
            get
            {
                return linkedpolist;
            }

            set
            {
                linkedpolist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedPolist"));
            }
        }
        public List<LinkedOffers> OfferToContactList
        {
            get { return offerToContactlist; }
            set
            {
                offerToContactlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferToContactList"));
            }
        }
        public ObservableCollection<LinkedOffers> CustomerContactList
        {
            get { return customercontactList; }
            set
            {
                customercontactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerContactList"));
            }
        }
        public ObservableCollection<LookupValue> CarriageMethodList
        {
            get { return carriageMethodList; }
            set
            {
                carriageMethodList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarriageMethodList"));
            }
        }
        public int SelectedIndexOfferType
        {
            get { return selectedIndexOfferType; }
            set
            {
                selectedIndexOfferType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOfferType"));
            }
        }
        public ObservableCollection<CustomerPlant> CustomerPlants
        {
            get
            {
                return customerPlant;
            }

            set
            {
                customerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlants"));
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
        public LookupValue SelectedIndexStatus
        {
            get { return selectedIndexstatus; }
            set
            {
                selectedIndexstatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexStatus"));

                // Ensure OfferInfo is initialized
                if (OfferInfo == null)
                {
                    OfferInfo = new LinkedOffers(); // Replace OfferInfoType with the actual type of OfferInfo
                }

                OfferInfo.SelectedIndexStatus = SelectedIndexStatus;
            }
        }
        public LookupValue SelectedIndexCarriageMethod
        {
            get { return selectedIndexCarriageMethod; }
            set
            {
                selectedIndexCarriageMethod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCarriageMethod"));
            }
        }
        public int SelectedIndexCarriageMethods
        {
            get { return selectedIndexCarriageMethods; }
            set
            {
                selectedIndexCarriageMethods = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCarriageMethods"));
            }
        }
        public LinkedOffers SelectedIndexLinkedOffer
        {
            get { return selectedIndexLinkedOffer; }
            set
            {
                selectedIndexLinkedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLinkedOffer"));
                //LinkedOfferClickAction(SelectedIndexLinkedOffer);
            }
        }
        public LinkedOffers SelectedIndexLinkedPo
        {
            get { return selectedIndexLinkedPo; }
            set
            {
                selectedIndexLinkedPo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLinkedPo"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public string SenderName
        {
            get { return senderName; }
            set
            {
                senderName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SenderName"));
            }
        }
        public string Sender
        {
            get { return sender; }
            set
            {
                sender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sender"));
            }
        }
        public string SenderEmployeeCode
        {
            get { return senderEmployeeCode; }
            set
            {
                senderEmployeeCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SenderEmployeeCode"));
            }
        }

        public string Emailbody
        {
            get { return emailbody; }
            set
            {
                emailbody = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Emailbody"));
            }
        }
        public string SenderIdPerson
        {
            get { return senderIdPerson; }
            set
            {
                senderIdPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SenderIdPerson"));
            }
        }
        public DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateTime"));
            }
        }
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Subject"));
            }
        }
        public string ToRecipientName
        {
            get { return toRecipientName; }
            set
            {
                toRecipientName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToRecipientName"));
            }
        }
        public string CCName
        {
            get { return cCName; }
            set
            {
                cCName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CCName"));
            }
        }
        public ObservableCollection<Emailattachment> POAttachementsList
        {
            get { return pOAttachementsList; }
            set
            {
                pOAttachementsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POAttachementsList"));
            }
        }

        public string AttachmentCnt
        {
            get
            {
                return attachmentCnt;
            }
            set
            {
                attachmentCnt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentCnt"));

            }
        }
        public ObservableCollection<LookupValue> AttachmentTypeList
        {
            get { return attachmentTypeList; }
            set
            {
                attachmentTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentTypeList"));


            }
        }
        public LinkedOffers OfferInfo
        {
            get
            {
                return offerInfo;
            }

            set
            {
                offerInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferInfo"));
            }
        }
        public bool IsOfferLoaded
        {
            get { return isOfferLoaded; }
            set { isOfferLoaded = value; OnPropertyChanged(new PropertyChangedEventArgs("IsOfferLoaded")); }
        }
        public ObservableCollection<LogEntryByPORequest> ListPORequestChangeLog
        {
            get { return listPOChangeLog; }
            set
            {
                SetProperty(ref listPOChangeLog, value, () => ListPORequestChangeLog);
            }
        }


        public string LinkedOfferListCount
        {
            get { return linkedOfferListCount; }
            set
            {
                linkedOfferListCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedOfferListCount"));
            }
        }

        public string LinkedPolistCount
        {
            get { return linkedPolistCount; }
            set
            {
                linkedPolistCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedPolistCount"));
            }
        }

        public ObservableCollection<Emailattachment> PoTypePOAttachementsList
        {
            get { return poTypePOAttachementsList; }
            set
            {
                poTypePOAttachementsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PoTypePOAttachementsList"));
            }
        }

        public ObservableCollection<LinkedOffers> Polist
        {
            get
            {
                return polist;
            }

            set
            {
                polist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Polist"));
            }
        }
        // [Rahul.Gadhave][GEOS2-8655][Date:08-07-2025]
        long PoidCountry;
        public Int64 POIdCountry
        {
            get
            {
                return PoidCountry;
            }
            set
            {
                PoidCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POIdCountry"));
            }
        }
        //[Rahul.Gadhave]
        string country;
        string countryIconUrl;
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryIconUrl"));
            }
        }
        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        long idCustomerPlant;
        public Int64 IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {
                idCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCustomerPlant"));
            }
        }
        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        long idCustomerGroup;
        public Int64 IdCustomerGroup
        {
            get
            {
                return idCustomerGroup;
            }
            set
            {
                idCustomerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCustomerGroup"));
            }
        }
        bool isEmailLoad;
        public bool IsEmailLoad
        {
            get
            {
                return isEmailLoad;
            }
            set
            {
                isEmailLoad = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmailLoad"));
            }
        }
        private ObservableCollection<Customer> customers;
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return customers;
            }

            set
            {
                customers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customers"));
            }
        }
        private int selectedIndexCustomerGroup = -1;
        private int selectedIndexCompanyPlant = -1;
        private ObservableCollection<CustomerPlant> entireCompanyPlantList;
        private ObservableCollection<People> peoplelist;

        
        public ObservableCollection<CustomerPlant> EntireCompanyPlantList
        {
            get
            {
                return entireCompanyPlantList;
            }

            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }

        public ObservableCollection<People> PeopleList
        {
            get
            {
                return peoplelist;
            }

            set
            {
                peoplelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PeopleList"));
            }
        }
        public int SelectedIndexCustomerGroup
        {
            get { return selectedIndexCustomerGroup; }
            set
            {
                selectedIndexCustomerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCustomerGroup"));

                if (selectedIndexCustomerGroup >= 0)
                {
                    CustomerPlants = new ObservableCollection<CustomerPlant>(); CustomerPlants = new ObservableCollection<CustomerPlant>(
                    EntireCompanyPlantList
                    .Where(cpl => cpl.IdCustomer == Customers[SelectedIndexCustomerGroup].IdCustomer || cpl.CustomerPlantName == "---")
                    .OrderBy(cpl => cpl.City)
                    .ToList()
                    );

                    if (CustomerPlants.Count >= 0)
                        SelectedIndexCompanyPlant = -1;
                    else
                        SelectedIndexCompanyPlant = 0;
                    if (SelectedIndexCustomerGroup != -1)
                    {
                        IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                    }
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

                if (selectedIndexCompanyPlant == -1)
                {
                    IdCustomerPlant = 0;
                    CustomerPlants.FirstOrDefault();
                }
                if (SelectedIndexCompanyPlant != -1)
                {
                    IdCustomerPlant = CustomerPlants[selectedIndexCompanyPlant].IdCustomerPlant;
                }
            }
        }
        //[pallavi.kale][GEOS2-8961][06.11.2025]
        public bool IsPermissionCustomerEdit
        {
            get { return isPermissionCustomerEdit; }
            set
            {
                isPermissionCustomerEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionCustomerEdit"));
            }
        }
        #endregion

        #region public ICommand
        public ICommand CommandLinkedOfferClick { get; set; }
        public ICommand AddPORequestsCancelButtonCommand { get; set; }
        public ICommand AddPORequestsAcceptButtonCommand { get; set; }
        public ICommand LinkedOfferOpenCommand { get; set; }
        public ICommand EditCustomerGroupButtonCommand { get; set; }
        public ICommand OpenAttachmentCommand { get; set; }
        public ICommand DoubleClickOnAttachmentCommand { get; set; }
        public ICommand AddLinkedOffersButtonCommand { get; set; } //[pramod.misal][23.04.2025][GEOS2-7250] https://helpdesk.emdep.com/browse/GEOS2-7250
        public ICommand EditValueChangedCommand { get; set; }
        public ICommand DeleteOfferForPO { get; set; }
        public ICommand AddLinkedPOButtonCommand{ get; set; } //[pooja.jadhav][GEOS2-7252][09-05-2025]
        public ICommand EditLinkedPOClickCommand { get; set; }
        public ICommand OpenEmailInNewWindowCommand { get; set; }//[pramod.misal][GEOS2-8643][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-8643                    
        public ICommand LoadedCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand PreviewMouseMoveCommand { get; set; }

        public ICommand ExpandAttachementCommand { get; set; }//[pramod.misal][GEOS2-9225][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9225
        public ICommand ContractAttachementCommand { get; set; }//[pramod.misal][GEOS2-9225][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9225 
        public ICommand POToClickCommand { get; set; }//[pramod.misal][GEOS2-9324][06-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        public ICommand POCcClickCommand { get; set; }//[pramod.misal][GEOS2-9324][06-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        public ICommand SendMailOnEmailIconCommand { get; set; }//[rahul.gadhave][GEOS2-9218][10-11-2025] https://helpdesk.emdep.com/browse/GEOS2-9218
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
        public EditPORequestsViewModel()
        {
            PreviewMouseMoveCommand = new DelegateCommand<object>(PreviewMouseMoveAction);
            CommandLinkedOfferClick = new RelayCommand(new Action<object>(LinkedOfferClickAction));
            AddPORequestsCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            LinkedOfferOpenCommand = new RelayCommand(new Action<object>(LinkedOfferOpenCommandAction));
            EditCustomerGroupButtonCommand = new RelayCommand(new Action<object>(EditCustomerGroupButtonCommandAction));
            OpenAttachmentCommand = new RelayCommand(new Action<object>(OpenAttachmentCommandAction));
            AddLinkedOffersButtonCommand = new RelayCommand(new Action<object>(AddLinkedOffersAction));
            AddPORequestsAcceptButtonCommand = new RelayCommand(new Action<object>(PORequestAcceptCommandAction));
            //EditValueChangedCommand = new RelayCommand(new Action<object> (OnEditValueChanged));
            EditValueChangedCommand = new DelegateCommand<object>(OnEditValueChanged);
            DeleteOfferForPO = new RelayCommand(new Action<object>(DeleteOfferForPOAction));
            //AddLinkedOffersButtonCommand = new RelayCommand(new Action<object>(AddLinkedOffersAction));
            AddLinkedPOButtonCommand = new RelayCommand(new Action<object>(AddLinkedPOAction));
            EditLinkedPOClickCommand = new RelayCommand(new Action<object>(EditLinkedPOClickAction));
            OpenEmailInNewWindowCommand = new RelayCommand(new Action<object>(OpenEmailInNewWindow)); //[pramod.misal][GEOS2-8643][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-8643
            DoubleClickOnAttachmentCommand = new RelayCommand(new Action<object>(DoubleClickOnAttachmentCommandAction));//[pramod.misal][GEOS2-8650][25-06-2025]
            LoadedCommand = new RelayCommand(new Action<object>(LoadedCommandAction));//[rdixit][GEOS2-8305][08.08.2025]
            ExpandAttachementCommand = new RelayCommand(new Action<object>(ExpandAttachementCommandAction)); //[pramod.misal][GEOS2-9225][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9225
            ContractAttachementCommand = new RelayCommand(new Action<object>(ContractAttachementCommandAction)); //[pramod.misal][GEOS2-9225][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9225

            POToClickCommand = new RelayCommand(new Action<object>(POToClickCommandAction)); //[pramod.misal][GEOS2-8643][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-8643
            POCcClickCommand= new RelayCommand(new Action<object>(POCcClickCommandAction)); //[pramod.misal][GEOS2-8643][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-8643

            SendMailOnEmailIconCommand = new RelayCommand(new Action<object>(SendMailOnEmailIconCommandAction)); //[rahul.gadhave][GEOS2-9218][10-11-2025] https://helpdesk.emdep.com/browse/GEOS2-9218
            CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);
            //[pramod.misal][GEOS2-9049][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9049
            if (LinkedPolist==null)
            {
                LinkedPolistCount = "0";
            }
            IsContractBtnVisible = Visibility.Hidden;
            IsExpandBtnVisible = Visibility.Visible;

        }
        #endregion

        #region Methods
        public void PreviewMouseMoveAction(object Obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                //OTMCommon.Instance.Zoomfactor = "80%";//[pramod.misal][10-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9297]
                if (!IsEmailLoad)
                {
                    Processing();
                    Zoomfactor = "80%";
                    NewZoomfactor = "80";
                    OTMCommon.Instance.Zoomfactor = null;
                }                
                //Zoomfactor = "80%";
                IsEmailLoad = true;
                CloseProcessing();
                
                GeosApplication.Instance.Logger.Log("Method LoadedCommandAction Executed...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in LoadedCommandAction: {ex.Message}", category: Category.Exception, priority: Priority.High);
            }
        }
        //[rdixit][GEOS2-8305][08.08.2025]
        public void LoadedCommandAction(object Obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                Task.Run(() =>
                {
                    try
                    {
                        //FillAttachementType();
                        FillPOAttachementsList(MainObj);
                        //FillEmailDetails(MainObj);
                    }
                    catch (Exception ex)
                    {
                    }
                });
                GeosApplication.Instance.Logger.Log("Method LoadedCommandAction Executed...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in LoadedCommandAction: {ex.Message}", category: Category.Exception, priority: Priority.High);
            }
        }
        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {

            try
            {
                #region Offers
                if (e.Column.FieldName == "LinkedPO")
                {
                    List<object> filterItems = new List<object>();

                    //// Blank
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(Blanks)",
                    //    EditValue = CriteriaOperator.Parse("IsNullOrEmpty([LinkedPO])")
                    //});

                    //// Non-blank
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(Non blanks)",
                    //    EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([LinkedPO])")
                    //});

                    // Unique individual names from comma-separated values
                    HashSet<string> uniqueNames = new HashSet<string>();
                    foreach (var poRequest in Linkedofferlist)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.LinkedPO))
                        {
                            string[] names = poRequest.LinkedPO.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var name in names)
                            {
                                string trimmedName = name.Trim();
                                if (!string.IsNullOrWhiteSpace(trimmedName) && uniqueNames.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse("LinkedPO Like ?", $"%{trimmedName}%")
                                    });
                                }
                            }
                        }
                    }
                    // Final assignment (REMOVE the null line!)
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString()).ToList();
                }
                #endregion
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public async Task EditInIt(PORequestDetails Obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInIt ...", category: Category.Info, priority: Priority.Low);
                // [Rahul.Gadhave][GEOS2-8655][Date:08-07-2025]
                MainObj = Obj;
                POIdCountry = Obj.IdCountry;
                PORequestdetail = Obj;
                IdCustomerGroup = Obj.IdCustomerGroup;
                IdCustomerPlant = Obj.IdCustomerPlant;
                IdPORequest = Obj.IdPORequest;
                initialPORequestStatus = (LookupValue)Obj.PORequestStatus.Clone();
                isOfferLoaded = true;
                FillServiceUrl();
                //[rdixit][GEOS2-8305][08.08.2025]
                var tasks = new List<Task>();
                tasks.Add(Task.Run(() => FillStatusList()));
                tasks.Add(Task.Run(() => GetPoRequestLinkedOffers(Obj.Offers)));
                tasks.Add(Task.Run(() => FillCustomerGroupList()));
                tasks.Add(Task.Run(() => FillEntireCompanyPlantList()));
                tasks.Add(Task.Run(() => FillPeopleList()));
                Task.WaitAll(tasks.ToArray());
                SelectedIndexCustomerGroup = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == MainObj.IdCustomerGroup));
                if (CustomerPlants != null)
                {
                    SelectedIndexCompanyPlant = CustomerPlants.IndexOf(CustomerPlants.FirstOrDefault(i => i.IdCustomerPlant == MainObj.IdCustomerPlant));
                    if (SelectedIndexCompanyPlant==-1)
                    {
                        IdCustomerPlant = 0;
                    }
                    
                }

                if (Linkedofferlist?.Count > 0)
                {
                    LinkedOfferListCount = Linkedofferlist.Count.ToString();
                    IsLinkedOffersExist = Visibility.Visible;
                    //[pramod.misal][GEOS2-9049][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9049
                    SelectedIndexLinkedOffer = Linkedofferlist.FirstOrDefault();
                    LinkedOfferClickAction(SelectedIndexLinkedOffer);
                }
                else
                {
                    LinkedOfferListCount = "0";
                    IsLinkedOffersExist = Visibility.Hidden;
                }

                SelectedIndexStatus = StatusList.FirstOrDefault(i => i.IdLookupValue == Obj.PORequestStatus.IdLookupValue);          
                ////[pramod.misal][GEOS2-7247][02.05.2025]
                FillEmailDetails(Obj);
                FillListPORequestChangeLog(IdPORequest, Linkedofferlist);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in EditInIt: {ex.Message}", category: Category.Exception, priority: Priority.High);
            }
        }

        /// <summary>
        /// [pramod.misal][GEOS2-8643][26-05-2025] https://helpdesk.emdep.com/browse/GEOS2-8643
        /// <param name="parameter"></param>
        /// </summary>
        private void OpenEmailInNewWindow(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmailInNewWindow()...", category: Category.Info, priority: Priority.Low);
                Processing();
                if (parameter != null)
                {
                    EmailPreviewWindowModel emailPreviewViewModel = new EmailPreviewWindowModel();
                    EmailPreviewWindow emailPreviewWindow = new EmailPreviewWindow();                   
                    emailPreviewViewModel.InIt(this, Emailbody);
                    emailPreviewViewModel.FillTOCCInIt(ToRecipientNameList, CCNameList, SelectedIndexCustomerGroup, SelectedIndexCompanyPlant);//[pramod.misal][21-11-2025] https://helpdesk.emdep.com/browse/GEOS2-9896
                    EventHandler handle = delegate { emailPreviewWindow.Close(); };
                    emailPreviewViewModel.RequestClose += handle;
                    emailPreviewWindow.DataContext = emailPreviewViewModel;
                    //emailPreviewWindow.ShowDialog();
                    emailPreviewWindow.Show();


                }
                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenEmailInNewWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmailInNewWindow() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmailInNewWindow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenEmailInNewWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// [pramod.misal][GEOS2-9324][06-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        /// <param name="parameter"></param>
        /// </summary>
        private void POToClickCommandAction(object parameter)
        {
            try
            {
                ToRecipientName ToObject = parameter as ToRecipientName;
                GeosApplication.Instance.Logger.Log("Method POToClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                if (ToObject!= null)
                {
                    if (ToObject.IsEmdepContact== Visibility.Hidden)
                    {
                        AddContactViewModel addPOContactViewModel = new AddContactViewModel();
                        AddContactView addPOContactView = new AddContactView();
                        int IdCustomerGroup = 0;
                        int Idplant = 0;
                        if (SelectedIndexCustomerGroup !=-1)
                        {
                            IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                        }
                        if (SelectedIndexCompanyPlant != -1)
                        {
                            Idplant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
                        }
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        addPOContactViewModel.InitCCFromEmail(ToObject, IdCustomerGroup, Idplant);
                        EventHandler handle = delegate { addPOContactView.Close(); };
                        addPOContactViewModel.RequestClose += handle;
                        addPOContactView.DataContext = addPOContactViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        addPOContactView.ShowDialogWindow();
                        if (addPOContactViewModel.IsSave == true)
                        {
                            ToObject.IsEmdepContact = Visibility.Visible;
                            ToObject.IsNotEmdepContact = Visibility.Hidden;
                        }

                    }
                    else
                    {
                        List<Company> salesOwners = OTMCommon.Instance.UserAuthorizedPlantsList.Cast<Company>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdCompany));
                        int t = GeosApplication.Instance.ActiveUser.IdUser;
                        //OTMService = new OTMServiceController("localhost:6699");
                        int IdPerson = OTMService.GetPeopleDetailsbyEmpCode_V2680(ToObject.RecipientName);
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        //PeopleContact = new People();
                        //OTMService = new OTMServiceController("localhost:6699");
                        PeopleContact = (OTMService.GetContactsByIdPermission_V2680(GeosApplication.Instance.ActiveUser.IdUser, null, salesOwnersIds, GeosApplication.Instance.IdUserPermission, IdPerson)); 
                        //int personId = Convert.ToInt32(((People)detailView.DataControl.CurrentItem).IdPerson);
                        EditContactViewModel editContactViewModel = new EditContactViewModel();
                        EditContactView editContactView = new EditContactView();
                       if(PeopleContact != null && PeopleContact.IdPerson > 0)
                        editContactViewModel.InIt(PeopleContact);
                        editContactViewModel.InItPermisssion(IsPermissionCustomerEdit);  //[pallavi.kale][GEOS2-8961][07.11.2025]
                        EventHandler handle = delegate { editContactView.Close(); };
                        editContactViewModel.RequestClose += handle;
                        editContactView.DataContext = editContactViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                        //var ownerInfo = (detailView as FrameworkElement);
                        //editContactView.Owner = Window.GetWindow(ownerInfo);
                        editContactView.ShowDialogWindow();
                    }



                }
               
                
                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method POToClickCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POToClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// [pramod.misal][GEOS2-9324][06-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        /// <param name="parameter"></param>
        /// </summary>
        private void POCcClickCommandAction(object parameter)
        {
            try
            {
                ToCCName ToObject = parameter as ToCCName;
                GeosApplication.Instance.Logger.Log("Method POToClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                if (ToObject != null)
                {
                    if (ToObject.IsEmdepContact == Visibility.Hidden)
                    {
                        AddContactViewModel addPOContactViewModel = new AddContactViewModel();
                        AddContactView addPOContactView = new AddContactView();
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        int IdCustomerGroup = 0;
                        int IdCustomerplant=0;

                        if ((SelectedIndexCustomerGroup != -1))
                        {
                            IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                        }
                        if ((SelectedIndexCompanyPlant != -1))
                        {
                            IdCustomerplant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
                        }
                       

                        addPOContactViewModel.InitTOFromEmail(ToObject, IdCustomerGroup, IdCustomerplant);
                        EventHandler handle = delegate { addPOContactView.Close(); };
                        addPOContactViewModel.RequestClose += handle;
                        addPOContactView.DataContext = addPOContactViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        addPOContactView.ShowDialogWindow();
                        if (addPOContactViewModel.IsSave == true)
                        {
                            ToObject.IsEmdepContact = Visibility.Visible;
                            ToObject.IsNotEmdepContact = Visibility.Hidden;
                        }

                    }
                    else
                    {

                        List<Company> salesOwners = OTMCommon.Instance.UserAuthorizedPlantsList.Cast<Company>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdCompany));
                        int t = GeosApplication.Instance.ActiveUser.IdUser;
                        //OTMService = new OTMServiceController("localhost:6699");
                        int IdPerson = OTMService.GetPeopleDetailsbyEmpCode_V2680(ToObject.CCName);
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        //PeopleContact = new People();
                        PeopleContact = (OTMService.GetContactsByIdPermission_V2680(GeosApplication.Instance.ActiveUser.IdUser, null, salesOwnersIds, GeosApplication.Instance.IdUserPermission, IdPerson));
                        //int personId = Convert.ToInt32(((People)detailView.DataControl.CurrentItem).IdPerson)                                               
                        EditContactViewModel editContactViewModel = new EditContactViewModel();
                        EditContactView editContactView = new EditContactView();
                        if (PeopleContact != null && PeopleContact.IdPerson > 0)
                            editContactViewModel.InIt(PeopleContact);
                        editContactViewModel.InItPermisssion(IsPermissionCustomerEdit);  //[pallavi.kale][GEOS2-8961][07.11.2025]
                        EventHandler handle = delegate { editContactView.Close(); };
                        editContactViewModel.RequestClose += handle;
                        editContactView.DataContext = editContactViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        
                        editContactView.ShowDialogWindow();


                    }



                }


                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method POToClickCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POToClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[pramod.misal][22.04.2025][GEOS2-7248]
        private void FillAttachementType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttachementType()...", category: Category.Info, priority: Priority.Low);

                AttachmentTypeList = new ObservableCollection<LookupValue>(OTMService.GetLookupValues(182).ToList());

                GeosApplication.Instance.Logger.Log(string.Format("Method FillAttachementType()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachementType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachementType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillAttachementType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-7247][02.05.2025]
        private void FillEmailDetails(PORequestDetails obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmailDetails()...", category: Category.Info, priority: Priority.Low);

                if (obj != null)
                {
                    //[pramod.misal][07-08-2025][GEOS2-9187]https://helpdesk.emdep.com/browse/GEOS2-9187
                    SenderName = obj.SenderName;
                    if (!string.IsNullOrEmpty(SenderName))
                    {

                        Sender = obj.Sender;
                        SenderEmployeeCode = obj.SenderEmployeeCode;
                        SenderIdPerson = obj.SenderIdPerson;
                    }

                    if (obj.IdEmail > 0)
                    {
                        FillServiceUrl();
                        Emailbody = OTMService.OTM_GetEmailBodyByIdEmail_V2640(obj.IdEmail);
                    }
                    DateTime = obj.DateTime;
                    Subject = obj.Subject;


                    #region Match With name Old code

                    //if (!string.IsNullOrEmpty(obj.ToRecipientName) || obj.ToRecipientNameList.Count > 0)
                    //{
                    //    //string ToIdPerson = obj.ToIdPerson;
                    //    string UpdatedToRecipientName = obj.ToRecipientName.Replace(",", "; ");
                    //    ToRecipientName = UpdatedToRecipientName;
                    //    //[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
                    //    if (ToRecipientNameList == null || ToRecipientNameList.Count == 0)
                    //        ToRecipientNameList = new List<ToRecipientName>();
                    //    ToRecipientNameList = obj.ToRecipientNameList;


                    //    if (ToRecipientNameList.Count > 0)
                    //    {
                    //        int count = ToRecipientNameList.Count;
                    //        int i = 0;


                    //        ToRecipientNameList.ForEach(to =>
                    //        {


                    //            if (PeopleList.Any(emp =>
                    //            !string.IsNullOrWhiteSpace(emp.FullName) &&
                    //            !string.IsNullOrWhiteSpace(to.RecipientName) &&
                    //            !IsEmail(to.RecipientName) && // New condition
                    //            (
                    //                Normalize(emp.FullName) == Normalize(to.RecipientName) ||
                    //                Normalize(to.RecipientName) == Normalize(emp.FullName)
                    //            )
                    //             ))
                    //            {
                    //                to.IsEmdepContact = Visibility.Visible;
                    //                to.IsNotEmdepContact = Visibility.Hidden;
                    //            }
                    //            else
                    //            {
                    //                to.IsNotEmdepContact = Visibility.Visible;
                    //                to.IsEmdepContact = Visibility.Hidden;
                    //            }
                    //        });

                    //    }


                    //}

                    #endregion


                    if (!string.IsNullOrEmpty(obj.ToRecipientName) || obj.ToRecipientNameList.Count > 0)
                    {
                        string ToIdPerson = obj.ToIdPerson;
                        string UpdatedToRecipientName = obj.ToRecipientName.Replace(",", "; ");
                        ToRecipientName = UpdatedToRecipientName;
                        //[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
                        if (ToRecipientNameList == null || ToRecipientNameList.Count == 0)
                            ToRecipientNameList = new List<ToRecipientName>();
                        ToRecipientNameList = obj.ToRecipientNameList;
                        // ✅ Split ToIdPerson safely
                        var toIdList = ToIdPerson?
                            .Split(',')
                            .Select(id => id.Trim())
                            .Where(id => !string.IsNullOrEmpty(id))
                            .ToList() ?? new List<string>();

                        // ✅ Assign each ID to ToRecipientNameList (index-based)
                        for (int i = 0; i < ToRecipientNameList.Count; i++)
                        {
                            string currentId = (i < toIdList.Count) ? toIdList[i] : null;

                            // skip "null" or empty ids
                            if (!string.IsNullOrEmpty(currentId) && (!(currentId=="null")))
                            {
                                ToRecipientNameList[i].ToIdperson = (currentId);
                            }
                            else
                            {
                                ToRecipientNameList[i].ToIdperson = "null";
                            }
                        }


                        if (ToRecipientNameList.Count > 0)
                        {                            
                            ToRecipientNameList.ForEach(to =>
                            {
                                if (!(to.ToIdperson=="null"))
                                {
                                    if (PeopleList.Any(emp => emp.IdPerson == Convert.ToInt64(to.ToIdperson)) || PeopleList.Any(emp => emp.IdEmployee == Convert.ToInt64(to.ToIdperson)) || PeopleList.Any(emp => emp.IdUser == Convert.ToInt64(to.ToIdperson)))
                                    {
                                        to.IsEmdepContact = Visibility.Visible;
                                        to.IsNotEmdepContact = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        to.IsNotEmdepContact = Visibility.Visible;
                                        to.IsEmdepContact = Visibility.Hidden;
                                    }
                                }                                
                                else
                                {
                                    int IdPerson = OTMService.GetPeopleDetailsbyEmpCode_V2680(to.RecipientName);
                                    if (IdPerson !=0)
                                    {
                                        to.IsEmdepContact = Visibility.Visible;
                                        to.IsNotEmdepContact = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        to.IsNotEmdepContact = Visibility.Visible;
                                        to.IsEmdepContact = Visibility.Hidden;

                                    }
                                   
                                }
                            });

                        }


                       




                        
                    }

                    #region Match with CCname

                    //if (!string.IsNullOrEmpty(obj.CCName) || obj.CCNameList.Count > 0)
                    //{
                    //    string UpdatedCCName = obj.CCName.Replace(",", "; ");
                    //    CCName = UpdatedCCName;
                    //    //[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
                    //    if (CCNameList == null || CCNameList.Count == 0)
                    //        CCNameList = new List<ToCCName>();
                    //    CCNameList = obj.CCNameList;
                    //    if (CCNameList.Count > 0)
                    //    {
                    //        int count = CCNameList.Count;
                    //        int i = 0;


                    //        CCNameList.ForEach(cc =>
                    //        {
                    //            if (PeopleList.Any(emp =>
                    //             !string.IsNullOrWhiteSpace(emp.FullName) &&
                    //             !string.IsNullOrWhiteSpace(cc.CCName) &&
                    //             !IsEmail(cc.CCName) && // New condition
                    //             (
                    //                 Normalize(emp.FullName) == Normalize(cc.CCName) ||
                    //                 Normalize(cc.CCName) == Normalize(emp.FullName)
                    //             )
                    //              ))
                    //            {
                    //                cc.IsEmdepContact = Visibility.Visible;
                    //                cc.IsNotEmdepContact = Visibility.Hidden;
                    //            }
                    //            else
                    //            {
                    //                cc.IsNotEmdepContact = Visibility.Visible;
                    //                cc.IsEmdepContact = Visibility.Hidden;
                    //            }
                    //        });


                    //    }





                    //}

                    #endregion

                    if (!string.IsNullOrEmpty(obj.CCName) || obj.CCNameList.Count > 0)
                    {
                        string CCIdPerson = obj.CCIdPerson;
                        string UpdatedCCName = obj.CCName.Replace(",", "; ");
                        CCName = UpdatedCCName;
                        //[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
                        if (CCNameList == null || CCNameList.Count == 0)
                            CCNameList = new List<ToCCName>();
                        CCNameList = obj.CCNameList;

                        // ✅ Split ToIdPerson safely
                        var ccIdPerson = CCIdPerson
                            .Split(',')
                            .Select(id => id.Trim())
                            .Where(id => !string.IsNullOrEmpty(id))
                            .ToList() ?? new List<string>();

                        // ✅ Assign each ID to ToRecipientNameList (index-based)
                        for (int i = 0; i < CCNameList.Count; i++)
                        {
                            string currentId = (i < ccIdPerson.Count) ? ccIdPerson[i] : null;

                            // skip "null" or empty ids
                            if (!string.IsNullOrEmpty(currentId) && (!(currentId == "null")))
                            {
                                CCNameList[i].CCIdperson = (currentId);
                            }
                            else
                            {
                                CCNameList[i].CCIdperson = "null";
                            }
                        }

                        if (CCNameList.Count > 0)
                        {
                            CCNameList.ForEach(cc =>
                            {
                                if (!(cc.CCIdperson == "null"))
                                {
                                    if (PeopleList.Any(emp => emp.IdPerson == Convert.ToInt64(cc.CCIdperson)) || PeopleList.Any(emp => emp.IdEmployee == Convert.ToInt64(cc.CCIdperson)) || PeopleList.Any(emp => emp.IdUser == Convert.ToInt64(cc.CCIdperson)))
                                    {
                                        cc.IsEmdepContact = Visibility.Visible;
                                        cc.IsNotEmdepContact = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        cc.IsNotEmdepContact = Visibility.Visible;
                                        cc.IsEmdepContact = Visibility.Hidden;
                                    }
                                }
                                else
                                {

                                    int IdPerson = OTMService.GetPeopleDetailsbyEmpCode_V2680((cc.CCName));
                                    if (IdPerson != 0)
                                    {
                                        cc.IsEmdepContact = Visibility.Visible;
                                        cc.IsNotEmdepContact = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        cc.IsNotEmdepContact = Visibility.Visible;
                                        cc.IsEmdepContact = Visibility.Hidden;

                                    }
                                   
                                }


                            });


                        }





                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Method FillEmailDetails()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmailDetails() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmailDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillEmailDetails() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private bool IsEmail(string input)
        {
            return input.Contains("@"); // Or use Regex for more strict check
        }
        // Helper method to normalize and remove accents
        string Normalize(string input)
        {
            return new string(
                input.Trim().ToLower()
                     .Normalize(NormalizationForm.FormD)
                     .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                     .ToArray()
            );
        }
        //[pramod.misal][07-05-2025][GEOS2-7248]
        public void FillPOAttachementsList(PORequestDetails obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPOAttachementsList()...", category: Category.Info, priority: Priority.Low);

                Int64 IdEmail = obj.IdEmail;
                FillServiceUrl();
                //OTMService = new OTMServiceController("localhost:6699");
                POAttachementsList = new ObservableCollection<Emailattachment>(OTMService.OTM_GetEmailAttachementByIdEmail_V2660(IdEmail));
                if (POAttachementsList.Count > 0 || POAttachementsList != null)
                {
                    foreach (Emailattachment item in POAttachementsList)
                    {

                        item.AttachmentImage = IconFromExtension(Path.GetExtension(item.AttachmentName));
                    }
                    AttachmentCnt = POAttachementsList.Count.ToString();
                }
                if (POAttachementsList == null || POAttachementsList.Count == 0)
                {
                    AttachmentCnt = "0";
                }

                initialPOAttachementList = POAttachementsList.Select(x => (Emailattachment)x.Clone()).ToObservableCollection();
                GeosApplication.Instance.Logger.Log("Method FillPOAttachementsList()....executed successfully", category: Category.Info, priority: Priority.Low);
                CloseProcessing();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPOAttachementsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPOAttachementsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillPOAttachementsList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private BitmapImage IconFromExtension(string fileExtension)
        {
            BitmapImage bitmapImage = new BitmapImage();
            if (GeosApplication.FileExtentionIcon == null)
                GeosApplication.FileExtentionIcon = new Dictionary<string, BitmapImage>();
            if (GeosApplication.FileExtentionIcon.Any(i => i.Key.ToString().ToLower() == fileExtension.ToLower()))
            {
                bitmapImage = GeosApplication.FileExtentionIcon.FirstOrDefault(i => i.Key.ToString().ToLower() == fileExtension.ToLower()).Value;
                return bitmapImage;
            }
            else
            {
                // Create a temporary file with the given extension to extract the icon.
                Guid newGuid = Guid.NewGuid();
                string tempFile = Path.Combine(Path.GetTempPath(), newGuid + fileExtension);
                // Create an empty file with the specified extension if it doesn't exist.
                if (!File.Exists(tempFile))
                {
                    File.Create(tempFile).Dispose(); // Create and immediately close the file
                }
                // Extract the associated icon from the file
                Icon icon = Icon.ExtractAssociatedIcon(tempFile);
                // Convert the Icon to a BitmapImage with transparency
                if (icon != null)
                {
                    using (Bitmap bitmap = icon.ToBitmap())
                    {
                        // Make the background transparent by modifying the Bitmap's pixel format
                        bitmap.MakeTransparent(System.Drawing.Color.Transparent); // Replace black with transparency
                        using (MemoryStream iconStream = new MemoryStream())
                        {
                            bitmap.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png); // Save as PNG to retain transparency
                            iconStream.Seek(0, SeekOrigin.Begin); // Reset the stream position
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = iconStream;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            // Cleanup temp file
                            if (File.Exists(tempFile))
                            {
                                File.Delete(tempFile);
                            }
                            GeosApplication.FileExtentionIcon.Add(fileExtension, bitmapImage);
                            return bitmapImage;
                        }
                    }
                }
            }
            return null; // Return null if no icon is found
        }
        private void OpenAttachmentCommandAction(object obj)
        {
            try
            {
                Processing();
                GeosApplication.Instance.Logger.Log("Method OpenAttachmentCommandAction()...", category: Category.Info, priority: Priority.Low);
                Emailattachment Attachment = (Emailattachment)obj;
                Attachment.FileDocInBytes = OTMService.GetPoAttachmentByte(Attachment.AttachmentPath);
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();


                if (Attachment.FileDocInBytes != null)
                {
                    documentViewModel.OpenPORequestAttachment(Attachment);
                    if (documentViewModel.IsPresent)
                    {
                        documentView.DataContext = documentViewModel;
                        documentView.Show();
                    }
                    CloseProcessing();
                    GeosApplication.Instance.Logger.Log("Method OpenAttachmentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }

                else
                {
                    CustomMessageBox.Show(string.Format("Could not find file {0}", Attachment.AttachmentName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                }
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in OpenAttachmentCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in OpenAttachmentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAttachmentCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [pramod.misal][GEOS2-8650][26-06-2025]https://helpdesk.emdep.com/browse/GEOS2-8650
        /// </summary>
        /// <param name="obj"></param>
        private void DoubleClickOnAttachmentCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method DoubleClickOnAttachmentCommandAction()...", category: Category.Info, priority: Priority.Low);
                Emailattachment Attachment = (Emailattachment)obj;
                if (Attachment != null)
                {
                    Processing();
                    //[pramod.misal][GEOS2-8650][26 - 06 - 2025]
                    Attachment.FileDocInBytes = OTMService.GetPoAttachmentByte(Attachment.AttachmentPath);
                    DocumentView documentView = new DocumentView();
                    DocumentViewModel documentViewModel = new DocumentViewModel();


                    if (Attachment.FileDocInBytes != null)
                    {
                        documentViewModel.OpenPORequestAttachment(Attachment);
                        if (documentViewModel.IsPresent)
                        {
                            documentView.DataContext = documentViewModel;
                            documentView.Show();
                        }
                        CloseProcessing();
                        GeosApplication.Instance.Logger.Log("Method DoubleClickOnAttachmentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }

                    else
                    {
                        CustomMessageBox.Show(string.Format("Could not find file {0}", Attachment.AttachmentName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                    }

                }

            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in DoubleClickOnAttachmentCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in DoubleClickOnAttachmentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DoubleClickOnAttachmentCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// https://helpdesk.emdep.com/browse/GEOS2-7250
        /// </summary>
        /// <param name="gcComments"></param>
        public void AddLinkedOffersAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLinkedOffersAction()...", category: Category.Info, priority: Priority.Low);

                Processing();

                PORequestLinkedOffersViewModel linkedOffersViewModel = new PORequestLinkedOffersViewModel();
                PORequestLinkedOffersView linkedOffersView = new PORequestLinkedOffersView();
                linkedOffersViewModel.InIt(Linkedofferlist, IdCustomerGroup, IdCustomerPlant);
                EventHandler handle = delegate { linkedOffersView.Close(); };
                linkedOffersViewModel.RequestClose += handle;
                linkedOffersView.DataContext = linkedOffersViewModel;
                CloseProcessing();
                linkedOffersView.ShowDialogWindow();

                if (linkedOffersViewModel.IsAccepted)
                {
                    if (linkedOffersViewModel.SelectedIndexPOLinkedOffer != null)
                    {
                        if (Linkedofferlist==null)
                        {
                            Linkedofferlist = new ObservableCollection<LinkedOffers>();
                        }
                        linkedOffersViewModel.SelectedIndexPOLinkedOffer.ToList().ForEach(item => Linkedofferlist.Add(item));
                    }
                }
                if (IdCustomerGroup == 0 && linkedOffersViewModel.SelectedIndexPOLinkedOffer!=null)
                {
                    IdCustomerGroup = linkedOffersViewModel.SelectedIndexPOLinkedOffer[0].IdCustomer;
                    SelectedIndexCustomerGroup = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == IdCustomerGroup));
                }
                if (IdCustomerPlant == 0 && linkedOffersViewModel.SelectedIndexPOLinkedOffer != null)
                {
                    IdCustomerPlant = linkedOffersViewModel.SelectedIndexPOLinkedOffer[0].IdSite;
                    SelectedIndexCompanyPlant = CustomerPlants.IndexOf(CustomerPlants.FirstOrDefault(i => i.IdCustomerPlant == IdCustomerPlant));
                }
                if (Linkedofferlist!=null && Linkedofferlist.Count > 0)
                {
                    LinkedOfferListCount = Linkedofferlist.Count.ToString();
                }
                else
                {
                    LinkedOfferListCount = "0";
                }

                GeosApplication.Instance.Logger.Log("Method AddLinkedOffersAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method AddLinkedOffersAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCurrenciesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrenciesList ...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                Currencies = new ObservableCollection<Currency>();
                Currencies = new ObservableCollection<Currency>(OTMServiceThread.GetAllPOCurrencies_V2590().OrderBy(c => c.Name));
                if (Currencies != null)
                {
                    foreach (var bpItem in Currencies.GroupBy(tpa => tpa.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillCurrenciesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert ByteArrayToBitmapImage ...", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log("Method ByteArrayToBitmapImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ByteArrayToBitmapImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        //[Rahul.gadhave][GEOS2-7246][Date:03-06-2025]
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList..."), category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                StatusList = new ObservableCollection<LookupValue>();
                StatusList = OTMServiceThread.GetLookupValues_V2660();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList()....executed successfully"), category: Category.Info, priority: Priority.Low);
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
        private void FillShippingAddressList(LinkedOffers Obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillShippingAddressList ...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                ShippingAddressList = new ObservableCollection<ShippingAddress>();
                //ShippingAddressList = new ObservableCollection<ShippingAddress>(OTMService.OTM_GetShippingAddressForShowAll_V2630(Obj.IdCustomer).OrderBy(sa => sa.Address).ThenBy(sa => sa.City).ThenBy(sa => sa.CountriesName));
                ShippingAddressList = new ObservableCollection<ShippingAddress>(OTMServiceThread.OTM_GetShippingAddress_V2620(Obj.IdSite).OrderBy(sa => sa.Address).ThenBy(sa => sa.City).ThenBy(sa => sa.CountriesName));
                GeosApplication.Instance.Logger.Log("Method FillShippingAddressList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddressList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddressList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        private void FillCarriageMethodList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCarriageMethodList..."), category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                CarriageMethodList = new ObservableCollection<LookupValue>();
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                // [Rahul.Gadhave][GEOS2-8655][Date:08-07-2025]
                //CarriageMethodList = OTMService.GetCarriageMethod_V2640();
                CarriageMethodList = OTMServiceThread.GetCarriageMethod_V2660();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCarriageMethodList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCarriageMethodList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCarriageMethodList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillOfferTo(LinkedOffers Obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Getting All FillOfferTo on list - FillOfferTo()", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                CustomerContactList = new ObservableCollection<LinkedOffers>(OTMServiceThread.GetPoRequestOfferTo_V2640(Obj));
                OfferToContactList = new List<LinkedOffers>(CustomerContactList);
                GeosApplication.Instance.Logger.Log("Getting All FillOfferTo on list successfully - FillOfferTo()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferTo() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);

            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferTo() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillOfferTo() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][GEOS2-9225][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9225
        /// <param name="parameter"></param>
        /// </summary>
        private void ContractAttachementCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ContractAttachementCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                IsContractBtnVisible = Visibility.Hidden;
                IsExpandBtnVisible = Visibility.Visible;
                if (parameter != null)
                {
                    if (parameter is DevExpress.Xpf.Docking.LayoutPanel panel)
                    {
                        // set the width for auto-hide group
                        DevExpress.Xpf.Docking.AutoHideGroup.SetAutoHideSize(panel, new System.Windows.Size(580, panel.ActualHeight));

                        // optional: activate/expand
                        panel.IsActive = true;

                        //var manager = panel.GetDockLayoutManager();
                        //if (manager != null)
                        //{
                        //    // collapse to AutoHide (like clicking Hide button)
                        //    manager.DockController.Hide(panel);
                        //}

                    }

                }
                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method ContractAttachementCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ContractAttachementCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ContractAttachementCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ContractAttachementCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][GEOS2-9225][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9225
        /// <param name="parameter"></param>
        /// </summary>
        private void ExpandAttachementCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExpandAttachementCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                IsContractBtnVisible = Visibility.Visible;
                IsExpandBtnVisible = Visibility.Hidden;
                if (parameter != null)
                {
                    if (parameter is DevExpress.Xpf.Docking.LayoutPanel panel)
                    {
                        // set the width for auto-hide group
                        DevExpress.Xpf.Docking.AutoHideGroup.SetAutoHideSize(panel, new System.Windows.Size(1500, panel.ActualHeight));

                        // optional: activate/expand
                        panel.IsActive = true;
                    }

                }
                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method ExpandAttachementCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExpandAttachementCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExpandAttachementCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ExpandAttachementCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }


        private void LinkedOfferOpenCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Start LinkedOfferOpenCommandAction constructor", category: Category.Info, priority: Priority.Low);
            try
            {
                //OTMService = new OTMServiceController("localhost:6699");
                string basePath = OTMService.GetCommercialOffersPath_V2640();

                EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany));

                basePath = basePath.Replace("{0}", emdepSite.FileServerIP);

                basePath = basePath.Replace("{1}", emdepSite.ShortName);

                string path = OTMService.GetCommercialPath();
                if (SelectedIndexLinkedOffer != null)
                {
                    LinkedOffers Doc = SelectedIndexLinkedOffer;
                    string oldpath = Path.Combine($"{path} {Doc.Year}", $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code);
                    string newPathPath = Path.Combine(basePath, Doc.Year, $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code);


                    //string filePath = completePath + "\\" + offerslink.AttachmentFileName;

                    if (Directory.Exists(newPathPath))
                    {
                        if (Directory.Exists(newPathPath))
                        {
                            //Directory.CreateDirectory(completePath);
                            string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
                            ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
                            info.Arguments = "/n," + "\"" + newPathPath + "\"";
                            info.WindowStyle = ProcessWindowStyle.Normal;
                            System.Diagnostics.Process.Start(info);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), newPathPath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked path: {newPathPath}", category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else if (Directory.Exists(oldpath))
                    {
                        if (Directory.Exists(oldpath))
                        {
                            //Directory.CreateDirectory(completePath);
                            string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
                            ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
                            info.Arguments = "/n," + "\"" + oldpath + "\"";
                            info.WindowStyle = ProcessWindowStyle.Normal;
                            System.Diagnostics.Process.Start(info);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), oldpath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked path: {oldpath}", category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), oldpath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        string NewPath = newPathPath;
                        GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked NewPath: {NewPath}", category: Category.Warn, priority: Priority.Low);
                        string oldPath = oldpath;
                        //GeosApplication.Instance.Logger.Log($"Checking Offer Path: {oldPath}", category: Category.Info, priority: Priority.Low);
                        GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked OldPath: {oldPath}", category: Category.Warn, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("End LinkedOfferOpenCommandAction", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log("Cheaking Offer Path", category: Category.Info, priority: Priority.Low);
            }

        }
        //[rdixit][GEOS2-8305][08.08.2025]
        private void GetPoRequestOfferType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method GetPoRequestOfferType..."), category: Category.Info, priority: Priority.Low);
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                OfferTypeList = new ObservableCollection<LinkedOffers>(OTMServiceThread.OTM_GetPoRequestOfferType_V2640());
                GeosApplication.Instance.Logger.Log(string.Format("Method GetPoRequestOfferType()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetPoRequestOfferType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPoRequestOfferType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-8305][08.08.2025]
        private void GetLinkedPOList(string code)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method GetLinkedPOList..."), category: Category.Info, priority: Priority.Low);
                LinkedPolist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetPoRequestLinkedPO_V2660(offerInfo.Code).OrderBy(x => x.ReceivedIn));
                GeosApplication.Instance.Logger.Log(string.Format("Method GetLinkedPOList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetLinkedPOList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetLinkedPOList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LinkedOfferClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LinkedOfferClickAction()...", category: Category.Info, priority: Priority.Low);
                offerInfo = obj as LinkedOffers;
                IsLinkedOfferSelected = true;
                IsEditCustomerGroupButtonEnabled = true;
                IsOfferLoaded = true;
               
                if (offerInfo != null)
                {
                    Processing();
                    //[rdixit][GEOS2-8305][08.08.2025]
                    FillServiceUrl();
                    var tasks = new List<Task>();
                    // Run all methods in parallel
                    tasks.Add(Task.Run(() => FillOfferTo(offerInfo)));
                    tasks.Add(Task.Run(() => FillShippingAddressList(offerInfo)));
                    tasks.Add(Task.Run(() => FillCurrenciesList()));
                    tasks.Add(Task.Run(() => FillCarriageMethodList()));
                    tasks.Add(Task.Run(() => GetPoRequestOfferType()));
                    //tasks.Add(Task.Run(() => FillServiceUrl()));
                    tasks.Add(Task.Run(() => GetLinkedPOList(offerInfo.Code)));
                    Task.WaitAll(tasks.ToArray());
                    if (LinkedPolist.Count>0)
                    {
                        LinkedPolistCount= LinkedPolist.Count().ToString();
                    }
                    else
                    {
                        LinkedPolistCount = "0";
                    }
                    SelectedIndexLinkedPo = LinkedPolist.FirstOrDefault(i => i.LinkedPO == offerInfo.LinkedPO);
                    IdSite = offerInfo.IdSite;
                    Code = offerInfo.Code;
                    CompanyGroup = offerInfo.CustomerGroup;
                    CompanyPlant = offerInfo.Plant;
                    RFQ = offerInfo.RFQ;
                    Amount = offerInfo.Amount;
                    Discount = offerInfo.Discount;
                    CustomerGroup = offerInfo.CustomerGroup;
                    Plant = offerInfo.Plant;
                    //Rahul.gadhave
                    IdStatus = offerInfo.IdStatus;
                    Country = offerInfo.Country;
                    CountryIconUrl = offerInfo.CountryIconUrl;
                    //end
                    Currencies = new ObservableCollection<Currency>(
                    Currencies.OrderBy(x => x.IdCurrency));
                    SelectedIndexCurrency = Currencies.IndexOf(Currencies.FirstOrDefault(i => i.IdCurrency == offerInfo.IdCurrency));
                    SelectedIndexShipTo = ShippingAddressList.IndexOf(ShippingAddressList.FirstOrDefault(i => i.IdShippingAddress == offerInfo.IdShippingAddress));
                    if (!string.IsNullOrWhiteSpace(offerInfo?.CarriageMethod) && CarriageMethodList != null)
                    {
                        SelectedIndexCarriageMethods = CarriageMethodList.ToList().FindIndex(i => !string.IsNullOrWhiteSpace(i.Value)
                        && (i.Value.IndexOf(offerInfo.CarriageMethod, StringComparison.OrdinalIgnoreCase) >= 0 || offerInfo.CarriageMethod.IndexOf(i.Value, StringComparison.OrdinalIgnoreCase) >= 0));
                    }
                    else
                    {
                        SelectedIndexCarriageMethods = 0;
                    }
                    if (offerInfo.IdPerson != null)
                    {
                        var personIds = offerInfo.IdPerson.Split(',').Select(id => id.Trim()).ToList();
                        SelectedOffer = OfferToContactList.Where(i => personIds.Contains(i.IdPerson.ToString())).Cast<object>().ToList();
                        offerInfo.OffersContact = OfferToContactList.Where(i => personIds.Contains(i.IdPerson.ToString())).Cast<LinkedOffers>().ToList();
                    }
                    offerInfo.Currency = currencies[selectedIndexCurrency].Name;
                    offerInfo.CarriageMethod = CarriageMethodList[selectedIndexCarriageMethods].Value;
                    if (selectedIndexShipTo != -1)
                    {
                        string address = string.Format("{0} {1} {2}", shippingAddressList[selectedIndexShipTo].Address, shippingAddressList[selectedIndexShipTo].ZipCode, shippingAddressList[selectedIndexShipTo].City);
                        offerInfo.ShippingAddress = address;
                    }
                    IsOfferLoaded = false;
                    SelectedIndexOfferType = OfferTypeList.IndexOf(OfferTypeList.FirstOrDefault(i => i.IdOfferType == offerInfo.IdOfferType));
                    DoubleClickLinkOffer = true;
                    initialLinkedOffers = (LinkedOffers)offerInfo.Clone();
                    IsLinkedOffersExist = Visibility.Visible;
                    CloseProcessing();
                    GeosApplication.Instance.Logger.Log("Method LinkedOfferClickAction() executed successfully...", category: Category.Info, priority: Priority.Low);                   
                }
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method LinkedOfferClickAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditCustomerGroupButtonCommandAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerGroupButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                TempCompany = new List<Data.Common.Company>();
                TempCompany.Add(OTMService.GetCompanyDetailsById_V2580(IdSite));
                EditCustomerView editCustomerView = new EditCustomerView();
                EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel();

                editCustomerViewModel.InIt(TempCompany);
                EventHandler handle = delegate { editCustomerView.Close(); };
                editCustomerViewModel.RequestClose += handle;
                editCustomerView.DataContext = editCustomerViewModel;
                CloseProcessing();
                editCustomerView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method EditCustomerGroupButtonCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditCustomerGroupButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void CloseProcessing()
        {
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()...", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
            }
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()....executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void OnEditValueChanged(object obj)
        {
            #region Old Code
            if (isOfferLoaded)
                return;

            var args = obj as DevExpress.Xpf.Editors.EditValueChangedEventArgs;
            if (args == null) return;

            var o = args.OldValue as IList;
            var n = args.NewValue as IList;
            //var oldItems = o.OfType<LinkedOffers>().ToList();
            //var newItems = n.OfType<LinkedOffers>().ToList();
            var oldItems = o != null ? o.OfType<LinkedOffers>().ToList() : new List<LinkedOffers>();
            var newItems = n != null ? n.OfType<LinkedOffers>().ToList() : new List<LinkedOffers>();

            if (oldItems == null || newItems == null)
                return;

            // Items added
            var addedItems = newItems.Except(oldItems).ToList();
            foreach (var added in addedItems)
            {
                added.IsNew = true;
                if (offerInfo.OffersContact == null)
                {
                    offerInfo.OffersContact = new List<LinkedOffers>();
                }
                offerInfo.OffersContact.Add(added);
            }

            // Items removed
            var removedItems = oldItems.Except(newItems).ToList();
            foreach (var removed in removedItems)
            {
                //var m = (LinkedOffers)offerInfo.OffersContact.Where(x => x.IdPerson == removed.IdPerson);
                //m.IsDelete = true;
                offerInfo.OffersContact.Remove(offerInfo.OffersContact.FirstOrDefault(x => x.IdPerson == removed.IdPerson && x.IsNew == true));
                //((LinkedOffers)offerInfo.OffersContact.FirstOrDefault(x => x.IdPerson == removed.IdPerson)).IsDelete = true;
                if (offerInfo.OffersContact.FirstOrDefault(x => x.IdPerson == removed.IdPerson) is LinkedOffers linkedOffer) linkedOffer.IsDelete = true;

            }
            #endregion

        }
        private Action Processing()
        {
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
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }
        /// <summary>
        /// [001][ashish.malkhede] https://helpdesk.emdep.com/browse/GEOS2-7251
        /// </summary>
        /// <param name="obj"></param>
        private void PORequestAcceptCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PORequestAcceptCommandAction()....", category: Category.Info, priority: Priority.Low);
            try
            {
                #region Validation

                #endregion

                #region Save Offer Information

                Processing();

                //POregistereddetail.EmailAttachmentList = POAttachementsList.ToList();
                if (POAttachementsList.Count > 0)
                {
                    foreach (Emailattachment item in POAttachementsList)
                    {
                        item.IdAttachementType = item.AttachmentTypeList[item.SelectedIndexAttachementType].IdLookupValue;
                        item.AttachmentImage = null;
                    }

                }

                //Add Linked Offer
                if (linkedofferlist!=null&&linkedofferlist.Count > 0)
                {
                    linkedofferlist = new ObservableCollection<LinkedOffers>(linkedofferlist.Where(x => x.IsNewLinkedOffer));

                }

                if (DoubleClickLinkOffer == true)
                {
                    //offerInfo.Linkedofferlist = linkedofferlist;
                    //offerInfo.DeletedLinkedofferlist = DeletedLinkedofferlist;
                    offerInfo.RFQ = RFQ;
                    offerInfo.Amount = Amount;
                    offerInfo.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                    offerInfo.Discount = discount;
                    if (selectedIndexShipTo != -1)
                    {
                        offerInfo.IdShippingAddress = Convert.ToInt32(ShippingAddressList[selectedIndexShipTo].IdShippingAddress);
                    }
                    //offerInfo.IdCarriageMethod = CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue;
                    if (OfferInfo.SelectedIndexStatus == null)
                    {
                        OfferInfo.SelectedIndexStatus = new LookupValue();
                    }
                    //OfferInfo.SelectedIndexStatus.IdLookupValue = SelectedIndexStatus.IdLookupValue;
                    if (POIdCountry == offerInfo.IdCountry) // Domestic
                    {
                        if (CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue == 1632) // Air
                        {
                            offerInfo.IdCarriageMethod = 244; // Air (N)
                            offerInfo.CarriageMethod = "Air (N)";
                        }
                        else if (CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue == 1633) // Sea
                        {
                            offerInfo.IdCarriageMethod = 243; // Sea (N)
                            offerInfo.CarriageMethod = "Sea (N)";
                        }
                        else if (CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue == 1634) // Ground
                        {
                            offerInfo.IdCarriageMethod = 242; // Ground (N)
                            offerInfo.CarriageMethod = "Ground (N)";
                        }
                        else
                        {
                            offerInfo.IdCarriageMethod = CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue;
                        }
                    }
                    else // International
                    {
                        if (CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue == 1632) // Air
                        {
                            offerInfo.IdCarriageMethod = 52; // Air (I)
                            offerInfo.CarriageMethod = "Air (I)";
                        }
                        else if (CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue == 1633) // Sea
                        {
                            offerInfo.IdCarriageMethod = 51; // Sea (I)
                            offerInfo.CarriageMethod = "Sea (I)";
                        }
                        else if (CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue == 1634) // Ground
                        {
                            offerInfo.IdCarriageMethod = 50; // Ground (I)
                            offerInfo.CarriageMethod = "Ground (I)";
                        }
                        else
                        {
                            offerInfo.IdCarriageMethod = CarriageMethodList[SelectedIndexCarriageMethods].IdLookupValue;
                        }
                    }
                    OfferInfo.IdPORequest = IdPORequest;
                    PORequestdetail.PORequestStatus.Value = SelectedIndexStatus.Value;
                    PORequestdetail.PORequestStatus.IdLookupValue = SelectedIndexStatus.IdLookupValue;
                    offerInfo.Currency = currencies[selectedIndexCurrency].Name;
                    //offerInfo.CarriageMethod = CarriageMethodList[selectedIndexCarriageMethods].Value;
                    if (selectedIndexShipTo != -1)
                    {
                        string address = string.Format("{0} {1} {2}", shippingAddressList[selectedIndexShipTo].Address, shippingAddressList[selectedIndexShipTo].ZipCode, shippingAddressList[selectedIndexShipTo].City);
                        offerInfo.ShippingAddress = address;
                    }
                }
                PORequestdetail.PORequestStatus.Value = SelectedIndexStatus.Value;
                PORequestdetail.PORequestStatus.IdLookupValue = SelectedIndexStatus.IdLookupValue;
                if (OfferInfo.SelectedIndexStatus != null)
                {
                    PORequestdetail.PORequestStatus.HtmlColor = OfferInfo.SelectedIndexStatus.HtmlColor;
                }

                //OfferInfo.SelectedIndexStatus.IdLookupValue = SelectedIndexStatus.IdLookupValue;
                OfferInfo.IdPORequest = IdPORequest;
                offerInfo.Linkedofferlist = linkedofferlist;
                offerInfo.DeletedLinkedofferlist = DeletedLinkedofferlist;

                #region Change Log
                ChangeLogByOfferInfo(initialLinkedOffers);
                ChangeLogByPoRequest(initialPORequestStatus, initialPOAttachementList);
                if (tempPORequestLog != null)
                {
                    offerInfo.OfferChangeLog = tempPORequestLog.ToList();
                }
                //  Linked Offer
                //if (offerInfo.Linkedofferlist != null)
                //{
                //    if (offerInfo.Linkedofferlist.Count > 0)
                //    {
                //        var s = string.Join(",", offerInfo.Linkedofferlist.Select(x => x.Code));
                //        PORequestdetail.Offers += "," + s;
                //    }
                //}
                if (offerInfo.Linkedofferlist != null && offerInfo.Linkedofferlist.Count > 0)
                {
                    var s = string.Join(",", offerInfo.Linkedofferlist.Select(x => x.Code));
                    if (string.IsNullOrEmpty(PORequestdetail.Offers))
                    {
                        PORequestdetail.Offers = s;
                    }
                    else
                    {
                        PORequestdetail.Offers += "," + s;
                    }
                }
                // unlinked offer
                if (offerInfo.DeletedLinkedofferlist != null)
                {
                    if (offerInfo.DeletedLinkedofferlist.Count > 0)
                    {
                        var codesToRemove = offerInfo.DeletedLinkedofferlist.Select(x => x.Code).ToList();

                        // Split current Offers string into a list, removing empty entries and trimming whitespace
                        var currentOffers = PORequestdetail.Offers
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToList();

                        // Remove codes that match
                        currentOffers.RemoveAll(code => codesToRemove.Contains(code));

                        // Reconstruct the Offers string
                        PORequestdetail.Offers = string.Join(",", currentOffers);
                    }
                }
                #endregion
                FillServiceUrl();
                //OTMService = new OTMServiceController("localhost:6699");
                //bool isSave = OTMService.UpdateOffer(offerInfo, POAttachementsList.ToList());
                offerInfo.CurrencyIconBytes = null;
                offerInfo.ActivityLinkedItemImage = null;
                if(SelectedIndexCustomerGroup!=-1)
                {
                    PORequestdetail.Group = Customers[SelectedIndexCustomerGroup].CustomerName;
                    PORequestdetail.IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                }
                else
                {
                    PORequestdetail.Group = null;
                    PORequestdetail.IdCustomerGroup = 0;
                }
                if (SelectedIndexCompanyPlant!=-1)
                {
                    PORequestdetail.Plant = CustomerPlants[SelectedIndexCompanyPlant].City;
                    PORequestdetail.IdCustomerPlant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;

                }
                else
                {
                    PORequestdetail.Plant = null;
                    PORequestdetail.IdCustomerPlant = 0;

                }




                //OTMService = new OTMServiceController("localhost:6699");
                //bool isSave = OTMService.UpdateOffer(offerInfo, POAttachementsList.ToList());
                bool isSave = OTMService.UpdateOffer_V2670(PORequestdetail.IdEmail, offerInfo, POAttachementsList.ToList(), IdCustomerGroup, IdCustomerPlant);
                if (PORequestdetail.PORequestStatus.IdLookupValue == 2075 && DoubleClickLinkOffer == true)
                {
                    //OTMService = new OTMServiceController("localhost:6699");
                    //[Rahul.Gadhave][GEOS2-7253][Date:05/06/2025]
                    string basePath = OTMService.GetCommercialOffersPath_V2640();
                    EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany));
                    basePath = basePath.Replace("{0}", emdepSite.FileServerIP);
                    basePath = basePath.Replace("{1}", emdepSite.ShortName);

                    string year = OfferInfo.Year;
                    string plant = OfferInfo.PlantFullName;
                    string offerCode = OfferInfo.Code;
                    foreach (var attachment in POAttachementsList)
                    {
                        if (attachment.IdAttachementType == 0)
                            continue;

                        //string destinationSubfolder = attachment.IdAttachementType == 2258 ? "01 - QUOTATIONS"
                        //                            : attachment.IdAttachementType == 2259 ? "02 - WO"
                        //                            : null;
                        //[pramod.misal][GEOS2-8432][13-06-2025]
                        string destinationSubfolder = attachment.IdAttachementType == 2260 ? "01 - QUOTATIONS"  // type offer
                                                    : attachment.IdAttachementType == 2261 ? "02 - WO"  // type general
                                                    : null;


                        if (destinationSubfolder == null)
                            continue;

                        string targetPath = Path.Combine(basePath, year, plant, offerCode, destinationSubfolder);
                        string fallbackOldPath = Path.Combine($"{OTMService.GetCommercialPath()} {year}", plant, offerCode, destinationSubfolder);

                        string finalPathToUse = null;

                        if (Directory.Exists(targetPath))
                        {
                            finalPathToUse = targetPath;
                        }
                        else if (Directory.Exists(fallbackOldPath))
                        {
                            finalPathToUse = fallbackOldPath;
                        }
                        else
                        {
                            Directory.CreateDirectory(targetPath);
                            finalPathToUse = targetPath;
                        }
                        string attpath = attachment.AttachmentPath?.Trim();
                        if (!string.IsNullOrEmpty(attpath) && File.Exists(attpath))
                        {
                            string destinationFile = Path.Combine(finalPathToUse, Path.GetFileName(attachment.AttachmentPath));
                            File.Copy(attachment.AttachmentPath, destinationFile, overwrite: true);
                        }
                    }
                }
                //[Rahul.Gadhave][GEOS2-9883][Date:1-12-2025] https://helpdesk.emdep.com/browse/GEOS2-9883
                if (PORequestdetail.PORequestStatus.IdLookupValue==2075)
                {
                    GenerateEmailPdf(MainObj, POAttachementsList);
                }
                #endregion
                CloseProcessing();
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OfferUpdatedSuccessMsg").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method PORequestAcceptCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in PORequestAcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in PORequestAcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in PORequestAcceptCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][07-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7254
        /// </summary>
        /// <param name="oldOffersData"></param>
        private void ChangeLogByOfferInfo(LinkedOffers oldOffersData)
        {
            GeosApplication.Instance.Logger.Log("Method ChangeLogByOfferInfo ...", category: Category.Info, priority: Priority.Low);
            tempPORequestLog = new ObservableCollection<LogEntryByPOOffer>();
            if (oldOffersData != null)
            {
                if (oldOffersData.RFQ != offerInfo.RFQ)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByRFQ").ToString(), offerInfo.Code.ToString().Trim(), oldOffersData.RFQ.ToString().Trim(), offerInfo.RFQ.ToString().Trim()), IdLogEntryType = 22 });
                }
                if (oldOffersData.Amount != offerInfo.Amount)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByAmount").ToString(), offerInfo.Code.ToString().Trim(), oldOffersData.Amount.ToString().Trim(), offerInfo.Amount.ToString().Trim()), IdLogEntryType = 8 });
                }
                if (oldOffersData.IdCurrency != offerInfo.IdCurrency)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByCurrency").ToString(), offerInfo.Code.ToString().Trim(), oldOffersData.Currency.ToString().Trim(), offerInfo.Currency.ToString().Trim()), IdLogEntryType = 8 });
                }
                if (oldOffersData.Discount != offerInfo.Discount)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByDiscount").ToString(), offerInfo.Code.ToString().Trim(), oldOffersData.Discount.ToString().Trim(), offerInfo.Discount.ToString().Trim()), IdLogEntryType = 1 });
                }
                if (oldOffersData.IdShippingAddress != offerInfo.IdShippingAddress)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByShippingAdress").ToString(), offerInfo.Code.ToString().Trim(), (oldOffersData.ShippingAddress ?? "").ToString().Trim(), offerInfo.ShippingAddress.ToString().Trim()), IdLogEntryType = 11 });
                }
                if (oldOffersData.IdCarriageMethod != offerInfo.IdCarriageMethod)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByCarriageMethod").ToString(), offerInfo.Code.ToString().Trim(), oldOffersData.CarriageMethod.ToString().Trim(), offerInfo.CarriageMethod.ToString().Trim()), IdLogEntryType = 3 });
                }
                if (offerInfo.OffersContact != null)
                {
                    foreach (LinkedOffers clog in offerInfo.OffersContact)
                    {
                        if (clog.IsNew)
                        {
                            tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByOfferToAdded").ToString(), offerInfo.Code.ToString().Trim(), clog.Name.ToString().Trim()), IdLogEntryType = 13 });
                        }
                        else if (clog.IsDelete)
                        {
                            tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = offerInfo.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByLinkedOfferRemoved").ToString(), offerInfo.Code.ToString().Trim(), clog.Name.ToString().Trim()), IdLogEntryType = 13 });
                        }
                    }
                }
            }
            //linked offer log
            if (offerInfo.Linkedofferlist != null)
            {
                foreach (LinkedOffers link in offerInfo.Linkedofferlist)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = PORequestdetail.IdPORequest, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByPORequestOfferLinked").ToString(), link.Code.ToString().Trim()), IdLogEntryType = 25 });
                }
            }
            // unlinked offer log
            if (offerInfo.DeletedLinkedofferlist != null)
            {
                foreach (LinkedOffers unlink in offerInfo.DeletedLinkedofferlist)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = PORequestdetail.IdPORequest, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByPORequestOfferUnLinked").ToString(), unlink.Code.ToString().Trim()), IdLogEntryType = 25 });
                }
            }
            GeosApplication.Instance.Logger.Log("Method ChangeLogByOfferInfo() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            var words = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1)
            {
                // Only one word: return first letter
                return words[0][0].ToString().ToUpper(CultureInfo.InvariantCulture);
            }

            // Multiple words: return first letter of first word + first letter of last word
            string firstInitial = words.First()[0].ToString().ToUpper(CultureInfo.InvariantCulture);
            string lastInitial = words.Last()[0].ToString().ToUpper(CultureInfo.InvariantCulture);

            return firstInitial + lastInitial;
        }
        private void DeleteOfferForPOAction(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLinkedOffer"].ToString(),
                     Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    if (obj != null)
                    {
                        var v = (LinkedOffers)obj;
                        if (Linkedofferlist.Any(x => x.IdOffer == v.IdOffer && x.IsNewLinkedOffer == true))
                        {
                            Linkedofferlist.Remove((LinkedOffers)obj);
                            if (Linkedofferlist.Count > 0)
                            {
                                LinkedOfferListCount = Linkedofferlist.Count.ToString();
                            }
                            else
                            {
                                LinkedOfferListCount = "0";
                            }
                        }
                        else
                        {
                            if (DeletedLinkedofferlist == null)
                                DeletedLinkedofferlist = new ObservableCollection<LinkedOffers>();

                            DeletedLinkedofferlist.Add((LinkedOffers)obj);
                            Linkedofferlist.Remove((LinkedOffers)obj);
                            if (Linkedofferlist.Count > 0)
                            {
                                LinkedOfferListCount = Linkedofferlist.Count.ToString();
                            }
                            else
                            {
                                LinkedOfferListCount = "0";
                            }
                        }
                        if (LinkedOfferListCount == "0")
                        {
                            IsLinkedOffersExist = Visibility.Hidden;

                        }

                        ClearOffersection();
                        IsLinkedOfferSelected = false;
                        IsEditCustomerGroupButtonEnabled = false;

                    }

                    DoubleClickLinkOffer = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void ClearOffersection()
        {

            Code = string.Empty;
            CustomerGroup = string.Empty;
            Plant = string.Empty;
            Country = string.Empty;
            CountryIconUrl = string.Empty;
            RFQ = string.Empty;
            OfferToContactList = null;
            Amount = 0.00;
            Currencies = null;
            Discount = 0.0;
            ShippingAddressList = null;
            CarriageMethodList = null;
            LinkedPolist = null;
            if (LinkedPolist == null)
            {
                LinkedPolistCount = "0";
            }

        }
        /// <summary>
        /// [001][ashish.malkhede][07-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7254
        /// </summary>
        /// <param name="idPORequest"></param>
        /// <param name="linkedOffers"></param>
        public void FillListPORequestChangeLog(long idPORequest, ObservableCollection<LinkedOffers> linkedOffers)
        {
            GeosApplication.Instance.Logger.Log("Method FillListPORequestChangeLog ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (linkedOffers != null)
                {


                    string linkedOfferId = string.Join(",", linkedOffers.Select(o => o.IdOffer));
                    FillServiceUrl();
                    //ListPORequestChangeLog = new ObservableCollection<LogEntryByPORequest>(OTMService.GetAllPORequestChangeLog_V2640(IdPORequest, linkedOfferId));
                    /// [pramod.misal]][05-08-2025][GEOS2-9167]https://helpdesk.emdep.com/browse/GEOS2-9049
                    //OTMService = new OTMServiceController("localhost:6699");
                    ListPORequestChangeLog = new ObservableCollection<LogEntryByPORequest>(OTMService.GetAllPORequestChangeLog_V2660(IdPORequest, linkedOfferId));
                }
                else
                {
                    // Ensure list is always initialized when linkedOffers is null
                    ListPORequestChangeLog = new ObservableCollection<LogEntryByPORequest>();

                }
                GeosApplication.Instance.Logger.Log("Method FillListPORequestChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillListPORequestChangeLog() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillListPORequestChangeLog() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeLogByPoRequest(LookupValue oldPORequestStatus, ObservableCollection<Emailattachment> emailattachments)
        {
            GeosApplication.Instance.Logger.Log("Method ChangeLogByOfferInfo ...", category: Category.Info, priority: Priority.Low);
            if (tempPORequestLog == null)
            {
                tempPORequestLog = new ObservableCollection<LogEntryByPOOffer>();
            }
            if (oldPORequestStatus != null)
            {
                if (oldPORequestStatus.IdLookupValue != PORequestdetail.PORequestStatus.IdLookupValue)
                {
                    tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = PORequestdetail.IdPORequest, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByPORequestStatus").ToString(), oldPORequestStatus.Value.ToString().Trim(), PORequestdetail.PORequestStatus.Value.ToString().Trim()), IdLogEntryType = 25 });
                }
            }
            if (emailattachments.Count > 0)
            {
                foreach (Emailattachment att in emailattachments)
                {
                    var e = POAttachementsList.FirstOrDefault(x => x.IdAttachment == att.IdAttachment);
                    string prvType = att.AttachmentTypeList.FirstOrDefault(x => x.IdLookupValue == att.IdAttachementType).Value;
                    string newType = att.AttachmentTypeList.FirstOrDefault(x => x.IdLookupValue == e.IdAttachementType).Value;
                    if (att.IdAttachementType != e.IdAttachementType)
                    {
                        tempPORequestLog.Add(new LogEntryByPOOffer() { IdOffer = PORequestdetail.IdPORequest, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("PORequestChangeLogByPORequestEmailAttachement").ToString(), att.AttachmentName, prvType.ToString().Trim(), newType.ToString().Trim()), IdLogEntryType = 25 });
                    }
                }
            }
            GeosApplication.Instance.Logger.Log("Method ChangeLogByOfferInfo() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //[001][pooja.jadhav][GEOS2-7252][09-05-2025]
        //[002][ashish.malkhede][GEOS2-9207] https://helpdesk.emdep.com/browse/GEOS2-9207
        public void AddLinkedPOAction(object obj)
        {
            PoTypePOAttachementsList = new ObservableCollection<Emailattachment>();

            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLinkedPOAction()...", category: Category.Info, priority: Priority.Low);

                foreach (Emailattachment item in POAttachementsList)
                {
                    #region Old code
                    //if (item.IdAttachementType == 2257)
                    //{
                    //    PoTypePOAttachementsList.Add(item);
                    //}

                    #endregion
                    //[Rahul.Gadhave][GEOS2-9326][Date:01-09-2025]
                    if (item.IdAttachementType == 2259 || item.SelectedIndexAttachementType == 1)
                    {
                        PoTypePOAttachementsList.Add(item);
                    }
                }
                if (PoTypePOAttachementsList.Count > 0)
                {
                    if (PoTypePOAttachementsList.Count == 1)
                    {
                        if (!GeosApplication.Instance.IsLoadOneTime)
                        {
                            Processing();
                        }
                        offerInfo.Attachment = PoTypePOAttachementsList[0];
                        FillServiceUrl();
                        //OTMService = new OTMServiceController("localhost:6699");
                        PORequestDetails PODetails = OTMService.GetPODetailsbyAttachment_V2680(Convert.ToInt32(offerInfo.Attachment.IdAttachment));//[002]
                        PODetails.OfferInfo = offerInfo;
                        EditRegisteredPOsViewModel editRegisteredPOsViewModel = new EditRegisteredPOsViewModel();
                        EditRegisteredPOsView editRegisteredPOsView = new EditRegisteredPOsView();

                        if (PODetails.IsPOExist)
                        {
                            FillPODetails();
                            LinkedPO.Attachment = PoTypePOAttachementsList[0];
                            if (OTMCommon.Instance.SelectedSinglePlantForPO != null)
                            {
                                OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.SelectedSinglePlantForPO;
                            }
                            LinkedPO.OfferCode = offerInfo.Code;
                            editRegisteredPOsViewModel.EditInIt(LinkedPO);
                            EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                            editRegisteredPOsViewModel.RequestClose += handle;
                            editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                            CloseProcessing();

                            editRegisteredPOsView.ShowDialogWindow();
                        }
                        else
                        {
                            PODetails.IsNewPO = true;
                           // [Rahul.gadhave][GEOS2-9878][Date:19 - 11 - 2025]
                            if (PODetails.Sender == null || PODetails.POdate == DateTime.MinValue)
                            {
                               // OTMService = new OTMServiceController("localhost:6699");
                                PODetails = OTMService.GetPODetailsbyIdEmail_V2690(Convert.ToInt32(MainObj.IdEmail));
                                PODetails.OfferInfo = offerInfo;
                                if (PODetails.Sender == null)
                                {
                                    //OTMService = new OTMServiceController("localhost:6699");
                                    string a = MainObj.Sender;
                                    string b = MainObj.Recipient;
                                    string c = MainObj.CCRecipient;

                                    // Create list and split CC emails
                                    List<string> allEmails = new List<string> { a, b };

                                    if (!string.IsNullOrEmpty(c))
                                    {
                                        allEmails.AddRange(
                                            c.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                        );
                                    }

                                    // Remove duplicates
                                    allEmails = allEmails.Distinct().ToList();
                                    PORequestDetails PODetailsnew = new PORequestDetails();
                                    PODetailsnew = OTMService.GetPODetailsbyEmail_V2690(allEmails);
                                    PODetails.Sender = PODetailsnew.Sender;
                                }
                            }

                            if (OTMCommon.Instance.SelectedSinglePlantForPO != null)
                            {
                                OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.SelectedSinglePlantForPO;
                            }
                            //[Rahul.Gadhave][GEOS2-9326][Date:01-09-2025]
                            if (offerInfo.Attachment.SelectedIndexAttachementType != 1)
                            {
                                MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["GoAheadPOLinkedOffer"].ToString(),
                                Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                if (MessageBoxResult1 == MessageBoxResult.Yes)
                                {
                                    editRegisteredPOsViewModel.InitNewPO(PODetails, offerInfo);
                                    EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                                    editRegisteredPOsViewModel.RequestClose += handle;
                                    editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                                    CloseProcessing();

                                    editRegisteredPOsView.ShowDialogWindow();
                                }
                            }
                            else
                            {
                                editRegisteredPOsViewModel.InitNewPO(PODetails, offerInfo);
                                EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                                editRegisteredPOsViewModel.RequestClose += handle;
                                editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                                CloseProcessing();
                                editRegisteredPOsView.ShowDialogWindow();
                            }
                            //editRegisteredPOsViewModel.InitNewPO(PODetails, offerInfo);
                            //EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                            //editRegisteredPOsViewModel.RequestClose += handle;
                            //editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                            //CloseProcessing();

                            //editRegisteredPOsView.ShowDialogWindow();
                            //[pramod.misal][GEOS2-9109][1-08-2025]
                            if (editRegisteredPOsViewModel.IsNewPo == true)
                            {
                                var details = editRegisteredPOsViewModel.poregistereddetails;
                                // Map properties manually
                                LinkedOffers newLinkedPO = new LinkedOffers
                                {
                                    Code = details.Code,
                                    ReceivedIn = details.ReceptionDateNew,
                                    PoType = details.Type,
                                    IdPOType = details.IdPOType,
                                    Confirmation= "Email Not Send",
                                    IdCustomerPurchaseOrder = Convert.ToInt32(details.IdPO)    
                                };
                                SelectedIndexLinkedOffer = newLinkedPO;
                                LinkedPolist.Add(newLinkedPO);

                                LinkedPolist = new ObservableCollection<LinkedOffers>(LinkedPolist.OrderBy(po => po.ReceivedIn));

                                LinkedPolist.OrderBy(date => date.ReceivedIn);
                                if (LinkedPolist.Count > 0)
                                {
                                    LinkedPolistCount = LinkedPolist.Count().ToString();
                                }
                                else
                                {
                                    LinkedPolistCount = "0";
                                }

                                foreach (var offer in Linkedofferlist)
                                {
                                    if (offer.Code == editRegisteredPOsViewModel.OfferCodeForLinkedPO)
                                    {
                                        if (!string.IsNullOrEmpty(offer.LinkedPO))
                                        {
                                            var linkedPOs = offer.LinkedPO.Split(',').Select(p => p.Trim()).ToList();

                                            if (!linkedPOs.Contains(newLinkedPO.Code))
                                            {
                                                linkedPOs.Add(newLinkedPO.Code);
                                                offer.LinkedPO = string.Join(",", linkedPOs);
                                            }
                                        }
                                        else
                                        {
                                            offer.LinkedPO = newLinkedPO.Code;
                                        }

                                    }
                                }

                            }
                            if (editRegisteredPOsViewModel.poregistereddetails != null)
                            {
                                if (editRegisteredPOsViewModel.poregistereddetails.IdStatus == 3)
                                {
                                    FillServiceUrl();
                                    //OTMService = new OTMServiceController("localhost:6699");
                                    foreach (var offer in Linkedofferlist)
                                    {
                                        if (offer.Code == editRegisteredPOsViewModel.poregistereddetails.OfferCode)
                                        {
                                            offer.Status = "Accepted";
                                            offer.HtmlColor = "#00FF00";
                                            offer.IsNewLinkedOffer = true;
                                        }
                                    }
                                }
                            }
                        }
                        CloseProcessing();
                    }
                    if (PoTypePOAttachementsList.Count > 1)
                    {
                        offerInfo.PoTypePOAttachementsList = PoTypePOAttachementsList;
                        Processing();
                        SelectPORequestViewModel selectPORequestViewModel = new SelectPORequestViewModel();
                        SelectPORequestView selectPORequestView = new SelectPORequestView();
                        EventHandler handle = delegate { selectPORequestView.Close(); };
                        selectPORequestViewModel.RequestClose += handle;
                        selectPORequestView.DataContext = selectPORequestViewModel;
                        selectPORequestViewModel.Init(offerInfo);
                        CloseProcessing();
                        selectPORequestView.ShowDialogWindow();
                        if (selectPORequestViewModel.NewLinkedPo != null)
                        {
                             LinkedPolist.Add(selectPORequestViewModel.NewLinkedPo);
                      
                        }
                    }
                }
                else
                {
                    
                    FillServiceUrl();
                    //OTMService = new OTMServiceController("localhost:6699");
                    //PORequestDetails PODetails = OTMService.GetPODetailsbyAttachment(Convert.ToInt32(offerInfo.Attachment.IdAttachment));
                    PORequestDetails PODetails = new PORequestDetails();
                    FillServiceUrl();
                    // [Rahul.gadhave][GEOS2-9878][Date:19 - 11 - 2025]
                    //OTMService = new OTMServiceController("localhost:6699");
                    PODetails = OTMService.GetPODetailsbyIdEmail_V2690(Convert.ToInt32(MainObj.IdEmail));
                    PODetails.OfferInfo = offerInfo;
                    if(PODetails.Sender==null)
                    {
                        //OTMService = new OTMServiceController("localhost:6699");
                        string a = MainObj.Sender;
                        string b = MainObj.Recipient;
                        string c = MainObj.CCRecipient;

                        // Create list and split CC emails
                        List<string> allEmails = new List<string> { a, b };

                        if (!string.IsNullOrEmpty(c))
                        {
                            allEmails.AddRange(
                                c.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                            );
                        }

                        // Remove duplicates
                        allEmails = allEmails.Distinct().ToList();
                        PORequestDetails PODetailsnew = new PORequestDetails();
                        PODetailsnew = OTMService.GetPODetailsbyEmail_V2690(allEmails);
                        PODetails.Sender = PODetailsnew.Sender;
                    }
                    EditRegisteredPOsViewModel editRegisteredPOsViewModel = new EditRegisteredPOsViewModel();
                    EditRegisteredPOsView editRegisteredPOsView = new EditRegisteredPOsView();
                    PODetails.IsNewPO = true;
                    if (OTMCommon.Instance.SelectedSinglePlantForPO != null)
                    {
                        OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.SelectedSinglePlantForPO;
                    }
                    //[Rahul.Gadhave][GEOS2-9326][Date:01-09-2025]
                    MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["GoAheadPOLinkedOffer"].ToString(),
                    Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult1 == MessageBoxResult.Yes)
                    {
                        if (!GeosApplication.Instance.IsLoadOneTime)
                        {
                            Processing();
                        }
                        //[Rahul.Gadhave][GEOS2-9880][Date:28-11-2025]
                        GenerateEmailPdf(MainObj, POAttachementsList);
                        //editRegisteredPOsViewModel.InitNewPO(PODetails, offerInfo);
                        editRegisteredPOsViewModel.InitNewPOForGoAhed(PODetails, offerInfo, pdfResultDto);
                        EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                        editRegisteredPOsViewModel.RequestClose += handle;
                        editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                        CloseProcessing();
                        editRegisteredPOsView.ShowDialogWindow();

                        if (editRegisteredPOsViewModel.IsNewPo == true)
                        {
                            var details = editRegisteredPOsViewModel.poregistereddetails;
                            // Map properties manually
                            LinkedOffers newLinkedPO = new LinkedOffers
                            {
                                Code = details.Code,
                                ReceivedIn = details.ReceptionDateNew,
                                PoType = details.Type,
                                IdPOType = details.IdPOType,
                                Confirmation = "Email Not Send",
                                IdCustomerPurchaseOrder = Convert.ToInt32(details.IdPO)
                            };
                            SelectedIndexLinkedOffer = newLinkedPO;
                            LinkedPolist.Add(newLinkedPO);

                            LinkedPolist = new ObservableCollection<LinkedOffers>(LinkedPolist.OrderBy(po => po.ReceivedIn));

                            LinkedPolist.OrderBy(date => date.ReceivedIn);
                            if (LinkedPolist.Count > 0)
                            {
                                LinkedPolistCount = LinkedPolist.Count().ToString();
                            }
                            else
                            {
                                LinkedPolistCount = "0";
                            }

                            foreach (var offer in Linkedofferlist)
                            {
                                if (offer.Code == editRegisteredPOsViewModel.OfferCodeForLinkedPO)
                                {
                                    if (!string.IsNullOrEmpty(offer.LinkedPO))
                                    {
                                        var linkedPOs = offer.LinkedPO.Split(',').Select(p => p.Trim()).ToList();

                                        if (!linkedPOs.Contains(newLinkedPO.Code))
                                        {
                                            linkedPOs.Add(newLinkedPO.Code);
                                            offer.LinkedPO = string.Join(",", linkedPOs);
                                        }
                                    }
                                    else
                                    {
                                        offer.LinkedPO = newLinkedPO.Code;
                                    }

                                }
                            }

                        }
                        if (editRegisteredPOsViewModel.poregistereddetails != null)
                        {
                            if (editRegisteredPOsViewModel.poregistereddetails.IdStatus == 3)
                            {
                                FillServiceUrl();
                                //OTMService = new OTMServiceController("localhost:6699");
                                foreach (var offer in Linkedofferlist)
                                {
                                    if (offer.Code == editRegisteredPOsViewModel.poregistereddetails.OfferCode)
                                    {
                                        offer.Status = "Accepted";
                                        offer.HtmlColor = "#00FF00";
                                        offer.IsNewLinkedOffer = true;
                                    }
                                }
                            }
                        }
                    }

                    //CustomMessageBox.Show(string.Format(Application.Current.FindResource("OTMSelectAttachementType").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddLinkedPOAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLinkedPOAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// [001][13-05-2025][ashish.malkhede]https://helpdesk.emdep.com/browse/GEOS2-7252
        /// </summary>
        /// <param name="obj"></param>
        private void EditLinkedPOClickAction(object obj)
        {
            try
            {
                LinkedOffers offers = (LinkedOffers)((DevExpress.Xpf.Grid.GridCellData)obj).Row;
                if (offers != null)
                {
                    offers.OfferCode = offerInfo.Code;
                }
                SelectedIndexLinkedPo = offers;
                GeosApplication.Instance.Logger.Log("Method EditLinkedPOClickAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                EditRegisteredPOsViewModel editRegisteredPOsViewModel = new EditRegisteredPOsViewModel();
                EditRegisteredPOsView editRegisteredPOsView = new EditRegisteredPOsView();
                if (OTMCommon.Instance.SelectedSinglePlantForPO != null)
                {
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.SelectedSinglePlantForPO;
                }
                editRegisteredPOsViewModel.Currencies = Currencies;
                offers.IdOfferCustomerGroup = IdCustomerGroup;
                editRegisteredPOsViewModel.EditInIt(offers);
                EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                editRegisteredPOsViewModel.RequestClose += handle;
                editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                CloseProcessing();

                editRegisteredPOsView.ShowDialogWindow();

                if (editRegisteredPOsViewModel.poregistereddetails != null && SelectedIndexLinkedPo != null && editRegisteredPOsViewModel.IsCancelPo == false)
                {
                    // Find the existing item in the list (by Code or any other unique key)
                    var itemToUpdate = LinkedPolist.FirstOrDefault(s => s == SelectedIndexLinkedPo);

                    if (itemToUpdate != null)
                    {
                        itemToUpdate.Code = editRegisteredPOsViewModel.poregistereddetails.Code;
                        itemToUpdate.ReceivedIn = editRegisteredPOsViewModel.poregistereddetails.ReceptionDateNew;
                        itemToUpdate.PoType = editRegisteredPOsViewModel.poregistereddetails.Type;
                        itemToUpdate.IdPOType = editRegisteredPOsViewModel.poregistereddetails.IdPOType;

                    }
                }


                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Method End EditLinkedPOClickAction()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method EditLinkedPOClickAction()...", category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][14-05-2025][ashish.malkhede]https://helpdesk.emdep.com/browse/GEOS2-7252
        /// </summary>
        private void FillPODetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillPODetails...", category: Category.Info, priority: Priority.Low);
                FillServiceUrl();
                //OTMService = new OTMServiceController("localhost:6699");
                Polist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetPoRequestLinkedPO_V2660(offerInfo.Code));
                // Split offerInfo.LinkedPO into individual PO numbers
                var linkedPOList = offerInfo.LinkedPO
                    ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim()) // remove spaces if any
                    .ToList();

                // Find first matching LinkedPO from the list
                LinkedPO = Polist.FirstOrDefault(i => linkedPOList.Contains(i.LinkedPO));

                if (LinkedPO == null)
                {
                    LinkedPO = new LinkedOffers();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillPODetails()...", category: Category.Info, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-8305][08.08.2025]
        public void FillServiceUrl()
        {
            try
            {
                if (OTMCommon.Instance.SelectedSinglePlantForPO == null)
                {
                    string serviceurl= serviceUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    OTMCommon.Instance.SelectedSinglePlantForPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany);
                    string serviceurl = serviceUrl = ((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    OTMService = new OTMServiceController(serviceurl);
                }
            }
            catch (Exception ex)
            {
            }
        }
        //[rdixit][GEOS2-8305][08.08.2025]
        private void GetPoRequestLinkedOffers(string offers)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method GetPoRequestLinkedOffers..."), category: Category.Info, priority: Priority.Low);
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                if (offers != null)
                    Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMServiceThread.OTM_GetPoRequestLinkedOffers_V2660(offers));
                GeosApplication.Instance.Logger.Log(string.Format("Method GetPoRequestLinkedOffers()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetPoRequestLinkedOffers() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPoRequestLinkedOffers() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        //[rahul.gadhave][GEOS2-9041][Date:21-08-2025]
        private void FillCustomerGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomerGroupList..."), category: Category.Info, priority: Priority.Low);
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                var customerList = OTMServiceThread.GetAllCustomers_V2630();
                Customers = new ObservableCollection<Customer>(customerList.OrderBy(c => c.CustomerName));
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomerGroupList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCustomerGroupList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCustomerGroupList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        //[pramod.misal][GEOS2-9324][01-10-2025]https://helpdesk.emdep.com/browse/GEOS2-9324
        private void FillPeopleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPeopleList ...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                //EntireCompanyPlantList = new ObservableCollection<CustomerPlant>();


                //OTMServiceThread = new OTMServiceController("localhost:6699");
                PeopleList = new ObservableCollection<People>(OTMServiceThread.GetPeoples());

                GeosApplication.Instance.Logger.Log("Method FillPeopleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPeopleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPeopleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }

        }
        private void FillEntireCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEntireCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                EntireCompanyPlantList = new ObservableCollection<CustomerPlant>();
                //EntireCompanyPlantList = new ObservableCollection<CustomerPlant>(OTMService.OTM_GetCustomerPlant_V2590());

                //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
                EntireCompanyPlantList = new ObservableCollection<CustomerPlant>(OTMServiceThread.OTM_GetCustomerPlant_V2620());

                GeosApplication.Instance.Logger.Log("Method FillEntireCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEntireCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEntireCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        //[rahul.gadhave][GEOS2-9218][10-11-2025] https://helpdesk.emdep.com/browse/GEOS2-9218
        private void SendMailOnEmailIconCommandAction(object parameter)
        {
            try
            {
                LinkedOffers offers = (LinkedOffers)((DevExpress.Xpf.Grid.GridCellData)parameter).Row;
                if (offers != null)
                {
                    offers.OfferCode = offerInfo.Code;
                    offers.MailFromEditPoScreen = true;
                }
                SelectedIndexLinkedPo = offers;
                GeosApplication.Instance.Logger.Log("Method EditLinkedPOClickAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                EditRegisteredPOsViewModel editRegisteredPOsViewModel = new EditRegisteredPOsViewModel();
                EditRegisteredPOsView editRegisteredPOsView = new EditRegisteredPOsView();
                if (OTMCommon.Instance.SelectedSinglePlantForPO != null)
                {
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.SelectedSinglePlantForPO;
                }
                editRegisteredPOsViewModel.Currencies = Currencies;
                offers.IdOfferCustomerGroup = IdCustomerGroup;
                editRegisteredPOsViewModel.EditInIt(offers);
                EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                editRegisteredPOsViewModel.RequestClose += handle;
                editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                CloseProcessing();
                offers.MailFromEditPoScreen = false;
            }

            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendMailOnEmailIconCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailOnEmailIconCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }

        }
        //[Rahul.Gadhave][GEOS2-9880][Date:28-11-2025]
        public void GenerateEmailPdf(PORequestDetails data, ObservableCollection<Emailattachment> POAttachementsList)
        {
            try
            {
                //OTMService = new OTMServiceController("localhost:6699");
                string html = OTMService.ReadMailTemplate("OTM_GoAheadPOEmailTemplate.html");
                StringBuilder attachmentRows = new StringBuilder();
                if (POAttachementsList != null && POAttachementsList.Count > 0)
                {
                    foreach (var file in POAttachementsList)
                    {
                        long sizeBytes = file.FileDocInBytes?.Length ?? 0;
                        string sizeFormatted = FormatFileSize(sizeBytes);

                        attachmentRows.AppendLine(
                            $"<tr><td>{file.AttachmentName}</td><td>{sizeFormatted}</td></tr>");
                    }
                }
                else
                {
                    attachmentRows.Append("<tr><td colspan='2'>No attachments</td></tr>");
                }
                // 3. Replace template placeholders
                html = html.Replace("{{Sender}}", data.Sender ?? "")
                           .Replace("{{Recipient}}", data.Recipient ?? "")
                           .Replace("{{CCRecipient}}", data.CCRecipient ?? "")
                           .Replace("{{ReceptionDate}}", data.DateTime.ToString("yyyy-MM-dd HH:mm"))
                           .Replace("{{Subject}}", data.Subject ?? "")
                           .Replace("{{AttachmentRows}}", attachmentRows.ToString())
                           .Replace("{{EmailBodyHtml}}", Emailbody);



                pdfResultDto = OTMService.GenerateEmailPdf_V2690(MainObj.IdEmail, Emailbody, offerInfo.Year, offerInfo.CustomerGroup, offerInfo.Name, offerInfo.Code, html);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate PDF from email.", ex);
            }
        }
        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} B";

            double kb = bytes / 1024.0;
            if (kb < 1024)
                return $"{kb:F1} KB";

            double mb = kb / 1024.0;
            return $"{mb:F1} MB";
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
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error = string.Empty;
                //me[BindableBase.GetPropertyName(() => SelectedIndexPoType)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexCustomerGroup)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +
                //me[BindableBase.GetPropertyName(() => Code)] +
                //me[BindableBase.GetPropertyName(() => ReceptionDate)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexSender)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexShipTo)] +
                //me[BindableBase.GetPropertyName(() => Amount)] +
                ////me[BindableBase.GetPropertyName(() => SelectedIndexCurrency)] +
                //me[BindableBase.GetPropertyName(() => RegisterPOAttachmentSavedFileName)];

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
                //   string PoType = BindableBase.GetPropertyName(() => SelectedIndexPoType);


                //if (columnName == PoType)
                //{
                //    return RegisterPoEditValidation.GetErrorMessage(PoType, SelectedIndexPoType);
                //}
                //else if (columnName == CustomerGroup)
                //    return RegisterPoEditValidation.GetErrorMessage(CustomerGroup, SelectedIndexCustomerGroup);

                return null;
            }

        }
        public void Dispose()
        {

        }
        #endregion
    }
}
