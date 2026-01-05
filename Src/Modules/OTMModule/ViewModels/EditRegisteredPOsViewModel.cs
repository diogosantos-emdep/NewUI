using DevExpress.DataProcessing;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf;
using DevExpress.Xpf.CodeView.View;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking.Platform;
using DevExpress.XtraEditors;
using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;
using static DevExpress.XtraBars.Docking2010.Views.BaseRegistrator;
using DocumentView = Emdep.Geos.Modules.OTM.Views.DocumentView;
using ShippingAddress = Emdep.Geos.Data.Common.OTM.ShippingAddress;
using SplashScreenView = Emdep.Geos.Modules.Crm.Views.SplashScreenView;
using static Emdep.Geos.UI.CustomControls.CustomMessageBox;

namespace Emdep.Geos.Modules.OTM.ViewModels
{

    /// <summary>
    /// //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
    /// </summary>
    public class EditRegisteredPOsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region TaskLog

        /// <summary>
        /// //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>

        #endregion

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");

        #endregion

        #region Declaration
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        private bool isSave;
        private Int64 isSaveIdPo;

        string serviceUrl;
        private ObservableCollection<PORegisteredDetails> poSender;
        private ObservableCollection<PORegisteredDetails> poSenderByGroup;
        private ObservableCollection<CustomerPlant> customerPlant;
        private ObservableCollection<Currency> currencies;
        private ObservableCollection<Customer> customers;
        private ObservableCollection<POType> pOTypeList;
        private ObservableCollection<ShippingAddress> shippingAddressList;
        private int selectedPOSender;
        private int selectedCustomerPlant;
        private int selectedPoType;
        private int selectedGroup;
        private ObservableCollection<CustomerPlant> entireCompanyPlantList;
        private int selectedIndexPoType = -1;
        private int selectedIndexCustomerGroup = -1;
        private int selectedIndexCompanyPlant = -1;
        private int selectedIndexCurrency = -1;
        private int selectedIndexShipTo = -1;
        string code;
        double amount;
        string currency;
        private Int32 selectedIndexSender;
        DateTime receptiondate;
        DateTime? receptiondateNew;

        string shippingaddress;
        string remarks;
        string creatorcode;
        string updatercode;
        string cancelercode;
        string creator;
        DateTime? creationdate;
        string updater;
        DateTime? updaterdate;
        string canceler;
        DateTime? cancellationdate;
        string isok;
        string iscancelled;
        private bool iscancelledKey = false;
        private bool isOkKey = false;
        private Visibility isPoCanceled = Visibility.Hidden;
        private Visibility isAddPO = Visibility.Hidden;
        private Visibility isEditPO = Visibility.Hidden;
        private Visibility isEmailBtnVisibile = Visibility.Visible;
        string OfferCodeForPOType = string.Empty;
        string PoAbbreviation = string.Empty;


        private ObservableCollection<LinkedOffers> linkedOffersList;
        private ObservableCollection<LogEntryByPOOffer> listPOChangeLog;
        private ObservableCollection<LogEntryByPOOffer> tempPOChangeLog;
        public List<Data.Common.Company> TempCompany { get; set; }

        //[pramod.nisal][GEOS2-6463][25 - 11 - 2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        //private bool isOTMCancelPO = false;
        //private bool isOTMFullUpdatePO = false;
        //private bool isOTMUnlinkOfferfromPO;
        private bool isOTMViewOnly = true;
        public bool IsNewPo = false;
        public bool IsCancelPo = false;

        private byte[] fileInBytes;
        private ObservableCollection<RegisterPoAttachments> registerPoAttachmentList;
        private string registerPoAttachmentSavedFileName;
        private string fileName;
        string FileTobeSavedByName = "";
        private List<Object> attachmentObjectList;
        private RegisterPoAttachments selecteddRegisterPoFile;
        private string uniqueFileName;
        string attachmentFileName;
        private ImageSource attachmentImage;
        string selectedType;
        string selectedCustomerGroupName;
        bool isBusy;
        private string informationError;
        ObservableCollection<People> peopleContacts;
        private int preidCurrency;
        private Visibility isfalgavailable = Visibility.Hidden;
        private long idPO;
        double convertedamount;
        private bool isTypeVisisble = false;
        private bool isGroupVisisble = false;
        private bool isPlantVisisble = false;
        private bool isNumberVisisble = false;
        private bool isreceptionDateVisisble = false;
        private bool isSenderVisisble = false;
        private bool isShipToVisisble = false;
        private bool isValueToVisisble = false;
        private bool isCurrencyToVisisble = false;
        private bool isAttachementToVisisble = false;
        private bool isChooseFileToVisisble = false;
        private bool isRemarkToVisisble = false;
        private bool isNokVisisble = false;
        private bool isCanceledVisisble = false;
        private bool isAddLinkofferbtnVisisble = false;
        private bool isUnlinkLinkofferbtnVisisble = false;
        public bool IsPOGoAhead = false;//[pramod.misal][Date:11-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9173]
        private Visibility isCanceled = Visibility.Hidden;
        //[Rahul.Gadhave][GEOS2-7226][Date:24-03-2025]
        private bool isshowall = false;

        private int selectedCustomerGroupForLinkedOffer;//[pramod.misal][01.04.2025][GEOS2-7725]
        private int selectedCustomerPlantForLinkedOffer;
        private bool isPermissionCustomerEdit=true;//[pallavi.kale][GEOS2-8961][06.11.2025]
        private int shipindex; //[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
        private string plantCity;//[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
        PdfResultDto RefpdfResultDto;
        #endregion

        #region Properties
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463

        //[pramod.misal][01.04.2025][GEOS2-7725]
        string windowHeader;

        private bool isShowAllSender = false;


        public string PlantCity
        {
            get
            {
                return plantCity;
            }
            set
            {
                plantCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantCity"));
            }
        }

        public int Shipindex
        {
            get
            {
                return shipindex;
            }
            set
            {
                shipindex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Shipindex"));
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
        public int SelectedCustomerGroupForLinkedOffer
        {
            get
            {
                return selectedCustomerGroupForLinkedOffer;
            }
            set
            {
                selectedCustomerGroupForLinkedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerGroupForLinkedOffer"));
            }
        }

        public int SelectedCustomerPlantForLinkedOffer
        {
            get
            {
                return selectedCustomerPlantForLinkedOffer;
            }
            set
            {
                selectedCustomerPlantForLinkedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPlantForLinkedOffer"));
            }
        }

        public bool IsUnlinkLinkofferbtnVisisble
        {
            get => isUnlinkLinkofferbtnVisisble;
            set
            {
                if (isUnlinkLinkofferbtnVisisble != value)
                {
                    isUnlinkLinkofferbtnVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsUnlinkLinkofferbtnVisisble"));
                }
            }
        }
        public bool IsAddLinkofferbtnVisisble
        {
            get => isAddLinkofferbtnVisisble;
            set
            {
                if (isAddLinkofferbtnVisisble != value)
                {
                    isAddLinkofferbtnVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsAddLinkofferbtnVisisble"));
                }
            }
        }
        public bool IsCanceledVisisble
        {
            get => isCanceledVisisble;
            set
            {
                if (isCanceledVisisble != value)
                {
                    isCanceledVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsCanceledVisisble"));
                }
            }
        }
        public bool IsNokVisisble
        {
            get => isNokVisisble;
            set
            {
                if (isNokVisisble != value)
                {
                    isNokVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsNokVisisble"));
                }
            }
        }
        public bool IsRemarkToVisisble
        {
            get => isRemarkToVisisble;
            set
            {
                if (isRemarkToVisisble != value)
                {
                    isRemarkToVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsRemarkToVisisble"));
                }
            }
        }
        public bool IsChooseFileToVisisble
        {
            get => isChooseFileToVisisble;
            set
            {
                if (isChooseFileToVisisble != value)
                {
                    isChooseFileToVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChooseFileToVisisble"));
                }
            }
        }
        public bool IsAttachementToVisisble
        {
            get => isAttachementToVisisble;
            set
            {
                if (isAttachementToVisisble != value)
                {
                    isAttachementToVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsAttachementToVisisble"));
                }
            }
        }

        public bool IsCurrencyToVisisble
        {
            get => isCurrencyToVisisble;
            set
            {
                if (isCurrencyToVisisble != value)
                {
                    isCurrencyToVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsCurrencyToVisisble"));
                }
            }
        }
        public bool IsValueToVisisble
        {
            get => isValueToVisisble;
            set
            {
                if (isValueToVisisble != value)
                {
                    isValueToVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsValueToVisisble"));
                }
            }
        }

        public bool IsShipToVisisble
        {
            get => isShipToVisisble;
            set
            {
                if (isShipToVisisble != value)
                {
                    isShipToVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsShipToVisisble"));
                }
            }
        }
        public bool IsSenderVisisble
        {
            get => isSenderVisisble;
            set
            {
                if (isSenderVisisble != value)
                {
                    isSenderVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsSenderVisisble"));
                }
            }
        }

        public bool IsreceptionDateVisisble
        {
            get => isreceptionDateVisisble;
            set
            {
                if (isreceptionDateVisisble != value)
                {
                    isreceptionDateVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsreceptionDateVisisble"));
                }
            }
        }


        public bool IsNumberVisisble
        {
            get => isNumberVisisble;
            set
            {
                if (isNumberVisisble != value)
                {
                    isNumberVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsNumberVisisble"));
                }
            }
        }
        public bool IsPlantVisisble
        {
            get => isPlantVisisble;
            set
            {
                if (isPlantVisisble != value)
                {
                    isPlantVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsPlantVisisble"));
                }
            }
        }

        public bool IsGroupVisisble
        {
            get => isGroupVisisble;
            set
            {
                if (isGroupVisisble != value)
                {
                    isGroupVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsGroupVisisble"));
                }
            }
        }

        public bool IsTypeVisisble
        {
            get => isTypeVisisble;
            set
            {
                if (isTypeVisisble != value)
                {
                    isTypeVisisble = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsTypeVisisble"));
                }
            }
        }

        public bool IsOTMViewOnly
        {
            get => isOTMViewOnly;
            set
            {
                if (isOTMViewOnly != value)
                {
                    isOTMViewOnly = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsOTMViewOnly"));
                }
            }
        }

        public double ConvertedAmount
        {
            get { return convertedamount; }
            set
            {
                convertedamount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConvertedAmount"));
            }
        }
        public long IdPO
        {
            get
            {
                return idPO;
            }
            set
            {
                idPO = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPO"));
            }
        }

        public int PreIdCurrency
        {
            get
            {
                return preidCurrency;
            }
            set
            {
                preidCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreIdCurrency"));
            }
        }
        public Visibility Isfalgavailable
        {
            get { return isfalgavailable; }
            set
            {
                isfalgavailable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Isfalgavailable"));
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
        public ObservableCollection<People> PeopleContacts
        {
            get { return peopleContacts; }
            set
            {
                peopleContacts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PeopleContacts"));
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
        public string SelectedCustomerGroupName
        {
            get { return selectedCustomerGroupName; }
            set
            {
                selectedCustomerGroupName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerGroupName"));
            }
        }

        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }

        public ImageSource AttachmentImage
        {
            get { return attachmentImage; }
            set
            {
                attachmentImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentImage"));

            }
        }

        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set
            {
                uniqueFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName"));
            }
        }
        public RegisterPoAttachments SelectedRegisterPoFile
        {
            get
            {
                return selecteddRegisterPoFile;
            }

            set
            {
                selecteddRegisterPoFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegisterPoFile"));

            }
        }
        public ObservableCollection<RegisterPoAttachments> RegisterPoAttachmentList
        {
            get
            {
                return registerPoAttachmentList;
            }
            set
            {
                registerPoAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegisterPoAttachmentList"));

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
        public string RegisterPOAttachmentSavedFileName
        {
            get
            {
                return registerPoAttachmentSavedFileName;
            }
            set
            {
                registerPoAttachmentSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegisterPOAttachmentSavedFileName"));
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
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
            }
        }

        public Int64 IsSaveIdPo
        {
            get { return isSaveIdPo; }
            set
            {
                isSaveIdPo = value;
            }
        }

        public ObservableCollection<LinkedOffers> LinkedOffersDetails
        {
            get
            {
                return linkedOffersList;
            }

            set
            {
                linkedOffersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedOffersDetails"));
            }
        }
        public Visibility IsPoCanceled
        {
            get { return isPoCanceled; }
            set
            {
                isPoCanceled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPoCanceled"));
            }
        }



        public Visibility IsAddPO
        {
            get { return isAddPO; }
            set
            {
                isAddPO = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddPO"));
            }
        }

        public Visibility IsEditPO
        {
            get { return isEditPO; }
            set
            {
                isEditPO = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditPO"));
            }
        }

        public Visibility IsEmailBtnVisibile
        {
            get { return isEmailBtnVisibile; }
            set
            {
                isEmailBtnVisibile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmailBtnVisibile"));
            }
        }
        public ObservableCollection<POType> POTypeList
        {
            get
            {
                return pOTypeList;
            }

            set
            {
                pOTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POTypeList"));
            }
        }

        public Visibility IsCanceled
        {
            get { return isCanceled; }
            set
            {
                isCanceled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCanceled"));
            }
        }
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
        public ObservableCollection<PORegisteredDetails> POSender
        {
            get
            {
                return poSender;
            }

            set
            {
                poSender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POSender"));
            }
        }

        public ObservableCollection<PORegisteredDetails> POSenderByGroup
        {
            get
            {
                return poSenderByGroup;
            }

            set
            {
                poSenderByGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("poSenderByGroup"));
            }
        }
        public int SelectedPOSender
        {
            get
            {
                return selectedPOSender;
            }

            set
            {
                selectedPOSender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPOSender"));
            }
        }
        public int SelectedCustomerPlant
        {
            get
            {
                return selectedCustomerPlant;
            }

            set
            {
                selectedCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPlant"));
            }
        }
        public int SelectedGroup
        {
            get
            {
                return selectedGroup;
            }

            set
            {
                selectedGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedGroup"));

            }
        }
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
        public int SelectedIndexPoType
        {
            get { return selectedIndexPoType; }
            set
            {
                selectedIndexPoType = value;
                if(selectedIndexPoType !=-1)
                {
                    string PoAbbreviation= POTypeList[selectedIndexPoType].Abbreviation;
                    Code = PoAbbreviation + "_" + OfferCodeForPOType;
                    //[pramod.misal][Date:11-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9173]
                    if (POTypeList[selectedIndexPoType].IdPoType == 1)
                    {
                        IsPOGoAhead = true;
                    }
                    else
                    {
                        IsPOGoAhead = false;
                    }
                    
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPoType"));
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
                    CustomerPlants = new ObservableCollection<CustomerPlant>();
                    //CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == Customers[SelectedGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList().GroupBy(cpl => cpl.IdCountry).Select(group => group.First()).ToList());
                    CustomerPlants = new ObservableCollection<CustomerPlant>(
                    EntireCompanyPlantList
                    .Where(cpl => cpl.IdCustomer == Customers[SelectedIndexCustomerGroup].IdCustomer || cpl.CustomerPlantName == "---")
                    .OrderBy(cpl => cpl.City)
                    .ToList()
                    );

                    if (CustomerPlants.Count >= 0)
                        SelectedIndexCompanyPlant = -1;
                    else
                        SelectedIndexCompanyPlant = 0;

                    if (CustomerPlants != null && CustomerPlants.Count > 0)
                    {
                        Isfalgavailable = Visibility.Visible;
                    }
                    else
                    {
                        Isfalgavailable = Visibility.Hidden;
                    }
                    //[pooja.jadhav][GEOS2-7057][12-03-2025] 
                    //FillShippingAddressList();


                    //POSenderByGroup = new ObservableCollection<PORegisteredDetails>(
                        //POSender.Where(pos => pos.IdCustomer == Customers[SelectedIndexCustomerGroup].IdCustomer || pos.SiteName == "---"));
                    if (SelectedIndexSender == -1)
                    {
                        SelectedIndexSender = 0;
                    }

                    //[pramod.misal][01.04.2025][GEOS2-7725]
                    SelectedCustomerGroupForLinkedOffer = Customers[SelectedIndexCustomerGroup].IdCustomer;
                   // SelectedCustomerGroupForLinkedOffer = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;


                }
                // else
                // {
                //// SelectedIndexCompanyPlant = -1;
                // //    CustomerPlants = null;
                // }

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
                    CustomerPlants.FirstOrDefault();
                }

                //[pooja.jadhav][GEOS2-7057][12-03-2025]
                if (selectedIndexCompanyPlant >= 0)
                {
                    FillShippingAddressList();
                    ReorderShippingAddressList(CustomerPlants[SelectedIndexCompanyPlant].Country); //[nsatpute][14.08.2025][GEOS2-9177]
                    FillSenderList(); //[pooja.jadhav][04-09-2025][GEOS2-9322] 
                    SelectedIndexSender = -1;
                }
                if (SelectedIndexShipTo == -1)
                {
                    SelectedIndexShipTo = 0;
                }
                if (SelectedIndexCompanyPlant == -1)
                {
                    if (ShippingAddressList == null)
                    {
                        ShippingAddressList = new ObservableCollection<ShippingAddress>();
                    }
                    else
                    {
                        ShippingAddressList.Clear();
                    }
                }
               
                if (SelectedIndexCompanyPlant == -1)
                {
                    if(POSenderByGroup!=null)
                    {
                        POSenderByGroup.Clear();
                    }
                }

                if (SelectedIndexCompanyPlant != -1)
                {
                    SelectedCustomerPlantForLinkedOffer = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
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
        public int SelectedIndexShipTo
        {
            get { return selectedIndexShipTo; }
            set
            {
                selectedIndexShipTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexShipTo"));
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
        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Amount"));
            }
        }
        public DateTime ReceptionDate
        {
            get { return receptiondate; }
            set
            {
                receptiondate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDate"));
            }
        }
        public DateTime? ReceptionDateNew
        {
            get { return receptiondateNew; }
            set
            {
                receptiondateNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDateNew"));
            }
        }
        public Int32 SelectedIndexSender
        {
            get { return selectedIndexSender; }
            set
            {
                selectedIndexSender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSender"));
            }
        }
        public string ShippingAddress
        {
            get { return shippingaddress; }
            set
            {
                shippingaddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShippingAddress"));
            }
        }
        public PORegisteredDetails poregistereddetails { get; set; }
        public PORegisteredDetails poregistereddetailsforemail { get; set; }
        public PORegisteredDetails initialPORegisteredDetails { get; set; }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }
        public string CreatorCode
        {
            get { return creatorcode; }
            set
            {
                creatorcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatorCode"));
            }
        }
        public string UpdaterCode
        {
            get { return updatercode; }
            set
            {
                updatercode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdaterCode"));
            }
        }
        public string CancelerCode
        {
            get { return cancelercode; }
            set
            {
                cancelercode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancelerCode"));
            }
        }
        public string Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Creator"));
            }
        }
        public DateTime? CreationDate
        {
            get { return creationdate; }
            set
            {
                creationdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDate"));
            }
        }
        public string Updater
        {
            get { return updater; }
            set
            {
                updater = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Updater"));
            }
        }
        public DateTime? UpdaterDate
        {
            get { return updaterdate; }
            set
            {
                updaterdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdaterDate"));
            }
        }
        public string Canceler
        {
            get { return canceler; }
            set
            {
                canceler = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Canceler"));
            }
        }
        public DateTime? CancellationDate
        {
            get { return cancellationdate; }
            set
            {
                cancellationdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancellationDate"));
            }
        }
        public string AttachmentFileName
        {
            get { return attachmentFileName; }
            set
            {
                attachmentFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentFileName"));
            }
        }
        public ObservableCollection<LogEntryByPOOffer> ListPOChangeLog
        {
            get { return listPOChangeLog; }
            set
            {
                SetProperty(ref listPOChangeLog, value, () => ListPOChangeLog);
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

        bool IsPoEmailGoAhed = false;
        #endregion

        #region public ICommand
        public ICommand EditRegisteredPOsViewCancelButtonCommand { get; set; }
        public ICommand AddLinkedOffersButtonCommand { get; set; }
        public ICommand AddSourceButtonCommand { get; set; }
        public ICommand EditContactButtonCommand { get; set; }
        public ICommand EditRegisteredPOsViewAcceptCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand LinkedItemCancelCommand { get; set; }
        public ICommand EditCustomerGroupButtonCommand { get; set; }
        public ICommand ChangeCustomerNameCommand { get; set; }
        public ICommand SendEmailToCustomerButtonCommand { get; set; }
        public ICommand CurrencySelectedIndexChangedCommand { get; set; }
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

        byte[] modifierProfilePhotoBytes;
        public byte[] ModifierProfilePhotoBytes
        {
            get { return modifierProfilePhotoBytes; }
            set
            {
                modifierProfilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifierProfilePhotoBytes"));
            }
        }
        byte[] creatorprofilePhotoBytes;
        public byte[] CreatorProfilePhotoBytes
        {
            get { return creatorprofilePhotoBytes; }
            set
            {
                creatorprofilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatorProfilePhotoBytes"));
            }
        }
        byte[] cancelerprofilePhotoBytes;
        public byte[] CancelerProfilePhotoBytes
        {
            get { return cancelerprofilePhotoBytes; }
            set
            {
                cancelerprofilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancelerProfilePhotoBytes"));
            }
        }
        public string IsOK
        {
            get { return isok; }
            set
            {
                isok = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOK"));
            }
        }
        public string IsCancelled
        {
            get { return iscancelled; }
            set
            {
                iscancelled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancelled"));
            }
        }
        public bool IscancelledKey
        {
            get => iscancelledKey;
            set
            {
                if (iscancelledKey != value)
                {
                    iscancelledKey = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IscancelledKey"));
                }
            }
        }
        public bool IsOkKey
        {
            get => isOkKey;
            set
            {
                if (isOkKey != value)
                {
                    isOkKey = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsOkKey"));
                }
            }
        }
        //[Rahul.Gadhave][GEOS2-7226][Date:24-03-2025]
        public bool IsShowAll
        {
            get => isshowall;
            set
            {
                if (isshowall != value)
                {
                    isshowall = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsShowAll"));
                }
                FillShippingAddressList();
            }
        }

        private int idStatus;
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
        Int64 idOffer;
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOffer"));
            }
        }
        private string offercode;
        public string OfferCode
        {
            get
            {
                return offercode;
            }

            set
            {
                offercode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCode"));
            }
        }

        private string offercodeForlinkPO;
        public string OfferCodeForLinkedPO
        {
            get
            {
                return offercodeForlinkPO;
            }

            set
            {
                offercodeForlinkPO = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCodeForLinkedPO"));
            }
        }

        //[pooja.jadhav][04-09-2025][GEOS2-9322] 
        public bool IsShowAllSender
        {
            get => isShowAllSender;
            set
            {
                if (isShowAllSender != value)
                {
                    isShowAllSender = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsShowAllSender"));
                }
                FillSenderList();

                if (SelectedIndexCompanyPlant == -1)
                {
                    SelectedIndexSender = -1;
                }
                else
                {
                    SelectedIndexSender = 0;
                }
            }
        }
        #endregion

        #region Constructor
        public EditRegisteredPOsViewModel()
        {
            EditRegisteredPOsViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            AddLinkedOffersButtonCommand = new RelayCommand(new Action<object>(AddLinkedOffersAction));
            AddSourceButtonCommand = new RelayCommand(new Action<object>(AddSourceButtonCommandAction));
            EditCustomerGroupButtonCommand = new RelayCommand(new Action<object>(EditCustomerGroupButtonCommandAction));
            EditContactButtonCommand = new RelayCommand(new Action<object>(EditContactButtonCommandAction));
            EditRegisteredPOsViewAcceptCommand = new RelayCommand(new Action<object>(AcceptCommandAction));
            ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
            OpenPDFDocumentCommand = new DelegateCommand<object>(OpenPDFDocument);
            LinkedItemCancelCommand = new DelegateCommand<object>(LinkedItemCancelAction);
            ChangeCustomerNameCommand = new DelegateCommand<object>(ChangeCustomerNameCommandAction); //[pramod.misal][GEOS2-4848][23.11.2023]
            CurrencySelectedIndexChangedCommand = new DelegateCommand<object>(new Action<object>((obj) => { SelectedIndexChangedCommandAction(obj); }));
            SendEmailToCustomerButtonCommand = new RelayCommand(new Action<object>(SendEmailToCustomerButtonCommandCommandAction));//[pramod.misal][GEOS2-5077][17-01-2024]

            //[pramod.misal][20.12.2024][GEOS2-6465]
            Setpermission();
        }
        #endregion

        #region Methods

        //[pramod.misal][20.12.2024][GEOS2-6465]
        private void Setpermission()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Setpermission ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.IsOTMViewOnly
                && !GeosApplication.Instance.IsOTMFullUpdatePO && !GeosApplication.Instance.IsOTMCancelPO && !GeosApplication.Instance.IsOTMUnlinkOfferfromPO)
                {
                    IsTypeVisisble = true;
                    IsGroupVisisble = true;
                    isPlantVisisble = true;
                    IsNumberVisisble = true;
                    IsreceptionDateVisisble = true;
                    IsSenderVisisble = true;
                    IsShipToVisisble = true;
                    IsValueToVisisble = true;
                    IsCurrencyToVisisble = true;
                    IsAttachementToVisisble = true;
                    IsChooseFileToVisisble = true;
                    IsRemarkToVisisble = true;
                    IsNokVisisble = true;
                    IsCanceledVisisble = true;
                    IsAddLinkofferbtnVisisble = true;
                    IsUnlinkLinkofferbtnVisisble = true;

                }
                else if (GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMFullUpdatePO &&
                    !GeosApplication.Instance.IsOTMCancelPO && !GeosApplication.Instance.IsOTMUnlinkOfferfromPO)
                {
                    IsTypeVisisble = true;
                    IsGroupVisisble = true;
                    isPlantVisisble = true;
                    IsreceptionDateVisisble = true;
                    IsShipToVisisble = true;
                    IsValueToVisisble = true;
                    IsCurrencyToVisisble = true;

                }
                else if ((GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMCancelPO &&
                    !GeosApplication.Instance.IsOTMFullUpdatePO && !GeosApplication.Instance.IsOTMUnlinkOfferfromPO))
                {
                    //for cancel
                    IsCanceledVisisble = true;
                }
                else if (GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMUnlinkOfferfromPO &&
                    !GeosApplication.Instance.IsOTMFullUpdatePO && !GeosApplication.Instance.IsOTMCancelPO)
                {
                    //Unlink
                    IsAddLinkofferbtnVisisble = true;
                    IsUnlinkLinkofferbtnVisisble = true;
                }
                else if (GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMUnlinkOfferfromPO && GeosApplication.Instance.IsOTMFullUpdatePO &&
                    !GeosApplication.Instance.IsOTMCancelPO)
                {
                    //for update
                    IsTypeVisisble = true;
                    IsGroupVisisble = true;
                    isPlantVisisble = true;
                    IsreceptionDateVisisble = true;
                    IsShipToVisisble = true;
                    IsValueToVisisble = true;
                    IsCurrencyToVisisble = true;
                    //unlink
                    IsAddLinkofferbtnVisisble = true;
                    IsUnlinkLinkofferbtnVisisble = true;

                }
                else if (GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMFullUpdatePO && GeosApplication.Instance.IsOTMCancelPO && GeosApplication.Instance.IsOTMUnlinkOfferfromPO)
                {

                    IsTypeVisisble = true;
                    IsGroupVisisble = true;
                    isPlantVisisble = true;
                    IsNumberVisisble = true;
                    IsreceptionDateVisisble = true;
                    IsSenderVisisble = true;
                    IsShipToVisisble = true;
                    IsValueToVisisble = true;
                    IsCurrencyToVisisble = true;
                    IsAttachementToVisisble = true;
                    IsChooseFileToVisisble = true;
                    IsRemarkToVisisble = true;
                    IsNokVisisble = true;
                    IsCanceledVisisble = true;
                    IsAddLinkofferbtnVisisble = true;
                    IsUnlinkLinkofferbtnVisisble = true;
                }

                else if (GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMCancelPO && GeosApplication.Instance.IsOTMUnlinkOfferfromPO)
                {
                    IsCanceledVisisble = true;
                    IsAddLinkofferbtnVisisble = true;
                    IsUnlinkLinkofferbtnVisisble = true;
                }

                else if (GeosApplication.Instance.IsOTMViewOnly && GeosApplication.Instance.IsOTMCancelPO && GeosApplication.Instance.IsOTMFullUpdatePO)
                {
                    //for update
                    IsTypeVisisble = true;
                    IsGroupVisisble = true;
                    isPlantVisisble = true;
                    IsreceptionDateVisisble = true;
                    IsShipToVisisble = true;
                    IsValueToVisisble = true;
                    IsCurrencyToVisisble = true;
                    //canceled
                    IsCanceledVisisble = true;

                }


                //[pooja.jadhav][25-03-2025][GEOS2-7224]
                //[ashish.malkhede][03-04-2025][GEOS2-7223]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 142))
                {
                    IsreceptionDateVisisble = true;
                    IsUnlinkLinkofferbtnVisisble = true;
                }
                else
                {
                    IsreceptionDateVisisble = false;
                    IsUnlinkLinkofferbtnVisisble = false;

                }
                GeosApplication.Instance.Logger.Log("Method Setpermission()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in Setpermission: {ex.Message}", category: Category.Exception, priority: Priority.High);
            }
        }

        private void ChangeCustomerNameCommandAction(object obj)
        {
            //CustomerPlants.FirstOrDefault();
            //var closePopupArgs = obj as DevExpress.Xpf.Editors.ClosePopupEventArgs;
            //if (closePopupArgs?.EditValue is Emdep.Geos.Data.Common.Customer selectedCustomer)
            //{
            //    int id = selectedCustomer.IdCustomer;

            //    SelectedIndexCompanyPlant = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == id));
            //}
            //SelectedIndexCompanyPlant = 0;
        }
        /// <summary>
        /// [001][ashish.malkhede][07-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6464
        /// </summary>
        /// <param name="ObjPORegistered"></param>
        public void EditInIt(PORegisteredDetails ObjPORegistered)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInIt ...", category: Category.Info, priority: Priority.Low);
                IsEditPO = Visibility.Visible;
                IsAddPO = Visibility.Hidden;
                poregistereddetailsforemail = ObjPORegistered;
                poregistereddetails = ObjPORegistered;
                initialPORegisteredDetails = (PORegisteredDetails)ObjPORegistered.Clone();
                IdPO = poregistereddetails.IdPO;
                FillServiceUrlNew();
                var tasks = new List<Task>();
                //[GEOS2-8305][rdixit][22.08.2025]
                // Run all methods in parallel
                tasks.Add(Task.Run(() => FillLinkedOfferList()));
                tasks.Add(Task.Run(() => FillListPOChangeLog()));
                tasks.Add(Task.Run(() => FillPOTypeList()));
                tasks.Add(Task.Run(() => FillCurrenciesList()));
                tasks.Add(Task.Run(() => FillEntireCompanyPlantList())); 
                tasks.Add(Task.Run(() => FillCustomerGroupList()));
                tasks.Add(Task.Run(() => FillPOSenderList()));
                tasks.Add(Task.Run(() => FillEmployeeCode()));
                Task.WaitAll(tasks.ToArray());

                AssignImageToLinkedOffers();

                SelectedIndexPoType = POTypeList.IndexOf(POTypeList.FirstOrDefault(i => i.IdPoType == poregistereddetails.IdPOType));
                SelectedType = POTypeList.FirstOrDefault(i => i.IdPoType == poregistereddetails.IdPOType).Type;
                SelectedIndexCustomerGroup = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == poregistereddetails.IdCustomerPlant));
                SelectedCustomerGroupName = Customers.FirstOrDefault(i => i.IdCustomer == poregistereddetails.IdCustomerPlant).CustomerName;
                SelectedIndexCompanyPlant = CustomerPlants.IndexOf(CustomerPlants.FirstOrDefault(i => i.IdCustomerPlant == poregistereddetails.IdSite));
                CreatorCode = poregistereddetails.CreatorCode;
                UpdaterCode = poregistereddetails.UpdaterCode;
                CancelerCode = poregistereddetails.CancelerCode;
                Creator = poregistereddetails.Creator;
                CreationDate = poregistereddetails.CreationDate;
                Updater = poregistereddetails.Updater;
                UpdaterDate = poregistereddetails.UpdaterDate;
                Canceler = poregistereddetails.Canceler;
                CancellationDate = poregistereddetails.CancellationDate;
                Code = poregistereddetails.Code;
                ReceptionDate = poregistereddetails.ReceptionDate;


                string ReceptionDateString = poregistereddetails.ReceptionDate.ToString(); // Assuming it's a string
                DateTime? ReceptiondateNew = null;
                if (DateTime.TryParse(ReceptionDateString, out DateTime parsedDate))
                {
                    ReceptiondateNew = parsedDate; // Assign the parsed date if successful
                }
                ReceptionDateNew = ReceptiondateNew;
                ReceptionDateNew = (ReceptiondateNew.HasValue && ReceptiondateNew.Value != DateTime.MinValue) ? ReceptiondateNew : null;

                SelectedIndexSender = POSenderByGroup.IndexOf(POSenderByGroup.FirstOrDefault(i => i.IdPerson == poregistereddetails.IdSender));

                bool exists = POSenderByGroup.Any(i => i.IdPerson == poregistereddetails.IdSender);
                if (!exists && poregistereddetails.IdSender!=0)
                {
                    IsShowAllSender = true;
                    SelectedIndexSender = POSenderByGroup.IndexOf(POSenderByGroup.FirstOrDefault(i => i.IdPerson == poregistereddetails.IdSender));
                }
                    
                var existingShippingAddress = ShippingAddressList.FirstOrDefault(i => i.IdShippingAddress == poregistereddetails.IdShippingAddress);
                if (existingShippingAddress == null)
                {
                    if (poregistereddetails.IdShippingAddress != 0)
                    {
                        var cleanedAddress = poregistereddetails.Address;
                        if (!string.IsNullOrEmpty(poregistereddetails.CountriesName))
                        {
                            var toRemove = $"({poregistereddetails.CountriesName})";
                            cleanedAddress = cleanedAddress.Replace(toRemove, "").Trim();
                        }

                        var newShippingAddress = new ShippingAddress
                        {
                            IdShippingAddress = poregistereddetails.IdShippingAddress,
                            Address = cleanedAddress,
                            ZipCode = poregistereddetails.ZipCode,
                            City = poregistereddetails.City,
                            CountryIconUrl = poregistereddetails.CountryIconUrl,
                            CountriesName = poregistereddetails.CountriesName,
                            Region = poregistereddetails.Region
                        };
                        // Add the new ShippingAddress to the list
                        ShippingAddressList.Add(newShippingAddress);
                    }
                }
                if (poregistereddetails.offersLinked != null && poregistereddetails.offersLinked.Count > 0)
                {
                    var offer = poregistereddetails.offersLinked.FirstOrDefault();
                    OfferCodeForPOType = offer.Code;
                }
                //SelectedIndexShipTo = ShippingAddressList.IndexOf(ShippingAddressList.FirstOrDefault(i => i.IdShippingAddress == poregistereddetails.IdShippingAddress));
                Amount = poregistereddetails.POValue;
                Remarks = poregistereddetails.Remarks;
                SelectedIndexCurrency = Currencies.IndexOf(Currencies.FirstOrDefault(i => i.IdCurrency == poregistereddetails.IdCurrency));
                PreIdCurrency = poregistereddetails.IdCurrency;
                IsOK = poregistereddetails.IsOK;
                IsCancelled = poregistereddetails.IsCancelled;
                RegisterPOAttachmentSavedFileName = poregistereddetails.AttachmentFileName;
                ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(RegisterPOAttachmentSavedFileName, true);
                AttachmentImage = objImage;

                if (CreatorCode != null)
                    CreatorProfilePhotoBytes = GetEmployeesImage_V2590(CreatorCode);

                if (UpdaterCode != null)
                    ModifierProfilePhotoBytes = GetEmployeesImage_V2590(UpdaterCode);

                if (CancelerCode != null)
                    CancelerProfilePhotoBytes = GetEmployeesImage_V2590(CancelerCode);

                // UI Updates
                IsOkKey = IsOK == "PO Not OK";
                IscancelledKey = IsCancelled == "PO Cancelled";
                IsPoCanceled = IsCancelled == "PO Cancelled" ? Visibility.Visible : Visibility.Hidden;
                IsCanceled = IsPoCanceled;
                GeosApplication.Instance.Logger.Log("Method EditInIt()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in EditInIt: {ex.Message}", category: Category.Exception, priority: Priority.High);
            }

        }

        //[GEOS2-8305][rdixit][22.08.2025]
        void AssignImageToLinkedOffers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AssignImageToLinkedOffers ...", category: Category.Info, priority: Priority.Low);
                if (LinkedOffersDetails != null)
                {
                    foreach (var linkedOffer in LinkedOffersDetails)
                    {
                        string customerName = linkedOffer.CutomerName;

                        if (!string.IsNullOrEmpty(customerName))
                        {
                            byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customerName);

                            if (bytes != null)
                            {
                                // Assuming linkedOffer has a property to hold image data (e.g., LinkedOfferImage).
                                linkedOffer.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                            }
                            else
                            {
                                // Set a default image based on the theme.
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/wAccount.png");
                                }
                                else
                                {
                                    linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/blueAccount.png");
                                }
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AssignImageToLinkedOffers()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in AssignImageToLinkedOffers: {ex.Message}", category: Category.Exception, priority: Priority.High);
            }
        }
        //}
        /// <summary>
        /// [001][ashish.malkhede][01/08/2025] https://helpdesk.emdep.com/browse/GEOS2-9115
        /// </summary>
        /// <param name="PODetails"></param>
        /// <param name="SelectedofferInfo"></param>
        public void InitNewPO(PORequestDetails PODetails, LinkedOffers SelectedofferInfo)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitNewPO()....", category: Category.Info, priority: Priority.Low);
                var tasks = new List<Task>();
                //[GEOS2-8305][rdixit][22.08.2025]
                // Run all methods in parallel
                FillServiceUrlNew();
                tasks.Add(Task.Run(() => FillPOTypeList()));
                tasks.Add(Task.Run(() => FillCurrenciesList()));
                tasks.Add(Task.Run(() => FillEntireCompanyPlantList()));
                tasks.Add(Task.Run(() => FillCustomerGroupListForNewPO(PODetails.OfferInfo, SelectedofferInfo)));
                tasks.Add(Task.Run(() => FillPOSenderList()));
                tasks.Add(Task.Run(() => FillEmployeeCode()));
                Task.WaitAll(tasks.ToArray());

                IsAddPO = Visibility.Visible;
                IsEditPO = Visibility.Hidden;
                SelectedIndexPoType = POTypeList.IndexOf(POTypeList.FirstOrDefault(i => i.IdPoType == 1));
                SelectedType = POTypeList.FirstOrDefault(i => i.IdPoType == 1).Type;
                //[Rahul.Gadhave][GEOS2-9326][Date:01-09-2025]
                if(SelectedofferInfo.Attachment==null)
                {
                    SelectedofferInfo.Attachment = new Emailattachment();
                }
                if (SelectedofferInfo.Attachment.SelectedIndexAttachementType != 1)
                {
                    var itemToRemove = POTypeList.FirstOrDefault(x => x.IdPoType == 1);
                    if (itemToRemove != null)
                    {
                        POTypeList.Remove(itemToRemove);
                    }
                    SelectedType = POTypeList.FirstOrDefault(i => i.IdPoType == 2).Type;
                    SelectedIndexPoType = POTypeList.IndexOf(POTypeList.FirstOrDefault(i => i.IdPoType == 2));
                    PoAbbreviation = POTypeList.FirstOrDefault(i => i.IdPoType == 2).Abbreviation;
                }
                else
                {
                    if (SelectedofferInfo.Attachment.SelectedIndexAttachementType == 1)
                    {
                        PoAbbreviation = POTypeList.FirstOrDefault(i => i.IdPoType == 1).Abbreviation;
                    }
                }
                if (SelectedofferInfo.IdStatus != 0 && SelectedofferInfo.IdOffer != 0)
                {
                    IdStatus = SelectedofferInfo.IdStatus;
                    IdOffer = SelectedofferInfo.IdOffer;
                    OfferCode = SelectedofferInfo.Code;
                    OfferCodeForLinkedPO = SelectedofferInfo.Code;
                    OfferCodeForPOType = SelectedofferInfo.Code;

                }
                Code = PoAbbreviation + "_" + SelectedofferInfo.Code;
                OfferCodeForPOType = SelectedofferInfo.Code;
                SelectedIndexCustomerGroup = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == SelectedofferInfo.IdCustomer));
                SelectedCustomerGroupName = Customers.FirstOrDefault(i => i.IdCustomer == SelectedofferInfo.IdCustomer).CustomerName;
                //ReceptionDate = poregistereddetails.ReceptionDate;
                SelectedIndexCompanyPlant = CustomerPlants.IndexOf(CustomerPlants.FirstOrDefault(i => i.IdCustomerPlant == SelectedofferInfo.IdSite));
                //POSenderByGroup = POSender;
                // [Rahul.gadhave][GEOS2-9878][Date:19 - 11 - 2025]
                if (POSenderByGroup!=null)
                {
                    SelectedIndexSender = POSenderByGroup.IndexOf(POSenderByGroup.FirstOrDefault(i => i.FullName == PODetails.Sender));
                }
                if (PODetails.TransferAmount != null)
                {
                    Amount = (double)PODetails.TransferAmount;
                }
                SelectedIndexCurrency = Currencies.IndexOf(Currencies.FirstOrDefault(i => i.Name == PODetails.Currency));
                if (SelectedIndexCurrency == -1)
                {
                    SelectedIndexCurrency = Currencies.IndexOf(Currencies.FirstOrDefault(i => i.Name == SelectedofferInfo.Currency));
                }
                SelectedIndexShipTo = -1;
                if (PODetails.PONumber != null)
                {
                    Code = PODetails.PONumber;
                }
                IsreceptionDateVisisble = true;
                if (PODetails.POdate != null)
                {
                    ReceptionDate = PODetails.POdate;
                }
                if (ReceptionDate == DateTime.MinValue)
                {
                    ReceptionDateNew = null;
                }
                else
                {
                    ReceptionDateNew = ReceptionDate;
                }

                CreationDate = null;
                if (PODetails.OfferInfo.Attachment.FileDocInBytes != null)
                {
                    SelectedofferInfo.CommericalAttachementsDocInBytes = PODetails.OfferInfo.Attachment.FileDocInBytes;
                }
                RegisterPOAttachmentSavedFileName = PODetails.OfferInfo.Attachment.AttachmentName;
                ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(RegisterPOAttachmentSavedFileName, true);
                AttachmentImage = objImage;
                LinkedOffersDetails = new ObservableCollection<LinkedOffers> { SelectedofferInfo };

                //[Rahul.Gadhave][Date:28-11-2025]
                //if(SelectedIndexPoType==0)
                //{
                //    SelectedofferInfo.CommericalAttachementsDocInBytes=
                //}





                if (poregistereddetails == null)
                {
                    poregistereddetails = new PORegisteredDetails();
                    //poregistereddetails.OffersLinked = new ObservableCollection<LinkedOffers>();
                }
                poregistereddetails.OffersLinked = LinkedOffersDetails.Select(x => (LinkedOffers)x.Clone()).ToObservableCollection();//pramod.misal
                foreach (var linkedOffer in LinkedOffersDetails)
                {
                    string customerName = linkedOffer.CustomerGroup;
                    linkedOffer.AttachmentFileName = PODetails.OfferInfo.Attachment.AttachmentName;
                    if (!string.IsNullOrEmpty(customerName))
                    {
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customerName);

                        if (bytes != null)
                        {
                            // Assuming linkedOffer has a property to hold image data (e.g., LinkedOfferImage).
                            linkedOffer.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            // Set a default image based on the theme.
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/wAccount.png");
                            }
                            else
                            {
                                linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/blueAccount.png");
                            }
                        }
                    }
                }
                IsNewPo = true;
                IsEmailBtnVisibile = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log("Method InitNewPO()...Executed", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method InitNewPO()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }        
        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        public void EditInIt(LinkedOffers LinkedPO)
        {
            try
            {
                if (LinkedPO.MailFromEditPoScreen == false)
                {
                    GeosApplication.Instance.Logger.Log("Method EditInIt() ...", category: Category.Info, priority: Priority.Low);

                    IsEditPO = Visibility.Visible;
                    IsAddPO = Visibility.Hidden;
                    int CurrentCurrencyId = 0;
                    var selectedCurrency = GeosApplication.Instance.Currencies.SingleOrDefault(cur => cur.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString());
                    if (selectedCurrency != null)
                    {
                        CurrentCurrencyId = selectedCurrency.IdCurrency;
                    }
                    //FillServiceUrl();
                    FillServiceUrlNew();

                    //OTMService = new OTMServiceController("localhost:6699");
                    IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                    poregistereddetails = OTMServiceThread.GetPORegisteredDetailsByIdPo(LinkedPO.IdCustomerPurchaseOrder, CurrentCurrencyId);
                    poregistereddetails.IdOfferCustomerGroup = LinkedPO.IdOfferCustomerGroup;
                    if (poregistereddetails.IdPO != 0)
                    {
                        EditInIt(poregistereddetails);
                        OfferCodeForPOType = LinkedPO.OfferCode;
                        if (LinkedOffersDetails != null)
                        {
                            foreach (LinkedOffers linked in LinkedOffersDetails)
                            {
                                if (LinkedPO.Attachment != null)
                                {
                                    linked.Attachment = LinkedPO.Attachment;
                                    linked.AttachmentFileName = LinkedPO.Attachment.AttachmentName;
                                    linked.CommericalAttachementsDocInBytes = LinkedPO.Attachment.FileDocInBytes;

                                    RegisterPOAttachmentSavedFileName = LinkedPO.Attachment.AttachmentName;
                                    ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(LinkedPO.Attachment.AttachmentName, true);
                                    AttachmentImage = objImage;
                                }
                            }
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method EditInIt()...Executed", category: Category.Info, priority: Priority.Low);
                }
                if (LinkedPO.MailFromEditPoScreen == true)
                {
                    GeosApplication.Instance.Logger.Log("Method EditInIt() ...", category: Category.Info, priority: Priority.Low);
                    try
                    {
                        IsEditPO = Visibility.Hidden;
                        IsAddPO = Visibility.Hidden;
                        int CurrentCurrencyId = 0;
                        var selectedCurrency = GeosApplication.Instance.Currencies.SingleOrDefault(cur => cur.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString());
                        if (selectedCurrency != null)
                        {
                            CurrentCurrencyId = selectedCurrency.IdCurrency;
                        }
                        //FillServiceUrl();
                        FillServiceUrlNew();

                        //OTMService = new OTMServiceController("localhost:6699");
                        IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                        poregistereddetails = OTMServiceThread.GetPORegisteredDetailsByIdPo(LinkedPO.IdCustomerPurchaseOrder, CurrentCurrencyId);
                        poregistereddetails.IdOfferCustomerGroup = LinkedPO.IdOfferCustomerGroup;
                        if (poregistereddetails.IdPO != 0)
                        {
                            EditInIt(poregistereddetails);
                            OfferCodeForPOType = LinkedPO.OfferCode;

                        }
                        GeosApplication.Instance.Logger.Log("Method SendEmailToCustomerButtonCommandCommandAction()....", category: Category.Info, priority: Priority.Low);
                        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        {
                            DXSplashScreen.Show(x =>
                            {
                                Window win = new Window
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
                            }, x => new SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
                        }
                        //For Mail
                        POEmailConfirmationViewModel pOEmailConfirmationViewModel = new POEmailConfirmationViewModel();
                        POEmailConfirmationView pOEmailConfirmationView = new POEmailConfirmationView();
                        pOEmailConfirmationViewModel.InIt(poregistereddetailsforemail);
                        EventHandler handle = delegate { pOEmailConfirmationView.Close(); };
                        pOEmailConfirmationViewModel.RequestClose += handle;
                        pOEmailConfirmationView.DataContext = pOEmailConfirmationViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        pOEmailConfirmationView.ShowDialogWindow();
                        GeosApplication.Instance.Logger.Log("Method SendEmailToCustomerButtonCommandCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SendEmailToCustomerButtonCommandCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in EditInIt(): {ex.Message}", category: Category.Exception, priority: Priority.High);
            }
        }
        //[pramod.misal][GEOS2-5077][17-01-2024]
        private void SendEmailToCustomerButtonCommandCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendEmailToCustomerButtonCommandCommandAction()....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window
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
                    }, x => new SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
                }
                POEmailConfirmationViewModel pOEmailConfirmationViewModel = new POEmailConfirmationViewModel();
                POEmailConfirmationView pOEmailConfirmationView = new POEmailConfirmationView();
                pOEmailConfirmationViewModel.InIt(poregistereddetailsforemail);
                EventHandler handle = delegate { pOEmailConfirmationView.Close(); };
                pOEmailConfirmationViewModel.RequestClose += handle;
                pOEmailConfirmationView.DataContext = pOEmailConfirmationViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                pOEmailConfirmationView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method SendEmailToCustomerButtonCommandCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendEmailToCustomerButtonCommandCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendEmailToCustomerButtonCommandCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SendEmailToCustomerButtonCommandCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //private void FillAllList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillAllList ...", category: Category.Info, priority: Priority.Low);


        //        GeosApplication.Instance.Logger.Log("Method FillAllList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillAllList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillAllList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

        //    }
        //}
        public void FillLinkedOfferList()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillLinkedOfferList ...", category: Category.Info, priority: Priority.Low);
                //FillServiceUrl();

                //OTMService = new OTMServiceController("localhost:6699");
                //LinkedOffersDetails = new ObservableCollection<LinkedOffers>(OTMService.GetOTM_GetLinkedOffers_V2590(poregistereddetails));
                //[Rahul.Gadhave][GEOS-7040][Date:28-02-2025][https://helpdesk.emdep.com/browse/GEOS2-7040]
                //LinkedOffersDetails = new ObservableCollection<LinkedOffers>(OTMService.GetOTM_GetLinkedOffers_V2630(poregistereddetails));
                //FillServiceUrlNew();

                // OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                LinkedOffersDetails = new ObservableCollection<LinkedOffers>(OTMServiceThread.GetOTM_GetLinkedOffers_V2660(poregistereddetails));
                poregistereddetails.OffersLinked = LinkedOffersDetails.Select(x => (LinkedOffers)x.Clone()).ToObservableCollection();                
                GeosApplication.Instance.Logger.Log("Method FillLinkedOfferList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedOfferList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedOfferList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }

        }
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }
        private void FillPOTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPOTypeList ...", category: Category.Info, priority: Priority.Low);
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                POTypeList = new ObservableCollection<POType>(OTMServiceThread.OTM_GetAllPOTypeStatus_V2670().OrderBy(PT => PT.Type));
                GeosApplication.Instance.Logger.Log("Method FillPOTypeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPOTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPOTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        private void FillEmployeeCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeCode() started...", category: Category.Info, priority: Priority.Low);
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                GeosApplication.Instance.EmployeeCode = OTMServiceThread.GetEmployeeCodeByUserID(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.Logger.Log("Method FillEmployeeCode() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeCode() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeCode() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        private void FillCurrenciesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrenciesList ...", category: Category.Info, priority: Priority.Low);
                if (Currencies == null || Currencies?.Count == 0)
                {
                    IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                    Currencies = new ObservableCollection<Currency>(OTMServiceThread.GetAllPOCurrencies_V2590().OrderBy(c => c.Name));
                    if (Currencies != null)
                    {
                        foreach (var bpItem in Currencies.GroupBy(tpa => tpa.Name))
                        {
                            ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                            bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillCurrenciesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrenciesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        private void FillEntireCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEntireCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
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
        private void FillCustomerGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerGroupList ...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                //[pooja.jadhav][GEOS2-7052][11-04-2025]
                //[pooja.jadhav][GEOS2-9179][13-08-2025]
                ObservableCollection<Customer> CustomersList;
                if (poregistereddetails.IdOfferCustomerGroup!=0 && poregistereddetails.IdCustomerPlant==10)
                {
                    CustomersList = new ObservableCollection<Customer>(OTMService.GetAllCustomers_V2660(Convert.ToInt32(poregistereddetails.IdOfferCustomerGroup)));
                }
                else
                {
                    CustomersList = new ObservableCollection<Customer>(OTMService.GetAllCustomers_V2660(poregistereddetails.IdCustomerPlant));
                }
                

                // Check if the customer already exists
                var existingCustomer = CustomersList.FirstOrDefault(c => c.IdCustomer == poregistereddetails.IdCustomerPlant);
                if (existingCustomer == null)
                {
                    CustomersList.Add(new Customer
                    {
                        IdCustomer = poregistereddetails.IdCustomerPlant,
                        CustomerName = poregistereddetails.Group,
                        IsEnabled = false
                    });
                }
                else
                {
                    existingCustomer.IsEnabled = false;
                }

                // Sort once and assign to ObservableCollection
                Customers = new ObservableCollection<Customer>(CustomersList.OrderBy(c => c.CustomerName));
                GeosApplication.Instance.Logger.Log("Method FillCustomerGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPlantsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        public string CleanAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return address;
            address = System.Text.RegularExpressions.Regex.Replace(address, @"\s+", " ");
            address = address.Trim();

            return address;
        }
        private void FillShippingAddressList()
        {
            try
            {
				//[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
                Shipindex = 0;
                GeosApplication.Instance.Logger.Log("Method FillShippingAddressList ...", category: Category.Info, priority: Priority.Low);
                ShippingAddressList = new ObservableCollection<ShippingAddress>();
                //OTMService = new OTMServiceController("localhost:6699");
                //ShippingAddressList = new ObservableCollection<ShippingAddress>(OTMService.OTM_GetShippingAddress_V2590(customers[SelectedIndexCustomerGroup].IdCustomer));
                //[Rahul.Gadhave][GEOS2-7056][Date:07-03-2025]
                //[pooja.jadhav][GEOS2-7057][12-03-2025]
                int index = (SelectedIndexCompanyPlant == -1) ? 0 : SelectedIndexCompanyPlant;
                //[Rahul.Gadhave][GEOS2 - 7226][Date: 24 - 03 - 2025]
                if (IsShowAll == true)
                {
                    // ShippingAddressList = new ObservableCollection<ShippingAddress>(OTMService.OTM_GetShippingAddressForShowAll_V2630(customers[SelectedIndexCustomerGroup].IdCustomer).OrderBy(sa => sa.Address).ThenBy(sa => sa.City).ThenBy(sa => sa.CountriesName));
                    var customerId = customers[SelectedIndexCustomerGroup].IdCustomer;
                    // Get addresses for selected customer
                    var addresses = OTMService.OTM_GetShippingAddressForShowAll_V2680(customerId)
                                              .OrderBy(sa => sa.Address)
                                              .ThenBy(sa => sa.City)
                                              .ThenBy(sa => sa.CountriesName)
                                              .ToList();
					//[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
                    Customer selectedCustomer = customers[SelectedIndexCustomerGroup];
                    CustomerPlant selectedplant = CustomerPlants[SelectedIndexCompanyPlant];
                    PlantCity = selectedplant.City.Trim();

                    if (customerId != 10)
                    {
                        var additionalAddresses = OTMService.OTM_GetShippingAddressForShowAll_V2680(10)
                                                            .OrderBy(sa => sa.Address)
                                                            .ThenBy(sa => sa.City)
                                                            .ThenBy(sa => sa.CountriesName);
                        addresses.AddRange(additionalAddresses);
                    }
                    ShippingAddressList = new ObservableCollection<ShippingAddress>(addresses);
					//[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
                    Shipindex = ShippingAddressList
                       .ToList()
                       .FindIndex(sa =>
                           sa.Address != null &&
                           sa.Address.Trim().IndexOf(PlantCity, StringComparison.OrdinalIgnoreCase) >= 0
                       );

                    if (shipindex >= 0)
                    {
                        SelectedIndexShipTo = shipindex;   
                    }
                    else
                    {
                        SelectedIndexShipTo = -1;          
                    }
                }
                else
                {
                    int idCustomerPlant = 0;

                    if (SelectedIndexCompanyPlant >= 0 && SelectedIndexCompanyPlant < CustomerPlants.Count)
                    {
                        idCustomerPlant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
                    }
                    //OTMService = new OTMServiceController("localhost:6699");
                    var addresses = OTMService.OTM_GetShippingAddress_V2680(idCustomerPlant)
                                              .OrderBy(sa => sa.Address)
                                              .ThenBy(sa => sa.City)
                                              .ThenBy(sa => sa.CountriesName)
                                              .ToList();

                    // If customer ID is not 10, also get addresses for ID 10
                    var selectedCustomerId = customers[SelectedIndexCustomerGroup].IdCustomer;
					//[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
                    Customer selectedCustomer = customers[SelectedIndexCustomerGroup];
                    CustomerPlant selectedplant = CustomerPlants[SelectedIndexCompanyPlant];
                    PlantCity = selectedplant.City.Trim();
                    if (selectedCustomerId != 10)
                    {
                        var additionalAddresses = OTMService.OTM_GetShippingAddressForShowAll_V2680(10)
                                               .OrderBy(sa => sa.Address)
                                              .ThenBy(sa => sa.City)
                                              .ThenBy(sa => sa.CountriesName)
                                              .ToList();
                        addresses.AddRange(additionalAddresses);
                    }

                    ShippingAddressList = new ObservableCollection<ShippingAddress>(addresses);

                    



                }

                //else
                //{
                //    int idCustomerPlant = 0;

                //    if (SelectedIndexCompanyPlant >= 0 && SelectedIndexCompanyPlant < CustomerPlants.Count)
                //    {
                //        idCustomerPlant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
                //    }
                //    ShippingAddressList = new ObservableCollection<ShippingAddress>(OTMService.OTM_GetShippingAddress_V2620(idCustomerPlant).OrderBy(sa => sa.Address).ThenBy(sa => sa.City).ThenBy(sa => sa.CountriesName));
                //}

                GeosApplication.Instance.Logger.Log("Method FillShippingAddressList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddressList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddressList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        //[nsatpute][14.08.2025][GEOS2-9177]
        private void ReorderShippingAddressList(string countryName)
        {
            #region oldcode
            var selectedCustomerId = customers[SelectedIndexCustomerGroup].IdCustomer;
            if (selectedCustomerId == 10)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method ReorderShippingAddressList ...", category: Category.Info, priority: Priority.Low);
                    if (string.IsNullOrWhiteSpace(countryName) || ShippingAddressList == null)
                        return;

                    var reordered = new List<ShippingAddress>();
                    string regionName = string.Empty;

                    // Normalize filter values
                    string countryFilter = countryName.Trim();

                    // Stage 1: Add all addresses from the given country
                    foreach (var add in ShippingAddressList)
                    {
                        if (string.Equals(add.CountriesName?.Trim(), countryFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            reordered.Add(add);
                            if (string.IsNullOrEmpty(regionName) && !string.IsNullOrWhiteSpace(add.Region))
                                regionName = add.Region.Trim();
                        }
                    }

                    // Stage 2: Add other addresses from the same region (excluding already added)
                    if (!string.IsNullOrEmpty(regionName))
                    {
                        var sameRegion = ShippingAddressList
                            .Where(add => !reordered.Contains(add) &&
                                          string.Equals(add.Region?.Trim(), regionName, StringComparison.OrdinalIgnoreCase))
                            .OrderBy(add => add.CountriesName, StringComparer.OrdinalIgnoreCase); // sort by country name inside region

                        reordered.AddRange(sameRegion);
                    }

                    // Stage 3: Add the remaining addresses sorted by Region then CountriesName
                    reordered.AddRange(
                        ShippingAddressList
                            .Except(reordered)
                            .OrderBy(x => x.Region, StringComparer.OrdinalIgnoreCase)
                            .ThenBy(x => x.CountriesName, StringComparer.OrdinalIgnoreCase)
                    );

                    // Replace list (preserve UI binding)
                    ShippingAddressList = new ObservableCollection<ShippingAddress>(reordered);

                    if (poregistereddetails != null)
                        SelectedIndexShipTo = ShippingAddressList.IndexOf(ShippingAddressList.FirstOrDefault(i => i.IdShippingAddress == poregistereddetails.IdShippingAddress));

                    GeosApplication.Instance.Logger.Log("Method ReorderShippingAddressList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ReorderShippingAddressList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
            #endregion old code
            else
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method ReorderShippingAddressList ...", category: Category.Info, priority: Priority.Low);

                    if (string.IsNullOrWhiteSpace(countryName) || ShippingAddressList == null)
                        return;

                    string regionName = string.Empty;
                    string countryFilter = countryName.Trim();

                    // Separate Emdep addresses (IdCustomerPlant == 10)
                    var emdepAddresses = ShippingAddressList
                        .Where(add => add.IdCustomerPlant == 10);
                    //.OrderBy(add => add.Address)
                    //.ThenBy(add => add.City)
                    //.ThenBy(add => add.CountriesName)
                    //.ToList();

                    // Remaining (non-Emdep) addresses
                    var nonEmdepAddresses = ShippingAddressList
                        .Where(add => add.IdCustomerPlant != 10)
                        .ToList();

                    var reordered = new List<ShippingAddress>();

                    // Stage 1: Add addresses from the given country
                    foreach (var add in nonEmdepAddresses)
                    {
                        if (string.Equals(add.CountriesName?.Trim(), countryFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            reordered.Add(add);
                            if (string.IsNullOrEmpty(regionName) && !string.IsNullOrWhiteSpace(add.Region))
                                regionName = add.Region.Trim();
                        }
                    }

                    // Stage 2: Add addresses from the same region (excluding already added)
                    if (!string.IsNullOrEmpty(regionName))
                    {
                        var sameRegion = nonEmdepAddresses
                            .Where(add => !reordered.Contains(add) &&
                                          string.Equals(add.Region?.Trim(), regionName, StringComparison.OrdinalIgnoreCase))
                            .OrderBy(add => add.CountriesName, StringComparer.OrdinalIgnoreCase);

                        reordered.AddRange(sameRegion);
                    }

                    // Stage 3: Add remaining non-Emdep addresses (sorted by Region then Country)
                    var remaining = nonEmdepAddresses
                        .Where(add => !reordered.Contains(add))
                        .OrderBy(x => x.Region, StringComparer.OrdinalIgnoreCase)
                        .ThenBy(x => x.CountriesName, StringComparer.OrdinalIgnoreCase);

                    reordered.AddRange(remaining);

                    // Stage 4: Finally, add Emdep addresses at the end
                    reordered.AddRange(emdepAddresses);

                    // Replace list (preserve UI binding)
                    ShippingAddressList = new ObservableCollection<ShippingAddress>(reordered);

                    // Restore selected index if valid
                    //if (poregistereddetails != null)
                    //{
                    //    SelectedIndexShipTo = ShippingAddressList.IndexOf(
                    //        ShippingAddressList.FirstOrDefault(i => i.IdShippingAddress == poregistereddetails.IdShippingAddress)
                    //    );
                    //}
					//[pramod.misal][20.11.2025][GEOS2-9892]https://helpdesk.emdep.com/browse/GEOS2-9892
                    Shipindex = ShippingAddressList
                       .ToList()
                       .FindIndex(sa =>
                           sa.Address != null &&
                           sa.Address.Trim().IndexOf(PlantCity, StringComparison.OrdinalIgnoreCase) >= 0
                       );

                    if (shipindex >= 0)
                    {
                        SelectedIndexShipTo = shipindex;   // ← assign index
                    }
                    else
                    {
                        SelectedIndexShipTo = -1;          // nothing found
                    }

                    GeosApplication.Instance.Logger.Log("Method ReorderShippingAddressList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in ReorderShippingAddressList(): " + ex.Message, category: Category.Exception, priority: Priority.High);
                }
            }
        }

        private void FillPOSenderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPOSenderList ...", category: Category.Info, priority: Priority.Low);
                //[pooja.jadhav][GEOS2-7054][10-03-2025]
                //FillServiceUrlNew();
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                ///OTMServiceThread = new OTMServiceController("localhost:6699");
                POSender = new ObservableCollection<PORegisteredDetails>(OTMServiceThread.OTM_GetPOSender_V2670().OrderBy(po => po.FirstName));
                GeosApplication.Instance.Logger.Log("Method FillPOSenderList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPOSenderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPOSenderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        //[pramod.misal][GEOS2-6460][28-11-2024]
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
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
            //[Rahul.Gadhave][GEOS2-9880][Date:28-11-2025]
            if (IsNewPo && IsPoEmailGoAhed)
            {
                // Use the actual saved path from the PdfResultDto
                string pdfPath = RefpdfResultDto?.FileFullPath;

                if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
                {
                    try
                    {
                        File.Delete(pdfPath);
                    }
                    catch (Exception ex)
                    {
                        // log or handle exception if delete fails
                        Debug.WriteLine($"Failed to delete file: {ex.Message}");
                    }
                }
            }
            IsNewPo = false;
            IsCancelPo = true;
            IsPoEmailGoAhed = false;

        }
        /// <summary>
        /// [001][ashish.malkhede][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// [002][ashish.malkhede][08-07-2025] https://helpdesk.emdep.com/browse/GEOS2-9105
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AcceptCommandAction()....", category: Category.Info, priority: Priority.Low);
            if (poregistereddetails != null && IsNewPo == false)
            {

                IsBusy = true;
                InformationError = null;


                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexPoType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCustomerGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("Code"));
                PropertyChanged(this, new PropertyChangedEventArgs("Remarks"));
                PropertyChanged(this, new PropertyChangedEventArgs("ReceptionDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ReceptionDateNew"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSender"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexShipTo"));
                PropertyChanged(this, new PropertyChangedEventArgs("Amount"));
                PropertyChanged(this, new PropertyChangedEventArgs("RegisterPOAttachmentSavedFileName"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCurrency"));
                //PropertyChanged(this, new PropertyChangedEventArgs("RegisterPOAttachmentSavedFileName"));//[pramod.misal][Date:11-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9173]
                if (error != null)
                {
                    IsBusy = false;
                    return;
                }

                // type
                poregistereddetails.IdPOType = POTypeList[SelectedIndexPoType].IdPoType;
                poregistereddetails.Type = POTypeList[SelectedIndexPoType].Type;
                // Customer group
                poregistereddetails.IdCustomerPlant = customers[SelectedIndexCustomerGroup].IdCustomer;
                poregistereddetails.Group = customers[SelectedIndexCustomerGroup].CustomerName;
                // customer plant
                poregistereddetails.IdSite = customerPlant[SelectedIndexCompanyPlant].IdCustomerPlant;
                poregistereddetails.Plant = customerPlant[SelectedIndexCompanyPlant].CustomerPlantName;
                if (poregistereddetails.Plant != null)
                {
                    int index = poregistereddetails.Plant.IndexOf('(');
                    if (index != -1)
                    {
                        poregistereddetails.Plant = poregistereddetails.Plant.Substring(0, index).Trim();
                    }
                }
                // Number(Code)
                poregistereddetails.Code = Code;
                // Reception Date
                poregistereddetails.ReceptionDate = ReceptionDate;
                poregistereddetails.ReceptionDateNew = ReceptionDateNew;

                // Sender
                //poregistereddetails.IdPerson = POSender[SelectedIndexSender].IdPerson;
                if (SelectedIndexSender != -1)
                {
                    poregistereddetails.IdSender = POSenderByGroup[SelectedIndexSender].IdPerson;
                    poregistereddetails.Sender = POSenderByGroup[SelectedIndexSender].FullName;
                }
                // Ship To
                poregistereddetails.IdShippingAddress = shippingAddressList[SelectedIndexShipTo].IdShippingAddress;
                poregistereddetails.ShippingAddress = shippingAddressList[SelectedIndexShipTo].FullAddress;
                // Amount
                poregistereddetails.POValue = Amount;
                /// [001] [Rahul.Gadhave][19-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
                poregistereddetails.Amount = ConvertedAmount;
                // Currency
                poregistereddetails.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                poregistereddetails.Currency = Currencies[SelectedIndexCurrency].Name;
                poregistereddetails.CurrencyIconBytes = Currencies[SelectedIndexCurrency].CurrencyIconbytes;
                poregistereddetails.OffersLinked.ToList().ForEach(x => x.ActivityLinkedItemImage = null);
                //Attachment
                poregistereddetails.AttachmentFileName = RegisterPOAttachmentSavedFileName;
                //attachement in bytes
                byte[] CopyCommericalAttachementsDocInBytes = null;
                foreach (var offer in poregistereddetails.offersLinked)
                {
                    if (SelectedRegisterPoFile != null)
                    {
                        offer.CommericalAttachementsDocInBytes = SelectedRegisterPoFile.ConnectorAttachementsDocInBytes;
                        offer.AttachmentFileName = RegisterPOAttachmentSavedFileName;
                    }
                    else
                    {
                        if (offer.CommericalAttachementsDocInBytes == null)
                        {
                            offer.CommericalAttachementsDocInBytes = CopyCommericalAttachementsDocInBytes;
                            offer.AttachmentFileName = RegisterPOAttachmentSavedFileName;
                        }
                        else
                            CopyCommericalAttachementsDocInBytes = offer.CommericalAttachementsDocInBytes;
                    }
                }
                if (LinkedOffersDetails.Count > 0)
                {
                    string joinOffer = string.Join(";", LinkedOffersDetails.Select(j => j.Code));
                    poregistereddetails.LinkedOffer = joinOffer.ToString();
                }


                // Remarks
                if (Remarks != "" && Remarks != null)
                {
                    string cleanedRemarks = System.Text.RegularExpressions.Regex.Replace(Remarks, @"^(\r\n)+|(\r\n)+$", "");

                    if (poregistereddetails.Remarks != cleanedRemarks)
                    {
                        poregistereddetails.Remarks = cleanedRemarks;
                        if (poregistereddetails.offersLinked != null)
                        {
                            poregistereddetails.offersLinked.ToList().ForEach(r => r.Remark = cleanedRemarks);
                            poregistereddetails.offersLinked.ToList().ForEach(r => r.IsUpdate = true);
                        }
                    }
                }
                else
                {
                    poregistereddetails.Remarks = "";
                    if (poregistereddetails.offersLinked != null)
                    {
                        poregistereddetails.offersLinked.ToList().ForEach(r => r.Remark = "");
                        poregistereddetails.offersLinked.ToList().ForEach(r => r.IsUpdate = true);
                    }
                }
                // Not Ok
                if (IsOkKey)
                {
                    poregistereddetails.IsOK = "PO Not OK";
                }
                else
                {
                    poregistereddetails.IsOK = "PO OK";
                }
                if (IscancelledKey)
                {
                    poregistereddetails.IsCancelled = "PO Cancelled";
                    poregistereddetails.Canceler = GeosApplication.Instance.ActiveUser.FullName;
                    poregistereddetails.CancellationDate = DateTime.Now;
                    poregistereddetails.CancelerCode = GeosApplication.Instance.EmployeeCode;
                }
                else
                {
                    poregistereddetails.IsCancelled = "PO Not Cancelled";
                    poregistereddetails.Canceler = null;
                    poregistereddetails.CancellationDate = null;
                    poregistereddetails.CancelerCode = null;
                }

                poregistereddetails.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                poregistereddetails.Updater = GeosApplication.Instance.ActiveUser.FullName;
                poregistereddetails.UpdaterDate = DateTime.Now;
                poregistereddetails.UpdaterCode = GeosApplication.Instance.EmployeeCode;
                AddChangeLogByPO(initialPORegisteredDetails);
                if (tempPOChangeLog != null)
                {
                    poregistereddetails.LogEntriesByPO = tempPOChangeLog.ToList();
                }
                try
                {
                    //FillServiceUrl();
                    FillServiceUrlNew();

                    //OTMService = new OTMServiceController("localhost:6699");
                    IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                    //OTMServiceThread = new OTMServiceController("localhost:6699");
                    //IsSave = OTMService.UpdatePurchaseOrder_V2590(poregistereddetails);
                    IsSave = OTMServiceThread.UpdatePurchaseOrder_V2660(poregistereddetails);

                        if (IsSave == true)
                        {
                            if (initialPORegisteredDetails.POValue != poregistereddetails.POValue)
                            {
                                try
                                {
                                    GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                                    int CurrentCurrencyId = 0;
                                    var selectedCurrency = GeosApplication.Instance.Currencies
                                       .SingleOrDefault(cur => cur.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString());
                                    if (selectedCurrency != null)
                                    {
                                        CurrentCurrencyId = selectedCurrency.IdCurrency;
                                    }
                                    int idCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                                    if (CurrentCurrencyId == Currencies[SelectedIndexCurrency].IdCurrency)
                                    {
                                        ConvertedAmount = OTMService.GetOfferAmountByCurrencyConversion_V2590(idCurrency, CurrentCurrencyId, IdPO);
                                        poregistereddetails.Amount = ConvertedAmount;
                                    }
                                    else
                                    {
                                        ConvertedAmount = OTMService.GetOfferAmountByCurrencyConversion_V2590(idCurrency, CurrentCurrencyId, IdPO);
                                        poregistereddetails.Amount = ConvertedAmount;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    IsBusy = false;
                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                    GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditPOUpdateSuccessMsg").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                        GeosApplication.Instance.Logger.Log("Method AcceptCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
            if (IsNewPo == true)
            {
                poregistereddetails = new PORegisteredDetails();
                IsBusy = true;
                InformationError = null;


                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexPoType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCustomerGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("Code"));
                PropertyChanged(this, new PropertyChangedEventArgs("Remarks"));
                PropertyChanged(this, new PropertyChangedEventArgs("ReceptionDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ReceptionDateNew"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSender"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexShipTo"));
                PropertyChanged(this, new PropertyChangedEventArgs("Amount"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCurrency"));
                PropertyChanged(this, new PropertyChangedEventArgs("RegisterPOAttachmentSavedFileName"));
                //if (IsPOGoAhead)//[pramod.misal][Date:11-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9173]
                //    PropertyChanged(this, new PropertyChangedEventArgs("RegisterPOAttachmentSavedFileName"));
                if (error != null)
                {
                    IsBusy = false;
                    return;
                }

                // type
                poregistereddetails.IdPOType = POTypeList[SelectedIndexPoType].IdPoType;
                poregistereddetails.Type = POTypeList[SelectedIndexPoType].Type;
                // Customer group
                poregistereddetails.IdCustomerPlant = customers[SelectedIndexCustomerGroup].IdCustomer;
                poregistereddetails.Group = customers[SelectedIndexCustomerGroup].CustomerName;
                // customer plant
                poregistereddetails.IdSite = customerPlant[SelectedIndexCompanyPlant].IdCustomerPlant;
                poregistereddetails.Plant = customerPlant[SelectedIndexCompanyPlant].CustomerPlantName;
                if (poregistereddetails.Plant != null)
                {
                    int index = poregistereddetails.Plant.IndexOf('(');
                    if (index != -1)
                    {
                        poregistereddetails.Plant = poregistereddetails.Plant.Substring(0, index).Trim();
                    }
                }
                // Number(Code)
                poregistereddetails.Code = Code;
                // Reception Date
                //poregistereddetails.ReceptionDate = ReceptionDate;
                poregistereddetails.ReceptionDateNew = ReceptionDateNew;


                // Sender
                //poregistereddetails.IdPerson = POSender[SelectedIndexSender].IdPerson;
                if (SelectedIndexSender != -1)
                {
                    poregistereddetails.IdSender = POSenderByGroup[SelectedIndexSender].IdPerson;
                    poregistereddetails.Sender = POSenderByGroup[SelectedIndexSender].FullName;
                }
                // Ship To
                poregistereddetails.IdShippingAddress = shippingAddressList[SelectedIndexShipTo].IdShippingAddress;
                poregistereddetails.ShippingAddress = shippingAddressList[SelectedIndexShipTo].FullAddress;
                // Amount
                poregistereddetails.POValue = Amount;
                /// [001] [Rahul.Gadhave][19-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
                poregistereddetails.Amount = ConvertedAmount;
                // Currency
                poregistereddetails.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                poregistereddetails.Currency = Currencies[SelectedIndexCurrency].Name;
                poregistereddetails.CurrencyIconBytes = Currencies[SelectedIndexCurrency].CurrencyIconbytes;
                foreach (LinkedOffers l in LinkedOffersDetails)
                {
                    if (l.Attachment != null)
                    {
                        l.Attachment.AttachmentImage = null;

                    }
                    l.PoTypePOAttachementsList = null;
                }
                poregistereddetails.OffersLinked = LinkedOffersDetails;
                poregistereddetails.OffersLinked.ToList().ForEach(x => x.ActivityLinkedItemImage = null);
                poregistereddetails.OffersLinked.ToList().ForEach(x => x.CurrencyIconBytes = null);
                //Attachment
                poregistereddetails.AttachmentFileName = RegisterPOAttachmentSavedFileName;
                //attachement in bytes
                byte[] CopyCommericalAttachementsDocInBytes = null;
                foreach (var offer in poregistereddetails.offersLinked)
                {
                    if (SelectedRegisterPoFile != null)
                    {
                        offer.CommericalAttachementsDocInBytes = SelectedRegisterPoFile.ConnectorAttachementsDocInBytes;
                        offer.AttachmentFileName = RegisterPOAttachmentSavedFileName;
                    }
                    else
                    {
                        if (offer.CommericalAttachementsDocInBytes == null)
                        {
                            offer.CommericalAttachementsDocInBytes = CopyCommericalAttachementsDocInBytes;
                            offer.AttachmentFileName = RegisterPOAttachmentSavedFileName;
                        }
                        else
                            CopyCommericalAttachementsDocInBytes = offer.CommericalAttachementsDocInBytes;
                    }
                }
                if (LinkedOffersDetails.Count > 0)
                {
                    string joinOffer = string.Join(";", LinkedOffersDetails.Select(j => j.Code));
                    poregistereddetails.LinkedOffer = joinOffer.ToString();
                }

                // Remarks
                if (Remarks != "" && Remarks != null)
                {

                    string cleanedRemarks = System.Text.RegularExpressions.Regex.Replace(Remarks, @"^(\r\n)+|(\r\n)+$", "");

                    if (poregistereddetails.Remarks != cleanedRemarks)
                    {
                        poregistereddetails.Remarks = cleanedRemarks;
                        if (poregistereddetails.offersLinked != null)
                        {
                            poregistereddetails.offersLinked.ToList().ForEach(r => r.Remark = cleanedRemarks);
                            poregistereddetails.offersLinked.ToList().ForEach(r => r.IsUpdate = true);
                        }
                    }
                }
                // Not Ok
                if (IsOkKey)
                {
                    poregistereddetails.IsOK = "PO Not OK";
                }
                else
                {
                    poregistereddetails.IsOK = "PO OK";
                }
                if (IscancelledKey)
                {
                    poregistereddetails.IsCancelled = "PO Cancelled";
                    poregistereddetails.Canceler = GeosApplication.Instance.ActiveUser.FullName;
                    poregistereddetails.CancellationDate = DateTime.Now;
                    poregistereddetails.CancelerCode = GeosApplication.Instance.EmployeeCode;
                }
                else
                {
                    poregistereddetails.IsCancelled = "PO Not Cancelled";
                    poregistereddetails.Canceler = null;
                    poregistereddetails.CancellationDate = null;
                    poregistereddetails.CancelerCode = null;
                }

                poregistereddetails.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                poregistereddetails.Creator = GeosApplication.Instance.ActiveUser.FullName;
                poregistereddetails.CreationDate = DateTime.Now;
                poregistereddetails.CreatorCode = GeosApplication.Instance.EmployeeCode;
                AddChangeLogByNewPO();
                //AddChangeLogByPO(initialPORegisteredDetails);
                if (tempPOChangeLog != null)
                {
                    poregistereddetails.LogEntriesByPO = tempPOChangeLog.ToList();
                }
                try
                {
                    //FillServiceUrl();
                    FillServiceUrlNew();

                    // OTMService = new OTMServiceController("localhost:6699");
                    IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                    poregistereddetails.CurrencyIconBytes = null;
                    //OTMService = new OTMServiceController("localhost:6699");
                    //[pramod.misal][GEOS2-9109][1-08-2025]
                    if (IdStatus == 1 || IdStatus == 2)
                    {
                        poregistereddetails.IdStatus = 3;
                        poregistereddetails.IdOffer = IdOffer;
                        poregistereddetails.OfferCode = OfferCode;
                    }
                    //OTMServiceThread = new OTMServiceController("localhost:6699");
                    //[Rahul.Gadhave][GEOS2-9437][10 - 09 - 2025]
                    bool exists = OTMServiceThread.ChectCustomerpurchaseOrderExist_V2670(poregistereddetails);
                    if (exists)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PurchaseorderExist").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);                      
                        IsBusy = false;
                    }
                    else
                    {
                        //OTMServiceThread = new OTMServiceController("localhost:6699");
                        IsSaveIdPo = OTMServiceThread.InsertPurchaseOrder_V2660(poregistereddetails); //[002]
                        if (IsSaveIdPo > 0)
                        {
                            IsSave = true;
                            poregistereddetails.IdPO = IsSaveIdPo;
                            if (poregistereddetails.ReceptionDateNew != null)
                            {
                                poregistereddetails.ReceptionDate = poregistereddetails.ReceptionDateNew.Value;
                            }
                            else
                            {
                                poregistereddetails.ReceptionDate = DateTime.MinValue;
                            }
                            if (CustomerPlants != null &&
                                SelectedIndexCompanyPlant >= 0 &&
                                SelectedIndexCompanyPlant < CustomerPlants.Count &&
                                CustomerPlants[SelectedIndexCompanyPlant]?.Country != null)
                            {
                                poregistereddetails.Country = CustomerPlants[SelectedIndexCompanyPlant].Country;
                            }
                            else
                            {
                                poregistereddetails.Country = string.Empty;
                            }
                            //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("InsertPOUpdateSuccessMsg").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            CustomPromptResultOTMEmail promptResult = CustomMessageBox.ShowOTMMPrompt(string.Format(Application.Current.Resources["OTMEditPrompt"].ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.YesNo);
                            if (promptResult.Result == MessageBoxResult.Yes&& promptResult.ViewModel.IsDetailsChecked==true)
                            {
                                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                {
                                    DXSplashScreen.Show(x =>
                                    {
                                        Window win = new Window
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
                                    }, x => new SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
                                }
                                POEmailConfirmationViewModel pOEmailConfirmationViewModel = new POEmailConfirmationViewModel();
                                POEmailConfirmationView pOEmailConfirmationView = new POEmailConfirmationView();
                                pOEmailConfirmationViewModel.InIt(poregistereddetails);
                                EventHandler handle = delegate { pOEmailConfirmationView.Close(); };
                                pOEmailConfirmationViewModel.RequestClose += handle;
                                pOEmailConfirmationView.DataContext = pOEmailConfirmationViewModel;
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                pOEmailConfirmationView.ShowDialogWindow();
                                GeosApplication.Instance.Logger.Log("Method SendEmailToCustomerButtonCommandCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
                            }
                        }

                        RequestClose(null, null);
                        GeosApplication.Instance.Logger.Log("Method AcceptCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
                    }
                   
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
        }
        //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public void AddLinkedOffersAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLinkedOffersAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                LinkedOffersViewModel linkedOffersViewModel = new LinkedOffersViewModel();
                LinkedOffersView linkedOffersView = new LinkedOffersView();
                //[pramod.misal][01.04.2025][GEOS2-7725]
                if (poregistereddetails == null)
                {
                    poregistereddetails = new PORegisteredDetails();
                    var selectedCustomer = CustomerPlants[SelectedIndexCompanyPlant];
                    if (selectedCustomer != null)
                    {

                        poregistereddetails.IdSite = selectedCustomer.IdCustomerPlant;
                        // You can now access selectedCustomer.Name, selectedCustomer.Email, etc.
                    }
                    //SelectedIndexCustomerGroup = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == offerinfo.IdCustomer));
                }
                poregistereddetails.IdCustomerPlant = SelectedCustomerGroupForLinkedOffer;
                poregistereddetails.IdSite = SelectedCustomerPlantForLinkedOffer;

                linkedOffersViewModel.InIt(poregistereddetails);
                EventHandler handle = delegate { linkedOffersView.Close(); };
                linkedOffersViewModel.RequestClose += handle;
                linkedOffersView.DataContext = linkedOffersViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                linkedOffersView.ShowDialogWindow();
                if (linkedOffersViewModel.IsAccepted)
                {
                    if (linkedOffersViewModel.SelectedIndexLinkedOffer != null)
                    {
                        LinkedOffersDetails.Add(linkedOffersViewModel.SelectedIndexLinkedOffer);
                        poregistereddetails.offersLinked.Add(linkedOffersViewModel.SelectedIndexLinkedOffer);
                    }
                }
                if (true)
                {

                }
                GeosApplication.Instance.Logger.Log("Method AddLinkedOffersAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLinkedOffersAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="oldPODetails"></param>
        public void AddChangeLogByPO(PORegisteredDetails oldPODetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangeLogByPO ...", category: Category.Info, priority: Priority.Low);
                tempPOChangeLog = new ObservableCollection<LogEntryByPOOffer>();
                if (oldPODetails != null)
                {
                    if (oldPODetails.IdPOType != poregistereddetails.IdPOType)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByType").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Type.ToString().Trim(), poregistereddetails.Type.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.IdCustomerPlant != poregistereddetails.IdCustomerPlant)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByCustomerGroup").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Group.ToString().Trim(), poregistereddetails.Group.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.IdSite != poregistereddetails.IdSite)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByCustomerPlant").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Plant.ToString().Trim(), poregistereddetails.Plant.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.Code != poregistereddetails.Code)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByPOCode").ToString(), oldPODetails.Code.ToString().Trim(), oldPODetails.Code.ToString().Trim(), poregistereddetails.Code.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.ReceptionDate != poregistereddetails.ReceptionDate)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByReceptionDate").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.ReceptionDate.ToShortDateString().Trim(), poregistereddetails.ReceptionDate.ToShortDateString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.IdSender != poregistereddetails.IdSender)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogBySender").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Sender.ToString().Trim(), poregistereddetails.Sender.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.IdShippingAddress != poregistereddetails.IdShippingAddress)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByShippingAdress").ToString(), poregistereddetails.Code.ToString().Trim(), (oldPODetails.ShippingAddress ?? "").ToString().Trim(), poregistereddetails.ShippingAddress.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.POValue != poregistereddetails.POValue)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByAmount").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.POValue.ToString().Trim(), poregistereddetails.POValue.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.IdCurrency != poregistereddetails.IdCurrency)
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByCurrency").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Currency.ToString().Trim(), poregistereddetails.Currency.ToString().Trim()), IdLogEntryType = 4 });

                    if (oldPODetails.AttachmentFileName != poregistereddetails.AttachmentFileName)
                    {
                        if (oldPODetails.AttachmentFileName == null)
                        {
                            oldPODetails.AttachmentFileName = "None";
                        }
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByAttachment").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.AttachmentFileName.ToString().Trim(), poregistereddetails.AttachmentFileName.ToString().Trim()), IdLogEntryType = 4 });
                    }

                    if (oldPODetails.IsOK != poregistereddetails.IsOK)
                    {
                        string oldPOOk = "";
                        string newPOOk = "";
                        if (oldPODetails.IsOK == "PO OK")
                            oldPOOk = "OK";
                        else
                            oldPOOk = "NOK";
                        if (poregistereddetails.IsOK == "PO OK")
                            newPOOk = "OK";
                        else
                            newPOOk = "NOK";
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByNotOk").ToString(), poregistereddetails.Code.ToString().Trim(), oldPOOk.Trim(), newPOOk.ToString().Trim()), IdLogEntryType = 4 });
                    }

                    if (oldPODetails.IsCancelled != poregistereddetails.IsCancelled)
                    {
                        string oldPOCanceled = "";
                        string newPOCanceled = "";
                        if (oldPODetails.IsCancelled == "PO Cancelled")
                            oldPOCanceled = "Cancel";
                        else
                            oldPOCanceled = "Not Cancel";
                        if (poregistereddetails.IsCancelled == "PO Cancelled")
                            newPOCanceled = "Cancel";
                        else
                            newPOCanceled = "Not Cancel";
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByCancelled").ToString(), poregistereddetails.Code.ToString().Trim(), oldPOCanceled.ToString().Trim(), newPOCanceled.ToString().Trim()), IdLogEntryType = 4 });
                        //if (poregistereddetails.IsCancelled == "PO Cancelled")
                        //    tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByCancelled").ToString(), poregistereddetails.Code.ToString().Trim(),oldPODetails.IsCancelled.ToString().Trim(), poregistereddetails.IsCancelled.ToString().Trim()), IdLogEntryType = 4 });
                        //else
                        //    tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByUnCancelled").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.IsCancelled.ToString().Trim(), poregistereddetails.IsCancelled.ToString().Trim()), IdLogEntryType = 4 });
                    }

                    if (oldPODetails.Remarks != poregistereddetails.Remarks)
                    {
                        if (oldPODetails.Remarks == null && oldPODetails.Remarks == "")
                        {
                            oldPODetails.Remarks = "None";
                            tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByRemarks").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Remarks.ToString().Trim(), poregistereddetails.Remarks.ToString().Trim()), IdLogEntryType = 4 });
                        }

                    }
                    // Added Linked offer Log
                    if (poregistereddetails.OffersLinked != null)
                    {
                        foreach (LinkedOffers lnkoffer in poregistereddetails.OffersLinked)
                        {
                            if (lnkoffer.IsNew)
                            {
                                tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByLinkedOfferAdded").ToString(), poregistereddetails.Code.ToString().Trim(), lnkoffer.Code.ToString().Trim()), IdLogEntryType = 4 });
                            }
                            else if (lnkoffer.IsDelete)
                            {
                                tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByLinkedOfferdelete").ToString(), poregistereddetails.Code.ToString().Trim(), lnkoffer.Code.ToString().Trim()), IdLogEntryType = 4 });
                            }
                            //else if(lnkoffer.IsUpdate)
                            //{
                            //    if (oldPODetails.Remarks != lnkoffer.Remark)
                            //        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByRemarks").ToString(), poregistereddetails.Code.ToString().Trim(), oldPODetails.Remarks.ToString().Trim(), poregistereddetails.Remarks.ToString().Trim()), IdLogEntryType = 4 });
                            //}
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddChangeLogGroupAndPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddChangeLogGroupAndPlant()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private byte[] GetEmployeesImage_V2590(string EmployeeCode)
        {
            try
            {
                bool isProfileUpdate = false;
                //HrmService = new HrmServiceController("localhost:6699");
                isProfileUpdate = OTMService.IsProfileUpdate(EmployeeCode);
                string ProfileImagePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/Rounded/" + EmployeeCode + ".png";
                byte[] ImageBytes = null;
                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage_V2590()...", category: Category.Info, priority: Priority.Low);
                try
                {
                    if (isProfileUpdate)
                    {
                        #region isProfileUpdate
                        ImageBytes = GeosRepositoryServiceController.GetEmployeesImage(EmployeeCode);
                        #endregion
                    }
                    else
                    {
                        #region ImageUrlBytePair
                        if (GeosApplication.ImageUrlBytePair == null)
                            GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                        if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == ProfileImagePath.ToLower()))
                        {
                            ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == ProfileImagePath.ToLower()).Value;
                        }
                        else
                        {
                            if (GeosApplication.IsImageURLException == false)
                            {
                                //using (WebClient webClient = new WebClient())
                                //{
                                //    ImageBytes = webClient.DownloadData(ProfileImagePath);
                                //}
                                ImageBytes = Utility.ImageUtil.GetImageByWebClient(ProfileImagePath);
                            }
                            else
                            {
                                ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                            }
                            if (ImageBytes.Length > 0)
                            {
                                GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                                return ImageBytes;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {

                    if (isProfileUpdate)
                    {
                        #region isProfileUpdate
                        ImageBytes = GeosRepositoryServiceController.GetEmployeesImage(EmployeeCode);
                        #endregion
                    }
                    else
                    {
                        #region ImageUrlBytePair
                        if (GeosApplication.IsImageURLException == false)
                            GeosApplication.IsImageURLException = true;
                        if (!string.IsNullOrEmpty(ProfileImagePath))
                        {
                            ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                            GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                        }
                        #endregion
                    }
                }

                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage_V2520()....executed successfully", category: Category.Info, priority: Priority.Low);
                return ImageBytes;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetEmployeesImage_V2520()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][07-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6464
        /// </summary>
        public void FillListPOChangeLog()
        {
            GeosApplication.Instance.Logger.Log("Method FillListPOChangeLog ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //FillServiceUrl();
                //FillServiceUrlNew();

                // OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                ListPOChangeLog = new ObservableCollection<LogEntryByPOOffer>(OTMServiceThread.GetAllPOChangeLog_V2590(poregistereddetails.IdPO));
                GeosApplication.Instance.Logger.Log("Method FillListPOChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListPOChangeLog() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  FillListPOChangeLog() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public void AddSourceButtonCommandAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSourceButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddContactViewModel addPOContactViewModel = new AddContactViewModel();
                AddContactView addPOContactView = new AddContactView();

                //addPOContactViewModel.InIt(poregistereddetails);
                EventHandler handle = delegate { addPOContactView.Close(); };
                addPOContactViewModel.RequestClose += handle;
                addPOContactView.DataContext = addPOContactViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addPOContactView.ShowDialogWindow();
                if (addPOContactViewModel.ContactData != null)
                {
                    //POSender.a addPOContactViewModel.Contact;
                    PORegisteredDetails POSenderAdd = new PORegisteredDetails();
                    POSenderAdd.FirstName = addPOContactViewModel.ContactData.Name;
                    POSenderAdd.LastName = addPOContactViewModel.ContactData.Surname;
                    POSenderAdd.FullName = addPOContactViewModel.ContactData.FullName;
                    POSenderAdd.IdPerson = addPOContactViewModel.ContactData.IdPerson;
                    POSenderAdd.IdGender = Convert.ToInt16(addPOContactViewModel.ContactData.IdPersonGender);
                    POSenderByGroup.Insert(0, POSenderAdd);

                }
                if (true)
                {

                }
                GeosApplication.Instance.Logger.Log("Method AddSourceButtonCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddSourceButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][09122024] https://helpdesk.emdep.com/browse/GEOS2-6464
        /// </summary>
        /// <param name="gcComments"></param>
        public void EditContactButtonCommandAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditContactButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<Crm.Views.SplashScreenView>(); }
                EditContactViewModel editPOContactViewModel = new EditContactViewModel();
                EditContactView editPOContactView = new EditContactView();
                Int32 IdPerson = POSenderByGroup[SelectedIndexSender].IdPerson;
                // OTMService = new OTMServiceController("localhost:6699");
                PeopleContacts = new ObservableCollection<People>(OTMService.GetContactsByIdPermission_V2590(IdPerson));
                GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                if (PeopleContacts != null && PeopleContacts.Any())
                {
                    People p = PeopleContacts.FirstOrDefault();
                    editPOContactViewModel.InIt(p);
                    editPOContactViewModel.InItPermisssion(IsPermissionCustomerEdit);  //[pallavi.kale][GEOS2-8961][07.11.2025]
                    EventHandler handle = delegate { editPOContactView.Close(); };
                    editPOContactViewModel.RequestClose += handle;
                    editPOContactView.DataContext = editPOContactViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    editPOContactView.ShowDialogWindow();
                    GeosApplication.Instance.Logger.Log("Method EditContactButtonCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EditSourceButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "*.*";
                dlg.Filter = "All Supported Files|*.pdf;*.tif;*.tiff;*.jpg;*.jpeg;*.docx;*.png|PDF Files|*.pdf|TIFF Files|*.tif;*.tiff|Image Files|*.jpg;*.jpeg;*.png|Word Documents|*.docx";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    registerPoAttachmentList = new ObservableCollection<RegisterPoAttachments>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    RegisterPOAttachmentSavedFileName = file.Name;

                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                        FileName = file.FullName;
                    }
                    ObservableCollection<RegisterPoAttachments> newAttachmentList = new ObservableCollection<RegisterPoAttachments>();
                    RegisterPoAttachments attachment = new RegisterPoAttachments();
                    attachment.SavedFileName = file.Name;
                    attachment.ConnectorAttachementsDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    RegisterPoAttachmentList = newAttachmentList;

                    if (RegisterPoAttachmentList.Count > 0)
                    {
                        SelectedRegisterPoFile = registerPoAttachmentList[0];
                    }

                    ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(RegisterPOAttachmentSavedFileName, true);

                    AttachmentImage = objImage;


                    //[pramod.misal][14.12.2024][]
                    if (SelectedRegisterPoFile.ConnectorAttachementsDocInBytes != null && SelectedRegisterPoFile.SavedFileName != null)
                    {
                        if (poregistereddetails.OffersLinked.Any(offer => offer.AttachmentFileName != SelectedRegisterPoFile.SavedFileName))
                        {
                            foreach (var item in poregistereddetails.OffersLinked)
                            {
                                item.IsUpdate = true;
                            }
                        }
                        if (poregistereddetails.OffersLinked.Any(offer => offer.AttachmentFileName == SelectedRegisterPoFile.SavedFileName))
                        {
                            foreach (var item in poregistereddetails.OffersLinked)
                            {
                                item.IsNew = true;
                            }
                        }
                    }



                    #region old code
                    // For Image
                    //attachment.FileExtension = file.Extension;
                    //attachment.FileUploadName = file.Name;
                    //attachment.IsUploaded = true;
                    //attachment.TransactionOperation = ModelBase.TransactionOperations.Add;

                    //var theIcon = IconFromFilePath(FileName);
                    //string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";
                    //if (theIcon != null)
                    //{
                    //    // Save it to disk, or do whatever you want with it.
                    //    if (!Directory.Exists(tempPath))
                    //    {
                    //        System.IO.Directory.CreateDirectory(tempPath);
                    //    }

                    //    if (!File.Exists(tempPath + UniqueFileName + file.Extension + ".ico"))
                    //    {
                    //        using (var stream = new System.IO.FileStream(tempPath + UniqueFileName + file.Extension + ".ico", System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    //        {
                    //            theIcon.Save(stream);
                    //            stream.Close();
                    //            stream.Dispose();
                    //        }
                    //    }
                    //    theIcon.Dispose();
                    //}

                    //// useful to get icon end process of temp. used imgage 
                    //BitmapImage image = new BitmapImage();
                    //image.BeginInit();
                    //image.CacheOption = BitmapCacheOption.OnLoad;
                    //image.UriSource = new Uri(tempPath + UniqueFileName + file.Extension + ".ico", UriKind.RelativeOrAbsolute);
                    //image.EndInit();
                    //attachment.AttachmentImage = image;
                    //AttachmentObjectList.Add(attachment);

                    #endregion
                }
                GeosApplication.Instance.Logger.Log("Method BrowseFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument..."), category: Category.Info, priority: Priority.Low);

                // Open PDF in another window

                if (SelectedRegisterPoFile == null)
                {
                    SelectedRegisterPoFile = new RegisterPoAttachments();
                    //}
                    //else
                    //{
                    foreach (var detail in LinkedOffersDetails)
                    {
                        if (detail != null)
                        {
                            if (detail.AttachmentFileName != null && detail.CommericalAttachementsDocInBytes != null)
                            {
                                SelectedRegisterPoFile.SavedFileName = detail.AttachmentFileName;
                                SelectedRegisterPoFile.ConnectorAttachementsDocInBytes = detail.CommericalAttachementsDocInBytes;
                            }
                        }
                    }
                }
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenPdfByRegisterPoAttachment(SelectedRegisterPoFile, obj);

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
        /// <summary>
        /// [001][ashish.malkhede][11-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="obj"></param>
        private void LinkedItemCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method LinkedItemCancelAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj is LinkedOffers)
                {
                    LinkedOffers linkedItem = obj as LinkedOffers;
                    if (linkedItem != null)
                    {
                        if (LinkedOffersDetails.Count == 1)
                        {
                            CustomMessageBox.Show(System.Windows.Application.Current.FindResource("EditRegisteredPOsLinkedOffer").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                        //poregistereddetails.offersLinked.Remove(poregistereddetails.OffersLinked.FirstOrDefault(x => x.IdOffer == linkedItem.IdOffer));
                        LinkedOffersDetails.Remove(LinkedOffersDetails.FirstOrDefault(x => x.IdOffer == linkedItem.IdOffer));
                        if (linkedItem.IsNew)
                        {
                            poregistereddetails.offersLinked.Remove(poregistereddetails.offersLinked.FirstOrDefault(x => x.IdOffer == linkedItem.IdOffer));
                        }

                        poregistereddetails.offersLinked.ToList().ForEach(x =>
                        {
                            if (x.IdOffer == linkedItem.IdOffer)
                            {
                                x.IsDelete = true;
                            }
                        });
                    }
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method LinkedItemCancelAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method LinkedItemCancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// [001] [Rahul.Gadhave][19-12-2024]
        private void SelectedIndexChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int CurrentCurrencyId = 0;
                var selectedCurrency = GeosApplication.Instance.Currencies
                   .SingleOrDefault(cur => cur.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString());
                if (selectedCurrency != null)
                {
                    CurrentCurrencyId = selectedCurrency.IdCurrency;
                }
                int idCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
                if (CurrentCurrencyId != Currencies[SelectedIndexCurrency].IdCurrency)
                {
                    ConvertedAmount = OTMService.GetOfferAmountByCurrencyConversion_V2590(idCurrency, CurrentCurrencyId, IdPO);
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method SelectedIndexChangedCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditCustomerGroupButtonCommandAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerGroupButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<Views.SplashScreenView>(); }
                TempCompany = new List<Data.Common.Company>();
                TempCompany.Add(OTMService.GetCompanyDetailsById_V2580(poregistereddetails.IdSite));
                EditCustomerView editCustomerView = new EditCustomerView();
                EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel();

                editCustomerViewModel.InIt(TempCompany);
                EventHandler handle = delegate { editCustomerView.Close(); };
                editCustomerViewModel.RequestClose += handle;
                editCustomerView.DataContext = editCustomerViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                editCustomerView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method EditCustomerGroupButtonCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditCustomerGroupButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    
        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;

            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }
            return result;
        }

        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        private void FillCustomerGroupListForNewPO(LinkedOffers offerinfo, LinkedOffers SelectedofferInfo)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerPlantsList ...", category: Category.Info, priority: Priority.Low);
                Customers = new ObservableCollection<Customer>();
                // OTMService = new OTMServiceController("localhost:6699");

                ObservableCollection<Customer> CustomersList = new ObservableCollection<Customer>();
                Customer cust = new Customer();

                if (SelectedofferInfo != null)
                {
                    cust.IdCustomer = SelectedofferInfo.IdCustomer;
                    cust.CustomerName = SelectedofferInfo.CustomerGroup;
                    cust.IsEnabled = false;
                }

                //[pooja.jadhav][GEOS2-9179][13-08-2025]
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                CustomersList = new ObservableCollection<Customer>(OTMServiceThread.GetAllCustomers_V2660(cust.IdCustomer));

                var existingCustomer = CustomersList.FirstOrDefault(c => c.IdCustomer == cust.IdCustomer);
                if (existingCustomer == null)
                {
                    CustomersList.Add(cust);
                }
                else
                {
                    existingCustomer.IsEnabled = false;
                }

                Customers = new ObservableCollection<Customer>(CustomersList.OrderBy(c => c.CustomerName));
                GeosApplication.Instance.Logger.Log("Method FillCustomerPlantsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPlantsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPlantsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        public void AddChangeLogByNewPO()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangeLogByNewPO ...", category: Category.Info, priority: Priority.Low);
                if (tempPOChangeLog == null)
                {
                    tempPOChangeLog = new ObservableCollection<LogEntryByPOOffer>();
                }
                if (poregistereddetails.OffersLinked != null)
                {
                    foreach (LinkedOffers lnkoffer in poregistereddetails.OffersLinked)
                    {
                        tempPOChangeLog.Add(new LogEntryByPOOffer() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("POChangeLogByAddeNewPO").ToString(), lnkoffer.Code.ToString().Trim(), poregistereddetails.Code), IdLogEntryType = 4 });
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddChangeLogByNewPO() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddChangeLogByNewPO() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        //[GEOS2-8305][rdixit][22.08.2025]
        public void FillServiceUrl()
        {
            try
            {
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        //[GEOS2-8305][pramod.misal][03-09-2025]
        public void FillServiceUrlNew()
        {
            try
            {
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = serviceUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    string serviceurl = serviceUrl = (OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();

                    OTMService = new OTMServiceController(serviceurl);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// [pooja.jadhav][04-09-2025][GEOS2-9322] OTM - Limit the Sender list in Edit PO
        /// </summary>
        private void FillSenderList()
        {
            PORegisteredDetails POSenderAdd = new PORegisteredDetails();
            
            if (IsShowAllSender)
            {
                POSenderByGroup = new ObservableCollection<PORegisteredDetails>(
                        POSender.Where(pos => pos.IdCustomer == Customers[SelectedIndexCustomerGroup].IdCustomer || pos.SiteName == "---"));
            }
            else
            {
                if(SelectedIndexCompanyPlant==-1)
                {
                    POSenderByGroup.Clear();
                }
                else
                {
                    POSenderByGroup = new ObservableCollection<PORegisteredDetails>(
                        POSender.Where(pos => (pos.IdCustomer == Customers[SelectedIndexCustomerGroup].IdCustomer && pos.IdSite == CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant) || pos.SiteName == "---"));
                }
            }
        }
        //[Rahul.Gadhave][GEOS2-9880][Date:28-11-2025]
        public void InitNewPOForGoAhed(PORequestDetails PODetails, LinkedOffers SelectedofferInfo, PdfResultDto pdfResultDto)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitNewPOForGoAhed()....", category: Category.Info, priority: Priority.Low);
                var tasks = new List<Task>();
                //[GEOS2-8305][rdixit][22.08.2025]
                // Run all methods in parallel
                RefpdfResultDto = pdfResultDto;
                FillServiceUrlNew();
                tasks.Add(Task.Run(() => FillPOTypeList()));
                tasks.Add(Task.Run(() => FillCurrenciesList()));
                tasks.Add(Task.Run(() => FillEntireCompanyPlantList()));
                tasks.Add(Task.Run(() => FillCustomerGroupListForNewPO(PODetails.OfferInfo, SelectedofferInfo)));
                tasks.Add(Task.Run(() => FillPOSenderList()));
                tasks.Add(Task.Run(() => FillEmployeeCode()));
                Task.WaitAll(tasks.ToArray());

                IsAddPO = Visibility.Visible;
                IsEditPO = Visibility.Hidden;
                SelectedIndexPoType = POTypeList.IndexOf(POTypeList.FirstOrDefault(i => i.IdPoType == 1));
                SelectedType = POTypeList.FirstOrDefault(i => i.IdPoType == 1).Type;
                //[Rahul.Gadhave][GEOS2-9326][Date:01-09-2025]
                if (SelectedofferInfo.Attachment == null)
                {
                    SelectedofferInfo.Attachment = new Emailattachment();
                }
                if (SelectedofferInfo.Attachment.SelectedIndexAttachementType != 1)
                {
                    var itemToRemove = POTypeList.FirstOrDefault(x => x.IdPoType == 1);
                    if (itemToRemove != null)
                    {
                        POTypeList.Remove(itemToRemove);
                    }
                    SelectedType = POTypeList.FirstOrDefault(i => i.IdPoType == 2).Type;
                    SelectedIndexPoType = POTypeList.IndexOf(POTypeList.FirstOrDefault(i => i.IdPoType == 2));
                    PoAbbreviation = POTypeList.FirstOrDefault(i => i.IdPoType == 2).Abbreviation;
                }
                else
                {
                    if (SelectedofferInfo.Attachment.SelectedIndexAttachementType == 1)
                    {
                        PoAbbreviation = POTypeList.FirstOrDefault(i => i.IdPoType == 1).Abbreviation;
                    }
                }
                if (SelectedofferInfo.IdStatus != 0 && SelectedofferInfo.IdOffer != 0)
                {
                    IdStatus = SelectedofferInfo.IdStatus;
                    IdOffer = SelectedofferInfo.IdOffer;
                    OfferCode = SelectedofferInfo.Code;
                    OfferCodeForLinkedPO = SelectedofferInfo.Code;
                    OfferCodeForPOType = SelectedofferInfo.Code;

                }
                Code = PoAbbreviation + "_" + SelectedofferInfo.Code;
                OfferCodeForPOType = SelectedofferInfo.Code;
                SelectedIndexCustomerGroup = Customers.IndexOf(Customers.FirstOrDefault(i => i.IdCustomer == SelectedofferInfo.IdCustomer));
                SelectedCustomerGroupName = Customers.FirstOrDefault(i => i.IdCustomer == SelectedofferInfo.IdCustomer).CustomerName;
                //ReceptionDate = poregistereddetails.ReceptionDate;
                SelectedIndexCompanyPlant = CustomerPlants.IndexOf(CustomerPlants.FirstOrDefault(i => i.IdCustomerPlant == SelectedofferInfo.IdSite));
                //POSenderByGroup = POSender;
                // [Rahul.gadhave][GEOS2-9878][Date:19 - 11 - 2025]
                if (POSenderByGroup != null)
                {
                    SelectedIndexSender = POSenderByGroup.IndexOf(POSenderByGroup.FirstOrDefault(i => i.FullName == PODetails.Sender));
                }
                if (PODetails.TransferAmount != null)
                {
                    Amount = (double)PODetails.TransferAmount;
                }
                SelectedIndexCurrency = Currencies.IndexOf(Currencies.FirstOrDefault(i => i.Name == PODetails.Currency));
                if (SelectedIndexCurrency == -1)
                {
                    SelectedIndexCurrency = Currencies.IndexOf(Currencies.FirstOrDefault(i => i.Name == SelectedofferInfo.Currency));
                }
                SelectedIndexShipTo = -1;
                if (PODetails.PONumber != null)
                {
                    Code = PODetails.PONumber;
                }
                IsreceptionDateVisisble = true;
                if (PODetails.POdate != null)
                {
                    ReceptionDate = PODetails.POdate;
                }
                if (ReceptionDate == DateTime.MinValue)
                {
                    ReceptionDateNew = null;
                }
                else
                {
                    ReceptionDateNew = ReceptionDate;
                }

                CreationDate = null;
                //if (PODetails.OfferInfo.Attachment.FileDocInBytes != null)
                //{
                //    SelectedofferInfo.CommericalAttachementsDocInBytes = PODetails.OfferInfo.Attachment.FileDocInBytes;
                //}
                //RegisterPOAttachmentSavedFileName = PODetails.OfferInfo.Attachment.AttachmentName;
                //ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(RegisterPOAttachmentSavedFileName, true);
                //AttachmentImage = objImage;
                LinkedOffersDetails = new ObservableCollection<LinkedOffers> { SelectedofferInfo };

                //[Rahul.Gadhave][Date:28-11-2025]
                if (SelectedIndexPoType == 0 && pdfResultDto.FileBytes!=null)
                {
                    SelectedofferInfo.CommericalAttachementsDocInBytes = pdfResultDto.FileBytes;
                    if(SelectedRegisterPoFile==null)
                    {
                        SelectedRegisterPoFile = new RegisterPoAttachments();
                    }
                    SelectedRegisterPoFile.ConnectorAttachementsDocInBytes= pdfResultDto.FileBytes;
                    SelectedRegisterPoFile.SavedFileName= pdfResultDto.FileName;
                    RegisterPOAttachmentSavedFileName = pdfResultDto.FileName;
                    ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(RegisterPOAttachmentSavedFileName, true);
                    AttachmentImage = objImage;
                }
                if (poregistereddetails == null)
                {
                    poregistereddetails = new PORegisteredDetails();
                    //poregistereddetails.OffersLinked = new ObservableCollection<LinkedOffers>();
                }
                poregistereddetails.OffersLinked = LinkedOffersDetails.Select(x => (LinkedOffers)x.Clone()).ToObservableCollection();//pramod.misal
                foreach (var linkedOffer in LinkedOffersDetails)
                {
                    string customerName = linkedOffer.CustomerGroup;
                    linkedOffer.AttachmentFileName = PODetails.OfferInfo.Attachment.AttachmentName;
                    if (!string.IsNullOrEmpty(customerName))
                    {
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customerName);

                        if (bytes != null)
                        {
                            // Assuming linkedOffer has a property to hold image data (e.g., LinkedOfferImage).
                            linkedOffer.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            // Set a default image based on the theme.
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/wAccount.png");
                            }
                            else
                            {
                                linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/blueAccount.png");
                            }
                        }
                    }
                }
                IsNewPo = true;
                IsEmailBtnVisibile = Visibility.Hidden;
                IsPoEmailGoAhed = true;
                GeosApplication.Instance.Logger.Log("Method InitNewPOForGoAhed()...Executed", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method InitNewPOForGoAhed()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => SelectedIndexPoType)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCustomerGroup)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +
                    me[BindableBase.GetPropertyName(() => Code)] +
                    me[BindableBase.GetPropertyName(() => Remarks)] +
                    me[BindableBase.GetPropertyName(() => ReceptionDate)] +
                    me[BindableBase.GetPropertyName(() => ReceptionDateNew)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexSender)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexShipTo)] +
                    me[BindableBase.GetPropertyName(() => Amount)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCurrency)];
                    //me[BindableBase.GetPropertyName(() => RegisterPOAttachmentSavedFileName)];
                    // only validate POAttachment if IsNewPo == true
                    if (IsPOGoAhead)//[pramod.misal][Date:11-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9173]
                    error += me[BindableBase.GetPropertyName(() => RegisterPOAttachmentSavedFileName)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        // [nsatpute][01-12-2024][GEOS2-6462]
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string PoType = BindableBase.GetPropertyName(() => SelectedIndexPoType);
                string CustomerGroup = BindableBase.GetPropertyName(() => SelectedIndexCustomerGroup);
                string CompanyPlant = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);               // SelectedIndexGender
                string Number = BindableBase.GetPropertyName(() => Code);
                string Remark = BindableBase.GetPropertyName(() => Remarks);
                string ReceptionDatePo = BindableBase.GetPropertyName(() => ReceptionDate);
                string ReceptionDateNewPo = BindableBase.GetPropertyName(() => ReceptionDateNew);
                string Sender = BindableBase.GetPropertyName(() => SelectedIndexSender);
                string ShipTo = BindableBase.GetPropertyName(() => SelectedIndexShipTo);
                string Amountpo = BindableBase.GetPropertyName(() => Amount);
                string Currency = BindableBase.GetPropertyName(() => SelectedIndexCurrency);

                string POAttachment = BindableBase.GetPropertyName(() => RegisterPOAttachmentSavedFileName);
                string informationError = BindableBase.GetPropertyName(() => InformationError);

                if (columnName == PoType)
                {
                    return RegisterPoEditValidation.GetErrorMessage(PoType, SelectedIndexPoType);
                }
                else if (columnName == CustomerGroup)
                    return RegisterPoEditValidation.GetErrorMessage(CustomerGroup, SelectedIndexCustomerGroup);

                else if (columnName == CompanyPlant)
                    return RegisterPoEditValidation.GetErrorMessage(CompanyPlant, SelectedIndexCompanyPlant);

                else if (columnName == Number)
                    return RegisterPoEditValidation.GetErrorMessage(Number, Code);

                else if (columnName == Remark && !IsPOGoAhead)
                {
                    return RegisterPoEditValidation.GetErrorMessage(Number, Remarks);
                }
                    

                else if (columnName == ReceptionDateNewPo)
                    return RegisterPoEditValidation.GetErrorMessage(ReceptionDatePo, ReceptionDateNew);

                else if (columnName == ReceptionDatePo)
                    return RegisterPoEditValidation.GetErrorMessage(ReceptionDatePo, ReceptionDate);

                else if (columnName == Sender)
                    return RegisterPoEditValidation.GetErrorMessage(Sender, SelectedIndexSender);

                else if (columnName == ShipTo)
                    return RegisterPoEditValidation.GetErrorMessage(ShipTo, SelectedIndexShipTo);

                else if (columnName == Amountpo)
                    return RegisterPoEditValidation.GetErrorMessage(Amountpo, Amount);

                else if (columnName == Currency)
                    return RegisterPoEditValidation.GetErrorMessage(Currency, SelectedIndexCurrency);

                else if (columnName == POAttachment && IsPOGoAhead)
                {
                    return RegisterPoEditValidation.GetErrorMessage(columnName, RegisterPOAttachmentSavedFileName);
                    
                }
                return null;
            }

        }
        public void Dispose()
        {

        }
        #endregion
    }
}
